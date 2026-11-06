using System;
using System.Linq;

public partial class Page_UserFile : System.Web.UI.Page {
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
			//Form
			fillForm();

			//Action
			AP.Menu(ap, "UserAccount", myID, Role.Manager);
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
            nama.InnerHtml = d.Nama;
            nik.InnerHtml = (d.UserNumber != null ? d.UserNumber : "");
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			email.InnerHtml = d.Email;
			sms.InnerHtml = d.SMS;

			//Status
			status.InnerHtml = DL.Status(d.Status);

			//Blokir Account
			if (d.Blokir.HasValue)
				blokir.InnerHtml = "Blokir Account";

			//Last Login
			if (!d.LastLogin.HasValue) {
				lastlogin.InnerHtml = "Tidak Pernah";
				lastlogin.Attributes["class"] = "text-danger";
			}
			else {
				string css = d.LastLogin.Value.AddMonths(3) < DateTime.Now ? "text-danger" : "";
				lastlogin.InnerHtml = Str.FullDateJam(d.LastLogin);
				lastlogin.Attributes["class"] = css;
			}

			//Password
			if (d.Recycle) {
				pass.InnerHtml = "Wajib Ganti";
				pass.Attributes["class"] = "text-warning";
			}
			else {
				if (d.PassStrong) {
					pass.InnerHtml = "Baik";
					pass.Attributes["class"] = "text-success";
				}
				else {
					pass.InnerHtml = "Kurang Baik";
					pass.Attributes["class"] = "text-red";
				}
			}

			//Recycle-Pass
			recyclelast.InnerHtml = Str.FullDateJam(d.RecycleLast);
		}
	}
}