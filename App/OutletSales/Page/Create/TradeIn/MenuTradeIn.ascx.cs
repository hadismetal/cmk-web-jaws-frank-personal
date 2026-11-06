using System;

public partial class Page_Register_MenuTradeIn : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "TradeIn.aspx");
			App.MenuURL(l2, "TradeIn_Form");
			App.MenuURL(l3, "TradeIn_Data");
			App.MenuURL(l4, "TradeIn_CharProduct");

		}
	}
}