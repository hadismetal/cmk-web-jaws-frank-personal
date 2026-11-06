using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Customer System
/// </summary>
public partial class Payment
{

    public static string validSalesOrderPayment(SalesOrder r)
    {
        using (Map_DB db = new Map_DB())
        {

            string result = "";

            var srd = db.SalesReceiptDetail.Where(p => p.SalesReceipt.SalesOrder == r && !p.VoidReference.HasValue);

            // check if sum is more than 50 percent (50%)
            var srd_50 = srd.Where(p => p.Nominal > 0);
    		var srd_50_minus = srd.Where(p => p.Nominal <= 0);

            if (srd_50.Count() > 0)
            {
                var srd_total = srd_50.Sum(p => p.Nominal);
    			var srd_total_minus = srd_50_minus.Sum(p => p.Nominal);

                if (srd_total + srd_total_minus > (r.TotalBayar * 150 / 100)) { result += "Invalid Over Payment."; }
            }

            // check if not duplicated (nd)
            var srd_nd = srd.Where(p => p.Nominal > 0).OrderByDescending(p => p.ID).Take(2);

            if (srd_nd.Count() == 2)
            {
                var srd1 = srd.OrderBy(p => p.OperatorTgl).First();
                var srd2 = srd.OrderByDescending(p => p.OperatorTgl).First();

                if (srd1.IDPaymentType == 5 && srd2.IDPaymentType == 5) { result += ""; }
                else
                {
                    if (srd1.Nominal == srd2.Nominal) { result += ""; }//{ result += "Duplikasi pembayaran. "; }
                }
            }

            // check if not duplicated voucher payment
            var srd_dv = srd.Where(p => p.IDPaymentType == 5 && p.Nominal > 0);

            if (srd_dv.Count() > 0)
            {
                //var result = srd_dv.GroupBy(x => (x.Input, x.CreatedBy))
                //      .Where(x => x.Count() > 1)
                //      .SelectMany(x => x);

                var query = srd_dv.GroupBy(x => x.CCNumber)
                          .Where(g => g.Count() > 1)
                          .Select(y => y.Key)
                          //.SelectMany(x => x)
                          //.ToList()
                          ;

                if (query.Count() > 0)
                {
                    result += "Duplikasi Voucher Payment. ";
                }
            }

            return result;
            }
    }
    
