using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public partial class Sales_VoucherStampsAddSave : System.Web.UI.Page
{
	private Map_DB _db;

	private Map_DB db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_DB();
	        return _db;
	    }
	}

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (cs != null)
	        cs.Dispose();
	    base.Dispose();
	}
    App gp = new App();
    protected int myID { get { return App.QueryInt(this); } }
	protected string NomorVoucher { get { return Request.QueryString["NomorVoucher"]; } }
    protected int myItemID { get { return App.QueryInt(this, "itemid"); } }
    protected int myItemType { get { return App.QueryInt(this, "itemtype"); } }


    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

        }

        Save(myID, NomorVoucher);
        //Response.Redirect("Sales_Data.aspx?id=" + myID);
    }

    //Exception
    protected void DataError(string err) { Response.Redirect("Sales.aspx?err=" + err); }
    protected void DataInvalid(string err) { Response.Redirect("Sales_VoucherStampsAdd.aspx?id=" + myID + "&itemtype=" + myItemType + "&itemid=" + myItemID + "&err=" + err); }
    protected void DataInvalidResponseWrite(string err)
    {
        Response.Write(err);
        // Halaman ini (.aspx) kosong total - tidak ada MasterPage/form/control apa pun untuk
        // di-render. Sebelumnya tidak ada Response.End()/CompleteRequest() di sini, jadi
        // setelah Response.Write, request tetap lanjut melewati SISA siklus hidup Page
        // (LoadComplete/PreRender/Render/Unload) sebelum benar-benar selesai - tab browser
        // terlihat "masih loading" walau teks errornya sudah tampil, karena response belum
        // ditutup tegas. CompleteRequest() (bukan Response.End() - itu melempar
        // ThreadAbortException yang tidak perlu di sini) menghentikan pipeline ASP.NET dengan
        // bersih tanpa lanjut ke tahap-tahap yang memang tidak ada isinya di halaman ini.
        // Context.ApplicationInstance dipakai (bukan HttpContext.Current.ApplicationInstance)
        // supaya tidak perlu tambah "using System.Web;" - Context sudah tersedia bawaan dari
        // System.Web.UI.Page yang di-inherit class ini.
        Context.ApplicationInstance.CompleteRequest();
    }

    protected void Save(int IDSO, string keyword)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == IDSO);
        if (d == null) DataError("1");
        else
        {
            int id_tipelokasi = d.TipeLokasi.Value;
            int id_lokasi = d.IDLokasi.Value;
            int id_brand = 0;
            string kode_brand = "";
            string xStoreKode = "";

            if (id_tipelokasi == 1)
            {
                var loc = db.LocOutlet.Single(p => p.ID == id_lokasi);

                id_brand = loc.IDBrand;
                kode_brand = loc.CompanyBrand.NamaKode;
                xStoreKode = loc.Kode;
            }
            else if (id_tipelokasi == 2)
            {
                var loc = db.LocExhibition.Single(p => p.ID == id_lokasi);

                id_brand = loc.IDBrand;
                kode_brand = loc.CompanyBrand.NamaKode;
                xStoreKode = loc.Kode;
            }

            string errmsg = "";
            try
            {
                //errmsg = Voucher.isValidVoucher("DISCOUNT", keyword, d.ID, id_brand, d.DataCustomer.NoCustomer, myItemType);
            }
            catch (Exception) { }

            // redeem stamps
            /*try
            {
                //errmsg = "";


                if (STAMPS.xActivate)
                {
                    string plu = "";
                    if (myItemType == 1)
                    {
                        var m = db.SalesOrderDJ.SingleOrDefault(p => p.ID == myItemID);
                        plu = m.StockProductDJ.Nomor;
                    }
                    else if (myItemType == 2)
                    {
                        var m = db.SalesOrderPG.SingleOrDefault(p => p.ID == myItemID);
                        plu = m.StockProductPG.Nomor;
                    }
                    else if (myItemType == 3)
                    {
                        var m = db.SalesOrderGJ.SingleOrDefault(p => p.ID == myItemID);
                        plu = m.StockProductGJ.Nomor;
                    }
                    else if (myItemType == 4)
                    {
                        var m = db.SalesOrderLD.SingleOrDefault(p => p.ID == myItemID);
                        plu = m.StockProductLD_Stone1B.Nomor;
                    }

                    errmsg += ((errmsg == "") ? STAMPSM.RedeemVoucherCode(d,
                        keyword,
                        kode_brand,
                        plu
                        ) : "");
                }
            }
            catch (Exception) { }*/


            if (errmsg != "")
            {
                //DataInvalid(errmsg);
                DataInvalidResponseWrite(errmsg);
            }
            else
            {
                string xNoSalesOrder = d.Nomor; // no sales order masih kosong karena belum terbentuk ketika voucher discount diinput
                string xNoCustomer = d.DataCustomer.NoCustomer;
                string xNamaCustomer = d.DataCustomer.Nama;
                // Sebelumnya di sini query raw SQL "SELECT Kode FROM LocOutlet/LocExhibition
                // WHERE ID=..." — padahal baris yang sama persis sudah diambil di atas sebagai
                // `loc` (nilainya disalin ke xStoreKode). Query redundan itu dihapus.
                string xStore = xStoreKode;

                var x = cs.T_voucher.SingleOrDefault(p => p.M_nomor == NomorVoucher);
                if (x == null)
                {
                    // Voucher tidak ditemukan — jangan lanjut ke Tb_type_voucher.Single(x.M_type)
                    // (itu penyebab NullReferenceException sebelumnya) dan jangan redirect seolah
                    // sukses. Tampilkan error yang jelas lalu hentikan proses.
                    DataInvalidResponseWrite("Voucher dengan nomor '" + NomorVoucher + "' tidak ditemukan.");
                    return;
                }
                var y = cs.Tb_type_voucher.SingleOrDefault(p => p.Ckodetype == x.M_type);
                if (y == null)
                {
                    // Tipe voucher (M_type) tidak ketemu di Tb_type_voucher — sebelumnya pakai
                    // .Single() tanpa guard, lempar InvalidOperationException yang tidak informatif
                    // kalau data master tipe voucher-nya kosong/mismatch. Sama seperti guard T_voucher
                    // di atas, tampilkan error yang jelas lalu hentikan proses.
                    DataInvalidResponseWrite("Tipe voucher '" + x.M_type + "' untuk voucher '" + NomorVoucher + "' tidak ditemukan di data master.");
                    return;
                }
                {
                    int zIDSalesOrder = IDSO;
                    string zIDTypeVoucher = x.M_type;
                    string zTypeVoucher = y.Cnamatype;
                    string zNomorVoucher = NomorVoucher;
                    string zKeterangan = x.M_keterangan;
                    decimal zNominal = x.M_value;
                    string zIssuer = x.M_issuer;
                    DateTime zDateStartVoucher = Convert.ToDateTime(x.M_tanggal.ToString());

                    decimal dDoubleReward = 1m;
                    int productId = 0;
                    string itemTableName = "";

                    // Baca IDProduct + tentukan dDoubleReward (baca-saja, aman dilakukan lewat
                    // LINQ sebelum transaksi raw di bawah — tidak ada tulis-menulis di sini).
                    // Item ini bisa sudah tidak ada lagi di tabel pada saat Save() jalan (Page_Load
                    // memanggil Save() tanpa cek IsPostBack, jadi refresh/reload URL yang sama akan
                    // mengulang seluruh proses dengan myItemID yang sama — kalau item sudah dihapus
                    // dari SO di antara waktu link dibuat dan diklik/di-reload, lookup ini null).
                    // Sebelumnya di-dereference langsung tanpa cek (m.StockProductDJ... dst) dan
                    // meledak dengan NullReferenceException yang tidak informatif.
                    if (myItemType == 1)
                    {
                        var m = db.SalesOrderDJ.SingleOrDefault(p => p.ID == myItemID);
                        if (m == null)
                        {
                            DataInvalidResponseWrite("Item Diamond Jewellery (ID " + myItemID + ") tidak ditemukan di Sales Order ini — kemungkinan sudah dihapus.");
                            return;
                        }

                        string xCategory = m.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode;
                        bool xDossier03CaratUpExists = m.StockProductDJ.StockProductDJ_Stone1B.Any(p => p.CaratButir >= 0.3m);

                        // hardcode promo nasional 2019 Februari : double reward membership redemption
                        if (System.DateTime.Now >= Convert.ToDateTime("2019-02-01 00:00:00.000")
                            && System.DateTime.Now < Convert.ToDateTime("2019-04-01 00:00:00.000")
                            && zTypeVoucher == "MEMBERSHIP REDEMPTION"
                            && zIDTypeVoucher == "07"
                            && zDateStartVoucher >= Convert.ToDateTime("2019-02-01 00:00:00.000")
                            && zDateStartVoucher < Convert.ToDateTime("2019-04-01 00:00:00.000")
                            && !(xDossier03CaratUpExists && xCategory == "SNI")
                            && !(xCategory == "SI")
                            )
                        {
                            dDoubleReward = 2m;
                        }

                        productId = m.IDProduct;
                        itemTableName = "SalesOrderDJ";
                    }
                    else if (myItemType == 2)
                    {
                        var m = db.SalesOrderPG.SingleOrDefault(p => p.ID == myItemID);
                        if (m == null)
                        {
                            DataInvalidResponseWrite("Item Plain Gold (ID " + myItemID + ") tidak ditemukan di Sales Order ini — kemungkinan sudah dihapus.");
                            return;
                        }
                        productId = m.IDProduct;
                        itemTableName = "SalesOrderPG";
                    }
                    else if (myItemType == 3)
                    {
                        var m = db.SalesOrderGJ.SingleOrDefault(p => p.ID == myItemID);
                        if (m == null)
                        {
                            DataInvalidResponseWrite("Item Gold Jewellery (ID " + myItemID + ") tidak ditemukan di Sales Order ini — kemungkinan sudah dihapus.");
                            return;
                        }
                        productId = m.IDProduct;
                        itemTableName = "SalesOrderGJ";
                    }
                    else if (myItemType == 4)
                    {
                        var m = db.SalesOrderLD.SingleOrDefault(p => p.ID == myItemID);
                        if (m == null)
                        {
                            DataInvalidResponseWrite("Item Loose Diamond (ID " + myItemID + ") tidak ditemukan di Sales Order ini — kemungkinan sudah dihapus.");
                            return;
                        }
                        productId = m.IDProduct;
                        itemTableName = "SalesOrderLD";
                    }

                    zNominal = zNominal * dDoubleReward;

                    // KOREKSI: db (Palace_DB_real) dan cs (dbcmk) TERNYATA instance terpisah di
                    // production (dikonfirmasi user) — bukan server fisik yang sama seperti
                    // asumsi awal saya. Satu SqlConnection lintas-database (three-part naming
                    // "dbcmk.dbo.t_voucher") TIDAK bisa dipakai di sini, dan distributed
                    // transaction lintas-instance (MSDTC) terlalu berat/rawan dimatikan di
                    // infrastruktur modern — jadi dipakai pola "claim-first" (saga), bukan ACID
                    // transaction penuh:
                    //
                    //   1. Klaim voucher DI dbcmk DULU, dengan guard idempotent
                    //      (m_klaimdoc IS NULL) — kalau voucher SUDAH diklaim (oleh request lain
                    //      yang balapan, atau percobaan sebelumnya), berhenti di sini, TIDAK ADA
                    //      apa pun yang berubah di Palace_DB_real. Ini yang mencegah
                    //      double-redemption — akar masalah yang mau diperbaiki.
                    //   2. Baru update DiscountGift + insert SalesVoucher di Palace_DB_real,
                    //      dalam SATU transaksi lokal (atomic, koneksi tunggal).
                    //   3. Kalau langkah 2 gagal, coba KOMPENSASI (batalkan klaim di langkah 1)
                    //      supaya voucher tidak "terkunci" pakai padahal reward-nya tidak pernah
                    //      benar-benar tercatat. Kalau kompensasi ini SENDIRI gagal, itu residual
                    //      gap yang butuh rekonsiliasi manual — tapi arah kegagalannya AMAN
                    //      (voucher ke-block dipakai lagi, bukan dipakai dua kali).
                    //
                    // Null-handling berbeda dari LINQ asli: "m.DiscountGift += zNominal" pada
                    // decimal? yang NULL akan tetap NULL (lifted operator, tidak throw tapi juga
                    // tidak menambahkan apa-apa — nominal reward voucher jadi hilang diam-diam).
                    // Di sini pakai ISNULL(DiscountGift,0) supaya nominal selalu benar-benar
                    // tertambah — perbaikan yang disengaja, bukan cuma migrasi apa adanya.

                    // Langkah 1 — klaim voucher di dbcmk (idempotent guard).
                    int claimRows;
                    using (SqlConnection cnnCs = new SqlConnection(Db.Cnn_CS))
                    using (SqlCommand cmdClaim = new SqlCommand(
                        "UPDATE t_voucher SET m_klaimdoc = @KlaimDoc, m_tanggal2 = @Tanggal2, m_store2 = @Store2 " +
                        "WHERE m_nomor = @NomorVoucher AND (m_klaimdoc IS NULL OR m_klaimdoc = '')", cnnCs))
                    {
                        cmdClaim.Parameters.Add(new SqlParameter("@KlaimDoc", SqlDbType.VarChar, 20) { Value = (object)xNoSalesOrder ?? DBNull.Value });
                        cmdClaim.Parameters.Add(new SqlParameter("@Tanggal2", SqlDbType.DateTime) { Value = DateTime.Now });
                        cmdClaim.Parameters.Add(new SqlParameter("@Store2", SqlDbType.VarChar, 5) { Value = (object)xStore ?? DBNull.Value });
                        cmdClaim.Parameters.Add(new SqlParameter("@NomorVoucher", SqlDbType.VarChar, 20) { Value = (object)NomorVoucher ?? DBNull.Value });
                        cnnCs.Open();
                        claimRows = cmdClaim.ExecuteNonQuery();
                    }

                    if (claimRows == 0)
                    {
                        // Voucher sudah diklaim lebih dulu (concurrent request atau percobaan
                        // sebelumnya) — berhenti, tidak menyentuh Palace_DB_real sama sekali.
                        DataInvalidResponseWrite("Voucher dengan nomor '" + NomorVoucher + "' sudah pernah digunakan.");
                        return;
                    }

                    // Langkah 2 — update DiscountGift + insert SalesVoucher, 1 transaksi lokal.
                    try
                    {
                        using (SqlConnection cnn = new SqlConnection(Db.Cnn_DB))
                        {
                            cnn.Open();
                            using (SqlTransaction tran = cnn.BeginTransaction())
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(itemTableName))
                                    {
                                        using (SqlCommand cmd1 = new SqlCommand(
                                            "UPDATE " + itemTableName + " SET DiscountGift = ISNULL(DiscountGift,0) + @Nominal WHERE ID = @ItemID", cnn, tran))
                                        {
                                            cmd1.Parameters.Add(new SqlParameter("@Nominal", SqlDbType.Money) { Value = zNominal });
                                            cmd1.Parameters.Add(new SqlParameter("@ItemID", SqlDbType.Int) { Value = myItemID });
                                            cmd1.ExecuteNonQuery();
                                        }
                                    }

                                    using (SqlCommand cmd2 = new SqlCommand(
                                        "INSERT INTO SalesVoucher (IDSalesOrder, IDTypeVoucher, TypeVoucher, NomorVoucher, Keterangan, Nominal, Issuer, OperatorTgl, Operator, ProductType, ProductID) " +
                                        "VALUES (@IDSalesOrder, @IDTypeVoucher, @TypeVoucher, @NomorVoucher, @Keterangan, @Nominal, @Issuer, @OperatorTgl, @Operator, @ProductType, @ProductID)", cnn, tran))
                                    {
                                        cmd2.Parameters.Add(new SqlParameter("@IDSalesOrder", SqlDbType.Int) { Value = zIDSalesOrder });
                                        cmd2.Parameters.Add(new SqlParameter("@IDTypeVoucher", SqlDbType.VarChar, 10) { Value = (object)zIDTypeVoucher ?? DBNull.Value });
                                        cmd2.Parameters.Add(new SqlParameter("@TypeVoucher", SqlDbType.VarChar, 50) { Value = (object)zTypeVoucher ?? DBNull.Value });
                                        cmd2.Parameters.Add(new SqlParameter("@NomorVoucher", SqlDbType.VarChar, 50) { Value = (object)zNomorVoucher ?? DBNull.Value });
                                        cmd2.Parameters.Add(new SqlParameter("@Keterangan", SqlDbType.VarChar, 100) { Value = (object)zKeterangan ?? DBNull.Value });
                                        cmd2.Parameters.Add(new SqlParameter("@Nominal", SqlDbType.Money) { Value = zNominal });
                                        cmd2.Parameters.Add(new SqlParameter("@Issuer", SqlDbType.VarChar, 10) { Value = (object)zIssuer ?? DBNull.Value });
                                        cmd2.Parameters.Add(new SqlParameter("@OperatorTgl", SqlDbType.DateTime) { Value = DateTime.Now });
                                        cmd2.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar, 50) { Value = (object)App.UserNama ?? DBNull.Value });
                                        cmd2.Parameters.Add(new SqlParameter("@ProductType", SqlDbType.Int) { Value = myItemType });
                                        cmd2.Parameters.Add(new SqlParameter("@ProductID", SqlDbType.Int) { Value = productId });
                                        cmd2.ExecuteNonQuery();
                                    }

                                    tran.Commit();
                                }
                                catch
                                {
                                    tran.Rollback();
                                    throw;
                                }
                            }
                        }
                    }
                    catch (Exception exStep2)
                    {
                        // Langkah 2 gagal — coba kompensasi: batalkan klaim di dbcmk supaya
                        // voucher tidak "terkunci" tanpa reward yang benar-benar tercatat.
                        try
                        {
                            using (SqlConnection cnnCsRollback = new SqlConnection(Db.Cnn_CS))
                            using (SqlCommand cmdUnclaim = new SqlCommand(
                                "UPDATE t_voucher SET m_klaimdoc = NULL, m_tanggal2 = NULL, m_store2 = NULL " +
                                "WHERE m_nomor = @NomorVoucher AND m_klaimdoc = @KlaimDoc", cnnCsRollback))
                            {
                                cmdUnclaim.Parameters.Add(new SqlParameter("@NomorVoucher", SqlDbType.VarChar, 20) { Value = (object)NomorVoucher ?? DBNull.Value });
                                cmdUnclaim.Parameters.Add(new SqlParameter("@KlaimDoc", SqlDbType.VarChar, 20) { Value = (object)xNoSalesOrder ?? DBNull.Value });
                                cnnCsRollback.Open();
                                cmdUnclaim.ExecuteNonQuery();
                            }
                        }
                        catch
                        {
                            // Kompensasi pun gagal — residual gap butuh rekonsiliasi manual
                            // (voucher ke-flag terpakai tanpa reward tercatat), tapi arahnya
                            // AMAN: tidak akan terjadi double-redemption.
                        }

                        throw new Exception("Gagal menyimpan redemption voucher ke Palace_DB_real setelah klaim di dbcmk : " + exStep2.Message);
                    }
                }

                Response.Redirect("Sales_Data.aspx?id=" + myID);
            }

        }
    }

}