using System;
using System.Linq;

public partial class Page_Account_StockKeeperUpdate : System.Web.UI.Page {
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
			App.ConfirmDelete(del, Role.Manager);
		}
	}
	protected void fillForm() {
        DLC.User(username);
        
		var d = db.DataSK.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
            if (d.IDUser != null)
                username.SelectedValue = d.IDUser.ToString();

            //DLC
            DLC.LocationList(cabang, d.TipeLokasi.Value);

            string Lokasi = "";
            string Brand = "";

            if (d.TipeLokasi == 1)
            {
                tipelokasi.Text = "Outlet";
                var x = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
                Lokasi = x.Nama;
                Brand = Db.SingleString("SELECT Nama FROM CompanyBrand WHERE ID=" + x.IDBrand, Db.Cnn_DB);
            }
            else
            {
                tipelokasi.Text = "Exhibition";
                var x = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                Lokasi = x.Nama;
                Brand = Db.SingleString("SELECT Nama FROM CompanyBrand WHERE ID=" + x.IDBrand, Db.Cnn_DB);
            }

            brand.Text = Brand;
            lokasi.Text = Lokasi;

			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);

			//DLC-Data
            tipe.SelectedValue = d.TipeLokasi.ToString();
            if(d.TipeLokasi.ToString() == tipe.SelectedValue)
                cabang.SelectedValue = d.IDLokasi.ToString();

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
			var d = db.DataSK.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
				//Master
                d.IDUser = Convert.ToInt32(username.SelectedValue);

				//Account
				d.Nama = Str.CapTitle(nama.Text);
                d.TipeLokasi = Convert.ToInt32(tipe.SelectedValue);
                d.IDLokasi = Convert.ToInt32(cabang.SelectedValue);
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
		var d = db.DataSK.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			try {
				//Commit
				db.DataSK.DeleteOnSubmit(d);
				db.SubmitChanges();

				//Appendix
				AP.Delete("DataSK", myID);

				//Redirect
				App.ReloadClose(this);
			}
			catch { nodel.Visible = true; }
		}
	}
    protected void FillLokasi(object sender, EventArgs e)
    {
        cabang.Items.Clear();
        DLC.LocationList(cabang, Convert.ToInt32(tipe.SelectedValue));
    }
}