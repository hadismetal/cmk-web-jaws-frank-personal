using System;

public partial class Page_Register_MenuSales : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Sales.aspx");
			App.MenuURL(l2, "Sales_Form");
			App.MenuURL(l3, "Sales_Data");
			App.MenuURL(l4, "Sales_CharProduct");

		}
	}
}