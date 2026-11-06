using System;

public partial class Menu : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
            App.MenuPath(a2, "/Page/Report/");

			//Role Protection
            a2.Visible = Role.Manager;
		}
	}
}