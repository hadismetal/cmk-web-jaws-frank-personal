using System;

public partial class Page_Register_MenuDJ : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "DJ.aspx");
			App.MenuURL(l2, "DJ_Form");
			App.MenuURL(l3, "DJ_Data");
			App.MenuURL(l4, "DJ_CharProduct");
			App.MenuURL(l5, "DJ_CharDesign");
			App.MenuURL(l6, "DJ_Stone");
			App.MenuURL(l7, "DJ_Costing");
			App.MenuURL(l8, "DJ_Picture");
			App.MenuURL(l9, "DJ_Submit.aspx");
		}
	}
}