using System;

public partial class Page_Register_MenuDJ : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "PG.aspx");
			App.MenuURL(l2, "PG_Form.aspx");
		}
	}
}