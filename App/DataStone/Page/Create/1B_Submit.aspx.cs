using System;
using System.Linq;

public partial class Page_Create_1B_Submit : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("1B.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.Stone1B.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nama.InnerHtml =
				"<a href=\"../Account/1B_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nama + "</a>";

			//Account
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			tgl.InnerHtml = Str.Tgl(d.Tgl);

			//Parcel
			kodeparcel.InnerHtml = d.KodeParcel;
			parcel01.InnerHtml = d.Parcel101.Nama + " (" + d.Parcel101.NamaKode + ")";
			parcel02.InnerHtml = d.Parcel102.Nama + " (" + d.Parcel102.NamaKode + ")";
			parcel03.InnerHtml = d.Parcel103.Nama + " (" + d.Parcel103.NamaKode + ")";
			parcel04.InnerHtml = d.Parcel104.Nama + " (" + d.Parcel104.NamaKode + ")";
			parcel05.InnerHtml = d.Parcel105.Nama + " (" + d.Parcel105.NamaKode + ")";
			parcel06.InnerHtml = d.Parcel106.Nama + " (" + d.Parcel106.NamaKode + ")";
			parcel07.InnerHtml = d.Parcel107.Nama + " (" + d.Parcel107.NamaKode + ")";
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.Stone1B.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Status
			d.Draft = false;
			d.DraftDate = null;
			d.Disable = false;

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("1B.aspx?d=1");
		}
	}
}