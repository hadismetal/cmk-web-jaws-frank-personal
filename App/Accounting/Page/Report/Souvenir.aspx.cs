using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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

        int Tipe = -1;

        if (!IsPostBack)
        {

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);

            if (Request.QueryString["tipelokasi"] != "")
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);

            DLC.LocationList(lokasi, Tipe);

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);  
        }

    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesOrderSouvenir
                 select p;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        int TipeLokasi = -1;
        int IDLokasi = -1;

        try
        {
            TipeLokasi = Convert.ToInt32(tipelokasi.SelectedValue);
        }
        catch (Exception) { }

        try
        {
            IDLokasi = Convert.ToInt32(lokasi.SelectedValue);
        }
        catch (Exception) { }

        if (TipeLokasi == 1 || TipeLokasi == 2)
        {
            rs = rs.Where(p => p.SalesOrder.TipeLokasi == TipeLokasi);

            if (IDLokasi > 0)
            {
                rs = rs.Where(p => p.SalesOrder.IDLokasi == IDLokasi);
            }
        }

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text));

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }
		
		rs = rs.Where(p => p.SalesOrder.StatusVoid == false);

        rs = rs.OrderByDescending(p => p.SalesOrder.Tgl);

        var rsList = rs.ToList();
        if (rsList.Count > 0)
        {
            decimal TotalHargaM = 0m;
            decimal TotalQty = 0m;
            decimal TotalHargaJual = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalGrossWeight = 0m;
            decimal TotalNetWeight = 0m;

            int prev = 0;
            int index = 1;


            /// Master Brand
            var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

            var djIds = rsList.Where(p => p.TipeProduct == 1).Select(p => p.IDProductSouvenir.Value).Distinct().ToList();
            var djDict = db.StockProductDJ.Where(p => djIds.Contains(p.ID)).ToDictionary(p => p.ID);
            var gjIds = rsList.Where(p => p.TipeProduct == 2).Select(p => p.IDProductSouvenir.Value).Distinct().ToList();
            var gjDict = db.StockProductGJ.Where(p => gjIds.Contains(p.ID)).ToDictionary(p => p.ID);
            var pgIds = rsList.Where(p => p.TipeProduct == 3).Select(p => p.IDProductSouvenir.Value).Distinct().ToList();
            var pgDict = db.StockProductPG.Where(p => pgIds.Contains(p.ID)).ToDictionary(p => p.ID);
            var ldIds = rsList.Where(p => p.TipeProduct == 4).Select(p => p.IDProductSouvenir.Value).Distinct().ToList();
            var ldDict = db.StockProductLD_Stone1B.Where(p => ldIds.Contains(p.ID)).ToDictionary(p => p.ID);

            foreach (var r in rsList)
            {
                int Qty = 1;


                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
                
                td = new TableCell();
                td.Text = Str.FullDate(r.SalesOrder.Tgl);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.Invoice;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesOrder.Nomor;
                tr.Cells.Add(td);

                string Nomor = "";
                string Tipe = "";
                decimal HargaM = 0m;
                decimal HargaJual = 0m;
                decimal Butir = 0m;
                decimal Carat = 0m;
                string ProductItem = "";
                string ProductCategory = "";
                decimal GrossWeight = 0m;
                decimal NetWeight = 0m;
                if (r.TipeProduct == null && r.IDProductSouvenir != null)
                {
                    Nomor = Db.SingleString("SELECT Nama FROM Souvenir WHERE ID=" + r.IDProductSouvenir, Db.Cnn_DB);
                    Tipe = "Souvenir";
                    HargaM = 0m;
                    HargaJual = 0m;
                    ProductItem = "Souvenir";
                    ProductCategory = "Souvenir";
                }
                else if (r.TipeProduct == 1)
                {
                    Tipe = "Diamond Jewellery";
                    var d = djDict[r.IDProductSouvenir.Value];
                    Nomor = d.Nomor;
                    HargaM = d.StockProductDJ_Costing.TotalBiaya + d.StockProductDJ_CostingProduct.Total;
                    HargaJual = d.StockProductDJ_PricingMU.HargaRupiahRound.Value;
                    ProductItem = d.StockProductDJ_CharProduct.CharProductItem.Nama;
                    ProductCategory = d.StockProductDJ_CharProduct.CharProductCategory.Nama;
                    Butir = d.StockProductDJ_CharProduct.StoneQty.Value;
                    Carat = d.StockProductDJ_CharProduct.StoneCarat;
                    GrossWeight = d.StockProductDJ_CharProduct.GrossWeight;
                    NetWeight = d.StockProductDJ_CharProduct.NettoWeight;
                }
                else if (r.TipeProduct == 2)
                {
                    Tipe = "Gold Jewellery";
                    var d = gjDict[r.IDProductSouvenir.Value];
                    Nomor = d.Nomor;
                    HargaM = d.StockProductGJ_Costing.TotalBiaya + d.StockProductGJ_CostingProduct.Total;
                    HargaJual = d.StockProductGJ_PricingMU.HargaRupiahRound.Value;
                    ProductItem = d.StockProductGJ_CharProduct.CharProductItem.Nama;
                    ProductCategory = d.StockProductGJ_CharProduct.CharProductCategory.Nama;
                    GrossWeight = d.StockProductGJ_CharProduct.GrossWeight;
                    NetWeight = d.StockProductGJ_CharProduct.NettoWeight;
                }
                else if (r.TipeProduct == 3)
                {
                    Tipe = "Plain Gold";
                    var d = pgDict[r.IDProductSouvenir.Value];
                    Nomor = d.Nomor;
                    HargaM = d.TotalHarga24.Value/d.TotalRate.Value;
                    HargaJual = d.TotalHargaJual.Value;
                    ProductItem = d.CharProductItem.Nama;
                    ProductCategory = "Plain Gold";
                    GrossWeight = d.GrossWeight.Value;
                    NetWeight = d.GrossWeight.Value;
                }
                else if (r.TipeProduct == 4)
                {
                    Tipe = "Loose Diamond";
                    var d = ldDict[r.IDProductSouvenir.Value];
                    Nomor = d.Nomor;
                    HargaM = d.HargaTotalM.Value;
                    HargaJual = d.HargaTotal.Value;
                    ProductItem = d.CharProductItem.Nama;
                    ProductCategory = "Loose Diamond";
                    Butir = 1m;
                    Carat = d.TotalCarat;
                    GrossWeight = 0m;
                    NetWeight = 0m;
                }

                td = new TableCell();
                td.Text = Nomor;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ProductItem;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ProductCategory;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                string Lokasi = "";
                //if (r.SalesOrder.TipeLokasi == 1)
                //    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.SalesOrder.IDLokasi, Db.Cnn_DB);
                //else if (r.SalesOrder.TipeLokasi == 2)
                //    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID=" + r.SalesOrder.IDLokasi, Db.Cnn_DB);

                try
                {
                    if (r.SalesOrder.TipeLokasi == 0)
                        Lokasi = rsWarehouse[r.SalesOrder.IDLokasi.Value];
                    else if (r.SalesOrder.TipeLokasi == 1)
                        Lokasi = rsOutlet[r.SalesOrder.IDLokasi.Value];
                    else if (r.SalesOrder.TipeLokasi == 2)
                        Lokasi = rsExhibition[r.SalesOrder.IDLokasi.Value];
                }
                catch (Exception ex) { throw ex; }

                td = new TableCell();
                td.Text = Lokasi;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Tipe;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesOrder.IDCustomer.HasValue ? r.SalesOrder.CustomerNama : "";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesOrder.IDSales.HasValue ? r.SalesOrder.DataSales.Nama : "";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = HargaM.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = HargaJual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Butir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Carat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = GrossWeight.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NetWeight.ToString("N2");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = (r.SalesOrder.Keterangan != "") ? r.SalesOrder.Keterangan : "";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                TotalQty += Qty;
                TotalHargaM += HargaM;
                TotalHargaJual += HargaJual;
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalGrossWeight += GrossWeight;
                TotalNetWeight += NetWeight;

                tb.Rows.Add(tr);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 10;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalHargaM.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalHargaJual.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalButir.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            lokasi.SelectedValue = Request.QueryString["lokasi"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "Souvenir_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "Souvenir_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            DLC.LocationList(lokasi, Tipe);
        }
        else
        {
            lokasi.Items.Clear();
        }
        lokasi.Focus();
    }
}