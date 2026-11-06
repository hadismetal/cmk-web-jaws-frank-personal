using System;

public partial class MasterPage : System.Web.UI.MasterPage {
	protected void Page_Load(object sender, EventArgs e) {
		App.Init(this);

		if (!IsPostBack)
			module.InnerHtml = App.NamaModule;

		//Sistem Callout
		if (Request.QueryString["d"] != null)
			callout.Visible = callout_script.Visible = !IsPostBack;
	}
}
