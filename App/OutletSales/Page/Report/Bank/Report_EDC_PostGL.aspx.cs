using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_Bank_Report_EDC_PostGL : System.Web.UI.Page
{
    protected string id { get { return Request.QueryString["id"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        PostToGL();
    }
    protected void PostToGL()
    {
        System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
        nfi.NumberDecimalDigits = 2;

        string query = "SELECT TOP 1 ISNULL(NomorSO,'') [NomorSO], Nominal, NomorEDC, TanggalCair FROM ListUploadEDC WHERE ID = @ID";
        DataTable dt = new DataTable();
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@ID", SqlDbType.VarChar) { Value = id }
        };
        dt = Rs(query, Db.Cnn_DB, parameters, false);
        string message = "";

        if (dt.Rows.Count > 0)
        {
            string brand = "FC";
            string nosodp = dt.Rows[0]["NomorSO"].ToString();
            string nominal = Convert.ToDecimal(dt.Rows[0]["Nominal"]).ToString("N2", nfi).Replace(",", "");
            string tglcair = Convert.ToDateTime(dt.Rows[0]["TanggalCair"]).ToString("yyyy-MM-dd");
            string appcode = dt.Rows[0]["NomorEDC"].ToString();

            string Cnn_FI = ConfigurationManager.ConnectionStrings["Cnn_FI"].ConnectionString;

            string query2 = "sp_posting_approval_code_mas_tjuy";
            List<SqlParameter> parameters2 = new List<SqlParameter>()
            {
                new SqlParameter("@p_brand", SqlDbType.VarChar) { Value = brand },
                new SqlParameter("@p_nomor", SqlDbType.VarChar) { Value = nosodp },
                new SqlParameter("@p_approvalcode", SqlDbType.VarChar) { Value = appcode },
                new SqlParameter("@p_nominal", SqlDbType.Decimal) { Value = nominal },
                new SqlParameter("@p_tanggal", SqlDbType.DateTime) { Value = tglcair },
                new SqlParameter("@id_list_upload_edc", SqlDbType.Int) { Value = id }
            };
            Execute(query2, Cnn_FI, parameters2, true);
            message = "Proses Posting Berhasil!!!";
            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + nosodp + "#" + nominal + "#" + tglcair + "#" + appcode + "#" + brand + "');", true);
        }
        else
        {
            message = "Data Tidak Ditemukan.";
        }
        Response.Redirect("Report_EDC.aspx?refreshpage=1&msgpost=" + message + "");
    }

    //Data Table Builder
    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, bool isstoredprocedure)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = isstoredprocedure ? CommandType.StoredProcedure : CommandType.Text;
            foreach (var x in param)
            {
                cmd.Parameters.Add(x);
            }
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
            sqlAdapter.SelectCommand.CommandTimeout = 600; // 300 sec or 5 mins for long time query
            DataSet objDS = new DataSet();
            sqlAdapter.Fill(objDS, "data");
            sqlCnn.Close();
            rs = objDS.Tables["data"];
        }
        return rs;
    }

    //Execute Command
    protected void Execute(string strSql, string CnnString, List<SqlParameter> param, bool isstoredprocedure)
    {
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = isstoredprocedure ? CommandType.StoredProcedure : CommandType.Text;
            foreach (var x in param)
            {
                cmd.Parameters.Add(x);
            }
            cmd.CommandTimeout = 600; // 300 sec or 5 mins for long time query
            sqlCnn.Open();
            cmd.ExecuteNonQuery();
            sqlCnn.Close();
        }
    }
}