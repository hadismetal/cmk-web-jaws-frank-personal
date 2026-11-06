using System;
using System.Linq;

public partial class Page_Register_Titipan_ItemAddSave : System.Web.UI.Page {
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
    App gp = new App();
    protected int myID { get { return App.QueryInt(this); } }
    protected int myTitipanID { get { return App.QueryInt(this, "tid"); } }
    protected int myItemID { get { return App.QueryInt(this, "itemid"); } }
    protected int myDocID { get { return App.QueryInt(this, "docid"); } }
    protected int myTipeID { get { return App.QueryInt(this, "tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			
		}

        Save();
	}

	//Exception
	protected void DataError() { Response.Redirect("Titipan.aspx"); }

	
	protected void Save() {
        var d = db.DocTitipan.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            
                var x = db.DocTitipan_Product.SingleOrDefault(p => p.IDForm == myID && p.IDProduct == myItemID);
                if (x != null)
                {
                    x.IDDocQC = myDocID;
                    db.SubmitChanges();
                }
            
            Response.Redirect("Titipan_Data.aspx?id=" + myID);
        }	
	}
}