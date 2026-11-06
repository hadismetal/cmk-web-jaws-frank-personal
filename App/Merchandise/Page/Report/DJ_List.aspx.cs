using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

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
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);
            DLC.CharProductSegmen(productsegmen);
            //DLC.CompanyBrand(brand);

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
        int ProductSegmen = Convert.ToInt32(productsegmen.SelectedValue);
        if (ProductSegmen > 0) rs = rs.Where(p => p.StockProductDJ_PricingSegmen.ProductSegmen == ProductSegmen);
        //int Brand = Convert.ToInt32(brand.SelectedValue);
        //if (Brand > 0) rs = rs.Where(p => p.LocWarehouse.CompanyBrand.ID == Brand);

        if (orderby.SelectedValue == "1")
        {
            if (tipe == 0)
            {
                rs = from p in rs
                     join q in db.LocWarehouse on p.StockActualDJ.IDLokasi equals q.ID
                     orderby q.Nama
                     select p;
            }
            else if(tipe == 1)
            {
                rs = from p in rs
                     join q in db.LocOutlet on p.StockActualDJ.IDLokasi equals q.ID
                     orderby q.Nama
                     select p;
            }
            else if (tipe == 2)
            {
                rs = from p in rs
                     join q in db.LocExhibition on p.StockActualDJ.IDLokasi equals q.ID
                     orderby q.Nama
                     select p;
            }
        }
        else if (orderby.SelectedValue == "2")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductLevel.Nama);
        else if (orderby.SelectedValue == "3")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductCategory.Nama);
        else if (orderby.SelectedValue == "4")
            rs.OrderBy(p => p.StockProductDJ_CharProduct.CharProductItem.Nama);
        else
        {
            
        }

        if (Cabang > 0)
        {
            int pageNumber = 1;
            int numberOfObjectsPerPage = 30;

            if (Request.QueryString["page"] != "")
                pageNumber = Convert.ToInt32(Request.QueryString["page"]);
            if (pageNumber < 0)
                pageNumber = 1;

            decimal TotalPage = Convert.ToDecimal(rs.Count()) / Convert.ToDecimal(numberOfObjectsPerPage);

            TotalPage = Math.Ceiling(TotalPage);

            rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

            string href = App.RefreshURL(this);

            //Customize-Qstr
            href += "&kw=" + kw.Text;
            href += "&cabang=" + cabang.Text;
            href += "&productitem=" + productitem.Text;
            href += "&productcategory=" + productcategory.Text;
            href += "&productlevel=" + productlevel.Text;
            href += "&tipe=" + tipe;
            href += "&order=" + orderby.Text;
            href += "&substorage=" + substorage.SelectedValue;
            href += "&productsegmen=" + productsegmen.SelectedValue;
            href += "&page=" + (pageNumber + 1).ToString();

            string href2 = App.RefreshURL(this);

            //Customize-Qstr
            href2 += "&kw=" + kw.Text;
            href2 += "&cabang=" + cabang.Text;
            href2 += "&productitem=" + productitem.Text;
            href2 += "&productcategory=" + productcategory.Text;
            href2 += "&productlevel=" + productlevel.Text;
            href2 += "&tipe=" + tipe;
            href2 += "&order=" + orderby.Text;
            href2 += "&substorage=" + substorage.SelectedValue;
            href2 += "&productsegmen=" + productsegmen.SelectedValue;
            href2 += "&page=" + (pageNumber - 1).ToString();

            string href3 = App.RefreshURL(this);

            //Customize-Qstr
            href3 += "&kw=" + kw.Text;
            href3 += "&cabang=" + cabang.Text;
            href3 += "&productitem=" + productitem.Text;
            href3 += "&productcategory=" + productcategory.Text;
            href3 += "&productlevel=" + productlevel.Text;
            href3 += "&tipe=" + tipe;
            href3 += "&order=" + orderby.Text;
            href3 += "&substorage=" + substorage.SelectedValue;
            href3 += "&productsegmen=" + productsegmen.SelectedValue;
            href3 += "&page=1";

            string href4 = App.RefreshURL(this);

            //Customize-Qstr
            href4 += "&kw=" + kw.Text;
            href4 += "&cabang=" + cabang.Text;
            href4 += "&productitem=" + productitem.Text;
            href4 += "&productcategory=" + productcategory.Text;
            href4 += "&productlevel=" + productlevel.Text;
            href4 += "&tipe=" + tipe;
            href4 += "&order=" + orderby.Text;
            href4 += "&substorage=" + substorage.SelectedValue;
            href4 += "&productsegmen=" + productsegmen.SelectedValue;
            href4 += "&page=" + TotalPage;

            string paging = "";

            paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
            paging += "<a href=\"" + href3 + "\">First</a>&nbsp;&nbsp;";
            paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
            paging += "<a href=\"" + href + "\">Next</a>&nbsp;&nbsp;";
            paging += "<a href=\"" + href4 + "\">Last</a>";
            paging += "<br /> Page " + (pageNumber + 1) + " of " + TotalPage + " </div>";

            paging_text.Text = paging;

            if (rs != null)
            {
                int index = (pageNumber > 0) ? (pageNumber * 30) + 1 : 1;
                decimal TotalStock = 0m;
                decimal TotalResell = 0m;
                decimal TotalExchange = 0m;
                decimal TotalHarga = 0m;
                decimal TotalModal = 0m;
                decimal TotalM = 0m;
                decimal TotalR = 0m;
                decimal TotalHJual = 0m;
                decimal TotalNetWeight = 0m;
                decimal TotalGrossWeight = 0m;
                decimal TotalButirAll = 0m;
                decimal TotalCaratAll = 0m;
                decimal TotalBunga = 0m;
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
                    td.Text = TD_Hyperlink(r.Nomor);
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
                    umur += Convert.ToDecimal((DateTime.Today - r.Tgl.Value).TotalDays);
                    td = new TableCell();
                    td.Text = Convert.ToString(umur); //(DateTime.Today - r.Tgl.Value).TotalDays.ToString();                
                    tr.Cells.Add(td);

                    var bg = from p in db.BungaBank
                             select p;
                    //bunga
                    bunga += Convert.ToDecimal(bg.First().Persen / 100 * umur / 360 * r.StockProductDJ_PricingMU.ModalRupiah);



                    tb.Rows.Add(tr);

                    TotalStock += Jual;
                    TotalResell += Resell;
                    TotalExchange += Exchange;
                    TotalHarga += Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah);
                    TotalModal += Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah);
                    TotalM += AllM;
                    TotalR += r.StockProductDJ_PricingMU.ModalUSD;
                    TotalHJual += r.StockProductDJ_PricingMU.HargaRupiahRound.Value;
                    TotalNetWeight += r.StockProductDJ_CharProduct.NettoWeight;
                    TotalGrossWeight += r.StockProductDJ_CharProduct.GrossWeight;
                    TotalButirAll += TotalButir;
                    TotalCaratAll += TotalCarat;
                    TotalBunga += bunga;
                    index++;

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
                    td.Text = rs.Count().ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "";
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

                    tb.Rows.Add(tr);
                }
            }
        }
        else
        {
            if (rs != null)
            {
                int index = 1;
                int count = 1;
                decimal TotalStock = 0m;
                decimal TotalResell = 0m;
                decimal TotalExchange = 0m;
                decimal TotalHarga = 0m;
                decimal TotalModal = 0m;
                decimal TotalM = 0m;
                decimal TotalR = 0m;
                decimal TotalHJual = 0m;
                decimal TotalNetWeight = 0m;
                decimal TotalGrossWeight = 0m;
                decimal TotalButirAll = 0m;
                decimal TotalCaratAll = 0m;
                decimal TotalBunga = 0m;
                int Qty = 0;

                string prev = "";
                foreach (var r in rs)
                {
                    decimal AllM = 0m;

                    var x = r.StockProductDJ_CostingProduct;
                    var z = r.StockProductDJ_Costing;

                    AllM = (x.Total + z.TotalBiaya);

                    //Sub-Header
                    if (prev != tipe.ToString() +  " "  + r.StockActualDJ.IDLokasi.ToString())
                    {
                        if (count > 1)
                        {
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
                            td2.Text = Qty.ToString("N0");
                            tr2.Cells.Add(td2);

                            td2 = new TableCell();
                            td2.Text = "";
                            tr2.Cells.Add(td2);

                            td2 = new TableCell();
                            td2.Text = "";
                            tr2.Cells.Add(td2);

                            td2 = new TableCell();
                            td2.Text = "<b>" + TotalM.ToString("N2") + "</b>";
                            tr2.Cells.Add(td2);

                            td2 = new TableCell();
                            td2.Text = "<b>" + TotalR.ToString("N2") + "</b>";
                            tr2.Cells.Add(td2);

                            td2 = new TableCell();
                            td2.Text = "<b>" + TotalHJual.ToString("N0") + "</b>";
                            tr2.Cells.Add(td2);

                            td2 = new TableCell();
                            td2.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
                            tr2.Cells.Add(td2);

                            td2 = new TableCell();
                            td2.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
                            tr2.Cells.Add(td2);

                            td2 = new TableCell();
                            td2.Text = "<b>" + TotalButirAll.ToString("N0") + "</b>";
                            tr2.Cells.Add(td2);

                            td2 = new TableCell();
                            td2.Text = "<b>" + TotalCaratAll.ToString("N3") + "</b>";
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
                            td2.Text = "<b>" + Str.Number2(TotalBunga);
                            tr2.Cells.Add(td2);

                            tb.Rows.Add(tr2);
                        }

                        Sub_Header(tipe,r.StockActualDJ.IDLokasi);
                        prev = tipe.ToString() + " " + r.StockActualDJ.IDLokasi.ToString();

                        index = 1;
                        TotalStock = 0m;
                        TotalResell = 0m;
                        TotalExchange = 0m;
                        TotalHarga = 0m;
                        TotalModal = 0m;
                        TotalM = 0m;
                        TotalR = 0m;
                        TotalHJual = 0m;
                        TotalNetWeight = 0m;
                        TotalGrossWeight = 0m;
                        TotalButirAll = 0m;
                        TotalCaratAll = 0m;
                        TotalBunga = 0m;
                    }

                    count++;

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = index.ToString() + ".";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Hyperlink(r.Nomor);
                    tr.Cells.Add(td);

                    string KodeKaret;
                    KodeKaret = (r.ProtoNomor != null) ? r.ProtoNomor : "";

                    td = new TableCell();
                    td.Text = "<small>" + KodeKaret + "</small>";
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
                    umur += Convert.ToDecimal((DateTime.Today - r.Tgl.Value).TotalDays);
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



                    tb.Rows.Add(tr);

                    TotalStock += Jual;
                    TotalResell += Resell;
                    TotalExchange += Exchange;
                    TotalHarga += Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah);
                    TotalModal += Convert.ToDecimal(r.StockProductDJ_PricingMU.ModalRupiah);
                    TotalM += AllM;
                    TotalR += r.StockProductDJ_PricingMU.ModalUSD;
                    TotalHJual += r.StockProductDJ_PricingMU.HargaRupiahRound.Value;
                    TotalNetWeight += r.StockProductDJ_CharProduct.NettoWeight;
                    TotalGrossWeight += r.StockProductDJ_CharProduct.GrossWeight;
                    TotalButirAll += TotalButir;
                    TotalCaratAll += TotalCarat;
                    TotalBunga += bunga;
                    Qty++;
                    index++;

                }
            }
        }
        
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
            productsegmen.SelectedValue = Request.QueryString["productsegmen"];
            //brand.SelectedValue = Request.QueryString["brand"];
            //datepicker.Text = Request.QueryString["dari"];
            //datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    protected void Sub_Header(int TipeLokasi, int IDLokasi)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        string Lokasi = "";
        if (tipe == 0)
            Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + IDLokasi, Db.Cnn_DB);
        else if (tipe == 1)
            Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + IDLokasi, Db.Cnn_DB);
        else if (tipe == 2)
            Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + IDLokasi, Db.Cnn_DB);

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

    protected string TD_Hyperlink(string PLU)
    {
        string x = string.Empty;

        x += "<a target='_blank' href='DJ_Detail.aspx?refreshPage=1&kw=" + PLU + "&tipe=" + tipe + "'>" + PLU + "</a>";

        return x;
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
        href += "&order=" + orderby.Text;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&productsegmen=" + productsegmen.SelectedValue;
       // href += "&brand=" + brand.Text;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "DJ_ListExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&order=" + orderby.Text;
        href += "&productsegmen=" + productsegmen.SelectedValue;
        //href += "&brand=" + brand.Text;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "DJ_ListPrint.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;
        href += "&order=" + orderby.Text;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&productsegmen=" + productsegmen.SelectedValue;
        //href += "&brand=" + brand.Text;
        //href += "&dari=" + datepicker.Text;
        //href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
}