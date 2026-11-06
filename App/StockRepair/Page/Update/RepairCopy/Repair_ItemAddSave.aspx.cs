using System;
using System.Linq;

public partial class Page_Register_Repair_ItemAddSave : System.Web.UI.Page {
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
    App gp = new App();
	protected int myID { get { return App.QueryInt(this); } }
	protected int myIDItem { get { return App.QueryInt(this, "item"); } }
    protected int myItemIndex { get { return App.QueryInt(this, "itemindex"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			
		}

        Save();
	}

	//Exception
	protected void DataError() { Response.Redirect("Repair.aspx"); }

	
	protected void Save() {
        var d = db.DocRepair.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            if (myItemIndex == 1)
            {
                var s = db.StockProductDJ.SingleOrDefault(p => p.ID == myIDItem);
                if (s != null)
                {
                    d.IDProduct = s.ID;
                    db.SubmitChanges();
                }
            }
            else if (myItemIndex == 2)
            {
                var s = db.StockProductDJCustomer.SingleOrDefault(p => p.ID == myIDItem);
                if (s != null)
                {
                    d.IDProductTitipan = s.ID;
                    db.SubmitChanges();
                }
            }
            Response.Redirect("Repair.aspx?id=" + myID + "&status=1");
        }	
	}
}