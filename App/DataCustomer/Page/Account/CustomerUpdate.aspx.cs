using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Account_CustomerUpdate : System.Web.UI.Page
{
    private Map_DB _db;
    private Map_CS _cs;

    private Map_DB db
    {
        get
        {
            if (_db == null)
                _db = new Map_DB();
            return _db;
        }
    }

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
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            nama.Focus();

            //DLC
            //DLC.CompanyBrand(brand);
            //DLC.OutletExhibitionList(location);

            //Form
            fillForm();

            //Action
            save.Visible = Role.Operator;
            //App.ConfirmDelete(del, Role.Manager);
        }
    }
    protected void fillForm()
    {
        //string x_header = "";
        //string x_id_jaws = "";
        //string x_store_location = "";
        //string x_kode_customer = "";
        //string x_nama_customer = "";
        //string x_customer_brand = "";

        var d = db.DataCustomer.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            h1_st.Text = d.Nama;
            h1_id.Text = Str.ID_Account(d.ID);

            kode.Text = d.NoCustomer;
            nama.Text = d.Nama;
            brand.Text = d.CompanyBrand.Nama;
            tgl.Text = d.Tgl.ToString("dd-MM-yyyy");

            var cust_cmk = cs.Mscustomer.Where(p => p.M_kode == d.NoCustomer);

            string xCmkStatus = "";

            if (cust_cmk.Count() > 0)
            {
                if (d.Nama == cust_cmk.First().M_nama)
                {
                    string xdt1 = System.DateTime.Now.ToString("yyyy-MM-dd");
                    string xdt2 = d.Tgl.ToString("yyyy-MM-dd");

                    //lit_msg.Text = xdt1 + "<br />" + xdt2;
                    
                    // edit nama customer rule
                    if (xdt1 != xdt2)
                    {
                        save.Disabled = true;
                        nama.Enabled = false;
                        keterangan.Enabled = false;
                    }

                    // data cmk ditemukan
                    xCmkStatus  = "<div class='alert alert-success' role='alert'><div class='row'>"
                                + "<div class='col-xs-2 col-sm-2 col-md-1'><span style='font-size:34px;' class='glyphicon glyphicon-ok' aria-hidden='true'></span>&nbsp;&nbsp;&nbsp;</div>"
                                + "<div class='col-xs-10 col-sm-10 col-md-11'>"
                                + "<div class='col-xs-12 col-sm-12 col-md-12'>Myapps Status<br /><b>Active</b></div>"
                                + "<div class='col-xs-12 col-sm-12 col-md-6' style='border-top:solid 1px;'>Kode Customer<br /><b>" + cust_cmk.First().M_kode + "</b></div>"
                                + "<div class='col-xs-12 col-sm-12 col-md-6' style='border-top:solid 1px;'>Nama Customer<br /><b>" + cust_cmk.First().M_nama + "</b></div>"
                                + "</div></div></div>"
                                ;

                }
                else
                {
                    save.Disabled = true;
                    nama.Enabled = false;
                    keterangan.Enabled = false;

                    // data cmk ditemukan namun beda nama
                    xCmkStatus = "<div class='alert alert-danger' role='alert'><div class='row'>"
                                + "<div class='col-xs-2 col-sm-2 col-md-1'><span style='font-size:34px;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>&nbsp;&nbsp;&nbsp;</div>"
                                + "<div class='col-xs-10 col-sm-10 col-md-11'>"
                                + "<div class='col-xs-12 col-sm-12 col-md-12'>Myapps Status<br /><b>Nama tidak sama</b></div>"
                                + "<div class='col-xs-12 col-sm-12 col-md-6' style='border-top:solid 1px;'>Kode Customer<br /><b>" + cust_cmk.First().M_kode + "</b></div>"
                                + "<div class='col-xs-12 col-sm-12 col-md-6' style='border-top:solid 1px;'>Nama Customer<br /><b>" + cust_cmk.First().M_nama + "</b></div>"
                                + "</div></div></div>"
                                ;

                }
            }
            else
            {
                save.Disabled = true;
                nama.Enabled = false;
                keterangan.Enabled = false;

                var cust_cmk_merge = cs.Mscustomermerge.Where(p => p.M_kode == d.NoCustomer);
                if (cust_cmk_merge.Count() > 0)
                {
                    var rs_final = cs.Mscustomer.Single(p => p.M_kode == cust_cmk_merge.First().M_kodemerge);

                    // data cmk tidak ditemukan karena merge
                    xCmkStatus = "<div class='alert alert-info' role='alert'><div class='row'>"
                                + "<div class='col-xs-2 col-sm-2 col-md-1'><span style='font-size:34px;' class='glyphicon glyphicon-exclamation-sign' aria-hidden='true'></span>&nbsp;&nbsp;&nbsp;</div>"
                                + "<div class='col-xs-10 col-sm-10 col-md-11'>"
                                + "<div class='col-xs-12 col-sm-12 col-md-12'>Myapps Status<br /><b>Merge Customer</b></div>"
                                + "<div class='col-xs-12 col-sm-12 col-md-6' style='border-top:solid 1px;'>Kode Final<br /><b>" + cust_cmk_merge.First().M_kodemerge + "</b></div>"
                                + "<div class='col-xs-12 col-sm-12 col-md-6' style='border-top:solid 1px;'>Nama Customer<br /><b>" + rs_final.M_nama + "</b></div>"
                                ;

                    var rs_merge_report = from p in cs.Mscustomermerge where p.M_kodemerge == cust_cmk_merge.First().M_kodemerge select p;

                    xCmkStatus += "<div class='col-xs-12 col-sm-12 col-md-6' style='border-top:solid 1px;'>Kode Merge<br />";
                    foreach (var x in rs_merge_report)
                    {
                        xCmkStatus += "<b>" + x.M_kode + "</b><br />";
                    }
                    xCmkStatus += "</div>";

                    xCmkStatus += "<div class='col-xs-12 col-sm-12 col-md-6' style='border-top:solid 1px;'>Operator Merge<br /><b>" 
                                + cust_cmk_merge.OrderByDescending(p => p.M_tglmerge).First().M_usermerge
                                + "<br />" + Str.TglJam(cust_cmk_merge.OrderByDescending(p => p.M_tglmerge).First().M_tglmerge)
                                + "</b></div>";

                    xCmkStatus += "</div></div></div>";

                }
                else
                {
                    // data cmk tidak ditemukan
                    xCmkStatus = "<div class='alert alert-danger' role='alert'><div class='row'>"
                                + "<div class='col-xs-2 col-sm-2 col-md-1'><span style='font-size:34px;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>&nbsp;&nbsp;&nbsp;</div>"
                                + "<div class='col-xs-10 col-sm-10 col-md-11'>"
                                + "<div class='col-xs-12 col-sm-12 col-md-12'>Myapps Status<br /><b>Data tidak ditemukan</b></div>"
                                + "</div></div></div>"
                                ;
                }

                
            }

            lit_cmk_status.Text = xCmkStatus;

        }
    }

    //Save
    protected bool valid()
    {
        TextBox txt_nama = nama;

        bool status = false;

        if (System.Text.RegularExpressions.Regex.IsMatch(txt_nama.Text, "^[a-zA-Z]"))
        {
            status = true;
        }

        return status;

        //get { return true; }
        //get { return Fv.isDateTime(tgl, tgl_ERR); }
    }
    protected void save_Click(object sender, EventArgs e)
    {
        if (valid())
        {
            var d = db.DataCustomer.SingleOrDefault(p => p.ID == myID);
            if (d == null) App.DataNull(this);
            else
            {
                var cust_cmk = cs.Mscustomer.Where(p => p.M_kode == d.NoCustomer);

                if (cust_cmk.Count() > 0)
                {
                    // data ditemukan

                    try
                    {
                        Db.Execute("EXEC sp_updatecustomer_allbrand "
                            + "@kodecust = N'" + d.NoCustomer + "'"
                            + "," + "@namacust = N'" + nama.Text.Trim() + "'"
                            + "," + "@operator = N'" + (App.UserID.ToString() + "#" + App.UserNama) + "'"
                        , Db.Cnn_CS);
                    }
                    catch { }

                }

                //d.Nama = Str.CapTitle(nama.Text);
                //d.Keterangan = Str.CapFirst(keterangan.Text);
                
                //db.SubmitChanges();

                //Redirect
                App.Reload(this, myID);
            }
        }
    }
    //protected void del_Click(object sender, EventArgs e)
    //{
    //    var d = db.DataCustomer.SingleOrDefault(p => p.ID == myID);
    //    if (d == null) App.DataNull(this);
    //    else
    //    {
    //        try
    //        {
    //            //Commit
    //            db.DataCustomer.DeleteOnSubmit(d);
    //            db.SubmitChanges();

    //            //Appendix
    //            AP.Delete("DataCustomer", myID);

    //            //Redirect
    //            App.ReloadClose(this);
    //        }
    //        catch { nodel.Visible = true; }
    //    }
    //}

    //protected string Generate_Nomor(string Kode)
    //{
    //    string x = string.Empty;

    //    int Tahun = DateTime.Today.Year;
    //    int Bulan = DateTime.Today.Month;

    //    /*int Nomor = db.StockReceiveDJ.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
    //    Nomor = Nomor + 1;

    //    return Str.KodeFormat(Tahun, Bulan, Nomor, 4);*/
    //    int Nomor = 1;

    //    try
    //    {
    //        /*var s = (from p in db.DataCustomer
    //                 where p.Tgl.Year == Tahun 
    //                 && p.NoCustomer != null
    //                 orderby p.NoCustomer.Remove(3).EndsWith(Kode) descending
    //                 select p).First();*/

    //        Nomor = Db.SingleInteger("SELECT TOP 1 CAST(RIGHT(NoCustomer,6) AS INT) FROM DataCustomer WHERE NoCustomer is not null "
    //                    + "AND LEFT(NoCustomer,3) = '" + Kode + "'"
    //                    + "ORDER BY RIGHT(NoCustomer,6) DESC", Db.Cnn_DB);

    //        //if (s.NoCustomer != null)
    //        Nomor = Nomor + 1;
    //    }
    //    catch (Exception ex) { }

    //    string PLU = Kode + Tahun.ToString().Remove(0, 2) + Str.KodeFormat(Nomor, 6);

    //    return PLU;

    //}
}