using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using System.Xml;


public partial class STAMPSM
{
    //static Stamps_API api = new Stamps_API(STAMPS.xAPI);
	static STAMPS_API_CONNECT.Stamps_API api = new STAMPS_API_CONNECT.Stamps_API();																			   


    public static string GetMembership(Literal litTest, string xStampsID, string module, int id, string cabang)
    {
        string errMsg = "";

        var result = api.GetMembership(xStampsID, STAMPS.xMerchant);
        var obj = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseMembership>(result.data.ToString());

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
        string xstatusmembership = "";
		Int32 xstatus = 10;
        Int32 xstampspoint = 0;

        xnama = (obj.user.name == null) ? "" : obj.user.name.ToString();
        xalamat = (obj.user.address == null) ? "" : obj.user.address.ToString();
        xtelp = (obj.user.phone == null) ? "" : obj.user.phone.ToString();
        xemail = (obj.user.email == null) ? "" : obj.user.email.ToString();
        xtgllahir = (obj.user.birthday == null) ? System.DateTime.Now : Convert.ToDateTime(obj.user.birthday.ToString());
        xcabang = cabang;
        xkodecust = (obj.user.primary_member_id == null) ? "" : obj.user.primary_member_id.ToString();
        xstatusmembership = (obj.membership.status_text == null) ? "" : obj.membership.status_text.ToString();
		xstatus = (obj.membership.status == null) ? 10 : Convert.ToInt32(obj.membership.status.ToString());
        xstampspoint = (obj.membership.stamps == null) ? 0 : Convert.ToInt32(obj.membership.stamps.ToString());

        errMsg = Customer.Customer_Create_Dbcmk_Stamps(module, id, xnama, xalamat, xtelp, xemail, xtgllahir, xcabang, xkodecust, xstatusmembership, xstatus, xstampspoint, Convert.ToInt32(xStampsID));
        errMsg = errMsg + postStampsMemberIntegration(obj.user, obj.membership) + "(" + obj.user.primary_member_id.ToString() + ")";

        return errMsg ;
    }

    protected static string separatorDoubleDash(string x)
    {
        return " -- " + x;
    }

