using System;
using System.Linq;

public partial class Page_Account_2_File : System.Web.UI.Page {
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
			AP.Menu(ap, "Stone2", myID, Role.Manager);
		}
	}
	protected void fillForm() {
		var d = db.Stone2.SingleOrDefault(p => p.ID == myID);
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
			parcel01.InnerHtml = d.Parcel201.Nama + " (" + d.Parcel201.NamaKode + ")";
			parcel02.InnerHtml = d.Parcel202.Nama + " (" + d.Parcel202.NamaKode + ")";
			parcel03.InnerHtml = d.Parcel203.Nama + " (" + d.Parcel203.NamaKode + ")";
			parcel04.InnerHtml = d.Parcel204.Nama + " (" + d.Parcel204.NamaKode + ")";
			parcel05.InnerHtml = d.Parcel205.Nama + " (" + d.Parcel205.NamaKode + ")";
			parcel06.InnerHtml = d.Parcel206.Nama + " (" + d.Parcel206.NamaKode + ")";
			parcel07.InnerHtml = d.Parcel207.Nama + " (" + d.Parcel207.NamaKode + ")";
		}
	}
}