    public static void FillPayment(int myID, Table tbl)
    {
        using (Map_DB db = new Map_DB())
        {

    
            decimal totalPaid = 0m;
		
            var rs = from p in db.SalesReceiptDetail
                     where p.SalesReceipt.IDSalesOrder.Value == myID
    				 orderby p.SalesReceipt.ID ascending
                     orderby p.VoidReference descending
                     select p;
		
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
                        var z = Common.SplitInParts(r.SalesReceiptDetailExtension.TransactionNumber.Trim(),4);

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

                    tbl.Rows.Add(tr);

                    index++;
                }

                Sub_Total_Paid(totalPaid, tbl);
            }
            }
    }

    public static void FillPaymentDP(int myID, Table tbl, string myConfirm)
    {
        using (Map_DB db = new Map_DB())
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
    }

    protected static string popoverRowContent(string w, string x, string y)
    {
        string z = "";

        z += "<div style='float:left; width:" + w + "px;'>" + x + "</div>"
            + "<div style='float:left;'>&nbsp;=&nbsp;</div>"
            + "<div style='float:left;'> " + y + "</div>"
            ;

        z = "<div>" + z + "<div style='clear:both;'></div></div>";

        return z;
    }

    protected static void Sub_Total_PaidDP(decimal TotalPaid, Table tbl, string myConfirm)
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

    protected static void Sub_Total_Paid(decimal TotalPaid, Table tbl)
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

    protected void recalcSalesOrder(SalesOrder d)
    {
        using (Map_DB db = new Map_DB())
        {
            var x = db.SalesOrder.SingleOrDefault(p => p.ID == d.ID);

            if (x != null)
            {
                var y = db.SalesReceiptDetail.Where(p => p.SalesReceipt.SalesOrder.ID == d.ID).Sum(p => p.Nominal);

                x.Paid = y;
                x.Unpaid = x.TotalBayar - y;

            }
        }
    }

    public static decimal recalcMDR(decimal Amount, int ID_Bank, int ID_Program, int ID_CardType, int ID_PaymentType)
    {
        using (Map_DB db = new Map_DB())
        {

            /// ID_PaymentType belum terpakai, table MDR sementara berlaku hanya untuk pembayaran Credit Card
            /// MDR untuk Debit Card dan Credit Card berbeda !!!
            /// ID_PaymentType : 2-> CreditCard , 4-> DebitCard

            decimal MDR_Result = 0m;
            if(ID_PaymentType == 2)
            {
                /// sementara kunci hanya untuk Credit Card dahulu (2018-10-31)
                var d = db.MDRRate.SingleOrDefault(p => p.IDBank == ID_Bank
                            && p.IDProgram == ID_Program
                            && p.IDJenisKartuKredit == ID_CardType
                        );
                if (d != null) { MDR_Result = (Amount * d.MDRRate1) / 100; }
            }
            return MDR_Result;
            }
    }
	
	public static decimal recalcMDRNew(int ID_Payment, int ID_Edc, int ID_Bank, int ID_Jenis_Kartu, int ID_Program, decimal Nominal, string Tgl)
    {
        string query = "sp_get_nominal_mdr_new " + ID_Payment + "," + ID_Edc + "," + ID_Bank + "," + ID_Jenis_Kartu + "," + ID_Program + "," + Nominal + ",'" + Tgl + "'";
        decimal MDR_Result = Db.SingleDecimal(query, Db.Cnn_DB);
        return MDR_Result;
    }

    #region Void Sales Order Payment Satuan (Khusus untuk CC, DB, Cash, Xfer)



    #endregion

    #region Void Sales Order Payment

    public static string voidSalesOrderPaymentAll(SalesOrder d)
    {
        using (Map_DB db = new Map_DB())
        {

            string result = "";

            foreach (var r in d.SalesReceipt)
            {
                foreach (var rslt in r.SalesReceiptDetail.Where(p => p.VoidReference == null))
                {
                    result += voidSalesOrderPayment(d, rslt);
                }
            }

            /*var srd = db.SalesReceiptDetail.Where(p => p.VoidReference == null && p.SalesReceipt.IDSalesOrder == d.ID);

            foreach (var rslt in srd)
            {
                //if (srd.Sum(p => p.Nominal) > 0)
                //{
                //    result += voidSalesOrderPayment(d, rslt);
                //}

                result += voidSalesOrderPayment(d, rslt);

            }*/
        
            postSalesOrderPayment(d);

            return result;
            }
    }

    protected static string voidSalesOrderPayment(SalesOrder d, SalesReceiptDetail rslt, bool isSingleData = false)
    {
        using (Map_DB db = new Map_DB())
        {

            string result = "";

            decimal Pembayaran = 0m;
            Pembayaran = rslt.Nominal.Value * -1;

            SalesReceiptDetail n = new SalesReceiptDetail();

            n.IDBankIssuer = rslt.IDBankIssuer;
            n.IDCard = rslt.IDCard;
            n.CCName = rslt.CCName;
            n.CCNumber = rslt.CCNumber;
            n.IDEDC = rslt.IDEDC;
            n.IDPaymentType = rslt.IDPaymentType;
            n.IDProgram = rslt.IDProgram;
            n.IDSalesReceipt = rslt.IDSalesReceipt;
            n.MDR = rslt.MDR * -1;
            n.Nominal = Pembayaran;
            n.OperatorNama = App.UserNama;
            n.OperatorTgl = System.DateTime.Now;
            n.VoidReference = rslt.ID; // Add Void Reference ID dari pembayaran yang mau di void

            db.SalesReceiptDetail.InsertOnSubmit(n);
            db.SubmitChanges();

            int IDvoid = n.ID;
            Db.Execute("UPDATE SalesReceiptDetail SET VoidReference = " + IDvoid + " WHERE ID = " + rslt.ID, Db.Cnn_DB);

            if (rslt.IDPaymentType == 5) { returnVoucher(d, rslt); }
            if (rslt.IDPaymentType == 8) { returnDPPO(d, rslt); }
            if (rslt.IDPaymentType == 9) { result += returnGiftCard(d, rslt); }
    		if (rslt.IDPaymentType == 11) { result += returnLakuEmasNumber(d,rslt); }

            if (isSingleData) { postSalesOrderPayment(d); }

            return result;
            }
    }

    public static void postSalesOrderPayment(SalesOrder d)
    {
        using (Map_DB db = new Map_DB())
        {

            try
            {
                var rsCB = (from p in db.CompanyBrand
                            where p.Draft == false && p.Disable == false
                            select p.Nama).First();

                string CB = ""; /// mondial, franknco, missmondial, thepalace
                if (rsCB.Count() > 0) { CB = rsCB.ToLower(); }

                string SPname = "";
                if (CB == "mondial") { SPname = "sp_getposmd"; }
                else if (CB == "miss mondial") { SPname = "sp_getposmd"; }
                else if (CB == "the palace") { SPname = "sp_getpostp"; }
                else if (CB == "frank & co") { SPname = "sp_getpos"; }

                Db.Execute("EXEC " + SPname + " " + "@p_nomor = N'" + d.Nomor + "'," + "@id_pos = " + d.ID.ToString() + "", Db.Cnn_CS);

                if (CB == "mondial") { SPname = "sp_getpaymentmd"; }
                else if (CB == "miss mondial") { SPname = "sp_getpaymentmd"; }
                else if (CB == "the palace") { SPname = "sp_getpaymenttp"; }
                else if (CB == "frank & co") { SPname = "sp_getpayment"; }

                Db.Execute("EXEC " + SPname + " " + "@p_nomor = N'" + d.Nomor + "'," +  "@id_so = " + d.ID.ToString() + "", Db.Cnn_CS);
            }
            catch { }

            //Db.Execute("EXEC sp_getpayment '" + d.Nomor + "', " + d.ID.ToString(), Db.Cnn_CS);
            //Db.Execute("EXEC sp_getpos '" + d.Nomor + "', " + d.ID.ToString(), Db.Cnn_CS);
            }
    }

    public static void returnVoucher(SalesOrder d, SalesReceiptDetail rslt)
    {
        using (Map_CS cs = new Map_CS())
        {

            if (rslt.IDPaymentType == 5 && rslt.SalesReceipt.SalesOrder.ID == d.ID)
            {
                string voucherNumber = rslt.CCNumber;
                var x = cs.T_voucher.SingleOrDefault(p => p.M_nomor == voucherNumber);

                if (x != null)
                {
                    x.M_klaimdoc = "";
                    x.M_tanggal2 = null;
                    x.M_store2 = "";

                    cs.SubmitChanges();
                }
            }
            }
    }

    public static void returnDPPO(SalesOrder d, SalesReceiptDetail rslt)
    {
        using (Map_DB db = new Map_DB())
        {

            if (rslt.IDPaymentType == 8 && rslt.SalesReceipt.SalesOrder.ID == d.ID)
            {
                var x = db.SalesReceiptDPPO.SingleOrDefault(p => p.NomorDP == rslt.CCNumber);

                if (x != null)
                {
                    x.StatusDP = 0;
                    x.IDSalesOrder = null;
                    db.SubmitChanges();
                }

            }
            }
    }
	
	public static void returnDPPOPerPaymentItem(SalesOrder d, SalesReceiptDetail rslt)
    {
        using (Map_DB db = new Map_DB())
        {

            if (rslt.IDPaymentType == 8 && rslt.SalesReceipt.SalesOrder.ID == d.ID)
            {
                var x = db.SalesReceiptDPPO.SingleOrDefault(p => p.NomorDP == rslt.CCNumber);
                if (x != null)
                {
                    x.StatusDP = 0;
                    x.IDSalesOrder = null;
                    db.SubmitChanges();
                }
                checkDPPOPerPaymentItem(rslt.CCNumber,x,d);
            }
            }
    }

    public static void checkDPPOPerPaymentItem(string NomorDP, SalesReceiptDPPO y, SalesOrder d)
    {
        using (Map_DB db = new Map_DB())
        {

            var x = db.SalesReceiptDetail.Where(p => p.CCNumber == NomorDP);
            foreach(var item in x)
            {
                if(!item.VoidReference.HasValue)
                {
                    y.StatusDP = 1;
                    y.IDSalesOrder = d.ID;
                    db.SubmitChanges();
                    break;
                }
            }
            }
    }

    public static string returnGiftCard(SalesOrder d, SalesReceiptDetail rslt)
    {
        using (Map_DB db = new Map_DB())
        {

            string result = "";

            /*
            postTransactionVoid(
            Literal z
            , string InvoiceNumber
            , string PaymentId
            , string CardNumber
            , string Type
            , string AmountDeduction
            , string BranchId
            , string PaymentMethod
            , string UserCashier
            )
        
            */
            if (rslt.IDPaymentType == 9 && rslt.SalesReceipt.SalesOrder.ID == d.ID)
            {
                Literal z = new Literal();
                string InvoiceNumber = "";
                string PaymentId = "";
                string CardNumber = "";
                string Type = "";
                string AmountDeduction = "";
                string BranchId = "";
                string PaymentMethod = "";
                string UserCashier = "";

                string Lokasi = "";
                if (d.TipeLokasi == 1)
                {
                    Lokasi = db.LocOutlet.Single(p => p.ID == d.IDLokasi).Kode;
                }
                else if (d.TipeLokasi == 2)
                {
                    Lokasi = db.LocExhibition.Single(p => p.ID == d.IDLokasi).Kode;
                }

                InvoiceNumber = d.Nomor.ToString();
                PaymentId = rslt.ID.ToString();
                CardNumber = rslt.CCNumber.ToString();
                Type = "VOID";
                AmountDeduction = Convert.ToDecimal(rslt.Nominal).ToString("N0").Replace(".","");
                BranchId = Lokasi;
                PaymentMethod = "Gift Card";
                UserCashier = d.Operator;

                string[] response1;

                try
                {

                    response1 = RESTClient.postTransactionVoid(z
                        , InvoiceNumber
                        , PaymentId
                        , CardNumber
                        , Type
                        , AmountDeduction
                        , BranchId
                        , PaymentMethod
                        , UserCashier).Split('-');

                    result += response1[0] + " - " + response1[1];

                    if (response1[0].Trim() == "1410" || response1[0].Trim() == "1430")
                    {
                        int xid = rslt.ID;
                        var r = db.SalesReceiptDetail.Single(p => p.ID == xid);

                        // jika sukses maka ganti flag Verifikasi
                        r.GC_VoidVerification = true;
                        r.GC_VoidVerificationTgl = System.DateTime.Now;
                        db.SubmitChanges();
                    }

                }
                catch (Exception) { }

            }

            return result;
            }
    }
	
	public static string returnLakuEmasNumber(SalesOrder d, SalesReceiptDetail rslt)
    {
        string result = "";
        try
        {
            Db.Execute("UPDATE SalesReceiptDetailLakuEmas SET NoLakuEmas = '"+rslt.CCNumber.Trim()+ "(VOID #" + d.ID + ")' WHERE NoLakuEmas = '"+rslt.CCNumber.Trim()+"' ", Db.Cnn_DB);
        }
        catch { }
        return result;
    }

    public static bool showVoidPaymentStatusGiftCard(SalesOrder d)
    {
        using (Map_DB db = new Map_DB())
        {

            bool status = false;

            var x = db.SalesReceiptDetail.Where(p => p.SalesReceipt.SalesOrder.ID == d.ID
                && p.IDPaymentType == 9
                && p.SalesReceipt.SalesOrder.StatusVoid == true
                && p.GC_VoidVerification == null
                && p.GC_VoidVerificationTgl == null
                && p.Nominal > 0
                );

            if (x.Count() > 0)
            {
                status = true;
            }

            return status;
            }
    }
	
	public static bool showVoidPaymentStatusGiftCard(int id)
	{
		using (Map_DB db = new Map_DB())
		{

			SalesOrder d = db.SalesOrder.SingleOrDefault(p => p.ID == id);
			bool status = false;

			var x = db.SalesReceiptDetail.Where(p => p.SalesReceipt.SalesOrder.ID == d.ID
				&& p.IDPaymentType == 9
				&& p.SalesReceipt.SalesOrder.StatusVoid == true
				&& p.GC_VoidVerification == null
				&& p.GC_VoidVerificationTgl == null
				&& p.Nominal > 0
				);

			if (x.Count() > 0)
			{
				status = true;
			}

			return status;
			}
	}

    public static bool showPaymentStatusGiftCard(SalesOrder d)
    {
        using (Map_DB db = new Map_DB())
        {

            bool status = true;

            var x = db.SalesReceiptDetail.Where(p => p.SalesReceipt.SalesOrder.ID == d.ID
                && p.IDPaymentType == 9
                && p.SalesReceipt.SalesOrder.StatusVoid == false
                && p.GC_Verification == null
                && p.GC_VerificationTgl == null
                && p.Nominal > 0
                );

            if (x.Count() > 0)
            {
                status = false;
            }

            return status;
            }
    }


    #endregion
    #region Void Down Payment

    protected static void voidDownPaymentAll(int IDDP)
    {

    }

    protected static void voidDownPayment(int IDDP, int IDPAYMENT)
    {

    }

    protected static void postDownPayment(int IDSO)
    {

    }

    #endregion
	
	#region Save Payment
	public static void savetposasynchronous_so(int ID, string Nomor, string Brand)
	{
        string query_so = "";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@p_nomor", SqlDbType.VarChar){ Value = Nomor },
            new SqlParameter("@id_pos", SqlDbType.Int){ Value = ID }
        };
        if (Brand == "MD")
            query_so = "sp_getposmd";
        else if (Brand == "TP")
            query_so = "sp_getpostp";
        else if (Brand == "FC")
            query_so = "sp_getpos";
        else if (Brand == "PL")
            query_so = "sp_getposlei";

        Task.Run(async () => { await ExecuteAsync(query_so, Db.Cnn_CS, parameters, CommandType.StoredProcedure); });
    }

	public static void savetposasynchronous_ti(int ID, string Nomor, string Brand)
	{
        string query_ti = "";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@p_nomor", SqlDbType.VarChar){ Value = Nomor },
            new SqlParameter("@id_tradein", SqlDbType.Int){ Value = ID}
        };

        if (Brand == "MD")
            query_ti = "sp_gettradeinmd";
        else if (Brand == "TP")
            query_ti = "sp_gettradeintp";
        else if (Brand == "FC")
            query_ti = "sp_gettradein";
        else if (Brand == "PL")
            query_ti = "sp_gettradeinlei";

        Task.Run(async () => { await ExecuteAsync(query_ti, Db.Cnn_CS, parameters, CommandType.StoredProcedure); });
    }

    public static async Task ExecuteAsync(string strSql, string CnnString, List<SqlParameter> parameters, CommandType command)
    {
        using (SqlConnection conn = new SqlConnection(CnnString))
        {
            await conn.OpenAsync();
            using (SqlCommand sqlcmd = new SqlCommand(strSql, conn))
            {
                sqlcmd.CommandType = command;
                if (parameters != null)
                {
                    foreach (var x in parameters)
                    {
                        sqlcmd.Parameters.Add(x);
                    }
                }
                sqlcmd.CommandTimeout = 300;
                await sqlcmd.ExecuteNonQueryAsync();
            }
        }
    }
    #endregion
}
