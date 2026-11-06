using System;
using System.Linq;

public partial class Page_Approval_CZ_Delete : System.Web.UI.Page
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
    App price = new App();

    decimal Setting = 0m;

    protected string NoTTB { get { return Request.QueryString["nottb"]; } }

	//Exception
	protected void DeleteError(string sebab) {
		Response.Redirect("CZ.aspx");
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		//var d = db.StockReceivePG.SingleOrDefault(p => p.NoTTB == "" + NoTTB + "" && p.Approval == null);
		//if (d == null) App.DataNull(this);
        var d = from p in db.StockReceivePG
                where p.NoTTB.Equals(NoTTB) && p.Approval == null
                select p;
        if(d.Count() == 0)
            App.DataNull(this);
		else {
			//Delete
			//deleteProcess(d.First());
            db.StockReceivePG.DeleteAllOnSubmit(d);
            db.SubmitChanges();

			//Redirect
			Response.Redirect("CZ.aspx?d=1");
		}
	}

	//Delete
	protected void deleteProcess(StockReceivePG d) {
        Db.Execute("DELETE FROM StockReceivePG WHERE NoTTB='" + NoTTB + "' AND Approval is null", Db.Cnn_DB);
	}
}