using System;
using System.Linq;

public partial class Page_Create_SalesAccount : System.Web.UI.Page {
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

    private Map_AC _ac;

    private Map_AC ac
    {
        get
        {
            if (_ac == null)
                _ac = new Map_AC();
            return _ac;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (ac != null)
	        ac.Dispose();
	    base.Dispose();
	}
	protected int myIDGroup { get { return App.QueryInt(this, "idgroup"); } }
	protected int myIDBrand { get { return App.QueryInt(this, "idbrand"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			nama.Focus();

			//Form
			fillForm();

			//Form-Default
			tgl.Text = Str.Tgl(DateTime.Today);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Sales.aspx"); }

	//Form
	protected void fillForm() {

		var d1 = db.DataSalesGroup.SingleOrDefault(p => p.ID == myIDGroup && !p.Disable);
		if (d1 == null) DataError();
		else
			group.InnerHtml = d1.Nama + " (" + d1.NamaKode + ")";

		var d2 = db.CompanyBrand.SingleOrDefault(p => p.ID == myIDBrand && !p.Disable);
		if (d2 == null) DataError();
		else
			brand.InnerHtml = d2.Nama + " (" + d2.NamaKode + ")";
	}

	//Save
	protected bool valid {
		get { return Fv.isDateTime(tgl, tgl_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			DataSales n = new DataSales();

			//Master
			n.IDGroup = myIDGroup;
			n.IDBrand = myIDBrand;

			//Account
			n.Nama = Str.CapTitle(nama.Text);
			n.Keterangan = Str.CapFirst(keterangan.Text);
			n.Tgl = Convert.ToDateTime(tgl.Text);

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;
			n.Disable = true;

			//Commit
			db.DataSales.InsertOnSubmit(n);
			db.SubmitChanges();

			//Redirect
			Response.Redirect("SalesAddr.aspx?id=" + n.ID);
		}
	}
}