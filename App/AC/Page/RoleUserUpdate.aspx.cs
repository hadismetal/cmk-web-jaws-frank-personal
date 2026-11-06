using System;
using System.Linq;

public partial class Page_RoleUserUpdate : System.Web.UI.Page {
	private Map_AC _db;

	private Map_AC db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_AC();
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
	protected string myKode { get { return Request.QueryString["km"]; } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			fillHeader();
			fillForm();
		}
	}
	protected void fillHeader() {
		var d = db.ModuleIndex.SingleOrDefault(p => p.Kode == myKode);
		if (d == null) App.DataNull(this);
		else {
			header_group.InnerHtml = d.ModuleGroup.Nama;
			header_module.InnerHtml = d.Nama;
		}
	}
	protected void fillForm() {
		var d = db.UserRole.SingleOrDefault(p => p.ID == myID && p.Module == myKode);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_user.InnerHtml = d.UserAccount.Nama;

			//Control-Field
			role01.Checked = d.Manager;
			role02.Checked = d.Operator;
			role21.Checked = d.DownloadFile;
			role22.Checked = d.DownloadExcel;
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.UserRole.SingleOrDefault(p => p.ID == myID && p.Module == myKode);
		if (d == null) App.DataNull(this);
		else {
			//Control-Field
			d.Manager = role01.Checked;
			d.Operator = role02.Checked;
			d.DownloadFile = role21.Checked;
			d.DownloadExcel = role22.Checked;

			//Commit
			db.SubmitChanges();

			//Redirect
			App.ReloadClose(this);
		}
	}
}