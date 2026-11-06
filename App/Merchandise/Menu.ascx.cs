using System;

public partial class Menu : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//Summary
            App.MenuPath(a0, "/Page/Summary.aspx");
            App.MenuPath(a0, "/Page/Summary.aspx");

			//Role Protection
            a2.Visible = Role.Manager;
            Li6.Visible = Role.Operator;
            a2.Visible = Role.Manager;
		}
	}
}