using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Data.SqlClient;


public partial class DOKRM_index : System.Web.UI.Page
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

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        base.Dispose();
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static ISalesLocationResponse GetUserOutletAndDefaultSales()
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getDefaultLocationAndSalesOrAE", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = App.UserID });
                command.Parameters.Add(new SqlParameter("@IsReport", SqlDbType.Int) { Value = 1 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];
                List<IDataLocation> dataLocation = Helper.GetDataFromDataTable<IDataLocation>(dt);
                List<IDataSalesWithLocation> dataSales = Helper.GetDataFromDataTable<IDataSalesWithLocation>(dt2);

                return new ISalesLocationResponse
                {
                    sales = dataSales,
                    location = dataLocation
                };
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<ILocRepair> GetLocReparasi()
    {
        DataTable dt2 = Db.Rs("SELECT ID_LokasiReparasi [ID], JenisLokasi [Nama], LokasiReparasi [Kode] FROM [DB_PROD_MANUFAKTUR].[dbmfg].[dbo].[msLokasiReparasi] WHERE StatusAktif = 'A'", Db.Cnn_DB);

        List<ILocRepair> locRepair = Helper.GetDataFromDataTable<ILocRepair>(dt2);

        locRepair.Insert(0, new ILocRepair
        {
            ID = 0,
            Kode = "ALL",
            Nama = "-- Semua Lokasi Reparasi --"
        });

        return locRepair;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<CancelItem> Cancel(int IDDocRepair = 0) {
        List<CancelItem> cancel = new List<CancelItem>();
        string cancelRpr = @"
                    BEGIN TRANSACTION;
                    BEGIN TRY
                        
                        DELETE FROM DocRepair_Stone1A WHERE IDForm = @ID;
                        DELETE FROM DocRepair_Stone1B WHERE IDForm = @ID;
                        DELETE FROM DocRepair_Stone2 WHERE IDForm = @ID;
                        DELETE FROM DocRepair_Stone3 WHERE IDForm = @ID;
                        DELETE FROM DocRepair_Stone4 WHERE IDForm = @ID;
                        DELETE FROM DocRepair_Stone5 WHERE IDForm = @ID;

                        DELETE A FROM (
                        	SELECT * FROM DocRepair_CharProduct WHERE ID =@ID
                        ) A

                        DELETE A FROM (
                        	SELECT * FROM DocQCDJ_Stone1A WHERE IDForm IN (
                        		SELECT ID FROM DocQCDJ WHERE ID IN (SELECT IDQC FROM DocRepair WHERE ID = @ID)
                        	)
                        ) A
                        
                        DELETE A FROM (
                        	SELECT * FROM DocQCDJ_Stone1B WHERE IDForm IN (
                        		SELECT ID FROM DocQCDJ WHERE ID IN (SELECT IDQC FROM DocRepair WHERE ID = @ID)
                        	)
                        ) A
                        
                        DELETE A FROM (
                        	SELECT * FROM DocQCDJ_Stone2 WHERE IDForm IN (
                        		SELECT ID FROM DocQCDJ WHERE ID IN (SELECT IDQC FROM DocRepair WHERE ID = @ID)
                        	)
                        ) A
                        
                        DELETE A FROM (
                        	SELECT * FROM DocQCDJ_Stone3 WHERE IDForm IN (
                        		SELECT ID FROM DocQCDJ WHERE ID IN (SELECT IDQC FROM DocRepair WHERE ID = @ID)
                        	)
                        ) A
                        
                        DELETE A FROM (
                        	SELECT * FROM DocQCDJ_Stone4 WHERE IDForm IN (
                        		SELECT ID FROM DocQCDJ WHERE ID IN (SELECT IDQC FROM DocRepair WHERE ID = @ID)
                        	)
                        ) A
                        
                        DELETE A FROM (
                        	SELECT * FROM DocQCDJ_Stone5 WHERE IDForm IN (
                        		SELECT ID FROM DocQCDJ WHERE ID IN (SELECT IDQC FROM DocRepair WHERE ID = @ID)
                        	)
                        ) A

                        DELETE A FROM (
	                        SELECT * FROM DocRepair_Package_new WHERE IDDocRepair_Pivot IN (SELECT ID FROM DocRepair_Pivot where IDDocRepair = @ID)
                        ) A
                        
                        DELETE A FROM (
                        	SELECT * FROM DocRepair_Pivot where IDDocRepair = @ID
                        ) A
                        
                        DELETE A FROM (
                        	SELECT * FROM DocRepair WHERE ID = @ID
                        ) A

                        COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH

                        ROLLBACK TRANSACTION;
                        THROW;

                    END CATCH;
                    ";
        try
        {
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cancelRpr, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", IDDocRepair);
                    conn.Open();
                    cmd.ExecuteReader();
                    conn.Close();
                    CancelItem cancelItem = new CancelItem
                    {
                        Code = 200,
                        Message = "Data Repair Berhasil di Cancel"
                    };
                    cancel.Add(cancelItem);
                }
            }
        }
        catch (Exception ex)
        {
            cancel.Add(new CancelItem
            {
                Code = 500,
                Message = ex.Message.ToString()
            });
        }


        return cancel;
    }

    public class CancelItem
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IStatusRepair> GetStatusReparasi()
    {
        string query = "SELECT * FROM StatusReparasi";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);

        List<IStatusRepair> statusRepair = new List<IStatusRepair>();

        statusRepair.Add(new IStatusRepair
        {
            ID = 0,
            Nama = "-- Semua Status --"
        });

        foreach (DataRow dr in dt.Rows)
        {
            statusRepair.Add(new IStatusRepair
            {
                ID = (int)dr["ID"],
                Nama = dr["Nama"].ToString()
            });
        }


        return statusRepair;

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IDocReportRepair> GetDocRepair(string Nomor, int IDLocation)
    {
        DataSet ds = ExecuteDocRepair(new IDocReportRepairArgs
        {
            Nomor = Nomor,
            IDLocation = IDLocation,
        });

        DataTable dt = ds.Tables[0];

        List<IDocReportRepair> docReportRepair = new List<IDocReportRepair>();

        foreach (DataRow dr in dt.Rows)
        {
            docReportRepair.Add(new IDocReportRepair
            {
                NomorDocRepair = (string)dr["RPRNumber"],
                PLU = (string)dr["PLU"],
                IDBrand = (int)dr["IDBrand"],
                IDAsal = (int)dr["IDAsal"],
                IDItem = (int)dr["IDItem"],
                StatusKonfirmasiToko = (string)dr["status_konfirmasi_toko"],
                NamaOutlet = (string)dr["Nama"],
                IDDocRepair = (int)dr["IDDocRepair"]
            });
        }


        return docReportRepair;
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetDetailInvoice(int IDDocRepair) {
        DataSet ds = ExecuteDocRepairPayment(IDDocRepair);        

        DataTable dt = ds.Tables[0];
        List<object> Data = new List<object>();
        foreach (DataRow dr in dt.Rows)
        {
            Data.Add(new
            {
                NomorRepair = (string)dr["NomorRepair"],
                TglRepair = dr["TglRepair"].ToString(),
                NomorInvoice = (string)dr["NomorInvoice"],
                NomorSO = (string)dr["NomorSO"],
                SalesNama = (string)dr["SalesNama"],
                CustomerNama = (string)dr["CustomerNama"],
                NoCustomer = (string)dr["NoCustomer"],
                AddrAlamat = (string)dr["AddrAlamat"],                
                AfterTax = Convert.ToDecimal(dr["AfterTax"]),
                BeforeTax = Convert.ToDecimal(dr["BeforeTax"]),
                Tax = Convert.ToDecimal(dr["Tax"]),
                NamaItem = (string)dr["NamaItem"],
                TglNow = dr["TglNow"].ToString(),
                Store = (string)dr["Store"]
            });
        }
        return Data[0];
    }

    public static DataSet ExecuteDocRepairPayment(int IDDocRepair)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getDetailInvoiceRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.Int) { Value = IDDocRepair });
                command.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.Int) { Value = 2 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }   

    public static DataSet ExecuteDocRepair(IDocReportRepairArgs detailRepair)
    {
        string getRepairConfirmation = @"
            CREATE TABLE #tempRepair(
                ViewType varchar(50), 
                RPRNumber varchar(50),
                idd_reparasi int,                
                PLU varchar(50),
                IDBrand int,
                IDAsal int, 
                IDItem int,
                NamaItem varchar(100),
                KodeItem char(5),
                IDPackage int, 
                JenisReparasi varchar(50),
                SelisihSize numeric(9,2),
                beratTimbang numeric (9,2),
                Sumber int,
                status_konfirmasi_toko varchar(50),
                tanggal varchar(50)
            )
            INSERT INTO #tempRepair
            exec [DB_PROD_MANUFAKTUR].[dbmfg].[dbo].[sp_get_rpr_dalam_pengajuan] @Nomor, @IDBrand, @IDLocation,  @ViewType
            
            SELECT c.ID AS IDDocRepair, RPRNumber, PLU, a.IDBrand, a.IDAsal, a.IDItem
            , IIF(a.status_konfirmasi_toko = 'Dalam Pengajuan','Butuh Konfirmasi',a.status_konfirmasi_toko) as status_konfirmasi_toko, b.Nama
            FROM #tempRepair a
            JOIN LocOutlet b on b.ID = a.IDAsal
            JOIN DocRepair c on c.Nomor = a.RPRNumber

            DROP TABLE #tempRepair
        ";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(getRepairConfirmation, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@Nomor", SqlDbType.VarChar) { Value = detailRepair.Nomor });
                command.Parameters.Add(new SqlParameter("@IDBrand", SqlDbType.Int) { Value = 7 });
                command.Parameters.Add(new SqlParameter("@IDLocation", SqlDbType.Int) { Value = detailRepair.IDLocation });
                command.Parameters.Add(new SqlParameter("@ViewType", SqlDbType.VarChar) { Value = "MainRPRData" });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }

    public class IDocReportRepairArgs
    {
        public string Nomor { get; set; }        
        public int IDLocation { get; set; }        
    }

    public class IDocReportRepair
    {
        public string NomorDocRepair { get; set; }
        public string PLU { get; set; }
        public int IDBrand { get; set; }
        public int IDAsal { get; set; }
        public int IDItem { get; set; }
        public string StatusKonfirmasiToko { get; set; }
        public string NamaOutlet { get; set; } 
        public int IDDocRepair { get; set; }
    }

    public class IInsertDocQCParams
    {
        public string QCNama { get; set; }
        public string KodeCustLama { get; set; }
        public string IDProduct { get; set; }
        public string Nomor { get; set; }
        public string Blemish { get; set; }
        public int JumlahButir { get; set; }
        public string Sertifikat { get; set; }
        public string NonCZ { get; set; }
        public string CZ { get; set; }
        public string Logo { get; set; }
        public string KadarEmas { get; set; }
        public string Merk { get; set; }
        public decimal NetWeight { get; set; }
        public string WarnaEmas { get; set; }
        public string FisikKeseluruhan { get; set; }
        public string OperatorNama { get; set; }
        public int Draft { get; set; }
        public string Keterangan { get; set; }
        public decimal GrossWeight { get; set; }
        public List<object> QC_Additional { get; set; }
        public string TipeProduct { get; set; }
        public string KodeLokasi { get; set; }
    }

    public class IInsertDocRepairParams
    {
        public string PLU { get; set; }
        public string Nomor { get; set; }
        public int TipeLokasi { get; set; }
        public int IDLokasi { get; set; }
        public string KodeLokasi { get; set; }
        public int IDSales { get; set; }
        public string SalesNama { get; set; }
        public string CustomerNama { get; set; }
        public int IDCustomer { get; set; }
        public string Operator { get; set; }
        public string Keterangan { get; set; }
        public string ImgPicture { get; set; }
        public int IDProduct { get; set; }
        public int IDProductTitipan { get; set; }
        public decimal EstimasiHarga { get; set; }
        public int IDQC { get; set; }
        public int IDQCTitipan { get; set; }
        public string TipeProduct { get; set; }
        public int IDLocReparasi { get; set; }
        public List<object> DocRepair_Package { get; set; }
    }

    public class IDataSalesWithLocation
    {
        public int IDSales { get; set; }
        public string NamaSales { get; set; }
        public string NIK { get; set; }
        public string NamaOutlet { get; set; }
        public string KodeOutlet { get; set; }
        public int IDLokasi { get; set; }
        public int TipeLokasi { get; set; }
        public string BrandOutlet { get; set; }
        public int IDBrand { get; set; }
    }

    public class IDataLocation
    {
        public string NamaOutlet { get; set; }
        public string KodeOutlet { get; set; }
        public int IDLokasi { get; set; }
        public int TipeLokasi { get; set; }
        public string BrandOutlet { get; set; }
        public int IDBrand { get; set; }
    }

    public class ISalesLocationResponse
    {
        public List<IDataSalesWithLocation> sales { get; set; }
        public List<IDataLocation> location { get; set; }
    }

    public class ILocRepair
    {
        public int ID { get; set; }
        public string Nama { get; set; }
        public string Kode { get; set; }
    }

    public class IStatusRepair
    {
        public int ID { get; set; }
        public string Nama { get; set; }
    }
}