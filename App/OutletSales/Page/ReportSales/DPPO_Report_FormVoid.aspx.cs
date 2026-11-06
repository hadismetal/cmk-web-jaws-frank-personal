using System;
using System.Linq;

public partial class Page_ReportSales_DPPO_Report_FormVoid : System.Web.UI.Page
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
    string Module = "pos"; // pos, acc

    protected int myID { get { return App.QueryInt(this, "id"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            keterangan.Focus();

            //Form
            fillForm();
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("DPPO_Report.aspx"); }

    //Form
    protected void fillForm()
    {
        
        bool isValidVoid = true;
        string msg = "";

        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.StatusDP == 0 && p.Draft == false);
        if (d == null) DataError();
        else
        {
            so.InnerHtml = d.NomorDP;
            so.Style.Add("color","red");

        //    if (d.StatusVoid == true)
        //    {
        //        // kondisi sudah tervoid
        //        lit_keteranganvoid.Text = "<br />" + d.KeteranganVoid.Trim();
        //        keterangan.Visible = false;
        //        isValidVoid = false;

        //        // kondisi jika saldo gift card belum terverifikasi untuk pengembalian saldo
        //        if (Payment.showVoidPaymentStatusGiftCard(d))
        //        {
        //            isValidVoid = false;
        //            voidgiftcard.Visible = true;
        //            msg += "Terdapat Pembayaran Gift Card yang tidak dapat terverifikasi. Silahkan melakukan verifikasi ulang dengan menekan tombol <b>Verifikasi</b>. Jika tombol tidak berfungsi, silahkan menghubungi IT Administrator.<br /><br />";
        //        }
        //    }
        //    else
        //    {
        //        isValidVoid = true;

        //        if (!VoidValidation.validVoidSalesOrderRole(d, Module)) 
        //        {
        //            div_keterangan_void.Visible = false;
        //            isValidVoid = false;
        //            msg += "<br />" + "Void tidak dapat diteruskan karena sudah lewat hari.";
        //        }
        //        if (!VoidValidation.validVoidSalesOrderNoResell(d))
        //        {
        //            div_keterangan_void.Visible = false;
        //            isValidVoid = false;
        //            msg += "<br />" + "Void tidak dapat diteruskan karena terdapat PLU yang sudah di resell / tradein.";
        //        }
        //        if (!VoidValidation.validVoidSalesOrderNoVoucherExpire(d))
        //        {
        //            div_keterangan_void.Visible = false;
        //            isValidVoid = false;
        //            msg += "<br />" + "Void tidak dapat diteruskan karena terdapat pembayaran menggunakan Voucher yang sudah expire.";
        //        }
        //        if (!VoidValidation.validVoidSalesOrderNoMoreThanOneYear(d))
        //        {
        //            div_keterangan_void.Visible = false;
        //            isValidVoid = false;
        //            msg += "<br />" + "Void tidak dapat diteruskan karena sudah lewat dari 1 tahun.";
        //        }
        //    }
        }

        if (msg != "")
        {
            div_alert.Visible = true;
            msg = "<b>Warning !!!</b> " + msg;
            lit_warning.Text = msg;
        }

        save.Visible = (isValidVoid);
    }

    protected void back_Click(object sender, EventArgs e)
    {
        Response.Redirect("DPPO_Report.aspx");
    }

    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.StatusDP == 0 && p.Draft == false);
        if (d != null)
        {
            string txtKeterangan = keterangan.Text.Trim();

            //voidSMSCourtesy(d, txtKeterangan);
            //voidPayment(d);
            //voidSalesOrder(d, txtKeterangan);

            //d.Keterangan = (d.Keterangan != null) ? d.Keterangan + ", " + txtKeterangan : txtKeterangan;
            //d.TglVoid = DateTime.Today;
            d.StatusDP = 2;
            //d.Unpaid = d.TotalBayar;
            d.TotalBayar = 0;
            db.SubmitChanges();
			
			string tgl = DateTime.Now.ToString("yyyy-MM-dd");

            Db.Execute("UPDATE SalesReceiptDPPO SET TglVoid = '" + tgl + "', VoidKeterangan = '" + txtKeterangan + " (Void by " + App.UserNama + ", " + Str.FullDateJam(System.DateTime.Now) + ")' WHERE ID = " + myID.ToString(), Db.Cnn_DB);

        }

        Response.Redirect("DPPO_Report.aspx?d=1");
    }

   

    //protected void voidPayment(SalesOrder d)
    //{
    //    string result = "";

    //    result = Payment.voidSalesOrderPaymentAll(d);

    //    //return result;
    //}

    

    

}