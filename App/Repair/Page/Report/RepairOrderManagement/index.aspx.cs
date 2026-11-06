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
    public static List<ILocRepair> GetLocationRepair()
    {
        var locationRepair = Db.Rs("SELECT ID_LokasiReparasi [ID], JenisLokasi [Nama], LokasiReparasi [Kode] FROM [DB_PROD_MANUFAKTUR].[dbmfg].[dbo].[msLokasiReparasi] WHERE StatusAktif = 'A'", Db.Cnn_DB);
        List<ILocRepair> locRepair = Helper.GetDataFromDataTable<ILocRepair>(locationRepair);

        locRepair.Insert(0, new ILocRepair
        {
            ID = 0,
            Nama = "-- Semua Lokasi --",
            Kode = ""
        });

        return locRepair;
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
            Nama = "-- Semua --"
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
    public static List<IDocReportRepair> GetDocRepair(string PLU, string TglStart, string TglEnd, int IDBrand, int IDLocation, int TipeLocation, string TipeProduct, int IDlocReparasi, int Status)
    {
        DataSet ds = ExecuteDocRepair(new IDocReportRepairArgs
        {
            PLU = PLU,
            TglStart = TglStart,
            TglEnd = TglEnd,
            IDBrand = IDBrand,
            IDLocation = IDLocation,
            TipeLocation = TipeLocation,
            TipeProduct = TipeProduct,
            IDLocReparasi = IDlocReparasi,
            Status = Status
        });

        DataTable dt = ds.Tables[0];

        List<IDocReportRepair> docReportRepair = new List<IDocReportRepair>();

        foreach (DataRow dr in dt.Rows)
        {
            docReportRepair.Add(new IDocReportRepair
            {
                IDDocRepair = (int)dr["IDDocRepair"],
                NamaCustomer = dr["NamaCustomer"].ToString(),
                NamaSales = dr["NamaSales"].ToString(),
                No = (int)dr["No"],
                NomorDocRepair = dr["NomorDocRepair"].ToString(),
                NomorInvoice = dr["NomorInvoice"].ToString(),
                PLU = dr["PLU"].ToString(),
                IDStatus = (int)dr["IDStatus"],
                Status = dr["Status"].ToString()
            });
        }


        return docReportRepair;
    }

    public static DataSet ExecuteDocRepair(IDocReportRepairArgs detailRepair)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getReportRepair_new", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@PLU", SqlDbType.VarChar) { Value = detailRepair.PLU });
                command.Parameters.Add(new SqlParameter("@TglStart", SqlDbType.Date) { Value = detailRepair.TglStart });
                command.Parameters.Add(new SqlParameter("@TglEnd", SqlDbType.Date) { Value = detailRepair.TglEnd });
                command.Parameters.Add(new SqlParameter("@IDBrand", SqlDbType.Int) { Value = detailRepair.IDBrand });
                command.Parameters.Add(new SqlParameter("@IDLocation", SqlDbType.Int) { Value = detailRepair.IDLocation });
                command.Parameters.Add(new SqlParameter("@TipeLocation", SqlDbType.Int) { Value = detailRepair.TipeLocation });
                command.Parameters.Add(new SqlParameter("@TipeProduct", SqlDbType.VarChar) { Value = detailRepair.TipeProduct });
                command.Parameters.Add(new SqlParameter("@IDLocReparasi", SqlDbType.VarChar) { Value = detailRepair.IDLocReparasi });
                command.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int) { Value = detailRepair.Status });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }

    public class IDocReportRepairArgs
    {
        public string PLU { get; set; }
        public string TglStart { get; set; }
        public string TglEnd { get; set; }
        public int IDBrand { get; set; }
        public int IDLocation { get; set; }
        public int TipeLocation { get; set; }
        public string TipeProduct { get; set; }
        public int IDLocReparasi { get; set; }
        public int Status { get; set; }
    }

    public class IDocReportRepair
    {
        public int No { get; set; }
        public int IDDocRepair { get; set; }
        public string NomorDocRepair { get; set; }
        public string NomorInvoice { get; set; }
        public string PLU { get; set; }
        public string NamaCustomer { get; set; }
        public string NamaSales { get; set; }
        public int IDStatus { get; set; }
        public string Status { get; set; }
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