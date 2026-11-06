using System;
using System.Linq;

public partial class Page_Create_4 : System.Web.UI.Page {
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
			DLC.Parcel401(parcel01);
			DLC.Parcel402(parcel02);
			DLC.Parcel403(parcel03);

			//DLC-Control
			App.DropDown(save, parcel01, parcel01_ERR, "4-01");
			App.DropDown(save, parcel02, parcel02_ERR, "4-02");
			App.DropDown(save, parcel03, parcel03_ERR, "4-03");
		}
	}

	//Parameter
	protected int myParcel01 { get { return Convert.ToInt32(parcel01.SelectedValue); } }
	protected int myParcel02 { get { return Convert.ToInt32(parcel02.SelectedValue); } }
	protected int myParcel03 { get { return Convert.ToInt32(parcel03.SelectedValue); } }

	//Save
	protected bool valid_KodeParcel {
		get {
			int count = db.Stone4.Count(p =>
				p.Parcel01 == myParcel01 &&
				p.Parcel02 == myParcel02 &&
				p.Parcel03 == myParcel03
				);

			bool x = count == 0;
			kodeparcel_ERR.Visible = !x;

			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid_KodeParcel) {
			//Session-Create
			App.Session("4-01", parcel01.SelectedValue);
			App.Session("4-02", parcel02.SelectedValue);
			App.Session("4-03", parcel03.SelectedValue);

			//Redirect
			string href = string.Empty;

			href += "4_Account.aspx";
			href += "?parcel01=" + myParcel01;
			href += "&parcel02=" + myParcel02;
			href += "&parcel03=" + myParcel03;

			Response.Redirect(href);
		}
	}
}