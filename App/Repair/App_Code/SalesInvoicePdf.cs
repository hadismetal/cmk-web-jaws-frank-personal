using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using System.IO;

/// <summary>
/// Summary description for SalesInvoicePdf
/// </summary>
public class SalesInvoicePdf
{
    public static string WriteSO(int ID) 
    {
        string query = "EXEC sp_print_sales_order " + ID;
        DataSet ds = QuerySPDataSet(query, Db.Cnn_DB);
        DataTable header = ds.Tables[0];
        DataTable item = ds.Tables[1];
        DataTable payment = ds.Tables[2];
		
		string brand = header.Rows[0]["BRAND"].ToString();

        PdfDocument doc = new PdfDocument();
        doc.Info.Title = header.Rows[0]["NOSO"].ToString();

        // Create an empty page
        PdfPage page = doc.AddPage();

        // Get an XGraphics object for drawing
        XGraphics gfx = XGraphics.FromPdfPage(page);

        // Create a TextFormatter
        XTextFormatter xtfleft = new XTextFormatter(gfx);
        xtfleft.Alignment = XParagraphAlignment.Left;
        XTextFormatter xtfright = new XTextFormatter(gfx);
        xtfright.Alignment = XParagraphAlignment.Right;
        XTextFormatter xtfcenter = new XTextFormatter(gfx);
        xtfcenter.Alignment = XParagraphAlignment.Center;
        XTextFormatter xtfjustify = new XTextFormatter(gfx);
        xtfjustify.Alignment = XParagraphAlignment.Justify;

        // Create a font
        XFont fontTitle = new XFont("Consolas", 20, XFontStyle.Bold);
        XFont fontParagraph = new XFont("Consolas", 12, XFontStyle.Regular);
        XFont fontParagraph2 = new XFont("Consolas", 16, XFontStyle.Regular);
        XFont fontParagraph3 = new XFont("Consolas", 8, XFontStyle.Regular);
        XFont fontParagraph3S = new XFont("Consolas", 7, XFontStyle.Regular);
        XFont fontParagraph4 = new XFont("Consolas", 8, XFontStyle.Bold);
        XFont fontParagraph5 = new XFont("Consolas", 8, XFontStyle.Italic);
        XFont fontParagraph6 = new XFont("Consolas", 8, XFontStyle.Underline);

        // Draw the text
        /*gfx.DrawString("Hello, World!", fontTitle, XBrushes.Black,
          new XRect(0, 0, page.Width, page.Height),
          XStringFormats.Center);*/

        // Create a div
        gfx.DrawString("INVOICE GABUNGAN", fontParagraph2, XBrushes.Black, new XPoint(20, 30));
        gfx.DrawString("PT. CENTRAL MEGA KENCANA", fontParagraph, XBrushes.Black, new XPoint(20, 50));
        gfx.DrawString("NPWP : 01.764.536.7-073.000", fontParagraph, XBrushes.Black, new XPoint(20, 65));

        XRect xr2 = new XRect(new XPoint(400, 20), new XPoint(580, 65));
        gfx.DrawRectangle(XBrushes.Transparent, xr2);
        xtfleft.DrawString(header.Rows[0]["STOREADDR"].ToString().Replace("  ",""), fontParagraph3S, XBrushes.Black, xr2, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.8), new XPoint(20, 70), new XPoint(580, 70));

        XRect xr3 = new XRect(new XPoint(20, 75), new XPoint(250, 85));
        gfx.DrawRectangle(XBrushes.Transparent, xr3);
        xtfleft.DrawString("No.                     : " + header.Rows[0]["NOSO"].ToString(), fontParagraph3, XBrushes.Black, xr3, XStringFormats.TopLeft);

        XRect xr4 = new XRect(new XPoint(20, 85), new XPoint(250, 95));
        gfx.DrawRectangle(XBrushes.Transparent, xr4);
        xtfleft.DrawString("Date / Tanggal Faktur   : " + Convert.ToDateTime(header.Rows[0]["TGLSO"]).ToString("dd-MM-yyyy"), fontParagraph3, XBrushes.Black, xr4, XStringFormats.TopLeft);

        XRect xr5 = new XRect(new XPoint(310, 75), new XPoint(580, 85));
        gfx.DrawRectangle(XBrushes.Transparent, xr5);
        xtfleft.DrawString("Customer Code / Kode Pelanggan  : " + header.Rows[0]["CUSTOMERCODE"].ToString(), fontParagraph3, XBrushes.Black, xr5, XStringFormats.TopLeft);

        XRect xr6 = new XRect(new XPoint(310, 85), new XPoint(580, 95));
        gfx.DrawRectangle(XBrushes.Transparent, xr6);
        xtfleft.DrawString("To / Kepada                     : " + header.Rows[0]["CUSTOMERNAME"].ToString(), fontParagraph3, XBrushes.Black, xr6, XStringFormats.TopLeft);

