using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_Bank_Report_EDC_Update : System.Web.UI.Page
{
    protected string id { get { return Request.QueryString["id"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        saveUpdate();
    }
    protected void saveUpdate()
    {
        string message = "";
        try
        {
            string query = "DELETE FROM ListUploadEDC WHERE ID =" + id;
            Db.Execute(query, Db.Cnn_DB);
            message = "Proses Delete Berhasil!!!";
        }
        catch (Exception ex)
        {
            message = ex.Message.ToString();
        }
        Response.Redirect("Report_EDC.aspx?refreshpage=1&msgupload=" + message + "");
    }
}