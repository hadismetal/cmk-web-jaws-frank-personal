using System;

public partial class MasterPageExport : System.Web.UI.MasterPage {
	protected void Page_Load(object sender, EventArgs e) {
		App.Init(this);

        if (!IsPostBack)
        { }
	}
}
