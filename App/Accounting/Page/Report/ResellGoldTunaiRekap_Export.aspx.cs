using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Report_ResellGoldTunaiRekap_Export : System.Web.UI.Page
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
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=ResellGoldTunaiRekap_Export.xls");
            fillTable(sender, e);
		}
	}

    public class ResellLE
    {
        public int ID { get; set; }
    }

    public class ListResellLE
    {
        public List<ResellLE> ListID { get; set; }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DataTable dtle = Db.Rs("SELECT ID FROM Resell WHERE GroupResell = 'GT' ", Db.Cnn_DB);
        ListResellLE lrle = new ListResellLE
        {
            ListID = new List<ResellLE>()
        };
        foreach (DataRow item in dtle.Rows)
        {
            ResellLE rle = new ResellLE
            {
                ID = Convert.ToInt32(item["ID"])
            };
            lrle.ListID.Add(rle);
        }
        dtle.Dispose();

        var rs = (from p in db.ResellDJ
                  where !(from z in db.TradeIn
                          where !z.Draft
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.StatusVoid == null
                          && (from b in lrle.ListID select b.ID).Contains(p.Resell.ID)
                  select new
                  {
                      Resell = p.Resell,
                      Tgl = p.SalesOrder.Tgl,
                      HargaBeli = (from z in db.SalesOrderDJ
                                   where z.IDProduct == p.IDProduct
                                   && z.SalesOrder.ID == p.SalesOrder.ID
                                   select z.TotalBayar).First(),
                      NoInvoice = (from z in db.SalesOrderDJ
                                   where z.IDProduct == p.IDProduct
                                   && z.SalesOrder.ID == p.SalesOrder.ID
                                   select z.Invoice).First(),
                      TipeLokasiB = p.SalesOrder.TipeLokasi,
                      IDLokasiB = p.SalesOrder.IDLokasi,
                      Keterangan = p.Resell.Keterangan,
                      Nomor = p.StockProductDJ.Nomor,
                      HargaRupiah = p.HargaRupiah,
                      Tipe = "Diamond Jewellery",
                      Item = p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                      HargaM = (p.StockProductDJ.StockProductDJ_Costing.TotalBiaya + p.StockProductDJ.StockProductDJ_CostingProduct.Total),
                      HargaR = p.StockProductDJ.StockProductDJ_PricingMU.ModalUSD,
                      ProductCategory = p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory,
                      ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.ProductItem,
                      Category = p.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.Nama,
                  }).Union
                  (from p in db.ResellPG
                   where !(from z in db.TradeIn
                           where !z.Draft
                           select z.IDResell).Contains(p.IDForm)
                           && p.Resell.StatusVoid == null
                           && (from b in lrle.ListID select b.ID).Contains(p.Resell.ID)
                   select new
                   {
                       Resell = p.Resell,
                       Tgl = p.SalesOrder.Tgl,
                       HargaBeli = (from z in db.SalesOrderPG
                                    where z.IDProduct == p.IDProduct
                                    && z.SalesOrder.ID == p.SalesOrder.ID
                                    select z.TotalBayar).First(),
                       NoInvoice = (from z in db.SalesOrderPG
                                    where z.IDProduct == p.IDProduct
                                    && z.SalesOrder.ID == p.SalesOrder.ID
                                    select z.Invoice).First(),
                       TipeLokasiB = p.SalesOrder.TipeLokasi,
                       IDLokasiB = p.SalesOrder.IDLokasi,
                       Keterangan = p.Resell.Keterangan,
                       Nomor = p.StockProductPG.Nomor,
                       HargaRupiah = p.HargaRupiah,
                       Tipe = "Plain Gold",
                       Item = p.StockProductPG.CharProductItem.Nama,
                       HargaM = (p.StockProductPG.TotalHargaUSD.Value),
                       HargaR = 0m,
                       ProductCategory = 4,
                       ProductItem = p.StockProductPG.ProductItem,
                       Category = "Plain Gold",

                   }).Union(
                  from p in db.ResellGJ
                  where !(from z in db.TradeIn
                          where !z.Draft
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.StatusVoid == null
                          && (from b in lrle.ListID select b.ID).Contains(p.Resell.ID)
                  select new
                  {
                      Resell = p.Resell,
                      Tgl = p.SalesOrder.Tgl,
                      HargaBeli = (from z in db.SalesOrderGJ
                                   where z.IDProduct == p.IDProduct
                                   && z.SalesOrder.ID == p.SalesOrder.ID
                                   select z.TotalBayar).First(),
                      NoInvoice = (from z in db.SalesOrderGJ
                                   where z.IDProduct == p.IDProduct
                                   && z.SalesOrder.ID == p.SalesOrder.ID
                                   select z.Invoice).First(),
                      TipeLokasiB = p.SalesOrder.TipeLokasi,
                      IDLokasiB = p.SalesOrder.IDLokasi,
                      Keterangan = p.Resell.Keterangan,
                      Nomor = p.StockProductGJ.Nomor,
                      HargaRupiah = p.HargaRupiah,
                      Tipe = "Gold Jewellery",
                      Item = p.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
                      HargaM = (p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total),
                      HargaR = p.StockProductGJ.StockProductGJ_PricingMU.ModalUSD,
                      ProductCategory = p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory,
                      ProductItem = p.StockProductGJ.StockProductGJ_CharProduct.ProductItem,
                      Category = p.StockProductGJ.StockProductGJ_CharProduct.CharProductCategory.Nama,
                  }).Union(
                  from p in db.ResellLD
                  where !(from z in db.TradeIn
                          where !z.Draft
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.StatusVoid == null
                          && (from b in lrle.ListID select b.ID).Contains(p.Resell.ID)
                  select new
                  {
                      Resell = p.Resell,
                      Tgl = p.SalesOrder.Tgl,
                      HargaBeli = (from z in db.SalesOrderLD
                                   where z.IDProduct == p.IDProduct
                                   && z.SalesOrder.ID == p.SalesOrder.ID
                                   select z.TotalBayar).First(),
                      NoInvoice = (from z in db.SalesOrderLD
                                   where z.IDProduct == p.IDProduct
                                   && z.SalesOrder.ID == p.SalesOrder.ID
                                   select z.Invoice).First(),
                      TipeLokasiB = p.SalesOrder.TipeLokasi,
                      IDLokasiB = p.SalesOrder.IDLokasi,
                      Keterangan = p.Resell.Keterangan,
                      Nomor = p.StockProductLD_Stone1B.Nomor,
                      HargaRupiah = p.HargaRupiah,
                      Tipe = "Loose Diamond",
                      Item = p.StockProductLD_Stone1B.CharProductItem.Nama,
                      HargaM = (p.StockProductLD_Stone1B.HargaTotalM.Value),
                      HargaR = 0m,
                      ProductCategory = 998,
                      ProductItem = p.StockProductLD_Stone1B.ProductItem.Value,
                      Category = "Loose Diamond",
                  });

        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);

        if (ProductItem > 0)
            rs = rs.Where(p => p.ProductItem == ProductItem);
        if (ProductCategory > 0)
            rs = rs.Where(p => p.ProductCategory == ProductCategory);

        string Status = "";
        Status = Request.QueryString["status"];
        if (Status == "1")
            rs = rs.Where(p => p.Resell.Draft == false);
        else if (Status == "2")
            rs = rs.Where(p => p.Resell.Draft == true);

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.Resell.IDLokasi == c.IDLokasi && p.Resell.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.Resell.IDLokasi) && p.Resell.TipeLokasi == 1);
        }

        rs = rs.OrderByDescending(p => p.Resell.Tgl);

        string keyword = Request.QueryString["kw"];
        if (keyword != "") rs = rs.Where(p => p.Resell.Nomor.Contains(keyword) || p.Resell.CustomerNama.Contains(Request.QueryString["kw"]) || p.Nomor.Contains(Request.QueryString["kw"]));

        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl < Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal Total = 0m;
            decimal TotalQty = 0m;
            decimal TotalBeli = 0m;
            decimal TotalM = 0m;
            decimal TotalR = 0m;

            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.Tgl.Value);
                tr.Cells.Add(td);

                string LokasiBeli = "";
                if (r.TipeLokasiB == 1)
                    LokasiBeli = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasiB, Db.Cnn_DB);
                else if (r.TipeLokasiB == 2)
                    LokasiBeli = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasiB, Db.Cnn_DB);

                td = new TableCell();
                td.Text = LokasiBeli;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NoInvoice;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HargaBeli.Value.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(r.Keterangan);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.Resell.Tgl);
                tr.Cells.Add(td);

                string Lokasi = "";
                if (r.Resell.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.Resell.IDLokasi, Db.Cnn_DB);
                else if (r.Resell.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibirion WHERE ID=" + r.Resell.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Resell.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Resell.CustomerNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Tipe;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Item;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Category;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HargaRupiah.Value.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalQty++;
                Total += r.HargaRupiah.Value;
                TotalBeli += r.HargaBeli.Value;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 4;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalBeli.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>-</b>";
            td2.ColumnSpan = 9;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalQty.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(Total).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
}