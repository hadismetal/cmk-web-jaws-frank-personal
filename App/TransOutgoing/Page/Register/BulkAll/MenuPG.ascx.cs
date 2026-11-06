using System;

public partial class Page_Register_Bulk_MenuPG : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
            App.MenuURL(l1, "PG.aspx");
            App.MenuURL(l2, "PG_Form.aspx");
            App.MenuURL(l3, "PG_Product.aspx");
            App.MenuURL(l4, "PG_Finish.aspx");
		}
	}
}