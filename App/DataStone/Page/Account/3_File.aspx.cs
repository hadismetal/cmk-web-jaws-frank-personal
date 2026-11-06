using System;
using System.Linq;

public partial class Page_Account_3_File : System.Web.UI.Page {
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
			AP.Menu(ap, "Stone3", myID, Role.Manager);
		}
	}
	protected void fillForm() {
		var d = db.Stone3.SingleOrDefault(p => p.ID == myID);
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
			parcel01.InnerHtml = d.Parcel301.Nama + " (" + d.Parcel301.NamaKode + ")";
			parcel02.InnerHtml = d.Parcel302.Nama + " (" + d.Parcel302.NamaKode + ")";
			parcel03.InnerHtml = d.Parcel303.Nama + " (" + d.Parcel303.NamaKode + ")";
			parcel04.InnerHtml = d.Parcel304.Nama + " (" + d.Parcel304.NamaKode + ")";
			//parcel05.InnerHtml = d.Parcel305.Nama + " (" + d.Parcel305.NamaKode + ")";
			parcel06.InnerHtml = d.Parcel306.Nama + " (" + d.Parcel306.NamaKode + ")";
            //parcel07.InnerHtml = d.Parcel307.Nama + " (" + d.Parcel307.NamaKode + ")";
            //parcel08.InnerHtml = d.Parcel308.Nama + " (" + d.Parcel308.NamaKode + ")";
            //parcel09.InnerHtml = d.Parcel309.Nama + " (" + d.Parcel309.NamaKode + ")";
            //parcel10.InnerHtml = d.Parcel310.Nama + " (" + d.Parcel310.NamaKode + ")";
            //parcel11.InnerHtml = d.Parcel311.Nama + " (" + d.Parcel311.NamaKode + ")";
            //parcel12.InnerHtml = d.Parcel312.Nama + " (" + d.Parcel312.NamaKode + ")";
        }
	}
}