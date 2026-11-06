using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_DPPO_SPK_Export : System.Web.UI.Page
{
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
    protected int myID { get { return Convert.ToInt32(Request.QueryString["id"]); } }
    protected string brand { get { return Request.QueryString["brand"]; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        EditSPKExcel();
    }
    protected void CreateExcel()
    {
        string excelpath = "";
        excelpath = Server.MapPath("~/App_Data/Data/test.xlsx");
        FileInfo finame = new FileInfo(excelpath);
        if (System.IO.File.Exists(excelpath))
        {
            System.IO.File.Delete(excelpath);
        }
        if (!System.IO.File.Exists(excelpath))
        {
            ExcelPackage excel = new ExcelPackage(finame);
            var sheetcreate = excel.Workbook.Worksheets.Add(DateTime.UtcNow.Date.ToString());
            DataTable Dtvalue = new DataTable();
            Dtvalue = Db.Rs("SELECT TOP 10 ID, Email, Nama FROM UserAccount", Db.Cnn_AC);
            if (Dtvalue.Rows.Count > 0)
            {
                for (int i = 0; i < Dtvalue.Rows.Count;)
                {
                    sheetcreate.Cells[i + 1, 1].Value = Dtvalue.Rows[i][0].ToString();
                    sheetcreate.Cells[i + 1, 2].Value = Dtvalue.Rows[i][1].ToString();
                    sheetcreate.Cells[i + 1, 3].Value = Dtvalue.Rows[i][2].ToString();
                    i++;
                }
            }
            sheetcreate.Cells[1, 1, 4, 32].Merge = true;
            sheetcreate.Cells[1, 1, 4, 32].Value = "PEMBAHARUAN FORMAT SPK PESANAN 2020";
            sheetcreate.Cells[1, 1, 4, 32].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.LightHorizontal;
            sheetcreate.Cells[1, 1, 4, 32].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue);
            excel.Save();

            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            //workSheet.Cells[1, 1].LoadFromCollection(data, true);  
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=User.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.Position = 0;
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
    protected void EditSPKExcel()
    {
        var dp = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
        if (dp != null && (dp.IDProductSample1 != null || dp.IDProduct != null))
        {
            var product = db.StockProductDJ.SingleOrDefault(p => p.ID == dp.IDProduct);
            var productsample = db.StockProductDJ.SingleOrDefault(p => p.ID == dp.IDProductSample1);
            var product1 = (product != null) ? product : productsample;

            string excelpath = Server.MapPath("~/Form/DataSPK/SPK-MJ.xlsx");

            using (var templateStream = new FileStream(excelpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (ExcelPackage excel = new ExcelPackage(templateStream))
            {
                //var sheetcreate = excel.Workbook.Worksheets.Add(DateTime.UtcNow.Date.ToString());
                var sheetcreate = excel.Workbook.Worksheets["SPK"];
                sheetcreate.Cells[14, 5, 14, 15].Value = dp.NomorDP;
                sheetcreate.Cells[12, 5, 12, 15].Value = dp.CustomerNama;
                sheetcreate.Cells[16, 5, 16, 14].Value = product1.StockProductDJ_CharProduct.CharProductCategory.Nama;
                sheetcreate.Cells[18, 5, 18, 6].Value = (dp.Tgl.HasValue) ? Convert.ToDateTime(dp.Tgl).Day.ToString() : "";
                sheetcreate.Cells[18, 7, 18, 9].Value = (dp.Tgl.HasValue) ? Convert.ToDateTime(dp.Tgl).Month.ToString() : "";
                sheetcreate.Cells[18, 10, 18, 11].Value = (dp.Tgl.HasValue) ? Convert.ToDateTime(dp.Tgl).Year.ToString() : "";

                if (dp.DeliveryDate != null)
                {
                    sheetcreate.Cells[20, 5, 20, 6].Value = (dp.DeliveryDate.HasValue) ? Convert.ToDateTime(dp.DeliveryDate).Day.ToString() : "";
                    sheetcreate.Cells[20, 7, 20, 9].Value = (dp.DeliveryDate.HasValue) ? Convert.ToDateTime(dp.DeliveryDate).Month.ToString() : "";
                    sheetcreate.Cells[20, 10, 20, 11].Value = (dp.DeliveryDate.HasValue) ? Convert.ToDateTime(dp.DeliveryDate).Year.ToString() : "";
                }
                string lokasi = "";
                string kodelokasi = "";
                if (dp.TipeLokasi == 1)
                {
                    var lokasipo = db.LocOutlet.SingleOrDefault(p => p.ID == dp.IDLokasi);
                    if (lokasipo != null)
                    {
                        lokasi = lokasipo.Nama;
                        kodelokasi = lokasipo.Kode;
                    }
                }
                else if (dp.TipeLokasi == 2)
                {
                    var lokasipo = db.LocExhibition.SingleOrDefault(p => p.ID == dp.IDLokasi);
                    if (lokasipo != null)
                    {
                        lokasi = lokasipo.Nama;
                        kodelokasi = lokasipo.Kode;
                    }
                }
                sheetcreate.Cells[22, 5, 23, 7].Value = kodelokasi;
                sheetcreate.Cells[6, 22, 6, 31].Value = product1.ProtoNomor;
                sheetcreate.Cells[8, 22, 8, 31].Value = product1.Nomor;
                sheetcreate.Cells[11, 17, 12, 21].Value = product1.StockProductDJ_CharProduct.CharProductItem.Nama;
                sheetcreate.Cells[11, 22, 12, 23].Value = "1";
                sheetcreate.Cells[11, 24, 12, 26].Value = product1.StockProductDJ_CharProduct.DimensiP;
                sheetcreate.Cells[11, 27, 12, 28].Value = product1.StockProductDJ_CharProduct.DimensiL;
                sheetcreate.Cells[11, 29, 12, 31].Value = product1.StockProductDJ_CharProduct.DimensiD;

                if (product1.StockProductDJ_CharDesign.TargetGender != 2)
                {
                    sheetcreate.Cells[15, 19, 16, 20].Value = product1.StockProductDJ_CharProduct.DimensiR;
                    sheetcreate.Cells[15, 21, 16, 31].Value = product1.StockProductDJ_CharDesign.Grafir;
                }
                else
                {
                    sheetcreate.Cells[17, 19, 18, 20].Value = product1.StockProductDJ_CharProduct.DimensiR;
                    sheetcreate.Cells[17, 21, 18, 31].Value = product1.StockProductDJ_CharDesign.Grafir;
                }

                sheetcreate.Cells[20, 22, 20, 31].Value = (dp.IDFontType != null) ? db.FontType.SingleOrDefault(p => p.ID == dp.IDFontType).Nama : "";
                sheetcreate.Cells[23, 17, 23, 20].Value = product1.StockProductDJ_CharProduct.NettoWeight;
                if (dp.IDFrameColor == null)
                    sheetcreate.Cells[23, 21, 23, 26].Value = product1.StockProductDJ_CharProduct.CharFrameColor.Nama;
                else
                    sheetcreate.Cells[23, 21, 23, 26].Value = db.CharFrameColor.SingleOrDefault(p => p.ID == dp.IDFrameColor).Nama;

                string plunumber = product1.Nomor;
                string stonemodelist = "i"; // b:bandroll / c:certificate
                string plutype = "dj"; // dj or ld

                string spname = "sp_get_stone_information_for_bandroll_certificate";
                string query = " exec " + spname
                             + " '" + stonemodelist + "'"
                             + ",'" + plunumber + "'"
                             + ",'" + plutype + "'"
                             ;

                DataTable dt = new DataTable();
                dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

                string GIA = "";
                int TotalButir = 0;
                decimal TotalCarat = 0m;
                string BrandStone = "";

                System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
                nfi.NumberDecimalDigits = 2;

                if (dt.Rows.Count > 0)
                {
                    int startrow = 30;
                    foreach (DataRow dr in dt.Rows)
                    {
                        sheetcreate.Cells[startrow, 22, startrow, 23].Value = dr["ShapeName"].ToString().ToUpper();
                        sheetcreate.Cells[startrow, 24, startrow, 25].Value = Convert.ToInt32(dr["TotalButir"]).ToString();
                        sheetcreate.Cells[startrow, 26, startrow, 27].Value = Convert.ToDecimal(dr["TotalCarat"]).ToString("N3", nfi);
                        sheetcreate.Cells[startrow, 28, startrow, 29].Value = dr["Colour"].ToString();
                        sheetcreate.Cells[startrow, 30, startrow, 31].Value = dr["Clarity"].ToString();

                        if (!string.IsNullOrEmpty(dr["GIA"].ToString()))
                            GIA = GIA + dr["GIA"].ToString() + ",";

                        if (!string.IsNullOrEmpty(dr["BrandName"].ToString()))
                            BrandStone = BrandStone + dr["BrandName"].ToString() + ",";

                        TotalButir += Convert.ToInt32(dr["TotalButir"]);
                        TotalCarat += Convert.ToDecimal(dr["TotalCarat"]);
                        startrow += 1;
                    }
                }

                sheetcreate.Cells[27, 22, 28, 31].Value = (BrandStone != "") ? BrandStone.Remove(BrandStone.Length - 1) : "";
                sheetcreate.Cells[45, 22, 45, 31].Value = "";//(GIA != "" ) ? GIA.Remove(GIA.Length - 1) : "";
                sheetcreate.Cells[42, 24, 42, 25].Value = TotalButir;
                sheetcreate.Cells[42, 26, 42, 27].Value = TotalCarat;
                sheetcreate.Cells[62, 18, 62, 22].Value = dp.SalesNama + " / " + kodelokasi;

                if (brand == "FC")
                {
                    System.Drawing.Image logo = System.Drawing.Image.FromFile(@"D:\web\Images\spk\logo_spk_franknco.jpg");
                    var picture = sheetcreate.Drawings.AddPicture("", logo);
                    picture.From.Row = 18;
                    picture.From.Column = 13;
                    picture.SetSize(75, 75);
                    logo.Dispose();
                    picture.Dispose();
                }
                else if (brand == "MM")
                {
                    System.Drawing.Image logo = System.Drawing.Image.FromFile(@"D:\web\Images\spk\logo_spk_missmondial.jpg");
                    var picture = sheetcreate.Drawings.AddPicture("", logo);
                    picture.From.Row = 18;
                    picture.From.Column = 13;
                    picture.SetSize(75, 75);
                    logo.Dispose();
                    picture.Dispose();
                }
                else if (brand == "M")
                {
                    System.Drawing.Image logo = System.Drawing.Image.FromFile(@"D:\web\Images\spk\logo_spk_mondial.jpg");
                    var picture = sheetcreate.Drawings.AddPicture("", logo);
                    picture.From.Row = 18;
                    picture.From.Column = 13;
                    picture.SetSize(75, 75);
                    logo.Dispose();
                    picture.Dispose();
                }
                else if (brand == "TP")
                {
                    System.Drawing.Image logo = System.Drawing.Image.FromFile(@"D:\web\Images\spk\logo_spk_thepalace.jpg");
                    var picture = sheetcreate.Drawings.AddPicture("", logo);
                    picture.From.Row = 18;
                    picture.From.Column = 13;
                    picture.SetSize(75, 75);
                    logo.Dispose();
                    picture.Dispose();
                }

                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    string URL = ConfigurationManager.AppSettings["ImageAPI"] + product1.ImgPicture;
                    byte[] bytes = wc.DownloadData(URL);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms))
                    {
                        var picturePLU = sheetcreate.Drawings.AddPicture("PLU", img);
                        picturePLU.From.Row = 28;
                        picturePLU.From.Column = 3;
                        picturePLU.SetSize(300, 300);
                        picturePLU.SendToBack();
                        picturePLU.Dispose();
                    }
                }

                //var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
                //workSheet.Cells[1, 1].LoadFromCollection(data, true);  
                using (var memoryStream = new MemoryStream())
                {
                    string excelname = "SPK(" + dp.NomorDP + ")";
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + excelname + ".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
    }
}