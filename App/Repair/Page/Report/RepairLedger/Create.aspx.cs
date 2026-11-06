using System;
using System.Linq;
using System.Data;

public partial class Page_Create : System.Web.UI.Page
{
	private Map_AC _ac;

	private Map_AC ac
	{
	    get
	    {
	        if (_ac == null)
	            _ac = new Map_AC();
	        return _ac;
	    }
	}

	public override void Dispose()
	{
	    if (ac != null)
	        ac.Dispose();
	    base.Dispose();
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
            lit_nik_fixed.Text = "?";
            txt_nama.Focus();
		}
	}

	//Save
	
	protected void save_Click(object sender, EventArgs e)
    {

        string user_nik = txt_nik.Text.Trim();
        string user_name = txt_nama.Text.Trim();
        string user_description = txt_keterangan.Text.Trim();
        string user_email = txt_email.Text.Trim();

        string errMsg = "";

        if (user_nik == "")
        {
            errMsg += "NIK harus diinput dan sudah terdaftar di MyApps !!!<br />";
        }
        else
        {
            if (user_name == "" || user_description == "" || user_email == "")
            {
                errMsg += "Informasi karyawan tidak Valid. Silahkan dilengkapi terlebih dahulu data berikut :<br />";
                errMsg += "<ul>";
                if (user_name == "") { errMsg += "<li>Nama Karyawan</li>"; }
                if (user_description == "") { errMsg += "<li>Jabatan Karyawan</li>"; }
                if (user_email == "")
                {
                    errMsg += "<li>Email Kantor Karyawan</li>";
                }
                errMsg += "</ul>";

            }
            
            // check if email is already exists
            errMsg += (Email.valid_Email(user_email)) ? "" : "Email <b>" + user_email + "</b> sudah pernah dibuat.<br />";

            // check if email domain is valid
            errMsg += (Email.valid_Email2(user_email)) ? "" : "Format domain Email tidak valid.<br />";
            
        }

        if (errMsg == "")
        {
            UserAccount n = new UserAccount();

            //Input
            n.InputDate = DateTime.Now;

            //Account
            n.Nama = Str.CapTitle(txt_nama.Text);
            n.Keterangan = Str.CapFirst(txt_keterangan.Text);
            n.Email = Str.CapLower(txt_email.Text);
            n.SMS = Str.CapUpper(sms.Text);
            n.UserNumber = Str.CapUpper(lit_nik_fixed.Text);

            //Password
            string randomPass = Common.RandomNumber(8);
            n.Pass = Common.Encrypt(randomPass);
            n.PassStrong = Common.StrongPass(randomPass);

            //Recycle-Pass
            n.Recycle = true;

            //Commit
            ac.UserAccount.InsertOnSubmit(n);
            ac.SubmitChanges();

            //Session-Create
            App.Session("GeneratedPass", randomPass);

            //Redirect
            Response.Redirect("CreateRole.aspx?id=" + n.ID);
        }
        else
        {
            lit_warning.Text = "<div class='alert alert-danger'>" + errMsg + "</div>";
        }
	}


    //btn_clear
    protected void btn_clear_Click(object sender, EventArgs e)
    {
        clearForm();
    }

    protected void clearForm()
    {
        txt_nik.Text = "";
        txt_nama.Text = "";
        txt_keterangan.Text = "";
        txt_email.Text = "";

        txt_nama.Enabled = true;
        txt_keterangan.Enabled = true;
        txt_email.Enabled = true;

        lit_warning.Text = "";
        save.Visible = false;
        lit_nik_fixed.Text = "?";
    }

    protected void btn_search_nik_Click(object sender, EventArgs e)
    {
        string errMsg = "";
        string warMsg = "";
        lit_warning.Text = "";
        lit_error.Text = "";

        string user_nik = txt_nik.Text.Trim();
        string user_name = txt_nama.Text.Trim();
        string user_description = txt_keterangan.Text.Trim();
        string user_email = txt_email.Text.Trim();

        if (user_nik == "")
        {
            errMsg += "NIK harus diinput dan sudah terdaftar di MyApps !!!<br />";
        }
        else
        {

            try
            {
                string errNama = "";
                string errJabatan = "";
                string errEmail = "";

                string[] infokaryawan = getNamaKaryawan(user_nik).Split('^');

                if (infokaryawan[0].Trim() != "")
                {
                    lit_nik_fixed.Text = user_nik;
                    user_name = (infokaryawan[0].Trim() != "") ? infokaryawan[0] : user_name;
                    txt_nama.Text = user_name;
                    txt_nama.Enabled = false;
                }
                else
                {
                    errNama = " Nama Karyawan, ";
                }
                
                if (infokaryawan[1].Trim() != "")
                {

                    user_description = (infokaryawan[1].Trim() != "") ? infokaryawan[1] : user_description;
                    txt_keterangan.Text = user_description;
                    txt_keterangan.Enabled = false;
                }
                else
                {
                    errJabatan = " Jabatan Karyawan, ";
                }

                if (infokaryawan[2].Trim() != "")
                {
                    user_email = (infokaryawan[2].Trim() != "") ? infokaryawan[2] : user_email;
                    txt_email.Text = user_email;
                }
                else
                {
                    errEmail = " Email kantor Karyawan, ";
                }

                if (errNama != "" || errJabatan != "" || errEmail != "")
                {
                    warMsg += "Gagal mendapatkan data karyawan MyApps : " + errNama + errJabatan + errEmail + ".<br />Anda dapat meneruskan proses pembuatan user login di aplikasi JAWS namun pastikan NIK karyawan sudah terinput dengan benar (sesuai dengan data HR Personalia)<br />";
                }

            }
            catch (Exception)
            {
                warMsg += "Gagal mendapatkan data karyawan MyApps.<br />Anda dapat meneruskan proses pembuatan user login di aplikasi JAWS namun pastikan NIK karyawan sudah terinput dengan benar (sesuai dengan data HR Personalia)<br />";
            }


            if (user_name == "" || user_description == "" || user_email == "")
            {
                errMsg += "Informasi karyawan tidak Valid. Silahkan dilengkapi terlebih dahulu data berikut :<br />";
                errMsg += "<ul>";
                if (user_name == "") { errMsg += "<li>Nama Karyawan</li>"; }
                if (user_description == "") { errMsg += "<li>Jabatan Karyawan</li>"; }
                if (user_email == "")
                {
                    errMsg += "<li>Email Kantor Karyawan</li>";
                }
                errMsg += "</ul>";

            }
            

        }

        // check if email is already exists
        errMsg += (Email.valid_Email(user_email)) ? "" : "Email <b>" + user_email + "</b> sudah pernah dibuat.<br />";

        // check if email domain is valid
        errMsg += (Email.valid_Email2(user_email)) ? "" : "Format domain Email tidak valid.<br />";
        
        if (errMsg == "")
        {
            save.Visible = true;

            // check jika nik sudah pernah dibuatkan user jaws
            // keluarkan warning namun boleh tetap dibuatkan dengan email yang berbeda
            var d = ac.UserAccount.Where(p => p.UserNumber == user_nik);
            if (d.Count() > 0)
            {
                warMsg += "NIK <b>" + user_nik + "</b> terdaftar sudah pernah memiliki <b>" + d.Count() + "</b> User login JAWS : ";
                warMsg += "<ul>";
                foreach (var r in d)
                {
                    warMsg += "<li><b>" + r.Email + "</b>" + " (" + Str.FullDateJam(r.InputDate) + ")</li>";
                }
                warMsg += "</ul>";
            }

        }
        else
        {
            lit_error.Text = "<div class='alert alert-danger'>" + errMsg + "</div>";
        }

        if (warMsg != "")
        {
            lit_warning.Text = "<div class='alert alert-warning'>" + warMsg + "</div>";
        }

    }

    protected string getNamaKaryawan(string nik)
    {
        string result = "";
        string namakaryawan = "";
        string jabatankaryawan = "";
        string emailkaryawan = "";
        string spname = "sp_getdatakaryawan";
        string query = " exec " + spname
                     + " '" + nik + "'"
                     ;
        try
        {
            DataTable dt = new DataTable();
            dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_CS"].ConnectionString);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    namakaryawan = dtRow["m_nama"].ToString();
                    jabatankaryawan = dtRow["m_jabatan"].ToString();
                    emailkaryawan = dtRow["m_emailkantor"].ToString();
                    result = namakaryawan
                           + "^" + jabatankaryawan
                           + "^" + emailkaryawan
                           ;
                }
            }
        }
        catch (Exception) { }

        return result;
    }
}