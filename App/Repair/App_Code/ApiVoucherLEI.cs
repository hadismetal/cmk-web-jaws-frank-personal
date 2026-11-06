using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Globalization;

public class ApiVoucherLEI
{
    const string urlloginlei = "https://test-apiext.lakuemas.com/app/leicmk/login";
    const string urladdtransactionlei = "https://test-apiext.lakuemas.com/app/leicmk/salesorder";
    const string userlei = "cmk_jaws";
    const string passwordlei = "LeiCmkJawsVoucher2023$%#";

    public void AddTransactionLEI(int idso)
    {
        DataSet tb = PostTransaction(idso);
        DataTable tbheader = tb.Tables[0];
        DataTable tbdetail = tb.Tables[1];

        if (tbheader.Rows.Count == 1 && tbdetail.Rows.Count > 0)
        {
            if (Convert.ToInt32(tbheader.Rows[0]["Status"]) == 0)
            {
                long id = Convert.ToInt64(tbheader.Rows[0]["ID"]);
                string jsonlogin = JsonConvert.SerializeObject(new { username = userlei, password = passwordlei });
                string resultlogin = this.WebRequestPOST(urlloginlei, jsonlogin);
                ResponseLoginLEI objectlogin = JsonConvert.DeserializeObject<ResponseLoginLEI>(resultlogin);
                if (objectlogin.status == 1)
                {
                    string kodelokasi = tbheader.Rows[0]["KodeStore"].ToString();
                    string namalokasi = tbheader.Rows[0]["NamaStore"].ToString();
                    string noso = tbheader.Rows[0]["NoSalesOrder"].ToString();
                    DateTime tgl = Convert.ToDateTime(tbheader.Rows[0]["Tgl"]);
                    string kodecustomer = tbheader.Rows[0]["KodeCustomer"].ToString();
                    string namacustomer = tbheader.Rows[0]["NamaCustomer"].ToString();
                    int totalitem = Convert.ToInt32(tbheader.Rows[0]["TotalItem"]);

                    List<RequestSalesOrderItemLEI> rpgs = new List<RequestSalesOrderItemLEI>();
                    foreach (DataRow item in tbdetail.Rows)
                    {
                        RequestSalesOrderItemLEI rpg = new RequestSalesOrderItemLEI
                        {
                            barcode_number = item["BarcodeNumber"].ToString(),
                            no_invoice = item["Invoice"].ToString(),
                            kd_item = item["KodeItem"].ToString(),
                            nm_item = item["NamaItem"].ToString(),
                            total_value = Convert.ToInt64(item["TotalValue"])
                        };
                        rpgs.Add(rpg);
                    }
					

                    RequestSalesOrderLEI rs = new RequestSalesOrderLEI
                    {
                        no_sales_order = noso,
                        tgl_transaksi = tgl.ToString("yyyy-MM-dd hh:mm:ss", new CultureInfo("en-ID")),
                        kd_store = kodelokasi,
                        nm_store = namalokasi,
                        kd_customer = kodecustomer,
                        nm_customer = namacustomer,
                        total_item = totalitem,
                        item = rpgs
                    };
                    string json = JsonConvert.SerializeObject(rs);
                    string result = this.WebRequestPOSTBearer(urladdtransactionlei, objectlogin.data.token, json);
                    ResponseSalesOrderLEI objecttrans = JsonConvert.DeserializeObject<ResponseSalesOrderLEI>(result);
                    if (objecttrans.status == 1)
                    {
                        UpdateStatusTransaction(id, objecttrans.status, objecttrans.message, objecttrans.data.transaction_id);
                    }
                    else
                    {
                        UpdateStatusTransaction(id, objecttrans.status, objecttrans.message, "");
                    }
                }
                else
                {
                    UpdateStatusTransaction(id, objectlogin.status, objectlogin.message, "");
                }
            }
        }
    }

    public DataSet PostTransaction(int id)
    {
        string query = "EXEC sp_insert_voucher_lei_transaction_new " + id;
        DataSet ds = Ds(query, Db.Cnn_DB);
        return ds;
    }

