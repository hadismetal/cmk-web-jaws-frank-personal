using System;

public partial class Page_MenuFile : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//QueryString
			string qstr = "?id=" + App.QueryInt(this.Page);
			a1.HRef += qstr;
			a2.HRef += qstr;
			a3.HRef += qstr;
			a4.HRef += qstr;
			a5.HRef += qstr;

			//Active-Menu
			App.MenuURL(l1, "File.aspx");
			App.MenuURL(l2, "Update.aspx");
			App.MenuURL(l3, "UpdatePass.aspx");
			App.MenuURL(l4, "UpdateRole.aspx");
			App.MenuURL(l4, "UpdateRoleAdd.aspx");
			App.MenuURL(l5, "Log.aspx");
			App.MenuURL(l5, "LogDaily.aspx");
		}
	}
}