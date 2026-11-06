using System;

public partial class Page_Create_MenuOutlet : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Outlet.aspx");
			App.MenuURL(l2, "OutletAccount.aspx");
			App.MenuURL(l3, "OutletAddr.aspx");
			App.MenuURL(l4, "OutletSubmit.aspx");
		}
	}
}