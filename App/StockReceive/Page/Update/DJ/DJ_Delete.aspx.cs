using System;
using System.Linq;

public partial class Page_Approval_DJ_Delete : System.Web.UI.Page {
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
		Response.Redirect("DJ.aspx");
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Approval == null);
		if (d == null) App.DataNull(this);
		else {
			//Delete
			deleteProcess(d);

			//Redirect
			Response.Redirect("DJ.aspx?d=1");
		}
	}

	//Delete
	protected void deleteProcess(StockReceiveDJ d) {

        Db.Execute("DELETE FROM StockReceiveDJ_CharDesign WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_CharProduct WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_Finishing WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_Stone1A WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_Stone1B WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_Stone2 WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_Stone3 WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_Stone4 WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_Stone5 WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_Costing WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_CostingProduct WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_PricingBiaya WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_PricingProduct WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_PricingMU WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ_PricingSegmen WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM StockReceiveDJ WHERE ID=" + d.ID, Db.Cnn_DB);

	}
}