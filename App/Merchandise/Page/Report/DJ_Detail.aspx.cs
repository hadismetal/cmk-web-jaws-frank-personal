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
		Button3.Visible = Role.DownloadExcel;

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);
            DLC.CompanyBrand(brand);

            DLC.LocationList(cabang, tipe);

            //datepicker.Text = Str.Tgl(DateTime.Now);
            //datepicker2.Text = Str.Tgl(DateTime.Now);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
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
                 && p.StockActualDJ.IDLokasi > 0
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

        int Substorage = Convert.ToInt32(substorage.SelectedValue);
        if (Substorage > 0)
        {
            rs = rs.Where(p => p.StockActualDJ.Substorage == Substorage);
        }

        if (kw.Text != "")
        {
            rs = from p in rs
                 where p.Nomor.Contains(kw.Text) || p.ProtoNomor.Contains(kw.Text)
                 select p;
        }
        int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0) rs = rs.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
        else rs = rs.Where(p => p.StockActualDJ.TipeLokasi == tipe);
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
        int Brand = Convert.ToInt32(brand.SelectedValue);
        if (Brand > 0) rs = rs.Where(p => p.LocWarehouse.CompanyBrand.ID == Brand);

        if (orderby.SelectedValue == "1")
            rs.OrderBy(p => p.StockActualDJ.NamaLokasi);
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductLevel.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductCategory.Nama);
        else if (orderby.SelectedValue == "4")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductItem.Nama);

        int pageNumber = 1;
        int numberOfObjectsPerPage = 30;

        decimal TotalPage = Convert.ToDecimal(rs.Count()) / Convert.ToDecimal(numberOfObjectsPerPage);

        TotalPage = Math.Ceiling(TotalPage);

        if (Request.QueryString["page"] != "")
            pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        if (pageNumber < 0)
            pageNumber = 1;

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
                td.Text = r.StockProductDJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "1";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductDJ_CharProduct.NettoWeight.ToString("N2");
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
                }

                td = new TableCell();
                //td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah).ToString("N2") + " / " + Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N2");
                td.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound.Value).ToString("N0");
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
                td.Text = "<b>" + Qty.ToString() + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalNetWeight.ToString("0.00") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>-</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalButirAll.ToString("0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalKaratAll.ToString("0.000") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHargaRp.ToString("N0") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }

        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&page=" + (pageNumber + 1).ToString();

        string href2 = App.RefreshURL(this);

        //Customize-Qstr
        href2 += "&kw=" + kw.Text;
        href2 += "&productitem=" + productitem.Text;
        href2 += "&productcategory=" + productcategory.Text;
        href2 += "&productlevel=" + productlevel.Text;
        href2 += "&tipe=" + tipe;
        href2 += "&orderby=" + orderby.SelectedValue;
        href2 += "&substorage=" + substorage.SelectedValue;
        href2 += "&page=" + (pageNumber - 1).ToString();

        string href3 = App.RefreshURL(this);

        //Customize-Qstr
        href3 += "&kw=" + kw.Text;
        href3 += "&productitem=" + productitem.Text;
        href3 += "&productcategory=" + productcategory.Text;
        href3 += "&productlevel=" + productlevel.Text;
        href3 += "&tipe=" + tipe;
        href3 += "&orderby=" + orderby.SelectedValue;
        href3 += "&substorage=" + substorage.SelectedValue;
        href3 += "&page=1";

        string href4 = App.RefreshURL(this);

        //Customize-Qstr
        href4 += "&kw=" + kw.Text;
        href4 += "&productitem=" + productitem.Text;
        href4 += "&productcategory=" + productcategory.Text;
        href4 += "&productlevel=" + productlevel.Text;
        href4 += "&tipe=" + tipe;
        href4 += "&orderby=" + orderby.SelectedValue;
        href4 += "&substorage=" + substorage.SelectedValue;
        href4 += "&page=" + TotalPage;

        string paging = "";

        paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
        paging += "<a href=\"" + href3 + "\">First</a>&nbsp;&nbsp;";
        paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
        paging += "<a href=\"" + href + "\">Next</a>&nbsp;&nbsp;";
        paging += "<a href=\"" + href4 + "\">Last</a>";
        paging += "<br /> Page " + (pageNumber + 1) + " of " + TotalPage + " </div>";

        paging_text.Text = paging;

    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            cabang.SelectedValue = Request.QueryString["cabang"];
            kw.Text = Request.QueryString["kw"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            orderby.SelectedValue = Request.QueryString["orderby"];
            substorage.SelectedValue = Request.QueryString["substorage"];
            brand.SelectedValue = Request.QueryString["brand"];
            //datepicker.Text = Request.QueryString["dari"];
            //datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&brand=" + brand.Text;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "DJ_DetailExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&brand=" + brand.Text;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "DJ_DetailPrint.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&brand=" + brand.Text;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
}