using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class GJ : System.Web.UI.Page
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

    //Filter
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
        string href = "GJ_List_Print.aspx?id=1";

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
        string href = "GJ_List_Export.aspx?id=1";

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
        DropDownList ddlc_tipe = tipelokasifrom; // condition : tipelokasifrom (outgoing) , tipelokasito (incoming)
        DropDownList ddlc_id = lokasifrom; // condition : lokasifrom (outgoing) , lokasito (incoming)
        DropDownList ddlf_tipe = tipelokasito; // free : tipelokasito (outgoing) , tipelokasifrom (incoming)
        DropDownList ddlf_id = lokasito; // free : lokasito (outgoing) , lokasifrom (incoming)
        DropDownList ddl_brand = brand;
        DropDownList ddl_status = status;
        TextBox txt_TglFrom = tgl;
        TextBox txt_TglTo = tgl2;
        TextBox txt_Keyword = kw;
        System.Web.UI.Page page = this;
        System.Web.HttpRequest request = Request;

        /// Fill Filter default date & all dropdownlist
        RPT.Filter_OutgoingIncoming(ddlc_tipe, ddlc_id, ddlf_tipe, ddlf_id, ddl_brand, ddl_status, txt_TglFrom, txt_TglTo, txt_Keyword, page, request);
    
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        String Mode = "GJ"; // DJ, PG, GJ, LD
        String Tipe = "rekap"; // rekap, detail
        String Modul = "accounting"; // accounting, outlet (outlet tidak ada harga M dan harga R pada tabel)
        String Sign = "outgoing"; // incoming, outgoing
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
        RPT.Fill_OutgoingIncoming(Mode, Tipe, Modul, Sign, Action, tbl_Table, s_tF, s_kF, s_tT, s_kT, s_vB, s_stats, s_pf, s_pt, s_kw, page, request, response);

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