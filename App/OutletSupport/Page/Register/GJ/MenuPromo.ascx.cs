using System;

public partial class Page_Create_MenuProduct : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "GJ.aspx");
			App.MenuURL(l2, "GJInfo.aspx");
			
		}
	}
}