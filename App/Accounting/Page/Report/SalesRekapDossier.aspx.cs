using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;

public partial class Page_Summary : System.Web.UI.Page
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
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductSegmen(productsegmen);

            var x = from n in db.PaymentType
                    select new { n.ID, n.Nama };

            foreach (var r in x)
            {
                payment.Items.Add(new ListItem(r.Nama, r.ID.ToString()));
            }

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);

            int Tipe = 1;
            if (tipelokasi.SelectedValue != "")
                Tipe = Convert.ToInt32(tipelokasi.SelectedValue);

            DLC.LocationList(lokasi, 1);

            status.SelectedValue = "2";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int IDLokasi = Convert.ToInt32(lokasi.SelectedValue);

        int Filter = Convert.ToInt32(Request.QueryString["filter"]);

        var rs = (from p in db.StockProductDJ_Stone1B
                  join q in db.SalesOrderDJ on p.IDForm equals q.IDProduct
                  where q.StockProductDJ.Legacy.Value == null && !q.SalesOrder.Draft
                  && q.StockProductDJ.StockProductDJ_Stone1B.Count() > 0
                  && !(from z in db.SalesOrderDJ
                       where z.StockProductDJ.Legacy.Value == 1
                       select z.SalesOrder.ID).Contains(q.IDForm)
                      //&& p.SalesOrder.StatusPembayaran == false
                  && q.SalesOrder.StatusPembayaran == true
                  && ((Filter == 1) ?
                  (from z in db.StockBasic
                   select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                  : (Filter == 2) ? !(from z in db.StockBasic
                                      select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                  : 1 == 1)
                  select new
                  {
					  ProductType = "DJ",
                      ProtoNomor = p.StockProductDJ.ProtoNomor,
                      ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.ProductItem,
                      ProductCategory = p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory,
                      ProductSegmen = p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen,
                      TipeLokasi = q.SalesOrder.TipeLokasi.Value,
                      IDLokasi = q.SalesOrder.IDLokasi.Value,
                      NoSO = q.SalesOrder.Nomor,
                      CustomerNama = q.SalesOrder.CustomerNama,
					  SalesNama = q.SalesOrder.SalesNama,
                      Invoice = q.Invoice,
                      Nomor = q.StockProductDJ.Nomor,
                      StatusVoid = q.SalesOrder.StatusVoid.Value,
                      Tgl = q.SalesOrder.Tgl.Value,
                      Stone1B = p.Stone1B,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      GIA = p.GIA,
                      
					  HargaBarcode = q.HargaRupiah.GetValueOrDefault(),
                      HargaNet = q.TotalBayar.GetValueOrDefault(),
                      
					  //HargaMBatu = (p.HargaTotalM.GetValueOrDefault () <= 1 ) ? q.StockProductDJ.StockProductDJ_Costing.TotalBiaya + q.StockProductDJ.StockProductDJ_CostingProduct.Total : p.HargaTotalM.GetValueOrDefault(),
					  //HargaM = p.HargaTotalM.GetValueOrDefault(),
                      HargaMBatu = (p.HargaTotalM.GetValueOrDefault() > 0) ? p.HargaTotalM.GetValueOrDefault() : 1,
					  HargaRBatu = p.HargaTotal.GetValueOrDefault(),
                      
					  HargaMBatuJual = p.HargaMJual.GetValueOrDefault(),
                      HargaRBatuJual = p.HargaRJual.GetValueOrDefault(),
					  
					  HargaMtotal = q.StockProductDJ.StockProductDJ_Costing.TotalBiaya + q.StockProductDJ.StockProductDJ_CostingProduct.Total,
					  HargaRtotal = q.StockProductDJ.StockProductDJ_PricingMU.ModalUSD,
					  
					  Rapaport = p.HargaInputH.GetValueOrDefault(),
					  
                      DiscMJual = p.DiscMJual.GetValueOrDefault(),
                      DiscRJual = p.DiscRJual.GetValueOrDefault(),
					  Keterangan = q.SalesOrder.Keterangan
                  }).Concat(
                  from p in db.SalesOrderLD
                  where !p.SalesOrder.Draft
                      //&& p.SalesOrder.StatusPembayaran == false
                  && p.SalesOrder.StatusPembayaran == true
                  select new
                  {
					  ProductType = "LD",
                      ProtoNomor = "-",
                      ProductItem = DLC.ProductItem(),
                      ProductCategory = DLC.ProductCategory(),
                      ProductSegmen = DLC.ProductSegmen(),
                      TipeLokasi = p.SalesOrder.TipeLokasi.Value,
                      IDLokasi = p.SalesOrder.IDLokasi.Value,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
					  SalesNama = p.SalesOrder.SalesNama,
                      Invoice = p.Invoice,
                      Nomor = p.StockProductLD_Stone1B.Nomor,
                      StatusVoid = p.SalesOrder.StatusVoid.Value,
                      Tgl = p.SalesOrder.Tgl.Value,
                      Stone1B = p.StockProductLD_Stone1B.Stone1B,
                      TotalButir = p.StockProductLD_Stone1B.TotalButir,
                      TotalCarat = p.StockProductLD_Stone1B.TotalCarat,
                      GIA = p.StockProductLD_Stone1B.GIA,
					  
                      HargaBarcode = p.HargaRupiah.Value,
                      HargaNet = p.TotalBayar.GetValueOrDefault(),
                      
					  HargaMBatu = p.StockProductLD_Stone1B.HargaTotalM.GetValueOrDefault(),
                      HargaRBatu = p.StockProductLD_Stone1B.HargaTotal.GetValueOrDefault(),
					  
                      HargaMBatuJual = p.StockProductLD_Stone1B.HargaMJual.GetValueOrDefault(),
                      HargaRBatuJual = p.StockProductLD_Stone1B.HargaRJual.GetValueOrDefault(),
					  
					  HargaMtotal = p.StockProductLD_Stone1B.HargaTotalM.GetValueOrDefault(),
					  HargaRtotal = p.StockProductLD_Stone1B.HargaTotal.GetValueOrDefault(),
					  
					  Rapaport = p.StockProductLD_Stone1B.HargaInputH.GetValueOrDefault(),
					  
                      DiscMJual = p.StockProductLD_Stone1B.DiscMJual.GetValueOrDefault(),
                      DiscRJual = p.StockProductLD_Stone1B.DiscRJual.GetValueOrDefault(),
					  Keterangan = p.SalesOrder.Keterangan
                  });

        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductSegmen = Convert.ToInt32(Request.QueryString["productsegmen"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int Payment = Convert.ToInt32(Request.QueryString["payment"]);

        if (ProductItem > 0)
            rs = rs.Where(p => p.ProductItem == ProductItem);
        if (ProductCategory > 0)
            rs = rs.Where(p => p.ProductCategory == ProductCategory);
        if (ProductSegmen > 0)
            rs = rs.Where(p => p.ProductSegmen == ProductSegmen);

        if (Tipe >= 0)
            rs = rs.Where(p => p.TipeLokasi == Tipe);
        if (IDLokasi >= 0)
            rs = rs.Where(p => p.IDLokasi == IDLokasi);

        rs = rs.OrderByDescending(p => p.Tgl).ThenBy(p => p.NoSO);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.NoSO == "" + keyword + "" || p.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text) || p.Nomor.Contains(kw.Text));

        if (status.SelectedValue == "1")
            rs = rs.Where(p => p.StatusVoid == true);
        else if (status.SelectedValue == "2")
            rs = rs.Where(p => p.StatusVoid == false);
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalHargaMbatu = 0m;
            decimal TotalHargaRbatu = 0m;
            decimal TotalHargaMBatuJual = 0m;
            decimal TotalHargaRBatuJual = 0m;
            decimal TotalHargaMTotal = 0m;
            decimal TotalHargaRTotal = 0m;
            decimal TotalHargabarcode = 0m;
            decimal TotalHarganet = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalCw = 0m;
			
            int prev = 0;
            int index = 1;
            string SO = "";
            foreach (var r in rs)
            {
                decimal Butir = 0m;
                decimal Carat = 0m;
                decimal TotalC = 0m;
                int Qty = 1;

                Butir = r.TotalButir;
                Carat = r.TotalCarat / r.TotalButir;
                TotalC = r.TotalCarat;
                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                string Lokasi = "";
                if (r.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                else if (r.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NoSO;
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.Invoice;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CustomerNama;
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.SalesNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                string CategoryNama = Db.SingleString("SELECT Nama FROM CharProductCategory WHERE ID=" + r.ProductCategory, Db.Cnn_DB);
                string ItemNama = Db.SingleString("SELECT Nama FROM CharProductItem WHERE ID=" + r.ProductItem, Db.Cnn_DB);
                string SegmenNama = Db.SingleString("SELECT Nama FROM CharProductSegmen WHERE ID=" + r.ProductSegmen, Db.Cnn_DB);

                td = new TableCell();
                td.Text = CategoryNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProtoNomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ItemNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Stone1B.Parcel106.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Stone1B.Parcel103.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Stone1B.Parcel104.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Butir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Carat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalC.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<a href=\"http://www.gia.edu/cs/Satellite?pagename=GST%2FDispatcher&childpagename=GIA%2FPage%2FReportCheck&c=Page&cid=1355954554547&reportno=" + r.GIA + "\" target=\"_blank\">" + r.GIA + "</a>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HargaBarcode.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HargaNet.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HargaMBatu.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HargaRBatu.ToString("N2");
                tr.Cells.Add(td);
				
				if (r.ProductType == "LD")
				{
					decimal HargaMJual = 0m;
					HargaMJual = (r.HargaMBatuJual + (r.HargaMBatuJual * r.DiscMJual / 100)) * r.TotalCarat;

					td = new TableCell();
					td.Text = HargaMJual.ToString("N2");
					tr.Cells.Add(td);

					decimal HargaRJual = 0m;

					HargaRJual = (r.HargaRBatuJual + (r.HargaRBatuJual * r.DiscRJual / 100)) * r.TotalCarat;

					td = new TableCell();
					td.Text = (HargaRJual).ToString("N2");
					tr.Cells.Add(td);
				}
				else
				{
					td = new TableCell();
					td.Text = r.HargaMBatuJual.ToString("N2");
					tr.Cells.Add(td);
					
					
					td = new TableCell();
					td.Text = r.HargaRBatuJual.ToString("N2");
					tr.Cells.Add(td);
				}
				
                td = new TableCell();
                td.Text = r.HargaMtotal.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HargaRtotal.ToString("N2");
                tr.Cells.Add(td);
				
				decimal Rapaport = 0m;
				
				if (r.ProductType == "LD")
				{
					decimal HargaRJual = 0m;
					
					HargaRJual = (r.HargaRBatuJual + (r.HargaRBatuJual * r.DiscRJual / 100)) * r.TotalCarat;

                    Rapaport = TotalC == 0 ? 0 : (HargaRJual / TotalC) / (1 + (r.DiscRJual / 100));
					
					td = new TableCell();
					td.Text = Rapaport.ToString("N2");
					tr.Cells.Add(td);
				}
				else
				{
					Rapaport = (r.HargaRBatuJual / TotalC) / (1+ (r.DiscRJual / 100));
					
					td = new TableCell();
					td.Text = Rapaport.ToString("N2");
					tr.Cells.Add(td);
				}
				

                td = new TableCell();
                td.Text = r.DiscMJual.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscRJual.ToString("N2");
                tr.Cells.Add(td);
				
				if (r.ProductType == "LD")
				{
					td = new TableCell();
					td.Text = r.HargaRBatu == 0 ? "0" : "<b>" + Convert.ToDecimal(((r.HargaRBatu - r.HargaMBatu) / r.HargaRBatu) * 100).ToString("N2") + " %</b>";
					tr.Cells.Add(td);
				}
				else
				{
					td = new TableCell();
                    td.Text = r.HargaRtotal == 0 ? "0" : "<b>" + Convert.ToDecimal(((r.HargaRtotal - r.HargaMtotal) / r.HargaRtotal) * 100).ToString("N2") + " %</b>";
					tr.Cells.Add(td);
				}
				
				td = new TableCell();
                td.Text = r.Keterangan;
                tr.Cells.Add(td);
				
                tb.Rows.Add(tr);
				
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalCw += TotalC;
                TotalHargabarcode += r.HargaBarcode;
                TotalHarganet += r.HargaNet;
                TotalHargaMbatu += r.HargaMBatu;
                TotalHargaRbatu += r.HargaRBatu;
                TotalHargaMBatuJual += r.HargaMBatuJual;
                TotalHargaRBatuJual += r.HargaRBatuJual;
                TotalHargaMTotal += r.HargaMtotal;
                TotalHargaRTotal += r.HargaRtotal;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 13;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalButir.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalCw.ToString("N3") + "</b>";
            tr2.Cells.Add(td2);
			
			
            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalHargabarcode.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalHarganet.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);
			
            td2 = new TableCell();
            td2.Text = "<b>" + TotalHargaMbatu.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);
			
            td2 = new TableCell();
            td2.Text = "<b>" + TotalHargaRbatu.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);
			
            td2 = new TableCell();
            td2.Text = "<b>" + TotalHargaMBatuJual.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);
			
            td2 = new TableCell();
            td2.Text = "<b>" + TotalHargaRBatuJual.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);
			
            td2 = new TableCell();
            td2.Text = "<b>" + TotalHargaMTotal.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);
			
            td2 = new TableCell();
            td2.Text = "<b>" + TotalHargaRTotal.ToString("N2") + "</b>";
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
            td2.Text = "";
            tr2.Cells.Add(td2);



            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {

        tipelokasi.Items.Clear();
        tipelokasi.Items.Add(new ListItem("All", "-1"));
        tipelokasi.Items.Add(new ListItem("Outlet", "1"));
        tipelokasi.Items.Add(new ListItem("Exhibition", "2"));

        if (App.Refresh(this))
        {
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            DLC.LocationList(lokasi, Convert.ToInt32(Request.QueryString["tipelokasi"]));

            lokasi.SelectedValue = Request.QueryString["lokasi"];

            kw.Text = Request.QueryString["kw"];

            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productsegmen.SelectedValue = Request.QueryString["productsegmen"];
            filter.SelectedValue = Request.QueryString["filter"];
            status.SelectedValue = Request.QueryString["status"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            payment.SelectedValue = Request.QueryString["payment"];
        }
        else
        {
            lokasi.Items.Clear();
            lokasi.Items.Add(new ListItem("All", "-1"));
        }

    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productsegmen=" + productsegmen.Text;
        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "SalesRekapDossier_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productsegmen=" + productsegmen.Text;
        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "SalesRekapDossier_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productsegmen=" + productsegmen.Text;
        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;

        Response.Redirect(href);
    }
    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        lokasi.Items.Clear();
        if (Tipe == 0 || Tipe == 1 || Tipe == 2) { DLC.LocationList(lokasi, Tipe); }
        else { lokasi.Items.Add(new ListItem("All", "-1")); }
        lokasi.Focus();
    }
}