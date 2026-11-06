using System;
using System.Linq;

public partial class Page_Approval_Resell_Delete : System.Web.UI.Page {
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

	//Exception
	protected void DeleteError(string sebab) {
		Response.Redirect("TradeIn.aspx");
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

		var d = db.TradeIn.SingleOrDefault(p => p.ID == myID && p.Draft == true);
		if (d == null) App.DataNull(this);
		else {
			//Delete
            deleteProcess(d);
			deleteProcess(d.Resell);
            deleteProcess(d.SalesOrder);

			//Redirect
			Response.Redirect("TradeIn.aspx?d=1");
		}
	}

    //Delete
    protected void deleteProcess(TradeIn d)
    {
        Db.Execute("DELETE FROM TradeIn WHERE ID=" + d.ID, Db.Cnn_DB);
    }

	//Delete
	protected void deleteProcess(Resell d) {

        var dj = d.ResellDJ;
        var pg = d.ResellPG;
        var gj = d.ResellGJ;

        var j = db.Resell.SingleOrDefault(p=>p.ID == d.ID);

        var x = db.SalesOrderDJ.SingleOrDefault(p => p.SalesOrder.ID == j.ID && p.SalesOrder.StatusVoid == false);
        if (x != null)
        {
            x.StatusResell = 0;
            db.SubmitChanges();
        }
   
        var y = db.SalesOrderPG.SingleOrDefault(p => p.SalesOrder.ID == j.ID && p.SalesOrder.StatusVoid == false);
        if (y != null)
        {
            y.StatusResell = 0;
            db.SubmitChanges();
        }

        var z = db.SalesOrderGJ.SingleOrDefault(p => p.SalesOrder.ID == j.ID && p.SalesOrder.StatusVoid == false);
        if (z != null)
        {
            z.StatusResell = 0;
            db.SubmitChanges();
        }

        Db.Execute("DELETE FROM ResellDJ WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ResellPG WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ResellGJ WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM Resell WHERE ID=" + d.ID, Db.Cnn_DB);
	}

    //Delete
    protected void deleteProcess(SalesOrder d)
    {

        var dj = d.SalesOrderDJ;
        var pg = d.SalesOrderPG;
        var gj = d.SalesOrderGJ;
        var ld = d.SalesOrderLD;
        var pc = d.SalesOrderPackaging;
        var sv = d.SalesOrderSouvenir;

        foreach (var r in dj)
        {
            var x = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                x.StatusPenjualan = 0;
                db.SubmitChanges();
            }
        }

        foreach (var r in pg)
        {
            var x = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                x.StatusPenjualan = 0;
                db.SubmitChanges();
            }
        }

        foreach (var r in gj)
        {
            var x = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                x.StatusPenjualan = 0;
                db.SubmitChanges();
            }
        }

        foreach (var r in ld)
        {
            var x = db.StockProductLD.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                x.StatusPenjualan = 0;
                db.SubmitChanges();
            }
        }

        foreach (var r in pc)
        {
            var x = db.StockActualPackaging.SingleOrDefault(p => p.ID == r.IDProduct && p.TipeLokasi == d.TipeLokasi && p.IDLokasi == d.IDLokasi);
            if (x != null)
            {
                x.InHand = x.InHand + r.Qty.Value;
                db.SubmitChanges();
            }
        }

        foreach (var r in sv)
        {
            var x = db.StockActualSouvenir.SingleOrDefault(p => p.ID == r.IDProduct && p.TipeLokasi == d.TipeLokasi && p.IDLokasi == d.IDLokasi);
            if (x != null)
            {
                x.InHand = x.InHand + r.Qty.Value;
                db.SubmitChanges();
            }
        }

        var resell = from p in db.ResellDJ
                     where p.IDSalesOrder == d.ID
                     select p;

        if (resell.Count() > 0)
            Db.Execute("DELETE FROM Resell WHERE IDResell = " + resell.First().IDForm, Db.Cnn_DB);

        Db.Execute("DELETE FROM ResellDJ WHERE IDSalesOrder = " + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ResellPG WHERE IDSalesOrder = " + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ResellGJ WHERE IDSalesOrder = " + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ResellLD WHERE IDSalesOrder = " + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM TradeIn WHERE IDSalesOrder = " + d.ID, Db.Cnn_DB);

        Db.Execute("DELETE FROM SalesOrderDJ WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM SalesOrderPG WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM SalesOrderGJ WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM SalesOrderLD WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM SalesOrderPackaging WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM SalesOrderSouvenir WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM SalesOrder WHERE ID=" + d.ID, Db.Cnn_DB);

    }
}