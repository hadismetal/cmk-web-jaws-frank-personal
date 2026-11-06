using System;
using System.Linq;

public partial class Page_Account_WarehouseUpdate : System.Web.UI.Page {
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

			//DLC
			DLC.LocWarehouseGroup(group);
			DLC.CompanyBrand(brand);

			//Form
			fillForm();

			//Action
			save.Visible = Role.Operator;
			App.ConfirmDelete(del, Role.Manager);
		}
	}
	protected void fillForm() {
		var d = db.LocWarehouse.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);

			//DLC-Data
			App.DropDown(group, d.LocWarehouseGroup.NamaKode, d.IDGroup.ToString());
			App.DropDown(brand, d.CompanyBrand.NamaKode, d.IDBrand.ToString());

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
	protected bool valid {
		get { return Fv.isDateTime(tgl, tgl_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.LocWarehouse.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
				//Master
				d.IDGroup = Convert.ToInt32(group.SelectedValue);

				//Account
				d.Nama = Str.CapTitle(nama.Text);
				d.Keterangan = Str.CapFirst(keterangan.Text);
				d.Tgl = Convert.ToDateTime(tgl.Text);
				d.Disable = disable.Checked;

				//Commit
				db.SubmitChanges();

				//Redirect
				App.Reload(this, myID);
			}
		}
	}
	protected void del_Click(object sender, EventArgs e) {
		var d = db.LocWarehouse.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			try {
				//Commit
				db.LocWarehouse.DeleteOnSubmit(d);
				db.SubmitChanges();

				//Appendix
				AP.Delete("LocWarehouse", myID);

				//Redirect
				App.ReloadClose(this);
			}
			catch { nodel.Visible = true; }
		}
	}
}