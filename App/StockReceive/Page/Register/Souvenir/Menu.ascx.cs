using System;

public partial class Page_Register_Souvenir_MenuDJ : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Product.aspx");
			App.MenuURL(l2, "ProductInfo.aspx");
			App.MenuURL(l3, "Product_Submit.aspx");
		}
	}
}