using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class PG : System.Web.UI.Page
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

            //Form-Default
            tgl.Text = Str.Tgl(DateDrv.AwalBulan());
            tgl2.Text = Str.Tgl(DateTime.Today);

            //DLC
            DLC.LocationType(locationtypefrom, locationtypeto);
            DLC.CompanyBrand(brand);
            DLC.OutgoingApprovalStatus(status);
            
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
        href += "&from=" + locationtypefrom.SelectedValue;
        href += "&to=" + locationtypeto.SelectedValue;
        href += "&status=" + status.SelectedValue;
        href += "&keyfrom=" + txt_from.Text.Trim();
        href += "&keyto=" + txt_to.Text.Trim();
        href += "&keyword=" + kw.Text.Trim();

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "PG_List_Export.aspx?id=1";

        //Customize-Qstr
        href += "&pf=" + tgl.Text.Trim();
        href += "&pt=" + tgl2.Text.Trim();
        href += "&brand=" + brand.SelectedValue;
        href += "&from=" + locationtypefrom.SelectedValue;
        href += "&to=" + locationtypeto.SelectedValue;
        href += "&status=" + status.SelectedValue;
        href += "&keyfrom=" + txt_from.Text.Trim();
        href += "&keyto=" + txt_to.Text.Trim();
        href += "&keyword=" + kw.Text.Trim();

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "PG_List_Print.aspx?id=1";

        //Customize-Qstr
        href += "&pf=" + tgl.Text.Trim();
        href += "&pt=" + tgl2.Text.Trim();
        href += "&brand=" + brand.SelectedValue;
        href += "&from=" + locationtypefrom.SelectedValue;
        href += "&to=" + locationtypeto.SelectedValue;
        href += "&status=" + status.SelectedValue;
        href += "&keyfrom=" + txt_from.Text.Trim();
        href += "&keyto=" + txt_to.Text.Trim();
        href += "&keyword=" + kw.Text.Trim();

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            tgl.Text = Request.QueryString["pf"];
            tgl2.Text = Request.QueryString["pt"];
            brand.SelectedValue = Request.QueryString["brand"];
            locationtypefrom.SelectedValue = Request.QueryString["from"];
            locationtypeto.SelectedValue = Request.QueryString["to"];
            status.SelectedValue = Request.QueryString["status"];
            kw.Text = Request.QueryString["keyword"];

            txt_from.Text = Request.QueryString["keyfrom"];
            txt_to.Text = Request.QueryString["keyto"];

            int UserID = App.UserID;
            var d = from p in db.DataAdminSales
                    where p.IDUser == UserID
                    select p;
            if (d.Count() > 0)
            {
                var x = d.First();
                string href = "";

                if (x.TipeLokasi == 1)
                {
                    locationtypefrom.Items.Clear();
                    locationtypefrom.Items.Add(new ListItem("Outlet", "1"));
                }
                else if (x.TipeLokasi == 2)
                {
                    locationtypefrom.Items.Clear();
                    locationtypefrom.Items.Add(new ListItem("Exhibition", "2"));
                }
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
                    locationtypefrom.Items.Clear();
                    locationtypefrom.Items.Add(new ListItem("Warehouse", "0"));
                }
                else if (x.TipeLokasi == 1)
                {
                    locationtypefrom.Items.Clear();
                    locationtypefrom.Items.Add(new ListItem("Outlet", "1"));
                }
                else if (x.TipeLokasi == 2)
                {
                    locationtypefrom.Items.Clear();
                    locationtypefrom.Items.Add(new ListItem("Exhibition", "2"));
                }
            }
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;

        //GET DDL BRAND FROM-TO TYPE VALUE
        int vB = Convert.ToInt32(brand.SelectedValue);
        int tF = 0; // type from
        int tT = 0; // type to
        int p_mod = 0;
        int p_itm = 0;
        int stats = 0;
        string kF = txt_from.Text.Trim();
        string kT = txt_to.Text.Trim();

        string s_pf = tgl.Text.Trim();
        string s_pt = tgl2.Text.Trim();

        DateTime d_pf = Convert.ToDateTime(s_pf);
        DateTime d_pt = Convert.ToDateTime(s_pt).AddDays(1);

        if (status.SelectedIndex > 0) { stats = Convert.ToInt32(status.SelectedValue); }
        if (locationtypefrom.SelectedIndex > 0) { tF = Convert.ToInt32(locationtypefrom.SelectedValue); }
        if (locationtypeto.SelectedIndex > 0) { tT = Convert.ToInt32(locationtypeto.SelectedValue); }

        //SELECT
        var rs = (from p in db.StockOutgoingPG select p);
        //WHERE
        //rs = rs.Where(p => p.IDBrand == vB && p.ApprovalTgl >= d_pf && p.ApprovalTgl < d_pt && p.ApprovalTgl != null && p.ApprovalTgl != null);
        rs = rs.Where(p => p.IDBrand == vB && p.Tgl >= d_pf && p.Tgl < d_pt && p.ApprovalTgl != null && p.ApprovalTgl != null);
        if (locationtypefrom.SelectedIndex > 0) { rs = rs.Where(p => p.TipeAsal == tF); }
        if (locationtypeto.SelectedIndex > 0) { rs = rs.Where(p => p.TipeTujuan == tT); }
        if (status.SelectedIndex > 0) { rs = rs.Where(p => p.StatusTransOut == stats); }

        string keyword = kw.Text.Trim();
        if (keyword != "")
            rs = rs.Where(p => p.Nomor.Contains(keyword));

        if (kF != "")
        {
            if (tF == 0)
            {
                rs = from p in rs
                     join q in db.LocWarehouse on p.IDAsal equals q.ID
                     where q.Nama.Contains(kF) || q.Nama.Contains(kF)
                     select p;
            }
            if (tF == 1)
            {
                rs = from p in rs
                     join q in db.LocOutlet on p.IDAsal equals q.ID
                     where q.Nama.Contains(kF) || q.Nama.Contains(kF)
                     select p;
            }
            if (tF == 2)
            {
                rs = from p in rs
                     join q in db.LocExhibition on p.IDAsal equals q.ID
                     where q.Nama.Contains(kF) || q.Nama.Contains(kF)
                     select p;
            }
        }

        if (kT != "")
        {
            if (tT == 0)
            {
                rs = from p in rs
                     join q in db.LocWarehouse on p.IDTujuan equals q.ID
                     where q.Nama.Contains(kT) || q.Nama.Contains(kT)
                     select p;
            }
            if (tT == 1)
            {
                rs = from p in rs
                     join q in db.LocOutlet on p.IDTujuan equals q.ID
                     where q.Nama.Contains(kT) || q.Nama.Contains(kT)
                     select p;
            }
            if (tT == 2)
            {
                rs = from p in rs
                     join q in db.LocExhibition on p.IDTujuan equals q.ID
                     where q.Nama.Contains(kT) || q.Nama.Contains(kT)
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
            /*rs = from p in rs
                 where (p.TipeAsal == d.First().TipeLokasi && p.IDAsal == d.First().IDLokasi)
                 || (p.TipeTujuan == d.First().TipeLokasi && p.IDTujuan == d.First().IDLokasi)
                 select p;*/

            rs = from p in rs
                 where (p.TipeAsal == f.First().TipeLokasi && p.IDAsal == f.First().IDLokasi)
                 select p;
        }

        //if (tgl.Text != "" && tgl2.Text != "")
        //{
        //    rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(tgl.Text) && p.Tgl < Convert.ToDateTime(tgl2.Text).AddDays(1));
        //}

        //ORDERBY
        rs = rs.OrderBy(p => p.Tgl);

        int index = 1;

        rs = rs.OrderByDescending(p => p.Tgl);

        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            foreach (var item in rs)
            {

                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(item.ID);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Print(item.ID);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                td.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(item.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(item.Keterangan);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.NamaAsal;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.NamaTujuan;
                tr.Cells.Add(td);

                int Qty = 0;
                Qty = item.StockOutgoingPG_Product.Count();

                td = new TableCell();
                td.Text = Str.Number0(Qty);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                decimal Netto = 0m;
                Netto = item.StockOutgoingPG_Product.Sum(p => p.StockProductPG.NettoWeight.Value);

                td = new TableCell();
                td.Text = Str.Number2(Netto);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                decimal Gross = 0m;
                Gross = item.StockOutgoingPG_Product.Sum(p => p.StockProductPG.GrossWeight.Value);

                td = new TableCell();
                td.Text = Str.Number2(Gross);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Str.Number0(item.Butir);
                //td.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = Str.Number3(item.Carat);
                //td.HorizontalAlign = HorizontalAlign.Right;
                //tr.Cells.Add(td);

                decimal PriceR = 0m;
                PriceR = item.StockOutgoingPG_Product.Sum(p => p.StockProductPG.TotalHargaJual.Value);

                td = new TableCell();
                td.Text = Str.Number0(PriceR);
                td.HorizontalAlign = HorizontalAlign.Right;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Operator + "<br />" + Str.Tgl(item.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.Approval + "<br />" + Str.Tgl(item.ApprovalTgl);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }


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

        x += "<a class=\"btn btn-info\" href=\"PG_Detail.aspx?id=" + ID 
            + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  View";
        x += "</a>";

        return x;
    }
    protected string TD_Print(int ID)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"PG_Print.aspx?id=" + ID
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
}