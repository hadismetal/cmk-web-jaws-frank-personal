using System;

public partial class MasterFile : System.Web.UI.MasterPage {
	protected void Page_Load(object sender, EventArgs e) {
		App.Init(this);

		//Sistem Callout
		// if (Request.QueryString["d"] != null)
		// 	callout.Visible = callout_script.Visible = !IsPostBack;
	}
}
