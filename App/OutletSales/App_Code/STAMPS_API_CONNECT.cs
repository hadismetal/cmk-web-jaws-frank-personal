using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for STAMPS_API_CONNECT
/// </summary>
public class STAMPS_API_CONNECT
{
    #region "Property"
    public class Item
    {
        public string product_name; // Di isi dengan `ProductCode` oleh Pihak CMK
        public int quantity;
        public ItemExtraData extra_data;
        public Int32 stamps_subtotal;
        public Int64 subtotal;
    }

    public class ItemExtraData
    {
        /*
         * Jika ada informasi tambah yang ingin disimpan.
         * Silahkan tambahkan property pada class `ItemExtraData` berdasarkan kebutuhan.
         * Informasi property dibawah merupakan contoh saja.
         */

        public string category;
        public string item;
        public string level;
        public string stonedistribution;
        public string type;
		public int poincategory;
		public int poincategorynominal;
    }

    public class ExtraData
    {
        /*
         * Jika ada informasi tambahan yang ingin disimpan, 
         * silahkan tambahkan property pada class `ExtraData` ini.
         */
    }

    public class StampsPayment
    {
        public Int32 payment_method;
        public Int64 value;
        public bool eligible_for_membership;
    }

    public class VoucherRedeemption
    {
        public string voucher_code;
    }

    public class ResponseData
    {
        public int status_code;
        public string status_message;

        [JsonConverter(typeof(PreventDoubleSerialization))]
        public string error_message = "";

        // Field terstruktur dari body error STAMPS (contoh: "duplicate_invoice_number") - dulu
        // hanya error_message (teks bebas) yang ditangkap, jadi deteksi kasus tertentu (misal
        // duplicate invoice) harus pakai string-match yang fragile. error_code jauh lebih presisi.
        public string error_code = "";

        [JsonConverter(typeof(PreventDoubleSerialization))]
        public string data = "";

