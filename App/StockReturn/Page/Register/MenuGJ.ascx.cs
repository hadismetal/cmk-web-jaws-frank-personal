using System;

public partial class Page_Register_MenuGJ : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "GJ.aspx");
			App.MenuURL(l2, "GJ_Form");
			App.MenuURL(l3, "GJ_List");
		}
	}
}