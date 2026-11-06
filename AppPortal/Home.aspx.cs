using System;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;

public partial class Home : System.Web.UI.Page {
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
		App.Protect();

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
            img_1.Visible = true;
            img_2.Visible = true;

            img_1.Src = "/Images/web/logo_web_mondial.png";
            img_2.Src = "/Images/web/logo_web_missmondial.png";
        }
        else if (CB.ToLower().Contains("MONDIAL".ToLower()))
        {
            img_1.Visible = true;
            img_2.Visible = true;

            img_1.Src = "/Images/web/logo_web_mondial.png";
            img_2.Src = "/Images/web/logo_web_missmondial.png";
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

        // Audit System
        div_IT.Visible = false;

        if ((App.UserNama == "Administrator" && (App.UserID == 1))
            || (App.UserNama == "Andika" && (App.UserID == 253 || App.UserID == 163 || App.UserID == 162))
            //|| (App.UserNama == "Setiawan Susilo" && (App.UserID == 160 || App.UserID == 267 || App.UserID == 163))
            || (App.UserNama == "Budhi Hartono" && (App.UserID == 561 || App.UserID == 265 || App.UserID == 160))
            || (App.UserNama == "Mirah Jaya" && (App.UserID == 30))
            || (App.UserNama == "Muna" && (App.UserID == 35))
            || (App.UserNama == "Evi" && (App.UserID == 29))
            )
        {
            JAWSAuditSystem.ExecuteAllAuditProcedure();
            var audit = db.IT_SP_Audit.Sum(p => p.TotalIssue);

            div_IT.Visible = true;
            span_badge.InnerText = audit.ToString();
        }

		if (!IsPostBack) {
			//licensedto.Text = App.LicensedTo; //Licensed
			fillForm();
		}
	}
	protected void fillForm() {
        var a = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
		if (a != null) {
			nama.InnerHtml = a.Nama;

			//Ganti-Pass
			gantipass.Visible = Request.QueryString["d"] != null;

			//Ketentuan
			ketentuan.Visible = !a.ReadKetentuan;
            usernumber.Visible = (a.UserNumber == null && a.ReadKetentuan);
            module.Visible = !ketentuan.Visible && !(a.UserNumber == null);

            if ((a.UserNumber == null))
            {
                div_search.Visible = true;
            }

			//Module
			if (module.Visible) {
				fillModule();
				fillModuleNull();
			}
		}
	}

    //UserNumber
    protected void btn_search_Click(object sender, EventArgs e)
    {
        string xTest = "0";

        string xlahirtgl = "";
        string xlahirbln = "";
        string xlahirthn = "";
        string xlahirtgl2 = "";
        string xlahirbln2 = "";
        string xlahirthn2 = "";
        string nikkaryawan = "";

        bool xvalid1 = false;
        bool xvalid2 = false;
        bool xvalid3 = false;

        nikkaryawan = txt_usernumber.Text.Trim();
        xlahirtgl = txt_lahir_tgl.Text.Trim();
        xlahirbln = txt_lahir_bln.Text.Trim();
        xlahirthn = txt_lahir_thn.Text.Trim();

        bool valid = false;
        div_save.Visible = false;
        btn_save.Visible = false;
        error.Visible = false;

        if (!(nikkaryawan == "" && xlahirtgl == "" && xlahirbln == "" && xlahirthn == ""))
        {
            try
            {
                xTest = "1";

                string[] infokaryawan = getNamaKaryawan(nikkaryawan).Split('^');

                xTest = "2";

                xlahirtgl2 = infokaryawan[3];
                xlahirbln2 = infokaryawan[4];
                xlahirthn2 = infokaryawan[5];

                xTest = "3";

                if (xlahirtgl.PadLeft(2, '0') == xlahirtgl2.PadLeft(2, '0')) { xvalid1 = true; }
                if (xlahirbln.PadLeft(2, '0') == xlahirbln2.PadLeft(2, '0')) { xvalid2 = true; }
                if (xlahirthn.PadLeft(4, '0') == xlahirthn2.PadLeft(4, '0')) { xvalid3 = true; }

                xTest = "4";

                if (xvalid1 && xvalid2 && xvalid3)
                {
                    xTest = "5";

                    valid = (infokaryawan[2] == "1" ? true : false);

                    txt_usernama.InnerText = infokaryawan[0];
                    txt_userjabatan.InnerText = infokaryawan[1];
                    txt_userstatus.InnerHtml = (valid ? "<span style='color:green'>Aktif</span>" : "<span style='color:red'>Karyawan sudah resign atau tidak aktif</span>");

                    xTest = "6";

                    div_save.Visible = (valid ? true : false);
                    btn_save.Visible = (valid ? true : false);
                }
                else
                {
                    xTest = "7";

                    error.Visible = true;
                    errorMsg("NIK dan Tanggal lahir tidak cocok " + (xTest != "(" + xTest + ")" ? "" : "")
                        //+ ". " + xlahirtgl
                        //+ ". " + xlahirbln
                        //+ ". " + xlahirthn
                        //+ ". " + xlahirtgl2
                        //+ ". " + xlahirbln2
                        //+ ". " + xlahirthn2
                        );
                }

            }
            catch (Exception)
            {
                error.Visible = true;
                errorMsg("User Login MyApps tidak ditemukan / Terjadi kesalahan Teknis. Silahkan hubungi IT Administrator. " + (xTest != "(" + xTest + ")" ? "" : ""));
            }
        }
        else
        {
            error.Visible = true;
            errorMsg("Silahkan melengkapi NIK dan tanggal lahir. " + (xTest != "(" + xTest + ")" ? "" : ""));
        }

    }

    protected void errorMsg(string msg) { errMsg.Text = msg; }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        var a = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
        if (a != null)
        {
            //Commit
            a.Keterangan = txt_userjabatan.InnerText.Trim();
            a.UserNumber = txt_usernumber.Text.Trim();
            ac.SubmitChanges();

            //Redirect
            Response.Redirect("Home.aspx");
        }
    }

    protected string getNamaKaryawan(string nik)
    {
        string result = "";
        string namakaryawan = "";
        string jabatankaryawan = "";
        string validlogin = "";
        string lahirtgl = "";
        string lahirbln = "";
        string lahirthn = "";
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
                    validlogin = dtRow["validlogin"].ToString();
                    lahirtgl = dtRow["lahir_tgl"].ToString();
                    lahirbln = dtRow["lahir_bln"].ToString();
                    lahirthn = dtRow["lahir_thn"].ToString();
                    result = namakaryawan 
                           + "^" + jabatankaryawan
                           + "^" + validlogin
                           + "^" + lahirtgl
                           + "^" + lahirbln
                           + "^" + lahirthn
                           ;
                }
            }
        }
        catch (Exception)
        {
            error.Visible = true;
            errorMsg("Tidak dapat terhubung dengan database personalia. Silahkan hubungi IT Administrator");
        }

        return result;
    }

	//Ketentuan
	protected void setuju_Click(object sender, EventArgs e) {
        var a = ac.UserAccount.SingleOrDefault(p => p.ID == App.UserID);
		if (a != null) {
			//Commit
			a.ReadKetentuan = true;
			ac.SubmitChanges();

			//Redirect
			Response.Redirect("Home.aspx");
		}
	}

	//Module
	protected void fillModule() {
		//SELECT
        var rs = from p in ac.ModuleIndex
				 orderby p.ID
				 select p;

		int prev = 0;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			if (AC.Role(r.Kode)) {
				HtmlGenericControl div = div1;
				string css = string.Empty;

				//Assign Style
				if (r.IDGroup > 0) { div = div1; css = "btn-success"; }
				if (r.IDGroup > 3) { div = div2; css = "btn-info"; }
				if (r.IDGroup > 6) { div = div3; css = "btn-info"; }
				if (r.IDGroup > 8) { div = div4; css = "btn-info"; }

				//Group Module
				if (prev != r.IDGroup) {
					printGroup(r, div);
					prev = r.IDGroup;
				}
				//Button
				printButton(r, div, css);
			}
		}
	}
	protected void fillModuleNull() {
		div1.Visible = div1.InnerHtml != "";
		div2.Visible = div2.InnerHtml != "";
		div3.Visible = div3.InnerHtml != "";
		div4.Visible = div4.InnerHtml != "";
	}

	//Display Menu
	protected void printGroup(ModuleIndex r, HtmlGenericControl div) {
		string x = string.Empty;

		x += "<h4>";
		x += r.ModuleGroup.Nama;
		x += "</h4>";

		div.InnerHtml += x;
	}
	protected void printButton(ModuleIndex r, HtmlGenericControl div, string css) {
		string x = string.Empty;

		//HREF
		string href = "/" + r.Kode + "/";
		href += "?uid=" + App.UserID;
		href += "&sid=" + Session.SessionID;

		//x += "<p>";
		x += "<a style='padding:6px 10px; margin:0 3px 3px 0; width:144px; font-size:12px;' href=\"" + href + "\" class=\"btn " + css + "\">";
		x += r.Nama;
		x += "</a>";
		//x += "</p>";

		div.InnerHtml += x;
	}
}