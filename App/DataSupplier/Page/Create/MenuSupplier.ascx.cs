using System;

public partial class Page_Create_MenuSupplier : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Supplier.aspx");
			App.MenuURL(l2, "SupplierAccount.aspx");
			App.MenuURL(l3, "SupplierAddr.aspx");
			App.MenuURL(l4, "SupplierSubmit.aspx");
		}
	}
}