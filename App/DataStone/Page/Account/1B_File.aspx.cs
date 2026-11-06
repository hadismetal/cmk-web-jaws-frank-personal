using System;
using System.Linq;

public partial class Page_Account_1B_File : System.Web.UI.Page {
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
			AP.Menu(ap, "Stone1B", myID, Role.Manager);
		}
	}
	protected void fillForm() {
		var d = db.Stone1B.SingleOrDefault(p => p.ID == myID);
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
			parcel01.InnerHtml = d.Parcel101.Nama + " (" + d.Parcel101.NamaKode + ")";
			parcel02.InnerHtml = d.Parcel102.Nama + " (" + d.Parcel102.NamaKode + ")";
			parcel03.InnerHtml = d.Parcel103.Nama + " (" + d.Parcel103.NamaKode + ")";
			parcel04.InnerHtml = d.Parcel104.Nama + " (" + d.Parcel104.NamaKode + ")";
			parcel05.InnerHtml = d.Parcel105.Nama + " (" + d.Parcel105.NamaKode + ")";
			parcel06.InnerHtml = d.Parcel106.Nama + " (" + d.Parcel106.NamaKode + ")";
			parcel07.InnerHtml = d.Parcel107.Nama + " (" + d.Parcel107.NamaKode + ")";
		}
	}
}