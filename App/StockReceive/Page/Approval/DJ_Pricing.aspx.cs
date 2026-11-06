using System;
using System.Linq;

public partial class Page_Approval_DJ_Pricing : System.Web.UI.Page {
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
    App price = new App();

    decimal Setting = 0m;

	protected int myID { get { return App.QueryInt(this); } }

	//Exception
	protected void PricingError(string sebab) {
		Response.Redirect("DJ_Form.aspx?id=" + myID + "&prcerror=" + sebab);
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

		var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Delete
			deleteProcess(d);

			//Process
			var product = d.StockReceiveDJ_CharProduct;
			if (product == null) PricingError("Product Char.");
			else {
				saveStone(d);
				saveFinishing(d, product);

				saveProduct(d, product);
				saveBiaya(d, product);

				saveMU(d, product);
				saveSegmen(d, product);
                saveTotalStone(d);
			}

			//Redirect
			Response.Redirect("DJ_PricingSubmit.aspx?id=" + myID);
		}
	}

	//Delete
	protected void deleteProcess(StockReceiveDJ d) {
		if (d.StockReceiveDJ_PricingProduct != null)
			db.StockReceiveDJ_PricingProduct.DeleteOnSubmit(
				d.StockReceiveDJ_PricingProduct);

        if (d.StockReceiveDJ_CostingProduct != null)
            db.StockReceiveDJ_CostingProduct.DeleteOnSubmit(
                d.StockReceiveDJ_CostingProduct);

		if (d.StockReceiveDJ_PricingBiaya != null)
			db.StockReceiveDJ_PricingBiaya.DeleteOnSubmit(
				d.StockReceiveDJ_PricingBiaya);

		if (d.StockReceiveDJ_PricingMU != null)
			db.StockReceiveDJ_PricingMU.DeleteOnSubmit(
				d.StockReceiveDJ_PricingMU);

		if (d.StockReceiveDJ_PricingSegmen != null)
			db.StockReceiveDJ_PricingSegmen.DeleteOnSubmit(
				d.StockReceiveDJ_PricingSegmen);

		//Commit
		db.SubmitChanges();
	}

	//Stone Pricing
	protected void saveStone(StockReceiveDJ d) {
		saveStone1A(d);
		saveStone1B(d);
		saveStone2(d);
		saveStone3(d);
		saveStone4(d);
		saveStone5(d);
	}
	protected void saveStone1A(StockReceiveDJ d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(0);
        decimal OngkosPB = 0m;

		var rs = from p in d.StockReceiveDJ_Stone1A select p;
		foreach (var r in rs) {

			StonePricing1A prc = r.Stone1A.StonePricing1A;
            StoneCosting1A cost = r.Stone1A.StoneCosting1A;
            if (prc == null || cost == null)
            {
                PricingError(sebab + " (" + r.Stone1A.Nama + ")");
            }
			else 
            {
                if (!prc.Harga.HasValue
                    || !prc.ApprovalTgl.HasValue
                    || !cost.Harga.HasValue
                    )
                {
                    PricingError(sebab + " (" + r.Stone1A.Nama + ")");
                }
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockReceiveDJ_CharProduct.CharProductItem.NamaKode, r.Stone1A.Parcel101.NamaKode, d.StockReceiveDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                    {
                        var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                        if (s != null)
                        {
                            var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Stone1A.SingleOrDefault(p => p.IDStone == r.IDStone);
                            if(t!=null)
                            {
                            //Harga
                            r.HargaSatuan = t.HargaSatuan;
                            r.HargaTotal = t.HargaTotal;

                            r.HargaSatuanM = cost.Harga.Value;
                            r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                            //Tgl. Efektif
                            r.Efektif = r.Efektif;

                            //Setting
                            r.Setting = r.Setting;
                                }
                        }
                    }*/

                    //Commit
                    db.SubmitChanges();
                }
			}
		}
	}
	protected void saveStone1B(StockReceiveDJ d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(1);

        decimal OngkosPB = 0m;

		var rs = from p in d.StockReceiveDJ_Stone1B where p.StatusTerpasang == 1 select p;
		foreach (var r in rs) {

			StonePricing1B prc = r.Stone1B.StonePricing1B;
            StoneCosting1B cost = r.Stone1B.StoneCosting1B;
            if (prc == null || cost == null)
            {
                PricingError(sebab + " (" + r.Stone1B.Nama + ")");
            }
			else {

                if (!prc.Harga.HasValue
                    || !prc.ApprovalTgl.HasValue
                    || !prc.InputH.HasValue
                    || !prc.InputP.HasValue
                    || !cost.Harga.HasValue
                    || !cost.InputH.HasValue
                    || !cost.InputP.HasValue
                    )
                {
                    PricingError(sebab + " (" + r.Stone1B.Nama + ")");
                }
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    //Rapaport
                    r.HargaInputH = prc.InputH.Value;
                    r.HargaInputP = prc.InputP.Value;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    r.HargaMInputH = cost.InputH.Value;
                    r.HargaMInputP = cost.InputP.Value;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockReceiveDJ_CharProduct.CharProductItem.NamaKode, r.Stone1B.Parcel101.NamaKode, d.StockReceiveDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                    {
                        var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                        if (s != null)
                        {
                            var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Stone1B.SingleOrDefault(p => p.IDStone == r.IDStone);
                            if (t != null)
                            {
                                //Harga
                                r.HargaSatuan = t.HargaSatuan;
                                r.HargaTotal = t.HargaTotal;

                                //Rapaport
                                r.HargaInputH = t.HargaInputH;
                                r.HargaInputP = t.HargaInputP;

                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                                //Tgl. Efektif
                                r.Efektif = r.Efektif;

                                //Setting
                                r.Setting = r.Setting;
                            }
                        }
                    }*/

                    //Commit
                    db.SubmitChanges();
                }
			}
		}
	}
	protected void saveStone2(StockReceiveDJ d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(2);

        decimal OngkosPB = 0m;

		var rs = from p in d.StockReceiveDJ_Stone2 select p;
		foreach (var r in rs) {

			StonePricing2 prc = r.Stone2.StonePricing2;
            StoneCosting2 cost = r.Stone2.StoneCosting2;
            if (prc == null || cost == null)
            {
                PricingError(sebab + " (" + r.Stone2.Nama + ")");
            }
			else {

                if (!prc.Harga.HasValue
                    || !prc.ApprovalTgl.HasValue
                    || !cost.Harga.HasValue
                    )
                {
                    PricingError(sebab + " (" + r.Stone2.Nama + ")");
                }
				else 
                {
					//Harga
					r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

					//Tgl. Efektif
					r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockReceiveDJ_CharProduct.CharProductItem.NamaKode, r.Stone2.Parcel201.NamaKode, d.StockReceiveDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                    {
                        var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                        if (s != null)
                        {
                            var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Stone2.SingleOrDefault(p => p.IDStone == r.IDStone);
                            if (t != null)
                            {
                                //Harga
                                r.HargaSatuan = t.HargaSatuan;
                                r.HargaTotal = t.HargaTotal;

                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                                //Tgl. Efektif
                                r.Efektif = r.Efektif;

                                //Setting
                                r.Setting = r.Setting;
                            }
                        }
                    }*/

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	protected void saveStone3(StockReceiveDJ d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(3);

        decimal OngkosPB = 0m;

		var rs = from p in d.StockReceiveDJ_Stone3 select p;
		foreach (var r in rs) {

			StonePricing3 prc = r.Stone3.StonePricing3;
            StoneCosting3 cost = r.Stone3.StoneCosting3;
            if (prc == null || cost == null)
            {
                PricingError(sebab + " (" + r.Stone3.Nama + ")");
            }
			else {

                if (!prc.Harga.HasValue
                    || !prc.ApprovalTgl.HasValue
                    || !cost.Harga.HasValue
                    )
                {
                    PricingError(sebab + " (" + r.Stone3.Nama + ")");
                }
				else {
					//Harga
					r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

					//Tgl. Efektif
					r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockReceiveDJ_CharProduct.CharProductItem.NamaKode, r.Stone3.Parcel301.NamaKode, d.StockReceiveDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                    {
                        var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                        if (s != null)
                        {
                            var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Stone3.SingleOrDefault(p => p.IDStone == r.IDStone);
                            if (t != null)
                            {
                                //Harga
                                r.HargaSatuan = t.HargaSatuan;
                                r.HargaTotal = t.HargaTotal;

                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                                //Tgl. Efektif
                                r.Efektif = r.Efektif;

                                //Setting
                                r.Setting = r.Setting;
                            }
                        }
                    }*/

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	protected void saveStone4(StockReceiveDJ d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(4);

        decimal OngkosPB = 0m;

		var rs = from p in d.StockReceiveDJ_Stone4 select p;
		foreach (var r in rs) {

			StonePricing4 prc = r.Stone4.StonePricing4;
            StoneCosting4 cost = r.Stone4.StoneCosting4;
            if (prc == null || cost == null)
            {
                PricingError(sebab + " (" + r.Stone4.Nama + ")");
            }
			else {

                if (!prc.Harga.HasValue
                    || !prc.ApprovalTgl.HasValue
                    || !cost.Harga.HasValue
                    )
                {
                    PricingError(sebab + " (" + r.Stone4.Nama + ")");
                }
				else {
					//Harga
					r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

					//Tgl. Efektif
					r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockReceiveDJ_CharProduct.CharProductItem.NamaKode, r.Stone4.Parcel401.NamaKode, d.StockReceiveDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                    {
                        var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                        if (s != null)
                        {
                            var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Stone4.SingleOrDefault(p => p.IDStone == r.IDStone);
                            if (t != null)
                            {
                                //Harga
                                r.HargaSatuan = t.HargaSatuan;
                                r.HargaTotal = t.HargaTotal;

                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                                //Tgl. Efektif
                                r.Efektif = r.Efektif;

                                //Setting
                                r.Setting = r.Setting;
                            }
                        }
                    }*/

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	protected void saveStone5(StockReceiveDJ d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(5);

        decimal OngkosPB = 0m;

		var rs = from p in d.StockReceiveDJ_Stone5 select p;
		foreach (var r in rs) {

			StonePricing5 prc = r.Stone5.StonePricing5;
            StoneCosting5 cost = r.Stone5.StoneCosting5;
            if (prc == null || cost == null)
            {
                PricingError(sebab + " (" + r.Stone5.Nama + ")");
            }
			else {

                if (!prc.Harga.HasValue
                    || !prc.ApprovalTgl.HasValue
                    || !cost.Harga.HasValue
                    )
                {
                    PricingError(sebab + " (" + r.Stone5.Nama + ")");
                }
				else {
					//Harga
					r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

					//Tgl. Efektif
					r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockReceiveDJ_CharProduct.CharProductItem.NamaKode, r.Stone5.Parcel501.NamaKode, d.StockReceiveDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                    {
                        var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                        if (s != null)
                        {
                            var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Stone5.SingleOrDefault(p => p.IDStone == r.IDStone);
                            if (t != null)
                            {
                                //Harga
                                r.HargaSatuan = t.HargaSatuan;
                                r.HargaTotal = t.HargaTotal;

                                r.HargaSatuanM = cost.Harga.Value;
                                r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                                //Tgl. Efektif
                                r.Efektif = r.Efektif;

                                //Setting
                                r.Setting = r.Setting;
                            }
                        }
                    }*/

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}

	//Finishing
	protected void saveFinishing(StockReceiveDJ d, StockReceiveDJ_CharProduct product) {
		var rs = from p in d.StockReceiveDJ_Finishing select p;
		foreach (var r in rs) {
			string sebab = "";
			bool ok = true;

			//Pricing
			var prc_ff = db.PricingTableFF.SingleOrDefault(p =>
				p.IDItem == product.ProductItem &&
				p.IDFinishing == r.IDFinishing &&
				p.Biaya.HasValue &&
				p.ApprovalTgl.HasValue
				);

			//Pricing - Validasi
			if (prc_ff == null) { ok = false; sebab = "Finishing Fee"; }

			//Process
            if (!ok)
            {
                PricingError(sebab + " (" + r.CharProcessFinishing.Nama + ")");
            }
			else {
				r.Biaya = prc_ff.Biaya.Value;
				r.Efektif = prc_ff.ApprovalTgl.Value;

                /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
                {
                    var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                    if (s != null)
                    {
                        var t = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_Finishing.SingleOrDefault(p => p.IDFinishing == r.IDFinishing);
                        if (t != null)
                        {
                            r.Biaya = t.Biaya;
                            r.Efektif = t.Efektif;
                        }
                    }
                }*/

				//Commit
				db.SubmitChanges();
			}
		}
	}

	//Product
	protected void saveProduct(StockReceiveDJ d, StockReceiveDJ_CharProduct product) {
		string sebab = "";
		bool ok = true;

		//Pricing

        int IDTG_Costing = CVV.TG_PG1_Costing;
        int IDTG_Pricing = CVV.TG_PG1_Pricing;
        if (d.StockReceiveDJ_CharProduct.CharFrameMaterial.NamaKode == "AU")
        {
            IDTG_Costing = CVV.TG_PG1_Costing;
            IDTG_Pricing = CVV.TG_PG1_Pricing;
        }
        else if (d.StockReceiveDJ_CharProduct.CharFrameMaterial.NamaKode == "PL")
        {
            IDTG_Costing = CVV.TG_PG_Paladium;
            IDTG_Pricing = CVV.TG_PG_Paladium2;
        }
        else if (d.StockReceiveDJ_CharProduct.CharFrameMaterial.NamaKode == "PT")
        {
            IDTG_Costing = CVV.TG_PG_Platina;
            IDTG_Pricing = CVV.TG_PG_Platina2;
        }
        else if (d.StockReceiveDJ_CharProduct.CharFrameMaterial.NamaKode == "SIL")
        {
            IDTG_Costing = CVV.TG_PG_Silver;
            IDTG_Pricing = CVV.TG_PG_Silver2;
        }

		var prc_tgM = db.PricingTableTG.SingleOrDefault(p =>
			p.ID == IDTG_Costing &&
			p.Harga.HasValue &&
			p.ApprovalTgl.HasValue);

        var prc_tg = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == IDTG_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

		//Pricing - Validasi
		if (prc_tg == null || prc_tgM == null) { ok = false; sebab = "Today Gold Price"; }

		//Process
        if (!ok)
        {
            PricingError(sebab);
        }
		else {
			StockReceiveDJ_PricingProduct n = new StockReceiveDJ_PricingProduct();

			n.ID = d.ID;

			//Gold Price
			n.GoldRate = prc_tg.Harga.Value;
			n.EfektifGold = prc_tg.ApprovalTgl.Value;

			//Informasi Berat
			n.Netto = product.NettoWeight;
			n.Kadar = product.KadarTukaran;

			//Total
			n.TotalProduct = n.GoldRate * n.Netto * (n.Kadar / (decimal) 100);
            n.TotalStone1A = Convert.ToDecimal(TotalStone1A(d));
            n.TotalStone1B = Convert.ToDecimal(TotalStone1B(d));
            n.TotalStone2 = Convert.ToDecimal(TotalStone2(d));
            n.TotalStone3 = Convert.ToDecimal(TotalStone3(d));
            n.TotalStone4 = Convert.ToDecimal(TotalStone4(d));
            n.TotalStone5 = Convert.ToDecimal(TotalStone5(d));
            n.Total = n.TotalProduct + Convert.ToDecimal(n.TotalStone1A) + Convert.ToDecimal(n.TotalStone1B) + Convert.ToDecimal(n.TotalStone2) + Convert.ToDecimal(n.TotalStone3) + Convert.ToDecimal(n.TotalStone4) + Convert.ToDecimal(n.TotalStone5);

			//Commit
			db.StockReceiveDJ_PricingProduct.InsertOnSubmit(n);
			db.SubmitChanges();

            /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
            {
                var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                if (s != null)
                {
                    var u = db.StockReceiveDJ_PricingProduct.SingleOrDefault(p => p.ID == n.ID);
                    //Gold Price
                    n.GoldRate = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingProduct.GoldRate;
                    n.EfektifGold = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingProduct.EfektifGold;
                }
            }*/

            //Costing
            StockReceiveDJ_CostingProduct m = new StockReceiveDJ_CostingProduct();

            m.ID = d.ID;

            //Gold Price
            m.GoldRate = prc_tgM.Harga.Value;
            m.EfektifGold = prc_tgM.ApprovalTgl.Value;

            //Informasi Berat
            m.Netto = product.NettoWeight;
            m.Kadar = product.KadarTukaran;

            //Total
            m.TotalProduct = m.GoldRate * n.Netto * (n.Kadar / (decimal)100);
            m.TotalStone1A = Convert.ToDecimal(TotalMStone1A(d));
            m.TotalStone1B = Convert.ToDecimal(TotalMStone1B(d));
            m.TotalStone2 = Convert.ToDecimal(TotalMStone2(d));
            m.TotalStone3 = Convert.ToDecimal(TotalMStone3(d));
            m.TotalStone4 = Convert.ToDecimal(TotalMStone4(d));
            m.TotalStone5 = Convert.ToDecimal(TotalMStone5(d));
            m.Total = m.TotalProduct + Convert.ToDecimal(m.TotalStone1A) + Convert.ToDecimal(m.TotalStone1B) + Convert.ToDecimal(m.TotalStone2) + Convert.ToDecimal(m.TotalStone3) + Convert.ToDecimal(m.TotalStone4) + Convert.ToDecimal(m.TotalStone5);

            //Commit
            db.StockReceiveDJ_CostingProduct.InsertOnSubmit(m);
            db.SubmitChanges();
		}
	}

	//Biaya
	protected void saveBiaya(StockReceiveDJ d, StockReceiveDJ_CharProduct product) {
		string sebab = "";
		bool ok = true;

		//Pricing
		var prc_fm = db.PricingTableFM.SingleOrDefault(p =>
			p.IDBrand == d.LocWarehouse.IDBrand &&
			p.IDItem == product.ProductItem &&
			p.IDCons == product.ProcessCons &&
			p.Biaya.HasValue &&
            p.Pembagi.HasValue &&
			p.ApprovalTgl.HasValue);

		var prc_fs = db.PricingTableFS.SingleOrDefault(p =>
			p.ID == FS_ID(d, product) &&
			p.Biaya.HasValue &&
			p.ApprovalTgl.HasValue);

		var prc_us = db.PricingTableUS.SingleOrDefault(p =>
			p.ID == CVV.US_General_Costing &&
			p.Harga.HasValue &&
			p.ApprovalTgl.HasValue);

		//Pricing - Validasi
		if (prc_fm == null) { ok = false; sebab = "Mounting Fee"; }
		if (prc_fs == null) { ok = false; sebab = "Setting Fee"; }
		if (prc_us == null) { ok = false; sebab = "USD Rate"; }

		//Process
		if (!ok) PricingError(sebab);
		else {
			StockReceiveDJ_PricingBiaya n = new StockReceiveDJ_PricingBiaya();

			n.ID = d.ID;

			//Informasi
			n.NamaBrand = d.LocWarehouse.CompanyBrand.Nama;
			n.NamaItem = product.CharProductItem.Nama;
			n.NamaCons = product.CharProcessCons.Nama;

			//Mounting
			n.Mounting = prc_fm.Biaya.Value;
			n.MountingNO = prc_fm.InputNO.Value;
			n.MountingRO = prc_fm.InputRO.Value;

			//Setting
			//n.Setting = prc_fs.Biaya.Value;
            /*Sementara di Hardcode*/
            n.Setting = Setting;

            /*end hardcode*/



			//Finishing
			n.Finishing = TotalFinishing(d);

			//Total
			n.TotalRupiah = n.Mounting + n.Setting + n.Finishing;
			n.TotalRate = prc_us.Harga.Value;
			n.Total = n.TotalRupiah / n.TotalRate;

			//Tgl. Efektif
			n.EfektifMounting = prc_fm.ApprovalTgl.Value;
			n.EfektifSetting = prc_fs.ApprovalTgl.Value;
			n.EfektifRate = prc_us.ApprovalTgl.Value;

            /*if (d.Proto != null && d.Proto != 0 && d.RequestCustomerNama != null)
            {
                var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                if (s != null)
                {
                    n.Mounting = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.Mounting;
                    n.MountingNO = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.MountingNO;
                    n.MountingRO = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.MountingRO;

                    //Setting
                    //n.Setting = prc_fs.Biaya.Value;
                    n.Setting = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.Setting;



                    //Finishing
                    n.Finishing = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.Finishing;

                    //Total
                    n.TotalRupiah = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.TotalRupiah;
                    n.TotalRate = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.TotalRate;
                    n.Total = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.Total;

                    //Tgl. Efektif
                    n.EfektifMounting = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.EfektifMounting;
                    n.EfektifSetting = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.EfektifSetting;
                    n.EfektifRate = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingBiaya.EfektifRate;
                }
            }*/


			//Commit
			db.StockReceiveDJ_PricingBiaya.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected int FS_ID(StockReceiveDJ d, StockReceiveDJ_CharProduct product) {
		return 1; //Masih di-hardcoded. BELUM SELESAI !
	}

	//Mark-Up
	protected void saveMU(StockReceiveDJ d, StockReceiveDJ_CharProduct product) {
		string sebab = "";
		bool ok = true;

		//Pricing
		var prc_mu = db.PricingTableMU.SingleOrDefault(p =>
			p.IDCategory == product.ProductCategory &&
			p.IDLevel == product.ProductLevel &&
			p.IDDist == product.StoneDist &&
			p.Persen.HasValue &&
			p.ApprovalTgl.HasValue);

		var prc_us = db.PricingTableUS.SingleOrDefault(p =>
			p.ID == CVV.US_General_Pricing &&
			p.Harga.HasValue &&
			p.ApprovalTgl.HasValue);

		//Pricing - Validasi
		if (prc_mu == null) { ok = false; sebab = "Diamond Jewelry MU"; }
		if (prc_us == null) { ok = false; sebab = "USD Rate"; }

		//Process
		if (!ok) PricingError(sebab);
		else {
			StockReceiveDJ_PricingMU n = new StockReceiveDJ_PricingMU();

			n.ID = d.ID;

			//Informasi
			n.NamaCategory = product.CharProductCategory.Nama;
			n.NamaLevel = product.CharProductLevel.Nama;
			n.NamaDist = product.CharStoneDist.Nama;

			//Modal
			n.ModalUSD = TotalModal(d.ID); 
            
            decimal rate = prc_us.Harga.Value;
            DateTime efektif_rate = prc_us.ApprovalTgl.Value;
            if(Db.SingleBool("SELECT dbo.fx_isFashionRate(" + product.ID + ", 0) ", Db.Cnn_DB))
            {
                rate = Db.SingleDecimal("SELECT Harga From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
                efektif_rate = Db.SingleTime("SELECT ApprovalTgl From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
            }
            n.ModalRate = rate;
			n.ModalRupiah = n.ModalUSD * n.ModalRate;

			//Mark-Up
			n.MU = prc_mu.Persen.Value;

            decimal OngkosLain = 0m;

            try
            {
                OngkosLain = d.StockReceiveDJ_Costing.OngkosLain.Value;
            }
            catch (Exception)
            {
                OngkosLain = 0m;
            }

			//Harga Final
            //n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + (OngkosLain / d.StockReceiveDJ_Costing.TotalRate.Value);
            n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d) + (OngkosLain / d.StockReceiveDJ_Costing.TotalRate.Value);
            n.HargaRupiah = (TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d)) * n.ModalRate + OngkosLain;
            n.HargaRupiahRound = price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d)) * n.ModalRate + OngkosLain);
			
			//Tgl. Efektif
			n.EfektifRate = efektif_rate;
            /*if (d.Proto != null && d.Proto!=0 && d.RequestCustomerNama != null)
            {
                var s = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                if (s != null)
                {
                    n.HargaUSD = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingMU.HargaUSD;
                    n.HargaRupiah = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingMU.HargaUSD;
                    n.HargaRupiahRound = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingMU.HargaRupiahRound;
					//Tgl. Efektif
					n.EfektifRate = s.KodeKaretDJ.ProductOrder.First().ProductDesign.ProductDesign_PricingMU.EfektifRate;
                }
            }*/
            


			

			//Commit
			db.StockReceiveDJ_PricingMU.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}

	//Segmen System
	protected void saveSegmen(StockReceiveDJ d, StockReceiveDJ_CharProduct product) {
		string sebab = "Segmen System";
		bool ok = true;

		//Pricing - Validasi
		ok = checkSegmen(product);

		//Process
		if (!ok) PricingError(sebab);
		else {
			//Product Value
			decimal harga = HargaUSD(d.ID);
			//decimal carat = product.StoneCarat;

            decimal carat = 0m;

            try
            {
                carat = (from p in db.StockReceiveDJ_Stone1A where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockReceiveDJ_Stone1B where p.IDForm == d.ID && p.StatusTerpasang ==1 select new { Carat = p.CaratButir }).Union
                            (from p in db.StockReceiveDJ_Stone2 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockReceiveDJ_Stone3 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockReceiveDJ_Stone4 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockReceiveDJ_Stone5 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Max(p => p.Carat);
            }
            catch (Exception) { }

			//Parameter
			int IDSegmen = 0;
			int SegmenSistem = 0;
			decimal SegmenMinimum = 0;
			DateTime EfektifSegmen = DateTime.Now;

			//SELECT
			var rs = from p in db.PricingTableSS
					 where
						 p.IDCategory == product.ProductCategory &&
						 p.IDLevel == product.ProductLevel &&
						 p.IDDist == product.StoneDist && p.CharProductSegmen.NamaKode != "NN"
					 orderby p.Minimum
					 select p;

			foreach (var r in rs) {
				if ((r.SegmenSistem == 0 && harga >= r.Minimum.Value) ||
					(r.SegmenSistem == 1 && carat >= r.Minimum.Value)) {

					IDSegmen = r.IDSegmen;
					SegmenSistem = r.SegmenSistem;
					SegmenMinimum = r.Minimum.Value;
					EfektifSegmen = r.ApprovalTgl.Value;
				}
			}

			StockReceiveDJ_PricingSegmen n = new StockReceiveDJ_PricingSegmen();

			n.ID = d.ID;
			n.ProductSegmen = IDSegmen;

			//Informasi
			n.NamaCategory = product.CharProductCategory.Nama;
			n.NamaLevel = product.CharProductLevel.Nama;
			n.NamaDist = product.CharStoneDist.Nama;

			//Segmen
			n.SegmenSistem = SegmenSistem;
			n.SegmenMinimum = SegmenMinimum;

			//Harga dan Carat
			n.HargaUSD = harga;
			n.TotalCarat = carat;

			//Tgl. Efektif
			n.EfektifSegmen = EfektifSegmen;

			///Commit
			db.StockReceiveDJ_PricingSegmen.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected bool checkSegmen(StockReceiveDJ_CharProduct product) {
		bool ok = true;

		var rs = from p in db.PricingTableSS
				 where
					 p.IDCategory == product.ProductCategory &&
					 p.IDLevel == product.ProductLevel &&
					 p.IDDist == product.StoneDist && p.CharProductSegmen.NamaKode != "NN"
				 select p;

		foreach (var r in rs) {
			if (!r.Minimum.HasValue || !r.ApprovalTgl.HasValue) {
				ok = false;
				break;
			}
		}

		return ok;
	}

	//Sub-Total
	protected decimal TotalStone(StockReceiveDJ d) {
		decimal total = 0;

		var rs_1A = from p in d.StockReceiveDJ_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_1A) total += r;

		var rs_1B = from p in d.StockReceiveDJ_Stone1B where p.HargaTotal.HasValue && p.StatusTerpasang == 1 select p.HargaTotal.Value;
		foreach (var r in rs_1B) total += r;

		var rs_2 = from p in d.StockReceiveDJ_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_2) total += r;

		var rs_3 = from p in d.StockReceiveDJ_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_3) total += r;

		var rs_4 = from p in d.StockReceiveDJ_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_4) total += r;

		var rs_5 = from p in d.StockReceiveDJ_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_5) total += r;

		return total;
	}
    //Sub-Total
    protected decimal TotalStone1A(StockReceiveDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_1A = from p in d.StockReceiveDJ_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_1A) total += r;
        }
        catch (Exception) { }
        return total;
    }
    protected decimal TotalStone1B(StockReceiveDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_1B = from p in d.StockReceiveDJ_Stone1B where p.HargaTotal.HasValue && p.StatusTerpasang == 1 select p.HargaTotal.Value;
            foreach (var r in rs_1B) total += r;
        }
        catch (Exception) { }
        return total;
    }
    protected decimal TotalStone2(StockReceiveDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_2 = from p in d.StockReceiveDJ_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_2) total += r;
        }
        catch (Exception) { }

        return total;
    }
    protected decimal TotalStone3(StockReceiveDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_3 = from p in d.StockReceiveDJ_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_3) total += r;
        }
        catch (Exception) { }

        return total;
    }
    protected decimal TotalStone4(StockReceiveDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_4 = from p in d.StockReceiveDJ_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_4) total += r;
        }
        catch (Exception) { }

        return total;
    }
    protected decimal TotalStone5(StockReceiveDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_5 = from p in d.StockReceiveDJ_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_5) total += r;
        }
        catch (Exception) { }

        return total;
    }
    protected decimal TotalMStone1A(StockReceiveDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in d.StockReceiveDJ_Stone1A where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_1A) total += r;

        return total;
    }
    protected decimal TotalMStone1B(StockReceiveDJ d)
    {
        decimal total = 0;

        var rs_1B = from p in d.StockReceiveDJ_Stone1B where p.HargaTotalM.HasValue && p.StatusTerpasang == 1 select p.HargaTotalM.Value;
        foreach (var r in rs_1B) total += r;

        return total;
    }
    protected decimal TotalMStone2(StockReceiveDJ d)
    {
        decimal total = 0;

        var rs_2 = from p in d.StockReceiveDJ_Stone2 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_2) total += r;

        return total;
    }
    protected decimal TotalMStone3(StockReceiveDJ d)
    {
        decimal total = 0;

        var rs_3 = from p in d.StockReceiveDJ_Stone3 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_3) total += r;

        return total;
    }
    protected decimal TotalMStone4(StockReceiveDJ d)
    {
        decimal total = 0;

        var rs_4 = from p in d.StockReceiveDJ_Stone4 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_4) total += r;

        return total;
    }
    protected decimal TotalMStone5(StockReceiveDJ d)
    {
        decimal total = 0;

        var rs_5 = from p in d.StockReceiveDJ_Stone5 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }
	protected decimal TotalFinishing(StockReceiveDJ d) {
		decimal total = 0;

		var rs = from p in d.StockReceiveDJ_Finishing where p.Biaya.HasValue select p.Biaya.Value;
		foreach (var r in rs) total += r;

		return total;
	}
	protected decimal TotalModal(int ID) {
		decimal total = 0;

		var product = db.StockReceiveDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
		var biaya = db.StockReceiveDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += product.Total;
		total += biaya.Total;

		return total;
	}
    protected decimal TotalToMarkUp(int ID)
    {
        decimal total = 0;

        var product = db.StockReceiveDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockReceiveDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        var stone3 = from p in db.StockReceiveDJ_Stone3
                     where p.IDForm == ID && (p.GIA == "" || p.GIA == null)
                     select p;
        decimal Stone3ToMarkUp = 0m;
        try
        {
            Stone3ToMarkUp = stone3.Sum(p => p.HargaTotal.Value);
        }
        catch (Exception) { }

        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(product.TotalStone3) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(Stone3ToMarkUp) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(Stone3ToMarkUp);
        total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(Stone3ToMarkUp);
        total += biaya.Total;

        return total;
    }
    protected decimal TotalStone1B(int ID)
    {
        decimal total = 0;

        var product = db.StockReceiveDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockReceiveDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalStone1B);
        //total += biaya.Total;

        return total;
    }
    protected decimal TotalStone3NMU(int ID)
    {
        decimal total = 0;

        var stone3 = from p in db.StockReceiveDJ_Stone3
                     where p.IDForm == ID && p.GIA != ""
                     select p;
        try
        {
            total = stone3.Sum(p => p.HargaTotal.Value);
        }
        catch (Exception) { }

        return total;
    }
	protected decimal HargaUSD(int ID) {
		var mu = db.StockReceiveDJ_PricingMU.SingleOrDefault(p => p.ID == ID);
		return mu.HargaUSD;
	}
    protected void saveTotalStone(StockReceiveDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in d.StockReceiveDJ_Stone1A select p.TotalButir;
		foreach (var r in rs_1A) total += r;

        var rs_1B = from p in d.StockReceiveDJ_Stone1B where p.StatusTerpasang == 1 select p.TotalButir;
		foreach (var r in rs_1B) total += r;

        var rs_2 = from p in d.StockReceiveDJ_Stone2 select p.TotalButir;
		foreach (var r in rs_2) total += r;

        var rs_3 = from p in d.StockReceiveDJ_Stone3 select p.TotalButir;
		foreach (var r in rs_3) total += r;

        var rs_4 = from p in d.StockReceiveDJ_Stone4 select p.TotalButir;
		foreach (var r in rs_4) total += r;

        var rs_5 = from p in d.StockReceiveDJ_Stone5 select p.TotalButir;
		foreach (var r in rs_5) total += r;

        d.StockReceiveDJ_CharProduct.StoneQty = total;
        db.SubmitChanges();
                
    }
}