using System;
using System.Linq;

public partial class Page_Create_1A : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			parcel01.Focus();

			//DLC
			DLC.Parcel101(parcel01);
			DLC.Parcel102(parcel02, false);
			DLC.Parcel103(parcel03);
			DLC.Parcel104(parcel04);

			//DLC-Control
			App.DropDown(save, parcel01, parcel01_ERR, "1A-01");
			App.DropDown(save, parcel02, parcel02_ERR, "1A-02");
			App.DropDown(save, parcel03, parcel03_ERR, "1A-03");
			App.DropDown(save, parcel04, parcel04_ERR, "1A-04");
		}
	}

	//Parameter
	protected int myParcel01 { get { return Convert.ToInt32(parcel01.SelectedValue); } }
	protected int myParcel02 { get { return Convert.ToInt32(parcel02.SelectedValue); } }
	protected int myParcel03 { get { return Convert.ToInt32(parcel03.SelectedValue); } }
	protected int myParcel04 { get { return Convert.ToInt32(parcel04.SelectedValue); } }

	//Save
	protected bool valid_KodeParcel {
		get {
			int count = db.Stone1A.Count(p =>
				p.Parcel01 == myParcel01 &&
				p.Parcel02 == myParcel02 &&
				p.Parcel03 == myParcel03 &&
				p.Parcel04 == myParcel04
				);

			bool x = count == 0;
			kodeparcel_ERR.Visible = !x;

			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid_KodeParcel) {
			//Session-Create
			App.Session("1A-01", parcel01.SelectedValue);
			App.Session("1A-02", parcel02.SelectedValue);
			App.Session("1A-03", parcel03.SelectedValue);
			App.Session("1A-04", parcel04.SelectedValue);

			//Redirect
			string href = string.Empty;

			href += "1A_Account.aspx";
			href += "?parcel01=" + myParcel01;
			href += "&parcel02=" + myParcel02;
			href += "&parcel03=" + myParcel03;
			href += "&parcel04=" + myParcel04;

			Response.Redirect(href);
		}
	}
}