using System;

public partial class Page_Create_Menu3 : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "3.aspx");
			App.MenuURL(l2, "3_Account.aspx");
			App.MenuURL(l3, "3_Submit.aspx");
		}
	}
}