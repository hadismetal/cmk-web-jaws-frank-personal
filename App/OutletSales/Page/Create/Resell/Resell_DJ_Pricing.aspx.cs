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
    protected int myItemID { get { return App.QueryInt(this,"itemid"); } }

	//Exception
	protected void PricingError(string sebab) {
		Response.Redirect("Resell.aspx?id=" + myID + "&prcerror=" + sebab);
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

		var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myItemID);
		if (d == null) App.DataNull(this);
		else {
			//Delete
			deleteProcess(d);

			//Process
			var product = d.StockProductDJ_CharProduct;
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
			Response.Redirect("Resell_DJ_Submit.aspx?id=" + myID + "&itemid=" + myItemID);
		}
	}

	//Delete
	protected void deleteProcess(StockProductDJ d) {
		if (d.StockProductDJ_PricingProduct != null)
			db.StockProductDJ_PricingProduct.DeleteOnSubmit(
				d.StockProductDJ_PricingProduct);

		if (d.StockProductDJ_PricingBiaya != null)
			db.StockProductDJ_PricingBiaya.DeleteOnSubmit(
				d.StockProductDJ_PricingBiaya);

		if (d.StockProductDJ_PricingMU != null)
			db.StockProductDJ_PricingMU.DeleteOnSubmit(
				d.StockProductDJ_PricingMU);

        if (d.StockProductDJ_CostingProduct != null)
            db.StockProductDJ_CostingProduct.DeleteOnSubmit(
                d.StockProductDJ_CostingProduct);

        if (d.StockProductDJ_Costing != null)
            db.StockProductDJ_Costing.DeleteOnSubmit(
                d.StockProductDJ_Costing);

		//Commit
		db.SubmitChanges();
	}

	//Stone Pricing
	protected void saveStone(StockProductDJ d) {
		saveStone1A(d);
		saveStone1B(d);
		saveStone2(d);
		saveStone3(d);
		saveStone4(d);
		saveStone5(d);
	}
	protected void saveStone1A(StockProductDJ d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(0);
        decimal OngkosPB = 0m;

		var rs = from p in d.StockProductDJ_Stone1A select p;
		foreach (var r in rs) {

			StonePricing1A prc = r.Stone1A.StonePricing1A;
            StoneCosting1A cost = r.Stone1A.StoneCosting1A;
			if (prc == null || cost == null) PricingError(sebab);
			else {

				if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
				else {

					//Harga
					r.HargaSatuan = prc.Harga.Value;
					r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

					//Tgl. Efektif
					r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone1A.Parcel101.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	protected void saveStone1B(StockProductDJ d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(1);

        decimal OngkosPB = 0m;

		var rs = from p in d.StockProductDJ_Stone1B select p;
		foreach (var r in rs) {

			StonePricing1B prc = r.Stone1B.StonePricing1B;
            StoneCosting1B cost = r.Stone1B.StoneCosting1B;
			if (prc == null || cost == null) PricingError(sebab);
			else {

				if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue || !prc.InputH.HasValue || !prc.InputP.HasValue) PricingError(sebab);
				else {
					//Harga
					r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

					//Rapaport
					r.HargaInputH = prc.InputH.Value;
					r.HargaInputP = prc.InputP.Value;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

                    r.DiscMJual = 0;
                    r.DiscRJual = 0;
                    r.HargaMJual = r.HargaTotalM;
                    r.HargaRJual = r.HargaTotal;

					//Tgl. Efektif
					r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone1B.Parcel101.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	protected void saveStone2(StockProductDJ d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(2);

        decimal OngkosPB = 0m;

		var rs = from p in d.StockProductDJ_Stone2 select p;
		foreach (var r in rs) {

			StonePricing2 prc = r.Stone2.StonePricing2;
            StoneCosting2 cost = r.Stone2.StoneCosting2;
			if (prc == null || cost == null) PricingError(sebab);
			else {

				if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
				else {
					//Harga
					r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

					//Tgl. Efektif
					r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone2.Parcel201.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	protected void saveStone3(StockProductDJ d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(3);

        decimal OngkosPB = 0m;

		var rs = from p in d.StockProductDJ_Stone3 select p;
		foreach (var r in rs) {

			StonePricing3 prc = r.Stone3.StonePricing3;
            StoneCosting3 cost = r.Stone3.StoneCosting3;
			if (prc == null || cost == null) PricingError(sebab);
			else {

				if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
				else {
					//Harga
					r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

					//Tgl. Efektif
					r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone3.Parcel301.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	protected void saveStone4(StockProductDJ d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(4);

        decimal OngkosPB = 0m;

		var rs = from p in d.StockProductDJ_Stone4 select p;
		foreach (var r in rs) {

			StonePricing4 prc = r.Stone4.StonePricing4;
            StoneCosting4 cost = r.Stone4.StoneCosting4;
			if (prc == null || cost == null) PricingError(sebab);
			else {

				if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
				else {
					//Harga
					r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

					//Tgl. Efektif
					r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone4.Parcel401.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	protected void saveStone5(StockProductDJ d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(5);

        decimal OngkosPB = 0m;

		var rs = from p in d.StockProductDJ_Stone5 select p;
		foreach (var r in rs) {

			StonePricing5 prc = r.Stone5.StonePricing5;
            StoneCosting5 cost = r.Stone5.StoneCosting5;
			if (prc == null || cost == null) PricingError(sebab);
			else {

				if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
				else {
					//Harga
					r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    r.HargaSatuanM = cost.Harga.Value;
                    r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;

					//Tgl. Efektif
					r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone5.Parcel501.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;


					//Commit
					db.SubmitChanges();
				}
			}
		}
	}

	//Finishing
	protected void saveFinishing(StockProductDJ d, StockProductDJ_CharProduct product) {
		var rs = from p in d.StockProductDJ_Finishing select p;
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
			if (!ok) PricingError(sebab);
			else {
				r.Biaya = prc_ff.Biaya.Value;
				r.Efektif = prc_ff.ApprovalTgl.Value;

				//Commit
				db.SubmitChanges();
			}
		}
	}

	//Product
	protected void saveProduct(StockProductDJ d, StockProductDJ_CharProduct product) {
		string sebab = "";
		bool ok = true;

		//Pricing
		var prc_tgM = db.PricingTableTG.SingleOrDefault(p =>
			p.ID == CV.TG_PG1_Costing &&
			p.Harga.HasValue &&
			p.ApprovalTgl.HasValue);

        var prc_tg = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == CV.TG_PG1_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

		//Pricing - Validasi
		if (prc_tg == null || prc_tgM == null) { ok = false; sebab = "Today Gold Price"; }

		//Process
		if (!ok) PricingError(sebab);
		else {
			StockProductDJ_PricingProduct n = new StockProductDJ_PricingProduct();

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
			db.StockProductDJ_PricingProduct.InsertOnSubmit(n);
			db.SubmitChanges();

            //Costing
            StockProductDJ_CostingProduct m = new StockProductDJ_CostingProduct();

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
            db.StockProductDJ_CostingProduct.InsertOnSubmit(m);
            db.SubmitChanges();

            
		}
	}

	//Biaya
	protected void saveBiaya(StockProductDJ d, StockProductDJ_CharProduct product) {
		string sebab = "";
		bool ok = true;

        var x = db.Resell.SingleOrDefault(p => p.ID == myID);
        int IDBrand = x.DataCustomer.IDBrand;

		//Pricing
		var prc_fm = db.PricingTableFM.SingleOrDefault(p =>
            p.IDBrand == IDBrand &&
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
			p.ID == CV.US_General_Costing &&
			p.Harga.HasValue &&
			p.ApprovalTgl.HasValue);

		//Pricing - Validasi
		if (prc_fm == null) { ok = false; sebab = "Mounting Fee"; }
		if (prc_fs == null) { ok = false; sebab = "Setting Fee"; }
		if (prc_us == null) { ok = false; sebab = "USD Rate"; }

		//Process
		if (!ok) PricingError(sebab);
		else {
			StockProductDJ_PricingBiaya n = new StockProductDJ_PricingBiaya();

			n.ID = d.ID;

			//Informasi
            n.NamaBrand = "Calculator";
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

			//Commit
			db.StockProductDJ_PricingBiaya.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected int FS_ID(StockProductDJ d, StockProductDJ_CharProduct product) {
		return 1; //Masih di-hardcoded. BELUM SELESAI !
	}

	//Mark-Up
	protected void saveMU(StockProductDJ d, StockProductDJ_CharProduct product) {
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
			p.ID == CV.US_General_Pricing &&
			p.Harga.HasValue &&
			p.ApprovalTgl.HasValue);

		//Pricing - Validasi
		if (prc_mu == null) { ok = false; sebab = "Diamond Jewelry MU"; }
		if (prc_us == null) { ok = false; sebab = "USD Rate"; }

		//Process
		if (!ok) PricingError(sebab);
		else {
			StockProductDJ_PricingMU n = new StockProductDJ_PricingMU();

			n.ID = d.ID;

			//Informasi
			n.NamaCategory = product.CharProductCategory.Nama;
			n.NamaLevel = product.CharProductLevel.Nama;
			n.NamaDist = product.CharStoneDist.Nama;

			//Modal
			n.ModalUSD = TotalModal(d.ID);
			n.ModalRate = prc_us.Harga.Value;
			n.ModalRupiah = n.ModalUSD * n.ModalRate;

			//Mark-Up
			n.MU = prc_mu.Persen.Value;

            decimal OngkosLain = 0m;

			//Harga Final
            n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID);
            n.HargaRupiah = (TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID)) * n.ModalRate  + OngkosLain;
            n.HargaRupiahRound = price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID)) * n.ModalRate + OngkosLain);
            


			//Tgl. Efektif
			n.EfektifRate = prc_us.ApprovalTgl.Value;

			//Commit
			db.StockProductDJ_PricingMU.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}

	//Segmen System
	protected void saveSegmen(StockProductDJ d, StockProductDJ_CharProduct product) {
		string sebab = "Segmen System";
		bool ok = true;

		//Pricing - Validasi
		ok = checkSegmen(product);

		//Process
        if (!ok) PricingError(sebab);
        else
        {
            //Product Value
            decimal harga = HargaUSD(d.ID);
            //decimal carat = product.StoneCarat;

            decimal carat = 0m;

            try
            {
                carat = (from p in db.StockProductDJ_Stone1A where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone1B where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone2 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone3 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone4 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone5 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Max(p => p.Carat);
            }
            catch (Exception ex) { }

            //Parameter
            int IDSegmen = 1;
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

            foreach (var r in rs)
            {
                if ((r.SegmenSistem == 0 && harga >= r.Minimum.Value) ||
                    (r.SegmenSistem == 1 && carat >= r.Minimum.Value))
                {

                    IDSegmen = r.IDSegmen;
                    SegmenSistem = r.SegmenSistem;
                    SegmenMinimum = r.Minimum.Value;
                    EfektifSegmen = r.ApprovalTgl.Value;
                }
            }

            StockProductDJ_PricingSegmen n = new StockProductDJ_PricingSegmen();

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
            db.StockProductDJ_PricingSegmen.InsertOnSubmit(n);
            db.SubmitChanges();

        }
	}
	protected bool checkSegmen(StockProductDJ_CharProduct product) {
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
	protected decimal TotalStone(StockProductDJ d) {
		decimal total = 0;

		var rs_1A = from p in d.StockProductDJ_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_1A) total += r;

		var rs_1B = from p in d.StockProductDJ_Stone1B where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_1B) total += r;

		var rs_2 = from p in d.StockProductDJ_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_2) total += r;

		var rs_3 = from p in d.StockProductDJ_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_3) total += r;

		var rs_4 = from p in d.StockProductDJ_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_4) total += r;

		var rs_5 = from p in d.StockProductDJ_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_5) total += r;

		return total;
	}
    //Sub-Total
    protected decimal TotalStone1A(StockProductDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_1A = from p in d.StockProductDJ_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_1A) total += r;
        }
        catch (Exception ex) { }
        return total;
    }
    protected decimal TotalStone1B(StockProductDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_1B = from p in d.StockProductDJ_Stone1B where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_1B) total += r;
        }
        catch(Exception ex){}
        return total;
    }
    protected decimal TotalStone2(StockProductDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_2 = from p in d.StockProductDJ_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_2) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalStone3(StockProductDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_3 = from p in d.StockProductDJ_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_3) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalStone4(StockProductDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_4 = from p in d.StockProductDJ_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_4) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalStone5(StockProductDJ d)
    {
        decimal total = 0;

        try
        {
            var rs_5 = from p in d.StockProductDJ_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_5) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalMStone1A(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in d.StockProductDJ_Stone1A where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_1A) total += r;

        return total;
    }
    protected decimal TotalMStone1B(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1B = from p in d.StockProductDJ_Stone1B where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_1B) total += r;

        return total;
    }
    protected decimal TotalMStone2(StockProductDJ d)
    {
        decimal total = 0;

        var rs_2 = from p in d.StockProductDJ_Stone2 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_2) total += r;

        return total;
    }
    protected decimal TotalMStone3(StockProductDJ d)
    {
        decimal total = 0;

        var rs_3 = from p in d.StockProductDJ_Stone3 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_3) total += r;

        return total;
    }
    protected decimal TotalMStone4(StockProductDJ d)
    {
        decimal total = 0;

        var rs_4 = from p in d.StockProductDJ_Stone4 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_4) total += r;

        return total;
    }
    protected decimal TotalMStone5(StockProductDJ d)
    {
        decimal total = 0;

        var rs_5 = from p in d.StockProductDJ_Stone5 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }
	protected decimal TotalFinishing(StockProductDJ d) {
		decimal total = 0;

		var rs = from p in d.StockProductDJ_Finishing where p.Biaya.HasValue select p.Biaya.Value;
		foreach (var r in rs) total += r;

		return total;
	}
	protected decimal TotalModal(int ID) {
		decimal total = 0;

		var product = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
		var biaya = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += product.Total;
		total += biaya.Total;

		return total;
	}
    protected decimal TotalToMarkUp(int ID)
    {
        decimal total = 0;

        var product = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(product.TotalStone3) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        total += biaya.Total;

        return total;
    }
    protected decimal TotalStone1B(int ID)
    {
        decimal total = 0;

        var product = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalStone1B);
        //total += biaya.Total;

        return total;
    }
	protected decimal HargaUSD(int ID) {
		var mu = db.StockProductDJ_PricingMU.SingleOrDefault(p => p.ID == ID);
		return mu.HargaUSD;
	}
    protected void saveTotalStone(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in d.StockProductDJ_Stone1A select p.TotalButir;
		foreach (var r in rs_1A) total += r;

        var rs_1B = from p in d.StockProductDJ_Stone1B select p.TotalButir;
		foreach (var r in rs_1B) total += r;

        var rs_2 = from p in d.StockProductDJ_Stone2 select p.TotalButir;
		foreach (var r in rs_2) total += r;

        var rs_3 = from p in d.StockProductDJ_Stone3 select p.TotalButir;
		foreach (var r in rs_3) total += r;

        var rs_4 = from p in d.StockProductDJ_Stone4 select p.TotalButir;
		foreach (var r in rs_4) total += r;

        var rs_5 = from p in d.StockProductDJ_Stone5 select p.TotalButir;
		foreach (var r in rs_5) total += r;

        d.StockProductDJ_CharProduct.StoneQty = total;
        db.SubmitChanges();
                
    }
}