using System;
using System.Linq;

public partial class Page_Create_AdminSalesAddr : System.Web.UI.Page {
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

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			addralamat.Focus();

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("AdminSales_LocationType.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.DataAdminSales.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nama.InnerHtml =
				"<a href=\"../Account/AdminSalesFile.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nama + "</a>";
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.DataAdminSales.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			d.AddrAlamat = Str.CapTitle(addralamat.Text);
			d.AddrNoTelp = Str.CapUpper(addrnotelp.Text);
			d.AddrNoFax = Str.CapUpper(addrnofax.Text);
			d.AddrEmail = Str.CapLower(addremail.Text);

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("AdminSalesSubmit.aspx?id=" + myID);
		}
	}
}