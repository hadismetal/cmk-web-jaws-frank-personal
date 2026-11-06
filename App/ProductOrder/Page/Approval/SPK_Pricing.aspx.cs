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

	protected int myID { get { return App.QueryInt(this); } }

	//Exception
	protected void PricingError(string sebab) {
		Response.Redirect("SPK_Form.aspx?id=" + myID + "&prcerror=" + sebab);
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Delete
			deleteProcess(d);

            string testDebug = "";

			//Process
			var product = d.ProductOrder_CharProduct;
			if (product == null) PricingError("Product Char.");
			else {
				saveStone(d);
				saveFinishing(d, product);

				saveProduct(d, product);
				saveBiaya(d, product);

                testDebug = saveMU(d, product);
				saveSegmen(d, product);
                saveTotalStone(d);
			}

			//Redirect
            //Response.Redirect("SPK_Form.aspx?id=" + myID + testDebug);
            Response.Redirect("SPK_Form.aspx?id=" + myID);
		}
	}

	//Delete
	protected void deleteProcess(ProductOrder d) {
		if (d.ProductOrder_PricingProduct != null)
			db.ProductOrder_PricingProduct.DeleteOnSubmit(
				d.ProductOrder_PricingProduct);

		if (d.ProductOrder_PricingBiaya != null)
			db.ProductOrder_PricingBiaya.DeleteOnSubmit(
				d.ProductOrder_PricingBiaya);

		if (d.ProductOrder_PricingMU != null)
			db.ProductOrder_PricingMU.DeleteOnSubmit(
				d.ProductOrder_PricingMU);

		//Commit
		db.SubmitChanges();
	}

	//Stone Pricing
	protected void saveStone(ProductOrder d) {
		saveStone1A(d);
		saveStone1B(d);
		saveStone2(d);
		saveStone3(d);
		saveStone4(d);
		saveStone5(d);
	}
	protected void saveStone1A(ProductOrder d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(0);
        decimal OngkosPB = 0m;

		var rs = from p in d.ProductOrder_Stone1A select p;
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
                    OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone1A.Parcel101.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	protected void saveStone1B(ProductOrder d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(1);

        decimal OngkosPB = 0m;

		var rs = from p in d.ProductOrder_Stone1B select p;
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

					//Tgl. Efektif
					r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone1B.Parcel101.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	protected void saveStone2(ProductOrder d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(2);

        decimal OngkosPB = 0m;

		var rs = from p in d.ProductOrder_Stone2 select p;
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
                    OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone2.Parcel201.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	protected void saveStone3(ProductOrder d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(3);

        decimal OngkosPB = 0m;

		var rs = from p in d.ProductOrder_Stone3 select p;
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
                    OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone3.Parcel301.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	protected void saveStone4(ProductOrder d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(4);

        decimal OngkosPB = 0m;

		var rs = from p in d.ProductOrder_Stone4 select p;
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
                    OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone4.Parcel401.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}
	protected void saveStone5(ProductOrder d) {
		//Exception-Cause
		string sebab = DL.StoneIndex(5);

        decimal OngkosPB = 0m;

		var rs = from p in d.ProductOrder_Stone5 select p;
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
                    OngkosPB = price.CalculateOngkosPB(d.ProductOrder_CharProduct.CharProductItem.NamaKode, r.Stone5.Parcel501.NamaKode, d.ProductOrder_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

					//Commit
					db.SubmitChanges();
				}
			}
		}
	}

	//Finishing
	protected void saveFinishing(ProductOrder d, ProductOrder_CharProduct product) {
		var rs = from p in d.ProductOrder_Finishing select p;
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
	protected void saveProduct(ProductOrder d, ProductOrder_CharProduct product) {
		string sebab = "";
		bool ok = true;

        //Pricing

        int IDTG_Costing = CV.TG_PG1_Costing;
        int IDTG_Pricing = CV.TG_PG1_Pricing;
        if (d.ProductOrder_CharProduct.CharFrameMaterial.NamaKode == "EMS")
        {
            IDTG_Costing = CV.TG_PG1_Costing;
            IDTG_Pricing = CV.TG_PG1_Pricing;
        }
        else if (d.ProductOrder_CharProduct.CharFrameMaterial.NamaKode == "PAL")
        {
            IDTG_Costing = CV.TG_PG_Paladium;
            IDTG_Pricing = CV.TG_PG_Paladium2;
        }
        else if (d.ProductOrder_CharProduct.CharFrameMaterial.NamaKode == "PLA")
        {
            IDTG_Costing = CV.TG_PG_Platina;
            IDTG_Pricing = CV.TG_PG_Platina2;
        }
        else if (d.ProductOrder_CharProduct.CharFrameMaterial.NamaKode == "SIL")
        {
            IDTG_Costing = CV.TG_PG_Silver;
            IDTG_Pricing = CV.TG_PG_Silver2;
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
		if (!ok) PricingError(sebab);
		else {
			ProductOrder_PricingProduct n = new ProductOrder_PricingProduct();

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
			db.ProductOrder_PricingProduct.InsertOnSubmit(n);
			db.SubmitChanges();

            
		}
	}

	//Biaya
	protected void saveBiaya(ProductOrder d, ProductOrder_CharProduct product) {
		string sebab = "";
		bool ok = true;

		//Pricing
		var prc_fm = db.PricingTableFM.SingleOrDefault(p =>
            p.IDBrand == d.IDBrand &&
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
			ProductOrder_PricingBiaya n = new ProductOrder_PricingBiaya();

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
			n.Setting = prc_fs.Biaya.Value;

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
			db.ProductOrder_PricingBiaya.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected int FS_ID(ProductOrder d, ProductOrder_CharProduct product) {
		return 1; //Masih di-hardcoded. BELUM SELESAI !
	}

	//Mark-Up
	protected string saveMU(ProductOrder d, ProductOrder_CharProduct product) {
        string strDebug = "";

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
			ProductOrder_PricingMU n = new ProductOrder_PricingMU();

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

            strDebug = "&TotalToMarkUp=" + TotalToMarkUp(d.ID).ToString()
                    + "&MU=" + n.MU.ToString()
                    + "&TotalStone1B=" + TotalStone1B(d.ID).ToString()
                    + "&TotalStone3NMU=" + TotalStone3NMU(d.ID).ToString()
                    ;

			//Harga Final
            n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID);
            n.HargaRupiah = (TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID)) * n.ModalRate + OngkosLain;
            n.HargaRupiahRound = price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID)) * n.ModalRate + OngkosLain);
            



			//Tgl. Efektif
			n.EfektifRate = prc_us.ApprovalTgl.Value;

			//Commit
			db.ProductOrder_PricingMU.InsertOnSubmit(n);
			db.SubmitChanges();
		}

        return strDebug;
	}

	//Segmen System
	protected void saveSegmen(ProductOrder d, ProductOrder_CharProduct product) {
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
                carat = (from p in db.ProductOrder_Stone1A where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductOrder_Stone1B where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductOrder_Stone2 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductOrder_Stone3 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductOrder_Stone4 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductOrder_Stone5 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Max(p => p.Carat);
            }
            catch (Exception) { }

        }
	}
	protected bool checkSegmen(ProductOrder_CharProduct product) {
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
	protected decimal TotalStone(ProductOrder d) {
		decimal total = 0;

		var rs_1A = from p in d.ProductOrder_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_1A) total += r;

		var rs_1B = from p in d.ProductOrder_Stone1B where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_1B) total += r;

		var rs_2 = from p in d.ProductOrder_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_2) total += r;

		var rs_3 = from p in d.ProductOrder_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_3) total += r;

		var rs_4 = from p in d.ProductOrder_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_4) total += r;

		var rs_5 = from p in d.ProductOrder_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
		foreach (var r in rs_5) total += r;

		return total;
	}
    //Sub-Total
    protected decimal TotalStone1A(ProductOrder d)
    {
        decimal total = 0;

        try
        {
            var rs_1A = from p in d.ProductOrder_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_1A) total += r;
        }
        catch (Exception) { }
        return total;
    }
    protected decimal TotalStone1B(ProductOrder d)
    {
        decimal total = 0;

        try
        {
            var rs_1B = from p in d.ProductOrder_Stone1B where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_1B) total += r;
        }
        catch(Exception) { }
        return total;
    }
    protected decimal TotalStone2(ProductOrder d)
    {
        decimal total = 0;

        try
        {
            var rs_2 = from p in d.ProductOrder_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_2) total += r;
        }
        catch (Exception) { }

        return total;
    }
    protected decimal TotalStone3(ProductOrder d)
    {
        decimal total = 0;

        try
        {
            var rs_3 = from p in d.ProductOrder_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_3) total += r;
        }
        catch (Exception) { }

        return total;
    }
    protected decimal TotalStone4(ProductOrder d)
    {
        decimal total = 0;

        try
        {
            var rs_4 = from p in d.ProductOrder_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_4) total += r;
        }
        catch (Exception) { }

        return total;
    }
    protected decimal TotalStone5(ProductOrder d)
    {
        decimal total = 0;

        try
        {
            var rs_5 = from p in d.ProductOrder_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_5) total += r;
        }
        catch (Exception) { }

        return total;
    }
    protected decimal TotalMStone1A(ProductOrder d)
    {
        decimal total = 0;

        var rs_1A = from p in d.ProductOrder_Stone1A where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_1A) total += r;

        return total;
    }
    protected decimal TotalMStone1B(ProductOrder d)
    {
        decimal total = 0;

        var rs_1B = from p in d.ProductOrder_Stone1B where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_1B) total += r;

        return total;
    }
    protected decimal TotalMStone2(ProductOrder d)
    {
        decimal total = 0;

        var rs_2 = from p in d.ProductOrder_Stone2 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_2) total += r;

        return total;
    }
    protected decimal TotalMStone3(ProductOrder d)
    {
        decimal total = 0;

        var rs_3 = from p in d.ProductOrder_Stone3 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_3) total += r;

        return total;
    }
    protected decimal TotalMStone4(ProductOrder d)
    {
        decimal total = 0;

        var rs_4 = from p in d.ProductOrder_Stone4 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_4) total += r;

        return total;
    }
    protected decimal TotalMStone5(ProductOrder d)
    {
        decimal total = 0;

        var rs_5 = from p in d.ProductOrder_Stone5 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }
	protected decimal TotalFinishing(ProductOrder d) {
		decimal total = 0;

		var rs = from p in d.ProductOrder_Finishing where p.Biaya.HasValue select p.Biaya.Value;
		foreach (var r in rs) total += r;

		return total;
	}
	protected decimal TotalModal(int ID) {
		decimal total = 0;

		var product = db.ProductOrder_PricingProduct.SingleOrDefault(p => p.ID == ID);
		var biaya = db.ProductOrder_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += product.Total;
		total += biaya.Total;

		return total;
	}
    protected decimal TotalToMarkUp(int ID)
    {
        decimal total = 0;

        var product = db.ProductOrder_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.ProductOrder_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        var stone3 = from p in db.ProductOrder_Stone3
                     where p.IDForm == ID && (p.GIA == "" || p.GIA == null)
                     select p;
        decimal Stone3ToMarkUp = 0m;
        try
        {
            Stone3ToMarkUp = stone3.Sum(p => p.HargaTotal.Value);
        }
        catch (Exception) { }

        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(product.TotalStone3) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(Stone3ToMarkUp) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        total += biaya.Total;

        return total;
    }
    protected decimal TotalStone1B(int ID)
    {
        decimal total = 0;

        var product = db.ProductOrder_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.ProductOrder_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalStone1B);
        //total += biaya.Total;

        return total;
    }
    protected decimal TotalStone3NMU(int ID)
    {
        decimal total = 0;

        var stone3 = from p in db.ProductOrder_Stone3
                     where p.IDForm == ID && p.GIA != ""
                     select p;
        try
        {
            total = Convert.ToDecimal(stone3.Sum(p => p.HargaTotal.Value));
        }
        catch (Exception) { }

        //total += biaya.Total;

        return total;
    }
	protected decimal HargaUSD(int ID) {
		var mu = db.ProductOrder_PricingMU.SingleOrDefault(p => p.ID == ID);
		return mu.HargaUSD;
	}
    protected void saveTotalStone(ProductOrder d)
    {
        decimal total = 0;

        var rs_1A = from p in d.ProductOrder_Stone1A select p.TotalButir;
		foreach (var r in rs_1A) total += r;

        var rs_1B = from p in d.ProductOrder_Stone1B where p.StatusTerpasang == 1 select p.TotalButir;
		foreach (var r in rs_1B) total += r;

        var rs_2 = from p in d.ProductOrder_Stone2 select p.TotalButir;
		foreach (var r in rs_2) total += r;

        var rs_3 = from p in d.ProductOrder_Stone3 select p.TotalButir;
		foreach (var r in rs_3) total += r;

        var rs_4 = from p in d.ProductOrder_Stone4 select p.TotalButir;
		foreach (var r in rs_4) total += r;

        var rs_5 = from p in d.ProductOrder_Stone5 select p.TotalButir;
		foreach (var r in rs_5) total += r;

        d.ProductOrder_CharProduct.StoneQty = total;
        db.SubmitChanges();
                
    }
}