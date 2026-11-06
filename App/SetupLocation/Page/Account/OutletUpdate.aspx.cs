using System;
using System.Linq;

public partial class Page_Account_OutletUpdate : System.Web.UI.Page {
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
			DLC.LocOutletGroup(group);
			DLC.CompanyBrand(brand);

			//Form
			fillForm();

			//Action
			save.Visible = Role.Operator;
			App.ConfirmDelete(del, Role.Manager);
		}
	}
	protected void fillForm() {
		var d = db.LocOutlet.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);

			//DLC-Data
			App.DropDown(group, d.LocOutletGroup.NamaKode, d.IDGroup.ToString());
			App.DropDown(brand, d.CompanyBrand.NamaKode, d.IDBrand.ToString());

			//Account
			nama.Text = d.Nama;
            kode.Text = d.Kode;
			keterangan.Text = d.Keterangan;
			tgl.Text = Str.Tgl(d.Tgl);
            sn.Text = d.SN.HasValue ? d.SN.Value.ToString() : "0"; 

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
    protected bool validKode
    {
        get
        {
            bool check = true;
            if (kode.Text.Length == 3)
                check = true;
            else
            {
                kode_ERR.Visible = check;
                check = false;
            }

            return check;
        }
    }
	protected void save_Click(object sender, EventArgs e) {
		if (valid && validKode) {
			var d = db.LocOutlet.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
				//Master
				d.IDGroup = Convert.ToInt32(group.SelectedValue);

				//Account
				d.Nama = Str.CapTitle(nama.Text);
                d.Kode = Str.CapUpper(kode.Text);
				d.Keterangan = Str.CapFirst(keterangan.Text);
				d.Tgl = Convert.ToDateTime(tgl.Text);
				d.Disable = disable.Checked;
                d.SN = Convert.ToInt32(sn.Text);

				//Commit
				db.SubmitChanges();

				//Redirect
				App.Reload(this, myID);
			}
		}
	}
	protected void del_Click(object sender, EventArgs e) {
		var d = db.LocOutlet.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			try {
				//Commit
				db.LocOutlet.DeleteOnSubmit(d);
				db.SubmitChanges();

				//Appendix
				AP.Delete("LocOutlet", myID);

				//Redirect
				App.ReloadClose(this);
			}
			catch { nodel.Visible = true; }
		}
	}
}