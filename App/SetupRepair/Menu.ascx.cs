using System;

public partial class Menu : System.Web.UI.UserControl
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			//Summary
			App.MenuPath(a0, "/Page/Summary.aspx");

			//Module Menu
			App.MenuPath(a1, "/Page/Business/");
			App.MenuPath(a2, "/Page/Information/");
			App.MenuPath(a3, "/Page/Settings/");

			a1.Visible = false;
			a2.Visible = false;
			a3.Visible = false;
		}
	}
}