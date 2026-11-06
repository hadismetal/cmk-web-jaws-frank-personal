using System;

public partial class Page_Register_MenuRepair : System.Web.UI.UserControl {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
            App.MenuURL(l1, "RepairInvoice.aspx");

            App.MenuURL(l2, "RepairInvoice_Form.aspx");
            App.MenuURL(l2, "RepairInvoice_DataRepair.aspx");

            //App.MenuURL(l3, "RepairInvoice_Picture.aspx");

            App.MenuURL(l4, "RepairInvoice_Data.aspx");
            App.MenuURL(l4, "RepairInvoice_WorkAdd.aspx");

            App.MenuURL(l5, "RepairInvoice_CharProduct02.aspx");

            App.MenuURL(l6, "RepairInvoice_DataStone.aspx");
            App.MenuURL(l6, "RepairInvoice_DataStoneAdd.aspx");
            App.MenuURL(l6, "RepairInvoice_DataStoneUpdate.aspx");

            App.MenuURL(l7, "RepairInvoice_Submit");
		}
	}
}