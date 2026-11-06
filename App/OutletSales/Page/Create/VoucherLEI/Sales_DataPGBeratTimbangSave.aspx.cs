using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_Sales_Sales_DataPGBeratTimbangSave : System.Web.UI.Page
{
    protected int myID { get { return App.QueryInt(this, "id"); } }
    protected string myIDSoPg { get { return Request.QueryString["idsopg"]; } }
    protected string WeightPG { get { return Request.QueryString["weightpg"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);
        
        Db.Execute("UPDATE SalesOrderPG SET BeratTimbang = '"+ WeightPG.Replace(",",".") +"' WHERE ID = "+ myIDSoPg + " ", Db.Cnn_DB);
        Response.Redirect("Sales_Data.aspx?refreshPage=1&id="+myID+"");
    }
}