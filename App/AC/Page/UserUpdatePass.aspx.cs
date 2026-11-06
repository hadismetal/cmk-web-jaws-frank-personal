using System;
using System.Linq;

public partial class Page_UserUpdatePass : System.Web.UI.Page {
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
			pass1.Focus();

			//Form
			fillForm();

			//Caps-Lock
			App.CapsLock(pass1);
			App.CapsLock(pass2);
		}
	}
	protected void fillForm() {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);
		}
	}

	//Save Password
	protected void save_Click(object sender, EventArgs e) {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			d.Pass = Common.Encrypt(pass1.Text);
			d.PassStrong = Common.StrongPass(pass1.Text);

			//Recycle-Pass
			d.Recycle = true;

			//Reset-Blokir
			d.PassInvalid = 0;
			d.PassResetKey = null;
			d.Blokir = null;

			//Commit
			db.SubmitChanges();

			//Redirect
			App.Reload(this, myID);
		}
	}
}