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
		Response.Redirect("Summary.aspx");
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Approval == null);
		if (d == null) App.DataNull(this);
		else {
			//Delete
			deleteProcess(d);

			//Redirect
			Response.Redirect("Summary.aspx?d=1");
		}
	}

	//Delete
	protected void deleteProcess(ProductDesign d) {

        Db.Execute("DELETE FROM ProductDesign_CharDesign WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductDesign_CharProduct WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductDesign_Finishing WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductDesign_PricingBiaya WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductDesign_PricingMU WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductDesign_PricingProduct WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductDesign_PricingSegmen WHERE ID=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductDesign_Stone1A WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductDesign_Stone1B WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductDesign_Stone2 WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductDesign_Stone3 WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductDesign_Stone4 WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductDesign_Stone5 WHERE IDForm=" + d.ID, Db.Cnn_DB);
        Db.Execute("DELETE FROM ProductDesign WHERE ID=" + d.ID, Db.Cnn_DB);
	}
}