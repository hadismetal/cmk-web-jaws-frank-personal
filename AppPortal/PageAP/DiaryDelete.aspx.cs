using System;
using System.Linq;

public partial class AP_DiaryDelete : System.Web.UI.Page {
	private Map_AP _db;

	private Map_AP db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_AP();
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

		var d = db.Diary.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Role Protection
			App.ProtectRole(RoleAP.Delete(d.Module));

			//Commit
			db.Diary.DeleteOnSubmit(d);
			db.SubmitChanges();

			//Redirect
			string href = string.Empty;

			href += "Diary.aspx";
			href += "?module=" + d.Module;
			href += "&datatable=" + d.DataTable;
			href += "&dataid=" + d.DataID;
			href += "&d=1";

			Response.Redirect(href);
		}
	}
}