using System;
using System.Linq;

public partial class Page_Register_Repair_StoneAddSave : System.Web.UI.Page {
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
	protected int myID { get { return App.QueryInt(this); } }
	protected int myStoneIndex { get { return App.QueryInt(this, "si"); } }
	protected int myIDStone { get { return App.QueryInt(this, "idstone"); } }
    static App price = new App();

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			totalbutir.Focus();

			//Number-Format
			App.NumberFormat(totalbutir);
			App.NumberFormat(totalcarat);

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Repair.aspx"); }
    protected void LinkError() { Response.Redirect("Repair_DataStone.aspx?id=" + myID); }
    protected void ProductError() { Response.Redirect("Repair_CharProduct02.aspx?id=" + myID); }

	//Form
	protected void fillForm() {
		var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Repair_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Cancel-Link
			cancel.HRef += "?id=" + myID;

            //Cancel-Link
            cancel.HRef += "?id=" + myID;

            //Stone Data
            stone_index.InnerHtml = DL.StoneIndex(myStoneIndex);
            switch (myStoneIndex)
            {
                case 0: fillStone1A(); break;
                case 1: fillStone1B(); break;
                case 2: fillStone2(); break;
                case 3: fillStone3(); break;
                case 4: fillStone4(); break;
                case 5: fillStone5(); break;
            }
		}
	}

    //Stone Data
    protected void fillStone1A()
    {
        var d = db.Stone1A.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) LinkError();
        else
        {
            //stone_nama.InnerHtml = d.Nama;
            stone_nama.InnerHtml = d.Parcel101.Nama + " " + d.Parcel102.Nama; //+" " + d.Parcel103.Nama + " " + d.Parcel104.Nama;

            //Size Control
            if (!d.Parcel102.SizeMin.HasValue || !d.Parcel102.SizeMax.HasValue) sizeError();
            else
                sizeDisplay(
                    d.Parcel102.SizeMin,
                    d.Parcel102.SizeMax
                    );
        }
    }
    protected void fillStone1B()
    {
        var d = db.Stone1B.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) LinkError();
        else
        {
            //stone_nama.InnerHtml = d.Nama;
            stone_nama.InnerHtml = d.Parcel101.Nama + " " + d.Parcel102.Nama + " " + d.Parcel103.Nama + " " + d.Parcel104.Nama
                + " " + d.Parcel105.Nama + " " + d.Parcel106.Nama + " " + d.Parcel107.Nama;

            //Size Control
            if (!d.Parcel102.SizeMin.HasValue || !d.Parcel102.SizeMax.HasValue) sizeError();
            else
                sizeDisplay(
                    d.Parcel102.SizeMin,
                    d.Parcel102.SizeMax
                    );

            //GIA
            div_gia.Visible = true;
        }
    }
    protected void fillStone2()
    {
        var d = db.Stone2.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) LinkError();
        else
        {
            //stone_nama.InnerHtml = d.Nama;
            stone_nama.InnerHtml = d.Parcel201.Nama + " " + d.Parcel202.Nama + " " + d.Parcel203.Nama + " " + d.Parcel204.Nama;

            //Size Control
            if (!d.Parcel203.SizeMin.HasValue || !d.Parcel203.SizeMax.HasValue) sizeError();
            else
                sizeDisplay(
                    d.Parcel203.SizeMin,
                    d.Parcel203.SizeMax
                    );
        }
    }
    protected void fillStone3()
    {
        var d = db.Stone3.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) LinkError();
        else
        {
            //stone_nama.InnerHtml = d.Nama;
            stone_nama.InnerHtml = d.Parcel301.Nama + " " + d.Parcel302.Nama + " " + d.Parcel303.Nama + " " + d.Parcel304.Nama;

            //Size Control
            if (!d.Parcel303.SizeMin.HasValue || !d.Parcel303.SizeMax.HasValue) sizeError();
            else
                sizeDisplay(
                    d.Parcel303.SizeMin,
                    d.Parcel303.SizeMax
                    );

            //GIA
            div_gia.Visible = true;
        }
    }
    protected void fillStone4()
    {
        var d = db.Stone4.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) LinkError();
        else
        {
            //stone_nama.InnerHtml = d.Nama;
            stone_nama.InnerHtml = d.Parcel401.Nama + " " + d.Parcel402.Nama + " " + d.Parcel403.Nama;

            //Size Control
            if (!d.Parcel403.SizeMin.HasValue || !d.Parcel403.SizeMax.HasValue) sizeError();
            else
                sizeDisplay(
                    d.Parcel403.SizeMin,
                    d.Parcel403.SizeMax
                    );
        }
    }
    protected void fillStone5()
    {
        var d = db.Stone5.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) LinkError();
        else
        {
            //stone_nama.InnerHtml = d.Nama;
            stone_nama.InnerHtml = d.Parcel501.Nama + " " + d.Parcel502.Nama + " " + d.Parcel503.Nama + " " + d.Parcel504.Nama
                + " " + d.Parcel505.Nama + " " + d.Parcel506.Nama + " " + d.Parcel507.Nama;

            //Size Control
            p_sizecontrol.Visible = false;
        }
    }

	//Size Control
	protected void sizeError() {
		save.Disabled = true;

		//Belum Setup
		sizecontrol.Attributes["class"] = "text-red";
		sizecontrol.InnerHtml = "<i class=\"icon-warning-sign\"></i> Belum Setup";
	}
	protected void sizeDisplay(decimal? SizeMin, decimal? SizeMax) {
		string x = string.Empty;

		x += Str.Number3(SizeMin);
		x += " - ";
		x += Str.Number3(SizeMax);

		sizecontrol.InnerHtml = x;
	}

    //Validasi Carat
    protected bool carat1A(int TotalButir, decimal TotalCarat)
    {
        var d = db.Stone1A.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) { LinkError(); return false; }
        else
        {
            decimal Min = d.Parcel102.SizeMin.Value;
            decimal Max = d.Parcel102.SizeMax.Value;

            decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
            return (CB >= Min && CB <= Max);
        }
    }
    protected bool carat1B(int TotalButir, decimal TotalCarat)
    {
        var d = db.Stone1B.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) { LinkError(); return false; }
        else
        {
            decimal Min = d.Parcel102.SizeMin.Value;
            decimal Max = d.Parcel102.SizeMax.Value;

            decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
            return (CB >= Min && CB <= Max);
        }
    }
    protected bool carat2(int TotalButir, decimal TotalCarat)
    {
        var d = db.Stone2.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) { LinkError(); return false; }
        else
        {
            decimal Min = d.Parcel203.SizeMin.Value;
            decimal Max = d.Parcel203.SizeMax.Value;

            decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
            return (CB >= Min && CB <= Max);
        }
    }
    protected bool carat3(int TotalButir, decimal TotalCarat)
    {
        var d = db.Stone3.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) { LinkError(); return false; }
        else
        {
            decimal Min = d.Parcel303.SizeMin.Value;
            decimal Max = d.Parcel303.SizeMax.Value;

            decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
            return (CB >= Min && CB <= Max);
        }
    }
    protected bool carat4(int TotalButir, decimal TotalCarat)
    {
        var d = db.Stone4.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) { LinkError(); return false; }
        else
        {
            decimal Min = d.Parcel403.SizeMin.Value;
            decimal Max = d.Parcel403.SizeMax.Value;

            decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
            return (CB >= Min && CB <= Max);
        }
    }

	//Save
	protected bool valid {
		get {
			bool x = true;

			x = x && Fv.isInt(totalbutir, totalbutir_ERR);
			x = x && Fv.isDecimal(totalcarat, totalcarat_ERR);

			//Value
			x = x && valid_TotalButir;
			x = x && valid_TotalCarat;

			//Size-Control
			x = x && valid_CaratButir;

			return x;
		}
	}
	protected bool valid_TotalButir {
		get {
			int dv = Convert.ToInt32(totalbutir.Text);

			bool x = dv > 0;
			totalbutir_VAL.Visible = !x;

			return x;
		}
	}
	protected bool valid_TotalCarat {
		get {
			decimal dv = Convert.ToDecimal(totalcarat.Text);

			bool x = dv > 0;
			totalcarat_VAL.Visible = !x;

			return x;
		}
	}
	protected bool valid_CaratButir {
		get {
			bool x = true;

			int TotalButir = Convert.ToInt32(totalbutir.Text);
			decimal TotalCarat = Convert.ToDecimal(totalcarat.Text);

            switch (myStoneIndex)
            {
                case 0: x = carat1A(TotalButir, TotalCarat); break;
                case 1: x = carat1B(TotalButir, TotalCarat); break;
                case 2: x = carat2(TotalButir, TotalCarat); break;
                case 3: x = carat3(TotalButir, TotalCarat); break;
                case 4: x = carat4(TotalButir, TotalCarat); break;
            }
			caratbutir_ERR.Visible = !x;
			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
			if (d == null) DataError();
			else {
				//Process
				var product = d.DocRepair_CharProduct;
				if (product == null) ProductError();
				else {
                    switch (myStoneIndex)
                    {
                        case 0: save1A(d); break;
                        case 1: save1B(d); break;
                        case 2: save2(d); break;
                        case 3: save3(d); break;
                        case 4: save4(d); break;
                        case 5: save5(d); break;
                    }

					//Recalculate
					//Lib.RecalWeight(db, d, product);
				}

				//Redirect
				Response.Redirect("Repair_DataStone.aspx?id=" + myID);
			}
		}
	}

    protected void save1A(DocRepair ms)
    {
        var d = db.Stone1A.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) LinkError();
        else
        {
            DocRepair_Stone1A n = new DocRepair_Stone1A();

            n.IDForm = ms.ID;
            n.IDStone = d.ID;

            int totalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
            decimal totalCarat = Convert.ToDecimal(totalcarat.Text);

            n.TotalButir = totalButir;
            n.TotalCarat = totalCarat;
            n.CaratButir = totalButir > 0 ? totalCarat / totalButir : 0;
            n.Keterangan = keterangan.Text;

            //Stone Pricing
            StonePricing1A prc = db.StonePricing1A.SingleOrDefault(p => p.ID == n.IDStone);

            //Tgl. Efektif
            n.Efektif = prc.ApprovalTgl.Value;

            string xa = ms.DocRepair_CharProduct.CharProductItem.NamaKode;
            string xb = d.Parcel101.NamaKode;
            string xc = ms.DocRepair_CharProduct.CharStoneDist.NamaKode;
            decimal xd = Convert.ToDecimal(totalCarat);
            decimal xe = Convert.ToDecimal(totalButir);

            //Setting
            decimal OngkosPB = price.CalculateOngkosPB(xa , xb , xc , xd , xe);
            n.Setting = OngkosPB;

            var x = db.Stone1A.SingleOrDefault(p => p.ID == n.IDStone && !p.Disable);
            var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                    p.ID == CV.US_General_Pricing &&
                    p.Harga.HasValue &&
                    p.ApprovalTgl.HasValue);

            n.HargaSatuan = x.StonePricing1A.Harga.Value;
            n.HargaSatuanM = x.StoneCosting1A.Harga;
            n.HargaTotal = (x.StonePricing1A.Harga.Value * totalCarat);
            n.HargaTotalM = x.StoneCosting1A.Harga.Value * totalCarat;
            n.HargaTotalRupiah = ((x.StonePricing1A.Harga.Value * totalCarat) * prc_us.Harga * 1.9m) + OngkosPB;

            //Dimensi
            n.DimensiP = Convert.ToDecimal(dimensip.Text);
            if (dimensil.Text != "") n.DimensiL = Convert.ToDecimal(dimensil.Text);
            if (dimensit.Text != "") n.DimensiT = Convert.ToDecimal(dimensit.Text);

            //Commit
            db.DocRepair_Stone1A.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected void save1B(DocRepair ms)
    {
        var d = db.Stone1B.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) LinkError();
        else
        {
            DocRepair_Stone1B n = new DocRepair_Stone1B();

            int totalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
            decimal totalCarat = Convert.ToDecimal(totalcarat.Text);

            n.TotalButir = totalButir;
            n.TotalCarat = totalCarat;
            n.CaratButir = totalButir > 0 ? totalCarat / totalButir : 0;
            n.Keterangan = keterangan.Text;

            //Stone Pricing
            StonePricing1B prc = db.StonePricing1B.SingleOrDefault(p => p.ID == n.IDStone);

            //Tgl. Efektif
            n.Efektif = prc.ApprovalTgl.Value;

            string xa = ms.DocRepair_CharProduct.CharProductItem.NamaKode;
            string xb = d.Parcel101.NamaKode;
            string xc = ms.DocRepair_CharProduct.CharStoneDist.NamaKode;
            decimal xd = Convert.ToDecimal(totalCarat);
            decimal xe = Convert.ToDecimal(totalButir);

            //Setting
            decimal OngkosPB = price.CalculateOngkosPB(xa, xb, xc, xd, xe);
            n.Setting = OngkosPB;

            var x = db.Stone1B.SingleOrDefault(p => p.ID == n.IDStone && !p.Disable);
            var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                    p.ID == CV.US_General_Pricing &&
                    p.Harga.HasValue &&
                    p.ApprovalTgl.HasValue);

            n.HargaSatuan = x.StonePricing1B.Harga.Value;
            n.HargaSatuanM = x.StoneCosting1B.Harga;
            n.HargaTotal = (x.StonePricing1B.Harga.Value * totalCarat) + ((d.StonePricing1B.Harga.Value * totalCarat) * d.StonePricing1B.MarkUp / 100);
            n.HargaTotalM = x.StoneCosting1B.Harga.Value * totalCarat;
            n.HargaTotalRupiah = ((x.StonePricing1B.Harga.Value * totalCarat) + ((d.StonePricing1B.Harga.Value * totalCarat) * d.StonePricing1B.MarkUp / 100) * prc_us.Harga) + OngkosPB;

            //Dimensi
            n.DimensiP = Convert.ToDecimal(dimensip.Text);
            if (dimensil.Text != "") n.DimensiL = Convert.ToDecimal(dimensil.Text);
            if (dimensit.Text != "") n.DimensiT = Convert.ToDecimal(dimensit.Text);

            //GIA
            n.GIA = Str.CapUpper(gia.Text);

            //Commit
            db.DocRepair_Stone1B.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected void save2(DocRepair ms)
    {
        var d = db.Stone2.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) LinkError();
        else
        {
            DocRepair_Stone2 n = new DocRepair_Stone2();

            n.IDForm = ms.ID;
            n.IDStone = d.ID;

            int totalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
            decimal totalCarat = Convert.ToDecimal(totalcarat.Text);

            n.TotalButir = totalButir;
            n.TotalCarat = totalCarat;
            n.CaratButir = totalButir > 0 ? totalCarat / totalButir : 0;
            n.Keterangan = keterangan.Text;

            //Stone Pricing
            StonePricing2 prc = db.StonePricing2.SingleOrDefault(p => p.ID == n.IDStone);

            //Tgl. Efektif
            n.Efektif = prc.ApprovalTgl.Value;

            string xa = ms.DocRepair_CharProduct.CharProductItem.NamaKode;
            string xb = d.Parcel201.NamaKode;
            string xc = ms.DocRepair_CharProduct.CharStoneDist.NamaKode;
            decimal xd = Convert.ToDecimal(totalCarat);
            decimal xe = Convert.ToDecimal(totalButir);

            //Setting
            decimal OngkosPB = price.CalculateOngkosPB(xa, xb, xc, xd, xe);
            n.Setting = OngkosPB;

            var x = db.Stone2.SingleOrDefault(p => p.ID == n.IDStone && !p.Disable);
            var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                    p.ID == CV.US_General_Pricing &&
                    p.Harga.HasValue &&
                    p.ApprovalTgl.HasValue);

            n.HargaSatuan = x.StonePricing2.Harga.Value;
            n.HargaSatuanM = x.StoneCosting2.Harga;
            n.HargaTotal = (x.StonePricing2.Harga.Value * totalCarat);
            n.HargaTotalM = x.StoneCosting2.Harga.Value * totalCarat;
            n.HargaTotalRupiah = ((x.StonePricing2.Harga.Value * totalCarat) * prc_us.Harga) + OngkosPB;

            //Dimensi
            n.DimensiP = Convert.ToDecimal(dimensip.Text);
            if (dimensil.Text != "") n.DimensiL = Convert.ToDecimal(dimensil.Text);
            if (dimensit.Text != "") n.DimensiT = Convert.ToDecimal(dimensit.Text);

            //Commit
            db.DocRepair_Stone2.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected void save3(DocRepair ms)
    {
        var d = db.Stone3.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) LinkError();
        else
        {
            DocRepair_Stone3 n = new DocRepair_Stone3();

            n.IDForm = ms.ID;
            n.IDStone = d.ID;

            int totalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
            decimal totalCarat = Convert.ToDecimal(totalcarat.Text);

            n.TotalButir = totalButir;
            n.TotalCarat = totalCarat;
            n.CaratButir = totalButir > 0 ? totalCarat / totalButir : 0;
            n.Keterangan = keterangan.Text;

            //Stone Pricing
            StonePricing3 prc = db.StonePricing3.SingleOrDefault(p => p.ID == n.IDStone);

            //Tgl. Efektif
            n.Efektif = prc.ApprovalTgl.Value;

            string xa = ms.DocRepair_CharProduct.CharProductItem.NamaKode;
            string xb = d.Parcel301.NamaKode;
            string xc = ms.DocRepair_CharProduct.CharStoneDist.NamaKode;
            decimal xd = Convert.ToDecimal(totalCarat);
            decimal xe = Convert.ToDecimal(totalButir);

            //Setting
            decimal OngkosPB = price.CalculateOngkosPB(xa, xb, xc, xd, xe);
            n.Setting = OngkosPB;

            var x = db.Stone3.SingleOrDefault(p => p.ID == n.IDStone && !p.Disable);
            var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                    p.ID == CV.US_General_Pricing &&
                    p.Harga.HasValue &&
                    p.ApprovalTgl.HasValue);

            n.HargaSatuan = x.StonePricing3.Harga.Value;
            n.HargaSatuanM = x.StoneCosting3.Harga;
            n.HargaTotal = (x.StonePricing3.Harga.Value * totalCarat);
            n.HargaTotalM = x.StoneCosting3.Harga.Value * totalCarat;
            n.HargaTotalRupiah = ((x.StonePricing3.Harga.Value * totalCarat) * prc_us.Harga) + OngkosPB;

            //Dimensi
            n.DimensiP = Convert.ToDecimal(dimensip.Text);
            if (dimensil.Text != "") n.DimensiL = Convert.ToDecimal(dimensil.Text);
            if (dimensit.Text != "") n.DimensiT = Convert.ToDecimal(dimensit.Text);

            //GIA
            n.GIA = Str.CapUpper(gia.Text);

            //Commit
            db.DocRepair_Stone3.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected void save4(DocRepair ms)
    {
        var d = db.Stone4.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) LinkError();
        else
        {
            DocRepair_Stone4 n = new DocRepair_Stone4();

            n.IDForm = ms.ID;
            n.IDStone = d.ID;

            int totalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
            decimal totalCarat = Convert.ToDecimal(totalcarat.Text);

            n.TotalButir = totalButir;
            n.TotalCarat = totalCarat;
            n.CaratButir = totalButir > 0 ? totalCarat / totalButir : 0;
            n.Keterangan = keterangan.Text;

            //Stone Pricing
            StonePricing4 prc = db.StonePricing4.SingleOrDefault(p => p.ID == n.IDStone);

            //Tgl. Efektif
            n.Efektif = prc.ApprovalTgl.Value;

            string xa = ms.DocRepair_CharProduct.CharProductItem.NamaKode;
            string xb = d.Parcel401.NamaKode;
            string xc = ms.DocRepair_CharProduct.CharStoneDist.NamaKode;
            decimal xd = Convert.ToDecimal(totalCarat);
            decimal xe = Convert.ToDecimal(totalButir);

            //Setting
            decimal OngkosPB = price.CalculateOngkosPB(xa, xb, xc, xd, xe);
            n.Setting = OngkosPB;

            var x = db.Stone4.SingleOrDefault(p => p.ID == n.IDStone && !p.Disable);
            var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                    p.ID == CV.US_General_Pricing &&
                    p.Harga.HasValue &&
                    p.ApprovalTgl.HasValue);

            n.HargaSatuan = x.StonePricing4.Harga.Value;
            n.HargaSatuanM = x.StoneCosting4.Harga;
            n.HargaTotal = (x.StonePricing4.Harga.Value * totalCarat);
            n.HargaTotalM = x.StoneCosting4.Harga.Value * totalCarat;
            n.HargaTotalRupiah = ((x.StonePricing4.Harga.Value * totalCarat) * prc_us.Harga) + OngkosPB;

            //Dimensi
            n.DimensiP = Convert.ToDecimal(dimensip.Text);
            if (dimensil.Text != "") n.DimensiL = Convert.ToDecimal(dimensil.Text);
            if (dimensit.Text != "") n.DimensiT = Convert.ToDecimal(dimensit.Text);

            //Commit
            db.DocRepair_Stone4.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected void save5(DocRepair ms)
    {
        var d = db.Stone5.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
        if (d == null) LinkError();
        else
        {
            DocRepair_Stone5 n = new DocRepair_Stone5();

            n.IDForm = ms.ID;
            n.IDStone = d.ID;

            int totalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
            decimal totalCarat = Convert.ToDecimal(totalcarat.Text);

            n.TotalButir = totalButir;
            n.TotalCarat = totalCarat;
            n.CaratButir = totalButir > 0 ? totalCarat / totalButir : 0;
            n.Keterangan = keterangan.Text;

            //Stone Pricing
            StonePricing5 prc = db.StonePricing5.SingleOrDefault(p => p.ID == n.IDStone);

            //Tgl. Efektif
            n.Efektif = prc.ApprovalTgl.Value;

            string xa = ms.DocRepair_CharProduct.CharProductItem.NamaKode;
            string xb = d.Parcel501.NamaKode;
            string xc = ms.DocRepair_CharProduct.CharStoneDist.NamaKode;
            decimal xd = Convert.ToDecimal(totalCarat);
            decimal xe = Convert.ToDecimal(totalButir);

            //Setting
            decimal OngkosPB = price.CalculateOngkosPB(xa, xb, xc, xd, xe);
            n.Setting = OngkosPB;

            var x = db.Stone5.SingleOrDefault(p => p.ID == n.IDStone && !p.Disable);
            var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                    p.ID == CV.US_General_Pricing &&
                    p.Harga.HasValue &&
                    p.ApprovalTgl.HasValue);

            n.HargaSatuan = x.StonePricing5.Harga.Value;
            n.HargaSatuanM = x.StoneCosting5.Harga;
            n.HargaTotal = (x.StonePricing5.Harga.Value * totalCarat);
            n.HargaTotalM = x.StoneCosting5.Harga.Value * totalCarat;
            n.HargaTotalRupiah = ((x.StonePricing5.Harga.Value * totalCarat) * prc_us.Harga) + OngkosPB;

            //Dimensi
            n.DimensiP = Convert.ToDecimal(dimensip.Text);
            if (dimensil.Text != "") n.DimensiL = Convert.ToDecimal(dimensil.Text);
            if (dimensit.Text != "") n.DimensiT = Convert.ToDecimal(dimensit.Text);

            //Commit
            db.DocRepair_Stone5.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
}