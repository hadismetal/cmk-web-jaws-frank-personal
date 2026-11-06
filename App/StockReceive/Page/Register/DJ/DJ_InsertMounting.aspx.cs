using System;
using System.Linq;

public partial class Page_Register_DJ_InsertKodeKaret : System.Web.UI.Page {
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
	protected int myIDMounting { get { return App.QueryInt(this, "idmounting"); } }
	
	//Exception
	protected void DataError() { Response.Redirect("DJ.aspx"); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Process
            var mt = db.StockReceiveMounting.SingleOrDefault(p => p.ID == myIDMounting);
            if (mt == null) DataError();
            else
            {
                d.IDMounting = mt.ID;

                db.SubmitChanges();

                //Redirect
                Response.Redirect("DJ_Data01.aspx?id=" + myID);
            }
		}
	}
	
}