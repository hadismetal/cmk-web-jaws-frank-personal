using System;
using System.Linq;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page {
	private Map_AC _ac;
	private Map_DB _db;

	private Map_AC ac
	{
	    get
	    {
	        if (_ac == null)
	            _ac = new Map_AC();
	        return _ac;
	    }
	}

	private Map_DB db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_DB();
	        return _db;
	    }
	}

	public override void Dispose()
	{
	    if (ac != null)
	        ac.Dispose();
	    if (db != null)
	        db.Dispose();
	    base.Dispose();
	}

	protected void Page_Load(object sender, EventArgs e) 
    {
        /// announcement
        //if (System.DateTime.Now <= Convert.ToDateTime("2017-10-16").AddDays(1)) { div_announcement_1.Visible = true; }

        string CB = ""; /// mondial, franknco, missmondial, thepalace
        CB = System.Configuration.ConfigurationManager.AppSettings["LicensedTo"];

        HtmlImage Img1 = img_1;
        HtmlImage Img2 = img_2;
        HtmlImage Img3 = img_3;

        img_1.Visible = false;
        img_2.Visible = false;
        img_3.Visible = false;

        if (CB.ToLower().Contains("WHOLESALE".ToLower()))
        {
            img_1.Visible = true;
            img_1.Src = "/Images/web/logo_web_cmk_2017.png";
        }
        else if (CB.ToLower().Contains("MONDIAL RARE JEWELS".ToLower()))
        {
            img_1.Visible = true;

            img_1.Src = "/Images/web/logo_web_mrj.png";
        }
        else if (CB.ToLower().Contains("MISS MONDIAL".ToLower()))
        {
            img_2.Visible = true;
            img_3.Visible = true;

            img_2.Src = "/Images/web/logo_web_mondial.png";
            img_3.Src = "/Images/web/logo_web_missmondial.png";
        }
        else if (CB.ToLower().Contains("MONDIAL".ToLower()))
        {
            img_2.Visible = true;
            img_3.Visible = true;

            img_2.Src = "/Images/web/logo_web_mondial.png";
            img_3.Src = "/Images/web/logo_web_missmondial.png";
        }
        else if (CB.ToLower().Contains("THE PALACE".ToLower()))
        {
            img_1.Visible = true;
            img_1.Src = "/Images/web/logo_web_thepalace.png";
        }
        else if (CB.ToLower().Contains("FRANK".ToLower()))
        {
            img_1.Visible = true;
            img_1.Src = "/Images/web/logo_web_franknco.png";
        }
        else if (CB.ToLower().Contains("ZIRCON".ToLower()))
        {
            img_1.Visible = true;
            img_1.Src = "/Images/web/logo_web_cubiczirconia.png";
        }
        else
        {
            img_1.Visible = true;
            img_1.Src = "/Images/web/logo_web_cmk_2017.png";
        }

		if (!IsPostBack) {
			//licensedto.Text = App.LicensedTo; //Licensed

            div_mode.Visible = false;
            if (Request.Url.ToString().Contains("dev")) { div_mode.Visible = true; lit_mode.Text = "Development Version"; }
            if (Request.Url.ToString().Contains("local")) { div_mode.Visible = true; lit_mode.Text = "Localhost Version"; }

			//Activation
			if (!Act.Registered(myKey))
				Server.Transfer("/Page/InvalidServer.aspx");

			//Init
			email.Focus();
			gagal.Visible = false;
			debug.Visible = App.Debug; //Debug Warning

			//Caps-Lock
			App.CapsLock(pass);

			//Sign-Out
			if (Request.QueryString["so"] != null) {
				Session.Abandon();
				Response.Redirect("/");
			}

			//Active-Login
			if (App.SignIn) Response.Redirect("/Home.aspx");
		}
	}

	//Activation
	protected string[] myKey {
		get {
			string[] x =
			{	
				"*", //Bypass
				"5368-55C7-1AC0-643A-DA68-AF58-1D47-6AD2", //Laptop
				"0000-0000-0000-0000-0000-0000-0000-0000", //Development Server
				"9999-9999-9999-9999-9999-9999-9999-9999", //Live Server
			};
			return x;
		}
	}

	//Sign-In Procedure
	protected void signIn(object sender, EventArgs e) {
		gagal.Visible = false;

		var d = ac.UserAccount.SingleOrDefault(p => p.Email == email.Text.Trim() && p.Status == 1);
		if (d == null) ErrorEmail();
		else {
			if (d.Blokir.HasValue && d.Blokir > DateTime.Now) ErrorBlokir(d.Blokir);
			else {
				if (d.Pass != Common.Encrypt(pass.Text)) {
					int DefaultSuspend = 15; //Constant

					d.PassInvalid++;
					ac.SubmitChanges();

					if (d.PassInvalid == 1) ErrorPass("Password Salah 1x");
					if (d.PassInvalid == 2) ErrorPass("Password Salah 2x, Kesempatan Terakhir");
					if (d.PassInvalid >= 3) {
						ErrorPass("Blokir Account (" + DefaultSuspend + " menit)");

						d.PassInvalid = 0;
						d.Blokir = DateTime.Now.AddMinutes(DefaultSuspend);

						//Commit
						ac.SubmitChanges();
						Log.Access(d.ID, d.Nama, 1); //LOG : Blokir
					}
				}
				else {
					App.UserID = d.ID;
					App.UserNama = d.Nama;

					//Reset-Blokir
					d.PassInvalid = 0;
					d.PassResetKey = null;
					d.Blokir = null;

					//Recycle-Pass
                    if (!d.RecycleLast.HasValue ||
                        (d.RecycleLast.HasValue && d.RecycleLast.Value.AddMonths(12) < DateTime.Now)) //Setiap 12 Bulan
                        d.Recycle = true;
                    else
                        d.Recycle = false;

					//Login-Info
					d.LastLogin = DateTime.Now;
					d.SessionID = Session.SessionID;

					//Commit
					ac.SubmitChanges();
					Log.AccessSignIn(0); //LOG : Sign-In

					//Redirect
					if (d.Recycle)
						Response.Redirect("/HomeProfile.aspx?gp=1");
					else
						Response.Redirect("/Home.aspx");
				}
			}
		}
	}

	//Error System
	protected string errIcon {
		get { return "<i class=\"icon-arrow-right icon-large\"></i> "; }
	}
	protected void ErrorEmail() {
		email.Focus();
		gagal.Visible = true;
		alasan.InnerHtml = errIcon + "Alamat Email Tidak Terdaftar";
	}
	protected void ErrorPass(string x) {
		pass.Focus();
		gagal.Visible = true;
		alasan.InnerHtml = errIcon + x;
	}
	protected void ErrorBlokir(DateTime? suspend) {
		TimeSpan x = suspend.Value - DateTime.Now;
		email.Focus();
		gagal.Visible = true;
		alasan.InnerHtml = errIcon + "Blokir Account (" + x.Minutes + " Menit)";
	}
}