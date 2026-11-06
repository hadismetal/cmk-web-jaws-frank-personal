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
using System.Web;
using System.IO;
using System.Reflection;


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
    public static string AddLocationServicePoint(string Kode, string LokasiOld, string LokasiNew)
    {
        string Message = "";
        var oldData = LokasiOld.Split(',').Select(x => x.Trim()).ToList();
        var newData = LokasiNew.Split(',').Select(x => x.Trim()).ToList();

        // Find added items
        var added = newData.Except(oldData).ToList();
        var removed = oldData.Except(newData).ToList();

        string addedLocations = string.Join(",", added);            
        string removedLocations = string.Join(",", removed);

        string Query = @"

            CREATE TABLE #TempAddedLocations (Kode VARCHAR(200));
            CREATE TABLE #TempRemovedLocations (Kode VARCHAR(200));

            declare @IDServicePoint int, @IDProvince int
            SELECT @IDServicePoint = ID, @IDProvince = IDRepairMasterProvince FROM RepairMasterLocationProvince WHERE Kode = @Kode
            
            INSERT INTO #TempAddedLocations (Kode)
            SELECT value FROM STRING_SPLIT(@AddedLocations, ',')

            INSERT INTO #TempRemovedLocations (Kode)
            SELECT value FROM STRING_SPLIT(@RemovedLocations, ',')

            IF EXISTS (SELECT 1 FROM #TempAddedLocations)
            BEGIN
                INSERT INTO RepairMasterLocationProvinceChild (Kode, IDRepairMasterProvince, IDRepairMasterLocationProvince)
                SELECT Kode, @IDServicePoint, @IDProvince FROM #TempAddedLocations 
            END

            IF EXISTS (SELECT 1 FROM #TempRemovedLocations)
            BEGIN
                DELETE A 
                FROM 
                (SELECT * FROM RepairMasterLocationProvinceChild 
                WHERE Kode in (SELECT Kode FROM #TempRemovedLocations) 
                AND IDRepairMasterProvince = @IDProvince 
                AND IDRepairMasterLocationProvince = @IDServicePoint)A
            END


            DROP TABLE #TempAddedLocations, #TempRemovedLocations
        ";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            try
            {                            
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@Kode", SqlDbType.VarChar) { Value = Kode });
                    command.Parameters.Add(new SqlParameter("@AddedLocations", SqlDbType.VarChar) { Value = addedLocations });
                    command.Parameters.Add(new SqlParameter("@RemovedLocations", SqlDbType.VarChar) { Value = removedLocations });

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);                
                }
                
            }
            catch (Exception ex)
            {                
                Message = ex.Message;
            }
        }
        return Message;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IListServicePoint> GetMasterServicePoint()
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getBrandListServicePoint", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Command", SqlDbType.Int) { Value = 0 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];

                List<IListServicePoint> listServicePoint = new List<IListServicePoint>();

                // Loop through each DataRow in the DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    // Create a new IMapDataLocation object
                    IListServicePoint location = new IListServicePoint();

                    // Use reflection to set the properties dynamically
                    foreach (PropertyInfo prop in typeof(IListServicePoint).GetProperties())
                    {
                        if (dt.Columns.Contains(prop.Name) && dr[prop.Name] != DBNull.Value)
                        {
                            prop.SetValue(location, Convert.ChangeType(dr[prop.Name], prop.PropertyType), null);
                        }
                    }

                    // Add the location object to the list
                    listServicePoint.Add(location);
                }

                return listServicePoint;
            }
        }
    }

    [WebMethod]
    public static string CreateServicePoint(ICreateServicePoint parameters)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getBrandListServicePoint", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Command", SqlDbType.Int) { Value = 3 });
                command.Parameters.Add(new SqlParameter("@Kode", SqlDbType.VarChar) { Value = parameters.Kode });
                command.Parameters.Add(new SqlParameter("@IDProvince", SqlDbType.Int) { Value = parameters.IDProvince });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];


                string Msg = "";

                foreach (DataRow dr in dt.Rows)
                {
                    Msg = dr["Message"].ToString();
                }

                return Msg;
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetLocation()
    {
        DataTable dt = Db.Rs(@"SELECT ID, Nama, Kode, 1 [TipeLokasi] FROM LocOutlet WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB);
        DataTable dt2 = Db.Rs("SELECT ID, Nama, Kode, 2 [TipeLokasi] FROM LocExhibition WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB);
        DataTable dt3 = Db.Rs("SELECT * FROM RepairMasterProvince", Db.Cnn_DB);
        

        List<ILocation> locations = new List<ILocation>();

        foreach (DataRow dr in dt.Rows)
        {
            ILocation location = new ILocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            locations.Add(location);
        }

        foreach (DataRow dr in dt2.Rows)
        {
            ILocation location = new ILocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            locations.Add(location);
        }
        

        List<IProvince> provinces = new List<IProvince>();
        foreach (DataRow dr in dt3.Rows)
        {
            provinces.Add(new IProvince
            {
                ID = (int)dr["ID"],
                Province = dr["Province"].ToString()
            });

        }

        


        return new
        {
            locations = locations,
            provinces = provinces
        };
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static IListServicePointDetail GetMasterServicePointDetail(string Kode, int IDProvince)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getBrandListServicePoint", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Command", SqlDbType.Int) { Value = 1 });
                command.Parameters.Add(new SqlParameter("@Kode", SqlDbType.VarChar) { Value = Kode });
                command.Parameters.Add(new SqlParameter("@IDProvince", SqlDbType.Int) { Value = IDProvince });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];

                List<IListServicePoint> listServicePoint = new List<IListServicePoint>();

                // Loop through each DataRow in the DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    // Create a new IMapDataLocation object
                    IListServicePoint location = new IListServicePoint();

                    // Use reflection to set the properties dynamically
                    foreach (PropertyInfo prop in typeof(IListServicePoint).GetProperties())
                    {
                        if (dt.Columns.Contains(prop.Name) && dr[prop.Name] != DBNull.Value)
                        {
                            prop.SetValue(location, Convert.ChangeType(dr[prop.Name], prop.PropertyType), null);
                        }
                    }

                    // Add the location object to the list
                    listServicePoint.Add(location);
                }

                DataTable dt2 = ds.Tables[1];

                List<IListServicePoint> listServicePointDetail = new List<IListServicePoint>();

                // Loop through each DataRow in the DataTable
                foreach (DataRow dr in dt2.Rows)
                {
                    // Create a new IMapDataLocation object
                    IListServicePoint location = new IListServicePoint();

                    // Use reflection to set the properties dynamically
                    foreach (PropertyInfo prop in typeof(IListServicePoint).GetProperties())
                    {
                        if (dt2.Columns.Contains(prop.Name) && dr[prop.Name] != DBNull.Value)
                        {
                            prop.SetValue(location, Convert.ChangeType(dr[prop.Name], prop.PropertyType), null);
                        }
                    }

                    // Add the location object to the list
                    listServicePointDetail.Add(location);
                }

                DataTable dt4 = Db.Rs(@"
                SELECT ID, Nama, Kode, 1 [TipeLokasi], IDBrand FROM frank_db.dbo.LocOutlet WHERE Disable = 0 
                UNION
                SELECT ID, Nama, Kode, 1 [TipeLokasi], IDBrand FROM palace_db_real.dbo.LocOutlet WHERE Disable = 0 
                UNION
                SELECT ID, Nama, Kode, 1 [TipeLokasi], IDBrand FROM mondial_db_real.dbo.LocOutlet WHERE Disable = 0 
                ", Db.Cnn_DB);
                DataTable dt5 = Db.Rs(@"
                SELECT ID, Nama, Kode, 2 [TipeLokasi], IDBrand FROM frank_db.dbo.LocExhibition WHERE Disable = 0 
                UNION
                SELECT ID, Nama, Kode, 2 [TipeLokasi], IDBrand FROM palace_db_real.dbo.LocExhibition WHERE Disable = 0 
                UNION
                SELECT ID, Nama, Kode, 2 [TipeLokasi], IDBrand FROM mondial_db_real.dbo.LocExhibition WHERE Disable = 0         
                ", Db.Cnn_DB);

                List<ILocation1> location1 = new List<ILocation1>();


                foreach (DataRow dr in dt4.Rows)
                {
                    ILocation1 location = new ILocation1();
                    location.IDLokasi = (int)dr["ID"];
                    location.TipeLokasi = (int)dr["TipeLokasi"];
                    location.NamaOutlet = dr["Nama"].ToString();
                    location.KodeOutlet = dr["Kode"].ToString();
                    location.IDBrand = (int)dr["IDBrand"];
                    location1.Add(location);
                }

                foreach (DataRow dr in dt5.Rows)
                {
                    ILocation1 location = new ILocation1();
                    location.IDLokasi = (int)dr["ID"];
                    location.TipeLokasi = (int)dr["TipeLokasi"];
                    location.NamaOutlet = dr["Nama"].ToString();
                    location.KodeOutlet = dr["Kode"].ToString();
                    location.IDBrand = (int)dr["IDBrand"];
                    location1.Add(location);
                }

                return new IListServicePointDetail
                {
                    listServicePoint = listServicePoint,
                    listServicePointDetail = listServicePointDetail,
                    allOutlet = location1
                };
            }
        }
    }

    public class ICreateServicePoint
    {
        public string Kode { get; set; }
        public int IDProvince { get; set; }
    }
    public class IAddLocationServicePoint
    {
        public string Kode { get; set; }
        public string location { get; set; }
    }

    public class IListServicePoint
    {
        public int ID { get; set; }
        public int TipeLokasi { get; set; }
        public string Kode { get; set; }
        public string Nama { get; set; }
        public int IDProvince { get; set; }
        public string Province { get; set; }
        public int IDBrand { get; set; }
        public string Brand { get; set; }
    }

    public class IListServicePointDetail
    {
        public List<IListServicePoint> listServicePoint { get; set; }
        public List<IListServicePoint> listServicePointDetail { get; set; }
        public List<ILocation1> allOutlet { get; set; }
    }

    public class ILocation
    {
        public string NamaOutlet { get; set; }
        public string KodeOutlet { get; set; }
        public int IDLokasi { get; set; }
        public int TipeLokasi { get; set; }
    }
    public class ILocation1
    {
        public string NamaOutlet { get; set; }
        public string KodeOutlet { get; set; }
        public int IDLokasi { get; set; }
        public int TipeLokasi { get; set; }
        public int IDBrand { get; set; }
    }

    public class IProvince
    {
        public int ID { get; set; }
        public string Province { get; set; }
    }
}