using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

public partial class STAMPSM
{
    static Stamps_API api = new Stamps_API(STAMPS.xAPI);


    public static string GetMembership(Literal litTest, string xStampsID, string module, int id, string cabang)
    {
        string errMsg = "";

        var result = api.GetMembership(xStampsID, STAMPS.xMerchant);
        var obj = JsonConvert.DeserializeObject<STAMPS_Response.ResponseMembership>(result.data.ToString());

        // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.
        //richTextBox1.Text = JsonConvert.SerializeObject(result);
        
        //litTest.Text += JsonConvert.SerializeObject(result) + "<br /><br />";


        //litTest.Text = JsonConvert.SerializeObject(result);

        //litTest.Text += "name : " + ((obj.user.name == null) ? "" : obj.user.name.ToString()) + "<br />";
        //litTest.Text += "phone : " + ((obj.user.phone == null) ? "" : obj.user.phone.ToString()) + "<br />";
        //litTest.Text += "email : " + ((obj.user.email == null) ? "" : obj.user.email.ToString()) + "<br />";
        //litTest.Text += "id : " + ((obj.user.id == null) ? "" : obj.user.id.ToString()) + "<br />";
        //litTest.Text += "address : " + ((obj.user.address == null) ? "" : obj.user.address.ToString()) + "<br />";
        //litTest.Text += "birthday : " + ((obj.user.birthday == null) ? "" : obj.user.birthday.ToString()) + "<br />";
        //litTest.Text += "primary_member_id : " + ((obj.user.primary_member_id == null) ? "" : obj.user.primary_member_id.ToString()) + "<br />";

        string xnama = "";
        string xalamat = "";
        string xtelp = "";
        string xemail = "";
        DateTime xtgllahir = System.DateTime.Now;
        string xcabang = "";
        string xkodecust = "";

        xnama = (obj.user.name == null) ? "" : obj.user.name.ToString();
        xalamat = (obj.user.address == null) ? "" : obj.user.address.ToString();
        xtelp = (obj.user.phone == null) ? "" : obj.user.phone.ToString();
        xemail = (obj.user.email == null) ? "" : obj.user.email.ToString();
        xtgllahir = (obj.user.birthday == null) ? System.DateTime.Now : Convert.ToDateTime(obj.user.birthday.ToString());
        xcabang = cabang;
        xkodecust = (obj.user.primary_member_id == null) ? "" : obj.user.primary_member_id.ToString();

        errMsg = Customer.Customer_Create_Dbcmk_Stamps(module, id, xnama, xalamat, xtelp, xemail, xtgllahir, xcabang, xkodecust);
        errMsg = postStampsMemberIntegration(obj.user);

        return errMsg;
    }

    protected static string separatorDoubleDash(string x)
    {
        return " -- " + x;
    }

