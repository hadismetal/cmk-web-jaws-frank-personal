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

        var d = db.StockAudit.SingleOrDefault(p => p.ID == myID && p.StatusFinish == false);
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
    protected void deleteProcess(StockAudit d)
    {

        var dj = d.StockAudit_Detail.First();

        int IDDJ = dj.IDDJ;
        int IDGJ = dj.IDGJ;
        int IDPG = dj.IDPG;
        int IDLD = dj.IDLD;

        db.StockAudit_Detail.DeleteAllOnSubmit(d.StockAudit_Detail);
        db.SubmitChanges();


        var x = db.StockAuditDJ.SingleOrDefault(p => p.ID == IDDJ);
        if (x != null)
        {
            db.StockAuditDJDetail.DeleteAllOnSubmit(x.StockAuditDJDetail);
            db.StockAuditItemDJ.DeleteAllOnSubmit(x.StockAuditItemDJ);
            db.StockAuditDJ.DeleteOnSubmit(x);
            db.SubmitChanges();
        }

        var y = db.StockAuditGJ.SingleOrDefault(p => p.ID == IDGJ);
        if (y != null)
        {
            db.StockAuditGJDetail.DeleteAllOnSubmit(y.StockAuditGJDetail);
            db.StockAuditItemGJ.DeleteAllOnSubmit(y.StockAuditItemGJ);
            db.StockAuditGJ.DeleteOnSubmit(y);
            db.SubmitChanges();
        }


        var z = db.StockAuditPG.SingleOrDefault(p => p.ID == IDPG);
        if (z != null)
        {
            db.StockAuditPGDetail.DeleteAllOnSubmit(z.StockAuditPGDetail);
            db.StockAuditItemPG.DeleteAllOnSubmit(z.StockAuditItemPG);
            db.StockAuditPG.DeleteOnSubmit(z);
            db.SubmitChanges();
        }


        var u = db.StockAuditLD.SingleOrDefault(p => p.ID == IDLD);
        if (u != null)
        {
            db.StockAuditLDDetail.DeleteAllOnSubmit(u.StockAuditLDDetail);
            db.StockAuditItemLD.DeleteAllOnSubmit(u.StockAuditItemLD);
            db.StockAuditLD.DeleteOnSubmit(u);
            db.SubmitChanges();
        }

        //
        db.StockAudit.DeleteOnSubmit(d);
        db.SubmitChanges();
    }
}