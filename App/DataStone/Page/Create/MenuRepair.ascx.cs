using System;

public partial class Page_Create_MenuRepair : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Repair.aspx");
			App.MenuURL(l2, "Repair_Account.aspx");
			App.MenuURL(l3, "Repair_Submit.aspx");
		}
	}
}