    private DataSet Ds(string strSql, string CnnString)
    {
        SqlConnection sqlCnn = new SqlConnection(CnnString);
        SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSql, sqlCnn);
        sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
        DataSet objDS = new DataSet();
        sqlAdapter.Fill(objDS);
        sqlCnn.Close();

        return objDS;
    }

    public void UpdateStatusTransaction(long id, int status, string response, string transid)
    {
        string query = "sp_update_status_voucher_lei_transaction_new " + id + "," +
            " " + status + "," +
            " '" + response + "','" + transid + "'";
        Db.Execute(query, Db.Cnn_DB);
    }

    //POST
    private string WebRequestPOST(string url, string json = null)
    {
        //JSON result
        string result = "";
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        WebRequest req = WebRequest.Create(url);
        req.Method = WebRequestMethods.Http.Post;
        req.ContentType = "application/json";

        using (StreamWriter sw = new StreamWriter(req.GetRequestStream()))
        {
            sw.Write(json);
        }

        WebResponse res = req.GetResponse();
        HttpWebResponse hres = (HttpWebResponse)res;
        using (StreamReader sr = new StreamReader(hres.GetResponseStream()))
        {
            result = sr.ReadToEnd();
        }
        return result;
    }
	
	/*private string WebRequestPOST(string url, string json = null)
    {
		request.ContentType = "application/json";
		request.Method = "POST";
		request.Timeout = 3000;
		request.Headers.Add();
		
		string result = "";
		using (var streamWriter = new StreamWriter(request.GetRequestStream()))
		{
			streamWriter.Write(json);
		}

		using (var response = (HttpWebResponse)request.GetResponse())
		{
			result = new StreamReader(response.GetResponseStream()).ReadToEnd();
		}
		return result;
	}*/
	
	/*private string WebRequestPOSTBearer(string url, string json = null)
    {
		request.ContentType = "application/json";
		request.Method = "POST";
		request.Timeout = 3000;
		request.Headers.Add();
		
		string result = "";
		using (var streamWriter = new StreamWriter(request.GetRequestStream()))
		{
			streamWriter.Write(json);
		}

		using (var response = (HttpWebResponse)request.GetResponse())
		{
			result = new StreamReader(response.GetResponseStream()).ReadToEnd();
		}
		return result;
	}*/

    //POST
    private string WebRequestPOSTBearer(string url, string token, string json = null)
    {
        //JSON result
        string result = "";
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
        WebRequest req = WebRequest.Create(url);
        req.Method = WebRequestMethods.Http.Post;
        req.ContentType = "application/json";
        req.Headers.Add("Authorization", "Bearer " + token);

        using (StreamWriter sw = new StreamWriter(req.GetRequestStream()))
        {
            sw.Write(json);
        }

        WebResponse res = req.GetResponse();
        HttpWebResponse hres = (HttpWebResponse)res;
        using (StreamReader sr = new StreamReader(hres.GetResponseStream()))
        {
            result = sr.ReadToEnd();
        }
        return result;
    }

    private class RequestSalesOrderLEI
    {
        public string no_sales_order { get; set; }
        public string tgl_transaksi { get; set; }
        public string kd_store { get; set; }
        public string nm_store { get; set; }
        public string kd_customer { get; set; }
        public string nm_customer { get; set; }
        public int total_item { get; set; }
        public List<RequestSalesOrderItemLEI> item { get; set; }
    }

    private class RequestSalesOrderItemLEI
    {
        public string barcode_number { get; set; }
        public string no_invoice { get; set; }
        public string kd_item { get; set; }
        public string nm_item { get; set; }
        public double total_value { get; set; }
    }

    private class ResponseLoginLEI
    {
        public int status { get; set; }
        public string message { get; set; }
        public ResponseDataLoginLEI data { get; set; }
    }

    private class ResponseDataLoginLEI
    {
        public string token { get; set; }
    }

    private class ResponseSalesOrderLEI
    {
        public int status { get; set; }
        public string message { get; set; }
        public ResponseDataSalesOrderLEI data { get; set; }
    }

    private class ResponseDataSalesOrderLEI
    {
        public string transaction_id { get; set; }
    }
}