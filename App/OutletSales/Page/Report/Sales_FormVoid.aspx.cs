using System;
using System.Linq;

public partial class Page_Register_Sales_Form : System.Web.UI.Page
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

    string ID = "";

    protected int myID { get { return App.QueryInt(this, "id"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            keterangan.Focus();

            //Form
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("Sales.aspx"); }

    //Form
    protected void fillForm()
    {

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            so.InnerHtml = d.Nomor;
        }

    }
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if(d!=null)
        {
            d.KeteranganVoid = keterangan.Text;
            db.SubmitChanges();
        }
        Response.Redirect("Sales_Void.aspx?id=" + myID);
    }


}