using System;
using System.Linq;

public partial class HomeProfile : System.Web.UI.Page {
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
		App.Protect();

		if (!IsPostBack) {
			licensedto.Text = App.LicensedTo; //Licensed

			fillForm();

			//Caps-Lock
			App.CapsLock(pass1);
			App.CapsLock(pass2);

			//Recycle
			if (Request.QueryString["gp"] != null)
				pass1.Focus();
		}
	}
	protected void fillForm() {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
		if (d == null) App.DataNull(this);
		else {
			nama.InnerHtml = d.Nama;
			email.InnerHtml = d.Email;
			//sms.InnerHtml = d.SMS;

			lastlogin.InnerHtml = Str.FullDateJam(d.LastLogin);
			recyclelast.InnerHtml = Str.FullDateJam(d.RecycleLast);

			//Pass-Strong
			strong.Visible = !d.PassStrong;
		}
	}

	//Save Password
	protected void save_Click(object sender, EventArgs e) {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
		if (d == null) App.DataNull(this);
		else {
			d.Pass = Common.Encrypt(pass1.Text);
			d.PassStrong = Common.StrongPass(pass1.Text);

			//Recycle-Pass
			d.Recycle = false;
			d.RecycleLast = DateTime.Now;

			//Commit
			db.SubmitChanges();
			Log.AccessSignIn(3); //LOG : Update Password

			//Redirect
			Response.Redirect("/Home.aspx?d=1");
		}
	}
}