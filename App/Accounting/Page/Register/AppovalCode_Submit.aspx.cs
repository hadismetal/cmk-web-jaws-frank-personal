using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Register_AppovalCode_Submit : System.Web.UI.Page
{
    protected int idpayment { get { return Convert.ToInt32(Request.QueryString["id"]); } }
    protected string approvalcode { get { return Request.QueryString["appcode"]; } }
    protected int idedc { get { return Convert.ToInt32(Request.QueryString["idedc"]); } }
    protected string isdp { get { return Request.QueryString["isdp"]; } }
    protected string so { get { return Request.QueryString["so"]; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        save();

    }
    private void save()
    {
        string msg = "";
        try
        {
            using (Map_DB db = new Map_DB())
            {
                if (isdp == "F")
                {
                    var payment = db.SalesReceiptDetail.SingleOrDefault(p => p.ID == idpayment);
                    var paymentext = payment.SalesReceiptDetailExtension;
                    if (paymentext != null)
                    {
                        paymentext.TransactionNumber = approvalcode;
                        payment.IDEDC = idedc;
						if((idedc == 31 || idedc == 32))
						{
							payment.IDPaymentType = 15;
						}
						else if(idedc == 29 || idedc == 33)
						{
							payment.IDPaymentType = 13;
						}

                        db.SubmitChanges();
                        msg = "Update Approval Code '"+approvalcode+"' pada SO '"+so+"' Berhasil";
                    }
                    else
                    {
                        SalesReceiptDetailExtension srde = new SalesReceiptDetailExtension();
                        srde.ID = payment.ID;
                        srde.VerificationDateTime = DateTime.Now;
                        srde.VerificationFlag = 1;
                        srde.PaymentName = payment.PaymentType.Nama;
                        srde.TransactionNumber = approvalcode;
                        srde.VerificationOperator = App.UserNama;
                        db.SalesReceiptDetailExtension.InsertOnSubmit(srde);
                        db.SubmitChanges();
                        msg = "Update Approval Code '" + approvalcode + "' pada SO '" + so + "' Berhasil";
                    }
                }
                else
                {
                    var paymentdp = db.SalesReceiptDPPODetail.SingleOrDefault(p => p.ID == idpayment);
                    var paymentext = paymentdp.SalesReceiptDPPODetailExtension;
                    if (paymentext != null)
                    {
                        paymentext.TransactionNumber = approvalcode;
                        paymentdp.IDEDC = idedc;
						if((idedc == 31 || idedc == 32))
						{
							paymentdp.IDPaymentType = 15;
						}
						else if(idedc == 29 || idedc == 33)
						{
							paymentdp.IDPaymentType = 13;
						}

                        db.SubmitChanges();
                        msg = "Update Approval Code '" + approvalcode + "' pada DP '" + so + "' Berhasil";
                    }
                    else
                    {
                        SalesReceiptDPPODetailExtension srde = new SalesReceiptDPPODetailExtension();
                        srde.ID = paymentdp.ID;
                        srde.VerificationDateTime = DateTime.Now;
                        srde.VerificationFlag = 1;
                        srde.PaymentName = paymentdp.PaymentType.Nama;
                        srde.TransactionNumber = approvalcode;
                        srde.VerificationOperator = App.UserNama;
                        db.SalesReceiptDPPODetailExtension.InsertOnSubmit(srde);
                        db.SubmitChanges();
                        msg = "Update Approval Code '" + approvalcode + "' pada DP '" + so + "' Berhasil";
                    }
                }
            }
        }
        catch(Exception ex)
        {
            msg = ex.Message.ToString();
        }
        Response.Redirect("ApprovalCode.aspx?msg=" + msg + "&d=1");
    }
}