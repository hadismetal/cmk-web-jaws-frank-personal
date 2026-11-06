using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Create_AdminSalesSubmit : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("AdminSales.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.DataAdminSales.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nama.InnerHtml =
				"<a href=\"../Account/AdminSalesFile.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nama + "</a>";

			//Master
            string Lokasi = "";
            string Brand = "";

            if (d.TipeLokasi == 1)
            {
                tipelokasi.Text = "Outlet";
                var x = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
                Lokasi = x.Nama;
                Brand = Db.SingleString("SELECT Nama FROM CompanyBrand WHERE ID=" + x.IDBrand, Db.Cnn_DB);
            }
            else
            {
                tipelokasi.Text = "Exhibition";
                var x = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                Lokasi = x.Nama;
                Brand = Db.SingleString("SELECT Nama FROM CompanyBrand WHERE ID=" + x.IDBrand, Db.Cnn_DB);
            }

            brand.InnerText = Brand;
            lokasi.InnerText = Lokasi;

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
		var d = db.DataAdminSales.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Status
			d.Draft = false;
			d.DraftDate = null;
			d.Disable = false;

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("AdminSales_LocationType.aspx?d=1");
		}
	}
}