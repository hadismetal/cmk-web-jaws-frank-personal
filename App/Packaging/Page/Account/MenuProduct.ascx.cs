using System;

public partial class Page_Account_MenuProduct : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//QueryString
			string qstr = "?id=" + App.QueryInt(this.Page);
			a1.HRef += qstr;
			a2.HRef += qstr;
			a3.HRef += qstr;
            a4.HRef += qstr;

			//Active-Menu
			App.MenuURL(l1, "ProductFile.aspx");
			App.MenuURL(l2, "ProductUpdate.aspx");
			App.MenuURL(l3, "ProductUpdateAddr.aspx");
            App.MenuURL(l4, "ProductUpdateImage.aspx");

			//Role Protection
			l2.Visible = Role.Operator || Role.Manager;
			l3.Visible = Role.Operator || Role.Manager;
            l4.Visible = Role.Operator || Role.Manager;
		}
	}
}