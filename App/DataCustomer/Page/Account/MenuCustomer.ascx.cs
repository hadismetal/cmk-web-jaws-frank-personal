using System;

public partial class Page_Account_MenuCustomer : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//QueryString
			string qstr = "?id=" + App.QueryInt(this.Page);
			a1.HRef += qstr;
			a2.HRef += qstr;
			//a3.HRef += qstr;
            //a4.HRef += qstr;

			//Active-Menu
			App.MenuURL(l1, "CustomerFile.aspx");
            App.MenuURL(l2, "CustomerUpdate.aspx");
            //App.MenuURL(l3, "CustomerUpdateAddr.aspx");
            //App.MenuURL(l4, "CustomerUpdateAddInfo.aspx");

			//Role Protection
			//l2.Visible = Role.Operator || Role.Manager;
			//l3.Visible = Role.Operator || Role.Manager;
		}
	}
}