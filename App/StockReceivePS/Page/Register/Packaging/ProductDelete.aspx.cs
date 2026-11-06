using System;
using System.Linq;

public partial class Page_Register_DJ_ProductDelete : System.Web.UI.Page {
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
	protected int myTableID { get { return App.QueryInt(this, "tid"); } }

	//Exception
	protected void DataError() { Response.Redirect("Product.aspx"); }
	protected void LinkError() { Response.Redirect("ProductAdd.aspx?id=" + myID); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.StockReceivePackaging.SingleOrDefault(p => p.ID == myID);
		if (d == null) DataError();
		else {
            
			//Redirect
			Response.Redirect("ProductInfo.aspx?id=" + myID);
		}
	}
}