        XRect xr7 = new XRect(new XPoint(310, 95), new XPoint(580, 105));
        gfx.DrawRectangle(XBrushes.Transparent, xr7);
        xtfleft.DrawString("Given To / Diberikan Kepada     : " + (!string.IsNullOrEmpty(header.Rows[0]["CUSTOMERREFERENCENAME"].ToString()) ? header.Rows[0]["CUSTOMERREFERENCENAME"] : header.Rows[0]["CUSTOMERNAME"].ToString()), fontParagraph3, XBrushes.Black, xr7, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), new XPoint(20, 110), new XPoint(580, 110));

        XRect xr8 = new XRect(new XPoint(20, 110), new XPoint(100, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr8);
        xtfleft.DrawString("CODE", fontParagraph4, XBrushes.Black, xr8, XStringFormats.TopLeft);

        XRect xr9 = new XRect(new XPoint(105, 110), new XPoint(402, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr9);
        xtfleft.DrawString("ITEM & Description / Nama Barang & Keterangan", fontParagraph4, XBrushes.Black, xr9, XStringFormats.TopLeft);

        XRect xr10 = new XRect(new XPoint(403, 110), new XPoint(450, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr10);
        xtfcenter.DrawString("Qty", fontParagraph4, XBrushes.Black, xr10, XStringFormats.TopLeft);

        XRect xr11 = new XRect(new XPoint(455, 110), new XPoint(580, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr11);
        xtfright.DrawString("Price / Harga Jual (Rp) ", fontParagraph4, XBrushes.Black, xr11, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), new XPoint(20, 120), new XPoint(580, 120));
		
		int enter1 = 0;
        int enter = 0;
		foreach (DataRow dr in item.Rows)
        {
            //Item Desc
            XRect xr12 = new XRect(new XPoint(20, 120 + enter1), new XPoint(100, 130 + enter1));
            gfx.DrawRectangle(XBrushes.Transparent, xr12);
            xtfleft.DrawString(dr["KODE"].ToString(), fontParagraph3, XBrushes.Black, xr12, XStringFormats.TopLeft);

            XRect xr13 = new XRect(new XPoint(105, 120 + enter1), new XPoint(402, 130 + enter1));
            gfx.DrawRectangle(XBrushes.Transparent, xr13);
            xtfleft.DrawString(dr["NAMA"].ToString(), fontParagraph3, XBrushes.Black, xr13, XStringFormats.TopLeft);

            XRect xr14 = new XRect(new XPoint(403, 120 + enter1), new XPoint(450, 130 + enter1));
            gfx.DrawRectangle(XBrushes.Transparent, xr14);
            xtfcenter.DrawString(dr["QTY"].ToString(), fontParagraph3, XBrushes.Black, xr14, XStringFormats.TopLeft);

            XRect xr15 = new XRect(new XPoint(455, 120 + enter1), new XPoint(580, 130 + enter1));
            gfx.DrawRectangle(XBrushes.Transparent, xr15);
            xtfright.DrawString(Convert.ToInt32(dr["TOTALBAYAR"]).ToString("N0"), fontParagraph3, XBrushes.Black, xr15, XStringFormats.TopLeft);

            enter1 += 10;
        }
        enter = enter1 - 10;
        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 1), new XPoint(20, 130 + enter), new XPoint(580, 130 + enter));
		
		decimal TotalBayar = 0m;
		decimal RatePPN = (Convert.ToDateTime(header.Rows[0]["TGLSO"]) < Convert.ToDateTime("2022-04-01 00:00:00.000")) ? 1.02m : 1.022m;
		int PercentagePPN = (Convert.ToDateTime(header.Rows[0]["TGLSO"]) < Convert.ToDateTime("2022-04-01 00:00:00.000")) ? 10 : 11;
        foreach (DataRow dr in payment.Rows)
        {
            TotalBayar += Convert.ToDecimal(dr["NOMINAL"]);
        }
        decimal TotalBayarExcVAT = Math.Ceiling(TotalBayar / RatePPN);
        decimal PPN = TotalBayar - TotalBayarExcVAT;

        XRect xr16 = new XRect(new XPoint(20, 130 + enter), new XPoint(250, 170 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr16);
        xtfleft.DrawString("Say / Terbilang : " + Str.Terbilang(Convert.ToDecimal(TotalBayar.ToString("N2"))).TrimEnd().TrimStart(), fontParagraph3, XBrushes.Black, xr16, XStringFormats.TopLeft);

        XRect xr17 = new XRect(new XPoint(20, 180 + enter), new XPoint(250, 190 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr17);
        xtfleft.DrawString("Detail of payment", fontParagraph4, XBrushes.Black, xr17, XStringFormats.TopLeft);

        int enter2 = 0;
        foreach (DataRow dr in payment.Rows)
        {
            XRect xr17A = new XRect(new XPoint(20, 190 + enter + enter2), new XPoint(250, 200 + enter + enter2));
            gfx.DrawRectangle(XBrushes.Transparent, xr17A);
            xtfleft.DrawString(dr["PAYMENTNAME"].ToString() + " : " + Convert.ToDecimal(dr["NOMINAL"]).ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr17A, XStringFormats.TopLeft);
            enter2 += 10;
        }
		
		XRect xr17AA = new XRect(new XPoint(20, 200 + enter + enter2), new XPoint(250, 210 + enter + enter2));
        gfx.DrawRectangle(XBrushes.Transparent, xr17AA);
        xtfleft.DrawString("Detail of invoice", fontParagraph4, XBrushes.Black, xr17AA, XStringFormats.TopLeft);
		
		int enter3 = 0;
        foreach (DataRow dr in item.Rows)
        {
			if(dr["INVOICE"].ToString() != "")
			{
				XRect xr17AAA = new XRect(new XPoint(20, 210 + enter + enter2 + enter3), new XPoint(250, 220 + enter + enter2 + enter3));
				gfx.DrawRectangle(XBrushes.Transparent, xr17AAA);
				xtfleft.DrawString(dr["INVOICE"].ToString(), fontParagraph3, XBrushes.Black, xr17AAA, XStringFormats.TopLeft);
				enter3 += 10;
			}
        }
		
		XPoint[] xps = new XPoint[5] { new XPoint(400, 132 + enter), new XPoint(580, 132 + enter), new XPoint(580, 270 + enter), new XPoint(400, 270 + enter), new XPoint(400, 132 + enter) };
        gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);

        XRect xr19 = new XRect(new XPoint(403, 135 + enter), new XPoint(500, 165 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr19);
        xtfleft.DrawString("Price Exclude VAT / Harga Jual Tidak Termasuk PPN", fontParagraph3, XBrushes.Black, xr19, XStringFormats.TopLeft);

        XRect xr20 = new XRect(new XPoint(403, 175 + enter), new XPoint(500, 205 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr20);
        xtfleft.DrawString("VAT / PPN (" + PercentagePPN.ToString() + "% x 20% x Harga Jual Tidak Termasuk PPN )", fontParagraph3, XBrushes.Black, xr20, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.25), new XPoint(400, 210 + enter), new XPoint(580, 210 + enter));

        XRect xr21 = new XRect(new XPoint(403, 215 + enter), new XPoint(500, 225 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr21);
        xtfleft.DrawString("Total", fontParagraph3, XBrushes.Black, xr21, XStringFormats.TopLeft);

        XRect xr22 = new XRect(new XPoint(403, 235 + enter), new XPoint(500, 245 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr22);
        xtfleft.DrawString("Payment / Pembayaran", fontParagraph3, XBrushes.Black, xr22, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.25), new XPoint(400, 250 + enter), new XPoint(580, 250 + enter));

        XRect xr23 = new XRect(new XPoint(403, 255 + enter), new XPoint(500, 265 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr23);
        xtfleft.DrawString("Balance Due / Sisa", fontParagraph3, XBrushes.Black, xr23, XStringFormats.TopLeft);

        XRect xr24 = new XRect(new XPoint(505, 135 + enter), new XPoint(577, 165 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr24);
        xtfright.DrawString(TotalBayarExcVAT.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr24, XStringFormats.TopLeft);

        XRect xr25 = new XRect(new XPoint(505, 175 + enter), new XPoint(577, 205 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr25);
        xtfright.DrawString(PPN.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr25, XStringFormats.TopLeft);

        XRect xr26 = new XRect(new XPoint(505, 215 + enter), new XPoint(577, 225 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr26);
        xtfright.DrawString(TotalBayar.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr26, XStringFormats.TopLeft);

        XRect xr27 = new XRect(new XPoint(505, 235 + enter), new XPoint(577, 245 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr27);
        xtfright.DrawString(TotalBayar.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr27, XStringFormats.TopLeft);

        XRect xr28 = new XRect(new XPoint(505, 255 + enter), new XPoint(577, 265 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr28);
        xtfright.DrawString("0", fontParagraph3, XBrushes.Black, xr28, XStringFormats.TopLeft);

        XRect xr29 = new XRect(new XPoint(373, 350 + enter), new XPoint(470, 360 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr29);
        xtfcenter.DrawString(header.Rows[0]["CUSTOMERNAME"].ToString(), fontParagraph4, XBrushes.Black, xr29, XStringFormats.TopLeft);

        XRect xr30 = new XRect(new XPoint(475, 350 + enter), new XPoint(577, 360 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr30);
        xtfcenter.DrawString(header.Rows[0]["SALESNAME"].ToString(), fontParagraph4, XBrushes.Black, xr30, XStringFormats.TopLeft);

        XRect xr31 = new XRect(new XPoint(373, 360 + enter), new XPoint(470, 370 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr31);
        xtfcenter.DrawString("(Customer / Pelanggan)", fontParagraph3, XBrushes.Black, xr31, XStringFormats.TopLeft);

        XRect xr32 = new XRect(new XPoint(475, 360 + enter), new XPoint(577, 370 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr32);
        xtfcenter.DrawString("(JR / Staff Penjualan)", fontParagraph3, XBrushes.Black, xr32, XStringFormats.TopLeft);
		
		WriteNotes(gfx, fontParagraph3, xtfleft, enter, brand);
		WriteDeclaration(gfx, fontParagraph3, xtfleft, enter);
		
		// Save the document...190116
        string filename = header.Rows[0]["NOSO"].ToString().Replace("/", "-") + ".pdf";
        string foldername = "D:/Web/uploadjaws/frank/so";
        if (!Directory.Exists(foldername))
        {
            Directory.CreateDirectory(foldername);
        }

        string SaveLocation = foldername + "/" + filename;
        if (File.Exists(SaveLocation))
        {
            File.Delete(SaveLocation);
        }

        doc.Save(SaveLocation);
        // ...and start a viewer.
        //System.Diagnostics.Process.Start(SaveLocation);
		return SaveLocation;
    }
	
	public static string WriteInvoiceDJ(int IDSalesDetail)
	{
        string query = "EXEC sp_print_sales_detail " + IDSalesDetail + ", 'DJ'";
        DataTable header = Db.Rs(query, Db.Cnn_DB);
		
		string query2 = "EXEC sp_get_asp_new 1," + header.Rows[0]["IDPRODUCT"].ToString();
		DataTable asp = Db.Rs(query2, Db.Cnn_DB);
		
		string brand = header.Rows[0]["BRAND"].ToString();
		
		PdfDocument doc = new PdfDocument();
        doc.Info.Title = header.Rows[0]["INVOICE"].ToString();

        // Create an empty page
        PdfPage page = doc.AddPage();

        // Get an XGraphics object for drawing
        XGraphics gfx = XGraphics.FromPdfPage(page);

        // Create a TextFormatter
        XTextFormatter xtfleft = new XTextFormatter(gfx);
        xtfleft.Alignment = XParagraphAlignment.Left;
        XTextFormatter xtfright = new XTextFormatter(gfx);
        xtfright.Alignment = XParagraphAlignment.Right;
        XTextFormatter xtfcenter = new XTextFormatter(gfx);
        xtfcenter.Alignment = XParagraphAlignment.Center;
        XTextFormatter xtfjustify = new XTextFormatter(gfx);
        xtfjustify.Alignment = XParagraphAlignment.Justify;

        // Create a font
        XFont fontTitle = new XFont("Consolas", 20, XFontStyle.Bold);
        XFont fontParagraph = new XFont("Consolas", 12, XFontStyle.Regular);
        XFont fontParagraph2 = new XFont("Consolas", 16, XFontStyle.Regular);
        XFont fontParagraph3 = new XFont("Consolas", 8, XFontStyle.Regular);
        XFont fontParagraph3S = new XFont("Consolas", 7, XFontStyle.Regular);
        XFont fontParagraph4 = new XFont("Consolas", 8, XFontStyle.Bold);
        XFont fontParagraph5 = new XFont("Consolas", 8, XFontStyle.Italic);
        XFont fontParagraph6 = new XFont("Consolas", 8, XFontStyle.Underline);

        // Draw the text
        /*gfx.DrawString("Hello, World!", fontTitle, XBrushes.Black,
          new XRect(0, 0, page.Width, page.Height),
          XStringFormats.Center);*/

        // Create a div
        gfx.DrawString("INVOICE GABUNGAN", fontParagraph2, XBrushes.Black, new XPoint(20, 30));
        gfx.DrawString("PT. CENTRAL MEGA KENCANA", fontParagraph, XBrushes.Black, new XPoint(20, 50));
        gfx.DrawString("NPWP : 01.764.536.7-073.000", fontParagraph, XBrushes.Black, new XPoint(20, 65));

        XRect xr2 = new XRect(new XPoint(400, 20), new XPoint(580, 65));
        gfx.DrawRectangle(XBrushes.Transparent, xr2);
        xtfleft.DrawString(header.Rows[0]["STOREADDR"].ToString(), fontParagraph3S, XBrushes.Black, xr2, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.8), new XPoint(20, 70), new XPoint(580, 70));

        XRect xr3 = new XRect(new XPoint(20, 75), new XPoint(250, 85));
        gfx.DrawRectangle(XBrushes.Transparent, xr3);
        xtfleft.DrawString("No.                     : " + header.Rows[0]["INVOICE"].ToString(), fontParagraph3, XBrushes.Black, xr3, XStringFormats.TopLeft);

        XRect xr4 = new XRect(new XPoint(20, 85), new XPoint(250, 95));
        gfx.DrawRectangle(XBrushes.Transparent, xr4);
        xtfleft.DrawString("Date / Tanggal Faktur   : " + Convert.ToDateTime(header.Rows[0]["TGLSO"]).ToString("dd-MM-yyyy"), fontParagraph3, XBrushes.Black, xr4, XStringFormats.TopLeft);

        XRect xr5 = new XRect(new XPoint(310, 75), new XPoint(580, 85));
        gfx.DrawRectangle(XBrushes.Transparent, xr5);
        xtfleft.DrawString("Customer Code / Kode Pelanggan  : " + header.Rows[0]["CUSTOMERCODE"].ToString(), fontParagraph3, XBrushes.Black, xr5, XStringFormats.TopLeft);

        XRect xr6 = new XRect(new XPoint(310, 85), new XPoint(580, 95));
        gfx.DrawRectangle(XBrushes.Transparent, xr6);
        xtfleft.DrawString("To / Kepada                     : " + header.Rows[0]["CUSTOMERNAME"].ToString(), fontParagraph3, XBrushes.Black, xr6, XStringFormats.TopLeft);

        XRect xr7 = new XRect(new XPoint(310, 95), new XPoint(580, 105));
        gfx.DrawRectangle(XBrushes.Transparent, xr7);
        xtfleft.DrawString("Given To / Diberikan Kepada     : " + (!string.IsNullOrEmpty(header.Rows[0]["CUSTOMERREFERENCENAME"].ToString()) ? header.Rows[0]["CUSTOMERREFERENCENAME"] : header.Rows[0]["CUSTOMERNAME"].ToString()), fontParagraph3, XBrushes.Black, xr7, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), new XPoint(20, 110), new XPoint(580, 110));

        XRect xr8 = new XRect(new XPoint(20, 110), new XPoint(100, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr8);
        xtfleft.DrawString("CODE", fontParagraph4, XBrushes.Black, xr8, XStringFormats.TopLeft);

        XRect xr9 = new XRect(new XPoint(105, 110), new XPoint(402, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr9);
        xtfleft.DrawString("ITEM & Description / Nama Barang & Keterangan", fontParagraph4, XBrushes.Black, xr9, XStringFormats.TopLeft);

        XRect xr10 = new XRect(new XPoint(403, 110), new XPoint(450, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr10);
        xtfcenter.DrawString("Qty", fontParagraph4, XBrushes.Black, xr10, XStringFormats.TopLeft);

        XRect xr11 = new XRect(new XPoint(455, 110), new XPoint(580, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr11);
        xtfright.DrawString("Price / Harga Jual (Rp) ", fontParagraph4, XBrushes.Black, xr11, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), new XPoint(20, 120), new XPoint(580, 120));
		
		//Item Desc
		XRect xr12 = new XRect(new XPoint(20, 120), new XPoint(100, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr12);
		xtfleft.DrawString(header.Rows[0]["KODE"].ToString(), fontParagraph3, XBrushes.Black, xr12, XStringFormats.TopLeft);

		XRect xr13 = new XRect(new XPoint(105, 120), new XPoint(402, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr13);
		xtfleft.DrawString(header.Rows[0]["NAMA"].ToString(), fontParagraph3, XBrushes.Black, xr13, XStringFormats.TopLeft);

		XRect xr14 = new XRect(new XPoint(403, 120), new XPoint(450, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr14);
		xtfcenter.DrawString(header.Rows[0]["QTY"].ToString(), fontParagraph3, XBrushes.Black, xr14, XStringFormats.TopLeft);

		XRect xr15 = new XRect(new XPoint(455, 120), new XPoint(580, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr15);
		xtfright.DrawString(Convert.ToInt32(header.Rows[0]["TOTALBAYAR"]).ToString("N0"), fontParagraph3, XBrushes.Black, xr15, XStringFormats.TopLeft);
		
		gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 1), new XPoint(20, 130), new XPoint(580, 130));
		
		XRect xr16 = new XRect(new XPoint(20, 130), new XPoint(250, 170));
        gfx.DrawRectangle(XBrushes.Transparent, xr16);
        xtfleft.DrawString("Say / Terbilang : " + Str.Terbilang(Convert.ToDecimal(header.Rows[0]["TOTALBAYAR"])).TrimEnd().TrimStart(), fontParagraph3, XBrushes.Black, xr16, XStringFormats.TopLeft);
		
		XPoint[] xps = new XPoint[5] { new XPoint(400, 132), new XPoint(580, 132), new XPoint(580, 270), new XPoint(400, 270), new XPoint(400, 132) };
        gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
		
		decimal RatePPN = (Convert.ToDateTime(header.Rows[0]["TGLSO"]) < Convert.ToDateTime("2022-04-01 00:00:00.000")) ? 1.02m : 1.022m;
		int PercentagePPN = (Convert.ToDateTime(header.Rows[0]["TGLSO"]) < Convert.ToDateTime("2022-04-01 00:00:00.000")) ? 10 : 11;
		decimal TotalBayar = Convert.ToDecimal(header.Rows[0]["TOTALBAYAR"]);
        decimal TotalBayarExcVAT = Math.Ceiling(TotalBayar / RatePPN);
        decimal PPN = TotalBayar - TotalBayarExcVAT;

        XRect xr19 = new XRect(new XPoint(403, 135), new XPoint(500, 165));
        gfx.DrawRectangle(XBrushes.Transparent, xr19);
        xtfleft.DrawString("Price Exclude VAT / Harga Jual Tidak Termasuk PPN", fontParagraph3, XBrushes.Black, xr19, XStringFormats.TopLeft);

        XRect xr20 = new XRect(new XPoint(403, 175), new XPoint(500, 205));
        gfx.DrawRectangle(XBrushes.Transparent, xr20);
        xtfleft.DrawString("VAT / PPN (" + PercentagePPN.ToString() + "% x 20% x Harga Jual Tidak Termasuk PPN )", fontParagraph3, XBrushes.Black, xr20, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.25), new XPoint(400, 210), new XPoint(580, 210));

        XRect xr21 = new XRect(new XPoint(403, 215), new XPoint(500, 225));
        gfx.DrawRectangle(XBrushes.Transparent, xr21);
        xtfleft.DrawString("Total", fontParagraph3, XBrushes.Black, xr21, XStringFormats.TopLeft);

        XRect xr22 = new XRect(new XPoint(403, 235), new XPoint(500, 245));
        gfx.DrawRectangle(XBrushes.Transparent, xr22);
        xtfleft.DrawString("Payment / Pembayaran", fontParagraph3, XBrushes.Black, xr22, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.25), new XPoint(400, 250), new XPoint(580, 250));

        XRect xr23 = new XRect(new XPoint(403, 255), new XPoint(500, 265));
        gfx.DrawRectangle(XBrushes.Transparent, xr23);
        xtfleft.DrawString("Balance Due / Sisa", fontParagraph3, XBrushes.Black, xr23, XStringFormats.TopLeft);

        XRect xr24 = new XRect(new XPoint(505, 135), new XPoint(577, 165));
        gfx.DrawRectangle(XBrushes.Transparent, xr24);
        xtfright.DrawString(TotalBayarExcVAT.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr24, XStringFormats.TopLeft);

        XRect xr25 = new XRect(new XPoint(505, 175 ), new XPoint(577, 205));
        gfx.DrawRectangle(XBrushes.Transparent, xr25);
        xtfright.DrawString(PPN.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr25, XStringFormats.TopLeft);

        XRect xr26 = new XRect(new XPoint(505, 215), new XPoint(577, 225));
        gfx.DrawRectangle(XBrushes.Transparent, xr26);
        xtfright.DrawString(TotalBayar.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr26, XStringFormats.TopLeft);

        XRect xr27 = new XRect(new XPoint(505, 235), new XPoint(577, 245));
        gfx.DrawRectangle(XBrushes.Transparent, xr27);
        xtfright.DrawString(TotalBayar.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr27, XStringFormats.TopLeft);

        XRect xr28 = new XRect(new XPoint(505, 255 ), new XPoint(577, 265));
        gfx.DrawRectangle(XBrushes.Transparent, xr28);
        xtfright.DrawString("0", fontParagraph3, XBrushes.Black, xr28, XStringFormats.TopLeft);

        XRect xr29 = new XRect(new XPoint(373, 350), new XPoint(470, 360));
        gfx.DrawRectangle(XBrushes.Transparent, xr29);
        xtfcenter.DrawString(header.Rows[0]["CUSTOMERNAME"].ToString(), fontParagraph4, XBrushes.Black, xr29, XStringFormats.TopLeft);

        XRect xr30 = new XRect(new XPoint(475, 350), new XPoint(577, 360));
        gfx.DrawRectangle(XBrushes.Transparent, xr30);
        xtfcenter.DrawString(header.Rows[0]["SALESNAME"].ToString(), fontParagraph4, XBrushes.Black, xr30, XStringFormats.TopLeft);

        XRect xr31 = new XRect(new XPoint(373, 360), new XPoint(470, 370));
        gfx.DrawRectangle(XBrushes.Transparent, xr31);
        xtfcenter.DrawString("(Customer / Pelanggan)", fontParagraph3, XBrushes.Black, xr31, XStringFormats.TopLeft);

        XRect xr32 = new XRect(new XPoint(475, 360), new XPoint(577, 370));
        gfx.DrawRectangle(XBrushes.Transparent, xr32);
        xtfcenter.DrawString("(JR / Staff Penjualan)", fontParagraph3, XBrushes.Black, xr32, XStringFormats.TopLeft);
		
		string custnama1 = header.Rows[0]["CUSTOMERNAME"].ToString();
		string custrefnama1 = header.Rows[0]["CUSTOMERREFERENCENAME"].ToString();
		WriteASPDJ(brand, custnama1, custrefnama1, asp, gfx, fontParagraph3, xtfleft);
		
		xtfleft.Alignment = XParagraphAlignment.Left;
		WriteNotesInvoice(gfx, fontParagraph3, xtfleft, brand);
		WriteDeclarationInvoice(gfx, fontParagraph3, xtfleft);
		
		// Save the document...190116
        string filename = header.Rows[0]["INVOICE"].ToString().Replace("/", "-") + ".pdf";
        string foldername = "D:/Web/uploadjaws/frank/dj";
        if (!Directory.Exists(foldername))
        {
            Directory.CreateDirectory(foldername);
        }

        string SaveLocation = foldername + "/" + filename;
        if (File.Exists(SaveLocation))
        {
            File.Delete(SaveLocation);
        }

        doc.Save(SaveLocation);
        // ...and start a viewer.
        //System.Diagnostics.Process.Start(SaveLocation);
		return SaveLocation;
	}
	
	protected static void WriteASPDJ(string namabrand, string custnama, string custrefnama, DataTable asp, XGraphics gfx, XFont fontParagraph, XTextFormatter xtf)
	{
		string namaasp = asp.Rows[0]["NAMA"].ToString();
		if((custrefnama != "CMK CLUB" && custnama != "CMK CLUB"))
		{
			if(namaasp != "")
			{
				XRect xr1 = new XRect(new XPoint(20, 385), new XPoint(250, 395));
				gfx.DrawRectangle(XBrushes.Transparent, xr1);
				xtf.DrawString("After Sales Policy / Kebijakan Penjualan", fontParagraph, XBrushes.Black, xr1, XStringFormats.TopLeft);
				
				xtf.Alignment = XParagraphAlignment.Center;
				
				XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 485), new XPoint(20, 485), new XPoint(20, 395) };
				gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
				
				XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(150, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr2);
				xtf.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

				XRect xr3 = new XRect(new XPoint(151, 395), new XPoint(420, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr3);
				xtf.DrawString("TRADE IN / TUKAR TAMBAH", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);

				XRect xr4 = new XRect(new XPoint(421, 395), new XPoint(580, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr4);
				xtf.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);

				XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);
				
				XPoint[] xps3 = new XPoint[2] { new XPoint(150, 395), new XPoint(150, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

				XPoint[] xps4 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);

				XPoint[] xps5 = new XPoint[2] { new XPoint(20, 425), new XPoint(580, 425) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps5[0], xps5[1]);

				XPoint[] xps6 = new XPoint[2] { new XPoint(20, 435), new XPoint(580, 435) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps6[0], xps6[1]);

				XRect xr6 = new XRect(new XPoint(21, 415), new XPoint(150, 425));
				gfx.DrawRectangle(XBrushes.Transparent, xr6);
				xtf.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr6, XStringFormats.TopLeft);

				XRect xr7 = new XRect(new XPoint(151, 415), new XPoint(420, 425));
				gfx.DrawRectangle(XBrushes.Transparent, xr7);
				xtf.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr7, XStringFormats.TopLeft);

				XRect xr8 = new XRect(new XPoint(421, 415), new XPoint(580, 425));
				gfx.DrawRectangle(XBrushes.Transparent, xr8);
				xtf.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr8, XStringFormats.TopLeft);

				XPoint[] xps7 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps7[0], xps7[1]);

				XFont fontParagraphsmall = new XFont("Consolas", 6.5, XFontStyle.Regular);
				
				int rowscount = 0;
				
				string idasp = asp.Rows[0][0].ToString();
				int measpperiodmax = Convert.ToInt32(asp.Rows[0][4]);
				int measprumus = Convert.ToInt32(asp.Rows[0][5]);
				
				int ti1aspperiodmax = Convert.ToInt32(asp.Rows[0][7]);
				int ti1asprumus = Convert.ToInt32(asp.Rows[0][8]);
				
				int ti2aspperiodmax = Convert.ToInt32(asp.Rows[0][10]);
				int ti2asprumus = Convert.ToInt32(asp.Rows[0][11]);
				
				int ti3aspperiodmax = Convert.ToInt32(asp.Rows[0][10]);
				int ti3asprumus = Convert.ToInt32(asp.Rows[0][14]);
				
				int r1aspperiodmax = Convert.ToInt32(asp.Rows[0][16]);
				int r1asprumus = Convert.ToInt32(asp.Rows[0][17]);
				
				int r2aspperiodmax = Convert.ToInt32(asp.Rows[0][16]);
				int r2asprumus = Convert.ToInt32(asp.Rows[0][20]);
				
				int memchasprumus = Convert.ToInt32(asp.Rows[0][21]);
				
				string me = "", ti1 = "", ti2 = "", ti3 = "", rs1 = "", rs2 = "";
				string me_rumus1 = "", ti_rumus1 = "", ti_rumus2 = "", ti_rumus3 = "", rs_rumus1 = "", rs_rumus2 = "";
					
				if (memchasprumus != 0)
				{
					me = asp_text_period("<", memchasprumus, "d");
				}
				else
				{
					me = asp_text_period("<", measpperiodmax, "m");
				}
				
				if(ti1aspperiodmax < 12)
					ti1 = asp_text_period("<", ti1aspperiodmax, "m");
				else
					ti1 = asp_text_period("<", ti1aspperiodmax / 12, "y");
				ti2 = asp_text_period("<", ti2aspperiodmax / 12, "y");
				ti3 = asp_text_period(">", ti3aspperiodmax / 12, "y");
				rs1 = asp_text_period("<", r1aspperiodmax / 12, "y");
				rs2 = asp_text_period(">", r2aspperiodmax / 12, "y");

				me_rumus1 = asp_text_value(namabrand, "me", measprumus);
				ti_rumus1 = asp_text_value(namabrand, "ti", ti1asprumus, false);
				ti_rumus2 = asp_text_value(namabrand, "ti", ti2asprumus, false);
				ti_rumus3 = asp_text_value(namabrand, "ti", ti3asprumus, true);
				rs_rumus1 = asp_text_value(namabrand, "rs", r1asprumus, false);
				rs_rumus2 = asp_text_value(namabrand, "rs", r2asprumus, true);

				XRect xr9 = new XRect(new XPoint(21, 426), new XPoint(150, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr9);
				xtf.DrawString(me, fontParagraphsmall, XBrushes.Black, xr9, XStringFormats.TopLeft);

				XRect xr10 = new XRect(new XPoint(151, 426), new XPoint(240, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr10);
				xtf.DrawString(ti1, fontParagraphsmall, XBrushes.Black, xr10, XStringFormats.TopLeft);

				XRect xr11 = new XRect(new XPoint(241, 426), new XPoint(331, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr11);
				xtf.DrawString(ti2, fontParagraphsmall, XBrushes.Black, xr11, XStringFormats.TopLeft);

				XRect xr12 = new XRect(new XPoint(332, 426), new XPoint(420, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr12);
				xtf.DrawString(ti3, fontParagraphsmall, XBrushes.Black, xr12, XStringFormats.TopLeft);
				
				XPoint[] xps8 = new XPoint[2] { new XPoint(241, 425), new XPoint(241, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps8[0], xps8[1]);

				XPoint[] xps9 = new XPoint[2] { new XPoint(331, 425), new XPoint(331, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps9[0], xps9[1]);

				XRect xr13 = new XRect(new XPoint(421, 426), new XPoint(500, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr13);
				xtf.DrawString(rs1, fontParagraphsmall, XBrushes.Black, xr13, XStringFormats.TopLeft);

				XRect xr14 = new XRect(new XPoint(501, 426), new XPoint(580, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr14);
				xtf.DrawString(rs2, fontParagraphsmall, XBrushes.Black, xr14, XStringFormats.TopLeft);
				
				XPoint[] xps10 = new XPoint[2] { new XPoint(501, 425), new XPoint(501, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps10[0], xps10[1]);
				
				XRect xr15 = new XRect(new XPoint(21, 440), new XPoint(150, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr15);
				xtf.DrawString(me_rumus1, fontParagraphsmall, XBrushes.Black, xr15, XStringFormats.TopLeft);

				XRect xr16 = new XRect(new XPoint(151, 440), new XPoint(240, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr16);
				xtf.DrawString(ti_rumus1, fontParagraphsmall, XBrushes.Black, xr16, XStringFormats.TopLeft);
				
				XRect xr17 = new XRect(new XPoint(241, 440), new XPoint(331, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr17);
				xtf.DrawString(ti_rumus2, fontParagraphsmall, XBrushes.Black, xr17, XStringFormats.TopLeft);

				XRect xr18 = new XRect(new XPoint(332, 440), new XPoint(420, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr18);
				xtf.DrawString(ti_rumus3, fontParagraphsmall, XBrushes.Black, xr18, XStringFormats.TopLeft);
				
				XRect xr19 = new XRect(new XPoint(421, 440), new XPoint(500, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr19);
				xtf.DrawString(rs_rumus1, fontParagraphsmall, XBrushes.Black, xr19, XStringFormats.TopLeft);

				XRect xr20 = new XRect(new XPoint(501, 440), new XPoint(580, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr20);
				xtf.DrawString(rs_rumus2, fontParagraphsmall, XBrushes.Black, xr20, XStringFormats.TopLeft);
			}
			else
			{
				XRect xr1 = new XRect(new XPoint(20, 385), new XPoint(250, 395));
				gfx.DrawRectangle(XBrushes.Transparent, xr1);
				xtf.DrawString("After Sales Policy / Kebijakan Penjualan", fontParagraph, XBrushes.Black, xr1, XStringFormats.TopLeft);
				
				xtf.Alignment = XParagraphAlignment.Center;
				
				XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 485), new XPoint(20, 485), new XPoint(20, 395) };
				gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
				
				XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(150, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr2);
				xtf.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

				XRect xr3 = new XRect(new XPoint(151, 395), new XPoint(420, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr3);
				xtf.DrawString("TRADE IN / TUKAR TAMBAH", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);

				XRect xr4 = new XRect(new XPoint(421, 395), new XPoint(580, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr4);
				xtf.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);
				
				XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);
				
				XPoint[] xps3 = new XPoint[2] { new XPoint(150, 395), new XPoint(150, 415) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

				XPoint[] xps4 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 415) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);
				
				XRect xr5 = new XRect(new XPoint(100, 440), new XPoint(480, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr5);
				xtf.DrawString(asp.Rows[0]["KETERANGAN"].ToString(), fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
			}
		}
		else
		{
			XRect xr1 = new XRect(new XPoint(20, 385), new XPoint(250, 395));
			gfx.DrawRectangle(XBrushes.Transparent, xr1);
			xtf.DrawString("After Sales Policy / Kebijakan Penjualan", fontParagraph, XBrushes.Black, xr1, XStringFormats.TopLeft);
			
			xtf.Alignment = XParagraphAlignment.Center;
			
			XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 485), new XPoint(20, 485), new XPoint(20, 395) };
			gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
			
			XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(150, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr2);
			xtf.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

			XRect xr3 = new XRect(new XPoint(151, 395), new XPoint(420, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr3);
			xtf.DrawString("TRADE IN / TUKAR TAMBAH", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);

			XRect xr4 = new XRect(new XPoint(421, 395), new XPoint(580, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr4);
			xtf.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);
			
			XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);
			
			XPoint[] xps3 = new XPoint[2] { new XPoint(150, 395), new XPoint(150, 415) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

			XPoint[] xps4 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 415) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);
			
			XRect xr5 = new XRect(new XPoint(100, 440), new XPoint(480, 485));
			gfx.DrawRectangle(XBrushes.Transparent, xr5);
			xtf.DrawString("No Exchange - No Resell. Tidak dapat ditukar dan Tidak dapat dijual kembali.", fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
		}
	}
	
	public static string WriteInvoicePG(int IDSalesDetail)
	{
        string query = "EXEC sp_print_sales_detail " + IDSalesDetail + ", 'PG'";
        DataTable header = Db.Rs(query, Db.Cnn_DB);
		
		string query2 = "EXEC sp_get_asp_new 2," + header.Rows[0]["IDPRODUCT"].ToString();
		DataTable asp = Db.Rs(query2, Db.Cnn_DB);
		
		string brand = header.Rows[0]["BRAND"].ToString();
		
		PdfDocument doc = new PdfDocument();
        doc.Info.Title = header.Rows[0]["INVOICE"].ToString();

        // Create an empty page
        PdfPage page = doc.AddPage();

        // Get an XGraphics object for drawing
        XGraphics gfx = XGraphics.FromPdfPage(page);

        // Create a TextFormatter
        XTextFormatter xtfleft = new XTextFormatter(gfx);
        xtfleft.Alignment = XParagraphAlignment.Left;
        XTextFormatter xtfright = new XTextFormatter(gfx);
        xtfright.Alignment = XParagraphAlignment.Right;
        XTextFormatter xtfcenter = new XTextFormatter(gfx);
        xtfcenter.Alignment = XParagraphAlignment.Center;
        XTextFormatter xtfjustify = new XTextFormatter(gfx);
        xtfjustify.Alignment = XParagraphAlignment.Justify;

        // Create a font
        XFont fontTitle = new XFont("Consolas", 20, XFontStyle.Bold);
        XFont fontParagraph = new XFont("Consolas", 12, XFontStyle.Regular);
        XFont fontParagraph2 = new XFont("Consolas", 16, XFontStyle.Regular);
        XFont fontParagraph3 = new XFont("Consolas", 8, XFontStyle.Regular);
        XFont fontParagraph3S = new XFont("Consolas", 7, XFontStyle.Regular);
        XFont fontParagraph4 = new XFont("Consolas", 8, XFontStyle.Bold);
        XFont fontParagraph5 = new XFont("Consolas", 8, XFontStyle.Italic);
        XFont fontParagraph6 = new XFont("Consolas", 8, XFontStyle.Underline);

        // Draw the text
        /*gfx.DrawString("Hello, World!", fontTitle, XBrushes.Black,
          new XRect(0, 0, page.Width, page.Height),
          XStringFormats.Center);*/

        // Create a div
        gfx.DrawString("INVOICE GABUNGAN", fontParagraph2, XBrushes.Black, new XPoint(20, 30));
        gfx.DrawString("PT. CENTRAL MEGA KENCANA", fontParagraph, XBrushes.Black, new XPoint(20, 50));
        gfx.DrawString("NPWP : 01.764.536.7-073.000", fontParagraph, XBrushes.Black, new XPoint(20, 65));

        XRect xr2 = new XRect(new XPoint(450, 20), new XPoint(580, 65));
        gfx.DrawRectangle(XBrushes.Transparent, xr2);
        xtfleft.DrawString(header.Rows[0]["STOREADDR"].ToString(), fontParagraph3S, XBrushes.Black, xr2, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.8), new XPoint(20, 70), new XPoint(580, 70));

        XRect xr3 = new XRect(new XPoint(20, 75), new XPoint(250, 85));
        gfx.DrawRectangle(XBrushes.Transparent, xr3);
        xtfleft.DrawString("No.                     : " + header.Rows[0]["INVOICE"].ToString(), fontParagraph3, XBrushes.Black, xr3, XStringFormats.TopLeft);

        XRect xr4 = new XRect(new XPoint(20, 85), new XPoint(250, 95));
        gfx.DrawRectangle(XBrushes.Transparent, xr4);
        xtfleft.DrawString("Date / Tanggal Faktur   : " + Convert.ToDateTime(header.Rows[0]["TGLSO"]).ToString("dd-MM-yyyy"), fontParagraph3, XBrushes.Black, xr4, XStringFormats.TopLeft);

        XRect xr5 = new XRect(new XPoint(310, 75), new XPoint(580, 85));
        gfx.DrawRectangle(XBrushes.Transparent, xr5);
        xtfleft.DrawString("Customer Code / Kode Pelanggan  : " + header.Rows[0]["CUSTOMERCODE"].ToString(), fontParagraph3, XBrushes.Black, xr5, XStringFormats.TopLeft);

        XRect xr6 = new XRect(new XPoint(310, 85), new XPoint(580, 95));
        gfx.DrawRectangle(XBrushes.Transparent, xr6);
        xtfleft.DrawString("To / Kepada                     : " + header.Rows[0]["CUSTOMERNAME"].ToString(), fontParagraph3, XBrushes.Black, xr6, XStringFormats.TopLeft);

        XRect xr7 = new XRect(new XPoint(310, 95), new XPoint(580, 105));
        gfx.DrawRectangle(XBrushes.Transparent, xr7);
        xtfleft.DrawString("Given To / Diberikan Kepada     : " + (!string.IsNullOrEmpty(header.Rows[0]["CUSTOMERREFERENCENAME"].ToString()) ? header.Rows[0]["CUSTOMERREFERENCENAME"] : header.Rows[0]["CUSTOMERNAME"].ToString()), fontParagraph3, XBrushes.Black, xr7, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), new XPoint(20, 110), new XPoint(580, 110));

        XRect xr8 = new XRect(new XPoint(20, 110), new XPoint(100, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr8);
        xtfleft.DrawString("CODE", fontParagraph4, XBrushes.Black, xr8, XStringFormats.TopLeft);

        XRect xr9 = new XRect(new XPoint(105, 110), new XPoint(402, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr9);
        xtfleft.DrawString("ITEM & Description / Nama Barang & Keterangan", fontParagraph4, XBrushes.Black, xr9, XStringFormats.TopLeft);

        XRect xr10 = new XRect(new XPoint(403, 110), new XPoint(450, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr10);
        xtfcenter.DrawString("Qty", fontParagraph4, XBrushes.Black, xr10, XStringFormats.TopLeft);

        XRect xr11 = new XRect(new XPoint(455, 110), new XPoint(580, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr11);
        xtfright.DrawString("Price / Harga Jual (Rp) ", fontParagraph4, XBrushes.Black, xr11, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), new XPoint(20, 120), new XPoint(580, 120));
		
		//Item Desc
		XRect xr12 = new XRect(new XPoint(20, 120), new XPoint(100, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr12);
		xtfleft.DrawString(header.Rows[0]["KODE"].ToString(), fontParagraph3, XBrushes.Black, xr12, XStringFormats.TopLeft);

		XRect xr13 = new XRect(new XPoint(105, 120), new XPoint(402, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr13);
		xtfleft.DrawString(header.Rows[0]["NAMA"].ToString(), fontParagraph3, XBrushes.Black, xr13, XStringFormats.TopLeft);

		XRect xr14 = new XRect(new XPoint(403, 120), new XPoint(450, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr14);
		xtfcenter.DrawString(header.Rows[0]["QTY"].ToString(), fontParagraph3, XBrushes.Black, xr14, XStringFormats.TopLeft);

		XRect xr15 = new XRect(new XPoint(455, 120), new XPoint(580, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr15);
		xtfright.DrawString(Convert.ToInt32(header.Rows[0]["TOTALBAYAR"]).ToString("N0"), fontParagraph3, XBrushes.Black, xr15, XStringFormats.TopLeft);
		
		gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 1), new XPoint(20, 130), new XPoint(580, 130));
		
		XRect xr16 = new XRect(new XPoint(20, 130), new XPoint(250, 170));
        gfx.DrawRectangle(XBrushes.Transparent, xr16);
        xtfleft.DrawString("Say / Terbilang : " + Str.Terbilang(Convert.ToDecimal(header.Rows[0]["TOTALBAYAR"])).TrimEnd().TrimStart(), fontParagraph3, XBrushes.Black, xr16, XStringFormats.TopLeft);
		
		System.Net.WebClient wc = new System.Net.WebClient();
        byte[] bytes = wc.DownloadData("https://dev1.cmk.co.id/ImagePLU.aspx?plu=" + header.Rows[0]["KODE"].ToString());
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            XImage img = XImage.FromStream(ms); //XImage.FromStream(ms);        
            XRect xr18 = new XRect(new XPoint(315, 132), new XSize(75, 75));
            gfx.DrawImage(img, xr18);
			
			XPoint[] xp = new XPoint[5] { new XPoint(315, 132), new XPoint(390, 132), new XPoint(390, 207), new XPoint(315, 207), new XPoint(315, 132) };
			gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xp);
            ms.Close();
        }
		
		XPoint[] xps = new XPoint[5] { new XPoint(400, 132), new XPoint(580, 132), new XPoint(580, 270), new XPoint(400, 270), new XPoint(400, 132) };
        gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
		
		decimal RatePPN = (Convert.ToDateTime(header.Rows[0]["TGLSO"]) < Convert.ToDateTime("2022-04-01 00:00:00.000")) ? 1.02m : 1.022m;
		int PercentagePPN = (Convert.ToDateTime(header.Rows[0]["TGLSO"]) < Convert.ToDateTime("2022-04-01 00:00:00.000")) ? 10 : 11;
		decimal TotalBayar = Convert.ToDecimal(header.Rows[0]["TOTALBAYAR"]);
        decimal TotalBayarExcVAT = Math.Ceiling(TotalBayar / RatePPN);
        decimal PPN = TotalBayar - TotalBayarExcVAT;

        XRect xr19 = new XRect(new XPoint(403, 135), new XPoint(500, 165));
        gfx.DrawRectangle(XBrushes.Transparent, xr19);
        xtfleft.DrawString("Price Exclude VAT / Harga Jual Tidak Termasuk PPN", fontParagraph3, XBrushes.Black, xr19, XStringFormats.TopLeft);

        XRect xr20 = new XRect(new XPoint(403, 175), new XPoint(500, 205));
        gfx.DrawRectangle(XBrushes.Transparent, xr20);
        xtfleft.DrawString("VAT / PPN (" + PercentagePPN.ToString() + "% x 20% x Harga Jual Tidak Termasuk PPN )", fontParagraph3, XBrushes.Black, xr20, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.25), new XPoint(400, 210), new XPoint(580, 210));

        XRect xr21 = new XRect(new XPoint(403, 215), new XPoint(500, 225));
        gfx.DrawRectangle(XBrushes.Transparent, xr21);
        xtfleft.DrawString("Total", fontParagraph3, XBrushes.Black, xr21, XStringFormats.TopLeft);

        XRect xr22 = new XRect(new XPoint(403, 235), new XPoint(500, 245));
        gfx.DrawRectangle(XBrushes.Transparent, xr22);
        xtfleft.DrawString("Payment / Pembayaran", fontParagraph3, XBrushes.Black, xr22, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.25), new XPoint(400, 250), new XPoint(580, 250));

        XRect xr23 = new XRect(new XPoint(403, 255), new XPoint(500, 265));
        gfx.DrawRectangle(XBrushes.Transparent, xr23);
        xtfleft.DrawString("Balance Due / Sisa", fontParagraph3, XBrushes.Black, xr23, XStringFormats.TopLeft);

        XRect xr24 = new XRect(new XPoint(505, 135), new XPoint(577, 165));
        gfx.DrawRectangle(XBrushes.Transparent, xr24);
        xtfright.DrawString(TotalBayarExcVAT.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr24, XStringFormats.TopLeft);

        XRect xr25 = new XRect(new XPoint(505, 175 ), new XPoint(577, 205));
        gfx.DrawRectangle(XBrushes.Transparent, xr25);
        xtfright.DrawString(PPN.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr25, XStringFormats.TopLeft);

        XRect xr26 = new XRect(new XPoint(505, 215), new XPoint(577, 225));
        gfx.DrawRectangle(XBrushes.Transparent, xr26);
        xtfright.DrawString(TotalBayar.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr26, XStringFormats.TopLeft);

        XRect xr27 = new XRect(new XPoint(505, 235), new XPoint(577, 245));
        gfx.DrawRectangle(XBrushes.Transparent, xr27);
        xtfright.DrawString(TotalBayar.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr27, XStringFormats.TopLeft);

        XRect xr28 = new XRect(new XPoint(505, 255 ), new XPoint(577, 265));
        gfx.DrawRectangle(XBrushes.Transparent, xr28);
        xtfright.DrawString("0", fontParagraph3, XBrushes.Black, xr28, XStringFormats.TopLeft);

        XRect xr29 = new XRect(new XPoint(373, 350), new XPoint(470, 360));
        gfx.DrawRectangle(XBrushes.Transparent, xr29);
        xtfcenter.DrawString(header.Rows[0]["CUSTOMERNAME"].ToString(), fontParagraph4, XBrushes.Black, xr29, XStringFormats.TopLeft);

        XRect xr30 = new XRect(new XPoint(475, 350), new XPoint(577, 360));
        gfx.DrawRectangle(XBrushes.Transparent, xr30);
        xtfcenter.DrawString(header.Rows[0]["SALESNAME"].ToString(), fontParagraph4, XBrushes.Black, xr30, XStringFormats.TopLeft);

        XRect xr31 = new XRect(new XPoint(373, 360), new XPoint(470, 370));
        gfx.DrawRectangle(XBrushes.Transparent, xr31);
        xtfcenter.DrawString("(Customer / Pelanggan)", fontParagraph3, XBrushes.Black, xr31, XStringFormats.TopLeft);

        XRect xr32 = new XRect(new XPoint(475, 360), new XPoint(577, 370));
        gfx.DrawRectangle(XBrushes.Transparent, xr32);
        xtfcenter.DrawString("(JR / Staff Penjualan)", fontParagraph3, XBrushes.Black, xr32, XStringFormats.TopLeft);
		
		string kodebrand = header.Rows[0]["KODEBRAND"].ToString();
		string custnama1 = header.Rows[0]["CUSTOMERNAME"].ToString();
		string custrefnama1 = header.Rows[0]["CUSTOMERREFERENCENAME"].ToString();
		WriteASPPG(kodebrand, brand, custnama1, custrefnama1, gfx, fontParagraph3, xtfleft);
		
		xtfleft.Alignment = XParagraphAlignment.Left;
		WriteNotesInvoice(gfx, fontParagraph3, xtfleft, brand);
		WriteDeclarationInvoice(gfx, fontParagraph3, xtfleft);
		
		// Save the document...190116
        string filename = header.Rows[0]["INVOICE"].ToString().Replace("/", "-") + ".pdf";
        string foldername = "D:/Web/uploadjaws/frank/pg";
        if (!Directory.Exists(foldername))
        {
            Directory.CreateDirectory(foldername);
        }

        string SaveLocation = foldername + "/" + filename;
        if (File.Exists(SaveLocation))
        {
            File.Delete(SaveLocation);
        }

        doc.Save(SaveLocation);
        // ...and start a viewer.
        //System.Diagnostics.Process.Start(SaveLocation);
		return SaveLocation;
	}
	
	protected static void WriteASPPG(string kodebrand, string namabrand, string custnama, string custrefnama, XGraphics gfx, XFont fontParagraph, XTextFormatter xtf)
	{
		if(custnama != "CMK CLUB" && custrefnama != "CMK CLUB")
		{
			if(kodebrand != "P")
			{
				XRect xr1 = new XRect(new XPoint(20, 385), new XPoint(250, 395));
				gfx.DrawRectangle(XBrushes.Transparent, xr1);
				xtf.DrawString("After Sales Policy / Kebijakan Penjualan", fontParagraph, XBrushes.Black, xr1, XStringFormats.TopLeft);
				
				xtf.Alignment = XParagraphAlignment.Center;
				
				XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 485), new XPoint(20, 485), new XPoint(20, 395) };
				gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
				
				XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(140, 425));
				gfx.DrawRectangle(XBrushes.Transparent, xr2);
				xtf.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

				XRect xr3 = new XRect(new XPoint(141, 395), new XPoint(280, 425));
				gfx.DrawRectangle(XBrushes.Transparent, xr3);
				xtf.DrawString("TRADE IN - PG TO PG / TUKAR TAMBAH PLAIN GOLD KE PLAIN GOLD", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);
				
				XRect xr4 = new XRect(new XPoint(281, 395), new XPoint(420, 425));
				gfx.DrawRectangle(XBrushes.Transparent, xr4);
				xtf.DrawString("TRADE IN - PG TO DJ* / TUKAR TAMBAH PLAIN GOLD KE DIAMOND JEWELLERY", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);

				XRect xr5 = new XRect(new XPoint(421, 395), new XPoint(580, 425));
				gfx.DrawRectangle(XBrushes.Transparent, xr5);
				xtf.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
				
				XPoint[] xps1 = new XPoint[2] { new XPoint(140, 395), new XPoint(140, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps1[0], xps1[1]);
				
				XPoint[] xps2 = new XPoint[2] { new XPoint(280, 395), new XPoint(280, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);
				
				XPoint[] xps3 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);
				
				XPoint[] xps4 = new XPoint[2] { new XPoint(20, 425), new XPoint(580, 425) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);
				
				XPoint[] xps5 = new XPoint[2] { new XPoint(20, 435), new XPoint(580, 435) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps5[0], xps5[1]);
				
				XPoint[] xps6 = new XPoint[2] { new XPoint(20, 445), new XPoint(580, 445) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps6[0], xps6[1]);
				
				XRect xr6 = new XRect(new XPoint(21, 425), new XPoint(140, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr6);
				xtf.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr6, XStringFormats.TopLeft);

				XRect xr7 = new XRect(new XPoint(141, 425), new XPoint(280, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr7);
				xtf.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr7, XStringFormats.TopLeft);
				
				XRect xr8 = new XRect(new XPoint(281, 425), new XPoint(420, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr8);
				xtf.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr8, XStringFormats.TopLeft);

				XRect xr9 = new XRect(new XPoint(421, 425), new XPoint(580, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr9);
				xtf.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr9, XStringFormats.TopLeft);
				
				XRect xr10 = new XRect(new XPoint(21, 435), new XPoint(140, 445));
				gfx.DrawRectangle(XBrushes.Transparent, xr10);
				xtf.DrawString("<= 1 Weeks - 1 Minggu", fontParagraph, XBrushes.Black, xr10, XStringFormats.TopLeft);

				XRect xr11 = new XRect(new XPoint(141, 435), new XPoint(280, 445));
				gfx.DrawRectangle(XBrushes.Transparent, xr11);
				xtf.DrawString("> 1 Weeks - 1 Minggu", fontParagraph, XBrushes.Black, xr11, XStringFormats.TopLeft);
				
				XRect xr12 = new XRect(new XPoint(281, 435), new XPoint(420, 445));
				gfx.DrawRectangle(XBrushes.Transparent, xr12);
				xtf.DrawString("At anytime - Kapan pun", fontParagraph, XBrushes.Black, xr12, XStringFormats.TopLeft);

				XRect xr13 = new XRect(new XPoint(421, 435), new XPoint(580, 445));
				gfx.DrawRectangle(XBrushes.Transparent, xr13);
				xtf.DrawString("At anytime - Kapan pun", fontParagraph, XBrushes.Black, xr13, XStringFormats.TopLeft);
				
				XRect xr14 = new XRect(new XPoint(21, 450), new XPoint(140, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr14);
				xtf.DrawString("100% Original Sales Amount (Nilai Penjualan Asal)", fontParagraph, XBrushes.Black, xr14, XStringFormats.TopLeft);

				XRect xr15 = new XRect(new XPoint(141, 450), new XPoint(280, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr15);
				xtf.DrawString("Today Buying Price The Palace (Harga Beli The Palace)", fontParagraph, XBrushes.Black, xr15, XStringFormats.TopLeft);
				
				XRect xr16 = new XRect(new XPoint(281, 450), new XPoint(420, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr16);
				xtf.DrawString("Today Buying Price The Palace (Harga Beli The Palace)", fontParagraph, XBrushes.Black, xr16, XStringFormats.TopLeft);

				XRect xr17 = new XRect(new XPoint(421, 450), new XPoint(580, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr17);
				xtf.DrawString("Today Buying Price The Palace (Harga Beli The Palace)", fontParagraph, XBrushes.Black, xr17, XStringFormats.TopLeft);
			}
			else
			{
				XRect xr1 = new XRect(new XPoint(20, 385), new XPoint(250, 395));
				gfx.DrawRectangle(XBrushes.Transparent, xr1);
				xtf.DrawString("After Sales Policy / Kebijakan Penjualan", fontParagraph, XBrushes.Black, xr1, XStringFormats.TopLeft);
				
				xtf.Alignment = XParagraphAlignment.Center;
				
				XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 485), new XPoint(20, 485), new XPoint(20, 395) };
				gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
				
				XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(200, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr2);
				xtf.DrawString("MERCHENDISE EXCHANGE - PG to PG / TUKAR PLAIN GOLD KE PLAIN GOLD", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

				XRect xr3 = new XRect(new XPoint(201, 395), new XPoint(580, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr3);
				xtf.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);
				
				XPoint[] xps1 = new XPoint[2] { new XPoint(200, 395), new XPoint(200, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps1[0], xps1[1]);
				
				XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);
				
				XPoint[] xps3 = new XPoint[2] { new XPoint(20, 425), new XPoint(580, 425) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);
				
				XPoint[] xps4 = new XPoint[2] { new XPoint(20, 435), new XPoint(580, 435) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);

				XRect xr4 = new XRect(new XPoint(21, 415), new XPoint(200, 425));
				gfx.DrawRectangle(XBrushes.Transparent, xr4);
				xtf.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);

				XRect xr5 = new XRect(new XPoint(201, 415), new XPoint(580, 425));
				gfx.DrawRectangle(XBrushes.Transparent, xr5);
				xtf.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);

				XRect xr6 = new XRect(new XPoint(21, 425), new XPoint(200, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr6);
				xtf.DrawString("<= 1 Weeks - 1 Minggu", fontParagraph, XBrushes.Black, xr6, XStringFormats.TopLeft);

				XRect xr7 = new XRect(new XPoint(201, 425), new XPoint(580, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr7);
				xtf.DrawString("At anytime - Kapan pun", fontParagraph, XBrushes.Black, xr7, XStringFormats.TopLeft);

				XRect xr8 = new XRect(new XPoint(21, 440), new XPoint(200, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr8);
				xtf.DrawString("100% Original Sales Amount (Nilai Penjualan Asal)", fontParagraph, XBrushes.Black, xr8, XStringFormats.TopLeft);

				XRect xr9 = new XRect(new XPoint(201, 440), new XPoint(580, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr9);
				xtf.DrawString("Only gold value calculated based on " + namabrand + " today's buy back gold price " +
				"(Hanya nilai emas yang diperhitungkan berdasarkan harga beli " + namabrand + " pada hari pengembalian)", fontParagraph, XBrushes.Black, xr9, XStringFormats.TopLeft);			
			}
		}
		else
		{
			XRect xr1 = new XRect(new XPoint(20, 385), new XPoint(250, 395));
			gfx.DrawRectangle(XBrushes.Transparent, xr1);
			xtf.DrawString("After Sales Policy / Kebijakan Penjualan", fontParagraph, XBrushes.Black, xr1, XStringFormats.TopLeft);
			
			xtf.Alignment = XParagraphAlignment.Center;
			
			XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 485), new XPoint(20, 485), new XPoint(20, 395) };
			gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
			
			XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(200, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr2);
			xtf.DrawString("MERCHENDISE EXCHANGE - PG to PG / TUKAR PLAIN GOLD KE PLAIN GOLD", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

			XRect xr3 = new XRect(new XPoint(201, 395), new XPoint(580, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr3);
			xtf.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);
			
			XPoint[] xps1 = new XPoint[2] { new XPoint(200, 395), new XPoint(200, 485) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps1[0], xps1[1]);
			
			XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);
			
			XRect xr5 = new XRect(new XPoint(100, 440), new XPoint(480, 485));
			gfx.DrawRectangle(XBrushes.Transparent, xr5);
			xtf.DrawString("No Exchange - No Resell. Tidak dapat ditukar dan Tidak dapat dijual kembali.", fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
		}
	}
	
	public static string WriteInvoiceGJ(int IDSalesDetail)
	{
        string query = "EXEC sp_print_sales_detail " + IDSalesDetail + ", 'GJ'";
        DataTable header = Db.Rs(query, Db.Cnn_DB);
		
		string query2 = "EXEC sp_get_asp_new 3," + header.Rows[0]["IDPRODUCT"].ToString();
		DataTable asp = Db.Rs(query2, Db.Cnn_DB);
		
		string brand = header.Rows[0]["BRAND"].ToString();
		
		PdfDocument doc = new PdfDocument();
        doc.Info.Title = header.Rows[0]["INVOICE"].ToString();

        // Create an empty page
        PdfPage page = doc.AddPage();

        // Get an XGraphics object for drawing
        XGraphics gfx = XGraphics.FromPdfPage(page);

        // Create a TextFormatter
        XTextFormatter xtfleft = new XTextFormatter(gfx);
        xtfleft.Alignment = XParagraphAlignment.Left;
        XTextFormatter xtfright = new XTextFormatter(gfx);
        xtfright.Alignment = XParagraphAlignment.Right;
        XTextFormatter xtfcenter = new XTextFormatter(gfx);
        xtfcenter.Alignment = XParagraphAlignment.Center;
        XTextFormatter xtfjustify = new XTextFormatter(gfx);
        xtfjustify.Alignment = XParagraphAlignment.Justify;

        // Create a font
        XFont fontTitle = new XFont("Consolas", 20, XFontStyle.Bold);
        XFont fontParagraph = new XFont("Consolas", 12, XFontStyle.Regular);
        XFont fontParagraph2 = new XFont("Consolas", 16, XFontStyle.Regular);
        XFont fontParagraph3 = new XFont("Consolas", 8, XFontStyle.Regular);
        XFont fontParagraph3S = new XFont("Consolas", 7, XFontStyle.Regular);
        XFont fontParagraph4 = new XFont("Consolas", 8, XFontStyle.Bold);
        XFont fontParagraph5 = new XFont("Consolas", 8, XFontStyle.Italic);
        XFont fontParagraph6 = new XFont("Consolas", 8, XFontStyle.Underline);

        // Draw the text
        /*gfx.DrawString("Hello, World!", fontTitle, XBrushes.Black,
          new XRect(0, 0, page.Width, page.Height),
          XStringFormats.Center);*/

        // Create a div
        gfx.DrawString("INVOICE GABUNGAN", fontParagraph2, XBrushes.Black, new XPoint(20, 30));
        gfx.DrawString("PT. CENTRAL MEGA KENCANA", fontParagraph, XBrushes.Black, new XPoint(20, 50));
        gfx.DrawString("NPWP : 01.764.536.7-073.000", fontParagraph, XBrushes.Black, new XPoint(20, 65));

        XRect xr2 = new XRect(new XPoint(450, 20), new XPoint(580, 65));
        gfx.DrawRectangle(XBrushes.Transparent, xr2);
        xtfleft.DrawString(header.Rows[0]["STOREADDR"].ToString(), fontParagraph3S, XBrushes.Black, xr2, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.8), new XPoint(20, 70), new XPoint(580, 70));

        XRect xr3 = new XRect(new XPoint(20, 75), new XPoint(250, 85));
        gfx.DrawRectangle(XBrushes.Transparent, xr3);
        xtfleft.DrawString("No.                     : " + header.Rows[0]["INVOICE"].ToString(), fontParagraph3, XBrushes.Black, xr3, XStringFormats.TopLeft);

        XRect xr4 = new XRect(new XPoint(20, 85), new XPoint(250, 95));
        gfx.DrawRectangle(XBrushes.Transparent, xr4);
        xtfleft.DrawString("Date / Tanggal Faktur   : " + Convert.ToDateTime(header.Rows[0]["TGLSO"]).ToString("dd-MM-yyyy"), fontParagraph3, XBrushes.Black, xr4, XStringFormats.TopLeft);

        XRect xr5 = new XRect(new XPoint(310, 75), new XPoint(580, 85));
        gfx.DrawRectangle(XBrushes.Transparent, xr5);
        xtfleft.DrawString("Customer Code / Kode Pelanggan  : " + header.Rows[0]["CUSTOMERCODE"].ToString(), fontParagraph3, XBrushes.Black, xr5, XStringFormats.TopLeft);

        XRect xr6 = new XRect(new XPoint(310, 85), new XPoint(580, 95));
        gfx.DrawRectangle(XBrushes.Transparent, xr6);
        xtfleft.DrawString("To / Kepada                     : " + header.Rows[0]["CUSTOMERNAME"].ToString(), fontParagraph3, XBrushes.Black, xr6, XStringFormats.TopLeft);

        XRect xr7 = new XRect(new XPoint(310, 95), new XPoint(580, 105));
        gfx.DrawRectangle(XBrushes.Transparent, xr7);
        xtfleft.DrawString("Given To / Diberikan Kepada     : " + (!string.IsNullOrEmpty(header.Rows[0]["CUSTOMERREFERENCENAME"].ToString()) ? header.Rows[0]["CUSTOMERREFERENCENAME"] : header.Rows[0]["CUSTOMERNAME"].ToString()), fontParagraph3, XBrushes.Black, xr7, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), new XPoint(20, 110), new XPoint(580, 110));

        XRect xr8 = new XRect(new XPoint(20, 110), new XPoint(100, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr8);
        xtfleft.DrawString("CODE", fontParagraph4, XBrushes.Black, xr8, XStringFormats.TopLeft);

        XRect xr9 = new XRect(new XPoint(105, 110), new XPoint(402, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr9);
        xtfleft.DrawString("ITEM & Description / Nama Barang & Keterangan", fontParagraph4, XBrushes.Black, xr9, XStringFormats.TopLeft);

        XRect xr10 = new XRect(new XPoint(403, 110), new XPoint(450, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr10);
        xtfcenter.DrawString("Qty", fontParagraph4, XBrushes.Black, xr10, XStringFormats.TopLeft);

        XRect xr11 = new XRect(new XPoint(455, 110), new XPoint(580, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr11);
        xtfright.DrawString("Price / Harga Jual (Rp) ", fontParagraph4, XBrushes.Black, xr11, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), new XPoint(20, 120), new XPoint(580, 120));
		
		//Item Desc
		XRect xr12 = new XRect(new XPoint(20, 120), new XPoint(100, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr12);
		xtfleft.DrawString(header.Rows[0]["KODE"].ToString(), fontParagraph3, XBrushes.Black, xr12, XStringFormats.TopLeft);

		XRect xr13 = new XRect(new XPoint(105, 120), new XPoint(402, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr13);
		xtfleft.DrawString(header.Rows[0]["NAMA"].ToString(), fontParagraph3, XBrushes.Black, xr13, XStringFormats.TopLeft);

		XRect xr14 = new XRect(new XPoint(403, 120), new XPoint(450, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr14);
		xtfcenter.DrawString(header.Rows[0]["QTY"].ToString(), fontParagraph3, XBrushes.Black, xr14, XStringFormats.TopLeft);

		XRect xr15 = new XRect(new XPoint(455, 120), new XPoint(580, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr15);
		xtfright.DrawString(Convert.ToInt32(header.Rows[0]["TOTALBAYAR"]).ToString("N0"), fontParagraph3, XBrushes.Black, xr15, XStringFormats.TopLeft);
		
		gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 1), new XPoint(20, 130), new XPoint(580, 130));
		
		XRect xr16 = new XRect(new XPoint(20, 130), new XPoint(250, 170));
        gfx.DrawRectangle(XBrushes.Transparent, xr16);
        xtfleft.DrawString("Say / Terbilang : " + Str.Terbilang(Convert.ToDecimal(header.Rows[0]["TOTALBAYAR"])).TrimEnd().TrimStart(), fontParagraph3, XBrushes.Black, xr16, XStringFormats.TopLeft);
		
		/*System.Net.WebClient wc = new System.Net.WebClient();
        byte[] bytes = wc.DownloadData("https://dev1.cmk.co.id/ImagePLU.aspx?plu=" + header.Rows[0]["KODE"].ToString());
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            XImage img = XImage.FromStream(ms); //XImage.FromStream(ms);        
            XRect xr18 = new XRect(new XPoint(315, 132), new XSize(75, 75));
            gfx.DrawImage(img, xr18);
			
			XPoint[] xp = new XPoint[5] { new XPoint(315, 132), new XPoint(390, 132), new XPoint(390, 207), new XPoint(315, 207), new XPoint(315, 132) };
			gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xp);
            ms.Close();
        }*/
		
		XPoint[] xps = new XPoint[5] { new XPoint(400, 132), new XPoint(580, 132), new XPoint(580, 270), new XPoint(400, 270), new XPoint(400, 132) };
        gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
		
		decimal RatePPN = (Convert.ToDateTime(header.Rows[0]["TGLSO"]) < Convert.ToDateTime("2022-04-01 00:00:00.000")) ? 1.02m : 1.022m;
		int PercentagePPN = (Convert.ToDateTime(header.Rows[0]["TGLSO"]) < Convert.ToDateTime("2022-04-01 00:00:00.000")) ? 10 : 11;
		decimal TotalBayar = Convert.ToDecimal(header.Rows[0]["TOTALBAYAR"]);
        decimal TotalBayarExcVAT = Math.Ceiling(TotalBayar / RatePPN);
        decimal PPN = TotalBayar - TotalBayarExcVAT;

        XRect xr19 = new XRect(new XPoint(403, 135), new XPoint(500, 165));
        gfx.DrawRectangle(XBrushes.Transparent, xr19);
        xtfleft.DrawString("Price Exclude VAT / Harga Jual Tidak Termasuk PPN", fontParagraph3, XBrushes.Black, xr19, XStringFormats.TopLeft);

        XRect xr20 = new XRect(new XPoint(403, 175), new XPoint(500, 205));
        gfx.DrawRectangle(XBrushes.Transparent, xr20);
        xtfleft.DrawString("VAT / PPN (" + PercentagePPN.ToString() + "% x 20% x Harga Jual Tidak Termasuk PPN )", fontParagraph3, XBrushes.Black, xr20, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.25), new XPoint(400, 210), new XPoint(580, 210));

        XRect xr21 = new XRect(new XPoint(403, 215), new XPoint(500, 225));
        gfx.DrawRectangle(XBrushes.Transparent, xr21);
        xtfleft.DrawString("Total", fontParagraph3, XBrushes.Black, xr21, XStringFormats.TopLeft);

        XRect xr22 = new XRect(new XPoint(403, 235), new XPoint(500, 245));
        gfx.DrawRectangle(XBrushes.Transparent, xr22);
        xtfleft.DrawString("Payment / Pembayaran", fontParagraph3, XBrushes.Black, xr22, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.25), new XPoint(400, 250), new XPoint(580, 250));

        XRect xr23 = new XRect(new XPoint(403, 255), new XPoint(500, 265));
        gfx.DrawRectangle(XBrushes.Transparent, xr23);
        xtfleft.DrawString("Balance Due / Sisa", fontParagraph3, XBrushes.Black, xr23, XStringFormats.TopLeft);

        XRect xr24 = new XRect(new XPoint(505, 135), new XPoint(577, 165));
        gfx.DrawRectangle(XBrushes.Transparent, xr24);
        xtfright.DrawString(TotalBayarExcVAT.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr24, XStringFormats.TopLeft);

        XRect xr25 = new XRect(new XPoint(505, 175 ), new XPoint(577, 205));
        gfx.DrawRectangle(XBrushes.Transparent, xr25);
        xtfright.DrawString(PPN.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr25, XStringFormats.TopLeft);

        XRect xr26 = new XRect(new XPoint(505, 215), new XPoint(577, 225));
        gfx.DrawRectangle(XBrushes.Transparent, xr26);
        xtfright.DrawString(TotalBayar.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr26, XStringFormats.TopLeft);

        XRect xr27 = new XRect(new XPoint(505, 235), new XPoint(577, 245));
        gfx.DrawRectangle(XBrushes.Transparent, xr27);
        xtfright.DrawString(TotalBayar.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr27, XStringFormats.TopLeft);

        XRect xr28 = new XRect(new XPoint(505, 255 ), new XPoint(577, 265));
        gfx.DrawRectangle(XBrushes.Transparent, xr28);
        xtfright.DrawString("0", fontParagraph3, XBrushes.Black, xr28, XStringFormats.TopLeft);

        XRect xr29 = new XRect(new XPoint(373, 350), new XPoint(470, 360));
        gfx.DrawRectangle(XBrushes.Transparent, xr29);
        xtfcenter.DrawString(header.Rows[0]["CUSTOMERNAME"].ToString(), fontParagraph4, XBrushes.Black, xr29, XStringFormats.TopLeft);

        XRect xr30 = new XRect(new XPoint(475, 350), new XPoint(577, 360));
        gfx.DrawRectangle(XBrushes.Transparent, xr30);
        xtfcenter.DrawString(header.Rows[0]["SALESNAME"].ToString(), fontParagraph4, XBrushes.Black, xr30, XStringFormats.TopLeft);

        XRect xr31 = new XRect(new XPoint(373, 360), new XPoint(470, 370));
        gfx.DrawRectangle(XBrushes.Transparent, xr31);
        xtfcenter.DrawString("(Customer / Pelanggan)", fontParagraph3, XBrushes.Black, xr31, XStringFormats.TopLeft);

        XRect xr32 = new XRect(new XPoint(475, 360), new XPoint(577, 370));
        gfx.DrawRectangle(XBrushes.Transparent, xr32);
        xtfcenter.DrawString("(JR / Staff Penjualan)", fontParagraph3, XBrushes.Black, xr32, XStringFormats.TopLeft);
		
		string kodebrand = header.Rows[0]["KODEBRAND"].ToString();
		string custnama1 = header.Rows[0]["CUSTOMERNAME"].ToString();
		string custrefnama1 = header.Rows[0]["CUSTOMERREFERENCENAME"].ToString();
		WriteASPGJ(kodebrand, brand, custnama1, custrefnama1, gfx, fontParagraph3, xtfleft);
		
		xtfleft.Alignment = XParagraphAlignment.Left;
		WriteNotesInvoice(gfx, fontParagraph3, xtfleft, brand);
		WriteDeclarationInvoice(gfx, fontParagraph3, xtfleft);
		
		// Save the document...190116
        string filename = header.Rows[0]["INVOICE"].ToString().Replace("/", "-") + ".pdf";
        string foldername = "D:/Web/uploadjaws/frank/gj";
        if (!Directory.Exists(foldername))
        {
            Directory.CreateDirectory(foldername);
        }

        string SaveLocation = foldername + "/" + filename;
        if (File.Exists(SaveLocation))
        {
            File.Delete(SaveLocation);
        }

        doc.Save(SaveLocation);
        // ...and start a viewer.
        //System.Diagnostics.Process.Start(SaveLocation);
		return SaveLocation;
	}
	
	protected static void WriteASPGJ(string kodebrand, string namabrand, string custnama, string custrefnama, XGraphics gfx, XFont fontParagraph, XTextFormatter xtf)
	{
		if(custnama != "CMK CLUB" && custrefnama != "CMK CLUB")
		{
			XRect xr1 = new XRect(new XPoint(20, 385), new XPoint(250, 395));
			gfx.DrawRectangle(XBrushes.Transparent, xr1);
			xtf.DrawString("After Sales Policy / Kebijakan Penjualan", fontParagraph, XBrushes.Black, xr1, XStringFormats.TopLeft);
			
			xtf.Alignment = XParagraphAlignment.Center;
			
			XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 485), new XPoint(20, 485), new XPoint(20, 395) };
			gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
			
			XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(150, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr2);
			xtf.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

			XRect xr3 = new XRect(new XPoint(151, 395), new XPoint(420, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr3);
			xtf.DrawString("TRADE IN / TUKAR TAMBAH", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);

			XRect xr4 = new XRect(new XPoint(421, 395), new XPoint(580, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr4);
			xtf.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);
			
			XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);
			
			XPoint[] xps3 = new XPoint[2] { new XPoint(150, 395), new XPoint(150, 415) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

			XPoint[] xps4 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 415) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);
			
			XRect xr5 = new XRect(new XPoint(100, 440), new XPoint(480, 485));
			gfx.DrawRectangle(XBrushes.Transparent, xr5);
			xtf.DrawString("Only gold value calculated based on " + namabrand + " today's buy back gold price " +
				"(Hanya nilai emas yang diperhitungkan berdasarkan harga beli " + namabrand + " pada hari pengembalian)", fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
		}
		else
		{
			XRect xr1 = new XRect(new XPoint(20, 385), new XPoint(250, 395));
			gfx.DrawRectangle(XBrushes.Transparent, xr1);
			xtf.DrawString("After Sales Policy / Kebijakan Penjualan", fontParagraph, XBrushes.Black, xr1, XStringFormats.TopLeft);
			
			xtf.Alignment = XParagraphAlignment.Center;
			
			XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 485), new XPoint(20, 485), new XPoint(20, 395) };
			gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
			
			XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(150, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr2);
			xtf.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

			XRect xr3 = new XRect(new XPoint(151, 395), new XPoint(420, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr3);
			xtf.DrawString("TRADE IN / TUKAR TAMBAH", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);

			XRect xr4 = new XRect(new XPoint(421, 395), new XPoint(580, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr4);
			xtf.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);
			
			XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);
			
			XPoint[] xps3 = new XPoint[2] { new XPoint(150, 395), new XPoint(150, 415) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

			XPoint[] xps4 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 415) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);
			
			XRect xr5 = new XRect(new XPoint(100, 440), new XPoint(480, 485));
			gfx.DrawRectangle(XBrushes.Transparent, xr5);
			xtf.DrawString("No Exchange - No Resell. Tidak dapat ditukar dan Tidak dapat dijual kembali.", fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
		}
	}
	
	public static string WriteInvoiceLD(int IDSalesDetail)
	{
        string query = "EXEC sp_print_sales_detail " + IDSalesDetail + ", 'LD'";
        DataTable header = Db.Rs(query, Db.Cnn_DB);
		
		string query2 = "EXEC sp_get_asp_new 4," + header.Rows[0]["IDPRODUCT"].ToString();
		DataTable asp = Db.Rs(query2, Db.Cnn_DB);
		
		string brand = header.Rows[0]["BRAND"].ToString();
		
		PdfDocument doc = new PdfDocument();
        doc.Info.Title = header.Rows[0]["INVOICE"].ToString();

        // Create an empty page
        PdfPage page = doc.AddPage();

        // Get an XGraphics object for drawing
        XGraphics gfx = XGraphics.FromPdfPage(page);

        // Create a TextFormatter
        XTextFormatter xtfleft = new XTextFormatter(gfx);
        xtfleft.Alignment = XParagraphAlignment.Left;
        XTextFormatter xtfright = new XTextFormatter(gfx);
        xtfright.Alignment = XParagraphAlignment.Right;
        XTextFormatter xtfcenter = new XTextFormatter(gfx);
        xtfcenter.Alignment = XParagraphAlignment.Center;
        XTextFormatter xtfjustify = new XTextFormatter(gfx);
        xtfjustify.Alignment = XParagraphAlignment.Justify;

        // Create a font
        XFont fontTitle = new XFont("Consolas", 20, XFontStyle.Bold);
        XFont fontParagraph = new XFont("Consolas", 12, XFontStyle.Regular);
        XFont fontParagraph2 = new XFont("Consolas", 16, XFontStyle.Regular);
        XFont fontParagraph3 = new XFont("Consolas", 8, XFontStyle.Regular);
        XFont fontParagraph3S = new XFont("Consolas", 7, XFontStyle.Regular);
        XFont fontParagraph4 = new XFont("Consolas", 8, XFontStyle.Bold);
        XFont fontParagraph5 = new XFont("Consolas", 8, XFontStyle.Italic);
        XFont fontParagraph6 = new XFont("Consolas", 8, XFontStyle.Underline);

        // Draw the text
        /*gfx.DrawString("Hello, World!", fontTitle, XBrushes.Black,
          new XRect(0, 0, page.Width, page.Height),
          XStringFormats.Center);*/

        // Create a div
        gfx.DrawString("INVOICE GABUNGAN", fontParagraph2, XBrushes.Black, new XPoint(20, 30));
        gfx.DrawString("PT. CENTRAL MEGA KENCANA", fontParagraph, XBrushes.Black, new XPoint(20, 50));
        gfx.DrawString("NPWP : 01.764.536.7-073.000", fontParagraph, XBrushes.Black, new XPoint(20, 65));

        XRect xr2 = new XRect(new XPoint(450, 20), new XPoint(580, 65));
        gfx.DrawRectangle(XBrushes.Transparent, xr2);
        xtfleft.DrawString(header.Rows[0]["STOREADDR"].ToString(), fontParagraph3S, XBrushes.Black, xr2, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.8), new XPoint(20, 70), new XPoint(580, 70));

        XRect xr3 = new XRect(new XPoint(20, 75), new XPoint(250, 85));
        gfx.DrawRectangle(XBrushes.Transparent, xr3);
        xtfleft.DrawString("No.                     : " + header.Rows[0]["INVOICE"].ToString(), fontParagraph3, XBrushes.Black, xr3, XStringFormats.TopLeft);

        XRect xr4 = new XRect(new XPoint(20, 85), new XPoint(250, 95));
        gfx.DrawRectangle(XBrushes.Transparent, xr4);
        xtfleft.DrawString("Date / Tanggal Faktur   : " + Convert.ToDateTime(header.Rows[0]["TGLSO"]).ToString("dd-MM-yyyy"), fontParagraph3, XBrushes.Black, xr4, XStringFormats.TopLeft);

        XRect xr5 = new XRect(new XPoint(310, 75), new XPoint(580, 85));
        gfx.DrawRectangle(XBrushes.Transparent, xr5);
        xtfleft.DrawString("Customer Code / Kode Pelanggan  : " + header.Rows[0]["CUSTOMERCODE"].ToString(), fontParagraph3, XBrushes.Black, xr5, XStringFormats.TopLeft);

        XRect xr6 = new XRect(new XPoint(310, 85), new XPoint(580, 95));
        gfx.DrawRectangle(XBrushes.Transparent, xr6);
        xtfleft.DrawString("To / Kepada                     : " + header.Rows[0]["CUSTOMERNAME"].ToString(), fontParagraph3, XBrushes.Black, xr6, XStringFormats.TopLeft);

        XRect xr7 = new XRect(new XPoint(310, 95), new XPoint(580, 105));
        gfx.DrawRectangle(XBrushes.Transparent, xr7);
        xtfleft.DrawString("Given To / Diberikan Kepada     : " + (!string.IsNullOrEmpty(header.Rows[0]["CUSTOMERREFERENCENAME"].ToString()) ? header.Rows[0]["CUSTOMERREFERENCENAME"] : header.Rows[0]["CUSTOMERNAME"].ToString()), fontParagraph3, XBrushes.Black, xr7, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), new XPoint(20, 110), new XPoint(580, 110));

        XRect xr8 = new XRect(new XPoint(20, 110), new XPoint(100, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr8);
        xtfleft.DrawString("CODE", fontParagraph4, XBrushes.Black, xr8, XStringFormats.TopLeft);

        XRect xr9 = new XRect(new XPoint(105, 110), new XPoint(402, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr9);
        xtfleft.DrawString("ITEM & Description / Nama Barang & Keterangan", fontParagraph4, XBrushes.Black, xr9, XStringFormats.TopLeft);

        XRect xr10 = new XRect(new XPoint(403, 110), new XPoint(450, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr10);
        xtfcenter.DrawString("Qty", fontParagraph4, XBrushes.Black, xr10, XStringFormats.TopLeft);

        XRect xr11 = new XRect(new XPoint(455, 110), new XPoint(580, 120));
        gfx.DrawRectangle(XBrushes.Transparent, xr11);
        xtfright.DrawString("Price / Harga Jual (Rp) ", fontParagraph4, XBrushes.Black, xr11, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), new XPoint(20, 120), new XPoint(580, 120));
		
		//Item Desc
		XRect xr12 = new XRect(new XPoint(20, 120), new XPoint(100, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr12);
		xtfleft.DrawString(header.Rows[0]["KODE"].ToString(), fontParagraph3, XBrushes.Black, xr12, XStringFormats.TopLeft);

		XRect xr13 = new XRect(new XPoint(105, 120), new XPoint(402, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr13);
		xtfleft.DrawString(header.Rows[0]["NAMA"].ToString(), fontParagraph3, XBrushes.Black, xr13, XStringFormats.TopLeft);

		XRect xr14 = new XRect(new XPoint(403, 120), new XPoint(450, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr14);
		xtfcenter.DrawString(header.Rows[0]["QTY"].ToString(), fontParagraph3, XBrushes.Black, xr14, XStringFormats.TopLeft);

		XRect xr15 = new XRect(new XPoint(455, 120), new XPoint(580, 130));
		gfx.DrawRectangle(XBrushes.Transparent, xr15);
		xtfright.DrawString(Convert.ToInt32(header.Rows[0]["TOTALBAYAR"]).ToString("N0"), fontParagraph3, XBrushes.Black, xr15, XStringFormats.TopLeft);
		
		gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 1), new XPoint(20, 130), new XPoint(580, 130));
		
		XRect xr16 = new XRect(new XPoint(20, 130), new XPoint(250, 170));
        gfx.DrawRectangle(XBrushes.Transparent, xr16);
        xtfleft.DrawString("Say / Terbilang : " + Str.Terbilang(Convert.ToDecimal(header.Rows[0]["TOTALBAYAR"])).TrimEnd().TrimStart(), fontParagraph3, XBrushes.Black, xr16, XStringFormats.TopLeft);
		
		/*System.Net.WebClient wc = new System.Net.WebClient();
        byte[] bytes = wc.DownloadData("https://dev1.cmk.co.id/ImagePLU.aspx?plu=" + header.Rows[0]["KODE"].ToString());
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            XImage img = XImage.FromStream(ms); //XImage.FromStream(ms);        
            XRect xr18 = new XRect(new XPoint(315, 132), new XSize(75, 75));
            gfx.DrawImage(img, xr18);
			
			XPoint[] xp = new XPoint[5] { new XPoint(315, 132), new XPoint(390, 132), new XPoint(390, 207), new XPoint(315, 207), new XPoint(315, 132) };
			gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xp);
            ms.Close();
        }*/
		
		XPoint[] xps = new XPoint[5] { new XPoint(400, 132), new XPoint(580, 132), new XPoint(580, 270), new XPoint(400, 270), new XPoint(400, 132) };
        gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
		
		decimal RatePPN = (Convert.ToDateTime(header.Rows[0]["TGLSO"]) < Convert.ToDateTime("2022-04-01 00:00:00.000")) ? 1.02m : 1.022m;
		int PercentagePPN = (Convert.ToDateTime(header.Rows[0]["TGLSO"]) < Convert.ToDateTime("2022-04-01 00:00:00.000")) ? 10 : 11;
		decimal TotalBayar = Convert.ToDecimal(header.Rows[0]["TOTALBAYAR"]);
        decimal TotalBayarExcVAT = Math.Ceiling(TotalBayar / RatePPN);
        decimal PPN = TotalBayar - TotalBayarExcVAT;

        XRect xr19 = new XRect(new XPoint(403, 135), new XPoint(500, 165));
        gfx.DrawRectangle(XBrushes.Transparent, xr19);
        xtfleft.DrawString("Price Exclude VAT / Harga Jual Tidak Termasuk PPN", fontParagraph3, XBrushes.Black, xr19, XStringFormats.TopLeft);

        XRect xr20 = new XRect(new XPoint(403, 175), new XPoint(500, 205));
        gfx.DrawRectangle(XBrushes.Transparent, xr20);
        xtfleft.DrawString("VAT / PPN (" + PercentagePPN.ToString() + "% x 20% x Harga Jual Tidak Termasuk PPN )", fontParagraph3, XBrushes.Black, xr20, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.25), new XPoint(400, 210), new XPoint(580, 210));

        XRect xr21 = new XRect(new XPoint(403, 215), new XPoint(500, 225));
        gfx.DrawRectangle(XBrushes.Transparent, xr21);
        xtfleft.DrawString("Total", fontParagraph3, XBrushes.Black, xr21, XStringFormats.TopLeft);

        XRect xr22 = new XRect(new XPoint(403, 235), new XPoint(500, 245));
        gfx.DrawRectangle(XBrushes.Transparent, xr22);
        xtfleft.DrawString("Payment / Pembayaran", fontParagraph3, XBrushes.Black, xr22, XStringFormats.TopLeft);

        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.25), new XPoint(400, 250), new XPoint(580, 250));

        XRect xr23 = new XRect(new XPoint(403, 255), new XPoint(500, 265));
        gfx.DrawRectangle(XBrushes.Transparent, xr23);
        xtfleft.DrawString("Balance Due / Sisa", fontParagraph3, XBrushes.Black, xr23, XStringFormats.TopLeft);

        XRect xr24 = new XRect(new XPoint(505, 135), new XPoint(577, 165));
        gfx.DrawRectangle(XBrushes.Transparent, xr24);
        xtfright.DrawString(TotalBayarExcVAT.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr24, XStringFormats.TopLeft);

        XRect xr25 = new XRect(new XPoint(505, 175 ), new XPoint(577, 205));
        gfx.DrawRectangle(XBrushes.Transparent, xr25);
        xtfright.DrawString(PPN.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr25, XStringFormats.TopLeft);

        XRect xr26 = new XRect(new XPoint(505, 215), new XPoint(577, 225));
        gfx.DrawRectangle(XBrushes.Transparent, xr26);
        xtfright.DrawString(TotalBayar.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr26, XStringFormats.TopLeft);

        XRect xr27 = new XRect(new XPoint(505, 235), new XPoint(577, 245));
        gfx.DrawRectangle(XBrushes.Transparent, xr27);
        xtfright.DrawString(TotalBayar.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr27, XStringFormats.TopLeft);

        XRect xr28 = new XRect(new XPoint(505, 255 ), new XPoint(577, 265));
        gfx.DrawRectangle(XBrushes.Transparent, xr28);
        xtfright.DrawString("0", fontParagraph3, XBrushes.Black, xr28, XStringFormats.TopLeft);

        XRect xr29 = new XRect(new XPoint(373, 350), new XPoint(470, 360));
        gfx.DrawRectangle(XBrushes.Transparent, xr29);
        xtfcenter.DrawString(header.Rows[0]["CUSTOMERNAME"].ToString(), fontParagraph4, XBrushes.Black, xr29, XStringFormats.TopLeft);

        XRect xr30 = new XRect(new XPoint(475, 350), new XPoint(577, 360));
        gfx.DrawRectangle(XBrushes.Transparent, xr30);
        xtfcenter.DrawString(header.Rows[0]["SALESNAME"].ToString(), fontParagraph4, XBrushes.Black, xr30, XStringFormats.TopLeft);

        XRect xr31 = new XRect(new XPoint(373, 360), new XPoint(470, 370));
        gfx.DrawRectangle(XBrushes.Transparent, xr31);
        xtfcenter.DrawString("(Customer / Pelanggan)", fontParagraph3, XBrushes.Black, xr31, XStringFormats.TopLeft);

        XRect xr32 = new XRect(new XPoint(475, 360), new XPoint(577, 370));
        gfx.DrawRectangle(XBrushes.Transparent, xr32);
        xtfcenter.DrawString("(JR / Staff Penjualan)", fontParagraph3, XBrushes.Black, xr32, XStringFormats.TopLeft);
		
		string custnama1 = header.Rows[0]["CUSTOMERNAME"].ToString();
		string custrefnama1 = header.Rows[0]["CUSTOMERREFERENCENAME"].ToString();
		WriteASPLD(brand, custnama1, custrefnama1, asp, gfx, fontParagraph3, xtfleft);
		
		xtfleft.Alignment = XParagraphAlignment.Left;
		WriteNotesInvoice(gfx, fontParagraph3, xtfleft, brand);
		WriteDeclarationInvoice(gfx, fontParagraph3, xtfleft);
		
		// Save the document...190116
        string filename = header.Rows[0]["INVOICE"].ToString().Replace("/", "-") + ".pdf";
        string foldername = "D:/Web/uploadjaws/frank/ld";
        if (!Directory.Exists(foldername))
        {
            Directory.CreateDirectory(foldername);
        }

        string SaveLocation = foldername + "/" + filename;
        if (File.Exists(SaveLocation))
        {
            File.Delete(SaveLocation);
        }

        doc.Save(SaveLocation);
        // ...and start a viewer.
        //System.Diagnostics.Process.Start(SaveLocation);
		return SaveLocation;
	}
	
	protected static void WriteASPLD(string namabrand, string custnama, string custrefnama, DataTable asp, XGraphics gfx, XFont fontParagraph, XTextFormatter xtf)
	{
		string namaasp = asp.Rows[0]["NAMA"].ToString();
		if((custnama != "CMK CLUB") && (custrefnama != "CMK CLUB"))
		{
			if(namaasp != "")
			{
				XRect xr1 = new XRect(new XPoint(20, 385), new XPoint(250, 395));
				gfx.DrawRectangle(XBrushes.Transparent, xr1);
				xtf.DrawString("After Sales Policy / Kebijakan Penjualan", fontParagraph, XBrushes.Black, xr1, XStringFormats.TopLeft);
				
				xtf.Alignment = XParagraphAlignment.Center;
				
				XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 485), new XPoint(20, 485), new XPoint(20, 395) };
				gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
				
				XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(150, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr2);
				xtf.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

				XRect xr3 = new XRect(new XPoint(151, 395), new XPoint(420, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr3);
				xtf.DrawString("TRADE IN / TUKAR TAMBAH", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);

				XRect xr4 = new XRect(new XPoint(421, 395), new XPoint(580, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr4);
				xtf.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);

				XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);
				
				XPoint[] xps3 = new XPoint[2] { new XPoint(150, 395), new XPoint(150, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

				XPoint[] xps4 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);

				XPoint[] xps5 = new XPoint[2] { new XPoint(20, 425), new XPoint(580, 425) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps5[0], xps5[1]);

				XPoint[] xps6 = new XPoint[2] { new XPoint(20, 435), new XPoint(580, 435) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps6[0], xps6[1]);

				XRect xr6 = new XRect(new XPoint(21, 415), new XPoint(150, 425));
				gfx.DrawRectangle(XBrushes.Transparent, xr6);
				xtf.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr6, XStringFormats.TopLeft);

				XRect xr7 = new XRect(new XPoint(151, 415), new XPoint(420, 425));
				gfx.DrawRectangle(XBrushes.Transparent, xr7);
				xtf.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr7, XStringFormats.TopLeft);

				XRect xr8 = new XRect(new XPoint(421, 415), new XPoint(580, 425));
				gfx.DrawRectangle(XBrushes.Transparent, xr8);
				xtf.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr8, XStringFormats.TopLeft);

				XPoint[] xps7 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps7[0], xps7[1]);

				XFont fontParagraphsmall = new XFont("Consolas", 6.5, XFontStyle.Regular);
				
				int rowscount = 0;
				
				string idasp = asp.Rows[0][0].ToString();
				int measpperiodmax = Convert.ToInt32(asp.Rows[0][4]);
				int measprumus = Convert.ToInt32(asp.Rows[0][5]);
				
				int ti1aspperiodmax = Convert.ToInt32(asp.Rows[0][7]);
				int ti1asprumus = Convert.ToInt32(asp.Rows[0][8]);
				
				int ti2aspperiodmax = Convert.ToInt32(asp.Rows[0][10]);
				int ti2asprumus = Convert.ToInt32(asp.Rows[0][11]);
				
				int ti3aspperiodmax = Convert.ToInt32(asp.Rows[0][10]);
				int ti3asprumus = Convert.ToInt32(asp.Rows[0][14]);
				
				int r1aspperiodmax = Convert.ToInt32(asp.Rows[0][16]);
				int r1asprumus = Convert.ToInt32(asp.Rows[0][17]);
				
				int r2aspperiodmax = Convert.ToInt32(asp.Rows[0][16]);
				int r2asprumus = Convert.ToInt32(asp.Rows[0][20]);
				
				int memchasprumus = Convert.ToInt32(asp.Rows[0][21]);
				
				string me = "", ti1 = "", ti2 = "", ti3 = "", rs1 = "", rs2 = "";
				string me_rumus1 = "", ti_rumus1 = "", ti_rumus2 = "", ti_rumus3 = "", rs_rumus1 = "", rs_rumus2 = "";
					
				if (memchasprumus != 0)
				{
					me = asp_text_period("<", memchasprumus, "d");
				}
				else
				{
					me = asp_text_period("<", measpperiodmax, "m");
				}
				
				if(ti1aspperiodmax < 12)
					ti1 = asp_text_period("<", ti1aspperiodmax, "m");
				else
					ti1 = asp_text_period("<", ti1aspperiodmax / 12, "y");
				ti2 = asp_text_period("<", ti2aspperiodmax / 12, "y");
				ti3 = asp_text_period(">", ti3aspperiodmax / 12, "y");
				rs1 = asp_text_period("<", r1aspperiodmax / 12, "y");
				rs2 = asp_text_period(">", r2aspperiodmax / 12, "y");

				me_rumus1 = asp_text_value(namabrand, "me", measprumus);
				ti_rumus1 = asp_text_value(namabrand, "ti", ti1asprumus, false);
				ti_rumus2 = asp_text_value(namabrand, "ti", ti2asprumus, false);
				ti_rumus3 = asp_text_value(namabrand, "ti", ti3asprumus, true);
				rs_rumus1 = asp_text_value(namabrand, "rs", r1asprumus, false);
				rs_rumus2 = asp_text_value(namabrand, "rs", r2asprumus, true);

				XRect xr9 = new XRect(new XPoint(21, 426), new XPoint(150, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr9);
				xtf.DrawString(me, fontParagraphsmall, XBrushes.Black, xr9, XStringFormats.TopLeft);

				XRect xr10 = new XRect(new XPoint(151, 426), new XPoint(240, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr10);
				xtf.DrawString(ti1, fontParagraphsmall, XBrushes.Black, xr10, XStringFormats.TopLeft);

				XRect xr11 = new XRect(new XPoint(241, 426), new XPoint(331, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr11);
				xtf.DrawString(ti2, fontParagraphsmall, XBrushes.Black, xr11, XStringFormats.TopLeft);

				XRect xr12 = new XRect(new XPoint(332, 426), new XPoint(420, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr12);
				xtf.DrawString(ti3, fontParagraphsmall, XBrushes.Black, xr12, XStringFormats.TopLeft);
				
				XPoint[] xps8 = new XPoint[2] { new XPoint(241, 425), new XPoint(241, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps8[0], xps8[1]);

				XPoint[] xps9 = new XPoint[2] { new XPoint(331, 425), new XPoint(331, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps9[0], xps9[1]);

				XRect xr13 = new XRect(new XPoint(421, 426), new XPoint(500, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr13);
				xtf.DrawString(rs1, fontParagraphsmall, XBrushes.Black, xr13, XStringFormats.TopLeft);

				XRect xr14 = new XRect(new XPoint(501, 426), new XPoint(580, 435));
				gfx.DrawRectangle(XBrushes.Transparent, xr14);
				xtf.DrawString(rs2, fontParagraphsmall, XBrushes.Black, xr14, XStringFormats.TopLeft);
				
				XPoint[] xps10 = new XPoint[2] { new XPoint(501, 425), new XPoint(501, 485) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps10[0], xps10[1]);
				
				XRect xr15 = new XRect(new XPoint(21, 440), new XPoint(150, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr15);
				xtf.DrawString(me_rumus1, fontParagraphsmall, XBrushes.Black, xr15, XStringFormats.TopLeft);

				XRect xr16 = new XRect(new XPoint(151, 440), new XPoint(240, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr16);
				xtf.DrawString(ti_rumus1, fontParagraphsmall, XBrushes.Black, xr16, XStringFormats.TopLeft);
				
				XRect xr17 = new XRect(new XPoint(241, 440), new XPoint(331, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr17);
				xtf.DrawString(ti_rumus2, fontParagraphsmall, XBrushes.Black, xr17, XStringFormats.TopLeft);

				XRect xr18 = new XRect(new XPoint(332, 440), new XPoint(420, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr18);
				xtf.DrawString(ti_rumus3, fontParagraphsmall, XBrushes.Black, xr18, XStringFormats.TopLeft);
				
				XRect xr19 = new XRect(new XPoint(421, 440), new XPoint(500, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr19);
				xtf.DrawString(rs_rumus1, fontParagraphsmall, XBrushes.Black, xr19, XStringFormats.TopLeft);

				XRect xr20 = new XRect(new XPoint(501, 440), new XPoint(580, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr20);
				xtf.DrawString(rs_rumus2, fontParagraphsmall, XBrushes.Black, xr20, XStringFormats.TopLeft);
			}
			else
			{
				XRect xr1 = new XRect(new XPoint(20, 385), new XPoint(250, 395));
				gfx.DrawRectangle(XBrushes.Transparent, xr1);
				xtf.DrawString("After Sales Policy / Kebijakan Penjualan", fontParagraph, XBrushes.Black, xr1, XStringFormats.TopLeft);
				
				xtf.Alignment = XParagraphAlignment.Center;
				
				XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 485), new XPoint(20, 485), new XPoint(20, 395) };
				gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
				
				XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(150, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr2);
				xtf.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

				XRect xr3 = new XRect(new XPoint(151, 395), new XPoint(420, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr3);
				xtf.DrawString("TRADE IN / TUKAR TAMBAH", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);

				XRect xr4 = new XRect(new XPoint(421, 395), new XPoint(580, 415));
				gfx.DrawRectangle(XBrushes.Transparent, xr4);
				xtf.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);
				
				XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);
				
				XPoint[] xps3 = new XPoint[2] { new XPoint(150, 395), new XPoint(150, 415) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

				XPoint[] xps4 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 415) };
				gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);
				
				XRect xr5 = new XRect(new XPoint(100, 440), new XPoint(480, 485));
				gfx.DrawRectangle(XBrushes.Transparent, xr5);
				xtf.DrawString(asp.Rows[0]["KETERANGAN"].ToString(), fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
			}
		}
		else
		{
			XRect xr1 = new XRect(new XPoint(20, 385), new XPoint(250, 395));
			gfx.DrawRectangle(XBrushes.Transparent, xr1);
			xtf.DrawString("After Sales Policy / Kebijakan Penjualan", fontParagraph, XBrushes.Black, xr1, XStringFormats.TopLeft);
			
			xtf.Alignment = XParagraphAlignment.Center;
			
			XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 485), new XPoint(20, 485), new XPoint(20, 395) };
			gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
			
			XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(150, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr2);
			xtf.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

			XRect xr3 = new XRect(new XPoint(151, 395), new XPoint(420, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr3);
			xtf.DrawString("TRADE IN / TUKAR TAMBAH", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);

			XRect xr4 = new XRect(new XPoint(421, 395), new XPoint(580, 415));
			gfx.DrawRectangle(XBrushes.Transparent, xr4);
			xtf.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);
			
			XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);
			
			XPoint[] xps3 = new XPoint[2] { new XPoint(150, 395), new XPoint(150, 415) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

			XPoint[] xps4 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 415) };
			gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);
			
			XRect xr5 = new XRect(new XPoint(100, 440), new XPoint(480, 485));
			gfx.DrawRectangle(XBrushes.Transparent, xr5);
			xtf.DrawString("No Exchange - No Resell. Tidak dapat ditukar dan Tidak dapat dijual kembali.", fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
		}
	}
	
	protected static void WriteNotes(XGraphics gfx, XFont fontParagraph, XTextFormatter xtf, int enter, string brand)
	{
		XFont fontParagraphBold = new XFont("Consolas", 8, XFontStyle.Bold);
		XRect xr1 = new XRect(new XPoint(20, 385 + enter), new XPoint(580, 395 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr1);
        xtf.DrawString("Notes / Catatan", fontParagraphBold, XBrushes.Black, xr1, XStringFormats.TopLeft);
		
		string r2 = "1. All Jewellery pieces are checked to be in good condition and accompanied by original certificate / Perhiasan telah diperiksa dan dalam kondisi baik dan disertai sertifikat asli";
		XRect xr2 = new XRect(new XPoint(20, 395 + enter), new XPoint(580, 405 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr2);
        xtf.DrawString(r2, fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);
		
		string r3 = "2. Price including VAT 2 % / Harga Jual Termasuk PPN 2 %";
		XRect xr3 = new XRect(new XPoint(20, 405 + enter), new XPoint(580, 415 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr3);
        xtf.DrawString(r3, fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);
		
		string r4 = "3. Original Sales Amount = Price Exclude VAT / Nilai Penjualan Asal = Harga Jual Tidak Termasuk PPN";
		XRect xr4 = new XRect(new XPoint(20, 415 + enter), new XPoint(580, 425 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr4);
        xtf.DrawString(r4, fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);
		
		string r5 = "4. Frank & Co. guarantees that within 14 (fourteen) days of purchase from us, should a customer purchase an identical piece of " + 
					"    diamond jewellery item from another Indonesia based registered jeweler at lower price, " + brand + " will refund in full at   " + 
					"    purchase price. " + brand + " menjamin bahwa dalam waktu kurang dari 14 (empat belas) hari dari pembelian, jika pelanggan     " + 
					"    membeli sebuah perhiasan berlian yang sama dari toko perhiasan lain di Indonesia yang terdaftar dengan harga yang lebih     " +
					"    murah, " + brand + " akan mengembalikan keseluruhan uang dari pembelian";
		XRect xr5 = new XRect(new XPoint(20, 425 + enter), new XPoint(580, 475 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr5);
        xtf.DrawString(r5, fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
		
		string r6 = "5. Resell payment will be done by transferring to customer's bank account minimum in 2 working days / Pembayaran Resell akan   " + 
		"    dilakukan melalui transfer bank ke rekening pelanggan minimal 2 hari kerja";
		XRect xr6 = new XRect(new XPoint(20, 475 + enter), new XPoint(580, 495 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr4);
        xtf.DrawString(r6, fontParagraph, XBrushes.Black, xr6, XStringFormats.TopLeft);
	}
	
	protected static void WriteNotesInvoice(XGraphics gfx, XFont fontParagraph, XTextFormatter xtf, string brand)
	{
		XFont fontParagraphBold = new XFont("Consolas", 8, XFontStyle.Bold);
		XRect xr1 = new XRect(new XPoint(20, 495), new XPoint(580, 505));
        gfx.DrawRectangle(XBrushes.Transparent, xr1);
        xtf.DrawString("Notes / Catatan", fontParagraphBold, XBrushes.Black, xr1, XStringFormats.TopLeft);
		
		string r2 = "1. All Jewellery pieces are checked to be in good condition and accompanied by original certificate / Perhiasan telah diperiksa dan dalam kondisi baik dan disertai sertifikat asli";
		XRect xr2 = new XRect(new XPoint(20, 505), new XPoint(580, 515));
        gfx.DrawRectangle(XBrushes.Transparent, xr2);
        xtf.DrawString(r2, fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);
		
		string r3 = "2. Price including VAT 2 % / Harga Jual Termasuk PPN 2 %";
		XRect xr3 = new XRect(new XPoint(20, 515), new XPoint(580, 525));
        gfx.DrawRectangle(XBrushes.Transparent, xr3);
        xtf.DrawString(r3, fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);
		
		string r4 = "3. Original Sales Amount = Price Exclude VAT / Nilai Penjualan Asal = Harga Jual Tidak Termasuk PPN";
		XRect xr4 = new XRect(new XPoint(20, 525), new XPoint(580, 535));
        gfx.DrawRectangle(XBrushes.Transparent, xr4);
        xtf.DrawString(r4, fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);
		
		string r5 = "";
		if(brand == "Frank & co.")
		{
			r5 = "4. " + brand + " guarantees that within 14 (fourteen) days of purchase from us, should a customer purchase an identical piece of " + 
						"    diamond jewellery item from another Indonesia based registered jeweler at lower price, " + brand + " will refund in full at   " + 
						"    purchase price. " + brand + " menjamin bahwa dalam waktu kurang dari 14 (empat belas) hari dari pembelian, jika pelanggan     " + 
						"    membeli sebuah perhiasan berlian yang sama dari toko perhiasan lain di Indonesia yang terdaftar dengan harga yang lebih     " +
			"    murah, " + brand + " akan mengembalikan keseluruhan uang dari pembelian";
		}
		else if(brand == "The Palace")
		{
			r5 = "4. " + brand + " guarantees that within 14 (fourteen) days of purchase from us, should a customer purchase an identical piece of " + 
						"    diamond jewellery item from another Indonesia based registered jeweler at lower price, " + brand + " will refund in full at   " + 
						"    purchase price. " + brand + " menjamin bahwa dalam waktu kurang dari 14 (empat belas) hari dari pembelian, jika pelanggan     " + 
						"    membeli sebuah perhiasan berlian yang sama dari toko perhiasan lain di Indonesia yang terdaftar dengan harga yang lebih     " +
			"    murah, " + brand + " akan mengembalikan keseluruhan uang dari pembelian";
		}
		else if(brand == "Mondial" || brand == "Miss Mondial")
		{
			r5 = "4. " + brand + " guarantees that within 14 (fourteen) days of purchase from us, should a customer purchase an identical piece of " + 
						"    diamond jewellery item from another Indonesia based registered jeweler at lower price, " + brand + " will refund in full at   " + 
						"    purchase price. " + brand + " menjamin bahwa dalam waktu kurang dari 14 (empat belas) hari dari pembelian, jika pelanggan     " + 
						"    membeli sebuah perhiasan berlian yang sama dari toko perhiasan lain di Indonesia yang terdaftar dengan harga yang lebih     " +
			"    murah, " + brand + " akan mengembalikan keseluruhan uang dari pembelian";
		}
		
		XRect xr5 = new XRect(new XPoint(20, 535), new XPoint(580, 585));
        gfx.DrawRectangle(XBrushes.Transparent, xr5);
        xtf.DrawString(r5, fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
		
		string r6 = "5. Resell payment will be done by transferring to customer's bank account minimum in 2 working days / Pembayaran Resell akan   " + 
		"    dilakukan melalui transfer bank ke rekening pelanggan minimal 2 hari kerja";
		XRect xr6 = new XRect(new XPoint(20, 585), new XPoint(580, 605));
        gfx.DrawRectangle(XBrushes.Transparent, xr4);
        xtf.DrawString(r6, fontParagraph, XBrushes.Black, xr6, XStringFormats.TopLeft);
		
		string r7 = "5. Resell payment will be done by transferring to customer's bank account minimum in 2 working days / Pembayaran Resell akan   " + 
		"    dilakukan melalui transfer bank ke rekening pelanggan minimal 2 hari kerja";
		XRect xr7 = new XRect(new XPoint(20, 605), new XPoint(580, 655));
        gfx.DrawRectangle(XBrushes.Transparent, xr7);
        xtf.DrawString(r7, fontParagraph, XBrushes.Black, xr7, XStringFormats.TopLeft);
	}
	
	protected static void WriteDeclaration(XGraphics gfx, XFont fontParagraph, XTextFormatter xtf, int enter)
    {
		XFont fontParagraphBold = new XFont("Consolas", 8, XFontStyle.Bold);
		XRect xr1 = new XRect(new XPoint(20, 505 + enter), new XPoint(150, 515 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr1);
        xtf.DrawString("Declaration", fontParagraphBold, XBrushes.Black, xr1, XStringFormats.TopLeft);
        
		XPoint[] xps = new XPoint[5] { new XPoint(20, 515 + enter), new XPoint(580, 515 + enter), new XPoint(580, 575 + enter), new XPoint(20, 575 + enter), new XPoint(20, 515 + enter) };
        gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
		
		string r2 = "1. The diamonds herein invoiced have been purchased from legitimate sources not involved in funding conflict and in compliance " + 
		"    with United Nations Resolutions. The seller hereby guarantees that these diamonds are conflict-free, based on the seller's  " + 
		"    knowledge and/or written guarantees provided by the supplier of these diamonds";
		XRect xr2 = new XRect(new XPoint(20, 515 + enter), new XPoint(580, 545 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr2);
        xtf.DrawString(r2, fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);
		
		string r3 = "2. To the best of our knowledge and/or written assurance from our suppliers, we state that \"Diamonds invoiced herein are " + 
		"          natural and no properties of synthetics or CVD were noticed\". The diamonds have been checked and verified by the customer at " + 
		"   the time of delivery and we shall not be responsible for anything once goods leave our premises";
		XRect xr3 = new XRect(new XPoint(20, 545 + enter), new XPoint(580, 575 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr3);
        xtf.DrawString(r3, fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);
    }
	
	protected static void WriteDeclarationInvoice(XGraphics gfx, XFont fontParagraph, XTextFormatter xtf)
    {
		XFont fontParagraphBold = new XFont("Consolas", 8, XFontStyle.Bold);
		XRect xr1 = new XRect(new XPoint(20, 615), new XPoint(150, 625));
        gfx.DrawRectangle(XBrushes.Transparent, xr1);
        xtf.DrawString("Declaration", fontParagraphBold, XBrushes.Black, xr1, XStringFormats.TopLeft);
        
		XPoint[] xps = new XPoint[5] { new XPoint(20, 625), new XPoint(580, 625), new XPoint(580, 685), new XPoint(20, 685), new XPoint(20, 625) };
        gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);
		
		string r2 = "1. The diamonds herein invoiced have been purchased from legitimate sources not involved in funding conflict and in compliance " + 
		"    with United Nations Resolutions. The seller hereby guarantees that these diamonds are conflict-free, based on the seller's  " + 
		"    knowledge and/or written guarantees provided by the supplier of these diamonds";
		XRect xr2 = new XRect(new XPoint(20, 625), new XPoint(580, 655));
        gfx.DrawRectangle(XBrushes.Transparent, xr2);
        xtf.DrawString(r2, fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);
		
		string r3 = "2. To the best of our knowledge and/or written assurance from our suppliers, we state that \"Diamonds invoiced herein are " + 
		"          natural and no properties of synthetics or CVD were noticed\". The diamonds have been checked and verified by the customer at " + 
		"   the time of delivery and we shall not be responsible for anything once goods leave our premises";
		XRect xr3 = new XRect(new XPoint(20, 655), new XPoint(580, 685));
        gfx.DrawRectangle(XBrushes.Transparent, xr3);
        xtf.DrawString(r3, fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);
    }

	protected static DataSet QuerySPDataSet(string query, string connectionString)
	{
		DataSet ds = new DataSet();
		using (SqlConnection conn = new SqlConnection(connectionString))
		{
			conn.Open();
			SqlTransaction trans;
			trans = conn.BeginTransaction();
			try
			{
				SqlDataAdapter sda = new SqlDataAdapter(query, connectionString);
				sda.SelectCommand.CommandTimeout = 3000;
				sda.Fill(ds);
				sda.Dispose();
				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				string ErrorMessage = ex.Message.ToString();
				trans.Rollback();
				conn.Close();
			}
		}
		return ds;
	}
	
	protected static string asp_text_period(string mode, int val, string time)
    {
        string x = "";
        string xtime_eng = "";
        string xtime_ina = "";

        if (val > 0)
        {
            if (time == "d")
            {
                xtime_eng = val > 1 ? "Days" : "Day";
                xtime_ina = "Hari";
            }
            else if (time == "m")
            {
                xtime_eng = val > 1 ? "Months" : "Month";
                xtime_ina = "Bulan";
            }
            else if (time == "y")
            {
                xtime_eng = val > 1 ? "Years" : "Year";
                xtime_ina = "Tahun";
            }

            x = " " + mode + " " + val.ToString() + " " + xtime_eng + " - " + val.ToString() + " " + xtime_ina;
        }


        return x;
    }

    protected static string asp_text_value(string sbu, string mode, decimal val, bool buyingprice = true)
    {
        string x = "";

        int mode2 = 1;

        if (mode == "me") { mode2 = 1; }
        else if (val == 100m && buyingprice && (mode == "ti" || mode == "rs")) { mode2 = 2; }
        else if (val == 0m) { mode2 = 3; }

        if (mode2 == 1)
        {
            x = val.ToString() + "% \n Original sales amount (Nilai Penjualan Asal)";
        }
        else if (mode2 == 2)
        {
            x = "\n" + sbu + " buying price (Harga beli " + sbu + ")";
        }
        else if (mode2 == 3)
        {
            if (mode == "ti")
            {
                x = "No Trade In (Tidak berlaku Tukar Tambah)";
            }
            else if (mode == "rs")
            {
                x = "No Option to Resell (Tidak ada Opsi Penjualan Kembali)";
            }
        }

        return x;
    }
}