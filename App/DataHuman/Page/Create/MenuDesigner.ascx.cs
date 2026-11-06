using System;

public partial class Page_Create_MenuDesigner : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Designer.aspx");
			App.MenuURL(l2, "DesignerAccount.aspx");
			App.MenuURL(l3, "DesignerAddr.aspx");
			App.MenuURL(l4, "DesignerSubmit.aspx");
		}
	}
}