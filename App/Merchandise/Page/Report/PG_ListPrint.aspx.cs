using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Report_PG_ListPrint : System.Web.UI.Page
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

        decimal TotalHarga = 0m;
        decimal TotalBerat = 0m;
        decimal TotalQty = 0m;
        decimal TotalWeight = 0m;
        decimal TotalOngkos = 0m;
        decimal TotalTotal = 0m;
        decimal Harga = 0m;
        decimal OngkosR = 0m;
        decimal Berat = 0m;
        decimal Qty = 0m;
        decimal KadarLogam = 0m;
        decimal KadarTukaranJual = 0m;
        decimal KadarTukaranBeli = 0m;
        decimal TotalBerat24 = 0m;
        decimal TotalHarga24 = 0m;
        decimal TtlHargaJual = 0m;

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
            if (Request.QueryString["kw"] != "")
                rs = rs.Where(p => p.Nomor.Contains(Request.QueryString["kw"]));
            if (Request.QueryString["productlevel"] != "" && Request.QueryString["productlevel"] != "0")
                rs = rs.Where(p => p.ProductLevel == Convert.ToInt32(Request.QueryString["productlevel"]));
            if (Request.QueryString["model"] != "" && Request.QueryString["model"] != "0")
                rs = rs.Where(p => p.Model == Convert.ToInt32(Request.QueryString["model"]));
            if (Request.QueryString["framecolor"] != "" && Request.QueryString["framecolor"] != "0")
                rs = rs.Where(p => p.FrameColor == Convert.ToInt32(Request.QueryString["framecolor"]));
            if (Request.QueryString["productitem"] != "" && Request.QueryString["productitem"] != "0")
                rs = rs.Where(p => p.ProductItem == Convert.ToInt32(Request.QueryString["productitem"]));
            if (Request.QueryString["brand"] != "" && Request.QueryString["brand"] != "0")
                rs = rs.Where(p => p.IDBrand == Convert.ToInt32(Request.QueryString["brand"]));

            int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
            if (Substorage > 0)
            {
                rs = rs.Where(p => p.StockActualPG.Substorage == Substorage);
            }

            int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
            if (Cabang > 0)
            {
                rs = rs.Where(p => p.StockActualPG.IDLokasi == Cabang && p.StockActualPG.TipeLokasi == tipe);
            }
            else
            {
                rs = rs.Where(p => p.StockActualPG.TipeLokasi == tipe);
            }

            string status, status2 = "";

            foreach (var rslt in rs)
            {
                //if (!Response.IsClientConnected) break;
                TableRow tr = new TableRow();
                TableCell td;

                string Tanggal = Str.FullDate(Db.SingleTime("SELECT TOP 1 Tgl FROM StockProductPG WHERE NoTTB='" + rslt.NoTTB + "'", Db.Cnn_DB));

                Qty = Convert.ToDecimal((from p in db.StockProductPG
                                         where p.NoTTB == "" + rslt.NoTTB + ""
                                         select p).Count());

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + rslt.Nomor + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + Tanggal + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.SubStorage(rslt.StockActualPG.Substorage);
                tr.Cells.Add(td);

                string Lokasi = "";
                if (rslt.StockActualPG.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + rslt.StockActualPG.IDLokasi, Db.Cnn_DB);
                else if (rslt.StockActualPG.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + rslt.StockActualPG.IDLokasi, Db.Cnn_DB);
                else if (rslt.StockActualPG.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + rslt.StockActualPG.IDLokasi, Db.Cnn_DB);
                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.KodeBarang;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.CharGoldLevel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.CharGoldModel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.KadarLogam).ToString("N0") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt.CharFrameColor.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt.TotalHargaJual).ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalQty += 1;
                TotalWeight += Convert.ToDecimal(rslt.GrossWeight);
                TotalOngkos += Convert.ToDecimal(rslt.MountingMUSD);
                TotalHarga += Convert.ToDecimal(rslt.HargaUSD);
                TotalBerat += Convert.ToDecimal(rslt.NettoWeight);
                TotalTotal += (Convert.ToDecimal(rslt.HargaUSD) + Convert.ToDecimal(rslt.MountingMUSD));
                TtlHargaJual += Convert.ToDecimal(rslt.TotalHargaJual);
            }

            if (rs.Count() > 0)
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
                td.Text = "<b>" + TotalBerat.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TtlHargaJual.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }

    }
}