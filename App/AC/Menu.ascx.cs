using System;

public partial class Menu : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
			App.MenuPath(a1, "/Page/Create");
			App.MenuPath(a2, "/Page/User");
			App.MenuPath(a3, "/Page/Role");
			App.MenuPath(a4, "/Page/AccessLog");
		}
	}
}