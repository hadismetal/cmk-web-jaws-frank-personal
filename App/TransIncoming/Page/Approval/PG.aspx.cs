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

            //DLC
            DLC.CompanyBrand(brand);

            int Tipe = 0;
            try
            {
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
            }
            catch (Exception ex) { }

            DLC.LocationList(lokasi, Tipe);

            int UserID = App.UserID;
            var d = from p in db.DataAdminSales
                    where p.IDUser == UserID
                    select p;
            if (d.Count() > 0)
            {
                tipelokasi.Items.Clear();
                tipelokasi.Enabled = false;
                lokasi.Items.Clear();
                lokasi.Enabled = false;
            }

            var f = from p in db.DataSK
                    where p.IDUser == UserID
                    select p;
            if (f.Count() > 0)
            {
                tipelokasi.Items.Clear();
                tipelokasi.Enabled = false;
                lokasi.Items.Clear();
                lokasi.Enabled = false;
            }

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
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            //DLC.CompanyBrand(brand);
            //DLC.LocationType(ddl_from, ddl_to);

            brand.SelectedValue = Request.QueryString["brand"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            lokasi.SelectedValue = Request.QueryString["lokasi"];
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        tb.Visible = true;

        //GET DDL BRAND FROM-TO TYPE VALUE
        int vB = Convert.ToInt32(brand.SelectedValue);
        
        //WHERE
        string w = "1=1";
        //if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;

        //SELECT
        var rs = (from p in db.StockIncomingPG
                  where ((vB > 0) ? p.StockOutgoingPG.IDBrand == vB : 1 == 1) && p.StatusTransIn == 1 // 1 : New
                  select p);

        try
        {
            int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
            int Lokasi = Convert.ToInt32(lokasi.SelectedValue);

            if (Tipe == 1 || Tipe == 2 || Tipe == 0)
            {

                rs = rs.Where(p => p.StockOutgoingPG.IDTujuan == Lokasi && p.StockOutgoingPG.TipeTujuan == Tipe);
            }
        }
        catch (Exception ex) { }

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            /*rs = from p in rs
                 where (p.StockOutgoingPackaging.TipeAsal == d.First().TipeLokasi && p.StockOutgoingPackaging.IDAsal == d.First().IDLokasi)
                 || (p.StockOutgoingPackaging.TipeTujuan == d.First().TipeLokasi && p.StockOutgoingPackaging.IDTujuan == d.First().IDLokasi)
                 select p;*/

            rs = from p in rs
                 where (p.StockOutgoingPG.TipeTujuan == d.First().TipeLokasi && d.Any( q => q.IDLokasi == p.StockOutgoingPG.IDTujuan))
                 select p;
        }

        var f = from p in db.DataSK
                where p.IDUser == UserID
                select p;
        if (f.Count() > 0)
        {

            rs = from p in rs
                 where (p.StockOutgoingPG.TipeTujuan == f.First().TipeLokasi && p.StockOutgoingPG.IDTujuan == f.First().IDLokasi)
                 select p;
        }

        rs = rs.OrderBy(p => p.OperatorTgl);

        DateTime start;
        DateTime end;

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.OperatorTgl >= start && p.OperatorTgl < end.AddDays(1));
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
            foreach (var item in rs)
            {
                var rsI = from p in db.StockIncomingPG_Product where p.IDForm == item.ID select p;

                if (!Response.IsClientConnected) break;

                vF = item.StockOutgoingPG.IDAsal; // id from
                vT = item.StockOutgoingPG.IDTujuan; // id to
                tF = item.StockOutgoingPG.TipeAsal; // value from
                tT = item.StockOutgoingPG.TipeTujuan; // value to

                //Sub-Header
                //if (prev != r1.IDBrand)
                //{
                //    Sub_Header(vT, r1.ID);
                //    prev = r1.IDBrand;
                //}

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(item.ID);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++ + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + item.StockOutgoingPG.Nomor + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.StockOutgoingPG.TipeAsal) + "<br>" + TD_Nama(tF, vF) + "<br>" + TD_Addr(tF, vF) + "<br>" + TD_Group(tF, vF);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "&nbsp;&nbsp;&nbsp;&rarr;&nbsp;&nbsp;&nbsp;";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.StockOutgoingPG.TipeTujuan) + "<br>" + TD_Nama(tT, vT) + "<br>" + TD_Addr(tT, vT) + "<br>" + TD_Group(tT, vT);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rsI.Count().ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(item.StockOutgoingPG.Keterangan);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.IncomingApprovalStatus(item.StatusTransIn);
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
    protected string TD_HyperLink(int ID)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"PG_Approve.aspx?id=" + ID + "\">";
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
    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2 || Tipe == 0)
        {
            lokasi.Items.Clear();
            DLC.LocationList(lokasi, Tipe);
        }
        lokasi.Focus();
    }
}