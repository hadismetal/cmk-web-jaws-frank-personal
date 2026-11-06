using System;

public partial class Page_Create_MenuWarehouse : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Warehouse.aspx");
			App.MenuURL(l2, "WarehouseAccount.aspx");
			App.MenuURL(l3, "WarehouseAddr.aspx");
			App.MenuURL(l4, "WarehouseSubmit.aspx");
		}
	}
}