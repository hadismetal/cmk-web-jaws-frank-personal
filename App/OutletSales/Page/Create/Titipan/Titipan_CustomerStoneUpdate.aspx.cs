using System;
using System.Linq;

public partial class Page_Register_Titipan_StoneUpdate : System.Web.UI.Page {
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
	protected int myTableID { get { return App.QueryInt(this, "tid"); } }
    protected int myTitipanID { get { return App.QueryInt(this, "idtitipan"); } }

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
	protected void DataError() { Response.Redirect("Titipan.aspx"); }
	protected void LinkError() { Response.Redirect("Titipan_CustomerStone.aspx?id=" + myID + "&idtitipan=" + myTitipanID); }
    protected void ProductError() { Response.Redirect("Titipan_CharProduct01.aspx?id=" + myID + "&idtitipan=" + myTitipanID); }

	//Form
	protected void fillForm() {
		var d = db.StockProductDJCustomer.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Titipan_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Cancel-Link
			cancel.HRef += "?id=" + myID;

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
		var d = db.StockProductDJCustomer_Stone1A.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
            //stone_nama.InnerHtml = d.Nama;
            stone_nama.InnerHtml = d.Stone1A.Parcel101.Nama + " " + d.Stone1A.Parcel102.Nama + " " + d.Stone1A.Parcel103.Nama + " " + d.Stone1A.Parcel104.Nama;

			//Size Control
			if (!d.Stone1A.Parcel102.SizeMin.HasValue || !d.Stone1A.Parcel102.SizeMax.HasValue) sizeError();
			else
				sizeDisplay(
					d.Stone1A.Parcel102.SizeMin,
					d.Stone1A.Parcel102.SizeMax
					);

			totalbutir.Text = Str.Integer(d.TotalButir);
			totalcarat.Text = Str.Number3(d.TotalCarat);

			//Dimensi
			dimensip.Text = Str.Number2(d.DimensiP);
			dimensil.Text = Str.Number2(d.DimensiL);
			dimensit.Text = Str.Number2(d.DimensiT);
		}
	}
	protected void fillStone1B() {
		var d = db.StockProductDJCustomer_Stone1B.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
            //stone_nama.InnerHtml = d.Nama;
            stone_nama.InnerHtml = d.Stone1B.Parcel101.Nama + " " + d.Stone1B.Parcel102.Nama + " " + d.Stone1B.Parcel103.Nama + " " + d.Stone1B.Parcel104.Nama
                + " " + d.Stone1B.Parcel105.Nama + " " + d.Stone1B.Parcel106.Nama + " " + d.Stone1B.Parcel107.Nama;

			//Size Control
			if (!d.Stone1B.Parcel102.SizeMin.HasValue || !d.Stone1B.Parcel102.SizeMax.HasValue) sizeError();
			else
				sizeDisplay(
					d.Stone1B.Parcel102.SizeMin,
					d.Stone1B.Parcel102.SizeMax
					);

			totalbutir.Text = Str.Integer(d.TotalButir);
			totalcarat.Text = Str.Number3(d.TotalCarat);

			//Dimensi
			dimensip.Text = Str.Number2(d.DimensiP);
			dimensil.Text = Str.Number2(d.DimensiL);
			dimensit.Text = Str.Number2(d.DimensiT);

			//GIA
			div_gia.Visible = true;
			gia.Text = d.GIA;
		}
	}
	protected void fillStone2() {
		var d = db.StockProductDJCustomer_Stone2.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
            //stone_nama.InnerHtml = d.Nama;
            stone_nama.InnerHtml = d.Stone2.Parcel201.Nama + " " + d.Stone2.Parcel202.Nama + " " + d.Stone2.Parcel203.Nama + " " + d.Stone2.Parcel204.Nama;

			//Size Control
			if (!d.Stone2.Parcel203.SizeMin.HasValue || !d.Stone2.Parcel203.SizeMax.HasValue) sizeError();
			else
				sizeDisplay(
					d.Stone2.Parcel203.SizeMin,
					d.Stone2.Parcel203.SizeMax
					);

			totalbutir.Text = Str.Integer(d.TotalButir);
			totalcarat.Text = Str.Number3(d.TotalCarat);

			//Dimensi
			dimensip.Text = Str.Number2(d.DimensiP);
			dimensil.Text = Str.Number2(d.DimensiL);
			dimensit.Text = Str.Number2(d.DimensiT);
		}
	}
	protected void fillStone3() {
		var d = db.StockProductDJCustomer_Stone3.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
            //stone_nama.InnerHtml = d.Nama;
            stone_nama.InnerHtml = d.Stone3.Parcel301.Nama + " " + d.Stone3.Parcel302.Nama + " " + d.Stone3.Parcel303.Nama + " " + d.Stone3.Parcel304.Nama;

			//Size Control
			if (!d.Stone3.Parcel303.SizeMin.HasValue || !d.Stone3.Parcel303.SizeMax.HasValue) sizeError();
			else
				sizeDisplay(
					d.Stone3.Parcel303.SizeMin,
					d.Stone3.Parcel303.SizeMax
					);

			totalbutir.Text = Str.Integer(d.TotalButir);
			totalcarat.Text = Str.Number3(d.TotalCarat);

			//Dimensi
			dimensip.Text = Str.Number2(d.DimensiP);
			dimensil.Text = Str.Number2(d.DimensiL);
			dimensit.Text = Str.Number2(d.DimensiT);
		}
	}
	protected void fillStone4() {
		var d = db.StockProductDJCustomer_Stone4.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
            //stone_nama.InnerHtml = d.Nama;
            stone_nama.InnerHtml = d.Stone4.Parcel401.Nama + " " + d.Stone4.Parcel402.Nama + " " + d.Stone4.Parcel403.Nama;

			//Size Control
			if (!d.Stone4.Parcel403.SizeMin.HasValue || !d.Stone4.Parcel403.SizeMax.HasValue) sizeError();
			else
				sizeDisplay(
					d.Stone4.Parcel403.SizeMin,
					d.Stone4.Parcel403.SizeMax
					);

			totalbutir.Text = Str.Integer(d.TotalButir);
			totalcarat.Text = Str.Number3(d.TotalCarat);

			//Dimensi
			dimensip.Text = Str.Number2(d.DimensiP);
			dimensil.Text = Str.Number2(d.DimensiL);
			dimensit.Text = Str.Number2(d.DimensiT);
		}
	}
	protected void fillStone5() {
		var d = db.StockProductDJCustomer_Stone5.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
            //stone_nama.InnerHtml = d.Nama;
            stone_nama.InnerHtml = d.Stone5.Parcel501.Nama + " " + d.Stone5.Parcel502.Nama + " " + d.Stone5.Parcel503.Nama + " " + d.Stone5.Parcel504.Nama
                + " " + d.Stone5.Parcel505.Nama + " " + d.Stone5.Parcel506.Nama + " " + d.Stone5.Parcel507.Nama;

			//Size Control
			p_sizecontrol.Visible = false;

			totalbutir.Text = Str.Integer(d.TotalButir);
			totalcarat.Text = Str.Number3(d.TotalCarat);

			//Dimensi
			dimensip.Text = Str.Number2(d.DimensiP);
			dimensil.Text = Str.Number2(d.DimensiL);
			dimensit.Text = Str.Number2(d.DimensiT);
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
		var d = db.StockProductDJCustomer_Stone1A.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) { LinkError(); return false; }
		else {
			decimal Min = d.Stone1A.Parcel102.SizeMin.Value;
			decimal Max = d.Stone1A.Parcel102.SizeMax.Value;

			decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
			return (CB >= Min && CB <= Max);
		}
	}
	protected bool carat1B(int TotalButir, decimal TotalCarat) {
		var d = db.StockProductDJCustomer_Stone1B.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) { LinkError(); return false; }
		else {
			decimal Min = d.Stone1B.Parcel102.SizeMin.Value;
			decimal Max = d.Stone1B.Parcel102.SizeMax.Value;

			decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
			return (CB >= Min && CB <= Max);
		}
	}
	protected bool carat2(int TotalButir, decimal TotalCarat) {
		var d = db.StockProductDJCustomer_Stone2.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) { LinkError(); return false; }
		else {
			decimal Min = d.Stone2.Parcel203.SizeMin.Value;
			decimal Max = d.Stone2.Parcel203.SizeMax.Value;

			decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
			return (CB >= Min && CB <= Max);
		}
	}
	protected bool carat3(int TotalButir, decimal TotalCarat) {
		var d = db.StockProductDJCustomer_Stone3.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) { LinkError(); return false; }
		else {
			decimal Min = d.Stone3.Parcel303.SizeMin.Value;
			decimal Max = d.Stone3.Parcel303.SizeMax.Value;

			decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
			return (CB >= Min && CB <= Max);
		}
	}
	protected bool carat4(int TotalButir, decimal TotalCarat) {
		var d = db.StockProductDJCustomer_Stone4.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) { LinkError(); return false; }
		else {
			decimal Min = d.Stone4.Parcel403.SizeMin.Value;
			decimal Max = d.Stone4.Parcel403.SizeMax.Value;

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
			var d = db.StockProductDJCustomer.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
			if (d == null) DataError();
			else {
				//Process
				var product = d.StockProductDJCustomer_CharProduct;
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
					//Lib.RecalWeight(db, d, product);
				}

				//Redirect
                Response.Redirect("Titipan_CustomerStone.aspx?id=" + myID + "&idtitipan=" + myTitipanID);
			}
		}
	}

	//Process
	protected void save1A(StockProductDJCustomer ms) {
		var d = db.StockProductDJCustomer_Stone1A.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
			d.TotalButir = Convert.ToInt32(totalbutir.Text);
			d.TotalCarat = Convert.ToDecimal(totalcarat.Text);
			d.CaratButir = d.TotalButir > 0 ? d.TotalCarat / d.TotalButir : 0;

			//Dimensi
			d.DimensiP = Convert.ToDecimal(dimensip.Text);
			if (dimensil.Text != "") d.DimensiL = Convert.ToDecimal(dimensil.Text); else d.DimensiL = null;
			if (dimensit.Text != "") d.DimensiT = Convert.ToDecimal(dimensit.Text); else d.DimensiT = null;

			//Commit
			db.SubmitChanges();
		}
	}
	protected void save1B(StockProductDJCustomer ms) {
		var d = db.StockProductDJCustomer_Stone1B.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
			d.TotalButir = Convert.ToInt32(totalbutir.Text);
			d.TotalCarat = Convert.ToDecimal(totalcarat.Text);
			d.CaratButir = d.TotalButir > 0 ? d.TotalCarat / d.TotalButir : 0;

			//Dimensi
			d.DimensiP = Convert.ToDecimal(dimensip.Text);
			if (dimensil.Text != "") d.DimensiL = Convert.ToDecimal(dimensil.Text); else d.DimensiL = null;
			if (dimensit.Text != "") d.DimensiT = Convert.ToDecimal(dimensit.Text); else d.DimensiT = null;

			//GIA
			d.GIA = Str.CapUpper(gia.Text);

			//Commit
			db.SubmitChanges();
		}
	}
	protected void save2(StockProductDJCustomer ms) {
		var d = db.StockProductDJCustomer_Stone2.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
			d.TotalButir = Convert.ToInt32(totalbutir.Text);
			d.TotalCarat = Convert.ToDecimal(totalcarat.Text);
			d.CaratButir = d.TotalButir > 0 ? d.TotalCarat / d.TotalButir : 0;

			//Dimensi
			d.DimensiP = Convert.ToDecimal(dimensip.Text);
			if (dimensil.Text != "") d.DimensiL = Convert.ToDecimal(dimensil.Text); else d.DimensiL = null;
			if (dimensit.Text != "") d.DimensiT = Convert.ToDecimal(dimensit.Text); else d.DimensiT = null;

			//Commit
			db.SubmitChanges();
		}
	}
	protected void save3(StockProductDJCustomer ms) {
		var d = db.StockProductDJCustomer_Stone3.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
			d.TotalButir = Convert.ToInt32(totalbutir.Text);
			d.TotalCarat = Convert.ToDecimal(totalcarat.Text);
			d.CaratButir = d.TotalButir > 0 ? d.TotalCarat / d.TotalButir : 0;

			//Dimensi
			d.DimensiP = Convert.ToDecimal(dimensip.Text);
			if (dimensil.Text != "") d.DimensiL = Convert.ToDecimal(dimensil.Text); else d.DimensiL = null;
			if (dimensit.Text != "") d.DimensiT = Convert.ToDecimal(dimensit.Text); else d.DimensiT = null;

			//Commit
			db.SubmitChanges();
		}
	}
	protected void save4(StockProductDJCustomer ms) {
		var d = db.StockProductDJCustomer_Stone4.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
			d.TotalButir = Convert.ToInt32(totalbutir.Text);
			d.TotalCarat = Convert.ToDecimal(totalcarat.Text);
			d.CaratButir = d.TotalButir > 0 ? d.TotalCarat / d.TotalButir : 0;

			//Dimensi
			d.DimensiP = Convert.ToDecimal(dimensip.Text);
			if (dimensil.Text != "") d.DimensiL = Convert.ToDecimal(dimensil.Text); else d.DimensiL = null;
			if (dimensit.Text != "") d.DimensiT = Convert.ToDecimal(dimensit.Text); else d.DimensiT = null;

			//Commit
			db.SubmitChanges();
		}
	}
	protected void save5(StockProductDJCustomer ms) {
		var d = db.StockProductDJCustomer_Stone5.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
			d.TotalButir = Convert.ToInt32(totalbutir.Text);
			d.TotalCarat = Convert.ToDecimal(totalcarat.Text);
			d.CaratButir = d.TotalButir > 0 ? d.TotalCarat / d.TotalButir : 0;

			//Dimensi
			d.DimensiP = Convert.ToDecimal(dimensip.Text);
			if (dimensil.Text != "") d.DimensiL = Convert.ToDecimal(dimensil.Text); else d.DimensiL = null;
			if (dimensit.Text != "") d.DimensiT = Convert.ToDecimal(dimensit.Text); else d.DimensiT = null;

			//Commit
			db.SubmitChanges();
		}
	}
}