using System;

public partial class Page_Register_MenuGJ : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "LD.aspx");
			App.MenuURL(l2, "LD_Form");
			App.MenuURL(l3, "LD_Data");
			App.MenuURL(l4, "LD_CharProduct");
			App.MenuURL(l5, "LD_CharDesign");
			App.MenuURL(l8, "LD_Submit.aspx");
		}
	}
}