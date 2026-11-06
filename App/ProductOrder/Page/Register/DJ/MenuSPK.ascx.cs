using System;

public partial class Page_Register_MenuSPK : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
            App.MenuURL(l1, "SPK.aspx");
            App.MenuURL(l1, "SPK_DesignList.aspx");
            App.MenuURL(l1, "SPK_KodeKaretList.aspx");
			App.MenuURL(l2, "SPK_Form");
            App.MenuURL(l3, "SPK_Data");
            App.MenuURL(l4, "SPK_CharProduct");
            App.MenuURL(l5, "SPK_CharDesign");
            App.MenuURL(l6, "SPK_Stone");
            App.MenuURL(l8, "SPK_Picture");
            App.MenuURL(l9, "SPK_Submit.aspx");
		}
	}
}