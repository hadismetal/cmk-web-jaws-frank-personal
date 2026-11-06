using System;
using System.Linq;

public partial class Page_Create_5_Account : System.Web.UI.Page {
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
	protected int myParcel01 { get { return App.QueryInt(this, "parcel01"); } }
	protected int myParcel02 { get { return App.QueryInt(this, "parcel02"); } }
	protected int myParcel03 { get { return App.QueryInt(this, "parcel03"); } }
	protected int myParcel04 { get { return App.QueryInt(this, "parcel04"); } }
	protected int myParcel05 { get { return App.QueryInt(this, "parcel05"); } }
	protected int myParcel06 { get { return App.QueryInt(this, "parcel06"); } }
	protected int myParcel07 { get { return App.QueryInt(this, "parcel07"); } }
	protected int myParcel08 { get { return App.QueryInt(this, "parcel08"); } }
	protected int myParcel09 { get { return App.QueryInt(this, "parcel09"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			nama.Focus();

			//Form-Default
			tgl.Text = Str.Tgl(DateTime.Today);
			nama.Text = kodeparcel.Text = KodeParcel();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("5.aspx"); }

	//Parcel
	protected string KodeParcel() {
		string x = string.Empty;

		var d1 = db.Parcel501.SingleOrDefault(p => p.ID == myParcel01 && !p.Disable);
		if (d1 == null) DataError();
		else x += d1.NamaKode;

		var d2 = db.Parcel502.SingleOrDefault(p => p.ID == myParcel02 && !p.Disable);
		if (d2 == null) DataError();
		else x += d2.NamaKode;

		var d3 = db.Parcel503.SingleOrDefault(p => p.ID == myParcel03 && !p.Disable);
		if (d3 == null) DataError();
		else x += d3.NamaKode;

		var d4 = db.Parcel504.SingleOrDefault(p => p.ID == myParcel04 && !p.Disable);
		if (d4 == null) DataError();
		else x += d4.NamaKode;

		var d5 = db.Parcel505.SingleOrDefault(p => p.ID == myParcel05 && !p.Disable);
		if (d5 == null) DataError();
		else x += d5.NamaKode;

		var d6 = db.Parcel506.SingleOrDefault(p => p.ID == myParcel06 && !p.Disable);
		if (d6 == null) DataError();
		else x += d6.NamaKode;

		var d7 = db.Parcel507.SingleOrDefault(p => p.ID == myParcel07 && !p.Disable);
		if (d7 == null) DataError();
		else x += d7.NamaKode;

		var d8 = db.Parcel508.SingleOrDefault(p => p.ID == myParcel08 && !p.Disable);
		if (d8 == null) DataError();
		else x += d8.NamaKode;

		var d9 = db.Parcel509.SingleOrDefault(p => p.ID == myParcel09 && !p.Disable);
		if (d9 == null) DataError();
		else x += d9.NamaKode;

		return x;
	}

	//Save
	protected bool valid_Nama {
		get {
			int count = db.Stone5.Count(p => p.Nama == Str.CapUpper(nama.Text));

			bool x = count == 0;
			nama_ERR.Visible = !x;

			return x;
		}
	}
	protected bool valid_KodeParcel {
		get {
			int count = db.Stone5.Count(p =>
				p.Parcel01 == myParcel01 &&
				p.Parcel02 == myParcel02 &&
				p.Parcel03 == myParcel03 &&
				p.Parcel04 == myParcel04 &&
				p.Parcel05 == myParcel05 &&
				p.Parcel06 == myParcel06 &&
				p.Parcel07 == myParcel07 &&
				p.Parcel08 == myParcel08 &&
				p.Parcel09 == myParcel09
				);

			bool x = count == 0;
			kodeparcel_ERR.Visible = !x;

			return x;
		}
	}
	protected bool valid {
		get {
			bool x = true;

			x = x && valid_Nama;
			x = x && valid_KodeParcel;
			x = x && Fv.isDateTime(tgl, tgl_ERR);

			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			Stone5 n = new Stone5();

			//Account
			n.Nama = Str.CapUpper(nama.Text);
			n.Keterangan = Str.CapFirst(keterangan.Text);
			n.Tgl = Convert.ToDateTime(tgl.Text);

			//Parcel
			n.KodeParcel = kodeparcel.Text;
			n.Parcel01 = myParcel01;
			n.Parcel02 = myParcel02;
			n.Parcel03 = myParcel03;
			n.Parcel04 = myParcel04;
			n.Parcel05 = myParcel05;
			n.Parcel06 = myParcel06;
			n.Parcel07 = myParcel07;
			n.Parcel08 = myParcel08;
			n.Parcel09 = myParcel09;

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;
			n.Disable = true;
			n.StatusCosting = 1;

			//Commit
			db.Stone5.InsertOnSubmit(n);
			db.SubmitChanges();

			//Extention Table
			save_Costing(n.ID);
			save_Pricing(n.ID);

			//Redirect
			Response.Redirect("5_Submit.aspx?id=" + n.ID);
		}
	}
	protected void save_Costing(int myID) {
		StoneCosting5 n = new StoneCosting5();

		n.ID = myID;

		//Commit
		db.StoneCosting5.InsertOnSubmit(n);
		db.SubmitChanges();
	}
	protected void save_Pricing(int myID) {
		StonePricing5 n = new StonePricing5();

		n.ID = myID;

		//Commit
		db.StonePricing5.InsertOnSubmit(n);
		db.SubmitChanges();
	}
}