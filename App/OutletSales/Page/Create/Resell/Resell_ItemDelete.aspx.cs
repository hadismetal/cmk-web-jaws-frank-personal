using System;
using System.Linq;

public partial class Page_Register_Resell_ItemDelete : System.Web.UI.Page {
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
	protected int myItemIndex { get { return App.QueryInt(this, "itemindex"); } }
	protected int myIID { get { return App.QueryInt(this, "iid"); } }

	//Exception
	protected void DataError() { Response.Redirect("Resell.aspx"); }
	protected void LinkError() { Response.Redirect("Resell_Data.aspx?id=" + myID); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.Resell.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Process
			
				switch (myItemIndex) {
					case 0: saveDJ(); break;
					case 1: savePG(); break;
					case 2: saveGJ(); break;
                    case 3: saveLD(); break;
			}

			//Redirect
			Response.Redirect("Resell_Data.aspx?id=" + myID);
		}
	}
	protected void saveDJ() {
		var d = db.ResellDJ.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID);
		if (d == null) LinkError();
		else {
            var x = db.StockProductDJ.SingleOrDefault(p => p.ID == d.IDProduct);
            if (x != null)
                x.StatusPenjualan = 0;

			//Commit
			db.ResellDJ.DeleteOnSubmit(d);
			db.SubmitChanges();
		}
	}
    protected void savePG()
    {
        var d = db.ResellPG.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID);
        if (d == null) LinkError();
        else
        {
            var x = db.StockProductPG.SingleOrDefault(p => p.ID == d.IDProduct);
            if (x != null)
                x.StatusPenjualan = 0;

            //Commit
            db.ResellPG.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void saveGJ()
    {
        var d = db.ResellGJ.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID);
        if (d == null) LinkError();
        else
        {
            var x = db.StockProductGJ.SingleOrDefault(p => p.ID == d.IDProduct);
            if (x != null)
                x.StatusPenjualan = 0;

            //Commit
            db.ResellGJ.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void saveLD()
    {
        var d = db.ResellLD.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID);
        if (d == null) LinkError();
        else
        {
            var x = db.StockProductLD.SingleOrDefault(p => p.ID == d.IDProduct);
            if (x != null)
                x.StatusPenjualan = 0;

            //Commit
            db.ResellLD.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
}