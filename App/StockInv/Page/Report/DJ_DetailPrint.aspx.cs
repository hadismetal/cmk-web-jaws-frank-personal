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

    public class IIDProduct
    {
        public int IDProduct { get; set; }
    }

    protected void fillHeader()
    {

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
                 where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value 
                 && p.StockActualDJ.IDLokasi > 0 && !excludeReparasiIds.Contains(p.ID)
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
        var rsN = from p in db.CompanyBrand select p;
        var rsF = from p in db.CharFrameMaterial select p;

        if (Convert.ToInt32(Request.QueryString["cabang"]) > 0) { var v_cab = rsB.FirstOrDefault(p => p.IDLokasi == Convert.ToInt32(Request.QueryString["cabang"]) && p.TipeLokasi == Convert.ToInt32(Request.QueryString["tipe"])); cabang.InnerText = v_cab.NamaLokasi; } else { cabang.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productcategory"]) > 0) { var v_cat = rsC.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productcategory"])); category.InnerText = v_cat.Nama; } else { category.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productitem"]) > 0) { var v_itm = rsI.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productitem"])); item.InnerText = v_itm.Nama; } else { item.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["productlevel"]) > 0) { var v_lvl = rsL.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["productlevel"])); level.InnerText = v_lvl.Nama; } else { level.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["brand"]) > 0) { var v_brand = rsN.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["brand"])); brand.InnerText = v_brand.Nama; } else { brand.InnerText = "Semua"; }
        if (Convert.ToInt32(Request.QueryString["framematerial"]) > 0) { var v_framematerial = rsF.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["framematerial"])); framematerial.InnerText = v_framematerial.Nama; } else { framematerial.InnerText = "Semua"; }


        // if (Convert.ToInt32(Request.QueryString["cabang"]) > 0) { var v_cab = rs.SingleOrDefault(p => p.ID == Convert.ToInt32(Request.QueryString["category"])); cabang.InnerText = v_cab.Nama; } else { cabang.InnerText = "Semua"; }

        report_datetime.InnerText = Str.TglJam(System.DateTime.Now);

    }



    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //href += "&kw=" + kw.Text;
        //href += "&cabang=" + cabang.Text;
        //href += "&productitem=" + productitem.Text;
        //href += "&productcategory=" + productcategory.Text;
        //href += "&productlevel=" + productlevel.Text;
        //href += "&tipe=" + tipe;
        //href += "&orderby=" + orderby.SelectedValue;
        //href += "&substorage=" + substorage.SelectedValue;
        //href += "&brand=" + brand.Text;

        string s_kw = Request.QueryString["kw"];
        string s_cabang = Request.QueryString["cabang"];
        string s_productitem = Request.QueryString["productitem"];
        string s_productcategory = Request.QueryString["productcategory"];
        string s_productlevel = Request.QueryString["productlevel"];
        string s_tipe = Request.QueryString["tipe"];
        string s_orderby = Request.QueryString["orderby"];
        string s_substorage = Request.QueryString["substorage"];
        string s_stonedist = Request.QueryString["stonedist"];
        string s_brand = Request.QueryString["brand"];

        DateTime start;
        DateTime end;
        decimal netweight = 0m;
        decimal frameprice = 0m;
        decimal totalbutir = 0m;
        decimal totalcarat = 0m;
        decimal totalharga = 0m;
        decimal no = 0m;
        decimal ro = 0m;
        decimal finishing = 0m;
        decimal stonesetting = 0m;
        decimal hargausd = 0m;
        decimal hargarp = 0m;

        var rs = from p in db.StockProductDJ
                 where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value //&& p.Legacy == null
                           && !(from s in db.SalesOrderDJ
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                select s.IDProduct).Contains(p.ID)
                           && !(from s in db.SalesOrderSouvenir
                                where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 1
                                select s.IDProductSouvenir).Contains(p.ID)
                 select p;


        rs = rs.OrderByDescending(p => p.ApprovalTgl);

        /*if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end);
        }*/

        int Substorage = Convert.ToInt32(s_substorage);
        if (Substorage > 0)
        {
            rs = rs.Where(p => p.StockActualDJ.Substorage == Substorage);
        }

        if (s_kw != "")
        {
            rs = from p in rs
                 where p.Nomor.Contains(s_kw) || p.ProtoNomor.Contains(s_kw)
                 select p;
        }
        int Cabang = Convert.ToInt32(s_cabang);
        if (Cabang > 0) rs = rs.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
        else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);
        int ProductLevel = Convert.ToInt32(s_productlevel);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(s_productitem);
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(s_productcategory);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
        int DistribusiStone = Convert.ToInt32(s_stonedist);
        if (DistribusiStone > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.StoneDist == DistribusiStone);
        int Brand = Convert.ToInt32(s_brand);
        if (Brand > 0) rs = rs.Where(p => p.LocWarehouse.CompanyBrand.ID == Brand);

        if (s_orderby == "1")
            rs.OrderBy(p => p.StockActualDJ.NamaLokasi);
        else if (s_orderby == "2")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductLevel.Nama);
        else if (s_orderby == "3")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductCategory.Nama);
        else if (s_orderby == "4")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductItem.Nama);

        int pageNumber = 0;
        int numberOfObjectsPerPage = 30;

        decimal TotalPage = Convert.ToDecimal(rs.Count()) / Convert.ToDecimal(numberOfObjectsPerPage);

        TotalPage = Math.Ceiling(TotalPage);

        if (Request.QueryString["page"] != "")
            pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        if (pageNumber < 0)
            pageNumber = 0;

        rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

        int index = (pageNumber > 0) ? (pageNumber * 30) + 1 : 1;

        if (rs != null)
        {
            int Qty = 0;
            decimal TotalNetWeight = 0m;
            decimal TotalFramePriceM = 0m;
            decimal TotalFramePriceR = 0m;
            decimal TotalButirAll = 0m;
            decimal TotalKaratAll = 0m;
            decimal TotalHargaParcelMAll = 0m;
            decimal TotalHargaParcelRAll = 0m;
            decimal TotalNOMAll = 0m;
            decimal TotalNORAll = 0m;
            decimal TotalROMAll = 0m;
            decimal TotalRORAll = 0m;
            decimal TotalFinishingM = 0m;
            decimal TotalFinishingR = 0m;
            decimal TotalStoneSettingM = 0m;
            decimal TotalStoneSettingR = 0m;

            decimal TotalHargaM = 0m;
            decimal TotalHargaRp = 0m;
            decimal TotalHargaR = 0m;
            decimal TotalHargaAll = 0m;

            decimal TotalPromo = 0m;

            /// Master Brand
            var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Stone Distribution
            var rsStoneDist = (from p in db.CharStoneDist select p).ToDictionary(p => p.ID, p => p.Nama);


            int PrevID = 0;
            foreach (var r in rs)
            {
                decimal AllM = 0m;

                var x = r.StockProductDJ_CostingProduct;
                var z = r.StockProductDJ_Costing;

                AllM = (x.Total + z.TotalBiaya);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                /*td = new TableCell();
                td.Text = Str.Tgl(r.Tgl);
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = Str.Tgl(r.ApprovalTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HumanStockNama;
                tr.Cells.Add(td);

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
                td.Text = r.ProtoNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SupplierNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.CharProductCategory.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rsStoneDist[r.StockProductDJ_CharProduct.StoneDist];
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.NettoWeight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.GrossWeight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CostingProduct.TotalProduct.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_PricingProduct.TotalProduct.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_Costing.MountingNO).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_PricingBiaya.MountingNO.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_Costing.MountingRO.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_PricingBiaya.MountingRO.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_Finishing.Sum(y => y.BiayaModal)).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_Finishing.Sum(y => y.Biaya)).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_Costing.Setting).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_PricingBiaya.Setting).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = AllM.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                //td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah).ToString("N2") + " / " + Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N2");
                td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound.Value).ToString("N0");
                tr.Cells.Add(td);

                /// Promo 
                var pdj = (from p in db.PromoDJ_Detail
                           where p.IDProduct == r.ID && p.PromoDJ.TglStart < System.DateTime.Now && p.PromoDJ.Draft == false
                           orderby p.PromoDJ.ApprovalTgl descending
                           select p).FirstOrDefault();

                td = new TableCell();
                //td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah).ToString("N2") + " / " + Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N2");
                td.Text = Convert.ToDecimal((pdj != null) ? pdj.PromoDJ.PromoHarga : 0).ToString("N0");
                tr.Cells.Add(td);

                var stone = (from p in r.StockProductDJ_Stone1A
                             select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone1A.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                            from p in r.StockProductDJ_Stone1B
                            where p.StatusTerpasang == 1
                            select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone1B.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                            from p in r.StockProductDJ_Stone2
                            select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone2.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                            from p in r.StockProductDJ_Stone3
                            select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone3.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                            from p in r.StockProductDJ_Stone4
                            select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone4.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                            from p in r.StockProductDJ_Stone5
                            select new { HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone5.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir });

                decimal TotalButir = 0m;
                try
                {
                    TotalButir = stone.Sum(p => p.TotalButir);
                }
                catch (Exception ex) { }
                decimal TotalCarat = 0m;
                try
                {
                    TotalCarat = stone.Sum(p => p.TotalCarat);
                }
                catch (Exception ex) { }

                decimal TotalHargaParcelM = 0m;
                decimal TotalHargaParcelR = 0m;
                try
                {
                    TotalHargaParcelM = stone.Sum(p => p.TotalHargaM.Value);
                    TotalHargaParcelR = stone.Sum(p => p.TotalHarga.Value);
                }
                catch (Exception ex) { }

                if (stone.Count() > 0)
                {
                    Qty++;
                    int counter = 1;
                    foreach (var rstone in stone)
                    {
                        if (counter > 1)
                        {
                            tr = new TableRow();

                            td = new TableCell();
                            td.Text = "";
                            td.ColumnSpan = 26;
                            tr.Cells.Add(td);
                        }
                        else
                        {
                            index++;
                        }

                        td = new TableCell();
                        td.Text = rstone.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rstone.TotalButir.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rstone.TotalCarat.ToString("N3");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(rstone.TotalHargaM).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(rstone.TotalHarga).ToString("N2");
                        tr.Cells.Add(td);

                        //if (stone.Count() > 1)
                        tb.Rows.Add(tr);

                        counter++;
                    }
                }
                else
                {
                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);
                }

                decimal Jual = 0m;
                decimal Exchange = 0m;
                decimal Resell = 0m;

                if (r.StockActualDJ.Substorage == 1)
                    Jual = r.StockActualDJ.InHand;
                else if (r.StockActualDJ.Substorage == 3)
                    Exchange = r.StockActualDJ.InHand;
                else if (r.StockActualDJ.Substorage == 2)
                    Resell = r.StockActualDJ.InHand;


                TotalNetWeight += r.StockProductDJ_CharProduct.NettoWeight;
                TotalFramePriceM += r.StockProductDJ_CostingProduct.TotalProduct;
                TotalFramePriceR += r.StockProductDJ_PricingProduct.TotalProduct;
                TotalButirAll += TotalButir;
                TotalKaratAll += TotalCarat;
                TotalHargaParcelMAll += TotalHargaParcelM;
                TotalHargaParcelRAll += TotalHargaParcelR;
                TotalNOMAll += r.StockProductDJ_Costing.MountingNO.Value;
                TotalNORAll += r.StockProductDJ_PricingBiaya.MountingNO;
                TotalROMAll += r.StockProductDJ_Costing.MountingRO;
                TotalRORAll += r.StockProductDJ_PricingBiaya.MountingRO;
                try
                {
                    TotalFinishingM += r.StockProductDJ_Finishing.Sum(y => y.BiayaModal.Value);
                }
                catch (Exception ex) { }
                try
                {
                    TotalFinishingR += r.StockProductDJ_Finishing.Sum(y => y.Biaya.Value);
                }
                catch (Exception ex) { }

                TotalStoneSettingM += r.StockProductDJ_Costing.Setting;
                TotalStoneSettingR += r.StockProductDJ_PricingBiaya.Setting;
                TotalHargaM += (AllM);
                TotalHargaRp += (r.StockProductDJ_PricingMU.HargaRupiahRound.Value);
                TotalHargaR += r.StockProductDJ_PricingMU.ModalUSD;
                TotalHargaAll += r.StockProductDJ_PricingMU.ModalRupiah;

                TotalPromo += Convert.ToDecimal((pdj != null) ? pdj.PromoDJ.PromoHarga : 0);

                tb.Rows.Add(tr);
            }

            if (rs.Count() > 0)
            {
                if (Substorage != 97 && Substorage != 98 && Substorage != 99)
                    Qty = Convert.ToInt32(rs.Sum(p => p.StockActualDJ.InHand));
                else
                    Qty = rs.Count();
                //Qty = Convert.ToInt32(rs.Sum(p=>p.StockActualDJ.InHand));

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                td.ColumnSpan = 11;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + Qty.ToString() + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalNetWeight.ToString("0.00") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalFramePriceM.ToString("0.00") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalFramePriceR.ToString("0.00") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalNOMAll.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalNORAll.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalROMAll.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalRORAll.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalFinishingM.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalFinishingR.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalStoneSettingM.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalStoneSettingR.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHargaM.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHargaR.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHargaRp.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalPromo.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>-</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalButirAll.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalKaratAll.ToString("N3") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHargaParcelMAll.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHargaParcelRAll.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }


    }



}