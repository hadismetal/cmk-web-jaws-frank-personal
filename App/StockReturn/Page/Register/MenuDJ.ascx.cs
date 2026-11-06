using System;

public partial class Page_Register_MenuDJ : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "DJ.aspx");
			App.MenuURL(l2, "DJ_Form");
			App.MenuURL(l3, "DJ_List");
		}
	}
}