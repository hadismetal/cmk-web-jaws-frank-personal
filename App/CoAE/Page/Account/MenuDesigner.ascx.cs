using System;

public partial class Page_Account_MenuDesigner : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//QueryString
			string qstr = "?id=" + App.QueryInt(this.Page);
			a1.HRef += qstr;
			a2.HRef += qstr;
			a3.HRef += qstr;

			//Active-Menu
			App.MenuURL(l1, "DesignerFile.aspx");
            App.MenuURL(l2, "DesignerUpdate.aspx");
            App.MenuURL(l3, "DesignerUpdateAddr.aspx");

			//Role Protection
			l2.Visible = Role.Operator || Role.Manager;
			l3.Visible = Role.Operator || Role.Manager;
		}
	}
}