using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_Payment_Payment_Void : System.Web.UI.Page
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
    protected int IDSO { get { return App.QueryInt(this); } }
    protected int IDPayment { get { return App.QueryInt(this, "idpayment"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

        }

        Save();
    }
    //Exception
    protected void DataError() { Response.Redirect("Payment.aspx"); }

    protected void Save()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == IDSO);
        var srd = db.SalesReceiptDetail.SingleOrDefault(p => p.ID == IDPayment);
        var sr = db.SalesReceipt.SingleOrDefault(p => p.ID == srd.IDSalesReceipt);
        if (srd == null) DataError();
        else
        {
            //voidSalesOrderPayment(d, srd, false);
            voidSalesOrderPaymentNewGeneration(d, srd, sr);
            Response.Redirect("Payment_ItemList.aspx?id=" + IDSO);
        }
    }

    protected void voidSalesOrderPaymentNewGeneration(SalesOrder d, SalesReceiptDetail rslt, SalesReceipt rs)
    {
        decimal Pembayaran = 0m;
        Pembayaran = rslt.Nominal.Value * -1;

        /*if (rslt.IDPaymentType == 5)
        {
            bool voidx = false;
            // Validation and STAMPS Posting
            try
            {
                if (STAMPS.xActivate)
                {
                    // STAMPS VOID VOUCHER
                    voidx = STAMPSM.VoidVoucherByPLUPerDetailPayment(rslt);
                }
            }
            catch (Exception ex)
            { }
            if (voidx == true)
            {
                Payment.returnVoucher(d, rslt);
            }
        }
        if (rslt.IDPaymentType == 8) { Payment.returnDPPOPerPaymentItem(d, rslt); }
        if (rslt.IDPaymentType == 9) { Payment.returnGiftCard(d, rslt); }
        if (rslt.IDPaymentType == 11) { Payment.returnLakuEmasNumber(d, rslt); }*/

        d.Paid = d.Paid + Pembayaran;
        d.Unpaid = d.Unpaid - Pembayaran;

        db.SalesReceiptDetail.DeleteOnSubmit(rslt);
        db.SalesReceipt.DeleteOnSubmit(rs);
        db.SubmitChanges();
    }
	
	public string voidSalesOrderPayment(SalesOrder d, SalesReceiptDetail rslt, bool isSingleData = false)
    {
        string result = "";

        decimal Pembayaran = 0m;
        Pembayaran = rslt.Nominal.Value * -1;
		
		SalesReceipt m = new SalesReceipt();
		m.IDSalesOrder = IDSO;
		m.NoSalesOrder = d.Nomor;
		m.TotalBayar = Pembayaran;
        m.OperatorNama = App.UserNama;
        m.OperatorTgl = DateTime.Now;
		db.SalesReceipt.InsertOnSubmit(m);
		db.SubmitChanges();

        SalesReceiptDetail n = new SalesReceiptDetail();

        n.IDBankIssuer = rslt.IDBankIssuer;
        n.IDCard = rslt.IDCard;
        n.CCName = rslt.CCName;
        n.CCNumber = rslt.CCNumber;
        n.IDEDC = rslt.IDEDC;
        n.IDPaymentType = rslt.IDPaymentType;
        n.IDProgram = rslt.IDProgram;
        n.IDSalesReceipt = m.ID;
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
		
		if(0 < d.Unpaid + Pembayaran)
		{
			d.StatusPembayaran = false;
		}
		d.Paid = d.Paid + Pembayaran;
		d.Unpaid = d.Unpaid - Pembayaran;
		
		db.SubmitChanges();
			

        if (rslt.IDPaymentType == 5) 
		{ 
			bool voidx = false;
            // Validation and STAMPS Posting
            try
            {
                if (STAMPS.xActivate)
                {
                    // STAMPS VOID VOUCHER
                    voidx = STAMPSM.VoidVoucherByPLUPerDetailPayment(rslt);
                }
            }
            catch (Exception ex)
            {
                result = "Terjadi kesalahan teknis. Silahkan hubungi IT Support. (STAMPS Error message : "
                    + ex.Message
                    + ex.StackTrace
                    + ex.InnerException
                    + ")";
            }
            if (voidx == true)
            {
                Payment.returnVoucher(d, rslt);
            }  
		}
        if (rslt.IDPaymentType == 8) { Payment.returnDPPOPerPaymentItem(d, rslt); }
        if (rslt.IDPaymentType == 9) { result += Payment.returnGiftCard(d, rslt); }
		if (rslt.IDPaymentType == 11) { result += Payment.returnLakuEmasNumber(d,rslt); }

        if (isSingleData) { Payment.postSalesOrderPayment(d); }

        return result;
    }
}