    public static string AddTransaction(Literal litTest, SalesOrder d, bool isRequireEmail = false)
    {
        using (Map_DB db = new Map_DB())
        {

            string errMsg = "";

            /* 
             * Kami sarankan untuk menyimpan response body dari transaksi untuk memudahkan debugging apabila ada transaksi yang error
             * 
             * Notes 
             * 
             * 1. Untuk method `AddTransaction` akan mengembalikan `transaction_id`.
             *    Mohon untuk menyimpan `transaction_id` ini, karena akan digunakan untuk melakukan `CancelTransaction`
             *    
             * 2. Pengecekan pada server untuk `InvoiceNumber` bersifat `unique`. 
             */

            // Item
            List<Item> items = new List<Item>();

            //// Extra Data Item
            //List<ExtraDataItem> extra_data_items = new List<ExtraDataItem>();

            //// Extra Data Payments
            //List<ExtraDataPayment> extra_data_payments = new List<ExtraDataPayment>();

            var dj = d.SalesOrderDJ;
            var pg = d.SalesOrderPG;
            var gj = d.SalesOrderGJ;
            var ld = d.SalesOrderLD;

            int grandtotal_poinmembership = 0;

            // branded
            var s_branded = db.Stone1B.Where(p => p.Parcel106.NamaKode.Contains("") );

            if (dj.Count > 0)
            {
                foreach(var data in dj)
                {
                    var p = data.StockProductDJ;
                    var s = p.StockProductDJ_Stone1B;

                    //foreach(var stone in s)
                    //{
                    //    var b = db.Parcel106.Where(p => stone.IDStone);
                    //}

    
                    //string kdbrg = "DJ";
                    //string category = p.StockProductDJ_CharProduct.CharProductCategory.NamaKode;
                    //category = (category == "SI" || category == "SNI") ? category : "";

    
                    //string spname2 = "[SP_STAMPS_GET_POIN]";
                    //string query2 = " exec " + spname2
                    //             + Str.isString(xSO_LOCATION_CODE, false)
                    //             ;

                    //DataTable dt2 = new DataTable();
                    //dt2 = Db.Rs(query2, Db.Cnn_IN);

                    //if (dt2.Rows.Count == 1)
                    //{
                    //    foreach (DataRow dtRow in dt2.Rows)
                    //    {
                    //        grandtotal_poinmembership += Convert.ToInt32(dtRow["result"].ToString());

                    //    }

                    //}

    
                    Item item = new Item();
                    ItemExtraData itemextradata = new ItemExtraData();
                    itemextradata.type = "Diamond Jewellery";
                    itemextradata.item = data.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                    itemextradata.category = data.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.Nama;
                    itemextradata.level = data.StockProductDJ.StockProductDJ_CharProduct.CharProductLevel.Nama;
                    itemextradata.stonedistribution = data.StockProductDJ.StockProductDJ_CharProduct.CharStoneDist.Nama;

                    item.product_name = itemextradata.type
                                      + separatorDoubleDash(itemextradata.item)
                                      + separatorDoubleDash(itemextradata.category)
                                      + separatorDoubleDash(itemextradata.level)
                                      + separatorDoubleDash(itemextradata.stonedistribution)
                                      ;
                    item.quantity = 1;
                    item.stamps_subtotal = 0; // multiple conversion poin
                    item.subtotal = Convert.ToInt64(data.TotalBayar.Value); // harga nett
                    item.extra_data = itemextradata;
                    items.Add(item);

                    //string stoneBrand = "";
                    //var branded = data.StockProductDJ.StockProductDJ_Stone1B;
                    //if(branded != null)
                    //{
                    //    if (branded.Any(p => p.Stone1B.Parcel106.IsBranded == true))
                    //    {
                    //        stoneBrand = "Branded";
                    //    }
                    //    else if (branded.Any(p => p.Stone1B.Parcel106.IsBranded == false))
                    //    {
                    //        stoneBrand = "Generic";
                    //    }
                    //}
                
                    //ExtraDataItem extradataitem = new ExtraDataItem();
                    //extradataitem.product_name = data.StockProductDJ.Nomor;
                    //extradataitem.product_stone_branded = stoneBrand;
                    //extra_data_items.Add(extradataitem);
                }
            }

            if (pg.Count > 0)
            {
                foreach (var data in pg)
                {
                    Item item = new Item();
                    ItemExtraData itemextradata = new ItemExtraData();
                    itemextradata.type = "Plain Gold";
                    itemextradata.item = data.StockProductPG.CharProductItem.Nama;
                    itemextradata.category = "Plain Gold";
                    itemextradata.level = data.StockProductPG.CharGoldLevel.Nama;
                    itemextradata.stonedistribution = "Plain Gold";

                    item.product_name = itemextradata.type
                                      + separatorDoubleDash(itemextradata.item)
                                      + separatorDoubleDash(itemextradata.category)
                                      + separatorDoubleDash(itemextradata.level)
                                      + separatorDoubleDash(itemextradata.stonedistribution)
                                      ;
                    item.quantity = 1;
                    item.stamps_subtotal = 0; // multiple conversion poin
                    item.subtotal = Convert.ToInt64(data.TotalBayar.Value); // harga nett
                    item.extra_data = itemextradata;
                    items.Add(item);
                
                    //ExtraDataItem extradataitem = new ExtraDataItem();
                    //extradataitem.product_name = data.StockProductPG.Nomor;
                    //extradataitem.product_stone_branded = "";
                    //extra_data_items.Add(extradataitem);
                }
            }

            if (gj.Count > 0)
            {
                foreach (var data in gj)
                {
                    Item item = new Item();
                    ItemExtraData itemextradata = new ItemExtraData();
                    itemextradata.type = "Gold Jewellery";
                    itemextradata.item = data.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama;
                    itemextradata.category = data.StockProductGJ.StockProductGJ_CharProduct.CharProductCategory.Nama;
                    itemextradata.level = data.StockProductGJ.StockProductGJ_CharProduct.CharProductLevel.Nama;
                    itemextradata.stonedistribution = data.StockProductGJ.StockProductGJ_CharProduct.CharStoneDist.Nama;

                    item.product_name = itemextradata.type
                                      + separatorDoubleDash(itemextradata.item)
                                      + separatorDoubleDash(itemextradata.category)
                                      + separatorDoubleDash(itemextradata.level)
                                      + separatorDoubleDash(itemextradata.stonedistribution)
                                      ;
                    item.quantity = 1;
                    item.stamps_subtotal = 0; // multiple conversion poin
                    item.subtotal = Convert.ToInt64(data.TotalBayar.Value); // harga nett
                    item.extra_data = itemextradata;
                    items.Add(item);
                
                    //ExtraDataItem extradataitem = new ExtraDataItem();
                    //extradataitem.product_name = data.StockProductGJ.Nomor;
                    //extradataitem.product_stone_branded = "";
                    //extra_data_items.Add(extradataitem);
                }
            }

            if (ld.Count > 0)
            {
                foreach (var data in ld)
                {
                    Item item = new Item();
                    ItemExtraData itemextradata = new ItemExtraData();
                    itemextradata.type = "Loose Diamond";
                    itemextradata.item = data.StockProductLD_Stone1B.CharProductItem.Nama;
                    itemextradata.category = "Loose Diamond";
                    itemextradata.level = "Loose Diamond";
                    itemextradata.stonedistribution = "Loose Diamond";

                    item.product_name = itemextradata.type
                                      + separatorDoubleDash(itemextradata.item)
                                      + separatorDoubleDash(itemextradata.category)
                                      + separatorDoubleDash(itemextradata.level)
                                      + separatorDoubleDash(itemextradata.stonedistribution)
                                      ;
                    item.quantity = 1;
                    item.stamps_subtotal = 0; // multiple conversion poin
                    item.subtotal = Convert.ToInt64(data.TotalBayar.Value); // harga nett
                    item.extra_data = itemextradata;
                    items.Add(item);

                    //ExtraDataItem extradataitem = new ExtraDataItem();
                    //extradataitem.product_name = data.StockProductLD_Stone1B.Nomor;
                    //extradataitem.product_stone_branded = "";
                    //extra_data_items.Add(extradataitem);
                }
            }
        
            //// Extra Data
            List<ExtraData> extra_datas = new List<ExtraData>();

            //ExtraData extra_data = new ExtraData();
            //extra_data.extra_data_item = extra_data_items.ToArray();
            //extra_data.extra_data_payment = extra_data_payments.ToArray();

            //extra_data.grand_total = Convert.ToDouble(d.TotalBayar.Value);
            //extra_data.sales_name = d.DataSales.Nama;
            //extra_data.sales_nik = d.DataSales.NIK;
            //extra_data.total_point = 0; // multiconversion poin

            //// Add data into each lists
            //extra_datas.Add(extra_data);

            string xSO_LOCATION_CODE = "";

            if (d.TipeLokasi == 1)
            {
                var loc = db.LocOutlet.Single(p => p.ID == d.IDLokasi);

                xSO_LOCATION_CODE = loc.Kode.ToString();
            }
            else if (d.TipeLokasi == 2)
            {
                var loc = db.LocExhibition.Single(p => p.ID == d.IDLokasi);

                xSO_LOCATION_CODE = loc.Kode.ToString();
            }

    
            string st_email = d.DataCustomer.AddrEmail;
            string st_phone = d.DataCustomer.AddrNoTelp;

            string st_user_stamps = (st_email != "" && st_email != null) ? st_email : st_phone;

            short st_store = 0; // masih hardcode

            string spname = "[SP_STAMPS_GET_MAPPING_LOCATION]";
            string query = " exec " + spname
                         + Str.isString(xSO_LOCATION_CODE, false)
                         ;

            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    st_store = Convert.ToInt16(dtRow["STAMPS_LOC_ID"].ToString());
                
                }

            }
        
