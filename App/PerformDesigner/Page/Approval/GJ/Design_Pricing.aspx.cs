using System;
using System.Linq;

public partial class Page_Approval_Design_Pricing : System.Web.UI.Page {
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
		Response.Redirect("Design_Form.aspx?id=" + myID + "&prcerror=" + sebab);
	}

	protected void Page_Load(object sender, EventArgs e) {

		App.Protect();
		App.ProtectRole(Role.Manager);

		var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Draft == false && p.Approval == null);
		if (d == null) App.DataNull(this);
		else {
			//Delete
			deleteProcess(d);

			//Process
			var product = d.ProductDesignGJ_CharProduct;
			if (product == null) PricingError("Product Char.");
			else {
				saveFinishing(d, product);

				saveProduct(d, product);
				saveBiaya(d, product);

				saveMU(d, product);
				saveSegmen(d, product);
			}

			//Redirect
			Response.Redirect("Design_PricingSubmit.aspx?id=" + myID);
		}
	}

	//Delete
	protected void deleteProcess(ProductDesignGJ d) {
		if (d.ProductDesignGJ_PricingProduct != null)
			db.ProductDesignGJ_PricingProduct.DeleteOnSubmit(
				d.ProductDesignGJ_PricingProduct);

		if (d.ProductDesignGJ_PricingBiaya != null)
			db.ProductDesignGJ_PricingBiaya.DeleteOnSubmit(
				d.ProductDesignGJ_PricingBiaya);

		if (d.ProductDesignGJ_PricingMU != null)
			db.ProductDesignGJ_PricingMU.DeleteOnSubmit(
				d.ProductDesignGJ_PricingMU);

		if (d.ProductDesignGJ_PricingSegmen != null)
			db.ProductDesignGJ_PricingSegmen.DeleteOnSubmit(
				d.ProductDesignGJ_PricingSegmen);

		//Commit
		db.SubmitChanges();
	}

	//Finishing
	protected void saveFinishing(ProductDesignGJ d, ProductDesignGJ_CharProduct product) {
		var rs = from p in d.ProductDesignGJ_Finishing select p;
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
	protected void saveProduct(ProductDesignGJ d, ProductDesignGJ_CharProduct product) {
		string sebab = "";
		bool ok = true;

		//Pricing
		var prc_tg = db.PricingTableTG.SingleOrDefault(p =>
			p.ID == CV.TG_GJ_Costing &&
			p.Harga.HasValue &&
			p.ApprovalTgl.HasValue);

		//Pricing - Validasi
		if (prc_tg == null) { ok = false; sebab = "Today Gold Price"; }

		//Process
		if (!ok) PricingError(sebab);
		else {
			ProductDesignGJ_PricingProduct n = new ProductDesignGJ_PricingProduct();

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
			db.ProductDesignGJ_PricingProduct.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}

	//Biaya
	protected void saveBiaya(ProductDesignGJ d, ProductDesignGJ_CharProduct product) {
		string sebab = "";
		bool ok = true;

		//Pricing
		var prc_fm = db.PricingTableFM.SingleOrDefault(p =>
			p.IDBrand == d.DataHuman.IDBrand &&
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
			ProductDesignGJ_PricingBiaya n = new ProductDesignGJ_PricingBiaya();

			n.ID = d.ID;

			//Informasi
			n.NamaBrand = d.DataHuman.CompanyBrand.Nama;
			n.NamaItem = product.CharProductItem.Nama;
			n.NamaCons = product.CharProcessCons.Nama;

			//Mounting
			n.Mounting = prc_fm.Biaya.Value;
			n.MountingNO = prc_fm.InputNO.Value;
			n.MountingRO = prc_fm.InputRO.Value;

			//Setting
			//n.Setting = prc_fs.Biaya.Value;
			n.Setting =  Setting;

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
			db.ProductDesignGJ_PricingBiaya.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected int FS_ID(ProductDesignGJ d, ProductDesignGJ_CharProduct product) {
		return 1; //Masih di-hardcoded. BELUM SELESAI !
	}

	//Mark-Up
	protected void saveMU(ProductDesignGJ d, ProductDesignGJ_CharProduct product) {
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
		if (prc_mu == null) { ok = false; sebab = "Mark-Up Design"; }
		if (prc_us == null) { ok = false; sebab = "USD Rate"; }

		//Process
		if (!ok) PricingError(sebab);
		else {
			ProductDesignGJ_PricingMU n = new ProductDesignGJ_PricingMU();

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

			//Harga Final
            n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID);
            n.HargaRupiah = (TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID)) * n.ModalRate;
            n.HargaRupiahRound = price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID)) * n.ModalRate);

			//Tgl. Efektif
			n.EfektifRate = prc_us.ApprovalTgl.Value;

			//Commit
			db.ProductDesignGJ_PricingMU.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}

	//Segmen System
	protected void saveSegmen(ProductDesignGJ d, ProductDesignGJ_CharProduct product) {
		string sebab = "Segmen System";
		bool ok = true;

		//Pricing - Validasi
		ok = checkSegmen(product);

		//Process
		if (!ok) PricingError(sebab);
		else {
			//Product Value
			decimal harga = HargaUSD(d.ID);
			decimal carat = product.StoneCarat;

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
						 p.IDDist == product.StoneDist
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

			ProductDesignGJ_PricingSegmen n = new ProductDesignGJ_PricingSegmen();

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
			db.ProductDesignGJ_PricingSegmen.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected bool checkSegmen(ProductDesignGJ_CharProduct product) {
		bool ok = true;

		var rs = from p in db.PricingTableSS
				 where
					 p.IDCategory == product.ProductCategory &&
					 p.IDLevel == product.ProductLevel &&
					 p.IDDist == product.StoneDist
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
    protected decimal TotalStone(ProductDesignGJ d)
    {
        decimal total = 0;

        return total;
    }

	//Sub-Total
	protected decimal TotalStone1A(ProductDesignGJ d) {
		decimal total = 0;

		return total;
	}
    protected decimal TotalStone1B(ProductDesignGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone2(ProductDesignGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone3(ProductDesignGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone4(ProductDesignGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone5(ProductDesignGJ d)
    {
        decimal total = 0;

        return total;
    }
	protected decimal TotalFinishing(ProductDesignGJ d) {
		decimal total = 0;

		var rs = from p in d.ProductDesignGJ_Finishing where p.Biaya.HasValue select p.Biaya.Value;
		foreach (var r in rs) total += r;

		return total;
	}
	protected decimal TotalModal(int ID) {
		decimal total = 0;

		var product = db.ProductDesignGJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
		var biaya = db.ProductDesignGJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

		total += product.Total;
		total += biaya.Total;

		return total;
	}
    protected decimal TotalToMarkUp(int ID)
    {
        decimal total = 0;

        var product = db.ProductDesignGJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.ProductDesignGJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(product.TotalStone3) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        total += biaya.Total;

        return total;
    }
    protected decimal TotalStone1B(int ID)
    {
        decimal total = 0;

        var product = db.ProductDesignGJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.ProductDesignGJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalStone1B);
        //total += biaya.Total;

        return total;
    }
	protected decimal HargaUSD(int ID) {
		var mu = db.ProductDesignGJ_PricingMU.SingleOrDefault(p => p.ID == ID);
		return mu.HargaUSD;
	}
}