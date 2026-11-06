using System;
using System.Linq;
using System.Web;

public partial class Page_Register_DJ_StoneDelete : System.Web.UI.Page {
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

	//Exception
	protected void DataError() { Response.Redirect("DJLD.aspx"); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.StockProductDJLD.SingleOrDefault(p => p.ID == myID);
		if (d == null) DataError();
		else {

            Db.Execute("DELETE FROM StockProductDJLD WHERE ID=" + myID, Db.Cnn_DB);
            Uri MyUrl = Request.UrlReferrer;
			//Redirect
            Response.Redirect(MyUrl.OriginalString + "&d=1");
		}
	}
	
}