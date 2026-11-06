using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Create_3 : System.Web.UI.Page {
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
		//App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			parcel01.Focus();

			//DLC
			DLC.Parcel301(parcel01);
			DLC.Parcel302(parcel02);
			DLC.Parcel303(parcel03);
			DLC.Parcel304(parcel04);
			DLC.Parcel305(parcel05);
			DLC.Parcel306(parcel06);
			DLC.Parcel307(parcel07);
			DLC.Parcel308(parcel08);
            DLC.Parcel309(parcel09);
            DLC.Parcel310(parcel10);
            DLC.Parcel311(parcel11);
            DLC.Parcel312(parcel12);


            //DLC-Control
            App.DropDown(save, parcel01, parcel01_ERR, "3-01");
			App.DropDown(save, parcel02, parcel02_ERR, "3-02");
			App.DropDown(save, parcel03, parcel03_ERR, "3-03");
			App.DropDown(save, parcel04, parcel04_ERR, "3-04");
			App.DropDown(save, parcel05, parcel05_ERR, "3-05");
			App.DropDown(save, parcel06, parcel06_ERR, "3-06");
			App.DropDown(save, parcel07, parcel07_ERR, "3-07");
            App.DropDown(save, parcel08, parcel08_ERR, "3-08");
            App.DropDown(save, parcel09, parcel09_ERR, "3-09");
            App.DropDown(save, parcel10, parcel10_ERR, "3-10");
            App.DropDown(save, parcel11, parcel11_ERR, "3-11");
            App.DropDown(save, parcel12, parcel12_ERR, "3-12");
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
    protected int myParcel08 { get { return Convert.ToInt32(parcel08.SelectedValue); } }
    protected int myParcel09 { get { return Convert.ToInt32(parcel09.SelectedValue); } }
    protected int myParcel10 { get { return Convert.ToInt32(parcel10.SelectedValue); } }
    protected int myParcel11 { get { return Convert.ToInt32(parcel11.SelectedValue); } }
    protected int myParcel12 { get { return Convert.ToInt32(parcel12.SelectedValue); } }


    //Save
    protected bool valid_KodeParcel {
		get {
			int count = db.Stone3.Count(p =>
				p.Parcel01 == myParcel01 &&
				p.Parcel02 == myParcel02 &&
				p.Parcel03 == myParcel03 &&
				p.Parcel04 == myParcel04 &&
				p.Parcel05 == myParcel05 &&
				p.Parcel06 == myParcel06 &&
				p.Parcel07 == myParcel07 &&
                p.Parcel08 == myParcel08 &&
                p.Parcel09 == myParcel09 &&
                p.Parcel10 == myParcel10 &&
                p.Parcel11 == myParcel11 &&
                p.Parcel12 == myParcel12
				);

			bool x = count == 0;
			kodeparcel_ERR.Visible = !x;

			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid_KodeParcel) {
			//Session-Create
			App.Session("3-01", parcel01.SelectedValue);
			App.Session("3-02", parcel02.SelectedValue);
			App.Session("3-03", parcel03.SelectedValue);
			App.Session("3-04", parcel04.SelectedValue);
			App.Session("3-05", parcel05.SelectedValue);
			App.Session("3-06", parcel06.SelectedValue);
			App.Session("3-07", parcel07.SelectedValue);
            App.Session("3-08", parcel08.SelectedValue);
            App.Session("3-09", parcel09.SelectedValue);
            App.Session("3-10", parcel10.SelectedValue);
            App.Session("3-11", parcel11.SelectedValue);
            App.Session("3-12", parcel12.SelectedValue);

			//Redirect
			string href = string.Empty;

			href += "3_Account.aspx";
			href += "?parcel01=" + myParcel01;
			href += "&parcel02=" + myParcel02;
			href += "&parcel03=" + myParcel03;
			href += "&parcel04=" + myParcel04;
			href += "&parcel05=" + myParcel05;
			href += "&parcel06=" + myParcel06;
			href += "&parcel07=" + myParcel07;
            href += "&parcel08=" + myParcel08;
            href += "&parcel09=" + myParcel09;
            href += "&parcel10=" + myParcel10;
            href += "&parcel11=" + myParcel11;
            href += "&parcel12=" + myParcel12;

			Response.Redirect(href);
		}
	}
}