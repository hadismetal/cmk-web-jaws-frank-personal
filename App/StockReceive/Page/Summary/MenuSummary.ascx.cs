using System;

public partial class Page_Register_MenuSummary : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
            App.MenuURL(li2, "DJ");
            App.MenuURL(li3, "PG");
			App.MenuURL(li4, "GJ");
            App.MenuURL(li5, "LD");
            App.MenuURL(li6, "Mounting");
		}
	}
}