using System;
using System.Linq;

public partial class Page_Create_ExhibitionAccount : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("Exhibition.aspx"); }

	//Form
	protected void fillForm() {
		var d1 = db.LocExhibitionGroup.SingleOrDefault(p => p.ID == myIDGroup && !p.Disable);
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
    protected bool validKode
    {
        get
        {
            bool check = true;
            if (kode.Text.Length == 3)
                check = true;
            else
            {
                kode_ERR.Visible = check;
                check = false;
            }

            return check;
        }
    }
	protected void save_Click(object sender, EventArgs e) {
		if (valid && validKode) {
			LocExhibition n = new LocExhibition();

			//Master
			n.IDGroup = myIDGroup;
			n.IDBrand = myIDBrand;

			//Account
			n.Nama = Str.CapTitle(nama.Text);
            n.Kode = Str.CapUpper(kode.Text);
			n.Keterangan = Str.CapFirst(keterangan.Text);
			n.Tgl = Convert.ToDateTime(tgl.Text);

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;
			n.Disable = true;

			//Commit
			db.LocExhibition.InsertOnSubmit(n);
			db.SubmitChanges();

			//Redirect
			Response.Redirect("ExhibitionAddr.aspx?id=" + n.ID);
		}
	}
}