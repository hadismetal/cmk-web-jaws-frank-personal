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
        App.ProtectRole(Role.Manager);
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IListMasterPayment> GetPaymentListMaster()
    {
        DataTable dt = Db.Rs("SELECT ID, Nama FROM PaymentType", Db.Cnn_DB);

        List<IListMasterPayment> payments = new List<IListMasterPayment>();
        foreach (DataRow dr in dt.Rows)
        {
            payments.Add(new IListMasterPayment
            {
                ID = (int)dr["ID"],
                Nama = dr["Nama"].ToString()
            });
        }

        return payments;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IListPayment> GetMasterPaymentList()
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_masterPaymentRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Command", SqlDbType.Int) { Value = 0 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];

                List<IListPayment> listPayment = new List<IListPayment>();

                // Loop through each DataRow in the DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    // Create a new IMapDataLocation object
                    IListPayment location = new IListPayment();

                    // Use reflection to set the properties dynamically
                    foreach (PropertyInfo prop in typeof(IListPayment).GetProperties())
                    {
                        if (dt.Columns.Contains(prop.Name) && dr[prop.Name] != DBNull.Value)
                        {
                            prop.SetValue(location, Convert.ChangeType(dr[prop.Name], prop.PropertyType), null);
                        }
                    }

                    // Add the location object to the list
                    listPayment.Add(location);
                }

                return listPayment;
            }
        }
    }

    [WebMethod]
    public static string CreateMasterPayment(ICreateMasterPayment parameters)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getBrandListServicePoint", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Command", SqlDbType.Int) { Value = 1 });
                command.Parameters.Add(new SqlParameter("@IDPaymentType", SqlDbType.Int) { Value = parameters.IDPaymentTypes });
                command.Parameters.Add(new SqlParameter("@Disabled", SqlDbType.Int) { Value = parameters.IsDisabled });
                command.Parameters.Add(new SqlParameter("@Minimum", SqlDbType.Decimal) { Value = parameters.MinimumPayment });
                command.Parameters.Add(new SqlParameter("@Maximum", SqlDbType.Decimal) { Value = parameters.MaximumPayment });
                command.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar) { Value = App.UserNama });

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
    public static string UpdateConfigurationPayment(IUpdateConfigurationPayment parameters)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_masterPaymentRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Command", SqlDbType.Int) { Value = 2 });
                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = parameters.ID });
                command.Parameters.Add(new SqlParameter("@Disabled", SqlDbType.Int) { Value = parameters.Disabled });
                command.Parameters.Add(new SqlParameter("@Minimum", SqlDbType.Decimal) { Value = parameters.Minimum });
                command.Parameters.Add(new SqlParameter("@Maximum", SqlDbType.Decimal) { Value = parameters.Maximum });
                command.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar) { Value = App.UserNama });

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


    public class ICreateMasterPayment
    {

        public int IDPaymentTypes { get; set; }
        public decimal MinimumPayment { get; set; }
        public decimal MaximumPayment { get; set; }
        public int IsDisabled { get; set; }
        public string Nama { get; set; }
    }

    public class IUpdateConfigurationPayment
    {
        public int ID { get; set; }
        public decimal Minimum { get; set; }
        public decimal Maximum { get; set; }
        public int Disabled { get; set; }
    }

    public class IListMasterPayment
    {
        public int ID { get; set; }
        public string Nama { get; set; }
    }

    public class IListPayment
    {
        public int ID { get; set; }
        public int IDPaymentTypes { get; set; }
        public decimal MinimumPayment { get; set; }
        public decimal MaximumPayment { get; set; }
        public int IsDisabled { get; set; }
        public string Nama { get; set; }
    }

}