using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
public partial class Images : System.Web.UI.Page
{
    protected string myPath { get { return Request.QueryString["path"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        GetImageNew();
    }

    protected void GetImageOld()
    {
        if (string.IsNullOrEmpty(myPath)) return;

        string apiUrl = "https://image-james.cmk.co.id/api/Image/Image64?img=" + myPath;

        try
        {

            // Call API
            var request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = "GET";

            string ext = Path.GetExtension(myPath).ToLower();
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

    protected void GetImageNew()
    {
        if (string.IsNullOrWhiteSpace(myPath))
            return;

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
}
