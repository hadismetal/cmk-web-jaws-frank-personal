using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Data;
using System.Web.UI;
using System.Collections.Generic;

public partial class Page_Create_ReportSales_DPPO_Report_Detail : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        var x = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
        if (x != null)
        {
            bool isNotSameDay = false;

            //if (x.StatusDP != 2)
            //{

                if (!IsPostBack)
                {
                    // Semua harus terbayar di hari yang sama
                    if (x.Tgl < System.DateTime.Now.AddDays(-1)) { isNotSameDay = true; }

                }

                fillForm(isNotSameDay);
            //}
            //else
            //{
            //    show_Invalid_Payment_IsVoid(x);
            //}
        }
       
        //btn_add_payment_click(sender, e);
    }

    //Exception
    protected void DataError(string err) { Response.Redirect("DPPO_Report.aspx?err=" + err); }

    //Cancel
    protected void cancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("DPPO_Report.aspx" + hrefpattern);
    }


    //Form
    protected void fillForm(bool isNotSameDay = false)
    {
       

        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError("1");
        else
        {
            string KodeCustomer = d.DataCustomer.NoCustomer;

            var c = cs.Mscustomer.SingleOrDefault(p => p.M_kode == KodeCustomer);

            if (c == null) DataError("2");
            else
            {
                var m = cs.Msmembership.SingleOrDefault(p => p.M_kode == c.M_typemember);

                string Membership = "";

                if (m != null)
                {

                    Membership = "Non-Member";
                    if (m != null) { Membership = m.M_nama; }

                }

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

                brand.InnerHtml = BrandCompany;
                membership.InnerHtml = Membership;
                location.InnerHtml = Lokasi;
                keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
                estimate.InnerHtml = d.EstimasiHarga.ToString("N0");
                jpjr.InnerHtml = d.DataSales.Nama;
                customer.InnerHtml = d.DataCustomer.Nama;
                operator_input.InnerHtml = d.OperatorNama + " (" + Str.TglJam(d.OperatorTgl) + ")";
                tgl.InnerHtml = Str.Tgl(d.Tgl);
                tipe_dp.InnerHtml = d.IDProduct.HasValue ? "Stock" : "Purchase Order";
				
				int freegwpcount = Db.SingleInteger("SELECT COUNT('') FROM LetterWR WHERE IDSalesReceiptDPPO = " + myID.ToString(), Db.Cnn_DB);
                if (freegwpcount == 1)
                {
                    rowgwp.Visible = true;
                    freegwp.InnerHtml = Db.SingleString("SELECT TOP 1 Letter FROM LetterWR WHERE IDSalesReceiptDPPO = " + myID.ToString(), Db.Cnn_DB);
                }
                else
                {
                    rowgwp.Visible = false;
                    freegwp.InnerHtml = "";
                }

                //spn_payment_totalamountdue.InnerText = d.EstimasiHarga.ToString("N0");
                spn_payment_amountpaid.InnerText = (d.TotalBayar.HasValue ? (d.TotalBayar.Value).ToString("N0") : "0");
                //spn_payment_balancedue.InnerText = (d.TotalBayar.HasValue ? (d.EstimasiHarga - d.TotalBayar.Value).ToString("N0") : d.EstimasiHarga.ToString("N0"));

                fillPembayaran();


                //show print
                if (d.Draft == false && d.StatusDP != 2 && d.IDSalesOrder == null)
                {

                    div_print.Visible = true;
                    print.Disabled = false;
					if (d.IDProduct != null || d.IDProductSample1 != null)
                    {
                        exportSPK.Disabled = false;
						exportSPK.Visible = false;
                    }
                    else
                    {
                        exportSPK.Disabled = true;
						exportSPK.Visible = true;
                    }
                }
                else
                {

                    div_print.Visible = false;
                    print.Disabled = true;
					exportSPK.Disabled = true;
                }




                if (d.Draft == false && d.StatusDP == 2)
                {

                    show_Invalid_Payment_IsVoid(d);
                }
                else
                {
                    if (isNotSameDay) { show_Invalid_Payment_NotSameDay(d); }
                }
            }

            

            
        }
    }

    protected void fillPembayaran()
    {
        FillPaymentDP(myID, tb3, "1");
    }

    protected void show_Invalid_Payment_NotSameDay(SalesReceiptDPPO x)
    {
        string xText = "<div class='alert alert-danger' role='alert'><div class='row'>"
                    + "<div class='col-xs-2 col-sm-2 col-md-1'><span style='font-size:34px;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>&nbsp;&nbsp;&nbsp;</div>"
                    + "<div class='col-xs-10 col-sm-10 col-md-11'>"
                    + "<div class='col-xs-12 col-sm-12 col-md-12'><b>Warning !!!</b> Dokumen dengan nomor Down Payment # "
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
        if (d == null) { DataError("4"); }
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
                    + "<div class='col-xs-12 col-sm-12 col-md-12'><b>Warning !!!</b> Anda tidak dapat mengakses halaman ini. Dokumen dengan nomor # "
                    + "<b>" + x.NomorDP + "</b> dalam status <b>Void</b>.<br /><br />Keterangan Void :<br />" + x.Keterangan
                    + "</div>"
                    + "</div></div></div>"
                    ;

        lit_info_invalid.Text = xText;

        div_finished.Visible = true;

        //div_unfinished.Visible = false;
        //div_finished.Visible = true;
        //redirect_summary.Visible = true;
    }

    protected void print_Click(object sender, EventArgs e)
    {
        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.Draft == false && p.StatusDP != 2);
        if (d == null) DataError("5");
        else
        {
            //Redirect print
            redirectPrint(d);
            printCoupon(d);
        }
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

    protected void printCoupon(SalesReceiptDPPO d)
    {
        string spname = "[sp_create_coupon_prizedraw]";
        string query = " exec " + spname
                        + " 'DP'"
                        + ", " + d.ID.ToString() + ""
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
	
	protected void exportSPK_ServerClick(object sender, EventArgs e)
    {
        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.Draft == false && p.StatusDP != 2);
        if(d != null && (d.IDProduct != null || d.IDProductSample1 != null))
            redirectSPK(d);
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
                    var omega = db.SalesReceiptDPPODetailExtension.SingleOrDefault(p => p.ID == r.ID);
                    xicon = "<span class='glyphicon glyphicon-credit-card' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "Card Name", (r.CCName));
                    sContent += popoverRowContent(wlabel, "Card Number", (r.CCNumber));
                    sContent += popoverRowContent(wlabel, "Card Type", (r.IDCard.HasValue ? r.CardType.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "Card Category", (r.IDJenisKartuKredit.HasValue ? r.JenisKartuKredit.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "EDC", (r.IDEDC.HasValue ? r.EDCList.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "Bank Issuer", (r.IDBankIssuer.HasValue ? r.BankIssuer.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "Program", (r.IDProgram.HasValue ? r.ProgramCicilan.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "MDR", Str.Number0(r.MDR));
                    sContent += popoverRowContent(wlabel, "Approval Code", omega.TransactionNumber);
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
					var omega = db.SalesReceiptDPPODetailExtension.SingleOrDefault(p => p.ID == r.ID);
                    xicon = "<span class='glyphicon glyphicon-credit-card' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "Card Name", (r.CCName));
                    sContent += popoverRowContent(wlabel, "Card Number", (r.CCNumber));
                    sContent += popoverRowContent(wlabel, "Card Type", (r.IDCard.HasValue ? r.CardType.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "Card Category", (r.IDJenisKartuKredit.HasValue ? r.JenisKartuKredit.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "EDC", (r.IDEDC.HasValue ? r.EDCList.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "Bank Issuer", (r.IDBankIssuer.HasValue ? r.BankIssuer.Nama : " - "));
                    sContent += popoverRowContent(wlabel, "MDR", Str.Number0(r.MDR));
					sContent += popoverRowContent(wlabel, "Approval Code", omega.TransactionNumber);

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
}