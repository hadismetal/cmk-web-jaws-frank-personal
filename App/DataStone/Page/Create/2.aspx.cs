using System;
using System.Linq;

public partial class Page_Create_2 : System.Web.UI.Page {
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
			DLC.Parcel201(parcel01);
			DLC.Parcel202(parcel02);
			DLC.Parcel203(parcel03);
			DLC.Parcel204(parcel04);
			DLC.Parcel205(parcel05);
			DLC.Parcel206(parcel06);
			DLC.Parcel207(parcel07);

			//DLC-Control
			App.DropDown(save, parcel01, parcel01_ERR, "2-01");
			App.DropDown(save, parcel02, parcel02_ERR, "2-02");
			App.DropDown(save, parcel03, parcel03_ERR, "2-03");
			App.DropDown(save, parcel04, parcel04_ERR, "2-04");
			App.DropDown(save, parcel05, parcel05_ERR, "2-05");
			App.DropDown(save, parcel06, parcel06_ERR, "2-06");
			App.DropDown(save, parcel07, parcel07_ERR, "2-07");
		}
	}

	//Parameter
	protected int myParcel01 { get { return Convert.ToInt32(parcel01.SelectedValue); } }
	protected int myParcel02 { get { return Convert.ToInt32(parcel02.SelectedValue); } }
	protected int myParcel03 { get { return Convert.ToInt32(parcel03.SelectedValue); } }
	protected int myParcel04 { get { return Convert.ToInt32(parcel04.SelectedValue); } }
	protected int myParcel05 { get { return Convert.ToInt32(parcel05.SelectedValue); } }
	protected int myParcel06 { get { return Convert.ToInt32(parcel06.SelectedValue); } }
	protected int myParcel07 { get { return Convert.ToInt32(parcel07.SelectedValue); } }

	//Save
	protected bool valid_KodeParcel {
		get {
			int count = db.Stone2.Count(p =>
				p.Parcel01 == myParcel01 &&
				p.Parcel02 == myParcel02 &&
				p.Parcel03 == myParcel03 &&
				p.Parcel04 == myParcel04 &&
				p.Parcel05 == myParcel05 &&
				p.Parcel06 == myParcel06 &&
				p.Parcel07 == myParcel07
				);

			bool x = count == 0;
			kodeparcel_ERR.Visible = !x;

			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid_KodeParcel) {
			//Session-Create
			App.Session("2-01", parcel01.SelectedValue);
			App.Session("2-02", parcel02.SelectedValue);
			App.Session("2-03", parcel03.SelectedValue);
			App.Session("2-04", parcel04.SelectedValue);
			App.Session("2-05", parcel05.SelectedValue);
			App.Session("2-06", parcel06.SelectedValue);
			App.Session("2-07", parcel07.SelectedValue);

			//Redirect
			string href = string.Empty;

			href += "2_Account.aspx";
			href += "?parcel01=" + myParcel01;
			href += "&parcel02=" + myParcel02;
			href += "&parcel03=" + myParcel03;
			href += "&parcel04=" + myParcel04;
			href += "&parcel05=" + myParcel05;
			href += "&parcel06=" + myParcel06;
			href += "&parcel07=" + myParcel07;

			Response.Redirect(href);
		}
	}
}