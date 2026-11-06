using System;
using System.IO;

public partial class Pdf : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e)
    {
        string fileName = Request.QueryString["path"];

        if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
        {
            Response.ContentType = "application/pdf";
            Response.TransmitFile(fileName);
            Response.End();
        }
        else
        {
            Response.Write(fileName);
        }
    }
}

