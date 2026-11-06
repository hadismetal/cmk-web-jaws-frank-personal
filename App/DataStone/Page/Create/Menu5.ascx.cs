using System;

public partial class Page_Create_Menu5 : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "5.aspx");
			App.MenuURL(l2, "5_Account.aspx");
			App.MenuURL(l3, "5_Submit.aspx");
		}
	}
}