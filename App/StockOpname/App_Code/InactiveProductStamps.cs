using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

/// <summary>
/// Summary description for InactiveProductStamps
/// </summary>
public class InactiveProductStamps
{
    const string urladdtransaction = "https://stagingmember.cmkclub.com/api/webhooks/jaws/deactivate-variant";

    //POST
    public static string WebRequestPOST(string querystring)
    {
        //JSON result
        string result = "";
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        WebRequest req = WebRequest.Create(urladdtransaction);
        req.Method = WebRequestMethods.Http.Post;
        req.ContentType = "application/json";
        //req.Headers.Add("Authorization"); 
        req.Headers["Authorization"] = "token CyzOdMKGPLoUpTjGjvDyjjLP6mIVrw6Kfnvej5LK2w3J2XGGBz";
        //req.ContentLength = objBytes.Length;

        /*using (Stream stream = req.GetRequestStream())
        {
            stream.Write(objBytes, 0, objBytes.Length);
        }*/
		
		using (StreamWriter stream = new StreamWriter(req.GetRequestStream()))
        {
            stream.Write(querystring);
        }
		
		try
		{
			WebResponse res = req.GetResponse();
			HttpWebResponse hres = (HttpWebResponse)res;
			using (StreamReader sr = new StreamReader(hres.GetResponseStream()))
			{
				result = sr.ReadToEnd();
				if (!result.Contains("error_message"))
					InsertToDatabase(result);
			}
		}
		catch (WebException e)
        {
            using (WebResponse response = e.Response)
            {
                HttpWebResponse httpResponse = (HttpWebResponse) response;
                //Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    result = reader.ReadToEnd();
                    //Console.WriteLine(text);
					if (!result.Contains("error_message"))
                        InsertToDatabase(result);
                }
            }
        }
		
        return result;
    }
	
	public static void InsertToDatabase(string result) 
    {
        string query = "INSERT INTO STAMPS_API_DEACTIVE_VARIANT_RESPONSE(" +
            "MESSAGE_RESPONSE," +
            "TGL) VALUES('" + result + "', GETDATE())";

        Db.Execute(query, Db.Cnn_IN);
    }
}