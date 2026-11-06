using System;

public partial class InvalidServer : System.Web.UI.Page {
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			sk.InnerHtml = Act.ServerKey;
			email.HRef = mailTo;
		}
	}
	protected string mailTo {
		get {
			string x = string.Empty;

			x += "mailto:" + email.InnerHtml;
			x += "?subject=Activation : " + App.LicensedTo;
			x += "&body=" + sk.InnerHtml;

			return x;
		}
	}
}