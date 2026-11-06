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

    public static List<IDataSalesWithLocation> CheckSales(int IDUser = 0, string KodeLokasi = "")
    {
        string query = "SELECT" +
        " B.ID [IDSales], B.Nama [NamaSales], B.Keterangan [NIK], C.Nama [NamaOutlet], C.NamaKode [KodeOutlet], CASE WHEN D.ID IS NULL THEN E.ID ELSE D.ID END [IDLokasi], CASE WHEN D.ID IS NULL THEN 2 ELSE 1 END [TipeLokasi] " + 
        " FROM frank_ac.dbo.UserAccount A" + 
        " JOIN DataSales B ON A.UserNumber = B.Keterangan" + 
        " JOIN DataSalesGroup C ON B.IDGroup = C.ID" + 
        " LEFT JOIN LocOutlet D ON C.NamaKode = D.Kode" +
        " LEFT JOIN LocExhibition E ON C.NamaKode = E.Kode" +
        " WHERE B.Disable = 0 AND C.Disable = 0 AND ((D.ID IS NOT NULL AND D.Disable = 0) OR (E.ID IS NOT NULL AND E.Disable = 0))";

        if(IDUser != 0){
            query += " AND A.ID = " + IDUser;
        }

        if(KodeLokasi != "") {
            query += " AND C.NamaKode = '" + KodeLokasi + "'";
        }

        string queryAdd = "SELECT * FROM ( " + query + " ) A ORDER BY A.TipeLokasi, A.IDLokasi";

        DataTable dt = Db.Rs(queryAdd, Db.Cnn_DB);
        List<IDataSalesWithLocation> dataSales = new List<IDataSalesWithLocation>();
        foreach(DataRow dr in dt.Rows) {
            IDataSalesWithLocation sales = new IDataSalesWithLocation();
            sales.IDSales = (int)dr["IDSales"];
            sales.NamaSales = dr["NamaSales"].ToString();
            sales.NIK = dr["NIK"].ToString();
            sales.NamaOutlet = dr["NamaOutlet"].ToString();
            sales.IDLokasi = (int)dr["IDLokasi"];
            sales.TipeLokasi = (int)dr["TipeLokasi"];
            sales.NamaOutlet = dr["NamaOutlet"].ToString();
            sales.KodeOutlet = dr["KodeOutlet"].ToString();
            dataSales.Add(sales);
        }

        return dataSales;
    }

    public static List<IDataSalesWithLocation> CheckAdminSales(int IDUser = 0)
    {
        string query = "SELECT " +
        " B.ID [IDSales], B.Nama [NamaSales], B.Keterangan [NIK], CASE WHEN B.TipeLokasi = 1 THEN C.Nama ELSE D.Nama END [NamaOutlet], CASE WHEN B.TipeLokasi = 1 THEN C.Kode ELSE D.Kode END [KodeOutlet], B.IDLokasi [IDLokasi], B.TipeLokasi [TipeLokasi]" + 
        " FROM frank_ac.dbo.UserAccount A" + 
        " JOIN DataAdminSales B ON A.UserNumber = B.Keterangan" + 
        " LEFT JOIN LocOutlet C ON B.TipeLokasi = 1 AND C.ID = B.IDLokasi" + 
        " LEFT JOIN LocExhibition D ON B.TipeLokasi = 2 AND C.ID = B.IDLokasi" +
        " WHERE B.Disable = 0 AND ((B.TipeLokasi = 1 AND C.Disable = 0) OR (B.TipeLokasi = 2 AND D.Disable = 0))";


        if(IDUser != 0){
            query += " AND A.ID = " + IDUser;
        }

        string queryAdd = query + " ORDER BY B.TipeLokasi, B.IDLokasi";

        DataTable dt = Db.Rs(queryAdd, Db.Cnn_DB);

        List<IDataSalesWithLocation> dataSales = new List<IDataSalesWithLocation>();
        foreach(DataRow dr in dt.Rows) {
            IDataSalesWithLocation sales = new IDataSalesWithLocation();
            sales.IDSales = (int)dr["IDSales"];
            sales.NamaSales = dr["NamaSales"].ToString();
            sales.NIK = dr["NIK"].ToString();
            sales.NamaOutlet = dr["NamaOutlet"].ToString();
            sales.IDLokasi = (int)dr["IDLokasi"];
            sales.TipeLokasi = (int)dr["TipeLokasi"];
            sales.NamaOutlet = dr["NamaOutlet"].ToString();
            sales.KodeOutlet = dr["KodeOutlet"].ToString();
            dataSales.Add(sales);
        }

        return dataSales;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static ISalesLocationResponse GetUserOutletAndDefaultSales()
    {
        List<IDataSalesWithLocation> dataSalesLocation = CheckSales(App.UserID);
        List<IDataSalesWithLocation> dataAdminSalesLocation = CheckAdminSales(App.UserID);


        List<IDataSalesWithLocation> dataSales = new List<IDataSalesWithLocation>();
        List<IDataLocation> dataLocation = new List<IDataLocation>();

        if(dataSalesLocation.Count() == 0 && dataAdminSalesLocation.Count() == 0) {
            List<IDataSalesWithLocation> dataSalesLocationAll = CheckSales();

            foreach(IDataSalesWithLocation salesLocation in dataSalesLocationAll) {
                dataSales.Add(salesLocation);
            }

            List<IDataLocation> locations = Location();

            foreach(IDataLocation location in locations) {
                dataLocation.Add(location);
            } 

            return new ISalesLocationResponse {
                sales = dataSales, 
                location = dataLocation
            };
        }

        if(dataSalesLocation.Count() == 0 && dataAdminSalesLocation.Count() > 0) {
            foreach(IDataSalesWithLocation adminSalesLocation in dataAdminSalesLocation) {
                IDataLocation location = new IDataLocation {
                    NamaOutlet = adminSalesLocation.NamaOutlet,
                    KodeOutlet = adminSalesLocation.KodeOutlet,
                    IDLokasi = adminSalesLocation.IDLokasi,
                    TipeLokasi = adminSalesLocation.TipeLokasi
                };

                dataLocation.Add(location);
            }

            foreach(IDataLocation location in dataLocation){
                List<IDataSalesWithLocation> dataSalesLocationAll = CheckSales(0, location.KodeOutlet);

                foreach(IDataSalesWithLocation dataSalesWithLocation in dataSalesLocationAll) {
                    dataSales.Add(dataSalesWithLocation);
                }
            }

            

            return new ISalesLocationResponse {
                sales = dataSales, 
                location = dataLocation
            };
        }

        foreach(IDataSalesWithLocation salesLocation in dataSalesLocation) {
            IDataLocation location = new IDataLocation {
                NamaOutlet = salesLocation.NamaOutlet,
                KodeOutlet = salesLocation.KodeOutlet,
                IDLokasi = salesLocation.IDLokasi,
                TipeLokasi = salesLocation.TipeLokasi
            };

            dataSales.Add(salesLocation);
            dataLocation.Add(location);
        }

        return new ISalesLocationResponse {
            sales = dataSales, 
            location = dataLocation
        };
    }
    
    public static List<IDataLocation> Location()
    {
        DataTable dt = Db.Rs("SELECT ID, Nama, Kode, 1 [TipeLokasi] FROM LocOutlet WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB); 
        DataTable dt2 = Db.Rs("SELECT ID, Nama, Kode, 2 [TipeLokasi] FROM LocExhibition WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB); 
        
        List<IDataLocation> locations = new List<IDataLocation>();
        foreach(DataRow dr in dt.Rows) {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            locations.Add(location);
        }

        foreach(DataRow dr in dt2.Rows) {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            locations.Add(location);
        }

        return locations;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<ILocRepair> GetLocReparasi(){
        DataTable dt2 = Db.Rs("SELECT * FROM LocReparasi", Db.Cnn_DB);

        List<ILocRepair> locRepair = new List<ILocRepair>();
        foreach(DataRow dr in dt2.Rows) {
            ILocRepair loc = new ILocRepair {
                ID = (int)dr["ID"],
                Kode = dr["Kode"].ToString(),
                Nama = dr["Nama"].ToString()
            };
            locRepair.Add(loc);
        }

        
        return locRepair;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IStatusRepair> GetStatusReparasi() {
        string query = "SELECT * FROM StatusReparasi";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);

        List<IStatusRepair> statusRepair = new List<IStatusRepair>();

        statusRepair.Add(new IStatusRepair {
            ID = 0,
            Nama = "-- Semua --"
        });

        foreach(DataRow dr in dt.Rows) {
            statusRepair.Add(new IStatusRepair {
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
        DataSet ds = ExecuteDocRepair(new IDocReportRepairArgs {
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

        foreach(DataRow dr in dt.Rows) {
            docReportRepair.Add(new IDocReportRepair {
                IDDocRepair = (int)dr["IDDocRepair"],
                IDDocRepairPivot = (int)dr["IDDocRepairPivot"],
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

    [WebMethod]
    public static void ApprovePaymentRepair(int IDDocRepairPivot) {
        string query = "UPDATE DocRepair_Pivot SET Status = 9 WHERE ID = " + IDDocRepairPivot;
        Db.Rs(query, Db.Cnn_DB); 
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

    public class IDocReportRepairArgs {
        public string PLU {get;set;}
        public string TglStart {get;set;}
        public string TglEnd {get;set;}
        public int IDBrand {get;set;}
        public int IDLocation {get;set;}
        public int TipeLocation {get;set;}
        public string TipeProduct {get;set;}
        public int IDLocReparasi {get;set;}
        public int Status {get;set;}
    }

    public class IDocReportRepair {
        public int No {get;set;}
        public int IDDocRepair {get;set;}
        public int IDDocRepairPivot {get;set;}
        public string NomorDocRepair {get;set;}
        public string NomorInvoice {get;set;}
        public string PLU {get;set;}
        public string NamaCustomer {get;set;}
        public string NamaSales {get;set;}
        public int IDStatus {get;set;}
        public string Status {get;set;}
    }

    public class IInsertDocQCParams {
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

    public class IInsertDocRepairParams {
        public string PLU { get; set; }
        public string Nomor { get; set; }
        public int TipeLokasi { get; set; }
        public int IDLokasi { get; set; }
        public string KodeLokasi { get; set; }
        public int IDSales { get; set; }
        public string SalesNama { get; set; }
        public string CustomerNama { get; set; }
        public int IDCustomer {get;set;}
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

    public class IDataSalesWithLocation {
        public int IDSales {get; set;}
        public string NamaSales {get; set;}
        public string NIK {get; set;}
        public string NamaOutlet {get; set;}
        public string KodeOutlet {get; set;}
        public int IDLokasi {get; set;}
        public int TipeLokasi {get; set;}
    }

    public class IDataLocation {
        public string NamaOutlet {get; set;}
        public string KodeOutlet {get; set;}
        public int IDLokasi {get; set;}
        public int TipeLokasi {get; set;}
    }

    public class ISalesLocationResponse {
        public List<IDataSalesWithLocation> sales {get; set;}
        public List<IDataLocation> location {get; set;}
    }

    public class ILocRepair {
        public int ID {get;set;}
        public string Nama {get; set;}
        public string Kode {get; set;}
    }

    public class IStatusRepair {
        public int ID {get;set;}
        public string Nama {get;set;}
    }
}