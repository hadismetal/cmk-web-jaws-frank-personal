using System;

public partial class Page_Register_MenuResell : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Resell.aspx");
			App.MenuURL(l2, "Resell_Form");
			App.MenuURL(l3, "Resell_Data");
			App.MenuURL(l4, "Resell_CharProduct");

		}
	}
}