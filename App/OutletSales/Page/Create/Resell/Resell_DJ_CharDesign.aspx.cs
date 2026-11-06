using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_DJ_CharDesign02 : System.Web.UI.Page
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
    protected int myItemID { get { return App.QueryInt(this,"itemid"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("Resell.aspx"); }
    protected void LinkError() { Response.Redirect("Resell_ItemAddPLU.aspx?id=" + myID); }

    //Form
    protected void fillForm()
    {
        var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myItemID);
        if (d == null) DataError();
        else
        {
            DLC.CharProcessFinishing(cb);
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myItemID);
        if (d == null) DataError();
        else
        {
            var product = d.StockProductDJ_CharProduct;

            //Process
            saveFinishing(d);

            Lib.RecalWeight(db, d, product);

            //Redirect
            Response.Redirect("Resell_DJ_Stone.aspx?id=" + myID + "&itemid=" + myItemID);
        }
    }

    //Finishing
    protected void saveFinishing(StockProductDJ d)
    {
        var rs = from p in cb.Items.Cast<ListItem>()
                 select p;

        foreach (var r in rs)
        {
            int myIDFinishing = Convert.ToInt32(r.Value);
            if (r.Selected)
                insertProcess(d, myIDFinishing);
            else
                deleteProcess(d, myIDFinishing);
        }
    }
    protected void insertProcess(StockProductDJ ms, int myIDFinishing)
    {
        var d = ms.StockProductDJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d == null)
        {
            StockProductDJ_Finishing n = new StockProductDJ_Finishing();

            n.IDForm = ms.ID;
            n.IDFinishing = myIDFinishing;

            //Commit
            db.StockProductDJ_Finishing.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected void deleteProcess(StockProductDJ ms, int myIDFinishing)
    {
        var d = ms.StockProductDJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d != null)
        {
            //Commit
            db.StockProductDJ_Finishing.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
}