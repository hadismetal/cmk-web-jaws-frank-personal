using System;
using System.Linq;

public partial class Page_Create_4_Account : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("4.aspx"); }

	//Parcel
	protected string KodeParcel() {
		string x = string.Empty;

		var d1 = db.Parcel401.SingleOrDefault(p => p.ID == myParcel01 && !p.Disable);
		if (d1 == null) DataError();
		else x += d1.NamaKode;

		var d2 = db.Parcel402.SingleOrDefault(p => p.ID == myParcel02 && !p.Disable);
		if (d2 == null) DataError();
		else x += d2.NamaKode;

		var d3 = db.Parcel403.SingleOrDefault(p => p.ID == myParcel03 && !p.Disable);
		if (d3 == null) DataError();
		else x += d3.NamaKode;

		return x;
	}

	//Save
	protected bool valid_Nama {
		get {
			int count = db.Stone4.Count(p => p.Nama == Str.CapUpper(nama.Text));

			bool x = count == 0;
			nama_ERR.Visible = !x;

			return x;
		}
	}
	protected bool valid_KodeParcel {
		get {
			int count = db.Stone4.Count(p =>
				p.Parcel01 == myParcel01 &&
				p.Parcel02 == myParcel02 &&
				p.Parcel03 == myParcel03
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
			Stone4 n = new Stone4();

			//Account
			n.Nama = Str.CapUpper(nama.Text);
			n.Keterangan = Str.CapFirst(keterangan.Text);
			n.Tgl = Convert.ToDateTime(tgl.Text);

			//Parcel
			n.KodeParcel = kodeparcel.Text;
			n.Parcel01 = myParcel01;
			n.Parcel02 = myParcel02;
			n.Parcel03 = myParcel03;

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;
			n.Disable = true;
			n.StatusCosting = 1;

			//Commit
			db.Stone4.InsertOnSubmit(n);
			db.SubmitChanges();

			//Extention Table
			save_Costing(n.ID);
			save_Pricing(n.ID);

			//Redirect
			Response.Redirect("4_Submit.aspx?id=" + n.ID);
		}
	}
	protected void save_Costing(int myID) {
		StoneCosting4 n = new StoneCosting4();

		n.ID = myID;

		//Commit
		db.StoneCosting4.InsertOnSubmit(n);
		db.SubmitChanges();
	}
	protected void save_Pricing(int myID) {
		StonePricing4 n = new StonePricing4();

		n.ID = myID;

		//Commit
		db.StonePricing4.InsertOnSubmit(n);
		db.SubmitChanges();
	}
}