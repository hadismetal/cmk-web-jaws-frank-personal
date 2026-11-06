using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Library.SAP_ZFIFM_POST_FI_NEW;
using Library.SAP_ZMMFM_POBB_INTRFC_NEW;
using Library.SAP_ZMMFM_VOID_BUYBACK_INTRFC;
using Library.SAP_ZSDFM_CUSTOMER_INTRFC;
using Library.SAP_ZSDFM_SD_INTRFC;
using Library.SAP_ZSDFM_SD_VOID_INTRFC;

public partial class SAPM
{

    protected static bool checkSuccess = false;

    #region SalesOrder
    
    public static string postCustomerToSAP(SalesOrder d)
    {
        string msg = "";

        ZsdfmCustomerIntrfcResponse _output = new ZsdfmCustomerIntrfcResponse();

        _output = SAPM.PostDataCustomerToWs(d);

        string _outputXML = SAP.SerializeObject(_output);

        // insert database integration
        //SAP.InsertTableJawsIntegration(_outputXML, "", "");

        //lit_test_text.Text += _output;

        foreach (var item in _output.Customer)
        {
            if (!(item.Status.Contains("SUCCESS") || item.Status.Contains("EXIST")))
            {
                msg += "SAP Post Customer : " + item.Status
                    + ((item.Message != "") ? " (" + item.Message + "). " : "")
                    ;
            }
        }

        if (!checkSuccess) { msg = ""; }
        return msg;
    }

    public static string postSalesGoodsToSAP(SalesOrder d, bool isTradeIn)
    {
        string msg = "";

        ZsdfmSdIntrfcResponse _output = new ZsdfmSdIntrfcResponse();

        _output = SAPM.PostDataSalesToWs(d, isTradeIn);

        string _outputXML = SAP.SerializeObject(_output);

        bool status = true;

        // insert database integration
        string _spName = "SP_SAP_SD_SD_INSERT";
        string _spKey = d.Nomor;
        SAP.InsertTableJawsIntegration(_outputXML, _spName, _spKey);

        foreach (var item in _output.SalesOrder)
        {
            if (status
                && !item.Status.Contains("BILL_SUCCESS")
                )
            {
                status = false;
                msg += "SAP Post Sales : " + item.Status
                    + ((item.Message != "") ? " (" + item.Message + "). " : "")
                    ;
            }
        }

        if (!checkSuccess) { msg = ""; }
        return msg;
    }

    public static string isValidLocationSAP(SalesOrder d)
    {
        using (Map_DB db = new Map_DB())
        {

            string msg = "";

            string xKodeLokasi = "";
            string xIDBrand = "";
            string xTipeLokasi = "";
            string xIDLokasi = "";

            if (d.TipeLokasi == 1)
            {
                var loc = db.LocOutlet.Single(p => p.ID == d.IDLokasi);

                xKodeLokasi = loc.Kode.ToString();
                xIDBrand = loc.IDBrand.ToString();
                xTipeLokasi = d.TipeLokasi.ToString();
                xIDLokasi = d.IDLokasi.ToString();
            }
            else if (d.TipeLokasi == 2)
            {
                var loc = db.LocExhibition.Single(p => p.ID == d.IDLokasi);

                xKodeLokasi = loc.Kode.ToString();
                xIDBrand = loc.IDBrand.ToString();
                xTipeLokasi = d.TipeLokasi.ToString();
                xIDLokasi = d.IDLokasi.ToString();
            }

            string spname = "[SP_SAP_LOCATION_VALIDATION]";
            string query = " exec " + spname
                         + Str.isString(xKodeLokasi, false)
                         + Str.isString(xIDBrand)
                         + Str.isString(xTipeLokasi)
                         + Str.isString(xIDLokasi)
                         ;

            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    if (Convert.ToInt32(dtRow["isValid"].ToString()) == 0)
                    {
                        msg += "Invalid SAP Location Setting !!";
                    }
                }

            }

