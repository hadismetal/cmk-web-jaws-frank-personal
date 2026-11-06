using System;
using System.Linq;

public partial class Page_Create_Repair_Submit : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("Repair.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.StoneRepair.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nama.InnerHtml =
				"<a href=\"../Account/Repair_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nama + "</a>";

			//Account
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			tgl.InnerHtml = Str.Tgl(d.Tgl);

			//Parcel
			kodeparcel.InnerHtml = d.KodeParcel;
			parcel01.InnerHtml = d.ParcelRepair01.Nama + " (" + d.ParcelRepair01.NamaKode + ")";
			parcel02.InnerHtml = d.ParcelRepair02.Nama + " (" + d.ParcelRepair02.NamaKode + ")";
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.StoneRepair.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Status
			d.Draft = false;
			d.DraftDate = null;
			d.Disable = false;

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("Repair.aspx?d=1");
		}
	}
}