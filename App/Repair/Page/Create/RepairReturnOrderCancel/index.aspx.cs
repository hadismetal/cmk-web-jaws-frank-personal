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


public partial class RepairDeliveryOrder_Index : System.Web.UI.Page
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
        " B.ID [IDSales], B.Nama [NamaSales], B.Keterangan [NIK], C.Nama [NamaOutlet], C.NamaKode [KodeOutlet], CASE WHEN D.ID IS NULL THEN E.ID ELSE D.ID END [IDLokasi], CASE WHEN D.ID IS NULL THEN 2 ELSE 1 END [TipeLokasi], " +
        "CASE WHEN D.ID IS NOT NULL AND D.IDBrand = 7 THEN 'F'" +
        "WHEN D.ID IS NOT NULL AND D.IDBrand = 5 THEN 'P'" +
        "WHEN D.ID IS NOT NULL AND D.IDBrand NOT IN (5,7) THEN 'M'" +
        "WHEN E.ID IS NOT NULL AND E.IDBrand = 7 THEN 'F'" +
        "WHEN E.ID IS NOT NULL AND E.IDBrand = 5 THEN 'P'" +
        "WHEN E.ID IS NOT NULL AND E.IDBrand NOT IN (5,7) THEN 'M' END [Brand], " +
        "CASE WHEN D.ID IS NOT NULL THEN D.IDBrand ELSE E.IDBrand END [IDBrand] " +
        " FROM frank_ac.dbo.UserAccount A" +
        " JOIN DataSales B ON A.UserNumber = B.Keterangan" +
        " JOIN DataSalesGroup C ON B.IDGroup = C.ID" +
        " LEFT JOIN LocOutlet D ON C.NamaKode = D.Kode" +
        " LEFT JOIN LocExhibition E ON C.NamaKode = E.Kode" +
        " WHERE B.Disable = 0 AND C.Disable = 0 AND ((D.ID IS NOT NULL AND D.Disable = 0) OR (E.ID IS NOT NULL AND E.Disable = 0))";

        if (IDUser != 0)
        {
            query += " AND A.ID = " + IDUser;
        }

        if (KodeLokasi != "")
        {
            query += " AND C.NamaKode = '" + KodeLokasi + "'";
        }

        string queryAdd = "SELECT * FROM ( " + query + " ) A ORDER BY A.TipeLokasi, A.IDLokasi";

        DataTable dt = Db.Rs(queryAdd, Db.Cnn_DB);
        List<IDataSalesWithLocation> dataSales = new List<IDataSalesWithLocation>();
        foreach (DataRow dr in dt.Rows)
        {
            IDataSalesWithLocation sales = new IDataSalesWithLocation();
            sales.IDSales = (int)dr["IDSales"];
            sales.NamaSales = dr["NamaSales"].ToString();
            sales.NIK = dr["NIK"].ToString();
            sales.NamaOutlet = dr["NamaOutlet"].ToString();
            sales.IDLokasi = (int)dr["IDLokasi"];
            sales.TipeLokasi = (int)dr["TipeLokasi"];
            sales.NamaOutlet = dr["NamaOutlet"].ToString();
            sales.KodeOutlet = dr["KodeOutlet"].ToString();
            sales.BrandOutlet = dr["Brand"].ToString();
            sales.IDBrand = (int)dr["IDBrand"];
            dataSales.Add(sales);
        }

        return dataSales;
    }

    public static List<IDataSalesWithLocation> CheckAdminSales(int IDUser = 0)
    {
        string query = "SELECT " +
        " B.ID [IDSales], B.Nama [NamaSales], B.Keterangan [NIK], CASE WHEN B.TipeLokasi = 1 THEN C.Nama ELSE D.Nama END [NamaOutlet], CASE WHEN B.TipeLokasi = 1 THEN C.Kode ELSE D.Kode END [KodeOutlet], B.IDLokasi [IDLokasi], B.TipeLokasi [TipeLokasi], " +
        "CASE WHEN C.ID IS NOT NULL AND C.IDBrand = 7 THEN 'F'" +
        " WHEN C.ID IS NOT NULL AND C.IDBrand = 5 THEN 'P'" +
        " WHEN C.ID IS NOT NULL AND C.IDBrand NOT IN (5,7) THEN 'M'" +
        " WHEN D.ID IS NOT NULL AND D.IDBrand = 7 THEN 'F'" +
        " WHEN D.ID IS NOT NULL AND D.IDBrand = 5 THEN 'P'" +
        " WHEN D.ID IS NOT NULL AND D.IDBrand NOT IN (5,7) THEN 'M' END [Brand]," +
        "CASE WHEN C.ID IS NOT NULL THEN C.IDBrand ELSE D.IDBrand END [IDBrand]" +
        " FROM frank_ac.dbo.UserAccount A" +
        " JOIN DataAdminSales B ON A.UserNumber = B.Keterangan" +
        " LEFT JOIN LocOutlet C ON B.TipeLokasi = 1 AND C.ID = B.IDLokasi" +
        " LEFT JOIN LocExhibition D ON B.TipeLokasi = 2 AND C.ID = B.IDLokasi" +
        " WHERE B.Disable = 0 AND ((B.TipeLokasi = 1 AND C.Disable = 0) OR (B.TipeLokasi = 2 AND D.Disable = 0))";


        if (IDUser != 0)
        {
            query += " AND A.ID = " + IDUser;
        }

        string queryAdd = query + " ORDER BY B.TipeLokasi, B.IDLokasi";

        DataTable dt = Db.Rs(queryAdd, Db.Cnn_DB);

        List<IDataSalesWithLocation> dataSales = new List<IDataSalesWithLocation>();
        foreach (DataRow dr in dt.Rows)
        {
            IDataSalesWithLocation sales = new IDataSalesWithLocation();
            sales.IDSales = (int)dr["IDSales"];
            sales.NamaSales = dr["NamaSales"].ToString();
            sales.NIK = dr["NIK"].ToString();
            sales.NamaOutlet = dr["NamaOutlet"].ToString();
            sales.IDLokasi = (int)dr["IDLokasi"];
            sales.TipeLokasi = (int)dr["TipeLokasi"];
            sales.NamaOutlet = dr["NamaOutlet"].ToString();
            sales.KodeOutlet = dr["KodeOutlet"].ToString();
            sales.BrandOutlet = dr["Brand"].ToString();
            sales.IDBrand = (int)dr["IDBrand"];
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

        if (dataSalesLocation.Count() == 0 && dataAdminSalesLocation.Count() == 0)
        {
            List<IDataSalesWithLocation> dataSalesLocationAll = CheckSales();

            foreach (IDataSalesWithLocation salesLocation in dataSalesLocationAll)
            {
                dataSales.Add(salesLocation);
            }

            List<IDataLocation> locations = Location();

            foreach (IDataLocation location in locations)
            {
                dataLocation.Add(location);
            }

            return new ISalesLocationResponse
            {
                sales = dataSales,
                location = dataLocation
            };
        }

        if (dataSalesLocation.Count() == 0 && dataAdminSalesLocation.Count() > 0)
        {
            foreach (IDataSalesWithLocation adminSalesLocation in dataAdminSalesLocation)
            {
                IDataLocation location = new IDataLocation
                {
                    NamaOutlet = adminSalesLocation.NamaOutlet,
                    KodeOutlet = adminSalesLocation.KodeOutlet,
                    IDLokasi = adminSalesLocation.IDLokasi,
                    TipeLokasi = adminSalesLocation.TipeLokasi,
                    BrandOutlet = adminSalesLocation.BrandOutlet,
                    IDBrand = adminSalesLocation.IDBrand
                };

                dataLocation.Add(location);
            }

            foreach (IDataLocation location in dataLocation)
            {
                List<IDataSalesWithLocation> dataSalesLocationAll = CheckSales(0, location.KodeOutlet);

                foreach (IDataSalesWithLocation dataSalesWithLocation in dataSalesLocationAll)
                {
                    dataSales.Add(dataSalesWithLocation);
                }
            }



            return new ISalesLocationResponse
            {
                sales = dataSales,
                location = dataLocation
            };
        }

        foreach (IDataSalesWithLocation salesLocation in dataSalesLocation)
        {
            IDataLocation location = new IDataLocation
            {
                NamaOutlet = salesLocation.NamaOutlet,
                KodeOutlet = salesLocation.KodeOutlet,
                IDLokasi = salesLocation.IDLokasi,
                TipeLokasi = salesLocation.TipeLokasi,
                BrandOutlet = salesLocation.BrandOutlet,
                IDBrand = salesLocation.IDBrand
            };

            dataSales.Add(salesLocation);
            dataLocation.Add(location);
        }

        return new ISalesLocationResponse
        {
            sales = dataSales,
            location = dataLocation
        };
    }
    public static List<IDataLocation> Location()
    {
        DataTable dt = Db.Rs("SELECT ID, Nama, Kode, 1 [TipeLokasi], 'F' [Brand], IDBrand FROM LocOutlet WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB);
        DataTable dt2 = Db.Rs("SELECT ID, Nama, Kode, 2 [TipeLokasi], 'F' [Brand], IDBrand FROM LocExhibition WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB);

        List<IDataLocation> locations = new List<IDataLocation>();
        foreach (DataRow dr in dt.Rows)
        {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            location.BrandOutlet = dr["Brand"].ToString();
            location.IDBrand = (int)dr["IDBrand"];
            locations.Add(location);
        }

        foreach (DataRow dr in dt2.Rows)
        {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            location.BrandOutlet = dr["Brand"].ToString();
            location.IDBrand = (int)dr["IDBrand"];
            locations.Add(location);
        }

        return locations;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<ILocRepair> GetLocationRepair()
    {
        DataTable dt = Db.Rs("SELECT * FROM LocReparasi WHERE ID = 2", Db.Cnn_DB);

        List<ILocRepair> locRepair = new List<ILocRepair>();
        foreach (DataRow dr in dt.Rows)
        {
            ILocRepair loc = new ILocRepair
            {
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
    public static List<IDataLocation> GetLocationTujuan(string kodeStore)
    {
        DataTable dt = Db.Rs("exec sp_mappingMasterServicePoint " + kodeStore + ", 1", Db.Cnn_DB);

        List<IDataLocation> locations = new List<IDataLocation>();
        foreach (DataRow dr in dt.Rows)
        {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            location.BrandOutlet = dr["Brand"].ToString();
            location.IDBrand = (int)dr["IDBrand"];
            location.Province = dr["Province"].ToString();
            locations.Add(location);
        }

        return locations;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IDeliveryOrderRepairReport> GetSendDeliveryOrderRepair(int idbrandasal, int idasal, int tipeasal, int idbrandtujuan, int idtujuan, int tipetujuan)
    {
        DataSet ds = ExecuteGetSendDeliveryOrderRepair(idbrandasal, idasal, tipeasal, idbrandtujuan, idtujuan, tipetujuan);
        DataTable dt = ds.Tables[0];

        List<IDeliveryOrderRepairReport> doRepairReport = new List<IDeliveryOrderRepairReport>();
        foreach (DataRow dr in dt.Rows)
        {
            doRepairReport.Add(new IDeliveryOrderRepairReport
            {
                IDDocRepair = (int)dr["ID"],
                Image = RepairLib.GetPathImage(dr["Image"].ToString()),
                NamaCustomer = dr["NamaCustomer"].ToString(),
                NamaSales = dr["NamaSales"].ToString(),
                NomorReparasi = dr["NomorReparasi"].ToString(),
                PLU = dr["PLU"].ToString()
            });
        }
        return doRepairReport;
    }

    public static DataSet ExecutePostSendDeliveryOrderRepair(ISendDeliveryOrderParams sendDeliveryOrderParams, int IDBrandAsal)
    {
        string dbName = IDBrandAsal == 7 ? "frank_db" : IDBrandAsal == 5 ? "palace_db_real" : "mondial_db_real";
        string connectionString = "Data Source=dbprodjaws.intra.cmk.co.id;Initial Catalog=" + dbName + ";Persist Security Info=True;User ID=ITJAWS;Password=9HPZhTUMNAgipum;Connection Timeout=1290;max pool size=500;MultipleActiveResultSets=True;";        

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand("sp_insertSendDeliveryOrderRepair", connection))
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(sendDeliveryOrderParams.DO_Detail);
                DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDBrandAsal", SqlDbType.Int) { Value = sendDeliveryOrderParams.IDBrandAsal });
                command.Parameters.Add(new SqlParameter("@IDAsal", SqlDbType.Int) { Value = sendDeliveryOrderParams.IDAsal });
                command.Parameters.Add(new SqlParameter("@TipeAsal", SqlDbType.Int) { Value = sendDeliveryOrderParams.TipeAsal });
                command.Parameters.Add(new SqlParameter("@NamaAsal", SqlDbType.VarChar) { Value = sendDeliveryOrderParams.NamaAsal });
                command.Parameters.Add(new SqlParameter("@IDBrandTujuan", SqlDbType.Int) { Value = sendDeliveryOrderParams.IDBrandTujuan });
                command.Parameters.Add(new SqlParameter("@IDTujuan", SqlDbType.Int) { Value = sendDeliveryOrderParams.IDTujuan });
                command.Parameters.Add(new SqlParameter("@TipeTujuan", SqlDbType.Int) { Value = sendDeliveryOrderParams.TipeTujuan });
                command.Parameters.Add(new SqlParameter("@NamaTujuan", SqlDbType.VarChar) { Value = sendDeliveryOrderParams.NamaTujuan });
                command.Parameters.Add(new SqlParameter("@IDLokasiReparasi", SqlDbType.Int) { Value = sendDeliveryOrderParams.IDLokasiReparasi });
                command.Parameters.Add(new SqlParameter("@Kurir", SqlDbType.VarChar) { Value = sendDeliveryOrderParams.Kurir });
                command.Parameters.Add(new SqlParameter("@Keterangan", SqlDbType.VarChar) { Value = sendDeliveryOrderParams.Keterangan });
                command.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar) { Value = App.UserNama });
                command.Parameters.Add(new SqlParameter("@DO_Detail", SqlDbType.Structured) { Value = pDt });
                command.Parameters.Add(new SqlParameter("@IsCrossBrand", SqlDbType.Int) { Value = sendDeliveryOrderParams.IsCrossBrand });
                command.Parameters.Add(new SqlParameter("@IsReturn", SqlDbType.Int) { Value = 1 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }


    [WebMethod]
    public static string PostSendDeliveryOrderRepair(ISendDeliveryOrderParams args, int IDBrandAsal)
    {
        DataSet ds = ExecutePostSendDeliveryOrderRepair(args, IDBrandAsal);
        return "Success";
    }

    public static DataSet ExecuteGetSendDeliveryOrderRepair(int idbrandasal, int idlokasiasal, int tipeasal, int idbrandtujuan, int idtujuan, int tipetujuan)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getSendReturnOrderRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDBrandAsal", SqlDbType.Int) { Value = idbrandasal });
                command.Parameters.Add(new SqlParameter("@IDAsal", SqlDbType.Int) { Value = idlokasiasal });
                command.Parameters.Add(new SqlParameter("@TipeAsal", SqlDbType.Int) { Value = tipeasal });
                command.Parameters.Add(new SqlParameter("@IDBrandTujuan", SqlDbType.Int) { Value = idbrandtujuan });
                command.Parameters.Add(new SqlParameter("@IDTujuan", SqlDbType.Int) { Value = idtujuan });
                command.Parameters.Add(new SqlParameter("@TipeTujuan", SqlDbType.Int) { Value = tipetujuan });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }

    public class ISendDeliveryOrderParams
    {
        public int IDBrandAsal { get; set; }
        public int IDAsal { get; set; }
        public int TipeAsal { get; set; }
        public string NamaAsal { get; set; }
        public int IDBrandTujuan { get; set; }
        public int IDTujuan { get; set; }
        public int TipeTujuan { get; set; }
        public string NamaTujuan { get; set; }
        public int IDLokasiReparasi { get; set; }
        public string Kurir { get; set; }
        public string Keterangan { get; set; }
        public string Operator { get; set; }
        public int IsCrossBrand { get; set; }
        public List<object> DO_Detail { get; set; }
    }

    public class IDeliveryOrderRepairReport
    {
        public int IDDocRepair { get; set; }
        public string NomorReparasi { get; set; }
        public string NamaCustomer { get; set; }
        public string NamaSales { get; set; }
        public string PLU { get; set; }
        public string Image { get; set; }
    }
    public class ILocRepair
    {
        public int ID { get; set; }
        public string Nama { get; set; }
        public string Kode { get; set; }
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
        public string Province { get; set; }
    }
    public class ISalesLocationResponse
    {
        public List<IDataSalesWithLocation> sales { get; set; }
        public List<IDataLocation> location { get; set; }
    }
}