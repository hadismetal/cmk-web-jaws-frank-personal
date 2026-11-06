using System;

public partial class Page_Create_Menu1A : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "1A.aspx");
			App.MenuURL(l2, "1A_Account.aspx");
			App.MenuURL(l3, "1A_Submit.aspx");
		}
	}
}