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
using System.Web.Script.Serialization;


public partial class DOKRM_index : System.Web.UI.Page
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
    public static List<IPackageRepair> GetPackageItem(string kodeItem, int IDLocReparasi, int tipeReparasi)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("select *, NamaKode [KodeItem], Kode_Klasifikasi [KlasifikasiItem] from [DB_PROD_MANUFAKTUR].[dbmfg].[dbo].[V_MasterPackageReparasiNew] WHERE NamaKode = @kodeItem AND TipeReparasi = @tipeReparasi AND IDLocReparasi = @IDLocReparasi", connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@kodeItem", SqlDbType.VarChar) { Value = kodeItem });
                command.Parameters.Add(new SqlParameter("@tipeReparasi", SqlDbType.VarChar) { Value = tipeReparasi });
                command.Parameters.Add(new SqlParameter("@IDLocReparasi", SqlDbType.VarChar) { Value = IDLocReparasi });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                List<IPackageRepair> locRepair = Helper.GetDataFromDataTable<IPackageRepair>(ds.Tables[0]);
                return locRepair;
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<ILocRepair> GetLocationRepair()
    {
        var locationRepair = Db.Rs("SELECT ID_LokasiReparasi [ID], JenisLokasi [Nama], LokasiReparasi [Kode] FROM [DB_PROD_MANUFAKTUR].[dbmfg].[dbo].[msLokasiReparasi] WHERE StatusAktif = 'A'", Db.Cnn_DB);
        List<ILocRepair> locRepair = Helper.GetDataFromDataTable<ILocRepair>(locationRepair);
        return locRepair;
    }


    [WebMethod]
    public static int CreateDocRepair(IInsertFormRepair args)
    {
        ExecuteInsertRepair(args);
        return 1;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<ICharFrameColor> GetFrameColor()
    {
        string query = "SELECT ID, Nama FROM CharFrameColor";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);

        List<ICharFrameColor> frameColor = new List<ICharFrameColor>();
        foreach (DataRow dr in dt.Rows)
        {
            ICharFrameColor frame = new ICharFrameColor
            {
                ID = (int)dr["ID"],
                Nama = dr["Nama"].ToString()
            };
            frameColor.Add(frame);
        }

        return frameColor;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetDetailRepair(int IDDocRepair)
    {
        IDocRepair docRepair = ExecuteDetailDocRepair(IDDocRepair);
        return docRepair;
    }

    public static IDocRepair ExecuteDetailDocRepair(int IDDocRepair)
    {
        IDocRepairDetail docRepairDetail = new IDocRepairDetail();
        List<IDocRepairParcel> docRepairParcel = new List<IDocRepairParcel>();
        List<IDocRepairPackage> docRepairPackage = new List<IDocRepairPackage>();
        List<IDocRepairOther> docRepairOther = new List<IDocRepairOther>();
        List<IDocRepairCustomer> docRepairCustomer = new List<IDocRepairCustomer>();
        List<ISLokasiStoneSetting> docRepairLokasiStoneSetting = new List<ISLokasiStoneSetting>();

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
                        BeratTimbangActual = Convert.ToDecimal(dr["BeratTimbangActual"]),
                        BeratTimbangAkhir = Convert.ToDecimal(dr["BeratTimbangAkhir"]),
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
                        HargaBayar = Convert.ToDecimal(dr["HargaBayar"]),
                        TotalGramLebih = Convert.ToDecimal(dr["TotalGramLebih"]),
                        Status = (int)dr["Status"],
                        BeratAkhirTanpaBatu = Convert.ToDecimal(dr["BeratAkhirTanpaBatu"]),
                        KodeStore = dr["KodeStore"].ToString(),
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
                // DataTable dtOther = ds.Tables[1];

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

                // foreach (DataRow dr in dtOther.Rows)
                // {
                //     docRepairOther.Add(new IDocRepairOther
                //     {                                                
                //         NomorRepair = dr["NomorTrans"].ToString(),
                //         NamaPackage = dr["other_package_instruction"].ToString(),                        
                //         CostOther = Convert.ToDecimal(dr["other_cost_instruction"])
                //     });
                // }
            }

            string getDataCustomer = @"select b.ID,IDBrand, Nama, NoCustomer, ISNULL(AddrNoTelp,0) AddrNoTelp
            , ISNULL(AddrEmail,'') AddrEmail, ISNULL(b.IDCustomerStamps,0) as IDCustomerStamps, ax.KodeLokasi
                        from DocRepair a
                        JOIN DataCustomer b on b.ID=a.IDCustomer
                        join (
                            SELECT 1 AS TipeLokasi,ID AS IDLokasi, Kode as KodeLokasi from LocOutlet
                            UNION
                            SELECT 2 AS TipeLokasi,ID AS IDLokasi, Kode as KodeLokasi from LocExhibition
                        ) ax on ax.IDLokasi=a.IDLokasi and ax.TipeLokasi=a.TipeLokasi
                        where a.id=@IDDocRepair
                        
                        select ISNULL(C.IDProduct,0) IDProduct from DocRepair_PLUSetting a
                        JOIN DocRepair b on b.ID=a.IDDocRepair
                        LEFT JOIN StockActualDJ c on c.IDProduct=a.IDProduct and c.TipeLokasi=b.TipeLokasi and b.IDLokasi=c.IDLokasi
                        where IDDocRepair=@IDDocRepair";
            using (SqlCommand command1 = new SqlCommand(getDataCustomer, connection))
            {
                command1.CommandType = CommandType.Text;
                command1.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.VarChar) { Value = IDDocRepair });

                SqlDataAdapter adapter = new SqlDataAdapter(command1);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];

                foreach (DataRow rs in dt.Rows)
                {
                    docRepairCustomer.Add(new IDocRepairCustomer
                    {
                        IDCustomer = (int)rs["ID"],
                        NamaCustomer = (string)rs["Nama"],
                        NomorCustomer = (string)rs["NoCustomer"],
                        NomorTelpon = (string)rs["AddrNoTelp"],
                        Email = (string)rs["AddrEmail"],
                        IDCustomerStamps = (int)rs["IDCustomerStamps"],
                        KodeLokasi = (string)rs["KodeLokasi"]
                    });
                    
                }
                foreach (DataRow rs in dt1.Rows)
                {
                    docRepairLokasiStoneSetting.Add(new ISLokasiStoneSetting
                    {
                        IDProduct = (int)rs["IDProduct"]
                    });
                }
                
            }

        }        

        return new IDocRepair
        {
            docRepairDetail = docRepairDetail,
            docRepairPackage = docRepairPackage,
            docRepairParcel = docRepairParcel,
            docRepairOther = docRepairOther,
            docRepairCustomer = docRepairCustomer,
            docRepairLokasiStoneSetting = docRepairLokasiStoneSetting
        };
    }

    [WebMethod]
    public static object RedeemVoucher(string identifier, string voucher_code, string store)
    {
        try
        {

            string queryMappipng = "exec SP_STAMPS_GET_MAPPING_LOCATION '" + store + "'";
            int storeStampsId = Db.SingleInteger(queryMappipng, Db.Cnn_IN);

            var queryObject = new
            {
                token = TOKEN,
                identifier = identifier,
                voucher_code = voucher_code,
                store = storeStampsId
                // store = 1
            };

            string jsonData = JsonConvert.SerializeObject(queryObject);

            // string queryString = "?" + query.ToString();
            string apiUrl = BASE_URL + "vouchers/redeem"; // + queryString;
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string responseBody = client.UploadString(apiUrl, "POST", jsonData);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic data = serializer.Deserialize<dynamic>(responseBody);


                using (SqlConnection connection = new SqlConnection(Db.Cnn_IN))
                {
                    string query = "INSERT INTO TemporaryVoucherRedemption (RD_ID, VOUCHER_NAME, VOUCHER_CODE) VALUES (@RD_ID, @VOUCHER_NAME, @VOUCHER_CODE)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        command.Parameters.Add(new SqlParameter("@RD_ID", SqlDbType.Int) { Value = data["redemption"]["id"] });
                        command.Parameters.Add(new SqlParameter("@VOUCHER_NAME", SqlDbType.VarChar) { Value = data["voucher"]["name"] });
                        command.Parameters.Add(new SqlParameter("@VOUCHER_CODE", SqlDbType.VarChar) { Value = voucher_code });
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);
                    }
                }

                return data;

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
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    dynamic errorMessage = serializer.Deserialize<dynamic>(text);
                    return errorMessage;
                }
            }
        }
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IPaymentType> GetPaymentType()
    {
        string query = "SELECT A.* FROM V_PaymentRepairType A WHERE ID = 5";
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
    public static void UpdateStatusPayment(int IDDocRepair, List<IVoucherList> DataVoucher)
    {
        string INSERTDataVoucher = @"INSERT INTO VoucherDiscountRepair(IDDocRepair, KodeVoucher, NamaVoucher, JenisVoucher, NominalDiscount, Created_at, Created_by)
                                    VALUES(@IDDocRepair, @KodeVoucher, @NamaVoucher, @JenisVoucher, @NominalDiscount, GETDATE(), @Created_by)
                                    ";
        string kodeVoucher = "";
        string NamaVoucher = "";
        string JenisVoucher = "";
        decimal NominalDiscount = 0;

        foreach (var voucher in DataVoucher)
        { 
            kodeVoucher = voucher.KodeVoucher;
            NamaVoucher = voucher.Voucher;
            JenisVoucher = voucher.Nominal;
            NominalDiscount = voucher.NominalDiscount;

        }
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(INSERTDataVoucher, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.VarChar) { Value = IDDocRepair });
                command.Parameters.Add(new SqlParameter("@KodeVoucher", SqlDbType.VarChar) { Value = kodeVoucher });
                command.Parameters.Add(new SqlParameter("@NamaVoucher", SqlDbType.VarChar) { Value = NamaVoucher });
                command.Parameters.Add(new SqlParameter("@JenisVoucher", SqlDbType.VarChar) { Value = JenisVoucher });
                command.Parameters.Add(new SqlParameter("@NominalDiscount", SqlDbType.Decimal) { Value = NominalDiscount });
                command.Parameters.Add(new SqlParameter("@Created_by", SqlDbType.VarChar) { Value = App.UserNama });
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        string query = "UPDATE DocRepair_Pivot SET Status = 8 WHERE IDDocRepair = " + IDDocRepair;
        Db.Execute(query, Db.Cnn_DB);

        string query2 = "EXEC sp_insertLedgerRepair " + IDDocRepair + ", '" + App.UserNama + "'";
        Db.Execute(query2, Db.Cnn_DB);
    }


    public static void ExecuteInsertRepair(IInsertFormRepair insertFormRepair)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            int ID = 0;
            using (SqlCommand command = new SqlCommand("sp_editFormDocQC", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocQC", SqlDbType.Int) { Value = insertFormRepair.insertDocQCParams.IDDocQC });
                command.Parameters.Add(new SqlParameter("@Sertifikat", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.Sertifikat });
                command.Parameters.Add(new SqlParameter("@Logo", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.Logo });
                command.Parameters.Add(new SqlParameter("@KadarEmas", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.KadarEmas });
                command.Parameters.Add(new SqlParameter("@Merk", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.Merk });
                command.Parameters.Add(new SqlParameter("@NetWeight", SqlDbType.Decimal) { Value = insertFormRepair.insertDocQCParams.NetWeight });
                command.Parameters.Add(new SqlParameter("@WarnaEmas", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.WarnaEmas });
                command.Parameters.Add(new SqlParameter("@FisikKeseluruhan", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.FisikKeseluruhan });
                command.Parameters.Add(new SqlParameter("@OperatorNama", SqlDbType.VarChar) { Value = App.UserNama });
                command.Parameters.Add(new SqlParameter("@Keterangan", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.Keterangan });
                command.Parameters.Add(new SqlParameter("@GrossWeight", SqlDbType.Decimal) { Value = insertFormRepair.insertDocQCParams.GrossWeight });
                command.Parameters.Add(new SqlParameter("@TipeProduct", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.TipeProduct });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];

            }

            using (SqlCommand command = new SqlCommand("sp_insertFormRepair_Result", connection))
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(insertFormRepair.insertDocRepairParams.DocRepair_Package);
                DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.IDDocRepair });
                command.Parameters.Add(new SqlParameter("@PLU", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.PLU });
                command.Parameters.Add(new SqlParameter("@SalesNama", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.SalesNama });
                command.Parameters.Add(new SqlParameter("@CustomerNama", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.CustomerNama });
                command.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar) { Value = App.UserNama });
                command.Parameters.Add(new SqlParameter("@Keterangan", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.Keterangan });
                command.Parameters.Add(new SqlParameter("@ImgPicture", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.ImgPicture });
                command.Parameters.Add(new SqlParameter("@IDProduct", SqlDbType.Int) { Value = insertFormRepair.insertDocRepairParams.IDProduct });
                command.Parameters.Add(new SqlParameter("@IDProductTitipan", SqlDbType.Int) { Value = insertFormRepair.insertDocRepairParams.IDProductTitipan });
                command.Parameters.Add(new SqlParameter("@TipeProduct", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.TipeProduct });
                command.Parameters.Add(new SqlParameter("@DocRepair_Package", SqlDbType.Structured) { Value = pDt });
                command.Parameters.Add(new SqlParameter("@Pengrajin", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.Pengrajin });
                command.Parameters.Add(new SqlParameter("@IsFinish", SqlDbType.Int) { Value = 1 });
                command.Parameters.Add(new SqlParameter("@Weight", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.Weight });
                command.Parameters.Add(new SqlParameter("@Size", SqlDbType.Int) { Value = insertFormRepair.insertDocRepairParams.Size });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetVoucher(string voucher_code, string store)
    {

        string queryMappipng = "exec SP_STAMPS_GET_MAPPING_LOCATION '" + store + "'";
        int storeStampsId = Db.SingleInteger(queryMappipng, Db.Cnn_IN);

        try
        {
            var queryObject = new
            {
                token = TOKEN,
                voucher_code = voucher_code,
                store = storeStampsId
                // store = store
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

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic data = serializer.Deserialize<dynamic>(responseBody);
                return data;
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

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    dynamic errorMessage = serializer.Deserialize<dynamic>(text);
                    return errorMessage;
                }
            }
        }
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

    public class IInsertFormRepair
    {
        public IInsertDocQCParams insertDocQCParams { get; set; }
        public IInsertDocRepairParams insertDocRepairParams { get; set; }
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
        public List<IDocRepairOther> docRepairOther;
        public List<IDocRepairCustomer> docRepairCustomer;
        public List<ISLokasiStoneSetting> docRepairLokasiStoneSetting;
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
        public decimal BeratTimbang { get; set; }
        public decimal BeratTimbangActual { get; set; }
        public decimal BeratTimbangAkhir { get; set; }
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
        public decimal HargaBayar { get; set; }
        public decimal TotalGramLebih { get; set; }
        public int Status { get; set; }
        public decimal BeratAkhirTanpaBatu { get; set; }
        public string KodeStore { get; set; }
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
    }

    public class IDocRepairOther
    {
        public string NomorRepair { get; set; }
        public string NamaPackage { get; set; }
        public decimal CostOther { get; set; }
    }
    public class IDocRepairCustomer
    {
        public int IDCustomer { get; set; }
        public string NamaCustomer { get; set; }
        public string NomorCustomer { get; set; }
        public string NomorTelpon { get; set; }
        public string Email { get; set; }
        public int IDCustomerStamps { get; set; }
        public string KodeLokasi { get; set; }
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
    
    public class IPaymentType
    {
        public string Icon { get; set; }
        public string Nama { get; set; }
        public int ID { get; set; }
    }
    public class IVoucherList
    {
        public string KodeVoucher { get; set; }
        public string Voucher { get; set; }
        public string Nominal { get; set; }
        public int no { get; set; }
        public decimal NominalDiscount { get; set; }
    }
       public class ISLokasiStoneSetting
    {
        public int IDProduct { get; set; }        
    }
}