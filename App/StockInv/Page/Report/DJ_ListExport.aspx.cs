using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

public partial class Page_DJ : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
        App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
            //Generate

            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=DJ_List_ExportExcel.xls");

            fillTable(sender, e);
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

        rs = rs.OrderByDescending(p => p.ApprovalTgl);

        /*string datepicker = Request.QueryString["dari"];
        string datepicker2 = Request.QueryString["sampai"];

        if (datepicker != "" && datepicker2 != "")
        {
            start = Convert.ToDateTime(datepicker);
            end = Convert.ToDateTime(datepicker2);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end);
        }*/

        int Substorage = Convert.ToInt32(Request.QueryString["substorage"]);
        if (Substorage > 0)
        {
            rs = rs.Where(p => p.StockActualDJ.Substorage == Substorage);
        }

        int Cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        string kw = Request.QueryString["kw"];
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        int ProductSegmen = Convert.ToInt32(Request.QueryString["productsegmen"]);
        int Brand = Convert.ToInt32(Request.QueryString["brand"]);
        string orderby = Request.QueryString["orderby"];

        if (kw!= "")
        {
            rs = from p in rs
                 where p.Nomor.Contains(kw) || p.ProtoNomor.Contains(kw)
                 select p;
        }
        
        if (Cabang > 0) rs = rs.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
        else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
        
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
        
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
        if (ProductSegmen > 0) rs = rs.Where(p => p.StockProductDJ_PricingSegmen.ProductSegmen == ProductSegmen);
        if (Brand > 0) rs = rs.Where(p => p.LocWarehouse.CompanyBrand.ID == Brand);

        if(orderby=="1")
            rs.OrderBy(p=>p.LocWarehouse.Nama);
        else if (orderby == "2")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductLevel.Nama);
        else if (orderby == "3")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductCategory.Nama);
        else if (orderby == "4")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductItem.Nama);


        /// Master Brand
        var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocWarehouse
        var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocOutlet
        var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocExhibition
        var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);


        if (rs != null)
        {
            int index = 1;
            decimal TotalStock = 0m;
            decimal TotalResell = 0m;
            decimal TotalExchange = 0m;
            decimal TotalHarga = 0m;
            decimal TotalModal = 0m;
            decimal TotalMAwal = 0m;
            decimal TotalM = 0m;
            decimal TotalR = 0m;
            decimal TotalHJual = 0m;
            decimal TotalNetWeight = 0m;
            decimal TotalGrossWeight = 0m;
            decimal TotalButirAll = 0m;
            decimal TotalCaratAll = 0m;
            decimal TotalBunga = 0m;

            decimal TotalPromo = 0m;

            foreach (var r in rs)
            {
                decimal AllM = 0m;

                var x = r.StockProductDJ_CostingProduct;
                var z = r.StockProductDJ_Costing;

                AllM = (x.Total + z.TotalBiaya);

                decimal AllMAwal = AllM;
                try
                {
                    var d = db.StockReceiveDJ.SingleOrDefault(p => p.Nomor.Equals(r.Nomor));
                    var s = d.StockReceiveDJ_CostingProduct;
                    var t = d.StockReceiveDJ_Costing;
                    AllMAwal = (s.Total + t.TotalBiaya);
                }
                catch { }

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                string KodeKaret;
                KodeKaret = (r.ProtoNomor != null) ? r.ProtoNomor : "";

                td = new TableCell();
                td.Text = "<small>" + KodeKaret + "</small>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.CharProductCategory.Nama;
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
                //if (r.StockActualDJ.TipeLokasi == 0)
                //    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
                //else if (r.StockActualDJ.TipeLokasi == 1)
                //    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
                //else if (r.StockActualDJ.TipeLokasi == 2)
                //    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.StockActualDJ.IDLokasi, Db.Cnn_DB);
                if (r.StockActualDJ.TipeLokasi == 0)
                    Lokasi = rsWarehouse[r.StockActualDJ.IDLokasi];
                else if (r.StockActualDJ.TipeLokasi == 1)
                    Lokasi = rsOutlet[r.StockActualDJ.IDLokasi];
                else if (r.StockActualDJ.TipeLokasi == 2)
                    Lokasi = rsExhibition[r.StockActualDJ.IDLokasi];
                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(AllMAwal).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(AllM).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound.Value).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                var det = (from p in r.StockProductDJ_Stone1A
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in r.StockProductDJ_Stone1B
                           where p.StatusTerpasang == 1
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in r.StockProductDJ_Stone2
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in r.StockProductDJ_Stone3
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in r.StockProductDJ_Stone4
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in r.StockProductDJ_Stone5
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat });

                decimal TotalButir = 0m;
                decimal umur = 0m;
                decimal bunga = 0m;
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

                //umur
                try
                {
                    if (tipe == 0)
                        umur += Convert.ToDecimal((DateTime.Today - r.Tgl.Value).TotalDays);
                    else
                    {
                        DateTime tglmasuk;
                        var rslt = from p in db.StockIncomingDJ_Product
                                   where p.IDProduct == r.ID
                                   && p.StockIncomingDJ.StatusTransIn == 2
                                   orderby p.ApprovalTgl descending
                                   select p;
                        if (rslt != null)
                        {
                            var t = rslt.FirstOrDefault();
                            if (t != null)
                            {
                                tglmasuk = t.ApprovalTgl.Value;
                                umur += Convert.ToDecimal((DateTime.Today - tglmasuk.Date).TotalDays);
                            }
                        }
                    }
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = Convert.ToString(umur); //(DateTime.Today - r.Tgl.Value).TotalDays.ToString();                
                tr.Cells.Add(td);

                var bg = from p in db.BungaBank
                         select p;
                //bunga
                bunga += Convert.ToDecimal(bg.First().Persen / 100 * umur / 360 * r.StockProductDJ_PricingMU.ModalRupiah);

                td = new TableCell();
                td.Text = Str.Number2(bg.First().Persen / 100 * umur / 360 * r.StockProductDJ_PricingMU.ModalRupiah);
                tr.Cells.Add(td);


                /// Promo 
                var pdj = (from p in db.PromoDJ_Detail
                           where p.IDProduct == r.ID && p.PromoDJ.TglStart < System.DateTime.Now
                           orderby p.PromoDJ.ApprovalTgl ascending
                           select p).FirstOrDefault();

                td = new TableCell();
                //td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah).ToString("N2") + " / " + Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N2");
                td.Text = Convert.ToDecimal((pdj != null) ? pdj.PromoDJ.PromoHarga : 0).ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalStock += Jual;
                TotalResell += Resell;
                TotalExchange += Exchange;
                TotalHarga += Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah);
                TotalModal += Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah);
                TotalMAwal += AllMAwal;
                TotalM += AllM;
                TotalR += r.StockProductDJ_PricingMU.ModalUSD;
                TotalHJual += r.StockProductDJ_PricingMU.HargaRupiahRound.Value;
                TotalNetWeight += r.StockProductDJ_CharProduct.NettoWeight;
                TotalGrossWeight += r.StockProductDJ_CharProduct.GrossWeight;
                TotalButirAll += TotalButir;
                TotalCaratAll += TotalCarat;
                TotalBunga += bunga;

                TotalPromo += Convert.ToDecimal((pdj != null) ? pdj.PromoDJ.PromoHarga : 0);
                index++;

            }

            /// Totalan
            if (rs.Count() > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                td.ColumnSpan = 5;
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
                td.Text = "<b>" + TotalMAwal.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalM.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalR.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHJual.ToString("N0") + "</b>";
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
                td.Text = "<b>" + Str.Number2(TotalBunga);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Str.Number0(TotalPromo);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }


    }
}