            if (!checkSuccess) { msg = ""; }
            return msg;
            }
    }

    public static string prePostSalesOrderIntegration(SalesOrder d)
    {
        using (Map_DB db = new Map_DB())
        {

            string msg = "";

            string xSO_BRAND = "";
            string xSO_NUMBER = d.Nomor;
            string xSO_LOCATION_CODE = "";
            string xSO_DESCRIPTION = d.Keterangan;
            string xSO_DATETIME = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string xSO_SALES_USER_NUMBER = d.DataSales.NIK != null ? d.DataSales.NIK : "";
            string xSO_TOTAL_PRICE = "0";
            string xSO_TOTAL_DISCOUNT_1 = "0";
            string xSO_TOTAL_DISCOUNT_2 = "0";
            string xSO_TOTAL_DISCOUNT_3 = "0";
            string xSO_TOTAL_DISCOUNT_4 = "0";
            string xSO_TOTAL_DISCOUNT_5 = "0";
            string xSO_TOTAL_DISCOUNT_6 = "0";
            string xSO_TOTAL_DISCOUNT_7 = "0";
            string xSO_TOTAL_DISCOUNT_8 = "0";
            string xSO_TOTAL_DISCOUNT_9 = "0";
            string xSO_TOTAL_DISCOUNT_10 = "0";
            string xSO_TOTAL_FINALPRICE = "0";
            string xSO_TOTAL_PAYMENT = "0";
            string xSO_TOTAL_DPP = "0";
            string xSO_TOTAL_PPN = "0";
            string xSO_TOTAL_POINT = "0";
            string xSO_CUSTOMER_CODE = d.DataCustomer.NoCustomer != null ? d.DataCustomer.NoCustomer : "";
            string xSO_CUSTOMER_NAME = d.DataCustomer.Nama != null ? d.DataCustomer.Nama : "";
            string xSO_CUSTOMER_PHONE = d.DataCustomer.AddrNoTelp != null ? d.DataCustomer.AddrNoTelp : "";
            string xSO_CUSTOMER_EMAIL = d.DataCustomer.AddrEmail != null ? d.DataCustomer.AddrEmail : "";
            bool xSO_CUSTOMER_COURTESY = false;

            if (d.TipeLokasi == 1)
            {
                var loc = db.LocOutlet.Single(p => p.ID == d.IDLokasi);

                xSO_LOCATION_CODE = loc.Kode.ToString();
                xSO_BRAND = loc.CompanyBrand.NamaKode.ToString();
            }
            else if (d.TipeLokasi == 2)
            {
                var loc = db.LocExhibition.Single(p => p.ID == d.IDLokasi);

                xSO_LOCATION_CODE = loc.Kode.ToString();
                xSO_BRAND = loc.CompanyBrand.NamaKode.ToString();
            }

            string spname = "[SP_PREPOST_SALESORDER_INTEGRATION]";
            string query = " exec " + spname
                         + Str.isString(xSO_BRAND, false)
                         + Str.isString(xSO_NUMBER)
                         + Str.isString(xSO_LOCATION_CODE)
                         + Str.isString(xSO_DESCRIPTION)
                         + Str.isString(xSO_DATETIME)
                         + Str.isString(xSO_SALES_USER_NUMBER)
                         + Str.isString(xSO_TOTAL_PRICE)
                         + Str.isString(xSO_TOTAL_DISCOUNT_1)
                         + Str.isString(xSO_TOTAL_DISCOUNT_2)
                         + Str.isString(xSO_TOTAL_DISCOUNT_3)
                         + Str.isString(xSO_TOTAL_DISCOUNT_4)
                         + Str.isString(xSO_TOTAL_DISCOUNT_5)
                         + Str.isString(xSO_TOTAL_DISCOUNT_6)
                         + Str.isString(xSO_TOTAL_DISCOUNT_7)
                         + Str.isString(xSO_TOTAL_DISCOUNT_8)
                         + Str.isString(xSO_TOTAL_DISCOUNT_9)
                         + Str.isString(xSO_TOTAL_DISCOUNT_10)
                         + Str.isString(xSO_TOTAL_FINALPRICE)
                         + Str.isString(xSO_TOTAL_PAYMENT)
                         + Str.isString(xSO_TOTAL_DPP)
                         + Str.isString(xSO_TOTAL_PPN)
                         + Str.isString(xSO_TOTAL_POINT)
                         + Str.isString(xSO_CUSTOMER_CODE)
                         + Str.isString(xSO_CUSTOMER_NAME)
                         + Str.isString(xSO_CUSTOMER_PHONE)
                         + Str.isString(xSO_CUSTOMER_EMAIL)
                         + Str.isBoolean(xSO_CUSTOMER_COURTESY)
                         ;

            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    if (Convert.ToInt32(dtRow["isValid"].ToString()) == 0)
                    {
                        msg += "Invalid Prepost SalesOrder Integration !!";
                    }
                }

            }

            if (!checkSuccess) { msg = ""; }
            return msg;
            }
    }

    #endregion

    #region Resell

    public static string postCustomerToSAP(Resell d)
    {
        string msg = "";

        ZsdfmCustomerIntrfcResponse _output = new ZsdfmCustomerIntrfcResponse();

        _output = SAPM.PostDataCustomerToWs(d);

        string _outputXML = SAP.SerializeObject(_output);

        // insert database integration
        //SAP.InsertTableJawsIntegration(_outputXML, "", "");

        //lit_test_text.Text += _output;

        foreach (var item in _output.Customer)
        {
            if (!(item.Status.Contains("SUCCESS") || item.Status.Contains("EXIST")))
            {
                msg += "SAP Post Customer : " + item.Status
                    + ((item.Message != "") ? " (" + item.Message + "). " : "")
                    ;
            }
        }

        if (!checkSuccess) { msg = ""; }
        return msg;
    }

    public static string postResellGoodsToSAP(Resell d)
    {
        string msg = "";
        
        ZmmfmPobbIntrfcNewResponse _output = new ZmmfmPobbIntrfcNewResponse();

        _output = SAPM.PostDataResellToWs(d);

        string _outputXML = SAP.SerializeObject(_output);

        bool status = true;

        // insert database integration
        string _spName = "SP_SAP_MM_POBB_INSERT";
        string _spKey = d.Nomor;
        SAP.InsertTableJawsIntegration(_outputXML, _spName, _spKey);

        foreach (var item in _output.PurchaseOrder)
        {
            if (status
                && !item.Status.Contains("IV_SUCCESS")
                )
            {
                status = false;
                msg += "SAP Post Buyback : " + item.Status
                    + ((item.Message != "") ? " (" + item.Message + "). " : "")
                    ;
            }
        }

        if (!checkSuccess) { msg = ""; }
        return msg;
    }

    public static string isValidLocationSAP(Resell d)
    {
        using (Map_DB db = new Map_DB())
        {

            string msg = "";

            string xKodeLokasi = "";
            string xIDBrand = "";
            string xTipeLokasi = "";
            string xIDLokasi = "";

            if (d.TipeLokasi == 1)
            {
                var loc = db.LocOutlet.Single(p => p.ID == d.IDLokasi);

                xKodeLokasi = loc.Kode.ToString();
                xIDBrand = loc.IDBrand.ToString();
                xTipeLokasi = d.TipeLokasi.ToString();
                xIDLokasi = d.IDLokasi.ToString();
            }
            else if (d.TipeLokasi == 2)
            {
                var loc = db.LocExhibition.Single(p => p.ID == d.IDLokasi);

                xKodeLokasi = loc.Kode.ToString();
                xIDBrand = loc.IDBrand.ToString();
                xTipeLokasi = d.TipeLokasi.ToString();
                xIDLokasi = d.IDLokasi.ToString();
            }

            string spname = "[SP_SAP_LOCATION_VALIDATION]";
            string query = " exec " + spname
                         + Str.isString(xKodeLokasi, false)
                         + Str.isString(xIDBrand)
                         + Str.isString(xTipeLokasi)
                         + Str.isString(xIDLokasi)
                         ;

            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    if (Convert.ToInt32(dtRow["isValid"].ToString()) == 0)
                    {
                        msg += "Invalid SAP Location Setting !!";
                    }
                }

            }

            if (!checkSuccess) { msg = ""; }
            return msg;
            }
    }

    public static string prePostResellIntegration(Resell d)
    {
        using (Map_DB db = new Map_DB())
        {

            string msg = "";

            string xSO_BRAND = "";
            string xSO_NUMBER = d.Nomor;
            string xSO_LOCATION_CODE = "";
            string xSO_DESCRIPTION = d.Keterangan;
            string xSO_DATETIME = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string xSO_SALES_USER_NUMBER = d.DataSales.NIK != null ? d.DataSales.NIK : "";
            string xSO_TOTAL_PRICE = "0";
            string xSO_TOTAL_DISCOUNT_1 = "0";
            string xSO_TOTAL_DISCOUNT_2 = "0";
            string xSO_TOTAL_DISCOUNT_3 = "0";
            string xSO_TOTAL_DISCOUNT_4 = "0";
            string xSO_TOTAL_DISCOUNT_5 = "0";
            string xSO_TOTAL_DISCOUNT_6 = "0";
            string xSO_TOTAL_DISCOUNT_7 = "0";
            string xSO_TOTAL_DISCOUNT_8 = "0";
            string xSO_TOTAL_DISCOUNT_9 = "0";
            string xSO_TOTAL_DISCOUNT_10 = "0";
            string xSO_TOTAL_FINALPRICE = "0";
            string xSO_TOTAL_PAYMENT = "0";
            string xSO_TOTAL_DPP = "0";
            string xSO_TOTAL_PPN = "0";
            string xSO_TOTAL_POINT = "0";
            string xSO_CUSTOMER_CODE = d.DataCustomer.NoCustomer != null ? d.DataCustomer.NoCustomer : "";
            string xSO_CUSTOMER_NAME = d.DataCustomer.Nama != null ? d.DataCustomer.Nama : "";
            string xSO_CUSTOMER_PHONE = d.DataCustomer.AddrNoTelp != null ? d.DataCustomer.AddrNoTelp : "";
            string xSO_CUSTOMER_EMAIL = d.DataCustomer.AddrEmail != null ? d.DataCustomer.AddrEmail : "";
            bool xSO_CUSTOMER_COURTESY = false;

            if (d.TipeLokasi == 1)
            {
                var loc = db.LocOutlet.Single(p => p.ID == d.IDLokasi);

                xSO_LOCATION_CODE = loc.Kode.ToString();
                xSO_BRAND = loc.CompanyBrand.NamaKode.ToString();
            }
            else if (d.TipeLokasi == 2)
            {
                var loc = db.LocExhibition.Single(p => p.ID == d.IDLokasi);

                xSO_LOCATION_CODE = loc.Kode.ToString();
                xSO_BRAND = loc.CompanyBrand.NamaKode.ToString();
            }

            string spname = "[SP_PREPOST_BUYBACK_INTEGRATION]";
            string query = " exec " + spname
                         + Str.isString(xSO_BRAND, false)
                         + Str.isString(xSO_NUMBER)
                         + Str.isString(xSO_LOCATION_CODE)
                         + Str.isString(xSO_DESCRIPTION)
                         + Str.isString(xSO_DATETIME)
                         + Str.isString(xSO_SALES_USER_NUMBER)
                         + Str.isString(xSO_TOTAL_PRICE)
                         + Str.isString(xSO_TOTAL_DISCOUNT_1)
                         + Str.isString(xSO_TOTAL_DISCOUNT_2)
                         + Str.isString(xSO_TOTAL_DISCOUNT_3)
                         + Str.isString(xSO_TOTAL_DISCOUNT_4)
                         + Str.isString(xSO_TOTAL_DISCOUNT_5)
                         + Str.isString(xSO_TOTAL_DISCOUNT_6)
                         + Str.isString(xSO_TOTAL_DISCOUNT_7)
                         + Str.isString(xSO_TOTAL_DISCOUNT_8)
                         + Str.isString(xSO_TOTAL_DISCOUNT_9)
                         + Str.isString(xSO_TOTAL_DISCOUNT_10)
                         + Str.isString(xSO_TOTAL_FINALPRICE)
                         + Str.isString(xSO_TOTAL_PAYMENT)
                         + Str.isString(xSO_TOTAL_DPP)
                         + Str.isString(xSO_TOTAL_PPN)
                         + Str.isString(xSO_TOTAL_POINT)
                         + Str.isString(xSO_CUSTOMER_CODE)
                         + Str.isString(xSO_CUSTOMER_NAME)
                         + Str.isString(xSO_CUSTOMER_PHONE)
                         + Str.isString(xSO_CUSTOMER_EMAIL)
                         + Str.isBoolean(xSO_CUSTOMER_COURTESY)
                         ;

            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    if (Convert.ToInt32(dtRow["isValid"].ToString()) == 0)
                    {
                        msg += "Invalid Prepost SalesOrder Integration !!";
                    }
                }

            }

            if (!checkSuccess) { msg = ""; }
            return msg;
            }
    }

    #endregion

    #region Void 
    
    public static string postVoidBuybackToSAP(Resell d)
    {
        string msg = "";

        string xPoNoSap = "";

        string spname = "[SP_BUYBACK_GET_DATA]";
        string query = " exec " + spname
                     + Str.isString(d.Nomor, false)
                     ;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, Db.Cnn_IN);

        if (dt.Rows.Count == 1)
        {
            foreach (DataRow dtRow in dt.Rows)
            {
                xPoNoSap = dtRow["PoNoSap"].ToString();

                if (xPoNoSap == "")
                {
                    msg += "Invalid SAP PO Number !! (1)";
                }
            }

        }

        if (msg == "" && xPoNoSap != "")
        {
            ZmmfmVoidBuybackIntrfcResponse _output = new ZmmfmVoidBuybackIntrfcResponse();

            _output = SAPM.PostDataVoidBuybackToWs(d, xPoNoSap);

            string _outputXML = SAP.SerializeObject(_output);

            // insert database integration
            //string _spName = "SP_SAP_SD_SDVOID_INSERT";
            //string _spKey = d.Nomor;
            //SAP.InsertTableJawsIntegration(_outputXML, _spName, _spKey);

            //lit_test_text.Text += _output;

            foreach (var item in _output.ExTReturn)
            {
                if (!(new[] { "S", "I", "W", "E" }.Any(c => c.Contains(item.MessageIndicator))))
                {
                    // S, I, W : success
                    // E : Error with already message
                    //msg += item.Message + ". ";

                    if (item.MessageIndicator == "E" && item.Message.Contains("already"))
                    {
                        // valid
                    }
                    else
                    {
                        msg += "SAP Post Void Buyback : " + item.MessageIndicator
                            + ((item.Message != "") ? " (" + item.Message + "). " : "")
                            ;
                    }
                }

            }
        }
        else
        {
            msg += "Invalid SAP PO Number !! (2)";
        }

        if (!checkSuccess) { msg = ""; }
        return msg;
    }

    public static string postVoidSalesOrderToSAP(SalesOrder d)
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

        if (msg == "" && xBillNoSap != "")
        {
            ZsdfmSdVoidIntrfcResponse _output = new ZsdfmSdVoidIntrfcResponse();

            _output = SAPM.PostDataVoidSalesOrderToWs(d, xBillNoSap);

            string _outputXML = SAP.SerializeObject(_output);

            // insert database integration
            //string _spName = "SP_SAP_SD_SD_VOID_INSERT";
            //string _spKey = d.Nomor;
            //SAP.InsertTableJawsIntegration(_outputXML, _spName, _spKey);

            //lit_test_text.Text += _output;

            foreach (var item in _output.Invoice)
            {
                if (!item.Message.Contains("already cancelled"))
                {
                    if (!(item.Status.Contains("SO_SUCCESS")))
                    {
                        msg += item.Message + ". ";
                    }
                }

            }
        }
        else
        {
            msg += "Invalid SAP Bill Number !!";
        }

        if (!checkSuccess) { msg = ""; }
        return msg;
    }

    #endregion

    #region Payment
    
    public static string isPaymentLockedBySAP(SalesOrder d)
    {
        string msg = "";

        string spname = "[SP_SAP_PAYMENT_VALIDATION]";
        string query = " exec " + spname
                     + Str.isString(d.Nomor, false)
                     ;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, Db.Cnn_IN);

        if (dt.Rows.Count == 1)
        {
            foreach (DataRow dtRow in dt.Rows)
            {
                if (Convert.ToInt32(dtRow["isValid"].ToString()) == 0)
                {
                    msg += "Sales Order locked for SAP Posting (Payment) !!";
                }
            }

        }

        if (!checkSuccess) { msg = ""; }
        return msg;
    }

    public static string releaseSAPLockedByFailureSAPPosting(SalesOrder d)
    {
        string msg = "";

        string spname = "[SP_SAP_PAYMENT_RELEASE]";
        string query = " exec " + spname
                     + Str.isString(d.Nomor, false)
                     ;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, Db.Cnn_IN);

        if (dt.Rows.Count == 1)
        {
            foreach (DataRow dtRow in dt.Rows)
            {
                if (Convert.ToInt32(dtRow["isValid"].ToString()) == 0)
                {
                    msg += "Sales Order failed to release for SAP Posting (Payment) !!";
                }
            }

        }

        if (!checkSuccess) { msg = ""; }
        return msg;
    }

    public static string postPaymentSalesOrderToSAP(SalesOrder d, ref bool isDoneSAP, Literal litSAPError, Literal litSAPTimeStamp)
    {
        using (Map_DB db = new Map_DB())
        {

            string msg = "";

            string _spTimeStampKey = d.ID.ToString() + "." + System.DateTime.Now.ToString("yyyy.MM.dd.hh.mm.ss.ffffff");

            string _Brand = "";

            if (d.TipeLokasi == 1)
            {
                _Brand = db.LocOutlet.Single(p => p.ID == d.IDLokasi).Kode;
            }
            if (d.TipeLokasi == 2)
            {
                _Brand = db.LocExhibition.Single(p => p.ID == d.IDLokasi).Kode;
            }

            // INSERT OR UPDATE INITIAL DATA TO POST SAP
            string spname = "[SP_SALESORDER_PAYMENT_CHECK]";
            string query = " exec " + spname
                         + Str.isString(_Brand, false)
                         + Str.isString(d.Nomor)
                         + Str.isString(_spTimeStampKey)
                         ;

            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    if (Convert.ToInt32(dtRow["isValid"].ToString()) == 0)
                    {
                        if (Convert.ToInt32(dtRow["SAP_RESPONSE_STATUS"].ToString()) == 2)
                        {
                            isDoneSAP = true;
                            msg = ""; // done posting
                        }
                        else
                        {
                            msg += "Invalid SAP Pre Post Setting. Sales Order is locked !!";
                        }

                    }
                }

            }

            if (msg == String.Empty && !isDoneSAP)
            {
                ZfifmPostFiNewResponse _output = new ZfifmPostFiNewResponse();

                _output = SAPM.PostDataPaymentSalesOrderToWs(d, litSAPError);

                string _outputXML = SAP.SerializeObject(_output);

                bool status = true;

                foreach (var item in _output.Data)
                {
                    if (status && !item.Status.Contains("FI_SUCCESS"))
                    {
                        status = false;
                        msg += "SAP Post Payment : " + item.Status
                        + ((item.Message != "") ? " (" + item.Message + "). " : "")
                        ;
                    }
                }

                if (status)
                {
                    // insert database integration

                    litSAPTimeStamp.Text = _spTimeStampKey;

                    string _spName = "SP_SAP_FI_FI_INSERT";
                    string _spKey = d.Nomor;
                    SAP.InsertTableJawsIntegration(_outputXML, _spName, _spKey, _spTimeStampKey);
                }
            }

            if (!checkSuccess) { msg = ""; }
            return msg;
            }
    }
    
    #endregion

}

