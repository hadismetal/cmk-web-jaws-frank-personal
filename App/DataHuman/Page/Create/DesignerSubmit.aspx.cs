using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Create_DesignerSubmit : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("Designer.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.DataHuman.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nama.InnerHtml =
				"<a href=\"../Account/DesignerFile.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nama + "</a>";

			//Master
			group.InnerHtml = d.DataHumanGroup.Nama + " (" + d.DataHumanGroup.NamaKode + ")";
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
		var d = db.DataHuman.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Status
			d.Draft = false;
			d.DraftDate = null;
			d.Disable = false;

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("Designer.aspx?d=1");
		}
	}
}