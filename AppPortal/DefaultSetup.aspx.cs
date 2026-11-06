using System;
using System.Linq;
using System.Web.UI.HtmlControls;

public partial class DefaultSetup : System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			licensedto.Text = App.LicensedTo; //Licensed

			//Init
			pass.Focus();
			gagal.Visible = div_action.Visible = false;
		}
	}

	//Password
	protected void go_Click(object sender, EventArgs e) {
		if (pass.Text != App.SetupPass) {
			pass.Focus();
			gagal.Visible = true;
		}
		else {
			div_pass.Visible = false;
			div_action.Visible = true;
		}
	}

	//Result System
	protected void Result(HtmlGenericControl lb, bool OK) {
		lb.InnerHtml = OK ? "Berhasil" : "Tidak Berhasil";
		lb.Attributes["class"] = OK ? "text-success" : "text-danger";
	}

	//Mail-Testing
	protected void MT(object sender, EventArgs e) {
		Result(result_mt, sendMail);
	}
	protected bool sendMail {
		get {
			string subj = string.Empty;
			string body = string.Empty;

			subj += "Mail Testing : " + App.LicensedTo;
			body += Str.TglJam(DateTime.Now);

			return Mail.Send(Mail.DefaultAddress, subj, body);
		}
	}

	//Setup
	protected void SM(object sender, EventArgs e) { Result(result_sm, Module.Setup("SistemModule.txt") && AC.SetupCleanRole()); }
	protected void SA(object sender, EventArgs e) { Result(result_sa, AC.Setup()); }
}