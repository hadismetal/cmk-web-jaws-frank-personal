using System;
using System.Linq;

public partial class Page_Register_Sales_ItemAddSave : System.Web.UI.Page {
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

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			
		}

        Save();
	}

	//Exception
	protected void DataError() { Response.Redirect("Sales.aspx"); }

	
	protected void Save() {
        var d = db.StockActualLD_Stone1B.SingleOrDefault(p => p.IDProduct == myID && p.StockProductLD_Stone1B.ReturnProduct == false);
        if (d == null) DataError();
        else
        {
            d.StockProductLD_Stone1B.BookExpire = null;
            d.StockProductLD_Stone1B.TipeLokasi = null;
            d.StockProductLD_Stone1B.IDLokasi = null;
            db.SubmitChanges();
            Response.Redirect("LD_List.aspx?d=1");
        }	
	}
}