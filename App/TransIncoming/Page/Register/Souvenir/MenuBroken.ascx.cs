using System;

public partial class Page_Register_Souvenir_Menu : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
            App.MenuURL(l1, "ProductBroken.aspx");
            App.MenuURL(l2, "ProductBroken_Form.aspx");
            App.MenuURL(l3, "ProductBroken_QC.aspx");
            App.MenuURL(l4, "ProductBroken_Finish.aspx");
		}
	}
}