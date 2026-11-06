using System;
using System.Linq;

public partial class Page_Setup : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

		setup(true);
		setup();
		setup(false);

		//Redirect
		Response.Redirect("Summary.aspx?d=1");
	}

	//Setup-Flag
	protected void setup(bool PreProcess) {
		//setup(PreProcess, "TG");
		//setup(PreProcess, "US");
		setup(PreProcess, "FM");
		setup(PreProcess, "FS");
		setup(PreProcess, "FF");
		setup(PreProcess, "MU");
		setup(PreProcess, "SS");
        setup(PreProcess, "Repair");
	}
	protected void setup(bool PreProcess, string TB) {
		if (PreProcess)
			Db.Execute("UPDATE PricingTable" + TB + " SET SetupFlag = 0", Db.Cnn_DB);
		else
			Db.Execute("DELETE FROM PricingTable" + TB + " WHERE SetupFlag = 0", Db.Cnn_DB);
	}

	//Insert-DB
	protected void setup() {
		//setupTG();
		setupUS();
		setupFM();
		setupFS();
		setupFF();
		setupMU();
		setupSS();
        setupFR();
	}

	#region PricingTableTG
	protected void setupTG() {
		setupTG(CV.TG_PG1_Costing, "PG-1 / Costing", "Emas Rangka - COGS");
		setupTG(CV.TG_PG1_Pricing, "PG-1 / Pricing", "Emas Rangka - Retail");
		setupTG(CV.TG_PG2_Costing, "PG-2 / Costing", "Emas Aksesoris - COGS");
        setupTG(CV.TG_PG_Costing1, "PG / NON-CHAIN KUNING - Costing","Emas Aksesoris - Retail");
        setupTG(CV.TG_PG_Costing2, "PG / NON-CHAIN PUTIH - Costing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG_Costing3, "PG / CHAIN KUNING - Costing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG_Costing4, "PG / CHAIN PUTIH - Costing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG_Costing5, "PG / CHAIN KADAR 99 - Costing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG_Costing6, "PG / NON-CHAIN KADAR 99 - Costing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG_Pricing1, "PG / NON-CHAIN KUNING - Pricing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG_Pricing2, "PG / NON-CHAIN PUTIH - Pricing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG_Pricing3, "PG / CHAIN KUNING - Pricing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG_Pricing4, "PG / CHAIN PUTIH - Pricing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG_Pricing5, "PG / CHAIN KADAR 99 - Pricing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG_Pricing6, "PG / NON-CHAIN KADAR 99 - Pricing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG2_Pricing1, "PG LOKAL NON P1 ( Simple ) - Pricing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG2_Pricing2, "PG LOKAL NON P1 ( Fancy ) - Pricing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG2_Pricing3, "PG NON LOKAL (Simple) - Pricing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG2_Pricing4, "PG NON LOKAL (Fancy) - Pricing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG2_Pricing5, "PG LOKAL P1 (Simple) - Pricing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_PG2_Pricing6, "PG LOKAL P1 (Fancy) - Pricing", "Emas Aksesoris - Retail");
        setupTG(CV.TG_GJ_Costing, "GJ / Costing", "Emas Rangka");
        setupTG(CV.TG_GJ_Pricing, "GJ / Pricing", "Emas Rangka");
        setupTG(CV.TG_PG_Paladium, "PG 2 - Paladium Costing", "Emang Rangka Resell / Trade In");
        setupTG(CV.TG_PG_Platina, "PG 2 - Platina / Costing", "Emang Rangka Resell / Trade In");
        setupTG(CV.TG_PG_Silver, "PG 2 - Silver Costing", "Emang Rangka Resell / Trade In");
        setupTG(CV.TG_PG_Paladium2, "PG 2 - Paladium Pricing", "Emang Rangka Resell / Trade In");
        setupTG(CV.TG_PG_Platina2, "PG 2 - Platina / Pricing", "Emang Rangka Resell / Trade In");
        setupTG(CV.TG_PG_Silver2, "PG 2 - Silver Pricing", "Emang Rangka Resell / Trade In");
	}
	protected void setupTG(int ID, string Nama, string Keterangan) {
		var d = db.PricingTableTG.SingleOrDefault(p => p.ID == ID);
		if (d == null) {
			PricingTableTG n = new PricingTableTG();

			n.ID = ID;
			n.Nama = Nama;
			n.Keterangan = Keterangan;

			//Status
			n.SetupFlag = true;
			n.StatusPricing = 1;

			//Commit
			db.PricingTableTG.InsertOnSubmit(n);
			db.SubmitChanges();
		}
		else {
			d.Nama = Nama;
			d.Keterangan = Keterangan;

			//Status
			d.SetupFlag = true;

			//Commit
			db.SubmitChanges();
		}
	}
	#endregion
	#region PricingTableUS
	protected void setupUS() {
		setupUS(CV.US_General_Costing, "General / Costing", "Komponen Umum - COGS");
		setupUS(CV.US_General_Pricing, "General / Pricing", "Komponen Umum - Retail");
        setupUS(CV.US_PG_Fixed, "PG Fixed Rate", "Komponen Umum - Retail");
        setupUS(CV.US_LD_Rate, "LD Rate", "Komponen Umum - Retail");
	}
	protected void setupUS(int ID, string Nama, string Keterangan) {
		var d = db.PricingTableUS.SingleOrDefault(p => p.ID == ID);
		if (d == null) {
			PricingTableUS n = new PricingTableUS();

			n.ID = ID;
			n.Nama = Nama;
			n.Keterangan = Keterangan;

			//Status
			n.SetupFlag = true;
			n.StatusPricing = 1;

			//Commit
			db.PricingTableUS.InsertOnSubmit(n);
			db.SubmitChanges();
		}
		else {
			d.Nama = Nama;
			d.Keterangan = Keterangan;

			//Status
			d.SetupFlag = true;

			//Commit
			db.SubmitChanges();
		}
	}
	#endregion
	#region PricingTableFM
	protected void setupFM() {
		var rs = from p in db.CompanyBrand
				 where
					 !p.Disable
				 orderby p.SN
				 select p;

		foreach (var r1 in rs) setupFM(r1);
	}
	protected void setupFM(CompanyBrand r1) {
		var rs = from p in db.CharProductItem
				 where
					 !p.Disable
				 orderby p.SN
				 select p;

		foreach (var r2 in rs) setupFM(r1, r2);
	}
	protected void setupFM(CompanyBrand r1, CharProductItem r2) {
		var rs = from p in db.CharProcessCons
				 where
					 !p.Disable
				 orderby p.SN
				 select p;

		foreach (var r3 in rs) setupFM(r1, r2, r3);
	}
	protected void setupFM(CompanyBrand r1, CharProductItem r2, CharProcessCons r3) { setupFM(r1.ID, r2.ID, r3.ID); }
	protected void setupFM(int IDBrand, int IDItem, int IDCons) {
		var d = db.PricingTableFM.SingleOrDefault(
			p =>
			p.IDBrand == IDBrand &&
			p.IDItem == IDItem &&
			p.IDCons == IDCons
			);

		if (d == null) {
			PricingTableFM n = new PricingTableFM();

			n.IDBrand = IDBrand;
			n.IDItem = IDItem;
			n.IDCons = IDCons;

			//Status
			n.SetupFlag = true;
			n.StatusPricing = 1;

			//Commit
			db.PricingTableFM.InsertOnSubmit(n);
			db.SubmitChanges();
		}
		else {
			d.SetupFlag = true;
			db.SubmitChanges();
		}
	}
	#endregion
	#region PricingTableFS
	protected void setupFS() {
		setupFS(1, "Setting-1", "");
		setupFS(2, "Setting-2", "");
		setupFS(3, "Setting-3", "");
	}
	protected void setupFS(int ID, string Nama, string Keterangan) {
		var d = db.PricingTableFS.SingleOrDefault(p => p.ID == ID);
		if (d == null) {
			PricingTableFS n = new PricingTableFS();

			n.ID = ID;
			n.Nama = Nama;
			n.Keterangan = Keterangan;

			//Status
			n.SetupFlag = true;
			n.StatusPricing = 1;

			//Commit
			db.PricingTableFS.InsertOnSubmit(n);
			db.SubmitChanges();
		}
		else {
			d.Nama = Nama;
			d.Keterangan = Keterangan;

			//Status
			d.SetupFlag = true;

			//Commit
			db.SubmitChanges();
		}
	}
	#endregion
	#region PricingTableFF
	protected void setupFF() {
		var rs = from p in db.CharProductItem
				 where
					 !p.Disable
				 orderby p.SN
				 select p;

		foreach (var r1 in rs) setupFF(r1);
	}
	protected void setupFF(CharProductItem r1) {
		var rs = from p in db.CharProcessFinishing
				 where
					 !p.Disable
				 orderby p.SN
				 select p;

		foreach (var r2 in rs) setupFF(r1, r2);
	}
	protected void setupFF(CharProductItem r1, CharProcessFinishing r2) { setupFF(r1.ID, r2.ID); }
	protected void setupFF(int IDItem, int IDFinishing) {
		var d = db.PricingTableFF.SingleOrDefault(
			p =>
			p.IDItem == IDItem &&
			p.IDFinishing == IDFinishing
			);

		if (d == null) {
			PricingTableFF n = new PricingTableFF();

			n.IDItem = IDItem;
			n.IDFinishing = IDFinishing;

			//Status
			n.SetupFlag = true;
			n.StatusPricing = 1;

			//Commit
			db.PricingTableFF.InsertOnSubmit(n);
			db.SubmitChanges();
		}
		else {
			d.SetupFlag = true;
			db.SubmitChanges();
		}
	}
	#endregion
	#region PricingTableMU
	protected void setupMU() {
		var rs = from p in db.SettingsStoneDist
				 where
					!p.CharProductCategory.Disable &&
					!p.CharProductLevel.Disable &&
					!p.CharStoneDist.Disable
				 select p;

		foreach (var r1 in rs) setupMU(r1);
	}
	protected void setupMU(SettingsStoneDist r1) { setupMU(r1.IDCategory, r1.IDLevel, r1.IDDist); }
	protected void setupMU(int IDCategory, int IDLevel, int IDDist) {
		var d = db.PricingTableMU.SingleOrDefault(
			p =>
			p.IDCategory == IDCategory &&
			p.IDLevel == IDLevel &&
			p.IDDist == IDDist
			);

		if (d == null) {
			PricingTableMU n = new PricingTableMU();

			n.IDCategory = IDCategory;
			n.IDLevel = IDLevel;
			n.IDDist = IDDist;

			//Status
			n.SetupFlag = true;
			n.StatusPricing = 1;

			//Commit
			db.PricingTableMU.InsertOnSubmit(n);
			db.SubmitChanges();
		}
		else {
			d.SetupFlag = true;
			db.SubmitChanges();
		}
	}
	#endregion
	#region PricingTableSS
	protected void setupSS() {
		var rs = from p in db.SettingsStoneDist
				 where
					 !p.CharProductCategory.Disable &&
					 !p.CharProductLevel.Disable &&
					 !p.CharStoneDist.Disable
				 select p;

		foreach (var r1 in rs) setupSS(r1);
	}
	protected void setupSS(SettingsStoneDist r1) {
		var rs = from p in db.CharProductSegmen
				 where
					 !p.Disable
				 orderby p.SN
				 select p;

		foreach (var r2 in rs) setupSS(r1, r2);
	}
	protected void setupSS(SettingsStoneDist r1, CharProductSegmen r2) { setupSS(r1.IDCategory, r1.IDLevel, r1.IDDist, r2.ID); }
	protected void setupSS(int IDCategory, int IDLevel, int IDDist, int IDSegmen) {
		var d = db.PricingTableSS.SingleOrDefault(
			p =>
			p.IDCategory == IDCategory &&
			p.IDLevel == IDLevel &&
			p.IDDist == IDDist &&
			p.IDSegmen == IDSegmen
			);

		int? SegmenSistem = Pricing.SegmenSistem(IDCategory, IDLevel);
		if (SegmenSistem.HasValue) {
			if (d == null) {
				PricingTableSS n = new PricingTableSS();

				n.IDCategory = IDCategory;
				n.IDLevel = IDLevel;
				n.IDDist = IDDist;
				n.IDSegmen = IDSegmen;
				n.SegmenSistem = SegmenSistem.Value;

				//Status
				n.SetupFlag = true;
				n.StatusPricing = 1;

				//Commit
				db.PricingTableSS.InsertOnSubmit(n);
				db.SubmitChanges();
			}
			else {
				//Sistem Segmen
				if (d.SegmenSistem != SegmenSistem.Value) {
					d.SegmenSistem = SegmenSistem.Value;
					d.StatusPricing = 1; //Sistem berubah : Harus update ulang
				}

				d.SetupFlag = true;
				db.SubmitChanges();
			}
		}
	}
	#endregion
    #region PricingTableRepair
    protected void setupFR()
    {
        var rs = from p in db.CharProcessRepair
                 where
                     !p.Disable
                 orderby p.SN
                 select p;

        foreach (var r2 in rs) setupFR(r2);
    }
    protected void setupFR(CharProcessRepair r2) { setupFR(r2.ID); }
    protected void setupFR(int IDRepair)
    {
        var d = db.PricingTableRepair.SingleOrDefault(
            p =>
            p.IDRepair == IDRepair
            );

        if (d == null)
        {
            PricingTableRepair n = new PricingTableRepair();

            n.IDRepair = IDRepair;

            //Status
            n.SetupFlag = true;
            n.StatusPricing = 1;

            //Commit
            db.PricingTableRepair.InsertOnSubmit(n);
            db.SubmitChanges();
        }
        else
        {
            d.SetupFlag = true;
            db.SubmitChanges();
        }
    }
    #endregion
}