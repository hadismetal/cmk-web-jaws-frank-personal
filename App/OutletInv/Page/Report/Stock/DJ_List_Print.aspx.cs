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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected int idlokasi { get { return App.QueryInt(this, "idlokasi"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            fillTable(sender, e);
            fillHeader();
        }
    }
    protected void fillHeader()
    {

        var rs = from p in db.StockProductDJ
                 where p.ReturnProduct == false && p.StatusLebur == false
                           && !(from s in db.SalesOrderDJ
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.ID)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
                                select s.IDProductSouvenir).Contains(p.ID)
                 select p;
        var rsC = from p in db.CharProductCategory select p;
        var rsI = from p in db.CharProductItem select p;
        var rsL = from p in db.CharProductLevel select p;
        var rsB = from p in db.StockActualDJ select p;

        if (Convert.ToInt32(Request.QueryString["productcategory"]) > 0) { var v_cat = rsC.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productcategory"])); category.InnerText = v_cat.Nama; } else { category.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productitem"]) > 0) { var v_itm = rsI.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productitem"])); item.InnerText = v_itm.Nama; } else { item.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productlevel"]) > 0) { var v_lvl = rsL.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productlevel"])); level.InnerText = v_lvl.Nama; } else { level.InnerText = "Semua"; }


        // if (Convert.ToInt32(Request.QueryString["cabang"]) > 0) { var v_cab = rs.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["category"])); cabang.InnerText = v_cab.Nama; } else { cabang.InnerText = "Semua"; }

        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);

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

        // var rs = from p in db.StockProductDJ
        //          where p.ReturnProduct == false && p.StatusLebur == false
        //                    && !(from s in db.SalesOrderDJ
        //                         where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
        //                         select s.IDProduct).Contains(p.ID)
        //          select p;
        
        var rs = from p in db.StockProductDJ
                where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value && p.StockActualDJ.IDLokasi > 0  //&& p.Legacy == null
                        && p.StockActualDJ.IDLokasi > 0 && !excludeReparasiIds.Contains(p.ID)
                        && !(from s in db.SalesOrderDJ
                            where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                            select s.IDProduct).Contains(p.ID)
                        && !(from s in db.SalesOrderSouvenir
                            where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
                            select s.IDProductSouvenir).Contains(p.ID)
                select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.StockActualDJ.TipeLokasi == d.First().TipeLokasi && p.StockActualDJ.IDLokasi == d.First().IDLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.StockActualDJ.IDLokasi) && p.StockActualDJ.TipeLokasi == 1);
        }

        int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
        if (Substorage > 0)
        {
            rs = rs.Where(p => p.StockActualDJ.Substorage == Substorage);
        }

        rs = rs.OrderByDescending(p => p.ApprovalTgl);

        /*if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end);
        }*/

        string kw = Request.QueryString["kw"];
        if (Request.QueryString["kw"] != "")
            rs = rs.Where(p => p.Nomor.Contains(kw) || p.ProtoNomor.Contains(kw) || p.SupplierNomor.Contains(kw));
        else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);
        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);

        if (Request.QueryString["order"] == "1")
            rs.OrderBy(p => p.LocWarehouse.Nama);
        else if (Request.QueryString["order"] == "2")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductLevel.Nama);
        else if (Request.QueryString["order"] == "3")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductCategory.Nama);
        else if (Request.QueryString["order"] == "4")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            decimal TotalStock = 0m;
            decimal TotalResell = 0m;
            decimal TotalExchange = 0m;
            decimal TotalHarga = 0m;
            decimal TotalModal = 0m;
            decimal TotalM = 0m;
            decimal TotalR = 0m;
            decimal TotalNetWeight = 0m;
            decimal TotalGrossWeight = 0m;
            decimal TotalButirAll = 0m;
            decimal TotalCaratAll = 0m;
            foreach (var r in rs)
            {
                decimal AllM = 0m;

                var x = r.StockProductDJ_CostingProduct;
                var z = r.StockProductDJ_Costing;

                AllM = (x.Total + z.TotalBiaya);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                string KodeKaret;
                KodeKaret = (r.ProtoNomor != null) ? r.ProtoNomor : "";

                td = new TableCell();
                td.Text = "<small>" + KodeKaret + "</small>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SupplierNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;

                if (r.StockActualDJ.Substorage == 1)
                    Jual = r.StockActualDJ.InHand;
                else if (r.StockActualDJ.Substorage == 3)
                    Exchange = r.StockActualDJ.InHand;
                else if (r.StockActualDJ.Substorage == 2)
                    Resell = r.StockActualDJ.InHand;

                /*td = new TableCell();
                td.Text = Jual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Exchange.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0");
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = r.StockActualDJ.InHand.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = DL.SubStorage(r.StockActualDJ.Substorage);
                tr.Cells.Add(td);

                /*td = new TableCell();
                td.Text = r.StockActualDJ.InHand.ToString("N0");
                tr.Cells.Add(td);*/

                string Lokasi = "";
                if (r.StockActualDJ.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualDJ.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualDJ.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                var det = (from p in db.StockProductDJ_Stone1A
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in db.StockProductDJ_Stone1B
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in db.StockProductDJ_Stone2
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in db.StockProductDJ_Stone3
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in db.StockProductDJ_Stone4
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in db.StockProductDJ_Stone5
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat });

                decimal TotalButir = 0m;
                try
                {
                    TotalButir = det.Sum(p => p.TotalButir);
                }
                catch (Exception ex) { }
                decimal TotalCarat = 0m;
                try
                {
                    TotalCarat = det.Sum(p => p.TotalCarat);
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = TotalButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalCarat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.CharStoneDist.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound.Value).ToString("N0");
                tr.Cells.Add(td);

                decimal HargaPromo = 0m;

                var rslt = from p in db.PromoDJ_Detail
                           where p.PromoDJ.ApprovalNama != null && p.PromoDJ.TglStart.Value <= DateTime.Today
                           && p.PromoDJ.TglExpire.Value >= DateTime.Today && p.IDProduct.Value == r.ID
                           select p;
                if (rslt.Count() > 0)
                {
                    var disc = rslt.First();
                    if (disc.PromoDJ.Tipe == 1)
                    {
                        decimal DiscountHarga = 0m;
                        decimal DiscountPersen = 0m;
                        decimal Discount = 0m;
                        decimal TotalBayar = r.StockProductDJ_PricingMU.HargaRupiahRound.Value;

                        DiscountHarga = disc.PromoDJ.DiscountNominal.Value;
                        DiscountPersen = (TotalBayar * (disc.PromoDJ.DiscountPersen.Value / 100));
                        Discount = (DiscountHarga + DiscountPersen);
                        HargaPromo = TotalBayar - Discount;
                    }
                    if (disc.PromoDJ.Tipe == 3)
                    {
                        HargaPromo = disc.PromoDJ.PromoHarga.Value;

                    }
                }

                td = new TableCell();
                td.Text = (HargaPromo > 0) ? Convert.ToDecimal(HargaPromo).ToString("N0") : "-";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalStock += Jual;
                TotalResell += Resell;
                TotalExchange += Exchange;
                TotalHarga += Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah);
                TotalModal += Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah);
                TotalM += AllM;
                TotalR += r.StockProductDJ_PricingMU.ModalUSD;
                TotalNetWeight += r.StockProductDJ_CharProduct.NettoWeight;
                TotalGrossWeight = r.StockProductDJ_CharProduct.GrossWeight;
                TotalButirAll += TotalButir;
                TotalCaratAll += TotalCarat;

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
                td.Text = rs.Count().ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalButirAll.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
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

                tb.Rows.Add(tr);
            }
        }
    }
}