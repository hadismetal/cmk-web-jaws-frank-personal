using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Page_Register_PaymentDP_ItemList : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }
    protected string myConfirm { get { return Request.QueryString["confirm"]; } }
    protected string hrefpattern { get { return "?id=" + myID; } }


    string Module = "dp"; // pos, acc

    #region NavTab

    protected string Active_CreditCard { get; set; }
    protected string Active_DebitCard { get; set; }
    protected string Active_Cash { get; set; }
    protected string Active_DownPayment { get; set; }
    protected string Active_Transfer { get; set; }
    protected string Active_Voucher { get; set; }
    protected string Active_GiftCard { get; set; }
    protected string Active_BrideStoryPay { get; set; }
	protected string Active_QRIS { get;set; }

    protected string Tab_CreditCard { get; set; }
    protected string Tab_DebitCard { get; set; }
    protected string Tab_Cash { get; set; }
    protected string Tab_DownPayment { get; set; }
    protected string Tab_Transfer { get; set; }
    protected string Tab_Voucher { get; set; }
    protected string Tab_GiftCard { get; set; }
    protected string Tab_BrideStoryPay { get; set; }
	protected string Tab_QRIS { get;set; }

    protected string Href_CreditCard { get; set; }
    protected string Href_DebitCard { get; set; }
    protected string Href_Cash { get; set; }
    protected string Href_DownPayment { get; set; }
    protected string Href_Transfer { get; set; }
    protected string Href_Voucher { get; set; }
    protected string Href_GiftCard { get; set; }
    protected string Href_BrideStoryPay { get; set; }
	protected string Href_QRIS { get;set; }

    protected void active_CreditCard(bool isActive = true) { Active_CreditCard = isActive ? "active" : ""; }
    protected void active_DebitCard(bool isActive = true) { Active_DebitCard = isActive ? "active" : ""; }
    protected void active_Cash(bool isActive = true) { Active_Cash = isActive ? "active" : ""; }
    protected void active_DownPayment(bool isActive = true) { Active_DownPayment = isActive ? "active" : ""; }
    protected void active_Transfer(bool isActive = true) { Active_Transfer = isActive ? "active" : ""; }
    protected void active_Voucher(bool isActive = true) { Active_Voucher = isActive ? "active" : ""; }
    protected void active_GiftCard(bool isActive = true) { Active_GiftCard = isActive ? "active" : ""; }
    protected void active_BrideStoryPay(bool isActive = true) { Active_BrideStoryPay = isActive ? "active" : ""; }
	protected void active_QRIS(bool isActive = true) { Active_QRIS = isActive ? "active" : ""; }

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
	protected void enable_QRIS(bool isEnable = true)
    {
        Active_QRIS = isEnable ? "" : "disabled";
        Tab_QRIS = isEnable ? "tab" : "";
        Href_QRIS = isEnable ? "href='##qris'" : "";
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        var x = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
        if (x != null)
        {
            bool isNotSameDay = false;

            if (x.StatusDP != 2)
            {

            

                if (!IsPostBack)
                {
                    // Semua harus terbayar di hari yang sama
                    if (x.Tgl < System.DateTime.Now.AddDays(-1)) { isNotSameDay = true; }
					
					ddl_creditcard_edc.Items.Add(new ListItem("","0"));
                    ddl_creditcard_bankissuer.Items.Add(new ListItem("","0"));

                    ddl_debitcard_edc.Items.Add(new ListItem("", "0"));
                    ddl_debitcard_bankissuer.Items.Add(new ListItem("", "0"));

                    DLC.CardTypeList(ddl_creditcard_cardtype);
                    DLC.JenisKartu(ddl_creditcard_cardcategory);
                    EDCList(ddl_creditcard_edc);
                    BankList(ddl_creditcard_bankissuer);
                    DLC.ProgramList(ddl_creditcard_bankprogram);
                    DLC.CardTypeList(ddl_debitcard_cardtype);
                    DLC.JenisKartu(ddl_debitcard_cardcategory);
                    EDCList(ddl_debitcard_edc);
                    BankList(ddl_debitcard_bankissuer);
                    BankListTransfer(ddl_transfer_bankissuer);
                    BankList(ddl_bridestorypay_bankissuer);
					QRISList(ddl_qris_cardtype);
					QRISBankList(ddl_qris_bankissuer);

                    ddl_creditcard_cardtype.SelectedValue = "3";
                    ddl_creditcard_cardcategory.SelectedValue = "3";
                    ddl_creditcard_edc.SelectedValue = "0";
                    ddl_creditcard_bankissuer.SelectedValue = "0";
                    ddl_creditcard_bankprogram.SelectedValue = "1";
                    ddl_debitcard_cardtype.SelectedValue = "2";
                    ddl_debitcard_cardcategory.SelectedValue = "1";
                    ddl_debitcard_edc.SelectedValue = "0";
                    ddl_debitcard_bankissuer.SelectedValue = "0";
                    ddl_transfer_bankissuer.SelectedValue = "1";
                    ddl_bridestorypay_bankissuer.SelectedValue = "1";



                }

                save_payment_creditcard.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(save_payment_creditcard, null) + ";");
                save_payment_debitcard.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(save_payment_debitcard, null) + ";");
                save_payment_cash.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(save_payment_cash, null) + ";");
                save_payment_transfer.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(save_payment_transfer, null) + ";");
                save_payment_giftcard.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(save_payment_giftcard, null) + ";");
                save_payment_bridestorypay.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(save_payment_bridestorypay, null) + ";");


                fillForm(isNotSameDay);
            }
            else
            {
                show_Invalid_Payment_IsVoid(x);
            }
        }
       
        //btn_add_payment_click(sender, e);
    }

    //Exception
    protected void DataError() { Response.Redirect("PaymentDP.aspx"); }

    //Cancel
    protected void cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PaymentDP_ItemList.aspx" + hrefpattern);
    }


    //Form
    protected void fillForm(bool isNotSameDay = false)
    {
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
			enable_QRIS(false);

            rdo_cash_in.Enabled = false;
            rdo_cash_in.Checked = false;

        }
        else
        {
            if (myConfirm == "1")
            {
                div_review.Visible = false;
                div_confirm.Visible = true;
                enable_CreditCard(false);
                enable_DebitCard(false);
                enable_Cash(false);
                enable_DownPayment(false);
                enable_Transfer(false);
                enable_Voucher(false);
                enable_GiftCard(false); // belum jalan per 2018-11-02
                enable_BrideStoryPay(false);
				enable_QRIS(false);

                rdo_cash_in.Enabled = false;
                rdo_cash_in.Checked = false;
            }
            else
            {
                enable_CreditCard(true);
                enable_DebitCard(true);
                enable_Cash(true);
                enable_DownPayment(false);
                enable_Transfer(true);
                enable_Voucher(false);
                enable_GiftCard(false); // belum jalan per 2018-11-02
                enable_BrideStoryPay(true);
				enable_QRIS(true);

                rdo_cash_in.Enabled = true;
                rdo_cash_in.Checked = true;
            }
         
        }

        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            var c = cs.Mscustomer.SingleOrDefault(p => p.M_kode == d.DataCustomer.NoCustomer);
            var m = cs.Msmembership.SingleOrDefault(p => p.M_kode == c.M_typemember);

            //header_nomor.InnerHtml = d.Nomor;

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

            string Membership = "Non-Member";
            if (m != null) { Membership = m.M_nama; }

            brand.InnerHtml = BrandCompany;
            membership.InnerHtml = Membership;
            location.InnerHtml = Lokasi;
            keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
            estimate.InnerHtml = d.EstimasiHarga.ToString("N0");
            jpjr.InnerHtml = d.DataSales.Nama;
            customer.InnerHtml = d.DataCustomer.Nama;
            operator_input.InnerHtml = d.OperatorNama + " (" + Str.TglJam(d.DraftDate) + ")";
            tgl.InnerHtml = Str.Tgl(d.Tgl);
            tipe_dp.InnerHtml = d.IDProduct.HasValue ? "Stock" : "Purchase Order";
			
			int freegwpcount = Db.SingleInteger("SELECT COUNT('') FROM LetterWR WHERE IDSalesReceiptDPPO = " + myID.ToString(), Db.Cnn_DB); 
            if(freegwpcount == 1)
            {
                rowgwp.Visible = true;
                freegwp.InnerHtml = Db.SingleString("SELECT TOP 1 Letter FROM LetterWR WHERE IDSalesReceiptDPPO = " + myID.ToString(), Db.Cnn_DB);
            }
            else
            {
                rowgwp.Visible = false;
                freegwp.InnerHtml = "";
            }

            if(d.TotalBayar == null)
            {
                review.Disabled = true;
            }
            else
            {
                review.Disabled = false;
            }
			
			string kodecust = d.DataCustomer.NoCustomer;
			var cust = cs.Mscustomer.SingleOrDefault(p => p.M_kode == kodecust);
			
			if(cust != null)
			{
				txt_qris_custname.Text = cust.M_nama;
				txt_qris_custphonenumber.Text = cust.M_telepon1;
			}
            

            //spn_payment_totalamountdue.InnerText = d.EstimasiHarga.ToString("N0");
            spn_payment_amountpaid.InnerText = (d.TotalBayar.HasValue ? (d.TotalBayar.Value).ToString("N0") : "0");
            //spn_payment_balancedue.InnerText = (d.TotalBayar.HasValue ? (d.EstimasiHarga - d.TotalBayar.Value).ToString("N0") : d.EstimasiHarga.ToString("N0"));

            fillPembayaran();

       

            //show print
            if (d.Draft == false && d.StatusDP != 2)
            {
              
                div_print.Visible = true;
                save.Disabled = false;
                div_review.Visible = false;

                enable_CreditCard(false);
                enable_DebitCard(false);
                enable_Cash(false);
                enable_DownPayment(false);
                enable_Transfer(false);
                enable_Voucher(false);
                enable_GiftCard(false); // belum jalan per 2018-11-02
                enable_BrideStoryPay(false);

                rdo_cash_in.Enabled = false;
                rdo_cash_in.Checked = false;
            }
            else
            {
               
                div_print.Visible = false;
                save.Disabled = true;
            }

           
            if (isNotSameDay) { save.Disabled = true; show_Invalid_Payment_NotSameDay(d);  }
        }
    }

    protected void fillPembayaran()
    {
        FillPaymentDP(myID, tb3, myConfirm);
    }

    protected void show_Invalid_Payment_NotSameDay(SalesReceiptDPPO x)
    {
        string xText = "<div class='alert alert-danger' role='alert'><div class='row'>"
                    + "<div class='col-xs-2 col-sm-2 col-md-1'><span style='font-size:34px;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>&nbsp;&nbsp;&nbsp;</div>"
                    + "<div class='col-xs-10 col-sm-10 col-md-11'>"
                    + "<div class='col-xs-12 col-sm-12 col-md-12'><b>Warning !!!</b> Anda tidak dapat mengakses halaman ini. Dokumen dengan nomor Down Payment # "
                    + "<b>" + x.NomorDP + "</b> sudah lewat dari tanggal input transaksi. Tgl Input Transaksi : " + Str.FullDate(x.Tgl) + "."
                    + "</div>"
                    + "</div></div></div>"
                    ;

        lit_info_invalid.Text = xText;

        div_finished.Visible = true;
        //redirect_payment_list.Visible = true;
    }

    //Redirect Summary
    protected void redirect_report_Click(object sender, EventArgs e)
    {
        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID); // no-draft !!!
        if (d == null) { DataError(); }
        else
        {
            Response.Redirect("/OutletSales/Page/ReportSales/DPPO_Report.aspx");
        }
    }

    protected void show_Invalid_Payment_IsVoid(SalesReceiptDPPO x)
    {
        string xText = "<div class='alert alert-danger' role='alert'><div class='row'>"
                    + "<div class='col-xs-2 col-sm-2 col-md-1'><span style='font-size:34px;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>&nbsp;&nbsp;&nbsp;</div>"
                    + "<div class='col-xs-10 col-sm-10 col-md-11'>"
                    + "<div class='col-xs-12 col-sm-12 col-md-12'><b>Warning !!!</b> Anda tidak dapat mengakses halaman ini. Dokumen dengan nomor Sales Order # "
                    + "<b>" + x.NomorDP + "</b> dalam status <b>Void</b>.<br /><br />Keterangan Void :<br />" + x.Keterangan
                    + "</div>"
                    + "</div></div></div>"
                    ;

        lit_info_invalid.Text = xText;

        //div_unfinished.Visible = false;
        //div_finished.Visible = true;
        //redirect_summary.Visible = true;
    }


    protected void redirectPrint(SalesReceiptDPPO d)
    {
        //Redirect
        if (d.IDProduct != null)
        {
            string url1 = "/OutletSales/Page/Create/Payment/PaymentDP_Print.aspx?id=" + d.ID;
            Response.Write("<script>");
            Response.Write("window.open('" + url1 + "','_blank')");
            Response.Write("</script>");
        }
        else 
        {
            string url1 = "/OutletSales/Page/Create/Payment/PaymentDP_Print.aspx?id=" + d.ID;
            Response.Write("<script>");
            Response.Write("window.open('" + url1 + "','_blank')");
            Response.Write("</script>");

            string url2 = "/OutletSales/Page/Create/Payment/PaymentDP_POPrint.aspx?id=" + d.ID;
            Response.Write("<script>");
            Response.Write("window.open('" + url2 + "','_blank')");
            Response.Write("</script>");
        }
    }
	
	protected void redirectSPK(SalesReceiptDPPO d)
    {
        if(d.IDProduct != null || d.IDProductSample1 != null)
        {
			string brand = "FC";
            string url1 = "/OutletSales/Page/Report/DPPO_SPK_Export.aspx?id=" + d.ID + "&brand=" + brand;
            Response.Write("<script>");
            Response.Write("window.open('" + url1 + "','_blank')");
            Response.Write("</script>");
        }
    }

    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.Draft.Value == true);
        if (d == null) DataError();
        else
        {
                       
          
            string NoDP = "";
            NoDP = Document.Generate_Nomor_DownPayment_PO(d);         
            d.NomorDP = NoDP;
            d.Draft = false;
            d.DraftDate = null;

            db.SubmitChanges();
			
			Db.Execute("EXEC sp_solve_duplicate_po_number " + myID, Db.Cnn_DB);

            //Redirect print
            redirectPrint(d);
            printCoupon(d);
			
			if (d.IDProduct != null || d.IDProductSample1 != null)
            {
                redirectSPK(d);
            }

            //disable myconfirm


            string pageurl = "/OutletSales/Page/ReportSales/DPPO_Report_Detail.aspx?id=" + d.ID;
            Response.Write("<script>");
            Response.Write("window.location.href='" + pageurl + "'");
            Response.Write("</script>");

            //x += "<a data-toggle='tooltip' title='View' class=\"btn btn-primary\" target=\"_blank\" href=\"DPPO_Report_Detail.aspx?id=" + r.ID + "\">";

        }
    }

    protected void printCoupon(SalesReceiptDPPO d)
    {
        string spname = "[sp_create_coupon_prizedraw]";
        string query = " exec " + spname
                        + " 'DP'"
                        + ", " + d.ID.ToString() + ""
                        ;

    

        DataTable dt = new DataTable();
        dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        if(dt != null)
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
            var d = db.SalesReceiptDPPODetailExtension.Where(p =>
            p.TransactionNumber == x.Text.Trim()
            && (
                (p.SalesReceiptDPPODetail.SalesReceiptDPPO.StatusDP.Value == 0)
                ||
                (
                p.SalesReceiptDPPODetail.SalesReceiptDPPO.StatusDP.Value == 1
                && p.SalesReceiptDPPODetail.SalesReceiptDPPO.IDSalesOrder.Value > 0
                && p.SalesReceiptDPPODetail.SalesReceiptDPPO.SalesOrder.StatusVoid == false
                && p.SalesReceiptDPPODetail.SalesReceiptDPPO.SalesOrder.StatusPembayaran == true
                && p.SalesReceiptDPPODetail.SalesReceiptDPPO.SalesOrder.Unpaid.Value <= 0)
                ||
                (
                    p.SalesReceiptDPPODetail.SalesReceiptDPPO.StatusDP.Value == 1
                && p.SalesReceiptDPPODetail.SalesReceiptDPPO.IDSalesOrder.Value > 0
                && p.SalesReceiptDPPODetail.SalesReceiptDPPO.SalesOrder.Unpaid.Value > 0
                && p.SalesReceiptDPPODetail.SalesReceiptDPPO.SalesOrder.StatusVoid == false)
            ));

            if (d.Count() > 0)
            {
                y = "BridestoryPay Transaction Number <b>" + x.Text.Trim() + "</b> already used.";
            }
        }

        return y;
    }

    protected string valid_Amount(TextBox x)
    {
        string z = x.Text.Trim();
        string y = "";
        decimal parsedValue;

        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
        if (d != null)
        {

            if (y == "" && !decimal.TryParse(x.Text, out parsedValue)) { y = "Amount must be number only. "; }

            if (y == "" && (z == "")) { y = "Please fill the Amount. "; }

            if (y == "" && Convert.ToDecimal(z) < 1) { y = "Amount must greater than zero. "; }

            //if (y == "" && d.Unpaid.Value < Convert.ToDecimal(z) && d.Unpaid.Value > 0) { y = "Overpayment. Please reduce the Amount number. "; }

            //if (y == "" && Math.Abs(d.Unpaid.Value) < Math.Abs(Convert.ToDecimal(z)) && d.Unpaid.Value < 0) { y = "Underpayment. Please reduce the Amount number. "; }

        }

        return y;
    }

    protected string valid_Text(TextBox x, string o)
    {
        string y = "";
        if (x.Text.Trim() == "") { y = o + " must be filled. "; }
        return y;
    }

    protected string msgValidationError(string x)
    {
        string result = "";
        result = "<div class='alert alert-danger text-center' role='alert'>" + x + "</div>";
        return result;
    }

    protected void enableButtonSubmit()
    {
        save_payment_creditcard.Enabled = true;
        save_payment_debitcard.Enabled = true;
        save_payment_cash.Enabled = true;
        save_payment_transfer.Enabled = true;
        save_payment_giftcard.Enabled = true;
        save_payment_bridestorypay.Enabled = true;
    }

    //Review
    protected void review_Click(object sender, EventArgs e)
    {
        //div_review.Visible = false;
        //div_confirm.Visible = true;
        //calc();
        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.Draft == true);
      
        if (d == null) DataError();
        else
        {
            if(!d.TotalBayar.HasValue)
            {
                string msg = string.Empty;
                msg = "Payment : Paid Amount DP cannot be zero, please add more payment.";
                lit_message_general.Text = msgValidationError(msg);
            }
            else
            {
                Response.Redirect("PaymentDP_ItemList.aspx?id=" + myID + "&confirm=1#content_div_confirm");
            }
        }
     

       // Response.Redirect("PaymentDP_ItemList.aspx?id=" + myID + "&confirm=1#content_div_confirm");
    }

    protected void print_Click(object sender, EventArgs e)
    {
        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.Draft == false && p.StatusDP != 2);
        if (d == null) DataError();
        else
        {
            //Redirect print
            redirectPrint(d);
            printCoupon(d);
        }
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
			string Tgl = Convert.ToDateTime(db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID).Tgl).ToString("yyyy-MM-dd");
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
			decimal xMDR = Payment.recalcMDRNew(xIDPaymentType,xIDEDC,xIDBankIssuer,xIDJenisKartuKredit,xIDProgram,xNominal, Tgl);

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
			
			bool statusExtension = false;
            if (payid != 0)
            {
				//redirectGoToPayment();
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
			string Tgl = Convert.ToDateTime(db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID).Tgl).ToString("yyyy-MM-dd");
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
			decimal xMDR = Payment.recalcMDRNew(xIDPaymentType,xIDEDC,xIDBankIssuer,xIDJenisKartuKredit,xIDProgram,xNominal, Tgl);

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
			
			bool statusExtension = false;
            if (payid != 0)
            {
				//redirectGoToPayment();
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
    protected void save_payment_cash_Click(object sender, EventArgs e)
    {
        Literal litMsg = lit_message_general;
        TextBox txtAmount = txt_cash_amount;


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
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = 0;
            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
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
			string Tgl = Convert.ToDateTime(db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID).Tgl).ToString("yyyy-MM-dd");
            int xIDPaymentType = 3; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = 0;
            int xIDProgram = 0;
            int xIDEDC = 0;
            int xIDBankIssuer = Convert.ToInt32(ddlBankIssuer.SelectedValue);
            string xCCNumber = txtAccountNumber.Text.Trim();
            string xCCName = txtAccountName.Text.Trim();
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = 0;
            decimal xMDR = Payment.recalcMDRNew(xIDPaymentType, xIDEDC, xIDBankIssuer, xIDJenisKartuKredit, xIDProgram, xNominal, Tgl);

            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);
            if (payid != 0)
            {
                redirectGoToPayment();
            }
        }


    }
    protected void save_payment_giftcard_Click(object sender, EventArgs e)
    {
        //Literal litMsg = lit_message_general;
        //Literal litMsgTest = lit_message_test;
        //TextBox txtCardNumber = txt_giftcard_cardholdernumber;
        //TextBox txtCardUID = txt_giftcard_cardholderuid;
        //TextBox txtAmount = txt_giftcard_amount;

        //string zAmount = Convert.ToDecimal(txtAmount.Text.Trim()).ToString("N0").Replace(".", "");

        //string msg = "";
        //msg += valid_Amount(txtAmount);
        //msg += valid_Text(txtCardNumber, "Gift Card Number");

        //bool validExecute = false;
        //string[] response1;
        //string[] response2;
        //string[] response3;

        //////////// response 1
        //response1 = (RESTClient.isCardNumberUIDValid(litMsgTest, txtCardUID.Text.Trim(), txtCardNumber.Text.Trim())).Split('-');

        //if (response1[0].Trim() == "2410")
        //{
        //    ////////// response 2
        //    response2 = (RESTClient.isBalanceEnough(litMsg, txtCardNumber.Text.Trim(), zAmount)).Split('-');
        //    if (response2[0].Trim() == "1010")
        //    {
        //        validExecute = true;
        //    }
        //    else
        //    {
        //        //msg += zAmount;
        //        msg += response2[1].Trim() + "<br />(Code : 0x" + response2[0].Trim().PadLeft(6, '0') + ")";
        //    }
        //}
        //else
        //{
        //    msg += response1[1].Trim() + "<br />(Code : 0x" + response1[0].Trim().PadLeft(6, '0') + ")";
        //}

        //txtCardUID.Text = ""; // clear the UID input

        //// validation
        //if (msg != "")
        //{
        //    msg = "Gift Card : " + msg;
        //    litMsg.Text = msgValidationError(msg);
        //    enableButtonSubmit();
        //}
        //else
        //{
        //    if (validExecute)
        //    {

        //        int xIDPaymentType = 9; // 1	giftcard / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit, 9 Gift Card
        //        int xIDCard = 0;
        //        int xIDProgram = 0;
        //        decimal xMDR = 0;
        //        int xIDEDC = 0;
        //        int xIDBankIssuer = 0;
        //        string xCCNumber = txtCardNumber.Text.Trim();
        //        string xCCName = "";
        //        decimal xNominal = Convert.ToDecimal(txtAmount.Text);
        //        int xIDJenisKartuKredit = 0;
        //        int xECRFlag = 0;
        //        string xECRValue = "";

        //        // insert data
        //        int payid = 0;
        //        payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);

        //        // custom business rule
        //        // post to giftcard database using API
        //        if (payid != 0)
        //        {
        //            var w = db.SalesReceiptDetail.Single(p => p.ID == payid);
        //            var s = w.SalesReceipt.SalesOrder;
        //            string loc = "000";
        //            if (s.TipeLokasi == 1) { loc = db.LocOutlet.Single(p => p.ID == s.IDLokasi).Kode; }
        //            if (s.TipeLokasi == 2) { loc = db.LocExhibition.Single(p => p.ID == s.IDLokasi).Kode; }

        //            string xcardnumber = txtCardNumber.Text.Trim();
        //            string xinvoicenumber = s.Nomor;
        //            string xpayment_id = w.ID.ToString();
        //            string xtype = "SALE"; // SALE , VOID
        //            string xamount = xNominal.ToString("N0").Replace(".", "");
        //            string xbranch_id = loc;
        //            string xpayment_method = "Gift Card";
        //            string xuser_cashier = w.SalesReceipt.OperatorNama;

        //            ////////// response 3
        //            response3 = (RESTClient.postTransactionSale(litMsg, xinvoicenumber, xpayment_id, xcardnumber, xtype, xamount, xbranch_id, xpayment_method, xuser_cashier)).Split('-');

        //            if (response3[0].Trim() == "1310")
        //            {
        //                // Rubah Flag Verifikasi Pembayaran
        //                w.GC_Verification = true;
        //                w.GC_VerificationTgl = System.DateTime.Now;
        //                db.SubmitChanges();

        //                redirectGoToPayment();

        //            }
        //            else
        //            {

        //                msg = response3[1].Trim() + "<br />(Code : 0x" + response3[0].Trim().PadLeft(6, '0') + ")";
        //                litMsg.Text = msgValidationError(msg);
        //                enableButtonSubmit();

        //            }
        //        }
        //    }

        //}

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
			string Tgl = Convert.ToDateTime(db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID).Tgl).ToString("yyyy-MM-dd");
            int xIDPaymentType = 10; // 10 : Bridestory Pay
            int xIDCard = 0;
            int xIDProgram = 0;
            int xIDEDC = 0;
            int xIDBankIssuer = Convert.ToInt32(ddlBankIssuer.SelectedValue);
            string xCCNumber = txtAccountNumber.Text.Trim();
            string xCCName = txtAccountName.Text.Trim();
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = 0;
            decimal xMDR = Payment.recalcMDRNew(xIDPaymentType, xIDEDC, xIDBankIssuer, xIDJenisKartuKredit, xIDProgram, xNominal, Tgl);

            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            bool statusBSPAY = false;
            int payid = 0;

            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);

          

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
        //Response.Redirect("Payment_DPAdd.aspx?id=" + myID);
    }
    protected void save_payment_voucher_Click(object sender, EventArgs e)
    {
        //Response.Redirect("Payment_VoucherAdd.aspx?id=" + myID);
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
			string Tgl = Convert.ToDateTime(db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID).Tgl).ToString("yyyy-MM-dd");
            int xIDPaymentType = 13; // 1	Cash / Tunai, 2	Kartu Kredit, 3	Transfer, 4	Debit
            int xIDCard = 0;
            int xIDProgram = 0;
            int xIDEDC = Convert.ToInt32(ddlQRISType.SelectedValue);
            int xIDBankIssuer = Convert.ToInt32(ddlQRISBankIssuer.SelectedValue);//Convert.ToInt32(ddlQRISType.SelectedValue);
            string xCCNumber = txtQRISTransactionNumber.Text.Trim();
            string xCCName = txtQRISCustName.Text.Trim();
            decimal xNominal = Convert.ToDecimal(txtAmount.Text);
            int xIDJenisKartuKredit = 0;
            //decimal xMDR = (0.7m * xNominal) / 100;//Payment.recalcMDR(xNominal, xIDBankIssuer, xIDProgram, xIDJenisKartuKredit, xIDPaymentType);
			decimal xMDR = Payment.recalcMDRNew(xIDPaymentType,xIDEDC,xIDBankIssuer,xIDJenisKartuKredit,xIDProgram,xNominal, Tgl);

            int xECRFlag = 0;
            string xECRValue = "";

            // insert data
            int payid = 0;
            payid = savePayment(xIDPaymentType, xIDCard, xIDProgram, xMDR, xIDEDC, xIDBankIssuer, xCCNumber, xCCName, xNominal, xIDJenisKartuKredit, xECRFlag, xECRValue);

            bool statusExtension = false;
            if (payid != 0)
            {
                //redirectGoToPayment();
                statusExtension = savePaymentExtension(payid, txtQRISTransactionNumber.Text.Trim());

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
    protected bool savePaymentExtension(int IDSalesReceipt, string TransNumber)
    {
        bool status = true;

        var d = db.SalesReceiptDPPODetail.SingleOrDefault(p => p.ID == IDSalesReceipt && p.SalesReceiptDPPO.Draft == true);
         

        if (d == null) {  status = false; }
        else
        {           
            //lit_test_bridestorypay.Text += "1:" + d.ID.ToString();
            //lit_test_bridestorypay.Text += "2:" + d.PaymentType.Nama.ToString();
            //lit_test_bridestorypay.Text += "3:" + TransNumber;

            // add sales receipt header
            SalesReceiptDPPODetailExtension s = new SalesReceiptDPPODetailExtension();

            s.ID = d.ID;
            s.PaymentName = d.PaymentType.Nama;
            s.VerificationDateTime = System.DateTime.Now;
            s.VerificationFlag = 1;
            s.VerificationOperator = App.UserNama;
            s.TransactionNumber = TransNumber;

            db.SalesReceiptDPPODetailExtension.InsertOnSubmit(s);
            db.SubmitChanges();

        }

        return status;
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

        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.Draft == true);
        if (d == null) DataError();
        else
        {

            //// jika pengembalian cash dan tidak terdeteksi ada pembayaran lebih DP maka invalid Payment
            //if (msg == "" && xIDPaymentType == 1 && d.Unpaid < 0)
            //{
            //    var paymentdpexists = from p in db.SalesReceiptDetail
            //                          where p.SalesReceipt.IDSalesOrder == d.ID && p.SalesReceipt.NoSalesOrder == d.Nomor
            //                             && (p.IDPaymentType == 8 && p.Nominal > 0)
            //                          select p
            //                        ;
            //    if (paymentdpexists.Count() == 0)
            //    {
            //        msg = "Pengembalian Cash hanya untuk pembayaran <b>Down Payment lebih bayar</b>.";
            //    }
            //}

            //// validasi jika sudah lunas dengan inputan pembayaran selain cash out
            //if (msg == "" & (d.Unpaid.Value < 0 && xNominal > 0 && xIDPaymentType != 1))
            //{
            //    msg = "Invoice sudah lunas / Nominal pembayaran melebihi Total Kurang Bayar";
            //}

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


             
                IDSalesReceipt = d.ID;

                // add sales receipt detail payment
                SalesReceiptDPPODetail sd = new SalesReceiptDPPODetail();

                sd.IDSalesReceipt = IDSalesReceipt; //IDSalesReceipt
                sd.IDPaymentType = xIDPaymentType; //IDPaymentType
                if (xIDEDC != 0) { sd.IDEDC = xIDEDC; } //IDEDC
                if (xIDBankIssuer != 0) { sd.IDBankIssuer = xIDBankIssuer; } //IDBankIssuer
                sd.CCNumber = xCCNumber.ToUpper(); //CCNumber
                sd.CCName = xCCName.ToUpper(); //CCName
                if (xIDCard != 0) { sd.IDCard = xIDCard; } //IDCard
                if (xIDProgram != 0) { sd.IDProgram = xIDProgram; } //IDProgram
                sd.MDR = xMDR; //MDR
                sd.Nominal = xNominal; //Nominal
                sd.OperatorTgl = DateTime.Now; //OperatorTgl
                sd.OperatorNama = App.UserNama.ToUpper(); //OperatorNAma    
                if (xIDJenisKartuKredit != 0) { sd.IDJenisKartuKredit = xIDJenisKartuKredit; } //IDJenisKartuKredit
                

                db.SalesReceiptDPPODetail.InsertOnSubmit(sd);
                db.SubmitChanges();

                result = sd.ID; // return detail id of payment

                //Update Total Bayar DP
                var x = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == IDSalesReceipt);
                var y = db.SalesReceiptDPPODetail.Where(p => p.IDSalesReceipt == IDSalesReceipt).Sum(p => p.Nominal);

                x.TotalBayar = y;
                db.SubmitChanges();

            }

        }

        return result;

    }

    protected void redirectGoToPayment()
    {
        // Go to Payment
        Response.Redirect("/OutletSales/Page/Create/Payment/PaymentDP_ItemList.aspx?id=" + myID
            + "&rnd=" + System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(System.DateTime.Now.ToString())) + "#divrowpayment", false)
            ;
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
        confirm_readonly(true, "giftcard");
    }
    protected void btn_giftcard_cancel_Click(object sender, EventArgs e)
    {
        div_giftcard_confirm.Visible = true;
        div_giftcard_submit.Visible = false;
        btn_giftcard_cancel.Visible = false;
        confirm_readonly(false, "giftcard");
    }
	protected void EDCList(DropDownList c)
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
        string query = "IF EXISTS(SELECT 1 FROM SalesReceiptDPPO A " +
            "JOIN (SELECT ID, 1 Tipe, Kode FROM LocOutlet A " +
            "UNION ALL " +
            "SELECT ID, 2 Tipe, Kode FROM LocExhibition) B ON A.TipeLokasi = B.Tipe AND A.IDLokasi = B.ID " +
            "WHERE A.ID = @IDPO AND B.Kode LIKE '%P99%') " +
            "BEGIN " +
            "SELECT ID, Nama FROM BankIssuer WHERE ID NOT IN (31) " +
            "END " +
            "ELSE " +
            "BEGIN " +
            "SELECT ID, Nama FROM BankIssuer WHERE ID NOT IN (31, 41) " +
            "END";
        DataTable dt = Rs(query, Db.Cnn_DB, new List<SqlParameter>() { new SqlParameter("@IDPO", SqlDbType.Int) { Value = myID } }, CommandType.Text);
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
	protected void FillPaymentDP(int myID, Table tbl, string myConfirm)
    {

        decimal totalPaid = 0m;

        var rs = from p in db.SalesReceiptDPPODetail
                 where p.IDSalesReceipt == myID
                 select p;

        if (rs.Count() == 0) App.TableNoResult(tbl);
        else
        {
            int index = 1;
            foreach (var r in rs)
            {
                TableRow tr = new TableRow();
                TableCell td;
                

                if ((myConfirm != "1" &&   r.SalesReceiptDPPO.Draft == true && r.SalesReceiptDPPO.StatusDP == 0))
                {
                        
                    td = new TableCell();
                    td.Text += "<p>";
                    td.Text += "<a class=\"btn btn-danger btn-xs\" onClick=\"return confirm('Delete Payment, Are you Sure?');\" href=\"PaymentDP_ItemList_Delete.aspx?id=" + r.ID + "&id2=" + r.IDSalesReceipt + "\">";
                    td.Text += "  <i class=\"icon-trash\"></i> ";
                    td.Text += "  ";
                    td.Text += "</a>";
                    td.Text += "</p>";
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);
                }
                else
                {
                    td = new TableCell();
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);
                }


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

                }
                else if (r.IDPaymentType == 9)
                {
                    //gift card
                    xicon = "<span class='glyphicon glyphicon-credit-card' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "Card Number", (r.CCNumber));

                }
                else if (r.IDPaymentType == 1)
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
                    var z = Common.SplitInParts(r.SalesReceiptDPPODetailExtension.TransactionNumber.Trim(), 4);

                    //bridestorypay
                    xicon = "<span class='glyphicon glyphicon-phone' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "Account Name", (r.CCName));
                    sContent += popoverRowContent(wlabel, "Account Number", (r.CCNumber));
                    sContent += popoverRowContent(wlabel, "Bank Issuer", (r.IDBankIssuer.HasValue ? r.BankIssuer.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "BSPAY #", ((String.Join("-", z))));


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

                //if (r.VoidReference.HasValue)
                //{
                //    xheader += "<span class='pull-right' style='text-align:right; color:red;'>VOID # " + (r.VoidReference.Value).ToString() + "&nbsp;</span>";

                //}

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

                tbl.Rows.Add(tr);

                index++;
            }

            Sub_Total_PaidDP(totalPaid, tbl, myConfirm);
        }
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

    protected void Sub_Total_PaidDP(decimal TotalPaid, Table tbl, string myConfirm)
    {
        TableRow tr = new TableRow();
        TableCell td;

        td = new TableCell();
        td.ColumnSpan = 3;
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
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }
}