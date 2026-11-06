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

        var rs = (from p in db.SalesOrderDJ
                  where p.StockProductDJ.Legacy.Value == null && !p.SalesOrder.Draft
                  && !(from z in db.SalesOrderDJ
                         where z.StockProductDJ.Legacy.Value == 1
                         select z.SalesOrder.ID).Contains(p.IDForm)
                  //&& p.SalesOrder.StatusPembayaran == false
                  && p.SalesOrder.StatusPembayaran == true
                  && ((Filter == 1) ?
                  (from z in db.StockBasic
                   select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                  : (Filter == 2) ? !(from z in db.StockBasic
                                     select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                  : 1 == 1)
                  select new
                  {
                      SalesOrder = p.SalesOrder,
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Diamond Jewellery",
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      Butir = Convert.ToDecimal(((p.StockProductDJ.StockProductDJ_Stone1A.Count()>0) ? p.StockProductDJ.StockProductDJ_Stone1A.Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone1B.Where(y => y.StatusTerpasang == 1).Count()>0) ? p.StockProductDJ.StockProductDJ_Stone1B.Where(y => y.StatusTerpasang == 1).Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone2.Count()>0) ? p.StockProductDJ.StockProductDJ_Stone2.Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone3.Count()>0) ? p.StockProductDJ.StockProductDJ_Stone3.Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone4.Count()>0) ? p.StockProductDJ.StockProductDJ_Stone4.Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone5.Count()>0) ? p.StockProductDJ.StockProductDJ_Stone5.Sum(z => z.TotalButir) : 0m)),
                      Carat = Convert.ToDecimal(((p.StockProductDJ.StockProductDJ_Stone1A.Count()>0) ? p.StockProductDJ.StockProductDJ_Stone1A.Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone1B.Where(y => y.StatusTerpasang == 1).Count()>0) ? p.StockProductDJ.StockProductDJ_Stone1B.Where(y => y.StatusTerpasang == 1).Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone2.Count()>0) ? p.StockProductDJ.StockProductDJ_Stone2.Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone3.Count()>0) ? p.StockProductDJ.StockProductDJ_Stone3.Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone4.Count()>0) ? p.StockProductDJ.StockProductDJ_Stone4.Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone5.Count()>0) ? p.StockProductDJ.StockProductDJ_Stone5.Sum(z => z.TotalCarat) : 0m)),
                      Nomor = p.StockProductDJ.Nomor,
                      ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                      TotalBayar = p.TotalBayar.HasValue ? p.TotalBayar.Value : 0m,
                      Discount = p.Discount.HasValue ? p.Discount.Value : 0m,
                      DiscountProgram = p.DiscountProgram.HasValue ? p.DiscountProgram.Value : 0m,
                      DiscountGift = p.DiscountGift.HasValue ? p.DiscountGift.Value : 0m,
                      DiscountRound = p.DiscountRound.HasValue ? p.DiscountRound.Value : 0m,
                      NetWeight = p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight,
                      IDSO = p.SalesOrder.ID,
                      Category = p.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.Nama,
                      Harga = p.HargaRupiah.HasValue ? p.HargaRupiah.Value : 0m,
                      HargaM = (p.StockProductDJ.StockProductDJ_Costing.TotalBiaya + p.StockProductDJ.StockProductDJ_CostingProduct.Total),
                      HargaR = p.StockProductDJ.StockProductDJ_PricingMU.ModalUSD,
                      ProductCategory = p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory,
                      ProductSegmen = p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen,
                      IDItem = p.StockProductDJ.StockProductDJ_CharProduct.ProductItem,
                      StoneM = (p.StockProductDJ.StockProductDJ_CostingProduct.Total - p.StockProductDJ.StockProductDJ_CostingProduct.TotalProduct) *p.StockProductDJ.StockProductDJ_Costing.TotalRate.Value,
                      FrameM = p.StockProductDJ.StockProductDJ_CostingProduct.TotalProduct * p.StockProductDJ.StockProductDJ_Costing.TotalRate.Value,
                      Ongkos = ((p.StockProductDJ.StockProductDJ_Costing.MountingNO.Value + p.StockProductDJ.StockProductDJ_Costing.MountingRO)),
                  }).Concat(
                  from p in db.SalesOrderGJ
                  //where p.SalesOrder.StatusPembayaran == false
                  where !p.SalesOrder.Draft
                  && p.SalesOrder.StatusPembayaran == true
                  && ((Filter == 1) ?
                  (from z in db.StockBasic
                   select z.ProtoNomor).Contains((p.StockProductGJ.ProtoNomor.Contains("-") ? p.StockProductGJ.ProtoNomor.Substring(0, p.StockProductGJ.ProtoNomor.IndexOf('-')) : p.StockProductGJ.ProtoNomor))
                  : (Filter == 2) ? !(from z in db.StockBasic
                                      select z.ProtoNomor).Contains((p.StockProductGJ.ProtoNomor.Contains("-") ? p.StockProductGJ.ProtoNomor.Substring(0, p.StockProductGJ.ProtoNomor.IndexOf('-')) : p.StockProductGJ.ProtoNomor))
                  : 1 == 1)
                  select new
                  {
                      SalesOrder = p.SalesOrder,
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Gold Jewellery",
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      Butir = 0m,
                      Carat = 0m,
                      Nomor = p.StockProductGJ.Nomor,
                      ProductItem = p.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
                      TotalBayar = p.TotalBayar.HasValue ? p.TotalBayar.Value : 0m,
                      Discount = p.Discount.HasValue ? p.Discount.Value : 0m,
                      DiscountProgram = p.DiscountProgram.HasValue ? p.DiscountProgram.Value : 0m,
                      DiscountGift = p.DiscountGift.HasValue ? p.DiscountGift.Value : 0m,
                      DiscountRound = p.DiscountRound.HasValue ? p.DiscountRound.Value : 0m,
                      NetWeight = p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight,
                      IDSO = p.SalesOrder.ID,
                      Category = "Gold Jewellery",
                      Harga = p.HargaRupiah.HasValue ? p.HargaRupiah.Value : 0m,
                      HargaM = (p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total),
                      HargaR = p.StockProductGJ.StockProductGJ_PricingMU.ModalUSD,
                      ProductCategory = p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory,
                      ProductSegmen = p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen,
                      IDItem = p.StockProductGJ.StockProductGJ_CharProduct.ProductItem,
                      StoneM = 0m,
                      FrameM = p.StockProductGJ.StockProductGJ_CostingProduct.TotalProduct * p.StockProductGJ.StockProductGJ_Costing.TotalRate.Value,
                      Ongkos = ((p.StockProductGJ.StockProductGJ_Costing.MountingNO.Value + p.StockProductGJ.StockProductGJ_Costing.MountingRO)),
                  }).Concat(
                  from p in db.SalesOrderPG
                  //where p.SalesOrder.StatusPembayaran == false
                  where !(from z in db.SalesOrderPG
                       where z.StockProductPG.Legacy.Value == 1
                       select z.SalesOrder.ID).Contains(p.IDForm)
                  && !p.SalesOrder.Draft
                  && p.SalesOrder.StatusPembayaran == true
                  && ((Filter == 1) ? 1 == 0 : 1 == 1)
                  select new
                  {
                      SalesOrder = p.SalesOrder,
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Plain Gold",
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      Butir = 0m,
                      Carat = 0m,
                      Nomor = p.StockProductPG.Nomor,
                      ProductItem = p.StockProductPG.CharProductItem.Nama,
                      TotalBayar = p.TotalBayar.HasValue ? p.TotalBayar.Value : 0m,
                      Discount = p.Discount.HasValue ? p.Discount.Value : 0m,
                      DiscountProgram = p.DiscountProgram.HasValue ? p.DiscountProgram.Value : 0m,
                      DiscountGift = p.DiscountGift.HasValue ? p.DiscountGift.Value : 0m,
                      DiscountRound = p.DiscountRound.HasValue ? p.DiscountRound.Value : 0m,
                      NetWeight = p.StockProductPG.GrossWeight.Value,
                      IDSO = p.SalesOrder.ID,
                      Category = "Plain Gold",
                      Harga = p.HargaRupiah.HasValue ? p.HargaRupiah.Value : 0m,
                      HargaM = (p.StockProductPG.TotalHarga24.Value / p.StockProductPG.TotalRate.Value),
                      HargaR = 0m,
                      ProductCategory = 4,
                      ProductSegmen = 0,
                      IDItem = 0,
                      StoneM = 0m,
                      FrameM = p.StockProductPG.TotalHargaUSD.Value * p.StockProductPG.TotalRate.Value,
                      Ongkos = 0m,
                  })
                  .Concat(
                  from p in db.SalesOrderLD
                  //where p.SalesOrder.StatusPembayaran == false
                  where !p.SalesOrder.Draft
                  && p.SalesOrder.StatusPembayaran == true
                  && ((Filter == 1) ? 1 == 0 : 1 == 1)
                  select new
                  {
                      SalesOrder = p.SalesOrder,
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Loose Diamond",
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      Butir = Convert.ToDecimal(p.StockProductLD_Stone1B.TotalButir),
                      Carat = Convert.ToDecimal(p.StockProductLD_Stone1B.TotalCarat),
                      Nomor = p.StockProductLD_Stone1B.Nomor,
                      ProductItem = "-",
                      TotalBayar = p.TotalBayar.HasValue ? p.TotalBayar.Value : 0m,
                      Discount = p.Discount.HasValue ? p.Discount.Value : 0m,
                      DiscountProgram = p.DiscountProgram.HasValue ? p.DiscountProgram.Value : 0m,
                      DiscountGift = p.DiscountGift.HasValue ? p.DiscountGift.Value : 0m,
                      DiscountRound = p.DiscountRound.HasValue ? p.DiscountRound.Value : 0m,
                      NetWeight = 0m,
                      IDSO = p.SalesOrder.ID,
                      Category = "Loose Diamond",
                      Harga = p.HargaRupiah.HasValue ? p.HargaRupiah.Value : 0m,
                      HargaM = (p.StockProductLD_Stone1B.HargaTotalM.Value),
                      HargaR = 0m,
                      ProductCategory = 14,
                      ProductSegmen = 0,
                      IDItem = 0,
                      StoneM = p.StockProductLD_Stone1B.HargaTotalM.HasValue ? p.StockProductLD_Stone1B.HargaTotalM.Value : 0m,
                      FrameM = 0m,
                      Ongkos = 0m,
                  })
                  ;

        int TipeProduct = 0;
        TipeProduct = Convert.ToInt32(Request.QueryString["tipeproduct"]);
        if (TipeProduct > 0)
        {
            if (TipeProduct == 1)
                rs = rs.Where(p => p.Tipe == "Diamond Jewellery");
            else if (TipeProduct == 2)
                rs = rs.Where(p => p.Tipe == "Gold Jewellery");
            else if (TipeProduct == 3)
                rs = rs.Where(p => p.Tipe == "Plain Gold");
            else if (TipeProduct == 4)
                rs = rs.Where(p => p.Tipe == "Loose Diamond");
        }

        int Payment = Convert.ToInt32(Request.QueryString["payment"]);

        if (Tipe != 0)
            rs = rs.Where(p => p.TipeLokasi == Tipe);
        if (IDLokasi != 0)
            rs = rs.Where(p => p.IDLokasi == IDLokasi);

        rs = rs.OrderByDescending(p => p.Tgl).ThenBy(p=>p.NoSO);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.NoSO == "" + keyword + "" || p.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text) || p.Nomor.Contains(kw.Text));
        
        if(status.SelectedValue=="1")
            rs = rs.Where(p=> p.StatusVoid == true);
        else if (status.SelectedValue == "2")
            rs = rs.Where(p => p.StatusVoid == false);
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var rsg = rs.GroupBy(p => p.ProductCategory);

        if (rs.Count() > 0)
        {
            TableRow tr3 = new TableRow();
            TableCell td3;

            td3 = new TableCell();
            td3.Text = "";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "<b>Stone</b>";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "<b>Frame</b>";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "<b>Ongkos</b>";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "";
            tr3.Cells.Add(td3);

            td3 = new TableCell();
            td3.Text = "";
            tr3.Cells.Add(td3);

            tb.Rows.Add(tr3);

            decimal TotalHarga = 0m;
            decimal TotalHargaRupiah = 0m;
            decimal TotalHargaM = 0m;
            decimal TotalHargaR = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalNetWeight = 0m;
            decimal TotalBayar1 = 0m;
            decimal TotalBayar2 = 0m;
            decimal TotalBayar3 = 0m;
            decimal TotalBayar4 = 0m;
            decimal TotalBayar5 = 0m;
            decimal TotalBayar6 = 0m;
            decimal TotalBayar7 = 0m;
            decimal TotalBayar8 = 0m;
            decimal TotalHPP = 0m;
            decimal TotalGP = 0m;
            decimal TotalStoneM = 0m;
            decimal TotalFrameM = 0m;
            decimal TotalOngkos = 0m;
            int prev = 0;
            int index = 1;
            string SO = "";
            foreach (var r in rsg)
            {
                

                decimal Butir = 0m;
                decimal Carat = 0m;
                int Qty = r.Count();

                Butir = r.Sum(p=>p.Butir);
                Carat = r.Sum(p=>p.Carat);

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                string ProductCategory = "";
                ProductCategory = Db.SingleString("SELECT Nama FROM CharProductCategory WHERE ID=" + r.Key, Db.Cnn_DB);

                td = new TableCell();
                td.Text = "<p>" + ProductCategory + "</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Sum(p=>p.TotalBayar).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Sum(p=>p.StoneM).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Sum(p=>p.FrameM).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Sum(p=>p.Ongkos).ToString("N0");
                tr.Cells.Add(td);

                decimal HPP = 0m;

                HPP = r.Sum(p=>p.Ongkos) + r.Sum(p=>p.FrameM) + r.Sum(p=>p.StoneM);

                td = new TableCell();
                td.Text = HPP.ToString("N0");
                tr.Cells.Add(td);

                decimal GP = 0m;

                GP = r.Sum(p=>p.TotalBayar) - HPP;

                td = new TableCell();
                td.Text = GP.ToString("N0");
                tr.Cells.Add(td);

                decimal Persen = 0m;

                Persen = (GP / (r.Sum(p=>p.TotalBayar) > 0 ? r.Sum(p=>p.TotalBayar) : 1)) * 100;

                td = new TableCell();
                td.Text = Persen.ToString("N0") + "%";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalHarga += r.Sum(p=>p.TotalBayar);
                TotalHargaRupiah += r.Sum(p=>p.Harga);
                TotalHargaM += r.Sum(p=>p.HargaM);
                TotalHargaR += r.Sum(p=>p.HargaR);
                TotalDiscount += r.Sum(p=>p.Discount);
                TotalDiscountProgram += r.Sum(p=>p.DiscountProgram);
                TotalDiscountGift += r.Sum(p=>p.DiscountGift);
                TotalDiscountRound += r.Sum(p=>p.DiscountRound);
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalNetWeight += r.Sum(p=>p.NetWeight);
                TotalStoneM += r.Sum(p=>p.StoneM);
                TotalFrameM += r.Sum(p=>p.FrameM);
                TotalOngkos += r.Sum(p=>p.Ongkos);
                TotalHPP += HPP;
                TotalGP += GP;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 2;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalStoneM).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalFrameM).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalOngkos).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHPP).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalGP).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];

            filter.SelectedValue = Request.QueryString["filter"];
            status.SelectedValue = Request.QueryString["status"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            tipelokasi.Text = Request.QueryString["tipelokasi"];
            lokasi.Text = Request.QueryString["lokasi"];
            payment.SelectedValue = Request.QueryString["payment"];
            tipeproduct.SelectedValue = Request.QueryString["tipeproduct"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;
        href += "&tipeproduct=" + tipeproduct.SelectedValue;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "ProfitRekap_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;
        href += "&tipeproduct=" + tipeproduct.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "ProfitRekap_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&filter=" + filter.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&payment=" + payment.SelectedValue;
        href += "&tipeproduct=" + tipeproduct.SelectedValue;

        Response.Redirect(href);
    }
    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            ListItem item = new ListItem("All", "0");
            lokasi.Items.Add(item);
            DLC.LocationList(lokasi, Tipe);
        }
        lokasi.Focus();
    }
}