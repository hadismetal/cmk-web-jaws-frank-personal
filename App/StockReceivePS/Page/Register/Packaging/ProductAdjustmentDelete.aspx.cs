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
	protected void DataError() { Response.Redirect("ProductAdjustment.aspx"); }
	protected void LinkError() { Response.Redirect("ProductAdjustmentAdd.aspx?id=" + myID); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.AdjustmentPackaging.SingleOrDefault(p => p.ID == myID);
		if (d == null) DataError();
		else {
            var x = db.AdjustmentPackaging_Product.SingleOrDefault(p => p.ID == myTableID);
            if (x == null) LinkError();
            else
            {
                db.AdjustmentPackaging_Product.DeleteOnSubmit(x);
                db.SubmitChanges();
            }

			//Redirect
			Response.Redirect("ProductAdjustmentInfo.aspx?id=" + myID);
		}
	}
}