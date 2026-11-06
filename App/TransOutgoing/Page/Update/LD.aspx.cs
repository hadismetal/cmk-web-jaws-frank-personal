using System;
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            tb.Visible = false;

            //DLC
            DLC.CompanyBrand(brand);

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
        href += "&brand=" + brand.SelectedValue;
        href += "&kw=" + keyword.Text.Trim();

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            //DLC.CompanyBrand(brand);
            //DLC.LocationType(ddl_from, ddl_to);

            brand.SelectedValue = Request.QueryString["brand"];

        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;

        //FILTER
        int vB = Convert.ToInt32(Request.QueryString["brand"].ToString());
        string kw = Request.QueryString["kw"].ToString();

        //SELECT
        var rs = (from p in db.StockOutgoingLD
                  where ((vB > 0) ? p.IDBrand == vB : 1 == 1) && p.StatusTransOut == 0 // 0 : Draft
                        && (p.StockOutgoingLD_Product.Any(z => z.StockProductLD_Stone1B.Nomor.Contains(kw))
                            || p.NamaAsal.Contains(kw) || p.NamaTujuan.Contains(kw) || p.Operator.Contains(kw))
                  select p);

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
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

        rs = rs.OrderBy(p => p.OperatorTgl);

        int vF = 0; // id from
        int vT = 0; // id to
        int tF = 0; // value from
        int tT = 0; // value to

        int index = 1;

        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            string TotalData = rs.Count().ToString();
            string msg = "";

            if (rs.Count() > 100) { rs = rs.Take(100); msg = "Showing 100 from " + TotalData + " data"; }
            else { msg = "Showing " + TotalData + " data"; }
            if (keyword.Text != "") { msg += " with keyword '" + kw + "'"; }
            if (msg != "") { lit_msg.Text = msg; }
            rs = rs.OrderByDescending(p => p.OperatorTgl);

            foreach (var item in rs)
            {
                var rsI = from p in db.StockOutgoingLD_Product where p.IDForm == item.ID select p;

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
                td.Text = TD_HyperLink(item);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.TipeAsal) + "<br>" + TD_Nama(tF, vF) + TD_Group(tF, vF) + TD_Addr(tF, vF) + "<br>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class='glyphicon glyphicon-arrow-right' aria-hidden='true'></span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.TipeTujuan) + "<br>" + TD_Nama(tT, vT) + TD_Group(tT, vT) + TD_Addr(tT, vT) + "<br>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rsI.Count().ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.OutgoingApprovalStatus(item.StatusTransOut);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Operator + "<br>" + Str.FullDate(item.Tgl);
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

        //RESULT
        int prev = 0;


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
    protected string TD_HyperLink(StockOutgoingLD r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"LD_Form.aspx?id=" + r.ID + "&idb=" + r.IDBrand + "&tf= " + r.TipeAsal + "&tt=" + r.TipeTujuan + "&idf=" + r.IDAsal + "&idt=" + r.IDTujuan + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
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
}