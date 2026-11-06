using System;
using System.Linq;

public partial class Page_RoleUserDelete : System.Web.UI.Page {
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
		if (d == null) App.DataNull(this);
		else {
			if (!(myID == App.UserID && myKode == "AC")) { //Administrator mencoba update dirinya sendiri
				//Commit
				db.UserRole.DeleteOnSubmit(d);
				db.SubmitChanges();
			}

			//Redirect
			Response.Redirect("RoleDetail.aspx?d=1&km=" + myKode);
		}
	}
}