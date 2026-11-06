using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

            kw.Focus(); //Keyword

            //DLC
            DLC.CompanyBrand(brand);

            DLC.StockLocationList(cabang, tipe);


            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        var rs = from p in db.StockReceivePackaging_Product
                 where
                    (
                    p.Packaging.Nama.Contains(kw.Text) ||
                    p.Packaging.Kode.Contains(kw.Text) ||
                    p.Packaging.Keterangan.Contains(kw.Text)
                    )
                 select p;

        rs = rs.Where(p => p.StockReceivePackaging.Draft == false);
        rs = rs.OrderByDescending(p => p.Packaging.Tgl);

        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0)
            rs = rs.Where(p => p.StockReceivePackaging.IDWarehouse == Cabang);


        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.StockReceivePackaging.Tgl >= Convert.ToDateTime(dari.Text) && p.StockReceivePackaging.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        int Brand = Convert.ToInt32(brand.SelectedValue);
        if (Brand != null && Brand > 0)
            rs = rs.Where(p => p.Packaging.IDBrand == Brand);

        if (rs.Count() > 0)
        {
            int index = 1;
            decimal TotalHarga = 0;
            decimal TotalStock = 0;
            decimal TotalRusak = 0;
            decimal TotalHilang = 0;
            foreach (var r in rs)
            {


                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockReceivePackaging.NoDO;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockReceivePackaging.NoPO;
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.StockReceivePackaging.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Kode;
                tr.Cells.Add(td);

                string Lokasi = "";

                Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockReceivePackaging.IDWarehouse, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Packaging.Satuan;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Harga.Value.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Received.Value.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Damaged.Value.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NeverArrived.Value.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockReceivePackaging.Approval;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.FullDate(r.StockReceivePackaging.ApprovalTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockReceivePackaging.Keterangan;
                tr.Cells.Add(td);


                tb.Rows.Add(tr);

                index++;
                TotalHarga += r.Harga.Value;
                TotalStock += r.Received.Value;
                TotalRusak += r.Damaged.Value;
                TotalHilang += r.NeverArrived.Value;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 9;
            tr2.Cells.Add(td2);

            // td2 = new TableCell();
            // td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N2") + "</b>";
            // tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalStock).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalRusak).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHilang).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);


            tb.Rows.Add(tr2);
        }
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            cabang.SelectedValue = Request.QueryString["cabang"];
            kw.Text = Request.QueryString["kw"];
            brand.SelectedValue = Request.QueryString["brand"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&cabang=" + cabang.Text;
        href += "&kw=" + kw.Text;
        href += "&brand=" + brand.SelectedValue;
        href += "&tipe=" + tipe;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        Response.Redirect(href);
    }
}