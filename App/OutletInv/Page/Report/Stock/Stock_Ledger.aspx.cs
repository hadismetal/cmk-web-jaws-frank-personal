using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        /// Master Brand
        var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocWarehouse
        var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocOutlet
        var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocExhibition
        var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);


        int Tipe = Convert.ToInt32(tipe.SelectedValue);

        if (Tipe == 0)
        {
            var rs = from p in db.StockLedgerDJ
                     join q in db.StockProductDJ on p.IDProduct equals q.ID
                     where q.Nomor.Contains(kw.Text)
                     orderby p.OperatorTgl descending
                     select new { p, q };
            if (rs != null)
            {

                foreach (var r in rs)
                {

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.q.Nomor;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDateJam(r.p.OperatorTgl);
                    td.Style.Add("text-align", "right");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.q.StockProductDJ_CharProduct.CharProductItem.Nama;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    string TipeLokasi = "";
                    if (r.p.TipeLokasi == 0 && r.p.IDLokasi == 0)
                    {
                        Lokasi = "Repair";
                        TipeLokasi = "Repair";
                    }
                    else if (r.p.TipeLokasi == 0)
                    {
                        Lokasi = r.p.IDLokasi < 0 ? r.p.NamaLokasi : rsWarehouse[r.p.IDLokasi];
                        TipeLokasi = "Warehouse";
                    }
                    else if (r.p.TipeLokasi == 1)
                    {
                        Lokasi = r.p.IDLokasi < 0 ? r.p.NamaLokasi : rsOutlet[r.p.IDLokasi];
                        TipeLokasi = "Outlet";
                    }
                    else
                    {
                        Lokasi = r.p.IDLokasi < 0 ? r.p.NamaLokasi : rsExhibition[r.p.IDLokasi];
                        TipeLokasi = "Exhibition";
                    }

                    td = new TableCell();
                    td.Text = TipeLokasi;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Lokasi;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.p.Value.ToString("N0");
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = DL.LedgerRemark(r.p.Remark);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.p.Keterangan;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                }
            }
        }
        else if (Tipe == 1)
        {
            var rs = from p in db.StockLedgerGJ
                     join q in db.StockProductGJ on p.IDProduct equals q.ID
                     where q.Nomor.Contains(kw.Text)
                     orderby p.OperatorTgl descending
                     select new { p, q };
            if (rs != null)
            {

                foreach (var r in rs)
                {

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.q.Nomor;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDateJam(r.p.OperatorTgl);
                    td.Style.Add("text-align", "right");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.q.StockProductGJ_CharProduct.CharProductItem.Nama;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    string TipeLokasi = "";
                    if (r.p.TipeLokasi == 0 && r.p.IDLokasi == 0)
                    {
                        Lokasi = "Repair";
                        TipeLokasi = "Repair";
                    }
                    else if (r.p.TipeLokasi == 0)
                    {
                        Lokasi = rsWarehouse[r.p.IDLokasi];
                        TipeLokasi = "Warehouse";
                    }
                    else if (r.p.TipeLokasi == 1)
                    {
                        Lokasi = rsOutlet[r.p.IDLokasi];
                        TipeLokasi = "Outlet";
                    }
                    else
                    {
                        Lokasi = rsExhibition[r.p.IDLokasi];
                        TipeLokasi = "Exhibition";
                    }

                    td = new TableCell();
                    td.Text = TipeLokasi;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Lokasi;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.p.Value.ToString("N0");
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = DL.LedgerRemark(r.p.Remark);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.p.Keterangan;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                }
            }
        }
        else if (Tipe == 2)
        {
            var rs = from p in db.StockLedgerLD
                     join q in db.StockProductLD_Stone1B on p.IDProduct equals q.ID
                     where q.Nomor.Contains(kw.Text)
                     orderby p.OperatorTgl descending
                     select new { p, q };
            if (rs != null)
            {

                foreach (var r in rs)
                {

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.q.Nomor;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDateJam(r.p.OperatorTgl);
                    td.Style.Add("text-align", "right");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "Loose Diamond";
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    string TipeLokasi = "";
                    if (r.p.TipeLokasi == 0 && r.p.IDLokasi == 0)
                    {
                        Lokasi = "Repair";
                        TipeLokasi = "Repair";
                    }
                    else if (r.p.TipeLokasi == 0)
                    {
                        Lokasi = r.p.IDLokasi < 0 ? r.p.NamaLokasi : rsWarehouse[r.p.IDLokasi];
                        TipeLokasi = "Warehouse";
                    }
                    else if (r.p.TipeLokasi == 1)
                    {
                        Lokasi = r.p.IDLokasi < 0 ? r.p.NamaLokasi : rsOutlet[r.p.IDLokasi];
                        TipeLokasi = "Outlet";
                    }
                    else
                    {
                        Lokasi = r.p.IDLokasi < 0 ? r.p.NamaLokasi : rsExhibition[r.p.IDLokasi];
                        TipeLokasi = "Exhibition";
                    }

                    td = new TableCell();
                    td.Text = TipeLokasi;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Lokasi;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.p.Value.ToString("N0");
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = DL.LedgerRemark(r.p.Remark);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.p.Keterangan;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                }
            }
        }
        else if (Tipe == 3)
        {
            var rs = from p in db.StockLedgerPG
                     join q in db.StockProductPG on p.IDProduct equals q.ID
                     where q.Nomor.Contains(kw.Text)
                     orderby p.OperatorTgl descending
                     select new { p, q };
            if (rs != null)
            {

                foreach (var r in rs)
                {

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = r.q.Nomor;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDateJam(r.p.OperatorTgl);
                    td.Style.Add("text-align", "right");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.q.CharProductItem.Nama;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    string Lokasi = "";
                    string TipeLokasi = "";
                    if (r.p.TipeLokasi == 0 && r.p.IDLokasi == 0)
                    {
                        Lokasi = "Repair";
                        TipeLokasi = "Repair";
                    }
                    else if (r.p.TipeLokasi == 0)
                    {
                        Lokasi = r.p.IDLokasi < 0 ? r.p.NamaLokasi : rsWarehouse[r.p.IDLokasi];
                        TipeLokasi = "Warehouse";
                    }
                    else if (r.p.TipeLokasi == 1)
                    {
                        Lokasi = r.p.IDLokasi < 0 ? r.p.NamaLokasi : rsOutlet[r.p.IDLokasi];
                        TipeLokasi = "Outlet";
                    }
                    else
                    {
                        Lokasi = r.p.IDLokasi < 0 ? r.p.NamaLokasi : rsExhibition[r.p.IDLokasi];
                        TipeLokasi = "Exhibition";
                    }

                    td = new TableCell();
                    td.Text = TipeLokasi;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Lokasi;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.p.Value.ToString("N0");
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = DL.LedgerRemark(r.p.Remark);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.p.Keterangan;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                }
            }
        }
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            tipe.SelectedValue = Request.QueryString["tipe"];
            kw.Text = Request.QueryString["kw"];

        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&tipe=" + tipe.Text;


        Response.Redirect(href);
    }


}