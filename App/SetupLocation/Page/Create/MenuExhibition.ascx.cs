using System;

public partial class Page_Create_MenuExhibition : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Exhibition.aspx");
			App.MenuURL(l2, "ExhibitionAccount.aspx");
			App.MenuURL(l3, "ExhibitionAddr.aspx");
			App.MenuURL(l4, "ExhibitionSubmit.aspx");
		}
	}
}