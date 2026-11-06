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
        Response.Redirect("ProductOrderList.aspx");
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Approval == null);
		if (d == null) App.DataNull(this);
		else {
			//Delete
			deleteProcess(d);

			//Redirect
            Response.Redirect("ProductOrderList.aspx?d=1");
		}
	}

	//Delete
	protected void deleteProcess(ProductOrder d) {

        Db.Execute("DELETE FROM ProductOrder_CharDesign WHERE ID = " + d.ID + "; "
            + "DELETE FROM ProductOrder_CharProduct WHERE ID = " + d.ID + "; "
            + "DELETE FROM ProductOrder_Finishing WHERE IDForm = " + d.ID + "; "
            + "DELETE FROM ProductOrder_Stone1A WHERE IDForm = " + d.ID + "; "
            + "DELETE FROM ProductOrder_Stone1B WHERE IDForm = " + d.ID + "; "
            + "DELETE FROM ProductOrder_Stone2 WHERE IDForm = " + d.ID + "; "
            + "DELETE FROM ProductOrder_Stone3 WHERE IDForm = " + d.ID + "; "
            + "DELETE FROM ProductOrder_Stone4 WHERE IDForm = " + d.ID + "; "
            + "DELETE FROM ProductOrder_Stone5 WHERE IDForm = " + d.ID + "; "
            + "DELETE FROM ProductOrder WHERE ID = " + d.ID + "; "
            , Db.Cnn_DB);
	}
}