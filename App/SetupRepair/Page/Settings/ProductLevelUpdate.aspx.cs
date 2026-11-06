using System;
using System.Linq;

public partial class Page_Settings_ProductLevelUpdate : System.Web.UI.Page {
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
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			grup.Focus();

			//Form
			fillForm();
		}
	}
	protected void fillForm() {
		var d = db.CharProductLevel.SingleOrDefault(p => p.ID == myID && !p.Disable);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_nama.InnerHtml = d.Nama + " (" + d.NamaKode + ")";

			if (!d.Grup.HasValue)
				grup.SelectedIndex = 0;
			else
				grup.SelectedIndex = d.Grup.Value;
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.CharProductLevel.SingleOrDefault(p => p.ID == myID && !p.Disable);
		if (d == null) App.DataNull(this);
		else {
			d.Grup = grup.SelectedIndex;

			//Commit
			db.SubmitChanges();

			//Redirect
			App.ReloadClose(this);
		}
	}
}