using System;

public partial class Page_Create_MenuProduct : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Souvenir.aspx");
            App.MenuURL(l6, "Souvenir_DJ.aspx");
            App.MenuURL(l7, "Souvenir_GJ.aspx");
            App.MenuURL(l8, "Souvenir_PG.aspx");
            App.MenuURL(l9, "Souvenir_LD.aspx");			
		}
	}
}