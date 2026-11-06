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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=DJ_Detail_ExportExcel.xls");
            //Generate
            fillTable(sender, e);
        }
    }
    //
    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        String Mode = "DJ"; // DJ, PG, GJ, LD
        String Tipe = "detail"; // rekap, detail, list
        String Modul = "receiving"; // receiving
        String Sign = ""; // 
        String Action = "excel"; // display, excel, print

        Table tbl_Table = tb;

        // utk display menggunakan control, utk export excel dan print menggunakan querystring
        String s_tF = ""; // tipelokasifrom.SelectedValue; // tipe lokasi from : W, O, E
        String s_kF = ""; // lokasifrom.SelectedValue; // id lokasi from
        String s_tT = ""; // tipelokasito.SelectedValue; // tipe lokasi to : W, O, E
        String s_kT = Request.QueryString["cabang"]; // cabang.SelectedValue; // id lokasi to
        String s_vB = Request.QueryString["brand"]; // brand.SelectedValue; // brand company
        String s_stats = ""; // status.SelectedValue; // status data
        String s_pf = Request.QueryString["dari"]; // datepicker.Text.Trim(); // period from
        String s_pt = Request.QueryString["sampai"]; // datepicker2.Text.Trim(); // period to
        String s_kw = Request.QueryString["kw"]; // kw.Text.Trim(); // keyword

        String s_ob = Request.QueryString["orderby"]; // orderby.SelectedValue; // orderby 
        String s_sup = Request.QueryString["supplier"]; // supplier.SelectedValue; // supplier 
        String s_sk = Request.QueryString["sk"]; // sk.SelectedValue; // stock keeper 
        String s_sup_group = Request.QueryString["suppliergroup"]; // suppliergroup.SelectedValue; // supplier group

        String s_plevel = Request.QueryString["productlevel"]; // productlevel.SelectedValue; // product level
        String s_pitem = Request.QueryString["productitem"]; // productitem.SelectedValue; // product item
        String s_pcategory = Request.QueryString["productcategory"]; // productcategory.SelectedValue; // product category

        System.Web.UI.Page page = this;
        System.Web.HttpRequest request = Request;
        System.Web.HttpResponse response = Response;

        /// Fill Table
        //Lib.ReportReceiving_FillTable(Mode, Tipe, Modul, Sign, Action, tbl_Table, s_tF, s_kF, s_tT, s_kT, s_vB, s_stats, s_pf, s_pt, s_kw, s_ob, s_sup, s_sk, s_plevel, s_pitem, s_pcategory, s_sup_group, page, request, response);

        String s_pmodel = "";
        String s_pframe = "";


        /// Master Brand
        var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocWarehouse
        var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocOutlet
        var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocExhibition
        var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

        tbl_Table.Visible = true;

        DateTime start;
        DateTime end;

        if (Mode == "DJ")
        {
            if (Tipe.ToLower() == "rekap")
            {

            }
            else if (Tipe.ToLower() == "detail")
            {
                var rs = from p in db.StockReceiveDJ
                         join q in db.DataSupplier on p.Supplier equals q.ID

                         where /*p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false &&*/ !p.Draft
                         //&& !(from s in db.SalesOrderDJ
                         //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                         //     select s.IDProduct).Contains(p.ID)
                         select new { p, q };

                rs = rs.OrderByDescending(p => p.p.ApprovalTgl);

                if (s_pf != "" && s_pt != "")
                {
                    start = Convert.ToDateTime(s_pf);
                    end = Convert.ToDateTime(s_pt);
                    rs = rs.Where(p => p.p.ApprovalTgl >= start && p.p.ApprovalTgl < end.AddDays(1));
                }

                if (s_kw != "")
                {
                    rs = from p in rs
                         where p.p.Nomor.Contains(s_kw) || p.p.ProtoNomor.Contains(s_kw) || p.p.SupplierNomor.Contains(s_kw)
                         select p;
                }
                int Cabang = Convert.ToInt32(s_kT);
                //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
                if (Cabang > 0) rs = rs.Where(p => p.p.IDWarehouse == Cabang);
                //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);

                int ProductLevel = Convert.ToInt32(s_plevel);
                if (ProductLevel > 0) rs = rs.Where(p => p.p.StockReceiveDJ_CharProduct.ProductLevel == ProductLevel);
                int ProductItem = Convert.ToInt32(s_pitem);
                if (ProductItem > 0) rs = rs.Where(p => p.p.StockReceiveDJ_CharProduct.ProductItem == ProductItem);
                int ProductCategory = Convert.ToInt32(s_pcategory);
                if (ProductCategory > 0) rs = rs.Where(p => p.p.StockReceiveDJ_CharProduct.ProductCategory == ProductCategory);
                if (s_sk != "0") rs = rs.Where(p => p.p.HumanStockNama == "" + s_sk + "");
                int Brand = Convert.ToInt32(s_vB);
                if (Brand > 0) rs = rs.Where(p => p.p.LocWarehouse.CompanyBrand.ID == Brand);

                int SupplierGroup = Convert.ToInt32(s_sup_group);
                int Supplier = Convert.ToInt32(s_sup);
                if (SupplierGroup > 0)
                {
                    if (Supplier > 0)
                    {
                        rs = rs.Where(p => p.p.Supplier == Supplier);
                    }
                    else
                    {
                        rs = rs.Where(p => p.q.IDGroup == SupplierGroup);
                    }
                }


                if (s_ob == "1")
                    rs.OrderBy(p => p.p.LocWarehouse.Nama);
                else if (s_ob == "2")
                    rs.OrderBy(p => p.p.StockReceiveDJ_CharProduct.CharProductLevel.Nama);
                else if (s_ob == "3")
                    rs.OrderBy(p => p.p.StockReceiveDJ_CharProduct.CharProductCategory.Nama);
                else if (s_ob == "4")
                    rs.OrderBy(p => p.p.StockReceiveDJ_CharProduct.CharProductItem.Nama);

                if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                else
                {
                    int Qty = 0;
                    decimal TotalNetWeight = 0m;
                    decimal TotalGrossWeight = 0m;
                    decimal TotalFramePriceM = 0m;
                    decimal TotalFramePriceR = 0m;
                    decimal TotalButirAll = 0m;
                    decimal TotalKaratAll = 0m;
                    decimal TotalHargaParcelMAll = 0m;
                    decimal TotalHargaParcelRAll = 0m;
                    decimal TotalNOMAll = 0m;
                    decimal TotalNOMLotAll = 0m;
                    decimal TotalNORAll = 0m;
                    decimal TotalROMAll = 0m;
                    decimal TotalRORAll = 0m;
                    decimal TotalFinishingM = 0m;
                    decimal TotalFinishingR = 0m;
                    decimal TotalStoneSettingM = 0m;
                    decimal TotalStoneSettingR = 0m;
                    decimal TotalOngkosLainM = 0m;
                    decimal TotalOngkosLainR = 0m;
                    decimal TotalUnitPrice = 0m;

                    decimal TotalHargaM = 0m;
                    decimal TotalHargaRp = 0m;
                    decimal TotalHargaR = 0m;
                    decimal TotalHargaAll = 0m;

                    /// header table
                    TableHeaderRow htr = new TableHeaderRow();
                    TableHeaderCell htd;

                    htd = new TableHeaderCell();
                    htd.Text = "No.";
                    htd.Width = 50;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "No. PLU";
                    htd.Width = 90;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Tanggal Approve";
                    htd.Width = 90;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Stock Keeper";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Kode Karet";
                    htd.Width = 140;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Kode Barang";
                    htd.Width = 140;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Supplier";
                    htd.Width = 180;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Stone Dist.";
                    htd.Width = 140;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Item";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Category";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Segmen";
                    htd.Width = 130;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Qty";
                    htd.Width = 90;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Net Weight";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Gross Weight";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Frame Price M";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Frame Price R";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "NO M";
                    htd.Width = 80;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "NO M Lot";
                    htd.Width = 80;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "NO R";
                    htd.Width = 80;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "RO M";
                    htd.Width = 80;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "RO R";
                    htd.Width = 80;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Finishing M";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Finishing R";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Ongkos Lain M";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Ongkos Lain R";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Stone Setting M";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Stone Setting R";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Total M <span class='text-muted'>USD</span>";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Total R <span class='text-muted'>USD</span>";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Mark Up <span class='text-muted'>%</span>";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "H. Jual <span class='text-muted'>Rp</span>";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Nama Supplier";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "No. Validasi";
                    htd.Width = 120;
                    htr.Cells.Add(htd);
					
					htd = new TableHeaderCell();
                    htd.Text = "Product Order Nomor";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Parcel";
                    htd.Width = 130;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "T. Butir";
                    htd.Width = 70;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "T. Carat";
                    htd.Width = 70;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "GIA";
                    htd.Width = 120;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Unit Price";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Stone M";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Stone R";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Keterangan";
                    htd.Width = 150;
                    htr.Cells.Add(htd);

                    tbl_Table.Rows.Add(htr);

                    int PrevID = 0;
                    int index = 0;

                    foreach (var r in rs)
                    {
                        if (!response.IsClientConnected) break;

                        decimal AllM = 0m;
                        index++;

                        var x = r.p.StockReceiveDJ_CostingProduct;
                        var z = r.p.StockReceiveDJ_Costing;

                        AllM = (x.Total + z.TotalBiaya);

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = index.ToString() + ".";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.Nomor;
                        tr.Cells.Add(td);

                        /*td = new TableCell();
                        td.Text = Str.Tgl(r.Tgl);
                        tr.Cells.Add(td);*/

                        td = new TableCell();
                        td.Text = Str.Tgl(r.p.ApprovalTgl);
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.HumanStockNama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.ProtoNomor;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.SupplierNomor;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.SupplierNama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_CharProduct.CharStoneDist.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_CharProduct.CharProductItem.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_CharProduct.CharProductCategory.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_PricingSegmen.CharProductSegmen.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "1";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_CharProduct.NettoWeight.ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_CharProduct.GrossWeight.ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_CostingProduct.TotalProduct.ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_PricingProduct.TotalProduct.ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.p.StockReceiveDJ_Costing.MountingNO).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.p.StockReceiveDJ_Costing.MountingNO * r.p.StockReceiveDJ_Costing.Pembagi.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_PricingBiaya.MountingNO.ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_Costing.MountingRO.Value.ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_PricingBiaya.MountingRO.ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.p.StockReceiveDJ_Finishing.Sum(y => y.BiayaModal)).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.p.StockReceiveDJ_Finishing.Sum(y => y.Biaya)).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_Costing.OngkosLainCOGS.Value.ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_Costing.OngkosLain.Value.ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.p.StockReceiveDJ_Costing.Setting).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.p.StockReceiveDJ_PricingBiaya.Setting).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = AllM.ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_PricingMU.ModalUSD.ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.StockReceiveDJ_PricingMU.MU.ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        //td.Text = Convert.ToDecimal(r.StockReceiveDJ_PricingMU.ModalRupiah).ToString("N2") + " / " + Convert.ToDecimal(r.StockReceiveDJ_PricingMU.HargaRupiahRound).ToString("N2");
                        td.Text = Convert.ToDecimal(r.p.StockReceiveDJ_PricingMU.HargaRupiahRound.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.p.SupplierNama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = (r.p.NoValidasi == null) ? "" : r.p.NoValidasi;
                        tr.Cells.Add(td);
						
						td = new TableCell();
                        td.Text = (r.p.ProductOrderNomor == null) ? "" : r.p.ProductOrderNomor;
                        tr.Cells.Add(td);

                        var stone = (from p in r.p.StockReceiveDJ_Stone1A
                                     select new { GIA = Db.SingleString("SELECT TOP 1 ISNULL(CertificateNumber,'') FROM StockReceiveDJ_Stone1A WHERE ID = "+p.ID.ToString()+" ", Db.Cnn_DB), HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone1A.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                                    from p in r.p.StockReceiveDJ_Stone1B
                                    where p.StatusTerpasang == 1
                                    select new
                                    {
                                        GIA = (p.GIA == null) ? "" : p.GIA
                                        ,
                                        HargaParcelM = (p.HargaSatuanM == null) ? 0 : p.HargaSatuanM
                                        ,
                                        TotalHargaM = (p.HargaTotalM == null) ? 0 : p.HargaTotalM
                                        ,
                                        HargaParcel = (p.HargaSatuan == null) ? 0 : p.HargaSatuan
                                        ,
                                        TotalHarga = (p.HargaTotal == null) ? 0 : p.HargaTotal
                                        ,
                                        Nama = (p.Stone1B == null) ? "***INVALID***" : p.Stone1B.Nama
                                        ,
                                        TotalCarat = (p.TotalCarat == null) ? 0 : p.TotalCarat
                                        ,
                                        TotalButir = (p.TotalButir == null) ? 0 : p.TotalButir
                                    }).Concat(
                                    from p in r.p.StockReceiveDJ_Stone2
                                    select new { GIA = "", HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone2.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                                    from p in r.p.StockReceiveDJ_Stone3
                                    select new { GIA = p.GIA, HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone3.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                                    from p in r.p.StockReceiveDJ_Stone4
                                    select new { GIA = "", HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone4.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir }).Concat(
                                    from p in r.p.StockReceiveDJ_Stone5
                                    select new { GIA = "", HargaParcelM = p.HargaSatuanM, TotalHargaM = p.HargaTotalM, HargaParcel = p.HargaSatuan, TotalHarga = p.HargaTotal, Nama = p.Stone5.Nama, TotalCarat = p.TotalCarat, TotalButir = p.TotalButir });

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
                                    td.Text = "";
                                    tr.Cells.Add(td);
                                }

                                /*td = new TableCell();
                                td.Text = rstone.Nama;
                                tr.Cells.Add(td);*/

                                td = new TableCell();
                                td.Text = rstone.Nama;
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = rstone.TotalButir.ToString("N0").Replace(".", "").Replace(",", ".");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = rstone.TotalCarat.ToString("N3").Replace(".", "").Replace(",", ".");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = "<a href=\"http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + rstone.GIA + "\" target=\"_blank\">" + rstone.GIA + "</a>";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = r.p.StockReceiveDJ_Costing.UnitPrice.HasValue ? r.p.StockReceiveDJ_Costing.UnitPrice.Value.ToString("N0").Replace(".", "").Replace(",", ".") : "0";
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Convert.ToDecimal(rstone.TotalHargaM).ToString("N2").Replace(".", "").Replace(",", ".");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = Convert.ToDecimal(rstone.TotalHarga).ToString("N2").Replace(".", "").Replace(",", ".");
                                tr.Cells.Add(td);

                                td = new TableCell();
                                td.Text = (counter == 1) ? r.p.Keterangan : "";
                                tr.Cells.Add(td);

                                if (stone.Count() > 1)
                                    tbl_Table.Rows.Add(tr);

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


                        TotalNetWeight += r.p.StockReceiveDJ_CharProduct.NettoWeight;
                        TotalGrossWeight += r.p.StockReceiveDJ_CharProduct.GrossWeight;
                        TotalFramePriceM += r.p.StockReceiveDJ_CostingProduct.TotalProduct;
                        TotalFramePriceR += r.p.StockReceiveDJ_PricingProduct.TotalProduct;
                        TotalButirAll += TotalButir;
                        TotalKaratAll += TotalCarat;
                        TotalHargaParcelMAll += TotalHargaParcelM;
                        TotalHargaParcelRAll += TotalHargaParcelR;
                        TotalNOMAll += r.p.StockReceiveDJ_Costing.MountingNO;
                        TotalNOMLotAll += r.p.StockReceiveDJ_Costing.MountingNO * r.p.StockReceiveDJ_Costing.Pembagi.Value;
                        TotalNORAll += r.p.StockReceiveDJ_PricingBiaya.MountingNO;
                        TotalROMAll += r.p.StockReceiveDJ_Costing.MountingRO.Value;
                        TotalRORAll += r.p.StockReceiveDJ_PricingBiaya.MountingRO;
                        TotalOngkosLainM += r.p.StockReceiveDJ_Costing.OngkosLainCOGS.Value;
                        TotalOngkosLainR += r.p.StockReceiveDJ_Costing.OngkosLain.Value;
                        TotalUnitPrice += r.p.StockReceiveDJ_Costing.UnitPrice.Value;
                        try
                        {
                            TotalFinishingM += r.p.StockReceiveDJ_Finishing.Sum(y => y.BiayaModal.Value);
                        }
                        catch (Exception ex) { }
                        try
                        {
                            TotalFinishingR += r.p.StockReceiveDJ_Finishing.Sum(y => y.Biaya.Value);
                        }
                        catch (Exception ex) { }

                        TotalStoneSettingM += r.p.StockReceiveDJ_Costing.Setting;
                        TotalStoneSettingR += r.p.StockReceiveDJ_PricingBiaya.Setting;
                        TotalHargaM += (AllM);
                        TotalHargaRp += (r.p.StockReceiveDJ_PricingMU.HargaRupiahRound.Value);
                        TotalHargaR += r.p.StockReceiveDJ_PricingMU.ModalUSD;
                        TotalHargaAll += r.p.StockReceiveDJ_PricingMU.ModalRupiah;
                        Qty = rs.Count();

                        tbl_Table.Rows.Add(tr);
                    }

                    if (rs.Count() > 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = "<b>Grand Total</b>";
                        td.ColumnSpan = 11;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + Qty.ToString() + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalNetWeight.ToString("0.00").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalGrossWeight.ToString("0.00").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalFramePriceM.ToString("0.00").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalFramePriceR.ToString("0.00").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalNOMAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalNOMLotAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalNORAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalROMAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalRORAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalFinishingM.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalFinishingR.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalOngkosLainM.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalOngkosLainR.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalStoneSettingM.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalStoneSettingR.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalHargaM.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalHargaR.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalHargaRp.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
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
                        td.Text = "<b>" + TotalButirAll.ToString("0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalKaratAll.ToString("0.000").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalUnitPrice.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalHargaParcelMAll.ToString("0.00").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalHargaParcelRAll.ToString("0.00").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "";
                        tr.Cells.Add(td);


                        tbl_Table.Rows.Add(tr);
                    }
                }
            }
            else if (Tipe.ToLower() == "list")
            {
                var rs = from p in db.StockReceiveDJ
                         where /*p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false &&*/ !p.Draft
                         //&& !(from s in db.SalesOrderDJ
                         //     where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                         //     select s.IDProduct).Contains(p.ID)
                         select p;

                rs = rs.OrderByDescending(p => p.ApprovalTgl);

                if (s_pf != "" && s_pt != "")
                {
                    start = Convert.ToDateTime(s_pf);
                    end = Convert.ToDateTime(s_pt);
                    rs = rs.Where(p => p.ApprovalTgl >= start && p.ApprovalTgl < end.AddDays(1));
                }

                if (s_kw != "")
                {
                    rs = from p in rs
                         where p.Nomor.Contains(s_kw) || p.ProtoNomor.Contains(s_kw) || p.SupplierNomor.Contains(s_kw)
                         select p;
                }
                int Cabang = Convert.ToInt32(s_kT);
                //if (Cabang > 0)  rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == Cabang);
                if (Cabang > 0) rs = rs.Where(p => p.IDWarehouse == Cabang);
                //else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);
                int ProductLevel = Convert.ToInt32(s_plevel);
                if (ProductLevel > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.ProductLevel == ProductLevel);
                int ProductItem = Convert.ToInt32(s_pitem);
                if (ProductItem > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.ProductItem == ProductItem);
                int ProductCategory = Convert.ToInt32(s_pcategory);
                if (ProductCategory > 0) rs = rs.Where(p => p.StockReceiveDJ_CharProduct.ProductCategory == ProductCategory);
                int Brand = Convert.ToInt32(s_vB);
                if (Brand > 0) rs = rs.Where(p => p.LocWarehouse.CompanyBrand.ID == Brand);
                string Supplier = s_sup;

                if (Supplier != "")
                    rs = rs.Where(p => p.SupplierNama.Equals(Supplier));

                int SupplierGroup = Convert.ToInt32(s_sup_group);
                if (SupplierGroup > 0)
                {
                    rs = from p in rs
                         join q in db.DataSupplier on p.Supplier equals q.ID
                         join r in db.DataSupplierGroup on q.IDGroup equals r.ID
                         where r.ID == SupplierGroup
                         select p;
                }

                if (s_ob == "1")
                    rs.OrderBy(p => p.LocWarehouse.Nama);
                else if (s_ob == "2")
                    rs.OrderBy(p => p.StockReceiveDJ_CharProduct.CharProductLevel.Nama);
                else if (s_ob == "3")
                    rs.OrderBy(p => p.StockReceiveDJ_CharProduct.CharProductCategory.Nama);
                else if (s_ob == "4")
                    rs.OrderBy(p => p.StockReceiveDJ_CharProduct.CharProductItem.Nama);

                if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                else
                {
                    int index = 1;
                    decimal TotalStock = 0m;
                    decimal TotalResell = 0m;
                    decimal TotalExchange = 0m;
                    decimal TotalHarga = 0m;
                    decimal TotalModal = 0m;
                    decimal TotalMdolar = 0m;
                    decimal TotalM = 0m;
                    decimal TotalMAsal = 0m;
                    decimal TotalR = 0m;
                    decimal TotalNetWeight = 0m;
                    decimal TotalGrossWeight = 0m;
                    decimal TotalButirAll = 0m;
                    decimal TotalCaratAll = 0m;
                    decimal TotalUnitPrice = 0m;

                    /// header table
                    TableHeaderRow htr = new TableHeaderRow();
                    TableHeaderCell htd;

                    htd = new TableHeaderCell();
                    htd.Text = "No.";
                    htd.Width = 50;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "No. PLU";
                    htd.Width = 90;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Stock Keeper";
                    htd.Width = 150;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Supplier";
                    htd.Width = 150;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Kode Karet";
                    htd.Width = 140;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Kode Barang";
                    htd.Width = 140;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Item";
                    htd.Width = 80;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Qty";
                    htd.Width = 80;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Status";
                    htd.Width = 80;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Lokasi";
                    htd.Width = 150;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Harga (Rp)";
                    htd.Width = 200;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "M Supplier ($)";
                    htd.Width = 130;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "M Supplier (Rp)";
                    htd.Width = 130;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "M Awal ($)";
                    htd.Width = 130;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "M Reval ($)";
                    htd.Width = 130;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "R ($)";
                    htd.Width = 130;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Nett Weight (gr)";
                    htd.Width = 130;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Gross Weight (gr)";
                    htd.Width = 130;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Butir";
                    htd.Width = 50;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Carat";
                    htd.Width = 70;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Segmen";
                    htd.Width = 150;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Stone Distribution";
                    htd.Width = 150;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Keterangan";
                    htd.Width = 200;
                    htr.Cells.Add(htd);

                    tbl_Table.Rows.Add(htr);

                    foreach (var r in rs)
                    {
                        decimal AllM = 0m;

                        var x = r.StockReceiveDJ_CostingProduct;
                        var z = r.StockReceiveDJ_Costing;

                        AllM = (x.Total + z.TotalBiaya);

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = index.ToString() + ".";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TD_Hyperlink_DJ_List(r.Nomor, "0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.HumanStockNama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.SupplierNama;
                        tr.Cells.Add(td);

                        string KodeKaret;
                        KodeKaret = (r.ProtoNomor != null) ? r.ProtoNomor : "";

                        td = new TableCell();
                        td.Text = KodeKaret;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.SupplierNomor;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.StockReceiveDJ_CharProduct.CharProductItem.Nama;
                        tr.Cells.Add(td);

                        decimal Jual = 0m;
                        decimal Exchange = 0m;
                        decimal Resell = 0m;

                        //if (r.StockActualDJ.Substorage == 1)
                        //    Jual = r.StockActualDJ.InHand;
                        //else if (r.StockActualDJ.Substorage == 3)
                        //    Exchange = r.StockActualDJ.InHand;
                        //else if (r.StockActualDJ.Substorage == 2)
                        //    Resell = r.StockActualDJ.InHand;

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
                        td.Text = "1";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = DL.SubStorage(1);
                        tr.Cells.Add(td);

                        string Lokasi = "";
                        Lokasi = rsWarehouse[r.IDWarehouse];
                        //Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.IDWarehouse, Db.Cnn_DB);

                        td = new TableCell();
                        td.Text = Lokasi;
                        tr.Cells.Add(td);

                        /*td = new TableCell();
                        td.Text = Convert.ToDecimal(AllM * z.TotalRate).ToString("N0") + " / " + Convert.ToDecimal(r.StockReceiveDJ_PricingMU.ModalRupiah).ToString("N0");
                        tr.Cells.Add(td);*/

                        /// Harga (Rp)
                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.StockReceiveDJ_PricingMU.HargaRupiahRound).ToString("N0");
                        tr.Cells.Add(td);

                        /// M Supplier ($)
                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.StockReceiveDJ_Costing.UnitPrice / r.StockReceiveDJ_Costing.TotalRate).ToString("N2");
                        tr.Cells.Add(td);

                        /// M Supplier (Rp)
                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.StockReceiveDJ_Costing.UnitPrice.Value).ToString("N0");
                        tr.Cells.Add(td);

                        /// M Reval ($)
                        td = new TableCell();
                        td.Text = Convert.ToDecimal(AllM).ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        decimal MAsal = AllM;

                        try
                        {
                            var rs_m = (from p in db.RevalLogProductDJ
                                        where p.Nomor == r.Nomor
                                        orderby p.OperatorTgl ascending
                                        select p).First();

                            var x_m = rs_m.RevalLogProductDJ_CostingProduct;
                            var z_m = rs_m.RevalLogProductDJ_Costing;

                            MAsal = (x_m.Total + z_m.TotalBiaya);
                        }
                        catch (Exception ex) { }

                        /// M Awal ($)
                        td = new TableCell();
                        td.Text = Convert.ToDecimal(MAsal).ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.StockReceiveDJ_PricingMU.ModalUSD).ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.StockReceiveDJ_CharProduct.NettoWeight).ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(r.StockReceiveDJ_CharProduct.GrossWeight).ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        var det = (from p in r.StockReceiveDJ_Stone1A
                                   select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                                   from p in r.StockReceiveDJ_Stone1B
                                   where p.StatusTerpasang == 1
                                   select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                                   from p in r.StockReceiveDJ_Stone2
                                   select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                                   from p in r.StockReceiveDJ_Stone3
                                   select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                                   from p in r.StockReceiveDJ_Stone4
                                   select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                                   from p in r.StockReceiveDJ_Stone5
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
                        td.Text = TotalButir.ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalCarat.ToString("N3").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.StockReceiveDJ_PricingSegmen.CharProductSegmen.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.StockReceiveDJ_CharProduct.CharStoneDist.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = r.Keterangan;
                        tr.Cells.Add(td);

                        tbl_Table.Rows.Add(tr);

                        TotalStock += Jual;
                        TotalResell += Resell;
                        TotalExchange += Exchange;
                        TotalHarga += Convert.ToDecimal(r.StockReceiveDJ_PricingMU.HargaRupiahRound);
                        TotalModal += Convert.ToDecimal(AllM * z.TotalRate.Value);
                        TotalMdolar += Convert.ToDecimal(r.StockReceiveDJ_Costing.UnitPrice / r.StockReceiveDJ_Costing.TotalRate);
                        TotalM += (AllM);
                        TotalMAsal += MAsal;
                        TotalR += r.StockReceiveDJ_PricingMU.ModalUSD;
                        TotalNetWeight += r.StockReceiveDJ_CharProduct.NettoWeight;
                        TotalGrossWeight += r.StockReceiveDJ_CharProduct.GrossWeight;
                        TotalButirAll += TotalButir;
                        TotalCaratAll += TotalCarat;
                        TotalUnitPrice += r.StockReceiveDJ_Costing.UnitPrice.Value;
                        index++;
                    }

                    if (rs.Count() > 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.ColumnSpan = 7;
                        td.Text = "<b>Total</b>";
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

                        /*td = new TableCell();
                        td.Text = "<b>" + TotalModal.ToString("N0") + " / "  + TotalHarga.ToString("N2") + "</b>";
                        tr.Cells.Add(td);*/

                        td = new TableCell();
                        td.Text = "<b>" + TotalHarga.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalMdolar.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalUnitPrice.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalM.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalMAsal.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalR.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalNetWeight.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalGrossWeight.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalButirAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalCaratAll.ToString("N3").Replace(".", "").Replace(",", ".") + "</b>";
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

                        tbl_Table.Rows.Add(tr);
                    }
                }
            }
        }
        else if (Mode == "PG")
        {
            if (Tipe.ToLower() == "rekap")
            {
                decimal TotalHarga = 0m;
                decimal TotalHargaJual = 0m;
                decimal TotalBerat = 0m;
                decimal Harga = 0m;
                decimal OngkosR = 0m;
                decimal Berat = 0m;
                decimal Qty = 0m;
                decimal KadarLogam = 0m;
                decimal KadarTukaranJual = 0m;
                decimal KadarTukaranBeli = 0m;
                decimal TotalBerat24 = 0m;
                decimal TotalHarga24 = 0m;
                decimal TotalQty = 0m;
                decimal TotalWeight = 0m;
                decimal TotalOngkos = 0m;
                decimal TotalTotal = 0m;

                var rs = from p in db.StockReceivePG
                         where p.Draft == false && p.Approval != null
                         select p;


                if (s_pf != "" && s_pt != "")
                {
                    start = Convert.ToDateTime(s_pf);
                    end = Convert.ToDateTime(s_pt);
                    rs = rs.Where(p => p.Tgl >= start && p.Tgl < end.AddDays(1));
                }

                if (rs.Count() == 0) { TableNoResult(tbl_Table); }
                else
                {
                    if (s_plevel != "" && s_plevel != "0")
                        rs = rs.Where(p => p.ProductLevel == Convert.ToInt32(s_plevel));
                    if (s_pmodel != "" && s_pmodel != "0")
                        rs = rs.Where(p => p.Model == Convert.ToInt32(s_pmodel));
                    if (s_pframe != "" && s_pframe != "0")
                        rs = rs.Where(p => p.FrameColor == Convert.ToInt32(s_pframe));
                    if (s_pitem != "" && s_pitem != "0")
                        rs = rs.Where(p => p.ProductItem == Convert.ToInt32(s_pitem));

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
                                 KadarTukaranBeli = g.Select(x => x.KadarTukaranBeli),
                                 FrameColor = g.Select(x => x.CharFrameColor.Nama),
                                 Weight = g.Sum(x => x.GrossWeight),
                                 Ongkos = g.Sum(x => x.MountingR),
                                 TotalHarga = g.Sum(x => x.TotalHarga),
                                 TotalBerat24 = g.Sum(x => x.NettoWeight),
                                 TotalHarga24 = g.Sum(x => x.TotalHarga24),
                                 Harga = g.Sum(x => x.Harga),
                                 TotalHargaJual = g.Sum(x => x.TotalHargaJual),
                                 KodeSupplier = g.Select(x => x.SupplierNama)
                             };

                    if (s_kw != "")
                        pg = pg.Where(g => g.ProductItem.Contains(s_kw) || g.ProductLevel.Contains(s_kw) || g.Model.Contains(s_kw) || g.FrameColor.Contains(s_kw));

                    string status, status2 = "";

                    /// header table
                    TableHeaderRow htr = new TableHeaderRow();
                    TableHeaderCell htd;

                    htd = new TableHeaderCell();
                    htd.Text = "No.";
                    htd.Width = 50;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "No. TTB";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Tanggal";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Product Level";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Model";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Kadar Logam";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Kadar Tukaran Beli";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Frame Color";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Supplier";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Qty";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Total Weight";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Ongkos";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Harga";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Total Berat (24K)";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Total Harga (24K)";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    htd = new TableHeaderCell();
                    htd.Text = "Total Harga Jual";
                    htd.Width = 100;
                    htr.Cells.Add(htd);

                    tbl_Table.Rows.Add(htr);

                    int index = 0;

                    foreach (var rslt in pg)
                    {
                        index++;

                        //if (!Response.IsClientConnected) break;
                        TableRow tr = new TableRow();
                        TableCell td;

                        string Tanggal = Str.FullDate(Db.SingleTime("SELECT TOP 1 Tgl FROM StockReceivePG WHERE NoTTB='" + rslt.NoTTB + "'", Db.Cnn_DB));

                        Qty = Convert.ToDecimal((from p in db.StockReceivePG
                                                 where p.NoTTB == "" + rslt.NoTTB + ""
                                                 select p).Count());

                        td = new TableCell();
                        td.Text = index.ToString() + ".";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class=\"text-sm\">" + rslt.NoTTB + "</span>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<span class=\"text-sm\">" + Tanggal + "</span>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rslt.ProductLevel.First();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rslt.Model.First();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rslt.KadarLogam.First().ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rslt.KadarTukaranBeli.First().ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rslt.FrameColor.First();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rslt.KodeSupplier.First();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rslt.Qty.ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rslt.Weight.ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rslt.Ongkos.ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(rslt.Harga).ToString("N0".Replace(".", "").Replace(",", "."));
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = rslt.TotalBerat24.ToString("N2").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = (Convert.ToDecimal(rslt.TotalHarga)).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = (Convert.ToDecimal(rslt.TotalHargaJual)).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        tbl_Table.Rows.Add(tr);

                        TotalQty += rslt.Qty;
                        TotalWeight += Convert.ToDecimal(rslt.Weight);
                        TotalOngkos += Convert.ToDecimal(rslt.Ongkos);
                        TotalHarga += Convert.ToDecimal(rslt.Harga);
                        TotalHargaJual += Convert.ToDecimal(rslt.TotalHargaJual);
                        TotalBerat += Convert.ToDecimal(rslt.TotalBerat24);
                        TotalTotal += (Convert.ToDecimal(rslt.Harga) + Convert.ToDecimal(rslt.Ongkos));
                    }
                    if (pg.Count() > 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = "<b>Total</b>";
                        td.ColumnSpan = 9;
                        td.HorizontalAlign = HorizontalAlign.Right;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalWeight.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalOngkos.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalHarga.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalBerat.ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalTotal.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = "<b>" + TotalHargaJual.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                        tr.Cells.Add(td);

                        tbl_Table.Rows.Add(tr);
                    }
                }
            }
            else if (Tipe.ToLower() == "detail")
            {

            }
            else if (Tipe.ToLower() == "list")
            {

            }
        }
        else if (Mode == "GJ")
        {
            if (Tipe.ToLower() == "rekap")
            {

            }
            else if (Tipe.ToLower() == "detail")
            {

            }
            else if (Tipe.ToLower() == "list")
            {

            }
        }
        else if (Mode == "LD")
        {
            if (Tipe.ToLower() == "rekap")
            {

            }
            else if (Tipe.ToLower() == "detail")
            {

            }
            else if (Tipe.ToLower() == "list")
            {

            }
        }
    }
    protected static string TD_Hyperlink_DJ_List(string PLU, string tipe)
    {
        string x = string.Empty;

        x += "<a target='_blank' href='DJ_Detail.aspx?refreshPage=1&kw=" + PLU + "&tipe=" + tipe + "'>" + PLU + "</a>";

        return x;
    }

    protected static void TableNoResult(Table tb)
    {
        tb.Width = new Unit("100%");

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td.Text = "No Data";
        //td.ColumnSpan = TableColspan(tb);
        //td.Width = new Unit("100%");
        tr.Cells.Add(td);

        tr.CssClass = "no-result";
        tb.Rows.Add(tr);
    }
}