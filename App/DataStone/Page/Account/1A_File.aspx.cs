using System;
using System.Linq;

public partial class Page_Account_1A_File : System.Web.UI.Page {
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
			AP.Menu(ap, "Stone1A", myID, Role.Manager);
		}
	}
	protected void fillForm() {
		var d = db.Stone1A.SingleOrDefault(p => p.ID == myID);
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
		}
	}
}