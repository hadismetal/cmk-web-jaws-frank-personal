using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Linq.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Linq.Dynamic;
using System.Web.Configuration;
using System.Globalization;
using ExcelDataReader;

public partial class Page_Register_PG_Form : System.Web.UI.Page
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

    private Map_CS _d;

    private Map_CS d
    {
        get
        {
            if (_d == null)
                _d = new Map_CS();
            return _d;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (d != null)
            d.Dispose();
        base.Dispose();
    }
    App price = new App();

    decimal GoldPrice = 0m;
    static int i = 1;

    protected int myIDWarehouse { get { return App.QueryInt(this, "IDWarehouse"); } }
    protected string SJ { get { return Request.QueryString["sj"]; } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);


        if (!IsPostBack)
        {
            //Form
            DLC.CharGoldModel(ddl_Model);
            DLC.DDLSupplier(ddl_Supplier);
            ddl_Supplier.SelectedValue = "211";
            DLC.WarehouseList(ddl_Warehouse);

            if(!string.IsNullOrEmpty(SJ))
            {
                string nottb = string.Empty;
                DataTable dt = Db.Rs("SELECT NoTTB FROM StockReceivePG WHERE SupplierSJ = '" + SJ + "' GROUP BY NoTTB", Db.Cnn_DB);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        nottb += dr["NoTTB"] + System.Environment.NewLine;
                    }
                }

                success.Text = "PLU Berhasil diupload NoTTB : " + nottb;
                success.Visible = true;
            }    
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("PG.aspx"); }

    protected void LoadModel(object sender, EventArgs e)
    {
        if (ddl_rate.SelectedValue == "0")
        {
            DLC.CharGoldModel(ddl_Model);
        }
        else
        {
            DLC.CharGoldModelGJ(ddl_Model);
        }
    }

    protected void uploadData(object sender, EventArgs e)
    {
        error.Visible = false;
        success.Visible = false;
        string SuratJalan = string.Empty;
        string Brand = string.Empty;

        if (ddl_Warehouse.SelectedValue == "-1")
        {
            error.Text = "Pilih Warehouse terlebih dahulu!";
            error.Visible = true;
        }
        else if (string.IsNullOrEmpty(txtTukaranJual.Text) || txtTukaranJual.Text == "")
        {
            error.Text = "Isi Kadar Tukaran Jual!";
            error.Visible = true;
        }

        if(!error.Visible)
        {
            if (Path.GetExtension(FileUpload1.PostedFile.FileName) == ".xlsx")
            {
                /*IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(FileUpload1.PostedFile.InputStream);
                string suratJalan = string.Empty;
                while (reader.Read())
                {
                    suratJalan = reader.GetString(2);
                    break;
                }

                DataSet ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                if (ds.Tables.Count > 0)
                {
                    save_data(ds.Tables[0], suratJalan);
                }*/
				error.Text = "File tidak sesuai, gunakan file extensi .csv";
                error.Visible = true;
            }
            else if (Path.GetExtension(FileUpload1.PostedFile.FileName) == ".csv")
            {
                string kodeBarang = string.Empty;
                DataTable dt = new DataTable();
                dt.Columns.Add("SuratJalan");
                dt.Columns.Add("KodeBarang");
                dt.Columns.Add("NoFaktur");
                dt.Columns.Add("SKU");
                dt.Columns.Add("Segmen");
                dt.Columns.Add("Item");
                dt.Columns.Add("Color");
                dt.Columns.Add("Kadar", typeof(decimal));
                dt.Columns.Add("KadarTukarBeli", typeof(decimal));
                dt.Columns.Add("KadarTukarJual", typeof(decimal));
                dt.Columns.Add("Size", typeof(decimal));
                dt.Columns.Add("Weight", typeof(string));
                dt.Columns.Add("KodeMaterial", typeof(string));
                dt.Columns.Add("NoValidasi", typeof(string));
                int index = 0;
                try
                {
                    using (var reader = new StreamReader(FileUpload1.FileContent, Encoding.Default))
                    {
                        string[] stringSeparators = new string[] { "\r\n" };
                        string[] read = reader.ReadToEnd().Split(stringSeparators, StringSplitOptions.None);
                        for (int i = 1; i < read.Length; i++)
                        {
                            string[] row = read[i].Split(';');
                            if (row.Length <= 1 || row.All(x => string.IsNullOrEmpty(x))) continue;

                            DataRow dr = dt.NewRow();

                            if (!string.IsNullOrEmpty(SuratJalan) && row[1] != SuratJalan)
                            {
                                error.Text = "File mengandung data lebih dari 1 surat jalan";
                                error.Visible = true;
                                break;
                            }
                            if (!string.IsNullOrEmpty(Brand) && row[4] != Brand)
                            {
                                error.Text = "File mengandung data lebih dari 1 Brand";
                                error.Visible = true;
                                break;
                            }
                            

                            row[7] = row[7].ToUpper() == "MULTIFUNCTION" ? "Ladies Ring" : row[7];

                            dr["SuratJalan"] = row[1];
                            dr["KodeBarang"] = row[6];
                            dr["NoFaktur"] = row[15];
                            dr["SKU"] = row[3];
                            dr["Segmen"] = row[4] == "SDW" ? "CZC" : row[16];
                            dr["Item"] = row[7].ToUpper() == "RING" ? "Ladies Ring" : row[7];
                            dr["Color"] = row[10];
                            dr["Kadar"] = (Convert.ToDecimal(row[8]) * 0.1m);
                            dr["KadarTukarBeli"] = Convert.ToDecimal(row[21].Replace('.', ',').Replace("%", ""));
                            dr["KadarTukarJual"] = Convert.ToDecimal(txtTukaranJual.Text);
                            dr["Size"] = Convert.ToDecimal(row[9]);
                            dr["Weight"] = row[11];
                            dr["KodeMaterial"] = row[23];
                            dr["NoValidasi"] = row [24];

                            dt.Rows.Add(dr);
                            if (string.IsNullOrEmpty(SuratJalan)) SuratJalan = row[1];
                            if (string.IsNullOrEmpty(Brand)) Brand = row[4];
                            kodeBarang = row[6];

                        }
                    }
                    if (!error.Visible)
                    {
                        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
                        {
                            SqlCommand cmd = conn.CreateCommand();
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "dbo.sp_InsertPGSKK"; //The name of the above mentioned stored procedure.  
                            SqlParameter param = cmd.Parameters.AddWithValue("@Data", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  
                            SqlParameter param2 = cmd.Parameters.Add("@HumanStock", SqlDbType.VarChar);
                            SqlParameter param3 = cmd.Parameters.Add("@Supplier", SqlDbType.VarChar);
                            SqlParameter param4 = cmd.Parameters.Add("@Warehouse", SqlDbType.VarChar);
                            SqlParameter param5 = cmd.Parameters.Add("@Model", SqlDbType.VarChar);
                            SqlParameter param6 = cmd.Parameters.Add("@RateType", SqlDbType.Int);
                            param2.Value = App.UserNama;
                            param3.Value = ddl_Supplier.SelectedValue;
                            param4.Value = ddl_Warehouse.SelectedValue;
                            param5.Value = ddl_Model.SelectedValue;
                            param6.Value = Convert.ToInt32(ddl_rate.SelectedValue);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    error.Text = ex.Message;
                    error.Visible = true;
                }
            }
            else
            {
                error.Text = "File tidak sesuai, gunakan file extensi .csv";
                error.Visible = true;
            }
        }
        if (!error.Visible)
        {
            string href = App.CurrentPath;
            href += "?d=1&sj=" + SuratJalan;

            Response.Redirect(href);
        }
    }

    protected void save_data(DataTable tb, string suratJalan)
    {
        try
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(tb.Rows[2][0].ToString());
            dt.Columns.Add(tb.Rows[2][1].ToString());
            dt.Columns.Add(tb.Rows[2][2].ToString());
            dt.Columns.Add(tb.Rows[2][3].ToString());
            dt.Columns.Add(tb.Rows[2][4].ToString());
            dt.Columns.Add(tb.Rows[2][5].ToString());
            dt.Columns.Add(tb.Rows[2][6].ToString());
            dt.Columns.Add(tb.Rows[2][7].ToString());
            dt.Columns.Add(tb.Rows[2][8].ToString());
            dt.Columns.Add(tb.Rows[2][9].ToString(), typeof(decimal));
            dt.Columns.Add(tb.Rows[2][10].ToString(), typeof(decimal));
            dt.Columns.Add(tb.Rows[2][11].ToString());
            dt.Columns.Add(tb.Rows[2][12].ToString());

            for(int i = 3; i < tb.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();

                for(int j = 0; j < dt.Columns.Count; j++)
                {
                    dr[tb.Rows[2][j].ToString()] = tb.Rows[i][j].ToString();
                }
                dt.Rows.Add(dr);
            }

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_CS"].ConnectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "dbo.sp_InsertPGSKK"; //The name of the above mentioned stored procedure.  
                SqlParameter param = cmd.Parameters.AddWithValue("@Data", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  
                SqlParameter param2 = cmd.Parameters.Add("@SuratJalan", SqlDbType.VarChar);
                SqlParameter param3 = cmd.Parameters.Add("@Consignment", SqlDbType.Int);
                param2.Value = suratJalan;

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            error.Visible = false;
        }
        catch (Exception e)
        {
            error.Text = "Format template/data tidak sesuai";
            error.Visible = true;
        }

    }

    protected void btnTemplate_Click(object sender, EventArgs e)
    {
        StringBuilder builder = new StringBuilder();
		string[] namakolom = new string[] { "No.PO", "No. Surat Jalan", "Nama Toko", "SKU", "Brand", "Product Code", "Kode Karet", "Item", "Kadar", "Size", "Warna", "Berat", "Quantity", "Date", "Collection", "NO. FAKTUR", "SEGMEN", "No. PO CMK", "Tanggal Delivered", "Count", "Month", "Tukaran", "Notes", "Kode Material", "Nomor Validasi"};
		string result = "";
		foreach (var column in namakolom)
		{
			result += "\"" + column + "\";";
		}

		builder.Append(result.Remove(result.Length - 1));

		Response.Clear();
		Response.ContentType = "text/csv";
		Response.AddHeader("Content-Disposition", "attachment;filename=Template.csv");
		Response.Write(builder.ToString());
		Response.End();
    }
}