            int st_stamps = grandtotal_poinmembership; // stamps : multiconversion poin here

            string st_invoice = d.Nomor;
            int st_totalval = Convert.ToInt32(d.TotalBayar); // Grand Total Multiconversion Poin
            int st_subtotal = Convert.ToInt32(d.TotalBayar); // Grand Total Harga Nett
            string st_employeenik = d.DataSales.Keterangan;
            int st_numberpeople = 1;
            int st_tax = 0;
            string st_datetime = System.DateTime.Now.ToString("s");
            string st_requireemailnotif = isRequireEmail.ToString();

            var result = api.AddTransaction(st_user_stamps, st_store, st_stamps, st_invoice, st_totalval, st_subtotal, st_employeenik
                , st_numberpeople, st_tax, st_datetime, st_requireemailnotif, items, extra_datas);
            var obj = JsonConvert.DeserializeObject<STAMPS_Response.ResponseAddTransaction>(result.data.ToString());
            var objExistingID = JsonConvert.DeserializeObject<STAMPS_Response.ResponseExistingTransactionID>(result.extra_data.ToString());

            // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.
            //richTextBox1.Text = JsonConvert.SerializeObject(result);

            litTest.Text += JsonConvert.SerializeObject(result) + "<br /><br />";
            //litTest.Text += st_datetime + "<br /><br />";

            try
            {
                if(result.status_code == 200 || (result.status_code == 400 && result.error_message.Contains("already exists")))
                {
                    // add database integration
                    postAddTransaction(d, obj.transaction.id);

                    // success
                    //litTest.Text += "tid : " + ((obj.transaction.id == 0) ? "" : obj.transaction.id.ToString()) + "<br />";
                    //litTest.Text += "tdisc : " + ((obj.transaction.discount == 0) ? "" : obj.transaction.discount.ToString()) + "<br />";
                    //litTest.Text += "tnop : " + ((obj.transaction.number_of_people == 0) ? "" : obj.transaction.number_of_people.ToString()) + "<br />";
                    //litTest.Text += "tse : " + ((obj.transaction.stamps_earned == 0) ? "" : obj.transaction.stamps_earned.ToString()) + "<br />";
                    //litTest.Text += "tval : " + ((obj.transaction.value == 0) ? "" : obj.transaction.value.ToString()) + "<br />";
                    //litTest.Text += "cid : " + ((obj.customer.id == 0) ? "" : obj.customer.id.ToString()) + "<br />";
                    //litTest.Text += "cbal : " + ((obj.customer.balance == 0) ? "" : obj.customer.balance.ToString()) + "<br />";
                    //litTest.Text += "cmob : " + ((obj.customer.mobile_phone == "") ? "" : obj.customer.mobile_phone.ToString()) + "<br />";
                    //litTest.Text += "cstr : " + ((obj.customer.stamps_remaining == 0) ? "" : obj.customer.stamps_remaining.ToString()) + "<br />";
                    //litTest.Text += "csts : " + ((obj.customer.status == "") ? "" : obj.customer.status.ToString()) + "<br />";
                }
                else
                {
                    errMsg += "STAMPS Add Transaction : " + result.error_message + " (" + result.status_message + " " + result.status_code + ")";

                }

            }
            catch (Exception) { }

    
            return errMsg;
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
        
