using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using CrossBrand;
public partial class LDCrossBrand : System.Web.UI.Page
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
            tb.Visible = false;

            //Form-Default
            datepicker.Text = Str.Tgl(DateDrv.AwalBulan());
            datepicker2.Text = Str.Tgl(DateTime.Today);

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
                lokasi.Items.Clear();
                DLC.LocationList(lokasi, d.FirstOrDefault().TipeLokasi);

                tipelokasi.SelectedValue = d.FirstOrDefault().TipeLokasi.ToString();
                tipelokasi.Enabled = false;
                lokasi.SelectedValue = d.FirstOrDefault().IDLokasi.ToString();
                lokasi.Enabled = false;
            }

            var f = from p in db.DataSK
                    where p.IDUser == UserID
                    select p;
            if (f.Count() > 0)
            {
                lokasi.Items.Clear();
                DLC.LocationList(lokasi, d.FirstOrDefault().TipeLokasi);

                tipelokasi.SelectedValue = d.FirstOrDefault().TipeLokasi.ToString();
                tipelokasi.Enabled = false;
                lokasi.SelectedValue = d.FirstOrDefault().IDLokasi.ToString();
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

        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int Lokasi = Convert.ToInt32(lokasi.SelectedValue);
        DateTime start = Convert.ToDateTime(datepicker.Text);
        DateTime end = Convert.ToDateTime(datepicker2.Text);
        string tglMulai = start.ToString("yyyy-MM-dd");
        string tglAkhir = end.AddDays(1).ToString("yyyy-MM-dd");        

        //WHERE
        string w = "1=1";
        //if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;

        var rs = cross.OutgoingLD(0,tglMulai,tglAkhir).Where(c => ((Tipe == -1 && c.TipeAsal >= 0) || c.TipeAsal == Tipe)
        && ((Lokasi == -1 && c.IDAsal > 0) || c.IDAsal == Lokasi)
        && ((vB == 0 && c.IDBrandAsal > 0) || c.IDBrandAsal == vB)
        && c.Tgl >= start && c.Tgl < end.AddDays(1) && c.StatusTransOut == 1);

        //SELECT

        int index = 0;
        if (rs.Count() == 0) { App.TableNoResult(tb); }
        else
        {
            foreach (var item in rs)
            {

                if (!Response.IsClientConnected) break;

                int vF = item.IDAsal; // id from
                int vT = item.IDTujuan; // id to
                int tF = item.TipeAsal; // value from
                int tT = item.TipeTujuan; // value to
                int bF = item.IDBrandAsal;
                int bT = item.IDBrandTujuan;

                //Sub-Header
                //if (prev != r1.IDBrand)
                //{
                //    Sub_Header(vT, r1.ID);
                //    prev = r1.IDBrand;
                //}

                index++;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(item.ID);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.TipeAsal) + "<br>" + TD_Nama(tF, vF, bF) + "<br>" + TD_Addr(tF, vF, bF) + "<br>" + TD_Group(tF, vF, bF);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "&nbsp;&nbsp;&nbsp;&rarr;&nbsp;&nbsp;&nbsp;";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.LocationType(item.TipeTujuan) + "<br>" + TD_Nama(tT, vT, bT) + "<br>" + TD_Addr(tT, vT, bT) + "<br>" + TD_Group(tT, vT, bT);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(item.Keterangan);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = item.CrossBrand_StockOutgoingLD_Product.Count.ToString();
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
    protected string TD_HyperLink(int ID)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"LDCrossBrand_Approve.aspx?id=" + ID + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(int t, int r, int b)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = cross.Warehouses(b, r).SingleOrDefault();
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 1)
        {
            var rs = cross.Outlets(b, r).SingleOrDefault();
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }
        else if (t == 2)
        {
            var rs = cross.Exhibitions(b, r).SingleOrDefault();
            x += "<p><strong>" + rs.Nama + "</strong></p>";
        }

        return x;
    }
    protected string TD_Addr(int t, int r, int b)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = cross.Warehouses(b, r).SingleOrDefault();
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 1)
        {
            var rs = cross.Outlets(b, r).SingleOrDefault();
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }
        else if (t == 2)
        {
            var rs = cross.Exhibitions(b, r).SingleOrDefault();
            if (rs.AddrAlamat != "" && rs.AddrAlamat != null)
                x += "<p class=\"text-sm\">" + Str.HtmlBreak(rs.AddrAlamat) + "</p>";
        }

        return x;
    }
    protected string TD_Group(int t, int r, int b)
    {
        string x = string.Empty;

        if (t == 0)
        {
            var rs = cross.Warehouses(b, r).SingleOrDefault();
            x += "<p class=\"text-info\">" + rs.LocGroup + "</p>";
        }
        else if (t == 1)
        {
            var rs = cross.Outlets(b, r).SingleOrDefault();
            x += "<p class=\"text-info\">" + rs.LocGroup + "</p>";
        }
        else if (t == 2)
        {
            var rs = cross.Exhibitions(b, r).SingleOrDefault();
            x += "<p class=\"text-info\">" + rs.LocGroup + "</p>";
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