using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

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
                    //decimal GoldPrice = Lib.GetPGPricing(s.StockProductPG, false);
                    //decimal ModalTGP = gp.GetPGPricing(s.StockProductPG, true);
					
					decimal GoldPrice = GetPGPricingMyAPPSNewBanget(s.StockProductPG.ID,Convert.ToDecimal(s.StockProductPG.NettoWeight));
					decimal ModalTGP = GetPGPricing(s.StockProductPG, true);

                    if (s.StockProductPG.FixRate == true)
                    {
                        GoldPrice = s.StockProductPG.TotalHargaJual.Value;
                        ModalTGP = s.StockProductPG.Harga.Value;

                        GoldPrice = Math.Ceiling(GoldPrice / 10000) * 10000;
                    }



                    var c = db.SalesOrderPG.SingleOrDefault(p => p.IDProduct == myIDItem && p.SalesOrder.StatusVoid == false && p.StatusResell == 0);
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
                        u.Invoice = Document.Generate_Nomor_SalesOrder_PG(u);
                        db.SubmitChanges();

                        //Db.Execute("UPDATE A SET A.BeratTimbang = B.GrossWeight FROM SalesOrderPG A JOIN StockProductPG B ON A.IDProduct = B.ID WHERE A.ID = " + n.ID, Db.Cnn_DB);
						
						string query = "sp_update_berattimbang_sales_order_pg";
						List<SqlParameter> parameters = new List<SqlParameter>
						{
							new SqlParameter("@ID", SqlDbType.Int) { Value = n.ID },
							new SqlParameter("@BERATTIMBANG", SqlDbType.Decimal)
							{
								Precision = 18,
								Scale = 2,
								Value = x.GrossWeight // pass decimal, NOT txt.Text
							},
						};
						Execute(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
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
            Response.Redirect("Sales_Data.aspx?id=" + myID);
        }
	}
    
    protected decimal GetPGPricingMyAPPSNewBanget(int IDProduct, decimal WeightPG)
    {
        decimal GoldPrice = 0m;
        decimal TotalRupiah = 0m;

        string query = "sp_get_tgp_jual_beli_plu";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID_PRODUCT", SqlDbType.Int){ Value = IDProduct },
            new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = "PG" }
        };

        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
        if (dt.Rows.Count == 1)
        {
            GoldPrice = Convert.ToDecimal(dt.Rows[0]["HARGA_JUAL"]);
        }

        TotalRupiah = GoldPrice * Convert.ToDecimal(WeightPG);
        return TotalRupiah;
    }

    protected decimal GetPGPricing(StockProductPG r, bool Costing)
    {
        int IDPricing = 0;
        decimal GoldPrice = 0m;
        decimal TotalRupiah = 0m;

        if (Costing)
        {
            if (r.KadarLogam >= 99m)
            {
                if (r.ProductItem == 59) // 59 = Chain
                    IDPricing = 9;
                else
                    IDPricing = 10;
            }
            else
            {
                if (r.ProductItem != 59) // 59 = Chain
                {
                    if (r.FrameColor == 1)
                        IDPricing = 5;
                    else
                        IDPricing = 6;
                }
                else
                {
                    if (r.FrameColor == 1)
                        IDPricing = 7;
                    else
                        IDPricing = 8;
                }
            }
        }
        else
        {
			if(r.CharGoldLevel.ID == 13){ IDPricing = 37; }
			//else if(r.CharGoldLevel.ID == 14){ IDPricing = 38; }
			else if(r.CharGoldLevel.ID == 14){ IDPricing = 37; } //Update tgl 19 Oktober 2024, mappingan sama dengan id 13
            else if (r.CharGoldLevel.ID == 8 && r.CharFrameColor.ID == 2 && r.CharProductItem.ID == 59) // chain white 37.5
            {
                IDPricing = 36; //PG / CHAIN KADAR 37,5 - Pricing
            }
            else if (r.CharGoldLevel.ID == 6 || r.CharGoldLevel.ID == 7) // 6 = zirconia 37,5 ; 7 = zirconia 75 
            {
                if (r.CharGoldLevel.ID == 6) { IDPricing = 34; }
                else if (r.CharGoldLevel.ID == 7) { IDPricing = 35; }
            }
			else if (r.CharGoldLevel.ID == 9) { IDPricing = 37; }
            //else if (r.CharGoldLevel.ID == 10) {IDPricing = 38; }
			else if (r.CharGoldLevel.ID == 10) {IDPricing = 37; } //Update tgl 19 Oktober 2024, mappingan sama dengan id 9
            else if (r.KadarLogam >= 99m)
            {
                if (r.ProductItem == 59) // 59 = Chain
                    IDPricing = 15;
                else
                    IDPricing = 16;
            }
            else
            {
                if (r.ProductItem != 59) // 59 = Chain
                {
                    if (r.FrameColor == 1)
                        IDPricing = 11;
                    else
                        IDPricing = 12;
                }
                else
                {
                    if (r.FrameColor == 1)
                        IDPricing = 13;
                    else
                        IDPricing = 14;
                }
            }
        }

        GoldPrice = PGPrice(IDPricing);

        TotalRupiah = CalcPrice(GoldPrice, Convert.ToDecimal(r.NettoWeight), Costing);

        return TotalRupiah;
    }

    protected decimal PGPrice(int IDPricing)
    {
        decimal Price = 0m;

        var d = db.PricingTableTG.SingleOrDefault(p => p.ID == IDPricing);
        if (d != null)
            Price = d.Harga.Value;

        return Price;
    }

    protected decimal CalcPrice(decimal GoldPrice, decimal Weight, bool Costing)
    {
        decimal Price = 1m;
        decimal Rate = 1m;
        int IDRate = 1;

        if (Costing)
            IDRate = 1;
        else
            IDRate = 2;

        var d = db.PricingTableUS.SingleOrDefault(p => p.ID == IDRate);
        Rate = d.Harga.Value;

        //Price = GoldPrice * Rate * Weight;
        Price = GoldPrice * Weight;
        //Price = (Price > 0m) ? (Math.Ceiling(Price / 1000) * 1000) : 1m;

        return Price;
    }

    protected DataTable Rs(string query, string connection, List<SqlParameter> parameters, CommandType cmdtype)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = cmdtype;
                if (parameters != null)
                {
                    foreach (SqlParameter p in parameters)
                    {
                        cmd.Parameters.Add(p);
                    }
                }
                cmd.CommandTimeout = 300;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    dt.Load(reader); // Not streaming, but async-safe here
                }
            }
        }
        return dt;
    }
	
	//Execute Command
    protected void Execute(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                cmd.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                cmd.ExecuteNonQuery();
            }
        }
    }
}