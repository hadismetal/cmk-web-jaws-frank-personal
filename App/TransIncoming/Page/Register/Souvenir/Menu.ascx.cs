using System;

public partial class Page_Register_Souvenir_MenuDJ : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
            App.MenuURL(l1, "Product.aspx");
            App.MenuURL(l2, "Product_Form.aspx");
            App.MenuURL(l3, "Product_QC.aspx");
            App.MenuURL(l4, "Product_Finish.aspx");
		}
	}
}