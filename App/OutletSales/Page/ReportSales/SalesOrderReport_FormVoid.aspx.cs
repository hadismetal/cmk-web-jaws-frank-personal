using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Data;
using System.Web.UI;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Net.Http;
using System.Configuration;
using System.Data.SqlClient;


public partial class SalesOrderReport_FormVoid : System.Web.UI.Page
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
    protected void DataError() { Response.Redirect("SalesOrderReport.aspx"); }

    //Form
    protected void fillForm()
    {
        voidgiftcard.Visible = false;
        bool isValidVoid = true;
        string msg = "";

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            so.InnerHtml = d.Nomor;
            so.Style.Add("color", "red");
            int countvoucherlei = Db.SingleInteger("EXEC sp_count_total_voucher_lei_sales_order_new " + myID, Db.Cnn_DB);

            if (d.StatusVoid == true)
            {
                // kondisi sudah tervoid
                lit_keteranganvoid.Text = "<br />" + d.KeteranganVoid.Trim();
                keterangan.Visible = false;
                isValidVoid = false;

                // kondisi jika saldo gift card belum terverifikasi untuk pengembalian saldo
                if (Payment.showVoidPaymentStatusGiftCard(d))
                {
                    isValidVoid = false;
                    voidgiftcard.Visible = true;
                    msg += "Terdapat Pembayaran Gift Card yang tidak dapat terverifikasi. Silahkan melakukan verifikasi ulang dengan menekan tombol <b>Verifikasi</b>. Jika tombol tidak berfungsi, silahkan menghubungi IT Administrator.<br /><br />";
                }
            }
            else if (countvoucherlei > 0)
            {
                isValidVoid = false;
                msg += "Voucher LEI tidak dapat divoid.";
            }
            else
            {
                isValidVoid = true;
                int IDSO = 0;
                string statusApprove = "";
                string statusRejected = "";
                string query = @"select IDSO,ISNULL(Approval,'belom') Approval,isnull(Rejected,'belom') Rejected from ApprovalSOVoid where IDSO=@ID";

                        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {                                                                
                                cmd.Parameters.AddWithValue("@ID", myID);
                                conn.Open();                                

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    // Loop through the results
                                    while (reader.Read())
                                    {                                     
                                        IDSO = reader.GetInt32(reader.GetOrdinal("IDSO"));                                                                                
                                        statusApprove = Convert.ToString(reader["Approval"]);                                     
                                        statusApprove = Convert.ToString(reader["Rejected"]);                                                                             
                                    }
                                }
                                conn.Close(); 
                            }
                        }  

                if (!VoidValidation.validVoidSalesOrderRole(d, Module))
                {
                    div_keterangan_void.Visible = false;
                    isValidVoid = false;
                    msg += "<br />" + "Void tidak dapat diteruskan karena sudah lewat hari.";
                }
                if (!VoidValidation.validVoidSalesOrderNoResell(d))
                {
                    div_keterangan_void.Visible = false;
                    isValidVoid = false;
                    msg += "<br />" + "Void tidak dapat diteruskan karena terdapat PLU yang sudah di resell / tradein.";
                }
                if (!VoidValidation.validVoidSalesOrderNoVoucherExpire(d))
                {
                    div_keterangan_void.Visible = false;
                    isValidVoid = false;
                    msg += "<br />" + "Void tidak dapat diteruskan karena terdapat pembayaran menggunakan Voucher yang sudah expire.";
                }
                if (!VoidValidation.validVoidSalesOrderNoMoreThanOneYear(d))
                {
                    div_keterangan_void.Visible = false;
                    isValidVoid = false;
                    msg += "<br />" + "Void tidak dapat diteruskan karena sudah lewat dari 1 tahun.";
                }
                if (statusApprove == "belom" ||statusRejected == "belom")
                {
                    div_keterangan_void.Visible = false;
                    isValidVoid = false;
                    msg += "<br />" + "Void telah diajukan, dan perlu approval Store Manager.";
                }
            }
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
        Response.Redirect("SalesOrderReport.aspx");
    }

    protected void save_Click(object sender, EventArgs e)
    {
        string status_SAP = "";
        string status_STAMPS = "";

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.TglVoid == null);
        if (d != null)
        {
            // Validation and SAP Posting
            try
            {
                if (SAP.xActivate)
                {

                    status_SAP += ((status_SAP == "") ? SAPM.postVoidSalesOrderToSAP(d) : "");

                    if (d.TradeIn.HasValue)
                    {
                        var t = db.TradeIn.SingleOrDefault(p => p.IDSalesOrder.Value == d.ID);
                        var s = db.Resell.SingleOrDefault(p => p.ID == t.IDResell);

                        status_SAP += ((status_SAP == "") ? SAPM.postVoidBuybackToSAP(s) : "");
                    }
                }
            }
            catch (Exception ex)
            {
                status_SAP = "Terjadi kesalahan teknis. Silahkan hubungi IT Administrator. (Error message : " + ex.Message + ")";
            }


            // Validation and STAMPS Posting
            try
            {
                //api_doku(ConfigurationManager.AppSettings["APIJAMES"] + "/api/Payment/PaymentDokuVoid?id="+myID+"&tipe=SO");
                if (STAMPS.xActivate)
                {
                    // void all redeem voucher discount / payment stamps

                    bool voidx = false;
                    bool stampsNotActivate = false;

                    // STAMPS VOID VOUCHER
                    
                    if ((d.StatusVoid == false && d.StatusPembayaran == false))
                    {
                        string txtKeterangan = keterangan.Text.Trim();
                        txtKeterangan += " (Void by " + App.UserNama + ", " + Str.FullDateJam(System.DateTime.Now) + ")";
                        voidx = STAMPSM.VoidAllVoucherByPLU(d, 0, 0);

                        if (!(d.StatusVoid == false && d.StatusPembayaran == false && d.Draft == true))
                        {
                            voidSMSCourtesy(d, txtKeterangan);
                            voidPayment(d);
                        }
                        voidSalesOrder(d, txtKeterangan);
                        d.KeteranganVoid = txtKeterangan;
                        d.TglVoid = DateTime.Today;
                        d.StatusVoid = true;
                        d.Unpaid = d.TotalBayar;
                        d.Paid = 0;
                        db.SubmitChanges();

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "console.log([`" + status_STAMPS + "`])", true);
                        Response.Redirect("SalesOrderReport.aspx?d=1");


                    }
                    else
                    {

                        if (d.Unpaid != 0)
                        {
                            status_STAMPS += ((status_STAMPS == "") ? STAMPSM.CancelTransaction(lit_error_stamps, d) : "");
                        }
                        else if (d.Unpaid == 0)
                        {
                            status_STAMPS += ((status_STAMPS == "") ? STAMPSM.CancelTransaction(lit_error_stamps, d) : "");
                        }

                        status_SAP = ""; // di bypass untuk SAP
						
						//bypass kalo error di cmk club nya udh kevoid di jaws nya belon
						if (status_STAMPS.Contains("STAMPS Error : Transaction is already canceled.")) 
							status_STAMPS = "";

                        if (status_SAP == "" && status_STAMPS == "")
                        {

                            string txtKeterangan = keterangan.Text.Trim();
                            txtKeterangan += " (Void by " + App.UserNama + ", " + Str.FullDateJam(System.DateTime.Now) + ")";
                            voidx = STAMPSM.VoidAllVoucherByPLU(d, 0, 0);

                            if (!(d.StatusVoid == false && d.StatusPembayaran == false && d.Draft == true))
                            {
                                voidSMSCourtesy(d, txtKeterangan);
                                voidPayment(d);
                            }

                            voidSalesOrder(d, txtKeterangan);
                            d.KeteranganVoid = txtKeterangan;
                            d.TglVoid = DateTime.Today;
                            d.StatusVoid = true;
                            d.Unpaid = d.TotalBayar;
                            d.Paid = 0;
                            db.SubmitChanges();
                            // ToastrScripts("success","Void Berhasil","SalesOrderReport.aspx?d=1",status_STAMPS);
                            // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "console.log(["+status_STAMPS+"])", true);                
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "console.log([`" + status_STAMPS + "`])", true);
                            Response.Redirect("SalesOrderReport.aspx?d=1");

                        }
                        else
                        {

                            div_alert_2.Visible = true;
                            lit_error.Text = "";
                            if (status_SAP != "")
                            {
                                lit_error_sap.Text = " SAP Error : " + status_SAP + " . ";
                            }
                            if (status_STAMPS == " STAMPS Error : Transaction is already canceled.")
                            {
                                string txtKeterangan = keterangan.Text.Trim();
                                txtKeterangan += " (Void by " + App.UserNama + ", " + Str.FullDateJam(System.DateTime.Now) + ")";
                                voidx = STAMPSM.VoidAllVoucherByPLU(d, 0, 0);

                                if (!(d.StatusVoid == false && d.StatusPembayaran == false && d.Draft == true))
                                {
                                    voidSMSCourtesy(d, txtKeterangan);
                                    voidPayment(d);
                                }

                                voidSalesOrder(d, txtKeterangan);
                                d.KeteranganVoid = txtKeterangan;
                                d.TglVoid = DateTime.Today;
                                d.StatusVoid = true;
                                d.Unpaid = d.TotalBayar;
                                d.Paid = 0;
                                db.SubmitChanges();
                                // ToastrScripts("success","Void Berhasil","SalesOrderReport.aspx?d=1",status_STAMPS);
                                // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "console.log(["+status_STAMPS+"])", true);                
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "console.log([`" + status_STAMPS + "`])", true);
                                Response.Redirect("SalesOrderReport.aspx?d=1");
                            }
                            else
                            {
                                lit_error_stamps.Text = status_STAMPS;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "console.log([`" + status_STAMPS + "`])", true);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status_STAMPS = "Terjadi kesalahan teknis. Silahkan hubungi IT Support. (STAMPS Error message : "
                    + ex.Message
                    + ex.StackTrace
                    + ex.InnerException
                    + ")";
            }

        }

    }

    protected void save_Click1(object sender, EventArgs e)
    {
        string msg="";
        string status_SAP = "";
        string status_STAMPS = "";

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.TglVoid == null);
		string kodelokasi = "";
        if (d.TipeLokasi == 1)
            kodelokasi = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi).Kode;
        else if (d.TipeLokasi == 2)
            kodelokasi = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi).Kode;
        // if(kodelokasi == "E01")
        // {
        //     Db.Execute("UPDATE STAMPS_STOCK_ACTUAL_ONLINE SET STATUS_VOID = 1 WHERE NOMOR_SO = '" + d.Nomor + "' ", Db.Cnn_IN);
        // }
        // save_Click2(myID);
        if (d != null)        
        {
            try
            {
                //api_doku(ConfigurationManager.AppSettings["APIJAMES"] + "/api/Payment/PaymentDokuVoid?id="+myID+"&tipe=SO");
                if (STAMPS.xActivate)
                {
                    // void all redeem voucher discount / payment stamps

                    bool voidx = false;
                    bool stampsNotActivate = false;
                
                    
                     if ((d.Draft == true)){                        
                        // save_Click2(myID);
                    }else{  
                        string query = @"INSERT INTO ApprovalSOVoid (IDSO,KeteranganVoid,Approval,ApprovalTgl,Rejected,RejectedTgl,KeteranganReject, Created_at, Created_by)
                                        SELECT a.ID AS IDSO,@KeteranganVoid,NULL,NULL,NULL,NULL,NULL,GETDATE(),@CreatedBy
                                         FROM salesorder a                                         
                                         WHERE a.id =@ID";

                        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {                                
                                cmd.Parameters.AddWithValue("@KeteranganVoid", keterangan.Text.Trim());
                                cmd.Parameters.AddWithValue("@CreatedBy", App.UserID);
                                cmd.Parameters.AddWithValue("@ID", myID);
                                conn.Open();                                
                                int rowsAffected = cmd.ExecuteNonQuery();                        
                                conn.Close(); 

                                if (rowsAffected == 0)
                                {
                                    div_alert_2.Visible = true;
                                    msg = "<b>GAGAL</b> " + msg;
                                    lit_error.Text = msg;
                                }else{
                                    Response.Redirect("SalesOrderReport2.aspx?d=1");  
                                }
                            }
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                status_STAMPS = "Terjadi kesalahan teknis. Silahkan hubungi IT Support. (STAMPS Error message : " 
                    + ex.Message
                    + ex.StackTrace
                    + ex.InnerException
                    + ")";
            }

            

        }

    }
     

    protected void voidSMSCourtesy(SalesOrder d, string VoidDescription)
    {
        // void sms courtesy
        try
        {
            var cust = d.DataCustomer.NoCustomer.ToString().Trim();

            Db.Execute("EXEC sp_sms_courtesy "
                + "@so_no = N'" + d.Nomor + "'"
                + "," + "@so_id = " + d.ID + ""
                + "," + "@so_void_flag = 1"
                + "," + "@so_void_document = N'" + d.Nomor + "'"
                + "," + "@so_void_nocustomer = N'" + cust + "'"
                + "," + "@so_void_description = N'" + VoidDescription + "'"
            , Db.Cnn_DB);
        }
        catch (Exception) { }
    }

    protected void voidPayment(SalesOrder d)
    {
        string result = "";

        result = Payment.voidSalesOrderPaymentAll(d);

        //return result;
    }

    protected void voidSalesOrder(SalesOrder d, string VoidDescription)
    {
        var dj = d.SalesOrderDJ;
        var pg = d.SalesOrderPG;
        var gj = d.SalesOrderGJ;
        var ld = d.SalesOrderLD;
        var pc = d.SalesOrderPackaging;
        var sv = d.SalesOrderSouvenir;
        var vc = d.SalesVoucher;

        foreach (var r in vc)
        {
            string voucherNumber = r.NomorVoucher;
            var x = cs.T_voucher.SingleOrDefault(p => p.M_nomor == voucherNumber);

            if (x != null)
            {
                x.M_klaimdoc = "";
                x.M_tanggal2 = null;
                x.M_store2 = "";

                cs.SubmitChanges();
            }
        }

        foreach (var r in dj)
        {
            var x = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                Ledger.createLedgerVoidSalesOrderDJ(d, r);

                x.StatusPenjualan = 0;
                x.StockActualDJ.InHand = 1;
                x.StockActualDJ.Substorage = 1;
                db.SubmitChanges();
            }
        }

        foreach (var r in pg)
        {
            var x = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                Ledger.createLedgerVoidSalesOrderPG(d, r);

                x.StatusPenjualan = 0;
                x.StockActualPG.InHand = 1;
                x.StockActualPG.Substorage = 1;
                db.SubmitChanges();
            }
        }

        foreach (var r in gj)
        {
            var x = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                Ledger.createLedgerVoidSalesOrderGJ(d, r);

                x.StatusPenjualan = 0;
                x.StockActualGJ.InHand = 1;
                x.StockActualGJ.Substorage = 1;
                db.SubmitChanges();
            }
        }

        foreach (var r in ld)
        {
            var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                Ledger.createLedgerVoidSalesOrderLD(d, r);

                x.StatusPenjualan = 0;
                x.StockActualLD_Stone1B.InHand = 1;
                x.StockActualLD_Stone1B.Substorage = 1;
                db.SubmitChanges();
            }
        }

        foreach (var r in pc)
        {
            var x = db.StockActualPackaging.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.TipeLokasi == d.TipeLokasi && p.IDLokasi == d.IDLokasi);
            if (x != null)
            {
                if (r.Qty != null)
                {
                    x.InHand = x.InHand + r.Qty.Value;
                    db.SubmitChanges();
                }

            }
        }

        foreach (var r in sv)
        {
            if (r.TipeProduct == 0)
            {
                var x = db.StockActualSouvenir.SingleOrDefault(p => p.IDProduct == r.IDProduct && p.TipeLokasi == d.TipeLokasi && p.IDLokasi == d.IDLokasi);
                if (x != null)
                {
                    if (r.Qty != null)
                    {
                        x.InHand = x.InHand + r.Qty.Value;
                        db.SubmitChanges();
                    }
                }
            }
            else if (r.TipeProduct == 1)
            {
                var x = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                if (x != null)
                {
                    x.StatusPenjualan = 0;
                    x.StockActualDJ.InHand = 1;
                    db.SubmitChanges();
                }
            }
            else if (r.TipeProduct == 2)
            {
                var x = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                if (x != null)
                {
                    x.StatusPenjualan = 0;
                    x.StockActualGJ.InHand = 1;
                    db.SubmitChanges();
                }
            }
            else if (r.TipeProduct == 3)
            {
                var x = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                if (x != null)
                {
                    x.StatusPenjualan = 0;
                    x.StockActualPG.InHand = 1;
                    db.SubmitChanges();
                }
            }
            else if (r.TipeProduct == 4)
            {
                var x = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                if (x != null)
                {
                    x.StatusPenjualan = 0;
                    x.StockActualLD_Stone1B.InHand = 1;
                    db.SubmitChanges();
                }
            }
        }

        int tradein_resell_id = 0;
        int salesorder_id = 0;

        try
        {
            var tradein = db.TradeIn.SingleOrDefault(p => p.IDSalesOrder == d.ID);

            var resell = db.Resell.SingleOrDefault(p => p.ID == tradein.IDResell);

            foreach (var r in resell.ResellDJ)
            {
                var x = db.SalesOrderDJ.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                db.SubmitChanges();
            }

            foreach (var r in resell.ResellPG)
            {
                var x = db.SalesOrderPG.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                db.SubmitChanges();
            }

            foreach (var r in resell.ResellGJ)
            {
                var x = db.SalesOrderGJ.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                db.SubmitChanges();
            }

            foreach (var r in resell.ResellLD)
            {
                var x = db.SalesOrderLD.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                db.SubmitChanges();
            }

            tradein_resell_id = tradein.IDResell.Value;

        }
        catch (Exception) { }

        try
        {
            salesorder_id = d.ID;

            if (tradein_resell_id > 0)
            {
                Db.Execute("DELETE FROM ResellDJ WHERE IDForm = " + tradein_resell_id.ToString(), Db.Cnn_DB);
                Db.Execute("DELETE FROM ResellPG WHERE IDForm = " + tradein_resell_id.ToString(), Db.Cnn_DB);
                Db.Execute("DELETE FROM ResellGJ WHERE IDForm = " + tradein_resell_id.ToString(), Db.Cnn_DB);
                Db.Execute("DELETE FROM ResellLD WHERE IDForm = " + tradein_resell_id.ToString(), Db.Cnn_DB);
            }

            if (salesorder_id > 0)
            {
                Db.Execute("DELETE FROM TradeIn WHERE IDSalesOrder = " + salesorder_id.ToString(), Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrderPackaging WHERE IDForm = " + salesorder_id.ToString(), Db.Cnn_DB);

                string NamaKodeSBU = "";
                string TransactionType = "so";
                string TransactionNumber = d.Nomor;

                if (d.TipeLokasi == 0)
                {
                    NamaKodeSBU = db.LocWarehouse.SingleOrDefault(p => p.ID == d.IDLokasi).CompanyBrand.NamaKode;
                }
                else if (d.TipeLokasi == 1)
                {
                    NamaKodeSBU = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi).CompanyBrand.NamaKode;
                }
                else if (d.TipeLokasi == 2)
                {
                    NamaKodeSBU = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi).CompanyBrand.NamaKode;
                }

                Db.Execute("EXEC sp_repost_all_void_transaction 0 "
                    + ", '" + NamaKodeSBU + "'"
                    + ", '" + TransactionType + "'"
                    + ", '" + TransactionNumber + "'"
                    , Db.Cnn_CS);
            }
        }
        catch (Exception) { }

    }


    protected void voidgiftcard_Click(object sender, EventArgs e)
    {
        string result = "";

        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            var x = db.SalesReceiptDetail.Where(p => p.SalesReceipt.SalesOrder.ID == d.ID
                && p.IDPaymentType == 9
                && p.SalesReceipt.SalesOrder.StatusVoid == true
                && p.GC_VoidVerification == null
                && p.GC_VoidVerificationTgl == null
                && p.Nominal > 0
                );

            if (x.Count() > 0)
            {
                foreach (var r in x)
                {
                    result += "PAY # " + r.ID.ToString() + " - " + Payment.returnGiftCard(d, r);
                }

            }

            if (!Payment.showVoidPaymentStatusGiftCard(d))
            {
                Response.Redirect("SalesOrderReport_FormVoid.aspx?id=" + myID);
            }
            else
            {
                lit_warning_payment_gc_response.Text = "<br /><br />" + result;
            }
        }

    }

}