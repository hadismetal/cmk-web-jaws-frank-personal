using System;
using System.Linq;

public partial class Page_Approval_Sales_Delete : System.Web.UI.Page
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
        Response.Redirect("Summary.aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        var d = db.StockAuditPG.SingleOrDefault(p => p.ID == myID && p.StatusFinish == false);
        if (d == null) App.DataNull(this);
        else
        {
            //Delete
            deleteProcess(d);

            //Redirect
            Response.Redirect("Summary.aspx?d=1");
        }
    }

    //Delete
    protected void deleteProcess(StockAuditPG d)
    {



        var x = db.StockAuditPG.SingleOrDefault(p => p.ID == d.ID);
        if (x != null)
        {
            db.StockAuditPGDetail.DeleteAllOnSubmit(x.StockAuditPGDetail);
            db.StockAuditItemPG.DeleteAllOnSubmit(x.StockAuditItemPG);
            db.StockAuditPG.DeleteOnSubmit(x);
            db.SubmitChanges();
        }

    }
}