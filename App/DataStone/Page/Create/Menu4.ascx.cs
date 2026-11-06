using System;

public partial class Page_Create_Menu4 : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "4.aspx");
			App.MenuURL(l2, "4_Account.aspx");
			App.MenuURL(l3, "4_Submit.aspx");
		}
	}
}