using System;
using System.Linq;

public partial class Page_Account_5_File : System.Web.UI.Page {
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
			AP.Menu(ap, "Stone5", myID, Role.Manager);
		}
	}
	protected void fillForm() {
		var d = db.Stone5.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);

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

			//Parcel
			kodeparcel.InnerHtml = d.KodeParcel;
			parcel01.InnerHtml = d.Parcel501.Nama + " (" + d.Parcel501.NamaKode + ")";
			parcel02.InnerHtml = d.Parcel502.Nama + " (" + d.Parcel502.NamaKode + ")";
			parcel03.InnerHtml = d.Parcel503.Nama + " (" + d.Parcel503.NamaKode + ")";
			parcel04.InnerHtml = d.Parcel504.Nama + " (" + d.Parcel504.NamaKode + ")";
			parcel05.InnerHtml = d.Parcel505.Nama + " (" + d.Parcel505.NamaKode + ")";
			parcel06.InnerHtml = d.Parcel506.Nama + " (" + d.Parcel506.NamaKode + ")";
			parcel07.InnerHtml = d.Parcel507.Nama + " (" + d.Parcel507.NamaKode + ")";
			parcel08.InnerHtml = d.Parcel508.Nama + " (" + d.Parcel508.NamaKode + ")";
			parcel09.InnerHtml = d.Parcel509.Nama + " (" + d.Parcel509.NamaKode + ")";
		}
	}
}