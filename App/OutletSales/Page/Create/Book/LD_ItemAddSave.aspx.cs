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
    protected int myIDLokasi { get { return App.QueryInt(this, "idlokasi"); } }
    protected int myTipeLokasi { get { return App.QueryInt(this, "tipe"); } }

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
        var d = db.StockActualLD_Stone1B.SingleOrDefault(p => p.IDProduct == myID && p.StockProductLD_Stone1B.ReturnProduct == false && (p.StockProductLD_Stone1B.BookExpire < DateTime.Now || !p.StockProductLD_Stone1B.BookExpire.HasValue));
        if (d == null) DataError();
        else
        {
            d.StockProductLD_Stone1B.BookExpire = DateTime.Now.AddDays(3);
            d.StockProductLD_Stone1B.TipeLokasi = myTipeLokasi;
            d.StockProductLD_Stone1B.IDLokasi = myIDLokasi;
            db.SubmitChanges();
            Response.Redirect("LD_LocationType.aspx?d=1");
        }	
	}
}