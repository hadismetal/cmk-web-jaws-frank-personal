using System;

public partial class MasterPagePrint : System.Web.UI.MasterPage
{
	protected void Page_Load(object sender, EventArgs e) {
		App.Init(this);

	}
}
