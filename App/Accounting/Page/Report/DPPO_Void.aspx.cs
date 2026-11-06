using System;
using System.Linq;

public partial class Page_Approval_Sales_Void : System.Web.UI.Page
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
    App price = new App();

    decimal Setting = 0m;

    protected int myID { get { return App.QueryInt(this); } }

    //Exception
    protected void DeleteError(string sebab)
    {
        Response.Redirect("DPPO.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.StatusDP == 0);
        if (d == null) App.DataNull(this);
        else
        {
            

            d.StatusDP = 2;
            db.SubmitChanges();

            //Redirect
            Response.Redirect("DPPO.aspx?d=1");
        }
    }
}