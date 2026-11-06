using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UC_DetailPayment2Control : System.Web.UI.UserControl
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

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        base.Dispose();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void SetDetailPayment(int DPPOID)
    {
              
        var py = from p in db.SalesReceiptDPPODetail
                 where p.SalesReceiptDPPO.ID == DPPOID
                 select p;



        if (py.Count() > 0)
        {
            string DetailPayment = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"noborder\"><tr><td colspan=\"4\"><em>Detail of payment</em></td></tr>";
            
            int lakuemasdetect = 0;
            foreach (var r in py)
            {
                // keterangan credit card
                if (r.IDPaymentType == 2 && r.IDProgram.HasValue && r.IDBankIssuer.HasValue && r.IDCard.HasValue && r.IDEDC.HasValue && !(r.IDEDC == 24 || r.IDEDC == 26 || r.IDEDC == 27 || r.IDEDC == 28)){
                    DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">" + r.PaymentType.Nama + "</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + r.Nominal.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>(" + r.BankIssuer.Nama + " " + r.CardType.Nama + " " + r.ProgramCicilan.Nama + " / EDC : " + r.EDCList.Nama + ")</td></tr>";
                }
                // keterangan debit card
                else if (r.IDPaymentType == 4 && r.IDBankIssuer.HasValue && r.IDCard.HasValue && r.IDEDC.HasValue && !(r.IDEDC == 24 || r.IDEDC == 26 || r.IDEDC == 27 || r.IDEDC == 28)){
                    DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">" + r.PaymentType.Nama + "</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + r.Nominal.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>(" + r.BankIssuer.Nama + " " + r.CardType.Nama + " / EDC : " + r.EDCList.Nama + ")</td></tr>";
                }else if (r.IDPaymentType != 5){
                    string weightlakuemas = "";
                    string paymentNama = (r.PaymentType.Nama.Trim().ToUpper() == "LAKU EMAS") ? "Fine Gold / Logam Mulia" : ((r.PaymentType.Nama == "Exchange") ? r.PaymentType.Nama + "/Trade In" : r.PaymentType.Nama) ;
                    string width = "120";
                    if (r.IDPaymentType == 11)
                    {
                        weightlakuemas = Db.SingleDecimal("Exec sp_select_no_laku_emas '" + r.CCNumber + "'", Db.Cnn_DB) + " gram";
                        width = "200";
                    }
                    DetailPayment += "<tr><td style='vertical-align:top;' width=\"" + width + "\">" + paymentNama + "</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>" + ((r.IDPaymentType == 11) ? r.Nominal.Value.ToString("N0") + " (" + weightlakuemas + ")" : r.Nominal.Value.ToString("N0")) + "</td><td></td></tr>";
                }
                    
            }
            

            //firman add 27082018 grouping payment voucher pada print invoice
            var pymVchr = from pyv in py where pyv.IDPaymentType == 5 select pyv;

            if (pymVchr.Count() > 0)
            {

                var group_payment_voucher = from gpym in py
                                            group gpym by new { gpym.IDPaymentType, gpym.Nominal, gpym.CCName } into spym
                                            select new
                                            {
                                                spym.Key.IDPaymentType,
                                                spym.Key.CCName,
                                                spym.Key.Nominal,
                                                Qty = spym.Count(),
                                                Total = spym.Key.Nominal * spym.Count()
                                            };

                foreach (var pymVchrItem in group_payment_voucher)
                {
                    if (pymVchrItem.IDPaymentType == 5)
                    {
                        DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\"> Voucher </td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + pymVchrItem.Total.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>(" + pymVchrItem.Qty.ToString() + "x " + pymVchrItem.Nominal.Value.ToString("N0") + " " + pymVchrItem.CCName.ToString() + ")</td></tr>";
                    }
                }
            }

            DetailPayment += "</table>";
            detail_payment.Text = DetailPayment;
        
        }
    }
}
