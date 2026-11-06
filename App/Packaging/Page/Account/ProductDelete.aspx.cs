using System;
using System.Linq;

public partial class Page_Account_PackagingDelete : System.Web.UI.Page {
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
            var d = db.Packaging.SingleOrDefault(p => p.ID == myID && p.Draft == true);
            if (d != null)
            {
                db.Packaging.DeleteOnSubmit(d);
                db.SubmitChanges();
            }
            Response.Redirect("Packaging.aspx?d=1");
		}
	}
}