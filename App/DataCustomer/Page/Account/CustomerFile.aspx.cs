using System;
using System.Linq;

public partial class Page_Account_CustomerFile : System.Web.UI.Page {
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

		if (!IsPostBack) {
			//Form
			fillForm();

			//Action
			AP.Menu(ap, "DataCustomer", myID, Role.Manager);
		}
	}
	protected void fillForm() {
		var d = db.DataCustomer.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);

			//Master
			brand.InnerHtml = d.CompanyBrand.Nama + " (" + d.CompanyBrand.NamaKode + ")";

			//Account
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			tgl.InnerHtml = Str.Tgl(d.Tgl);

			//Disable
			if (d.Draft) {
				disable.InnerHtml = DL.DisableDraft();
				disable.Attributes["class"] += " text-draft";
			}
			else {
				disable.InnerHtml = DL.Disable(d.Disable);
				disable.Attributes["class"] += d.Disable ? " text-warning" : " text-success";
			}

			//Address
			addralamat.InnerHtml = Str.HtmlBreak(d.AddrAlamat);
			addrnotelp.InnerHtml = d.AddrNoTelp;
			addrnofax.InnerHtml = d.AddrNoFax;
			addremail.InnerHtml = d.AddrEmail;
		}
	}
}