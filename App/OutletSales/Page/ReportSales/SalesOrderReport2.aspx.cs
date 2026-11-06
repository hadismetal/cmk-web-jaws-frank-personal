using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System.Drawing;

public partial class SalesOrderReport2 : System.Web.UI.Page
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
    string Module = "pos"; // pos, acc

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            status.SelectedValue = "2";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        string kw_text = kw.Text;
        DateTime start_text = Convert.ToDateTime(dari.Text);
        DateTime end_text = Convert.ToDateTime(sampai.Text);
        string status_text = status.SelectedValue;

        TimeSpan ts = end_text.Date - start_text.Date;
        if (ts.TotalDays < 31)
        {
            lit_error.Text = string.Empty;
            string[] start_split = dari.Text.Split('-');
            string[] end_split = sampai.Text.Split('-');
            string start_new = start_split[2] + "-" + start_split[1] + "-" + start_split[0];
            string end_new = end_split[2] + "-" + end_split[1] + "-" + end_split[0];

            lit_error.Text = string.Empty;
            string location = getLocationUser();
            string query = "sp_report_sales_order_new";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@KEYWORD", SqlDbType.VarChar) { Value = kw_text },
                new SqlParameter("@TANGGALSTART", SqlDbType.DateTime) { Value = start_text },
                new SqlParameter("@TANGGALEND", SqlDbType.DateTime) { Value = end_new },
                new SqlParameter("@LOCCODE", SqlDbType.VarChar) { Value = location },
                new SqlParameter("@STATUS", SqlDbType.Int) { Value = status_text },
                new SqlParameter("@PAGE", SqlDbType.Int) { Value = 1 },
                new SqlParameter("@ROWS", SqlDbType.Int) { Value = 20 },
                new SqlParameter("@EXCEL", SqlDbType.Bit) { Value = 1 }
            };

            DataSet ds = DbAccess.Ds(query, Db.Cnn_DB, parameters);
            DataTable data = ds.Tables[0];
            int datacount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);

            decimal TotalBayar = 0m;
            decimal TotalPaid = 0m;
            decimal TotalUnpaid = 0m;
            decimal TotalQty = 0m;

            if (datacount > 0)
            {
                string ti_nomor = "";
                string ti_sisa_resell = "";

                int prev = 0;
                int index = 1;
                foreach (DataRow r in data.Rows)
                {
                    ti_nomor = "";
                    ti_sisa_resell = "";

                    decimal nilaiResell = 0m;
                    decimal nilaiHarusDibayar = 0m;

                    string rTradeIn = r["NOMORTI"].ToString();
                    decimal rTotalBayar = Convert.ToDecimal(r["TOTALBAYAR"]);
                    decimal rTotalResell = Convert.ToDecimal(r["TOTALRESELL"]);
                    string rNomor = r["NOMORSO"].ToString();
                    string rPLU = r["PLU"].ToString();
                    bool rStatusVoid = Convert.ToBoolean(r["STATUSVOID"]);
                    bool rStatusPembayaran = Convert.ToBoolean(r["STATUSPEMBAYARAN"]);
                    bool rDraft = Convert.ToBoolean(r["DRAFT"]);
                    DateTime rTgl = Convert.ToDateTime(r["TGLSO"]);
                    string rCustomerNama = r["CUSTNAMA"].ToString();
                    string rSalesNama = r["SALESNAMA"].ToString();
                    string rKeterangan = r["KETERANGAN"].ToString();
                    string rKeteranganVoid = r["KETERANGANVOID"].ToString();

                    int ID = Convert.ToInt32(r["ID"]);
                    decimal Harga = Convert.ToDecimal(r["TOTALBAYAR"]);
                    decimal Paid = Convert.ToDecimal(r["TERBAYAR"]);
                    decimal Unpaid = Convert.ToDecimal(r["SISABAYAR"]);
                    int Qty = Convert.ToInt32(r["QTY"]);
                    string rStatus = r["STATUS"].ToString();

                    if (!string.IsNullOrEmpty(rTradeIn))
                    {
                        ti_nomor = "<br /><span style='color:blue;'>" + rTradeIn + "</span>";
                        nilaiResell = rTotalResell;
                        nilaiHarusDibayar = rTotalBayar;
                        ti_sisa_resell = "<br /><span style='color:blue;'>" + (nilaiHarusDibayar - nilaiResell).ToString("N0") + "</span>";
                    }

                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = TD_HyperLinkNew(ID, rStatusVoid, rStatusPembayaran, rDraft, rNomor, rTgl, Unpaid);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    string Status = string.Empty;
                    switch (rStatus.ToLower())
                    {
                        case "void":
                            Status = "<label class='status-label status-void'>" + rStatus + "<label>";
                            break;
                        case "draft":
                            Status = "<label class='status-label status-draft'>" + rStatus + "<label>";
                            break;
                        case "waiting for submit":
                            Status = "<label class='status-label status-waitforsubmit'>" + rStatus + "<label>";
                            break;
                        case "lunas":
                            Status = "<label class='status-label status-lunas'>" + rStatus + "<label>";
                            break;
                        case "belum lunas":
                            Status = "<label class='status-label status-belumlunas'>" + rStatus + "<label>";
                            break;
                        default:
                            Status = "<label>" + rStatus + "<label>";
                            break;
                    }

                    td = new TableCell();
                    td.Text = Str.HtmlBreak(Status);
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.FullDate(rTgl);
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rNomor + ti_nomor;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rPLU;
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rCustomerNama;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rSalesNama;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Qty.ToString();
                    td.Style.Add("text-align", "center");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Harga.ToString("N0") + ti_sisa_resell;
                    td.Style.Add("text-align", "right");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Paid.ToString("N0");
                    td.Style.Add("text-align", "right");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Unpaid.ToString("N0");
                    td.Style.Add("text-align", "right");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Str.HtmlBreak(rKeterangan);
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = rKeteranganVoid;
                    td.Style.Add("text-align", "left");
                    tr.Cells.Add(td);

                    tb.Rows.Add(tr);

                    TotalBayar += Harga;
                    TotalUnpaid += Unpaid;
                    TotalPaid += Paid;
                    TotalQty += Qty;
                }
            }
            else
            {
                App.TableNoResult(tb);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 8;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalQty).ToString("N0") + "</b>";
            td2.Style.Add("text-align", "center");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalPaid).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalUnpaid).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            td2.ColumnSpan = 2;
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
        else
        {
            lit_error.Text = "<label style='font-size:larger;font-family:Lucida Console;color:red'>Jika periode lebih dari 31 hari, klik tombol Export</label>";
        }
    }

    //Display Table
    protected void exportTableNew(object sender, EventArgs e)
    {
        string kw_text = kw.Text;
        DateTime start_text = Convert.ToDateTime(dari.Text);
        DateTime end_text = Convert.ToDateTime(sampai.Text);
        string status_text = status.SelectedValue;

        string location = getLocationUser();

        string query = "sp_report_sales_order_new";

        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@KEYWORD", SqlDbType.VarChar) { Value = kw_text },
            new SqlParameter("@TANGGALSTART", SqlDbType.DateTime) { Value = start_text },
            new SqlParameter("@TANGGALEND", SqlDbType.DateTime) { Value = end_text },
            new SqlParameter("@LOCCODE", SqlDbType.VarChar) { Value = location },
            new SqlParameter("@STATUS", SqlDbType.Int) { Value = status_text },
            new SqlParameter("@PAGE", SqlDbType.Int) { Value = 1 },
            new SqlParameter("@ROWS", SqlDbType.Int) { Value = 20 },
            new SqlParameter("@EXCEL", SqlDbType.Bit) { Value = 1 }
        };

        DataSet ds = DbAccess.Ds(query, Db.Cnn_DB, parameters);
        DataTable data = ds.Tables[0];

        CreateExcel(data);
    }

    protected MemoryStream GetStream(ExcelPackage excelPackage)
    {
        MemoryStream fs = new MemoryStream();
        excelPackage.SaveAs(fs);
        fs.Position = 0;
        return fs;
    }

    protected void CreateExcel(DataTable dt)
    {
        using (var package = new ExcelPackage())
        {
            ExcelWorkbook wb = package.Workbook;
            ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Report Sales Order");

            sheet.Cells[1, 1].Value = "No";
            sheet.Cells[1, 2].Value = "Status";
            sheet.Cells[1, 3].Value = "Tanggal";
            sheet.Cells[1, 4].Value = "No. Referensi";
            sheet.Cells[1, 5].Value = "No. Trade In";
            sheet.Cells[1, 6].Value = "PLU";
            sheet.Cells[1, 7].Value = "Nama Customer";
            sheet.Cells[1, 8].Value = "Sales";
            sheet.Cells[1, 9].Value = "Qty";
            sheet.Cells[1, 10].Value = "Total";
            sheet.Cells[1, 11].Value = "Total Terbayar";
            sheet.Cells[1, 12].Value = "Total Belum Dibayar";
            sheet.Cells[1, 13].Value = "Keterangan";
            sheet.Cells[1, 14].Value = "Void Keterangan";

            decimal TotalBayar = 0m;
            decimal TotalPaid = 0m;
            decimal TotalUnpaid = 0m;
            decimal TotalQty = 0m;

            string ti_nomor = "";
            string ti_sisa_resell = "";

            foreach (DataRow r in dt.Rows)
            {
                ti_nomor = "";
                ti_sisa_resell = "";

                decimal nilaiResell = 0m;
                decimal nilaiHarusDibayar = 0m;

                string rTradeIn = r["NOMORTI"].ToString();
                decimal rTotalBayar = Convert.ToDecimal(r["TOTALBAYAR"]);
                decimal rTotalResell = Convert.ToDecimal(r["TOTALRESELL"]);
                string rNomor = r["NOMORSO"].ToString();
                string rPLU = r["PLU"].ToString();
                bool rStatusVoid = Convert.ToBoolean(r["STATUSVOID"]);
                bool rStatusPembayaran = Convert.ToBoolean(r["STATUSPEMBAYARAN"]);
                bool rDraft = Convert.ToBoolean(r["DRAFT"]);
                DateTime rTgl = Convert.ToDateTime(r["TGLSO"]);
                string rCustomerNama = r["CUSTNAMA"].ToString();
                string rSalesNama = r["SALESNAMA"].ToString();
                string rKeterangan = r["KETERANGAN"].ToString();
                string rKeteranganVoid = r["KETERANGANVOID"].ToString();

                int ID = Convert.ToInt32(r["ID"]);
                decimal Harga = Convert.ToDecimal(r["TOTALBAYAR"]);
                decimal Paid = Convert.ToDecimal(r["TERBAYAR"]);
                decimal Unpaid = Convert.ToDecimal(r["SISABAYAR"]);
                int Qty = Convert.ToInt32(r["QTY"]);
                string rStatus = r["STATUS"].ToString();

                if (!string.IsNullOrEmpty(rTradeIn))
                {
                    ti_nomor = "<br /><span style='color:blue;'>" + rTradeIn + "</span>";
                    nilaiResell = rTotalResell;
                    nilaiHarusDibayar = rTotalBayar;
                    ti_sisa_resell = "<br /><span style='color:blue;'>" + (nilaiHarusDibayar - nilaiResell).ToString("N0") + "</span>";
                }

                sheet.Cells[dt.Rows.IndexOf(r) + 2, 1].Value = (dt.Rows.IndexOf(r) + 1).ToString();
                sheet.Cells[dt.Rows.IndexOf(r) + 2, 2].Value = rStatus;
                sheet.Cells[dt.Rows.IndexOf(r) + 2, 3].Value = rTgl.ToString("dd-MM-yyyy");
                sheet.Cells[dt.Rows.IndexOf(r) + 2, 4].Value = rNomor;
                sheet.Cells[dt.Rows.IndexOf(r) + 2, 5].Value = rTradeIn;
                sheet.Cells[dt.Rows.IndexOf(r) + 2, 6].Value = rPLU;
                sheet.Cells[dt.Rows.IndexOf(r) + 2, 7].Value = rCustomerNama;
                sheet.Cells[dt.Rows.IndexOf(r) + 2, 8].Value = rSalesNama;
                sheet.Cells[dt.Rows.IndexOf(r) + 2, 9].Value = Qty;
                sheet.Cells[dt.Rows.IndexOf(r) + 2, 10].Value = Harga;
                sheet.Cells[dt.Rows.IndexOf(r) + 2, 11].Value = Paid;
                sheet.Cells[dt.Rows.IndexOf(r) + 2, 12].Value = Unpaid;
                sheet.Cells[dt.Rows.IndexOf(r) + 2, 13].Value = rKeterangan;
                sheet.Cells[dt.Rows.IndexOf(r) + 2, 14].Value = rKeteranganVoid;

                TotalBayar += Harga;
                TotalUnpaid += Unpaid;
                TotalPaid += Paid;
                TotalQty += Qty;
            }

            //create a range for the table
            ExcelRange range = sheet.Cells[1, 1, dt.Rows.Count + 1, 14];

            sheet.Cells[dt.Rows.Count + 2, 1, dt.Rows.Count + 2, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[dt.Rows.Count + 2, 1, dt.Rows.Count + 2, 14].Style.Fill.BackgroundColor.SetColor(Color.AliceBlue);
            sheet.Cells[dt.Rows.Count + 2, 1, dt.Rows.Count + 2, 14].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            sheet.Cells[dt.Rows.Count + 2, 1, dt.Rows.Count + 2, 8].Merge = true;
            sheet.Cells[dt.Rows.Count + 2, 1, dt.Rows.Count + 2, 8].Value = "Total";
            sheet.Cells[dt.Rows.Count + 2, 9].Value = TotalQty;
            sheet.Cells[dt.Rows.Count + 2, 10].Value = TotalBayar;
            sheet.Cells[dt.Rows.Count + 2, 11].Value = TotalPaid;
            sheet.Cells[dt.Rows.Count + 2, 12].Value = TotalUnpaid;
            sheet.Cells[dt.Rows.Count + 2, 13, dt.Rows.Count + 2, 14].Merge = true;

            //add a table to the range
            ExcelTable tab = sheet.Tables.Add(range, "DataSales");

            //format the table
            tab.TableStyle = TableStyles.None;

            sheet.Cells.AutoFitColumns();

            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    DateTime now = DateTime.Now;
                    string tgl = now.Day.ToString().PadLeft(2, '0');
                    string bulan = now.Month.ToString().PadLeft(2, '0');
                    string tahun = now.Year.ToString().PadLeft(4, '0');
                    string jam = now.Hour.ToString().PadLeft(2, '0');
                    string menit = now.Minute.ToString().PadLeft(2, '0');
                    string detik = now.Second.ToString().PadLeft(2, '0');

                    package.SaveAs(stream);

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition",
                    "attachment; filename=Sales_Order_Report_" + tgl + "_" + bulan + "_" + tahun + "_" + jam + "_" + menit + "_" + detik + ".xls");
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.BinaryWrite(stream.ToArray());
                    stream.SetLength(0);
                    stream.Dispose();
                    Response.End();
                }
                catch
                {
                    stream.SetLength(0);
                    stream.Dispose();
                }
            }
        }
    }

    protected string getLocationUser()
    {
        string result = "";
        int xUserID = App.UserID;
        int xisAM = 0;
        string listStore = "";
        //check area manager
        var am = from p in db.DataAreaManager where p.IDUser == App.UserID select p;
        int amcount = am.Count();
        if (amcount > 0)
        {
            int i = 0;
            foreach (var x in am)
            {
                int j = 0;
                var lokasi = x.DataAreaManager_Lokasi
                    .GroupBy(p => new { p.IDForm, p.TipeLokasi, p.IDLokasi })
                    .Select(c => new { c.Key.IDForm, c.Key.TipeLokasi, c.Key.IDLokasi });
                foreach (var y in lokasi)
                {
                    //listStore = "";
                    if (y.TipeLokasi == 1)
                        listStore += db.LocOutlet.Single(p => p.ID == y.IDLokasi).Kode;
                    else
                        listStore += db.LocExhibition.Single(p => p.ID == y.IDLokasi).Kode;
                    j++;
                    listStore += ",";
                }
                i++;
            }
            xisAM = 1;
        }

        if (xisAM == 0)
        {
            //check admin sales
            var ad = from p in db.DataAdminSales where p.IDUser == App.UserID select p;
            if (ad.Count() > 0)
            {
                int k = 0;
                foreach (var x in ad)
                {
                    listStore = "";
                    if (x.TipeLokasi == 1)
                        listStore = db.LocOutlet.Single(p => p.ID == x.IDLokasi).Kode;
                    else
                        listStore = db.LocExhibition.Single(p => p.ID == x.IDLokasi).Kode;
                    k++;
                    listStore += ",";
                }
            }
        }

        result = (listStore.Length > 0) ? listStore.Remove(listStore.Length - 1) : listStore;

        return result;
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];

            status.SelectedValue = Request.QueryString["status"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;


        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "SalesOrderReport_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;


        Response.Redirect(href);
    }
    protected string TD_HyperLinkNew(int ID, bool StatusVoid, bool StatusPembayaran, bool Draft, string Nomor, DateTime tglSO, decimal Unpaid)
    {
        string x = string.Empty;
        DateTime formattedDate = DateTime.Today;
        string Approver = "";
        string Rejected = "";

        string query = @"SELECT ISNULL(Approval,'jarpis')Approval,ISNULL(Rejected,'jarpis1') Rejected FROM ApprovalSOVoid WHERE IDSO=@IDSO";
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
        {
            using (SqlCommand command = new SqlCommand(query, conn))
            {
                command.CommandTimeout = 1;
                command.Parameters.Add(new SqlParameter("@IDSO", SqlDbType.Int) { Value = ID });

                conn.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Loop through the results
                    while (reader.Read())
                    {

                        Approver = Convert.ToString(reader["Approval"]);
                        Rejected = Convert.ToString(reader["Rejected"]);
                    }
                }
                conn.Close();
            }
        }
        if (!(StatusVoid == false && StatusPembayaran == false && Draft == true))
        {
            if (StatusPembayaran == false && StatusVoid == false && tglSO.Date == DateTime.Now.Date)
            {
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Create/Payment/Payment_ItemList.aspx?id=" + ID + "\">";
                x += "  <i class=\"icon-check\"></i> ";
                x += "  ";
                x += "</a>";
            }
            else if (StatusPembayaran == false && StatusVoid == false && tglSO.Date < DateTime.Now.Date && Unpaid == 0)
            {
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../Create/Payment/Payment_ItemList.aspx?id=" + ID + "\">";
                x += "  <i class=\"icon-check\"></i> ";
                x += "  ";
                x += "</a>";
            }
            else
            {
                x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"SalesOrderReport_Detail.aspx?id=" + ID + "\">";
                x += "  <i class=\"icon-search\"></i> ";
                x += "  ";
                x += "</a>";
            }
        }
        if (StatusVoid == false && Draft == false && Payment.showVoidPaymentStatusGiftCard(ID))
        {
            // Gift Card Payment Void Warning
            x += "&nbsp;<a class=\"btn btn-danger\" href=\"SalesOrderReport_FormVoid.aspx?id=" + ID + "\">";
            x += "  <i class=\"icon-warning-sign\"></i> ";
            x += "  ";
            x += "</a>";
        }
        //SAP Void Tidak ada lagi cancel Sales Order
        else if (StatusVoid == false && StatusPembayaran == false && Draft == true && (Nomor == null || Nomor == ""))
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-warning\" onClick=\"return confirm('Cancel Sales Order, Are you Sure?');\" href=\"SalesOrderReport_Delete.aspx?id=" + ID + "\">";
            x += "  <i class=\"icon-remove\"></i> ";
            x += "  ";
            x += "</a>";
        }
        else if (VoidValidation.validVoidSalesOrderRole(ID, Module) && ((StatusPembayaran == true || StatusPembayaran == false) && Draft == false))
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Void Sales Order, Are you Sure?');\" href=\"SalesOrderReport_FormVoid.aspx?id=" + ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  ";
            x += "</a>";
        }
        else if (StatusPembayaran == false && StatusVoid == false && Draft == false)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Void Sales Order, Are you Sure?');\" href=\"SalesOrderReport_FormVoid.aspx?id=" + ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  ";
            x += "</a>";
        }

        Response.Write("<script>console.log('" + Rejected + "');</script>");
        return x;
    }
}