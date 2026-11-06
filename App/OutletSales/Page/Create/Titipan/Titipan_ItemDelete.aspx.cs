using System;
using System.Linq;

public partial class Page_Register_Titipan_ItemDelete : System.Web.UI.Page {
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
	protected int myItemIndex { get { return App.QueryInt(this, "itemindex"); } }
	protected int myIID { get { return App.QueryInt(this, "iid"); } }

	//Exception
	protected void DataError() { Response.Redirect("Titipan.aspx"); }
	protected void LinkError() { Response.Redirect("Titipan_Data.aspx?id=" + myID); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Process
			
				saveDJ();
				
			}

			//Redirect
			Response.Redirect("Titipan_Data.aspx?id=" + myID);
	}
	protected void saveDJ() {
		var d = db.DocTitipan_Product.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID);
		if (d == null) LinkError();
		else {
            
			//Commit
			db.DocTitipan_Product.DeleteOnSubmit(d);
			db.SubmitChanges();
		}
	}
}