            return msg;
            }
    }

    //postAddRedemption
    public static string postAddRedemption(SalesOrder d, string VoucherNumber, int RedeemID, string KodeBrand, string plu)
    {
        string msg = "";

        int xRD_ID = RedeemID;
        string xSO_BRAND = KodeBrand;
        int xSO_ID = d.ID;
        string xSO_NUMBER = d.Nomor;
        string xPLU_NUMBER = plu;
        string xVC_NUMBER = VoucherNumber;
        string xTRANSACTION_MODE = "R";


        string spname = "[SP_STAMPS_REDEEM_VOUCHER_CODE]";
        string query = " exec " + spname
                     + Str.isNumber(xRD_ID, false)
                     + Str.isString(xSO_BRAND)
                     + Str.isNumber(xSO_ID)
                     + Str.isString(xSO_NUMBER)
                     + Str.isString(xVC_NUMBER)
                     + Str.isString(xPLU_NUMBER)
                     + Str.isString(xTRANSACTION_MODE)
                     ;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, Db.Cnn_IN);

        if (dt.Rows.Count == 1)
        {
            foreach (DataRow dtRow in dt.Rows)
            {
                if (Convert.ToInt32(dtRow["isValid"].ToString()) == 0)
                {
                    msg += "Invalid STAMPS Redeemption ID Integration - Transaction ID !!";
                }
            }

        }

        return msg;
    }

    public static string postAddTransaction(SalesOrder d, int StampsTransactionID)
    {
        using (Map_DB db = new Map_DB())
        {

            string msg = "";

            string xSO_BRAND = "";
            string xSO_NUMBER = d.Nomor;
            string xSO_LOCATION_CODE = "";

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

            string spname = "[SP_STAMPS_ADD_TRANSACTION]";
            string query = " exec " + spname
                         + Str.isString(xSO_BRAND, false)
                         + Str.isString(xSO_NUMBER)
                         + Str.isString(xSO_LOCATION_CODE)
                         + Str.isNumber(StampsTransactionID)
                         ;

            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    if (Convert.ToInt32(dtRow["isValid"].ToString()) == 0)
                    {
                        msg += "Invalid STAMPS Add Transaction Integration - Transaction ID !!";
                    }
                }

            }
        
            return msg;
            }
    }

    public static string postStampsMemberIntegration(STAMPS_Response.ResponseMembership.User user)
    {
        string msg = "";
        
        string spname = "[SP_STAMPS_GET_MEMBERSHIP_DETAIL]";
        string query = " exec " + spname
                     + Str.isNumber(Convert.ToInt32(user.id), false)
                     + Str.isString(user.primary_member_id != null ? user.primary_member_id : "")
                     + Str.isString(user.member_ids != null ? JsonConvert.SerializeObject(user.member_ids).ToString() : "")
                     + Str.isString(user.phone != null ? user.phone : "")
                     + Str.isString(user.email != null ? user.email : "")
                     + Str.isString(user.name != null ? user.name : "")
                     + Str.isString(user.birthday != null ? user.birthday : "")
                     + Str.isString(user.address != null ? user.address : "")
                     + Str.isString(App.UserNama)
                     ;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, Db.Cnn_IN);

        if (dt.Rows.Count == 1)
        {
            foreach (DataRow dtRow in dt.Rows)
            {
                if (Convert.ToInt32(dtRow["isValid"].ToString()) == 0)
                {
                    msg += "Invalid STAMPS Add Transaction Integration - User ID !!";
                }
            }

        }

        return msg;
    }

    public static string CancelTransaction(Literal litTest, SalesOrder d)
    {
        using (Map_DB db = new Map_DB())
        {

            string msg = "";

            string xSO_BRAND = "";
            string xSO_NUMBER = d.Nomor;
            string xSO_LOCATION_CODE = "";

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

            string spname = "[SP_STAMPS_VOID_TRANSACTION]";
            string query = " exec " + spname
                         + Str.isString(xSO_BRAND, false)
                         + Str.isString(xSO_NUMBER)
                         + Str.isString(xSO_LOCATION_CODE)
                         ;

            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    if (Convert.ToInt32(dtRow["isValid"].ToString()) == 0)
                    {
                        msg += "Invalid STAMPS Add Transaction Integration - Transaction ID !!";
                    }
                    else
                    {
                        int StampsTransactionID = Convert.ToInt32(dtRow["STAMPS_RESPONSE_TRANSACTION_ID"].ToString());

                        var result = api.CancelTransaction(StampsTransactionID);
                        var obj = JsonConvert.DeserializeObject<STAMPS_Response.ResponseCancelTransaction>(result.data.ToString());

                        // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.
                        //richTextBox1.Text = JsonConvert.SerializeObject(result);

    
    
                    }
                }

            }

            return msg;
            }
    }

    public static void ModifyTransaction(Literal litTest)
    {
        /*
         *  Notes
         * 
         *  1. `product_name` harus bernilai sama untuk transaksi telah dilakukan pada method `AddTransaction` untuk `transaction_id` tersebut.
         *     Bila tidak sama, akan menghasil `Product does not exists`
         *  
         *  2. Jika `quantity` yang dikirim pada List Item dibawah, bernilai lebih besar daripada quantity untuk `transaction_id` tersebut
         *     akan menghasilkan `Deducted quantity can't be greather than original quantity`
         *   
         */

        // Item
        List<Item> items = new List<Item>();

        Item item1 = new Item();
        item1.product_name = "AQUA";
        item1.quantity = -1;

        Item item2 = new Item();
        item2.product_name = "NESCAFE";
        item2.quantity = -2;

        items.Add(item1);
        items.Add(item2);

        var result = api.ModifyTransaction(22, -4000, -5000, items);
        var obj = JsonConvert.DeserializeObject<STAMPS_Response.ResponseModifyTransaction>(result.data.ToString());

        // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.
        //richTextBox1.Text = JsonConvert.SerializeObject(result);
    }

    public static void MemberSuggestions(Literal litTest, Table tbl, string Module, int DocID, string keyword)
    {
        /*
         *  Notes :
         *  
         *  1. Member Suggestions sama dengan Search Member.
         *  
         *  2. Jika merchant yang digunakan salah atau tidak ada, akan menghasilkan `Can't find merchant ID = 3`
         *  
         *  3. `Query` merupakan informasi yang akan dilakukan pencarian pada `Member Suggestions`
         *     Pencarian berdasarkan `Email`, `Nama`, `Phone`, dan `Code`.
         *     Jika pencarian tidak ditemukan, akan menghasilkan list suggestions kosong, 
         *     seperti disamping ini : {"suggestions":[]}
         *     
         */
        
        if(keyword != null)
        {
            var result = api.MemberSuggestions(keyword, STAMPS.xMerchant);
            var obj = JsonConvert.DeserializeObject<STAMPS_Response.ResponseMemberSuggestions>(result.data.ToString());

            // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.

            int index = 1;

            //litTest.Text += JsonConvert.SerializeObject(result) + "<br /><br />";

            if (obj.suggestions.Count() == 0) { App.TableNoResult(tbl); }
            else
            {

                foreach (var dc in obj.suggestions)
                {
                    string xMemberCode = "";
                    int xMemberId = 0;
                    string xMemberName = "";
                    string xMemberPhone = "";
                    string xMemberTiering = "";
                    int xStampsTotalPoint = 0;
                    bool xActiveMember = false;
                    string xMemberBirthday = "";
                    string xMemberEmail = "";

                    if (dc.member_ids != null)
                    {

                        int idxCustomerCode = 1;
                        foreach (var dc_code in dc.member_ids)
                        {
                            if(dc_code != null)
                            {
                                xMemberCode += (idxCustomerCode > 1 ? "<br />" : "") + dc_code.ToString();
                                idxCustomerCode++;
                            }

                        }
                    }

                    xMemberId = (dc.id != 0) ? dc.id : 0;
                    xMemberName = (dc.name != null) ? dc.name.ToString().Trim().ToUpper() : "";
                    xMemberPhone = (dc.phone != null) ? dc.phone.ToString().Trim().ToUpper() : "";
                    xMemberTiering = (dc.membership != null) ? dc.membership.ToString().Trim().ToUpper() : "";
                    xStampsTotalPoint = (dc.stamps.HasValue) ? dc.stamps.Value : 0;
                    xActiveMember = (dc.is_active);
                    xMemberBirthday = (dc.birthday != null) ? Convert.ToDateTime(dc.birthday).ToString("dd-MM-yyyy") : "";
                    xMemberEmail = (dc.email != null) ? dc.email.ToString().Trim().ToUpper() : "";

                    // test literal result
                    //litTest.Text += index.ToString() + ". " + xMemberId.ToString() + " - " + xMemberName.ToString() + " : " + xMemberCode + "<br />";

                    if (xMemberId > 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = MemberSuggestions_TD_HyperLink(Module, DocID, xMemberId, xMemberCode, keyword, xActiveMember);
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = index.ToString() + ".";
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = xMemberId.ToString();
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = xMemberName
                                + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
                                + HidingEmailStringMiddle(xMemberEmail)
                                ;
                        td.Style.Add("text-align", "left");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = xMemberBirthday;
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = (xMemberCode != "") ? xMemberCode : "<span class='text-red'>???</span>";
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = HidingPhoneStringMiddle(xMemberPhone);
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = xMemberTiering;
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Str.Number0(xStampsTotalPoint);
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = (xActiveMember) ? "Yes" : "<span class='text-red'>No</span>";
                        td.Style.Add("text-align", "center");
                        td.Style.Add("vertical-align", "top");
                        tr.Cells.Add(td);
                        
                        tbl.Rows.Add(tr);
                        index++;
                    }
                }
            }

        }
        

    }

    public static string prePostVoucherIntegration(string VoucherCode, int VoucherNominal
        , string VoucherDateStart, string VoucherDateExpired, string VoucherType
        , string VoucherName, int VoucherID, string VoucherAllowedType
        )
    {
        string msg = "";
        
        string spname = "[SP_PREPOST_VOUCHER_INTEGRATION]";
        string query = " exec " + spname
                     + Str.isString(VoucherCode, false)
                     + Str.isNumber(VoucherNominal)
                     + Str.isString(VoucherDateStart)
                     + Str.isString(VoucherDateExpired)
                     + Str.isString(VoucherType)
                     + Str.isString(VoucherName)
                     + Str.isNumber(VoucherID)
                     + Str.isString(VoucherAllowedType)
                     ;

        DataTable dt = new DataTable();
        dt = Db.Rs(query, Db.Cnn_IN);

        if (dt.Rows.Count == 1)
        {
            foreach (DataRow dtRow in dt.Rows)
            {
                if (Convert.ToInt32(dtRow["isValid"].ToString()) == 0)
                {
                    msg += "Invalid Prepost Voucher Integration !!";
                }
            }

        }

        return msg;
    }

    public static string ValidateVoucherCode(Literal litTest, string VoucherCode, SalesOrder d
        , ref int voucher_id
        , ref string voucher_name
        , ref string voucher_validity
        , ref string voucher_type
        , ref string voucher_nominal
        , ref string voucher_date_start
        , ref string voucher_date_end
        , ref string voucher_valid_for
        , ref bool voucher_is_redeemable
        , string VoucherAllowedType
        )
    {
        using (Map_DB db = new Map_DB())
        {

    
            string user_name = String.Empty;
            string user_email = String.Empty;
            string user_phone = String.Empty;

            voucher_validity = "";
            voucher_type = "";
            voucher_nominal = "";
            voucher_date_start = "";
            voucher_date_end = "";
            voucher_valid_for = "";
            voucher_is_redeemable = false;

            /*
             * 
             *  "voucher_validity":"static",
                "voucher_type":"discount",
                "voucher_nominal":1000000,
                "voucher_date_start":"2019-01-01",
                "voucher_date_end":"2019-12-31",
                "voucher_valid_for":0 
             * 
             */

            string errMsg = "";

            //VoucherCode = "VTIS-LYZAALZJ";

            string xSO_LOCATION_CODE = "";

            if (d.TipeLokasi == 1)
            {
                var loc = db.LocOutlet.Single(p => p.ID == d.IDLokasi);

                xSO_LOCATION_CODE = loc.Kode.ToString();
            }
            else if (d.TipeLokasi == 2)
            {
                var loc = db.LocExhibition.Single(p => p.ID == d.IDLokasi);

                xSO_LOCATION_CODE = loc.Kode.ToString();
            }
        
            short st_store = 0; // masih hardcode

            string spname = "[SP_STAMPS_GET_MAPPING_LOCATION]";
            string query = " exec " + spname
                         + Str.isString(xSO_LOCATION_CODE, false)
                         ;

            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    st_store = Convert.ToInt16(dtRow["STAMPS_LOC_ID"].ToString());

                }

            }
        
            var result = api.ValidateVoucherCode(VoucherCode, st_store);
            var obj = JsonConvert.DeserializeObject<STAMPS_Response.ResponseValidateVoucherCode>(result.data.ToString());

            // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.
            //richTextBox1.Text = JsonConvert.SerializeObject(result);
        
            string xstatus_code = result.status_code.ToString();

            try
            {
                if (result.status_code == 200)
                {
                    voucher_is_redeemable = Convert.ToBoolean(obj.is_redeemable);

                    if(voucher_is_redeemable || (result.status_code == 200 && !voucher_is_redeemable))
                    {
                        // view voucher data
                        voucher_id = obj.voucher.id;
                        voucher_name = (obj.voucher.name == null) ? "" : obj.voucher.name.ToString();
                    
                        voucher_validity = (obj.voucher.extra_data.voucher_validity == null) ? "" : obj.voucher.extra_data.voucher_validity.ToString();
                        voucher_type = (obj.voucher.extra_data.voucher_type == null) ? "" : obj.voucher.extra_data.voucher_type.ToString();
                        voucher_nominal = Str.Number0(Convert.ToInt32(obj.voucher.extra_data.voucher_nominal));
                        voucher_date_start = Str.FullDate(Convert.ToDateTime(obj.voucher.extra_data.voucher_date_start));
                        voucher_date_end = Str.FullDate(Convert.ToDateTime(obj.voucher.extra_data.voucher_date_end));
                        voucher_valid_for = Str.Number0(Convert.ToInt32(obj.voucher.extra_data.voucher_valid_for));

                        // add database myapps
                        if(voucher_validity.ToUpper() == "STATIC")
                        {
                            string xyz = prePostVoucherIntegration(
                                VoucherCode
                                , Convert.ToInt32(obj.voucher.extra_data.voucher_nominal)
                                , Convert.ToString(obj.voucher.extra_data.voucher_date_start)
                                , Convert.ToString(obj.voucher.extra_data.voucher_date_end)
                                , Convert.ToString(obj.voucher.extra_data.voucher_type)
                                , Convert.ToString(voucher_name)
                                , Convert.ToInt32(voucher_id)
                                , Convert.ToString(VoucherAllowedType)
                                );
                        }
                    }
                    else
                    {
                        //already used
                        if (obj.user != null)
                        {
                            //already used by another user
                            user_name = obj.user.name;
                            user_email = obj.user.email;
                            user_phone = obj.user.phone;

                            errMsg += "STAMPS Validate Voucher : Voucher already used by <b>" + user_name + "</b>.";

                        }
                        else
                        {
                            errMsg += "STAMPS Validate Voucher : Voucher already used.";
                        }

                    }

                }
                else
                {
                    errMsg += "STAMPS Validate Voucher : " + result.error_message + " (" + result.status_message + " " + result.status_code + ")";

                }

            }
            catch (Exception ex) { errMsg = "Terjadi kesalahan teknis. Silahkan hubungi IT Support. (STAMPS Error message : "
                        + ex.Message
                        + ex.StackTrace
                        + ex.InnerException
                        + ")";
            }
        
    
            return errMsg;
            }
    }

    public static string RedeemVoucherCode(SalesOrder d, string VoucherCode, string KodeBrand, string plu)
    {
        using (Map_DB db = new Map_DB())
        {

            string errMsg = "";

            ///////////////////

            string xSO_LOCATION_CODE = "";

            if (d.TipeLokasi == 1)
            {
                var loc = db.LocOutlet.Single(p => p.ID == d.IDLokasi);

                xSO_LOCATION_CODE = loc.Kode.ToString();
            }
            else if (d.TipeLokasi == 2)
            {
                var loc = db.LocExhibition.Single(p => p.ID == d.IDLokasi);

                xSO_LOCATION_CODE = loc.Kode.ToString();
            }

            short st_store = 0; // masih hardcode

            string spname = "[SP_STAMPS_GET_MAPPING_LOCATION]";
            string query = " exec " + spname
                         + Str.isString(xSO_LOCATION_CODE, false)
                         ;

            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dtRow in dt.Rows)
                {
                    st_store = Convert.ToInt16(dtRow["STAMPS_LOC_ID"].ToString());

                }

            }

    
            /*
             *  Notes :
             *  
             *  1. `Voucher Code` yang bisa digunakan sebagai sample 
             *  
             *     VTIS-4ZZFR444, VTIS-TXH1L2R1, VTIS-2PJACTA2, VTIS-8EH9YK37, VTIS-K9Y73XCR
             *     VTIS-E148FXLR, VTIS-KCWLL9ZP, VTIS-LRVTXFVP, VTIS-CJJ28CL8, VTIS-9W4A7JYQ
             *     VTIS-EH4Q29CC, VTIS-VAXYEAL2, VTIS-2F2KJJH4, VTIS-XWFL2F8F, VTIS-THRKHPY7
             *     VTIS-H2QLCUYR, VTIS-R4JVU4FJ, VTIS-PUKXTYRA, VTIS-AJYT3WFU, VTIS-QT3C9J8P
             * 
             *  2. Bila `Voucher Code` sudah terpakai, akan mengembalikan `Voucher quantity is 0!`
             *  
             *  3. Bila `Voucher Code` tidak terdaftar, akan mengembalikan `Invalid voucher code`
             *  
             *  4. Bila `Store` tidak terdaftar, akan mengembalikan `{number} is not a valid store ID`
             *     contoh : `3 is not a valid store ID`
             *     
             *  5. Bila `Identifier` tidak terdaftar, akan mengembalikan `User is not found`
             *  
             */

            string st_email = d.DataCustomer.AddrEmail;
            string st_phone = d.DataCustomer.AddrNoTelp;

            string st_user_stamps = (st_email != "" && st_email != null) ? st_email : st_phone;
        
            var result = api.RedeemVoucherCode(st_user_stamps, VoucherCode, st_store);
            var obj = JsonConvert.DeserializeObject<STAMPS_Response.ResponseRedeemVoucherCode>(result.data.ToString());

            // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.
            //richTextBox1.Text = JsonConvert.SerializeObject(result);
        
            try
            {
                if (result.status_code == 200)
                {
                    // add database integration
                    postAddRedemption(d,
                        VoucherCode,
                        Convert.ToInt32(obj.redemption.id)
                        , KodeBrand
                        , plu
                        );
                
                }
                else
                {
                    errMsg += "STAMPS Redeem Voucher Code : " + result.error_message + " (" + result.status_message + " " + result.status_code + ")";

                }

            }
            catch (Exception) { }

    
            return errMsg;

                }
    }

    public static bool VoidAllVoucherByPLU(SalesOrder d, int myItemType, int myItemID)
    {
        using (Map_DB db = new Map_DB())
        {

            bool result = false;
        
            string xSO_BRAND = "";
            string xSO_NUMBER = d.Nomor;
            string xSO_LOCATION_CODE = "";
            int xSO_ID = d.ID;
            int xMode = 0;

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

            string plu = "";
            if (myItemType == 1)
            {
                var m = db.SalesOrderDJ.SingleOrDefault(p => p.ID == myItemID);
                plu = m.StockProductDJ.Nomor;
            }
            else if (myItemType == 2)
            {
                var m = db.SalesOrderPG.SingleOrDefault(p => p.ID == myItemID);
                plu = m.StockProductPG.Nomor;
            }
            else if (myItemType == 3)
            {
                var m = db.SalesOrderGJ.SingleOrDefault(p => p.ID == myItemID);
                plu = m.StockProductGJ.Nomor;
            }
            else if (myItemType == 4)
            {
                var m = db.SalesOrderLD.SingleOrDefault(p => p.ID == myItemID);
                plu = m.StockProductLD_Stone1B.Nomor;
            }
        
            string spname = "[SP_STAMPS_VOID_REDEMPTION]";
            string query = " exec " + spname
                         + Str.isString(xSO_BRAND, false)
                         + Str.isNumber(xSO_ID)
                         + Str.isString(plu)
                         + Str.isNumber(xMode)
                         ;
        
            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if (dt.Rows.Count > 0)
            {
                int inc = 0;
                bool x = false;

                // semua void redemption harus return true
            
                foreach (DataRow dtRow in dt.Rows)
                {
                    if ((inc == 0 && !x) || (inc > 0 && x))
                    {
                        x = CancelRedemptions(Convert.ToInt32(dtRow["RD_ID"].ToString()));
                    
                        inc++;
                    }
                }

                // jika semua void redemption stamps return true maka lakukan pengembalian voucher internal
                if(x)
                {
                    xMode = 1;

                    string spname2 = "[SP_STAMPS_VOID_REDEMPTION]";
                    string query2 = " exec " + spname2
                                    + Str.isString(xSO_BRAND, false)
                                    + Str.isNumber(xSO_ID)
                                    + Str.isString(plu)
                                    + Str.isNumber(xMode)
                                    ;
                
                    Db.Execute(query2, Db.Cnn_IN);

                    result = true;
                }

            }
            else
            {
                result = true;
            }
        
            return result;
            }
    }

    public static bool CancelRedemptions(int StampsRedemptionID)
    {
        bool x = false;

        /*
         *  Notes :
         *
         *  1. `id` untuk cancel redemption merupakan `id` yang diperoleh dari hasil response
         *     saat melakukan `Redeem Voucher Code`
         *     
         *  2. Bila `Voucher Code` telah di-redeem, akan mengembalikan `Redemption is already canceled`
         *  
         *  3. Bila `Voucher Code` belum melakukan redeem atau tidak terdaftar, 
         *     akan mengembalikan `Redemption not found`
         *     
         */

        var result = api.CancelRedemptions(StampsRedemptionID);
        var obj = JsonConvert.DeserializeObject<STAMPS_Response.ResponseCancelRedemptions>(result.data.ToString());

        // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.
        //richTextBox1.Text = JsonConvert.SerializeObject(result);


        try
        {
            if (result.status_code == 200 || (result.status_code == 400 && result.error_message.Contains("already canceled")))
            {
                x = true;
            }

        }
        catch (Exception) { x = false; }

        return x;
    }

    public static void GetMembershipByCode(Literal litTest, string KodeCustomer)
    {
        /*
         *  Notes :
         *  
         *  1. Pencarian berdasarkan `Customer Code` dari sisi CMK
         *  
         */

        var result = api.GetMembershipByCode(KodeCustomer, STAMPS.xMerchant);
        var obj = JsonConvert.DeserializeObject<STAMPS_Response.ResponseGetMembershipByCode>(result.data.ToString());

        // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.
        //richTextBox1.Text = JsonConvert.SerializeObject(result);


        litTest.Text += JsonConvert.SerializeObject(result) + "<br /><br />";


        //litTest.Text = JsonConvert.SerializeObject(result);

        litTest.Text += "name : " + ((obj.user.name == null) ? "" : obj.user.name.ToString()) + "<br />";
        litTest.Text += "phone : " + ((obj.user.phone == null) ? "" : obj.user.phone.ToString()) + "<br />";
        litTest.Text += "email : " + ((obj.user.email == null) ? "" : obj.user.email.ToString()) + "<br />";
        litTest.Text += "id : " + ((obj.user.id == null) ? "" : obj.user.id.ToString()) + "<br />";
        litTest.Text += "address : " + ((obj.user.address == null) ? "" : obj.user.address.ToString()) + "<br />";
        litTest.Text += "birthday : " + ((obj.user.birthday == null) ? "" : obj.user.birthday.ToString()) + "<br />";

        //if (obj.user == null) {  }
        //else
        //{
        //    litTest.Text += "name : " + obj.user.name.ToString() + "<br />";
        //    litTest.Text += "phone : " + obj.user.phone.ToString() + "<br />";
        //    litTest.Text += "email : " + obj.user.email.ToString() + "<br />";
        //    litTest.Text += "id : " + obj.user.id.ToString() + "<br />";
        //    litTest.Text += "address : " + obj.user.address.ToString() + "<br />";
        //    litTest.Text += "birthday : " + obj.user.birthday.ToString() + "<br />";
        //}
    }

    #region TD_Hyperlink
    protected static string MemberSuggestions_TD_HyperLink(string xModule, int xDocID, int xStampsID, string xMemberCode, string xKeyword, bool xActiveMember)
    {
        string x = string.Empty;

        if(xMemberCode != "" && xActiveMember)
        {
            x += "<a class=\"btn btn-info btn-xs\" href=\"DataCustomerStamps.aspx";
            x += "?refreshPage=1&mod=" + xModule + "&id=" + xDocID + "&stid=" + xStampsID + "&key=" + xKeyword;
            x += "\">";
            x += "  <i class=\"icon-ok\"></i> ";
            x += "  ";
            x += "</a>";

        }
        else
        {
            x += "  <i class=\"icon-exclamation-sign text-red text-md\"></i> ";
        }

        return x;
    }
    #endregion

    protected static string HidingPhoneStringMiddle(string x)
    {
        if(x.Length > 10)
        {
            var aStringBuilder = new StringBuilder(x);
            int startidx = 6;
            int totalhidden = 4;
            char charhidden = '?';
            string charresult = charhidden.ToString();
            string charhiddenresult = new String(charhidden, totalhidden);

            if (totalhidden > 0 && charhidden.ToString().Trim() != "")
            {
                if (totalhidden > 1)
                {
                    charresult = "";

                    for (int i = 1; i <= totalhidden; i++)
                    {
                        charresult += charhidden;
                    }
                }

                aStringBuilder.Remove(startidx, totalhidden);
                aStringBuilder.Insert(startidx, charresult);
                x = aStringBuilder.ToString();

                int addidx = 3;
                string addstr = " ";
                x = x.Substring(0, addidx) + addstr + x.Substring(addidx);
                addidx = 7;
                x = x.Substring(0, addidx) + addstr + x.Substring(addidx);
                addidx = 12;
                x = x.Substring(0, addidx) + addstr + x.Substring(addidx);

                x = x.Replace(charhiddenresult, ("<span style='color:#DDD;'>" + charhiddenresult + "</span>"));

            }

        }

        return x;
    }

    protected static string HidingEmailStringMiddle(string email)
    {
        string result = "";

        if (email.Contains("@") && email.Contains("."))
        {
            MailAddress address = new MailAddress(email);
            string host = address.Host;
            string user = address.User;
            string addr = address.Address;
            string name = address.DisplayName;

            string x = user;

            if (x.Length > 3)
            {
                var aStringBuilder = new StringBuilder(x);
                int startidx = 3;
                int totalhidden = x.Length - startidx;
                char charhidden = '?';
                string charresult = charhidden.ToString();
                string charhiddenresult = new String(charhidden, totalhidden);

                if (totalhidden > 0 && charhidden.ToString().Trim() != "")
                {
                    if (totalhidden > 1)
                    {
                        charresult = "";

                        for (int i = 1; i <= totalhidden; i++)
                        {
                            charresult += charhidden;
                        }
                    }

                    aStringBuilder.Remove(startidx, totalhidden);
                    aStringBuilder.Insert(startidx, charresult);
                    x = aStringBuilder.ToString();

                    //int addidx = 3;
                    //string addstr = " ";
                    //x = x.Substring(0, addidx) + addstr + x.Substring(addidx);
                    //addidx = 7;
                    //x = x.Substring(0, addidx) + addstr + x.Substring(addidx);
                    //addidx = 12;
                    //x = x.Substring(0, addidx) + addstr + x.Substring(addidx);

                    x = x.Replace(charhiddenresult, ("<span style='color:#DDD;'>" + charhiddenresult + "</span>"));

                }

            }

            result = x + "@" + host;
        }
        
        return result;
    }
}

