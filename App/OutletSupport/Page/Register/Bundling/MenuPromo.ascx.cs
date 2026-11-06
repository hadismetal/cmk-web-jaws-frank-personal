using System;

public partial class Page_Create_MenuProduct : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Bundling.aspx");
			App.MenuURL(l2, "Product_DJ.aspx");
            App.MenuURL(l3, "Product_GJ.aspx");
            App.MenuURL(l4, "Product_PG.aspx");
            App.MenuURL(l5, "Product_LD.aspx");
            App.MenuURL(l6, "Bundling_DJ.aspx");
            App.MenuURL(l7, "Bundling_GJ.aspx");
            App.MenuURL(l8, "Bundling_PG.aspx");
            App.MenuURL(l9, "Bundling_LD.aspx");			
		}
	}
}