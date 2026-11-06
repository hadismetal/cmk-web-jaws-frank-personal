using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using CrossBrand;

public partial class DJ : System.Web.UI.Page
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
    Modelize cross = new Modelize();
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=DJ_List_Export.xls");
            fillTable(sender, e);
            FillHeader();
        }
    }


    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        String Mode = "DJCrossBrand"; // DJ, PG, GJ, LD
        String Tipe = "rekap"; // rekap, detail
        String Modul = "outlet"; // accounting, outlet (outlet tidak ada harga M dan harga R pada tabel)
        String Sign = "incoming"; // incoming, outgoing
        String Action = "excel"; // display, excel, print
        
        Table tbl_Table = tb;

        // utk display menggunakan control, utk export excel dan print menggunakan querystring
        String s_tF = Request.QueryString["tipelokasifrom"].Trim();
        String s_kF = Request.QueryString["lokasifrom"].Trim();
        String s_tT = Request.QueryString["tipelokasito"].Trim();
        String s_kT = Request.QueryString["lokasito"].Trim();
        String s_vBF = Request.QueryString["brandFrom"].Trim();
        String s_vBT = Request.QueryString["brandTo"].Trim();
        String s_stats = Request.QueryString["status"].Trim();
        String s_pf = Request.QueryString["pf"].Trim();
        String s_pt = Request.QueryString["pt"].Trim();
        String s_kw = Request.QueryString["keyword"].Trim();

        System.Web.UI.Page page = this;
        System.Web.HttpRequest request = Request;
        System.Web.HttpResponse response = Response;

        /// Fill Table
        Lib.ReportOutgoingIncoming_FillTable(Mode, Tipe, Modul, Sign, Action, tbl_Table, s_tF, s_kF, s_tT, s_kT, s_vBF, s_vBT, s_stats, s_pf, s_pt, s_kw, page, request, response);

    }
    protected void Sub_Header(int t, int r)
    {
        //TableRow tr = new TableRow();
        //TableCell td;

        //tr.CssClass = "sub-header";
        //string x = string.Empty;

        //if (t == 0)
        //{
        //    var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
        //    x += "<br />";
        //    x += rs.CompanyBrand.Nama + " (" + rs.CompanyBrand.NamaKode + ")";
        //}
        //else if (t == 1)
        //{
        //    var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
        //    x += "<br />";
        //    x += rs.CompanyBrand.Nama + " (" + rs.CompanyBrand.NamaKode + ")";
        //}
        //else if (t == 2)
        //{
        //    var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
        //    x += "<br />";
        //    x += rs.CompanyBrand.Nama + " (" + rs.CompanyBrand.NamaKode + ")";
        //}

        //td = new TableCell();
        //td.Text = x;
        //td.CssClass = "text-lg text-warning";
        //td.ColumnSpan = App.TableColspan(tb);
        //tr.Cells.Add(td);

        //tb.Rows.Add(tr);
    }
    protected string TD_Nama(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }

        return x;
    }
    protected string TD_Addr(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }

        return x;
    }
    protected string TD_Group(int t, int r)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = db.LocWarehouse.SingleOrDefault(p => p.ID == r);
            x += "<p class=\"text-info\">" + rs.LocWarehouseGroup.NamaKode + "</p>";
        }
        else if (t == 1)
        {
            var rs = db.LocOutlet.SingleOrDefault(p => p.ID == r);
            x += "<p class=\"text-info\">" + rs.LocOutletGroup.NamaKode + "</p>";
        }
        else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            x += "<p class=\"text-info\">" + rs.LocExhibitionGroup.NamaKode + "</p>";
        }

        return x;
    }

    protected void FillHeader()
    {
        var rsB = db.CompanyBrand.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["brandTo"]));
        var rsC = from p in db.CharProductCategory select p;
        var rsI = from p in db.CharProductItem select p;

        if (Convert.ToInt32(Request.QueryString["category"]) > 0) { var v_cat = rsC.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["category"])); category.InnerText = v_cat.Nama; } else { category.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["item"]) > 0) { var v_itm = rsI.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["item"])); item.InnerText = v_itm.Nama; } else { item.InnerText = "Semua"; }

        periode.InnerHtml = Request.QueryString["pf"];
        periode.InnerHtml += " s/d " + Request.QueryString["pt"];
        brand.InnerText = (rsB!=null) ? rsB.Nama : "All";
        lit_type_from.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["tipelokasifrom"]));
        lit_type_to.Text = DL.LocationType(Convert.ToInt32(Request.QueryString["tipelokasito"]));

        string From = ""; string To = "";
        int kF = Convert.ToInt32(Request.QueryString["lokasifrom"]);
        int kT = Convert.ToInt32(Request.QueryString["lokasiTo"]);
        int bF = Convert.ToInt32(Request.QueryString["brandFrom"]);
        int bT = Convert.ToInt32(Request.QueryString["brandTo"]);
        int tf = Convert.ToInt32(Request.QueryString["tipelokasifrom"]);
        int tT = Convert.ToInt32(Request.QueryString["tipelokasito"]);

        if (Convert.ToInt32(Request.QueryString["tipelokasifrom"]) == 00 && kF == 0) { From = "Semua"; }
        else if (Convert.ToInt32(Request.QueryString["tipelokasifrom"]) == 0 && kF > 0) { var rsf = cross.Warehouses(bF, kF, tf).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
        else if (Convert.ToInt32(Request.QueryString["tipelokasifrom"]) == 1 && kF > 0) { var rsf = cross.Outlets(bF, kF, tf).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
        else if (Convert.ToInt32(Request.QueryString["tipelokasifrom"]) == 2 && kF > 0) { var rsf = cross.Exhibitions(bF, kF, tf).SingleOrDefault(); From = rsf.Nama + "<br>(" + rsf.LocGroup + ")"; }
        if (Convert.ToInt32(Request.QueryString["tipelokasito"]) == 00 && kT == 0) { To = "Semua"; }
        else if (Convert.ToInt32(Request.QueryString["tipelokasito"]) == 0 && kT > 0) { var rst = cross.Warehouses(bT, kT, tT).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }
        else if (Convert.ToInt32(Request.QueryString["tipelokasito"]) == 1 && kT > 0) { var rst = cross.Outlets(bT, kT, tT).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }
        else if (Convert.ToInt32(Request.QueryString["tipelokasito"]) == 2 && kT > 0) { var rst = cross.Exhibitions(bT, kT, tT).SingleOrDefault(); To = rst.Nama + "<br>(" + rst.LocGroup + ")"; }


        from.InnerHtml = From;
        to.InnerHtml = To;

        // Label Outgoing
        lit_status.Text = "Outgoing";
        status.InnerText = Convert.ToInt32(Request.QueryString["status"]) > 0 ? DL.OutgoingApprovalStatus(Convert.ToInt32(Request.QueryString["status"])) : "Semua";
        //category.InnerText = Request.QueryString["category"];
        //item.InnerText = Request.QueryString["item"];
        search.InnerText = Request.QueryString["key"] == "" ? "-" : Request.QueryString["key"];

        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);
    }
}