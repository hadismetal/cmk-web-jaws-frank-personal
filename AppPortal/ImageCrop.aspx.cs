using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ImageCrop : System.Web.UI.Page
{
    protected string myPath { get { return Request.QueryString["path"]; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        GetImageNew();
    }

    protected void GetImageOld()
    {
        string sSize = Request["Size"];

        int Size = 90;

        if (sSize != null)

            Size = Int32.Parse(sSize);



        string apiUrl = "https://image-james.cmk.co.id/api/Image?img=";
        string Path = apiUrl + myPath;

        Bitmap bmp = CreateThumbnail(Path, Size, Size);



        if (bmp == null)
        {

            this.ErrorResult();

            return;

        }



        string OutputFilename = null;

        OutputFilename = Request.QueryString["OutputFilename"];



        if (OutputFilename != null)
        {

            if (this.User.Identity.Name == "")
            {

                // *** Custom error display here

                bmp.Dispose();

                this.ErrorResult();

            }

            try
            {

                bmp.Save(OutputFilename);

            }

            catch (Exception ex)
            {

                bmp.Dispose();

                this.ErrorResult();

                return;

            }

        }



        // Put user code to initialize the page here

        Response.ContentType = "image/jpeg";

        bmp.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);

        bmp.Dispose();
    }

    protected void ErrorResult()
    {

        Response.Clear();
        Response.StatusCode = 404;
        Response.End();

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

        Bitmap bmp = null;

        try
        {
            string sSize = Request["Size"];

            int Size = 90;

            if (!string.IsNullOrWhiteSpace(sSize))
                Size = Int32.Parse(sSize);

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

                default:
                    Response.StatusCode = 400;
                    Response.ContentType = "text/plain";
                    Response.Write("Unsupported file type.");
                    return;
            }

            // Encode physical path for API
            string encodedPath = HttpUtility.UrlEncode(filePath);

            string url = baseUrl + encodedPath;

            // Get image from API and create thumbnail
            bmp = CreateThumbnail(url, Size, Size);

            if (bmp == null)
            {
                Response.StatusCode = 404;
                Response.ContentType = "text/plain";
                Response.Write("Unable to retrieve image.");
                return;
            }

            // -----------------------------------------
            // Continue processing Bitmap here
            // -----------------------------------------

            Response.Clear();
            Response.Buffer = false;
            Response.ContentType = contentType;

            // Return Bitmap to browser
            if (contentType == "image/jpeg")
            {
                bmp.Save(
                    Response.OutputStream,
                    ImageFormat.Jpeg
                );
            }
            else if (contentType == "image/png")
            {
                bmp.Save(
                    Response.OutputStream,
                    ImageFormat.Png
                );
            }
            else
            {
                // For thumbnail output, JPEG is usually simplest
                Response.ContentType = "image/jpeg";

                bmp.Save(
                    Response.OutputStream,
                    ImageFormat.Jpeg
                );
            }

            Response.Flush();
        }
        catch (WebException)
        {
            Response.Clear();
            Response.StatusCode = 404;
            Response.ContentType = "text/plain";
            Response.Write("Unable to retrieve image.");
        }
        catch (Exception ex)
        {
            Response.Clear();
            Response.StatusCode = 500;
            Response.ContentType = "text/plain";
            Response.Write("Error: " + ex.Message);
        }
        finally
        {
            if (bmp != null)
                bmp.Dispose();
        }
    }

    protected static Bitmap CreateThumbnail(string imageUrl, int lnWidth, int lnHeight)
    {
        Bitmap bmpOut = null;

        try
        {
            Bitmap loBMP;

            using (WebClient wc = new WebClient())
            {
                byte[] imageBytes = wc.DownloadData(imageUrl);

                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    loBMP = new Bitmap(ms);
                }
            }

            decimal lnRatio;
            int lnNewWidth = 0;
            int lnNewHeight = 0;

            if (loBMP.Width < lnWidth && loBMP.Height < lnHeight)
                return loBMP;

            if (loBMP.Width > loBMP.Height)
            {
                lnRatio = (decimal)lnWidth / loBMP.Width;
                lnNewWidth = lnWidth;
                lnNewHeight = (int)(loBMP.Height * lnRatio);
            }
            else
            {
                lnRatio = (decimal)lnHeight / loBMP.Height;
                lnNewHeight = lnHeight;
                lnNewWidth = (int)(loBMP.Width * lnRatio);
            }

            bmpOut = new Bitmap(lnNewWidth, lnNewHeight);

            using (Graphics g = Graphics.FromImage(bmpOut))
            {
                g.InterpolationMode =
                    System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.FillRectangle(
                    Brushes.White,
                    0,
                    0,
                    lnNewWidth,
                    lnNewHeight);

                g.DrawImage(
                    loBMP,
                    0,
                    0,
                    lnNewWidth,
                    lnNewHeight);
            }

            loBMP.Dispose();
        }
        catch
        {
            return null;
        }
        return bmpOut;
    }
}