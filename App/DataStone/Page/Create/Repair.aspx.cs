using System;
using System.Linq;

public partial class Page_Create_Repair : System.Web.UI.Page {
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
			DLC.ParcelRepair01(parcel01);
			DLC.ParcelRepair02(parcel02);

			//DLC-Control
			App.DropDown(save, parcel01, parcel01_ERR, "Repair-01");
			App.DropDown(save, parcel02, parcel02_ERR, "Repair-02");
		}
	}

	//Parameter
	protected int myParcel01 { get { return Convert.ToInt32(parcel01.SelectedValue); } }
	protected int myParcel02 { get { return Convert.ToInt32(parcel02.SelectedValue); } }

	//Save
	protected bool valid_KodeParcel {
		get {
			int count = db.StoneRepair.Count(p =>
				p.Parcel01 == myParcel01 &&
				p.Parcel02 == myParcel02 
				);

			bool x = count == 0;
			kodeparcel_ERR.Visible = !x;

			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid_KodeParcel) {
			//Session-Create
			App.Session("Repair-01", parcel01.SelectedValue);
			App.Session("Repair-02", parcel02.SelectedValue);

			//Redirect
			string href = string.Empty;

			href += "Repair_Account.aspx";
			href += "?parcel01=" + myParcel01;
			href += "&parcel02=" + myParcel02;

			Response.Redirect(href);
		}
	}
}