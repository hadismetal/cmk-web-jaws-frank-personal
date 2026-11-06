using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class Page_DJ_Bulk : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            datepicker.Text = Str.Tgl(DateTime.Now.AddDays(1));
            datepicker2.Text = Str.Tgl(DateTime.Now.AddDays(1));

            tb.Visible = false;
            btn_click_modal_save.Visible = false;
        }
    }

    protected void PromoStatus(DropDownList ddl)
    {
        ddl.Items.Clear();
        ddl.Items.Add(new ListItem("Promo 1 Harga", "3"));
    }

    protected void DownloadTemplate(object sender, EventArgs e)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            using (ExcelPackage package = new ExcelPackage(ms))
            {
                // Add a new worksheet
                var worksheet = package.Workbook.Worksheets.Add("Upload");

                // Add some data to the worksheet
                worksheet.Cells[1, 1].Value = "Kode Karet";
                worksheet.Cells[1, 2].Value = "PLU";
                worksheet.Cells[1, 3].Value = "One Price";
                //worksheet.Cells[1, 4].Value = "Ketentuan Buyback";

                //worksheet.Cells[2, 9].Value = "Notes";
                //worksheet.Cells[3, 9].Value = "Jangan menggunakan file excel yang bukan dari download template. Harus menggunakan file excel ini untuk upload.";
                //worksheet.Cells[4, 9].Value = "Decimal menggunakan titik, misalnya 50000.00 (lima puluh ribu koma nol nol)";

                using (var headerRow = worksheet.Cells[1, 1, 1, 3])
                {
                    headerRow.Style.Font.Name = "Consolas";

                    headerRow.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerRow.Style.Fill.BackgroundColor.SetColor(Color.AliceBlue);
                }
                worksheet.Cells.AutoFitColumns();

                package.Save();
            }

            // Set the response to prompt the user to download the file
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=Template Upload Bulk One Price.xlsx");
            Response.BinaryWrite(ms.ToArray());
            Response.End();
        }
    }

    protected void Upload(object sender, EventArgs e)
    {
        string nama_promo = nama.Text;
        string start = datepicker.Text;
        string end = datepicker2.Text;

        chk_all.Checked = false;
        var file = fileupload.PostedFile;

        if (file.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        {
            using (var stream = file.InputStream)
            {
                using (var package = new ExcelPackage(stream))
                {
                    try
                    {
                        // Access the first worksheet in the Excel file
                        var worksheet = package.Workbook.Worksheets[0];

                        // Loop through the rows and columns of the worksheet
                        int rowCount = worksheet.Dimension.Rows;

                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("PLU", typeof(string));

                        DataTable dataTable3 = new DataTable();
                        dataTable3.Columns.Add("KodeKaret", typeof(string));
                        dataTable3.Columns.Add("PLU", typeof(string));
                        dataTable3.Columns.Add("OnePrice", typeof(int));

                        bool validasicuk = true;
                        bool validasi = false;
                        string errorvalidasi = "";

                        string kodekaret1 = "";
                        int oneprice1 = 0;

                        for (int row = 2; row <= rowCount; row++)
                        {
                            string kodekaret2 = worksheet.Cells[row, 1].Text;
                            string plu2 = worksheet.Cells[row, 2].Text;
                            int oneprice2 = Convert.ToInt32(worksheet.Cells[row, 3].Text);

                            DataRow dr = dataTable.NewRow();
                            dr["PLU"] = plu2;
                            dataTable.Rows.Add(dr);

                            DataRow dr1 = dataTable3.NewRow();
                            dr1["KodeKaret"] = kodekaret2;
                            dr1["PLU"] = plu2;
                            dr1["OnePrice"] = oneprice2;
                            dataTable3.Rows.Add(dr1);

                            if (row == 2)
                            {
                                kodekaret1 = kodekaret2;
                                oneprice1 = oneprice2;
                            }

                            if (kodekaret1 != kodekaret2)
                            {
                                validasicuk = false;
                                errorvalidasi = "Terdapat Kode Karet Yang Berbeda Pada Upload Excel.";
                                break;
                            }

                            if (oneprice1 != oneprice2)
                            {
                                validasicuk = false;
                                errorvalidasi = "Terdapat One Price Yang Berbeda Pada Upload Excel.";
                                break;
                            }
                        }

                        if (dataTable3.Rows.Count > 0)
                        {
                            if (validasicuk)
                            {
                                string query = "sp_validate_register_dj_one_price";
                                List<SqlParameter> parameters = new List<SqlParameter>()
                            {
                                new SqlParameter("@Data", SqlDbType.Structured){ Value = dataTable }
                            };

                                DataTable dt2 = Rs(query, Db.Cnn_DB, parameters);
                                validasi = Convert.ToBoolean(dt2.Rows[0]["STATUS"]);
                                errorvalidasi = dt2.Rows[0]["MESSAGE"].ToString();

                                if (validasi)
                                {
                                    using (Map_DB db = new Map_DB())
                                    {
                                        int row = 1;
                                        foreach (DataRow dr in dataTable3.Rows)
                                        {
                                            string kodekaret = dr["KodeKaret"].ToString();
                                            string plu = dr["PLU"].ToString();
                                            string oneprice = dr["OnePrice"].ToString();

                                            var r = db.StockProductDJ.FirstOrDefault(p => p.Nomor == plu);

                                            TableRow rowRow = new TableRow();

                                            TableCell cell = new TableCell();
                                            CheckBox chk = new CheckBox();
                                            chk.Attributes.Add("data-plu", plu);
                                            chk.Attributes.Add("onchange", "OnCheckOne(this);");
                                            cell.Controls.Add(chk);
                                            rowRow.Cells.Add(cell);

                                            cell = new TableCell();
                                            cell.Text = row.ToString();
                                            rowRow.Cells.Add(cell);

                                            cell = new TableCell();
                                            cell.Text = "<img src='/Images.aspx?path=" + r.ImgPicture + "' class='img-thumbnail' style='margin-top:3px;' />";
                                            rowRow.Cells.Add(cell);

                                            cell = new TableCell();
                                            cell.Text = r.ProtoNomor;
                                            rowRow.Cells.Add(cell);

                                            cell = new TableCell();
                                            cell.Text = r.Nomor;
                                            rowRow.Cells.Add(cell);

                                            cell = new TableCell();
                                            cell.Text = r.StockProductDJ_CharProduct.CharProductItem.Nama.ToString();
                                            rowRow.Cells.Add(cell);

                                            int TipeLokasi = r.StockActualDJ.TipeLokasi;
                                            int IDLokasi = r.StockActualDJ.IDLokasi;
                                            string NamaLokasi = "";

                                            if (TipeLokasi == 0)
                                                NamaLokasi = db.LocWarehouse.SingleOrDefault(p => p.ID == IDLokasi).Nama;
                                            else if (TipeLokasi == 1)
                                                NamaLokasi = db.LocOutlet.SingleOrDefault(p => p.ID == IDLokasi).Nama;
                                            else if (TipeLokasi == 2)
                                                NamaLokasi = db.LocExhibition.SingleOrDefault(p => p.ID == IDLokasi).Nama;

                                            cell = new TableCell();
                                            cell.Text = NamaLokasi;
                                            rowRow.Cells.Add(cell);

                                            cell = new TableCell();
                                            cell.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
                                            rowRow.Cells.Add(cell);

                                            cell = new TableCell();
                                            cell.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.GrossWeight).ToString("N2");
                                            rowRow.Cells.Add(cell);

                                            cell = new TableCell();
                                            cell.Text = Convert.ToInt32(r.StockProductDJ_CharProduct.StoneQty).ToString("N0");
                                            rowRow.Cells.Add(cell);

                                            cell = new TableCell();
                                            cell.Text = Convert.ToDecimal(r.StockProductDJ_CharProduct.StoneCarat).ToString("N3");
                                            rowRow.Cells.Add(cell);

                                            cell = new TableCell();
                                            cell.Text = Convert.ToDecimal(r.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                                            rowRow.Cells.Add(cell);

                                            cell = new TableCell();
                                            cell.Text = Convert.ToDecimal(oneprice).ToString("N0");
                                            rowRow.Cells.Add(cell);

                                            tb.Rows.Add(rowRow);
                                            row++;
                                        }
                                    }
                                    tb.Visible = true;
                                    lit_msg.Visible = false;
                                    lit_msg.Text = "";
                                    btn_click_modal_save.Visible = true;
                                }
                                else
                                {
                                    tb.Visible = false;
                                    lit_msg.Visible = true;
                                    lit_msg.Text = errorvalidasi;
                                    btn_click_modal_save.Visible = false;
                                }
                            }
                            else
                            {
                                tb.Visible = false;
                                lit_msg.Visible = true;
                                lit_msg.Text = errorvalidasi.ToString();
                                btn_click_modal_save.Visible = false;
                            }
                        }
                        else
                        {
                            tb.Visible = false;
                            lit_msg.Visible = true;
                            lit_msg.Text = "Data Di Excel Kosong.";
                            btn_click_modal_save.Visible = false;
                        }
                    }
                    catch(Exception ex)
                    {
                        tb.Visible = false;
                        lit_msg.Visible = true;
                        lit_msg.Text = ex.Message;//"Format Excel Tidak Sesuai, Silakan Download Ulang Template Excel.";
                        btn_click_modal_save.Visible = false;
                    }
                }
            }
        }
        else
        {
            tb.Visible = false;
            lit_msg.Text = "Please upload a valid Excel file.";
            lit_msg.Visible = false;
            btn_click_modal_save.Visible = false;
        }

        /*nama.Text = nama_promo;
        datepicker.Text = start;
        datepicker2.Text = end;*/
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false)]
    public static object CheckAvailableNamaPromo(string nama)
    {
        ResponMsg t = new ResponMsg() { msg = "", success = true };
        string query = "SELECT COUNT('') [Total] FROM PromoDJ WHERE Nama = @Nama AND Draft = 0";
        int x = 0;
        using (SqlConnection sqlCnn = new SqlConnection(Db.Cnn_DB))
        {
            SqlCommand cmd = new SqlCommand(query, sqlCnn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("@Nama", SqlDbType.VarChar) { Value = nama });
            sqlCnn.Open();
            x = (int)cmd.ExecuteScalar();
            sqlCnn.Close();
        }

        if(x > 0)
        {
            t.msg = "Nama Promo Sudah Pernah Digunakan.";
            t.success = false;
        }

        return t;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = false)]
    public static object Save(Products products)
    {
        ResponMsg t = new ResponMsg();
        int count = products.products.Count;
        if(count > 0)
        {
            t.success = true;
            t.msg = "";

            string kodekaret1 = "";
            int oneprice1 = 0;
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("PLU", typeof(string));

            int row = 1;
            foreach (var c in products.products)
            {
                string kodekaret2 = c.KodeKaret;
                string plu2 = c.PLU;
                int oneprice2 = Convert.ToInt32(c.OnePrice);

                DataRow dr = dataTable.NewRow();
                dr["PLU"] = c.PLU;
                dataTable.Rows.Add(dr);

                if (kodekaret1 == "" && row == 1)
                {
                    kodekaret1 = kodekaret2;
                    oneprice1 = oneprice2;
                }

                if (kodekaret1 != kodekaret2 && row > 1)
                {
                    t.success = false;
                    t.msg = "Terdapat Kode Karet Yang Berbeda Pada Upload Excel.";
                    break;
                }

                if (oneprice1 != oneprice2 && row > 1)
                {
                    t.success = false;
                    t.msg = "Terdapat One Price Yang Berbeda Pada Upload Excel.";
                    break;
                }
                row++;
            }

            if (t.success)
            {
                string query = "sp_insert_dj_one_price";
                string namapromo = products.Nama;
                string keterangan = products.Keterangan;
                string[] tgl_start = products.TglStart.Split('-');
                string[] tgl_end = products.TglExpired.Split('-');

                string tgl_start_new = tgl_start[2] + "-" + tgl_start[1] + "-" + tgl_start[0];
                string tgl_end_new = tgl_end[2] + "-" + tgl_end[1] + "-" + tgl_end[0];
                string operatornama = App.UserNama;
                string status_asp = products.NoExchange ? "1" : "0";
                bool status_disc_normal = products.NoDiscountNormal;
                bool status_disc_member = products.NoDiscountMember;
                int oneprice = oneprice1;

                List<SqlParameter> parameters = new List<SqlParameter>()
                {
                    new SqlParameter("@DATA", SqlDbType.Structured){ Value = dataTable },
                    new SqlParameter("@NAMA", SqlDbType.VarChar){ Value = namapromo },
                    new SqlParameter("@KETERANGAN", SqlDbType.VarChar){ Value = keterangan },
                    new SqlParameter("@TGL_START", SqlDbType.Date){ Value = tgl_start_new },
                    new SqlParameter("@TGL_EXPIRE", SqlDbType.Date){ Value = tgl_end_new },
                    new SqlParameter("@OPERATOR_NAMA", SqlDbType.VarChar){ Value = operatornama },
                    new SqlParameter("@STATUS_ASP", SqlDbType.Int){ Value = status_asp },
                    new SqlParameter("@STATUS_DISC_NORMAL", SqlDbType.Bit){ Value = status_disc_normal },
                    new SqlParameter("@STATUS_DISC_MEMBER", SqlDbType.Bit){ Value = status_disc_member },
                    new SqlParameter("@ONE_PRICE", SqlDbType.Money){ Value = oneprice }
                };

                DataTable dt2 = RsStatic(query, Db.Cnn_DB, parameters);
                t.success = Convert.ToBoolean(dt2.Rows[0]["STATUS"]);
                t.msg = dt2.Rows[0]["MESSAGE"].ToString();
            }
        }
        else
        {
            t.success = false;
            t.msg = "Data Kosong.";
        }
        return t;
    }

    //Data Table Builder
    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var x in param)
            {
                cmd.Parameters.Add(x);
            }
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
            sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
            DataSet objDS = new DataSet();
            sqlAdapter.Fill(objDS, "data");
            sqlCnn.Close();
            rs = objDS.Tables["data"];
        }
        return rs;
    }

    //Data Table Builder
    protected static DataTable RsStatic(string strSql, string CnnString, List<SqlParameter> param)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            SqlCommand cmd = new SqlCommand(strSql, sqlCnn);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (var x in param)
            {
                cmd.Parameters.Add(x);
            }
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
            sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
            DataSet objDS = new DataSet();
            sqlAdapter.Fill(objDS, "data");
            sqlCnn.Close();
            rs = objDS.Tables["data"];
        }
        return rs;
    }

    public class Products
    {
        public string Nama { get; set; }
        public string Keterangan { get; set; }
        public string TglStart { get; set; }
        public string TglExpired { get; set; }
        public List<Product> products { get; set; }
        public bool NoExchange { get; set; }
        public bool NoDiscountMember { get; set; }
        public bool NoDiscountNormal { get; set; }
    }
    public class Product
    {
        public string KodeKaret { get; set; }
        public string PLU { get; set; }
        public decimal OnePrice { get; set; }
    }
    public class ResponMsg
    {
        public bool success { get; set; }
        public string msg { get; set; }
    }
}
