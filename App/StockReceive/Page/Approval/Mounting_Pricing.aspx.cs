using System;
using System.Linq;

public partial class Page_Approval_Mounting_Pricing : System.Web.UI.Page {
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
		Response.Redirect("Mounting_Form.aspx?id=" + myID + "&prcerror=" + sebab);
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

		var d = db.StockReceiveMounting.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Delete
			deleteProcess(d);

			//Process
			var product = d.StockReceiveMounting_CharProduct;
			if (product == null) PricingError("Product Char.");
			else {
				saveFinishing(d, product);

				saveProduct(d, product);
				saveBiaya(d, product);

				saveMU(d, product);
				saveSegmen(d, product);
			}

			//Redirect
			Response.Redirect("Mounting_PricingSubmit.aspx?id=" + myID);
		}
	}

	//Delete
	protected void deleteProcess(StockReceiveMounting d) {
		if (d.StockReceiveMounting_PricingProduct != null)
			db.StockReceiveMounting_PricingProduct.DeleteOnSubmit(
				d.StockReceiveMounting_PricingProduct);

        if (d.StockReceiveMounting_CostingProduct != null)
            db.StockReceiveMounting_CostingProduct.DeleteOnSubmit(
                d.StockReceiveMounting_CostingProduct);

		if (d.StockReceiveMounting_PricingBiaya != null)
			db.StockReceiveMounting_PricingBiaya.DeleteOnSubmit(
				d.StockReceiveMounting_PricingBiaya);

		if (d.StockReceiveMounting_PricingMU != null)
			db.StockReceiveMounting_PricingMU.DeleteOnSubmit(
				d.StockReceiveMounting_PricingMU);

		if (d.StockReceiveMounting_PricingSegmen != null)
			db.StockReceiveMounting_PricingSegmen.DeleteOnSubmit(
				d.StockReceiveMounting_PricingSegmen);

		//Commit
		db.SubmitChanges();
	}

	//Finishing
	protected void saveFinishing(StockReceiveMounting d, StockReceiveMounting_CharProduct product) {
		var rs = from p in d.StockReceiveMounting_Finishing select p;
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
	protected void saveProduct(StockReceiveMounting d, StockReceiveMounting_CharProduct product) {
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
			StockReceiveMounting_PricingProduct n = new StockReceiveMounting_PricingProduct();

			n.ID = d.ID;

			//Gold Price
			n.GoldRate = prc_tg.Harga.Value;
			n.EfektifGold = prc_tg.ApprovalTgl.Value;

			//Informasi Berat
			n.Netto = product.NettoWeight;
			n.Kadar = product.KadarTukaran;

			//Total
			n.TotalProduct = n.GoldRate * n.Netto * (n.Kadar / (decimal) 100);
            n.Total = n.TotalProduct;

			//Commit
			db.StockReceiveMounting_PricingProduct.InsertOnSubmit(n);
			db.SubmitChanges();

            //Costing
            StockReceiveMounting_CostingProduct m = new StockReceiveMounting_CostingProduct();

            m.ID = d.ID;

            //Gold Price
            m.GoldRate = prc_tgM.Harga.Value;
            m.EfektifGold = prc_tgM.ApprovalTgl.Value;

            //Informasi Berat
            m.Netto = product.NettoWeight;
            m.Kadar = product.KadarTukaran;

            //Total
            m.TotalProduct = m.GoldRate * n.Netto * (n.Kadar / (decimal)100);
            m.Total = m.TotalProduct;

            //Commit
            db.StockReceiveMounting_CostingProduct.InsertOnSubmit(m);
            db.SubmitChanges();
		}
	}

	//Biaya
	protected void saveBiaya(StockReceiveMounting d, StockReceiveMounting_CharProduct product) {
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
			StockReceiveMounting_PricingBiaya n = new StockReceiveMounting_PricingBiaya();

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

			//Commit
			db.StockReceiveMounting_PricingBiaya.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected int FS_ID(StockReceiveMounting d, StockReceiveMounting_CharProduct product) {
		return 1; //Masih di-hardcoded. BELUM SELESAI !
	}

	//Mark-Up
	protected void saveMU(StockReceiveMounting d, StockReceiveMounting_CharProduct product) {
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
			StockReceiveMounting_PricingMU n = new StockReceiveMounting_PricingMU();

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

            try
            {
                OngkosLain = d.StockReceiveMounting_Costing.OngkosLain.Value;
            }
            catch (Exception ex)
            {
                OngkosLain = 0m;
            }

			//Harga Final
            n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + (OngkosLain / d.StockReceiveMounting_Costing.TotalRate.Value);
            n.HargaRupiah = (TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100))) * n.ModalRate  + OngkosLain;
            n.HargaRupiahRound = price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100))) * n.ModalRate + OngkosLain);


			//Tgl. Efektif
			n.EfektifRate = prc_us.ApprovalTgl.Value;

			//Commit
			db.StockReceiveMounting_PricingMU.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}

	//Segmen System
	protected void saveSegmen(StockReceiveMounting d, StockReceiveMounting_CharProduct product) {
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
						 p.IDDist == product.StoneDist
					 orderby p.Minimum
					 select p;

			foreach (var r in rs) {
				if ((r.SegmenSistem == 0 && harga >= r.Minimum.Value)) {

					IDSegmen = r.IDSegmen;
					SegmenSistem = r.SegmenSistem;
					SegmenMinimum = r.Minimum.Value;
					EfektifSegmen = r.ApprovalTgl.Value;
				}
			}

			StockReceiveMounting_PricingSegmen n = new StockReceiveMounting_PricingSegmen();

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

			//Tgl. Efektif
			n.EfektifSegmen = EfektifSegmen;

			///Commit
			db.StockReceiveMounting_PricingSegmen.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected bool checkSegmen(StockReceiveMounting_CharProduct product) {
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
	protected decimal TotalFinishing(StockReceiveMounting d) {
		decimal total = 0;

		var rs = from p in d.StockReceiveMounting_Finishing where p.Biaya.HasValue select p.Biaya.Value;
		foreach (var r in rs) total += r;

		return total;
	}
	protected decimal TotalModal(int ID) {
		decimal total = 0;

		var product = db.StockReceiveMounting_PricingProduct.SingleOrDefault(p => p.ID == ID);
		var biaya = db.StockReceiveMounting_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += product.Total;
		total += biaya.Total;

		return total;
	}
    protected decimal TotalToMarkUp(int ID)
    {
        decimal total = 0;

        var product = db.StockReceiveMounting_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.StockReceiveMounting_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalProduct);
        total += biaya.Total;

        return total;
    }
	protected decimal HargaUSD(int ID) {
		var mu = db.StockReceiveMounting_PricingMU.SingleOrDefault(p => p.ID == ID);
		return mu.HargaUSD;
	}
}