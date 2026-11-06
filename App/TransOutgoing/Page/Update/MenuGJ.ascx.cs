using System;

public partial class Page_Register_Update_MenuGJ : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
            App.MenuURL(l1, "GJ.aspx");
            App.MenuURL(l2, "GJ_Form.aspx");
            App.MenuURL(l3, "GJ_Product.aspx");
            App.MenuURL(l4, "GJ_Finish.aspx");
		}
	}
}