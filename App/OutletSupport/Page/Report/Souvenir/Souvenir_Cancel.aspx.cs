using System;
using System.Linq;

public partial class Page_Register_Sales_ItemAddSave : System.Web.UI.Page
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
    App gp = new App();
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
        }

        Save();
    }

    //Exception
    protected void DataError() { Response.Redirect("Sales.aspx"); }


    protected void Save()
    {

        var d = db.BundleItem.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            d.StatusActive = 2;
            db.SubmitChanges();
            Response.Redirect("Souvenir.aspx?d=1");
        }
    }

}