using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

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
		Ddl.Substorage(substorage);

        if (!IsPostBack)
        {

            //Generate
            DLC.CharProductItem(productitem);
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    public class IIDProduct
    {
        public int IDProduct { get; set; }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var queryExcludeReparasi = @"SELECT B.IDProduct FROM DocRepair A
        JOIN DocRepair_PLUSetting B ON A.ID = B.IDDocRepair
        JOIN DocRepairResult C ON C.IDWorkOrder = A.ID
        JOIN SalesOrderRepair D ON D.IDRepair = C.ID
        WHERE D.StatusPembayaran = 1 AND D.Draft = 0";

        List<IIDProduct> IdProducts = new List<IIDProduct>();
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(queryExcludeReparasi, connection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                var data = Helper.GetDataFromDataTable<IIDProduct>(dt);
                IdProducts = data;
            }
        }

        var excludeReparasiIds = IdProducts
                                    .Select(x => x.IDProduct)
                                    .ToList();

        var rs = (from p in db.StockProductDJ
                  where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
                  && !excludeReparasiIds.Contains(p.ID)
                            && !(from s in db.SalesOrderDJ
                                 where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                 select s.IDProduct).Contains(p.ID)
                            && !(from s in db.SalesOrderSouvenir
                                 where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
                                 select s.IDProductSouvenir).Contains(p.ID)
                  select new
                  {
                      InHand = p.StockActualDJ.InHand,
                      Nomor = p.Nomor,
                      Kodekaret = p.SupplierNomor,
                      Qty = 1,
                      Tipe = "Diamond Jewellery",
                      ApprovalTgl = p.ApprovalTgl,
                      TipeLokasi = p.StockActualDJ.TipeLokasi,
                      IDLokasi = p.StockActualDJ.IDLokasi,
                      Substorage = p.StockActualDJ.Substorage,
                      HargaM = Convert.ToDecimal((p.StockProductDJ_Costing.TotalRupiah + p.StockProductDJ_CostingProduct.Total)),
                      HargaR = Convert.ToDecimal((p.StockProductDJ_PricingMU.ModalUSD)),
                      HargaJual = p.StockProductDJ_PricingMU.HargaRupiahRound.Value,
                      GrossWeight = p.StockProductDJ_CharProduct.GrossWeight,
                      NetWeight = p.StockProductDJ_CharProduct.NettoWeight,
                      TotalButir = Convert.ToDecimal(p.StockProductDJ_CharProduct.StoneQty),
                      TotalCarat = Convert.ToDecimal(p.StockProductDJ_CharProduct.StoneCarat),
                      ProductItem = p.StockProductDJ_CharProduct.ProductItem,
                  }).Concat(
                  from p in db.StockProductGJ
                  where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
                            && !(from s in db.SalesOrderGJ
                                 where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                 select s.IDProduct).Contains(p.ID)
                            && !(from s in db.SalesOrderSouvenir
                                 where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 2
                                 select s.IDProductSouvenir).Contains(p.ID)
                  select new
                  {
                      InHand = p.StockActualGJ.InHand,
                      Nomor = p.Nomor,
                      Kodekaret = p.SupplierNomor,
                      Qty = 1,
                      Tipe = "Gold Jewellery",
                      ApprovalTgl = p.ApprovalTgl,
                      TipeLokasi = p.StockActualGJ.TipeLokasi,
                      IDLokasi = p.StockActualGJ.IDLokasi,
                      Substorage = p.StockActualGJ.Substorage,
                      HargaM = Convert.ToDecimal((p.StockProductGJ_Costing.TotalRupiah + p.StockProductGJ_CostingProduct.Total)),
                      HargaR = Convert.ToDecimal((p.StockProductGJ_PricingMU.ModalUSD)),
                      HargaJual = p.StockProductGJ_PricingMU.HargaRupiahRound.Value,
                      GrossWeight = p.StockProductGJ_CharProduct.GrossWeight,
                      NetWeight = p.StockProductGJ_CharProduct.NettoWeight,
                      TotalButir = 0m,
                      TotalCarat = 0m,
                      ProductItem = p.StockProductGJ_CharProduct.ProductItem,
                  }).Concat(
                  from p in db.StockProductLD_Stone1B
                  where p.ReturnProduct == false && !p.StockProductLD.Draft.Value //&& p.Legacy == null
                            && !(from s in db.SalesOrderLD
                                 where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                 select s.IDProduct).Contains(p.ID)
                            && !(from s in db.SalesOrderSouvenir
                                 where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 4
                                 select s.IDProductSouvenir).Contains(p.ID)
                  select new
                  {
                      InHand = p.StockActualLD_Stone1B.InHand,
                      Nomor = p.Nomor,
                      Kodekaret = "Loose Diamond",
                      Qty = 1,
                      Tipe = "Loose Diamond",
                      ApprovalTgl = p.StockProductLD.ApprovalTgl,
                      TipeLokasi = p.StockActualLD_Stone1B.TipeLokasi,
                      IDLokasi = p.StockActualLD_Stone1B.IDLokasi,
                      Substorage = p.StockActualLD_Stone1B.Substorage,
                      HargaM = Convert.ToDecimal(p.HargaTotalM.Value),
                      HargaR = Convert.ToDecimal(p.HargaTotal.Value),
                      HargaJual = 0m,
                      GrossWeight = 0m,
                      NetWeight = 0m,
                      TotalButir = 1m,
                      TotalCarat = Convert.ToDecimal(p.TotalCarat),
                      ProductItem = p.ProductItem.Value
                  }).Concat(
                  from p in db.StockProductPG
                  where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
                            && !(from s in db.SalesOrderPG
                                 where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                 select s.IDProduct).Contains(p.ID)
                            && !(from s in db.SalesOrderSouvenir
                                 where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 3
                                 select s.IDProductSouvenir).Contains(p.ID)
                  select new
                  {
                      InHand = p.StockActualPG.InHand,
                      Nomor = p.Nomor,
                      Kodekaret = p.KodeBarang,
                      Qty = 1,
                      Tipe = "Plain Gold",
                      ApprovalTgl = p.ApprovalTgl,
                      TipeLokasi = p.StockActualPG.TipeLokasi,
                      IDLokasi = p.StockActualPG.IDLokasi,
                      Substorage = p.StockActualPG.Substorage,
                      HargaM = Convert.ToDecimal(p.TotalHarga24.Value),
                      HargaR = 0m,
                      HargaJual = 0m,
                      GrossWeight = Convert.ToDecimal(p.GrossWeight.Value),
                      NetWeight = 0m,
                      TotalButir = 0m,
                      TotalCarat = 0m,
                      ProductItem = p.ProductItem
                  });

        int PI = Convert.ToInt32(productitem.SelectedValue);
        if (PI > 0)
            rs = rs.Where(p => p.ProductItem == PI);

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.TipeLokasi == d.First().TipeLokasi && p.IDLokasi == d.First().IDLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.IDLokasi) && p.TipeLokasi == 1);
        }

        //var rs = (from p in db.StockProductDJ
        //          where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
        //                    && !(from s in db.SalesOrderDJ
        //                         where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
        //                         select s.IDProduct).Contains(p.ID)
        //          select new
        //          {
        //              Qty = p.StockActualDJ.InHand,
        //              Tipe = "Diamond Jewellery",
        //              ApprovalTgl = p.ApprovalTgl,
        //              TipeLokasi = p.StockActualDJ.TipeLokasi,
        //              IDLokasi = p.StockActualDJ.IDLokasi,
        //              Substorage = p.StockActualDJ.Substorage,
        //          }).Union(
        //          from p in db.StockProductGJ
        //          where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
        //                    && !(from s in db.SalesOrderGJ
        //                         where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
        //                         select s.IDProduct).Contains(p.ID)
        //          select new
        //          {
        //              Qty = p.StockActualGJ.InHand,
        //              Tipe = "Gold Jewellery",
        //              ApprovalTgl = p.ApprovalTgl,
        //              TipeLokasi = p.StockActualGJ.TipeLokasi,
        //              IDLokasi = p.StockActualGJ.IDLokasi,
        //              Substorage = p.StockActualGJ.Substorage,
        //          });

        rs = rs.OrderByDescending(p => p.ApprovalTgl);

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        if (Substorage > 0)
        {
            rs = rs.Where(p => p.Substorage == Substorage);
        }

        if (rs != null)
        {
            int index = 1;
            int count = 1;

            decimal d_inhand = 0m;
            decimal d_hargajual = 0m;
            decimal d_totalbutir = 0m;
            decimal d_totalcarat = 0m;

            decimal TotalJual = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalQty = 0m;

            string prev = "";
            foreach (var r in rs)
            {

                //Sub-Header
                if (prev != r.TipeLokasi.ToString() + " " + r.IDLokasi.ToString())
                {
                    if (count > 1)
                    {
                        
                    }

                    Sub_Header(r.TipeLokasi, r.IDLokasi);
                    prev = r.TipeLokasi.ToString() + " " + r.IDLokasi.ToString();

                    index = 1;
                }

                count++;

                string ProductItem = Db.SingleString("SELECT Nama FROM CharProductItem WHERE ID=" + r.ProductItem, Db.Cnn_DB);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Kodekaret;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ProductItem;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Tipe;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.SubStorage(r.Substorage);
                tr.Cells.Add(td);

                d_inhand = (r.InHand != null) ? r.InHand : 0m;
                d_hargajual = (r.HargaJual != null) ? r.HargaJual : 0m;
                d_totalbutir = (r.TotalButir != null) ? r.TotalButir : 0m;
                d_totalcarat = (r.TotalCarat != null) ? r.TotalCarat : 0m;

                td = new TableCell();
                td.Text = d_inhand.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = d_hargajual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = d_totalbutir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = d_totalcarat.ToString("N3");
                tr.Cells.Add(td);
                
                tb.Rows.Add(tr);

                TotalQty += d_inhand;
                TotalJual += d_hargajual;
                TotalButir += d_totalbutir;
                TotalCarat += d_totalcarat;

                index++;

            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalJual.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalButir.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);

            TotalJual = 0m;
            TotalButir = 0m;
            TotalCarat = 0m;
            TotalQty = 0m;
        }

    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            substorage.SelectedValue = Request.QueryString["substorage"];
            productitem.SelectedValue = Request.QueryString["productitem"];
        }
    }

    protected void Sub_Header(int TipeLokasi, int IDLokasi)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        string Lokasi = "";
        if (TipeLokasi == 0)
            Lokasi += Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + IDLokasi, Db.Cnn_DB);
        else if (TipeLokasi == 1)
            Lokasi += Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + IDLokasi, Db.Cnn_DB);
        else if (TipeLokasi == 2)
            Lokasi += Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + IDLokasi, Db.Cnn_DB);

        x += "<br />";
        x += "<span class=\"text-orange\">";
        x += Lokasi;
        x += "</span>";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&substorage=" + substorage.SelectedValue;
        href += "&productitem=" + productitem.SelectedValue;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "ItemDetail_Export.aspx?id=1";

        //Customize-Qstr
        href += "&substorage=" + substorage.SelectedValue;
        href += "&productitem=" + productitem.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "ItemDetail_Print.aspx?id=1";

        //Customize-Qstr
        href += "&substorage=" + substorage.SelectedValue;
        href += "&productitem=" + productitem.SelectedValue;

        Response.Redirect(href);
    }
}