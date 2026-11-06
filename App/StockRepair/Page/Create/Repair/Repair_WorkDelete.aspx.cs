using System;
using System.Linq;

public partial class Page_Register_Repair_ItemDelete : System.Web.UI.Page {
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
	protected int myIID { get { return App.QueryInt(this, "iid"); } }

	//Exception
	protected void DataError() { Response.Redirect("Repair.aspx"); }
	protected void LinkError() { Response.Redirect("Repair_Data.aspx?id=" + myID); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			//Process
			
				delete();

			//Redirect
			Response.Redirect("Repair_Data.aspx?id=" + myID);
		}
	}
	protected void delete() {
		var d = db.DocRepair_Repair.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID);
		if (d == null) LinkError();
		else {
           
			//Commit
			db.DocRepair_Repair.DeleteOnSubmit(d);
			db.SubmitChanges();
		}
	}
}