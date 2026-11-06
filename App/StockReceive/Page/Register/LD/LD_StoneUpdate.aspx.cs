using System;
using System.Linq;

public partial class Page_Register_LD_StoneUpdate : System.Web.UI.Page {
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

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			totalcarat.Focus();

			//Number-Format
			App.NumberFormat(totalcarat);
			App.NumberFormat(dimensip);
			App.NumberFormat(dimensil);
			App.NumberFormat(dimensit);

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("LD.aspx"); }
	protected void LinkError() { Response.Redirect("LD_Stone.aspx?id=" + myID); }
	protected void ProductError() { Response.Redirect("LD_CharProduct01.aspx?id=" + myID); }

	//Form
	protected void fillForm() {
		var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/LD_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Cancel-Link
			cancel.HRef += "?id=" + myID;

			//Stone Data
			stone_index.InnerHtml = DL.StoneIndex(myStoneIndex);
			fillStone1B();
		}
	}

	//Stone Data
	protected void fillStone1B() {
		var d = db.StockReceiveLD_Stone1B.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
			stone_nama.InnerHtml = d.Stone1B.Nama;

			//Size Control
			if (!d.Stone1B.Parcel102.SizeMin.HasValue || !d.Stone1B.Parcel102.SizeMax.HasValue) sizeError();
			else
				sizeDisplay(
					d.Stone1B.Parcel102.SizeMin,
					d.Stone1B.Parcel102.SizeMax
					);

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
	protected bool carat1B(int TotalButir, decimal TotalCarat) {
		var d = db.StockReceiveLD_Stone1B.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) { LinkError(); return false; }
		else {
			decimal Min = d.Stone1B.Parcel102.SizeMin.Value;
			decimal Max = d.Stone1B.Parcel102.SizeMax.Value;

			decimal CB = TotalButir > 0 ? TotalCarat / TotalButir : 0;
			return (CB >= Min && CB <= Max);
		}
	}

	//Save
	protected bool valid {
		get {
			bool x = true;

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
			int dv = 1;

			bool x = dv > 0;

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

			int TotalButir = 1;
			decimal TotalCarat = Convert.ToDecimal(totalcarat.Text);

			x = carat1B(TotalButir, TotalCarat);
			caratbutir_ERR.Visible = !x;
			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.Draft);
			if (d == null) DataError();
			else {
				//Process
				save1B(d);

				//Redirect
				Response.Redirect("LD_Stone.aspx?id=" + myID);
			}
		}
	}

	protected void save1B(StockReceiveLD ms) {
		var d = db.StockReceiveLD_Stone1B.SingleOrDefault(p => p.IDForm == myID && p.ID == myTableID);
		if (d == null) LinkError();
		else {
			d.TotalButir = 1;
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
}