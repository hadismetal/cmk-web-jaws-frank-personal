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

        if (!IsPostBack)
        {

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);

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

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.SalesOrder.IDLokasi.Value) && p.SalesOrder.TipeLokasi == 1);
        }

        rs = rs.OrderByDescending(p => p.SalesOrder.Tgl);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text));

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var rsList = rs.ToList();
        if (rsList.Count > 0)
        {
            decimal TotalHargaM = 0m;
            decimal TotalQty = 0m;
            decimal TotalHargaJual = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;

            int prev = 0;
            int index = 1;

            var locOutletDict = db.LocOutlet.ToDictionary(p => p.ID);
            var locExhibitionDict = db.LocExhibition.ToDictionary(p => p.ID);

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
                td.Text = TD_HyperLink(r.SalesOrder.ID, 0);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.SalesOrder.Tgl);
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.Invoice;
                tr.Cells.Add(td);

                string Nomor = "";
                string Tipe = "";
                decimal HargaM = 0m;
                decimal HargaJual = 0m;
                decimal Butir = 0m;
                decimal Carat = 0m;
                string ProductItem = "";
                string ProductCategory = "";
                if (r.TipeProduct == null || r.TipeProduct == 0)
				{
					string item = Db.SingleString("SELECT Nama FROM Souvenir WHERE ID=" + r.IDProduct, Db.Cnn_DB);
					Nomor = item;
					Tipe = "Souvenir";
					HargaM = 0m;
					HargaJual = 0m;
					ProductItem = item;
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
                }
                else if (r.TipeProduct == 3)
                {
                    Tipe = "Plain Gold";
                    var d = pgDict[r.IDProductSouvenir.Value];
                    Nomor = d.Nomor;
                    HargaM = d.TotalHargaUSD.Value;
                    HargaJual = d.TotalHargaJual.Value;
                    ProductItem = d.CharProductItem.Nama;
                    ProductCategory = "Plain Gold";
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
                }

                td = new TableCell();
                td.Text = Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ProductItem;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ProductCategory;
                tr.Cells.Add(td);

                string Lokasi = "";
                if (r.SalesOrder.TipeLokasi == 1)
                    Lokasi = locOutletDict.ContainsKey(r.SalesOrder.IDLokasi.Value) ? locOutletDict[r.SalesOrder.IDLokasi.Value].Nama : "";
                else if (r.SalesOrder.TipeLokasi == 2)
                    Lokasi = locExhibitionDict.ContainsKey(r.SalesOrder.IDLokasi.Value) ? locExhibitionDict[r.SalesOrder.IDLokasi.Value].Nama : "";

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Tipe;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesOrder.IDCustomer.HasValue ? r.SalesOrder.CustomerNama : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesOrder.IDSales.HasValue ? r.SalesOrder.DataSales.Nama : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
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

                TotalQty += Qty;
                TotalHargaM += HargaM;
                TotalHargaJual += HargaJual;
                TotalButir += Butir;
                TotalCarat += Carat;

                tb.Rows.Add(tr);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 11;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
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
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "Souvenir_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "Souvenir_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(int ID , int SID)
    {
		var d = db.SalesOrder.SingleOrDefault(p => p.ID == ID);
		DateTime tgl = Convert.ToDateTime(d.Tgl);
		DateTime tglcuttoff = Convert.ToDateTime("2024-10-01 00:00:00.000");
		string x = string.Empty;

		if (tgl >= tglcuttoff)
		{
			DataTable dt = Db.Rs("SELECT TOP 1 Nomor, Status FROM FormApprovalSouvenir WHERE IDSalesOrder = " + ID, Db.Cnn_DB);
			if (dt.Rows.Count == 1)
			{
				if (Convert.ToInt32(dt.Rows[0]["Status"]) == 2)
				{
					x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"/OutletSales/Page/Create/Payment/Sales_Packaging.aspx?&id=" + ID + "&sid=" + SID + "\">";
					x += "  <i class=\"icon-search\"></i> ";
					x += "  Print";
					x += "</a>";
				}
				else
				{
					x += "";
				}
			}
			else
			{
				x += "";
			}
		}
		else
		{
			x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"/OutletSales/Page/Create/Payment/Sales_Packaging.aspx?&id=" + ID + "&sid=" + SID + "\">";
			x += "  <i class=\"icon-search\"></i> ";
			x += "  Print";
			x += "</a>";
		}

		return x;
    }
}