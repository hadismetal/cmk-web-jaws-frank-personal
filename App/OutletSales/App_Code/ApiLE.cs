using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for ApiLE
/// </summary>
public class ApiLE
{
    private static string urladdtransaction = "https://pos.lakuemas.com/api/jaws/push-data?";
    private static string urlgetcashboxcode = "https://pos.lakuemas.com/api/jaws/list-cashbox?";
    
    public static string AddTransactionLE(string customername, string customeremail, string customeraddress, string customerKTP, string customerhandphone, int bankid, string bankaccountnumber, string bankaccountname, string store, string keterangan, string cashboxcode, string thirdparty, string signaturekey, List<ItemPLU> items)
    {
        string result = "";
        AddTransaction at = new AddTransaction
        {
            customer_name = customername,
            customer_email = customeremail,
            customer_address = customeraddress,
            customer_ktp = customerKTP,
            customer_handphone = customerhandphone,
            bank_id = bankid,
            bank_account_number = bankaccountnumber,
            bank_account_name = bankaccountname,
            store_code = store,
            keterangan = keterangan,
            cashbox_code = cashboxcode,
            third_party = thirdparty,
            signature_key = signaturekey
        };
        string objectencoded = Newtonsoft.Json.JsonConvert.SerializeObject(at).Replace("{","").Replace("}","").Replace("\"", string.Empty).Replace(",","&").Replace(":","=");
        string url = urladdtransaction + objectencoded;
		
		string totalitemurl = "";
        int i = 0;
        foreach(ItemPLU item in items)
        {
            totalitemurl += "&item[" + i + "][plu_code]=" + item.plu_code + "&item[" + i + "][weight]=" + item.weight.ToString("#.##").Replace(".", "").Replace(",", ".") + "&item[" + i + "][jaws_price]=" + item.jaws_price.ToString("#.##").Replace(".", "").Replace(",", ".");
            i++;
        }
        url = url + totalitemurl;
		
        result = WebRequestPOST(url);
        return result;
    }

    public static string GetCashBox(string storecode, string thirdparty, string signaturekey)
    {
        string result = "";
        CashboxList cl = new CashboxList
        {
            store_code = storecode,
            third_party = thirdparty,
            signature_key = signaturekey
        };
        string objectencoded = Newtonsoft.Json.JsonConvert.SerializeObject(cl).Replace("{", "").Replace("}", "").Replace("\"", string.Empty).Replace(",", "&").Replace(":", "=");
        string url = urlgetcashboxcode + objectencoded;
        result = WebRequestPOST(url);
        return result;
    }
    public static string WebRequestGET(string url)
    {
        //JSON result
        string result = "";
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        WebRequest req = WebRequest.Create(url);
        req.ContentType = "application/json";
        req.Credentials = CredentialCache.DefaultCredentials;

        WebResponse res = req.GetResponse();
        string description = ((HttpWebResponse)res).StatusDescription;

        using (Stream dataStream = res.GetResponseStream())
        {
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            result = reader.ReadToEnd(); ;
        }

        return result;
    }
    //POST
    public static string WebRequestPOST(string url, string json = null)
    {
        //JSON result
        string result = "";
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        WebRequest req = WebRequest.Create(url);
        req.Method = WebRequestMethods.Http.Post;
        req.ContentType = "application/json";

        using (StreamWriter sw = new StreamWriter(req.GetRequestStream()))
        {
            string Json = json;
            sw.Write(Json);
        }

        WebResponse res = req.GetResponse();
        HttpWebResponse hres = (HttpWebResponse)res;
        using (StreamReader sr = new StreamReader(hres.GetResponseStream()))
        {
            result = sr.ReadToEnd();
        }
        return result;
    }

    public class AddTransaction
    {
        public string customer_name { get; set; }
        public string customer_email { get; set; }
        public string customer_address { get; set; }
        public string customer_ktp { get; set; }
        public string customer_handphone { get; set; }
        public int bank_id { get; set; }
        public string bank_account_number { get; set; }
        public string bank_account_name { get; set; }
        public string store_code { get; set; }
        public string plu_code { get; set; }
        public decimal weight { get; set; }
		public string keterangan { get;set; }
		public decimal jaws_price{ get;set; }
        public string cashbox_code { get; set; }
        public string third_party { get; set; }
        public string signature_key { get; set; }
    }
	
	public class ItemPLU
    {
        public string plu_code { get; set; }
        public decimal weight { get; set; }
        public decimal jaws_price { get; set; }
    }

    public class CashboxList
    {
        public string store_code { get; set; }
        public string third_party { get; set; }
        public string signature_key { get; set; }
    }

    public class ResponseCashboxList
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<ResponseCashboxListData> data { get; set; }
    }

    public class ResponseCashboxListData
    {
        public int id { get; set; }
        public string code { get; set; }
        public int? id_toko { get; set; }
        public int? id_toko_to { get; set; }
        public string status { get; set; }
        public DateTime created_at { get; set; }
    }

    public class ResponseAddTransaction
    {
        public string success { get; set; }
        public string message { get; set; }
		public DataTransaction data { get; set; }
    }
	
	public class DataTransaction
    {
        public string transaction_code { get; set; }
    }
	
	public static int MappingBank(int idbank)
    {
        int idbanklei = 1;
        switch (idbank)
        {
            case 1:
                idbanklei = 1; //BCA
                break;
            case 2:
                idbanklei = 2; //Mandiri
                break;
            case 14:
                idbanklei = 3; //Permata
                break;
            case 7:
                idbanklei = 4; //Danamon
                break;
            case 3:
                idbanklei = 5; //BNI
                break;
            case 13:
                idbanklei = 6; //BRI
                break;
            case 11:
                idbanklei = 7; //CIMB
                break;
            case 10:
                idbanklei = 8; //Mega
                break;
            case 18:
                idbanklei = 9; //NISP
                break;
            case 17:
                idbanklei = 10; //Panin
                break;
            case 23:
                idbanklei = 23; //Bank DKI
                break;
            default:
                idbanklei = 1; //
                break;
        }
        return idbanklei;
    }
}