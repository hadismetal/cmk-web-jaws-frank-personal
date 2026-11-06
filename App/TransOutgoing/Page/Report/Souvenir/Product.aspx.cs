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
    
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;

        //GET DDL BRAND FROM-TO TYPE VALUE
        int vB = Convert.ToInt32(brand.SelectedValue);
        DateTime pf = Convert.ToDateTime(tgl.Text.Trim());
        DateTime pt = Convert.ToDateTime(tgl2.Text.Trim());
        int tF = 0; // type from
        int tT = 0; // type to
        int kF = Convert.ToInt32(lokasifrom.SelectedValue);
        int kT = Convert.ToInt32(lokasito.SelectedValue);
		
        
        //WHERE
        string w = "1=1";
        //if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;
        if (status.SelectedIndex > 0) w += " AND StatusTransOut = " + status.SelectedValue;
        if (tipelokasifrom.SelectedIndex > 0) { tF = Convert.ToInt32(tipelokasifrom.SelectedValue); }
        if (tipelokasito.SelectedIndex > 0) { tT = Convert.ToInt32(tipelokasito.SelectedValue); }
        
        //SELECT
        var rs = (from p in db.StockOutgoingSouvenir.Where(w)
                  select p);
        rs = rs.Where(p => (vB >= 0) ? p.IDBrand == vB : 1 == 1 && p.Tgl >= pf && p.Tgl <= pt && p.ApprovalTgl != null);

        if (tipelokasifrom.SelectedIndex > 0) { rs = rs.Where(p => p.TipeAsal == tF); }
        if (tipelokasito.SelectedIndex > 0) { rs = rs.Where(p => p.TipeTujuan == tT); }

        if (kF > 0)
        {
            if (tF == 0)
            {
                rs = from p in rs
                     join q in db.LocWarehouse on p.IDAsal equals q.ID
                     where q.ID == kF
                     select p;
            }
            if (tF == 1)
            {
                rs = from p in rs
                     join q in db.LocOutlet on p.IDAsal equals q.ID
                     where q.ID == kF
                     select p;
            }
            if (tF == 2)
            {
                rs = from p in rs
                     join q in db.LocExhibition on p.IDAsal equals q.ID
                     where q.ID == kF
                     select p;
            }
        }

        if (kT > 0)
        {

            if (tT == 0)
            {
                rs = from p in rs
                     join q in db.LocWarehouse on p.IDTujuan equals q.ID
                     where q.ID == kT
                     select p;
            }
            if (tT == 1)
            {
                rs = from p in rs
                     join q in db.LocOutlet on p.IDTujuan equals q.ID
                     where q.ID == kT
                     select p;
            }
            if (tT == 2)
            {
                rs = from p in rs
                     join q in db.LocExhibition on p.IDTujuan equals q.ID
                     where q.ID == kT
                     select p;
            }
        }

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            /*rs = from p in rs
                 where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                 || (p.TipeTujuan == d.First().TipeLokasi && p.IDTujuan == d.First().IDLokasi)
                 select p;*/

            rs = from p in rs
                 where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                 select p;
        }

        var f = from p in db.DataSK
                where p.IDUser == UserID
                select p;
        if (f.Count() > 0)
        {
            rs = from p in rs
                 where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                 select p;
        }

        rs = rs.OrderByDescending(p => p.OperatorTgl);

        int vF = 0; // id from
        int vT = 0; // id to

        int index = 1;

        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            foreach (var item in rs)
            {
                var rsI = from p in db.StockOutgoingSouvenir_Product where p.IDForm == item.ID select p;

                if (!Response.IsClientConnected) break;

                vF = item.IDAsal; // id from
                vT = item.IDTujuan; // id to
                tF = item.TipeAsal; // value from
                tT = item.TipeTujuan; // value to

                //Sub-Header
                //if (prev != r1.IDBrand)
                //{
                //    Sub_Header(vT, r1.ID);
                //    prev = r1.IDBrand;
                //}

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = (item.StatusTransOut != 0) ? TD_HyperLink(item.ID) : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (item.StatusTransOut == 2) ? TD_Print(item.ID) : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.FullDate(item.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Keterangan;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text =  TD_Nama(tF, vF) ;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nama(tT, vT) ;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rsI.Count().ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Operator;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.FullDate(item.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Approval;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.FullDate(item.ApprovalTgl);
                tr.Cells.Add(td);
                tb.Rows.Add(tr);
            }
        }

        

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

     protected void Export(object sender, EventArgs e)
    {
        string href = "Product_Export.aspx?id=1";

        //Customize-Qstr
        href += "&pf=" + tgl.Text.Trim();
        href += "&pt=" + tgl2.Text.Trim();
        href += "&brand=" + brand.SelectedValue;
        href += "&tipelokasifrom=" + tipelokasifrom.SelectedValue;
        href += "&tipelokasito=" + tipelokasito.SelectedValue;
        href += "&status=" + status.SelectedValue;
        href += "&lokasifrom=" + lokasifrom.SelectedValue;
        href += "&lokasito=" + lokasito.Text.Trim();

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
    protected string TD_HyperLink(int ID)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"Product_Detail.aspx?id=" + ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  View";
        x += "</a>";

        return x;
    }
    protected string TD_Print(int ID)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"Product_Print.aspx?id=" + ID + "\">";
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
}