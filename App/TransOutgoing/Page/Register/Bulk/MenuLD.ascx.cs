using System;

public partial class Page_Register_BulkAll_MenuLD : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
            App.MenuURL(l1, "LD.aspx");
            App.MenuURL(l2, "LD_Form.aspx");
            App.MenuURL(l3, "LD_Product.aspx");
            App.MenuURL(l4, "LD_Finish.aspx");
		}
	}
}