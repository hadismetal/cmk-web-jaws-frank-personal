using System;

public partial class Page_Create_Menu1B : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "1B.aspx");
			App.MenuURL(l2, "1B_Account.aspx");
			App.MenuURL(l3, "1B_Submit.aspx");
		}
	}
}