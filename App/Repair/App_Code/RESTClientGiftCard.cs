using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Configuration;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for RESTClient
/// </summary>

public enum httpVerb
{
    GET,
    POST,
    PUT,
    DELETE
}

public class ResponseMsg
{
    public string code { get; set; }
    public string msg { get; set; }
}

public partial class RESTClient
{
    //protected static string xapidomain = "http://163.53.193.50/";
    protected static string xapidomain = "http://gcapi.cmk.co.id/";
    //protected static string xapidomain = "http://117.102.115.75:20002/";
    //protected static string xapidomain = "http://202.133.7.14:20002/";
    protected static string xapiurl = xapidomain + "api/hitapi/hitjaws";
    protected static string xapiclientid = "A0FCDE175F992222715E02555CD60F7A";
    protected static string xapiclientsecret = "A0FCDE175F992222D958C4ED9B625B2B";

    public string coderesult { get; set; }
    public string endPoint { get; set; }
    public httpVerb httpMethod { get; set; }

    public string Code { get; set; }
    public string Msg { get; set; }
    public string Client_id { get; set; }
    public string Client_secret { get; set; }

	public RESTClient()
	{
        endPoint = string.Empty;
        httpMethod = httpVerb.POST;

        Code = string.Empty;
        Msg = string.Empty;
        Client_id = string.Empty;
        Client_secret = string.Empty;

	}

    protected static string returnError(string xcode) { return xcode + " - " + "Technical Problem occured. Please contact IT Administrator."; }

    // Get Info Status Balance apakah saldo mencukupi atau tidak
    public static string isBalanceEnough(Literal z, string CardNumber, string AmountDeduction)
    {
        string status = "";
        string rcodevalid = "1010";
        string xcode = "1000";
        string xmsg = "";

        xmsg += msgEnc3Des(z, true, CardNumber);
        xmsg += msgEnc3Des(z, false, AmountDeduction);

        status = serialize3DesStr(z, xcode, xmsg, rcodevalid);

        if (status.Substring(0, 4) == xcode) { status = returnError(xcode); }

        return status;
    }

    // Get Info Status Balance apakah saldo mencukupi atau tidak
    public static string isCardNumberUIDValid(Literal z, string CardUid, string CardNumber)
    {
        //z.Text += "Test123123123123123123";

        string status = "";
        string rcodevalid = "2410";
        string xcode = "2400";
        string xmsg = "";

        xmsg += msgEnc3Des(z, true, CardUid);
        xmsg += msgEnc3Des(z, false, CardNumber);

        status = serialize3DesStr(z, xcode, xmsg, rcodevalid);

        if (status.Substring(0, 4) == xcode) { status = returnError(xcode); }

        //z.Text += "Test123123123123123123zxczxczxc";

        return status;
    }

    // Post Transaction Sales with AmountDeduction
    public static string postTransactionSale(Literal z, string InvoiceNumber, string PaymentId, string CardNumber, string Type, string AmountDeduction, string BranchId, string PaymentMethod, string UserCashier)
    {
        string status = "";
        string rcodevalid = "1310";
        string xcode = "1300";
        string xmsg = "";

        xmsg += msgEnc3Des(z, true, InvoiceNumber); //////////////
        xmsg += msgEnc3Des(z, false, PaymentId);
        xmsg += msgEnc3Des(z, false, CardNumber);
        xmsg += msgEnc3Des(z, false, Type);
        xmsg += msgEnc3Des(z, false, AmountDeduction);
        xmsg += msgEnc3Des(z, false, BranchId);
        xmsg += msgEnc3Des(z, false, PaymentMethod); //////////////
        xmsg += msgEnc3Des(z, false, UserCashier);

        status = serialize3DesStr(z, xcode, xmsg, rcodevalid);

        if (status.Substring(0, 4) == xcode) { status = returnError(xcode); }

        return status;
    }

