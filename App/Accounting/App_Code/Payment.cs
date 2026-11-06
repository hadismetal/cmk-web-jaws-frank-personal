using System;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

/// <summary>
/// Customer System
/// </summary>
public partial class Payment
{

    public static void FillPayment(int myID, Table tbl)
    {
        using (Map_DB db = new Map_DB())
        {

    
            decimal totalPaid = 0m;

            var rs = from p in db.SalesReceiptDetail
                     where p.SalesReceipt.IDSalesOrder.Value == myID
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
                        sContent += popoverRowContent(wlabel, "Bank Issuer", (r.IDBankIssuer.HasValue ? r.BankIssuer.Nama : " - "));
                        sContent += popoverRowContent(wlabel, "MDR", Str.Number0(r.MDR));

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
                        xicon = "<span class='glyphicon glyphicon-ok' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    }
                    else if (r.IDPaymentType == 6)
                    {
                        // tradein / exchange
                        xicon = "<span class='glyphicon glyphicon-ok' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    }
                    else if (r.IDPaymentType == 8)
                    {
                        // down payment
                        xicon = "<span class='glyphicon glyphicon-ok' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                        sContent += popoverRowContent(wlabel, "DP Number", (r.CCNumber));
                    }
                    else if (r.IDPaymentType == 5)
                    {
                        // voucher
                        xicon = "<span class='glyphicon glyphicon-ok' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                        sContent += popoverRowContent(wlabel, "Voucher Number", (r.CCNumber));
                        sContent += popoverRowContent(wlabel, "Voucher Type", (r.CCName));
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

    #region Void Sales Order Payment

    public static string voidSalesOrderPaymentAll(SalesOrder d)
    {
        using (Map_DB db = new Map_DB())
        {

            string result = "";

            //foreach (var r in d.SalesReceipt)
            //{
            //    foreach (var rslt in r.SalesReceiptDetail.Where(p => p.VoidReference == null))
            //    {
            //        result += voidSalesOrderPayment(d, rslt);
            //    }
            //}

            var srd = db.SalesReceiptDetail.Where(p => p.VoidReference == null && p.SalesReceipt.IDSalesOrder == d.ID);

            foreach (var rslt in srd)
            {
                if (srd.Sum(p => p.Nominal) > 0)
                {
                    result += voidSalesOrderPayment(d, rslt);
                }
            }

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
            n.MDR = rslt.MDR;
            n.Nominal = Pembayaran;
            n.OperatorNama = App.UserNama;
            n.OperatorTgl = System.DateTime.Now;
            n.VoidReference = rslt.ID; // Add Void Reference ID dari pembayaran yang mau di void

            db.SalesReceiptDetail.InsertOnSubmit(n);
            db.SubmitChanges();

            int IDvoid = n.ID;
            var xvoid = db.SalesReceiptDetail.Single(p => p.ID == rslt.ID);
            xvoid.VoidReference = IDvoid; // Update Void Reference dari ID pembayaran lawannya
            db.SubmitChanges();

            if (rslt.IDPaymentType == 5) { returnVoucher(d, rslt); }
            if (rslt.IDPaymentType == 8) { returnDPPO(d, rslt); }
            if (rslt.IDPaymentType == 9) { result += returnGiftCard(d, rslt); }

            if (isSingleData) { postSalesOrderPayment(d); }

            return result;
            }
    }

    protected static void postSalesOrderPayment(SalesOrder d)
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

                Db.Execute("EXEC " + SPname + " " + "@p_nomor = N'" + d.Nomor + "'," + "@id_pos = " + d.ID.ToString() + "", Db.Cnn_CS);
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
}
