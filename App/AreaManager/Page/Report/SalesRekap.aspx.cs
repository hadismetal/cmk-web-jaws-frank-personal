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
                      ProductCategory = 998,
                      ProductSegmen = 0,
                      IDItem = 0,
                  })
                  ;

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.SalesOrder.IDLokasi.Value) && p.SalesOrder.TipeLokasi == 1);
        }

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

        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductSegmen = Convert.ToInt32(Request.QueryString["productsegmen"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int Payment = Convert.ToInt32(Request.QueryString["payment"]);

        if (ProductItem > 0)
            rs = rs.Where(p => p.IDItem == ProductItem);
        if (ProductCategory > 0)
            rs = rs.Where(p => p.ProductCategory == ProductCategory);
        if (ProductSegmen > 0)
            rs = rs.Where(p => p.ProductSegmen == ProductSegmen);

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

        if (rs.Count() > 0)
        {
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
            int prev = 0;
            int index = 1;
            string SO = "";
            foreach (var r in rs)
            {
                

                decimal Butir = 0m;
                decimal Carat = 0m;
                int Qty = 1;

                Butir = r.Butir;
                Carat = r.Carat;

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
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

                string Lokasi = "";
                if (r.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                else if (r.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Tipe;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProductItem;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Category;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CustomerNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Harga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Discount.ToString("N0") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountProgram.ToString("N0") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountGift.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountRound.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.TotalBayar.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Butir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Carat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NetWeight.ToString("N2");
                tr.Cells.Add(td);

                var so = db.SalesOrder.SingleOrDefault(p => p.ID.Equals(r.IDSO));
                if (SO != r.NoSO)
                {
                    decimal Bayar1 = (Payment == 1 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar2 = (Payment == 2 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar3 = (Payment == 3 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar4 = (Payment == 4 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar5 = (Payment == 5 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 5).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar6 = (Payment == 6 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 6).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar7 = (Payment == 7 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 7).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar8 = (Payment == 8 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 8).Sum(o => o.Nominal))) : 0m;

                    TotalBayar1 += (Payment == 1 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal))) : 0m;
                    TotalBayar2 += (Payment == 2 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal))) : 0m;
                    TotalBayar3 += (Payment == 3 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal))) : 0m;
                    TotalBayar4 += (Payment == 4 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal))) : 0m;
                    TotalBayar5 += (Payment == 5 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 5).Sum(o => o.Nominal))) : 0m;
                    TotalBayar6 += (Payment == 6 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 6).Sum(o => o.Nominal))) : 0m;
                    TotalBayar7 += (Payment == 7 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 7).Sum(o => o.Nominal))) : 0m;
                    TotalBayar8 += (Payment == 8 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 8).Sum(o => o.Nominal))) : 0m;

                    td = new TableCell();
                    td.Text = Bayar1.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar2.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar3.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar4.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar5.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar6.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar7.ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar8.ToString("N0");
                    tr.Cells.Add(td);

                    string Bank = "-";
                    int counter = 1;
                    foreach (var u in so.SalesReceipt)
                    {
                        foreach (var t in u.SalesReceiptDetail)
                        {
                            if (t.IDBankIssuer.HasValue)
                            {
                                if (counter == 1)
                                    Bank = t.BankIssuer.Nama;
                                else
                                {
                                    Bank += ", " + t.BankIssuer.Nama;
                                    counter++;
                                }
                            }
                        }
                    }

                    td = new TableCell();
                    td.Text = Bank;
                    tr.Cells.Add(td);
                }
                else
                {
                    string Bank = "-";
                    int counter = 1;
                    foreach (var u in so.SalesReceipt)
                    {
                        foreach (var t in u.SalesReceiptDetail)
                        {
                            if (t.IDBankIssuer.HasValue)
                            {
                                if (counter == 1)
                                    Bank = t.BankIssuer.Nama;
                                else
                                {
                                    Bank += ", " + t.BankIssuer.Nama;
                                    counter++;
                                }
                            }
                        }
                    }

                    td = new TableCell();
                    td.Text = "";
                    td.ColumnSpan = 8;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bank;
                    tr.Cells.Add(td);
                }
                SO = r.NoSO;

                tb.Rows.Add(tr);

                TotalHarga += r.TotalBayar;
                TotalHargaRupiah += r.Harga;
                TotalHargaM += r.HargaM;
                TotalHargaR += r.HargaR;
                TotalDiscount += r.Discount;
                TotalDiscountProgram += r.DiscountProgram;
                TotalDiscountGift += r.DiscountGift;
                TotalDiscountRound += r.DiscountRound;
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalNetWeight += r.NetWeight;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 11;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHargaRupiah).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount/rs.Count()).ToString("N0") + " %</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0") + " %</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalCarat).ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalNetWeight).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar1).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar2).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar3).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar4).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar5).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar6).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar7).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar8).ToString("N0") + "</b>";
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
            tipeproduct.SelectedValue = Request.QueryString["tipeproduct"];
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
        href += "&tipeproduct=" + tipeproduct.SelectedValue;

        Response.Redirect(href);
    }
    protected void Export(object sender, EventArgs e)
    {
        string href = "SalesRekap_Export.aspx?id=1";

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
        href += "&tipeproduct=" + tipeproduct.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "SalesRekap_Print.aspx?id=1";

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
        href += "&tipeproduct=" + tipeproduct.SelectedValue;

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
                DLC.LocationListAM(lokasi, App.UserID);
            }
        }
        lokasi.Focus();
    }
}