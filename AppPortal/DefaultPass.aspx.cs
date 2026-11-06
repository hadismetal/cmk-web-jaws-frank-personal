using System;
using System.Linq;

public partial class DefaultPass : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			licensedto.Text = App.LicensedTo; //Licensed

			//Init
			email.Focus();
			gagal.Visible = false;

			//Caps-Lock
			App.CapsLock(pass1);
			App.CapsLock(pass2);
		}
	}

	//Send
	protected void sendKey(object sender, EventArgs e) {
		var d = db.UserAccount.SingleOrDefault(p => p.Email == email.Text.Trim() && p.Status == 1);
		if (d == null) {
			email.Focus();
			gagal.Visible = true;
		}
		else {
			//Generate
			d.PassResetKey = Common.RandomNumber(8);
			db.SubmitChanges();

			//Send
			sendMail(d);

			//User-Info
			userid.Text = d.ID.ToString();
			myMail.InnerHtml = d.Email;

			//Next Step
			div_1.Visible = false;
			div_2.Visible = true;
			resetkey.Focus();
		}
	}
	protected void sendMail(UserAccount d) {
		string subj = string.Empty;
		string body = string.Empty;

		subj += "Reset Password : " + App.LicensedTo;
		body += "<strong>Kode Rahasia :</strong><br />";
		body += d.PassResetKey;
		body += "<br />";

		Mail.Send(d.Email, subj, body);
	}

	//Validate
	protected int myID { get { return Convert.ToInt32(userid.Text); } }
	protected void validKey(object sender, EventArgs e) {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID);
		if (d == null) Response.Redirect("/");
		else {
			if (d.PassResetKey != resetkey.Text) {
				resetkey.Focus();
				keyerror.Visible = true;
			}
			else {
				//Next Step
				div_2.Visible = false;
				div_3.Visible = true;
				pass1.Focus();
			}
		}
	}

	//Save Password
	protected void save_Click(object sender, EventArgs e) {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID);
		if (d == null) Response.Redirect("/");
		else {
			d.Pass = Common.Encrypt(pass1.Text);
			d.PassStrong = Common.StrongPass(pass1.Text);

			//Recycle-Pass
			d.Recycle = false;
			d.RecycleLast = DateTime.Now;

			//Reset-Blokir
			d.PassInvalid = 0;
			d.PassResetKey = null;
			d.Blokir = null;

			//Commit
			db.SubmitChanges();
			Log.Access(d.ID, d.Nama, 2); //LOG : Reset Password

			//Redirect
			Response.Redirect("/");
		}
	}
}