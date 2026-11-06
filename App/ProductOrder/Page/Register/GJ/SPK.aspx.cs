using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Design : System.Web.UI.Page {
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
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
            spk_ERR.Visible = false;
		}
	}

    protected void getData(object sender, EventArgs e)
    {
        string Tipe = "1";
        if (rdo_no.Checked) { Tipe = "1"; }
        else if (rdo_ro.Checked) { Tipe = "2"; }
        //string Tipe = SPK.Text;

        if(Tipe=="1")
        {
            
                Response.Redirect("SPK_DesignList.aspx");
            
        }
        else if(Tipe=="2")
        {
            
                Response.Redirect("SPK_KodeKaretList.aspx");
            
        }
        else
        {
            Response.Redirect("SPK.aspx");
        }
    }
}