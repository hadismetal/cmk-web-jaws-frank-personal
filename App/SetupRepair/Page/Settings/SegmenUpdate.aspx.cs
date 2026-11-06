using System;
using System.Linq;

public partial class Page_Settings_SegmenUpdate : System.Web.UI.Page {
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
	protected int myIDCategory { get { return App.QueryInt(this, "idcategory"); } }
	protected int myIDLevel { get { return App.QueryInt(this, "idlevel"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			sistem.Focus();

			//DLC
			DLC.SegmenSistem(sistem);

			//Form
			fillHeader();
			fillForm();
		}
	}

	//Header
	protected void fillHeader() {
		headerCategory();
		headerLevel();
	}
	protected void headerCategory() {
		var d = db.CharProductCategory.SingleOrDefault(p => p.ID == myIDCategory && !p.Disable);
		if (d == null) App.DataNull(this);
		else
			header_category.InnerHtml = d.Nama + " (" + d.NamaKode + ")";
	}
	protected void headerLevel() {
		var d = db.CharProductLevel.SingleOrDefault(p => p.ID == myIDLevel && !p.Disable);
		if (d == null) App.DataNull(this);
		else
			header_level.InnerHtml = d.Nama + " (" + d.NamaKode + ")";
	}

	//Data-Form
	protected void fillForm() {
		var d = db.SettingsSegmen.SingleOrDefault(
			p =>
			p.IDCategory == myIDCategory &&
			p.IDLevel == myIDLevel
			);

		if (d == null)
			sistem.SelectedIndex = 0;
		else
			sistem.SelectedIndex = d.Sistem;
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.SettingsSegmen.SingleOrDefault(
			p =>
			p.IDCategory == myIDCategory &&
			p.IDLevel == myIDLevel
			);

		if (d == null) insertProcess();
		else {
			d.Sistem = sistem.SelectedIndex;

			//Commit
			db.SubmitChanges();
		}

		//Redirect
		App.ReloadClose(this);
	}
	protected void insertProcess() {
		SettingsSegmen n = new SettingsSegmen();

		n.IDCategory = myIDCategory;
		n.IDLevel = myIDLevel;
		n.Sistem = sistem.SelectedIndex;

		//Commit
		db.SettingsSegmen.InsertOnSubmit(n);
		db.SubmitChanges();
	}
}