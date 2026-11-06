using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

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

        int Tipe = -1;

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

            if (Request.QueryString["tipelokasi"] != "")
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);

            DLC.LocationList(lokasi, Tipe);

            status.SelectedValue = "2";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }

    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        int Filter = Convert.ToInt32(Request.QueryString["filter"]);

        var rs = (from p in db.SalesOrderDJ
                  where p.StockProductDJ.Legacy.Value == null && !p.SalesOrder.Draft
                  && !(from z in db.SalesOrderDJ
                       where z.StockProductDJ.Legacy.Value == 1
                       select z.SalesOrder.ID).Contains(p.IDForm)
                      //&& p.SalesOrder.StatusPembayaran == false
                  && p.SalesOrder.StatusPembayaran == true
                  && p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1)
                  && ((Filter == 1) ?
                  (from z in db.StockBasic
                   select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                  : (Filter == 2) ? !(from z in db.StockBasic
                                      select z.ProtoNomor).Contains((p.StockProductDJ.ProtoNomor.Contains("-") ? p.StockProductDJ.ProtoNomor.Substring(0, p.StockProductDJ.ProtoNomor.IndexOf('-')) : p.StockProductDJ.ProtoNomor))
                  : 1 == 1)
                  select new
                  {
                      ProtoNomor = p.StockProductDJ.ProtoNomor,
                      Supplier = p.StockProductDJ.Supplier.Value,
                      SalesOrder = p.SalesOrder,
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama + " (" + p.SalesOrder.DataCustomer.NoCustomer + ")",
					  CustomerReferenceNama = p.SalesOrder.CustomerReferenceNama,
					  Membership = Db.SingleString("SELECT TOP 1 ISNULL(Membership,'-') [Membership] FROM SalesOrder WHERE ID = " + p.SalesOrder.ID, Db.Cnn_DB),
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Diamond Jewellery",
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      Butir = Convert.ToDecimal(((p.StockProductDJ.StockProductDJ_Stone1A.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone1A.Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone1B.Where(y => y.StatusTerpasang == 1).Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone1B.Where(y => y.StatusTerpasang == 1).Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone2.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone2.Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone3.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone3.Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone4.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone4.Sum(z => z.TotalButir) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone5.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone5.Sum(z => z.TotalButir) : 0m)),
                      Carat = Convert.ToDecimal(((p.StockProductDJ.StockProductDJ_Stone1A.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone1A.Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone1B.Where(y => y.StatusTerpasang == 1).Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone1B.Where(y => y.StatusTerpasang == 1).Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone2.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone2.Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone3.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone3.Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone4.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone4.Sum(z => z.TotalCarat) : 0m) + ((p.StockProductDJ.StockProductDJ_Stone5.Count() > 0) ? p.StockProductDJ.StockProductDJ_Stone5.Sum(z => z.TotalCarat) : 0m)),
                      Nomor = p.StockProductDJ.Nomor,
                      ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
					  ProductLevel = p.StockProductDJ.StockProductDJ_CharProduct.CharProductLevel.Nama,
                      TotalBayar = p.TotalBayar.HasValue ? p.TotalBayar.Value : 0m,
                      Discount = p.Discount.HasValue ? p.Discount.Value : 0m, //Discount Normal Percentage
                      DiscountProgram = p.DiscountProgram.HasValue ? p.DiscountProgram.Value : 0m, //Discount Membership Percentage
                      DiscountGift = p.DiscountGift.HasValue ? p.DiscountGift.Value : 0m, //Discount Voucher
                      DiscountRound = p.DiscountRound.HasValue ? p.DiscountRound.Value : 0m, //Discount Round
                      NetWeight = p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight,
                      IDSO = p.SalesOrder.ID,
                      IDSOitem =p.ID,
                      Category = p.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.Nama,
                      Segmen = p.StockProductDJ.StockProductDJ_PricingSegmen.CharProductSegmen.Nama,
                      Harga = p.HargaRupiah.HasValue ? p.HargaRupiah.Value : 0m,
                      DiscountNominal = p.DiscountNominal.Value, //Discount Normal
                      DiscountProgramNominal = p.DiscountProgramNominal.Value, //Discount Membership
                      DiscountPromo = p.DiscountPromo.HasValue ? p.DiscountPromo.Value : 0, //Discount Promo
					  DiscountBank = p.DiscountBank, //Discount Bank
					  DiscountOther = p.DiscountOther, //Discount Other
					  PointReceived = Db.SingleInteger("SELECT TOP 1 ISNULL(PoinReceived,0) [PoinReceived] FROM SalesOrderDJ WHERE ID = " + p.ID, Db.Cnn_DB),
                      HargaM = (p.StockProductDJ.StockProductDJ_Costing.TotalBiaya + p.StockProductDJ.StockProductDJ_CostingProduct.Total),
                      HargaR = p.StockProductDJ.StockProductDJ_PricingMU.ModalUSD,
                      ProductCategory = p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory,
                      ProductSegmen = (p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen != 0 ? p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen : 6),
                      IDItem = p.StockProductDJ.StockProductDJ_CharProduct.ProductItem,
                      KadarTukaranBeli = 0m,
					  Keterangan = p.SalesOrder.Keterangan,
					  TglVoid = p.SalesOrder.TglVoid,
					  KeteranganVoid = p.SalesOrder.KeteranganVoid,
					  ExtraRingSize = (Db.SingleDecimal("SELECT TOP 1 ISNULL(ExtraRingSize,0) FROM SalesOrderDJ WHERE ID = " + p.ID, Db.Cnn_DB)),
					  FeeExtraRingSizePerSize = (Db.SingleDecimal("SELECT TOP 1 ISNULL(FeeExtraRingSizePerSize,0) FROM SalesOrderDJ WHERE ID = " + p.ID, Db.Cnn_DB)),
					  TotalFeeExtraRingSize = (Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalFeeExtraRingSize,0) FROM SalesOrderDJ WHERE ID = " + p.ID, Db.Cnn_DB))
                  }).Concat(
                  from p in db.SalesOrderGJ
                  //where p.SalesOrder.StatusPembayaran == false
                  where !p.SalesOrder.Draft
                  && p.SalesOrder.StatusPembayaran == true
                  && p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1)
                  && ((Filter == 1) ?
                  (from z in db.StockBasic
                   select z.ProtoNomor).Contains((p.StockProductGJ.ProtoNomor.Contains("-") ? p.StockProductGJ.ProtoNomor.Substring(0, p.StockProductGJ.ProtoNomor.IndexOf('-')) : p.StockProductGJ.ProtoNomor))
                  : (Filter == 2) ? !(from z in db.StockBasic
                                      select z.ProtoNomor).Contains((p.StockProductGJ.ProtoNomor.Contains("-") ? p.StockProductGJ.ProtoNomor.Substring(0, p.StockProductGJ.ProtoNomor.IndexOf('-')) : p.StockProductGJ.ProtoNomor))
                  : 1 == 1)
                  select new
                  {
                      ProtoNomor = p.StockProductGJ.ProtoNomor,
                      Supplier = p.StockProductGJ.Supplier.Value,
                      SalesOrder = p.SalesOrder,
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama + " (" + p.SalesOrder.DataCustomer.NoCustomer + ")",
					  CustomerReferenceNama = p.SalesOrder.CustomerReferenceNama,
					  Membership = Db.SingleString("SELECT TOP 1 ISNULL(Membership,'-') [Membership] FROM SalesOrder WHERE ID = " + p.SalesOrder.ID, Db.Cnn_DB),
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Gold Jewellery",
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      Butir = 0m,
                      Carat = 0m,
                      Nomor = p.StockProductGJ.Nomor,
                      ProductItem = p.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
					  ProductLevel = p.StockProductGJ.StockProductGJ_CharProduct.CharProductLevel.Nama,
                      TotalBayar = p.TotalBayar.HasValue ? p.TotalBayar.Value : 0m,
                      Discount = p.Discount.HasValue ? p.Discount.Value : 0m,
                      DiscountProgram = p.DiscountProgram.HasValue ? p.DiscountProgram.Value : 0m,
                      DiscountGift = p.DiscountGift.HasValue ? p.DiscountGift.Value : 0m,
                      DiscountRound = p.DiscountRound.HasValue ? p.DiscountRound.Value : 0m,
                      NetWeight = p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight,
                      IDSO = p.SalesOrder.ID,
                      IDSOitem =p.ID,
                      Category = "Gold Jewellery",
                      Segmen = p.StockProductGJ.StockProductGJ_PricingSegmen.CharProductSegmen.Nama,
                      Harga = p.HargaRupiah.HasValue ? p.HargaRupiah.Value : 0m,
                      DiscountNominal = p.DiscountNominal.Value,
                      DiscountProgramNominal = p.DiscountProgramNominal.Value,
                      DiscountPromo = p.DiscountPromo.HasValue ? p.DiscountPromo.Value : 0,
					  DiscountBank = p.DiscountBank, //Discount Bank
					  DiscountOther = p.DiscountOther, //Discount Other
					  PointReceived = Db.SingleInteger("SELECT TOP 1 ISNULL(PoinReceived,0) [PoinReceived] FROM SalesOrderGJ WHERE ID = " + p.ID, Db.Cnn_DB),
                      HargaM = (p.StockProductGJ.StockProductGJ_Costing.TotalBiaya + p.StockProductGJ.StockProductGJ_CostingProduct.Total),
                      HargaR = p.StockProductGJ.StockProductGJ_PricingMU.ModalUSD,
                      ProductCategory = p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory,
                      ProductSegmen = (p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen != 0 ? p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen : 6),
                      IDItem = p.StockProductGJ.StockProductGJ_CharProduct.ProductItem,
                      KadarTukaranBeli = 0m,
					  Keterangan = p.SalesOrder.Keterangan,
					  TglVoid = p.SalesOrder.TglVoid,
					  KeteranganVoid = p.SalesOrder.KeteranganVoid,
					  ExtraRingSize = (Db.SingleDecimal("SELECT TOP 1 ISNULL(ExtraRingSize,0) FROM SalesOrderGJ WHERE ID = " + p.ID, Db.Cnn_DB)),
					  FeeExtraRingSizePerSize = (Db.SingleDecimal("SELECT TOP 1 ISNULL(FeeExtraRingSizePerSize,0) FROM SalesOrderGJ WHERE ID = " + p.ID, Db.Cnn_DB)),
					  TotalFeeExtraRingSize = (Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalFeeExtraRingSize,0) FROM SalesOrderGJ WHERE ID = " + p.ID, Db.Cnn_DB))
                  }).Concat(
                  from p in db.SalesOrderPG
                  //where p.SalesOrder.StatusPembayaran == false
                  where !(from z in db.SalesOrderPG
                          where z.StockProductPG.Legacy.Value == 1
                          select z.SalesOrder.ID).Contains(p.IDForm)
				  && p.StockProductPG.CharProductItem.Nama != "LM"
                  && !p.SalesOrder.Draft
                  && p.SalesOrder.StatusPembayaran == true
                  && p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1)
                  && ((Filter == 1) ? 1 == 0 : 1 == 1)
                  select new
                  {
                      ProtoNomor = p.StockProductPG.KodeBarang,
                      Supplier = p.StockProductPG.IDSupplier.Value,
                      SalesOrder = p.SalesOrder,
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama + " (" + p.SalesOrder.DataCustomer.NoCustomer + ")",
					  CustomerReferenceNama = p.SalesOrder.CustomerReferenceNama,
					  Membership = Db.SingleString("SELECT TOP 1 ISNULL(Membership,'-') [Membership] FROM SalesOrder WHERE ID = " + p.SalesOrder.ID, Db.Cnn_DB),
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Plain Gold",
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      Butir = 0m,
                      Carat = 0m,
                      Nomor = p.StockProductPG.Nomor,
                      ProductItem = p.StockProductPG.CharProductItem.Nama,
					  ProductLevel = p.StockProductPG.CharGoldLevel.Nama,
                      TotalBayar = p.TotalBayar.HasValue ? p.TotalBayar.Value : 0m,
                      Discount = p.Discount.HasValue ? p.Discount.Value : 0m,
                      DiscountProgram = p.DiscountProgram.HasValue ? p.DiscountProgram.Value : 0m,
                      DiscountGift = p.DiscountGift.HasValue ? p.DiscountGift.Value : 0m,
                      DiscountRound = p.DiscountRound.HasValue ? p.DiscountRound.Value : 0m,
                      NetWeight = p.StockProductPG.GrossWeight.Value,
                      IDSO = p.SalesOrder.ID,
                      IDSOitem =p.ID,
                      Category = "Plain Gold",
                      Segmen = "PG",
                      Harga = p.HargaRupiah.HasValue ? p.HargaRupiah.Value : 0m,
                      DiscountNominal = p.DiscountNominal.Value,
                      DiscountProgramNominal = p.DiscountProgramNominal.Value,
                      DiscountPromo = p.DiscountPromo.HasValue ? p.DiscountPromo.Value : 0,
					  DiscountBank = p.DiscountBank, //Discount Bank
					  DiscountOther = p.DiscountOther, //Discount Other
					  PointReceived = Db.SingleInteger("SELECT TOP 1 ISNULL(PoinReceived,0) [PoinReceived] FROM SalesOrderPG WHERE ID = " + p.ID, Db.Cnn_DB),
                      HargaM = (p.StockProductPG.TotalHarga24.Value / p.StockProductPG.TotalRate.Value),
                      HargaR = 0m,
                      ProductCategory = 4,
                      ProductSegmen = 0,
                      IDItem = 0,
                      KadarTukaranBeli = p.StockProductPG.KadarTukaranBeli.HasValue ? p.StockProductPG.KadarTukaranBeli.Value : 0m,
					  Keterangan = p.SalesOrder.Keterangan,
					  TglVoid = p.SalesOrder.TglVoid,
					  KeteranganVoid = p.SalesOrder.KeteranganVoid,
					  ExtraRingSize = (Db.SingleDecimal("SELECT TOP 1 ISNULL(ExtraRingSize,0) FROM SalesOrderPG WHERE ID = " + p.ID, Db.Cnn_DB)),
					  FeeExtraRingSizePerSize = (Db.SingleDecimal("SELECT TOP 1 ISNULL(FeeExtraRingSizePerSize,0) FROM SalesOrderPG WHERE ID = " + p.ID, Db.Cnn_DB)),
					  TotalFeeExtraRingSize = (Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalFeeExtraRingSize,0) FROM SalesOrderPG WHERE ID = " + p.ID, Db.Cnn_DB))
                  })
                  .Concat(
                  from p in db.SalesOrderLD
                  //where p.SalesOrder.StatusPembayaran == false
                  where !p.SalesOrder.Draft
                  && p.SalesOrder.StatusPembayaran == true
                  && p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1)
                  && ((Filter == 1) ? 1 == 0 : 1 == 1)
                  select new
                  {
                      ProtoNomor = "-",
                      Supplier = 0,
                      SalesOrder = p.SalesOrder,
                      SalesNama = p.SalesOrder.SalesNama,
                      StatusVoid = p.SalesOrder.StatusVoid,
                      Invoice = p.Invoice,
                      CustomerNama = p.SalesOrder.CustomerNama + " (" + p.SalesOrder.DataCustomer.NoCustomer + ")",
					  CustomerReferenceNama = p.SalesOrder.CustomerReferenceNama,
					  Membership = Db.SingleString("SELECT TOP 1 ISNULL(Membership,'-') [Membership] FROM SalesOrder WHERE ID = " + p.SalesOrder.ID, Db.Cnn_DB),
                      NoSO = p.SalesOrder.Nomor,
                      Tipe = "Loose Diamond",
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      Butir = Convert.ToDecimal(p.StockProductLD_Stone1B.TotalButir),
                      Carat = Convert.ToDecimal(p.StockProductLD_Stone1B.TotalCarat),
                      Nomor = p.StockProductLD_Stone1B.Nomor,
                      ProductItem = "-",
					  ProductLevel = "-",
                      TotalBayar = p.TotalBayar.HasValue ? p.TotalBayar.Value : 0m,
                      Discount = p.Discount.HasValue ? p.Discount.Value : 0m,
                      DiscountProgram = p.DiscountProgram.HasValue ? p.DiscountProgram.Value : 0m,
                      DiscountGift = p.DiscountGift.HasValue ? p.DiscountGift.Value : 0m,
                      DiscountRound = p.DiscountRound.HasValue ? p.DiscountRound.Value : 0m,
                      NetWeight = 0m,
                      IDSO = p.SalesOrder.ID,
                      IDSOitem =p.ID,
                      Category = "Loose Diamond",
                      Segmen = "LD",
                      Harga = p.HargaRupiah.HasValue ? p.HargaRupiah.Value : 0m,
                      DiscountNominal = p.DiscountNominal.Value,
                      DiscountProgramNominal = p.DiscountProgramNominal.Value,
                      DiscountPromo = p.DiscountPromo.HasValue ? p.DiscountPromo.Value : 0,
					  DiscountBank = p.DiscountBank, //Discount Bank
					  DiscountOther = p.DiscountOther, //Discount Other
					  PointReceived = Db.SingleInteger("SELECT TOP 1 ISNULL(PoinReceived,0) [PoinReceived] FROM SalesOrderLD WHERE ID = " + p.ID, Db.Cnn_DB),
                      HargaM = (p.StockProductLD_Stone1B.HargaTotalM.Value),
                      HargaR = 0m,
                      ProductCategory = 998,
                      ProductSegmen = 0,
                      IDItem = 0,
                      KadarTukaranBeli = 0m,
					  Keterangan = p.SalesOrder.Keterangan,
					  TglVoid = p.SalesOrder.TglVoid,
					  KeteranganVoid = p.SalesOrder.KeteranganVoid,
					  ExtraRingSize = (Db.SingleDecimal("SELECT TOP 1 ISNULL(ExtraRingSize,0) FROM SalesOrderLD WHERE ID = " + p.ID, Db.Cnn_DB)),
					  FeeExtraRingSizePerSize = (Db.SingleDecimal("SELECT TOP 1 ISNULL(FeeExtraRingSizePerSize,0) FROM SalesOrderLD WHERE ID = " + p.ID, Db.Cnn_DB)),
					  TotalFeeExtraRingSize = (Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalFeeExtraRingSize,0) FROM SalesOrderLD WHERE ID = " + p.ID, Db.Cnn_DB))
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
        
        //int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        //int IDLokasi = Convert.ToInt32(lokasi.SelectedValue);
        
        //if (Tipe >= 0)
        //    rs = rs.Where(p => p.TipeLokasi == Tipe);
        //if (IDLokasi >= 0)
        //    rs = rs.Where(p => p.IDLokasi == IDLokasi);

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
            rs = rs.Where(p => p.TipeLokasi == TipeLokasi);

            if (IDLokasi > 0)
            {
                rs = rs.Where(p => p.IDLokasi == IDLokasi);
            }
        }

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.NoSO == "" + keyword + "" || p.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text) || p.Nomor.Contains(kw.Text));

        if (status.SelectedValue == "1")
            rs = rs.Where(p => p.StatusVoid == true);
        else if (status.SelectedValue == "2")
            rs = rs.Where(p => p.StatusVoid == false);
        // if (dari.Text != "" && sampai.Text != "")
        // {
        //     rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        // }

        rs = rs.OrderByDescending(p => p.Tgl).ThenBy(p => p.NoSO);

        if (rs.Count() > 0)
        {
            decimal TotalHargaNet = 0m;
            decimal TotalHargaRupiah = 0m;
            decimal TotalHargaM = 0m;
            decimal TotalHargaR = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
			decimal TotalDiscountNominal = 0m;
            decimal TotalDiscountProgramNominal = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalDiscountPromo = 0m;
			decimal TotalDiscountBank = 0m;
			decimal TotalDiscountOther = 0m;
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
			decimal TotalBayar10 = 0m;
			decimal TotalBayar13 = 0m;
            decimal TotalMDR = 0m;
            decimal TTerima = 0m;
            decimal TotalPrice_ex_Vat = 0m;
            decimal TotalPPN = 0m;
            decimal TerimaSetelahMDR = 0m;
			int TotalPointReceived = 0;
			decimal TotalExtraRingSize = 0m;
			decimal TotalFeeExtraRingSizePerSize = 0m;
			decimal TotalFeeExtraRingSize = 0m;

            int prev = 0;
            int index = 1;
            string SO = "";

            /// Master Brand
            var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocWarehouse
            var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocOutlet
            var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

            /// Master Lokasi LocExhibition
            var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

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
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NoSO;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.Invoice;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                string Lokasi = "";
				string KodeLokasi = "";
                //if (r.TipeLokasi == 1)
                //    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
                //else if (r.TipeLokasi == 2)
                //    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + r.IDLokasi, Db.Cnn_DB);
			
				bool IsPPN = false;

                try
                {
                    if (r.TipeLokasi == 0)
                        Lokasi = rsWarehouse[r.IDLokasi.Value];
                    else if (r.TipeLokasi == 1)
					{
						Lokasi = rsOutlet[r.IDLokasi.Value];
						KodeLokasi = Db.SingleString("SELECT TOP 1 ISNULL(Kode,'-') [Kode] FROM LocOutlet WHERE ID = " + r.IDLokasi.Value ,Db.Cnn_DB);
						IsPPN = Db.SingleBool("SELECT TOP 1 ISNULL(IsUsePPN,0) [IsUsePPN] FROM LocOutlet WHERE ID = " + r.IDLokasi.Value ,Db.Cnn_DB);
					}
                    else if (r.TipeLokasi == 2)
					{
						Lokasi = rsExhibition[r.IDLokasi.Value];
						KodeLokasi = Db.SingleString("SELECT TOP 1 ISNULL(Kode,'-') [Kode] FROM LocExhibition WHERE ID = " + r.IDLokasi.Value ,Db.Cnn_DB);
						IsPPN = Db.SingleBool("SELECT TOP 1 ISNULL(IsUsePPN,0) [IsUsePPN] FROM LocExhibition WHERE ID = " + r.IDLokasi.Value ,Db.Cnn_DB);
					}
                }
                catch (Exception ex) { throw ex; }

                td = new TableCell();
                td.Text = Lokasi;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Tipe;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProductItem;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.ProductLevel;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProtoNomor;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Category;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Segmen;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
				if(KodeLokasi == "CRL" || KodeLokasi == "CRP" || KodeLokasi == "CRS")
					if(r.CustomerNama == "CMK CLUB")
						td.Text = r.CustomerNama + "(" + r.CustomerReferenceNama + ")";
					else
						td.Text = r.CustomerNama;
				else
					td.Text = r.CustomerNama;
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CustomerReferenceNama;
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Membership;
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesNama;
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.Keterangan;
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = Str.FullDate(r.TglVoid);
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.KeteranganVoid;
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HargaM.ToString("N2");
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
				td.Text = r.DiscountNominal.ToString("N0");
				tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.DiscountPromo.ToString("N0");
                tr.Cells.Add(td);
				
				td = new TableCell();
				td.Text = r.DiscountProgramNominal.ToString("N0");
				tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountGift.ToString("N0");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.DiscountBank.ToString("N0");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.DiscountOther.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountRound.ToString("N0");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.ExtraRingSize.ToString("N0");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.FeeExtraRingSizePerSize.ToString("N0");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.TotalFeeExtraRingSize.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.TotalBayar.ToString("N0");
                tr.Cells.Add(td);
				
				td = new TableCell();
                td.Text = r.PointReceived.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.KadarTukaranBeli.ToString("N2");
                tr.Cells.Add(td);

                decimal Price_ex_Vat = 0m;
                decimal PPN = 0m;
                decimal PPNPembagi = 0m;
                decimal PPNLD = 0m;
                string getKeterangan = "";

                if (r.Tipe == "Diamond Jewellery")
                {
                    getKeterangan =@"select isnull(ax.PPNPercentage,1) AS [PPNPercentage],isnull(ax.PPNPembagi,1) AS [PPNPembagi]
										from (
											SELECT top 1 a.*
											FROM SalesOrderDJ A 
                                            JOIN SalesOrder AA on AA.ID=A.IDForm
											JOIN StockProductDJ B ON A.IDProduct = B.ID                                             
											LEFT JOIN DataSupplier C ON B.Supplier = C.ID 
											LEFT JOIN DataSupplierGroup D ON C.IDGroup = D.ID
											WHERE A.ID = @ID and  AA.Draft=0
											)ax";
        		        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
            	        {
            	            using (SqlCommand cmd = new SqlCommand(getKeterangan, conn))
            	            {                                                                
            	                cmd.Parameters.AddWithValue("@ID", r.IDSOitem);
            	                conn.Open();                                
            	                using (SqlDataReader reader = cmd.ExecuteReader())
            	                {
            	                    // Loop through the results
            	                    while (reader.Read())
            	                    {             								                         
            	                       	PPNPembagi = reader.GetDecimal(reader.GetOrdinal("PPNPembagi"));            	                       	                                        
            	                    }
            	                }
            	                conn.Close(); 
            	            }
            	        }                    
                }else if (r.Tipe == "Plain Gold"){
                    getKeterangan =@"select isnull(ax.PPNPercentage,1) AS [PPNPercentage],isnull(ax.PPNPembagi,1) AS [PPNPembagi]
										from (
											SELECT top 1 a.*
											FROM SalesOrderPG A 
                                            JOIN SalesOrder AA on AA.ID=A.IDForm
											JOIN StockProductPG B ON A.IDProduct = B.ID                                             
											LEFT JOIN DataSupplier C ON B.IDSupplier = C.ID 
											LEFT JOIN DataSupplierGroup D ON C.IDGroup = D.ID
											WHERE A.ID = @ID and  AA.Draft=0
											)ax";
        		        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
            	        {
            	            using (SqlCommand cmd = new SqlCommand(getKeterangan, conn))
            	            {                                                                
            	                cmd.Parameters.AddWithValue("@ID", r.IDSOitem);
            	                conn.Open();                                
            	                using (SqlDataReader reader = cmd.ExecuteReader())
            	                {
            	                    // Loop through the results
            	                    while (reader.Read())
            	                    {             								                         
            	                       	PPNPembagi = reader.GetDecimal(reader.GetOrdinal("PPNPembagi"));            	                       	                                        
            	                    }
            	                }
            	                conn.Close(); 
            	            }
            	        }    

                }else if (r.Tipe == "Gold Jewellery"){
                    getKeterangan =@"select isnull(ax.PPNPercentage,1) AS [PPNPercentage],isnull(ax.PPNPembagi,1) AS [PPNPembagi]
										from (
											SELECT top 1 a.*
											FROM SalesOrderGJ A 
                                            JOIN SalesOrder AA on AA.ID=A.IDForm
											JOIN StockProductGJ B ON A.IDProduct = B.ID                                             
											LEFT JOIN DataSupplier C ON B.Supplier = C.ID 
											LEFT JOIN DataSupplierGroup D ON C.IDGroup = D.ID
											WHERE A.ID = @ID and  AA.Draft=0
											)ax";
        		        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
            	        {
            	            using (SqlCommand cmd = new SqlCommand(getKeterangan, conn))
            	            {                                                                
            	                cmd.Parameters.AddWithValue("@ID", r.IDSOitem);
            	                conn.Open();                                
            	                using (SqlDataReader reader = cmd.ExecuteReader())
            	                {
            	                    // Loop through the results
            	                    while (reader.Read())
            	                    {             								                         
            	                       	PPNPembagi = reader.GetDecimal(reader.GetOrdinal("PPNPembagi"));            	                       	                                        
            	                    }
            	                }
            	                conn.Close(); 
            	            }
            	        }    
                }else{
                        getKeterangan =@"select isnull(ax.PPNPercentage,1) AS [PPNPercentage],isnull(ax.PPNPembagi,1) AS [PPNPembagi]
						from (
						SELECT top 1 a.*
						FROM SalesOrderLD A 
                        JOIN SalesOrder AA on AA.ID=A.IDForm
						JOIN StockProductLD_Stone1b B ON A.IDProduct = B.ID         
						WHERE A.ID = @ID and  AA.Draft=0
						)ax";
                    
        		        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
            	        {
            	            using (SqlCommand cmd = new SqlCommand(getKeterangan, conn))
            	            {                                                                
            	                cmd.Parameters.AddWithValue("@ID", r.IDSOitem);
            	                conn.Open();                                
            	                using (SqlDataReader reader = cmd.ExecuteReader())
            	                {
            	                    // Loop through the results
            	                    while (reader.Read())
            	                    {             								                         
            	                       	PPNPembagi = reader.GetDecimal(reader.GetOrdinal("PPNPembagi"));            	                       	
            	                    }
            	                }
            	                conn.Close(); 
            	            }
            	        } 
                }   
                Response.Write(PPNPembagi);

				if(IsPPN)
				{
					if(r.Tgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
					{
						Price_ex_Vat = r.TotalBayar / 1.02m;
						PPN = r.TotalBayar - Price_ex_Vat;	
					}
                    else if(r.Tgl < Convert.ToDateTime("2023-05-01"))
					{
						Price_ex_Vat = r.TotalBayar / 1.022m;
						PPN = r.TotalBayar - Price_ex_Vat;	
					}
                    else if(r.Tgl <= Convert.ToDateTime("2025-01-02 23:59:00.000"))
					{
                        
						if(r.Tipe == "Loose Diamond")
                        {
                            PPNPembagi = 1.011m;
                            Price_ex_Vat = r.TotalBayar / PPNPembagi;
						    PPN = r.TotalBayar - Price_ex_Vat;
                        }
                        else if(r.Supplier == null || r.Supplier == 0)
                        {
                            PPNPembagi = 1.0165m;
                            Price_ex_Vat = r.TotalBayar / PPNPembagi;
						    PPN = r.TotalBayar - Price_ex_Vat;
                        }
						else if(Db.SingleString("SELECT a.Keterangan FROM DataSupplierGroup a join DataSupplier b on a.ID = b.IDGroup WHERE b.ID = " + r.Supplier, Db.Cnn_DB).Contains("In"))
                        {
                            PPNPembagi = 1.0165m;
                            Price_ex_Vat = r.TotalBayar / PPNPembagi;
						    PPN = r.TotalBayar - Price_ex_Vat;
                        }
                        else if(Db.SingleString("SELECT a.Keterangan FROM DataSupplierGroup a join DataSupplier b on a.ID = b.IDGroup WHERE b.ID = " + r.Supplier, Db.Cnn_DB).Contains("Out"))
                        {
                            PPNPembagi = 1.011m;
                            Price_ex_Vat = r.TotalBayar / PPNPembagi;
						    PPN = r.TotalBayar - Price_ex_Vat;
                        }
                        else 
                        {
                            PPNPembagi = 1.011m;
                            Price_ex_Vat = r.TotalBayar / PPNPembagi;
						    PPN = r.TotalBayar - Price_ex_Vat;
                        }
					}
					else
					{
                        if(r.Tipe == "Loose Diamond")
                        {
                            Price_ex_Vat = r.TotalBayar / PPNPembagi;
						    PPN = r.TotalBayar - Price_ex_Vat;
                        }
                        else if(r.Supplier == null || r.Supplier == 0)
                        {
                            Price_ex_Vat = r.TotalBayar / PPNPembagi;
						    PPN = r.TotalBayar - Price_ex_Vat;
                        }
						else if(Db.SingleString("SELECT a.Keterangan FROM DataSupplierGroup a join DataSupplier b on a.ID = b.IDGroup WHERE b.ID = " + r.Supplier, Db.Cnn_DB).Contains("In"))
                        {
                            Price_ex_Vat = r.TotalBayar / PPNPembagi;
						    PPN = r.TotalBayar - Price_ex_Vat;
                        }
                        else if(Db.SingleString("SELECT a.Keterangan FROM DataSupplierGroup a join DataSupplier b on a.ID = b.IDGroup WHERE b.ID = " + r.Supplier, Db.Cnn_DB).Contains("Out"))
                        {
                            Price_ex_Vat = r.TotalBayar / PPNPembagi;
						    PPN = r.TotalBayar - Price_ex_Vat;
                        }
                        else 
                        {
                            Price_ex_Vat = r.TotalBayar / PPNPembagi;
						    PPN = r.TotalBayar - Price_ex_Vat;
                        }
					}
				}
				else
				{
					Price_ex_Vat = r.TotalBayar;
				}

                td = new TableCell();
                td.Text = Price_ex_Vat.ToString("N0");
                tr.Cells.Add(td);
                Response.Write(r.TotalBayar);

                td = new TableCell();
                td.Text = PPN.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = PPNPembagi.ToString("N4");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Butir.ToString("N0");
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Carat.ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.NetWeight.ToString("N2");
                tr.Cells.Add(td);

                decimal MDR = 0m;
                decimal Terima = 0m;
                decimal dTerimaSetelahMDR = 0m;

                var so = db.SalesOrder.SingleOrDefault(p => p.ID.Equals(r.IDSO));
                if (SO != r.NoSO)
                {
                    int sod = 1;
                    //sod = so.SalesOrderDJ.Count() + so.SalesOrderPG.Count() + so.SalesOrderGJ.Count() + so.SalesOrderLD.Count();

                    decimal Bayar1 = (Payment == 1 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar2 = (Payment == 2 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar3 = (Payment == 3 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar4 = (Payment == 4 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar5 = (Payment == 5 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 5).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar6 = (Payment == 6 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 6).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar7 = (Payment == 7 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 7).Sum(o => o.Nominal))) : 0m;
                    decimal Bayar8 = (Payment == 8 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 8).Sum(o => o.Nominal))) : 0m;
					decimal Bayar10 = (Payment == 10 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 10).Sum(o => o.Nominal))) : 0m;
					decimal Bayar13 = (Payment == 13 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 13).Sum(o => o.Nominal))) : 0m;
                    MDR = Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Sum(o => o.MDR.Value)));

                    TotalBayar1 += (Payment == 1 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 1).Sum(o => o.Nominal))) : 0m;
                    TotalBayar2 += (Payment == 2 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 2).Sum(o => o.Nominal))) : 0m;
                    TotalBayar3 += (Payment == 3 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 3).Sum(o => o.Nominal))) : 0m;
                    TotalBayar4 += (Payment == 4 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 4).Sum(o => o.Nominal))) : 0m;
                    TotalBayar5 += (Payment == 5 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 5).Sum(o => o.Nominal))) : 0m;
                    TotalBayar6 += (Payment == 6 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 6).Sum(o => o.Nominal))) : 0m;
                    TotalBayar7 += (Payment == 7 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 7).Sum(o => o.Nominal))) : 0m;
                    TotalBayar8 += (Payment == 8 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 8).Sum(o => o.Nominal))) : 0m;
					TotalBayar10 += (Payment == 10 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 10).Sum(o => o.Nominal))) : 0m;
					TotalBayar13 += (Payment == 13 || Payment == -1) ? Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Where(o => o.IDPaymentType == 13).Sum(o => o.Nominal))) : 0m;
                    //TotalMDR += Convert.ToDecimal(so.SalesReceipt.Sum(z => z.SalesReceiptDetail.Sum(o => o.MDR.Value)));

                    Terima = (Bayar1 + Bayar2 + Bayar3 + Bayar4 + Bayar5 + Bayar6 + Bayar7 + Bayar8 + Bayar10 + Bayar13);
                    dTerimaSetelahMDR = Terima - MDR;

                    td = new TableCell();
                    td.Text = Bayar1.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar2.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar3.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar4.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar5.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar6.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar7.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bayar8.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);
					
					td = new TableCell();
                    td.Text = Bayar10.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);
					
					td = new TableCell();
                    td.Text = Bayar13.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Terima.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = MDR.ToString("N0");
                    td.RowSpan = sod;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = dTerimaSetelahMDR.ToString("N0");
                    td.RowSpan = sod;
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
                    td.Style.Add("text-align", "center");
                    td.RowSpan = sod;
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
                    td.ColumnSpan = 13;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Bank;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);
                }
                SO = r.NoSO;

                tb.Rows.Add(tr);

                TotalHargaNet += r.TotalBayar;
                TotalPrice_ex_Vat += Price_ex_Vat;
                TotalPPN += PPN;
                TotalHargaRupiah += r.Harga;
                TotalHargaM += r.HargaM;
                TotalHargaR += r.HargaR;
                TotalDiscount += r.Discount;
                TotalDiscountProgram += r.DiscountProgram;
				TotalDiscountNominal += r.DiscountNominal;
                TotalDiscountProgramNominal += r.DiscountProgramNominal;
                TotalDiscountGift += r.DiscountGift;
                TotalDiscountRound += r.DiscountRound;
				TotalDiscountBank += r.DiscountBank;
				TotalDiscountOther += r.DiscountOther;
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalNetWeight += r.NetWeight;
                TTerima += Terima;
                TotalMDR += MDR;
                TerimaSetelahMDR += dTerimaSetelahMDR;
				TotalPointReceived += r.PointReceived;
				TotalExtraRingSize += r.ExtraRingSize;
				TotalFeeExtraRingSizePerSize += r.FeeExtraRingSizePerSize;
				TotalFeeExtraRingSize += r.TotalFeeExtraRingSize;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 19;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
            td2.Style.Add("text-align", "center");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHargaM).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHargaRupiah).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount / rs.Count()).ToString("N0") + " %</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0") + " %</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountNominal).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountPromo).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgramNominal).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountBank).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountOther).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalExtraRingSize).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalFeeExtraRingSizePerSize).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalFeeExtraRingSize).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHargaNet).ToString("N0") + "</b>";
            td2.Style.Add("color","blue");
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToInt64(TotalPointReceived).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalPrice_ex_Vat).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalPPN).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);
            td2 = new TableCell();
            td2.Text = "";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0") + "</b>";
            td2.Style.Add("text-align", "center");
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
			
			td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar10).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);
			
			td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar13).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TTerima).ToString("N0") + "</b>";
            td2.Style.Add("color", "blue");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalMDR).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            //TerimaSetelahMDR
            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TerimaSetelahMDR).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
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
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            lokasi.SelectedValue = Request.QueryString["lokasi"];
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
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
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
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
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
            DLC.LocationList(lokasi, Tipe);
        }
        else
        {
            lokasi.Items.Clear();
        }
        lokasi.Focus();
    }

}