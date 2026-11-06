using System;
using System.Linq;

public partial class Page_Approval_Sales_Delete : System.Web.UI.Page {
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
		Response.Redirect("Sales_Order.aspx");
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.StatusPembayaran == false);
		if (d == null) App.DataNull(this);
		else {
			//Delete
			deleteProcess(d);

			//Redirect
			Response.Redirect("Sales_Order.aspx?d=1");
		}
	}

	//Delete
	protected void deleteProcess(SalesOrder d) {

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

        int tradein_resell_id = 0;
        int salesorder_id = 0;

        try
        {
            var tradein = db.TradeIn.SingleOrDefault(p => p.IDSalesOrder == d.ID);

            var resell = db.Resell.SingleOrDefault(p => p.ID == tradein.IDResell);

            foreach (var r in resell.ResellDJ)
            {
                var x = db.SalesOrderDJ.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                db.SubmitChanges();
            }

            foreach (var r in resell.ResellPG)
            {
                var x = db.SalesOrderPG.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                db.SubmitChanges();
            }

            foreach (var r in resell.ResellGJ)
            {
                var x = db.SalesOrderGJ.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                db.SubmitChanges();
            }

            foreach (var r in resell.ResellLD)
            {
                var x = db.SalesOrderLD.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                db.SubmitChanges();
            }

            tradein_resell_id = tradein.IDResell.Value;

        }
        catch (Exception ex) { }

        try
        {
            salesorder_id = d.ID;

            if (tradein_resell_id > 0)
            {
                Db.Execute("DELETE FROM ResellDJ WHERE IDForm = " + tradein_resell_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM ResellPG WHERE IDForm = " + tradein_resell_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM ResellGJ WHERE IDForm = " + tradein_resell_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM ResellLD WHERE IDForm = " + tradein_resell_id, Db.Cnn_DB);
            }

            if (salesorder_id > 0)
            {
                Db.Execute("DELETE FROM TradeIn WHERE IDSalesOrder = " + salesorder_id, Db.Cnn_DB);

                Db.Execute("DELETE FROM SalesOrderDJ WHERE IDForm=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrderPG WHERE IDForm=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrderGJ WHERE IDForm=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrderLD WHERE IDForm=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrderPackaging WHERE IDForm=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrderSouvenir WHERE IDForm=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesVoucher WHERE IDSalesOrder=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrder WHERE ID=" + salesorder_id, Db.Cnn_DB);
            }
        }
        catch (Exception ex) { }

	}
}