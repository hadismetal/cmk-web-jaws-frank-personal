using System;

public partial class Page_Register_MenuDesign : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Design.aspx");
			App.MenuURL(l2, "Design_Form");
			App.MenuURL(l3, "Design_Data");
			App.MenuURL(l4, "Design_CharProduct");
			App.MenuURL(l5, "Design_CharDesign");
			App.MenuURL(l6, "Design_Stone");
			App.MenuURL(l8, "Design_Picture");
			App.MenuURL(l9, "Design_Submit.aspx");
		}
	}
}