using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class LD : System.Web.UI.Page
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

    protected string myV { get { return Request.QueryString["valid"]; } }

    private int IDLocation = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            tb.Visible = false;

            //DLC
            DLC.CompanyBrand(brandFrom);
            brandTo.SelectedValue = brandFrom.SelectedValue;
            foreach (ListItem x in brandFrom.Items)
            {
                brandTo.Items.Remove(x);
            }
            //App.Brand;
            DLC.LocationType(ddl_from, ddl_to);

            //Generate
            //fillFilter();

            fillDropdownLocation(ddl_locFrom, ddl_from, brandFrom);
            IDLocation = 0;
            fillDropdownLocation(ddl_locTo, ddl_to, brandTo);
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        Response.Redirect("LD_Form.aspx?idbf=" + brandFrom.SelectedValue + "&tf=" + ddl_from.SelectedValue + "&tt=" + ddl_to.SelectedValue + "&idf=" + ddl_locFrom.SelectedValue + "&idt=" + ddl_locTo.SelectedValue + "&idbt=" + brandTo.SelectedValue);
    }
    protected void fillFilter()
    {
        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            var x = d.First();
            string href = "";

            IDLocation = x.IDLokasi;
            if (x.TipeLokasi == 1)
            {
                ddl_from.Items.Clear();
                ddl_from.Items.Add(new ListItem("Outlet", "1"));
                brandFrom.SelectedValue = db.LocOutlet.SingleOrDefault(p => p.ID == IDLocation).IDBrand.ToString();
            }
            else if (x.TipeLokasi == 2)
            {
                ddl_from.Items.Clear();
                ddl_from.Items.Add(new ListItem("Exhibition", "2"));
                brandFrom.SelectedValue = db.LocExhibition.SingleOrDefault(p => p.ID == IDLocation).IDBrand.ToString();
            }

            brandFrom.SelectedValue = db.LocOutlet.SingleOrDefault(p => p.ID == IDLocation).IDBrand.ToString();
            ddl_locFrom.Enabled = false;
            brandFrom.Enabled = false;
            ddl_from.Enabled = false;
        }

        var f = from p in db.DataSK
                where p.IDUser == UserID
                select p;
        if (f.Count() > 0)
        {
            var x = f.First();
            string href = "";
            
            if (x.TipeLokasi == 0)
            {
                ddl_from.Items.Clear();
                ddl_from.Items.Add(new ListItem("Warehouse", "0"));
            }
            else if (x.TipeLokasi == 1)
            {
                ddl_from.Items.Clear();
                ddl_from.Items.Add(new ListItem("Outlet", "1"));
            }
            else if (x.TipeLokasi == 2)
            {
                ddl_from.Items.Clear();
                ddl_from.Items.Add(new ListItem("Exhibition", "2"));
            }
            IDLocation = x.IDLokasi;
        }
    }


    private void fillDropdownLocation(DropDownList location, DropDownList locationType, DropDownList brand)
    {

        var data = Db.Rs("sp_getLocationBrand " + brand.SelectedValue + ", " + locationType.SelectedValue + ", " + IDLocation, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);
        
        location.Items.Clear();
        location.DataBind();
        location.Items.Add(new ListItem("Location :", "0"));

        foreach (DataRow d in data.Rows)
        {

            string t = d["text"].ToString();
            string v = d["value"].ToString();
            location.Items.Add(new ListItem(t, v));
        }
        location.SelectedValue = IDLocation.ToString();
    }

    protected void ddl_from_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillDropdownLocation(ddl_locFrom, ddl_from, brandFrom);
    }

    protected void ddl_to_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillDropdownLocation(ddl_locTo, ddl_to, brandTo);
    }
}