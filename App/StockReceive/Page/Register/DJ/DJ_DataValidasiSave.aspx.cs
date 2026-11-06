using System;
using System.Linq;

public partial class Page_Register_DJ_DataValidasiSave : System.Web.UI.Page {
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

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (cs != null)
	        cs.Dispose();
	    base.Dispose();
	}
	protected int myID { get { return App.QueryInt(this); } }
    protected string myValidation { get { return Request.QueryString["link"]; } }

	//Exception
	protected void DataError() { Response.Redirect("DJ.aspx"); }
	protected void LinkError() { Response.Redirect("DJ_DataValidasi.aspx?id=" + myID); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			saveLink(d);

			//Redirect
			Response.Redirect("DJ_Data01.aspx?id=" + myID);
		}
	}

	protected void saveLink(StockReceiveDJ data) {
        
        var d = cs.T_brj.SingleOrDefault(
            p =>
            p.NoValidasi == myValidation
            );

		if (d == null) LinkError();
		else {
            data.NoValidasi = d.NoValidasi;


			//Commit
			db.SubmitChanges();
		}
	}
}