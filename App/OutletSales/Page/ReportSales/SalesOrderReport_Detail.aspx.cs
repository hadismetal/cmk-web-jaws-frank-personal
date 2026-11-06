using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

public partial class SalesOrderReport_Detail : System.Web.UI.Page
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

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        bool statusVoid = false;
        div_void.Visible = false;

        var x = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (x == null) DataError();
        else
        {
            //if (!(!x.Draft && !x.StatusVoid.Value && !x.TglVoid.HasValue))
            if (x.StatusVoid == true && x.TglVoid != null && x.KeteranganVoid != null)
            {
                statusVoid = true;
                div_void.Visible = true;

                string xText = "<div class='alert alert-danger' role='alert'><div class='row'>"
                                + "<div class='col-xs-2 col-sm-2 col-md-1'><span style='font-size:34px;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>&nbsp;&nbsp;&nbsp;</div>"
                                + "<div class='col-xs-10 col-sm-10 col-md-11'>"
                                + "<div class='col-xs-12 col-sm-12 col-md-12'><b>Warning !!!</b> Dokumen dengan nomor Sales Order # "
                                + "<b>" + x.Nomor + "</b> dalam status <b>Void</b>.<br /><br />Keterangan Void :<br />" + x.KeteranganVoid
                                + "</div>"
                                + "</div></div></div>"
                                ;

                if (x.Draft)
                {
                    xText = "<div class='alert alert-danger' role='alert'><div class='row'>"
                        + "<div class='col-xs-2 col-sm-2 col-md-1'><span style='font-size:34px;' class='glyphicon glyphicon-remove' aria-hidden='true'></span>&nbsp;&nbsp;&nbsp;</div>"
                        + "<div class='col-xs-10 col-sm-10 col-md-11'>"
                        + "<div class='col-xs-12 col-sm-12 col-md-12'><b>Warning !!!</b> Dokumen ini dalam status <b>Draft</b>."
                        + "</div>"
                        + "</div></div></div>"
                        ;
                }

                lit_info_invalid.Text = xText;
            }

            if (!IsPostBack)
            {
                fillForm(statusVoid);
            }

        }

    }

    //Exception
    protected void DataError() { Response.Redirect("SalesOrderReport.aspx"); }

    //Form
    protected void fillForm(bool statusVoid)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
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
            operator_input.InnerHtml = d.Operator + " (" + Str.TglJam(d.DraftDate) + ")";
            tgl.InnerHtml = Str.Tgl(d.Tgl);



            //Stone
            fillItem(statusVoid);

            decimal NilaiSO = 0m;
            decimal Paid = 0m;
            decimal Unpaid = 0m;

            try
            {
                NilaiSO = d.TotalRupiahBeforeDisc.Value;
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

            //adding firman 280818 perubahan bagian summary dari user control /UC/PaymentSummary.ascx
            paymentSummary.SebelumDiscount = NilaiSO.ToString("N0");
            paymentSummary.TotalDiscount = grandTotalDiscount.ToString("N0");
            paymentSummary.TotalHarusDiBayar = nilaiHarusDibayar.ToString("N0");
            paymentSummary.SebelumTradeIn = nilaiHarusDibayar.ToString("N0");
            paymentSummary.NilaiTradeIn = nilaiResell.ToString("N0");
            paymentSummary.SisaHarusDiBayar = (nilaiHarusDibayar - nilaiResell).ToString("N0");
            paymentSummary.TotalTerbayar = Paid.ToString("N0");

            invoice.Visible = false;
            booking.Visible = false;

            //adding firman 280818 perubahan summary_kurangbayar jadi status_pembayaran
            bool StatusBayar = Convert.ToBoolean(d.StatusPembayaran);
            if (Unpaid == 0)
            {
                if (StatusBayar == true && !statusVoid)
                {
                    paymentSummary.StatusPembayaran = "<font style='color:green;font-weight:bold';>Lunas</font>";
                    invoice.Visible = true;
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

            FillPayment(myID, tb3);
        }
    }

    protected void FillPayment(int myID, Table tbl)
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
                    // laku emas
                    xicon = "<span class='glyphicon glyphicon-inbox' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;
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
                    // Xendit
                    xicon = "<span class='icon-ticket' aria-hidden='true'></span>&nbsp;" + r.PaymentType.Nama;

                    sContent += popoverRowContent(wlabel, "Voucher Number", (r.CCNumber));
                    sContent += popoverRowContent(wlabel, "Voucher Type", (r.CCName));
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

                tbl.Rows.Add(tr);

                index++;
            }

            Sub_Total_Paid(totalPaid, tbl);
        }
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


    //Stone
    protected void fillItem(bool statusVoid)
    {
        decimal GT = 0m;
        decimal GT2 = 0m;
        //Item
        fillItemDJ(ref GT, ref GT2, statusVoid);
        fillItemPG(ref GT, ref GT2, statusVoid);
        fillItemGJ(ref GT, ref GT2, statusVoid);
        fillItemLD(ref GT, ref GT2, statusVoid);

        //Grand-Total
        Sub_Total(GT, GT2, true);
    }

    protected string fillVoucherDJ(SalesOrderDJ dj)
    {
        string text = "";
        var vcr = from p in db.SalesVoucher
                  where p.IDSalesOrder == dj.IDForm && p.ProductType == 1 && p.ProductID == dj.IDProduct
                  select p;
        int i = 1;
        text += "<div style=\"font-size: 12px;text-align:left\">";
        foreach (var voucher in vcr)
        {
            text += "<small>" + i + ".) " + voucher.NomorVoucher + " (" + Convert.ToInt32(voucher.Nominal).ToString("N0") + ")" + "</small>";
            text += "<br/>";
            i++;
        }
        text += "</div>";

        return text;
    }

    protected string fillVoucherPG(SalesOrderPG pg)
    {
        string text = "";
        var vcr = from p in db.SalesVoucher
                  where p.IDSalesOrder == pg.IDForm && p.ProductType == 2 && p.ProductID == pg.IDProduct
                  select p;
        int i = 1;
        text += "<div style=\"font-size: 12px;text-align:left\">";
        foreach (var voucher in vcr)
        {
            text += "<small>" + i + ".) " + voucher.NomorVoucher + " (" + Convert.ToInt32(voucher.Nominal).ToString("N0") + ")" + "</small>";
            text += "<br/>";
            i++;
        }
        text += "</div>";
        return text;
    }

    protected string fillVoucherGJ(SalesOrderGJ gj)
    {
        string text = "";
        var vcr = from p in db.SalesVoucher
                  where p.IDSalesOrder == gj.IDForm && p.ProductType == 3 && p.ProductID == gj.IDProduct
                  select p;
        int i = 1;
        text += "<div style=\"font-size: 12px;text-align:left\">";
        foreach (var voucher in vcr)
        {
            text += "<small>" + i + ".) " + voucher.NomorVoucher + " (" + Convert.ToInt32(voucher.Nominal).ToString("N0") + ")" + "</small>";
            text += "<br/>";
            i++;
        }
        text += "</div>";
        return text;
    }

    protected string fillVoucherLD(SalesOrderLD ld)
    {
        string text = "";
        var vcr = from p in db.SalesVoucher
                  where p.IDSalesOrder == ld.IDForm && p.ProductType == 4 && p.ProductID == ld.IDProduct
                  select p;
        int i = 1;
        text += "<div style=\"font-size: 12px;text-align:left\">";
        foreach (var voucher in vcr)
        {
            text += "<small>" + i + ".) " + voucher.NomorVoucher + " (" + Convert.ToInt32(voucher.Nominal).ToString("N0") + ")" + "</small>";
            text += "<br/>";
            i++;
        }
        text += "</div>";
        return text;
    }

    protected void fillItemDJ(ref decimal GT, ref decimal GT2, bool statusVoid)
    {
        var rs = from p in db.SalesOrderDJ
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(0);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = statusVoid ? "" : TD_HyperLink(r);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Invoice
                    + "<br />" + r.StockProductDJ.Nomor
                    + "<br />" + r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama
                    ;
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountNominal).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r.Discount).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountPromo).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgramNominal).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r.DiscountProgram).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0") + "<br />" + fillVoucherDJ(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountBank).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountOther).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountRound).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            decimal TotalFeeExtraRingSize = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalFeeExtraRingSize,0) FROM SalesOrderDJ WHERE ID = " + r.ID, Db.Cnn_DB);
            td.Text = Convert.ToDecimal(TotalFeeExtraRingSize).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);
            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GT += ST;
            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }

    }
    protected void fillItemPG(ref decimal GT, ref decimal GT2, bool statusVoid)
    {
        var rs = from p in db.SalesOrderPG
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(1);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = statusVoid ? "" : TD_HyperLink(r);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Invoice
                    + "<br />" + r.StockProductPG.Nomor
                    + "<br />" + r.StockProductPG.CharProductItem.Nama
                    ;
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountNominal).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r.Discount).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountPromo).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgramNominal).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r.DiscountProgram).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0") + "<br />" + fillVoucherPG(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountBank).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountOther).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountRound).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            decimal TotalFeeExtraRingSize = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalFeeExtraRingSize,0) FROM SalesOrderPG WHERE ID = " + r.ID, Db.Cnn_DB);
            td.Text = Convert.ToDecimal(TotalFeeExtraRingSize).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);
            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT += ST;

            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }
    }
    protected void fillItemGJ(ref decimal GT, ref decimal GT2, bool statusVoid)
    {
        var rs = from p in db.SalesOrderGJ
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(2);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = statusVoid ? "" : TD_HyperLink(r);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Invoice
                    + "<br />" + r.StockProductGJ.Nomor
                    + "<br />" + r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama
                    ;
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountNominal).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r.Discount).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountPromo).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgramNominal).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r.DiscountProgram).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0") + "<br />" + fillVoucherGJ(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountBank).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountOther).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountRound).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            decimal TotalFeeExtraRingSize = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalFeeExtraRingSize,0) FROM SalesOrderGJ WHERE ID = " + r.ID, Db.Cnn_DB);
            td.Text = Convert.ToDecimal(TotalFeeExtraRingSize).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);
            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GT += ST;
            GT2 += ST2;

            Sub_Total(ST, ST2, false);
        }
    }
    protected void fillItemLD(ref decimal GT, ref decimal GT2, bool statusVoid)
    {
        var rs = from p in db.SalesOrderLD
                 where p.IDForm == myID
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_Header(3);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = statusVoid ? "" : TD_HyperLink(r);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Invoice
                    + "<br />" + r.StockProductLD_Stone1B.Nomor
                    ;
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountNominal).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r.Discount).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountPromo).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgramNominal).ToString("N0")
                    + "<br />(" + Convert.ToDecimal(r.DiscountProgram).ToString("N2") + " %)"
                    ;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0") + "<br />" + fillVoucherLD(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountBank).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountOther).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountRound).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            decimal TotalFeeExtraRingSize = Db.SingleDecimal("SELECT TOP 1 ISNULL(TotalFeeExtraRingSize,0) FROM SalesOrderLD WHERE ID = " + r.ID, Db.Cnn_DB);
            td.Text = Convert.ToDecimal(TotalFeeExtraRingSize).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);
            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;

        }
        //Total
        if (rs.Count() > 0)
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
        td.ColumnSpan = 3;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(Total) + "</p>";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.ColumnSpan = 8;
        td.Text = "";
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
    protected void PrintSO_Click(object sender, EventArgs e)
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.StatusPembayaran == true && p.StatusVoid == false);
        var pg = d.SalesOrderPG;
        bool isvoucherlei = false;
        try
        {
            foreach (var r in pg)
            {
                if (r.StockProductPG.CharGoldLevel.NamaKode == "VLEI")
                {
                    ApiVoucherLEI le = new ApiVoucherLEI();
                    le.AddTransactionLEI(myID);
                    string pageurl = "/OutletSales/Page/Report/Print_InvoiceVoucherLEI.aspx?id=" + r.ID;
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");

                    isvoucherlei = true;
                }
            }

            //Response.Redirect("/OutletSales/Page/Create/Payment/Sales_Invoice.aspx?id=" + myID);
            if (isvoucherlei)
            {
                string pageurl = "/OutletSales/Page/Create/Payment/Sales_InvoiceVoucherLEI.aspx?id=" + myID;
                Response.Write("<script>");
                Response.Write("window.open('" + pageurl + "','_blank')");
                Response.Write("</script>");
            }
            else
            {
                if (d.Tgl <= Convert.ToDateTime("2023-08-03 23:59:00.000"))
                {
                    string pageurl = "/OutletSales/Page/Create/Payment/Sales_Invoice_2023_08_02.aspx?id=" + myID;
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
                else
                {
                    string pageurl = "/OutletSales/Page/Create/Payment/Sales_Invoice.aspx?id=" + myID;
                    Response.Write("<script>");
                    Response.Write("window.open('" + pageurl + "','_blank')");
                    Response.Write("</script>");
                }
            }

            printCoupon(d);
        }
        catch
        {
            lit_error.Text = "Terjadi gangguan pada sistem voucher. Silahkan hubungi IT LEI untuk melanjutkan proses!";
        }
    }

    protected void printCoupon(SalesOrder d)
    {
        string spname = "[sp_create_coupon_prizedraw]";
        string query = " exec " + spname
                        + " 'SO'"
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

    protected void PrintBooking_Click(object sender, EventArgs e)
    {
        Response.Redirect("/OutletSales/Page/Create/Payment/Sales_DP.aspx?id=" + myID);
    }
    protected string TD_HyperLink(SalesOrderDJ r)
    {
        string x = string.Empty;

        if (r.SalesOrder.StatusPembayaran == true && r.SalesOrder.StatusVoid == false && r.SalesOrder.Unpaid == 0)
        {
            var idso = System.Text.Encoding.UTF8.GetBytes(r.SalesOrder.ID.ToString());
            string xidso = System.Convert.ToBase64String(idso);

            var idinv = System.Text.Encoding.UTF8.GetBytes(r.ID.ToString());
            string xidinv = System.Convert.ToBase64String(idinv);

            var plu = System.Text.Encoding.UTF8.GetBytes(r.StockProductDJ.Nomor);
            string xplu = System.Convert.ToBase64String(plu);

            if (r.SalesOrder.Tgl <= Convert.ToDateTime("2023-08-03 23:59:00.000"))
            {
                x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoiceDJ_2023_08_02.aspx?id=" + r.ID + "\">";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
                x += "</div>";
            }
            else if (r.SalesOrder.Tgl <= Convert.ToDateTime("2023-10-22 23:59:00.000"))
            {
                x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoiceDJ_2023_10_23.aspx?id=" + r.ID + "\">";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
                x += "</div>";
            }
            else if (r.SalesOrder.Tgl <= Convert.ToDateTime("2025-03-04 23:59:00.000"))
            {
                x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoiceDJ_2025_02_28.aspx?id=" + r.ID + "\">";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
                x += "</div>";
            }
            else
            {
                x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoiceDJ.aspx?id=" + r.ID + "\">";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
                x += "</div>";
            }
            x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
            //x += "<a class=\"btn btn-primary\" target=\"_blank\" href=\"../Report/Print_Certificate_DJ5.aspx?idproduct=" + r.IDProduct + "\">";
			x += "<a class=\"btn btn-primary\" target=\"_blank\" href=\"../Report/Print_Certificate_DJ4.aspx?SO=" + xidso + "&INV=" + xidinv + "&PLU=" + xplu + "\">";
            x += "  <i class=\"icon-credit-card\"></i> ";
            x += "  ";
            x += "</a>";
            x += "</div>";
        }
        return x;
    }
    protected string TD_HyperLink(SalesOrderGJ r)
    {
        string x = string.Empty;

        if (r.SalesOrder.StatusPembayaran == true && r.SalesOrder.StatusVoid == false && r.SalesOrder.Unpaid == 0)
        {
            if (r.SalesOrder.Tgl <= Convert.ToDateTime("2023-08-03 23:59:00.000"))
            {
                x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoiceGJ_2023_08_02.aspx?id=" + r.ID + "\">";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
                x += "</div>";
            }
            else if (r.SalesOrder.Tgl <= Convert.ToDateTime("2025-03-04 23:59:00.000"))
            {
                x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoiceGJ_2025_02_28.aspx?id=" + r.ID + "\">";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
                x += "</div>";
            }
            else
            {
                x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoiceGJ.aspx?id=" + r.ID + "\">";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
                x += "</div>";
            }
        }
        return x;
    }
    protected string TD_HyperLink(SalesOrderPG r)
    {
        string x = string.Empty;

        if (r.SalesOrder.StatusPembayaran == true && r.SalesOrder.StatusVoid == false && r.SalesOrder.Unpaid == 0)
        {
            if (r.StockProductPG.CharGoldLevel.NamaKode == "VLEI")
            {
                x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoiceVoucherLEI.aspx?id=" + r.ID + "\">";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
                x += "</div>";
            }
            else
            {
                if (r.SalesOrder.Tgl <= Convert.ToDateTime("2023-08-03 23:59:00.000"))
                {
                    x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                    x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoicePG_2023_08_02.aspx?id=" + r.ID + "\">";
                    x += "  <i class=\"icon-print\"></i> ";
                    x += "  ";
                    x += "</a>";
                    x += "</div>";
                }
                else if (r.SalesOrder.Tgl <= Convert.ToDateTime("2023-10-22 23:59:00.000"))
                {
                    x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                    x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoicePG_2023_10_23.aspx?id=" + r.ID + "\">";
                    x += "  <i class=\"icon-print\"></i> ";
                    x += "  ";
                    x += "</a>";
                    x += "</div>";
                }
                else if (r.SalesOrder.Tgl <= Convert.ToDateTime("2025-03-04 23:59:00.000"))
                {
                    x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                    x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoicePG_2025_02_28.aspx?id=" + r.ID + "\">";
                    x += "  <i class=\"icon-print\"></i> ";
                    x += "  ";
                    x += "</a>";
                    x += "</div>";
                }
                else
                {
                    x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                    x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoicePG.aspx?id=" + r.ID + "\">";
                    x += "  <i class=\"icon-print\"></i> ";
                    x += "  ";
                    x += "</a>";
                    x += "</div>";
                }
            }
        }
        return x;
    }
    protected string TD_HyperLink(SalesOrderLD r)
    {
        string x = string.Empty;

        if (r.SalesOrder.StatusPembayaran == true && r.SalesOrder.StatusVoid == false && r.SalesOrder.Unpaid == 0)
        {
            if (r.SalesOrder.Tgl <= Convert.ToDateTime("2023-08-03 23:59:00.000"))
            {
                x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoiceLD_2023_08_02.aspx?id=" + r.ID + "\">";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
                x += "</div>";
            }
            else if (r.SalesOrder.Tgl <= Convert.ToDateTime("2025-03-04 23:59:00.000"))
            {
                x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoiceLD_2025_02_28.aspx?id=" + r.ID + "\">";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
                x += "</div>";
            }
            else
            {
                x += "<div class=\"btn-group\" style=\"padding-bottom:5px;\">";
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Report/Print_InvoiceLD.aspx?id=" + r.ID + "\">";
                x += "  <i class=\"icon-print\"></i> ";
                x += "  ";
                x += "</a>";
                x += "</div>";
            }
        }
        return x;
    }
}