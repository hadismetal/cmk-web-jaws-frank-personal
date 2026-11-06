using System;

public partial class Page_Create_MenuStockKeeper : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "StockKeeper.aspx");
			App.MenuURL(l2, "StockKeeperAccount.aspx");
			App.MenuURL(l3, "StockKeeperAddr.aspx");
			App.MenuURL(l4, "StockKeeperSubmit.aspx");
		}
	}
}