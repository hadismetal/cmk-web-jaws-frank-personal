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
    protected void DataError() { Response.Redirect("DJ.aspx"); }
    protected void LinkError() { Response.Redirect("DJ_CharDesign.aspx?id=" + myID); }

    //Form
    protected void fillForm()
    {
        var d = db.CalculatorProductDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            DLC.CharProcessFinishing(cb);
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.CalculatorProductDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            var product = d.CalculatorProductDJ_CharProduct;

            //Process
            saveFinishing(d);

            Lib.RecalWeight(db, d, product);

            //Redirect
            Response.Redirect("DJ_Stone.aspx?id=" + myID);
        }
    }

    //Finishing
    protected void saveFinishing(CalculatorProductDJ d)
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
    protected void insertProcess(CalculatorProductDJ ms, int myIDFinishing)
    {
        var d = ms.CalculatorProductDJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d == null)
        {
            CalculatorProductDJ_Finishing n = new CalculatorProductDJ_Finishing();

            n.IDForm = ms.ID;
            n.IDFinishing = myIDFinishing;

            //Commit
            db.CalculatorProductDJ_Finishing.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected void deleteProcess(CalculatorProductDJ ms, int myIDFinishing)
    {
        var d = ms.CalculatorProductDJ_Finishing.SingleOrDefault(p => p.IDFinishing == myIDFinishing);
        if (d != null)
        {
            //Commit
            db.CalculatorProductDJ_Finishing.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
}