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
    public static List<ILedger> GetLedger(string Keywords)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(@"SELECT a.*, b.IsCancelRepair
                                                        FROM StockLedgerReparasi a 
                                                        JOIN DocRepair_Pivot B ON B.IDDocRepair = a.IDDocRepair
                                                        WHERE a.NomorRepair = TRIM(@Keywords) OR a.PLU = TRIM(@Keywords)
                                                        ORDER BY A.CreatedAt desc
                                                        ", connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@Keywords", SqlDbType.VarChar) { Value = Keywords });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                return GetDataFromDataTable<ILedger>(dt);
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

    public class ILedger
    {
        public string NomorRepair { get; set; }
        public string PLU { get; set; }
        public string Keterangan { get; set; }
        public int Status { get; set; }
        public string CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string Lokasi { get; set; }
        public int IsCancelRepair { get; set; }// 0: No, 1: Cancel
        
    }

    public class IMessage
    {
        public string Message { get; set; }
    }
}