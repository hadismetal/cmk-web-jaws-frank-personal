using System;

public partial class Page_Register_Repair_MenuRepair : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Repair.aspx");
			App.MenuURL(l2, "Repair_Form");
			App.MenuURL(l3, "Repair_Data");
            App.MenuURL(l3, "Repair_DataStone");
			App.MenuURL(l4, "Repair_Finish");

		}
	}
}