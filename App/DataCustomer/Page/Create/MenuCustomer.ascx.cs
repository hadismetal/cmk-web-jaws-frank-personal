using System;

public partial class Page_Create_MenuCustomer : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Customer.aspx");
			App.MenuURL(l2, "CustomerAccount.aspx");
			App.MenuURL(l3, "CustomerAddr.aspx");
            App.MenuURL(l4, "CustomerAddInfo.aspx");
			App.MenuURL(l5, "CustomerSubmit.aspx");
		}
	}
}