using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
        App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
            

            DLC.LocationList(cabang, tipe);

            
		}
	}
    protected void save_Click(object sender, EventArgs e)
    {
        string Cabang = "";
        Cabang = cabang.SelectedValue;
        if (Cabang != "" && Cabang != "0")
            Response.Redirect("AdminSalesAccount.aspx?tipe=" + tipe + "&cabang=" + Cabang);
    }
}