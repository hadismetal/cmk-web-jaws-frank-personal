using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_DJ : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            saveDJ();
        }
    }

    protected void saveDJ()
    {
        CalculatorProductDJ n = new CalculatorProductDJ();


        //User-Info
        n.Operator = App.UserNama;
        n.OperatorTgl = DateTime.Now;



        //Commit
        db.CalculatorProductDJ.InsertOnSubmit(n);
        db.SubmitChanges();

        //Redirect
        Response.Redirect("DJ_CharProduct.aspx?id=" + n.ID);
    }
}