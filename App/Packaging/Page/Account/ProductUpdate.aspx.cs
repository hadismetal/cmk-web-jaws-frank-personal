using System;
using System.Linq;

public partial class Page_Account_PackagingUpdate : System.Web.UI.Page {
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

            DLC.CompanyBrand(brand);

			//Form
			fillForm();
            
			//Action
			save.Visible = Role.Operator;
			App.ConfirmDelete(del, Role.Manager);
		}
	}
	protected void fillForm() {
		var d = db.Packaging.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Master(d.ID);

			//Account
			nama.Text = d.Nama;
			keterangan.Text = d.Keterangan;
            brand.SelectedValue = d.IDBrand.ToString();
			harga.Text = Db.SingleDecimal("SELECT ISNULL(Harga, 0) FROM Packaging WHERE ID = " + myID.ToString(), Db.Cnn_DB).ToString("N0");

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
		get { return true; }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.Packaging.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
				//Account
				d.Nama = Str.CapTitle(nama.Text);
				d.Keterangan = Str.CapFirst(keterangan.Text);
				d.Disable = disable.Checked;
                d.IDBrand = Convert.ToInt32(brand.SelectedValue);

				//Commit
				db.SubmitChanges();

				string Harga = string.IsNullOrEmpty(harga.Text) ? "0" : harga.Text.Replace(".", "");
                Db.Execute("INSERT INTO PackagingPriceLog SELECT ID, Kode, ISNULL(Harga, 0), " + Harga + ", '" + App.UserNama + "', GETDATE() FROM Packaging WHERE ID = " + d.ID.ToString(), Db.Cnn_DB);

                Db.Execute("UPDATE Packaging SET Harga = " + Harga + " WHERE ID = " + d.ID.ToString(), Db.Cnn_DB);
				//Redirect
				App.Reload(this, myID);
			}
		}
	}
	protected void del_Click(object sender, EventArgs e) {
		var d = db.Packaging.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			try {
				//Commit
				db.Packaging.DeleteOnSubmit(d);
				db.SubmitChanges();

				//Appendix
				AP.Delete("Product", myID);

				//Redirect
				App.ReloadClose(this);
			}
			catch { nodel.Visible = true; }
		}
	}
}