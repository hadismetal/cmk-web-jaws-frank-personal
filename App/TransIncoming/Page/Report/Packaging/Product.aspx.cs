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

            int Tipe = 0;
            if (Request.QueryString["tipelokasifrom"] != "-1")
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasifrom"]);

            DLC.LocationList(lokasifrom, Tipe);

            int TipeTo = 0;
            if (Request.QueryString["tipelokasito"] != "-1")
                TipeTo = Convert.ToInt32(Request.QueryString["tipelokasito"]);

            DLC.LocationList(lokasito, TipeTo);

            //DLC
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
        href += "&brand=" + brand.SelectedValue;
        href += "&status=" + status.SelectedValue;
        href += "&tipelokasifrom=" + tipelokasifrom.SelectedValue;
        href += "&tipelokasito=" + tipelokasito.SelectedValue;
        href += "&status=" + status.SelectedValue;
        href += "&lokasifrom=" + lokasifrom.SelectedValue;
        href += "&lokasito=" + lokasito.Text.Trim();

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            //DLC.CompanyBrand(brand);
            //DLC.LocationType(ddl_from, ddl_to);

            brand.SelectedValue = Request.QueryString["brand"];
            status.SelectedValue = Request.QueryString["status"];
            tipelokasifrom.SelectedValue = Request.QueryString["tipelokasifrom"];
            tipelokasito.SelectedValue = Request.QueryString["tipelokasito"];
            lokasifrom.Text = Request.QueryString["lokasifrom"];
            lokasito.Text = Request.QueryString["lokasito"];

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
                    tipelokasifrom.Items.Clear();
                    tipelokasifrom.Items.Add(new ListItem("Outlet", "1"));
                }
                else if (x.TipeLokasi == 2)
                {
                    tipelokasifrom.Items.Clear();
                    tipelokasifrom.Items.Add(new ListItem("Exhibition", "2"));
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
                    tipelokasifrom.Items.Clear();
                    tipelokasifrom.Items.Add(new ListItem("Warehouse", "0"));
                }
                else if (x.TipeLokasi == 1)
                {
                    tipelokasifrom.Items.Clear();
                    tipelokasifrom.Items.Add(new ListItem("Outlet", "1"));
                }
                else if (x.TipeLokasi == 2)
                {
                    tipelokasifrom.Items.Clear();
                    tipelokasifrom.Items.Add(new ListItem("Exhibition", "2"));
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
        //int vB = 1;
        int tF = 0; // type from
        int tT = 0; // type to
        int kF = Convert.ToInt32(lokasifrom.SelectedValue);
        int kT = Convert.ToInt32(lokasito.SelectedValue);
        
        //WHERE
        string w = "1=1";
        //if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;
        if (status.SelectedIndex > 0) w += " AND StatusTransIn = " + status.SelectedValue;
        if (tipelokasifrom.SelectedIndex > 0) { tF = Convert.ToInt32(tipelokasifrom.SelectedValue); }
        if (tipelokasito.SelectedIndex > 0) { tT = Convert.ToInt32(tipelokasito.SelectedValue); }

        //SELECT
        var rs = (from p in db.StockIncomingPackaging.Where(w)
                  select p);

        if (tipelokasifrom.SelectedIndex > 0) { rs = rs.Where(p => p.StockOutgoingPackaging.TipeAsal == tF); }
        if (tipelokasito.SelectedIndex > 0) { rs = rs.Where(p => p.StockOutgoingPackaging.TipeTujuan == tT); }

        if (kF > 0)
        {
            if (tF == 0)
            {
                rs = from p in rs
                     join q in db.LocWarehouse on p.StockOutgoingPackaging.IDAsal equals q.ID
                     where q.ID == kF
                     select p;
            }
            if (tF == 1)
            {
                rs = from p in rs
                     join q in db.LocOutlet on p.StockOutgoingPackaging.IDAsal equals q.ID
                     where q.ID == kF
                     select p;
            }
            if (tF == 2)
            {
                rs = from p in rs
                     join q in db.LocExhibition on p.StockOutgoingPackaging.IDAsal equals q.ID
                     where q.ID == kF
                     select p;
            }
        }

        if (kT > 0)
        {

            if (tT == 0)
            {
                rs = from p in rs
                     join q in db.LocWarehouse on p.StockOutgoingPackaging.IDTujuan equals q.ID
                     where q.ID == kT
                     select p;
            }
            if (tT == 1)
            {
                rs = from p in rs
                     join q in db.LocOutlet on p.StockOutgoingPackaging.IDTujuan equals q.ID
                     where q.ID == kT
                     select p;
            }
            if (tT == 2)
            {
                rs = from p in rs
                     join q in db.LocExhibition on p.StockOutgoingPackaging.IDTujuan equals q.ID
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
                 where (p.StockOutgoingPackaging.TipeTujuan == d.First().TipeLokasi && p.StockOutgoingPackaging.IDTujuan == d.First().IDLokasi)
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
                 where (p.StockOutgoingPackaging.TipeTujuan == f.First().TipeLokasi && p.StockOutgoingPackaging.IDTujuan == f.First().IDLokasi)
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
                var rsI = from p in db.StockIncomingPackaging_Product where p.IDForm == item.ID select p;

                if (!Response.IsClientConnected) break;

                vF = item.StockOutgoingPackaging.IDAsal; // id from
                vT = item.StockOutgoingPackaging.IDTujuan; // id to
                tF = item.StockOutgoingPackaging.TipeAsal; // value from
                tT = item.StockOutgoingPackaging.TipeTujuan; // value to

                //Sub-Header
                //if (prev != r1.IDBrand)
                //{
                //    Sub_Header(vT, r1.ID);
                //    prev = r1.IDBrand;
                //}

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = (item.StatusTransIn != 0) ? TD_HyperLink(item.ID) : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (item.StatusTransIn == 2) ? TD_Print(item.ID) : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.StockOutgoingPackaging.TipeAsal) + "<br>" + TD_Nama(tF, vF) + "<br>" + TD_Addr(tF, vF) + "<br>" + TD_Group(tF, vF);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "&nbsp;&nbsp;&nbsp;&rarr;&nbsp;&nbsp;&nbsp;";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.StockOutgoingPackaging.TipeTujuan) + "<br>" + TD_Nama(tT, vT) + "<br>" + TD_Addr(tT, vT) + "<br>" + TD_Group(tT, vT);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rsI.Count().ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.OutgoingApprovalStatus(item.StatusTransIn);
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
        if (Tipe == 0 || Tipe == 1 || Tipe == 2)
        {
            lokasifrom.Items.Clear();
            ListItem item = new ListItem("All", "0");
            lokasifrom.Items.Add(item);
            DLC.LocationList(lokasifrom, Tipe);
        }
        lokasifrom.Focus();
    }
    protected void FillLokasiTo(object sender, EventArgs e)
    {
        int TipeTo = Convert.ToInt32(tipelokasito.SelectedValue);
        if (TipeTo == 0 || TipeTo == 1 || TipeTo == 2)
        {
            lokasito.Items.Clear();
            ListItem item = new ListItem("All", "0");
            lokasito.Items.Add(item);
            DLC.LocationList(lokasito, TipeTo);
        }
        lokasito.Focus();
    }
}