using System;
using System.Linq;

public partial class Page_Account_SupplierUpdateAddr : System.Web.UI.Page {
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
			addralamat.Focus();

			//Form
			fillForm();

			//Action
			save.Visible = Role.Operator;
		}
	}
	protected void fillForm() {
		var d = db.DataSupplier.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);

			addralamat.Text = d.AddrAlamat;
			addrnotelp.Text = d.AddrNoTelp;
			addrnofax.Text = d.AddrNoFax;
			addremail.Text = d.AddrEmail;
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.DataSupplier.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			d.AddrAlamat = Str.CapTitle(addralamat.Text);
			d.AddrNoTelp = Str.CapUpper(addrnotelp.Text);
			d.AddrNoFax = Str.CapUpper(addrnofax.Text);
			d.AddrEmail = Str.CapLower(addremail.Text);

			//Commit
			db.SubmitChanges();

			//Redirect
			App.Reload(this, myID);
		}
	}
}