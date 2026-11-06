using System;
using System.Linq;

public partial class Page_Register_LD_StoneDelete : System.Web.UI.Page
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
    protected int myStoneIndex { get { return App.QueryInt(this, "si"); } }
    protected int myTableID { get { return App.QueryInt(this, "tid"); } }

    //Exception
    protected void DataError() { Response.Redirect("LD.aspx"); }
    protected void LinkError() { Response.Redirect("LD_Stone.aspx?id=" + myID); }
    protected void ProductError() { Response.Redirect("LD_CharProduct01.aspx?id=" + myID); }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Process
            save1B(d);
            //Redirect
            Response.Redirect("LD_Stone.aspx?id=" + myID);
        }
    }

    protected void save1B(StockReceiveLD ms)
    {
        var d = db.StockReceiveLD_Stone1B.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
        if (d == null) LinkError();
        else
        {
            //Commit
            db.StockReceiveLD_Stone1B.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
}