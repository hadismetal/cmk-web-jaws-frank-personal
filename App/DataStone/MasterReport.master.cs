using System;

public partial class MasterReport : System.Web.UI.MasterPage {
	protected void Page_Load(object sender, EventArgs e) {
		App.Init(this);

		if (!IsPostBack) {
			module.InnerHtml = App.NamaModule;
			output_user.InnerHtml = App.UserNama;
		}

		//Tgl. Laporan
		output_tgl.InnerHtml = Str.FullDateJam(DateTime.Now);
	}
}
