using System;
using System.Linq;

public partial class Page_Register_Sales_ItemDelete : System.Web.UI.Page
{
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

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);
        //Response.Redirect("ghiui");
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            //Process		
            //Response.Redirect("ghjd");

            switch (myItemIndex)
            {
                case 0: saveSouvenir(); break;
                case 1: saveDJ(); break;
                case 2: saveGJ(); break;
                case 3: savePG(); break;
                case 4: saveLD(); break;
            }

            //Redirect
            Response.Redirect("Sales_Data.aspx?id=" + myID);
        }
    }
    protected void saveDJ()
    {
        var d = db.SalesOrderSouvenir.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID && p.TipeProduct == 1);
        if (d == null) LinkError();
        else
        {
            //Response.Redirect("Jdks");
            var x = db.StockProductDJ.SingleOrDefault(p => p.ID == d.IDProductSouvenir);
            if (x != null){
				x.StatusPenjualan = 0;
				db.SubmitChanges();
			}
            //x.StockActualDJ.InHand = x.StockActualDJ.InHand + 1;

            //Commit
            db.SalesOrderSouvenir.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void savePG()
    {
        var d = db.SalesOrderSouvenir.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID && p.TipeProduct == 3);
        if (d == null) LinkError();
        else
        {
            var x = db.StockProductPG.SingleOrDefault(p => p.ID == d.IDProductSouvenir);
            if (x != null){
				x.StatusPenjualan = 0;
				db.SubmitChanges();
			}
           // x.StockActualPG.InHand = x.StockActualPG.InHand + d.Qty.Value;

            //Commit
            db.SalesOrderSouvenir.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void saveGJ()
    {
        var d = db.SalesOrderSouvenir.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID && p.TipeProduct == 2);
        if (d == null) LinkError();
        else
        {
            var x = db.StockProductGJ.SingleOrDefault(p => p.ID == d.IDProductSouvenir);
            if (x != null){
				x.StatusPenjualan = 0;
				db.SubmitChanges();
			}
            //x.StockActualGJ.InHand = x.StockActualGJ.InHand + d.Qty.Value;

            //Commit
            db.SalesOrderSouvenir.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void saveLD()
    {
        var d = db.SalesOrderSouvenir.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID && p.TipeProduct == 4);
        if (d == null) LinkError();
        else
        {
            var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == d.IDProductSouvenir);
            if (x != null){
				x.StatusPenjualan = 0;
				db.SubmitChanges();
			}
            //x.StockActualLD_Stone1B.InHand = x.StockActualLD_Stone1B.InHand + d.Qty.Value;

            //Commit
            db.SalesOrderSouvenir.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void saveSouvenir()
    {
        var d = db.SalesOrderSouvenir.SingleOrDefault(p => p.IDForm == myID && p.ID == myIID && p.TipeProduct == 0);
        if (d == null) LinkError();
        else
        {
            var x = db.StockActualSouvenir.SingleOrDefault(p => p.ID == d.IDProduct && p.TipeLokasi == d.SalesOrder.TipeLokasi && p.IDLokasi == d.SalesOrder.IDLokasi);
            if (x != null){
                //x.InHand = x.InHand + d.Qty.Value;
				db.SubmitChanges();
			}
            //Commit
            db.SalesOrderSouvenir.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
}