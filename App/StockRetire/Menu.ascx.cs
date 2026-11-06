using System;

public partial class Menu : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
			App.MenuPath(a1, "/Page/Create/");
            App.MenuPath(a2, "/Page/Approve/");
            App.MenuPath(a3, "/Page/Report/");

			//Role Protection
			a1.Visible = Role.Operator;
            a2.Visible = Role.Manager;
            //a3.Visible = Role.Manager;
			a3.Visible = (Role.DownloadExcel || Role.DownloadFile);
            //a4.Visible = Role.Manager;
		}
	}
}