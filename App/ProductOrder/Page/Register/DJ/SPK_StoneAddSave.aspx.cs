using System;
using System.Linq;

public partial class Page_Register_SPK_StoneAddSave : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			totalbutir.Focus();

			//Number-Format
			App.NumberFormat(totalbutir);
			App.NumberFormat(totalcarat);
			App.NumberFormat(dimensip);
			App.NumberFormat(dimensil);
			App.NumberFormat(dimensit);

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("SPK.aspx"); }
	protected void LinkError() { Response.Redirect("SPK_Stone.aspx?id=" + myID); }
	protected void ProductError() { Response.Redirect("SPK_CharProduct01.aspx?id=" + myID); }

	//Form
	protected void fillForm() {
		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
            if (tipe == 1)
            {
                header_nomor.InnerHtml =
                    "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                    + d.ProductDesign.Nomor + "</a>";
            }
            else
            {
                header_nomor.InnerHtml =
                    "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                    + d.KodeKaretDJ.Nomor + "</a>";
            }

			//Cancel-Link
			cancel.HRef += "?id=" + myID;

            if ((d.ProductOrder_CharProduct.CharProductLevel.NamaKode == "MTG" || d.ProductOrder_CharProduct.CharProductLevel.NamaKode == "SMG") && myStoneIndex == 1)
                colterpasang.Visible = true;
            else
                colterpasang.Visible = false;

			//Stone Data
			stone_index.InnerHtml = DL.StoneIndex(myStoneIndex);
			switch (myStoneIndex) {
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
	protected void fillStone1A() {
		var d = db.Stone1A.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else {
			stone_nama.InnerHtml = d.Nama;

			//Size Control
			if (!d.Parcel102.SizeMin.HasValue || !d.Parcel102.SizeMax.HasValue) sizeError();
			else
				sizeDisplay(
					d.Parcel102.SizeMin,
					d.Parcel102.SizeMax
					);
		}
	}
	protected void fillStone1B() {
		var d = db.Stone1B.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else {
			stone_nama.InnerHtml = d.Nama;

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
	protected void fillStone2() {
		var d = db.Stone2.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else {
			stone_nama.InnerHtml = d.Nama;

			//Size Control
			if (!d.Parcel203.SizeMin.HasValue || !d.Parcel203.SizeMax.HasValue) sizeError();
			else
				sizeDisplay(
					d.Parcel203.SizeMin,
					d.Parcel203.SizeMax
					);
		}
	}
	protected void fillStone3() {
		var d = db.Stone3.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else {
			stone_nama.InnerHtml = d.Nama;

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
	protected void fillStone4() {
		var d = db.Stone4.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else {
			stone_nama.InnerHtml = d.Nama;

			//Size Control
			if (!d.Parcel403.SizeMin.HasValue || !d.Parcel403.SizeMax.HasValue) sizeError();
			else
				sizeDisplay(
					d.Parcel403.SizeMin,
					d.Parcel403.SizeMax
					);
		}
	}
	protected void fillStone5() {
		var d = db.Stone5.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else {
			stone_nama.InnerHtml = d.Nama;

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
	protected bool carat1A(int TotalButir, decimal TotalCarat) {
		var d = db.Stone1A.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) { LinkError(); return false; }
		else {
			decimal Min = d.Parcel102.SizeMin.Value;
			decimal Max = d.Parcel102.SizeMax.Value;

			decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
			return (CB >= Min && CB <= Max);
		}
	}
	protected bool carat1B(int TotalButir, decimal TotalCarat) {
		var d = db.Stone1B.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) { LinkError(); return false; }
		else {
			decimal Min = d.Parcel102.SizeMin.Value;
			decimal Max = d.Parcel102.SizeMax.Value;

			decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
			return (CB >= Min && CB <= Max);
		}
	}
	protected bool carat2(int TotalButir, decimal TotalCarat) {
		var d = db.Stone2.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) { LinkError(); return false; }
		else {
			decimal Min = d.Parcel203.SizeMin.Value;
			decimal Max = d.Parcel203.SizeMax.Value;

			decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
			return (CB >= Min && CB <= Max);
		}
	}
	protected bool carat3(int TotalButir, decimal TotalCarat) {
		var d = db.Stone3.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) { LinkError(); return false; }
		else {
			decimal Min = d.Parcel303.SizeMin.Value;
			decimal Max = d.Parcel303.SizeMax.Value;

			decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
			return (CB >= Min && CB <= Max);
		}
	}
	protected bool carat4(int TotalButir, decimal TotalCarat) {
		var d = db.Stone4.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) { LinkError(); return false; }
		else {
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

			x = x && Fv.isDecimal(totalbutir, totalbutir_ERR);
			x = x && Fv.isDecimal(totalcarat, totalcarat_ERR);
			x = x && Fv.isDecimal(dimensip, dimensip_ERR);
			x = x && Fv.isDecimal(dimensil, dimensil_ERR, true);
			x = x && Fv.isDecimal(dimensit, dimensit_ERR, true);

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
            int dv = Convert.ToInt32(totalbutir.Text.Replace(".", ""));

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

            int TotalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
			decimal TotalCarat = Convert.ToDecimal(totalcarat.Text);

			switch (myStoneIndex) {
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
			var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
			if (d == null) DataError();
			else {
				//Process
				var product = d.ProductOrder_CharProduct;
				if (product == null) ProductError();
				else {
					switch (myStoneIndex) {
						case 0: save1A(d); break;
						case 1: save1B(d); break;
						case 2: save2(d); break;
						case 3: save3(d); break;
						case 4: save4(d); break;
						case 5: save5(d); break;
					}

					//Recalculate
					Lib.RecalWeight(db, d, product);
				}

				//Redirect
				Response.Redirect("SPK_Stone.aspx?id=" + myID + "&tipe=" + tipe);
			}
		}
	}

	//Process
	protected void save1A(ProductOrder ms) {
		var d = db.Stone1A.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else {
			ProductOrder_Stone1A n = new ProductOrder_Stone1A();

			n.IDForm = ms.ID;
			n.IDStone = d.ID;

            n.TotalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
			n.TotalCarat = Convert.ToDecimal(totalcarat.Text);
			n.CaratButir = n.TotalButir > 0 ? n.TotalCarat / n.TotalButir : 0;

			//Dimensi
			n.DimensiP = Convert.ToDecimal(dimensip.Text);
			if (dimensil.Text != "") n.DimensiL = Convert.ToDecimal(dimensil.Text);
			if (dimensit.Text != "") n.DimensiT = Convert.ToDecimal(dimensit.Text);

			//Commit
			db.ProductOrder_Stone1A.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected void save1B(ProductOrder ms) {
		var d = db.Stone1B.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else {
			ProductOrder_Stone1B n = new ProductOrder_Stone1B();

			n.IDForm = ms.ID;
			n.IDStone = d.ID;

            n.TotalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
			n.TotalCarat = Convert.ToDecimal(totalcarat.Text);
			n.CaratButir = n.TotalButir > 0 ? n.TotalCarat / n.TotalButir : 0;
            n.StatusTerpasang = Convert.ToInt32(terpasang.Checked);

			//Dimensi
			n.DimensiP = Convert.ToDecimal(dimensip.Text);
			if (dimensil.Text != "") n.DimensiL = Convert.ToDecimal(dimensil.Text);
			if (dimensit.Text != "") n.DimensiT = Convert.ToDecimal(dimensit.Text);

			//GIA
			n.GIA = Str.CapUpper(gia.Text);

			//Commit
			db.ProductOrder_Stone1B.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected void save2(ProductOrder ms) {
		var d = db.Stone2.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else {
			ProductOrder_Stone2 n = new ProductOrder_Stone2();

			n.IDForm = ms.ID;
			n.IDStone = d.ID;

            n.TotalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
			n.TotalCarat = Convert.ToDecimal(totalcarat.Text);
			n.CaratButir = n.TotalButir > 0 ? n.TotalCarat / n.TotalButir : 0;

			//Dimensi
			n.DimensiP = Convert.ToDecimal(dimensip.Text);
			if (dimensil.Text != "") n.DimensiL = Convert.ToDecimal(dimensil.Text);
			if (dimensit.Text != "") n.DimensiT = Convert.ToDecimal(dimensit.Text);

			//Commit
			db.ProductOrder_Stone2.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected void save3(ProductOrder ms) {
		var d = db.Stone3.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else {
			ProductOrder_Stone3 n = new ProductOrder_Stone3();

			n.IDForm = ms.ID;
			n.IDStone = d.ID;

            n.TotalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
			n.TotalCarat = Convert.ToDecimal(totalcarat.Text);
			n.CaratButir = n.TotalButir > 0 ? n.TotalCarat / n.TotalButir : 0;

			//Dimensi
			n.DimensiP = Convert.ToDecimal(dimensip.Text);
			if (dimensil.Text != "") n.DimensiL = Convert.ToDecimal(dimensil.Text);
			if (dimensit.Text != "") n.DimensiT = Convert.ToDecimal(dimensit.Text);

            //GIA
            n.GIA = Str.CapUpper(gia.Text);

			//Commit
			db.ProductOrder_Stone3.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected void save4(ProductOrder ms) {
		var d = db.Stone4.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else {
			ProductOrder_Stone4 n = new ProductOrder_Stone4();

			n.IDForm = ms.ID;
			n.IDStone = d.ID;

            n.TotalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
			n.TotalCarat = Convert.ToDecimal(totalcarat.Text);
			n.CaratButir = n.TotalButir > 0 ? n.TotalCarat / n.TotalButir : 0;

			//Dimensi
			n.DimensiP = Convert.ToDecimal(dimensip.Text);
			if (dimensil.Text != "") n.DimensiL = Convert.ToDecimal(dimensil.Text);
			if (dimensit.Text != "") n.DimensiT = Convert.ToDecimal(dimensit.Text);

			//Commit
			db.ProductOrder_Stone4.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected void save5(ProductOrder ms) {
		var d = db.Stone5.SingleOrDefault(p => p.ID == myIDStone && !p.Disable);
		if (d == null) LinkError();
		else {
			ProductOrder_Stone5 n = new ProductOrder_Stone5();

			n.IDForm = ms.ID;
			n.IDStone = d.ID;

            n.TotalButir = Convert.ToInt32(totalbutir.Text.Replace(".", ""));
			n.TotalCarat = Convert.ToDecimal(totalcarat.Text);
			n.CaratButir = n.TotalButir > 0 ? n.TotalCarat / n.TotalButir : 0;

			//Dimensi
			n.DimensiP = Convert.ToDecimal(dimensip.Text);
			if (dimensil.Text != "") n.DimensiL = Convert.ToDecimal(dimensil.Text);
			if (dimensit.Text != "") n.DimensiT = Convert.ToDecimal(dimensit.Text);

			//Commit
			db.ProductOrder_Stone5.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
}