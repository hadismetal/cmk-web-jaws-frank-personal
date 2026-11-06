using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Create_PackagingSubmit : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("Product.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.Packaging.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nama.InnerHtml =
				"<a href=\"../Account/ProductFile.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nama + "</a>";

			//Account
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);

			//Address
            brand.InnerText = d.CompanyBrand.Nama;
            kode.InnerText = d.Kode;
            satuan.InnerText = d.Satuan;

            //Picture
            Upload.Image(imgPicture, d.ImgPicture);
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.Packaging.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Status
			d.Draft = false;
			d.DraftDate = null;
			d.Disable = false;

			//Commit
            db.SubmitChanges();
            
			//Redirect
			Response.Redirect("Product.aspx?d=1");
		}
	}
}