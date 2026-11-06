using System;

public partial class Page_Register_MenuGJ : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "GJ.aspx");
			App.MenuURL(l2, "GJ_Form");
			App.MenuURL(l3, "GJ_Data");
			App.MenuURL(l4, "GJ_CharProduct");
			App.MenuURL(l5, "GJ_CharDesign");
			App.MenuURL(l6, "GJ_Costing");
			App.MenuURL(l7, "GJ_Picture");
			App.MenuURL(l8, "GJ_Submit.aspx");
		}
	}
}