using System;
using System.Linq;

public partial class Page_Create_AdminSalesAccount : System.Web.UI.Page {
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
	protected int tipe { get { return App.QueryInt(this, "tipe"); } }
	protected int cabang { get { return App.QueryInt(this, "cabang"); } }

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
	protected void DataError() { Response.Redirect("Designer.aspx"); }

	//Form
	protected void fillForm() {
        DLC.User(username);
        string Brand ="";
        string Lokasi = "";

        if (tipe == 1)
        {
            tipelokasi.Text = "Outlet";
            var x = db.LocOutlet.SingleOrDefault(p => p.ID == cabang);
            Lokasi = x.Nama;
            Brand = Db.SingleString("SELECT Nama FROM CompanyBrand WHERE ID=" + x.IDBrand, Db.Cnn_DB);
        }
        else
        {
            tipelokasi.Text = "Exhibition";
            var x = db.LocExhibition.SingleOrDefault(p => p.ID == cabang);
            Lokasi = x.Nama;
            Brand = Db.SingleString("SELECT Nama FROM CompanyBrand WHERE ID=" + x.IDBrand, Db.Cnn_DB);
        }

        brand.InnerText = Brand;
        lokasi.InnerText = Lokasi;

        brand.InnerText = Brand;
	}

	//Save
	protected bool valid {
		get { return Fv.isDateTime(tgl, tgl_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			DataAdminSales n = new DataAdminSales();

			//Master
            n.TipeLokasi = tipe;
            n.IDLokasi = cabang;
            n.IDUser = Convert.ToInt32(username.SelectedValue);

			//Account
			n.Nama = Str.CapTitle(nama.Text);
			n.Keterangan = Str.CapFirst(keterangan.Text);
			n.Tgl = Convert.ToDateTime(tgl.Text);

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;
			n.Disable = true;

			//Commit
			db.DataAdminSales.InsertOnSubmit(n);
			db.SubmitChanges();

			//Redirect
			Response.Redirect("AdminSalesAddr.aspx?id=" + n.ID);
		}
	}
}