using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            tb.Visible = false;

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&pf=" + tgl.Text.Trim();
        href += "&pt=" + tgl2.Text.Trim();
        href += "&brand=" + brand.SelectedValue;
        href += "&tipelokasifrom=" + tipelokasifrom.SelectedValue;
        href += "&tipelokasito=" + tipelokasito.SelectedValue;
        href += "&status=" + status.SelectedValue;
        href += "&lokasifrom=" + lokasifrom.SelectedValue;
        href += "&lokasito=" + lokasito.Text.Trim();
        href += "&keyword=" + kw.Text.Trim();

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "DJ_List_Print.aspx?id=1";

        //Customize-Qstr
        href += "&pf=" + tgl.Text.Trim();
        href += "&pt=" + tgl2.Text.Trim();
        href += "&brand=" + brand.SelectedValue;
        href += "&tipelokasifrom=" + tipelokasifrom.SelectedValue;
        href += "&tipelokasito=" + tipelokasito.SelectedValue;
        href += "&status=" + status.SelectedValue;
        href += "&lokasifrom=" + lokasifrom.SelectedValue;
        href += "&lokasito=" + lokasito.Text.Trim();
        href += "&keyword=" + kw.Text.Trim();

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "DJ_List_Export.aspx?id=1";

        //Customize-Qstr
        href += "&pf=" + tgl.Text.Trim();
        href += "&pt=" + tgl2.Text.Trim();
        href += "&brand=" + brand.SelectedValue;
        href += "&tipelokasifrom=" + tipelokasifrom.SelectedValue;
        href += "&tipelokasito=" + tipelokasito.SelectedValue;
        href += "&status=" + status.SelectedValue;
        href += "&lokasifrom=" + lokasifrom.SelectedValue;
        href += "&lokasito=" + lokasito.Text.Trim();
        href += "&keyword=" + kw.Text.Trim();

        Response.Redirect(href);
    }

    protected void fillFilter()
    {
        DropDownList ddlc_tipe = tipelokasito; // condition : tipelokasifrom (outgoing) , tipelokasito (incoming)
        DropDownList ddlc_id = lokasito; // condition : lokasifrom (outgoing) , lokasito (incoming)
        DropDownList ddlf_tipe = tipelokasifrom; // free : tipelokasito (outgoing) , tipelokasifrom (incoming)
        DropDownList ddlf_id = lokasifrom; // free : lokasito (outgoing) , lokasifrom (incoming)
        DropDownList ddl_brand = brand;
        DropDownList ddl_status = status;
        TextBox txt_TglFrom = tgl;
        TextBox txt_TglTo = tgl2;
        TextBox txt_Keyword = kw;
        System.Web.UI.Page page = this;
        System.Web.HttpRequest request = Request;

        /// Fill Filter default date & all dropdownlist
        Lib.ReportOutgoingIncoming_Filter(ddlc_tipe, ddlc_id, ddlf_tipe, ddlf_id, ddl_brand, ddl_status, txt_TglFrom, txt_TglTo, txt_Keyword, page, request, "incoming");

    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        String Mode = "DJ"; // DJ, PG, GJ, LD
        String Tipe = "rekap"; // rekap, detail
        String Modul = "outlet"; // accounting, outlet (outlet tidak ada harga M dan harga R pada tabel)
        String Sign = "incoming"; // incoming, outgoing
        String Action = "display"; // display, excel, print

        Table tbl_Table = tb;

        // utk display menggunakan control, utk export excel dan print menggunakan querystring
        String s_tF = tipelokasifrom.SelectedValue;
        String s_kF = lokasifrom.SelectedValue;
        String s_tT = tipelokasito.SelectedValue;
        String s_kT = lokasito.SelectedValue;
        String s_vB = brand.SelectedValue;
        String s_stats = status.SelectedValue;
        String s_pf = tgl.Text.Trim();
        String s_pt = tgl2.Text.Trim();
        String s_kw = kw.Text.Trim();

        System.Web.UI.Page page = this;
        System.Web.HttpRequest request = Request;
        System.Web.HttpResponse response = Response;

        /// Fill Table
        Lib.ReportOutgoingIncoming_FillTable(Mode, Tipe, Modul, Sign, Action, tbl_Table, s_tF, s_kF, s_tT, s_kT, s_vB, s_stats, s_pf, s_pt, s_kw, page, request, response);

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
    protected string TD_HyperLink(int ID)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"DJ_Detail.aspx?id=" + ID
            + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  View";
        x += "</a>";

        return x;
    }
    protected string TD_Print(int ID)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"DJ_Print.aspx?id=" + ID
            
            + "\">";
        x += "  <i class=\"icon-print\"></i> ";
        x += "  Print";
        x += "</a>";

        return x;
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

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasifrom.SelectedValue);
        lokasifrom.Items.Clear();
        if (Tipe == 0 || Tipe == 1 || Tipe == 2) { DLC.LocationList(lokasifrom, Tipe); }
        else { lokasifrom.Items.Add(new ListItem("All", "-1")); }
        lokasifrom.Focus();
    }

    protected void FillLokasiTo(object sender, EventArgs e)
    {
        int TipeTo = Convert.ToInt32(tipelokasito.SelectedValue);
        lokasito.Items.Clear();
        if (TipeTo == 0 || TipeTo == 1 || TipeTo == 2) { DLC.LocationList(lokasito, TipeTo); }
        else { lokasito.Items.Add(new ListItem("All", "-1")); }
        lokasito.Focus();
    }
}