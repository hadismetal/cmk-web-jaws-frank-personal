using System;

public partial class Page_Create_MenuSales : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Sales.aspx");
			App.MenuURL(l2, "SalesAccount.aspx");
			App.MenuURL(l3, "SalesAddr.aspx");
			App.MenuURL(l4, "SalesSubmit.aspx");
		}
	}
}