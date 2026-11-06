using System;
using System.Linq;

public partial class Page_CreateSendPass : System.Web.UI.Page {
	private Map_AC _db;

	private Map_AC db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_AC();
	        return _db;
	    }
	}

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    base.Dispose();
	}
	protected int myID { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Create.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID && p.Status == 0);
		if (d == null) DataError();
		else {
			//Header
			header_nama.InnerHtml =
				"<a href=\"UserFile.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nama + "</a>";

			//Password
			pass.InnerHtml = GeneratedPass;
			email.InnerHtml = d.Email;
			a_mailto.HRef = mailTo;
		}
	}
	protected string mailTo {
		get {
			string x = string.Empty;

			x += "mailto:" + email.InnerHtml;
			x += "?subject=Password : " + App.LicensedTo;
			x += "&body=" + GeneratedPass;

			return x;
		}
	}

	//Password
	protected void sendPass(object sender, EventArgs e) {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID && p.Status == 0);
		if (d == null) DataError();
		else {
			bool OK = sendMail(d);

			result.InnerHtml = OK ? "Berhasil" : "Tidak Berhasil";
			result.Attributes["class"] = OK ? "text-success" : "text-danger";
		}
	}
	protected bool sendMail(UserAccount d) {
		string subj = string.Empty;
		string body = string.Empty;

		subj += "Password : " + App.LicensedTo;
		body += "<strong>Password :</strong><br />";
		body += GeneratedPass;
		body += "<br />";

		return Mail.Send(d.Email, subj, body);
	}
	protected string GeneratedPass {
		get { return App.Session("GeneratedPass"); }
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID && p.Status == 0);
		if (d == null) DataError();
		else {
			//Redirect
			Response.Redirect("CreateSubmit.aspx?id=" + myID);
		}
	}
}