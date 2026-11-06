using System;
using System.Collections.Generic;
using System.Data;
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

        DataTable dt = new DataTable();
        dt = Db.Rs("SELECT TOP 1 ISNULL(NomorSO,'') [NomorSO], Nominal, NomorEDC, TanggalUpload FROM ListUploadEDC WHERE ID = " + id, Db.Cnn_DB);
        string message = "";
        
        if (dt.Rows.Count > 0)
        {
            string brand = "FC";
            string nosodp = dt.Rows[0]["NomorSO"].ToString();
            string nominal = Convert.ToDecimal(dt.Rows[0]["Nominal"]).ToString("N2", nfi).Replace(",", "");
            string tglcair = Convert.ToDateTime(dt.Rows[0]["TanggalUpload"]).ToString("yyyy-MM-dd hh:mm tt");
            string appcode = dt.Rows[0]["NomorEDC"].ToString();

            Db.Execute(" EXEC dbfinance_new.dbo.sp_posting_approval_code '"+brand+"','"+nosodp+"','"+appcode+"', "+nominal+", '"+tglcair+"' ", Db.Cnn_DB);
            message = "Proses Posting Berhasil!!!";
           //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + nosodp + "#" + nominal + "#" + tglcair + "#" + appcode + "#" + brand + "');", true);
        }
        else
        {
            message = "Data Tidak Ditemukan.";
        }
        Response.Redirect("Report_EDC.aspx?refreshpage=1&msgpost="+message+"");
    }
}