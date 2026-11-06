using System;

public partial class Menu : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
			App.MenuPath(a1, "/Page/Status/");
			//App.MenuPath(a2, "/Page/Update/");
			//App.MenuPath(a3, "/Page/Approval/");

			//Role Protection
			a2.Visible = Role.Operator;
			a3.Visible = Role.Manager;
		}
	}
}