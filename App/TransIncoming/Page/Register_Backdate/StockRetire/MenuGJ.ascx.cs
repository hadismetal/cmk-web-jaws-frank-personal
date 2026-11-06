using System;

public partial class Page_Register_Retire2_MenuGJ : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
            App.MenuURL(l1, "GJ.aspx");
            App.MenuURL(l2, "GJ_Form.aspx");
            App.MenuURL(l3, "GJ_QC.aspx");
            App.MenuURL(l4, "GJ_Finish.aspx");
		}
	}
}