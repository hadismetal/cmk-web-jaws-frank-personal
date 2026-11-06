using System;
using System.Linq;

public partial class Page_Register_Sales_ItemDelete : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("Sales.aspx"); }
	protected void LinkError() { Response.Redirect("Sales_Data.aspx?id=" + myID); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Process
			
				switch (myItemIndex) {
					case 0: saveDJ(); break;
					case 1: savePG(); break;
					case 2: saveGJ(); break;
                    case 3: saveLD(); break;
                    case 9: savePackaging(); break;
                    case 10: saveSouvenir(); break;
				
			}

			//Redirect
			Response.Redirect("Sales_Data.aspx?id=" + myID);
		}
	}
	protected void saveDJ() {
		var d = db.SalesOrderDJ.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID);
		if (d == null) LinkError();
		else {
            var x = db.StockProductDJ.SingleOrDefault(p => p.ID == d.IDProduct);
            if (x != null){
				x.StatusPenjualan = 0;
				db.SubmitChanges();
			}

			//Commit
			db.SalesOrderDJ.DeleteOnSubmit(d);
			db.SubmitChanges();
		}
	}
    protected void savePG()
    {
        var d = db.SalesOrderPG.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID);
        if (d == null) LinkError();
        else
        {
            var x = db.StockProductPG.SingleOrDefault(p => p.ID == d.IDProduct);
            if (x != null){
				x.StatusPenjualan = 0;
				db.SubmitChanges();
			}

            //Commit
            db.SalesOrderPG.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void saveGJ()
    {
        var d = db.SalesOrderGJ.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID);
        if (d == null) LinkError();
        else
        {
            var x = db.StockProductGJ.SingleOrDefault(p => p.ID == d.IDProduct);
            if (x != null){
                x.StatusPenjualan = 0;
				db.SubmitChanges();
			}
            //Commit
            db.SalesOrderGJ.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void saveLD()
    {
        var d = db.SalesOrderLD.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID);
        if (d == null) LinkError();
        else
        {
            var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == d.IDProduct);
            if (x != null){
                x.StatusPenjualan = 0;
				db.SubmitChanges();
			}
            //Commit
            db.SalesOrderLD.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void savePackaging()
    {
        var d = db.SalesOrderPackaging.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID);
        if (d == null) LinkError();
        else
        {
            var x = db.StockActualPackaging.SingleOrDefault(p => p.ID == d.IDProduct && p.TipeLokasi == d.SalesOrder.TipeLokasi && p.IDLokasi == d.SalesOrder.IDLokasi);
            if (x != null)
			{
				//x.InHand = x.InHand + d.Qty.Value;
				db.SubmitChanges();
			}

            //Commit
            db.SalesOrderPackaging.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void saveSouvenir()
    {
        var d = db.SalesOrderSouvenir.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID);
        if (d == null) LinkError();
        else
        {
            var x = db.StockActualSouvenir.SingleOrDefault(p => p.ID == d.IDProduct && p.TipeLokasi == d.SalesOrder.TipeLokasi && p.IDLokasi == d.SalesOrder.IDLokasi);
            if (x != null)
			{
				//x.InHand = x.InHand + d.Qty.Value;
				db.SubmitChanges();
			}

            //Commit
            db.SalesOrderSouvenir.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
}