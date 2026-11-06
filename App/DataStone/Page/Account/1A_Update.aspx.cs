using System;
using System.Linq;

public partial class Page_Account_1A_Update : System.Web.UI.Page {
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

			//Form
			fillForm();

			//Action
			save.Visible = Role.Operator;
			//App.ConfirmDelete(del, Role.Manager);
		}
	}
	protected void fillForm() {
		var d = db.Stone1A.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);

			//Account
			nama.Text = d.Nama;
			keterangan.Text = d.Keterangan;
			tgl.Text = Str.Tgl(d.Tgl);

			//Disable
			if (d.Draft) div_disable.Visible = false;
			else {
				div_draft.Visible = false;
				disable.Checked = d.Disable;
			}
		}
	}

	//Save
	protected bool valid_Nama {
		get {
			int count = db.Stone1A.Count(p => p.Nama == Str.CapUpper(nama.Text) && p.ID != myID);

			bool x = count == 0;
			nama_ERR.Visible = !x;

			return x;
		}
	}
	protected bool valid {
		get {
			bool x = true;

			x = x && valid_Nama;
			x = x && Fv.isDateTime(tgl, tgl_ERR);

			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.Stone1A.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
				d.Nama = Str.CapUpper(nama.Text);
				d.Keterangan = Str.CapFirst(keterangan.Text);
				d.Tgl = Convert.ToDateTime(tgl.Text);
				d.Disable = disable.Checked;
                d.Draft = false;

				//Commit
				db.SubmitChanges();

				//Redirect
				App.Reload(this, myID);
			}
		}
	}
	protected void del_Click(object sender, EventArgs e) {
		var d = db.Stone1A.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			try {
				//Commit
				db.Stone1A.DeleteOnSubmit(d);
				db.SubmitChanges();

				//Appendix
				AP.Delete("Stone1A", myID);

				//Redirect
				App.ReloadClose(this);
			}
			catch { nodel.Visible = true; }
		}
	}
}