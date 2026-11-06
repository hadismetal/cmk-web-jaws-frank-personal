using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ImagePLU : System.Web.UI.Page
{
    protected string plu { get { return Request.QueryString["plu"]; } }
    protected string tipe { get { return Request.QueryString["tipe"]; } }

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
    protected void Page_Load(object sender, EventArgs e)
    {
        GetImageOld();
    }

    protected void GetImageOld()
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.CacheControl = "no-cache";

            string ContentType = string.Empty;
            if (plu != "")
            {
                var sp = db.StockProductDJ.SingleOrDefault(p => p.Nomor == plu);
                if (sp != null)
                {

                    string Ext = Upload.GetExt(sp.ImgPicture).ToLower();

                    if (Ext == ".png") ContentType = "image/png";
                    if (Ext == ".jpg") ContentType = "image/jpg";
                    if (Ext == ".jpeg") ContentType = "image/jpg";
                    if (Ext == ".bmp") ContentType = "image/bmp";

                    imgPLU.ImageUrl = sp.ImgPicture;

                }
                else
                {
                    imgPLU.ImageUrl = "";
                }
            }
            else
            {
                imgPLU.ImageUrl = "";
            }
            string apiUrl = "https://image-james.cmk.co.id/api/Image/Image64?img=" + imgPLU.ImageUrl;

            try
            {

                // Call API
                var request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "GET";

                string ext = Path.GetExtension(imgPLU.ImageUrl).ToLower();
                string contentType = "image/jpeg"; // default

                switch (ext)
                {
                    case ".jpg":
                    case ".jpeg":
                        contentType = "image/jpeg";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".bmp":
                        contentType = "image/bmp";
                        break;
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string apiResponse = reader.ReadToEnd().Trim();

                    // If it looks like JSON {"image":"..."} → extract base64
                    if (apiResponse.StartsWith("{"))
                    {
                        int start = apiResponse.IndexOf(":\"") + 2;
                        int end = apiResponse.LastIndexOf("\"");
                        apiResponse = apiResponse.Substring(start, end - start);
                    }

                    // If it has a prefix like data:image/jpeg;base64,... → strip it
                    if (apiResponse.StartsWith("data:"))
                    {
                        int commaIndex = apiResponse.IndexOf(",");
                        apiResponse = apiResponse.Substring(commaIndex + 1);
                    }

                    // Convert base64 → byte[]
                    byte[] bytes = Convert.FromBase64String(apiResponse);

                    // Send real image to browser
                    Response.Clear();
                    Response.ContentType = contentType; // change if PNG
                    Response.OutputStream.Write(bytes, 0, bytes.Length);
                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                Response.ContentType = "text/plain";
                Response.Write("Error: " + ex.Message);
            }
        }

        catch (Exception ex) { }
    }

    protected void GetImageNew()
    {
        if (string.IsNullOrWhiteSpace(plu)) return;

        string query = " IF(@TYPE = '' OR @TYPE = 'DJ') BEGIN SELECT ImgPicture FROM StockProductDJ WHERE Nomor = @PLU END ELSE BEGIN SELECT ImgPicture FROM StockProductPG WHERE Nomor = @PLU END";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@PLU", SqlDbType.VarChar) { Value = plu },
            new SqlParameter("@TYPE", SqlDbType.VarChar) { Value = tipe }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);
        string myPath = dt.Rows.Count > 0 ? dt.Rows[0]["ImgPicture"].ToString() : string.Empty;

        string baseUrl = ConfigurationManager.AppSettings["UrlImage"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            Response.StatusCode = 500;
            Response.ContentType = "text/plain";
            Response.Write("Image API URL is not configured.");
            return;
        }

        try
        {
            string filePath = myPath;

            // Example:
            // filePath = @"D:\ImagesMondial\StockReceiveDJ\OW00692WH.jpg";

            string ext = Path.GetExtension(filePath).ToLowerInvariant();

            string contentType;

            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;

                case ".png":
                    contentType = "image/png";
                    break;

                case ".bmp":
                    contentType = "image/bmp";
                    break;

                case ".gif":
                    contentType = "image/gif";
                    break;

                case ".webp":
                    contentType = "image/webp";
                    break;

                case ".pdf":
                    contentType = "application/pdf";
                    break;

                default:
                    Response.StatusCode = 400;
                    Response.ContentType = "text/plain";
                    Response.Write("Unsupported file type.");
                    return;
            }

            // Encode the physical path for use in query string
            string encodedPath = HttpUtility.UrlEncode(filePath);

            string url = baseUrl + encodedPath;

            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(url);

            request.Method = "GET";
            request.Timeout = 30000;
            request.ReadWriteTimeout = 30000;

            using (HttpWebResponse apiResponse =
                   (HttpWebResponse)request.GetResponse())
            {
                if (apiResponse.StatusCode != HttpStatusCode.OK)
                {
                    Response.StatusCode = (int)apiResponse.StatusCode;
                    Response.ContentType = "text/plain";
                    Response.Write("Unable to retrieve image.");
                    return;
                }

                Response.Clear();
                Response.Buffer = false;
                Response.ContentType = contentType;

                using (Stream input = apiResponse.GetResponseStream())
                {
                    byte[] buffer = new byte[81920];
                    int bytesRead;

                    while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        Response.OutputStream.Write(
                            buffer,
                            0,
                            bytesRead
                        );
                    }
                }

                Response.Flush();
            }
        }
        catch (WebException ex)
        {
            Response.Clear();
            Response.StatusCode = 404;
            Response.ContentType = "text/plain";
            Response.Write("Unable to retrieve image.");
        }
        catch (Exception)
        {
            Response.Clear();
            Response.StatusCode = 500;
            Response.ContentType = "text/plain";
            Response.Write("Error: Unable to process the image.");
        }
    }

    //Data Table Builder
    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = 240;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }
}