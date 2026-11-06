using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_Packaging_Sales_DataSOSave : System.Web.UI.Page
{
    protected int myID { get { return App.QueryInt(this, "id"); } }
	protected int iddoc { get { return Convert.ToInt32(Request.QueryString["iddoc"]); } }
    protected string noso { get { return Request.QueryString["noso"]; } }
    protected string nodp { get { return Request.QueryString["nodp"]; } }
    protected string norep { get { return Request.QueryString["norep"]; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        save();
    }

    protected void save()
    {
        if (!string.IsNullOrEmpty(noso))
        {
            Db.Execute("UPDATE SalesOrder SET LinkPackagingSalesOrder = '" + noso.Replace('+','/')+ "', LinkPackagingRepair = NULL, LinkPackagingDownPayment = NULL WHERE ID = " + myID , Db.Cnn_DB);
        }
        else if (!string.IsNullOrEmpty(nodp))
        {
            Db.Execute("UPDATE SalesOrder SET LinkPackagingDownPayment = '" + nodp.Replace('+','/') + "', LinkPackagingRepair = NULL, LinkPackagingSalesOrder = NULL WHERE ID = " + myID, Db.Cnn_DB);
        }
        else if (!string.IsNullOrEmpty(norep))
        {
            Db.Execute("UPDATE SalesOrder SET LinkPackagingRepair = '" + norep.Replace('+','/') + "', LinkPackagingSalesOrder = NULL, LinkPackagingDownPayment = NULL WHERE ID = " + myID, Db.Cnn_DB);
        }

        Response.Redirect("Sales_Form.aspx?id=" + myID + "&status=1");
    }
}