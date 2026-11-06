using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Settings_StoneDistUpdate : System.Web.UI.Page {
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
			cb.Focus();

			//DLC
			DLC.CharStoneDist(cb);

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
		var rs = from p in db.SettingsStoneDist
				 where
					p.IDCategory == myIDCategory &&
					p.IDLevel == myIDLevel
				 orderby p.CharStoneDist.SN
				 select p;

		foreach (var r in rs) {
			ListItem li = cb.Items.FindByValue(r.IDDist.ToString());
			if (li != null) li.Selected = true;
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var rs = from p in cb.Items.Cast<ListItem>()
				 select p;

		foreach (var r in rs) {
			int myIDDist = Convert.ToInt32(r.Value);
			if (r.Selected)
				insertProcess(myIDDist);
			else
				deleteProcess(myIDDist);
		}

		//Redirect
		App.ReloadClose(this);
	}
	protected void insertProcess(int myIDDist) {
		var d = db.SettingsStoneDist.SingleOrDefault(p =>
				p.IDCategory == myIDCategory &&
				p.IDLevel == myIDLevel &&
				p.IDDist == myIDDist
				);

		if (d == null) {
			SettingsStoneDist n = new SettingsStoneDist();

			n.IDCategory = myIDCategory;
			n.IDLevel = myIDLevel;
			n.IDDist = myIDDist;

			//Commit
			db.SettingsStoneDist.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
	protected void deleteProcess(int myIDDist) {
		var d = db.SettingsStoneDist.SingleOrDefault(p =>
				p.IDCategory == myIDCategory &&
				p.IDLevel == myIDLevel &&
				p.IDDist == myIDDist
				);

		if (d != null) {
			//Commit
			db.SettingsStoneDist.DeleteOnSubmit(d);
			db.SubmitChanges();
		}
	}
}