using System;
using System.Linq;

public partial class Page_UserUpdate : System.Web.UI.Page {
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
			nama.Focus();

			//Form
			fillForm();

			//Action
			App.ConfirmDelete(del);
		}
	}
	protected void fillForm() {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);

			//Account
			nama.Text = d.Nama;
			keterangan.Text = d.Keterangan;
			email.Text = d.Email;
			sms.Text = d.SMS;
            nik.Text = d.UserNumber;

            nama.Enabled = false;
            email.Enabled = false;
            nik.Enabled = false;

			//Disable
			if (d.Status == 0) div_disable.Visible = false;
			else {
				div_draft.Visible = false;
				disable.Checked = d.Status == 2;
			}

			//Administrator mencoba update dirinya sendiri
			if (App.UserID == myID) {
				del.Enabled = false;
				disable.Enabled = false;
			}
		}
	}

	//Save
	protected bool valid_Email {
		get {
			int count = db.UserAccount.Count(p => p.Email == Str.CapLower(email.Text) && p.ID != myID);

			bool x = count == 0;
			email_ERR.Visible = !x;

			return x;
		}
	}
    protected bool valid_Email2
    {
        get
        {
            bool x = false;
            if (Str.CapLower(email.Text).Contains("@centralmegakencana.com")) { x = true; }
            if (Str.CapLower(email.Text).Contains("@frankncojewellery.com")) { x = true; }
            if (Str.CapLower(email.Text).Contains("@mondialjeweler.com")) { x = true; }
            if (Str.CapLower(email.Text).Contains("@palacejeweler.com")) { x = true; }
            if (Str.CapLower(email.Text).Contains("@miss-mondial.com")) { x = true; }
            return x;
        }
    }
	protected void save_Click(object sender, EventArgs e) {
        if (valid_Email && valid_Email2)
        {
			var d = db.UserAccount.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
				//Account
				//d.Nama = Str.CapTitle(nama.Text);
				d.Keterangan = Str.CapFirst(keterangan.Text);
				//d.Email = Str.CapLower(email.Text);
				d.SMS = Str.CapUpper(sms.Text);

				//Status
				d.Status = d.Status == 0 ? 0 : disable.Checked ? 2 : 1;

				//Commit
				db.SubmitChanges();

				//Redirect
				App.Reload(this, myID);
			}
		}
	}
	protected void del_Click(object sender, EventArgs e) {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			try {
				//Commit
				db.UserAccount.DeleteOnSubmit(d);
				db.SubmitChanges();

				//Appendix
				AP.Delete("UserAccount", myID);

				//Redirect
				App.ReloadClose(this);
			}
			catch { nodel.Visible = true; }
		}
	}
}