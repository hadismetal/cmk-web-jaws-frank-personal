using System;

public partial class Page_Register_MenuCZ : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "CZ.aspx");
			App.MenuURL(l2, "CZ_Form.aspx");
		}
	}
}