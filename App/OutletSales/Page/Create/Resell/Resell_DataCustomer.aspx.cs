using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_DataCustomer : System.Web.UI.Page
{
    private Map_DB _db;

    private Map_DB db
    {
        get
        {
            if (_db == null)
                _db = new Map_DB();
            return _db;
        }
    }

    private Map_CS _cs;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (cs != null)
            cs.Dispose();
        base.Dispose();
    }
    /////////////////////////////////////////////////// DO NOT CHANGE THIS !!!
    protected string myModule = "Resell"; //////////////////////

    protected int myID { get { return App.QueryInt(this); } }
    protected int myTipe { get { return App.QueryInt(this, "tipe"); } }

    /////////////////////////////////////////////////// DO NOT CHANGE THIS !!!
    protected bool checkValidID()
    {
        var d = db.Resell.SingleOrDefault(p => p.ID == myID && p.Draft); //////////////////////
        if (d == null) { return false; }
        else { return true; }
    }

    /////////////////////////////////////////////////// DO NOT CHANGE THIS !!!
    protected string getLocation(int mode)
    {
        string result = "";

        if (checkValidID())
        {
            string Kode = "NONE";
            string IDBrand = "0";

            var x = db.Resell.SingleOrDefault(p => p.ID == myID && p.Draft); //////////////////////

            if (mode == 1)
            {
                // kode
                if (x.TipeLokasi == 1)
                {
                    var kd = db.LocOutlet.SingleOrDefault(p => p.ID == x.IDLokasi);
                    Kode = (kd.Kode != null) ? kd.Kode : "NONE";
                }
                else if (x.TipeLokasi == 2)
                {
                    var kd = db.LocExhibition.SingleOrDefault(p => p.ID == x.IDLokasi);
                    Kode = (kd.Kode != null) ? kd.Kode : "NONE";
                }
                result = Kode;
            }
            else if (mode == 2)
            {
                // id brand
                if (x.TipeLokasi == 1)
                {
                    var kd = db.LocOutlet.SingleOrDefault(p => p.ID == x.IDLokasi);
                    IDBrand = kd.IDBrand.ToString();
                }
                else if (x.TipeLokasi == 2)
                {
                    var kd = db.LocExhibition.SingleOrDefault(p => p.ID == x.IDLokasi);
                    IDBrand = kd.IDBrand.ToString();
                }
                result = IDBrand;
            }

        }

        return result;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            kw_nama.Focus();

            //Form
            fillForm();

            DLC.IndonesianCity(kota);
            DLC.Agama(agama);
            tgllahir.Text = Str.Tgl(DateTime.Today);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect(myModule + ".aspx"); }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Page-Qstr
        href += "&id=" + myID;

        //Customize-Qstr
        href += "&kw_nama=" + kw_nama.Text;
        href += "&kw_kota=" + kw_kota.Text;
        href += "&kw_alamat=" + kw_alamat.Text;
        href += "&kw_telp=" + kw_telp.Text;
        href += "&kw_bday_tgl=" + kw_birth_date.Text;
        href += "&kw_bday_mon=" + kw_birth_month.Text;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw_nama.Text = Request.QueryString["kw_nama"];
            kw_kota.Text = Request.QueryString["kw_kota"];
            kw_alamat.Text = Request.QueryString["kw_alamat"];
            kw_telp.Text = Request.QueryString["kw_telp"];
            kw_birth_date.Text = Request.QueryString["kw_bday_tgl"];
            kw_birth_month.Text = Request.QueryString["kw_bday_mon"];
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        string kodelokasi = "";

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            var x = d.First();
            if (x.TipeLokasi == 1) { kodelokasi = db.LocOutlet.SingleOrDefault(p => p.ID == x.IDLokasi).Kode; }
            else if (x.TipeLokasi == 2) { kodelokasi = db.LocExhibition.SingleOrDefault(p => p.ID == x.IDLokasi).Kode; }

        }


        //SELECT
        if (kw_nama.Text.Trim().Length > 2)
        {
            var rs = from p in cs.Mscustomer
                     orderby p.M_nama
                     where p.M_kode != null
                         && p.M_nama != null
                     select p;

            rs = rs.Where(p => (p.M_nama != null && p.M_nama.Contains(kw_nama.Text.Trim()))
                        || (p.M_kode != null && p.M_kode.Contains(kw_nama.Text.Trim()))
                        || (p.M_member != null && p.M_member.Contains(kw_nama.Text.Trim()))
                        );

            if (kw_kota.Text.Trim() != "") { rs = rs.Where(p => p.M_kota != null && p.M_kota.Contains(kw_kota.Text.Trim())); }
            if (kw_alamat.Text.Trim() != "") { rs = rs.Where(p => p.M_alamat != null && p.M_alamat.Contains(kw_alamat.Text.Trim())); }
            if (kw_telp.Text.Trim() != "") { rs = rs.Where(p => p.M_telepon1.Contains(kw_telp.Text.Trim()) || p.M_telepon2.Contains(kw_telp.Text.Trim())); }
            if (kw_birth_date.Text.Trim() != "") { rs = rs.Where(p => p.M_tgllahir != null && p.M_tgllahir.Value.Day.ToString().Contains(kw_birth_date.Text.Trim())); }
            if (kw_birth_month.Text.Trim() != "") { rs = rs.Where(p => p.M_tgllahir != null && p.M_tgllahir.Value.Month.ToString().Contains(kw_birth_month.Text.Trim())); }

            if (rs.Count() > 10) { lit_countcustomer.Text = "Showing 10 / " + rs.Count() + " data"; rs = rs.Take(10); }

            //NO-RESULT
            if (rs.Count() == 0) { App.TableNoResult(tb); }
            else
            {
                //RESULT
                int prev = 0;
                int index = 1;
                foreach (var r in rs)
                {
                    if (!Response.IsClientConnected) break;

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLink(r);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<span class='text-muted pull-left' style='text-align:left;'>" + r.M_kode + "</span>"
                            + ((r.M_member != null)
                            ? ("<span class='text-muted pull-right' style='text-align:left; color:green;'>" + r.M_member + "</span>") : "")
                            + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
                            + "<span class='pull-left' style='text-align:left;'><b>" + r.M_nama + "</b></span>";
                    tr.Cells.Add(td);

                    string telp1 = (r.M_telepon1 != null) ? r.M_telepon1.Trim() : "";
                    string telp2 = (r.M_telepon2 != null) ? r.M_telepon2.Trim() : "";

                    td = new TableCell();
                    td.Text = (r.M_telepon1 != null ? ((kodelokasi.Trim() != r.M_cabang.Trim() && telp1.Length > 3) ? telp1.Remove(telp1.Length - 3) + "***" : telp1.Trim()) : "")
                            + "<br />"
                            + (r.M_telepon2 != null ? ((kodelokasi.Trim() != r.M_cabang.Trim() && telp2.Length > 3) ? telp2.Remove(telp2.Length - 3) + "***" : telp2.Trim()) : "")
                            ;
                    tr.Cells.Add(td);

                    //birthdate
                    td = new TableCell();
                    td.Text = (r.M_tgllahir != null) ? ((Str.Tgl(r.M_tgllahir).Trim()) == "01-01-1900" ? "" : Str.Tgl(r.M_tgllahir)) : "";
                    tr.Cells.Add(td);

                    //ktp, email, alamat
                    string ktp_eml_addr = ((r.M_ktp != null) ? ("<span class='text-muted pull-left' style='text-align:left;'>" + r.M_ktp + "</span>") : "")
                        + ((r.M_email != null) ? ("<span class='text-muted pull-right' style='text-align:left; color:green;'>" + ((kodelokasi.Trim() != r.M_cabang.Trim() && r.M_email.Length > 5) ? "*****" + r.M_email.Remove(0, 5) : r.M_email) + "</span>") : "")
                                        + ((r.M_email != null || r.M_ktp != null) ? ((((r.M_email != null && r.M_email != "") || (r.M_ktp != null && r.M_ktp != ""))) ? "<div style='clear:both; border-bottom:solid 1px silver;'></div>" : "") : "")
                                        + "<span class='pull-left' style='text-align:left;'>" + r.M_alamat + "</span>";

                    td = new TableCell();
                    td.Text = ktp_eml_addr;
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);
                }
            }
        }
        else
        {
            lit_countcustomer.Text = "<span style='color:red;'>Minimal 3 karakter Nama Customer !</span>";
        }
    }

    protected string TD_HyperLink(Mscustomer r, bool isMemberExists = false, bool isMemberCode = false)
    {
        string x = string.Empty;

        string disabled = (isMemberExists && !isMemberCode) ? "disabled" : "";

        x += "<a class=\"btn btn-info " + disabled + "\" href=\"" + myModule + "_DataCustomerSave.aspx?id=" + myID + "&kdcust=" + r.M_kode + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }

    //btn_search_notelp_Click
    protected void btn_search_notelp_Click(object sender, EventArgs e)
    {
        int mode = 2;
        string msg = "";
        bool isMemberExists = false;
        bool isMemberCode = false;
        bool flagNewFormCustomer = false;

        string xtelp1 = addrnotelp1.Text.Trim();

        if (Customer.isValidHPNumber(xtelp1, 2))
        {
            string xtelp2 = ""; //// 628
            string xtelp3 = ""; //// 08
            string xtelp4 = ""; //// +628

            xtelp2 = Customer.reformatHPNumber(xtelp1); //// 628
            if (xtelp2.Length > 2) { xtelp3 = "08" + xtelp2.Substring(3); } //// 08
            if (xtelp2.Length > 4) { xtelp4 = "+628" + xtelp2.Substring(5); } //// +628

            var d = cs.Mscustomer.Where(p
                => p.M_telepon1.Equals(xtelp1)
                || p.M_telepon2.Equals(xtelp1)
                || p.M_telepon1.Equals(xtelp2)
                || p.M_telepon2.Equals(xtelp2)
                || ((xtelp2.Length > 2) ? p.M_telepon1.Equals(xtelp3) : 1 == 1)
                || ((xtelp2.Length > 2) ? p.M_telepon2.Equals(xtelp3) : 1 == 1)
                || ((xtelp2.Length > 4) ? p.M_telepon1.Equals(xtelp4) : 1 == 1)
                || ((xtelp2.Length > 4) ? p.M_telepon2.Equals(xtelp4) : 1 == 1)
                );

            if (d.Count() > 0)
            {
                mode = 3;

                isMemberExists = d.Any(p => p.M_typemember != null && p.M_typemember != "");

                string kodelokasi = "";

                int UserID = App.UserID;
                var y = from p in db.DataAdminSales
                        where p.IDUser == UserID
                        select p;
                if (y.Count() > 0)
                {
                    var x = y.First();
                    if (x.TipeLokasi == 1) { kodelokasi = db.LocOutlet.SingleOrDefault(p => p.ID == x.IDLokasi).Kode; }
                    else if (x.TipeLokasi == 2) { kodelokasi = db.LocExhibition.SingleOrDefault(p => p.ID == x.IDLokasi).Kode; }

                }

                //is open all condition for new form customer
                flagNewFormCustomer = true;

                if (d.Count() > 10 && isMemberExists)
                {
                    d = d.Where(p => p.M_typemember != "" && p.M_typemember != null);

                    if (d.Count() > 10)
                    {
                        msg = "Sistem mendeteksi No HP <b>" + xtelp1 + "</b> diatas terdapat lebih dari 10 data dan lebih dari 10 membership, silahkan melakukan pengecekan kembali atau mengisi form customer baru.";
                        flagNewFormCustomer = true;
                    }
                    else
                    {
                        msg = "Sistem mendeteksi No HP <b>" + xtelp1 + "</b> diatas terdapat lebih dari 10 data dan kurang dari 10 membership, data ditampilkan hanya customer yang memiliki membership :";

                    }
                }
                else if (d.Count() > 10)
                {
                    msg = "Sistem mendeteksi No HP <b>" + xtelp1 + "</b> diatas terdapat lebih dari 10 data, silahkan melakukan pengecekan kembali atau mengisi form customer baru.";
                    flagNewFormCustomer = true;
                }
                else
                {
                    msg = "Sistem mendeteksi No HP <b>" + xtelp1 + "</b> diatas terdaftar dengan data sebagai berikut :";
                }


                if (d.Count() <= 10)
                {
                    msg += "<br /><br />";
                    msg += "<div class='row'>";
                    msg += "<div class='col-xs-12 col-sm-12 col-md-12'>";
                    msg += "<table style='width:100%;'>";

                    foreach (var r in d)
                    {
                        isMemberCode = false;

                        if (r.M_typemember != "" && r.M_typemember != null)
                        {
                            isMemberCode = true;
                        }

                        string telp1 = (r.M_telepon1 != null) ? r.M_telepon1.Trim() : "";
                        string telp2 = (r.M_telepon2 != null) ? r.M_telepon2.Trim() : "";

                        msg += "<tr>";

                        msg += "<td style='width:50px; padding:5px;'>";
                        msg += TD_HyperLink(r, isMemberExists, isMemberCode);
                        msg += "</td>";


                        msg += "<td style='width:230px; padding:5px;'>";
                        msg += "<span class='text-muted pull-left' style='text-align:left;'>" + r.M_kode + "</span>"
                            + ((r.M_member != null)
                            ? ("<span class='text-muted pull-right' style='text-align:left; color:green;'>" + r.M_member + "</span>") : "")
                            + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
                            + "<span class='pull-left' style='text-align:left;'><b>" + r.M_nama + "</b></span>";
                        msg += "</td>";


                        msg += "<td style='padding:5px;'>";
                        msg += (r.M_telepon1 != null ? ((kodelokasi.Trim() != r.M_cabang.Trim() && telp1.Length > 3) ? telp1.Remove(telp1.Length - 3) + "***" : telp1.Trim()) : "")
                                + "<br />"
                                + (r.M_telepon2 != null ? ((kodelokasi.Trim() != r.M_cabang.Trim() && telp2.Length > 3) ? telp2.Remove(telp2.Length - 3) + "***" : telp2.Trim()) : "")
                                ;
                        msg += "</td>";

                        msg += "</tr>";
                    }
                    msg += "</table>";
                    msg += "</div>";
                    msg += "</div>";
                    msg += "<div class='row'>";
                    msg += "<div class='col-xs-12 col-sm-12 col-md-12'>";
                    msg += "<br />Silakan memilih salah satu data diatas atau mengisi form customer baru.";
                    msg += "</div>";
                    msg += "</div>";


                }


            }
            else
            {
                flagNewFormCustomer = true;
                msg = "No HP <b>" + xtelp1 + "</b> tidak ditemukan dalam sistem, silahkan menginput form data customer baru";
                mode = 2;
            }

            //lit_warning_notelphp
            if (flagNewFormCustomer)
            {
                btn_new_cust_form.Visible = true;
            }

        }
        else
        {
            mode = 1;
            msg = "Format nomor Handphone tidak valid !!!";
        }
        process_addcust2(mode, msg);
    }

    protected void process_addcust1()
    {
        div_cust_agama.Visible = false;
        div_cust_alamat.Visible = false;
        div_cust_email.Visible = false;
        div_cust_kota.Visible = false;
        div_cust_nama.Visible = false;
        div_cust_tgllahir.Visible = false;

        customerlist.Visible = false;
        addcustomer.Visible = true;
        addrnotelp1.Focus();
        save.Enabled = false;
        save.CssClass = "btn btn-primary disabled";
    }

    protected void process_addcust2(int mode, string msg, bool invisible = true)
    {
        lit_message.Text = "";
        customerlist.Visible = false;
        addcustomer.Visible = true;
        addrnotelp1.Focus();
        save.Enabled = false;
        save.CssClass = "btn btn-primary disabled";
        string xmode = "default";

        if (invisible)
        {
            div_cust_agama.Visible = false;
            div_cust_alamat.Visible = false;
            div_cust_email.Visible = false;
            div_cust_kota.Visible = false;
            div_cust_nama.Visible = false;
            div_cust_tgllahir.Visible = false;

        }
        else
        {
            div_cust_agama.Visible = true;
            div_cust_alamat.Visible = true;
            div_cust_email.Visible = true;
            div_cust_kota.Visible = true;
            div_cust_nama.Visible = true;
            div_cust_tgllahir.Visible = true;

            btn_new_cust_form.Visible = false;
            lit_msg_nomor_hp.Text = "";
            save.Enabled = true;
            save.CssClass = "btn btn-primary";

        }
        if (mode == 1)
        {
            xmode = "danger";
        }
        else if (mode == 2)
        {
            xmode = "success";

        }
        else if (mode == 3)
        {
            xmode = "warning";

        }

        if (mode != 0)
        {
            lit_msg_nomor_hp.Text = "<div class='alert alert-" + xmode + "' role='alert'>" + msg + "</div>";
        }
    }

    protected void AddCustomer_Click(object sender, EventArgs e)
    {
        process_addcust1();
    }

    //btn_new_cust_form_Click
    protected void btn_new_cust_form_Click(object sender, EventArgs e)
    {
        process_addcust2(0, "", false);
    }

    protected void Cancel_Click(object sender, EventArgs e)
    {
        customerlist.Visible = true;
        addcustomer.Visible = false;
    }

    protected void fillForm()
    {
        // Transaction Table
        if (!checkValidID()) DataError();
        else
        {
            cancel.HRef = myModule + "_Form.aspx";
            cancel.HRef += "?id=" + myID.ToString();
            cancel.HRef += "&status=1";
        }
    }

    protected void save_Click(object sender, EventArgs e)
    {
        string Kode = "NONE";
        int IDBrand = 0;

        try
        {
            Kode = getLocation(1);
            IDBrand = Convert.ToInt32(getLocation(2));
        }
        catch (Exception ex) { }

        string HariLahir = "";
        string BulanLahir = "";
        string TahunLahir = "";

        // validation string kosong
        string msg = "";
        if (nama.Text.Trim() == "") { msg = "Nama harus diisi."; }
        else if (addralamat.Text.Trim() == "") { msg = "Alamat harus diisi."; }
        else if (addrnotelp1.Text.Trim() == "") { msg = "No. Handphone harus diisi."; }

        // validation jumlah karakter minimal
        if (msg == "" && nama.Text.Trim().Count() < 3) { msg = "Nama harus lebih dari 2 karakter."; }
        else if (msg == "" && addralamat.Text.Trim().Count() < 3) { msg = "Alamat harus lebih dari 2 karakter."; }
        else if (msg == "" && addrnotelp1.Text.Trim().Count() < 3) { msg = "No. Handphone harus lebih dari 2 karakter."; }

        // validation kota
        if (msg == "" && kota.SelectedIndex == 0) { msg = "Kota harus dipilih."; }

        //// validation agama
        //if (msg == "" && agama.SelectedIndex == 0) { msg = "Agama harus dipilih."; }

        // validation email
        if (msg == "" && email.Text.Trim() != "")
        {
            if (!Customer.isValidEmail(email.Text.Trim(), 2))
            //if (!IsValidEmail(email.Text.Trim()))
            {
                msg = "Format Email salah.";
            }
        }

        // validation date
        try
        {
            if (tgllahir.Text != "")
            {
                DateTime TglLahir = Convert.ToDateTime(tgllahir.Text);
                HariLahir = TglLahir.Day.ToString();
                BulanLahir = TglLahir.Month.ToString();
                TahunLahir = TglLahir.Year.ToString();
            }
            else { msg = "Tanggal harus diisi."; }
        }
        catch { msg = "Format tanggal tidak valid."; }

        if (msg == "")
        {
            string xnama = nama.Text.Trim();
            string xalamat = addralamat.Text.Trim();
            string xkota = kota.SelectedValue.Trim();
            string xtelp = addrnotelp1.Text.Trim();
            string xemail = email.Text.Trim();
            DateTime xtgllahir = Convert.ToDateTime(tgllahir.Text.Trim());
            string xagama = agama.SelectedValue.Trim();
            string xcabang = Kode;

            msg = Customer.Customer_Create_Dbcmk(myModule, myID, xnama, xalamat, xkota, xtelp, xemail, xtgllahir, xagama, xcabang);

            if (msg == "")
            {
                //Redirect
                Response.Redirect(myModule + "_Form.aspx?id=" + myID + "&status=1");
            }
            else
            {
                lit_message.Text = msg;
            }

        }
        else
        {
            lit_message.Text = "<span style='color:red;'>" + msg + "</span>";
        }
    }



}