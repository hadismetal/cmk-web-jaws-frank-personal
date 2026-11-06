using System;

public partial class Page_Create_MenuAdminSales : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "AdminSales.aspx");
			App.MenuURL(l2, "AdminSalesAccount.aspx");
			App.MenuURL(l3, "AdminSalesAddr.aspx");
			App.MenuURL(l4, "AdminSalesSubmit.aspx");
		}
	}
}