using System;

public partial class Menu : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
			App.MenuPath(a1, "/Page/Register/");
			//App.MenuPath(a2, "/Page/PG/");
            App.MenuPath(a3, "/Page/Report/");

			//Role Protection
			a1.Visible = Role.Operator;
            //a2.Visible = Role.Operator;
            a3.Visible = Role.Manager;
		}
	}
}