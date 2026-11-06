using System;
using System.Linq;

public partial class Page_ReportSales_DPPO_Report_Delete : System.Web.UI.Page {
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

    

	protected int myID { get { return App.QueryInt(this); } }
    

    //Exception
    protected void DeleteError(string sebab) {
		Response.Redirect("DPPO_Report.aspx?d=1");
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

       

            var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.Draft == true && p.StatusDP == 0);
            if (d == null) App.DataNull(this);
            {
                var rs = from p in db.SalesReceiptDPPODetail
                         where p.IDSalesReceipt == d.ID
                         select p;

                if (rs.Count() > 0)
                {
                    db.SalesReceiptDPPODetail.DeleteAllOnSubmit(rs);
                }
                db.SalesReceiptDPPO.DeleteOnSubmit(d);
                db.SubmitChanges();

                //Redirect
                Response.Redirect("DPPO_Report.aspx?d=1");
            }
      
     
       
	}

}