using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Web;
using System.Configuration;


public partial class DOKRM_detail : System.Web.UI.Page
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

    private static string BASE_URL = ConfigurationManager.AppSettings["STAMPSBASEURL"];
    private static string TOKEN = ConfigurationManager.AppSettings["STAMPSAPI"];
    private static string MERCHANT = ConfigurationManager.AppSettings["STAMPSMerchant"];


    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IPaymentType> GetPaymentType()
    {
        string query = "SELECT * FROM V_PaymentRepairType";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);


        List<IPaymentType> paymentType = new List<IPaymentType>();
        foreach (DataRow dr in dt.Rows)
        {
            paymentType.Add(new IPaymentType
            {
                ID = (int)dr["ID"],
                Nama = (string)dr["Nama"],
                Icon = (string)dr["Icon"]
            });
        }

        return paymentType;
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetDefaultData()
    {
        List<IIDName> BankIssuers = new List<IIDName>();
        List<IIDName> QRISBankIssuers = new List<IIDName>();
        List<IIDName> EDCLists = new List<IIDName>();
        List<IIDName> Programs = new List<IIDName>();
        List<IIDName> CardTypes = new List<IIDName>();
        List<IIDName> CardClasifies = new List<IIDName>();
        List<IIDName> QRISEDC = new List<IIDName>();

        string query1 = "SELECT * FROM BankIssuer";
        DataTable dt1 = Db.Rs(query1, Db.Cnn_DB);
        foreach (DataRow dr in dt1.Rows)
        {
            BankIssuers.Add(new IIDName
            {
                ID = (int)dr["ID"],
                Nama = (string)dr["Nama"]
            });
        }

        string query2 = "SELECT * FROM QRISBankIssuer";
        DataTable dt2 = Db.Rs(query2, Db.Cnn_DB);
        foreach (DataRow dr in dt2.Rows)
        {
            QRISBankIssuers.Add(new IIDName
            {
                ID = (int)dr["ID"],
                Nama = (string)dr["Nama"]
            });
        }

        string query3 = "SELECT * FROM EDCList WHERE ID NOT IN (26, 27, 28, 29, 31, 33, 32)";
        DataTable dt3 = Db.Rs(query3, Db.Cnn_DB);
        foreach (DataRow dr in dt3.Rows)
        {
            EDCLists.Add(new IIDName
            {
                ID = (int)dr["ID"],
                Nama = (string)dr["Nama"]
            });
        }

        string query4 = "SELECT * FROM ProgramCicilan";
        DataTable dt4 = Db.Rs(query4, Db.Cnn_DB);
        foreach (DataRow dr in dt4.Rows)
        {
            Programs.Add(new IIDName
            {
                ID = (int)dr["ID"],
                Nama = (string)dr["Nama"]
            });
        }

        string query5 = "SELECT * FROM CardType";
        DataTable dt5 = Db.Rs(query5, Db.Cnn_DB);
        foreach (DataRow dr in dt5.Rows)
        {
            CardTypes.Add(new IIDName
            {
                ID = (int)dr["ID"],
                Nama = (string)dr["Nama"]
            });
        }

        string query6 = "SELECT * FROM JenisKartuKredit";
        DataTable dt6 = Db.Rs(query6, Db.Cnn_DB);
        foreach (DataRow dr in dt6.Rows)
        {
            CardClasifies.Add(new IIDName
            {
                ID = (int)dr["ID"],
                Nama = (string)dr["Nama"]
            });
        }

        string query7 = "SELECT * FROM EDCList WHERE ID IN (29, 33)";
        DataTable dt7 = Db.Rs(query7, Db.Cnn_DB);
        foreach (DataRow dr in dt7.Rows)
        {
            QRISEDC.Add(new IIDName
            {
                ID = (int)dr["ID"],
                Nama = (string)dr["Nama"]
            });
        }

        return new
        {
            BankIssuers = BankIssuers,
            QRISBankIssuer = QRISBankIssuers,
            CardTypes = CardTypes,
            CardClasifies = CardClasifies,
            Programs = Programs,
            EDCLists = EDCLists,
            QRISEDC = QRISEDC
        };
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetDetailRepair(int IDDocRepair)
    {
        IDocRepair docRepair = ExecuteDetailDocRepair(IDDocRepair);
        return docRepair;
    }

    [WebMethod]
    public static object InsertSalesOrderRepair(IInsertFormSalesOrderRepair insertFormSalesOrderRepair)
    {
        DataSet ds = ExecuteInsertSalesOrderRepair(insertFormSalesOrderRepair);
        DataTable dt = ds.Tables[0];

        List<object> Message = new List<object>();
        foreach (DataRow dr in dt.Rows)
        {
            Message.Add(new
            {
                IsFreeReparasi = (int)dr["IsFreeReparasi"]
            });
        }

        return Message;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object TestScriptNgentot(string voucher_code, string store)
    {
        try
        {
            var queryObject = new
            {
                token = TOKEN,
                voucher_code = voucher_code,
                store = store
            };

            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var prop in queryObject.GetType().GetProperties())
            {
                query[prop.Name] = prop.GetValue(queryObject).ToString();
            }

            string queryString = "?" + query.ToString();
            string apiUrl = BASE_URL + "vouchers/validate" + queryString;
            using (WebClient client = new WebClient())
            {
                string responseBody = client.DownloadString(apiUrl);
                return responseBody;
            }
        }
        catch (WebException e)
        {
            using (WebResponse response = e.Response)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    return text;
                }
            }
        }
    }

    [WebMethod]
    public static object TestScriptNgentotBapakKau(string identifier, string voucher_code, string store)
    {
        try
        {
            var queryObject = new
            {
                token = TOKEN,
                identifier = identifier,
                voucher_code = voucher_code,
                store = 1
            };

            string jsonData = JsonConvert.SerializeObject(queryObject);

            // string queryString = "?" + query.ToString();
            string apiUrl = BASE_URL + "vouchers/redeem"; // + queryString;
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string responseBody = client.UploadString(apiUrl, "POST", jsonData);
                return responseBody;
            }
        }
        catch (WebException e)
        {
            using (WebResponse response = e.Response)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                using (Stream data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    return text;
                }
            }
        }
    }

    public static IDocRepair ExecuteDetailDocRepair(int IDDocRepair)
    {
        IDocRepairDetail docRepairDetail = new IDocRepairDetail();
        List<IDocRepairParcel> docRepairParcel = new List<IDocRepairParcel>();
        List<IDocRepairPackage> docRepairPackage = new List<IDocRepairPackage>();

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getDetailRepairPayment", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.VarChar) { Value = IDDocRepair });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    docRepairDetail = new IDocRepairDetail
                    {
                        BeratTimbang = Convert.ToDecimal(dr["BeratTimbang"]),
                        BeratTimbangAwal = Convert.ToDecimal(dr["BeratTimbangAwal"]),
                        BeratTimbangAkhir = Convert.ToDecimal(dr["BeratTimbangAkhir"]),
                        BeratTimbangActual = Convert.ToDecimal(dr["BeratTimbangActual"]),
                        BeratTimbangPotong = Convert.ToDecimal(dr["BeratTimbangPotong"]),
                        FrameColor = dr["FrameColor"].ToString(),
                        FrameMaterial = dr["FrameMaterial"].ToString(),
                        IDDocRepair = (int)dr["IDDocRepair"],
                        ImgPicture = dr["ImgPicture"].ToString(),
                        Item = dr["Item"].ToString(),
                        KodeFrameMaterial = dr["KodeFrameMaterial"].ToString(),
                        KodeKaret = dr["KodeKaret"].ToString(),
                        NamaCustomer = dr["NamaCustomer"].ToString(),
                        NamaSales = dr["NamaSales"].ToString(),
                        NomorRepair = dr["NomorRepair"].ToString(),
                        PLU = dr["PLU"].ToString(),
                        SizeAkhir = Convert.ToDecimal(dr["SizeAkhir"]),
                        SizeAwal = Convert.ToDecimal(dr["SizeAwal"]),
                        TipeProduct = dr["TipeProduct"].ToString(),
                        TotalButir = (int)dr["TotalButir"],
                        TotalCarat = Convert.ToDecimal(dr["TotalCarat"]),
                        ImgProduct = dr["ImgProduct"].ToString(),
                        Store = dr["Store"].ToString(),
                        Invoice = dr["Invoice"].ToString(),
                        LocRepair = dr["LocRepair"].ToString(),
                        MarkUpR = Convert.ToDecimal(dr["MarkUpR"]),
                        MarkUpJual = Convert.ToDecimal(dr["MarkUpJual"]),
                        BatasToleransi = Convert.ToDecimal(dr["BatasToleransi"]),
                        HargaEmas = Convert.ToDecimal(dr["HargaEmas"]),
                        SelisihBerat = Convert.ToDecimal(dr["SelisihBerat"]),
                        HargaBayarM = Convert.ToDecimal(dr["HargaBayarM"]),
                        HargaBayarR = Convert.ToDecimal(dr["HargaBayarR"]),
                        HargaBayar = Convert.ToDecimal(dr["HargaBayar"]),
                        TotalGramLebih = Convert.ToDecimal(dr["TotalGramLebih"]),
                        Status = (int)dr["Status"],
                        IDCustomerStamps = dr["IDCustomerStamps"].ToString(),
                        KodeStore = dr["KodeStore"].ToString(),
                        BeratAkhirTanpaBatu = Convert.ToDecimal(dr["BeratAkhirTanpaBatu"])
                    };
                }
            }

            using (SqlCommand command = new SqlCommand("sp_getDetailParcelRepair_Payment", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.VarChar) { Value = IDDocRepair });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    docRepairParcel.Add(new IDocRepairParcel
                    {
                        IDDocRepair = (int)dr["IDDocRepair"],
                        Parcel = dr["Parcel"].ToString(),
                        CaratButir = Convert.ToDecimal(dr["CaratButir"]),
                        TotalCarat = Convert.ToDecimal(dr["TotalCarat"]),
                        TotalButir = Convert.ToDecimal(dr["TotalButir"]),
                        StoneType = dr["StoneType"].ToString(),
                        StoneTypeMap = dr["StoneTypeMap"].ToString(),
                        HargaSatuan = Convert.ToDecimal(dr["HargaSatuan"]),
                        HargaTotal = Convert.ToDecimal(dr["HargaTotal"]),
                        HargaTotalM = Convert.ToDecimal(dr["HargaTotalM"]),
                        HargaTotalRupiah = Convert.ToDecimal(dr["HargaTotalRupiah"]),
                        HargaTotalRupiahRound = Convert.ToDecimal(dr["HargaTotalRupiahRound"]),
                        Keterangan = dr["Keterangan"].ToString(),
                        Stone = dr["Stone"].ToString(),
                        Clarity = dr["Clarity"].ToString(),
                        Color = dr["Color"].ToString(),
                        HargaSettingRound = Convert.ToDecimal(dr["HargaSettingRound"]),
                        HargaPBRound = Convert.ToDecimal(dr["HargaPBRound"])
                    });
                }
            }

            using (SqlCommand command = new SqlCommand("sp_getDetailPackageRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.VarChar) { Value = IDDocRepair });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    docRepairPackage.Add(new IDocRepairPackage
                    {
                        IDPackage = (int)dr["IDPackage"],
                        IDDocRepair = (int)dr["IDDocRepair"],
                        NamaPackage = dr["NamaPackage"].ToString(),
                        NomorRepair = dr["NomorRepair"].ToString(),
                        TipePackage = (int)dr["TipePackage"],
                        NamaTipePackage = dr["NamaTipePackage"].ToString(),
                        Biaya = Convert.ToDecimal(dr["Biaya"]),
                        BiayaR = Convert.ToDecimal(dr["BiayaR"]),
                        BiayaJual = Convert.ToDecimal(dr["BiayaJual"]),
                    });
                }
            }
        }

        return new IDocRepair
        {
            docRepairDetail = docRepairDetail,
            docRepairPackage = docRepairPackage,
            docRepairParcel = docRepairParcel
        };
    }

    [WebMethod]
    public static void UpdateStatusPayment(int IDDocRepair)
    {
        string query = "UPDATE DocRepair_Pivot SET Status = 8 WHERE IDDocRepair = " + IDDocRepair;
        Db.Execute(query, Db.Cnn_DB);
    }

    public static DataSet ExecuteInsertSalesOrderRepair(IInsertFormSalesOrderRepair insertFormSalesOrderRepair)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_insertSalesOrderRepair", connection))
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(insertFormSalesOrderRepair.Payments);
                DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.VarChar) { Value = insertFormSalesOrderRepair.IDDocRepair });
                command.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar) { Value = App.UserNama });
                command.Parameters.Add(new SqlParameter("@Keterangan", SqlDbType.VarChar) { Value = insertFormSalesOrderRepair.Keterangan });
                command.Parameters.Add(new SqlParameter("@Payments", SqlDbType.Structured) { Value = pDt });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }


    public class IIDName
    {
        public int ID { get; set; }
        public string Nama { get; set; }
    }

    public class IInsertDocQCParams
    {
        public int IDDocQC { get; set; }
        public string Sertifikat { get; set; }
        public string Logo { get; set; }
        public string KadarEmas { get; set; }
        public string Merk { get; set; }
        public decimal NetWeight { get; set; }
        public string WarnaEmas { get; set; }
        public string FisikKeseluruhan { get; set; }
        public string OperatorNama { get; set; }
        public string Keterangan { get; set; }
        public decimal GrossWeight { get; set; }
        public string TipeProduct { get; set; }
    }

    public class IInsertDocRepairParams
    {
        public int IDDocRepair { get; set; }
        public string PLU { get; set; }
        public string SalesNama { get; set; }
        public string CustomerNama { get; set; }
        public string Operator { get; set; }
        public string Keterangan { get; set; }
        public string ImgPicture { get; set; }
        public int IDProduct { get; set; }
        public int IDProductTitipan { get; set; }
        public string TipeProduct { get; set; }
        public List<object> DocRepair_Package { get; set; }
        public string Pengrajin { get; set; }
        public int Size { get; set; }
        public decimal Weight { get; set; }
    }


    public class IPaymentRepair
    {
        public int IDPaymentType { get; set; }
        public int IDCard { get; set; }
        public int IDProgram { get; set; }
        public int IDEDC { get; set; }
        public int IDBankIssuer { get; set; }
        public string CCNumber { get; set; }
        public string CCName { get; set; }
        public decimal Nominal { get; set; }
        public int IDJenisKartuKredit { get; set; }
        public int NoIndex { get; set; }
        public string ApprovalKode { get; set; }
    }

    public class IInsertFormSalesOrderRepair
    {
        public int IDDocRepair { get; set; }
        public string Keterangan { get; set; }
        public List<IPaymentRepair> Payments { get; set; }
    }

    public class IPackageRepair
    {
        public int IDPackage { get; set; }
        public int IDLocReparasi { get; set; }
        public int IDLocPackageReparasi { get; set; }
        public string JenisReparasi { get; set; }
        public decimal Ongkos { get; set; }
        public int TipeReparasi { get; set; }
        public string NamaTipeReparasi { get; set; }
        public string Nama { get; set; }
        public string Kode { get; set; }
    }
    public class ILocRepair
    {
        public int ID { get; set; }
        public string Nama { get; set; }
        public string Kode { get; set; }
    }
    public class ILocAndPackageRepair
    {
        public List<IPackageRepair> PackageRepair { get; set; }
        public List<ILocRepair> LocRepair { get; set; }
    }
    public class ICheckPLUQCParameters
    {
        public string Tipe { get; set; }
        public string Keyword { get; set; }
    }
    public class ICharFrameColor
    {
        public string Nama { get; set; }
        public int ID { get; set; }
    }
    public class IPaymentType
    {
        public string Icon { get; set; }
        public string Nama { get; set; }
        public int ID { get; set; }
    }
    public class IHeaderPLUQC
    {
        public int ID { get; set; }
        public string NOMOR { get; set; }
        public string NOMORINVOICE { get; set; }
        public string NAMACUSTOMER { get; set; }
        public string IDCUSTOMER { get; set; }
        public string IMAGE { get; set; }
        public string ITEM { get; set; }
        public int ITEMID { get; set; }
        public string GROSS_WEIGHT { get; set; }
        public string FRAME_COLOR { get; set; }
        public int FRAME_COLOR_ID { get; set; }
        public decimal SIZE { get; set; }
    }
    public class IDetailPLUQC
    {
        public int ID { get; set; }
        public string STONETYPE { get; set; }
        public string STONETYPEMAP { get; set; }
        public string STONE { get; set; }
        public string TGL { get; set; }
        public int TOTAL_BUTIR { get; set; }
        public string TOTAL_CARAT { get; set; }
        public string GIA { get; set; }
        public string BRANDSTONE { get; set; }
    }
    public class IItemPLUQC
    {
        public IHeaderPLUQC Item { get; set; }
        public List<IDetailPLUQC> Stone { get; set; }
    }
    public class IDataSalesWithLocation
    {
        public int IDSales { get; set; }
        public string NamaSales { get; set; }
        public string NIK { get; set; }
        public string NamaOutlet { get; set; }
        public string KodeOutlet { get; set; }
        public int IDLokasi { get; set; }
        public int TipeLokasi { get; set; }
    }
    public class IDataLocation
    {
        public string NamaOutlet { get; set; }
        public string KodeOutlet { get; set; }
        public int IDLokasi { get; set; }
        public int TipeLokasi { get; set; }
    }
    public class ISalesLocationResponse
    {
        public List<IDataSalesWithLocation> sales { get; set; }
        public List<IDataLocation> location { get; set; }
    }
    public class IDataCustomer
    {
        public int ID { get; set; }
        public int CustomerStampsID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerBirthday { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerMembership { get; set; }
        public string CustomerEmail { get; set; }
    }
    public class IDataCustomerItem
    {
        public int IDProduct { get; set; }
        public int IDProductTitipan { get; set; }
        public string Nomor { get; set; }
        public decimal HargaBeli { get; set; }
        public string Item { get; set; }
        public string Image { get; set; }
        public int StatusStock { get; set; }
    }

    public class IDocRepair
    {
        public IDocRepairDetail docRepairDetail;
        public List<IDocRepairPackage> docRepairPackage;
        public List<IDocRepairParcel> docRepairParcel;
    }

    public class IDocRepairDetail
    {
        public int IDDocRepair { get; set; }
        public string NomorRepair { get; set; }
        public string NamaCustomer { get; set; }
        public string NamaSales { get; set; }
        public string PLU { get; set; }
        public string TipeProduct { get; set; }
        public string ImgPicture { get; set; }
        public string KodeKaret { get; set; }
        public string Item { get; set; }
        public string FrameColor { get; set; }
        public string FrameMaterial { get; set; }
        public string KodeFrameMaterial { get; set; }
        public int TotalButir { get; set; }
        public decimal TotalCarat { get; set; }
        public decimal BeratTimbangAwal { get; set; }
        public decimal BeratTimbangActual { get; set; }
        public decimal BeratTimbangPotong { get; set; }
        public decimal BeratTimbangAkhir { get; set; }
        public decimal BeratTimbang { get; set; }
        public decimal SizeAwal { get; set; }
        public decimal SizeAkhir { get; set; }
        public string ImgProduct { get; set; }
        public string Store { get; set; }
        public string Invoice { get; set; }
        public string LocRepair { get; set; }
        public decimal MarkUpR { get; set; }
        public decimal MarkUpJual { get; set; }
        public decimal BatasToleransi { get; set; }
        public decimal HargaEmas { get; set; }
        public decimal SelisihBerat { get; set; }
        public decimal HargaBayarM { get; set; }
        public decimal HargaBayarR { get; set; }
        public decimal HargaBayar { get; set; }
        public decimal TotalGramLebih { get; set; }
        public int Status { get; set; }
        public string IDCustomerStamps { get; set; }
        public string KodeStore { get; set; }
        public decimal BeratAkhirTanpaBatu { get; set; }
    }

    public class IDocRepairParcel
    {
        public int IDDocRepair { get; set; }
        public string Parcel { get; set; }
        public decimal CaratButir { get; set; }
        public decimal TotalCarat { get; set; }
        public decimal TotalButir { get; set; }
        public string StoneType { get; set; }
        public string StoneTypeMap { get; set; }
        public decimal HargaSatuan { get; set; }
        public decimal HargaTotal { get; set; }
        public decimal HargaTotalM { get; set; }
        public decimal HargaTotalRupiah { get; set; }
        public decimal HargaTotalRupiahRound { get; set; }
        public decimal HargaSettingRound { get; set; }
        public decimal HargaPBRound { get; set; }
        public string Keterangan { get; set; }
        public string Stone { get; set; }
        public string Clarity { get; set; }
        public string Color { get; set; }

    }

    public class IDocRepairPackage
    {
        public int IDDocRepair { get; set; }
        public string NomorRepair { get; set; }
        public string NamaPackage { get; set; }
        public int IDPackage { get; set; }
        public int TipePackage { get; set; }
        public string NamaTipePackage { get; set; }
        public decimal Biaya { get; set; }
        public decimal BiayaJual { get; set; }
        public decimal BiayaR { get; set; }

    }

    public class IVoucherRedeemArgs
    {
        public string identifier { get; set; }
        public string voucher_code { get; set; }
        public string store { get; set; }
    }
}