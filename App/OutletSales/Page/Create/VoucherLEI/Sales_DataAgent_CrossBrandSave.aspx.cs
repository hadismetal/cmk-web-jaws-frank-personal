using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_Sales_Sales_DataAgent_CrossBrandSave : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }
    protected string myKodeBrandCross { get { return Request.QueryString["kdbc"]; } }
    protected int myTipeLokasiCross { get { return App.QueryInt(this, "tlc"); } }
    protected int myIDLokasiCross { get { return App.QueryInt(this, "idlc"); } }
    protected int myIDSalesCross { get { return App.QueryInt(this, "idsc"); } }
    protected string myNamaSalesCross { get { return Request.QueryString["nsc"]; } }
	protected string myNIKSalesCross { get { return Request.QueryString["niksc"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            saveLink(d);

            //Redirect
            Response.Redirect("Sales_Form.aspx?id=" + myID + "&status=1");
        }
    }

    protected void DataError() { Response.Redirect("Sales.aspx"); }

    protected void saveLink(SalesOrder data)
    {
        string query = "UPDATE SalesOrder SET KodeBrandCross = '" + myKodeBrandCross + "' , TipeLokasiCross = '" + myTipeLokasiCross + "', IDLokasiCross = '" + myIDLokasiCross + "', IDSalesCross = '" + myIDSalesCross + "', NamaSalesCross = '" + myNamaSalesCross + "', NIKSalesCross = '" + myNIKSalesCross + "' WHERE ID = " + myID;
		Db.Execute(query, Db.Cnn_DB);
    }
}