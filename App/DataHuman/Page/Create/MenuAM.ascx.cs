using System;

public partial class Page_Create_MenuAM : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "AM.aspx");
			App.MenuURL(l3, "AMAddr.aspx");
			App.MenuURL(l4, "AMSubmit.aspx");
		}
	}
}