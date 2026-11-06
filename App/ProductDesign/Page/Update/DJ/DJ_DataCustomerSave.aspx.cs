using System;
using System.Linq;

public partial class Page_Register_Design_DataDesignerSave : System.Web.UI.Page {
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
	protected int myLink { get { return App.QueryInt(this, "link"); } }

	//Exception
	protected void DataError() { Response.Redirect("DJ.aspx"); }
	protected void LinkError() { Response.Redirect("DJ_DataCustomer.aspx?id=" + myID); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			saveLink(d);

			//Redirect
			Response.Redirect("DJ_Data01.aspx?id=" + myID + "&status=1");
		}
	}
	protected void saveLink(ProductDesign data) {

		var d = db.DataCustomer.SingleOrDefault(
			p =>
			p.ID == myLink && !p.Disable
			);

		if (d == null) LinkError();
		else {
			data.RequestCustomer = d.ID;
			data.RequestCustomerNama = d.Nama;

			//Commit
			db.SubmitChanges();
		}
	}
}