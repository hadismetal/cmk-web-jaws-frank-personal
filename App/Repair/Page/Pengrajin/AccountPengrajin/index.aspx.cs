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
        // App.Protect();
        // App.ProtectRole(Role.Operator);

        // if (!IsPostBack)
        // {
        // }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<ILocation> GetLocation()
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string query = @"
                DECLARE @TipeUser int = 9
                IF EXISTS(SELECT 1 FROM DataAdminSales a JOIN frank_ac.dbo.UserAccount b on b.UserNumber=LTRIM(RTRIM(a.Keterangan)) AND b.ID = " + App.UserID + @")
                BEGIN                    
                    set @TipeUser = 1
                END

                select b.* 
                FROM RepairMasterLocationProvince a
                JOIN LocOutlet b on b.Kode=a.Kode
                WHERE (@TipeUser = 9 OR 
                B.ID IN (SELECT IDLokasi FROM DataAdminSales a JOIN frank_ac.dbo.UserAccount b on b.UserNumber=LTRIM(RTRIM(a.Keterangan)) AND b.ID = " + App.UserID + @"))                 
                ";
            using (SqlCommand command = new SqlCommand(query, connection))
            {            
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                return GetDataFromDataTable<ILocation>(dt);
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IKaryawan> GetKaryawan(string nik)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_CS))
        {
            using (SqlCommand command = new SqlCommand("sp_getdatakaryawan", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@nik", SqlDbType.Int) { Value = nik });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                return GetDataFromDataTable<IKaryawan>(dt);
            }
        }
    }


    [WebMethod]
    public static IMessage InsertDataKaryawan(IArgsInsertKaryawan arguments)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_insertPengrajinRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@NIK", SqlDbType.VarChar) { Value = arguments.NIK });
                command.Parameters.Add(new SqlParameter("@EMAIL", SqlDbType.VarChar) { Value = arguments.Email });
                command.Parameters.Add(new SqlParameter("@JABATAN", SqlDbType.VarChar) { Value = arguments.Jabatan });
                command.Parameters.Add(new SqlParameter("@NAMA", SqlDbType.VarChar) { Value = arguments.Nama });
                command.Parameters.Add(new SqlParameter("@KODESTORE", SqlDbType.VarChar) { Value = arguments.KodeStore });
                command.Parameters.Add(new SqlParameter("@PASS", SqlDbType.VarChar) { Value = Common.Encrypt("123") });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                return GetDataFromDataTable<IMessage>(dt)[0];
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

    public class IArgsInsertKaryawan
    {
        public string UserNumber { get; set; }
        public string Nama { get; set; }
        public string Jabatan { get; set; }
        public string Email { get; set; }
        public string NIK { get; set; }
        public string KodeStore { get; set; }
    }
    public class ILocation
    {
        public int ID { get; set; }
        public string Nama { get; set; }
        public int TipeLokasi { get; set; }
        public string Kode { get; set; }
    }

    public class IKaryawan
    {
        public string m_nama { get; set; }
        public string m_nik { get; set; }
        public string m_emailkantor { get; set; }
        public string m_jabatan { get; set; }
    }

    public class IMessage
    {
        public string Message { get; set; }
    }
}