using System;
using System.Data;
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
        href += "&souvenir=" + packaging.SelectedValue;

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
        Lib.ReportOutgoingIncoming_Filter(ddlc_tipe, ddlc_id, ddlf_tipe, ddlf_id, ddl_brand, ddl_status, txt_TglFrom, txt_TglTo, txt_Keyword, page, request);
        fillDropDown();
        packaging.SelectedValue = request.QueryString["souvenir"];
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
        }

        var f = from p in db.DataSK
                where p.IDUser == UserID
                select p;
        if (f.Count() > 0)
        {
            
        }

        string spname = "sp_detailOutgoingSouvenir";
        string query = " exec " + spname
                     + "'" + reverseDate(tgl.Text) + "', "
                     + "'" + reverseDate(tgl2.Text) + "', "
                     + packaging.SelectedValue + ", "
                     + lokasifrom.SelectedValue + ", "
                     + lokasito.SelectedValue + ", "
                     + tipelokasifrom.SelectedValue + ", "
                     + tipelokasito.SelectedValue + ", "
                     + status.SelectedValue + ", "
                     + brand.SelectedValue;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        int index = 1;
        string plu = string.Empty;
        if (dt.Rows.Count == 0) { App.TableNoResult(tb); }
        else
        {
            foreach (DataRow dr in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell td;

                if (plu != dr["Nomor"].ToString())
                {
                    td = new TableCell();
                    td.Text = "<p>" + index++ + ".</p>";
                    td.RowSpan = Convert.ToInt32(dr["Counts"]);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["Nomor"].ToString();
                    td.RowSpan = Convert.ToInt32(dr["Counts"]);
                    plu = dr["Nomor"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["Tgl"].ToString();
                    td.RowSpan = Convert.ToInt32(dr["Counts"]);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["Location"].ToString();
                    td.RowSpan = Convert.ToInt32(dr["Counts"]);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dr["Tujuan"].ToString();
                    td.RowSpan = Convert.ToInt32(dr["Counts"]);
                    tr.Cells.Add(td);

                }

                td = new TableCell();
                td.Text = dr["Souvenir"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["QtyKirim"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["QtyTerima"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(dr["SelisihTerima"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Status"].ToString();
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }

        tb.Visible = true;

        //COUNT FROM RESULT
        //int iFW = rsW.Count();
        //int iFO = rsO.Count();
        //int iFE = rsE.Count();

        bool isValidFromTo = true;

        //NO-RESULT FROM RESULT
        //if ((vF == 0 || vT == 0) && rsW.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
        //if ((vF == 1 || vT == 1) && rsO.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }
        //if ((vF == 2 || vT == 2) && rsE.Count() == 0) { App.TableNoResult(tb); isValidFromTo = false; }

        //NO-RESULT FROM-TO (SAME LOCATION) RESULT
        //if (vF == 0 && vT == 0 && rsW.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
        //if (vF == 1 && vT == 1 && rsO.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }
        //if (vF == 2 && vT == 2 && rsE.Count() == 1) { App.TableNoResult(tb); isValidFromTo = false; }

        //NO-RESULT
        //if (rs.Count() == 0) App.TableNoResult(tb);


    }

        protected void Export(object sender, EventArgs e) {
        

        string href = "Product_Detail_Export.aspx?id=1";

        //Customize-Qstr
        href += "&pf=" + reverseDate(tgl.Text.Trim());
        href += "&pt=" + reverseDate(tgl2.Text.Trim());
        href += "&brand=" + brand.SelectedValue;
        href += "&tipelokasifrom=" + tipelokasifrom.SelectedValue;
        href += "&tipelokasito=" + tipelokasito.SelectedValue;
        href += "&status=" + status.SelectedValue;
        href += "&lokasifrom=" + lokasifrom.SelectedValue;
        href += "&lokasito=" + lokasito.Text.Trim();
        href += "&souvenir=" + packaging.SelectedValue;

        Response.Redirect(href);
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
        /*else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }*/

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
        /*else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
            x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }*/

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
        /*else if (t == 2)
        {
            var rs = db.LocExhibition.SingleOrDefault(p => p.ID == r);
            x += "<p class=\"text-info\">" + rs.LocExhibitionGroup.NamaKode + "</p>";
        }*/

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

    private string reverseDate(string date)
    {

        string newDate = string.Empty;
        
        string[] arr = date.Split('-');
        newDate = arr[2] + "-" + arr[1] + "-" + arr[0];
        
        return newDate;
    }

    private void fillDropDown()
    {
        var data = from p in db.Souvenir
                   where !p.Disable
                   select p;
        packaging.Items.Add(new ListItem("All", "0"));
        foreach (var d in data)
        {
            string t = d.Nama;
            string v = d.ID.ToString();
            packaging.Items.Add(new ListItem(t, v));
        }

    }
}