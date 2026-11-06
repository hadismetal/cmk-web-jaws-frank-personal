using System;

public partial class Page_Account_Menu3 : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//QueryString
			string qstr = "?id=" + App.QueryInt(this.Page);
			a1.HRef += qstr;
			a2.HRef += qstr;

			//Active-Menu
			App.MenuURL(l1, "3_File.aspx");
			App.MenuURL(l2, "3_Update.aspx");

            //Role Protection
            l2.Visible = Role.Manager;
		}
	}
}