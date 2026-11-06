using System;
using System.Linq;

public partial class Page_Parcel4_01Update : System.Web.UI.Page {
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

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			nama.Focus();

			//Number-Format
			App.NumberFormat(sn);

			//Form
			fillForm();

			//Action
			save.Visible = Role.Operator;
			App.ConfirmDelete(del, Role.Manager);
			AP.Menu(ap, "Parcel401", myID, Role.Manager);
		}
	}
	protected void fillForm() {
		var d = db.Parcel401.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama + " (" + d.NamaKode + ")";
			h1_id.Text = Str.ID_Master(d.ID);

			nama.Text = d.Nama;
			namakode.Text = d.NamaKode;
			keterangan.Text = d.Keterangan;
			sn.Text = d.SN.ToString();

			//Disable
			disable.Checked = d.Disable;
		}
	}

	//Save
	protected bool valid {
		get { return Fv.isInt(sn, sn_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.Parcel401.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
				d.Nama = Str.CapTitle(nama.Text);
				d.NamaKode = Str.CapUpper(namakode.Text);
				d.Keterangan = Str.CapFirst(keterangan.Text);
				d.SN = Convert.ToInt32(sn.Text);
				d.Disable = disable.Checked;

				//Commit
				db.SubmitChanges();

				//Redirect
				App.Reload(this, myID);
			}
		}
	}
	protected void del_Click(object sender, EventArgs e) {
		var d = db.Parcel401.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			try {
				//Commit
				db.Parcel401.DeleteOnSubmit(d);
				db.SubmitChanges();

				//Appendix
				AP.Delete("Parcel401", myID);

				//Redirect
				App.ReloadClose(this);
			}
			catch { nodel.Visible = true; }
		}
	}
}