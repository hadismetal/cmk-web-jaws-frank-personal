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
using System.IO;
using System.Reflection;



public partial class Page_Summary : System.Web.UI.Page
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

        locations.Add(new IDataLocation
        {
            IDLokasi = 0,
            TipeLokasi = 9,
            NamaOutlet = "-- Semua Lokasi --",
            KodeOutlet = "ALL",
            BrandOutlet = "F",
            IDBrand = 7,
        });
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
    public static List<ISummary> GetSummary(int TipeLokasi, int IDLokasi)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getSummaryDeliveryOrderRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = 9 });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = 0 });
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = App.UserID });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                return GetDataFromDataTable<ISummary>(dt);
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<DataJR> GetDataOnDuty()
    {
        string query = @"
            DECLARE @IsAm int = 0, @IDLokasi int = 0, @TipeLokasi int = 0 ;
            select @IsAm = count(*) FROM DataAreaManager a JOIN DataAreaManager_Lokasi b on b.IDForm=a.ID where A.IDUser = @IDUser
            select @IDLokasi = IDLokasi FROM DataAdminSales where IDUser = @IDUser

            select * into #tempLokasi FROM(
                select ID AS IDLokasi, 1 AS TipeLokasi, Nama AS NamaLokasi FROM LocOutlet
                UNION
                select ID AS IDLokasi, 2 AS TipeLokasi, Nama AS NamaLokasi FROM LocExhibition
            )a

            SELECT d.Nama, IIF(a.TipeLokasi = 1, 'Outlet', 'Exhibition') TipeLokasi, b.NamaLokasi, c.Nama as NamaShift, FORMAT(a.EfektifDate, 'dddd, dd MMMM yyyy', 'id-ID') as Periode
            from MasterJabatanPenggantiOpname a
            JOIN #tempLokasi b on b.IDLokasi=a.IDLokasi and a.TipeLokasi=b.TipeLokasi
            JOIN MasterShiftOpname c on c.ID=a.TipeShift
            JOIN frank_ac.dbo.UserAccount d on d.ID=a.IDUser
            WHERE EfektifDate >= CAST(GETDATE() AS DATE)
            AND (@IsAm = 0 or a.IDLokasi in (select IDLokasi FROM DataAreaManager_Lokasi a JOIN DataAreaManager b on b.ID=a.IDForm where IDUser = @IDUser))
            AND (@IsAm = 0 or a.TipeLokasi in (select TipeLokasi FROM DataAreaManager_Lokasi a JOIN DataAreaManager b on b.ID=a.IDForm where IDUser = @IDUser))
            AND (@IDLokasi = 0 OR a.IDLokasi = @IDLokasi) AND (@TipeLokasi = 0 or a.TipeLokasi = @TipeLokasi)
            ORDER BY TipeShift, EfektifDate asc

            drop table #tempLokasi
        ";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = App.UserID });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                return GetDataFromDataTable<DataJR>(dt);
            }
        }
        
    }

    public static List<T> GetDataFromDataTable<T>(DataTable dataTable) where T : class, new()
    {
        List<T> listOfData = new List<T>();

        // Loop through each DataRow in the DataTable
        foreach (DataRow dr in dataTable.Rows)
        {
            // Create a new IMapDataLocation object
            T data = new T();

            // Use reflection to set the properties dynamically
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                if (dataTable.Columns.Contains(prop.Name) && dr[prop.Name] != DBNull.Value)
                {
                    prop.SetValue(data, Convert.ChangeType(dr[prop.Name], prop.PropertyType), null);
                }
            }

            // Add the location object to the list
            listOfData.Add(data);
        }

        return listOfData;
    }

    public class ISummary
    {
        public string StatusName { get; set; }
        public int Jumlah { get; set; }
        public string Url { get; set; }
    }
    public class DataJR
    {
        public string Nama {get;set;}
        public string TipeLokasi {get;set;}    
        public string NamaLokasi {get;set;}
        public string NamaShift {get;set;}
        public string Periode {get;set;}
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
}