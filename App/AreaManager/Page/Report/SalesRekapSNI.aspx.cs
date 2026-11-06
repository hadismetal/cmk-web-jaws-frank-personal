using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

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

            //DLC.LocationList(lokasi, 1);
            FillLokasi(sender, e);

            status.SelectedValue = "2";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        int Tipe = 1;
        int IDLokasi = Convert.ToInt32(lokasi.SelectedValue);

        int Filter = Convert.ToInt32(Request.QueryString["filter"]);

        //var rs = (from p in db.StockProductDJ_Stone1B
        //          join q in db.SalesOrderDJ on p.IDForm equals q.IDProduct
        //          where q.StockProductDJ.Legacy.Value == null && !q.SalesOrder.Draft
        //          && q.StockProductDJ.StockProductDJ_Stone1B.Count() > 0
        //          && !(from z in db.SalesOrderDJ
        //               where z.StockProductDJ.Legacy.Value == 1
        //               select z.SalesOrder.ID).Contains(q.IDForm)
        //              //&& p.SalesOrder.StatusPembayaran == false
        //          && q.SalesOrder.StatusPembayaran == true
        //          && ((Filter == 1) ?
        //          (from z in db.StockBasic
        //           select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
        //          : (Filter == 2) ? !(from z in db.StockBasic
        //                              select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
        //          : 1 == 1)
        //          select new
        //          {
        //              ProtoNomor = p.StockProductDJ.ProtoNomor,
        //              ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.ProductItem,
        //              ProductCategory = p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory,
        //              ProductSegmen = p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen,
        //              TipeLokasi = q.SalesOrder.TipeLokasi.Value,
        //              IDLokasi = q.SalesOrder.IDLokasi.Value,
        //              NoSO = q.SalesOrder.Nomor,
        //              CustomerNama = q.SalesOrder.CustomerNama,
        //              Invoice = q.Invoice,
        //              Nomor = q.StockProductDJ.Nomor,
        //              StatusVoid = q.SalesOrder.StatusVoid.Value,
        //              Tgl = q.SalesOrder.Tgl.Value,
        //              Stone1B = p.Stone1B,
        //              TotalButir = p.TotalButir,
        //              TotalCarat = p.TotalCarat,
        //              GIA = p.GIA,
        //              HargaRupiah = q.HargaRupiah.Value,
        //              HargaM = q.StockProductDJ.StockProductDJ_Costing.TotalBiaya + q.StockProductDJ.StockProductDJ_CostingProduct.Total,
        //              HargaTotal = (p.HargaTotal.Value > 0 ) ? p.HargaTotal.Value : 1
        //          }).Union(
        //          from p in db.SalesOrderLD
        //          where !p.SalesOrder.Draft
        //              //&& p.SalesOrder.StatusPembayaran == false
        //          && p.SalesOrder.StatusPembayaran == true
        //          select new
        //          {
        //              ProtoNomor = "-",
        //              ProductItem = DLC.ProductItem(),
        //              ProductCategory = DLC.ProductCategory(),
        //              ProductSegmen = DLC.ProductSegmen(),
        //              TipeLokasi = p.SalesOrder.TipeLokasi.Value,
        //              IDLokasi = p.SalesOrder.IDLokasi.Value,
        //              NoSO = p.SalesOrder.Nomor,
        //              CustomerNama = p.SalesOrder.CustomerNama,
        //              Invoice = p.Invoice,
        //              Nomor = p.StockProductLD_Stone1B.Nomor,
        //              StatusVoid = p.SalesOrder.StatusVoid.Value,
        //              Tgl = p.SalesOrder.Tgl.Value,
        //              Stone1B = p.StockProductLD_Stone1B.Stone1B,
        //              TotalButir = p.StockProductLD_Stone1B.TotalButir,
        //              TotalCarat = p.StockProductLD_Stone1B.TotalCarat,
        //              GIA = p.StockProductLD_Stone1B.GIA,
        //              HargaRupiah = p.HargaRupiah.Value,
        //              HargaM = p.StockProductLD_Stone1B.HargaTotalM.Value,
        //              HargaTotal = (p.StockProductLD_Stone1B.HargaTotal.Value > 0) ? p.StockProductLD_Stone1B.HargaTotal.Value : 1
        //          });

        var rs = (from p in db.StockProductDJ_Stone1A
                  join q in db.SalesOrderDJ on p.IDForm equals q.IDProduct
                  where q.StockProductDJ.Legacy.Value == null && !q.SalesOrder.Draft
                  && q.StockProductDJ.StockProductDJ_Stone1A.Count() > 0
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
                      ProtoNomor = p.StockProductDJ.ProtoNomor,
                      ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.ProductItem,
                      ProductCategory = p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory,
                      ProductSegmen = p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen,
                      TipeLokasi = q.SalesOrder.TipeLokasi.Value,
                      IDLokasi = q.SalesOrder.IDLokasi.Value,
                      NoSO = q.SalesOrder.Nomor,
                      CustomerNama = q.SalesOrder.CustomerNama,
                      Invoice = q.Invoice,
                      Nomor = q.StockProductDJ.Nomor,
                      StatusVoid = q.SalesOrder.StatusVoid.Value,
                      Tgl = q.SalesOrder.Tgl.Value,
                      Brand = "-",
                      Colour = p.Stone1A.Parcel103.Nama,
                      Clarity = p.Stone1A.Parcel104.Nama,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      CaratButir = p.CaratButir,
                      GIA = "",
                      Tipe = "Reguler",
                      HargaRupiah = q.HargaRupiah.Value,
                      //HargaM = q.StockProductDJ.StockProductDJ_Costing.TotalBiaya + q.StockProductDJ.StockProductDJ_CostingProduct.Total,
                      HargaM = p.HargaTotalM.Value,
                      HargaR = p.HargaTotal.Value,
                      HargaTotal = (p.HargaTotal.Value > 0) ? p.HargaTotal.Value : 1,
                      HargaMJual = 0m,
                      HargaRJual = 0m,
                      DiscMJual = 0m,
                      DiscRJual = 0m
                  }).Union(from p in db.StockProductDJ_Stone1B
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
                      ProtoNomor = p.StockProductDJ.ProtoNomor,
                      ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.ProductItem,
                      ProductCategory = p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory,
                      ProductSegmen = p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen,
                      TipeLokasi = q.SalesOrder.TipeLokasi.Value,
                      IDLokasi = q.SalesOrder.IDLokasi.Value,
                      NoSO = q.SalesOrder.Nomor,
                      CustomerNama = q.SalesOrder.CustomerNama,
                      Invoice = q.Invoice,
                      Nomor = q.StockProductDJ.Nomor,
                      StatusVoid = q.SalesOrder.StatusVoid.Value,
                      Tgl = q.SalesOrder.Tgl.Value,
                      Brand = p.Stone1B.Parcel106.Nama,
                      Colour = p.Stone1B.Parcel103.Nama,
                      Clarity = p.Stone1B.Parcel104.Nama,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      CaratButir = p.CaratButir,
                      GIA = p.GIA,
                      Tipe = "SNI",
                      HargaRupiah = q.HargaRupiah.Value,
                      //HargaM = q.StockProductDJ.StockProductDJ_Costing.TotalBiaya + q.StockProductDJ.StockProductDJ_CostingProduct.Total,
                      HargaM = p.HargaTotalM.Value,
                      HargaR = p.HargaTotal.Value,
                      HargaTotal = (p.HargaTotal.Value > 0) ? p.HargaTotal.Value : 1,
                      HargaMJual = p.HargaMJual.Value,
                      HargaRJual = p.HargaRJual.Value,
                      DiscMJual = p.DiscMJual.Value,
                      DiscRJual = p.DiscRJual.Value
                  }).Union(
                  from p in db.SalesOrderLD
                  where !p.SalesOrder.Draft
                      //&& p.SalesOrder.StatusPembayaran == false
                  && p.SalesOrder.StatusPembayaran == true
                  select new
                  {
                      ProtoNomor = "-",
                      ProductItem = DLC.ProductItem(),
                      ProductCategory = DLC.ProductCategory(),
                      ProductSegmen = DLC.ProductSegmen(),
                      TipeLokasi = p.SalesOrder.TipeLokasi.Value,
                      IDLokasi = p.SalesOrder.IDLokasi.Value,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      Invoice = p.Invoice,
                      Nomor = p.StockProductLD_Stone1B.Nomor,
                      StatusVoid = p.SalesOrder.StatusVoid.Value,
                      Tgl = p.SalesOrder.Tgl.Value,
                      Brand = p.StockProductLD_Stone1B.Stone1B.Parcel106.Nama,
                      Colour = p.StockProductLD_Stone1B.Stone1B.Parcel103.Nama,
                      Clarity = p.StockProductLD_Stone1B.Stone1B.Parcel104.Nama,
                      TotalButir = p.StockProductLD_Stone1B.TotalButir,
                      TotalCarat = p.StockProductLD_Stone1B.TotalCarat,
                      CaratButir = p.StockProductLD_Stone1B.CaratButir,
                      GIA = p.StockProductLD_Stone1B.GIA,
                      Tipe = "SNI",
                      HargaRupiah = p.HargaRupiah.Value,
                      HargaM = p.StockProductLD_Stone1B.HargaTotalM.Value,
                      HargaR = p.StockProductLD_Stone1B.HargaTotal.Value,
                      HargaTotal = (p.StockProductLD_Stone1B.HargaTotal.Value > 0) ? p.StockProductLD_Stone1B.HargaTotal.Value : 1,
                      HargaMJual = p.StockProductLD_Stone1B.HargaMJual.Value,
                      HargaRJual = p.StockProductLD_Stone1B.HargaRJual.Value,
                      DiscMJual = p.StockProductLD_Stone1B.DiscMJual.Value,
                      DiscRJual = p.StockProductLD_Stone1B.DiscRJual.Value
                  });

        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductSegmen = Convert.ToInt32(Request.QueryString["productsegmen"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int Payment = Convert.ToInt32(Request.QueryString["payment"]);
        int ProductType = Convert.ToInt32(Request.QueryString["producttype"]);

        if (ProductItem > 0)
            rs = rs.Where(p => p.ProductItem == ProductItem);
        if (ProductCategory > 0)
            rs = rs.Where(p => p.ProductCategory == ProductCategory);
        if (ProductSegmen > 0)
            rs = rs.Where(p => p.ProductSegmen == ProductSegmen);
        if (ProductType == 1)
            rs = rs.Where(p => p.CaratButir < 0.3m);
        else if (ProductType == 2)
            rs = rs.Where(p => p.CaratButir >= 0.3m && p.CaratButir < 0.9m);
        else if (ProductType == 3)
            rs = rs.Where(p => p.CaratButir >= 0.9m);

        if (Tipe != 0)
            rs = rs.Where(p => p.TipeLokasi == Tipe);
        if (IDLokasi != 0)
            rs = rs.Where(p => p.IDLokasi == IDLokasi);

        rs = rs.OrderByDescending(p => p.Tgl).ThenBy(p => p.NoSO).ThenBy(p=>p.Nomor);

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
            string NoPLU = "";
            decimal TotalHargaRapaport = 0m;
            decimal TotalHargaM = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalHargaJual = 0m;

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

                if (NoPLU != r.Nomor)
                {
                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);
                }
                else
                {
                    td = new TableCell();
                    td.Text = "<p></p>";
                    tr.Cells.Add(td);
                }

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
                td.Text = r.Brand;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Colour;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Clarity;
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

                decimal HargaM = (r.HargaM);

                if (NoPLU != r.Nomor)
                {

                    td = new TableCell();
                    td.Text = r.HargaRupiah.ToString("N0");
                    tr.Cells.Add(td);

                    TotalHargaRapaport += r.HargaTotal;
                    TotalHargaM += (r.HargaM);
                    
                    TotalHargaJual += r.HargaRupiah;
                }
                else
                {
                    td = new TableCell();
                    td.Text = "-";
                    tr.Cells.Add(td);
                }
                NoPLU = r.Nomor;

                TotalButir += r.TotalButir;
                TotalCarat += r.TotalCarat;

                //td = new TableCell();
                //td.Text = "<b>" + Convert.ToDecimal(((r.HargaTotal - HargaM) / r.HargaTotal) * 100).ToString("N0") + " %</b>";
                //tr.Cells.Add(td);

                decimal HargaRJual = 0m;

                td = new TableCell();
                td.Text = SegmenNama;
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
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
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalHargaJual.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);



            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];

            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productsegmen.SelectedValue = Request.QueryString["productsegmen"];
            filter.SelectedValue = Request.QueryString["filter"];
            status.SelectedValue = Request.QueryString["status"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            lokasi.Text = Request.QueryString["lokasi"];
            payment.SelectedValue = Request.QueryString["payment"];
            producttype.SelectedValue = Request.QueryString["producttype"];
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
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;
        href += "&producttype=" + producttype.SelectedValue;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "SalesRekapSNI_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productsegmen=" + productsegmen.Text;
        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;
        href += "&producttype=" + producttype.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "SalesRekapSNI_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productsegmen=" + productsegmen.Text;
        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;
        href += "&producttype=" + producttype.SelectedValue;

        Response.Redirect(href);
    }
    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = 1;
        if (Tipe == 1 || Tipe == 2)
        {
            var am = from p in db.DataAreaManager
                     where p.IDUser == App.UserID
                     select p;
            if (am.Count() > 0)
            {
                DLC.LocationListAM(lokasi,App.UserID);
            }
        }
        lokasi.Focus();
    }
}