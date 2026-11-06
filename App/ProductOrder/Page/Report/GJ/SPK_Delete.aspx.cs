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

		var d = db.ProductOrderGJ.SingleOrDefault(p => p.ID == myID && p.Approval == null);
		if (d == null) App.DataNull(this);
		else {
			//Delete
			deleteProcess(d);

			//Redirect
            Response.Redirect("ProductOrderList.aspx?d=1");
		}
	}

	//Delete
	protected void deleteProcess(ProductOrderGJ d) {

        Db.Execute("DELETE FROM ProductOrderGJ_CharDesign WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductOrderGJ_CharProduct WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductOrderGJ_Finishing WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductOrderGJ WHERE ID=" + d.ID, Db.Cnn_DB);
	}
}