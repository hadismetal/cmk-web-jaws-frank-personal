using System;

public partial class Menu : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Role Protection
            Li1.Visible = Role.Operator;
            Li2.Visible = Role.Manager;
		}
	}
}