using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page
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

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharGoldLevel(productlevel);
            DLC.CharFrameColor(framecolor);
            DLC.CompanyBrand(brand);
            DLC.CharGoldModelAllFilter(model);

            datepicker.Text = Str.Tgl(DateDrv.AwalBulan());
            datepicker2.Text = Str.Tgl(DateTime.Now);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        String Mode = "PG"; // DJ, PG, GJ, LD
        String Tipe = "rekap"; // rekap, detail, list
        String Modul = "receiving"; // receiving
        String Sign = ""; // 
        String Action = "display"; // display, excel, print

        Table tbl_Table = tb;

        // utk display menggunakan control, utk export excel dan print menggunakan querystring
        String s_tF = ""; // tipelokasifrom.SelectedValue; // tipe lokasi from : W, O, E
        String s_kF = ""; // lokasifrom.SelectedValue; // id lokasi from
        String s_tT = ""; // tipelokasito.SelectedValue; // tipe lokasi to : W, O, E
        String s_kT = ""; //cabang.SelectedValue; // id lokasi to
        String s_vB = brand.SelectedValue; // brand company
        String s_stats = ""; // status.SelectedValue; // status data
        String s_pf = datepicker.Text.Trim(); // period from
        String s_pt = datepicker2.Text.Trim(); // period to
        String s_kw = kw.Text.Trim(); // keyword

        String s_ob = ""; // orderby.SelectedValue; // orderby 
        String s_sup = ""; // supplier.SelectedValue; // supplier 
        String s_sk = ""; // sk.SelectedValue; // stock keeper 
        String s_sup_group = ""; // suppliergroup.SelectedValue; // supplier group

        String s_plevel = productlevel.SelectedValue; // product level
        String s_pitem = productitem.SelectedValue; // product item
        String s_pcategory = ""; // productcategory.SelectedValue; // product category

        String s_pmodel = model.SelectedValue; // product item
        String s_pframecolor = framecolor.SelectedValue; // productcategory.SelectedValue; // product category

        System.Web.UI.Page page = this;
        System.Web.HttpRequest request = Request;
        System.Web.HttpResponse response = Response;

        /// Fill Table
        Lib.ReportReceiving_FillTable(Mode, Tipe, Modul, Sign, Action, tbl_Table, s_tF, s_kF, s_tT, s_kT, s_vB, s_stats, s_pf, s_pt, s_kw, s_ob, s_sup, s_sk, s_plevel, s_pitem, s_pcategory, s_sup_group, page, request, response, s_pmodel, s_pframecolor);

    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            framecolor.SelectedValue = Request.QueryString["framecolor"];
            brand.SelectedValue = Request.QueryString["brand"];
            model.SelectedValue = Request.QueryString["model"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&brand=" + brand.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;


        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "PG_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&brand=" + brand.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "PG_Export.aspx?id=1";

        href += "&kw=" + kw.Text;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&brand=" + brand.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;

        Response.Redirect(href);
    }
}