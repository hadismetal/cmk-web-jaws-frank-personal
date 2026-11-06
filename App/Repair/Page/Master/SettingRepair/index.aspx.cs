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
    [ScriptMethod(UseHttpGet = true)]
    public static object GetMasterSetting()
    {


        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_settingRepairMarkup", connection))
            {
                List<ISetting> settings = new List<ISetting>();
                List<IMarkUp> markups = new List<IMarkUp>();

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Command", SqlDbType.Int) { Value = 0 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];

                // Loop through each DataRow in the DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    // Create a new IMapDataLocation object
                    ISetting setting = new ISetting();

                    // Use reflection to set the properties dynamically
                    foreach (PropertyInfo prop in typeof(ISetting).GetProperties())
                    {
                        if (dt.Columns.Contains(prop.Name) && dr[prop.Name] != DBNull.Value)
                        {
                            prop.SetValue(setting, Convert.ChangeType(dr[prop.Name], prop.PropertyType), null);
                        }
                    }

                    // Add the setting object to the list
                    settings.Add(setting);
                }

                DataTable dt2 = ds.Tables[1];

                // Loop through each DataRow in the DataTable
                foreach (DataRow dr in dt2.Rows)
                {
                    // Create a new IMapDataLocation object
                    IMarkUp markup = new IMarkUp();

                    // Use reflection to set the properties dynamically
                    foreach (PropertyInfo prop in typeof(IMarkUp).GetProperties())
                    {
                        if (dt2.Columns.Contains(prop.Name) && dr[prop.Name] != DBNull.Value)
                        {
                            prop.SetValue(markup, Convert.ChangeType(dr[prop.Name], prop.PropertyType), null);
                        }
                    }

                    // Add the markup object to the list
                    markups.Add(markup);
                }


                return new
                {
                    settings = settings,
                    markups = markups
                };
            }
        }
    }

    [WebMethod]
    public static string UpdateConfigurationSetting(IUpdateConfigurationSetting parameters)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_settingRepairMarkup", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Command", SqlDbType.Int) { Value = 1 });
                command.Parameters.Add(new SqlParameter("@Nilai", SqlDbType.Decimal) { Value = parameters.Nilai });
                command.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.VarChar) { Value = parameters.Tipe });

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
    public static string UpdateConfigurationMarkUp(IUpdateConfigurationMarkUp parameters)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_settingRepairMarkup", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Command", SqlDbType.Int) { Value = 1 });
                command.Parameters.Add(new SqlParameter("@MarkUpR", SqlDbType.Decimal) { Value = parameters.MarkUpR });
                command.Parameters.Add(new SqlParameter("@MarkUpJual", SqlDbType.Decimal) { Value = parameters.MarkUpJual });
                command.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.VarChar) { Value = parameters.Tipe });

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

                return new IListServicePointDetail
                {
                    listServicePoint = listServicePoint,
                    listServicePointDetail = listServicePointDetail
                };
            }
        }
    }

    public class ICreateServicePoint
    {
        public string Kode { get; set; }
        public int IDProvince { get; set; }
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
    }

    public class ISetting
    {
        public int ID { get; set; }
        public string Nama { get; set; }
        public decimal Nilai { get; set; }
        public string Type { get; set; }
    }

    public class IMarkUp
    {
        public int ID { get; set; }
        public decimal MarkUpR { get; set; }
        public decimal MarkUpHargaJual { get; set; }
        public string Efektif { get; set; }
        public string Type { get; set; }
    }

    public class IUpdateConfigurationSetting
    {
        public int Command { get; set; }
        public decimal Nilai { get; set; }
        public string Tipe { get; set; }
    }

    public class IUpdateConfigurationMarkUp
    {
        public int Command { get; set; }
        public decimal MarkUpR { get; set; }
        public decimal MarkUpJual { get; set; }
        public string Tipe { get; set; }
    }
}