using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web.UI.WebControls;

public static class StampsBacklogWorker
{
    private static Timer _timer;

    // Penanda SO yang sudah dipastikan gagal PERMANEN (bukan transient) supaya tidak diulang
    // terus tiap siklus. CATATAN: ini cuma persist selama AppDomain masih hidup — kalau app
    // pool recycle/restart, daftar ini reset dan SO tersebut akan dicoba ulang sekali lagi
    // (lalu langsung ke-flag lagi begitu gagal dengan pola yang sama). Untuk exclusion yang
    // benar-benar permanen lintas-restart, perlu kolom/flag tambahan di level database yang
    // ikut dicek oleh sp_stamps_transaction_new — itu di luar yang bisa diubah dari sini.
    private static readonly HashSet<int> _permanentlyFailedThisSession = new HashSet<int>();

    // Backoff untuk kegagalan TRANSIENT (kategori "Gagal", bukan permanen) — sebelumnya SO yang
    // gagal transient dicoba lagi SETIAP siklus (tiap 5 menit) tanpa batas, ikut berkontribusi ke
    // volume request yang memicu error 429 "Too many requests" dari STAMPS API. In-memory saja
    // (reset kalau app pool restart) — konsisten dengan pola exclusion permanen di atas.
    private static readonly Dictionary<int, int> _consecutiveFailureCount = new Dictionary<int, int>();
    private static readonly Dictionary<int, int> _skipCyclesRemaining = new Dictionary<int, int>();
    private const int MaxBackoffCycles = 6; // ~30 menit dengan interval 5 menit

    // Pola pesan error dari STAMPS yang menandakan kegagalan PERMANEN (retry tidak akan
    // pernah berhasil untuk SO yang sama) — beda dengan connection-state error yang transient.
    private static readonly string[] PermanentFailurePatterns = new[]
    {
        "has been used",
        "already used",
        "already redeemed",
        "BadRequest 400", // HTTP 400 = data invalid, retry tidak akan berubah hasilnya
    };

