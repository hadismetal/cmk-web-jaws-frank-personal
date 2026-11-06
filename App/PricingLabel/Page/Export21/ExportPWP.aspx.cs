using Ionic.Zip;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class Page_ExportPWP : System.Web.UI.Page
{
    EncryptDecrypt enc = new EncryptDecrypt();
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            GetDataPWP(pwp_name);
        }
    }

    public void GetDataPWP(DropDownList c)
    {
        string query = "SELECT DISTINCT A.ID, A.Nama FROM PromoPWP A " +
        "JOIN PromoPWP_Detail B ON A.ID = B.IDForm " +
        "JOIN StockProductDJ C ON B.IDProduct = C.ID " +
        "WHERE TglStart <= GETDATE() AND TglExpired >= GETDATE() AND Status = 1 ";
        //"WHERE A.Nama + C.Nomor LIKE '%' + @KW + '%' ";
        DataTable dt = Rs(query, Db.Cnn_DB, null, CommandType.Text);

        c.Items.Clear();
        foreach (DataRow dr in dt.Rows)
        {
            c.Items.Add(new ListItem(dr["Nama"].ToString(), dr["ID"].ToString()));
        }
    }

    public class RequestPWP
    {
        public int id { get; set; }
        public int item { get; set; }
        public int tipe_location { get; set; }
        public int id_location { get; set; }
        public string no_plu_start { get; set; }
        public string no_plu_end { get; set; }
        public int clarity { get; set; }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false)]
    public static object Display(RequestPWP request)
    {
        string user = string.Empty;
        int userid = 0;
        if (IsSessionExpired())
        {
            return new ResponsePWP { success = false, error = "Session expired" };
        }
        else
        {
            user = HttpContext.Current.Session["UserNama"].ToString();
            userid = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        }

        try
        {
            string query = "sp_get_data_pwp";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("@ID", SqlDbType.Int){ Value = request.id },
                new SqlParameter("@ITEM", SqlDbType.VarChar){ Value = request.item },
                new SqlParameter("@TIPE_LOCATION", SqlDbType.Int){ Value = request.tipe_location },
                new SqlParameter("@ID_LOCATION", SqlDbType.Int){ Value = request.id_location },
                new SqlParameter("@NO_PLU_START", SqlDbType.VarChar){ Value = request.no_plu_start },
                new SqlParameter("@NO_PLU_END", SqlDbType.VarChar){ Value = request.no_plu_end },
                new SqlParameter("@CLARITY", SqlDbType.Int){ Value = request.clarity }
            };
            DataSet ds = DsStatic(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
            DataTable dtclarity = ds.Tables[0];
            DataTable dtproductitem = ds.Tables[1];
            DataTable dtlocation = ds.Tables[2];
            DataTable dtitem = ds.Tables[3];

            List<ResponseClarityPWP> data_clarity = new List<ResponseClarityPWP>();
            List<ResponseProductItemPWP> data_product_item = new List<ResponseProductItemPWP>();
            List<ResponseLocationPWP> data_location = new List<ResponseLocationPWP>();
            List<ResponseItemPWP> data_item = new List<ResponseItemPWP>();

            foreach (DataRow dr in dtclarity.Rows)
            {
                ResponseClarityPWP clarity = new ResponseClarityPWP();
                clarity.id = Convert.ToInt32(dr["ID"]);
                clarity.nama = dr["TEXT"].ToString();
                clarity.total = Convert.ToInt32(dr["TOTAL"]);

                data_clarity.Add(clarity);
            }

            foreach (DataRow dr in dtproductitem.Rows)
            {
                ResponseProductItemPWP productitem = new ResponseProductItemPWP();
                productitem.id = Convert.ToInt32(dr["ID"]);
                productitem.nama = dr["TEXT"].ToString();
                productitem.total = Convert.ToInt32(dr["TOTAL"]);

                data_product_item.Add(productitem);
            }

            foreach (DataRow dr in dtlocation.Rows)
            {
                ResponseLocationPWP location = new ResponseLocationPWP();
                location.id = Convert.ToInt32(dr["ID"]);
                location.tipe = Convert.ToInt32(dr["TIPE"]);
                location.nama = dr["TEXT"].ToString();
                location.kode = dr["KODE"].ToString();
                location.total = Convert.ToInt32(dr["TOTAL"]);

                data_location.Add(location);
            }

            foreach (DataRow dr in dtitem.Rows)
            {
                ResponseItemPWP item = new ResponseItemPWP();
                item.id_product = Convert.ToInt32(dr["ID_PRODUCT"]);
                item.tipe_product = Convert.ToInt32(dr["TIPE_PRODUCT"]);
                item.nomor = dr["NOMOR"].ToString();
                item.kode_karet = dr["KODE_KARET"].ToString();
                item.item = dr["ITEM"].ToString();
                item.location = dr["LOCATION"].ToString();
                item.clarity = dr["CLARITY"].ToString();
                item.harga_barcode = Convert.ToDecimal(dr["HARGA_BARCODE"]);
                item.harga_pwp = Convert.ToDecimal(dr["HARGA_PWP"]);
                item.kadar_logam = dr["KADAR_LOGAM"].ToString() + "%";
                data_item.Add(item);
            }

            return new ResponsePWP
            {
                success = true,
                error = string.Empty,
                data_clarity = data_clarity,
                data_product_item = data_product_item,
                data_location = data_location,
                data_item = data_item
            };
        }
        catch (Exception ex)
        {
            return new ResponsePWP
            {
                success = false,
                error = "Error: " + ex.Message
            };
        }
    }

    protected static bool IsSessionExpired()
    {
        if (HttpContext.Current.Session == null)
            return true;

        return HttpContext.Current.Session["UserID"] == null &&
               HttpContext.Current.Session["UserNama"] == null;
    }

    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType commandType)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = commandType;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }

    protected static DataSet DsStatic(string strSql, string CnnString, List<SqlParameter> param, CommandType commandType)
    {
        DataSet rs = new DataSet();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = commandType;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }

    public class ResponsePWP
    {
        public bool success { get; set; }
        public string error { get; set; }
        public List<ResponseClarityPWP> data_clarity { get; set; }
        public List<ResponseProductItemPWP> data_product_item { get; set; }
        public List<ResponseLocationPWP> data_location { get; set; }
        public List<ResponseItemPWP> data_item { get; set; }
    }

    public class ResponseClarityPWP
    {
        public int id { get; set; }
        public string nama { get; set; }
        public int total { get; set; }
    }

    public class ResponseProductItemPWP
    {
        public int id { get; set; }
        public string nama { get; set; }
        public int total { get; set; }
    }

    public class ResponseLocationPWP
    {
        public int id { get; set; }
        public int tipe { get; set; }
        public string nama { get; set; }
        public string kode { get; set; }
        public int total { get; set; }
    }

    public class ResponseItemPWP
    {
        public int id_product { get; set; }
        public int tipe_product { get; set; }
        public string nomor { get; set; }
        public string kode_karet { get; set; }
        public string item { get; set; }
        public string location { get; set; }
        public string clarity { get; set; }
        public decimal harga_barcode { get; set; }
        public decimal harga_pwp { get; set; }
        public string kadar_logam { get; set; }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(UseHttpGet = false)]
    public static object Export(int[] idproducts)
    {
        string user = string.Empty;
        int userid = 0;
        if (IsSessionExpired())
        {
            return new ResponseExport { success = false, error = "Session expired" };
        }
        else
        {
            user = HttpContext.Current.Session["UserNama"].ToString();
            userid = Convert.ToInt32(HttpContext.Current.Session["UserID"].ToString());
        }
        try
        {
            int limitdata = 500;

            // =========================================
            // JIKA <= 200 → NORMAL CSV
            // =========================================
            if (idproducts.Length <= limitdata)
            {
                StringBuilder builder = Set(idproducts);
                byte[] bytes = Encoding.UTF8.GetBytes(builder.ToString());
                string base64 = Convert.ToBase64String(bytes);

                string filename = "DJPWP_"
                    + App.UserNama.Replace(" ", "") + "_"
                    + System.DateTime.Now.Year.ToString().Substring(2)
                    + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
                    + System.DateTime.Now.Day.ToString().PadLeft(2, '0')
                    + System.DateTime.Now.Hour.ToString().PadLeft(2, '0')
                    + System.DateTime.Now.Minute.ToString().PadLeft(2, '0')
                    + System.DateTime.Now.Second.ToString().PadLeft(2, '0')
                    + System.DateTime.Now.Millisecond.ToString().PadLeft(3,'0')
                    + "_" + idproducts.Length.ToString() + ".csv";

                return new ResponseExport()
                {
                    success = true,
                    filename = filename,
                    filedata = base64
                };
            }

            using (MemoryStream zipStream = new MemoryStream())
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestSpeed;
                    int fileIndex = 1;

                    for (int i = 0; i < idproducts.Length; i += limitdata)
                    {
                        int[] chunk = idproducts
                            .Skip(i)
                            .Take(limitdata)
                            .ToArray();

                        StringBuilder builder = Set(chunk);
                        byte[] fileBytes = Encoding.UTF8.GetBytes(builder.ToString());

                        string entryName = GenerateFileName(user, idproducts.Length, ".csv", i + 1);

                        zip.AddEntry(entryName, fileBytes);

                        fileIndex++;
                    }

                    zip.Save(zipStream);
                }

                byte[] zipBytes = zipStream.ToArray();
                string base64Zip = Convert.ToBase64String(zipBytes);

                return new ResponseExport()
                {
                    success = true,
                    filename = "DJPWP.zip",
                    filedata = base64Zip
                };
            }
        }
        catch (Exception ex)
        {
            return new ResponseExport()
            {
                success = false,
                error = ex.ToString()
            };
        }
    }

    private static string GenerateFileName(string user, int totalData, string extension, int index)
    {
        DateTime now = DateTime.Now;

        return "DJPWP_"
            + user.Replace(" ", "") + "_"
            + now.Year.ToString().Substring(2)
            + now.Month.ToString().PadLeft(2, '0')
            + now.Day.ToString().PadLeft(2, '0')
            + now.Hour.ToString().PadLeft(2, '0')
            + now.Minute.ToString().PadLeft(2, '0')
            + now.Second.ToString().PadLeft(2, '0')
            + "_" + totalData.ToString()
            + "_" + index.ToString() + extension;
    }

    public class ResponseExport
    {
        public bool success { get; set; }
        public string error { get; set; }
        public string filename { get; set; }
        public string filedata { get; set; }
    }

    protected static StringBuilder Set(int[] idproducts)
    {
        string xTest = "0";
        using (Map_DB db = new Map_DB())
        {
            var dj = (from p in db.StockProductDJ
                      orderby p.Nomor
                      where (p.Legacy == null || p.Legacy == 0) && p.Draft == false
                         && idproducts.Contains(p.ID)
                      select p);
            dj = dj.OrderBy(p => p.Nomor);

            xTest = "5A";

            var x = (from p in dj
                     select new
                     {
                         SupplierNomor = p.SupplierNomor,
                         Supplier = p.Supplier,
                         ProductItem = p.StockProductDJ_CharProduct.CharProductItem.NamaKode,
                         ProductItemNama = p.StockProductDJ_CharProduct.CharProductItem.Nama,
                         ProductCategory = p.StockProductDJ_CharProduct.CharProductCategory.NamaKode,
                         Tgl = p.Tgl,
                         ID = p.ID,
                         ProductDimensiR = p.StockProductDJ_CharProduct.DimensiR,
                         Nomor = p.Nomor,
                         //Harga = q.PromoDJ.PromoHarga,
                         Harga = p.StockProductDJ_PricingMU.HargaRupiahRound,
                         KadarLogam = p.StockProductDJ_CharProduct.KadarLogam,
                         StoneQty = p.StockProductDJ_CharProduct.StoneQty,
                         StoneCarat = p.StockProductDJ_CharProduct.StoneCarat,
                         NettoWeight = p.StockProductDJ_CharProduct.NettoWeight,
                         CharFrameMaterial = p.StockProductDJ_CharProduct.CharFrameMaterial.NamaKode,
                         Keterangan = p.Keterangan,
                         ProductLevel = p.StockProductDJ_CharProduct.CharProductLevel.NamaKode,
                         CompanyBrand = p.LocWarehouse.CompanyBrand.NamaKode,
                         ExtraRingSize = (Db.SingleInteger("DECLARE @COUNT INT = (SELECT COUNT('') FROM PromoDJ A JOIN PromoDJ_Detail B ON A.ID = B.IDForm WHERE TglStart <= GETDATE() AND TglExpire >= GETDATE() AND B.IDProduct = '" + p.ID + "') IF(@COUNT = 1) BEGIN SELECT TOP 1 CAST(ISNULL(ExtraRingSize,0) AS INT) FROM PromoDJ A JOIN PromoDJ_Detail B ON A.ID = B.IDForm WHERE TglStart <= GETDATE() AND TglExpire >= GETDATE() AND B.IDProduct = '" + p.ID + "' END ELSE BEGIN SELECT 0 END ", Db.Cnn_DB)),
                         HargaPWP = (Db.SingleDecimal("SELECT ISNULL(HargaPWP, 0)[HargaPWP] FROM StockProductDJ A " +
                            "LEFT JOIN(SELECT ISNULL(HargaPWP, 0)[HargaPWP], B.IDProduct FROM PromoPWP A " +
                            "JOIN PromoPWP_Detail B ON A.ID = B.IDForm " +
                            "WHERE B.IDProduct = " + p.ID + ") B ON A.ID = B.IDProduct " +
                            "WHERE A.ID = " + p.ID, Db.Cnn_DB))
                     }).OrderBy(p => p.Nomor);

            xTest = "5B";

            StringBuilder builder = new StringBuilder();
            if (dj.Count() > 0)
            {
                xTest = "6";

                int count = 1;

                foreach (var rslt in x)
                {
                    string xNomor = "";
                    string xKode = "";
                    string xItem = "";
                    string xHarga = "";
                    string xKadar = "";
                    string xStoneQty = "";
                    string xStoneCrt = "";
                    string xNetto = "";
                    string xMaterial = "";
                    string xRingSize = "";
                    string xNettDisc = "";
                    string xHargaPWP = "";

                    xNomor = rslt.Nomor;
                    if (rslt.Keterangan == "Import Stock")
                    {
                        xKode = rslt.SupplierNomor;
                    }
                    else
                    {
                        if (rslt.Supplier.HasValue && rslt.Supplier.Value != 0)
                        {

                            //Kode += rslt.SupplierNomor.Substring(0, 1);
                            int IDGroup = Db.SingleInteger("SELECT IDGroup FROM DataSupplier WHERE ID=" + rslt.Supplier, Db.Cnn_DB);
                            var sp = Db.SingleString("SELECT NamaKode FROM DataSupplierGroup WHERE ID=" + IDGroup, Db.Cnn_DB);
                            xKode = sp.Substring(0, 1);
                        }
                        xKode += rslt.ProductItem;
                        xKode += rslt.ProductCategory;
                        xKode += Convert.ToDateTime(rslt.Tgl).ToString("MM");
                        xKode += Convert.ToDateTime(rslt.Tgl).ToString("yy");
                        xKode += "1";
                    }
                    xItem = rslt.ProductItemNama;
                    xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                    xKadar = Math.Floor(rslt.KadarLogam).ToString("N0") + "%";
                    xStoneQty = Convert.ToDecimal(rslt.StoneQty).ToString("N0");
                    xStoneCrt = Convert.ToDecimal(rslt.StoneCarat).ToString("N3");
                    xNetto = Convert.ToDecimal(rslt.NettoWeight).ToString("N2") + "gr";
                    xMaterial = rslt.CharFrameMaterial;
                    xRingSize = "0";
                    try
                    {
                        xRingSize = Convert.ToDecimal(rslt.ProductDimensiR).ToString("N2");
                    }
                    catch (Exception ex) { }
                    xRingSize += "sz";
                    xNettDisc = (Pricing.isNett(rslt.Nomor)) ? "N" : "D";
                    xHargaPWP = Convert.ToDecimal(rslt.HargaPWP).ToString("N0");

                    xTest = "7";

                    string xExtraRingSize = (rslt.ExtraRingSize > 0) ? " (+" + rslt.ExtraRingSize + " sz)" : "";

                    // Builder : Start
                    if (count > 1) { builder.Append("\n"); }

                    builder.Append("\"" + EncryptSha(xNomor) + "\",");
                    builder.Append("\"" + EncryptSha(xKode) + "\",");
                    builder.Append("\"" + EncryptSha(xItem) + "\",");
                    builder.Append("\"" + EncryptSha(xHarga + "#" + xHargaPWP) + "\",");
                    builder.Append("\"" + EncryptSha(xKadar) + "\",");
                    builder.Append("\"" + EncryptSha(xStoneQty) + "\",");
                    builder.Append("\"" + EncryptSha(xStoneCrt) + "\",");
                    builder.Append("\"" + EncryptSha(xNetto) + "\",");
                    builder.Append("\"" + EncryptSha(xMaterial) + "\",");
                    builder.Append("\"" + EncryptSha(xRingSize + xExtraRingSize) + "\",");
                    builder.Append("\"" + EncryptSha(xNettDisc) + "\",");
                    
                    /*builder.Append("\"" + xNomor + "\",");
                    builder.Append("\"" + xKode + "\",");
                    builder.Append("\"" + xItem + "\",");
                    builder.Append("\"" + xHarga + "\",");
                    builder.Append("\"" + xKadar + "\",");
                    builder.Append("\"" + xStoneQty + "\",");
                    builder.Append("\"" + xStoneCrt + "\",");
                    builder.Append("\"" + xNetto + "\",");
                    builder.Append("\"" + xMaterial + "\",");
                    builder.Append("\"" + xRingSize + xExtraRingSize + "\",");
                    builder.Append("\"" + xNettDisc + "\",");*/

                    xTest = "8";

                    // Builder : Stone List
                    builder.Append("\"0\","); //sudah tidak terpakai tapi tetap harus dibuild

                    string plunumber = rslt.Nomor;
                    string stonemodelist = "b"; // b:bandroll / c:certificate
                    string plutype = "dj"; // dj or ld

                    /*string spname = "sp_get_stone_information_for_bandroll_certificate";
                    string query = " exec " + spname
                                 + " '" + stonemodelist + "'"
                                 + ",'" + plunumber + "'"
                                 + ",'" + plutype + "'"
                                 ;*/

                    //DataTable dt = new DataTable();
                    //dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

                    string query = "sp_get_stone_information_for_bandroll_certificate_report_new";
                    List<SqlParameter> sqlParameters = new List<SqlParameter>()
                                {
                                    new SqlParameter { Value = plunumber, ParameterName = "@PLU", SqlDbType = SqlDbType.VarChar },
                                    new SqlParameter { Value = plutype, ParameterName = "@TYPE", SqlDbType = SqlDbType.VarChar },
                                    new SqlParameter { Value = stonemodelist, ParameterName = "@MODE", SqlDbType = SqlDbType.VarChar }
                                };

                    DataTable dt = DbAccess.Rs(query, Db.Cnn_DB, sqlParameters);

                    if (dt.Rows.Count > 0)
                    {
                        string TotalButir = "";
                        string TotalCarat = "";
                        string Keterangan = "";

                        foreach (DataRow dtRow in dt.Rows)
                        {
                            TotalButir = Convert.ToDecimal(dtRow["TotalButir"]).ToString("N0");
                            TotalCarat = Convert.ToDecimal(dtRow["TotalCarat"]).ToString("N3");
                            Keterangan = (dtRow["Keterangan"]).ToString();

                            builder.Append("\"" + EncryptSha(TotalButir) + "\",");
                            builder.Append("\"" + EncryptSha(TotalCarat + " " + Keterangan) + "\",");
                        }
                    }

                    xTest = "9";

                    count++;
                }
            }

            return builder;
        }
    }

    protected static string EncryptSha(string value)
    {
        byte[] _keyByte = new byte[0];
        string _key = "Pass@123#";
        string key = "Mondial123";
        string iv = "";
        byte[] _ivByte = new byte[8] { 1, 18, 35, 52, 69, 86, 103, 120 };

        string result = string.Empty;
        MemoryStream memoryStream = null;
        CryptoStream cryptoStream = null;
        if (!string.IsNullOrEmpty(value))
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _keyByte = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                    if (!string.IsNullOrEmpty(iv))
                    {
                        _ivByte = Encoding.UTF8.GetBytes(iv.Substring(0, 8));
                    }
                }
                else
                {
                    _keyByte = Encoding.UTF8.GetBytes(_key);
                }

                DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                memoryStream = new MemoryStream();
                cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(_keyByte, _ivByte), CryptoStreamMode.Write);
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
                result = Convert.ToBase64String(memoryStream.ToArray());
            }
            catch
            {

            }
            finally
            {
                cryptoStream.Dispose();
                memoryStream.Dispose();
            }
        }

        return result;
    }
}