using System;
using System.Linq;

public partial class Page_Register_Repair_DataDesignerSave : System.Web.UI.Page 
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
    protected string myCust { get { return Request.QueryString["kdcust"]; } }
	protected int myLink { get { return App.QueryInt(this, "link"); } }

	//Exception
	protected void DataError() { Response.Redirect("Repair.aspx"); }
    protected void LinkError() { Response.Redirect("Repair_DataCustomer.aspx?id=" + myID); }
    protected void DataError2() { Response.Redirect("Repair_DataCustomer.aspx?id=" + myID + "&err=1"); }
    protected void DataSuccess() { Response.Redirect("Repair_Form.aspx?id=" + myID + "&status=1"); }

	protected void Page_Load(object sender, EventArgs e) 
    {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
        if (d == null) DataError();
        else
        {
            bool status = Customer.saveLink_DocRepair(myID, myCust);
            if (status) { DataSuccess(); } else { DataError2(); }
        }
	}
	
}