    public static void Start()
    {
        if (_timer != null) return;
        _timer = new Timer(_ => Run(), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5));
    }

    private static void Run()
    {
        try
        {
            using (var db = new Map_DB())
            {
                string query = "EXEC sp_stamps_transaction_new";
                DataTable dt = Db.Rs(query, Db.Cnn_DB);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        int id = Convert.ToInt32(dr["ID"]);

                        if (_permanentlyFailedThisSession.Contains(id))
                        {
                            continue; // sudah ke-flag permanen gagal di siklus sebelumnya, skip
                        }

                        int skipRemaining;
                        if (_skipCyclesRemaining.TryGetValue(id, out skipRemaining) && skipRemaining > 0)
                        {
                            _skipCyclesRemaining[id] = skipRemaining - 1;
                            continue; // masih dalam masa backoff dari kegagalan transient sebelumnya
                        }

                        var d = db.SalesOrder.SingleOrDefault(p => p.ID == id);
                        try
                        {
                            STAMPSM.prePostSalesOrderIntegration(d);
                            // STAMPSM.AddTransaction TIDAK throw untuk error bisnis (misal STAMPS
                            // balas 400 tapi bukan "already exists") — dia cuma me-return string
                            // error lewat return value. Jadi sukses/gagal harus dicek dari return
                            // value ini, bukan cuma dari ada/tidaknya exception.
                            string stampsError = STAMPSM.AddTransaction(new Literal(), d);
                            if (string.IsNullOrEmpty(stampsError))
                            {
                                _consecutiveFailureCount.Remove(id);
                                _skipCyclesRemaining.Remove(id);
                                LogWorker(id, "StampsBacklogWorker", "Berhasil posting STAMPS dari backlog.");
                            }
                            else if (IsPermanentFailure(stampsError))
                            {
                                _permanentlyFailedThisSession.Add(id);
                                MarkPermanentFailure(GetSoBrand(db, d), d.Nomor);
                                LogWorker(id, "StampsBacklogWorker - PERMANEN, butuh review manual", stampsError);
                            }
                            else
                            {
                                ApplyBackoff(id);
                                LogWorker(id, "StampsBacklogWorker - Gagal", stampsError);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Tidak throw — SO ini akan dicoba lagi di siklus berikutnya
                            // selama sp_stamps_transaction_new masih menganggapnya pending.
                            ApplyBackoff(id);
                            LogWorker(id, "StampsBacklogWorker - Gagal", ex.Message);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogWorker(0, "StampsBacklogWorker - Fatal", ex.Message);
        }
    }

    // Backoff linear sederhana: lewati sejumlah siklus = jumlah kegagalan berturut-turut,
    // dibatasi MaxBackoffCycles supaya SO yang genuinely masih transient tetap tercoba lagi
    // dalam waktu wajar (bukan retry tanpa henti tiap 5 menit seperti sebelumnya).
    private static void ApplyBackoff(int id)
    {
        int failCount;
        _consecutiveFailureCount.TryGetValue(id, out failCount);
        failCount++;
        _consecutiveFailureCount[id] = failCount;
        _skipCyclesRemaining[id] = Math.Min(failCount, MaxBackoffCycles);
    }

    // SO_NUMBER bisa sama antar brand berbeda di jaws_integration (dikonfirmasi user) — semua
    // WHERE clause yang menyentuh SALESORDER di jaws_integration harus ikut key by SO_BRAND
    // juga, sama seperti yang dipakai SP_STAMPS_ADD_TRANSACTION_WITH_REDEMPTION (WHERE SO_BRAND
    // = @SO_BRAND AND SO_NUMBER = @SO_NUMBER), supaya tidak salah menandai SO milik brand lain
    // yang kebetulan punya nomor sama.
    private static string GetSoBrand(Map_DB db, SalesOrder d)
    {
        if (d.TipeLokasi == 1)
        {
            return db.LocOutlet.Single(p => p.ID == d.IDLokasi).CompanyBrand.NamaKode.ToString();
        }
        else if (d.TipeLokasi == 2)
        {
            return db.LocExhibition.Single(p => p.ID == d.IDLokasi).CompanyBrand.NamaKode.ToString();
        }
        return string.Empty;
    }

    private static bool IsPermanentFailure(string errorMessage)
    {
        foreach (var pattern in PermanentFailurePatterns)
        {
            if (errorMessage.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
        }
        return false;
    }

    // Tulis sentinel -1 (bukan 0/NULL) ke kolom yang jadi kriteria seleksi
    // sp_stamps_transaction_new (ISNULL(STAMPS_RESPONSE_TRANSACTION_ID,0) = 0), supaya SO ini
    // otomatis tidak ikut terpilih lagi di siklus berikutnya — persist di database, bukan cuma
    // in-memory, jadi tetap berlaku walau app pool recycle. -1 tidak akan pernah tertukar
    // dengan ID transaksi asli dari STAMPS (selalu angka positif). WHERE clause key by SO_BRAND
    // + SO_NUMBER (SO_NUMBER SENDIRI TIDAK UNIK LINTAS BRAND di jaws_integration — dikonfirmasi
    // user) DAN masih 0/NULL, sama seperti kriteria SP_STAMPS_ADD_TRANSACTION_WITH_REDEMPTION —
    // supaya tidak salah menimpa baris SO brand lain yang kebetulan bernomor sama, dan tidak
    // menimpa baris yang sudah benar-benar sukses di antara waktu cek dan tulis.
    private static void MarkPermanentFailure(string soBrand, string nomorSO)
    {
        try
        {
            using (SqlConnection cnn = new SqlConnection(Db.Cnn_IN))
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE [jaws_integration].[dbo].[SALESORDER] " +
                "SET STAMPS_RESPONSE_TRANSACTION_ID = -1 " +
                "WHERE SO_BRAND = @SO_BRAND AND SO_NUMBER = @SO_NUMBER AND ISNULL(STAMPS_RESPONSE_TRANSACTION_ID,0) = 0", cnn))
            {
                cmd.Parameters.Add(new SqlParameter("@SO_BRAND", SqlDbType.VarChar) { Value = soBrand });
                cmd.Parameters.Add(new SqlParameter("@SO_NUMBER", SqlDbType.VarChar) { Value = nomorSO });
                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch { /* kalau gagal tulis sentinel, exclusion in-memory di atas tetap jalan sebagai fallback */ }
    }

    // Db.Execute (Library.dll) cuma punya overload raw-SQL tanpa parameter, jadi di sini
    // pakai ADO manual seperti pola Rs() di STAMPSM.cs — parameterized, aman dari SQL injection.
    private static void LogWorker(int idso, string proses, string keterangan)
    {
        try
        {
            using (SqlConnection cnn = new SqlConnection(Db.Cnn_DB))
            using (SqlCommand cmd = new SqlCommand("sp_insert_log_salesorder", cnn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IDSO", SqlDbType.Int) { Value = idso });
                cmd.Parameters.Add(new SqlParameter("@PROSES", SqlDbType.VarChar) { Value = proses });
                cmd.Parameters.Add(new SqlParameter("@KETERANGAN", SqlDbType.VarChar) { Value = keterangan ?? "" });
                cnn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        catch { /* logging sendiri tidak boleh sampai menjatuhkan worker */ }
    }
}
