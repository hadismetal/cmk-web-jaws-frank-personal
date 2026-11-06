using System;

public partial class Page_Register_DJ_MenuDJ : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
            App.MenuURL(Li1, "DJDO.aspx");
            App.MenuURL(Li3, "DJDO_Detail");
            App.MenuURL(Li4, "DJDO_Finish");
		}
	}
}