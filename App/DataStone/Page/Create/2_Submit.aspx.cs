using System;
using System.Linq;

public partial class Page_Create_2_Submit : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("2.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.Stone2.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nama.InnerHtml =
				"<a href=\"../Account/2_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nama + "</a>";

			//Account
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			tgl.InnerHtml = Str.Tgl(d.Tgl);

			//Parcel
			kodeparcel.InnerHtml = d.KodeParcel;
			parcel01.InnerHtml = d.Parcel201.Nama + " (" + d.Parcel201.NamaKode + ")";
			parcel02.InnerHtml = d.Parcel202.Nama + " (" + d.Parcel202.NamaKode + ")";
			parcel03.InnerHtml = d.Parcel203.Nama + " (" + d.Parcel203.NamaKode + ")";
			parcel04.InnerHtml = d.Parcel204.Nama + " (" + d.Parcel204.NamaKode + ")";
			parcel05.InnerHtml = d.Parcel205.Nama + " (" + d.Parcel205.NamaKode + ")";
			parcel06.InnerHtml = d.Parcel206.Nama + " (" + d.Parcel206.NamaKode + ")";
			parcel07.InnerHtml = d.Parcel207.Nama + " (" + d.Parcel207.NamaKode + ")";
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.Stone2.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Status
			d.Draft = false;
			d.DraftDate = null;
			d.Disable = false;

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("2.aspx?d=1");
		}
	}
}