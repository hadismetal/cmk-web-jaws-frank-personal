using System;

public partial class Page_Create_Menu2 : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "2.aspx");
			App.MenuURL(l2, "2_Account.aspx");
			App.MenuURL(l3, "2_Submit.aspx");
		}
	}
}