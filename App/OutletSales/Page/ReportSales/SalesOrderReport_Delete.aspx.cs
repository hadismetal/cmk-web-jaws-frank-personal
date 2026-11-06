using System;
using System.Data;
using System.Linq;
using Library.SAP_ZSDFM_SD_VOID_INTRFC;

public partial class SalesOrderReport_Delete : System.Web.UI.Page
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
    App price = new App();

    decimal Setting = 0m;

	protected int myID { get { return App.QueryInt(this); } }

	//Exception
	protected void DeleteError(string sebab) {
		Response.Redirect("SalesOrderReport.aspx");
	}

	protected void Page_Load(object sender, EventArgs e)
    {
        string status_SAP = "";

        App.Protect();
		App.ProtectRole(Role.Manager);

		var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.StatusPembayaran == false);
		if (d == null) App.DataNull(this);
		else {
			//Delete
			deleteProcess(d);

			//Redirect
            Response.Redirect("SalesOrderReport.aspx?d=1");
		}
	}

    protected string postVoidSalesOrderToSAP(SalesOrder d)
    {
        string msg = "";

        string xBillNoSap = "";

        string spname = "[SP_SALESORDER_GET_DATA]";
        string query = " exec " + spname
                     + Str.isString(d.Nomor, false)
                     ;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, Db.Cnn_IN);

        if (dt.Rows.Count == 1)
        {
            foreach (DataRow dtRow in dt.Rows)
            {
                xBillNoSap = dtRow["BillNoSap"].ToString();

                if (xBillNoSap == "")
                {
                    msg += "Invalid SAP Bill Number !!";
                }
            }

        }

        if (msg == "")
        {
            ZsdfmSdVoidIntrfcResponse _output = new ZsdfmSdVoidIntrfcResponse();

            _output = SAPM.PostDataVoidSalesOrderToWs(d, xBillNoSap);

            string _outputXML = SAP.SerializeObject(_output);

            // insert database integration
            //string _spName = "SP_SAP_SD_SDVOID_INSERT";
            //string _spKey = d.Nomor;
            //SAP.InsertTableJawsIntegration(_outputXML, _spName, _spKey);

            //lit_test_text.Text += _output;

            foreach (var item in _output.Invoice)
            {
                if (!(item.Status.Contains("SO_SUCCESS") || item.Status.Contains("FAILED")))
                {
                    msg += item.Message + ". ";
                }
            }
        }

        return msg;
    }

    //Delete
    protected void deleteProcess(SalesOrder d) 
	{
		string status_SAP = "";
        string status_STAMPS = "";

        var dj = d.SalesOrderDJ;
        var pg = d.SalesOrderPG;
        var gj = d.SalesOrderGJ;
        var ld = d.SalesOrderLD;
        var pc = d.SalesOrderPackaging;
        var sv = d.SalesOrderSouvenir;

        foreach (var r in dj)
        {
            var x = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                x.StatusPenjualan = 0;
                db.SubmitChanges();
            }
        }

        foreach (var r in pg)
        {
            var x = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                x.StatusPenjualan = 0;
                db.SubmitChanges();
            }
        }

        foreach (var r in gj)
        {
            var x = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                x.StatusPenjualan = 0;
                db.SubmitChanges();
            }
        }

        foreach (var r in ld)
        {
            var x = db.StockProductLD.SingleOrDefault(p => p.ID == r.IDProduct);
            if (x != null)
            {
                x.StatusPenjualan = 0;
                db.SubmitChanges();
            }
        }

        foreach (var r in pc)
        {
            var x = db.StockActualPackaging.SingleOrDefault(p => p.ID == r.IDProduct && p.TipeLokasi == d.TipeLokasi && p.IDLokasi == d.IDLokasi);
            if (x != null)
            {
                x.InHand = x.InHand + r.Qty.Value;
                db.SubmitChanges();
            }
        }

        foreach (var r in sv)
        {
            var x = db.StockActualSouvenir.SingleOrDefault(p => p.ID == r.IDProduct && p.TipeLokasi == d.TipeLokasi && p.IDLokasi == d.IDLokasi);
            if (x != null)
            {
                x.InHand = x.InHand + r.Qty.Value;
                db.SubmitChanges();
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
                Ledger.createLedgerVoidTradeInDJ(d, r);

                var x = db.SalesOrderDJ.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                r.StockProductDJ.StatusPenjualan = 1;
                r.StockProductDJ.StockActualDJ.InHand = 0;
                r.StockProductDJ.StockActualDJ.Substorage = 1;
                db.SubmitChanges();
            }

            foreach (var r in resell.ResellPG)
            {
                Ledger.createLedgerVoidTradeInPG(d, r);

                var x = db.SalesOrderPG.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                r.StockProductPG.StatusPenjualan = 1;
                r.StockProductPG.StockActualPG.InHand = 0;
                r.StockProductPG.StockActualPG.Substorage = 1;
                db.SubmitChanges();
            }

            foreach (var r in resell.ResellGJ)
            {
                Ledger.createLedgerVoidTradeInGJ(d, r);

                var x = db.SalesOrderGJ.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                r.StockProductGJ.StatusPenjualan = 1;
                r.StockProductGJ.StockActualGJ.InHand = 0;
                r.StockProductGJ.StockActualGJ.Substorage = 1;
                db.SubmitChanges();
            }

            foreach (var r in resell.ResellLD)
            {
                Ledger.createLedgerVoidTradeInLD(d, r);

                var x = db.SalesOrderLD.SingleOrDefault(p => p.IDForm == r.IDSalesOrder && p.IDProduct == r.IDProduct);
                x.StatusResell = 0;
                r.StockProductLD_Stone1B.StatusPenjualan = 1;
                r.StockProductLD_Stone1B.StockActualLD_Stone1B.InHand = 0;
                r.StockProductLD_Stone1B.StockActualLD_Stone1B.Substorage = 1;
                db.SubmitChanges();
            }

            tradein_resell_id = tradein.IDResell.Value;

        }
        catch (Exception ex) { }

        try
        {
            salesorder_id = d.ID;

            if (tradein_resell_id > 0)
            {
                Db.Execute("DELETE FROM ResellDJ WHERE IDForm = " + tradein_resell_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM ResellPG WHERE IDForm = " + tradein_resell_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM ResellGJ WHERE IDForm = " + tradein_resell_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM ResellLD WHERE IDForm = " + tradein_resell_id, Db.Cnn_DB);
            }

            if (salesorder_id > 0)
            {
                Db.Execute("DELETE FROM TradeIn WHERE IDSalesOrder = " + salesorder_id, Db.Cnn_DB);

                Db.Execute("DELETE FROM SalesOrderDJ WHERE IDForm=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrderPG WHERE IDForm=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrderGJ WHERE IDForm=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrderLD WHERE IDForm=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrderPackaging WHERE IDForm=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrderSouvenir WHERE IDForm=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesVoucher WHERE IDSalesOrder=" + salesorder_id, Db.Cnn_DB);
                Db.Execute("DELETE FROM SalesOrder WHERE ID=" + salesorder_id, Db.Cnn_DB);
            }
        }
        catch (Exception ex) { }


		// Validation and STAMPS Posting
        try
        {
            if (STAMPS.xActivate)
            {
                // void all redeem voucher discount / payment stamps

                bool voidx = false;
                bool stampsNotActivate = false;

                if (STAMPS.xActivate)
                {
                    // STAMPS VOID VOUCHER
                    voidx = STAMPSM.VoidAllVoucherByPLU(d, 0, 0);
                }


                //status_STAMPS += ((status_STAMPS == "") ? STAMPSM.CancelTransaction(lit_error_stamps, d) : "");
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