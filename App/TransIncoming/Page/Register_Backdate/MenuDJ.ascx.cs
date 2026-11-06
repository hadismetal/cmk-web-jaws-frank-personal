using System;

public partial class Page_Register_Incoming2_MenuDJ : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
            App.MenuURL(l1, "DJ.aspx");
            App.MenuURL(l2, "DJ_Form.aspx");
            App.MenuURL(l3, "DJ_QC.aspx");
            App.MenuURL(l4, "DJ_Finish.aspx");
		}
	}
}