    // Post Transaction Void with AmountDeduction
    public static string postTransactionVoid(Literal z, string InvoiceNumber, string PaymentId, string CardNumber, string Type, string AmountDeduction, string BranchId, string PaymentMethod, string UserCashier)
    {
        string status = "";
        string rcodevalid = "1410";
        string xcode = "1400";
        string xmsg = "";

        xmsg += msgEnc3Des(z, true, InvoiceNumber); //////////////
        xmsg += msgEnc3Des(z, false, PaymentId);
        xmsg += msgEnc3Des(z, false, CardNumber);
        xmsg += msgEnc3Des(z, false, Type);
        xmsg += msgEnc3Des(z, false, AmountDeduction);
        xmsg += msgEnc3Des(z, false, BranchId);
        xmsg += msgEnc3Des(z, false, PaymentMethod); //////////////
        xmsg += msgEnc3Des(z, false, UserCashier);

        status = serialize3DesStr(z, xcode, xmsg, rcodevalid);

        if (status.Substring(0, 4) == xcode) { status = returnError(xcode); }

        return status;
    }

    protected static bool serialize3Des(Literal z, string pcode, string pmsg, string pcodevalid)
    {
        System.Net.ServicePointManager.Expect100Continue = false;
        bool status = false;
        string strResponse = string.Empty;

        RESTClient rClient = new RESTClient();

        rClient.endPoint = xapiurl; // URL here
        rClient.Code = pcode;
        rClient.Msg = pmsg;
        rClient.Client_id = xapiclientid;
        rClient.Client_secret = xapiclientsecret;

        strResponse = ("[" + rClient.makeRequest(z) + "]");

        var jsserialize = new JavaScriptSerializer();
        var jsserializeResult = jsserialize.Deserialize<List<ResponseMsg>>(strResponse);

        // return must be 1 data set
        if (jsserializeResult.Count() == 1)
        {
            var x = jsserializeResult.First();

            pcode = x.code;
            pmsg = x.msg;
        }

        status = (pcode == pcodevalid) ? true : false;

        return status;
    }

    protected static string serialize3DesStr(Literal z, string pcode, string pmsg, string pcodevalid)
    {
        System.Net.ServicePointManager.Expect100Continue = false;
        string status = "";
        string strResponse = string.Empty;

        RESTClient rClient = new RESTClient();

        rClient.endPoint = xapiurl; // URL here
        rClient.Code = pcode;
        rClient.Msg = pmsg;
        rClient.Client_id = xapiclientid;
        rClient.Client_secret = xapiclientsecret;


        //z.Text += "z3 = " + xapiurl + "<br />";
        //z.Text += "z3 = " + pcode + "<br />";
        //z.Text += "z3 = " + pmsg + "<br />";
        //z.Text += "z3 = " + xapiclientid + "<br />";
        //z.Text += "z3 = " + xapiclientsecret + "<br />";


        strResponse = ("[" + rClient.makeRequest(z) + "]");

        var jsserialize = new JavaScriptSerializer();
        var jsserializeResult = jsserialize.Deserialize<List<ResponseMsg>>(strResponse);

        // return must be 1 data set
        if (jsserializeResult.Count() == 1)
        {
            var x = jsserializeResult.First();

            pcode = x.code;
            pmsg = x.msg;
        }

        status = pcode + " - " + pmsg;

        return status;
    }

    protected static string msgEnc3Des(Literal z, bool isFirstParameter, string msg)
    {
        string result = "";

        //z.Text += "z1 = " + msg + "<br />";

        result = (isFirstParameter ? "" : "#") + EncryptDecrypt.Encrypt3Des(z, msg);

        //z.Text += "z2 = " + EncryptDecrypt.Encrypt3Des(z, msg) + "<br />" + "<br />";

        return result;
    }

    public string makeRequest(Literal z)
    {
        System.Net.ServicePointManager.Expect100Continue = false;

        string strResponsiveValue = String.Empty;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);

        var postData = "code=" + Code;
        postData += "&msg=" + Msg;
        postData += "&client_id=" + Client_id;
        postData += "&client_secret=" + Client_secret;

        var data = Encoding.ASCII.GetBytes(postData);

        request.Method = httpMethod.ToString();
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;
        request.Expect = "application/json";

        try
        {
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException("error code: " + response.StatusCode);

                }

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponsiveValue = reader.ReadToEnd();
                        }
                        //end of streamreader
                    }
                }
                //end of responseStream
            }
            //end of using response

        }
        catch (Exception ex) 
        { 
            //z.Text += "mReq : " + ex.Message.ToString(); 
        }

        return strResponsiveValue;
    }

}