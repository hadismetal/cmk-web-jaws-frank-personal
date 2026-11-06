using System;

public partial class Page_Register_MenuRepair : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			App.MenuURL(l1, "Repair.aspx");
			
            App.MenuURL(l2, "Repair_Form.aspx");
            App.MenuURL(l2, "Repair_DataCustomer.aspx");
            App.MenuURL(l2, "Repair_DataAgent.aspx");
            App.MenuURL(l2, "Repair_ItemAdd.aspx");

            App.MenuURL(l3, "Repair_Picture.aspx");

			App.MenuURL(l4, "Repair_Data.aspx");
            App.MenuURL(l4, "Repair_WorkAdd.aspx");

            App.MenuURL(l5, "Repair_CharProduct02.aspx");

            App.MenuURL(l6, "Repair_DataStone.aspx");
            App.MenuURL(l6, "Repair_DataStoneAdd.aspx");
            App.MenuURL(l6, "Repair_DataStoneUpdate.aspx");

            App.MenuURL(l7, "Repair_Submit");
		}
	}
}