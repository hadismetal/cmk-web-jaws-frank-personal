using System;

public partial class Menu : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
			App.MenuPath(a1, "/Page/Parcel1/");
			App.MenuPath(a2, "/Page/Parcel2/");
			App.MenuPath(a3, "/Page/Parcel3/");
			App.MenuPath(a4, "/Page/Parcel4/");
			App.MenuPath(a5, "/Page/Parcel5/");
			App.MenuPath(a6, "/Page/Settings/");
		}
	}
}