using System;

public partial class Page_MenuCreate : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Create.aspx");
			App.MenuURL(l2, "CreateRole.aspx");
			App.MenuURL(l3, "CreateSendPass.aspx");
			App.MenuURL(l4, "CreateSubmit.aspx");
		}
	}
}