        [JsonConverter(typeof(PreventDoubleSerialization))]
        public string extra_data = "";
    }
    #endregion
    public class PreventDoubleSerialization : JsonConverter
    {
        /*
	     * 
	     * Kegunaan `class` ini untuk menjaga supaya tidak double serialization.
	     * Digunakan hanya untuk menampilkan pada TextArea.
	     * 
	     */

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object value = serializer.Deserialize(reader);
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string str = (string)value == "" ? "\"\"" : (string)value;
            writer.WriteRawValue(str);
        }
    }

    #region "Class API"
    public class Stamps_API
    {
        private string BASE_URL = ConfigurationManager.AppSettings["STAMPSBASEURL"];
        private string TOKEN = ConfigurationManager.AppSettings["STAMPSAPI"];
        private string MERCHANT = ConfigurationManager.AppSettings["STAMPSMerchant"];

        #region "Core"
        private ResponseData ParseErrorResponse(WebException ex)
        {
            ResponseData response_data = new ResponseData();

            if (ex.Response == null)
            {
                response_data.status_code = 0;
                response_data.status_message = "Timeout/ConnectionFailure";
                return response_data;
            }

			//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebResponse errorResponse = ex.Response as HttpWebResponse;
            var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

            if (errorResponse.StatusCode == HttpStatusCode.NotFound || errorResponse.StatusCode == HttpStatusCode.InternalServerError)
            {
                /*
                 * Karena HTTP 404 dan HTTP 500 tidak mungkin ada error messsage di body, 
                 * kita encode message sebagai deskripsi literal dari HTTP 404 yaitu "not Found" 
                 * atau HTTP 500 "Internal Server error"
                 */

                response_data.status_code = (int)errorResponse.StatusCode;
                response_data.status_message = errorResponse.StatusCode.ToString();

                return response_data;
            }
            else
            {
                var deserializeObject = JsonConvert.DeserializeObject<dynamic>(resp.ToString());

                response_data.status_code = Convert.ToInt16(errorResponse.StatusCode);
                response_data.status_message = errorResponse.StatusCode.ToString();
                response_data.error_message = deserializeObject["error_message"] == null ?
                                              deserializeObject["detail"].ToString() :
                                              deserializeObject["error_message"].ToString();
                if (deserializeObject["error_code"] != null)
                {
                    response_data.error_code = deserializeObject["error_code"].ToString();
                }
                if (deserializeObject["extra"] != null)
                {
                    response_data.extra_data = deserializeObject["extra"].ToString();
                }

                return response_data;
            }
        }

        private ResponseData CoreAPI(string HttpMethod, HttpWebRequest request, string json = null)
        {
            request.ContentType = "application/json";
            request.Method = HttpMethod;
            request.Timeout = 8000;

            try
            {
                if (HttpMethod == "POST")
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(json);
                    }
                }
				
				//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    ResponseData response_data = new ResponseData();
                    response_data.status_code = Convert.ToInt16(response.StatusCode);
                    response_data.status_message = "";
                    response_data.error_message = "";
                    response_data.data = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    return response_data;
                }
            }
            catch (WebException ex)
            {
                return ParseErrorResponse(ex);
            }
        }
        #endregion

        #region "Get Method"
        public ResponseData GetMembership(string _user, string _merchant)
        {
            string CURRENT_URL = "v2/memberships/details?token="+TOKEN+"&user="+_user+"&merchant="+_merchant+"";
            var request = (HttpWebRequest)WebRequest.Create(BASE_URL + CURRENT_URL);
			request.Method = WebRequestMethods.Http.Get;

            return PopulatePrimaryMemberId(CoreAPI(request.Method, request));
        }

        public ResponseData MemberSuggestions(string _query, string _merchant)
        {
            string CURRENT_URL = "memberships/suggestions?token="+TOKEN+"&query="+_query+"&merchant="+_merchant+"";
            var request = (HttpWebRequest)WebRequest.Create(BASE_URL + CURRENT_URL);
            //request.Method = WebRequestMethods.Http.Get;

            return CoreAPI("GET", request);
        }

        public ResponseData ValidateVoucherCode(string _voucher_code, Int16 _store, string _identifier)
        {
            string CURRENT_URL = "vouchers/validate?token="+TOKEN+"&voucher_code="+_voucher_code+"&store="+_store.ToString()+"&user="+ _identifier+"";
            var request = (HttpWebRequest)WebRequest.Create(BASE_URL + CURRENT_URL);
            request.Method = WebRequestMethods.Http.Get;

            return CoreAPI(request.Method, request);
        }

        public ResponseData GetMembershipByCode(string _card_number, string _merchant)
        {
            string CURRENT_URL = "v2/memberships/user-by-card-number?token="+TOKEN+"&card_number="+_card_number+"&merchant="+_merchant+"";
            var request = (HttpWebRequest)WebRequest.Create(BASE_URL + CURRENT_URL);
            request.Method = WebRequestMethods.Http.Get;

            return PopulatePrimaryMemberId(CoreAPI(request.Method, request));
        }
        #endregion

        #region "Post Method"
        public ResponseData AddTransaction(string _user, Int16 _store, Int32 _stamps, string _invoiceNumber, long _totalValue, long _subTotal,
                                           string _employeeCode, int _numberOfPeople, Int32 _tax, string _createdDateTime, string _requireEmailNotification,
                                           List<Item> _items, List<ExtraData> _extra_data, List<StampsPayment> _payments)
        {
            const string CURRENT_URL = "v2/transactions/add";
            var request = (HttpWebRequest)WebRequest.Create(BASE_URL + CURRENT_URL);
            request.Method = WebRequestMethods.Http.Post;

            string JSON = new JavaScriptSerializer().Serialize(new
            {
                token = TOKEN,
                user = _user,
                store = _store,
                stamps = _stamps,
                invoice_number = _invoiceNumber,
                total_value = _totalValue,
                sub_total = _subTotal,
                employee_code = _employeeCode,
                number_of_people = _numberOfPeople,
                tax = _tax,
                created = _createdDateTime,
                require_email_notification = _requireEmailNotification,
                items = _items,
                extra_data = _extra_data,
                payments = _payments
            });
			
			ResponseData rd = CoreAPI(request.Method, request, JSON);
            try
            {
                var obj = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseAddTransaction>(rd.data.ToString());
                WriteResponse.ResponseApiStampsTransaction("I", obj.transaction.id, _invoiceNumber, System.DateTime.Now.ToString("s"), rd.status_code.ToString(), rd.status_message, rd.status_message, "F");
            }
            catch { }

            return rd;
        }

        public ResponseData AddTransactionWithRedeemptionVoucher(string _user, Int16 _store, Int32 _stamps, string _invoiceNumber, long _totalValue, long _subTotal,
                                           string _employeeCode, int _numberOfPeople, Int32 _tax, string _createdDateTime, string _requireEmailNotification,
                                           List<Item> _items, List<ExtraData> _extra_data, List<StampsPayment> _payments, List<VoucherRedeemption> _vouchers)
        {
            const string CURRENT_URL = "v2/transactions/add-with-redemptions";
            var request = (HttpWebRequest)WebRequest.Create(BASE_URL + CURRENT_URL);
            request.Method = WebRequestMethods.Http.Post;

            string JSON = new JavaScriptSerializer().Serialize(new
            {
                token = TOKEN,
                user = _user,
                store = _store,
                stamps = _stamps,
                invoice_number = _invoiceNumber,
                total_value = _totalValue,
                sub_total = _subTotal,
                employee_code = _employeeCode,
                number_of_people = _numberOfPeople,
                tax = _tax,
                created = _createdDateTime,
                require_email_notification = _requireEmailNotification,
                items = _items,
                extra_data = _extra_data,
                payments = _payments,
                voucher_redemptions = _vouchers
            });

            ResponseData rd = CoreAPI(request.Method, request, JSON);
            try
            {
                var obj = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseAddTransactionWithRedeemMultipleVoucherCode>(rd.data.ToString());
                WriteResponse.ResponseApiStampsTransaction("I", obj.transaction.id, _invoiceNumber, System.DateTime.Now.ToString("s"), rd.status_code.ToString(), rd.status_message, rd.status_message, "P");
            }
            catch { }

            return rd;
        }

        public ResponseData CancelTransaction(Int32 _id)
        {
            const string CURRENT_URL = "transactions/cancel";
            var request = (HttpWebRequest)WebRequest.Create(BASE_URL + CURRENT_URL);
            request.Method = WebRequestMethods.Http.Post;

            string JSON = new JavaScriptSerializer().Serialize(new
            {
                token = TOKEN,
                id = _id
            });
			
			ResponseData rd = CoreAPI(request.Method, request, JSON);
            try
            {
                var obj = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseCancelTransaction>(rd.data.ToString());
                WriteResponse.ResponseApiStampsTransaction("V", obj.transaction.id, "", System.DateTime.Now.ToString("s") , rd.status_code.ToString(), rd.status_message, rd.status_message, "F");
            }
            catch { }
            return rd;
        }

        public ResponseData ModifyTransaction(Int32 _id, Int32 _totalValue, Int32 _subTotal,
                                              Int32 _stamps, List<Item> _items)
        {
            const string CURRENT_URL = "v2/transactions/modify";
            var request = (HttpWebRequest)WebRequest.Create(BASE_URL + CURRENT_URL);
            request.Method = WebRequestMethods.Http.Post;

            string JSON = new JavaScriptSerializer().Serialize(new
            {
                token = TOKEN,
                id = _id,
                total_value = _totalValue,
                subtotal = _subTotal,
                stamps = _stamps,
                items = _items
            });

            return CoreAPI(request.Method, request, JSON);
        }

        public ResponseData CancelRedemptions(Int32 _id)
        {
            const string CURRENT_URL = "redemptions/cancel";
            var request = (HttpWebRequest)WebRequest.Create(BASE_URL + CURRENT_URL);
            request.Method = WebRequestMethods.Http.Post;

            string JSON = new JavaScriptSerializer().Serialize(new
            {
                token = TOKEN,
                id = _id
            });
			
			ResponseData rd = CoreAPI(request.Method, request, JSON);
            try
            {
                var obj = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseCancelRedemptions>(rd.data.ToString());
                WriteResponse.ResponseApiStampsVoucher("V", obj.redemption.id, "", "", "", "", System.DateTime.Now.ToString("s"), rd.status_code.ToString(), rd.status_message, rd.status_message, "F");
            }
            catch { }
            return rd;
        }

        public ResponseData RedeemVoucherCode(string _identifier, string _voucher_code, Int16 _store)
        {
            const string CURRENT_URL = "vouchers/redeem";
            var request = (HttpWebRequest)WebRequest.Create(BASE_URL + CURRENT_URL);
            request.Method = WebRequestMethods.Http.Post;

            string JSON = new JavaScriptSerializer().Serialize(new
            {
                token = TOKEN,
                identifier = _identifier,
                voucher_code = _voucher_code,
                store = _store
            });

            ResponseData rd = CoreAPI(request.Method, request, JSON);
            try
            {
                var obj = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseRedeemVoucherCode>(rd.data.ToString());
                WriteResponse.ResponseApiStampsVoucher("I", obj.redemption.id, _voucher_code, _voucher_code, "", "", System.DateTime.Now.ToString("s"), rd.status_code.ToString(), rd.status_message, rd.status_message, "F");
            }
            catch { }
            return rd;
        }
        #endregion

        #region "Logical Operation"
        private ResponseData PopulatePrimaryMemberId(ResponseData responseData)
        {
            // Prevent crashing populate primary member id when data is empty
            if (string.IsNullOrEmpty(responseData.data))
            {
                return responseData;
            }

            // Deserialized into object
            var objMembership = JsonConvert.DeserializeObject<STAMPS_API_RESPONSE.ResponseMembership>(responseData.data.ToString());

            string[] member_ids = new string[objMembership.user.member_ids.Length];

            // Duplicate data, and prevent sort original array
            Array.Copy(objMembership.user.member_ids, member_ids, member_ids.Length);

            // Sort alphabetic
            Array.Sort(member_ids);

            // Added field membership user
            STAMPS_API_RESPONSE.ResponseMembership.User user = objMembership.user;
            user.primary_member_id = member_ids[0];

            // Assign membership without changing
            STAMPS_API_RESPONSE.ResponseMembership.Membership membership = objMembership.membership;

            // Populate object membership
            STAMPS_API_RESPONSE.ResponseMembership responseMembership = new STAMPS_API_RESPONSE.ResponseMembership();
            responseMembership.user = user;
            responseMembership.membership = membership;

            // Re-serialized membership
            responseData.data = JsonConvert.SerializeObject(responseMembership);

            return responseData;
        }
        #endregion
    }
    
    #endregion
}