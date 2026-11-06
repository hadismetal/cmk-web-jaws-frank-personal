using System;
using System.Linq;

public partial class Page_RoleUserAddSave : System.Web.UI.Page {
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

		var d = db.UserRole.SingleOrDefault(p => p.ID == myID && p.Module == myKode);
		if (d == null) {
			UserRole n = new UserRole();

			n.ID = myID;
			n.Module = myKode;

			//Commit
			db.UserRole.InsertOnSubmit(n);
			db.SubmitChanges();
		}

		//Redirect
		Response.Redirect("RoleUserUpdate.aspx?id=" + myID + "&km=" + myKode);
	}
}