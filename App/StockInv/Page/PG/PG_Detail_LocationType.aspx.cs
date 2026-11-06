using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_PG : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
            
		}
	}

    //Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = "PG_Detail.aspx";

		//Customize-Qstr
		href += "?tipe=" + locationtype.Text;

		Response.Redirect(href);
	}
	
}