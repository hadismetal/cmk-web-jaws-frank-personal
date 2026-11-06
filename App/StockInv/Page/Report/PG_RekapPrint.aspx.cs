using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Report_PG_RekapPrint : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            //Generate

            fillTable(sender, e);
            fillHeader(sender, e);
        }
    }

    //Display Header
    protected void fillHeader(object sender, EventArgs e)
    {
        string kw = Request.QueryString["kw"];
        int brand_val = Convert.ToInt32(Request.QueryString["brand"]);
        int productlevel_val = Convert.ToInt32(Request.QueryString["productlevel"]);
        int productitem_val = Convert.ToInt32(Request.QueryString["productitem"]);
        int framecolor_val = Convert.ToInt32(Request.QueryString["framecolor"]);
        int model_val = Convert.ToInt32(Request.QueryString["model"]);

        var rspi = from p in db.CharProductItem where !p.Disable orderby p.SN select p;
        var rspl = from p in db.CharGoldLevel where !p.Disable orderby p.SN select p;
        var rsfc = from p in db.CharFrameColor where !p.Disable orderby p.SN select p;
        var rsb = from p in db.CompanyBrand where !p.Disable orderby p.SN select p;
        var rsm = from p in db.CharGoldModel where !p.Disable orderby p.SN select p;

        var rspib = rspi.SingleOrDefault(p => p.ID == productitem_val);
        var rsplb = rspl.SingleOrDefault(p => p.ID == productlevel_val);
        var rsfcb = rsfc.SingleOrDefault(p => p.ID == framecolor_val);
        var rsbb = rsb.SingleOrDefault(p => p.ID == brand_val);
        var rsmb = rsm.SingleOrDefault(p => p.ID == model_val);

        report_datetime.InnerText = Str.FullDate(DateTime.Now);
        brand.InnerHtml = brand_val > 0 ? rsbb.Nama : "Semua";
        item.InnerText = productitem_val > 0 ? rspib.Nama : "Semua";
        level.InnerText = productlevel_val > 0 ? rsplb.Nama : "Semua";
        model.InnerText = model_val > 0 ? rsmb.Nama : "Semua";
        frame.InnerText = framecolor_val > 0 ? rsfcb.Nama : "Semua";
        keywords.InnerText = kw;
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        DateTime start;
        DateTime end;

        decimal Qty = 0m;

        var rs = from p in db.StockProductPG
                 where p.Draft == false && p.Approval != null && p.ReturnProduct == false /*&& p.StatusPenjualan == 0*/ && p.StatusLebur == false
                 && p.StockActualPG.IDLokasi > 0
                 && !(from s in db.SalesOrderPG
                      where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                      select s.IDProduct).Contains(p.ID)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 3
                                select s.IDProductSouvenir).Contains(p.ID)
                 select p;

        if (rs.Count() > 0)
        {

            string kw = Request.QueryString["kw"];
            int brand = Convert.ToInt32(Request.QueryString["brand"]);
            int productlevel = Convert.ToInt32(Request.QueryString["productlevel"]);
            int productitem = Convert.ToInt32(Request.QueryString["productitem"]);
            int framecolor = Convert.ToInt32(Request.QueryString["framecolor"]);
            int model = Convert.ToInt32(Request.QueryString["model"]);

            if (productlevel > 0)
                rs = rs.Where(p => p.ProductLevel == productlevel);
            if (model > 0)
                rs = rs.Where(p => p.Model == model);
            if (framecolor > 0)
                rs = rs.Where(p => p.FrameColor == framecolor);
            if (productitem > 0)
                rs = rs.Where(p => p.ProductItem == productitem);

            int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
            if (Substorage > 0)
            {
                rs = rs.Where(p => p.StockActualPG.Substorage == Substorage);
            }

            var pg = from p in rs
                     group p by p.NoTTB into g
                     select new
                     {
                         NoTTB = g.Key,
                         Qty = g.Count(),
                         ProductItem = g.Select(x => x.CharProductItem.Nama),
                         ProductLevel = g.Select(x => x.CharGoldLevel.Nama),
                         Model = g.Select(x => x.CharGoldModel.Nama),
                         KadarLogam = g.Select(x => x.KadarLogam),
                         FrameColor = g.Select(x => x.CharFrameColor.Nama),
                         Weight = g.Sum(x => x.GrossWeight),
                         Ongkos = g.Sum(x => x.MountingM),
                         TotalHarga = g.Sum(x => x.TotalHarga),
                         TotalBerat24 = g.Sum(x => x.NettoWeight),
                         TotalHarga24 = g.Sum(x => x.TotalHarga24),
                         Harga = g.Sum(x => x.Harga),
                         TotalHargaJual = g.Sum(x=>x.TotalHargaJual)
                     };

            string status, status2 = "";

            if (kw != "")
                pg = pg.Where(g => g.ProductItem.Contains(kw) || g.ProductLevel.Contains(kw) || g.Model.Contains(kw) || g.FrameColor.Contains(kw));

            decimal TotalQty = 0m;
            decimal TotalWeight = 0m;
            decimal TotalOngkos = 0m;
            decimal TotalHarga = 0m;
            decimal TotalBerat = 0m;
            decimal TotalTotal = 0m;
            decimal TtlHargaJual = 0m;

            foreach (var rslt in pg)
            {
                //if (!Response.IsClientConnected) break;
                TableRow tr = new TableRow();
                TableCell td;

                string Tanggal = Str.FullDate(Db.SingleTime("SELECT TOP 1 Tgl FROM StockProductPG WHERE NoTTB='" + rslt.NoTTB + "'", Db.Cnn_DB));

                Qty = Convert.ToDecimal((from p in db.StockProductPG
                                         where p.NoTTB == "" + rslt.NoTTB + ""
                                         select p).Count());

                td = new TableCell();
                td.Text = rslt.NoTTB;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Tanggal;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.ProductLevel.First();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.Model.First();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.KadarLogam.First()).ToString("N0") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.FrameColor.First();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.Qty.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.Weight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.Ongkos).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.Harga).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.TotalBerat24).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (Convert.ToDecimal(rslt.Harga) + Convert.ToDecimal(rslt.Ongkos)).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.TotalHargaJual).ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalQty += rslt.Qty;
                TotalWeight += Convert.ToDecimal(rslt.Weight);
                TotalOngkos += Convert.ToDecimal(rslt.Ongkos);
                TotalHarga += Convert.ToDecimal(rslt.Harga);
                TotalBerat += Convert.ToDecimal(rslt.TotalBerat24);
                TotalTotal += (Convert.ToDecimal(rslt.Harga) + Convert.ToDecimal(rslt.Ongkos));
                TtlHargaJual += Convert.ToDecimal(rslt.TotalHargaJual);
            }

            if (pg.Count() > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalWeight.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalOngkos.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHarga.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalBerat.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalTotal.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TtlHargaJual.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
        
    }
}