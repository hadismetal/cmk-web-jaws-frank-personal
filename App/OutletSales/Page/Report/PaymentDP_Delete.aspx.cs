using System;
using System.Linq;

public partial class Page_Approval_Resell_Delete : System.Web.UI.Page {
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
    App price = new App();

    decimal Setting = 0m;

	protected int myID { get { return App.QueryInt(this); } }

	//Exception
	protected void DeleteError(string sebab) {
		Response.Redirect("PaymentDP.aspx");
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

		var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.Draft == true);
		if (d == null) App.DataNull(this);
		else {
			//Delete
			deleteProcess(d);

			//Redirect
			Response.Redirect("PaymentDP.aspx?d=1");
		}
	}

	//Delete
	protected void deleteProcess(SalesReceiptDPPO d) {

        var rs = from p in db.SalesReceiptDPPODetail
                 where p.IDSalesReceipt == d.ID
                 select p;

        if (rs.Count() > 0)
        {
            db.SalesReceiptDPPODetail.DeleteAllOnSubmit(rs);
        }
        db.SalesReceiptDPPO.DeleteOnSubmit(d);
        db.SubmitChanges();
	}
}