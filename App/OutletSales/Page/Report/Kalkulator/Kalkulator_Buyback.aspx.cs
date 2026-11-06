using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_Kalkulator_Kalkulator_Buyback : System.Web.UI.Page
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
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false)]
    public static object Display(RequestQC request)
    {
        string user = string.Empty;
        int userid = 0;
        if (IsSessionExpired())
        {
            return new ResponseQC { success = false, error = "Session expired" };
        }
        else
        {
            user = HttpContext.Current.Session["UserNama"].ToString();
            userid = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        }

        try
        {
            string query = "sp_get_product_kalkulator_buyback_new";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@KW", SqlDbType.VarChar){ Value = request.kw.Trim() },
                new SqlParameter("@SEARCHBY", SqlDbType.VarChar){ Value = request.searchby.Trim() },
                new SqlParameter("@TIPEPRODUCT", SqlDbType.VarChar){ Value = request.tipe_product.Trim() }
            };
            DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
            
            string urlImageAPI = ConfigurationManager.AppSettings["ImageAPI"];

            List<ResponseQCData> datas = new List<ResponseQCData>();
            foreach (DataRow dr in dt.Rows)
            {
                int id_product = Convert.ToInt32(dr["ID_PRODUCT"]);
                int id_qc = Convert.ToInt32(dr["ID_QC"]);
                string plu = dr["PLU"].ToString();
                string no_so = dr["NO_SO"].ToString();
                string tgl_so = Convert.ToDateTime(dr["TGL_SO"]).ToString("dd-MM-yyyy");
                decimal total_bayar = Convert.ToDecimal(dr["TOTAL_BAYAR"]);
                string no_qc = dr["NO_QC"].ToString();
                string image = dr["IMAGE"].ToString();
                if (string.IsNullOrEmpty(image))
                    image = "nopict.jpg";
                string imageurl = urlImageAPI + image;
                string item = dr["ITEM"].ToString();
                string href = "Kalkulator_Buyback_Pricing.aspx?tipe=" + request.tipe_product.Trim() + "&idproduct=" + id_product + "&idqc=" + id_qc;

                ResponseQCData data = new ResponseQCData()
                {
                    id_product = id_product,
                    id_qc = id_qc,
                    plu = plu,
                    no_so = no_so,
                    tgl_so = tgl_so,
                    total_bayar = total_bayar,
                    no_qc = no_qc,
                    image = imageurl,
                    item = item,
                    href = href
                };
                datas.Add(data);
            }
            return new ResponseQC { success = true, error = string.Empty, data = datas };
        }
        catch (Exception ex)
        {
            return new ResponseQC { success = false, error = ex.Message };
        }
    }

    protected static bool IsSessionExpired()
    {
        if (HttpContext.Current.Session == null)
            return true;

        return HttpContext.Current.Session["UserID"] == null &&
               HttpContext.Current.Session["UserNama"] == null;
    }

    protected static DataTable Rs(string query, string connection, List<SqlParameter> parameters, CommandType cmdtype)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(connection))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = cmdtype;
                if (parameters != null)
                {
                    foreach (SqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
        }
        return dt;
    }

    protected static DataSet Ds(string query, string connection, List<SqlParameter> parameters, CommandType cmdtype)
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection(connection))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = cmdtype;
                if (parameters != null)
                {
                    foreach (SqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(ds); // Fill the DataSet
                }
            }
        }
        return ds;
    }

    public class RequestQC
    {
        public string kw;
        public string searchby;
        public string tipe_product;
    }

    public class ResponseQC
    {
        public bool success { get; set; }
        public string error { get; set; }
        public List<ResponseQCData> data { get; set; }
    }

    public class ResponseQCData
    {
        public int id_product { get; set; }
        public int id_qc { get; set; }
        public string plu { get; set; }
        public string no_so { get; set; }
        public string tgl_so { get; set; }
        public decimal total_bayar { get; set; }
        public string no_qc { get; set; }
        public string image { get; set; }
        public string item { get; set; }
        public string href { get; set; }
    }
}