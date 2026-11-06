using System;
using System.Linq;

public partial class Page_Register_LD_StoneDelete : System.Web.UI.Page {
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

	//Exception
	protected void DataError() { Response.Redirect("LD.aspx"); }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        var d = db.StockReturnLD_Product.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            db.StockReturnLD_Product.DeleteOnSubmit(d);
            db.SubmitChanges();

            //Redirect
            Response.Redirect("LD_List.aspx?id=" + d.IDForm);
        }
    }
}