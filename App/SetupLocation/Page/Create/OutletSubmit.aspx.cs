using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Create_OutletSubmit : System.Web.UI.Page {
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
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Outlet.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.LocOutlet.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nama.InnerHtml =
				"<a href=\"../Account/OutletFile.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nama + "</a>";
            kode.InnerText = d.Kode;

			//Master
			group.InnerHtml = d.LocOutletGroup.Nama + " (" + d.LocOutletGroup.NamaKode + ")";
			brand.InnerHtml = d.CompanyBrand.Nama + " (" + d.CompanyBrand.NamaKode + ")";

			//Account
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			tgl.InnerHtml = Str.Tgl(d.Tgl);

			//Address
			addralamat.InnerHtml = Str.HtmlBreak(d.AddrAlamat);
			addrnotelp.InnerHtml = d.AddrNoTelp;
			addrnofax.InnerHtml = d.AddrNoFax;
			addremail.InnerHtml = d.AddrEmail;
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.LocOutlet.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Status
			d.Draft = false;
			d.DraftDate = null;
			d.Disable = false;

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("Outlet.aspx?d=1");
		}
	}
}