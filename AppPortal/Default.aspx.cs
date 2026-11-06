using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
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
        if (!IsPostBack)
        {
            //Activation
            if (!Act.Registered(myKey))
                Server.Transfer("/Page/InvalidServer.aspx");

            //Init
            email.Focus();

            //Sign-Out
            if (Request.QueryString["so"] != null)
            {
                Session.Abandon();
                Response.Redirect("/");
            }

            //Active-Login
            if (App.SignIn) Response.Redirect("/Home.aspx");
        }
    }

    //Activation
    protected string[] myKey
    {
        get
        {
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
    protected void signIn(object sender, EventArgs e)
    {
        var d = ac.UserAccount.SingleOrDefault(p => p.Email == email.Text.Trim() && p.Status == 1);
        if (d == null) ErrorEmail("Email Tidak Terdaftar. Hubungi IT Support.");
        else
        {
            if (d.Blokir.HasValue && d.Blokir > DateTime.Now)
                ErrorBlokir(d.Blokir);
            else
            {
                if (d.Pass != Common.Encrypt(pass.Text))
                {
                    ErrorPass("Password Salah");
                }
                else
                {
                    lblpasswordwarning.Visible = false;

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
                    // Field-field di atas (PassInvalid/PassResetKey/Blokir/Recycle/LastLogin/
                    // SessionID) semuanya "last write wins" - tidak ada logika bisnis yang
                    // butuh optimistic-concurrency check di sini. Sebelumnya SubmitChanges()
                    // tanpa try/catch berarti kalau 2 login untuk akun yang SAMA terjadi nyaris
                    // bersamaan, request kedua akan dapat ChangeConflictException ("Row not
                    // found or changed") begitu baris itu sudah berubah duluan oleh request
                    // pertama - dan karena tidak tertangkap, exception itu menembus sampai ke
                    // Page, membuat SELURUH proses login gagal (bukan cuma gagal update field
                    // ini). Retry sekali dengan Refresh(OverwriteCurrentValues) - field-field ini
                    // aman ditimpa ulang karena memang last-write-wins.
                    try
                    {
                        ac.SubmitChanges();
                    }
                    catch (System.Data.Linq.ChangeConflictException)
                    {
                        ac.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, d);
                        d.PassInvalid = 0;
                        d.PassResetKey = null;
                        d.Blokir = null;
                        if (!d.RecycleLast.HasValue ||
                            (d.RecycleLast.HasValue && d.RecycleLast.Value.AddMonths(12) < DateTime.Now))
                            d.Recycle = true;
                        else
                            d.Recycle = false;
                        d.LastLogin = DateTime.Now;
                        d.SessionID = Session.SessionID;
                        try
                        {
                            ac.SubmitChanges();
                        }
                        catch (System.Data.Linq.ChangeConflictException)
                        {
                            // Retry kedua juga konflik (sangat jarang) - biarkan login tetap
                            // sukses, cuma field last-login/session-tracking yang skip kali ini.
                        }
                    }
                    //Log.AccessSignIn(0); //LOG : Sign-In

                    //Redirect
                    //if (d.Recycle)
                    // Response.Redirect("/HomeProfile.aspx?gp=1");
                    //else
                    Response.Redirect("/Home.aspx");
                }
            }
        }
    }

    //Error System
    protected string errIcon
    {
        get { return "<i class=\"icon-arrow-right icon-large\"></i> "; }
    }
    protected void ErrorEmail(string x)
    {
        email.Focus();
        lblpasswordwarning.Text = x;
        lblpasswordwarning.Visible = true;
    }
    protected void ErrorPass(string x)
    {
        pass.Focus();
        lblpasswordwarning.Text = x;
        lblpasswordwarning.Visible = true;
    }
    protected void ErrorBlokir(DateTime? suspend)
    {
        TimeSpan x = suspend.Value - DateTime.Now;
        lblpasswordwarning.Text = "Akun Terblokir. Hubungi IT Support.";
        lblpasswordwarning.Visible = true;
    }

    protected void forgotPassword(object sender, EventArgs e)
    {
        string newpass = "";
        if (!String.IsNullOrEmpty(txtemailsent.Text) && !String.IsNullOrEmpty(txtnikuser.Text))
            newpass = Db.SingleString("EXEC sp_kirimemailresetpassword '" + txtemailsent.Text + "','" + txtnikuser.Text + "' ", Db.Cnn_AC);
        if (newpass != "")
        {
            var d = ac.UserAccount.SingleOrDefault(p => p.UserNumber == txtnikuser.Text.Trim() && p.Status == 1);
            if (d == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "popup", "alert('Sent Failed. Your NIK is not registered.');", true);
            }
            else
            {
                d.Pass = Common.Encrypt(newpass.Trim());
                d.Blokir = null;
                ac.SubmitChanges();
                ScriptManager.RegisterStartupScript(this, GetType(), "popup", "alert('Sent Success. Please Check Your Email.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "popup", "alert('Sent Failed');", true);
        }
    }
}