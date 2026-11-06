using System;
using System.Linq;

public partial class Page_Create_Payment_ItemList_Delete : System.Web.UI.Page {
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
    protected int myID2 { get { return Convert.ToInt32(Request.QueryString["id2"]); } }

    //Exception
    protected void DeleteError(string sebab) {
		Response.Redirect("PaymentDP_ItemList.aspx?id=" + myID2 + "");
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

		var d = db.SalesReceiptDPPODetail.SingleOrDefault(p => p.ID == myID && p.SalesReceiptDPPO.Draft == true);
		if (d == null) App.DataNull(this);
		else {
            //Delete
            db.SalesReceiptDPPODetail.DeleteOnSubmit(d);            
            db.SubmitChanges();

            //Update Total Bayar DP
            var x = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID2);
            var y = db.SalesReceiptDPPODetail.Where(p => p.IDSalesReceipt == myID2).Sum(p => p.Nominal);

            x.TotalBayar = y;
            db.SubmitChanges();

            //Redirect
            Response.Redirect("PaymentDP_ItemList.aspx?id=" + myID2 + "");
		}
	}

	//Delete
	//protected void deleteProcess(SalesReceiptDPPODetail d) {

 //       var rs = from p in db.SalesReceiptDPPODetail
 //                where p.IDSalesReceipt == d.ID
 //                select p;

 //       if (rs.Count() > 0)
 //       {
 //           db.SalesReceiptDPPODetail.DeleteAllOnSubmit(rs);
 //       }
 //       db.SalesReceiptDPPO.DeleteOnSubmit(d);
 //       db.SubmitChanges();
	//}
}