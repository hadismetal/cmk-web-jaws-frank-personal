using System;

public partial class Page_Register_MenuMounting : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Mounting.aspx");
			App.MenuURL(l2, "Mounting_Form");
			App.MenuURL(l3, "Mounting_Data");
			App.MenuURL(l4, "Mounting_CharProduct");
			App.MenuURL(l5, "Mounting_CharDesign");
			App.MenuURL(l6, "Mounting_Costing");
			App.MenuURL(l7, "Mounting_Picture");
			App.MenuURL(l8, "Mounting_Submit.aspx");
		}
	}
}