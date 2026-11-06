using System;

public partial class Page_Register_MenuLD : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "LD.aspx");
			App.MenuURL(l2, "LD_Form");
			App.MenuURL(l3, "LD_List");
		}
	}
}