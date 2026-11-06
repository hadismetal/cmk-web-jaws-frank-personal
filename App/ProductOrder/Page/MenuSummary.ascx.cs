using System;

public partial class Page_Register_MenuSPK : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Summary.aspx");
			App.MenuURL(l2, "ProductStoneRequest.aspx");
		}
	}
}