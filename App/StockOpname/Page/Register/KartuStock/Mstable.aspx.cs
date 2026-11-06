using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Mstable : System.Web.UI.Page
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
        // App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {            
            Ddl.Location(location, App.UserID);

            //datepicker.Text = Str.Tgl(DateTime.Now);
            //datepicker2.Text = Str.Tgl(DateTime.Now);

            //Generate            
        }
    }

    //Display Table
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
                        
            location.SelectedValue = Request.QueryString["location"];            
            //datepicker.Text = Request.QueryString["dari"];
            //datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    protected void NextStep(object sender, EventArgs e)
    {
        // Response.Write(location.SelectedValue);
        string href = "Mstable_Form.aspx?idlokasi="+location.SelectedValue;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "DJ_DetailPrint.aspx?id=1";

        //Customize-Qstr        
        Response.Redirect(href);
    }
}