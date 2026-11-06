using System;
using System.Linq;

public partial class Page_Register_Sales_ItemAddSave : System.Web.UI.Page {
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
    App gp = new App();
	protected int myID { get { return App.QueryInt(this); } }
	protected int myItemIndex { get { return App.QueryInt(this, "item"); } }
	protected int myIDItem { get { return App.QueryInt(this, "iditem"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			
		}

        Save();
	}

	//Exception
    protected void DataError(string msg = "") { Response.Redirect("Sales.aspx" + (msg != "" ? ("?msg=" + msg) : "")); }

	
	protected void Save() {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            if (myItemIndex == 1)
            {
                var s = db.StockActualDJ.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == myIDItem);
                if (s == null) DataError();
                else
                {
                    var c = db.SalesOrderDJ.SingleOrDefault(p => p.IDProduct == myIDItem && p.SalesOrder.StatusVoid == false);
                    if (c == null)
                    {
                        
                        decimal Discount = 0m;
                        decimal DiscountPersen = 0m;
                        decimal DiscountHarga = 0m;
                        decimal TotalBayar = s.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value;

                        var rs = from p in db.PromoDJ_Detail
                                 where p.PromoDJ.ApprovalNama != null && p.PromoDJ.TglStart.Value <= DateTime.Today 
                                 && p.PromoDJ.TglExpire.Value >= DateTime.Today && p.IDProduct.Value == s.IDProduct
                                 && (p.PromoDJ.StatusActive != false || p.PromoDJ.StatusActive == null)
                                 select p;
                        if (rs.Count() > 0)
                        {
                            var disc = rs.First();
                            if (disc.PromoDJ.Tipe == 1)
                            {
                                DiscountHarga = disc.PromoDJ.DiscountNominal.Value;
                                DiscountPersen = (TotalBayar * (disc.PromoDJ.DiscountPersen.Value/100));
                                Discount = (DiscountHarga + DiscountPersen);
                                TotalBayar = TotalBayar - Discount;
                            }
                            if (disc.PromoDJ.Tipe == 3)
                            {
                                TotalBayar = disc.PromoDJ.PromoHarga.Value;
                                Discount = s.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value - TotalBayar;
                                
                            }
                        }
						
                        var rstone = from p in db.StockProductDJ_Stone1B
                                     where p.IDForm == myIDItem
                                     select p;
                        foreach (var r in rstone)
                        {
                            //r.HargaMJual = r.Stone1B.StoneCosting1B.InputH.Value;
                            //r.DiscMJual = r.Stone1B.StoneCosting1B.InputP.Value;
                            //r.HargaRJual = r.Stone1B.StonePricing1B.InputH.Value;
                            //r.DiscRJual = r.Stone1B.StonePricing1B.InputP.Value;

                            r.HargaMJual = r.HargaTotalM;
                            r.DiscMJual = 0m;
                            r.HargaRJual = r.HargaTotal;
                            r.DiscRJual = 0m;
                            r.HargaMJual = r.HargaTotalM;
                            r.HargaRJual = r.HargaTotal;
                            db.SubmitChanges();
                        }

                        SalesOrderDJ n = new SalesOrderDJ();

                        n.IDForm = myID;
                        n.IDProduct = myIDItem;
                        
                        n.ModalRupiah = s.StockProductDJ.StockProductDJ_PricingMU.ModalRupiah;
                        n.HargaRupiah = s.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound;
                        n.Discount = 0m;
                        n.DiscountProgram = 0m;
                        n.DiscountGift = 0m;
                        n.DiscountRound = 0m;
                        n.DiscountNominal = 0m;
                        n.DiscountProgramNominal = 0m;
                        n.DiscountPromo = Discount;
                        n.TotalBayar = TotalBayar;
                        n.TotalRupiah = TotalBayar;
                        n.StatusResell = 0;

                        var x = db.StockProductDJ.SingleOrDefault(p => p.ID == myIDItem);
                        if (x != null)
                        {
                            x.StatusPenjualan = 1;

                            db.SubmitChanges();
                        }

                        db.SalesOrderDJ.InsertOnSubmit(n);
                        db.SubmitChanges();

                        var u = db.SalesOrderDJ.SingleOrDefault(p => p.ID == n.ID);
                        u.Invoice = Document.Generate_Nomor_SalesOrder_DJ(u);
                        db.SubmitChanges();
                    }
                }
            }
            else if (myItemIndex == 2)
            {
                var s = db.StockActualPG.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == myIDItem);
                if (s == null) DataError();
                else
                {
                    decimal GoldPrice = gp.GetPGPricing(s.StockProductPG, false);
                    decimal ModalTGP = gp.GetPGPricing(s.StockProductPG, true);

                    if (s.StockProductPG.FixRate == true)
                    {
                        GoldPrice = s.StockProductPG.TotalHargaJual.Value;
                        ModalTGP = s.StockProductPG.Harga.Value;

                        GoldPrice = Math.Ceiling(GoldPrice / 10000) * 10000;
                    }



                    var c = db.SalesOrderPG.SingleOrDefault(p => p.IDProduct == myIDItem && p.SalesOrder.StatusVoid == false);
                    if (c == null)
                    {
                        decimal Discount = 0m;
                        decimal DiscountPersen = 0m;
                        decimal DiscountHarga = 0m;
                        decimal TotalBayar = GoldPrice;

                        var rs = from p in db.PromoPG_Detail
                                 where p.PromoPG.ApprovalNama != null && p.PromoPG.TglStart <= DateTime.Today
                                 && p.PromoPG.TglExpire >= DateTime.Today && p.IDProduct == s.IDProduct
                                 && (p.PromoPG.StatusActive != false || p.PromoPG.StatusActive == null)
                                 select p;
                        if (rs.Count() > 0)
                        {
                            var disc = rs.First();
                            if (disc.PromoPG.Tipe == 1)
                            {
                                DiscountHarga = disc.PromoPG.DiscountNominal.Value;
                                DiscountPersen = (TotalBayar * (disc.PromoPG.DiscountPersen.Value / 100));
                                Discount = (DiscountHarga + DiscountPersen);
                                TotalBayar = TotalBayar - Discount;
                            }
                            if (disc.PromoPG.Tipe == 3)
                            {
                                TotalBayar = disc.PromoPG.PromoHarga.Value;
                                Discount = GoldPrice - TotalBayar;

                            }
                        }

                        SalesOrderPG n = new SalesOrderPG();

                        n.IDForm = myID;
                        n.IDProduct = myIDItem;
                        
                        n.ModalRupiah = s.StockProductPG.TotalHarga24;
                        n.ModalTGP = ModalTGP;
                        n.HargaRupiah = GoldPrice;
                        n.Discount = 0m;
                        n.DiscountProgram = 0m;
                        n.DiscountGift = 0m;
                        n.DiscountRound = 0m;
                        n.DiscountNominal = 0m;
                        n.DiscountProgramNominal = 0m;
                        n.DiscountPromo = Discount;
                        n.TotalBayar = TotalBayar;
                        n.TotalRupiah = GoldPrice;
                        n.StatusResell = 0;

                        var x = db.StockProductPG.SingleOrDefault(p => p.ID == myIDItem);
                        if (x != null)
                        {
                            x.StatusPenjualan = 1;

                            db.SubmitChanges();
                        }

                        db.SalesOrderPG.InsertOnSubmit(n);
                        db.SubmitChanges();

                        var u = db.SalesOrderPG.SingleOrDefault(p => p.ID == n.ID);
                        u.Invoice = Document.Generate_Nomor_SalesOrder_PGVoucherLEI(u);
                        db.SubmitChanges();
                    }
                }
            }
            else if (myItemIndex == 3)
            {
                var s = db.StockActualGJ.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == myIDItem);
                if (s == null) DataError();
                else
                {
                    var c = db.SalesOrderGJ.SingleOrDefault(p => p.IDProduct == myIDItem && p.SalesOrder.StatusVoid == false);
                    if (c == null)
                    {
                        decimal Discount = 0m;
                        decimal DiscountPersen = 0m;
                        decimal DiscountHarga = 0m;
                        decimal TotalBayar = s.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value;

                        var rs = from p in db.PromoGJ_Detail
                                 where p.PromoGJ.ApprovalNama != null && p.PromoGJ.TglStart <= DateTime.Today 
                                 && p.PromoGJ.TglExpire >= DateTime.Today && p.IDProduct == s.IDProduct
                                 && (p.PromoGJ.StatusActive != false || p.PromoGJ.StatusActive == null)
                                 select p;
                        if (rs.Count() > 0)
                        {
                            var disc = rs.First();
                            if (disc.PromoGJ.Tipe == 1)
                            {
                                DiscountHarga = disc.PromoGJ.DiscountNominal.Value;
                                DiscountPersen = (TotalBayar * (disc.PromoGJ.DiscountPersen.Value / 100));
                                Discount = (DiscountHarga + DiscountPersen);
                                TotalBayar = TotalBayar - Discount;
                            }
                            if (disc.PromoGJ.Tipe == 3)
                            {
                                TotalBayar = disc.PromoGJ.PromoHarga.Value;
                                Discount = s.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound.Value - TotalBayar;

                            }
                        }

                        SalesOrderGJ n = new SalesOrderGJ();

                        n.IDForm = myID;
                        n.IDProduct = myIDItem;
                        
                        n.ModalRupiah = s.StockProductGJ.StockProductGJ_PricingMU.ModalRupiah;
                        n.HargaRupiah = s.StockProductGJ.StockProductGJ_PricingMU.HargaRupiahRound;
                        n.Discount = 0m;
                        n.DiscountProgram = 0m;
                        n.DiscountGift = 0m;
                        n.DiscountRound = 0m;
                        n.DiscountNominal = 0m;
                        n.DiscountProgramNominal = 0m;
                        n.DiscountPromo = Discount;
                        n.TotalBayar = TotalBayar;
                        n.TotalRupiah = TotalBayar;
                        n.StatusResell = 0;

                        var x = db.StockProductGJ.SingleOrDefault(p => p.ID == myIDItem);
                        if (x != null)
                        {
                            x.StatusPenjualan = 1;

                            db.SubmitChanges();
                        }

                        db.SalesOrderGJ.InsertOnSubmit(n);
                        db.SubmitChanges();

                        var u = db.SalesOrderGJ.SingleOrDefault(p => p.ID == n.ID);
                        u.Invoice = Document.Generate_Nomor_SalesOrder_GJ(u);
                        db.SubmitChanges();
                    }
                }
            }
            else if (myItemIndex == 4)
            {
                var s = db.StockActualLD_Stone1B.SingleOrDefault(p => p.IDLokasi == d.IDLokasi && p.TipeLokasi == d.TipeLokasi && p.IDProduct == myIDItem);
                if (s == null) DataError();
                else
                {
                    var c = db.SalesOrderLD.SingleOrDefault(p => p.IDProduct == myIDItem && p.SalesOrder.StatusVoid == false);
                    if (c == null)
                    {
                        //var us = from p in db.PricingTableUS
                        //         where p.ID == CV.US_General_Pricing
                        //         select p;

                        var us = from p in db.PricingTableUS
                                 where p.ID == CV.US_LD_Rate
                                 select p;

                        decimal Harga = Math.Floor((s.StockProductLD_Stone1B.Stone1B.StonePricing1B.Harga.Value * s.StockProductLD_Stone1B.TotalCarat) * us.First().Harga.Value / 1000) * 1000;
                        StoneCosting1B cost = s.StockProductLD_Stone1B.Stone1B.StoneCosting1B;

                        var prod = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == myIDItem);

                        prod.HargaMJual = prod.Stone1B.StoneCosting1B.InputH.Value;
                        prod.DiscMJual = prod.Stone1B.StoneCosting1B.InputP.Value;
                        prod.HargaRJual = prod.Stone1B.StonePricing1B.InputH.Value;
                        prod.DiscRJual = prod.Stone1B.StonePricing1B.InputP.Value;

                        SalesOrderLD n = new SalesOrderLD();

                        n.IDForm = myID;
                        n.IDProduct = myIDItem;
                        
                        //n.ModalRupiah = s.StockProductLD_Stone1B.HargaTotalM.Value;
                        n.ModalRupiah = cost.Harga.Value * s.StockProductLD_Stone1B.TotalCarat;
                        n.HargaRupiah = Harga;
                        n.Discount = 0m;
                        n.DiscountProgram = 0m;
                        n.DiscountGift = 0m;
                        n.DiscountRound = 0m;
                        n.DiscountNominal = 0m;
                        n.DiscountProgramNominal = 0m;
                        n.TotalBayar = Harga;
                        n.TotalRupiah = Harga;
                        n.StatusResell = 0;

                        var x = db.StockProductLD.SingleOrDefault(p => p.ID == myIDItem);
                        if (x != null)
                        {
                            x.StatusPenjualan = 1;

                            db.SubmitChanges();
                        }

                        db.SalesOrderLD.InsertOnSubmit(n);
                        db.SubmitChanges();

                        var u = db.SalesOrderLD.SingleOrDefault(p => p.ID == n.ID);
                        u.Invoice = Document.Generate_Nomor_SalesOrder_LD(u);
                        db.SubmitChanges();
                    }
                }
            }
			saveSouvenirAnnivPalace(d);
			
            Response.Redirect("Sales_Data.aspx?id=" + myID);
        }
	}
	
	protected void saveSouvenirAnnivPalace(SalesOrder so) 
    {
        DateTime start = new DateTime(2022, 7, 1, 0, 0, 0);
        DateTime end = new DateTime(2022, 7, 31, 23, 59, 59);
        if (so.Tgl >= start && so.Tgl <= end) 
        {
			try
			{
				Db.Execute("EXEC sp_get_souvenir_anniversary " + so.ID , Db.Cnn_DB);
			}
			catch{}
        }
    }
}