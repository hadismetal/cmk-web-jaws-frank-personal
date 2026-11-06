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
		Response.Redirect("Resell.aspx");
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

		var d = db.Resell.SingleOrDefault(p => p.ID == myID && p.Draft == true);
		if (d == null) App.DataNull(this);
		else {
			//Delete
			deleteProcess(d);

			//Redirect
			Response.Redirect("Resell.aspx?d=1");
		}
	}

	//Delete
	protected void deleteProcess(Resell d) {

        var dj = d.ResellDJ;
        var pg = d.ResellPG;
        var gj = d.ResellGJ;
        var ld = d.ResellLD;

        var j = db.Resell.SingleOrDefault(p=>p.ID == myID);

        foreach (var r in dj)
        {
            var x = db.SalesOrderDJ.SingleOrDefault(p => p.SalesOrder.ID == r.IDSalesOrder && p.SalesOrder.StatusVoid == false && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                db.SubmitChanges();
            }
        }

        foreach (var r in pg)
        {
            var x = db.SalesOrderPG.SingleOrDefault(p => p.SalesOrder.ID == r.IDSalesOrder && p.SalesOrder.StatusVoid == false && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                db.SubmitChanges();
            }
        }

        foreach (var r in gj)
        {
            var x = db.SalesOrderGJ.SingleOrDefault(p => p.SalesOrder.ID == r.IDSalesOrder && p.SalesOrder.StatusVoid == false && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                db.SubmitChanges();
            }
        }

        foreach (var r in ld)
        {
            var x = db.SalesOrderLD.SingleOrDefault(p => p.SalesOrder.ID == r.IDSalesOrder && p.SalesOrder.StatusVoid == false && p.IDProduct == r.IDProduct);
            if (x != null)
            {
                x.StatusResell = 0;
                db.SubmitChanges();
            }
        }

        //var y = db.SalesOrderPG.SingleOrDefault(p => p.SalesOrder.ID == j.ID && p.SalesOrder.StatusVoid == false);
        //if (y != null)
        //{
        //    y.StatusResell = 0;
        //    db.SubmitChanges();
        //}

        //var z = db.SalesOrderGJ.SingleOrDefault(p => p.SalesOrder.ID == j.ID && p.SalesOrder.StatusVoid == false);
        //if (z != null)
        //{
        //    z.StatusResell = 0;
        //    db.SubmitChanges();
        //}

        //var zt = db.SalesOrderLD.SingleOrDefault(p => p.SalesOrder.ID == j.ID && p.SalesOrder.StatusVoid == false);
        //if (zt != null)
        //{
        //    zt.StatusResell = 0;
        //    db.SubmitChanges();
        //}

        Db.Execute("DELETE FROM ResellDJ WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ResellPG WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ResellGJ WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ResellLD WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM Resell WHERE ID=" + d.ID, Db.Cnn_DB);
	}
}