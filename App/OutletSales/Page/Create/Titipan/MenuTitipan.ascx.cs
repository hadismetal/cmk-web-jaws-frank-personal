using System;

public partial class Page_Register_MenuTitipan : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Titipan.aspx");
			App.MenuURL(l2, "Titipan_Form");
			App.MenuURL(l3, "Titipan_Data");
			App.MenuURL(l4, "Titipan_CharProduct");

		}
	}
}