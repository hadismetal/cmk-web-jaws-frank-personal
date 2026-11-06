using System;
using System.Linq;

public partial class Page_Create_1A_Account : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("1A.aspx"); }

	//Parcel
	protected string KodeParcel() {
		string x = string.Empty;

		var d1 = db.Parcel101.SingleOrDefault(p => p.ID == myParcel01 && !p.Disable);
		if (d1 == null) DataError();
		else x += d1.NamaKode;

		var d2 = db.Parcel102.SingleOrDefault(p => p.ID == myParcel02 && !p.Disable && p.Rapaport.HasValue && p.Rapaport == false);
		if (d2 == null) DataError();
		else x += d2.NamaKode;

		var d3 = db.Parcel103.SingleOrDefault(p => p.ID == myParcel03 && !p.Disable);
		if (d3 == null) DataError();
		else x += d3.NamaKode;

		var d4 = db.Parcel104.SingleOrDefault(p => p.ID == myParcel04 && !p.Disable);
		if (d4 == null) DataError();
		else x += d4.NamaKode;

		return x;
	}

	//Save
	protected bool valid_Nama {
		get {
			int count = db.Stone1A.Count(p => p.Nama == Str.CapUpper(nama.Text));

			bool x = count == 0;
			nama_ERR.Visible = !x;

			return x;
		}
	}
	protected bool valid_KodeParcel {
		get {
			int count = db.Stone1A.Count(p =>
				p.Parcel01 == myParcel01 &&
				p.Parcel02 == myParcel02 &&
				p.Parcel03 == myParcel03 &&
				p.Parcel04 == myParcel04
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
			Stone1A n = new Stone1A();

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

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;
			n.Disable = true;
			n.StatusCosting = 1;

			//Commit
			db.Stone1A.InsertOnSubmit(n);
			db.SubmitChanges();

			//Extention Table
			save_Costing(n.ID);
			save_Pricing(n.ID);

			//Redirect
			Response.Redirect("1A_Submit.aspx?id=" + n.ID);
		}
	}
	protected void save_Costing(int myID) {
		StoneCosting1A n = new StoneCosting1A();

		n.ID = myID;

		//Commit
		db.StoneCosting1A.InsertOnSubmit(n);
		db.SubmitChanges();
	}
	protected void save_Pricing(int myID) {
		StonePricing1A n = new StonePricing1A();

		n.ID = myID;

		//Commit
		db.StonePricing1A.InsertOnSubmit(n);
		db.SubmitChanges();
	}
}