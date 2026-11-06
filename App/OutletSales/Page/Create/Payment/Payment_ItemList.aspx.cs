using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_Payment_ItemList : System.Web.UI.Page
{
    string Module = "pos"; // pos, acc

    #region NavTab

    protected string Active_CreditCard { get; set; }
    protected string Active_DebitCard { get; set; }
    protected string Active_Cash { get; set; }
    protected string Active_DownPayment { get; set; }
    protected string Active_Transfer { get; set; }
    protected string Active_Voucher { get; set; }
    protected string Active_GiftCard { get; set; }
    protected string Active_BrideStoryPay { get; set; }
    protected string Active_LakuEmas { get; set; } 
    protected string Active_CashLEI { get; set; } 
    protected string Active_QRIS { get; set; }
    protected string Active_Xendit { get; set; }
    protected string Active_Ecommerce { get; set; }
    protected string Active_CashA { get; set; }

    protected string Tab_CreditCard { get; set; }
    protected string Tab_DebitCard { get; set; }
    protected string Tab_Cash { get; set; }
    protected string Tab_DownPayment { get; set; }
    protected string Tab_Transfer { get; set; }
    protected string Tab_Voucher { get; set; }
    protected string Tab_GiftCard { get; set; }
    protected string Tab_BrideStoryPay { get; set; }
    protected string Tab_LakuEmas { get; set; } 
    protected string Tab_CashLEI { get; set; }
    protected string Tab_QRIS { get; set; }
    protected string Tab_Xendit { get; set; }
    protected string Tab_Ecommerce { get; set; }
    protected string Tab_CashA { get; set; }

    protected string Href_CreditCard { get; set; }
    protected string Href_DebitCard { get; set; }
    protected string Href_Cash { get; set; }
    protected string Href_DownPayment { get; set; }
    protected string Href_Transfer { get; set; }
    protected string Href_Voucher { get; set; }
    protected string Href_GiftCard { get; set; }
    protected string Href_BrideStoryPay { get; set; }
    protected string Href_LakuEmas { get; set; }
    protected string Href_CashLEI { get; set; } 
    protected string Href_QRIS { get; set; }
    protected string Href_Xendit { get; set; }
    protected string Href_Ecommerce { get; set; }
    protected string Href_CashA { get; set; }

    protected void active_CreditCard(bool isActive = true) { Active_CreditCard = isActive ? "active" : ""; }
    protected void active_DebitCard(bool isActive = true) { Active_DebitCard = isActive ? "active" : ""; }
    protected void active_Cash(bool isActive = true) { Active_Cash = isActive ? "active" : ""; }
    protected void active_DownPayment(bool isActive = true) { Active_DownPayment = isActive ? "active" : ""; }
    protected void active_Transfer(bool isActive = true) { Active_Transfer = isActive ? "active" : ""; }
    protected void active_Voucher(bool isActive = true) { Active_Voucher = isActive ? "active" : ""; }
    protected void active_GiftCard(bool isActive = true) { Active_GiftCard = isActive ? "active" : ""; }
    protected void active_BrideStoryPay(bool isActive = true) { Active_BrideStoryPay = isActive ? "active" : ""; }
    protected void active_LakuEmas(bool isActive = true) { Active_LakuEmas = isActive ? "active" : ""; } 
    protected void active_CashLEI(bool isActive = true) { Active_CashLEI = isActive ? "active" : ""; }
    protected void active_QRIS(bool isActive = true) { Active_QRIS = isActive ? "active" : ""; }
    protected void active_Xendit(bool isActive = true) { Active_Xendit = isActive ? "active" : ""; }
    protected void active_Ecommerce(bool isActive = true) { Active_Ecommerce = isActive ? "active" : ""; }
    protected void active_CashA(bool isActive = true) { Active_CashA = isActive ? "active" : ""; }

    protected void enable_CreditCard(bool isEnable = true)
    {
        Active_CreditCard = isEnable ? "" : "disabled";
        Tab_CreditCard = isEnable ? "tab" : "";
        Href_CreditCard = isEnable ? "href='##creditcard'" : "";
    }
    protected void enable_DebitCard(bool isEnable = true)
    {
        Active_DebitCard = isEnable ? "" : "disabled";
        Tab_DebitCard = isEnable ? "tab" : "";
        Href_DebitCard = isEnable ? "href='##debitcard'" : "";
    }
    protected void enable_Cash(bool isEnable = true)
    {
        Active_Cash = isEnable ? "" : "disabled";
        Tab_Cash = isEnable ? "tab" : "";
        Href_Cash = isEnable ? "href='##cash'" : "";
    }

    protected void enable_CashA(bool isEnable = true)
    {
        Active_CashA = isEnable ? "" : "disabled";
        Tab_CashA = isEnable ? "tab" : "";
        Href_CashA = isEnable ? "href='##casha'" : "";
    }
    protected void enable_DownPayment(bool isEnable = true)
    {
        Active_DownPayment = isEnable ? "" : "disabled";
        Tab_DownPayment = isEnable ? "tab" : "";
        Href_DownPayment = isEnable ? "href='##downpayment'" : "";
    }
    protected void enable_Transfer(bool isEnable = true)
    {
        Active_Transfer = isEnable ? "" : "disabled";
        Tab_Transfer = isEnable ? "tab" : "";
        Href_Transfer = isEnable ? "href='##transfer'" : "";
    }
    protected void enable_Voucher(bool isEnable = true)
    {
        Active_Voucher = isEnable ? "" : "disabled";
        Tab_Voucher = isEnable ? "tab" : "";
        Href_Voucher = isEnable ? "href='##voucher'" : "";
    }
    protected void enable_GiftCard(bool isEnable = true)
    {
        Active_GiftCard = isEnable ? "" : "disabled";
        Tab_GiftCard = isEnable ? "tab" : "";
        Href_GiftCard = isEnable ? "href='##giftcard'" : "";
    }
    protected void enable_BrideStoryPay(bool isEnable = true)
    {
        Active_BrideStoryPay = isEnable ? "" : "disabled";
        Tab_BrideStoryPay = isEnable ? "tab" : "";
        Href_BrideStoryPay = isEnable ? "href='##bridestorypay'" : "";
    }
    //Tambahan By Andri Priyono Cara Pembayaran Laku Emas
    protected void enable_LakuEmas(bool isEnable = true)
    {
        Active_LakuEmas = isEnable ? "" : "disabled";
        Tab_LakuEmas = isEnable ? "tab" : "";
        Href_LakuEmas = isEnable ? "href='##lakuemas'" : "";
    }
    
    protected void enable_CashLEI(bool isEnable = true)
    {
        Active_CashLEI = isEnable ? "" : "disabled";
        Tab_CashLEI = isEnable ? "tab" : "";
        Href_CashLEI = isEnable ? "href='##cashlei'" : "";
    }

    protected void enable_QRIS(bool isEnable = true)
    {
        Active_QRIS = isEnable ? "" : "disabled";
        Tab_QRIS = isEnable ? "tab" : "";
        Href_QRIS = isEnable ? "href='##qris'" : "";
    }

    protected void enable_Xendit(bool isEnable = true)
    {
        Active_Xendit = isEnable ? "" : "disabled";
        Tab_Xendit = isEnable ? "tab" : "";
        Href_Xendit = isEnable ? "href='##xendit'" : "";
    }

    protected void enable_Ecommerce(bool isEnable = true)
    {
        Active_Ecommerce = isEnable ? "" : "disabled";
        Tab_Ecommerce = isEnable ? "tab" : "";
        Href_Ecommerce = isEnable ? "href='##ecommerce'" : "";
    }

    #endregion

    private Map_DB _db;
    private Map_CS _cs;

    // lazy properties — .NET 4 compatible
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
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        div_unfinished.Visible = false;
        div_finished.Visible = false;
        redirect_summary.Visible = false;

        var x = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (x != null)
        {
            bool isNotSameDay = false;

            if (!x.Draft && !x.StatusVoid.Value && !x.TglVoid.HasValue)
            {
                div_unfinished.Visible = true;
                div_finished.Visible = false;
                redirect_summary.Visible = false;
                bool isEnabledLakuEmas = false;
                if (!IsPostBack)
                {
                    if (Request.QueryString["alert"] == "dpinvalid")
                    {
                        ScriptManager.RegisterStartupScript(
                            this,
                            GetType(),
                            "alertErr",
                            "toastr.options = { positionClass: 'toast-top-center', timeOut: 0, extendedTimeOut: 0, closeButton: true }; toastr.error('DP Tidak Dapat Digunakan');",
                            true
                        );
                    }

                    if (Request.QueryString["alert"] == "dpinvalidr")
                    {
                        string dp = Request.QueryString["dp"];

                        ScriptManager.RegisterStartupScript(
                            this,
                            GetType(),
                            "alertErr",
                            "toastr.options = { positionClass: 'toast-top-center', timeOut: 0, extendedTimeOut: 0, closeButton: true }; toastr.error('DP Tidak Dapat Digunakan, milik " + dp.Replace("'", "\\'") + "');",
                            true
                        );
                    }



                    ddl_creditcard_edc.Items.Add(new ListItem("", "0"));
                    ddl_creditcard_bankissuer.Items.Add(new ListItem("", "0"));

                    ddl_debitcard_edc.Items.Add(new ListItem("", "0"));
                    ddl_debitcard_bankissuer.Items.Add(new ListItem("", "0"));

                    DLC.CardTypeList(ddl_creditcard_cardtype);
                    DLC.JenisKartu(ddl_creditcard_cardcategory);
                    //DLC.EDCList(ddl_creditcard_edc);
                    EDCListCredit(ddl_creditcard_edc);
                    BankList(ddl_creditcard_bankissuer);
                    DLC.ProgramList(ddl_creditcard_bankprogram);
                    DLC.ProgramList(ddl_creditlakuemas_bankprogram);
                    DLC.CardTypeList(ddl_debitcard_cardtype);
                    DLC.CardTypeList(ddl_debitlakuemas_cardtype);
                    DLC.CardTypeList(ddl_creditlakuemas_cardtype);
                    DLC.JenisKartu(ddl_debitcard_cardcategory);
                    DLC.JenisKartu(ddl_debitlakuemas_cardcategory);
                    DLC.JenisKartu(ddl_creditlakuemas_cardcategory);
                    //DLC.EDCList(ddl_debitcard_edc);
                    //if(IsPGOnly(x)){
                    EDCListDebit(ddl_debitcard_edc, false);
                    //}
                    //else{
                    //EDCListDebit(ddl_debitcard_edc, false);
                    //}
                    BankList(ddl_debitcard_bankissuer);
                    BankList(ddl_debitlakuemas_bankissuer);
                    BankList(ddl_creditlakuemas_bankissuer);
                    BankListTransfer(ddl_transfer_bankissuer);
                    BankList(ddl_bridestorypay_bankissuer);
                    QRISList(ddl_qris_cardtype);
                    QRISBankList(ddl_qris_bankissuer);
                    BankList(ddl_xendit_bankissuer);
                    QRISLEIList(ddl_qrislakuemas_cardtype);
                    QRISBankListLEI(ddl_qrislakuemas_bankissuer);
                    EcommercePlatformList(ddl_payment_ecommerce_platform);

                    ddl_creditcard_cardtype.SelectedValue = "3";
                    ddl_creditcard_cardcategory.SelectedValue = "3";
                    ddl_creditcard_edc.SelectedValue = "1";
                    ddl_creditcard_bankissuer.SelectedValue = "1";
                    ddl_creditcard_bankprogram.SelectedValue = "1";
                    ddl_debitcard_cardtype.SelectedValue = "2";
                    ddl_debitcard_cardcategory.SelectedValue = "1";
                    ddl_debitcard_edc.SelectedValue = "1";
                    ddl_debitcard_bankissuer.SelectedValue = "1";
                    ddl_debitlakuemas_cardtype.SelectedValue = "2";
                    ddl_debitlakuemas_cardcategory.SelectedValue = "1";
                    //ddl_debitcard_edc.SelectedValue = "1";
                    ddl_debitlakuemas_bankissuer.SelectedValue = "1";
                    ddl_creditlakuemas_cardtype.SelectedValue = "2";
                    ddl_creditlakuemas_cardcategory.SelectedValue = "1";
                    //ddl_debitcard_edc.SelectedValue = "1";
                    ddl_creditlakuemas_bankissuer.SelectedValue = "1";
                    ddl_creditlakuemas_bankprogram.SelectedValue = "1";
                    ddl_debitlakuemas_cardcategory.SelectedValue = "1";
                    ddl_transfer_bankissuer.SelectedValue = "1";
                    ddl_bridestorypay_bankissuer.SelectedValue = "1";
                    ddl_xendit_bankissuer.SelectedValue = "1";

                    decimal Unpaid = x.Unpaid.Value;

                    decimal AbsUnpaid = (Unpaid < 0) ? (-1 * Unpaid) : Unpaid;

                    // Semua harus terbayar di hari yang sama
                    if (Convert.ToDateTime(x.Tgl).Date < System.DateTime.Now.Date) { isNotSameDay = true; }

                    /// LEBIH BAYAR HANYA BISA UNTUK DOWN PAYMENT DAN PENGEMBALIAN HANYA BISA CASH OUT (BKK)
                    txt_creditcard_amount.Text = (isNotSameDay || Unpaid < 0) ? "0" : AbsUnpaid.ToString("N0");
                    txt_debitcard_amount.Text = (isNotSameDay || Unpaid < 0) ? "0" : AbsUnpaid.ToString("N0");
                    txt_cash_amount.Text = (isNotSameDay) ? "0" : AbsUnpaid.ToString("N0");
                    txt_transfer_amount.Text = (isNotSameDay || Unpaid < 0) ? "0" : AbsUnpaid.ToString("N0");
                    txt_bridestorypay_amount.Text = (isNotSameDay || Unpaid < 0) ? "0" : AbsUnpaid.ToString("N0");
                    txt_giftcard_amount.Text = (isNotSameDay || Unpaid < 0) ? "0" : AbsUnpaid.ToString("N0");
                    txt_cashlei_amount.Text = (isNotSameDay || Unpaid < 0) ? "0" : AbsUnpaid.ToString("N0");
                    txt_qris_amount.Text = (isNotSameDay || Unpaid < 0) ? "0" : AbsUnpaid.ToString("N0");
                    txt_xendit_amount.Text = (isNotSameDay || Unpaid < 0) ? "0" : AbsUnpaid.ToString("N0");
                    txt_qrislakuemas_amount.Text = (isNotSameDay || Unpaid < 0) ? "0" : AbsUnpaid.ToString("N0");
                    txt_casha_amount.Text = (isNotSameDay || Unpaid < 0) ? "0" : AbsUnpaid.ToString("N0");

                    if (IsPGOnly(x))
                    {
                        isEnabledLakuEmas = DifferencePGLMOngkos(isNotSameDay, Unpaid, AbsUnpaid, x);
                        isEnabledLakuEmas = CountTotalPaymentWithLEI(myID);
                    }
                    txt_debitlakuemas_amount.Text = (isNotSameDay || Unpaid < 0) ? "0" : AbsUnpaid.ToString("N0");
                    txt_creditlakuemas_amount.Text = (isNotSameDay || Unpaid < 0) ? "0" : AbsUnpaid.ToString("N0");
                    txt_ecommerce_amount.Text = (isNotSameDay || Unpaid < 0) ? "0" : AbsUnpaid.ToString("N0");
                    //txt_debitlakuemas_amount.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    //txt_debitlakuemas_amount.AutoPostBack = true;
                    //txt_debitlakuemas_amount.TextChanged += new EventHandler(onChangeAmountDebitLakuEmas);

                    string kodecust = x.DataCustomer.NoCustomer;
                    var cust = cs.Mscustomer.SingleOrDefault(p => p.M_kode == kodecust);

                    if (cust != null)
                    {
                        txt_qris_custname.Text = cust.M_nama;
                        txt_qris_custphonenumber.Text = cust.M_telepon1;
                        txt_qrislakuemas_custname.Text = cust.M_nama;
                        txt_qrislakuemas_custphonenumber.Text = cust.M_telepon1;
                    }
                }

                save_payment_creditcard.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(save_payment_creditcard, null) + ";");
                save_payment_debitcard.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(save_payment_debitcard, null) + ";");
                save_payment_cash.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(save_payment_cash, null) + ";");
                save_payment_transfer.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(save_payment_transfer, null) + ";");
                save_payment_giftcard.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(save_payment_giftcard, null) + ";");
                save_payment_bridestorypay.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(save_payment_bridestorypay, null) + ";");
                save_payment_casha.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(save_payment_casha, null) + ";");

                fillForm(isNotSameDay);

                if (!isNotSameDay && x.Unpaid.Value > 0)
                {
                    bool ispgonly = IsPGOnly(x);
                    int countpay = Db.SingleInteger("SELECT COUNT('') [Total] FROM SalesOrder A JOIN SalesReceipt B ON A.ID = B.IDSalesOrder JOIN SalesReceiptDetail C ON B.ID = C.IDSalesReceipt WHERE A.ID = " + myID + " AND VoidReference IS NULL", Db.Cnn_DB);
                    if (isEnabledLakuEmas == true && countpay > 0)
                    {
                        enable_LakuEmas(true);
                        enable_DownPayment(false);
                        enable_Voucher(false);
                        enable_GiftCard(false);
                        enable_CreditCard(false);
                        enable_DebitCard(false);
                        enable_Transfer(false);
                        enable_BrideStoryPay(false);
                        enable_Cash(true);
                        //enable_CashLEI(true);
                        enable_QRIS(false);
                        enable_Xendit(false);
                        enable_Ecommerce(false);
                        enable_CashA(false);
                    }
                    else if (isEnabledLakuEmas == false && countpay > 0)
                    {
                        enable_LakuEmas(false);
                        enable_DownPayment(true);
                        enable_Voucher(true);
                        enable_GiftCard(false);
                        enable_CreditCard(true);
                        enable_DebitCard(true);
                        enable_Transfer(true);
                        enable_BrideStoryPay(true);
                        enable_Cash(true);
                        //enable_CashLEI(false);
                        enable_QRIS(true);
                        enable_Xendit(true);
                        enable_Ecommerce(true);
                        enable_CashA(true);
                    }
                    else
                    {
                        if (ispgonly == true)
                        {
                            enable_LakuEmas(true);
                            //enable_CashLEI(true);
                        }
                        else
                        {
                            enable_LakuEmas(false);
                            //enable_CashLEI(false);
                        }
                    }
                }
                //CheckDebitLEIOnly(isNotSameDay,x);
            }
            else
            {
                show_Invalid_Payment_IsVoid(x.Nomor, x.KeteranganVoid);
            }
        }
        //btn_add_payment_click(sender, e);
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (cs != null)
            cs.Dispose();
        base.Dispose();
    }

    protected void show_Invalid_Payment_IsVoid(string nomor, string keteranganvoid)
    {
        string xText = "<div class='alert alert-danger' role='alert'><div class='row'>"
                    + "<div class='col-xs-2 col-sm-2 col-md-1'><span style='font-size:34px;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>&nbsp;&nbsp;&nbsp;</div>"
                    + "<div class='col-xs-10 col-sm-10 col-md-11'>"
                    + "<div class='col-xs-12 col-sm-12 col-md-12'><b>Warning !!!</b> Anda tidak dapat mengakses halaman ini. Dokumen dengan nomor Sales Order # "
                    + "<b>" + nomor + "</b> dalam status <b>Void</b>.<br /><br />Keterangan Void :<br />" + keteranganvoid
                    + "</div>"
                    + "</div></div></div>"
                    ;

        lit_info_invalid.Text = xText;

        div_unfinished.Visible = false;
        div_finished.Visible = true;
        redirect_summary.Visible = true;
    }

    protected void show_Invalid_Payment_NotSameDay(SalesOrder x)
    {
        string xText = "<div class='alert alert-danger' role='alert'><div class='row'>"
                    + "<div class='col-xs-2 col-sm-2 col-md-1'><span style='font-size:34px;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>&nbsp;&nbsp;&nbsp;</div>"
                    + "<div class='col-xs-10 col-sm-10 col-md-11'>"
                    + "<div class='col-xs-12 col-sm-12 col-md-12'><b>Warning !!!</b> Anda tidak dapat mengakses halaman ini. Dokumen dengan nomor Sales Order # "
                    + "<b>" + x.Nomor + "</b> sudah lewat dari tanggal input transaksi. Tgl Input Transaksi : " + Str.FullDate(x.Tgl) + "."
                    + "</div>"
                    + "</div></div></div>"
                    ;

        lit_info_invalid.Text = xText;

        div_finished.Visible = true;
        redirect_payment_list.Visible = true;
    }

    //Exception
    protected void DataError() { Response.Redirect("Payment.aspx"); }

    //Form
    protected void fillForm(bool isNotSameDay = false)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            //tradein
            div_tradein.Visible = false;

            string ti_nomor = "";
            string ti_keterangan = "";

            string ro_nomor = "";
            string ro_keterangan = "";

            if (d.TradeIn.HasValue)
            {
                var ti = db.TradeIn.SingleOrDefault(p => p.IDSalesOrder == d.ID);
                if (ti != null)
                {
                    ti_nomor = "<span style='color:blue;'>" + ti.Nomor + "</span>";
                    ti_keterangan = ti.Keterangan;


                    var ro = db.Resell.SingleOrDefault(p => p.ID == ti.IDResell);
                    if (ro != null)
                    {
                        ro_nomor = "<span style='color:green;'>" + ro.Nomor + "</span>";
                        ro_keterangan = ro.Keterangan;
                    }
                    div_tradein.Visible = true;
                    header_tradein.InnerHtml = ti_nomor;

                }

            }

            ////////////////////////////////////////// eReceipt
            bool flag_eReceipt_released = true; // false -> not released, true -> released

            // validasi  jika ada pembayaran giftcard
            bool validGiftCard = true;
            validGiftCard = Payment.showPaymentStatusGiftCard(d);

            string errorGiftCard = "";
            string errorPayment = "";

            errorGiftCard = (validGiftCard) ? "" : ("Invalid Verification Gift Card Payment. Please resubmit Gift Card Payment by clicking the button below.");
            errorPayment = validSalesOrderPayment(Convert.ToDecimal(d.TotalBayar));

            if (errorGiftCard != "")
            {
                displayError_Giftcard(errorGiftCard);
                verifygiftcard.Visible = true;
            }

            if (errorPayment != "")
            {
                displayError_Payment(errorPayment);
            }

            // Kondisi untuk DP lebih bayar dan mau Cash Out
            if (isNotSameDay)
            {
                enable_CreditCard(false);
                enable_DebitCard(false);
                enable_Cash(false);
                enable_DownPayment(false);
                enable_Transfer(false);
                enable_Voucher(false);
                enable_GiftCard(false); // belum jalan per 2018-11-02
                enable_BrideStoryPay(false);
                enable_LakuEmas(false); //Tambahan by Andri Priyono
                enable_CashLEI(false);
                enable_QRIS(false);
                enable_Xendit(false);
                enable_Ecommerce(false);
                enable_CashA(false);

                rdo_cash_out.Enabled = false;
                rdo_cash_out.Checked = false;
                rdo_cash_in.Enabled = false;
                rdo_cash_in.Checked = false;

                rdo_cashlei_out.Enabled = false;
                rdo_cashlei_out.Checked = false;
                rdo_cashlei_in.Enabled = false;
                rdo_cashlei_in.Checked = false;
            }
            else
            {
                if (d.Unpaid < 0)
                {
                    enable_CreditCard(false);
                    enable_DebitCard(false);
                    enable_Cash(true);
                    enable_DownPayment(false);
                    enable_Transfer(false);
                    enable_Voucher(false);
                    enable_GiftCard(false); // belum jalan per 2018-11-02
                    enable_BrideStoryPay(false);
                    enable_LakuEmas(false); //Tambahan by Andri Priyono
                    enable_CashLEI(false);
                    enable_QRIS(false);
                    enable_Xendit(false);
                    enable_Ecommerce(false);

                    rdo_cash_out.Enabled = true;
                    rdo_cash_out.Checked = true;
                    rdo_cash_in.Enabled = false;
                    rdo_cash_in.Checked = false;

                    rdo_cashlei_out.Enabled = true;
                    rdo_cashlei_out.Checked = true;
                    rdo_cashlei_in.Enabled = false;
                    rdo_cashlei_in.Checked = false;
                }
                else if (d.Unpaid > 0)
                {
                    enable_CreditCard(true);
                    enable_DebitCard(true);
                    enable_Cash(true);
                    enable_DownPayment(true);
                    enable_Transfer(true);
                    enable_Voucher(true);
                    enable_GiftCard(false); // belum jalan per 2018-11-02
                    enable_BrideStoryPay(true);
                    enable_LakuEmas(true); //Tambahan by Andri Priyono
                    enable_CashLEI(false);
                    enable_QRIS(true);
                    enable_Xendit(true);
                    enable_Ecommerce(true);
                    enable_CashA(true);

                    rdo_cash_in.Enabled = true;
                    rdo_cash_in.Checked = true;
                    rdo_cash_out.Enabled = false;
                    rdo_cash_out.Checked = false;

                    rdo_cashlei_in.Enabled = true;
                    rdo_cashlei_in.Checked = true;
                    rdo_cashlei_out.Enabled = false;
                    rdo_cashlei_out.Checked = false;
                }
                else
                {
                    enable_CreditCard(false);
                    enable_DebitCard(false);
                    enable_Cash(false);
                    enable_DownPayment(false);
                    enable_Transfer(false);
                    enable_Voucher(false);
                    enable_GiftCard(false); // belum jalan per 2018-11-02
                    enable_BrideStoryPay(false);
                    enable_LakuEmas(false); //Tambahan by Andri Priyono
                    enable_CashLEI(false);
                    enable_QRIS(false);
                    enable_Xendit(false);
                    enable_Ecommerce(false);
                    enable_CashA(false);

                    rdo_cash_in.Enabled = false;
                    rdo_cash_in.Checked = false;
                    rdo_cash_out.Enabled = false;
                    rdo_cash_out.Checked = false;

                    rdo_cashlei_in.Enabled = false;
                    rdo_cashlei_in.Checked = false;
                    rdo_cashlei_out.Enabled = false;
                    rdo_cashlei_out.Checked = false;
                }
            }

            // permanent disable transfer out
            //rdo_transfer_in.Checked = true;
            //rdo_transfer_out.Enabled = false;

            // kondisi sudah lunas dan belum lunas
            if (d.Unpaid <= 0 || isNotSameDay)
            {
                // lunas

                // disable all credit card payment

                txt_creditcard_amount.Attributes["disabled"] = "disabled";
                txt_creditcard_cardholdername.Attributes["disabled"] = "disabled";
                txt_creditcard_cardholdernumber.Attributes["disabled"] = "disabled";
                txt_creditcard_approvalcode.Attributes["disabled"] = "disabled";

                ddl_creditcard_bankissuer.Enabled = false;
                ddl_creditcard_bankprogram.Enabled = false;
                ddl_creditcard_cardcategory.Enabled = false;
                ddl_creditcard_cardtype.Enabled = false;
                ddl_creditcard_edc.Enabled = false;
                save_payment_creditcard.Enabled = false;
                //save_payment_creditcard_mdr.Enabled = false;

                // disable all debit card payment

                txt_debitcard_amount.Attributes["disabled"] = "disabled";
                txt_debitcard_cardholdername.Attributes["disabled"] = "disabled";
                txt_debitcard_cardholdernumber.Attributes["disabled"] = "disabled";
                txt_debitcard_approvalcode.Attributes["disabled"] = "disabled";
                //txt_debitcard_mdr.Attributes["disabled"] = "disabled";

                ddl_debitcard_bankissuer.Enabled = false;
                ddl_debitcard_cardcategory.Enabled = false;
                ddl_debitcard_cardtype.Enabled = false;
                ddl_debitcard_edc.Enabled = false;
                save_payment_debitcard.Enabled = false;
                //save_payment_debitcard_mdr.Enabled = false;

                // disable cash
                if (d.Unpaid == 0)
                {
                    txt_cash_amount.Attributes["disabled"] = "disabled";
                    rdo_cash_in.Enabled = false;
                }

                // disable transfer
                txt_transfer_amount.Attributes["disabled"] = "disabled";
                txt_transfer_name.Attributes["disabled"] = "disabled";
                txt_transfer_number.Attributes["disabled"] = "disabled";

                ddl_transfer_bankissuer.Enabled = false;

                // disable bridestorypay
                txt_bridestorypay_amount.Attributes["disabled"] = "disabled";
                txt_bridestorypay_name.Attributes["disabled"] = "disabled";
                txt_bridestorypay_number.Attributes["disabled"] = "disabled";
                txt_bridestorypay_transactionnumber.Attributes["disabled"] = "disabled";

                ddl_bridestorypay_bankissuer.Enabled = false;
                //btn_bridestorypay_confirm.Enabled = false;

                // disable gift card
                txt_giftcard_amount.Attributes["disabled"] = "disabled";
                txt_giftcard_cardholdernumber.Attributes["disabled"] = "disabled";


            }

            string Membership = "Non-Member";

            if (STAMPS.xActivate)
            {
                try
                {
                    Membership = Db.SingleString("EXEC sp_get_membership_customer 'READ', '" + d.ID.ToString() + "' ", Db.Cnn_DB);
                }
                catch { }

                if (Membership == "-")
                {
                    var c = d.DataCustomer;
                    Membership = c.NoMember; // tiering membership update from STAMPS
                }
            }
            else
            {
                var c = cs.Mscustomer.SingleOrDefault(p => p.M_kode == d.DataCustomer.NoCustomer);
                if (c != null)
                {
                    var m = cs.Msmembership.SingleOrDefault(p => p.M_kode == c.M_typemember);

                    if (m != null) { Membership = m.M_nama + " (" + m.M_kode + ")"; }
                }
            }

            //Header
            header_nomor.InnerHtml = d.Nomor;

            //Trans. Form
            string Lokasi = "";
            string BrandCompany = "";
            if (d.TipeLokasi == 1)
            {
                var loc = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
                Lokasi = loc.Nama;
                BrandCompany = loc.CompanyBrand.Nama;
            }
            else if (d.TipeLokasi == 2)
            {
                var loc = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                Lokasi = loc.Nama;
                BrandCompany = loc.CompanyBrand.Nama;
            }

            brand.InnerHtml = BrandCompany;
            membership.InnerHtml = Membership;
            location.InnerHtml = Lokasi;
            keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
            jpjr.InnerHtml = d.DataSales.Nama;
            customer.InnerHtml = d.DataCustomer.Nama + " (" + d.DataCustomer.NoCustomer + ")";
            givento.InnerHtml = (String.IsNullOrEmpty(d.CustomerReferenceNama) ? d.DataCustomer.Nama : d.CustomerReferenceNama);
            operator_input.InnerHtml = d.Operator + " (" + Str.TglJam(d.DraftDate) + ")";
            tgl.InnerHtml = Str.Tgl(d.Tgl);

            //Stone
            fillItem();

            decimal NilaiSO = 0m;
            decimal Paid = 0m;
            decimal Unpaid = 0m;

            try
            {
                NilaiSO = d.TotalRupiah.Value;
            }
            catch (Exception) { }
            try
            {
                Paid = d.Paid.Value;
            }
            catch (Exception) { }
            try
            {
                Unpaid = d.Unpaid.Value;
            }
            catch (Exception) { }

            var inv_dj = from p in d.SalesOrderDJ select p;
            var inv_pg = from p in d.SalesOrderPG select p;
            var inv_gj = from p in d.SalesOrderGJ select p;
            var inv_ld = from p in d.SalesOrderLD select p;

            decimal totalDiscNormal = 0m;
            decimal totalDiscPromo = 0m;
            decimal totalDiscBank = 0m;
            decimal totalDiscMember = 0m;
            decimal totalDiscVoucher = 0m;
            decimal totalDiscOther = 0m;
            decimal totalDiscRound = 0m;
            decimal grandTotalDiscount = 0m;

            decimal nilaiResell = 0m;
            decimal nilaiHarusDibayar = 0m;

            try
            {
                nilaiResell = d.TotalResell.HasValue ? d.TotalResell.Value : 0m;
            }
            catch (Exception) { }

            try
            {
                nilaiHarusDibayar = d.TotalBayar.HasValue ? d.TotalBayar.Value : 0m;
            }
            catch (Exception) { }

            if (inv_dj.Count() > 0)
            {
                totalDiscNormal += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountNominal));
                totalDiscPromo += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountPromo));
                totalDiscBank += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountBank));
                totalDiscMember += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountProgramNominal));
                totalDiscVoucher += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountGift));
                totalDiscOther += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountOther));
                totalDiscRound += Convert.ToDecimal(inv_dj.Sum(p => p.DiscountRound));
            }
            if (inv_pg.Count() > 0)
            {
                totalDiscNormal += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountNominal));
                totalDiscPromo += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountPromo));
                totalDiscBank += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountBank));
                totalDiscMember += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountProgramNominal));
                totalDiscVoucher += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountGift));
                totalDiscOther += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountOther));
                totalDiscRound += Convert.ToDecimal(inv_pg.Sum(p => p.DiscountRound));
            }
            if (inv_gj.Count() > 0)
            {
                totalDiscNormal += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountNominal));
                totalDiscPromo += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountPromo));
                totalDiscBank += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountBank));
                totalDiscMember += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountProgramNominal));
                totalDiscVoucher += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountGift));
                totalDiscOther += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountOther));
                totalDiscRound += Convert.ToDecimal(inv_gj.Sum(p => p.DiscountRound));
            }
            if (inv_ld.Count() > 0)
            {
                totalDiscNormal += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountNominal));
                totalDiscPromo += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountPromo));
                totalDiscBank += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountBank));
                totalDiscMember += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountProgramNominal));
                totalDiscVoucher += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountGift));
                totalDiscOther += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountOther));
                totalDiscRound += Convert.ToDecimal(inv_ld.Sum(p => p.DiscountRound));
            }

            grandTotalDiscount = totalDiscNormal + totalDiscPromo + totalDiscBank + totalDiscMember + totalDiscVoucher + totalDiscOther + totalDiscRound;

            //summary_sebelum_tradein.InnerText = nilaiHarusDibayar.ToString("N0");
            //summary_totalharusdibayar_setelahtradein.InnerText = (nilaiHarusDibayar - nilaiResell).ToString("N0");
            //summary_nilaiso.InnerText = NilaiSO.ToString("N0");
            //summary_resell_tradein.InnerText = nilaiResell.ToString("N0");
            //summary_totaldiscount.InnerText = grandTotalDiscount.ToString("N0");
            //summary_totalharusdibayar.InnerText = nilaiHarusDibayar.ToString("N0");
            //summary_totalpembayaran.InnerText = Paid.ToString("N0");
            //summary_kurangbayar.InnerText = Unpaid.ToString("N0");

            //adding firman 280818 perubahan bagian summary dari user control /UC/PaymentSummary.ascx
            paymentSummary.SebelumDiscount = NilaiSO.ToString("N0");
            paymentSummary.TotalDiscount = grandTotalDiscount.ToString("N0");
            paymentSummary.TotalHarusDiBayar = nilaiHarusDibayar.ToString("N0");
            paymentSummary.SebelumTradeIn = nilaiHarusDibayar.ToString("N0");
            paymentSummary.NilaiTradeIn = nilaiResell.ToString("N0");
            paymentSummary.SisaHarusDiBayar = (nilaiHarusDibayar - nilaiResell).ToString("N0");
            paymentSummary.TotalTerbayar = Paid.ToString("N0");

            bool StatusBayar = Convert.ToBoolean(d.StatusPembayaran);
            if (Unpaid == 0)
            {
                if (StatusBayar == true)
                {
                    paymentSummary.StatusPembayaran = "<font style='color:green;font-weight:bold';>Lunas</font>";
                }
                else
                {
                    paymentSummary.StatusPembayaran = "<font style='color:orange;font-weight:bold';>Waiting For Submit</font>";
                }
            }
            else if (Unpaid > 0)
            {
                paymentSummary.StatusPembayaran = "<font style='color:red;font-weight:bold';>Belum Lunas</font>";
            }
            else if (Unpaid < 0)
            {
                paymentSummary.StatusPembayaran = "<font style='color:red;font-weight:bold';>Lebih Bayar</font>";
            }

            spn_payment_totalamountdue.InnerText = (d.TotalBayar.HasValue ? (d.TotalBayar.Value).ToString("N0") : "0");
            spn_payment_amountpaid.InnerText = Paid.ToString("N0");
            spn_payment_balancedue.InnerText = Unpaid.ToString("N0");

            fillPembayaran();

            if (isNotSameDay)
            {
                show_Invalid_Payment_NotSameDay(d);
                if (Unpaid == 0)
                {
                    if (StatusBayar == true)
                    {
                        submit.Visible = false;
                        print.Visible = true;
                        div_alert_submit.Visible = false;
                    }
                    else
                    {
                        submit.Visible = true;
                        print.Visible = false;
                        div_alert_submit.Visible = true;
                    }
                }
                else
                {
                    submit.Visible = false;
                    print.Visible = false;
                    div_alert_submit.Visible = false;
                }
            }
            else
            {
                if (Unpaid == 0)
                {
                    if (StatusBayar == true)
                    {
                        submit.Visible = false;
                        print.Visible = true;
                        div_alert_submit.Visible = false;
                    }
                    else
                    {
                        submit.Visible = true;
                        print.Visible = false;
                        div_alert_submit.Visible = true;
                    }
                }
                else
                {
                    submit.Visible = false;
                    print.Visible = false;
                    div_alert_submit.Visible = false;
                }
            }
        }
    }

    protected void fillPembayaran()
    {
        FillPayment(myID, tb3);
    }

    //Stone
    protected void fillItem()
    {
        decimal GT = 0m;
        decimal GT2 = 0m;
        //Item
        fillItemDJ(ref GT, ref GT2);
        fillItemPG(ref GT, ref GT2);
        fillItemGJ(ref GT, ref GT2);
        fillItemLD(ref GT, ref GT2);

        //Grand-Total
        Sub_Total(GT, GT2, true);
    }

    // Pre-fetch SEMUA voucher untuk SO ini per tipe produk (1 query, bukan 1 query per baris
    // item seperti fillVoucherDJ/PG/GJ/LD versi lama) — hasilnya di-lookup dari memory per baris
    // lewat RenderVoucherHtml(). Ini pengganti fillVoucherDJ/PG/GJ/LD yang lama (dihapus, cuma
    // dipakai dari fillItemDJ/PG/GJ/LD masing-masing 1 kali, jadi aman diganti).
    protected Dictionary<int, List<Tuple<string, decimal>>> GetVoucherMapByProductType(int productType)
    {
        string query = @"
            SELECT ProductID, NomorVoucher, Nominal
            FROM SalesVoucher
            WHERE IDSalesOrder = @IDSalesOrder AND ProductType = @ProductType AND ProductID IS NOT NULL
            ORDER BY ID ASC";
        List<SqlParameter> param = new List<SqlParameter>()
        {
            new SqlParameter("@IDSalesOrder", SqlDbType.Int) { Value = myID },
            new SqlParameter("@ProductType", SqlDbType.Int) { Value = productType }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.Text);

        Dictionary<int, List<Tuple<string, decimal>>> map = new Dictionary<int, List<Tuple<string, decimal>>>();
        foreach (DataRow row in dt.Rows)
        {
            int productId = Convert.ToInt32(row["ProductID"]);
            if (!map.ContainsKey(productId)) map[productId] = new List<Tuple<string, decimal>>();
            map[productId].Add(Tuple.Create(row["NomorVoucher"].ToString(), Convert.ToDecimal(row["Nominal"])));
        }
        return map;
    }

    protected string RenderVoucherHtml(Dictionary<int, List<Tuple<string, decimal>>> voucherMap, int productId)
    {
        string text = "<div style=\"font-size: 12px;text-align:left\">";
        List<Tuple<string, decimal>> list;
        if (voucherMap.TryGetValue(productId, out list))
        {
            int i = 1;
            foreach (var v in list)
            {
                text += "<small>" + i + ".) " + v.Item1 + " (" + Convert.ToInt32(v.Item2).ToString("N0") + ")" + "</small>";
                text += "<br/>";
                i++;
            }
        }
        text += "</div>";
        return text;
    }

    //Item
    protected void fillItemDJ(ref decimal GT, ref decimal GT2)
    {
        // Raw parameterized query menggantikan LINQ — eliminasi 3 sumber N+1 per baris:
        // (1) r.StockProductDJ.Nomor / .StockProductDJ_CharProduct.CharProductItem.Nama
        //     (nav property lazy-load per baris),
        // (2) fillVoucherDJ(r) yang query ulang SalesVoucher tiap baris (sekarang: 1 query
        //     voucher untuk SELURUH SO, di-lookup per baris dari memory via voucherMap), dan
        // (3) "SELECT TOP 1 TotalFeeExtraRingSize FROM SalesOrderDJ WHERE ID = r.ID" yang
        //     sebenarnya query ULANG BARIS YANG SAMA — kolom ini memang tidak ada di mapping
        //     LINQ SalesOrderDJ (dikonfirmasi via reflection ke Library.dll), jadi query itu
        //     adalah workaround developer sebelumnya untuk keterbatasan itu. Sekarang diambil
        //     langsung dalam SELECT utama.
        string query = @"
            SELECT sod.ID, sod.Invoice, sod.IDProduct, sod.HargaRupiah, sod.DiscountNominal, sod.Discount,
                   sod.DiscountPromo, sod.DiscountProgramNominal, sod.DiscountProgram, sod.DiscountGift,
                   sod.DiscountBank, sod.DiscountOther, sod.DiscountRound, sod.TotalBayar,
                   ISNULL(sod.TotalFeeExtraRingSize, 0) AS TotalFeeExtraRingSize,
                   spd.Nomor, ISNULL(ci.Nama, '-') AS ItemNama
            FROM SalesOrderDJ sod
            JOIN StockProductDJ spd ON spd.ID = sod.IDProduct
            JOIN StockProductDJ_CharProduct cp ON cp.ID = spd.ID
            JOIN CharProductItem ci ON ci.ID = cp.ProductItem
            WHERE sod.IDForm = @IDForm
            ORDER BY sod.ID ASC";
        List<SqlParameter> param = new List<SqlParameter>()
        {
            new SqlParameter("@IDForm", SqlDbType.Int) { Value = myID }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.Text);
        Dictionary<int, List<Tuple<string, decimal>>> voucherMap = GetVoucherMapByProductType(1);

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (dt.Rows.Count > 0) Sub_Header(0);

        foreach (DataRow r in dt.Rows)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r["Invoice"].ToString()
                    + "<br />" + r["Nomor"].ToString()
                    + "<br />" + r["ItemNama"].ToString()
                    ;
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["HargaRupiah"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountNominal"]).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r["Discount"]).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountPromo"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountProgramNominal"]).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r["DiscountProgram"]).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountGift"]).ToString("N0") + "<br />" + RenderVoucherHtml(voucherMap, Convert.ToInt32(r["IDProduct"]));
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountBank"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountOther"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountRound"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["TotalFeeExtraRingSize"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["TotalBayar"]).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r["HargaRupiah"]);
            ST2 += Convert.ToDecimal(r["TotalBayar"]);

            index++;

        }
        //Total
        if (dt.Rows.Count > 0)
        {
            GT += ST;
            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }

    }
    protected void fillItemPG(ref decimal GT, ref decimal GT2)
    {
        // Raw parameterized query — sama alasannya dengan fillItemDJ. StockProductPG punya FK
        // langsung ke CharProductItem (tidak lewat tabel _CharProduct terpisah seperti DJ/GJ).
        string query = @"
            SELECT sop.ID, sop.Invoice, sop.IDProduct, sop.HargaRupiah, sop.DiscountNominal, sop.Discount,
                   sop.DiscountPromo, sop.DiscountProgramNominal, sop.DiscountProgram, sop.DiscountGift,
                   sop.DiscountBank, sop.DiscountOther, sop.DiscountRound, sop.TotalBayar,
                   ISNULL(sop.TotalFeeExtraRingSize, 0) AS TotalFeeExtraRingSize,
                   spp.Nomor, ISNULL(ci.Nama, '-') AS ItemNama
            FROM SalesOrderPG sop
            JOIN StockProductPG spp ON spp.ID = sop.IDProduct
            JOIN CharProductItem ci ON ci.ID = spp.ProductItem
            WHERE sop.IDForm = @IDForm
            ORDER BY sop.ID ASC";
        List<SqlParameter> param = new List<SqlParameter>()
        {
            new SqlParameter("@IDForm", SqlDbType.Int) { Value = myID }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.Text);
        Dictionary<int, List<Tuple<string, decimal>>> voucherMap = GetVoucherMapByProductType(2);

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (dt.Rows.Count > 0) Sub_Header(1);

        foreach (DataRow r in dt.Rows)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r["Invoice"].ToString()
                    + "<br />" + r["Nomor"].ToString()
                    + "<br />" + r["ItemNama"].ToString()
                    ;
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["HargaRupiah"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountNominal"]).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r["Discount"]).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountPromo"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountProgramNominal"]).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r["DiscountProgram"]).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountGift"]).ToString("N0") + "<br />" + RenderVoucherHtml(voucherMap, Convert.ToInt32(r["IDProduct"]));
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountBank"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountOther"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountRound"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["TotalFeeExtraRingSize"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["TotalBayar"]).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r["HargaRupiah"]);
            ST2 += Convert.ToDecimal(r["TotalBayar"]);

            index++;
        }

        //Total
        if (dt.Rows.Count > 0)
        {
            GT += ST;

            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }
    }
    protected void fillItemGJ(ref decimal GT, ref decimal GT2)
    {
        // Raw parameterized query — sama alasannya dengan fillItemDJ.
        string query = @"
            SELECT sog.ID, sog.Invoice, sog.IDProduct, sog.HargaRupiah, sog.DiscountNominal, sog.Discount,
                   sog.DiscountPromo, sog.DiscountProgramNominal, sog.DiscountProgram, sog.DiscountGift,
                   sog.DiscountBank, sog.DiscountOther, sog.DiscountRound, sog.TotalBayar,
                   ISNULL(sog.TotalFeeExtraRingSize, 0) AS TotalFeeExtraRingSize,
                   spg.Nomor, ISNULL(ci.Nama, '-') AS ItemNama
            FROM SalesOrderGJ sog
            JOIN StockProductGJ spg ON spg.ID = sog.IDProduct
            JOIN StockProductGJ_CharProduct cp ON cp.ID = spg.ID
            JOIN CharProductItem ci ON ci.ID = cp.ProductItem
            WHERE sog.IDForm = @IDForm
            ORDER BY sog.ID ASC";
        List<SqlParameter> param = new List<SqlParameter>()
        {
            new SqlParameter("@IDForm", SqlDbType.Int) { Value = myID }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.Text);
        Dictionary<int, List<Tuple<string, decimal>>> voucherMap = GetVoucherMapByProductType(3);

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (dt.Rows.Count > 0) Sub_Header(2);

        foreach (DataRow r in dt.Rows)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r["Invoice"].ToString()
                    + "<br />" + r["Nomor"].ToString()
                    + "<br />" + r["ItemNama"].ToString()
                    ;
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["HargaRupiah"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountNominal"]).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r["Discount"]).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountPromo"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountProgramNominal"]).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r["DiscountProgram"]).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountGift"]).ToString("N0") + "<br />" + RenderVoucherHtml(voucherMap, Convert.ToInt32(r["IDProduct"]));
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountBank"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountOther"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountRound"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["TotalFeeExtraRingSize"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["TotalBayar"]).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r["HargaRupiah"]);
            ST2 += Convert.ToDecimal(r["TotalBayar"]);

            index++;

        }
        //Total
        if (dt.Rows.Count > 0)
        {
            GT += ST;
            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }
    }
    protected void fillItemLD(ref decimal GT, ref decimal GT2)
    {
        // Raw parameterized query — sama alasannya dengan fillItemDJ. Tidak perlu join
        // CharProductItem (kolom Nama memang tidak ditampilkan di versi asli untuk LD).
        string query = @"
            SELECT sol.ID, sol.Invoice, sol.IDProduct, sol.HargaRupiah, sol.DiscountNominal, sol.Discount,
                   sol.DiscountPromo, sol.DiscountProgramNominal, sol.DiscountProgram, sol.DiscountGift,
                   sol.DiscountBank, sol.DiscountOther, sol.DiscountRound, sol.TotalBayar,
                   ISNULL(sol.TotalFeeExtraRingSize, 0) AS TotalFeeExtraRingSize,
                   spl.Nomor
            FROM SalesOrderLD sol
            JOIN StockProductLD_Stone1B spl ON spl.ID = sol.IDProduct
            WHERE sol.IDForm = @IDForm
            ORDER BY sol.ID ASC";
        List<SqlParameter> param = new List<SqlParameter>()
        {
            new SqlParameter("@IDForm", SqlDbType.Int) { Value = myID }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, param, CommandType.Text);
        Dictionary<int, List<Tuple<string, decimal>>> voucherMap = GetVoucherMapByProductType(4);

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (dt.Rows.Count > 0) Sub_Header(3);

        foreach (DataRow r in dt.Rows)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r["Invoice"].ToString()
                    + "<br />" + r["Nomor"].ToString()
                    ;
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["HargaRupiah"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountNominal"]).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r["Discount"]).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountPromo"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountProgramNominal"]).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r["DiscountProgram"]).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountGift"]).ToString("N0") + "<br />" + RenderVoucherHtml(voucherMap, Convert.ToInt32(r["IDProduct"]));
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountBank"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountOther"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["DiscountRound"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["TotalFeeExtraRingSize"]).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r["TotalBayar"]).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r["HargaRupiah"]);
            ST2 += Convert.ToDecimal(r["TotalBayar"]);

            index++;

        }
        //Total
        if (dt.Rows.Count > 0)
        {
            GT += ST;
            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }
    }
    //Custom Row
    protected void Sub_Header(int ItemIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.ItemIndex(ItemIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void Sub_Total(decimal Total, decimal TotalBayar, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 2;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(Total) + "</p>";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        td.ColumnSpan = 8;
        td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(TotalBayar) + "</p>";
        td.CssClass = "text-right";
        tr.Cells.Add(td);


        tb.Rows.Add(tr);
    }

    protected string TD_Nama(string Nama)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";

        return x;
    }
    protected string TD_Nama(string Nama, string GIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

        return x;
    }

    protected string valid_Amount(TextBox x)
    {
        string z = x.Text.Trim();
        string y = "";
        decimal parsedValue;

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d != null)
        {
            if (y == "" && (z == "")) { y = "Please fill the Amount. "; }

            if (y == "" && !decimal.TryParse(x.Text, out parsedValue)) { y = "Amount must be number only. "; }

            if (y == "" && Convert.ToDecimal(z) < 1) { y = "Amount must greater than zero. "; }

            if (y == "" && d.Unpaid.Value < Convert.ToDecimal(z) && d.Unpaid.Value > 0) { y = "Overpayment. Please reduce the Amount number. "; }

            if (y == "" && Math.Abs(d.Unpaid.Value) < Math.Abs(Convert.ToDecimal(z)) && d.Unpaid.Value < 0) { y = "Underpayment. Please reduce the Amount number. "; }

        }

        return y;
    }
    protected string valid_LakuEmas(TextBox x, string Field)
    {
        string y = "";
        decimal parsedValue;
        decimal weight;

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d != null)
        {
            if (Field == "txt_no_laku_emas")
            {
                if (x.Text == "") { y = "Please fill the Laku Emas Number. "; }
                else { y = ""; }
            }
            else if (Field == "txt_weight_lm")
            {
                if (x.Text == "") { y = "Please fill the Weight LM. "; }
                else { y = ""; }
                if (!decimal.TryParse(x.Text, out weight)) { y = "Weight LM must be number only. "; }
                else { y = ""; }
                if (decimal.TryParse(x.Text, out weight))
                {
                    if (decimal.Parse(x.Text) <= 0) { y = "Weight LM must be positive. "; }

                    else { y = ""; }
                }
                if (x.Text.Contains(","))
                {
                    //y = "Weight LM use '.' if there are decimal. ";
                    x.Text.Replace(",", ".");
                }
            }
            else if (Field == "txt_rate_lm")
            {
                if (x.Text == "") { y = "Please fill the Rate LM. "; }
                else { y = ""; }
                if (!decimal.TryParse(x.Text, out parsedValue)) { y = "Rate LM must be number only. "; }
                else { y = ""; }
                if (decimal.TryParse(x.Text, out parsedValue))
                {
                    if (decimal.Parse(x.Text) <= 0) { y = "Rate LM must be positive. "; }
                    else { y = ""; }
                }
                if (x.Text.Contains(","))
                {
                    //y = "Rate LM use '.' if there are decimal. ";
                    x.Text.Replace(",", ".");
                }
            }
            else if (Field == "txt_value_lm")
            {
                if (x.Text == "") { y = "Please fill the Value. "; }
                else { y = ""; }
                if (!decimal.TryParse(x.Text, out parsedValue)) { y = "Value must be number only. "; }
                else { y = ""; }
                if (decimal.TryParse(x.Text, out parsedValue))
                {
                    if (d.Unpaid.Value < Convert.ToDecimal(x.Text) && d.Unpaid.Value > 0) { y = "Overpayment. Please reduce the Amount number. "; }
                    else { y = ""; }
                    if (Math.Abs(d.Unpaid.Value) < Math.Abs(Convert.ToDecimal(x.Text)) && d.Unpaid.Value < 0) { y = "Underpayment. Please reduce the Amount number. "; }
                    else { y = ""; }
                }
            }
        }
        x.Dispose();
        return y;
    }

    //valid_BSP_TransactionNumber
    protected string valid_BSP_TransactionNumber(TextBox x)
    {
        string y = "";
        if (x.Text.Trim() == "") { y = "BridestoryPay Transaction Number must be filled. "; }
        else if (x.Text.Trim() != "")
        {
            decimal parsedValue;
            if (!decimal.TryParse(x.Text, out parsedValue))
            {
                y = "BridestoryPay Transaction Number must be number only. ";
            }
            else
            {
                if (x.Text.Length < 16) { y = "BridestoryPay Transaction Number must 16 total character filled. "; }
            }
        }

        if (y == "")
        {
            var d = db.SalesReceiptDetailExtension.Where(p =>
            p.TransactionNumber == x.Text.Trim()
            && (
                (p.SalesReceiptDetail.SalesReceipt.SalesOrder.StatusVoid == false
                && p.SalesReceiptDetail.SalesReceipt.SalesOrder.StatusPembayaran == true
                && p.SalesReceiptDetail.SalesReceipt.SalesOrder.Unpaid.Value <= 0)
                ||
                (p.SalesReceiptDetail.SalesReceipt.SalesOrder.Unpaid.Value > 0
                && p.SalesReceiptDetail.SalesReceipt.SalesOrder.StatusVoid == false)
            ));

            if (d.Count() > 0)
            {
                y = "BridestoryPay Transaction Number <b>" + x.Text.Trim() + "</b> already used.";
            }
        }

        return y;
    }

    protected string valid_Text(TextBox x, string o)
    {
        string y = "";
        if (x.Text.Trim() == "") { y = o + " must be filled. "; }
        return y;
    }

    protected void enableButtonSubmit()
    {
        save_payment_creditcard.Enabled = true;
        save_payment_debitcard.Enabled = true;
        save_payment_cash.Enabled = true;
        save_payment_transfer.Enabled = true;
        save_payment_giftcard.Enabled = true;
        save_payment_bridestorypay.Enabled = true;
        save_payment_casha.Enabled = true;
    }

    protected string msgValidationError(string x)
    {
        string result = "";
        result = "<div class='alert alert-danger text-center' role='alert'>" + x + "</div>";
        return result;
    }

    protected void save_payment_creditcard_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmount = txt_creditcard_amount;

        TextBox txtCardNumber = txt_creditcard_cardholdernumber;
        TextBox txtCardName = txt_creditcard_cardholdername;
        DropDownList ddlCardType = ddl_creditcard_cardtype;
        DropDownList ddlCardCategory = ddl_creditcard_cardcategory;
        DropDownList ddlEDC = ddl_creditcard_edc;
        DropDownList ddlBankIssuer = ddl_creditcard_bankissuer;
        DropDownList ddlBankProgram = ddl_creditcard_bankprogram;

        TextBox txtExtensionNumberTransaction = txt_creditcard_approvalcode;

        string msg = "";
        msg += valid_Amount(txtAmount);
        msg += valid_Text(txtCardName, "Card Holder Name");
        msg += valid_Text(txtCardNumber, "Card Holder Number");

        // validation
        if (msg != "")
        {
            msg = "Credit Card : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            active_CreditCard(true);
        }
        else
        {
            string Tgl = Convert.ToDateTime(db.SalesOrder.SingleOrDefault(p => p.ID == myID).Tgl).ToString("yyyy-MM-dd");
            int xIDPaymentType = 2; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = Convert.ToInt32(ddlCardType.SelectedValue);
            int xIDProgram = Convert.ToInt32(ddlBankProgram.SelectedValue);
            int xIDEDC = Convert.ToInt32(ddlEDC.SelectedValue);
            int xIDBankIssuer = Convert.ToInt32(ddlBankIssuer.SelectedValue);
            string xCCNumber = txtCardNumber.Text.Trim();
            string xCCName = txtCardName.Text.Trim();
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = Convert.ToInt32(ddlCardCategory.SelectedValue);
            //decimal xMDR = Payment.recalcMDR(xNominal, xIDBankIssuer, xIDProgram, xIDJenisKartuKredit, xIDPaymentType);
            decimal xMDR = Payment.recalcMDRNew(xIDPaymentType, xIDEDC, xIDBankIssuer, xIDJenisKartuKredit, xIDProgram, xNominal, Tgl);

            int xECRFlag = 0;
            string xECRValue = "";

            try
            {
                xECRFlag = Convert.ToInt32(hf_ecr_cc_flag.Value.ToString());
            }
            catch (Exception) { }
            xECRValue = hf_ecr_cc_value.Value.ToString();

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
            //save_Automatic_Status_Lunas();

            bool statusExtension = false;

            if (payid != 0)
            {
                statusExtension = savePaymentExtension(payid, txtExtensionNumberTransaction.Text.Trim());
				
                if (statusExtension)
                {
                    redirectGoToPayment();
                }
                else
                {
                    litMsg.Text = msgValidationError("Gagal menyimpan informasi Approval Code. Harap hubungi IT Support.");
                }
            }
        }
    }

    protected void save_payment_creditlakuemas_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmount = txt_creditlakuemas_amount;

        TextBox txtCardNumber = txt_creditlakuemas_cardholdernumber;
        TextBox txtCardName = txt_creditlakuemas_cardholdername;
        DropDownList ddlCardType = ddl_creditlakuemas_cardtype;
        DropDownList ddlCardCategory = ddl_creditlakuemas_cardcategory;
        DropDownList ddlEDC = ddl_edc_bank_credit_laku_emas;
        DropDownList ddlBankIssuer = ddl_creditlakuemas_bankissuer;
        DropDownList ddlBankProgram = ddl_creditlakuemas_bankprogram;

        TextBox txtExtensionNumberTransaction = txt_creditlakuemas_approvalcode;

        string msg = "";
        msg += valid_Amount(txtAmount);
        msg += valid_Text(txtCardName, "Card Holder Name");
        msg += valid_Text(txtCardNumber, "Card Holder Number");

        // validation
        if (msg != "")
        {
            msg = "Credit Card : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            active_CreditCard(true);
        }
        else
        {
            string Tgl = Convert.ToDateTime(db.SalesOrder.SingleOrDefault(p => p.ID == myID).Tgl).ToString("yyyy-MM-dd");
            int xIDPaymentType = 2; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = Convert.ToInt32(ddlCardType.SelectedValue);
            int xIDProgram = Convert.ToInt32(ddlBankProgram.SelectedValue);
            int xIDEDC = Convert.ToInt32(ddlEDC.SelectedValue);
            int xIDBankIssuer = Convert.ToInt32(ddlBankIssuer.SelectedValue);
            string xCCNumber = txtCardNumber.Text.Trim();
            string xCCName = txtCardName.Text.Trim();
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = Convert.ToInt32(ddlCardCategory.SelectedValue);
            //decimal xMDR = Payment.recalcMDR(xNominal, xIDBankIssuer, xIDProgram, xIDJenisKartuKredit, xIDPaymentType);
            decimal xMDR = Payment.recalcMDRNew(xIDPaymentType, xIDEDC, xIDBankIssuer, xIDJenisKartuKredit, xIDProgram, xNominal, Tgl);

            int xECRFlag = 0;
            string xECRValue = "";

            try
            {
                xECRFlag = Convert.ToInt32(hf_ecr_cclakuemas_flag.Value.ToString());
            }
            catch (Exception) { }
            xECRValue = hf_ecr_cc_value.Value.ToString();

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
            //save_Automatic_Status_Lunas();

            bool statusExtension = false;

            if (payid != 0)
            {
                statusExtension = savePaymentExtension(payid, txtExtensionNumberTransaction.Text.Trim());

                if (statusExtension)
                {
                    redirectGoToPayment();
                }
                else
                {
                    litMsg.Text = msgValidationError("Gagal menyimpan informasi Approval Code. Harap hubungi IT Support.");
                }
            }
        }
    }

    protected void save_payment_debitcard_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmount = txt_debitcard_amount;

        TextBox txtCardNumber = txt_debitcard_cardholdernumber;
        TextBox txtCardName = txt_debitcard_cardholdername;
        DropDownList ddlCardType = ddl_debitcard_cardtype;
        DropDownList ddlCardCategory = ddl_debitcard_cardcategory;
        DropDownList ddlEDC = ddl_debitcard_edc;
        DropDownList ddlBankIssuer = ddl_debitcard_bankissuer;

        TextBox txtExtensionNumberTransaction = txt_debitcard_approvalcode;

        string msg = "";
        msg += valid_Amount(txtAmount);
        msg += valid_Text(txtCardName, "Card Holder Name");
        msg += valid_Text(txtCardNumber, "Card Holder Number");

        // validation
        if (msg != "")
        {
            msg = "Debit Card : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            active_DebitCard(true);
        }
        else
        {
            string Tgl = Convert.ToDateTime(db.SalesOrder.SingleOrDefault(p => p.ID == myID).Tgl).ToString("yyyy-MM-dd");
            int xIDPaymentType = 4; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = Convert.ToInt32(ddlCardType.SelectedValue);
            int xIDProgram = 0;
            int xIDEDC = Convert.ToInt32(ddlEDC.SelectedValue);
            int xIDBankIssuer = Convert.ToInt32(ddlBankIssuer.SelectedValue);
            string xCCNumber = txtCardNumber.Text.Trim();
            string xCCName = txtCardName.Text.Trim();
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = Convert.ToInt32(ddlCardCategory.SelectedValue);
            //decimal xMDR = Payment.recalcMDR(xNominal, xIDBankIssuer, xIDProgram, xIDJenisKartuKredit, xIDPaymentType);
            decimal xMDR = Payment.recalcMDRNew(xIDPaymentType, xIDEDC, xIDBankIssuer, xIDJenisKartuKredit, xIDProgram, xNominal, Tgl);

            int xECRFlag = 0;
            string xECRValue = "";

            try
            {
                xECRFlag = Convert.ToInt32(hf_ecr_dc_flag.Value.ToString());
            }
            catch (Exception) { }
            xECRValue = hf_ecr_dc_value.Value.ToString();

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
            //save_Automatic_Status_Lunas();

            bool statusExtension = false;

            if (payid != 0)
            {
                statusExtension = savePaymentExtension(payid, txtExtensionNumberTransaction.Text.Trim());

                if (statusExtension)
                {
                    redirectGoToPayment();
                }
                else
                {
                    litMsg.Text = msgValidationError("Gagal menyimpan informasi Approval Code. Harap hubungi IT Support.");
                }
            }
        }
    }
    protected void save_payment_debitlakuemas_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmountLakuEmas = txt_debitlakuemas_amount;

        TextBox txtCardLakuEmasNumber = txt_debitlakuemas_cardholdernumber;
        TextBox txtCardLakuEmasName = txt_debitlakuemas_cardholdername;
        DropDownList ddlCardLakuEmasType = ddl_debitlakuemas_cardtype;
        DropDownList ddlCardLakuEmasCategory = ddl_debitlakuemas_cardcategory;
        //DropDownList ddlEDC = ddl_debitlakuemas_edc;
        DropDownList ddlBankIssuerLakuEmas = ddl_debitlakuemas_bankissuer;

        TextBox txtExtensionNumberTransactionLakuEmas = txt_debitlakuemas_approvalcode;
        DropDownList ddlDebitLakuEmas_Edc = ddl_edc_bank_debit_laku_emas;

        string msg = "";
        msg += valid_Amount(txtAmountLakuEmas);
        msg += valid_Text(txtCardLakuEmasName, "Card Holder Name");
        msg += valid_Text(txtCardLakuEmasNumber, "Card Holder Number");

        // validation
        if (msg != "")
        {
            msg = "Debit Laku Emas : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            //active_DebitLakuEmas(true);
        }
        else
        {
            string Tgl = Convert.ToDateTime(db.SalesOrder.SingleOrDefault(p => p.ID == myID).Tgl).ToString("yyyy-MM-dd");
            int xIDPaymentType = 4; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = Convert.ToInt32(ddlCardLakuEmasType.SelectedValue);
            int xIDProgram = 0;
            int xIDEDC = 24;//Convert.ToInt32(ddlEDC.SelectedValue);
            if (Convert.ToInt32(ddlDebitLakuEmas_Edc.SelectedValue) == 1)
                xIDEDC = 26;
            else if (Convert.ToInt32(ddlDebitLakuEmas_Edc.SelectedValue) == 2)
                xIDEDC = 27;
            else if (Convert.ToInt32(ddlDebitLakuEmas_Edc.SelectedValue) == 3)
                xIDEDC = 28;
            int xIDBankIssuer = Convert.ToInt32(ddlBankIssuerLakuEmas.SelectedValue);
            string xCCNumber = txtCardLakuEmasNumber.Text.Trim();
            string xCCName = txtCardLakuEmasName.Text.Trim();
            decimal xNominal = Convert.ToDecimal(txtAmountLakuEmas.Text);
            int xIDJenisKartuKredit = Convert.ToInt32(ddlCardLakuEmasCategory.SelectedValue);
            //decimal xMDR = Payment.recalcMDR(xNominal, xIDBankIssuer, xIDProgram, xIDJenisKartuKredit, xIDPaymentType);
            decimal xMDR = Payment.recalcMDRNew(xIDPaymentType, xIDEDC, xIDBankIssuer, xIDJenisKartuKredit, xIDProgram, xNominal, Tgl);

            int xECRFlag = 0;
            string xECRValue = "";

            try
            {
                xECRFlag = Convert.ToInt32(hf_ecr_dclakuemas_flag.Value.ToString());
            }
            catch (Exception) { }
            xECRValue = hf_ecr_dclakuemas_value.Value.ToString();

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
            //save_Automatic_Status_Lunas();

            bool statusExtension = false;

            if (payid != 0)
            {
                statusExtension = savePaymentExtension(payid, txtExtensionNumberTransactionLakuEmas.Text.Trim());

                if (statusExtension)
                {
                    redirectGoToPayment();
                }
                else
                {
                    litMsg.Text = msgValidationError("Gagal menyimpan informasi Approval Code. Harap hubungi IT Support.");
                }
            }
        }
    }

    protected void save_payment_qris_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmount = txt_qris_amount;

        TextBox txtQRISTransactionNumber = txt_qris_transactionnumber;
        TextBox txtQRISCustName = txt_qris_custname;
        TextBox txtQRISCustPhoneNumber = txt_qris_custphonenumber;

        DropDownList ddlQRISType = ddl_qris_cardtype;
        DropDownList ddlQRISBankIssuer = ddl_qris_bankissuer;

        string msg = "";
        msg += valid_Amount(txtAmount);

        // validation
        if (msg != "")
        {
            msg = "QRIS : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            active_QRIS(true);
        }
        else
        {
            string Tgl = Convert.ToDateTime(db.SalesOrder.SingleOrDefault(p => p.ID == myID).Tgl).ToString("yyyy-MM-dd");
            int xIDPaymentType = 13; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = 0;
            int xIDProgram = 0;
            int xIDEDC = Convert.ToInt32(ddlQRISType.SelectedValue);
            int xIDBankIssuer = Convert.ToInt32(ddlQRISBankIssuer.SelectedValue);//Convert.ToInt32(ddlQRISType.SelectedValue)
            string xCCNumber = txtQRISTransactionNumber.Text.Trim();
            string xCCName = txtQRISCustName.Text.Trim();
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = 0;
            //decimal xMDR = (0.7m * xNominal) / 100;//Payment.recalcMDR(xNominal, xIDBankIssuer, xIDProgram, xIDJenisKartuKredit, xIDPaymentType);
            decimal xMDR = Payment.recalcMDRNew(xIDPaymentType, xIDEDC, xIDBankIssuer, xIDJenisKartuKredit, xIDProgram, xNominal, Tgl);

            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
            //save_Automatic_Status_Lunas();
            if (payid != 0)
            {
                Db.Execute("UPDATE SalesReceiptDetail SET IDQRIS = '" + Convert.ToInt32(ddlQRISType.SelectedValue).ToString() + "', IDQRISBankIssuer = '" + Convert.ToInt32(ddlQRISBankIssuer.SelectedValue).ToString() + "' WHERE ID = " + payid, Db.Cnn_DB);
                redirectGoToPayment();
            }

            bool statusExtension = false;

            if (payid != 0)
            {
                statusExtension = savePaymentExtension(payid, xCCNumber);

                if (statusExtension)
                {
                    redirectGoToPayment();
                }
                else
                {
                    litMsg.Text = msgValidationError("Gagal menyimpan informasi Approval Code. Harap hubungi IT Support.");
                }
            }
        }
    }

    protected void save_payment_qrislakuemas_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmount = txt_qrislakuemas_amount;

        TextBox txtQRISTransactionNumber = txt_qrislakuemas_transactionnumber;
        TextBox txtQRISCustName = txt_qrislakuemas_custname;
        TextBox txtQRISCustPhoneNumber = txt_qrislakuemas_custphonenumber;

        DropDownList ddlQRISType = ddl_qrislakuemas_cardtype;
        DropDownList ddlQRISBankIssuer = ddl_qrislakuemas_bankissuer;

        string msg = "";
        msg += valid_Amount(txtAmount);

        // validation
        if (msg != "")
        {
            msg = "QRIS LEI: " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            active_QRIS(true);
        }
        else
        {
            string Tgl = Convert.ToDateTime(db.SalesOrder.SingleOrDefault(p => p.ID == myID).Tgl).ToString("yyyy-MM-dd");
            int xIDPaymentType = 15; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = 0;
            int xIDProgram = 0;
            int xIDEDC = Convert.ToInt32(ddlQRISType.SelectedValue);
            int xIDBankIssuer = Convert.ToInt32(ddlQRISBankIssuer.SelectedValue);//Convert.ToInt32(ddlQRISType.SelectedValue)
            string xCCNumber = txtQRISTransactionNumber.Text.Trim();
            string xCCName = txtQRISCustName.Text.Trim();
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = 0;
            //decimal xMDR = (0.7m * xNominal) / 100;//Payment.recalcMDR(xNominal, xIDBankIssuer, xIDProgram, xIDJenisKartuKredit, xIDPaymentType);
            decimal xMDR = Payment.recalcMDRNew(xIDPaymentType, xIDEDC, xIDBankIssuer, xIDJenisKartuKredit, xIDProgram, xNominal, Tgl);

            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
            //save_Automatic_Status_Lunas();
            if (payid != 0)
            {
                Db.Execute("UPDATE SalesReceiptDetail SET IDQRIS = '" + Convert.ToInt32(ddlQRISType.SelectedValue).ToString() + "', IDQRISBankIssuer = '" + Convert.ToInt32(ddlQRISBankIssuer.SelectedValue).ToString() + "' WHERE ID = " + payid, Db.Cnn_DB);
                redirectGoToPayment();
            }

            bool statusExtension = false;

            if (payid != 0)
            {
                statusExtension = savePaymentExtension(payid, xCCNumber);

                if (statusExtension)
                {
                    redirectGoToPayment();
                }
                else
                {
                    litMsg.Text = msgValidationError("Gagal menyimpan informasi Approval Code. Harap hubungi IT Support.");
                }
            }
        }
    }

    protected void save_payment_xendit_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmount = txt_xendit_amount;

        TextBox txtXenditTransactionID = txt_xendit_idtransaction;

        DropDownList ddlXenditType = ddl_payment_xendit_methode;
        DropDownList ddlXenditBankIssuer = ddl_xendit_bankissuer;

        string msg = "";
        msg += valid_Amount(txtAmount);

        // validation
        if (msg != "")
        {
            msg = "Xendit : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            active_Xendit(true);
        }
        else
        {
            int xIDPaymentType = 14; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = 0;
            int xIDProgram = 0;
            int xIDEDC = 30;
            int xIDBankIssuer = (ddlXenditType.SelectedValue == "1") ? 0 : Convert.ToInt32(ddlXenditBankIssuer.SelectedValue);//Convert.ToInt32(ddlQRISType.SelectedValue);
            string xCCNumber = txtXenditTransactionID.Text.Trim();
            string xCCName = "Xendit " + ddlXenditType.SelectedItem.Text;
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = 0;
            decimal xMDR = 0;//Payment.recalcMDR(xNominal, xIDBankIssuer, xIDProgram, xIDJenisKartuKredit, xIDPaymentType);
            if (xIDBankIssuer == 13)
                xMDR = (xNominal * 1.8m) / 100;
            else
                xMDR = (xNominal * 1.9m) / 100;

            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
			//save_Automatic_Status_Lunas();
            if (payid != 0)
            {
                redirectGoToPayment();
            }

        }
    }

    protected void save_payment_ecommerce_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmount = txt_ecommerce_amount;

        TextBox txtEcommerceTransactionID = txt_ecommerce_idtransaction;

        DropDownList ddlEcommercePlatform = ddl_payment_ecommerce_platform;

        string msg = "";
        msg += valid_Amount(txtAmount);

        // validation
        if (msg != "")
        {
            msg = "E-commerce : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            active_Ecommerce(true);
        }
        else
        {
            int xIDPaymentType = 16; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = 0;
            int xIDProgram = 0;
            int xIDEDC = Convert.ToInt32(ddlEcommercePlatform.SelectedValue);
            int xIDBankIssuer = 1;//Convert.ToInt32(ddlQRISType.SelectedValue);
            string xCCNumber = txtEcommerceTransactionID.Text.Trim();
            string xCCName = "Ecommerce " + ddlEcommercePlatform.SelectedItem.Text;
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = 0;
            decimal xMDR = 0;//Payment.recalcMDR(xNominal, xIDBankIssuer, xIDProgram, xIDJenisKartuKredit, xIDPaymentType);
            /*if(xIDBankIssuer == 13)
				xMDR = (xNominal * 1.8m) / 100;
			else
				xMDR = (xNominal * 1.9m) / 100;*/

            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
            //save_Automatic_Status_Lunas();
            if (payid != 0)
            {
                redirectGoToPayment();
            }
        }
    }

    protected void onChangeAmountDebitLakuEmas(object sender, EventArgs e)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        var pg = d.SalesOrderPG;

        decimal weight_pg = 0m;
        decimal weight_lm = 0m;
        decimal rate_lm = 0m;
        try
        {
            rate_lm = Convert.ToDecimal(GetLakuEmasPriceAPI());//Db.SingleDecimal("SELECT TOP 1 m_beli FROM msrate WHERE m_kode = 'LKLM' ORDER BY m_tanggal DESC ", Db.Cnn_CS);
        }
        catch { }
        decimal harga_pg = 0m;
        decimal harga_lm = 0m;
        decimal ongkos = 0m;
        foreach (var item in pg)
        {
            decimal tgpjual = GetTGPPGJual(item.StockProductPG);
            decimal weight_pgdetail = item.StockProductPG.NettoWeight ?? 0m;
            decimal value_pgdetail = item.TotalBayar ?? 0m;
            decimal weight_lmdetail = (75 * weight_pgdetail) / 100;
            decimal value_lmdetail = weight_lmdetail * tgpjual;

            ongkos += value_pgdetail - value_lmdetail;
            weight_pg += weight_pgdetail;
            harga_pg += value_pgdetail;
            weight_lm += weight_lmdetail;
            harga_lm += value_lmdetail;
        }
        /*weight_lm = (3 * weight_pg) / 4;
		harga_lm = weight_lm * rate_lm;
		harga_pg = d.Unpaid ?? 0m;
		ongkos = harga_pg - harga_lm;*/

        TextBox tx = (TextBox)sender;
        if (Convert.ToDecimal(tx.Text) >= 0 && Convert.ToDecimal(tx.Text) <= ongkos)
        {
            txt_debitlakuemas_amount.Text = (harga_lm + Convert.ToDecimal(tx.Text)).ToString("N0");
            hf_ecr_paiddclakuemas_value.Value = harga_pg.ToString("N0");

        }
        else
        {
            tx.Text = ongkos.ToString("N0");
            txt_debitlakuemas_amount.Text = harga_pg.ToString("N0");
            hf_ecr_paiddclakuemas_value.Value = harga_pg.ToString("N0");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Ongkos harus lebih besar dari 0 dan tidak lebih dari " + ongkos.ToString("N0") + "')", true);
        }
    }

    protected void save_payment_cash_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmount = txt_cash_amount;

        RadioButton rdoOut = rdo_cash_out;

        string msg = "";
        msg += valid_Amount(txtAmount);

        // validation
        if (msg != "")
        {
            msg = "Cash : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            active_Cash(true);
        }
        else
        {
            int xIDPaymentType = 1; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = 0;
            int xIDProgram = 0;
            decimal xMDR = 0;
            int xIDEDC = 0;
            int xIDBankIssuer = 0;
            string xCCNumber = "";
            string xCCName = "";
            decimal xNominal = Convert.ToDecimal(txtAmount.Text) * (rdoOut.Checked ? -1 : 1);
            int xIDJenisKartuKredit = 0;
            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
            //save_Automatic_Status_Lunas();
            if (payid != 0)
            {
                redirectGoToPayment();
            }
        }
    }

    protected void save_payment_cashlei_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmount = txt_cashlei_amount;

        RadioButton rdoOut = rdo_cashlei_out;

        string msg = "";
        msg += valid_Amount(txtAmount);

        // validation
        if (msg != "")
        {
            msg = "Cash LEI : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            active_CashLEI(true);
        }
        else
        {
            int xIDPaymentType = 12; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = 0;
            int xIDProgram = 0;
            decimal xMDR = 0;
            int xIDEDC = 0;
            int xIDBankIssuer = 0;
            string xCCNumber = "";
            string xCCName = "";
            decimal xNominal = Convert.ToDecimal(txtAmount.Text) * (rdoOut.Checked ? -1 : 1);
            int xIDJenisKartuKredit = 0;
            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
            //save_Automatic_Status_Lunas();
            if (payid != 0)
            {
                redirectGoToPayment();
            }
        }
    }

    protected void save_payment_transfer_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmount = txt_transfer_amount;

        TextBox txtAccountNumber = txt_transfer_number;
        TextBox txtAccountName = txt_transfer_name;

        DropDownList ddlBankIssuer = ddl_transfer_bankissuer;

        string msg = "";
        msg += valid_Amount(txtAmount);

        // validation
        if (msg != "")
        {
            msg = "Transfer : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            active_Transfer(true);
        }
        else
        {
            int xIDPaymentType = 3; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = 0;
            int xIDProgram = 0;
            int xIDEDC = 0;
            int xIDBankIssuer = Convert.ToInt32(ddlBankIssuer.SelectedValue);
            string xCCNumber = txtAccountNumber.Text.Trim();
            string xCCName = txtAccountName.Text.Trim();
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = 0;
            decimal xMDR = Payment.recalcMDR(xNominal, xIDBankIssuer, xIDProgram, xIDJenisKartuKredit, xIDPaymentType);

            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
            //save_Automatic_Status_Lunas();
            if (payid != 0)
            {
                redirectGoToPayment();
            }
        }
    }


    protected void save_payment_casha_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmount = txt_casha_amount;
        TextBox txtAccountNumber = txt_casha_number;
        TextBox txtAccountName = txt_casha_name;
        TextBox txtApprovalCode = txt_casha_approvalcode;

        string msg = "";
        msg += valid_Amount(txtAmount);
        msg += valid_Text(txtAccountNumber, "Gelombang Arisan");
        msg += valid_Text(txtAccountName, "Nama Paket & Arisan");


        if (string.IsNullOrEmpty(valid_Amount(txtApprovalCode)) == false)
        {
            msg += "Column Subsidi " + valid_Amount(txtApprovalCode);
        }

        // validation
        if (msg != "")
        {
            msg = "Cash-A : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            active_CashA(true);
        }
        else
        {
            int xIDPaymentType = 19; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = 0;
            int xIDProgram = 0;
            int xIDEDC = 0;
            int xIDBankIssuer = 0;
            string xCCNumber = txtAccountNumber.Text.Trim();
            string xCCName = txtAccountName.Text.Trim() + "___" + txtApprovalCode.Text.Trim();
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = 0;
            decimal xMDR = 0;


            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
            //save_Automatic_Status_Lunas();
            if (payid != 0)
            {
                redirectGoToPayment();
            }
        }
    }
    protected void save_payment_giftcard_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        Literal litMsgTest = lit_message_test;
        TextBox txtCardNumber = txt_giftcard_cardholdernumber;
        TextBox txtCardUID = txt_giftcard_cardholderuid;
        TextBox txtAmount = txt_giftcard_amount;

        string zAmount = Convert.ToDecimal(txtAmount.Text.Trim()).ToString("N0").Replace(".", "");

        string msg = "";
        msg += valid_Amount(txtAmount);
        msg += valid_Text(txtCardNumber, "Gift Card Number");

        bool validExecute = false;
        string[] response1;
        string[] response2;
        string[] response3;

        ////////// response 1
        response1 = (RESTClient.isCardNumberUIDValid(litMsgTest, txtCardUID.Text.Trim(), txtCardNumber.Text.Trim())).Split('-');

        if (response1[0].Trim() == "2410")
        {
            ////////// response 2
            response2 = (RESTClient.isBalanceEnough(litMsg, txtCardNumber.Text.Trim(), zAmount)).Split('-');
            if (response2[0].Trim() == "1010")
            {
                validExecute = true;
            }
            else
            {
                //msg += zAmount;
                msg += response2[1].Trim() + "<br />(Code : 0x" + response2[0].Trim().PadLeft(6, '0') + ")";
            }
        }
        else
        {
            msg += response1[1].Trim() + "<br />(Code : 0x" + response1[0].Trim().PadLeft(6, '0') + ")";
        }

        txtCardUID.Text = ""; // clear the UID input

        // validation
        if (msg != "")
        {
            msg = "Gift Card : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
        }
        else
        {
            if (validExecute)
            {

                int xIDPaymentType = 9; // 1	giftcard / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit, 9 Gift Card
                int xIDCard = 0;
                int xIDProgram = 0;
                decimal xMDR = 0;
                int xIDEDC = 0;
                int xIDBankIssuer = 0;
                string xCCNumber = txtCardNumber.Text.Trim();
                string xCCName = "";
                decimal xNominal = Convert.ToDecimal(txtAmount.Text);
                int xIDJenisKartuKredit = 0;
                int xECRFlag = 0;
                string xECRValue = "";

                // insert data
                int payid = 0;
                payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
                //save_Automatic_Status_Lunas();

                // custom business rule
                // post to giftcard database using API
                if (payid != 0)
                {
                    var w = db.SalesReceiptDetail.Single(p => p.ID == payid);
                    var s = w.SalesReceipt.SalesOrder;
                    string loc = "000";
                    if (s.TipeLokasi == 1) { loc = db.LocOutlet.Single(p => p.ID == s.IDLokasi).Kode; }
                    if (s.TipeLokasi == 2) { loc = db.LocExhibition.Single(p => p.ID == s.IDLokasi).Kode; }

                    string xcardnumber = txtCardNumber.Text.Trim();
                    string xinvoicenumber = s.Nomor;
                    string xpayment_id = w.ID.ToString();
                    string xtype = "SALE"; // SALE , VOID
                    string xamount = xNominal.ToString("N0").Replace(".", "");
                    string xbranch_id = loc;
                    string xpayment_method = "Gift Card";
                    string xuser_cashier = w.SalesReceipt.OperatorNama;

                    ////////// response 3
                    response3 = (RESTClient.postTransactionSale(litMsg, xinvoicenumber, xpayment_id, xcardnumber, xtype, xamount, xbranch_id, xpayment_method, xuser_cashier)).Split('-');

                    if (response3[0].Trim() == "1310")
                    {
                        // Rubah Flag Verifikasi Pembayaran
                        w.GC_Verification = true;
                        w.GC_VerificationTgl = System.DateTime.Now;
                        db.SubmitChanges();
                        redirectGoToPayment();

                    }
                    else
                    {

                        msg = response3[1].Trim() + "<br />(Code : 0x" + response3[0].Trim().PadLeft(6, '0') + ")";
                        litMsg.Text = msgValidationError(msg);
                        enableButtonSubmit();

                    }
                }
            }

        }

        //fillPembayaran();
    }
    protected void save_payment_bridestorypay_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmount = txt_bridestorypay_amount;

        TextBox txtAccountNumber = txt_bridestorypay_number;
        TextBox txtAccountName = txt_bridestorypay_name;

        DropDownList ddlBankIssuer = ddl_bridestorypay_bankissuer;

        TextBox txtBSP = txt_bridestorypay_transactionnumber;

        string msg = "";
        msg += valid_Amount(txtAmount);
        msg += valid_BSP_TransactionNumber(txtBSP);

        // validation
        if (msg != "")
        {
            msg = "BrideStory Pay : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            active_BrideStoryPay(true);
        }
        else
        {
            int xIDPaymentType = 10; // 10 : Bridestory Pay
            int xIDCard = 0;
            int xIDProgram = 0;
            int xIDEDC = 0;
            int xIDBankIssuer = Convert.ToInt32(ddlBankIssuer.SelectedValue);
            string xCCNumber = txtAccountNumber.Text.Trim();
            string xCCName = txtAccountName.Text.Trim();
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = 0;
            decimal xMDR = Payment.recalcMDR(xNominal, xIDBankIssuer, xIDProgram, xIDJenisKartuKredit, xIDPaymentType);

            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            bool statusBSPAY = false;
            int payid = 0;

            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
            //save_Automatic_Status_Lunas();

            // custom business rule
            if (payid != 0)
            {
                statusBSPAY = savePaymentExtension(payid, txtBSP.Text.Trim());

                if (statusBSPAY)
                {
                    redirectGoToPayment();
                }
                else
                {
                    litMsg.Text = msgValidationError("Gagal menyimpan informasi BSPAY Transaction Number. Harap hubungi IT Support.");
                }
            }

        }

    }

    protected void save_payment_downpayment_Click(object sender, EventArgs e)
    {
        Response.Redirect("Payment_DPAdd.aspx?id=" + myID);
    }
    protected void save_payment_voucher_Click(object sender, EventArgs e)
    {

        if (STAMPS.xActivate)
        {
            Response.Redirect("Payment_VoucherStampsAdd.aspx?id=" + myID);
        }
        else
        {
            Response.Redirect("Payment_VoucherAdd.aspx?id=" + myID);
        }
    }
    protected void save_payment_laku_emas_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;

        string msg = "";
        string msgnolakuemas = valid_LakuEmas(txt_no_laku_emas, "txt_no_laku_emas");
        string msgweight = valid_LakuEmas(txt_weight_lm, "txt_weight_lm");
        string msgrate = valid_LakuEmas(txt_rate_lm, "txt_rate_lm");
        string msgvalue = valid_LakuEmas(txt_value_lm, "txt_value_lm");

        msg = msgnolakuemas + msgweight + msgrate + msgvalue;

        // validation
        if (msg != "")
        {
            msg = "Laku Emas : " + msg;
            litMsg.Text = msgValidationError(msg);
            enableButtonSubmit();
            active_LakuEmas(true);
        }
        else
        {
            int xIDPaymentType = 11; // 11.Laku Emas
            int xIDCard = 0;
            int xIDProgram = 0;
            decimal xMDR = 0;
            int xIDEDC = 0;
            int xIDBankIssuer = 0;
            string xCCNumber = txt_no_laku_emas.Text;
            string xCCName = "";
            decimal xNominal = Convert.ToDecimal(txt_value_lm.Text.Replace(",", "."));
            int xIDJenisKartuKredit = 0;
            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            int payid = 0;

            string MessageError = Db.SingleString("Exec sp_insert_payment_laku_emas '" + txt_no_laku_emas.Text.Trim() + "','" + txt_weight_lm.Text.Replace(",", ".") + "','" + txt_rate_lm.Text.Replace(",", ".") + "','" + txt_value_lm.Text.Replace(",", ".") + "' ", Db.Cnn_DB);
            if (MessageError != "")
            {
                MessageError = "Laku Emas : " + MessageError;
                litMsg.Text = msgValidationError(MessageError);
                enableButtonSubmit();
                active_LakuEmas(true);
            }
            else
            {
                payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
                //save_Automatic_Status_Lunas();

                if (payid != 0)
                {
                    var so = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
                    savePaymentLakuTukar(so, payid, xCCNumber);
                    redirectGoToPayment();
                }
            }
        }
    }

    protected bool savePaymentExtension(int IDSalesReceipt, string TransNumber)
    {
        bool status = true;

        var d = db.SalesReceiptDetail.SingleOrDefault(p => p.SalesReceipt.IDSalesOrder == myID && p.SalesReceipt.SalesOrder.Draft == false && p.ID == IDSalesReceipt);
        if (d == null) { status = false; }
        else
        {
            //lit_test_bridestorypay.Text += "1:" + d.ID.ToString();
            //lit_test_bridestorypay.Text += "2:" + d.PaymentType.Nama.ToString();
            //lit_test_bridestorypay.Text += "3:" + TransNumber;

            // add sales receipt header
            SalesReceiptDetailExtension s = new SalesReceiptDetailExtension();

            s.ID = d.ID;
            s.PaymentName = d.PaymentType.Nama;
            s.VerificationDateTime = System.DateTime.Now;
            s.VerificationFlag = 1;
            s.VerificationOperator = App.UserNama;
            s.TransactionNumber = TransNumber;

            db.SalesReceiptDetailExtension.InsertOnSubmit(s);
            db.SubmitChanges();

            if ((d.IDPaymentType == 4 || d.IDPaymentType == 2) && (d.IDEDC == 24 || d.IDEDC == 26 || d.IDEDC == 27 || d.IDEDC == 28))
            {
                try
                {
                    savePaymentDebitLakuEmas(d.SalesReceipt.SalesOrder, d.ID);
                }
                catch { }
            }
            else if (IsPGOnly(d.SalesReceipt.SalesOrder))
            {
                try
                {
                    savePaymentDebitLakuEmas(d.SalesReceipt.SalesOrder, d.ID);
                }
                catch { }
            }
            else if (d.IDPaymentType == 15 && (d.IDEDC == 31 || d.IDEDC == 32))
            {
                try
                {
                    savePaymentDebitLakuEmas(d.SalesReceipt.SalesOrder, d.ID);
                }
                catch { }
            }
        }

        return status;
    }

    protected DataTable SaveToTPOS(string brand)
    {
        DataTable dt = new DataTable();
        DataTable t_pos = new DataTable("t_pos");
        t_pos.Columns.Add("m_cabang", typeof(string));        // varchar(5)
        t_pos.Columns.Add("m_nomor", typeof(string));         // varchar(40)
        t_pos.Columns.Add("m_tanggal", typeof(DateTime));     // datetime
        t_pos.Columns.Add("m_kodecust", typeof(string));      // varchar(20)
        t_pos.Columns.Add("m_kodesales", typeof(string));     // varchar(20)
        t_pos.Columns.Add("m_kodesales2", typeof(string));    // varchar(20)
        t_pos.Columns.Add("m_nama", typeof(string));          // varchar(100)
        t_pos.Columns.Add("m_keterangan", typeof(string));    // varchar(100)
        t_pos.Columns.Add("m_status", typeof(string));        // char(1)
        t_pos.Columns.Add("m_post", typeof(string));          // char(1)
        t_pos.Columns.Add("m_cancelby", typeof(string));      // varchar(50)
        t_pos.Columns.Add("m_canceldate", typeof(DateTime));  // datetime
        t_pos.Columns.Add("m_cancelnote", typeof(string));    // varchar(100)
        t_pos.Columns.Add("m_type", typeof(string));          // char(1)
        t_pos.Columns.Add("m_jaws", typeof(string));          // varchar(20)
        t_pos.Columns.Add("m_rate", typeof(decimal));         // numeric(15,2)
        t_pos.Columns.Add("m_ld", typeof(decimal));           // numeric(15,2)
        t_pos.Columns.Add("m_cabang2", typeof(string));       // varchar(10)

        DataTable t_pos2 = new DataTable("t_pos2");
        t_pos2.Columns.Add("m_cabang", typeof(string));                      // varchar(5)
        t_pos2.Columns.Add("m_nomor", typeof(string));                       // varchar(40)
        t_pos2.Columns.Add("m_kodebarang", typeof(string));                 // varchar(15)
        t_pos2.Columns.Add("m_productid", typeof(string));                  // varchar(25)
        t_pos2.Columns.Add("m_lokasi", typeof(string));                     // varchar(8)
        t_pos2.Columns.Add("m_qty", typeof(decimal));                       // numeric(9,2)
        t_pos2.Columns.Add("m_harga", typeof(decimal));                     // numeric(15,2)
        t_pos2.Columns.Add("m_disc", typeof(decimal));                      // numeric(5,2)
        t_pos2.Columns.Add("m_discount", typeof(decimal));                  // numeric(15,2)
        t_pos2.Columns.Add("m_discount2", typeof(decimal));                 // numeric(15,2)
        t_pos2.Columns.Add("m_discount3", typeof(decimal));                 // numeric(15,2)
        t_pos2.Columns.Add("m_discount4", typeof(decimal));                 // numeric(15,2)
        t_pos2.Columns.Add("m_kurs", typeof(string));                       // varchar(5)
        t_pos2.Columns.Add("m_rate", typeof(decimal));                      // numeric(15,2)
        t_pos2.Columns.Add("m_keterangan", typeof(string));                // varchar(100)
        t_pos2.Columns.Add("m_status", typeof(string));                    // char(1)
        t_pos2.Columns.Add("m_poin", typeof(decimal));                      // numeric(12,2)
        t_pos2.Columns.Add("m_discount5", typeof(decimal));                // numeric(15,2)
        t_pos2.Columns.Add("m_discount6", typeof(decimal));                // numeric(15,2)
        t_pos2.Columns.Add("m_discount7", typeof(decimal));                // numeric(15,2)
        t_pos2.Columns.Add("m_ppn", typeof(decimal));                       // numeric(6,4)
        t_pos2.Columns.Add("m_hargam", typeof(decimal));                    // numeric(15,4)
        t_pos2.Columns.Add("isRepostingVoid", typeof(string));             // varchar(1)
        t_pos2.Columns.Add("m_Extraringsize", typeof(decimal));            // numeric(15,2)
        t_pos2.Columns.Add("m_FeeExtraringSizePerSize", typeof(decimal));  // numeric(15,2)
        t_pos2.Columns.Add("m_total_bayar", typeof(decimal));              // numeric(38,2)

        DataTable t_pos3 = new DataTable("t_pos3");
        t_pos3.Columns.Add("m_cabang", typeof(string));       // varchar(5)
        t_pos3.Columns.Add("m_nomor", typeof(string));        // varchar(40)
        t_pos3.Columns.Add("m_tanggal", typeof(DateTime));    // datetime
        t_pos3.Columns.Add("m_carabayar", typeof(string));    // varchar(20)
        t_pos3.Columns.Add("m_edc", typeof(string));          // varchar(20)
        t_pos3.Columns.Add("m_bank", typeof(string));         // varchar(20)
        t_pos3.Columns.Add("m_nokartu", typeof(string));      // varchar(50)
        t_pos3.Columns.Add("m_jnkartu", typeof(string));      // varchar(20)
        t_pos3.Columns.Add("m_cclkartu", typeof(string));     // varchar(20)
        t_pos3.Columns.Add("m_nmkartu", typeof(string));      // varchar(50)
        t_pos3.Columns.Add("m_jumlah", typeof(decimal));      // numeric(15,2)
        t_pos3.Columns.Add("m_mdr", typeof(decimal));         // numeric(15,2)
        t_pos3.Columns.Add("m_noinv", typeof(string));        // varchar(30)
        t_pos3.Columns.Add("m_id", typeof(int));              // int

        DataTable t_pos4 = new DataTable("t_pos4");
        t_pos4.Columns.Add("m_nomor", typeof(string));          // varchar(40)
        t_pos4.Columns.Add("m_inv", typeof(string));            // varchar(40)
        t_pos4.Columns.Add("m_kodebarang", typeof(string));     // varchar(20)
        t_pos4.Columns.Add("m_hpp", typeof(decimal));           // decimal(18,0)
        t_pos4.Columns.Add("m_qty", typeof(int));               // int
        t_pos4.Columns.Add("m_idtrans", typeof(int));           // int
        t_pos4.Columns.Add("m_namabarang", typeof(string));     // varchar(255)
        t_pos4.Columns.Add("m_subled", typeof(string));         // varchar(20)
        t_pos4.Columns.Add("m_jenis", typeof(string));          // varchar(20)
        t_pos4.Columns.Add("m_id", typeof(int));                // int
        t_pos4.Columns.Add("m_tanggal", typeof(DateTime));      // datetime
        t_pos4.Columns.Add("m_totalharga", typeof(decimal));    // decimal(18,0)

        //Isi t_pos
        string queryShow = "sp_displaydatatpos";

        List<SqlParameter> parameters1 = new List<SqlParameter>
        {
            new SqlParameter("@ID_SALES_ORDER", SqlDbType.Int){ Value = myID }
        };
        DataSet ds = Ds(queryShow, Db.Cnn_DB, parameters1, CommandType.StoredProcedure);
        DataTable dt_tpos = ds.Tables[0];
        DataTable dt_tpos2 = ds.Tables[1];
        DataTable dt_tpos3 = ds.Tables[2];
        DataTable dt_tpos4 = ds.Tables[3];

        int rowcount_tpos = dt_tpos.Rows.Count;
        int rowcount_tpos2 = dt_tpos2.Rows.Count;
        int rowcount_tpos3 = dt_tpos3.Rows.Count;
        int rowcount_tpos4 = dt_tpos4.Rows.Count;

        if(rowcount_tpos == 1 && rowcount_tpos2 > 0)
        {
            foreach(DataRow dr in dt_tpos.Rows)
            {
                // Create and initialize a new DataRow
                DataRow dr2 = t_pos.NewRow();

                // Manually assign DBNull.Value to all columns
                dr2["m_cabang"] = dr["m_cabang"].ToString();
                dr2["m_nomor"] = dr["m_nomor"].ToString();
                dr2["m_tanggal"] = Convert.ToDateTime(dr["m_tanggal"]);
                dr2["m_kodecust"] = dr["m_kodecust"].ToString();
                dr2["m_kodesales"] = dr["m_kodesales"].ToString();
                dr2["m_kodesales2"] = dr["m_kodesales2"].ToString();
                dr2["m_nama"] = dr["m_nama"].ToString();
                dr2["m_keterangan"] = dr["m_keterangan"].ToString();
                dr2["m_status"] = dr["m_status"].ToString();
                dr2["m_post"] = DBNull.Value;
                dr2["m_cancelby"] = DBNull.Value;
                dr2["m_canceldate"] = DBNull.Value;
                dr2["m_cancelnote"] = DBNull.Value;
                dr2["m_type"] = dr["m_type"].ToString();
                dr2["m_jaws"] = DBNull.Value;
                dr2["m_rate"] = DBNull.Value;
                dr2["m_ld"] = DBNull.Value;
                dr2["m_cabang2"] = dr["m_cabang2"].ToString();

                // Add the row to the table
                t_pos.Rows.Add(dr2);
            }

            foreach (DataRow dr in dt_tpos2.Rows)
            {
                // Create and manually initialize the DataRow
                DataRow dr2 = t_pos2.NewRow();

                dr2["m_cabang"] = dr["m_cabang"].ToString();
                dr2["m_nomor"] = dr["m_nomor"].ToString();
                dr2["m_kodebarang"] = dr["m_kodebarang"].ToString();
                dr2["m_productid"] = dr["m_productid"].ToString();
                dr2["m_lokasi"] = dr["m_lokasi"].ToString();
                dr2["m_qty"] = Convert.ToInt32(dr["m_qty"]);
                dr2["m_harga"] = Math.Round(Convert.ToDecimal(dr["m_harga"]),2);
                dr2["m_disc"] = Math.Round(Convert.ToDecimal(dr["m_disc"]),2);
                dr2["m_discount"] = Math.Round(Convert.ToDecimal(dr["m_discount"]),2);
                dr2["m_discount2"] = Math.Round(Convert.ToDecimal(dr["m_discount2"]),2);
                dr2["m_discount3"] = Math.Round(Convert.ToDecimal(dr["m_discount3"]), 2);
                dr2["m_discount4"] = Math.Round(Convert.ToDecimal(dr["m_discount4"]), 2);
                dr2["m_kurs"] = DBNull.Value;
                dr2["m_rate"] = DBNull.Value;
                dr2["m_keterangan"] = DBNull.Value;
                dr2["m_status"] = dr["m_status"].ToString();
                dr2["m_poin"] = DBNull.Value;
                dr2["m_discount5"] = Math.Round(Convert.ToDecimal(dr["m_discount5"]), 2);
                dr2["m_discount6"] = Math.Round(Convert.ToDecimal(dr["m_discount6"]), 2);
                dr2["m_discount7"] = Math.Round(Convert.ToDecimal(dr["m_discount7"]), 2);
                dr2["m_ppn"] = Convert.ToDecimal(dr["m_ppn"]);
                dr2["m_hargam"] = DBNull.Value;
                dr2["isRepostingVoid"] = DBNull.Value;
                dr2["m_Extraringsize"] = dr["m_Extraringsize"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["m_Extraringsize"]);
                dr2["m_FeeExtraringSizePerSize"] = dr["m_FeeExtraringSizePerSize"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["m_FeeExtraringSizePerSize"]);
                dr2["m_total_bayar"] = dr["m_total_bayar"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["m_total_bayar"]);

                // Add the row to the table
                t_pos2.Rows.Add(dr2);
            }

            foreach(DataRow dr in dt_tpos3.Rows)
            {
                // Create and manually initialize the DataRow
                DataRow dr2 = t_pos3.NewRow();

                dr2["m_cabang"] = dr["m_cabang"].ToString();
                dr2["m_nomor"] = dr["m_nomor"].ToString();
                dr2["m_tanggal"] = Convert.ToDateTime(dr["m_tanggal"]);
                dr2["m_carabayar"] = dr["m_carabayar"].ToString();
                dr2["m_edc"] = dr["m_edc"].ToString();
                dr2["m_bank"] = dr["m_bank"].ToString();
                dr2["m_nokartu"] = dr["m_nokartu"].ToString();
                dr2["m_jnkartu"] = dr["m_jnkartu"].ToString();
                dr2["m_cclkartu"] = dr["m_cclkartu"].ToString();
                dr2["m_nmkartu"] = dr["m_nmkartu"].ToString();
                dr2["m_jumlah"] = Convert.ToDecimal(dr["m_jumlah"]);
                dr2["m_mdr"] = Convert.ToDecimal(dr["m_mdr"]);
                dr2["m_noinv"] = dr["m_noinv"] == DBNull.Value ? "" : dr["m_noinv"].ToString();
                dr2["m_id"] = Convert.ToInt32(dr["m_id"]);

                // Add the row to the table
                t_pos3.Rows.Add(dr2);
            }

            foreach(DataRow dr in dt_tpos4.Rows)
            {
                // Create and manually initialize the DataRow
                DataRow dr2 = t_pos4.NewRow();

                dr2["m_nomor"] = dr["m_nomor"].ToString();
                dr2["m_inv"] = dr["m_inv"].ToString();
                dr2["m_kodebarang"] = dr["m_kodebarang"].ToString();
                dr2["m_hpp"] = dr["m_hpp"];
                dr2["m_qty"] = dr["m_qty"];
                dr2["m_idtrans"] = dr["m_idtrans"];
                dr2["m_namabarang"] = dr["m_namabarang"];
                dr2["m_subled"] = dr["m_subled"];
                dr2["m_jenis"] = dr["m_jenis"];
                dr2["m_tanggal"] = Convert.ToDateTime(dr["m_tanggal"]);
                dr2["m_totalharga"] = Convert.ToDecimal(dr["m_totalharga"]);

                // Add the row to the table
                t_pos4.Rows.Add(dr2);
            }

            string query = "sp_getpos_new";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@t_pos", SqlDbType.Structured){ Value = t_pos },
                new SqlParameter("@t_pos2", SqlDbType.Structured){ Value = t_pos2 },
                new SqlParameter("@t_pos3", SqlDbType.Structured){ Value = t_pos3 },
                new SqlParameter("@t_pos4", SqlDbType.Structured){ Value = t_pos4 },
                new SqlParameter("@brand", SqlDbType.VarChar){ Value = brand }
            };

            dt = Rs(query, Db.Cnn_CS, parameters, CommandType.StoredProcedure);
        }

        // Root cause "There is no row at position 0" pada proses "Error Save To TPOS dbcmk":
        // kalau precondition (rowcount_tpos == 1 && rowcount_tpos2 > 0) di atas tidak terpenuhi,
        // ATAU sp_getpos_new tidak mengembalikan baris, dt akan tetap DataTable kosong (0 baris,
        // 0 kolom) — sementara caller (save_Click) langsung akses results.Rows[0]["SUCCESS"]
        // tanpa cek jumlah baris dulu, sehingga throw. Pastikan method ini SELALU mengembalikan
        // tepat 1 baris dengan kolom SUCCESS/ERROR, supaya kontrak yang diasumsikan caller
        // benar-benar terpenuhi.
        if (dt.Rows.Count == 0)
        {
            dt = new DataTable();
            dt.Columns.Add("SUCCESS", typeof(bool));
            dt.Columns.Add("ERROR", typeof(string));
            DataRow errRow = dt.NewRow();
            errRow["SUCCESS"] = false;
            errRow["ERROR"] = "Data Sales Order tidak lengkap untuk sinkronisasi ke TPOS (sp_displaydatatpos tidak mengembalikan data yang diharapkan, atau sp_getpos_new tidak mengembalikan hasil).";
            dt.Rows.Add(errRow);
        }

        return dt;
    }

    protected int savePayment(
            int xIDPaymentType,
            int xIDCard = 0,
            int xIDProgram = 0,
            decimal xMDR = 0,
            int xIDEDC = 0,
            int xIDBankIssuer = 0,
            string xCCNumber = "",
            string xCCName = "",
            decimal xNominal = 0,
            int xIDJenisKartuKredit = 0,
            int xECRFlag = 0,
            string xECRValue = ""
        )
    {
        Literal litMsg = lit_message_general;
        string msg = "";
        int result = 0;

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.Draft == false);
        if (d == null) DataError();
        else
        {

            // jika pengembalian cash dan tidak terdeteksi ada pembayaran lebih DP maka invalid Payment
            if (msg == "" && xIDPaymentType == 1 && d.Unpaid < 0)
            {
                var paymentdpexists = from p in db.SalesReceiptDetail
                                      where p.SalesReceipt.IDSalesOrder == d.ID && p.SalesReceipt.NoSalesOrder == d.Nomor
                                         && (p.IDPaymentType == 8 && p.Nominal > 0)
                                      select p
                                    ;
                if (paymentdpexists.Count() == 0)
                {
                    msg = "Pengembalian Cash hanya untuk pembayaran <b>Down Payment lebih bayar</b>.";
                }
            }

            // validasi jika sudah lunas dengan inputan pembayaran selain cash out
            if (msg == "" & (d.Unpaid.Value < 0 && xNominal > 0 && xIDPaymentType != 1))
            {
                msg = "Invoice sudah lunas / Nominal pembayaran melebihi Total Kurang Bayar";
            }

            if (msg != "")
            {
                msg = "Error : " + msg;
                litMsg.Text = msgValidationError(msg);
                enableButtonSubmit();

            }
            else
            {
                decimal TotalBayar = 0m;
                TotalBayar = xNominal;

                int IDSalesReceipt = 0;

                string NoInvoice = "";
                string NoDP = "";


                // add sales receipt header
                SalesReceipt s = new SalesReceipt();

                s.IDSalesOrder = myID;
                s.NoSalesOrder = d.Nomor;
                s.OperatorNama = App.UserNama;
                s.OperatorTgl = DateTime.Now;

                db.SalesReceipt.InsertOnSubmit(s);
                db.SubmitChanges();

                IDSalesReceipt = s.ID;

                // add sales receipt detail payment
                SalesReceiptDetail sd = new SalesReceiptDetail();

                sd.IDSalesReceipt = IDSalesReceipt;

                if (xIDCard != 0) { sd.IDCard = xIDCard; }
                if (xIDProgram != 0) { sd.IDProgram = xIDProgram; }
                if (xIDEDC != 0) { sd.IDEDC = xIDEDC; }
                if (xIDBankIssuer != 0) { sd.IDBankIssuer = xIDBankIssuer; }
                if (xIDJenisKartuKredit != 0) { sd.IDJenisKartuKredit = xIDJenisKartuKredit; }

                sd.MDR = xMDR;
                sd.IDPaymentType = xIDPaymentType;
                sd.CCNumber = xCCNumber.ToUpper();
                sd.CCName = xCCName.ToUpper();
                sd.Nominal = xNominal;
                sd.OperatorTgl = DateTime.Now;
                sd.OperatorNama = App.UserNama.ToUpper();

                // ECR
                //sd.Ecr_flag = xECRFlag;
                //sd.Ecr_value = xECRValue;

                if (xECRFlag != 0)
                {
                    sd.ECR_Verification = xECRFlag;
                    sd.ECR_ResponseValue = xECRValue;
                    sd.ECR_VerificationTgl = DateTime.Now;
                }

                // GC


                db.SalesReceiptDetail.InsertOnSubmit(sd);
                db.SubmitChanges();

                result = sd.ID; // return detail id of payment

                // sales receipt today information update
                // `s` sudah entity SalesReceipt yang barusan di-insert (ID == IDSalesReceipt) di
                // db context yang sama — re-fetch via SingleOrDefault cuma round-trip redundan
                // (identity map LINQ-to-SQL bakal balikin object yang sama juga). Begitu juga `sd`
                // adalah satu-satunya SalesReceiptDetail milik receipt ini (baru dibuat di atas),
                // jadi SUM-nya = sd.Nominal itu sendiri, tidak perlu query ulang.
                var x = s;
                var y = sd.Nominal;

                x.TotalBayar = y;
                db.SubmitChanges();

                // sales order information update
                var z = db.SalesReceipt.Where(p => p.IDSalesOrder == d.ID).Sum(p => p.TotalBayar);
                d.Paid = z.HasValue ? z : 0;
                d.Unpaid = d.TotalBayar - d.Paid;
                db.SubmitChanges();

                //firmand added 280818 pengambilan data dari user control
                paymentSummary.TotalTerbayar = Str.Number0(d.Paid);
                if (d.Unpaid == 0)
                {
                    paymentSummary.StatusPembayaran = "<font style='color:green;font-weight:bold';>Lunas</font>";
                }
                else if (d.Unpaid > 0)
                {
                    paymentSummary.StatusPembayaran = "<font style='color:red;font-weight:bold';>Belum Lunas</font>";
                }
                else if (d.Unpaid < 0)
                {
                    paymentSummary.StatusPembayaran = "<font style='color:red;font-weight:bold';>Lebih Bayar</font>";
                }

                spn_payment_amountpaid.InnerText = Str.Number0(d.Paid);

                if (d.Unpaid <= 0m)
                {
                    NoInvoice = Document.Generate_Nomor_Invoice(d);
                    //d.StatusPembayaran = true;
                    d.Tgl = (Module == "pos") ? DateTime.Today : d.Tgl;
                    d.TglLunas = (Module == "pos") ? DateTime.Today : d.Tgl;
                    x.NomorInvoice = NoInvoice;

                    decimal SisaDP = TotalBayar;

                    var dj = d.SalesOrderDJ;
                    foreach (var r in dj)
                    {
                        /*if (d.TradeIn == true)
                        {
                            r.StockProductDJ.StockActualDJ.Substorage = 3;
                            r.StockProductDJ.StatusPenjualan = 2;
                            r.StockProductDJ.StockActualDJ.InHand = 0;
                        }*/

                        decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                        if (SisaDP > 0)
                        {
                            if (TotalDP < r.TotalBayar)
                            {
                                if (SisaDP > r.TotalBayar)
                                {
                                    SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                    r.TotalDP = r.TotalBayar.Value;
                                }
                                else
                                {
                                    r.TotalDP = TotalDP + SisaDP;
                                    SisaDP = 0m;
                                }
                            }
                        }
                    }
                    var pg = d.SalesOrderPG;
                    foreach (var r in pg)
                    {
                        /*if (d.TradeIn == true)
                        {
                            r.StockProductPG.StockActualPG.Substorage = 3;
                            r.StockProductPG.StatusPenjualan = 2;
                            r.StockProductPG.StockActualPG.InHand = 0;
                        }*/

                        decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                        if (SisaDP > 0)
                        {
                            if (TotalDP < r.TotalBayar)
                            {
                                if (SisaDP > r.TotalBayar)
                                {
                                    SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                    r.TotalDP = r.TotalBayar.Value;
                                }
                                else
                                {
                                    r.TotalDP = TotalDP + SisaDP;
                                    SisaDP = 0m;
                                }
                            }
                        }
                    }
                    var gj = d.SalesOrderGJ;
                    foreach (var r in gj)
                    {
                        /*if (d.TradeIn == true)
                        {
                            r.StockProductGJ.StockActualGJ.Substorage = 3;
                            r.StockProductGJ.StatusPenjualan = 2;
                            r.StockProductGJ.StockActualGJ.InHand = 0;
                        }*/

                        decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                        if (SisaDP > 0)
                        {
                            if (TotalDP < r.TotalBayar)
                            {
                                if (SisaDP > r.TotalBayar)
                                {
                                    SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                    r.TotalDP = r.TotalBayar.Value;
                                }
                                else
                                {
                                    r.TotalDP = TotalDP + SisaDP;
                                    SisaDP = 0m;
                                }
                            }
                        }
                    }
                    var ld = d.SalesOrderLD;
                    foreach (var r in ld)
                    {
                        /*if (d.TradeIn == true)
                        {
                            r.StockProductLD_Stone1B.StockActualLD_Stone1B.Substorage = 3;
                            r.StockProductLD_Stone1B.StatusPenjualan = 2;
                            r.StockProductLD_Stone1B.StockActualLD_Stone1B.InHand = 0;
                        }*/

                        decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                        if (SisaDP > 0)
                        {
                            if (TotalDP < r.TotalBayar)
                            {
                                if (SisaDP > r.TotalBayar)
                                {
                                    SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                    r.TotalDP = r.TotalBayar.Value;
                                }
                                else
                                {
                                    r.TotalDP = TotalDP + SisaDP;
                                    SisaDP = 0m;
                                }
                            }
                        }
                    }
                    // Batch commit (Opsi A - eliminasi N+1 write): sebelumnya tiap item DJ/PG/GJ/LD
                    // di atas panggil db.SubmitChanges() sendiri-sendiri saat SisaDP dialokasikan ke
                    // item itu (bisa sampai puluhan round-trip DB kalau SO besar dilunasi sekaligus).
                    // Sekarang SEMUA perubahan r.TotalDP dari keempat tabel di atas cuma di-tracking
                    // di memory oleh DataContext, dan baru di-flush SEKALI di sini. Jangan tambahkan
                    // SubmitChanges() lagi di dalam loop manapun di atas kecuali ada baris SETELAHNYA
                    // yang query ulang ke DB dan butuh lihat perubahan itu sudah ke-commit (belum ada
                    // saat ini - alokasi SisaDP murni in-memory antar item/tabel).
                    db.SubmitChanges();
                }
                else
                {
                    NoDP = Document.Generate_Nomor_DownPayment(d);
                    //d.StatusPembayaran = false;
                    //d.TglLunas = null;
                    x.NomorDP = NoDP;

                    decimal SisaDP = TotalBayar;

                    // Batch commit (sama seperti cabang "lunas" di atas — Opsi A): dulu cabang DP ini
                    // luput dari perbaikan itu, tiap item DJ/PG/GJ/LD masih SubmitChanges() sendiri2
                    // saat SisaDP dialokasikan. Alokasi SisaDP di sini murni in-memory antar
                    // item/tabel — tidak ada baris setelahnya yang query ulang dan butuh lihat
                    // perubahan ini sudah ke-commit — jadi aman di-flush sekali di akhir saja.
                    var dj = d.SalesOrderDJ;
                    foreach (var r in dj)
                    {
                        //r.StockProductDJ.StatusPenjualan = 3;

                        decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                        if (SisaDP > 0)
                        {
                            if (TotalDP < r.TotalBayar)
                            {
                                if (SisaDP > r.TotalBayar)
                                {
                                    SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                    r.TotalDP = r.TotalBayar.Value;
                                }
                                else
                                {
                                    r.TotalDP = TotalDP + SisaDP;
                                    SisaDP = 0m;
                                }
                            }
                        }
                    }
                    var pg = d.SalesOrderPG;
                    foreach (var r in pg)
                    {
                        //r.StockProductPG.StatusPenjualan = 3;

                        decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                        if (SisaDP > 0)
                        {
                            if (TotalDP < r.TotalBayar)
                            {
                                if (SisaDP > r.TotalBayar)
                                {
                                    SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                    r.TotalDP = r.TotalBayar.Value;
                                }
                                else
                                {
                                    r.TotalDP = TotalDP + SisaDP;
                                    SisaDP = 0m;
                                }
                            }
                        }
                    }
                    var gj = d.SalesOrderGJ;
                    foreach (var r in gj)
                    {
                        //r.StockProductGJ.StatusPenjualan = 3;

                        decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                        if (SisaDP > 0)
                        {
                            if (TotalDP < r.TotalBayar)
                            {
                                if (SisaDP > r.TotalBayar)
                                {
                                    SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                    r.TotalDP = r.TotalBayar.Value;
                                }
                                else
                                {
                                    r.TotalDP = TotalDP + SisaDP;
                                    SisaDP = 0m;
                                }
                            }
                        }
                    }
                    var ld = d.SalesOrderLD;
                    foreach (var r in ld)
                    {
                        //r.StockProductLD_Stone1B.StatusPenjualan = 3;

                        decimal TotalDP = r.TotalDP.HasValue ? r.TotalDP.Value : 0m;

                        if (SisaDP > 0)
                        {
                            if (TotalDP < r.TotalBayar)
                            {
                                if (SisaDP > r.TotalBayar)
                                {
                                    SisaDP = TotalBayar - (r.TotalBayar.Value - TotalDP);
                                    r.TotalDP = r.TotalBayar.Value;
                                }
                                else
                                {
                                    r.TotalDP = TotalDP + SisaDP;
                                    SisaDP = 0m;
                                }
                            }
                        }
                    }
					
                    //Commit
                    db.SubmitChanges();
                }
            }
        }
        return result;
    }

    protected void redirectGoToPayment()
    {
        // Go to Payment
        Response.Redirect("/OutletSales/Page/Create/Payment/Payment_ItemList.aspx?id=" + myID
            + "&rnd=" + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(System.DateTime.Now.ToString())) + "#divrowpayment", false)
            ;
    }


    protected void printSalesOrder(int id)
    {

        string pageurl = "/OutletSales/Page/Create/Payment/Payment_ItemList.aspx?id=" + id;
        
        Response.Write("<script>");
        Response.Write("window.open('" + pageurl + "','_blank')");
        Response.Write("</script>");
    }

    protected void printCoupon(int id)
    {
        string spname = "[sp_create_coupon_prizedraw]";
        string query = " exec " + spname
                        + " 'SO'"
                        + ", " + id.ToString() + ""
                        ;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    string pageurl = dtRow["result"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
            }
        }
    }

    protected void printPayment(int id)
    {
        string query = "SELECT A.ID, B.ID [IDTradeIN], A.Nomor [NoSO], B.Nomor [NoTradeIn], A.Tgl [TglSO], B.IDResell FROM SalesOrder A " +
            "LEFT JOIN TradeIN B ON A.ID = B.IDSalesOrder WHERE A.StatusPembayaran = 1 AND A.StatusVoid = 0 " +
            "AND A.ID = @ID " +
            "SELECT ID FROM SalesOrderDJ WHERE IDForm = @ID " +
            "SELECT A.ID, C.NamaKode FROM SalesOrderPG A " +
            "JOIN StockProductPG B ON A.IDProduct = B.ID " +
            "JOIN CharGoldLevel C ON B.ProductLevel = C.ID WHERE IDForm = @ID " +
            "SELECT ID FROM SalesOrderGJ WHERE IDForm = @ID " +
            "SELECT ID FROM SalesOrderLD WHERE IDForm = @ID";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
        };
        DataSet ds = Ds(query, Db.Cnn_DB, parameters, CommandType.Text);
        DataTable dt = ds.Tables[0];
        DataTable dtdj = ds.Tables[1];
        DataTable dtpg = ds.Tables[2];
        DataTable dtgj = ds.Tables[3];
        DataTable dtld = ds.Tables[4];

        if (dt.Rows.Count == 1)
        {
            DateTime TglSO = Convert.ToDateTime(dt.Rows[0]["TglSO"]);
            int IDResell = dt.Rows[0]["IDResell"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["IDResell"]);
            bool isvoucherlei = false;
            if (IDResell > 0)
            {
                string pageurl2 = "/OutletSales/Page/Create/Resell/Print_Receipt.aspx?id=" + IDResell;
                Response.Write("<script>");
                Response.Write("window.open('" + pageurl2 + "','_blank')");
                Response.Write("</script>");
            }

            if (TglSO <= Convert.ToDateTime("2023-08-03 23:59:00.000"))
            {
                foreach (DataRow r in dtdj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceDJ_2023_08_02.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtpg.Rows)
                {
                    string level = r["NamaKode"].ToString().ToUpper();
                    if (level == "VLEI")
                    {
                        string pageurl = "../../Report/Print_InvoiceVoucherLEI.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");

                        isvoucherlei = true;
                    }
                    else
                    {
                        string pageurl = "../../Report/Print_InvoicePG_2023_08_02.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");
                    }
                }
                foreach (DataRow r in dtgj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceGJ_2023_08_02.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtld.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceLD_2023_08_02.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }

                if (isvoucherlei)
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_InvoiceVoucherLEI.aspx?id=" + dt.Rows[0]["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
                else
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_Invoice_2023_08_02.aspx?id=" + dt.Rows[0]["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
            }
            else if (TglSO <= Convert.ToDateTime("2023-10-22 23:59:00.000"))
            {
                foreach (DataRow r in dtdj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceDJ_2023_10_23.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtpg.Rows)
                {
                    string level = r["NamaKode"].ToString().ToUpper();
                    if (level == "VLEI")
                    {
                        string pageurl = "../../Report/Print_InvoiceVoucherLEI.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");

                        isvoucherlei = true;
                    }
                    else
                    {
                        string pageurl = "../../Report/Print_InvoicePG_2023_10_23.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");
                    }
                }
                foreach (DataRow r in dtgj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceGJ_2023_08_02.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtld.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceLD_2023_08_02.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }

                if (isvoucherlei)
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_InvoiceVoucherLEI.aspx?id=" + dt.Rows[0]["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
                else
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_Invoice_2023_08_02.aspx?id=" + dt.Rows[0]["ID"];
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
            }
            else if (TglSO <= Convert.ToDateTime("2025-03-04 23:59:00.000"))
            {
                foreach (DataRow r in dtdj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceDJ_2025_02_28.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtpg.Rows)
                {
                    string level = r["NamaKode"].ToString().ToUpper();
                    if (level == "VLEI")
                    {
                        string pageurl = "../../Report/Print_InvoiceVoucherLEI.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");

                        isvoucherlei = true;
                    }
                    else
                    {
                        string pageurl = "../../Report/Print_InvoicePG_2025_02_28.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");
                    }
                }
                foreach (DataRow r in dtgj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceGJ_2025_02_28.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtld.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceLD_2025_02_28.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }

                if (isvoucherlei)
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_InvoiceVoucherLEI.aspx?id=" + dt.Rows[0]["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
                else
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_Invoice.aspx?id=" + dt.Rows[0]["ID"];
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
            }
            else
            {
                foreach (DataRow r in dtdj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceDJ.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtpg.Rows)
                {
                    string level = r["NamaKode"].ToString().ToUpper();
                    if (level == "VLEI")
                    {
                        string pageurl = "../../Report/Print_InvoiceVoucherLEI.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");

                        isvoucherlei = true;
                    }
                    else
                    {
                        string pageurl = "../../Report/Print_InvoicePG.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");
                    }
                }
                foreach (DataRow r in dtgj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceGJ.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtld.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceLD.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }

                if (isvoucherlei)
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_InvoiceVoucherLEI.aspx?id=" + dt.Rows[0]["ID"];
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
                else
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_Invoice.aspx?id=" + dt.Rows[0]["ID"];
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
            }
        }
    }

    protected void confirm_readonly(bool read_only, string mode)
    {
        if (mode == "creditcard")
        {
            txt_creditcard_amount.ReadOnly = read_only;
        }
        else if (mode == "debitcard")
        {
            txt_debitcard_amount.ReadOnly = read_only;
        }
        else if (mode == "cash")
        {
            txt_cash_amount.ReadOnly = read_only;
        }
        else if (mode == "transfer")
        {
            txt_transfer_amount.ReadOnly = read_only;
        }
        else if (mode == "giftcard")
        {
            txt_giftcard_cardholdernumber.ReadOnly = read_only;
            txt_giftcard_amount.ReadOnly = read_only;
        }
        else if (mode == "bridestorypay")
        {
            txt_bridestorypay_amount.ReadOnly = read_only;
        }
    }

    protected void btn_giftcard_confirm_Click(object sender, EventArgs e)
    {
        div_giftcard_confirm.Visible = false;
        div_giftcard_submit.Visible = true;
        btn_giftcard_cancel.Visible = true;
        //btn_giftcard_confirm.Visible = false;
        confirm_readonly(true, "giftcard");
        //upl_giftcard.Update();
    }

    protected void btn_giftcard_cancel_Click(object sender, EventArgs e)
    {
        div_giftcard_confirm.Visible = true;
        div_giftcard_submit.Visible = false;
        btn_giftcard_cancel.Visible = false;
        //btn_giftcard_confirm.Visible = true;
        confirm_readonly(false, "giftcard");
        //upl_giftcard.Update();
    }
    protected string validSalesOrderPayment(decimal TotalBayar)
    {
        string result = "";
        // Materialize sekali — srd_50/srd_50_minus/srd_dv di bawah masing2 filter ulang srd, dan
        // tiap .Count()/.Sum()/.GroupBy() pada IQueryable yang sama jadi round-trip DB sendiri
        // (sampai 5x per call, dan method ini dipanggil 2x per siklus submit — page render +
        // saat submit — jadi sampai 10 round-trip total). LINQ-to-Objects di memori sesudah
        // materialize sekali menghasilkan hasil yang identik.
        var srd = db.SalesReceiptDetail.Where(p => p.SalesReceipt.IDSalesOrder == myID && !p.VoidReference.HasValue).ToList();

        // check if sum is more than 50 percent (50%)
        var srd_50 = srd.Where(p => p.Nominal > 0);
        var srd_50_minus = srd.Where(p => p.Nominal <= 0);

        if (srd_50.Count() > 0)
        {
            var srd_total = srd_50.Sum(p => p.Nominal);
            var srd_total_minus = srd_50_minus.Sum(p => p.Nominal);

            if (srd_total + srd_total_minus > (TotalBayar * 150 / 100)) { result += "Invalid Over Payment."; }
        }

        // check if not duplicated (nd)
        //var srd_nd = srd.Where(p => p.Nominal > 0).OrderByDescending(p => p.ID).Take(2);

        /*Hapus*/
        /*if (srd_nd.Count() == 2)
        {
            var srd1 = srd.OrderBy(p => p.OperatorTgl).First();
            var srd2 = srd.OrderByDescending(p => p.OperatorTgl).First();

            if (srd1.IDPaymentType == 5 && srd2.IDPaymentType == 5) { result += ""; }
            else
            {
                if (srd1.Nominal == srd2.Nominal) { result += ""; }//{ result += "Duplikasi pembayaran. "; }
            }
        }*/
        /*Hapus*/

        // check if not duplicated voucher payment
        var srd_dv = srd.Where(p => p.IDPaymentType == 5 && p.Nominal > 0);

        if (srd_dv.Count() > 0)
        {
            var query = srd_dv.GroupBy(x => x.CCNumber)
                      .Where(g => g.Count() > 1)
                      .Select(y => y.Key);

            if (query.Count() > 0)
            {
                result += "Duplikasi Voucher Payment. ";
            }
        }

        return result;
    }

    protected string validSalesOrderPaymentBeforeSubmit(decimal TotalBayar)
    {
        string result = "";
        // Materialize sekali — srd_50/srd_50_minus/srd_dv di bawah masing2 filter ulang srd, dan
        // tiap .Count()/.Sum()/.GroupBy() pada IQueryable yang sama jadi round-trip DB sendiri
        // (sampai 5x per call, dan method ini dipanggil 2x per siklus submit — page render +
        // saat submit — jadi sampai 10 round-trip total). LINQ-to-Objects di memori sesudah
        // materialize sekali menghasilkan hasil yang identik.
        var srd = db.SalesReceiptDetail.Where(p => p.SalesReceipt.IDSalesOrder == myID && !p.VoidReference.HasValue).ToList();

        // check if sum is more than 50 percent (50%)
        var srd_50 = srd.Where(p => p.Nominal > 0);
        var srd_50_minus = srd.Where(p => p.Nominal <= 0);

        if (srd_50.Count() > 0)
        {
            var srd_total = srd_50.Sum(p => p.Nominal);
            var srd_total_minus = srd_50_minus.Sum(p => p.Nominal);

            if (srd_total + srd_total_minus > (TotalBayar * 150 / 100)) { result += "Invalid Over Payment."; }
        }

        // check if not duplicated (nd)
        //var srd_nd = srd.Where(p => p.Nominal > 0).OrderByDescending(p => p.ID).Take(2);

        /*Hapus*/
        /*if (srd_nd.Count() == 2)
        {
            var srd1 = srd.OrderBy(p => p.OperatorTgl).First();
            var srd2 = srd.OrderByDescending(p => p.OperatorTgl).First();

            if (srd1.IDPaymentType == 5 && srd2.IDPaymentType == 5) { result += ""; }
            else
            {
                if (srd1.Nominal == srd2.Nominal) { result += ""; }//{ result += "Duplikasi pembayaran. "; }
            }
        }*/
        /*Hapus*/

        // check if not duplicated voucher payment
        var srd_dv = srd.Where(p => p.IDPaymentType == 5 && p.Nominal > 0);

        if (srd_dv.Count() > 0)
        {
            var query = srd_dv.GroupBy(x => x.CCNumber)
                      .Where(g => g.Count() > 1)
                      .Select(y => y.Key);

            if (query.Count() > 0)
            {
                result += "Duplikasi Voucher Payment. ";
            }
        }

        if (ValidasiBeforeSubmit())
            result += "Total Bayar Tidak Sama Dengan Jumlah Yang Dibayarkan. Mohon Refresh Halaman Ulang.";

        return result;
    }

    protected void UpdateStatusPLU(int idso)
    {
        string query = "sp_update_status_plu_salesorder";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = idso }
        };
        Execute(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
    }

    /*Simplified Cara Bayar DP Atau Diskon Voucher dan Payment Voucher*/
    protected string UpdateStatusPaymentType(SalesOrder d)
    {
        string result = string.Empty;
        /*string query = "sp_get_payment_dp_voucher";
        DataSet ds = Ds(query, Db.Cnn_DB, new List<SqlParameter> { new SqlParameter("@ID", SqlDbType.Int) { Value = d.ID } }, CommandType.StoredProcedure);
        DataTable dtdp = ds.Tables[0];
        DataTable dtvoucherpayment = ds.Tables[1];
        DataTable dtvoucherdiskon = ds.Tables[2];

        foreach (DataRow r in dtvoucherpayment.Rows)
        {
            string NoVoucher = r["CCNUMBER"].ToString();
            // redeem stamps
            try
            {
                int id_tipelokasi = d.TipeLokasi.Value;
                int id_lokasi = d.IDLokasi.Value;
                int id_brand = 0;
                string kode_brand = "";

                if (id_tipelokasi == 1)
                {
                    var loc = db.LocOutlet.Single(p => p.ID == id_lokasi);

                    id_brand = loc.IDBrand;
                    kode_brand = loc.CompanyBrand.NamaKode;
                }
                else if (id_tipelokasi == 2)
                {
                    var loc = db.LocExhibition.Single(p => p.ID == id_lokasi);

                    id_brand = loc.IDBrand;
                    kode_brand = loc.CompanyBrand.NamaKode;
                }

                if (STAMPS.xActivate)
                {
                    result += STAMPSM.RedeemVoucherCode(d, NoVoucher, kode_brand, "");
                }
            }
            catch (Exception ex) { result += ex.Message; }

        }

        foreach (DataRow r in dtvoucherdiskon.Rows)
        {

            string NoVoucher = r["VCNUMBER"].ToString();
            string PLU = r["PLU"].ToString();
            // redeem stamps
            try
            {
                int id_tipelokasi = d.TipeLokasi.Value;
                int id_lokasi = d.IDLokasi.Value;
                int id_brand = 0;
                string kode_brand = "";

                if (id_tipelokasi == 1)
                {
                    var loc = db.LocOutlet.Single(p => p.ID == id_lokasi);

                    id_brand = loc.IDBrand;
                    kode_brand = loc.CompanyBrand.NamaKode;
                }
                else if (id_tipelokasi == 2)
                {
                    var loc = db.LocExhibition.Single(p => p.ID == id_lokasi);

                    id_brand = loc.IDBrand;
                    kode_brand = loc.CompanyBrand.NamaKode;
                }

                if (STAMPS.xActivate)
                {
                    result += STAMPSM.RedeemVoucherCode(d, NoVoucher, kode_brand, PLU);
                }
            }
            catch (Exception ex) { result += ex.Message; }

        }*/

        string query_get_voucher = @"
            SELECT ISNULL(A.Nomor,'') [NoSalesOrder], ISNULL(C.CCNumber,'') [NomorVoucher], C.Nominal, ISNULL(A1.Kode, '') [Store], A.OperatorTgl [TglSalesOrder] FROM SalesOrder A WITH (NOLOCK)
            JOIN (SELECT ID, 1 Tipe, Kode FROM LocOutlet WITH (NOLOCK) UNION ALL SELECT ID, 2 Tipe, Kode FROM LocExhibition WITH (NOLOCK)) A1 ON A.IDLokasi = A1.ID AND A.TipeLokasi = A1.Tipe
            JOIN SalesReceipt B WITH (NOLOCK) ON A.ID = B.IDSalesOrder
            JOIN SalesReceiptDetail C WITH (NOLOCK) ON B.ID = C.IDSalesReceipt
            WHERE A.ID = @ID AND C.IDPaymentType = 5
            AND VoidReference IS NULL

            UNION ALL

            SELECT ISNULL(B.Nomor,'') [NoSalesOrder], A.NomorVoucher, A.Nominal, ISNULL(B1.Kode, '') [Store], B.OperatorTgl [TglSalesOrder] FROM SalesVoucher A WITH (NOLOCK)
            JOIN SalesOrder B WITH (NOLOCK) ON A.IDSalesOrder = B.ID
            JOIN (SELECT ID, 1 Tipe, Kode FROM LocOutlet WITH (NOLOCK) UNION ALL SELECT ID, 2 Tipe, Kode FROM LocExhibition WITH (NOLOCK)) B1 ON B.IDLokasi = B1.ID AND B.TipeLokasi = B1.Tipe
            WHERE A.IDSalesOrder = @ID
            ";
        List<SqlParameter> param = new List<SqlParameter>()
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = d.ID }
        };
        DataTable dtvoucher = Rs(query_get_voucher, Db.Cnn_DB, param, CommandType.Text);
        DataTable dtvoucher_add = new DataTable("UDT_KlaimDocVoucher");
        dtvoucher_add.Columns.Add("NoSalesOrder", typeof(string));
        dtvoucher_add.Columns.Add("NomorVoucher", typeof(string));
        dtvoucher_add.Columns.Add("Nominal", typeof(decimal));
        dtvoucher_add.Columns.Add("TglSalesOrder", typeof(DateTime));
        dtvoucher_add.Columns.Add("Store", typeof(string));
        foreach (DataRow dr in dtvoucher.Rows)
        {
            DataRow newRow = dtvoucher_add.NewRow();

            newRow["NoSalesOrder"] = dr["NoSalesOrder"];
            newRow["NomorVoucher"] = dr["NomorVoucher"];
            newRow["Nominal"] = dr["Nominal"];
            newRow["TglSalesOrder"] = dr["TglSalesOrder"];
            newRow["Store"] = dr["Store"];
            dtvoucher_add.Rows.Add(newRow);
        }

        if (string.IsNullOrEmpty(result))
        {
            string queryupdate = "sp_update_status_payment_dp_voucher";
            Execute(queryupdate, Db.Cnn_DB,
                new List<SqlParameter> { new SqlParameter("@ID", SqlDbType.Int) { Value = d.ID } },
                CommandType.StoredProcedure);
        }

        /*if (string.IsNullOrEmpty(result))
        {
            string query_update_klaimdoc = @"sp_update_klaimdoc_voucher_pelunasan";
            List<SqlParameter> paramklaim = new List<SqlParameter>()
            {
                new SqlParameter("@Data", SqlDbType.Structured) { Value = dtvoucher_add }
            };
            Execute(query_update_klaimdoc, Db.Cnn_CS, paramklaim, CommandType.StoredProcedure);
        }*/

        return result;
    }

    protected void InsertLogError(int id, string proses, string keterangan)
    {
        string query = "sp_insert_log_salesorder";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@IDSO", SqlDbType.Int){ Value = id },
            new SqlParameter("@PROSES", SqlDbType.VarChar){ Value = proses },
            new SqlParameter("@KETERANGAN", SqlDbType.VarChar){ Value = keterangan }
        };
        Execute(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
    }

    public class LogSalesOrder
    {
        public string no_so { get; set; }
        public List<LogSalesOrderData> data { get; set; }
    }

    public class LogSalesOrderData
    {
        public int no { get; set; }
        public string proses { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public string keterangan { get; set; }
        public int lead_time_seconds { get; set; }
        public bool success { get; set; }
    }

    protected void save_Click(object sender, EventArgs e)
    {
        string status_SAP = "";
        string status_TPOS = "";

        LogSalesOrder log = new LogSalesOrder();

        LogSalesOrderData data = new LogSalesOrderData();
        log.data = new List<LogSalesOrderData>();
        DateTime start = DateTime.Now;
        var d0 = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        DateTime end = DateTime.Now;

        data.no = 1;
        data.proses = "Search Data Sales Order.";
        data.start_time = start.ToString("s");
        data.end_time = end.ToString("s");
        data.keterangan = "";
        data.lead_time_seconds = Convert.ToInt32((end - start).TotalSeconds);
        data.success = true;

        log.no_so = d0.Nomor;
        log.data.Add(data);

        var d = Rs("SELECT COUNT(*) [Total] FROM SalesOrder WHERE ID = @ID AND Draft = 0 AND StatusPembayaran = 0",
            Db.Cnn_DB, new List<SqlParameter>() { new SqlParameter("@ID", SqlDbType.Int) { Value = myID } }, CommandType.Text);

        int rowcount = d.Rows.Count;
        try
        {
            if (rowcount == 0) 
            { 
                DataError();
                LogSalesOrderData data_error_1 = new LogSalesOrderData();
                data_error_1.no = 999;
                data_error_1.proses = "Cek Validate Data Sales Order.";
                data_error_1.start_time = "";
                data_error_1.end_time = "";
                data_error_1.keterangan = "Error Data Sales Order Tidak Ditemukan";
                data_error_1.lead_time_seconds = 0;
                data_error_1.success = true;
                log.data.Add(data_error_1);
            }
            else
            {
                LogSalesOrderData data1 = new LogSalesOrderData();
                DateTime start1 = DateTime.Now;
                var d1 = Rs("SELECT TOP 1 A.ID,  ISNULL(A.TotalBayar,0) [TotalBayar], ISNULL(A.TradeIn,0) TradeIn, ISNULL(B.ID, 0) [IDTradeIn], ISNULL(B.Nomor, '') [NomorTradeIn] FROM SalesOrder A LEFT JOIN TradeIn B ON A.ID = B.IDSalesOrder WHERE A.ID = @ID",
                            Db.Cnn_DB, new List<SqlParameter>() { new SqlParameter("@ID", SqlDbType.Int) { Value = myID } }, CommandType.Text);
                DateTime end1 = DateTime.Now;
                data1.no = 2;
                data1.proses = "Search Data Trade In When Submit Sales Order.";
                data1.start_time = start1.ToString("s");
                data1.end_time = end1.ToString("s");
                data1.keterangan = "";
                data1.lead_time_seconds = Convert.ToInt32((end1 - start1).TotalSeconds);
                data1.success = true;
                log.data.Add(data1);

                decimal TotalBayar = Convert.ToDecimal(d1.Rows[0]["TotalBayar"]);
                string errorPayment = "";
                LogSalesOrderData data2 = new LogSalesOrderData();
                DateTime start2 = DateTime.Now;
                errorPayment += validSalesOrderPaymentBeforeSubmit(TotalBayar);
                DateTime end2 = DateTime.Now;
                data2.no = 3;
                data2.proses = "Validate Data Sales Order.";
                data2.start_time = start2.ToString("s");
                data2.end_time = end2.ToString("s");
                data2.keterangan = "";
                data2.lead_time_seconds = Convert.ToInt32((end2 - start2).TotalSeconds);
                data2.success = true;
                log.data.Add(data2);

                string errorVoucher = "";
                errorVoucher += UpdateStatusPaymentType(d0);

                if (errorPayment == "" && errorVoucher == "")
                {
                    /*int idsalesorder = d.ID;
                    string email = d.DataCustomer.AddrEmail;
                    string invoice_number = d.Nomor;
                    string date = Convert.ToDateTime(d.OperatorTgl).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    string handphone = d.DataCustomer.AddrNoTelp;*/
                    //This Method Update StatusPmebayaran to 1 AND UpdateStatus PLU Too
                    LogSalesOrderData data3 = new LogSalesOrderData();
                    DateTime start3 = DateTime.Now;
                    UpdateStatusPLU(myID);
                    DateTime end3 = DateTime.Now;
                    data3.no = 4;
                    data3.proses = "Update Data Sales Order Status Pembayaran to 1 and PLU.";
                    data3.start_time = start3.ToString("s");
                    data3.end_time = end3.ToString("s");
                    data3.keterangan = "";
                    data3.lead_time_seconds = Convert.ToInt32((end3 - start3).TotalSeconds);
                    data3.success = true;
                    log.data.Add(data3);

                    //RedeemKetupatEmas(idsalesorder, email, invoice_number, date, handphone);
                    try
                    {
                        string brand = ConfigurationManager.AppSettings["KodeBrandMyApps"];
                        LogSalesOrderData data4 = new LogSalesOrderData();
                        DateTime start4 = DateTime.Now;
                        DataTable results = SaveToTPOS(brand);
                        DateTime end4 = DateTime.Now;
                        data4.no = 5;
                        data4.proses = "Save To TPOS dbcmk.";
                        data4.start_time = start4.ToString("s");
                        data4.end_time = end4.ToString("s");
                        data4.keterangan = "";
                        data4.lead_time_seconds = Convert.ToInt32((end4 - start4).TotalSeconds);
                        data4.success = true;
                        log.data.Add(data4);

                        // SaveToTPOS() sekarang dijamin selalu mengembalikan minimal 1 baris, tapi
                        // guard ini tetap dipasang di titik pemakaiannya (bukan cuma di sumbernya)
                        // supaya tidak crash lagi kalau suatu saat ada jalur lain yang memanggil
                        // SaveToTPOS tanpa lewat perbaikan itu.
                        if (results.Rows.Count > 0)
                        {
                            bool success = Convert.ToBoolean(results.Rows[0]["SUCCESS"]);
                            string message = results.Rows[0]["ERROR"].ToString();

                            if (!success)
                            {
                                lit_error_payment.Text = message;
                            }
                        }
                        else
                        {
                            lit_error_payment.Text = "Data hasil sinkronisasi TPOS tidak ditemukan.";
                        }

                        if (Convert.ToBoolean(d1.Rows[0]["TradeIn"]) == true)
                        {
                            try
                            {
                                int IDTradeIn = Convert.ToInt32(d1.Rows[0]["IDTradeIn"]);
                                string NomorTradeIn = d1.Rows[0]["NomorTradeIn"].ToString();
                                LogSalesOrderData data5 = new LogSalesOrderData();
                                DateTime start5 = DateTime.Now;
                                Payment.savetposasynchronous_ti(IDTradeIn, NomorTradeIn, brand);
                                DateTime end5 = DateTime.Now;
                                data5.no = 6;
                                data5.proses = "Save To Trade In dbcmk.";
                                data5.start_time = start5.ToString("s");
                                data5.end_time = end5.ToString("s");
                                data5.keterangan = "";
                                data5.lead_time_seconds = Convert.ToInt32((end5 - start5).TotalSeconds);
                                data5.success = true;
                                log.data.Add(data5);
                            }
                            catch (Exception ex)
                            {
                                LogSalesOrderData data_error_0 = new LogSalesOrderData();
                                data_error_0.no = 999;
                                data_error_0.proses = "Error Save To TradeIn dbcmk.";
                                data_error_0.start_time = "";
                                data_error_0.end_time = "";
                                data_error_0.keterangan = ex.Message;
                                data_error_0.lead_time_seconds = 0;
                                data_error_0.success = false;
                                log.data.Add(data_error_0);

                                lit_error_payment.Text = ex.Message;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogSalesOrderData data_error_1 = new LogSalesOrderData();
                        data_error_1.no = 999;
                        data_error_1.proses = "Error Save To TPOS dbcmk.";
                        data_error_1.start_time = "";
                        data_error_1.end_time = "";
                        data_error_1.keterangan = ex.Message;
                        data_error_1.lead_time_seconds = 0;
                        data_error_1.success = false;
                        log.data.Add(data_error_1);
                        lit_error_payment.Text = ex.Message;
                    }

                    // Validation and SAP Posting
                    /*try
					{
						bool isDoneSAP = false;
						if (SAP.xActivate)
						{
							status_SAP += ((status_SAP == "") ? SAPM.postPaymentSalesOrderToSAP(d, ref isDoneSAP, lit_error_sap, lit_keypostsap) : "");
							status_SAP += ((status_SAP == "" && !isDoneSAP) ? SAPM.isPaymentLockedBySAP(d) : "");
						}

					}
					catch (Exception ex)
					{
						status_SAP = "Terjadi kesalahan teknis. Silahkan hubungi IT Support. (SAP Error message : " + ex.Message + ")";
					}*/

                    // Validation and STAMPS Posting — deferred to a background work item so the
                    // response is not blocked on the STAMPS HTTP call (synchronous HttpWebRequest,
                    // 8s timeout — root cause of the non-linear latency under concurrent load).
                    // Payment status is already committed above (UpdateStatusPLU), so STAMPS/reward
                    // posting must not gate the response. StampsBacklogWorker still catches any SO
                    // left without a StampsTransactionID (sp_stamps_transaction_new) if this
                    // background attempt fails, so no transaction is silently lost.
                    if (STAMPS.xActivate)
                    {
                        int soIdForStamps = myID;
                        System.Web.Hosting.HostingEnvironment.QueueBackgroundWorkItem(ct =>
                        {
                            // Retry sekali kalau kena connection-state error (indikasi koneksi dari
                            // pool sedang dalam keadaan tidak konsisten, bukan error dari data/logic).
                            // Aman dari double-redeem: STAMPS API sendiri unique-check di
                            // invoice_number — respons "already exists" sudah ditangani sebagai
                            // sukses di dalam STAMPSM.AddTransaction, jadi retry tidak akan membuat
                            // transaksi/redeem dobel di sisi STAMPS.
                            for (int attempt = 1; attempt <= 2; attempt++)
                            {
                                try
                                {
                                    using (Map_DB bgDb = new Map_DB())
                                    {
                                        var soBg = bgDb.SalesOrder.SingleOrDefault(p => p.ID == soIdForStamps);
                                        if (soBg != null)
                                        {
                                            STAMPSM.prePostSalesOrderIntegration(soBg);
                                            STAMPSM.AddTransaction(new Literal(), soBg);
                                        }
                                    }
                                    break; // sukses, tidak perlu retry
                                }
                                catch (Exception ex)
                                {
                                    // Exception filter (catch...when) tidak dipakai karena project ini
                                    // cuma punya compiler C# 5 (tidak ada paket Roslyn/CodeDom provider) —
                                    // "when" baru ada di C# 6, tidak akan compile di server ini.
                                    bool isConnectionStateError = ex.Message != null
                                        && ex.Message.Contains("connection's current state is open");

                                    if (attempt == 1 && isConnectionStateError)
                                    {
                                        continue; // lanjut ke attempt ke-2 dengan koneksi baru
                                    }

                                    // gagal permanen (bukan connection-state, atau sudah retry sekali)
                                    // — StampsBacklogWorker retries unposted SO every 5 min
                                    break;
                                }
                            }
                        });
                    }

                    LogSalesOrderData data6 = new LogSalesOrderData();
                    data6.no = 7;
                    data6.proses = "Send Transaction This SO To Stamps.";
                    data6.start_time = DateTime.Now.ToString("s");
                    data6.end_time = data6.start_time;
                    data6.keterangan = "Deferred to background work item (HostingEnvironment.QueueBackgroundWorkItem), no longer blocking save_Click.";
                    data6.lead_time_seconds = 0;
                    data6.success = true;
                    log.data.Add(data6);

                    status_SAP = ""; // matiin error SAP.
                    if (errorPayment == "" && status_SAP == "")
                    {
                        printPayment(myID); // print sales order and invoice per item
                        printCoupon(myID); // print coupon based on store proc condition

                        string pageurl = "/OutletSales/Page/Create/Payment/Payment_ItemList.aspx?id=" + myID;
                        Response.Write("<script>");
                        Response.Write("window.location.href = '" + pageurl + "';");
                        Response.Write("</script>");
                    }
                    else
                    {
                        /*try
						{
							if (SAP.xActivate)
							{
								status_SAP += SAPM.releaseSAPLockedByFailureSAPPosting(d);
							}
						}
						catch (Exception) { }*/

                        status_SAP = ""; // matiin error SAP.
                        lit_error_payment.Text += (errorPayment == "") ? "" : "<div class='alert alert-danger' role='alert'>" + errorPayment + "</div>";
                        lit_error_sap.Text += (status_SAP == "") ? "" : "<div class='alert alert-danger' role='alert'>" + status_SAP + "</div>";

                        if (errorPayment != "")
                        {
                            LogSalesOrderData data_error_4 = new LogSalesOrderData();
                            data_error_4.no = 999;
                            data_error_4.proses = "Cek Error Payment Data Sales Order.";
                            data_error_4.start_time = "";
                            data_error_4.end_time = "";
                            data_error_4.keterangan = errorPayment;
                            data_error_4.lead_time_seconds = 0;
                            data_error_4.success = false;
                            log.data.Add(data_error_4);
                        }
                    }
                }
                else
                {
                    LogSalesOrderData data_error_5 = new LogSalesOrderData();
                    data_error_5.no = 999;
                    data_error_5.proses = "Cek Error Voucher.";
                    data_error_5.start_time = "";
                    data_error_5.end_time = "";
                    data_error_5.keterangan = errorVoucher;
                    data_error_5.lead_time_seconds = 0;
                    data_error_5.success = false;
                    log.data.Add(data_error_5);

                    lit_error_payment.Text = (errorPayment == "") ? "" : "<div class='alert alert-danger' role='alert'>" + errorPayment + "</div>";
                    lit_error_stamps.Text = (errorVoucher == "") ? "" : "<div class='alert alert-danger' role='alert'>" + errorVoucher + "</div>";
                }
            }
            try
            {
                LogSalesOrderData data6 = new LogSalesOrderData();
                DateTime start6 = DateTime.Now;
                DateTime end6 = DateTime.Now;
                data6.no = 8;
                data6.proses = "Send All Pending Transaction To Stamps.";
                data6.start_time = start6.ToString("s");
                data6.end_time = end6.ToString("s");
                data6.keterangan = "Moved to background StampsBacklogWorker timer (runs every 5 min), no longer executed inline in save_Click.";
                data6.lead_time_seconds = 0;
                data6.success = true;
                log.data.Add(data6);
            }
            catch (Exception ex) 
            {
                LogSalesOrderData data_error_4 = new LogSalesOrderData();
                data_error_4.no = 999;
                data_error_4.proses = "Cek Error Payment Data Sales Order.";
                data_error_4.start_time = "";
                data_error_4.end_time = "";
                data_error_4.keterangan = ex.Message;
                data_error_4.lead_time_seconds = Convert.ToInt32((end - start).TotalSeconds);
                data_error_4.success = false;
                log.data.Add(data_error_4);
            }
            for (int i = tb3.Rows.Count - 1; i >= 1; i--)
            {
                tb3.Rows.RemoveAt(i);
            }
            FillPayment(myID, tb3);
        }
        catch(Exception ex)
        {
            LogSalesOrderData data_error_4 = new LogSalesOrderData();
            data_error_4.no = 999;
            data_error_4.proses = "Cek Error Payment Data Sales Order.";
            data_error_4.start_time = "";
            data_error_4.end_time = "";
            data_error_4.keterangan = ex.Message;
            data_error_4.lead_time_seconds = 0;
            data_error_4.success = false;
            log.data.Add(data_error_4);
            lit_error_stamps.Text = "<div class='alert alert-danger' role='alert'>Terjadi kesalahan teknis. Silahkan hubungi IT Support! " + ex.Message + "</div>";
        }

        if (log.data.Count > 0)
        {
            InsertLogError(myID, "Submit SO", JsonConvert.SerializeObject(log.data));
        }
    }

    protected void print_Click(object sender, EventArgs e)
    {
        string query = "SELECT A.ID, B.ID [IDTradeIN], A.Nomor [NoSO], B.Nomor [NoTradeIn], A.Tgl [TglSO], B.IDResell FROM SalesOrder A " +
            "LEFT JOIN TradeIN B ON A.ID = B.IDSalesOrder WHERE A.StatusPembayaran = 1 AND A.StatusVoid = 0 " +
            "AND A.ID = @ID " +
            "SELECT ID FROM SalesOrderDJ WHERE IDForm = @ID " +
            "SELECT A.ID, C.NamaKode FROM SalesOrderPG A " +
            "JOIN StockProductPG B ON A.IDProduct = B.ID " +
            "JOIN CharGoldLevel C ON B.ProductLevel = C.ID WHERE IDForm = @ID " +
            "SELECT ID FROM SalesOrderGJ WHERE IDForm = @ID " +
            "SELECT ID FROM SalesOrderLD WHERE IDForm = @ID";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID", SqlDbType.Int){ Value = myID }
        };
        DataSet ds = Ds(query, Db.Cnn_DB, parameters, CommandType.Text);
        DataTable dt = ds.Tables[0];
        DataTable dtdj = ds.Tables[1];
        DataTable dtpg = ds.Tables[2];
        DataTable dtgj = ds.Tables[3];
        DataTable dtld = ds.Tables[4];

        if (dt.Rows.Count == 1)
        {
            DateTime TglSO = Convert.ToDateTime(dt.Rows[0]["TglSO"]);
            int IDResell = dt.Rows[0]["IDResell"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[0]["IDResell"]);
            bool isvoucherlei = false;
            if (IDResell > 0)
            {
                string pageurl2 = "/OutletSales/Page/Create/Resell/Print_Receipt.aspx?id=" + IDResell;
                Response.Write("<script>");
                Response.Write("window.open('" + pageurl2 + "','_blank')");
                Response.Write("</script>");
            }

            if (TglSO <= Convert.ToDateTime("2023-08-03 23:59:00.000"))
            {
                foreach (DataRow r in dtdj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceDJ_2023_08_02.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtpg.Rows)
                {
                    string level = r["NamaKode"].ToString().ToUpper();
                    if (level == "VLEI")
                    {
                        string pageurl = "../../Report/Print_InvoiceVoucherLEI.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");

                        isvoucherlei = true;
                    }
                    else
                    {
                        string pageurl = "../../Report/Print_InvoicePG_2023_08_02.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");
                    }
                }
                foreach (DataRow r in dtgj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceGJ_2023_08_02.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtld.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceLD_2023_08_02.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }

                if (isvoucherlei)
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_InvoiceVoucherLEI.aspx?id=" + dt.Rows[0]["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
                else
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_Invoice_2023_08_02.aspx?id=" + dt.Rows[0]["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
            }
            else if (TglSO <= Convert.ToDateTime("2023-10-22 23:59:00.000"))
            {
                foreach (DataRow r in dtdj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceDJ_2023_10_23.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtpg.Rows)
                {
                    string level = r["NamaKode"].ToString().ToUpper();
                    if (level == "VLEI")
                    {
                        string pageurl = "../../Report/Print_InvoiceVoucherLEI.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");

                        isvoucherlei = true;
                    }
                    else
                    {
                        string pageurl = "../../Report/Print_InvoicePG_2023_10_23.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");
                    }
                }
                foreach (DataRow r in dtgj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceGJ_2023_08_02.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtld.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceLD_2023_08_02.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }

                if (isvoucherlei)
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_InvoiceVoucherLEI.aspx?id=" + dt.Rows[0]["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
                else
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_Invoice_2023_08_02.aspx?id=" + dt.Rows[0]["ID"];
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
            }
            else if (TglSO <= Convert.ToDateTime("2025-03-04 23:59:00.000"))
            {
                foreach (DataRow r in dtdj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceDJ_2025_02_28.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtpg.Rows)
                {
                    string level = r["NamaKode"].ToString().ToUpper();
                    if (level == "VLEI")
                    {
                        string pageurl = "../../Report/Print_InvoiceVoucherLEI.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");

                        isvoucherlei = true;
                    }
                    else
                    {
                        string pageurl = "../../Report/Print_InvoicePG_2025_02_28.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");
                    }
                }
                foreach (DataRow r in dtgj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceGJ_2025_02_28.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtld.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceLD_2025_02_28.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }

                if (isvoucherlei)
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_InvoiceVoucherLEI.aspx?id=" + dt.Rows[0]["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
                else
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_Invoice.aspx?id=" + dt.Rows[0]["ID"];
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
            }
            else
            {
                foreach (DataRow r in dtdj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceDJ.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtpg.Rows)
                {
                    string level = r["NamaKode"].ToString().ToUpper();
                    if (level == "VLEI")
                    {
                        string pageurl = "../../Report/Print_InvoiceVoucherLEI.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");

                        isvoucherlei = true;
                    }
                    else
                    {
                        string pageurl = "../../Report/Print_InvoicePG.aspx?id=" + r["ID"].ToString();
                        Response.Write("<script>");
                        Response.Write("window.open('" + pageurl + "','_blank')");
                        Response.Write("</script>");
                    }
                }
                foreach (DataRow r in dtgj.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceGJ.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                foreach (DataRow r in dtld.Rows)
                {
                    string pageurl = "../../Report/Print_InvoiceLD.aspx?id=" + r["ID"].ToString();
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }

                if (isvoucherlei)
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_InvoiceVoucherLEI.aspx?id=" + dt.Rows[0]["ID"];
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
                else
                {
                    string pageurl3 = "/OutletSales/Page/Create/Payment/Sales_Invoice.aspx?id=" + dt.Rows[0]["ID"];
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl3 + "','_blank')");
                    Response.Write("</script>");
                }
            }
        }
    }

    //Redirect Payment List
    protected void redirect_payment_list_Click(object sender, EventArgs e)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && !p.Draft); // no-draft !!!
        if (d == null) { DataError(); }
        else
        {
            Response.Redirect("/OutletSales/Page/Create/Payment/Payment.aspx");
        }
    }

    //Redirect Summary
    protected void redirect_summary_Click(object sender, EventArgs e)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && !p.Draft); // no-draft !!!
        if (d == null) { DataError(); }
        else
        {
            Response.Redirect("/OutletSales/Page/Summary.aspx");
        }
    }

    //verifygiftcard_Click
    protected void verifygiftcard_Click(object sender, EventArgs e)
    {
        string[] response3;

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && !p.Draft); // no-draft !!!
        if (d == null) { DataError(); }
        else
        {
            var x = db.SalesReceiptDetail.Where(p => p.SalesReceipt.SalesOrder.ID == d.ID
                && p.IDPaymentType == 9
                && p.SalesReceipt.SalesOrder.StatusVoid == false
                && p.GC_VoidVerification == null
                && p.GC_VoidVerificationTgl == null
                && p.Nominal > 0
                );

            if (x.Count() > 0)
            {
                // hoisted out of the loop: d (and its location) never changes across rows,
                // this used to re-query LocOutlet/LocExhibition on every iteration
                var s = d;
                string loc = "000";
                if (s.TipeLokasi == 1) { loc = db.LocOutlet.Single(p => p.ID == s.IDLokasi).Kode; }
                if (s.TipeLokasi == 2) { loc = db.LocExhibition.Single(p => p.ID == s.IDLokasi).Kode; }

                foreach (var r in x)
                {
                    Literal litMsg = new Literal();

                    string xcardnumber = r.CCNumber;
                    string xinvoicenumber = d.Nomor;
                    string xpayment_id = r.ID.ToString();
                    string xtype = "SALE"; // SALE , VOID
                    string xamount = r.Nominal.Value.ToString("N0").Replace(".", "");
                    string xbranch_id = loc;
                    string xpayment_method = "Gift Card";
                    string xuser_cashier = r.SalesReceipt.OperatorNama;

                    ////////// response 3
                    response3 = (RESTClient.postTransactionSale(litMsg, xinvoicenumber, xpayment_id, xcardnumber, xtype, xamount, xbranch_id, xpayment_method, xuser_cashier)).Split('-');

                    if (response3[0].Trim() == "1310" || response3[0].Trim() == "1330")
                    {
                        // Rubah Flag Verifikasi Pembayaran
                        r.GC_Verification = true;
                        r.GC_VerificationTgl = System.DateTime.Now;
                        db.SubmitChanges();

                        redirectGoToPayment();

                    }
                }

            }


        }
    }

    protected void displayError_SAP(string msg)
    {
        lit_error_sap.Text = "<div class='alert alert-danger'>" + msg + "</div>";
    }

    protected void displayWarning_Payment(string msg)
    {
        lit_warning_payment.Text = "<div class='alert alert-warning'>" + msg + "</div>";
    }

    protected void displayError_Payment(string msg)
    {
        lit_error_payment.Text = "<div class='alert alert-danger'>" + msg + "</div>";
    }

    protected void displayError_Giftcard(string msg)
    {
        lit_error_giftcard.Text = "<div class='alert alert-danger'>" + msg + "</div>";
    }

    protected void SentAllTransactionToStamps()
    {
        string query = "EXEC sp_stamps_transaction_new";
        DataTable dt = new DataTable();
        dt = Db.Rs(query, Db.Cnn_DB);

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                int id = Convert.ToInt32(dr["ID"]);
                var d = db.SalesOrder.SingleOrDefault(p => p.ID == id);
                try
                {
                    STAMPSM.prePostSalesOrderIntegration(d);
                    STAMPSM.AddTransaction(new Literal(), d);
                }
                catch { }
            }
        }
    }

    protected void FillPayment(int myID, Table tbl)
    {
        decimal totalPaid = 0m;

        // DataContext terpisah (bukan `db` milik class) khusus untuk query ini, supaya LoadOptions
        // bisa diset — LoadOptions HARUS diset SEBELUM DataContext dipakai query apa pun, dan `db`
        // (shared) sudah dipakai query lain sebelum FillPayment() dipanggil di request yang sama.
        // Eager-load PaymentType/CardType/JenisKartuKredit/EDCList/BankIssuer/ProgramCicilan/
        // SalesReceiptDetailExtension di sini (1 JOIN besar) — dulu masing2 lazy-load per baris di
        // loop render di bawah (sampai 7 nav property × N baris = N+1 dikonfirmasi evaluasi).
        Map_DB dbFillPayment = new Map_DB();
        DataLoadOptions loadOptions = new DataLoadOptions();
        loadOptions.LoadWith<SalesReceiptDetail>(p => p.PaymentType);
        loadOptions.LoadWith<SalesReceiptDetail>(p => p.CardType);
        loadOptions.LoadWith<SalesReceiptDetail>(p => p.JenisKartuKredit);
        loadOptions.LoadWith<SalesReceiptDetail>(p => p.EDCList);
        loadOptions.LoadWith<SalesReceiptDetail>(p => p.BankIssuer);
        loadOptions.LoadWith<SalesReceiptDetail>(p => p.ProgramCicilan);
        loadOptions.LoadWith<SalesReceiptDetail>(p => p.SalesReceiptDetailExtension);
        loadOptions.LoadWith<SalesReceiptDetail>(p => p.SalesReceipt);
        loadOptions.LoadWith<SalesReceipt>(p => p.SalesOrder);
        dbFillPayment.LoadOptions = loadOptions;

        // Materialize sekali - rs cuma dibaca lewat .Count() lalu foreach di bawah, tanpa .ToList()
        // query yang sama re-execute ke DB dua kali (sekali untuk Count, sekali untuk foreach).
        var rs = (from p in dbFillPayment.SalesReceiptDetail
                 where p.SalesReceipt.IDSalesOrder.Value == myID
                 //orderby p.SalesReceipt.ID ascending
                 //orderby p.VoidReference ascending
				 orderby p.ID ascending
                 select p).ToList();

        if (rs.Count() == 0) App.TableNoResult(tbl);
        else
        {
            int index = 1;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "middle");
                tr.Cells.Add(td);

                string xheader = "";
                string xicon = "";
                string xinfo = "";

                string wlabel = "100";
                string sTitle = "";
                string sContent = "";

                /*
                1	Cash
                2	Credit Card
                3	Transfer
                4	Debit Card
                5	Voucher
                6	Exchange
                7	Poin
                8	Down Payment
                9	Gift Card
                10  Bridestory Pay
                */

                if (r.IDPaymentType == 2)
                {
                    //credit card
                    xicon = "<span class='glyphicon glyphicon-credit-card' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "Card Name", (r.CCName));
                    sContent += popoverRowContent(wlabel, "Card Number", (r.CCNumber));
                    sContent += popoverRowContent(wlabel, "Card Type", (r.IDCard.HasValue ? r.CardType.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "Card Category", (r.IDJenisKartuKredit.HasValue ? r.JenisKartuKredit.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "EDC", (r.IDEDC.HasValue ? r.EDCList.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "Bank Issuer", (r.IDBankIssuer.HasValue ? r.BankIssuer.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "Program", (r.IDProgram.HasValue ? r.ProgramCicilan.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "MDR", Str.Number0(r.MDR));

                    sContent += popoverRowContent(wlabel, "Approval Code", (r.SalesReceiptDetailExtension != null) ? r.SalesReceiptDetailExtension.TransactionNumber : "");

                }
                else if (r.IDPaymentType == 3)
                {
                    //transfer
                    xicon = "<span class='glyphicon glyphicon-transfer' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "Account Name", (r.CCName));
                    sContent += popoverRowContent(wlabel, "Account Number", (r.CCNumber));
                    sContent += popoverRowContent(wlabel, "Bank", (r.IDBankIssuer.HasValue ? r.BankIssuer.Nama : " - "));

                }
                else if (r.IDPaymentType == 4)
                {
                    //debit card
                    xicon = "<span class='glyphicon glyphicon-credit-card' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "Card Name", (r.CCName));
                    sContent += popoverRowContent(wlabel, "Card Number", (r.CCNumber));
                    sContent += popoverRowContent(wlabel, "Card Type", (r.IDCard.HasValue ? r.CardType.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "Card Category", (r.IDJenisKartuKredit.HasValue ? r.JenisKartuKredit.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "EDC", (r.IDEDC.HasValue ? r.EDCList.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "Bank Issuer", (r.IDBankIssuer.HasValue ? r.BankIssuer.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "MDR", Str.Number0(r.MDR));

                    sContent += popoverRowContent(wlabel, "Approval Code", (r.SalesReceiptDetailExtension != null) ? r.SalesReceiptDetailExtension.TransactionNumber : "");

                }
                else if (r.IDPaymentType == 9)
                {
                    //gift card
                    xicon = "<span class='glyphicon glyphicon-credit-card' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "Card Number", (r.CCNumber));

                }
                else if (r.IDPaymentType == 1 || r.IDPaymentType == 12)
                {
                    // cash
                    xicon = "<span class='icon-money' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                }
                else if (r.IDPaymentType == 6)
                {
                    // tradein / exchange
                    xicon = "<span class='glyphicon glyphicon-ok' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                }
                else if (r.IDPaymentType == 8)
                {
                    // down payment
                    xicon = "<span class='glyphicon glyphicon-download-alt' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "DP Number", (r.CCNumber));
                }
                else if (r.IDPaymentType == 5)
                {
                    // voucher
                    xicon = "<span class='icon-ticket' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "Voucher Number", (r.CCNumber));
                    sContent += popoverRowContent(wlabel, "Voucher Type", (r.CCName));
                }
                else if (r.IDPaymentType == 10)
                {
                    var z = Common.SplitInParts(r.SalesReceiptDetailExtension.TransactionNumber.Trim(), 4);

                    //bridestorypay
                    xicon = "<span class='glyphicon glyphicon-phone' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "Account Name", (r.CCName));
                    sContent += popoverRowContent(wlabel, "Account Number", (r.CCNumber));
                    sContent += popoverRowContent(wlabel, "Bank Issuer", (r.IDBankIssuer.HasValue ? r.BankIssuer.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "BSPAY #", ((String.Join("-", z))));


                }
                else if (r.IDPaymentType == 11)
                {
                    // lakuemas
                    xicon = "<span class='icon-inbox' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;
                    sContent += popoverRowContent(wlabel, "No.Laku Emas", (r.CCNumber));
                }
                else if (r.IDPaymentType == 13)
                {
                    // qris
                    xicon = "<span class='glyphicon glyphicon-qrcode' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;
                    //string qristype = Db.SingleString("SELECT TOP 1 ISNULL(B.Nama, '-') [QRISType] FROM SalesReceiptDetail A LEFT JOIN QRISType B ON A.IDQRIS = B.ID WHERE A.ID = " + r.ID, Db.Cnn_DB);
                    //string qrisbankissuer = Db.SingleString("SELECT TOP 1 ISNULL(B.Nama, '-') [QRISBankIssuer] FROM SalesReceiptDetail A LEFT JOIN QRISBankIssuer B ON A.IDQRISBankIssuer = B.ID WHERE A.ID = " + r.ID, Db.Cnn_DB);

                    sContent += popoverRowContent(wlabel, "Approval Code", (r.CCNumber));
                    sContent += popoverRowContent(wlabel, "Customer", (r.CCName));
                    //sContent += popoverRowContent(wlabel, "QRIS Bank Issuer", (qrisbankissuer));
                    //sContent += popoverRowContent(wlabel, "QRIS Type", (qristype));
                    sContent += popoverRowContent(wlabel, "QRIS Bank Issuer", (r.IDBankIssuer.HasValue ? r.BankIssuer.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "QRIS Type", (r.IDEDC.HasValue ? r.EDCList.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "MDR", Str.Number0(r.MDR));
                }
                else if (r.IDPaymentType == 14)
                {
                    // qris
                    xicon = "<span class='glyphicon glyphicon-phone' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "Tipe", (r.CCName));
                    sContent += popoverRowContent(wlabel, "Trans. ID", (r.CCNumber));
                    sContent += popoverRowContent(wlabel, "Bank Issuer", (r.IDBankIssuer.HasValue ? r.BankIssuer.Nama : " - "));
                }
                else if (r.IDPaymentType == 15)
                {
                    // qris
                    xicon = "<span class='glyphicon glyphicon-qrcode' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;
                    //string qristype = Db.SingleString("SELECT TOP 1 ISNULL(B.Nama, '-') [QRISType] FROM SalesReceiptDetail A LEFT JOIN QRISType B ON A.IDQRIS = B.ID WHERE A.ID = " + r.ID, Db.Cnn_DB);
                    //string qrisbankissuer = Db.SingleString("SELECT TOP 1 ISNULL(B.Nama, '-') [QRISBankIssuer] FROM SalesReceiptDetail A LEFT JOIN QRISBankIssuer B ON A.IDQRISBankIssuer = B.ID WHERE A.ID = " + r.ID, Db.Cnn_DB);

                    sContent += popoverRowContent(wlabel, "Approval Code", (r.CCNumber));
                    sContent += popoverRowContent(wlabel, "Customer", (r.CCName));
                    sContent += popoverRowContent(wlabel, "QRIS Bank Issuer", (r.IDBankIssuer.HasValue ? r.BankIssuer.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "QRIS Type", (r.IDEDC.HasValue ? r.EDCList.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "MDR", Str.Number0(r.MDR));
                }
                else if (r.IDPaymentType == 16)
                {
                    // e-commerce
                    xicon = "<span class='glyphicon glyphicon-globe' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "Platform", (r.EDCList.Nama));
                    sContent += popoverRowContent(wlabel, "Trans. Name", (r.CCName));
                    sContent += popoverRowContent(wlabel, "Trans. ID", (r.CCNumber));
                }
                else if (r.IDPaymentType == 19)
                {
                    // e-commerce
                    xicon = "<span class='glyphicon glyphicon-globe' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;


                    string input = r.CCName;
                    // Split the string by the delimiter "___"
                    string[] parts = input.Split(new string[] { "___" }, StringSplitOptions.None);

                    sContent += popoverRowContent(wlabel, "Nama & Paket Arisan", (parts[0]));
                    sContent += popoverRowContent(wlabel, "Gelombang Arisan / PCS Paket", (r.CCNumber));
                    sContent += popoverRowContent(wlabel, "Subsidi", (parts[1]));
                }
                else
                {
                    // Unknown Payment !!!

                }

                if (sContent != "")
                {
                    xinfo += "<button style='border: 0;padding: 0;' type='button' class='btn btn-default btn-xs pull-left'";
                    xinfo += "data-html='true' data-container='body' data-toggle='popover' data-placement='bottom' data-content=\"" + sContent + "\">";
                    xinfo += "<span style='color:blue;'>&nbsp;Show / Hide Detail...&nbsp;</span>";
                    xinfo += "</button>";
                }

                xheader += "<span class='pull-left' style='text-align:left;'>" + xicon + "</span>";
                xheader += "<span class='pull-right' style='text-align:left;'>" + Str.TglJam(r.OperatorTgl) + "</span>";
                xheader += "<div style='clear:both; border-bottom:solid 1px silver;'></div>";
                xheader += xinfo;
                xheader += "<span class='text-muted pull-right' style='text-align:right;'>PAY # " + (r.ID).ToString() + "</span>";

                if (r.VoidReference.HasValue)
                {
                    xheader += "<span class='pull-right' style='text-align:right; color:red;'>VOID # " + (r.VoidReference.Value).ToString() + "&nbsp;</span>";

                }

                td = new TableCell();
                td.Text = xheader;
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                totalPaid += r.Nominal.Value;

                td = new TableCell();
                td.Text = r.Nominal.Value.ToString("N0");
                td.CssClass = "text-md";
                td.Style.Add("vertical-align", "middle");
                if (r.Nominal.Value < 0)
                {
                    td.Style.Add("color", "red");
                }
                else if (r.Nominal.Value > 0)
                {
                    td.Style.Add("color", "green");
                }
                else
                {
                    td.Style.Add("color", "black");
                }
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                if (r.SalesReceipt.SalesOrder.Tgl.Value.Date == DateTime.Now.Date && r.SalesReceipt.SalesOrder.StatusPembayaran == false)
                {
                    if (!r.VoidReference.HasValue && r.SalesReceipt.SalesOrder.StatusPembayaran == false)
                    {
                        //Exchange
                        if (r.IDPaymentType != 6)
                        {
                            td.Text = "<a href=\"../Payment/Payment_Void.aspx?id=" + myID + "&idpayment=" + r.ID.ToString() + " \" class=\"btn btn-danger\" onclick=\"showOverlay();\" ><i class=\"glyphicon glyphicon-remove\"></i></a>";
                        }
                        else
                        {
                            td.Text = "Exchange Can't be Void";
                        }
                    }
                    else
                    {
                        td.Text = "";
                    }
                }
                else
                {
                    td.Text = "";
                }
                td.CssClass = "text-md";
                td.Style.Add("vertical-align", "middle");
                tr.Cells.Add(td);

                tbl.Rows.Add(tr);

                index++;
            }

            Sub_Total_Paid(totalPaid, tbl);
        }

        // Dispose di sini (bukan langsung setelah .ToList()) — masih ada nav property yang diakses
        // sepanjang loop render di atas (r.SalesReceipt.SalesOrder dkk); dispose lebih awal bikin
        // ObjectDisposedException begitu ada akses yang belum ke-cover LoadWith di atas.
        dbFillPayment.Dispose();
    }
    protected string popoverRowContent(string w, string x, string y)
    {
        string z = "";

        z += "<div style='float:left; width:" + w + "px;'>" + x + "</div>"
            + "<div style='float:left;'>&nbsp;=&nbsp;</div>"
            + "<div style='float:left;'> " + y + "</div>"
            ;

        z = "<div>" + z + "<div style='clear:both;'></div></div>";

        return z;
    }

    protected void Sub_Total_Paid(decimal TotalPaid, Table tbl)
    {
        TableRow tr = new TableRow();
        TableCell td;

        td = new TableCell();
        td.ColumnSpan = 2;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += "Total Paid";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(TotalPaid) + "</p>";
        td.CssClass = "text-right";
        tr.Cells.Add(td);


        tbl.Rows.Add(tr);
    }

    protected void IsDJOnly(SalesOrder so)
    {
        int sogj = so.SalesOrderGJ.Count;
        int sopg = so.SalesOrderPG.Count;
        int sold = so.SalesOrderLD.Count;
        int total = sogj + sopg + sold;
        if (total > 0)
        {
            enable_Voucher(false);
        }
    }
    protected bool IsPGOnly(SalesOrder so)
    {
        bool istrue = false;
        int sogj = so.SalesOrderGJ.Count;
        int sodj = so.SalesOrderDJ.Count;
        int sold = so.SalesOrderLD.Count;
        //int total = sogj + sodj + sold;
        int total = sodj + sold + sogj;
        if (total == 0)
        {
            istrue = true;
        }
        return istrue;
    }
    protected void CheckDebitLEIOnly(bool IsNotSameDay, SalesOrder so)
    {
        var sr = so.SalesReceipt;
        int countdebitlei = 0;
        if (sr.Count() > 0)
        {
            foreach (var x in sr)
            {
                foreach (var srd in x.SalesReceiptDetail)
                {
                    if (srd.IDPaymentType == 4 && (srd.IDEDC == 26 || srd.IDEDC == 27))
                    {
                        countdebitlei++;
                    }
                }
            }
            if (IsNotSameDay)
            {
                enable_CreditCard(false);
                enable_DebitCard(false);
                enable_Cash(false);
                enable_DownPayment(false);
                enable_Transfer(false);
                enable_Voucher(false);
                enable_GiftCard(false); // belum jalan per 2018-11-02
                enable_BrideStoryPay(false);
                enable_LakuEmas(false); //Tambahan by Andri Priyono
            }
            else
            {
                if (countdebitlei > 0 && so.Unpaid > 0)
                {
                    enable_CreditCard(true);
                    enable_DebitCard(true);
                    enable_Cash(true);
                    enable_DownPayment(true);
                    enable_Transfer(true);
                    enable_Voucher(true);
                    enable_GiftCard(false); // belum jalan per 2018-11-02
                    enable_BrideStoryPay(true);
                    enable_LakuEmas(true); //Tambahan by Andri Priyono
                }
            }
        }
    }
    protected bool DifferencePGLMOngkos(bool isNotSameDay, decimal Unpaid, decimal AbsUnpaid, SalesOrder x)
    {
        bool isEnabled = false;
        var pg = x.SalesOrderPG;
        decimal weight_pg = 0m;
        decimal weight_lm = 0m;
        decimal rate_lm = 0m;
        try
        {
            rate_lm = Convert.ToDecimal(GetLakuEmasPriceAPI());//Db.SingleDecimal("SELECT TOP 1 m_beli FROM msrate WHERE m_kode = 'LKLM' ORDER BY m_tanggal DESC ", Db.Cnn_CS);
        }
        catch { }
        decimal harga_pg = 0m;
        decimal harga_lm = 0m;
        decimal ongkos = 0m;
        foreach (var item in pg)
        {
            decimal tgpjual = GetTGPPGJual(item.StockProductPG);
            decimal weight_pgdetail = item.StockProductPG.NettoWeight ?? 0m;
            decimal value_pgdetail = item.TotalBayar ?? 0m;
            decimal weight_lmdetail = (75 * weight_pgdetail) / 100;
            decimal value_lmdetail = weight_lmdetail * tgpjual;

            ongkos += value_pgdetail - value_lmdetail;
            weight_pg += weight_pgdetail;
            harga_pg += value_pgdetail;
            weight_lm += weight_lmdetail;
            harga_lm += value_lmdetail;
        }
        //weight_lm = (3 * weight_pg) / 4;
        //harga_lm = weight_lm * rate_lm;
        //harga_pg = Unpaid;
        //ongkos = harga_pg - harga_lm;

        txt_pg_weight.Text = weight_pg.ToString("N4");
        txt_lm_weight.Text = weight_lm.ToString("N4");
        txt_pg_amount.Text = harga_pg.ToString("N0");
        txt_lm_amount.Text = harga_lm.ToString("N0");
        txt_ongkos_amount.Text = ongkos.ToString("N0");
        txt_debitlakuemas_rate.Text = rate_lm.ToString("N0");
        txt_debitlakuemas_lm_amount.Text = Math.Ceiling(harga_lm).ToString("N0");
        txt_debitlakuemas_ongkos_amount.Text = Math.Floor(ongkos).ToString("N0");
        txt_debitlakuemas_amount.Text = (Math.Ceiling(harga_lm) + Math.Floor(ongkos)).ToString("N0");
        hf_ecr_paiddclakuemas_value.Value = harga_pg.ToString("N0");

        txt_creditlakuemas_rate.Text = rate_lm.ToString("N0");
        txt_creditlakuemas_lm_amount.Text = Math.Ceiling(harga_lm).ToString("N0");
        txt_creditlakuemas_ongkos_amount.Text = Math.Floor(ongkos).ToString("N0");
        txt_creditlakuemas_amount.Text = (Math.Ceiling(harga_lm) + Math.Floor(ongkos)).ToString("N0");
        hf_ecr_paidcclakuemas_value.Value = harga_pg.ToString("N0");

        div_debit_lei.InnerHtml = DetailDebitLakuEmas(x);
        div_credit_lei.InnerHtml = DetailDebitLakuEmas(x);

        if (ongkos < 0m)
        {
            save_payment_debitlakuemas.Enabled = false;
            save_payment_debitlakuemas.Attributes["title"] = "Can't use this payment method. Switch to other payment methods.";
            isEnabled = false;
            //enable_DebitLakuEmas(false);
        }
        else
        {
            isEnabled = true;
            //enable_DebitLakuEmas(true);
        }
        return isEnabled;
    }

    protected bool CountTotalPaymentWithLEI(int idso)
    {
        int countPayment = Db.SingleInteger("EXEC sp_check_payment_using_lei " + idso, Db.Cnn_DB);
        if (countPayment != 0)
            return true;
        else
            return false;
    }

    protected void savePaymentDebitLakuEmas(SalesOrder x, int sd)
    {
        var pg = x.SalesOrderPG;
        decimal Unpaid = x.Unpaid.Value;
        decimal AbsUnpaid = (Unpaid < 0) ? (-1 * Unpaid) : Unpaid;

        decimal weight_pg = 0m;
        decimal weight_lm = 0m;
        decimal rate_lm = 0m;
        RateLM lm = new RateLM();

        try
        {
            lm = GetLakuEmasAllPriceAPI();
            rate_lm = lm.mid_price;
        }
        catch { }
        decimal harga_pg = 0m;
        decimal harga_lm = 0m;
        decimal ongkos = 0m;
        string PLU = "";
        foreach (var item in pg)
        {
            decimal TGPJual = GetTGPPGJual(item.StockProductPG);
            decimal TGPBeli = GetTGPPGBeli(item.StockProductPG);
            weight_pg += item.StockProductPG.NettoWeight ?? 0m;
            PLU += item.StockProductPG.Nomor + ",";

            int countform = Db.SingleInteger("SELECT COUNT('') FROM SalesOrderPGLEI WHERE IDSalesOrder = " + item.IDForm + " AND IDProduct = " + item.IDProduct + " ", Db.Cnn_DB);
            if (countform == 0)
                Db.Execute("INSERT INTO SalesOrderPGLEI(IDProduct,IDSalesOrder,TGPJual,TGPBeli,OperatorTgl) VALUES(" + item.IDProduct + "," + item.IDForm + ",'" + TGPJual + "','" + TGPBeli + "', GETDATE()) ", Db.Cnn_DB);
        }
        PLU = PLU.TrimEnd(',');
        weight_lm = (3 * weight_pg) / 4;
        harga_lm = weight_lm * rate_lm;
        harga_pg = Unpaid;
        ongkos = harga_pg - harga_lm;
        Db.Execute("EXEC sp_update_debit_laku_emas_payment " + sd + ",'" + PLU + "', " + rate_lm + ", " + lm.buy_price + ", " + lm.sell_price + ", " + lm.mid_price + " ", Db.Cnn_DB);
    }

    protected void savePaymentLakuTukar(SalesOrder x, int sd, string nolakuemas)
    {
        var pg = x.SalesOrderPG;
        decimal Unpaid = x.Unpaid.Value;
        decimal AbsUnpaid = (Unpaid < 0) ? (-1 * Unpaid) : Unpaid;

        decimal weight_pg = 0m;

        RateLM lm = new RateLM();

        try
        {
            lm = GetLakuEmasAllPriceAPI();
            Db.Execute("UPDATE SalesReceiptDetailLakuEmas SET RateLEIBuy = '" + lm.buy_price + "', RateLEIMid = '" + lm.mid_price + "' , RateLEISell = '" + lm.sell_price + "' WHERE NoLakuEmas = '" + nolakuemas + "' ", Db.Cnn_DB);
        }
        catch { }

        string PLU = "";
        foreach (var item in pg)
        {
            decimal TGPJual = GetTGPPGJual(item.StockProductPG);
            decimal TGPBeli = GetTGPPGBeli(item.StockProductPG);
            weight_pg += item.StockProductPG.NettoWeight ?? 0m;
            PLU += item.StockProductPG.Nomor + ",";

            int countform = Db.SingleInteger("SELECT COUNT('') FROM SalesOrderPGLEI WHERE IDSalesOrder = " + item.IDForm + " AND IDProduct = " + item.IDProduct + " ", Db.Cnn_DB);
            if (countform == 0)
                Db.Execute("INSERT INTO SalesOrderPGLEI(IDProduct,IDSalesOrder,TGPJual,TGPBeli,OperatorTgl) VALUES(" + item.IDProduct + "," + item.IDForm + ",'" + TGPJual + "','" + TGPBeli + "', GETDATE()) ", Db.Cnn_DB);
        }
    }

    protected string DetailDebitLakuEmas(SalesOrder d)
    {
        string div = "";
        decimal? totalongkos = 0m;
        decimal? totalamount = 0m;
        var pg = d.SalesOrderPG;
        foreach (var x in pg)
        {
            decimal TGPJual = GetTGPPGJual(x.StockProductPG);
            decimal? weightpg75 = x.StockProductPG.NettoWeight;
            decimal? valuepg75 = x.TotalBayar;
            decimal? weightlm = (x.StockProductPG.NettoWeight * 75) / 100;
            decimal? valuelm = ((x.StockProductPG.NettoWeight * 75) / 100) * TGPJual;
            decimal? ongkos = valuepg75 - valuelm;

            string plu = "<div class=\"form-group col-xs-12\" >" +
                         "<label class=\"col-xs-11\"><strong>" + x.StockProductPG.Nomor + "(" + x.StockProductPG.CharProductItem.Nama.ToUpper() + "-" + x.StockProductPG.CharFrameColor.Nama.ToUpper() + ")</strong></label>" +
                         "</div>";

            string weight_pg = "<div class=\"form-group col-xs-12\" style=\"padding-bottom:10px;\">" +
                                "<label class=\"col-xs-3\">Weight PG 75%</label>" +
                                "<label class=\"col-xs-1\">:</label>" +
                                "<div class=\"col-xs-7\">" +
                                  "<div class=\"col-xs-8\">" +
                                    "<label style=\"text-align:center\">" + weightpg75.ToString().Remove(weightpg75.ToString().Length - 2) + "</label>" +
                                  "</div>&nbsp; gr" +
                                "</div>" +
                               "</div>";

            string value_pg = "<div class=\"form-group col-xs-12\" style=\"padding-bottom:10px;\">" +
                                "<label class=\"col-xs-3\">Value PG 75%</label>" +
                                "<label class=\"col-xs-1\" style=\"margin-left:0px;margin-right:0px\">:</label>" +
                                "<div class=\"col-xs-7\">" +
                                  "<div class=\"col-xs-8\">" +
                                    "<label style=\"text-align:center\">" + valuepg75.ToString().Remove(valuepg75.ToString().Length - 5) + "</label>" +
                                  "</div>&nbsp; IDR" +
                                "</div>" +
                               "</div>";

            string weight_lm = "<div class=\"form-group col-xs-12\" style=\"padding-bottom:10px;\">" +
                                "<label class=\"col-xs-3\">Weight LM</label>" +
                                "<label class=\"col-xs-1\">:</label>" +
                                "<div class=\"col-xs-7\">" +
                                   "<div class=\"col-xs-8\">" +
                                      "<label style=\"text-align:center\">" + weightlm + "</label>" +
                                   "</div>&nbsp; gr" +
                                "</div>" +
                               "</div>";
            string value_lm = "<div class=\"form-group col-xs-12\" style=\"padding-bottom:10px;\">" +
                                "<label class=\"col-xs-3\">Value LM</label>" +
                                "<label class=\"col-xs-1\">:</label>" +
                                "<div class=\"col-xs-7\">" +
                                    "<div class=\"col-xs-8\">" +
                                       "<label style=\"text-align:center\">" + valuelm.ToString().Remove(valuelm.ToString().Length - 5) + "</label>" +
                                    "</div>&nbsp; IDR" +
                                "</div>" +
                               "</div>";
            string tgp_jual_palace = "<div class=\"form-group col-xs-12\" style=\"padding-bottom:10px;\">" +
                                "<label class=\"col-xs-3\">TGP Jual</label>" +
                                "<label class=\"col-xs-1\">:</label>" +
                                "<div class=\"col-xs-7\">" +
                                    "<div class=\"col-xs-8\">" +
                                       "<label style=\"text-align:center\">" + TGPJual.ToString() + "</label>" +
                                    "</div>&nbsp; IDR" +
                                "</div>" +
                               "</div>";
            string ongkos_sisa = "<div class=\"form-group col-xs-12\" style=\"padding-bottom:10px;\">" +
                                "<label class=\"col-xs-3\">Ongkos</label>" +
                                    "<label class=\"col-xs-1\">:</label>" +
                                    "<div class=\"col-xs-7\">" +
                                        "<div class=\"col-xs-8\">" +
                                        "<label style=\"text-align:center\">" + ongkos.ToString().Remove(ongkos.ToString().Length - 5) + "</label>" +
                                        "</div>&nbsp; IDR" +
                                    "</div>" +
                                "</div><hr/>";

            totalongkos += ongkos;
            totalamount += valuepg75;
            div += plu + weight_pg + value_pg + weight_lm + value_lm + tgp_jual_palace + ongkos_sisa;
        }
        return div;
    }

    protected void EDCListDebit(DropDownList c, bool ispgonly = false)
    {
        string query = "SELECT ID, Nama FROM EDCList WHERE (@ISPGONLY = 0 AND ID IN (1, 2, 3, 13, 10)) " +
            "OR " +
            "(@ISPGONLY = 1 AND ID IN (26, 27, 28))";

        int ispgonlyparam = ispgonly ? 1 : 0;
        DataTable dt = Rs(query, Db.Cnn_DB, new List<SqlParameter>() { new SqlParameter("@ISPGONLY", SqlDbType.Int) { Value = ispgonlyparam } }, CommandType.Text);
        foreach (DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            c.Items.Add(new ListItem(t, v));
        }

    }

    protected void QRISList(DropDownList c)
    {
        string query = "SELECT ID, Nama FROM EDCList WHERE Nama IN ('QRIS BCA','QRIS BNI','QRIS MANDIRI','QRIS BRI')";
        DataTable dt = Rs(query, Db.Cnn_DB, null, CommandType.Text);
        foreach (DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            c.Items.Add(new ListItem(t, v));
        }
    }

    protected void QRISLEIList(DropDownList c)
    {
        string query = "SELECT ID, Nama FROM EDCList WHERE Nama = 'QRIS BCA LEI' OR Nama = 'QRIS BNI LEI'";
        DataTable dt = Rs(query, Db.Cnn_DB, null, CommandType.Text);
        foreach (DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            c.Items.Add(new ListItem(t, v));
        }
    }

    protected void QRISBankList(DropDownList c)
    {
        string query = "SELECT ID, Nama FROM BankIssuer WHERE Nama IN ('BCA','BNI','BRI','MANDIRI','MEGA','CIMB NIAGA','OVO','GOPAY','SAKUKU','SHOPEEPAY','LINKAJA','LAINNYA')";
        DataTable dt = Rs(query, Db.Cnn_DB, null, CommandType.Text);
        foreach (DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            c.Items.Add(new ListItem(t, v));
        }
    }

    protected void QRISBankListLEI(DropDownList c)
    {
        string query = "SELECT ID, Nama FROM BankIssuer WHERE Nama IN ('BCA','BNI','BRI','MANDIRI','MEGA','CIMB NIAGA','OVO','GOPAY','SAKUKU','SHOPEEPAY','LINKAJA','LAINNYA')";
        DataTable dt = Rs(query, Db.Cnn_DB, null, CommandType.Text);
        foreach (DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            c.Items.Add(new ListItem(t, v));
        }
    }

    protected void EDCListCredit(DropDownList c)
    {
        string query = "SELECT ID, Nama FROM EDCList WHERE ID IN (1, 2, 3, 13, 10)";
        DataTable dt = Rs(query, Db.Cnn_DB, null, CommandType.Text);
        foreach (DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            c.Items.Add(new ListItem(t, v));
        }
    }

    protected void BankList(DropDownList c)
    {
        string query = "SELECT ID, Nama FROM BankIssuer WHERE ID NOT IN (31, 41)";
        DataTable dt = Rs(query, Db.Cnn_DB, null, CommandType.Text);
        foreach (DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            c.Items.Add(new ListItem(t, v));
        }
    }

    protected void BankListTransfer(DropDownList c)
    {
        string query = "IF EXISTS(SELECT 1 FROM SalesOrder A " +
            "JOIN (SELECT ID, 1 Tipe, Kode FROM LocOutlet A " +
            "UNION ALL " +
            "SELECT ID, 2 Tipe, Kode FROM LocExhibition) B ON A.TipeLokasi = B.Tipe AND A.IDLokasi = B.ID " +
            "WHERE A.ID = @IDSO AND B.Kode LIKE '%P99%') " +
            "BEGIN " +
            "SELECT ID, Nama FROM BankIssuer WHERE ID NOT IN (31) " +
            "END " +
            "ELSE " +
            "BEGIN " +
            "SELECT ID, Nama FROM BankIssuer WHERE ID NOT IN (31, 41) " +
            "END";
        DataTable dt = Rs(query, Db.Cnn_DB, new List<SqlParameter>() { new SqlParameter("@IDSO", SqlDbType.Int) { Value = myID } }, CommandType.Text);
        foreach (DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            c.Items.Add(new ListItem(t, v));
        }
    }

    protected void EcommercePlatformList(DropDownList c)
    {
        string query = "SELECT ID, Nama FROM EDCList WHERE ID IN (25, 34, 35, 36, 37, 41)";
        DataTable dt = Rs(query, Db.Cnn_DB, null, CommandType.Text);
        foreach (DataRow item in dt.Rows)
        {
            string t = item["Nama"].ToString();
            string v = item["ID"].ToString();
            c.Items.Add(new ListItem(t, v));
        }
    }
    protected string GetLakuEmasPriceAPI()
    {
        //JSON result
        string result = "";
        string output = "";
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        WebRequest req = WebRequest.Create("https://apiext.lakuemas.com/cmk_club/get_price");
        req.Credentials = CredentialCache.DefaultCredentials;

        WebResponse res = req.GetResponse();
        string description = ((HttpWebResponse)res).StatusDescription;

        using (Stream dataStream = res.GetResponseStream())
        {
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            result = reader.ReadToEnd();
            APILakuEmas lm = JsonConvert.DeserializeObject<APILakuEmas>(result);
            output = lm.data.mid_price.ToString();
        }
        return output;
    }

    protected RateLM GetLakuEmasAllPriceAPI()
    {
        //JSON result
        string result = "";
        WebRequest req = WebRequest.Create("https://apiext.lakuemas.com/cmk_club/get_price");
        req.Credentials = CredentialCache.DefaultCredentials;

        WebResponse res = req.GetResponse();
        string description = ((HttpWebResponse)res).StatusDescription;

        using (Stream dataStream = res.GetResponseStream())
        {
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            result = reader.ReadToEnd();
        }
        APILakuEmas lm = JsonConvert.DeserializeObject<APILakuEmas>(result);
        return lm.data;
    }

    public class APILakuEmas
    {
        public int success;
        public RateLM data;
    }

    public class RateLM
    {
        public decimal buy_price;
        public decimal sell_price;
        public decimal mid_price;
    }

    // sp_get_tgp_jual_beli tidak menerima parameter apa pun — hasilnya SAMA setiap kali dipanggil
    // dalam request yang sama, tapi GetTGPPGBeli/GetTGPPGJual masing2 mengeksekusinya ulang PER
    // ITEM PG (bisa 15-20 exec tambahan untuk 1 payment save dengan beberapa item PG — N+1 paling
    // mekanis di seluruh evaluasi). Cache sekali per request di sini.
    private DataTable _tgpJualBeliCache;
    protected DataTable GetTgpJualBeliTable()
    {
        if (_tgpJualBeliCache == null)
        {
            _tgpJualBeliCache = Db.Rs("EXEC sp_get_tgp_jual_beli ", Db.Cnn_DB);
        }
        return _tgpJualBeliCache;
    }

    protected decimal GetTGPPGBeli(StockProductPG r)
    {
        decimal GoldPrice = 0m;
        DataTable dt = GetTgpJualBeliTable();
        if (dt.Rows.Count > 0)
        {
            if (r.CharGoldLevel.ID == 8 && r.CharFrameColor.ID == 2 && r.CharProductItem.ID == 59) // chain white 37.5
            {
                //IDPricing = 36; //PG / CHAIN KADAR 37,5 - Pricing
                GoldPrice = Convert.ToDecimal(dt.Rows[0][2].ToString());
            }
            else if (r.CharGoldLevel.ID == 6 || r.CharGoldLevel.ID == 7) // 6 = zirconia 37,5 ; 7 = zirconia 75 
            {
                if (r.CharGoldLevel.ID == 6)
                {
                    //IDPricing = 34; 
                    GoldPrice = Convert.ToDecimal(dt.Rows[8][2].ToString());
                }
                else if (r.CharGoldLevel.ID == 7)
                {
                    //IDPricing = 35; 
                    GoldPrice = Convert.ToDecimal(dt.Rows[6][2].ToString());
                }
            }
            else if (r.KadarLogam >= 99m)
            {
                if (r.ProductItem == 59) // 59 = Chain
                    GoldPrice = Convert.ToDecimal(dt.Rows[5][2].ToString());//IDPricing = 15;
                else
                    GoldPrice = Convert.ToDecimal(dt.Rows[5][2].ToString());//IDPricing = 16;
            }
            else
            {
                if (r.ProductItem != 59) // 59 = Chain
                {
                    if (r.FrameColor == 1)
                        GoldPrice = Convert.ToDecimal(dt.Rows[3][2].ToString());//IDPricing = 11;
                    else
                        GoldPrice = Convert.ToDecimal(dt.Rows[4][2].ToString());//IDPricing = 12;
                }
                else
                {
                    if (r.FrameColor == 1)
                        GoldPrice = Convert.ToDecimal(dt.Rows[1][2].ToString());//IDPricing = 13;
                    else
                        GoldPrice = Convert.ToDecimal(dt.Rows[2][2].ToString());//IDPricing = 14;
                }
            }
        }

        return GoldPrice;
    }

    protected decimal GetTGPPGJual(StockProductPG r)
    {
        decimal GoldPrice = 0m;
        DataTable dt = GetTgpJualBeliTable();
        if (dt.Rows.Count > 0)
        {
            if (r.CharGoldLevel.ID == 8 && r.CharFrameColor.ID == 2 && r.CharProductItem.ID == 59) // chain white 37.5
            {
                //IDPricing = 36; //PG / CHAIN KADAR 37,5 - Pricing
                GoldPrice = Convert.ToDecimal(dt.Rows[0][1].ToString());
            }
            else if (r.CharGoldLevel.ID == 6 || r.CharGoldLevel.ID == 7) // 6 = zirconia 37,5 ; 7 = zirconia 75 
            {
                if (r.CharGoldLevel.ID == 6)
                {
                    //IDPricing = 34; 
                    GoldPrice = Convert.ToDecimal(dt.Rows[8][1].ToString());
                }
                else if (r.CharGoldLevel.ID == 7)
                {
                    //IDPricing = 35; 
                    GoldPrice = Convert.ToDecimal(dt.Rows[6][1].ToString());
                }
            }
            else if (r.KadarLogam >= 99m)
            {
                if (r.ProductItem == 59) // 59 = Chain
                    GoldPrice = Convert.ToDecimal(dt.Rows[5][1].ToString());//IDPricing = 15;
                else
                    GoldPrice = Convert.ToDecimal(dt.Rows[5][1].ToString());//IDPricing = 16;
            }
            else
            {
                if (r.ProductItem != 59) // 59 = Chain
                {
                    if (r.FrameColor == 1)
                        GoldPrice = Convert.ToDecimal(dt.Rows[3][1].ToString());//IDPricing = 11;
                    else
                        GoldPrice = Convert.ToDecimal(dt.Rows[4][1].ToString());//IDPricing = 12;
                }
                else
                {
                    if (r.FrameColor == 1)
                        GoldPrice = Convert.ToDecimal(dt.Rows[1][1].ToString());//IDPricing = 13;
                    else
                        GoldPrice = Convert.ToDecimal(dt.Rows[2][1].ToString());//IDPricing = 14;
                }
            }
        }

        return GoldPrice;
    }

    protected bool ValidasiBeforeSubmit()
    {
        bool isdifferent = false;
        string query =
            "DECLARE @ISDIFFERENT INT = 0 " +
            "DECLARE @TOTAL_BAYAR MONEY = (SELECT TOP 1 ISNULL(TotalBayar,0) FROM SalesOrder WHERE ID = @IDSO) " +
            "DECLARE @SUM_CARA_BAYAR MONEY = 0 " +
            "IF EXISTS(SELECT 1 FROM SalesReceipt A JOIN SalesReceiptDetail B ON A.ID = B.IDSalesReceipt WHERE A.IDSalesOrder = @IDSO) " +
            "BEGIN " +
            "SET @SUM_CARA_BAYAR = (SELECT SUM(B.Nominal) FROM SalesReceipt A JOIN SalesReceiptDetail B ON A.ID = B.IDSalesReceipt WHERE A.IDSalesOrder = @IDSO) " +
            "END " +
            "IF(@SUM_CARA_BAYAR <> @TOTAL_BAYAR) " +
            "BEGIN " +
            "SET @ISDIFFERENT = 1 " +
            "END " +
            "SELECT @ISDIFFERENT [ISDIFFERENT]";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@IDSO", SqlDbType.Int) { Value = myID }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);

        // Kandidat kuat untuk "There is no row at position 0" pada validasi sebelum submit
        // (dipanggil dari validSalesOrderPaymentBeforeSubmit, bagian dari alur pengecekan
        // pembayaran/voucher sebelum SO di-submit) — query di atas SEHARUSNYA selalu
        // mengembalikan 1 baris (SELECT scalar), tapi kalau eksekusinya gagal di tengah jalan
        // dt bisa kosong. Tanpa data pasti, anggap TIDAK ada perbedaan (aman/tidak memblokir
        // submit) daripada crash seluruh flow submit SO.
        if (dt.Rows.Count == 0)
        {
            return isdifferent;
        }

        int result = Convert.ToInt32(dt.Rows[0][0]);

        if(result == 1)
        {
            isdifferent = true;
        }
        return isdifferent;
    }

    //Data Table Builder
    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
				cmd.CommandTimeout = 240;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    //sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }

    //Data Table Builder
    protected DataSet Ds(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        DataSet ds = new DataSet();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
				cmd.CommandTimeout = 240;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    //sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(ds);
                }
            }
        }
        return ds;
    }

    //Execute Command
    protected void Execute(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
				cmd.CommandTimeout = 240;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                //cmd.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                cmd.ExecuteNonQuery();
            }
        }
    }
}
