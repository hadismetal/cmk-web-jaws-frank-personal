using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for InvoicePDF
/// </summary>
public partial class InvoicePDF
{
    #region PrintInvoice
    public string writeInvoice(int idsalesorder, int tipeproduct, int idproduct, string iduser = "ADMIN", string url = null)
    {
        using (Map_DB db = new Map_DB())
        {
            var so = db.SalesOrder.SingleOrDefault(p => p.ID == idsalesorder);
            string LocAddr = "";
            if (so.TipeLokasi == 1)
            {
                LocAddr = Db.SingleString("SELECT TOP 1 AddrAlamat FROM LocOutlet WHERE ID = " + so.IDLokasi, Db.Cnn_DB);
            }
            else if (so.TipeLokasi == 2)
            {
                LocAddr = Db.SingleString("SELECT TOP 1 AddrAlamat FROM LocExhibition WHERE ID = 14" + so.IDLokasi, Db.Cnn_DB);
            }

            PdfDocument doc = new PdfDocument();
            doc.Info.Title = "Invoice CMK";

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
            gfx.DrawString("INVOICE", fontParagraph2, XBrushes.Black, new XPoint(20, 30));
            gfx.DrawString("PT. CENTRAL MEGA KENCANA", fontParagraph, XBrushes.Black, new XPoint(20, 50));
            gfx.DrawString("NPWP : 01.764.536.7-073.000", fontParagraph3, XBrushes.Black, new XPoint(20, 65));

            XRect xr2 = new XRect(new XPoint(450, 20), new XPoint(580, 65));
            gfx.DrawRectangle(XBrushes.Transparent, xr2);
            xtfleft.DrawString(LocAddr, fontParagraph3S, XBrushes.Black, xr2, XStringFormats.TopLeft);

            gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.8), new XPoint(20, 70), new XPoint(580, 70));

            XRect xr3 = new XRect(new XPoint(20, 75), new XPoint(250, 85));
            gfx.DrawRectangle(XBrushes.Transparent, xr3);
            xtfleft.DrawString("No.                     : " + so.Nomor, fontParagraph3, XBrushes.Black, xr3, XStringFormats.TopLeft);

            XRect xr4 = new XRect(new XPoint(20, 85), new XPoint(250, 95));
            gfx.DrawRectangle(XBrushes.Transparent, xr4);
            xtfleft.DrawString("Date / Tanggal Faktur   : " + Convert.ToDateTime(so.Tgl).ToString("dd-MM-yyyy"), fontParagraph3, XBrushes.Black, xr4, XStringFormats.TopLeft);

            XRect xr5 = new XRect(new XPoint(310, 75), new XPoint(580, 85));
            gfx.DrawRectangle(XBrushes.Transparent, xr5);
            xtfleft.DrawString("Customer Code / Kode Pelanggan  : " + so.DataCustomer.NoCustomer, fontParagraph3, XBrushes.Black, xr5, XStringFormats.TopLeft);

            XRect xr6 = new XRect(new XPoint(310, 85), new XPoint(580, 95));
            gfx.DrawRectangle(XBrushes.Transparent, xr6);
            xtfleft.DrawString("To / Kepada                     : " + so.DataCustomer.Nama, fontParagraph3, XBrushes.Black, xr6, XStringFormats.TopLeft);

            XRect xr7 = new XRect(new XPoint(310, 95), new XPoint(580, 105));
            gfx.DrawRectangle(XBrushes.Transparent, xr7);
            xtfleft.DrawString("Given To / Diberikan Kepada     : " + (!string.IsNullOrEmpty(so.CustomerReferenceNama) ? so.CustomerReferenceNama : so.CustomerNama), fontParagraph3, XBrushes.Black, xr7, XStringFormats.TopLeft);

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
            xtfright.DrawString("Price / Harga Jual (Rp)", fontParagraph4, XBrushes.Black, xr11, XStringFormats.TopLeft);

            gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), new XPoint(20, 120), new XPoint(580, 120));

            Invoice product = new Invoice();
            if (tipeproduct == 1) //DJ
            {
                var pd = (from p in db.SalesOrderDJ
                          where p.IDForm == idsalesorder && p.IDProduct == idproduct
                          select new
                          {
                              ID = p.IDProduct,
                              IDSalesOrder = p.IDForm,
                              NomorSO = p.SalesOrder.Nomor,
                              TanggalSO = p.SalesOrder.Tgl,
                              PLU = p.StockProductDJ.Nomor,
                              InvoiceN = p.Invoice,
                              ProductItem = p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                              TotalBayar = p.TotalBayar ?? 0m,
                              ImageUrl = p.StockProductDJ.ImgPicture
                          }).SingleOrDefault();

                if (pd != null)
                {
                    product.ID = pd.ID;
                    product.IDSalesOrder = pd.IDSalesOrder;
                    product.NomorSO = pd.NomorSO;
                    product.TanggalSO = Convert.ToDateTime(pd.TanggalSO).ToString("dd-MM-yyyy");
                    product.Item = pd.ProductItem;
                    product.PLU = pd.PLU;
                    product.NomorInvoice = pd.InvoiceN;
                    product.Qty = 1;
                    product.Tipe = 1;
                    product.TotalBayar = pd.TotalBayar;
                    product.Image = pd.ImageUrl;
                }
            }
            else if (tipeproduct == 2)
            {
                var pd = (from p in db.SalesOrderPG
                          where p.IDForm == idsalesorder && p.IDProduct == idproduct
                          select new
                          {
                              ID = p.IDProduct,
                              IDSalesOrder = p.IDForm,
                              NomorSO = p.SalesOrder.Nomor,
                              TanggalSO = p.SalesOrder.Tgl,
                              PLU = p.StockProductPG.Nomor,
                              InvoiceN = p.Invoice,
                              ProductItem = p.StockProductPG.CharProductItem.Nama,
                              TotalBayar = p.TotalBayar ?? 0m,
                              ImageUrl = p.StockProductPG.ImgPicture
                          }).SingleOrDefault();

                if (pd != null)
                {
                    product.ID = pd.ID;
                    product.IDSalesOrder = pd.IDSalesOrder;
                    product.NomorSO = pd.NomorSO;
                    product.TanggalSO = Convert.ToDateTime(pd.TanggalSO).ToString("dd-MM-yyyy");
                    product.Item = pd.ProductItem;
                    product.PLU = pd.PLU;
                    product.NomorInvoice = pd.InvoiceN;
                    product.Qty = 1;
                    product.Tipe = 1;
                    product.TotalBayar = pd.TotalBayar;
                    product.Image = pd.ImageUrl;
                }
            }
            else if (tipeproduct == 3)
            {
                var pd = (from p in db.SalesOrderGJ
                          where p.IDForm == idsalesorder && p.IDProduct == idproduct
                          select new
                          {
                              ID = p.IDProduct,
                              IDSalesOrder = p.IDForm,
                              NomorSO = p.SalesOrder.Nomor,
                              TanggalSO = p.SalesOrder.Tgl,
                              PLU = p.StockProductGJ.Nomor,
                              InvoiceN = p.Invoice,
                              ProductItem = p.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
                              TotalBayar = p.TotalBayar ?? 0m,
                              ImageUrl = p.StockProductGJ.ImgPicture
                          }).SingleOrDefault();

                if (pd != null)
                {
                    product.ID = pd.ID;
                    product.IDSalesOrder = pd.IDSalesOrder;
                    product.NomorSO = pd.NomorSO;
                    product.TanggalSO = Convert.ToDateTime(pd.TanggalSO).ToString("dd-MM-yyyy");
                    product.Item = pd.ProductItem;
                    product.PLU = pd.PLU;
                    product.NomorInvoice = pd.InvoiceN;
                    product.Qty = 1;
                    product.Tipe = 1;
                    product.TotalBayar = pd.TotalBayar;
                    product.Image = pd.ImageUrl;
                }
            }
            else if (tipeproduct == 4)
            {
                var pd = (from p in db.SalesOrderLD
                          where p.IDForm == idsalesorder && p.IDProduct == idproduct
                          select new
                          {
                              ID = p.IDProduct,
                              IDSalesOrder = p.IDForm,
                              NomorSO = p.SalesOrder.Nomor,
                              TanggalSO = p.SalesOrder.Tgl,
                              PLU = p.StockProductLD_Stone1B.Nomor,
                              InvoiceN = p.Invoice,
                              ProductItem = p.StockProductLD_Stone1B.CharProductItem.Nama,
                              TotalBayar = p.TotalBayar ?? 0m,
                              ImageUrl = ""
                          }).SingleOrDefault();

                if (pd != null)
                {
                    product.ID = pd.ID;
                    product.IDSalesOrder = pd.IDSalesOrder;
                    product.NomorSO = pd.NomorSO;
                    product.TanggalSO = Convert.ToDateTime(pd.TanggalSO).ToString("dd-MM-yyyy");
                    product.Item = pd.ProductItem;
                    product.PLU = pd.PLU;
                    product.NomorInvoice = pd.InvoiceN;
                    product.Qty = 1;
                    product.Tipe = 1;
                    product.TotalBayar = pd.TotalBayar;
                    product.Image = pd.ImageUrl;
                }
            }
        
            decimal TotalBayar = 0m;
            //Item Desc
            XRect xr12 = new XRect(new XPoint(20, 120), new XPoint(100, 130));
            gfx.DrawRectangle(XBrushes.Transparent, xr12);
            xtfleft.DrawString(product.PLU, fontParagraph3, XBrushes.Black, xr12, XStringFormats.TopLeft);

            XRect xr13 = new XRect(new XPoint(105, 120), new XPoint(402, 130));
            gfx.DrawRectangle(XBrushes.Transparent, xr13);
            xtfleft.DrawString(product.Item, fontParagraph3, XBrushes.Black, xr13, XStringFormats.TopLeft);

            XRect xr14 = new XRect(new XPoint(403, 120), new XPoint(450, 130));
            gfx.DrawRectangle(XBrushes.Transparent, xr14);
            xtfcenter.DrawString(product.Qty.ToString(), fontParagraph3, XBrushes.Black, xr14, XStringFormats.TopLeft);

            XRect xr15 = new XRect(new XPoint(455, 120), new XPoint(580, 130));
            gfx.DrawRectangle(XBrushes.Transparent, xr15);
            xtfright.DrawString(product.TotalBayar.ToString("N0"), fontParagraph3, XBrushes.Black, xr15, XStringFormats.TopLeft);
        
            TotalBayar += product.TotalBayar;

            gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 1), new XPoint(20, 130), new XPoint(580, 130));

            XRect xr16 = new XRect(new XPoint(20, 132), new XPoint(250, 172));
            gfx.DrawRectangle(XBrushes.Transparent, xr16);
            xtfleft.DrawString("Say / Terbilang : " + Str.Terbilang(Convert.ToDecimal(product.TotalBayar.ToString("N0"))).TrimEnd().TrimStart(), fontParagraph3, XBrushes.Black, xr16, XStringFormats.TopLeft);

            /*XRect xr17 = new XRect(new XPoint(20, 182), new XPoint(250, 192));
            gfx.DrawRectangle(XBrushes.Transparent, xr17);
            xtfleft.DrawString("Detail of payment", fontParagraph3, XBrushes.Black, xr17, XStringFormats.TopLeft);*/

            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] bytes = wc.DownloadData(product.Image);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                XImage img = XImage.FromStream(ms); //XImage.FromStream(ms);        
                XRect xr18 = new XRect(new XPoint(300, 132), new XSize(50, 50));
                gfx.DrawImage(img, xr18);
                ms.Close();
            }

            decimal TotalBayarExcVAT = Math.Ceiling(TotalBayar / 1.02m);
            decimal PPN = TotalBayar - TotalBayarExcVAT;

            XPoint[] xps = new XPoint[5] { new XPoint(400, 132), new XPoint(580, 132), new XPoint(580, 270), new XPoint(400, 270), new XPoint(400, 132) };

            gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);

            XRect xr19 = new XRect(new XPoint(403, 135), new XPoint(500, 165));
            gfx.DrawRectangle(XBrushes.Transparent, xr19);
            xtfleft.DrawString("Price Exclude VAT / Harga Jual Tidak Termasuk PPN", fontParagraph3, XBrushes.Black, xr19, XStringFormats.TopLeft);

            XRect xr20 = new XRect(new XPoint(403, 175), new XPoint(500, 205));
            gfx.DrawRectangle(XBrushes.Transparent, xr20);
            xtfleft.DrawString("VAT / PPN (10% x 20% x Harga Jual Tidak Termasuk PPN )", fontParagraph3, XBrushes.Black, xr20, XStringFormats.TopLeft);

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
            xtfright.DrawString(TotalBayarExcVAT.ToString("N0"), fontParagraph3, XBrushes.Black, xr24, XStringFormats.TopLeft);

            XRect xr25 = new XRect(new XPoint(505, 175), new XPoint(577, 205));
            gfx.DrawRectangle(XBrushes.Transparent, xr25);
            xtfright.DrawString(PPN.ToString("N0"), fontParagraph3, XBrushes.Black, xr25, XStringFormats.TopLeft);

            XRect xr26 = new XRect(new XPoint(505, 215), new XPoint(577, 225));
            gfx.DrawRectangle(XBrushes.Transparent, xr26);
            xtfright.DrawString(TotalBayar.ToString("N0"), fontParagraph3, XBrushes.Black, xr26, XStringFormats.TopLeft);

            XRect xr27 = new XRect(new XPoint(505, 235), new XPoint(577, 245));
            gfx.DrawRectangle(XBrushes.Transparent, xr27);
            xtfright.DrawString(TotalBayar.ToString("N0"), fontParagraph3, XBrushes.Black, xr27, XStringFormats.TopLeft);

            XRect xr28 = new XRect(new XPoint(505, 255), new XPoint(577, 265));
            gfx.DrawRectangle(XBrushes.Transparent, xr28);
            xtfright.DrawString("0", fontParagraph3, XBrushes.Black, xr28, XStringFormats.TopLeft);

            XRect xr29 = new XRect(new XPoint(373, 350), new XPoint(470, 360));
            gfx.DrawRectangle(XBrushes.Transparent, xr29);
            xtfcenter.DrawString(so.DataCustomer.Nama, fontParagraph4, XBrushes.Black, xr29, XStringFormats.TopLeft);

            XRect xr30 = new XRect(new XPoint(475, 350), new XPoint(577, 360));
            gfx.DrawRectangle(XBrushes.Transparent, xr30);
            xtfcenter.DrawString(so.SalesNama, fontParagraph4, XBrushes.Black, xr30, XStringFormats.TopLeft);

            XRect xr31 = new XRect(new XPoint(373, 360), new XPoint(470, 370));
            gfx.DrawRectangle(XBrushes.Transparent, xr31);
            xtfcenter.DrawString("(Customer / Pelanggan)", fontParagraph3, XBrushes.Black, xr31, XStringFormats.TopLeft);

            XRect xr32 = new XRect(new XPoint(475, 360), new XPoint(577, 370));
            gfx.DrawRectangle(XBrushes.Transparent, xr32);
            xtfcenter.DrawString("(JR / Staff Penjualan)", fontParagraph3, XBrushes.Black, xr32, XStringFormats.TopLeft);

            writeASP(gfx, fontParagraph3, fontParagraph5, xtfleft, xtfcenter, 10, "", tipeproduct, idproduct, TotalBayar);

    
            // Save the document...190116
            string filename = product.NomorInvoice.Replace("/", "_") + ".pdf";
            string foldername = url + "/INVOICE/" + iduser;
            if (!Directory.Exists(foldername))
            {
                Directory.CreateDirectory(foldername);
            }

            string SaveLocation = foldername + "\\" + filename;
            if (File.Exists(SaveLocation))
            {
                File.Delete(SaveLocation);
            }
            doc.Save(SaveLocation);
            // ...and start a viewer.
            //System.Diagnostics.Process.Start(SaveLocation);
            return SaveLocation;
            }
}

    private void writeASP(XGraphics gfx, XFont fontParagraph, XFont fontParagraphItalic, XTextFormatter xtf, XTextFormatter xtfcenter, int enter, string brand, int tipeproduct, int idproduct, decimal totalbayar = 0)
    {
        XRect xr1 = new XRect(new XPoint(20, 385), new XPoint(450, 395));
        gfx.DrawRectangle(XBrushes.Transparent, xr1);
        xtf.DrawString("After Sales Policy / Kebijakan Penjualan", fontParagraph, XBrushes.Black, xr1, XStringFormats.TopLeft);

        if (tipeproduct == 1) //DJ
        {
            using (Map_DB db = new Map_DB())
            {
                var d = db.StockProductDJ.SingleOrDefault(p => p.ID == idproduct);
                string[] asp = ASPProduct.getASPDJParameter(d.ID, totalbayar);

                XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 465), new XPoint(20, 465), new XPoint(20, 395) };
                gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);

                XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(170, 415));
                gfx.DrawRectangle(XBrushes.Transparent, xr2);
                xtfcenter.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

                XRect xr3 = new XRect(new XPoint(171, 395), new XPoint(420, 415));
                gfx.DrawRectangle(XBrushes.Transparent, xr3);
                xtfcenter.DrawString("TRADE IN / TUKAR TAMBAH", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);

                XRect xr4 = new XRect(new XPoint(421, 395), new XPoint(580, 415));
                gfx.DrawRectangle(XBrushes.Transparent, xr4);
                xtfcenter.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);

                XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
                gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);

                if (string.IsNullOrEmpty(asp[0]))
                {
                    string keterangan = "";

                    XPoint[] xps3 = new XPoint[2] { new XPoint(170, 395), new XPoint(170, 415) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

                    XPoint[] xps4 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 415) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);

                    XRect xr5 = new XRect(new XPoint(20, 420), new XPoint(580, 445));
                    gfx.DrawRectangle(XBrushes.Transparent, xr5);
                    xtfcenter.DrawString(keterangan, fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
                }
                else
                {
                    XPoint[] xps3 = new XPoint[2] { new XPoint(170, 395), new XPoint(170, 426) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

                    XPoint[] xps4 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 426) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);

                    XPoint[] xps2A = new XPoint[2] { new XPoint(20, 425), new XPoint(580, 425) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2A[0], xps2A[1]);

                    XPoint[] xps2B = new XPoint[2] { new XPoint(20, 435), new XPoint(580, 435) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2B[0], xps2B[1]);

                    XRect xr6 = new XRect(new XPoint(21, 415), new XPoint(170, 425));
                    gfx.DrawRectangle(XBrushes.Transparent, xr6);
                    xtfcenter.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr6, XStringFormats.TopLeft);

                    XRect xr7 = new XRect(new XPoint(171, 415), new XPoint(420, 425));
                    gfx.DrawRectangle(XBrushes.Transparent, xr7);
                    xtfcenter.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr7, XStringFormats.TopLeft);

                    XRect xr8 = new XRect(new XPoint(421, 415), new XPoint(580, 425));
                    gfx.DrawRectangle(XBrushes.Transparent, xr8);
                    xtfcenter.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr8, XStringFormats.TopLeft);

                    XPoint[] xps5 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps5[0], xps5[1]);

                    XFont fontParagraphsmall = new XFont("Consolas", 6.5, XFontStyle.Regular);

                    XRect xr9 = new XRect(new XPoint(21, 426), new XPoint(170, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr9);
                    xtfcenter.DrawString("< 1 Month - 1 Bulan", fontParagraphsmall, XBrushes.Black, xr9, XStringFormats.TopLeft);

                    XRect xr10 = new XRect(new XPoint(171, 426), new XPoint(254, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr10);
                    xtfcenter.DrawString("< 1 Year - 1 Tahun", fontParagraphsmall, XBrushes.Black, xr10, XStringFormats.TopLeft);

                    XRect xr11 = new XRect(new XPoint(255, 426), new XPoint(338, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr11);
                    xtfcenter.DrawString("< 2 Years - 2 Tahun", fontParagraphsmall, XBrushes.Black, xr11, XStringFormats.TopLeft);

                    XRect xr12 = new XRect(new XPoint(339, 426), new XPoint(420, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr12);
                    xtfcenter.DrawString("> 2 Years - 2 Tahun", fontParagraphsmall, XBrushes.Black, xr12, XStringFormats.TopLeft);

                    XRect xr13 = new XRect(new XPoint(421, 426), new XPoint(506, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr13);
                    xtfcenter.DrawString("< 1 Year - 1 Tahun", fontParagraphsmall, XBrushes.Black, xr13, XStringFormats.TopLeft);

                    XRect xr14 = new XRect(new XPoint(507, 426), new XPoint(580, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr14);
                    xtfcenter.DrawString("> 1 Year - 1 Tahun", fontParagraphsmall, XBrushes.Black, xr14, XStringFormats.TopLeft);

                    XRect xr15 = new XRect(new XPoint(21, 435), new XPoint(170, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr15);
                    xtfcenter.DrawString(asp[2] + "%" + Environment.NewLine + "Original sales amount" + Environment.NewLine + "(Nilai Penjualan Asal)", fontParagraphsmall, XBrushes.Black, xr15, XStringFormats.TopLeft);

                    XRect xr16 = new XRect(new XPoint(171, 435), new XPoint(254, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr16);
                    xtfcenter.DrawString(asp[5] + "%" + Environment.NewLine + "Original sales amount" + Environment.NewLine + "(Nilai Penjualan Asal)", fontParagraphsmall, XBrushes.Black, xr16, XStringFormats.TopLeft);

                    XRect xr17 = new XRect(new XPoint(255, 435), new XPoint(338, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr17);
                    xtfcenter.DrawString(asp[8] + "%" + Environment.NewLine + "Original sales amount" + Environment.NewLine + "(Nilai Penjualan Asal)", fontParagraphsmall, XBrushes.Black, xr17, XStringFormats.TopLeft);

                    XRect xr18 = new XRect(new XPoint(339, 435), new XPoint(420, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr18);
                    xtfcenter.DrawString(Environment.NewLine + brand + " buying price" + Environment.NewLine + "(Harga beli " + brand + ")", fontParagraphsmall, XBrushes.Black, xr18, XStringFormats.TopLeft);

                    XRect xr19 = new XRect(new XPoint(421, 435), new XPoint(506, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr19);
                    xtfcenter.DrawString(asp[14] + "%" + Environment.NewLine + "Original sales amount" + Environment.NewLine + "(Nilai Penjualan Asal)", fontParagraphsmall, XBrushes.Black, xr19, XStringFormats.TopLeft);

                    XRect xr20 = new XRect(new XPoint(507, 435), new XPoint(580, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr20);
                    xtfcenter.DrawString(Environment.NewLine + brand + " buying price" + Environment.NewLine + "Harga beli " + brand + ")", fontParagraphsmall, XBrushes.Black, xr20, XStringFormats.TopLeft);

                    XPoint[] xps6 = new XPoint[2] { new XPoint(170, 426), new XPoint(170, 465) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps6[0], xps6[1]);

                    XPoint[] xps7 = new XPoint[2] { new XPoint(254, 426), new XPoint(254, 465) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps7[0], xps7[1]);

                    XPoint[] xps8 = new XPoint[2] { new XPoint(338, 426), new XPoint(338, 465) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps8[0], xps8[1]);

                    XPoint[] xps9 = new XPoint[2] { new XPoint(420, 426), new XPoint(420, 465) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps9[0], xps9[1]);

                    XPoint[] xps10 = new XPoint[2] { new XPoint(506, 426), new XPoint(506, 465) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps10[0], xps10[1]);
                }
                    }
}
        else if (tipeproduct == 2)
        {
            using (Map_DB db = new Map_DB())
            {
                var d = db.StockProductPG.SingleOrDefault(p => p.ID == idproduct);
                string[] asp = ASPProduct.getASPPGParameter();

                if (d.LocWarehouse.CompanyBrand.NamaKode.ToUpper() != "P")
                {
                    XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 465), new XPoint(20, 465), new XPoint(20, 395) };
                    gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);

                    XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(170, 415));
                    gfx.DrawRectangle(XBrushes.Transparent, xr2);
                    xtfcenter.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

                    XRect xr3 = new XRect(new XPoint(171, 395), new XPoint(420, 415));
                    gfx.DrawRectangle(XBrushes.Transparent, xr3);
                    xtfcenter.DrawString("TRADE IN / TUKAR TAMBAH", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);

                    XRect xr4 = new XRect(new XPoint(421, 395), new XPoint(580, 415));
                    gfx.DrawRectangle(XBrushes.Transparent, xr4);
                    xtfcenter.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);

                    XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);

                    string bren = "";
                    if (d.LocWarehouse.CompanyBrand.NamaKode.ToUpper().StartsWith("P"))
                        bren = "The Palace";
                    else if (d.LocWarehouse.CompanyBrand.NamaKode.ToUpper().StartsWith("M"))
                        bren = "Mondial";
                    else if (d.LocWarehouse.CompanyBrand.NamaKode.ToUpper().StartsWith("S"))
                        bren = "Miss Mondial";
                    else
                        bren = "Frank & Co";

                    string keterangan = "Only gold value calculated based on " + bren + " today's buy back gold price " +
                            "Hanya nilai emas yang diperhitungkan berdasarkan harga beli " + bren + " pada hari pengembalian";

                    if (string.IsNullOrEmpty(asp[0]))
                    {
                        XPoint[] xps3 = new XPoint[2] { new XPoint(170, 395), new XPoint(170, 415) };
                        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

                        XPoint[] xps4 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 415) };
                        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);

                        XRect xr5 = new XRect(new XPoint(25, 420), new XPoint(585, 445));
                        gfx.DrawRectangle(XBrushes.Transparent, xr5);
                        xtfcenter.DrawString(keterangan, fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
                    }
                    else
                    {
                        XPoint[] xps3 = new XPoint[2] { new XPoint(170, 395), new XPoint(170, 415) };
                        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

                        XPoint[] xps4 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 415) };
                        gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);

                        XRect xr5 = new XRect(new XPoint(25, 420), new XPoint(585, 445));
                        gfx.DrawRectangle(XBrushes.Transparent, xr5);
                        xtfcenter.DrawString(keterangan, fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
                    }
                }
                else
                {
                    XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(580, 395), new XPoint(580, 465), new XPoint(20, 465), new XPoint(20, 395) };
                    gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);

                    XRect xr2 = new XRect(new XPoint(21 + 5, 395), new XPoint(162 - 5, 415));
                    gfx.DrawRectangle(XBrushes.Transparent, xr2);
                    xtfcenter.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

                    XRect xr3 = new XRect(new XPoint(163 + 5, 395), new XPoint(305 - 5, 415));
                    gfx.DrawRectangle(XBrushes.Transparent, xr3);
                    xtfcenter.DrawString("TRADE IN PG to PG / TUKAR TAMBAH PG ke PG", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);

                    XRect xr4 = new XRect(new XPoint(306 + 5, 395), new XPoint(447 - 5, 415));
                    gfx.DrawRectangle(XBrushes.Transparent, xr4);
                    xtfcenter.DrawString("TRADE IN PG to DJ / TUKAR TAMBAH PG ke DJ", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);

                    XRect xr4A = new XRect(new XPoint(448 + 5, 395), new XPoint(580 - 5, 415));
                    gfx.DrawRectangle(XBrushes.Transparent, xr4A);
                    xtfcenter.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr4A, XStringFormats.TopLeft);

                    XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);

    
                    XPoint[] xps3 = new XPoint[2] { new XPoint(162, 395), new XPoint(162, 465) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

                    XPoint[] xps4 = new XPoint[2] { new XPoint(305, 395), new XPoint(305, 465) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);

                    XPoint[] xps4A = new XPoint[2] { new XPoint(447, 395), new XPoint(447, 465) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4A[0], xps4A[1]);

                    /*XRect xr5 = new XRect(new XPoint(20, 420), new XPoint(590, 445));
                    gfx.DrawRectangle(XBrushes.Transparent, xr5);
                    xtfcenter.DrawString(ASP.ASPPG.Keterangan, fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
                    */

                    XRect xr6 = new XRect(new XPoint(21, 415), new XPoint(162, 425));
                    gfx.DrawRectangle(XBrushes.Transparent, xr6);
                    xtfcenter.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr6, XStringFormats.TopLeft);

                    XRect xr7 = new XRect(new XPoint(163, 415), new XPoint(305, 425));
                    gfx.DrawRectangle(XBrushes.Transparent, xr7);
                    xtfcenter.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr7, XStringFormats.TopLeft);

                    XRect xr8 = new XRect(new XPoint(306, 415), new XPoint(447, 425));
                    gfx.DrawRectangle(XBrushes.Transparent, xr8);
                    xtfcenter.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr8, XStringFormats.TopLeft);

                    XRect xr8A = new XRect(new XPoint(448, 415), new XPoint(580, 425));
                    gfx.DrawRectangle(XBrushes.Transparent, xr8A);
                    xtfcenter.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr8A, XStringFormats.TopLeft);

                    XRect xr6B = new XRect(new XPoint(21, 425), new XPoint(162, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr6B);
                    xtfcenter.DrawString("<= 2 Weeks - 2 Minggu", fontParagraph, XBrushes.Black, xr6B, XStringFormats.TopLeft);

                    XRect xr7B = new XRect(new XPoint(163, 425), new XPoint(305, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr7B);
                    xtfcenter.DrawString("> 2 Weeks - 2 Minggu", fontParagraph, XBrushes.Black, xr7B, XStringFormats.TopLeft);

                    XRect xr8B = new XRect(new XPoint(306, 425), new XPoint(447, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr8B);
                    xtfcenter.DrawString("At anytime - Kapan pun", fontParagraph, XBrushes.Black, xr8B, XStringFormats.TopLeft);

                    XRect xr8AB = new XRect(new XPoint(448, 425), new XPoint(580, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr8AB);
                    xtfcenter.DrawString("At anytime - Kapan pun", fontParagraph, XBrushes.Black, xr8AB, XStringFormats.TopLeft);

                    XRect xr6C = new XRect(new XPoint(21, 436 + 5), new XPoint(162, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr6C);
                    xtfcenter.DrawString("100% Original Sales Amount - Nilai Penjualan Asal", fontParagraph, XBrushes.Black, xr6C, XStringFormats.TopLeft);

                    XRect xr7C = new XRect(new XPoint(163, 436 + 5), new XPoint(305, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr7C);
                    xtfcenter.DrawString("Today Buying Price The Palace", fontParagraph, XBrushes.Black, xr7C, XStringFormats.TopLeft);

                    XRect xr8C = new XRect(new XPoint(306, 436 + 5), new XPoint(447, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr8C);
                    xtfcenter.DrawString("Today Selling Price The Palace", fontParagraph, XBrushes.Black, xr8C, XStringFormats.TopLeft);

                    XRect xr8AC = new XRect(new XPoint(448, 436 + 5), new XPoint(580, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr8AC);
                    xtfcenter.DrawString("Today Buying Price The Palace", fontParagraph, XBrushes.Black, xr8AC, XStringFormats.TopLeft);

                    XPoint[] xps5 = new XPoint[2] { new XPoint(20, 415), new XPoint(580, 415) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps5[0], xps5[1]);

                    XPoint[] xps5A = new XPoint[2] { new XPoint(20, 425), new XPoint(580, 425) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps5A[0], xps5A[1]);

                    XPoint[] xps5B = new XPoint[2] { new XPoint(20, 435), new XPoint(580, 435) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps5B[0], xps5B[1]);

                }
                    }
}
        else if (tipeproduct == 3)
        {
            using (Map_DB db = new Map_DB())
            {
                var d = db.StockProductGJ.SingleOrDefault(p => p.ID == idproduct);
                string[] asp = ASPProduct.getASPGJParameter(d, false);

                XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(590, 395), new XPoint(590, 465), new XPoint(20, 465), new XPoint(20, 395) };
                gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);

                XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(170, 415));
                gfx.DrawRectangle(XBrushes.Transparent, xr2);
                xtfcenter.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

                XRect xr3 = new XRect(new XPoint(171, 395), new XPoint(420, 415));
                gfx.DrawRectangle(XBrushes.Transparent, xr3);
                xtfcenter.DrawString("TRADE IN / TUKAR TAMBAH", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);

                XRect xr4 = new XRect(new XPoint(421, 395), new XPoint(590, 415));
                gfx.DrawRectangle(XBrushes.Transparent, xr4);
                xtfcenter.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);

                XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(590, 415) };
                gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);

                if (string.IsNullOrEmpty(asp[0]))
                {
                    string keterangan = "";
                    XRect xr5 = new XRect(new XPoint(20, 395), new XPoint(590, 445));
                    gfx.DrawRectangle(XBrushes.Transparent, xr5);
                    xtfcenter.DrawString(keterangan, fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
                }
                    }
}
        else if (tipeproduct == 4)
        {
            using (Map_DB db = new Map_DB())
            {
                var d = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == idproduct);
                string[] asp = ASPProduct.getASPGJParameter(1, false);

                XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(590, 395), new XPoint(590, 465), new XPoint(20, 465), new XPoint(20, 395) };
                gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);

                XRect xr2 = new XRect(new XPoint(21, 395), new XPoint(170, 415));
                gfx.DrawRectangle(XBrushes.Transparent, xr2);
                xtfcenter.DrawString("MERCHENDISE EXCHANGE / PENUKARAN BARANG", fontParagraph, XBrushes.Black, xr2, XStringFormats.TopLeft);

                XRect xr3 = new XRect(new XPoint(171, 395), new XPoint(420, 415));
                gfx.DrawRectangle(XBrushes.Transparent, xr3);
                xtfcenter.DrawString("TRADE IN / TUKAR TAMBAH", fontParagraph, XBrushes.Black, xr3, XStringFormats.TopLeft);

                XRect xr4 = new XRect(new XPoint(421, 395), new XPoint(590, 415));
                gfx.DrawRectangle(XBrushes.Transparent, xr4);
                xtfcenter.DrawString("CUSTOMER'S OPTION TO RESELL / OPSI PENJUALAN KEMBALI", fontParagraph, XBrushes.Black, xr4, XStringFormats.TopLeft);

                XPoint[] xps2 = new XPoint[2] { new XPoint(20, 415), new XPoint(590, 415) };
                gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2[0], xps2[1]);

                if (string.IsNullOrEmpty(asp[0]))
                {
                    string keterangan = "";

                    XRect xr5 = new XRect(new XPoint(20, 395), new XPoint(590, 445));
                    gfx.DrawRectangle(XBrushes.Transparent, xr5);
                    xtfcenter.DrawString(keterangan, fontParagraph, XBrushes.Black, xr5, XStringFormats.TopLeft);
                }
                else
                {
                    XPoint[] xps3 = new XPoint[2] { new XPoint(170, 395), new XPoint(170, 426) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps3[0], xps3[1]);

                    XPoint[] xps4 = new XPoint[2] { new XPoint(420, 395), new XPoint(420, 426) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps4[0], xps4[1]);

                    XPoint[] xps2A = new XPoint[2] { new XPoint(20, 425), new XPoint(590, 425) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2A[0], xps2A[1]);

                    XPoint[] xps2B = new XPoint[2] { new XPoint(20, 435), new XPoint(590, 435) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps2B[0], xps2B[1]);

                    XRect xr6 = new XRect(new XPoint(21, 415), new XPoint(170, 425));
                    gfx.DrawRectangle(XBrushes.Transparent, xr6);
                    xtfcenter.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr6, XStringFormats.TopLeft);

                    XRect xr7 = new XRect(new XPoint(171, 415), new XPoint(420, 425));
                    gfx.DrawRectangle(XBrushes.Transparent, xr7);
                    xtfcenter.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr7, XStringFormats.TopLeft);

                    XRect xr8 = new XRect(new XPoint(421, 415), new XPoint(590, 425));
                    gfx.DrawRectangle(XBrushes.Transparent, xr8);
                    xtfcenter.DrawString("Period - Dalam Waktu", fontParagraph, XBrushes.Black, xr8, XStringFormats.TopLeft);

                    XPoint[] xps5 = new XPoint[2] { new XPoint(20, 415), new XPoint(590, 415) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps5[0], xps5[1]);

                    XFont fontParagraphsmall = new XFont("Consolas", 6.5, XFontStyle.Regular);

                    XRect xr9 = new XRect(new XPoint(21, 426), new XPoint(170, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr9);
                    xtfcenter.DrawString("< 1 Month - 1 Bulan", fontParagraphsmall, XBrushes.Black, xr9, XStringFormats.TopLeft);

                    XRect xr10 = new XRect(new XPoint(171, 426), new XPoint(254, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr10);
                    xtfcenter.DrawString("< 1 Year - 1 Tahun", fontParagraphsmall, XBrushes.Black, xr10, XStringFormats.TopLeft);

                    XRect xr11 = new XRect(new XPoint(255, 426), new XPoint(338, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr11);
                    xtfcenter.DrawString("< 2 Years - 2 Tahun", fontParagraphsmall, XBrushes.Black, xr11, XStringFormats.TopLeft);

                    XRect xr12 = new XRect(new XPoint(339, 426), new XPoint(420, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr12);
                    xtfcenter.DrawString("> 2 Years - 2 Tahun", fontParagraphsmall, XBrushes.Black, xr12, XStringFormats.TopLeft);

                    XRect xr13 = new XRect(new XPoint(421, 426), new XPoint(506, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr13);
                    xtfcenter.DrawString("< 1 Year - 1 Tahun", fontParagraphsmall, XBrushes.Black, xr13, XStringFormats.TopLeft);

                    XRect xr14 = new XRect(new XPoint(507, 426), new XPoint(590, 435));
                    gfx.DrawRectangle(XBrushes.Transparent, xr14);
                    xtfcenter.DrawString("> 1 Year - 1 Tahun", fontParagraphsmall, XBrushes.Black, xr14, XStringFormats.TopLeft);

                    XRect xr15 = new XRect(new XPoint(21, 435), new XPoint(170, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr15);
                    xtfcenter.DrawString(asp[2] + "%" + Environment.NewLine + "Original sales amount" + Environment.NewLine + "(Nilai Penjualan Asal)", fontParagraphsmall, XBrushes.Black, xr15, XStringFormats.TopLeft);

                    XRect xr16 = new XRect(new XPoint(171, 435), new XPoint(254, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr16);
                    xtfcenter.DrawString(asp[5] + "%" + Environment.NewLine + "Original sales amount" + Environment.NewLine + "(Nilai Penjualan Asal)", fontParagraphsmall, XBrushes.Black, xr16, XStringFormats.TopLeft);

                    XRect xr17 = new XRect(new XPoint(255, 435), new XPoint(338, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr17);
                    xtfcenter.DrawString(asp[8] + "%" + Environment.NewLine + "Original sales amount" + Environment.NewLine + "(Nilai Penjualan Asal)", fontParagraphsmall, XBrushes.Black, xr17, XStringFormats.TopLeft);

                    XRect xr18 = new XRect(new XPoint(339, 435), new XPoint(420, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr18);
                    xtfcenter.DrawString(Environment.NewLine + brand + " buying price" + Environment.NewLine + "(Harga beli " + brand + ")", fontParagraphsmall, XBrushes.Black, xr18, XStringFormats.TopLeft);

                    XRect xr19 = new XRect(new XPoint(421, 435), new XPoint(506, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr19);
                    //xtfcenter.DrawString(ASP.ASPLD.R1_Rumus.ToString("N2") + "%" + Environment.NewLine + "Original sales amount" + Environment.NewLine + "(Nilai Penjualan Asal)", fontParagraphsmall, XBrushes.Black, xr19, XStringFormats.TopLeft);
                    xtfcenter.DrawString(Environment.NewLine + brand + " buying price" + Environment.NewLine + "Harga beli " + brand + ")", fontParagraphsmall, XBrushes.Black, xr19, XStringFormats.TopLeft);

                    XRect xr20 = new XRect(new XPoint(507, 435), new XPoint(590, 465));
                    gfx.DrawRectangle(XBrushes.Transparent, xr20);
                    xtfcenter.DrawString(Environment.NewLine + brand + " buying price" + Environment.NewLine + "Harga beli " + brand + ")", fontParagraphsmall, XBrushes.Black, xr20, XStringFormats.TopLeft);

                    XPoint[] xps6 = new XPoint[2] { new XPoint(170, 426), new XPoint(170, 465) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps6[0], xps6[1]);

                    XPoint[] xps7 = new XPoint[2] { new XPoint(254, 426), new XPoint(254, 465) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps7[0], xps7[1]);

                    XPoint[] xps8 = new XPoint[2] { new XPoint(338, 426), new XPoint(338, 465) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps8[0], xps8[1]);

                    XPoint[] xps9 = new XPoint[2] { new XPoint(420, 426), new XPoint(420, 465) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps9[0], xps9[1]);

                    XPoint[] xps10 = new XPoint[2] { new XPoint(506, 426), new XPoint(506, 465) };
                    gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps10[0], xps10[1]);
                }
                    }
}

    }

    public string writeSO(int idsalesorder, string iduser = "ADMIN", string url = null)
    {
        using (Map_DB db = new Map_DB())
        {
            App app = new App();
            var so = db.SalesOrder.SingleOrDefault(p => p.ID == idsalesorder);
            string LocAddr = "";
            if (so.TipeLokasi == 1)
            {
                LocAddr = Db.SingleString("SELECT TOP 1 AddrAlamat FROM LocOutlet WHERE ID = " + so.IDLokasi, Db.Cnn_DB);
            }
            else if (so.TipeLokasi == 2)
            {
                LocAddr = Db.SingleString("SELECT TOP 1 AddrAlamat FROM LocExhibition WHERE ID = 14" + so.IDLokasi, Db.Cnn_DB);
            }

            PdfDocument doc = new PdfDocument();
            doc.Info.Title = "Invoice CMK";

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
            xtfleft.DrawString(LocAddr, fontParagraph3S, XBrushes.Black, xr2, XStringFormats.TopLeft);

            gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.8), new XPoint(20, 70), new XPoint(580, 70));

            XRect xr3 = new XRect(new XPoint(20, 75), new XPoint(250, 85));
            gfx.DrawRectangle(XBrushes.Transparent, xr3);
            xtfleft.DrawString("No.                     : " + so.Nomor, fontParagraph3, XBrushes.Black, xr3, XStringFormats.TopLeft);

            XRect xr4 = new XRect(new XPoint(20, 85), new XPoint(250, 95));
            gfx.DrawRectangle(XBrushes.Transparent, xr4);
            xtfleft.DrawString("Date / Tanggal Faktur   : " + Convert.ToDateTime(so.Tgl).ToString("dd-MM-yyyy"), fontParagraph3, XBrushes.Black, xr4, XStringFormats.TopLeft);

            XRect xr5 = new XRect(new XPoint(310, 75), new XPoint(580, 85));
            gfx.DrawRectangle(XBrushes.Transparent, xr5);
            xtfleft.DrawString("Customer Code / Kode Pelanggan  : " + so.DataCustomer.NoCustomer, fontParagraph3, XBrushes.Black, xr5, XStringFormats.TopLeft);

            XRect xr6 = new XRect(new XPoint(310, 85), new XPoint(580, 95));
            gfx.DrawRectangle(XBrushes.Transparent, xr6);
            xtfleft.DrawString("To / Kepada                     : " + so.DataCustomer.Nama, fontParagraph3, XBrushes.Black, xr6, XStringFormats.TopLeft);

            XRect xr7 = new XRect(new XPoint(310, 95), new XPoint(580, 105));
            gfx.DrawRectangle(XBrushes.Transparent, xr7);
            xtfleft.DrawString("Given To / Diberikan Kepada     : " + (!string.IsNullOrEmpty(so.CustomerReferenceNama) ? so.CustomerReferenceNama : so.CustomerNama), fontParagraph3, XBrushes.Black, xr7, XStringFormats.TopLeft);

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
            var pi = (from p in db.SalesOrderDJ
                      where p.IDForm == idsalesorder
                      select new
                      {
                          Nomor = p.StockProductDJ.Nomor,
                          Item = p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama,
                          Qty = 1,
                          TotalBayar = p.TotalBayar ?? 0m
                      })
                     .Union(from p in db.SalesOrderPG
                            where p.IDForm == idsalesorder
                            select new
                            {
                                Nomor = p.StockProductPG.Nomor,
                                Item = p.StockProductPG.CharProductItem.Nama,
                                Qty = 1,
                                TotalBayar = p.TotalBayar ?? 0m
                            })
                      .Union(from p in db.SalesOrderGJ
                             where p.IDForm == idsalesorder
                             select new
                             {
                                 Nomor = p.StockProductGJ.Nomor,
                                 Item = p.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama,
                                 Qty = 1,
                                 TotalBayar = p.TotalBayar ?? 0m
                             })
                       .Union(from p in db.SalesOrderLD
                              where p.IDForm == idsalesorder
                              select new
                              {
                                  Nomor = p.StockProductLD_Stone1B.Nomor,
                                  Item = p.StockProductLD_Stone1B.CharProductItem.Nama,
                                  Qty = 1,
                                  TotalBayar = p.TotalBayar ?? 0m
                              }).ToList();

            foreach (var i in pi)
            {
                //Item Desc
                XRect xr12 = new XRect(new XPoint(20, 120 + enter1), new XPoint(100, 130 + enter1));
                gfx.DrawRectangle(XBrushes.Transparent, xr12);
                xtfleft.DrawString(i.Nomor, fontParagraph3, XBrushes.Black, xr12, XStringFormats.TopLeft);

                XRect xr13 = new XRect(new XPoint(105, 120 + enter1), new XPoint(402, 130 + enter1));
                gfx.DrawRectangle(XBrushes.Transparent, xr13);
                xtfleft.DrawString(i.Item, fontParagraph3, XBrushes.Black, xr13, XStringFormats.TopLeft);

                XRect xr14 = new XRect(new XPoint(403, 120 + enter1), new XPoint(450, 130 + enter1));
                gfx.DrawRectangle(XBrushes.Transparent, xr14);
                xtfcenter.DrawString(i.Qty.ToString(), fontParagraph3, XBrushes.Black, xr14, XStringFormats.TopLeft);

                XRect xr15 = new XRect(new XPoint(455, 120 + enter1), new XPoint(580, 130 + enter1));
                gfx.DrawRectangle(XBrushes.Transparent, xr15);
                xtfright.DrawString(i.TotalBayar.ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr15, XStringFormats.TopLeft);

                enter1 += 10;
            }
            enter = enter1 - 10;
            gfx.DrawLine(new XPen(XColor.FromKnownColor(XKnownColor.Black), 1), new XPoint(20, 130 + enter), new XPoint(590, 130 + enter));

            var pd = db.SalesReceiptDetail.Where(p => p.SalesReceipt.IDSalesOrder == idsalesorder);

            decimal TotalBayar = 0m;
            foreach (var i in pd)
            {
                TotalBayar += Convert.ToDecimal(i.Nominal);
            }
            decimal TotalBayarExcVAT = Math.Ceiling(TotalBayar / 1.02m);
            decimal PPN = TotalBayar - TotalBayarExcVAT;

            XRect xr16 = new XRect(new XPoint(20, 132 + enter), new XPoint(250, 172 + enter));
            gfx.DrawRectangle(XBrushes.Transparent, xr16);
            xtfleft.DrawString("Say / Terbilang : " + Str.Terbilang(Convert.ToDecimal(TotalBayar.ToString("N2"))).TrimEnd().TrimStart(), fontParagraph3, XBrushes.Black, xr16, XStringFormats.TopLeft);

            XRect xr17 = new XRect(new XPoint(20, 182 + enter), new XPoint(250, 192 + enter));
            gfx.DrawRectangle(XBrushes.Transparent, xr17);
            xtfleft.DrawString("Detail of payment", fontParagraph4, XBrushes.Black, xr17, XStringFormats.TopLeft);

            int enter2 = 0;
            foreach (var i in pd)
            {
                XRect xr17A = new XRect(new XPoint(20, 202 + enter + enter2), new XPoint(250, 212 + enter + enter2));
                gfx.DrawRectangle(XBrushes.Transparent, xr17A);
                xtfleft.DrawString(i.PaymentType.Nama + " : " + Convert.ToDecimal(i.Nominal).ToString("N0").Replace(",", "."), fontParagraph3, XBrushes.Black, xr17A, XStringFormats.TopLeft);
                enter2 += 10;
            }

            /*System.Net.WebClient wc = new System.Net.WebClient();
            byte[] bytes = wc.DownloadData("https://dev1.cmk.co.id/ImagePLU.aspx?plu=B19014863");
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                XImage img = XImage.FromStream(ms); //XImage.FromStream(ms);        
                XRect xr18 = new XRect(new XPoint(300, 132 + enter), new XSize(50, 50));
                gfx.DrawImage(img, xr18);
                ms.Close();
            }*/

            XPoint[] xps = new XPoint[5] { new XPoint(400, 132 + enter), new XPoint(580, 132 + enter), new XPoint(580, 270 + enter), new XPoint(400, 270 + enter), new XPoint(400, 132 + enter) };

            gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);

            XRect xr19 = new XRect(new XPoint(403, 135 + enter), new XPoint(500, 165 + enter));
            gfx.DrawRectangle(XBrushes.Transparent, xr19);
            xtfleft.DrawString("Price Exclude VAT / Harga Jual Tidak Termasuk PPN", fontParagraph3, XBrushes.Black, xr19, XStringFormats.TopLeft);

            XRect xr20 = new XRect(new XPoint(403, 175 + enter), new XPoint(500, 205 + enter));
            gfx.DrawRectangle(XBrushes.Transparent, xr20);
            xtfleft.DrawString("VAT / PPN (10% x 20% x Harga Jual Tidak Termasuk PPN )", fontParagraph3, XBrushes.Black, xr20, XStringFormats.TopLeft);

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
            xtfcenter.DrawString(so.CustomerNama, fontParagraph4, XBrushes.Black, xr29, XStringFormats.TopLeft);

            XRect xr30 = new XRect(new XPoint(475, 350 + enter), new XPoint(577, 360 + enter));
            gfx.DrawRectangle(XBrushes.Transparent, xr30);
            xtfcenter.DrawString(so.SalesNama, fontParagraph4, XBrushes.Black, xr30, XStringFormats.TopLeft);

            XRect xr31 = new XRect(new XPoint(373, 360 + enter), new XPoint(470, 370 + enter));
            gfx.DrawRectangle(XBrushes.Transparent, xr31);
            xtfcenter.DrawString("(Customer / Pelanggan)", fontParagraph3, XBrushes.Black, xr31, XStringFormats.TopLeft);

            XRect xr32 = new XRect(new XPoint(475, 360 + enter), new XPoint(577, 370 + enter));
            gfx.DrawRectangle(XBrushes.Transparent, xr32);
            xtfcenter.DrawString("(JR / Staff Penjualan)", fontParagraph3, XBrushes.Black, xr32, XStringFormats.TopLeft);

            //writeASP(gfx, fontParagraph3, fontParagraph5, xtfleft, xtfcenter, enter);

            // Save the document...190116
            string filename = so.Nomor.Replace("/", "_") + ".pdf";
            string foldername = url + "/SO/" + iduser;
            if (!Directory.Exists(foldername))
            {
                Directory.CreateDirectory(foldername);
            }

            string SaveLocation = foldername + "\\" + filename;
            if (File.Exists(SaveLocation))
            {
                File.Delete(SaveLocation);
            }
            doc.Save(SaveLocation);
            // ...and start a viewer.
            //System.Diagnostics.Process.Start(SaveLocation);

            return SaveLocation;
            }
}


    private void writeDeclaration(XGraphics gfx, XFont fontParagraph, XTextFormatter xtf, int enter)
    {
        XRect xr32 = new XRect(new XPoint(20, 385 + enter), new XPoint(150, 395 + enter));
        gfx.DrawRectangle(XBrushes.Transparent, xr32);
        xtf.DrawString("After Sales Policy / Kebijakan Penjualan", fontParagraph, XBrushes.Black, xr32, XStringFormats.TopLeft);

        XPoint[] xps = new XPoint[5] { new XPoint(20, 395), new XPoint(590, 395), new XPoint(590, 445), new XPoint(20, 445), new XPoint(20, 395) };
        gfx.DrawLines(new XPen(XColor.FromKnownColor(XKnownColor.Black), 0.5), xps);


    }

    public class Invoice
    {
        public int ID { get; set; }
        public int IDSalesOrder { get; set; }
        public string NomorSO { get; set; }
        public string TanggalSO { get; set; }
        public string NomorInvoice { get; set; }
        public int Tipe { get; set; }
        public string PLU { get; set; }
        public string Item { get; set; }
        public int Qty { get; set; }
        public decimal TotalBayar { get; set; }
        public string Image { get; set; }
    }
    #endregion
}