using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            //Generate

            fillTable(sender, e);
            fillHeader();
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        String Mode = "DJ"; // DJ, PG, GJ, LD
        String Tipe = "list"; // rekap, detail, list
        String Modul = "receiving"; // receiving
        String Sign = ""; // 
        String Action = "print"; // display, excel, print

        Table tbl_Table = tb;

        // utk display menggunakan control, utk export excel dan print menggunakan querystring
        String s_tF = ""; // tipelokasifrom.SelectedValue; // tipe lokasi from : W, O, E
        String s_kF = ""; // lokasifrom.SelectedValue; // id lokasi from
        String s_tT = ""; // tipelokasito.SelectedValue; // tipe lokasi to : W, O, E
        String s_kT = Request.QueryString["cabang"]; // cabang.SelectedValue; // id lokasi to
        String s_vB = Request.QueryString["brand"]; // brand.SelectedValue; // brand company
        String s_stats = ""; // status.SelectedValue; // status data
        String s_pf = Request.QueryString["dari"]; // datepicker.Text.Trim(); // period from
        String s_pt = Request.QueryString["sampai"]; // datepicker2.Text.Trim(); // period to
        String s_kw = Request.QueryString["kw"]; // kw.Text.Trim(); // keyword

        String s_ob = Request.QueryString["orderby"]; // orderby.SelectedValue; // orderby 
        String s_sup = Request.QueryString["supplier"]; // supplier.SelectedValue; // supplier 
        String s_sk = Request.QueryString["sk"]; // sk.SelectedValue; // stock keeper 
        String s_sup_group = Request.QueryString["suppliergroup"]; // suppliergroup.SelectedValue; // supplier group

        String s_plevel = Request.QueryString["productlevel"]; // productlevel.SelectedValue; // product level
        String s_pitem = Request.QueryString["productitem"]; // productitem.SelectedValue; // product item
        String s_pcategory = Request.QueryString["productcategory"]; // productcategory.SelectedValue; // product category

        System.Web.UI.Page page = this;
        System.Web.HttpRequest request = Request;
        System.Web.HttpResponse response = Response;

        /// Fill Table
        Lib.ReportReceiving_FillTable(Mode, Tipe, Modul, Sign, Action, tbl_Table, s_tF, s_kF, s_tT, s_kT, s_vB, s_stats, s_pf, s_pt, s_kw, s_ob, s_sup, s_sk, s_plevel, s_pitem, s_pcategory, s_sup_group, page, request, response);

    }

    protected void fillHeader()
    {

        var rs = from p in db.StockReceiveDJ
                 //where p.ReturnProduct == false && p.StatusPenjualan == 0
                 select p;
        var rsC = from p in db.CharProductCategory select p;
        var rsI = from p in db.CharProductItem select p;
        var rsL = from p in db.CharProductLevel select p;
        var rsB = from p in db.StockActualDJ select p;
        var rsN = from p in db.CompanyBrand select p;

        periode.InnerHtml = Request.QueryString["dari"];
        periode.InnerHtml += " s/d " + Request.QueryString["sampai"];

        if (Convert.ToInt32(Request.QueryString["cabang"]) > 0) { var v_cab = rsB.SingleOrDefault(p => p.IDLokasi == Convert.ToInt32(Request.QueryString["cabang"]) && p.TipeLokasi == Convert.ToInt32(Request.QueryString["tipe"])); cabang.InnerText = v_cab.NamaLokasi; } else { cabang.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productcategory"]) > 0) { var v_cat = rsC.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productcategory"])); category.InnerText = v_cat.Nama; } else { category.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productitem"]) > 0) { var v_itm = rsI.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productitem"])); item.InnerText = v_itm.Nama; } else { item.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productlevel"]) > 0) { var v_lvl = rsI.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productlevel"])); level.InnerText = v_lvl.Nama; } else { level.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["brand"]) > 0) { var v_brand = rsN.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["brand"])); brand.InnerText = v_brand.Nama; } else { brand.InnerText = "Semua"; }
        if (Request.QueryString["supplier"] != "") { supplier.InnerText = Request.QueryString["supplier"]; } else { supplier.InnerText = "Semua"; }

        // if (Convert.ToInt32(Request.QueryString["cabang"]) > 0) { var v_cab = rs.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["category"])); cabang.InnerText = v_cab.Nama; } else { cabang.InnerText = "Semua"; }

        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);
    }
}