    public static string AddTransaction(Literal litTest, SalesOrder d)
    {
        using (Map_DB db = new Map_DB())
        using (Map_CS cs = new Map_CS())
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
            List<STAMPS_API_CONNECT.Item> items = new List<STAMPS_API_CONNECT.Item>();

            //// Extra Data Item
            //List<ExtraDataItem> extra_data_items = new List<ExtraDataItem>();

            //// Extra Data Payments
            //List<ExtraDataPayment> extra_data_payments = new List<ExtraDataPayment>();

            var dj = d.SalesOrderDJ;
            var pg = d.SalesOrderPG;
            var gj = d.SalesOrderGJ;
            var ld = d.SalesOrderLD;

            int grandtotal_poinmembership = 0;

            long total_payment_without_exchange = 0;

            var sd = db.SalesReceiptDetail.Where(p => p.SalesReceipt.SalesOrder == d);

            var sd_non_exchange = sd.Where(p => p.PaymentType.Nama.Trim().ToUpper() != "EXCHANGE");
		
    		decimal redemption_nominal = 0m;
            redemption_nominal = NominalVoucherRewardRedemptionPayment(d);

            if(sd_non_exchange.Count() > 0)
            {
                total_payment_without_exchange = Convert.ToInt64(sd_non_exchange.Sum(p => p.Nominal)) - Convert.ToInt64(redemption_nominal);

            }

            if (dj.Count > 0)
            {
                foreach (var data in dj)
                {
                    long TotalBayarPerItem = Convert.ToInt64(data.TotalBayar);
    				long ReducedVoucher = Convert.ToInt64(NominalVoucherRewardRedemption(data));																
                    long TotalBayarPosting = 0;
                    
                    if(TotalBayarPerItem >= total_payment_without_exchange)
                    {
                        TotalBayarPosting = total_payment_without_exchange;
                    }
                    else if (total_payment_without_exchange > TotalBayarPerItem)
                    {
                        TotalBayarPosting = TotalBayarPerItem;
                    }
                    else
                    {
                        TotalBayarPosting = 0;
                    }
    				TotalBayarPosting = TotalBayarPosting - ReducedVoucher;										   
                    
                    total_payment_without_exchange = (total_payment_without_exchange - TotalBayarPerItem);

                    int total_poinmembership = 0;
    				int divisor = 0;

                    bool isBranded = Pricing.isDiamondBranded(data.StockProductDJ.Nomor);
                    bool isGeneric = Pricing.isDiamondGeneric(data.StockProductDJ.Nomor);

                    string kdbrg = "DJ";
                    string category = data.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode;
                    category = (category == "SI" || category == "SNI") ? category : "";

                    string xGeneric = "T";
                    string xBranded = "T";

                    if (isGeneric) { xGeneric = "Y"; }
                    if (isBranded) { xGeneric = "T"; xBranded = "Y"; }
				
    				if (CheckLimitationDiscountVoucherDJ(data))
                    {
    					string spname2 = "[SP_STAMPS_GET_POIN]";
    					string query2 = " exec " + spname2
    								 + Str.isString(kdbrg, false)
    								 + Str.isString(category)
    								 + Str.isString(xBranded)
    								 + Str.isString(xGeneric)
    								 + Str.isNumber(TotalBayarPosting)
    								 + Str.isString(Convert.ToInt32(NominalPembagiPromoDJ(data)).ToString())
    								 ;

    					DataTable dt2 = new DataTable();
    					dt2 = Db.Rs(query2, Db.Cnn_IN);

    					if (dt2.Rows.Count == 1)
    					{
    						foreach (DataRow dtRow in dt2.Rows)
    						{
    							total_poinmembership = Convert.ToInt32(Convert.ToDecimal(dtRow["result"].ToString()));
    							divisor = Convert.ToInt32(Convert.ToDecimal(dtRow["divisor"].ToString()));
    						}

    					}

    					grandtotal_poinmembership += total_poinmembership;
					
    					dt2.Dispose();
    				}

                    STAMPS_API_CONNECT.Item item = new STAMPS_API_CONNECT.Item();
                    STAMPS_API_CONNECT.ItemExtraData itemextradata = new STAMPS_API_CONNECT.ItemExtraData();
                    itemextradata.type = "DIAMOND JEWELLERY";
                    itemextradata.item = data.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama.ToUpper();
                    itemextradata.category = data.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.Nama.ToUpper();
                    itemextradata.level = data.StockProductDJ.StockProductDJ_CharProduct.CharProductLevel.Nama.ToUpper();
                    itemextradata.stonedistribution = data.StockProductDJ.StockProductDJ_CharProduct.CharStoneDist.Nama.ToUpper();

                    item.product_name = data.StockProductDJ.Nomor + separatorDoubleDash(data.StockProductDJ.ProtoNomor) + separatorDoubleDash(itemextradata.type) + separatorDoubleDash(itemextradata.item) + separatorDoubleDash(itemextradata.category) + separatorDoubleDash(itemextradata.level) + separatorDoubleDash(itemextradata.stonedistribution);
                    item.quantity = 1;
                    item.stamps_subtotal = total_poinmembership; // multiple conversion poin
                    item.subtotal = TotalBayarPerItem; // harga nett
                    item.extra_data = itemextradata;
                    items.Add(item);
    				Db.Execute("UPDATE SalesOrderDJ SET PoinReceived = " + total_poinmembership + ", PoinPembagi = " + divisor + " WHERE ID = " + data.ID, Db.Cnn_DB);				
				
    				//DeactivePLUStamps(data.StockProductDJ.Nomor);
                }
            }

            if (pg.Count > 0)
            {
                foreach (var data in pg)
                {
                    long TotalBayarPerItem = Convert.ToInt64(data.TotalBayar);
    				long ReducedVoucher = Convert.ToInt64(NominalVoucherRewardRedemption(data));
                    long TotalBayarPosting = 0;

                    if (TotalBayarPerItem >= total_payment_without_exchange)
                    {
                        TotalBayarPosting = total_payment_without_exchange;
                    }
                    else if (total_payment_without_exchange > TotalBayarPerItem)
                    {
                        TotalBayarPosting = TotalBayarPerItem;
                    }
                    else
                    {
                        TotalBayarPosting = 0;
                    }
    				TotalBayarPosting = TotalBayarPosting - ReducedVoucher;

                    total_payment_without_exchange = (total_payment_without_exchange - TotalBayarPerItem);

                    int total_poinmembership = 0;
    				int divisor = 0;

                    bool isBranded = false; // Pricing.isDiamondBranded(data.StockProductDJ.Nomor);
                    bool isGeneric = false; // Pricing.isDiamondGeneric(data.StockProductDJ.Nomor);

                    string kdbrg = "PG";
                    string category = "";

                    string xGeneric = "T";
                    string xBranded = "T";

                    if (isGeneric) { xGeneric = "Y"; }
                    if (isBranded) { xGeneric = "T"; xBranded = "Y"; }
				
    				if (CheckLimitationDiscountVoucherPG(data))
                    {
    					string spname2 = "[SP_STAMPS_GET_POIN]";
    					string query2 = " exec " + spname2
    								 + Str.isString(kdbrg, false)
    								 + Str.isString(category)
    								 + Str.isString(xBranded)
    								 + Str.isString(xGeneric)
    								 + Str.isNumber(TotalBayarPosting)
    								 //+ Str.isString(Convert.ToInt32(NominalPembagiPromoPG(data)).ToString())
    								 + Str.isString("0")
    								 ;

    					DataTable dt2 = new DataTable();
    					dt2 = Db.Rs(query2, Db.Cnn_IN);

    					if (dt2.Rows.Count == 1)
    					{
    						foreach (DataRow dtRow in dt2.Rows)
    						{
    							//total_poinmembership = Convert.ToInt32(Convert.ToDecimal(dtRow["result"].ToString()));
    							total_poinmembership = Convert.ToInt32(NominalPoinPromoPG(data));
    							divisor = Convert.ToInt32(Convert.ToDecimal(dtRow["divisor"].ToString()));
    						}

    					}

    					grandtotal_poinmembership += total_poinmembership;
					
    					dt2.Dispose();
    				}

                    STAMPS_API_CONNECT.Item item = new STAMPS_API_CONNECT.Item();
                    STAMPS_API_CONNECT.ItemExtraData itemextradata = new STAMPS_API_CONNECT.ItemExtraData();
                    itemextradata.type = "PLAIN GOLD";
                    itemextradata.item = data.StockProductPG.CharProductItem.Nama.ToUpper();
                    itemextradata.category = "PLAIN GOLD";
                    itemextradata.level = data.StockProductPG.CharGoldLevel.Nama.ToUpper();
                    itemextradata.stonedistribution = "PLAIN GOLD";

                    item.product_name = data.StockProductPG.Nomor + separatorDoubleDash(data.StockProductPG.KodeBarang) + separatorDoubleDash(itemextradata.type) + separatorDoubleDash(itemextradata.item) + separatorDoubleDash(itemextradata.category) + separatorDoubleDash(itemextradata.level) + separatorDoubleDash(itemextradata.stonedistribution);
                    item.quantity = 1;
                    item.stamps_subtotal = total_poinmembership; // multiple conversion poin
                    item.subtotal = TotalBayarPerItem; // harga nett
                    item.extra_data = itemextradata;
                    items.Add(item);
    				Db.Execute("UPDATE SalesOrderPG SET PoinReceived = " + total_poinmembership + ", PoinPembagi = " + divisor + " WHERE ID = " + data.ID, Db.Cnn_DB);																								   
				
    				//DeactivePLUStamps(data.StockProductPG.Nomor);
                }
            }

            if (gj.Count > 0)
            {
                foreach (var data in gj)
                {
                    long TotalBayarPerItem = Convert.ToInt64(data.TotalBayar);
    				long ReducedVoucher = Convert.ToInt64(NominalVoucherRewardRedemption(data));
                    long TotalBayarPosting = 0;

                    if (TotalBayarPerItem >= total_payment_without_exchange)
                    {
                        TotalBayarPosting = total_payment_without_exchange;
                    }
                    else if (total_payment_without_exchange > TotalBayarPerItem)
                    {
                        TotalBayarPosting = TotalBayarPerItem;
                    }
                    else
                    {
                        TotalBayarPosting = 0;
                    }
    				TotalBayarPosting = TotalBayarPosting - ReducedVoucher;

                    total_payment_without_exchange = (total_payment_without_exchange - TotalBayarPerItem);

                    int total_poinmembership = 0;
    				int divisor = 0;

                    bool isBranded = false; // Pricing.isDiamondBranded(data.StockProductGJ.Nomor);
                    bool isGeneric = false; // Pricing.isDiamondGeneric(data.StockProductGJ.Nomor);

                    string kdbrg = "GJ";
                    string category = "";

                    string xGeneric = "T";
                    string xBranded = "T";

                    if (isGeneric) { xGeneric = "Y"; }
                    if (isBranded) { xGeneric = "T"; xBranded = "Y"; }
				
    				if (CheckLimitationDiscountVoucherGJ(data))
                    {
    					string spname2 = "[SP_STAMPS_GET_POIN]";
    					string query2 = " exec " + spname2
    								 + Str.isString(kdbrg, false)
    								 + Str.isString(category)
    								 + Str.isString(xBranded)
    								 + Str.isString(xGeneric)
    								 + Str.isNumber(TotalBayarPosting)
    								 + Str.isString("0")
    								 ;

    					DataTable dt2 = new DataTable();
    					dt2 = Db.Rs(query2, Db.Cnn_IN);

    					if (dt2.Rows.Count == 1)
    					{
    						foreach (DataRow dtRow in dt2.Rows)
    						{
    							total_poinmembership = Convert.ToInt32(Convert.ToDecimal(dtRow["result"].ToString()));
    							divisor = Convert.ToInt32(Convert.ToDecimal(dtRow["divisor"].ToString()));
    						}

    					}

    					grandtotal_poinmembership += total_poinmembership;
					
    					dt2.Dispose();
    				}

                    STAMPS_API_CONNECT.Item item = new STAMPS_API_CONNECT.Item();
                    STAMPS_API_CONNECT.ItemExtraData itemextradata = new STAMPS_API_CONNECT.ItemExtraData();
                    itemextradata.type = "GOLD JEWELLERY";
                    itemextradata.item = data.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama.ToUpper();
                    itemextradata.category = data.StockProductGJ.StockProductGJ_CharProduct.CharProductCategory.Nama.ToUpper();
                    itemextradata.level = data.StockProductGJ.StockProductGJ_CharProduct.CharProductLevel.Nama.ToUpper();
                    itemextradata.stonedistribution = data.StockProductGJ.StockProductGJ_CharProduct.CharStoneDist.Nama.ToUpper();

                    item.product_name = data.StockProductGJ.Nomor + separatorDoubleDash(data.StockProductGJ.ProtoNomor) + separatorDoubleDash(itemextradata.type) + separatorDoubleDash(itemextradata.item) + separatorDoubleDash(itemextradata.category) + separatorDoubleDash(itemextradata.level) + separatorDoubleDash(itemextradata.stonedistribution);
                    item.quantity = 1;
                    item.stamps_subtotal = total_poinmembership; // multiple conversion poin
                    item.subtotal = TotalBayarPerItem; // harga nett
                    item.extra_data = itemextradata;
                    items.Add(item);
    				Db.Execute("UPDATE SalesOrderGJ SET PoinReceived = " + total_poinmembership + ", PoinPembagi = " + divisor + " WHERE ID = " + data.ID, Db.Cnn_DB);																									   
				
    				//DeactivePLUStamps(data.StockProductGJ.Nomor);
                }
            }

            if (ld.Count > 0)
            {
                foreach (var data in ld)
                {
                    long TotalBayarPerItem = Convert.ToInt64(data.TotalBayar);
    				long ReducedVoucher = Convert.ToInt64(NominalVoucherRewardRedemption(data));
                    long TotalBayarPosting = 0;

                    if (TotalBayarPerItem >= total_payment_without_exchange)
                    {
                        TotalBayarPosting = total_payment_without_exchange;
                    }
                    else if (total_payment_without_exchange > TotalBayarPerItem)
                    {
                        TotalBayarPosting = TotalBayarPerItem;
                    }
                    else
                    {
                        TotalBayarPosting = 0;
                    }
    				TotalBayarPosting = TotalBayarPosting - ReducedVoucher;

                    total_payment_without_exchange = (total_payment_without_exchange - TotalBayarPerItem);

                    int total_poinmembership = 0;
    				int divisor = 0;

                    bool isBranded = Pricing.isDiamondBranded(data.StockProductLD_Stone1B.Nomor);
                    bool isGeneric = Pricing.isDiamondGeneric(data.StockProductLD_Stone1B.Nomor);

                    string kdbrg = "LD";
                    string category = "";

                    string xGeneric = "T";
                    string xBranded = "T";

                    if (isGeneric) { xGeneric = "Y"; }
                    if (isBranded) { xGeneric = "T"; xBranded = "Y"; }
				
    				if (CheckLimitationDiscountVoucherLD(data))
                    {
    					string spname2 = "[SP_STAMPS_GET_POIN]";
    					string query2 = " exec " + spname2
    								 + Str.isString(kdbrg, false)
    								 + Str.isString(category)
    								 + Str.isString(xBranded)
    								 + Str.isString(xGeneric)
    								 + Str.isNumber(TotalBayarPosting)
    								 + Str.isString("0")
    								 ;

    					DataTable dt2 = new DataTable();
    					dt2 = Db.Rs(query2, Db.Cnn_IN);

    					if (dt2.Rows.Count == 1)
    					{
    						foreach (DataRow dtRow in dt2.Rows)
    						{
    							total_poinmembership = Convert.ToInt32(Convert.ToDecimal(dtRow["result"].ToString()));
    							divisor = Convert.ToInt32(Convert.ToDecimal(dtRow["divisor"].ToString()));
    						}

    					}

    					grandtotal_poinmembership += total_poinmembership;
					
    					dt2.Dispose();
    				}

                    STAMPS_API_CONNECT.Item item = new STAMPS_API_CONNECT.Item();
                    STAMPS_API_CONNECT.ItemExtraData itemextradata = new STAMPS_API_CONNECT.ItemExtraData();
                    itemextradata.type = "LOOSE DIAMOND";
                    itemextradata.item = data.StockProductLD_Stone1B.CharProductItem.Nama.ToUpper();
                    itemextradata.category = "LOOSE DIAMOND";
                    itemextradata.level = "LOOSE DIAMOND";
                    itemextradata.stonedistribution = "LOOSE DIAMOND";

                    item.product_name = data.StockProductLD_Stone1B.Nomor + separatorDoubleDash("NONE") + separatorDoubleDash(itemextradata.type) + separatorDoubleDash(itemextradata.item) + separatorDoubleDash(itemextradata.category) + separatorDoubleDash(itemextradata.level) + separatorDoubleDash(itemextradata.stonedistribution);
                    item.quantity = 1;
                    item.stamps_subtotal = total_poinmembership; // multiple conversion poin
                    item.subtotal = TotalBayarPerItem; // harga nett
                    item.extra_data = itemextradata;
                    items.Add(item);
				
    				Db.Execute("UPDATE SalesOrderLD SET PoinReceived = " + total_poinmembership + ", PoinPembagi = " + divisor + " WHERE ID = " + data.ID, Db.Cnn_DB);																									   
				
    				//DeactivePLUStamps(data.StockProductLD_Stone1B.Nomor);
                }
            }

            //// Extra Data
            List<STAMPS_API_CONNECT.ExtraData> extra_datas = new List<STAMPS_API_CONNECT.ExtraData>();

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

            var mscust = cs.Mscustomer.SingleOrDefault(p => p.M_kode == d.DataCustomer.NoCustomer);
            string st_email = mscust.M_email;
            string st_phone = mscust.M_telepon1;
            string namacustomer = mscust.M_nama;

            string st_user_stamps = (st_phone != "" && st_phone != null) ? st_phone : st_email;
            st_user_stamps = (namacustomer.ToUpper() == "CMK CLUB") ? "" : st_user_stamps;// Tambahan By Andri Jika Akun default maka transaksi yang di add di jaws tidak punya user

            short st_store = 2; // masih hardcode

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
                    //st_store = Convert.ToInt16(dtRow["STAMPS_LOC_ID"].ToString());

                }

            }

    		//st_store = "E02";
            //// Stamps Payment
            List<STAMPS_API_CONNECT.StampsPayment> stamps_payments = new List<STAMPS_API_CONNECT.StampsPayment>();

            var sr = db.SalesReceiptDetail.Where(p => p.SalesReceipt.SalesOrder == d);

            foreach(var data in sr)
            {
                int stamps_payment_id = 60; // OTHERS
                string jaws_payment_id = data.PaymentType.Nama;

                switch (jaws_payment_id.ToUpper())
                {
                    case "CASH":
                        //statements 
                        stamps_payment_id = 10;
                        break;
                    case "DEBIT CARD":
                        //statements 
                        stamps_payment_id = 20;
                        break;
                    case "CREDIT CARD":
                        //statements 
                        stamps_payment_id = 30;
                        break;
    				case "BRIDESTORY PAY":
                        //statements
                        stamps_payment_id = 40;
                        break;
                    case "VOUCHER":
                        //statements 
                        stamps_payment_id = 50;
                        break;
                }

                //if (jaws_payment_id ==)
                //stamps_payment_id = data.PaymentType.ID;

                STAMPS_API_CONNECT.StampsPayment payment = new STAMPS_API_CONNECT.StampsPayment();
                payment.payment_method = stamps_payment_id; // mapping ID payment jaws to ID stamps
                payment.value = Convert.ToInt64(data.Nominal.Value);
                payment.eligible_for_membership = (data.PaymentType.Nama.ToUpper() == "EXCHANGE" ? false : true);

                stamps_payments.Add(payment);
            }

    
            int st_stamps = grandtotal_poinmembership; // stamps : multiconversion poin here

            string st_invoice = d.Nomor;
            long st_totalval = Convert.ToInt64(d.TotalBayar); // Grand Total Harga Nett
            long st_subtotal = Convert.ToInt64(d.TotalBayar); // Grand Total Harga Nett
            string st_employeenik = d.DataSales.Keterangan;
            int st_numberpeople = 1;
            int st_tax = 0;
            string st_datetime = Convert.ToDateTime(d.OperatorTgl).ToString("O");//System.DateTime.Now.ToString("s");
		
    		bool isRequireEmail = Db.SingleBool("SELECT TOP 1 ISNULL(IsSentEreceipt,0) [IsSentEreceipt] FROM SalesOrder WHERE ID = " + d.ID.ToString(),Db.Cnn_DB);
		
            string st_requireemailnotif = isRequireEmail.ToString();

            var result = api.AddTransaction(st_user_stamps, st_store, st_stamps, st_invoice, st_totalval, st_subtotal, st_employeenik
                , st_numberpeople, st_tax, st_datetime, st_requireemailnotif, items, extra_datas, stamps_payments);
    		while(result == null){
    			result = api.AddTransaction(st_user_stamps, st_store, st_stamps, st_invoice, st_totalval, st_subtotal, st_employeenik
                , st_numberpeople, st_tax, st_datetime, st_requireemailnotif, items, extra_datas, stamps_payments);
    		}
            var obj = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseAddTransaction>(result.data.ToString());
            var objExistingID = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseExistingTransactionID>(result.extra_data.ToString());

            // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.
            //richTextBox1.Text = JsonConvert.SerializeObject(result);

            //litTest.Text += JsonConvert.SerializeObject(result) + "<br /><br />";
            //litTest.Text += st_datetime + "<br /><br />";
		
    		foreach(var data in sr)
            {
    			if(data.IDPaymentType == 5)
                {
                    int countrd_id = Db.SingleInteger("select count('') FROM STAMPS_VOUCHER_REDEMPTION WHERE VC_NUMBER = '" + data.CCNumber + "'", Db.Cnn_IN);
                    if (countrd_id > 0)
                    {
                        int rd_id = Db.SingleInteger("select top 1 ISNULL(RD_ID,0) [RD_ID] FROM STAMPS_VOUCHER_REDEMPTION WHERE VC_NUMBER = '" + data.CCNumber + "' ORDER BY RD_ID DESC ", Db.Cnn_IN);
    					try
    					{
    						if (rd_id != 0)
    							AddTransactionWithRedemptionVoucher(rd_id.ToString(), data.SalesReceipt.SalesOrder.Nomor);
    					}
    					catch{}
                    }
                }
    		}
		
    		var discvoucher = d.SalesVoucher;
    		if(discvoucher.Count > 0)
    		{
    			try
    			{
    				foreach (var x in discvoucher) {
    					int countrd_id = Db.SingleInteger("select count('') FROM STAMPS_VOUCHER_REDEMPTION WHERE VC_NUMBER = '" + x.NomorVoucher + "'", Db.Cnn_IN);
    					if (countrd_id > 0)
    					{
    						int rd_id = Db.SingleInteger("select top 1 ISNULL(RD_ID,0) FROM STAMPS_VOUCHER_REDEMPTION WHERE VC_NUMBER = '" + x.NomorVoucher + "' ORDER BY RD_ID DESC ", Db.Cnn_IN);
    						if (rd_id != 0)
    							litTest.Text = AddTransactionWithRedemptionVoucher(rd_id.ToString(), d.Nomor);
    					}
    				}
    			}
    			catch { }
    		}

            try
            {
                if (result.status_code == 200 || (result.status_code == 400 && result.error_message.Contains("already exists")))
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
    public static string postAddRedemption(int SOID, string SONO, string VoucherNumber, int RedeemID, string KodeBrand, string plu)
    {
        string msg = "";
        int xRD_ID = RedeemID;
        string xSO_BRAND = KodeBrand;
        int xSO_ID = SOID;
        string xSO_NUMBER = SONO;
        string xPLU_NUMBER = plu;
        string xVC_NUMBER = VoucherNumber;
        string xTRANSACTION_MODE = "R";
		 
		//msg += Str.isNumber(xRD_ID,false)+";";
		//msg += Str.isString(xSO_BRAND)+";";
		//msg += Str.isNumber(xSO_ID)+";";
		//msg += Str.isString(xSO_NUMBER)+";";
		//msg += Str.isString(xPLU_NUMBER)+";";
		//msg += Str.isString(xSO_BRAND)+";";
		//msg += Str.isString(xVC_NUMBER)+";";
		//msg += Str.isString(xTRANSACTION_MODE);


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
		
    		string querygetidtrans = "SELECT TOP 1 ISNULL(IDTransactionStamps,0) [IDTransactionStamps] FROM SalesOrder WHERE ID = " + d.ID.ToString();
    		int IDTransactionStampsSO = Db.SingleInteger(querygetidtrans, Db.Cnn_DB);
		
    		if(IDTransactionStampsSO == 0 && StampsTransactionID > 0)
    		{
    			Db.Execute("UPDATE SalesOrder SET IDTransactionStamps = " + StampsTransactionID + " WHERE ID = " + d.ID.ToString(), Db.Cnn_DB);
    		}

            return msg;
            }
    }

    public static string postStampsMemberIntegration
        (STAMPS_API_RESPONSE.ResponseMembership.User user,
            STAMPS_API_RESPONSE.ResponseMembership.Membership membership
        )
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
                     + Str.isString(membership.status_text != null ? membership.status_text : "")
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
                        // msg += "Invalid STAMPS Add Transaction Integration - Transaction ID !!";
    					msg += "STAMPS Error : Transaction doesn't exists. <br /> Segera hubungi Tim CRM.";
                    }
                    else
                    {
                        int StampsTransactionID = Convert.ToInt32(dtRow["STAMPS_RESPONSE_TRANSACTION_ID"].ToString());
					
    					if(d.Unpaid != null)
                        {
                            if(d.Unpaid == 0) {
    							var result = api.CancelTransaction(StampsTransactionID);
    							var obj = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseCancelTransaction>(result.data.ToString());                            
                                try
                                {
                                    int status_code = result.status_code;
                                    if (status_code == 400 ||status_code == 401 ||status_code == 405 ||status_code == 500 || status_code == 502 || status_code == 503 ||status_code == 504)
                                    {
                                        msg += "STAMPS Error : " + result.error_message.ToString() + ".";
                                    }else if (status_code == 403 ||status_code == 404 ){
                                        msg += " STAMPS Error : " + result.error_message.ToString() + ". <br /> Segera hubungi Tim CRM.";
                                    }else if (status_code == 200)
                                    {
                                        msg+=result.error_message.ToString();
                                    }
                                
                                }
                                catch (Exception ex)
                                {
                                    // Handle serialization error
                                    msg += "Error serializing object: "+ ex.Message;
                                }
    						}
    					}
                        // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.
                        //richTextBox1.Text = JsonConvert.SerializeObject(result);

    
    
                    }
                }

            }

            return msg;
            }
    }
	
	public static string DeleteTransaction(Literal litTest, SalesOrder d)
    {
        using (Map_DB db = new Map_DB())
        {

            string msg = "";
            string xSO_BRAND = "";
            string xSO_NUMBER = d.Nomor;

            if (d.TipeLokasi == 1)
            {
                var loc = db.LocOutlet.Single(p => p.ID == d.IDLokasi);
                xSO_BRAND = loc.CompanyBrand.NamaKode.ToString();
            }
            else if (d.TipeLokasi == 2)
            {
                var loc = db.LocExhibition.Single(p => p.ID == d.IDLokasi);
                xSO_BRAND = loc.CompanyBrand.NamaKode.ToString();
            }

            string spname = "[SP_STAMPS_DELETE_TRANSACTION]";
            string query = " exec " + spname
                         + Str.isString(xSO_BRAND, false)
                         + Str.isString(xSO_NUMBER)
                         ;

            DataTable dt = new DataTable();
            dt = Db.Rs(query, Db.Cnn_IN);

            if(dt.Rows[0]["isValid"].ToString() == "0")
            {
                msg = dt.Rows[0]["Message"].ToString();
            }

            return msg;
            }
    }

    public static string ModifyTransaction(Literal litTest, SalesOrder d)
    {
        string msg = "";

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
        List<STAMPS_API_CONNECT.Item> items = new List<STAMPS_API_CONNECT.Item>();

        STAMPS_API_CONNECT.Item item1 = new STAMPS_API_CONNECT.Item();
        item1.product_name = "Diamond Jewellery -- Pendant -- Fashion -- Diamond Jewellery 1 -- FANCY DESIGN";
        item1.quantity = 0;

        items.Add(item1);

        var result = api.ModifyTransaction(86, 0, 0, -1, items);
        var obj = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseModifyTransaction>(result.data.ToString());

        // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.
        //richTextBox1.Text = JsonConvert.SerializeObject(result);

        return msg;
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

        if (keyword != null)
        {
            var result = api.MemberSuggestions(keyword, STAMPS.xMerchant);
            var obj = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseMemberSuggestions>(result.data.ToString());

            // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.

            int index = 1;

            //litTest.Text += JsonConvert.SerializeObject(result) + "<br /><br />";

            if (obj.suggestions.Length == 0) { App.TableNoResult(tbl); }
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
                            if (dc_code != null)
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

                        //td = new TableCell();
                        //td.Text = (xActiveMember) ? "Yes" : "<span class='text-red'>No</span>";
                        //td.Style.Add("text-align", "center");
                        //td.Style.Add("vertical-align", "top");
                        //tr.Cells.Add(td);

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

            string errMsg = "";

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

            short st_store = 2; // masih hardcode

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
                    //st_store = Convert.ToInt16(dtRow["STAMPS_LOC_ID"].ToString());

                }

            }

            var result = api.ValidateVoucherCode(VoucherCode, st_store);
            //var obj = JsonConvert.DeserializeObject<STAMPS_Response.ResponseValidateVoucherCode>(result.data.ToString());
    		var obj = JsonConvert.DeserializeObject<ResponseValidateVoucherCodeUpdate>(result.data.ToString());

            // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.
            //richTextBox1.Text = JsonConvert.SerializeObject(result);

            string xstatus_code = result.status_code.ToString();

            // errMsg +=JsonConvert.SerializeObject(result);
            // errMsg +=JsonConvert.SerializeObject(obj.voucher.value);
            try        
            {
                if (result.status_code == 200)            
                {
                    voucher_is_redeemable = Convert.ToBoolean(obj.is_redeemable);

                    if (voucher_is_redeemable || (result.status_code == 200 && !voucher_is_redeemable))
                    {
                        // view voucher data
                        voucher_id = obj.voucher.id;
                        voucher_name = (obj.voucher.name == null) ? "" : obj.voucher.name.ToString();

                        //voucher_validity = (obj.voucher.extra_data.voucher_validity == null) ? "" : obj.voucher.extra_data.voucher_validity.ToString();					
                        voucher_validity = (obj.voucher.validity == null) ? "" : obj.voucher.validity.ToString();
                        voucher_type = (obj.voucher.extra_data.voucher_type == null) ? "" : obj.voucher.extra_data.voucher_type.ToString();                    
                        decimal asd = (obj.voucher.value == null || obj.voucher.value == 0) ? obj.voucher.extra_data.voucher_nominal : obj.voucher.value ?? 0;
                        voucher_nominal = (Convert.ToInt32(asd)).ToString();
                        // voucher_nominal = Str.Number0(Convert.ToInt32(obj.voucher.extra_data.voucher_nominal));
                        //voucher_date_start = Str.FullDate(Convert.ToDateTime(obj.voucher.extra_data.voucher_date_start));
    					voucher_date_start = Str.FullDate(Convert.ToDateTime(obj.voucher.start_date));
                        //voucher_date_end = Str.FullDate(Convert.ToDateTime(obj.voucher.extra_data.voucher_date_end));
    					voucher_date_end = Str.FullDate(Convert.ToDateTime(obj.voucher.end_date));
                        //voucher_valid_for = Str.Number0(Convert.ToInt32(obj.voucher.extra_data.voucher_valid_for));
                    
                        string x1 = VoucherAllowedType.Trim().ToUpper();
                        string x2 = voucher_type.Trim().ToUpper();
                    
                        if (x1 != x2)
                        {
                            errMsg += "STAMPS Validate Voucher : Voucher Type is not valid. Voucher Type allowed : <b>" + x1 + "</b>.";

                        }
                        else if (Convert.ToInt32(obj.voucher.extra_data.voucher_nominal) <= 0)
                        {
                            errMsg += "STAMPS Validate Voucher : Voucher Value is not valid.";

                        }
                        else
                        {

                            // add database myapps
                            //if (voucher_validity.ToUpper() == "STATIC")
                            //{
                                string xyz = prePostVoucherIntegration(
                                    VoucherCode
                                    , Convert.ToInt32(voucher_nominal)
                                    // , Convert.ToInt32(obj.voucher.extra_data.voucher_nominal)
                                    , Convert.ToString(obj.voucher.start_date)
                                    , Convert.ToString(obj.voucher.end_date)
                                    , Convert.ToString(obj.voucher.extra_data.voucher_type)
                                    , Convert.ToString(voucher_name)
                                    , Convert.ToInt32(voucher_id)
                                    , Convert.ToString(VoucherAllowedType)
                                    );
                            //}
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
            catch (Exception ex)
            {
                errMsg = "Terjadi kesalahan teknis. Silahkan hubungi IT Support. (STAMPS Error message : "
                         + ex.Message
                         + ex.StackTrace
                         + ex.InnerException
                         + ")";
            }

    
            return errMsg;
            }
    }
	#region "Response Validate Voucher Code"
    public class ResponseValidateVoucherCodeUpdate
    {
        public ResponseValidateVoucherCodeUpdate.Voucher voucher;
        public ResponseValidateVoucherCodeUpdate.User user;
        public bool is_redeemable;

        public class Voucher
        {
            public Int32 id;
            public string name;
            public decimal? value;
            public dynamic extra_data;
            public string validity;
            public string start_date;
            public string end_date;
        }

        public class User
        {
            public string name;
            public string email;
            public string phone;
        }
    }
    #endregion

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

            short st_store = 2; // masih hardcode

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
                    //st_store = Convert.ToInt16(dtRow["STAMPS_LOC_ID"].ToString());

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
            var obj = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseRedeemVoucherCode>(result.data.ToString());

            // Dilakukan `serialize` kembali, hanya untuk menampilkan `JSON String` pada TextArea.
            //richTextBox1.Text = JsonConvert.SerializeObject(result);
            try
            {
                if (result.status_code == 200)
                {
                    int SOID = d.ID;
                    string SONO = d.Nomor;

    
                    //errMsg += SOID.ToString()
                    //    + "<br />" + SONO.ToString()
                    //    + "<br />" + VoucherCode.ToString()
                    //    + "<br />" + obj.redemption.id.ToString()
                    //    + "<br />" + KodeBrand.ToString()
                    //    + "<br />" + plu.ToString()
                    //    ;

                    // add database integration
                    postAddRedemption(SOID, SONO,
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
            catch (Exception ) { }

    
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
            else if(myItemType == 0)
            {
                plu = "";
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
                if (x)
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
	
	public static bool VoidVoucherByPLUPerDetailPayment(SalesReceiptDetail srd)
    {
        bool result = false;
        if(srd.IDPaymentType == 5)
        {
            string VoucherCode = srd.CCNumber;
            DataTable dt = Db.Rs("EXEC SP_STAMPS_VOUCHER_REDEMPTION_PER_PAYMENT_DETAIL '"+ VoucherCode +"'", Db.Cnn_IN);
            if(dt.Rows.Count == 1)
            {
                result = CancelRedemptions(Convert.ToInt32(dt.Rows[0]["RD_ID"].ToString()));
            }
        }
        return result;
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
        var obj = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseCancelRedemptions>(result.data.ToString());

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
        var obj = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseGetMembershipByCode>(result.data.ToString());

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

        if (xMemberCode != "" && xActiveMember)
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
        if (x.Length > 10)
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
	
	protected static string AddTransactionWithRedemptionVoucher(string _redemption_id, string _invoicenumber)
    {
        string TOKEN = "ecef302a566d6d73c355198c3981cd024df5be2a";

        const string CURRENT_URL = "https://crm.cmkclub.com/api/v2/redemptions/add-invoice-number";
        var request = (HttpWebRequest)WebRequest.Create(CURRENT_URL);

        string JSON = new JavaScriptSerializer().Serialize(new
        {
            token = TOKEN,
            redemption_id = _redemption_id,
            invoice_number = _invoicenumber,
        });

        return CoreAPI(request, JSON);
    }
	
    protected static string CoreAPI(HttpWebRequest request, string json = null)
    {
        request.ContentType = "application/json";
        request.Timeout = 3000;
		request.Method = "POST";

        try
        {
            if (request.Method == "POST")
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
				using (Stream stream = response.GetResponseStream())
                {
                    XmlTextReader reader = new XmlTextReader(stream);
                }
                return new StreamReader(response.GetResponseStream()).ReadToEnd();
                //return new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
        }
        catch (WebException ex)
        {
            HttpWebResponse errorResponse = ex.Response as HttpWebResponse;
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

            if (errorResponse.StatusCode == HttpStatusCode.NotFound || errorResponse.StatusCode == HttpStatusCode.InternalServerError)
            {
                /* 
                 * Karena HTTP 404 dan HTTP 500 tidak mungkin ada error messsage di body, 
                 * kita encode message sebagai deskripsi literal dari HTTP 404 yaitu "not Found" 
                 * atau HTTP 500 "Internal Server error"
                 */

                dynamic obj = new ExpandoObject();
                obj.statusCode = (int)errorResponse.StatusCode;
                obj.message = errorResponse.StatusCode.ToString();

                return JsonConvert.SerializeObject(obj);
            }
            else
            {
                var deserializeObject = JsonConvert.DeserializeObject<dynamic>(resp.ToString());

                dynamic obj = new ExpandoObject();
                obj.statusCode = Convert.ToInt16(errorResponse.StatusCode);
                obj.message = deserializeObject["error_message"] == null ? deserializeObject["detail"] : deserializeObject["error_message"];

                return JsonConvert.SerializeObject(obj);
            }
        }
    }
	
    public static bool CheckLimitationDiscountVoucherDJ(SalesOrderDJ dj)
    {
        using (Map_DB db = new Map_DB())
        {

            bool isgetpoin = false;
            decimal percent = 0.3m;

            DateTime now = Convert.ToDateTime(dj.SalesOrder.Tgl);
            int month = now.Month;
            int year = now.Year;
            if (month == 7 && year == 2021)
            {
                percent = 0.4m;
            }

            if (now <= Convert.ToDateTime("2024-05-17 23:59:00.000"))
            {
                if (dj.TotalBayar * percent <= dj.DiscountGift)
                {
                    isgetpoin = false;
                }
                else
                {
                    isgetpoin = true;
                }
            }
            else
            {
                string[] store_exclude = new string[] { "E00", "S00", "P00", "M00" };
                string store = "";
                bool isgetpoinbasedonstore = false;

                if (dj.SalesOrder.TipeLokasi == 1)
                    store = db.LocOutlet.SingleOrDefault(p => p.ID == dj.SalesOrder.IDLokasi).Kode;
                else if (dj.SalesOrder.TipeLokasi == 2)
                    store = db.LocExhibition.SingleOrDefault(p => p.ID == dj.SalesOrder.IDLokasi).Kode;
                foreach(string x in store_exclude)
                {
                    if (x == store)
                    {
                        isgetpoinbasedonstore = true;
                        break;
                    }
                }

                if (isgetpoinbasedonstore) { isgetpoin = true; }
                else
                {
                    decimal pembilang = Convert.ToDecimal(dj.HargaRupiah) - Convert.ToDecimal(dj.DiscountPromo) - Convert.ToDecimal(dj.TotalBayar);
                    decimal pembagi = Convert.ToDecimal(dj.HargaRupiah) - Convert.ToDecimal(dj.DiscountPromo);
                    if (pembilang / pembagi >= percent)
                    {
                        isgetpoin = false;
                    }
                    else
                    {
                        isgetpoin = true;
                    }
                }
            }
            return isgetpoin;
            }
    }
    public static bool CheckLimitationDiscountVoucherPG(SalesOrderPG pg)
    {
        using (Map_DB db = new Map_DB())
        {

            bool isgetpoin = false;
            decimal percent = 0.3m;

            DateTime now = Convert.ToDateTime(pg.SalesOrder.Tgl);
            int month = now.Month;
            int year = now.Year;
            if (month == 7 && year == 2021)
            {
                percent = 0.4m;
            }

            if (now <= Convert.ToDateTime("2024-05-17 23:59:00.000"))
            {
                if (pg.TotalBayar * percent <= pg.DiscountGift)
                {
                    isgetpoin = false;
                }
                else
                {
                    isgetpoin = true;
                }
            }
            else
            {
                string[] store_exclude = new string[] { "E00", "S00", "P00", "M00" };
                string store = "";
                bool isgetpoinbasedonstore = false;

                if (pg.SalesOrder.TipeLokasi == 1)
                    store = db.LocOutlet.SingleOrDefault(p => p.ID == pg.SalesOrder.IDLokasi).Kode;
                else if (pg.SalesOrder.TipeLokasi == 2)
                    store = db.LocExhibition.SingleOrDefault(p => p.ID == pg.SalesOrder.IDLokasi).Kode;
                foreach (string x in store_exclude)
                {
                    if (x == store)
                    {
                        isgetpoinbasedonstore = true;
                        break;
                    }
                }

                if (isgetpoinbasedonstore) { isgetpoin = true; }
                else
                {
                    decimal pembilang = Convert.ToDecimal(pg.HargaRupiah) - Convert.ToDecimal(pg.DiscountPromo) - Convert.ToDecimal(pg.TotalBayar);
                    decimal pembagi = Convert.ToDecimal(pg.HargaRupiah) - Convert.ToDecimal(pg.DiscountPromo);
                    if (pembilang / pembagi >= percent)
                    {
                        isgetpoin = false;
                    }
                    else
                    {
                        isgetpoin = true;
                    }
                }
            }
            return isgetpoin;
            }
    }
    public static bool CheckLimitationDiscountVoucherGJ(SalesOrderGJ gj)
    {
        using (Map_DB db = new Map_DB())
        {

            bool isgetpoin = false;
            decimal percent = 0.3m;

            DateTime now = Convert.ToDateTime(gj.SalesOrder.Tgl);
            int month = now.Month;
            int year = now.Year;
            if (month == 7 && year == 2021)
            {
                percent = 0.4m;
            }

            if (now <= Convert.ToDateTime("2024-05-17 23:59:00.000"))
            {
                if (gj.TotalBayar * percent <= gj.DiscountGift)
                {
                    isgetpoin = false;
                }
                else
                {
                    isgetpoin = true;
                }
            }
            else
            {
                string[] store_exclude = new string[] { "E00", "S00", "P00", "M00" };
                string store = "";
                bool isgetpoinbasedonstore = false;

                if (gj.SalesOrder.TipeLokasi == 1)
                    store = db.LocOutlet.SingleOrDefault(p => p.ID == gj.SalesOrder.IDLokasi).Kode;
                else if (gj.SalesOrder.TipeLokasi == 2)
                    store = db.LocExhibition.SingleOrDefault(p => p.ID == gj.SalesOrder.IDLokasi).Kode;
                foreach (string x in store_exclude)
                {
                    if (x == store)
                    {
                        isgetpoinbasedonstore = true;
                        break;
                    }
                }

                if (isgetpoinbasedonstore) { isgetpoin = true; }
                else
                {
                    decimal pembilang = Convert.ToDecimal(gj.HargaRupiah) - Convert.ToDecimal(gj.DiscountPromo) - Convert.ToDecimal(gj.TotalBayar);
                    decimal pembagi = Convert.ToDecimal(gj.HargaRupiah) - Convert.ToDecimal(gj.DiscountPromo);
                    if (pembilang / pembagi >= percent)
                    {
                        isgetpoin = false;
                    }
                    else
                    {
                        isgetpoin = true;
                    }
                }
            }
            return isgetpoin;
            }
    }
    public static bool CheckLimitationDiscountVoucherLD(SalesOrderLD ld)
    {
        using (Map_DB db = new Map_DB())
        {

            bool isgetpoin = false;
            decimal percent = 0.3m;

            DateTime now = Convert.ToDateTime(ld.SalesOrder.Tgl);
            int month = now.Month;
            int year = now.Year;
            if (month == 7 && year == 2021)
            {
                percent = 0.4m;
            }

            if (now <= Convert.ToDateTime("2024-05-17 23:59:00.000"))
            {
                if (ld.TotalBayar * percent <= ld.DiscountGift)
                {
                    isgetpoin = false;
                }
                else
                {
                    isgetpoin = true;
                }
            }
            else
            {
                string[] store_exclude = new string[] { "E00", "S00", "P00", "M00" };
                string store = "";
                bool isgetpoinbasedonstore = false;

                if (ld.SalesOrder.TipeLokasi == 1)
                    store = db.LocOutlet.SingleOrDefault(p => p.ID == ld.SalesOrder.IDLokasi).Kode;
                else if (ld.SalesOrder.TipeLokasi == 2)
                    store = db.LocExhibition.SingleOrDefault(p => p.ID == ld.SalesOrder.IDLokasi).Kode;
                foreach (string x in store_exclude)
                {
                    if (x == store)
                    {
                        isgetpoinbasedonstore = true;
                        break;
                    }
                }

                if (isgetpoinbasedonstore) { isgetpoin = true; }
                else
                {
                    decimal pembilang = Convert.ToDecimal(ld.HargaRupiah) - Convert.ToDecimal(ld.DiscountPromo) - Convert.ToDecimal(ld.TotalBayar);
                    decimal pembagi = Convert.ToDecimal(ld.HargaRupiah) - Convert.ToDecimal(ld.DiscountPromo);
                    if (pembilang / pembagi >= percent)
                    {
                        isgetpoin = false;
                    }
                    else
                    {
                        isgetpoin = true;
                    }
                }
            }
            return isgetpoin;
            }
    }
	public static decimal NominalVoucherRewardRedemption(SalesOrderDJ dj)
    {
        decimal cutpay = 0m;
        var vrd = dj.SalesOrder.SalesVoucher.Where(p => p.ProductID == dj.StockProductDJ.ID);
		if(vrd != null)
		{
			foreach(var v in vrd)
			{
				if(v.NomorVoucher.StartsWith("JC500") || 
					v.NomorVoucher.StartsWith("JWLCER") || 
					v.NomorVoucher.StartsWith("JC2000") || 
					v.NomorVoucher.StartsWith("JCCMK") || 
					v.NomorVoucher.StartsWith("JC10000") || 
					v.NomorVoucher.StartsWith("JC20000") ||
					v.NomorVoucher.StartsWith("JC200"))
				{
					cutpay += v.Nominal ?? 0m;
				}
			}
		}
        return cutpay;
    }

    public static decimal NominalVoucherRewardRedemption(SalesOrderPG pg)
    {
        decimal cutpay = 0m;
        var vrd = pg.SalesOrder.SalesVoucher.Where(p => p.ProductID == pg.StockProductPG.ID);
		if(vrd != null)
		{
			foreach(var v in vrd)
			{
				if (v.NomorVoucher.StartsWith("JC500") ||
					v.NomorVoucher.StartsWith("JWLCER") ||
					v.NomorVoucher.StartsWith("JC2000") ||
					v.NomorVoucher.StartsWith("JCCMK") ||
					v.NomorVoucher.StartsWith("JC10000") ||
					v.NomorVoucher.StartsWith("JC20000") ||
					v.NomorVoucher.StartsWith("JC200"))
				{
					cutpay += v.Nominal ?? 0m;
				}
			}
		}
        return cutpay;
    }
    public static decimal NominalVoucherRewardRedemption(SalesOrderGJ gj)
    {
        decimal cutpay = 0m;
        var vrd = gj.SalesOrder.SalesVoucher.Where(p => p.ProductID == gj.StockProductGJ.ID);
		if(vrd != null)
		{
			foreach(var v in vrd)
			{
				if (v.NomorVoucher.StartsWith("JC500") ||
					v.NomorVoucher.StartsWith("JWLCER") ||
					v.NomorVoucher.StartsWith("JC2000") ||
					v.NomorVoucher.StartsWith("JCCMK") ||
					v.NomorVoucher.StartsWith("JC10000") ||
					v.NomorVoucher.StartsWith("JC20000") ||
					v.NomorVoucher.StartsWith("JC200"))
				{
					cutpay += v.Nominal ?? 0m;
				}
			}
		}
        return cutpay;
    }
    public static decimal NominalVoucherRewardRedemption(SalesOrderLD ld)
    {
        decimal cutpay = 0m;
        var vrd = ld.SalesOrder.SalesVoucher.Where(p => p.ProductID == ld.StockProductLD_Stone1B.ID);
		if(vrd != null)
		{
			foreach(var v in vrd)
			{
				if (v.NomorVoucher.StartsWith("JC500") ||
					v.NomorVoucher.StartsWith("JWLCER") ||
					v.NomorVoucher.StartsWith("JC2000") ||
					v.NomorVoucher.StartsWith("JCCMK") ||
					v.NomorVoucher.StartsWith("JC10000") ||
					v.NomorVoucher.StartsWith("JC20000") ||
					v.NomorVoucher.StartsWith("JC200"))
				{
					cutpay += v.Nominal ?? 0m;
				}
			}
		}
        return cutpay;
    }
	public static decimal NominalVoucherRewardRedemptionPayment(SalesOrder so)
    {
        using (Map_CS cs = new Map_CS())
        {

            decimal cutpay = 0m;
            var voucher = cs.T_voucher.Where(p => p.M_klaimdoc == so.Nomor);
            if(voucher != null)
            {
                foreach(var v in voucher)
                {
                    if(v.M_nomor.StartsWith("JC500") || v.M_nomor.StartsWith("JWLCER") || v.M_nomor.StartsWith("JC2000") || v.M_nomor.StartsWith("JCCMK") || v.M_nomor.StartsWith("JC10000") || v.M_nomor.StartsWith("JC20000") || v.M_nomor.StartsWith("JC200"))
                    {
                        cutpay += v.M_value;
                    }
                }
            }
            return cutpay;
            }
    }
	
	public static decimal NominalPembagiPromoDJ(SalesOrderDJ dj)
	{
		using (Map_DB db = new Map_DB())
		{

			decimal divisor = 0m;
			// promo
			var t = from p in db.PromoDJ_Detail
					where p.PromoDJ.ApprovalNama != null && p.PromoDJ.TglStart.Value <= dj.SalesOrder.Tgl
					&& p.PromoDJ.TglExpire.Value >= dj.SalesOrder.Tgl && p.IDProduct.Value == dj.IDProduct
					&& p.PromoDJ.Draft == false
					select p;
			if (t.Count() > 0)
			{
				var disc = t.First();
				var discP = disc.PromoDJ;
			
				bool statuspembagi = Db.SingleBool("SELECT ISNULL(StatusPembagiPoin,0) [F] FROM PromoDJ WHERE ID = " + discP.ID,Db.Cnn_DB);
				decimal nominalpembagi = Db.SingleDecimal("SELECT ISNULL(PembagiPoin,0) [F] FROM PromoDJ WHERE ID = " + discP.ID,Db.Cnn_DB);
			
				if(statuspembagi)
					divisor = nominalpembagi;
			}
			return divisor;
			}
	}
	
	public static decimal? NominalPoinPromoPG(SalesOrderPG pg)
	{
		decimal? nominal = 0m;
		
		nominal = (pg.TotalBayar - pg.DiscountPromo - pg.DiscountGift - pg.DiscountProgramNominal) / ((pg.HargaRupiah - pg.DiscountPromo) * pg.StockProductPG.GrossWeight);
		
		return nominal;
	}
	
	public class PLU 
    {
        public string merchant_slug;
        public string variant_code;
    }
	
	public static void DeactivePLUStamps(string nomorplu)
	{
		string res = "";
		PLU obj = new PLU
		{
			merchant_slug = "franknco",
			variant_code = nomorplu
		};
		string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
		res = InactiveProductStamps.WebRequestPOST(json);
	}
}

