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
using System.Web;
using System.IO;


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


    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
        }
    }

    [WebMethod]
    public static string UploadImage(string imageData, string fileName)
    {
        try
        {
            // Define the physical path where you want to save the image
            string filePath = RepairLib.PATH_IMAGES + fileName;

            // Convert the Base64 string back to bytes
            byte[] imageBytes = Convert.FromBase64String(imageData);

            // Save the image to the specified path
            File.WriteAllBytes(filePath, imageBytes);
            return filePath;
        }
        catch (Exception ex)
        {
            return "failed";
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetCheckPLUQC(string Keyword = "")
    {

        DataSet dsx = ExecuteCheckPLUAvailability(Keyword);
        DataTable dtx = dsx.Tables[0];

        string message = "";

        foreach (DataRow dr in dtx.Rows)
        {
            message = dr["Message"].ToString();
        }

        if (message != "")
        {
            return message;
        }

        DataSet ds = ExecuteQCDetail(Keyword);


        DataTable dtMsg = ds.Tables[2];
        foreach (DataRow dr in dtMsg.Rows)
        {
            message = dr["Message"].ToString();
        }

        if (!String.IsNullOrEmpty(message))
        {
            return message;
        }

        // Assuming the stored procedure returns two tables
        DataTable dt1 = ds.Tables[0];


        IHeaderPLUQC headerPlu = new IHeaderPLUQC();
        foreach (DataRow dr in dt1.Rows)
        {
            IHeaderPLUQC header = new IHeaderPLUQC
            {
                NOMOR = dr["NOMOR"].ToString(),
                TYPE = dr["TYPE"].ToString(),
                NOMORINVOICE = dr["NOMORINVOICE"].ToString(),
                NAMACUSTOMER = dr["NAMACUSTOMER"].ToString(),
                IDCUSTOMER = dr["IDCUSTOMER"].ToString(),
                FRAME_COLOR = dr["FRAME_COLOR"].ToString(),
                FRAME_COLOR_ID = (int)dr["FRAME_COLOR_ID"],
                GROSS_WEIGHT = dr["GROSS_WEIGHT"].ToString(),
                ID = (int)dr["ID"],
                IMAGE = dr["IMAGE"].ToString(),
                ITEM = dr["ITEM"].ToString(),
                ITEMID = (int)dr["ITEMID"],
                SIZE = Convert.ToDecimal(dr["SIZE"])
            };


            headerPlu = header;
        }


        DataTable dt2 = ds.Tables[1];
        List<IDetailPLUQC> detailPlu = new List<IDetailPLUQC>();
        foreach (DataRow dr in dt2.Rows)
        {
            IDetailPLUQC detail = new IDetailPLUQC
            {
                BRANDSTONE = dr["BRANDSTONE"].ToString(),
                GIA = dr["GIA"].ToString(),
                STONE = dr["STONE"].ToString(),
                STONETYPE = dr["STONETYPE"].ToString(),
                STONETYPEMAP = dr["STONETYPEMAP"].ToString(),
                ID = (int)dr["ID"],
                TGL = dr["TGL"].ToString(),
                TOTAL_BUTIR = (int)dr["TOTAL_BUTIR"],
                TOTAL_CARAT = dr["TOTAL_CARAT"].ToString(),
                CLARITY = dr["CLARITY"].ToString(),
                COLOR = dr["COLOR"].ToString(),
            };

            detailPlu.Add(detail);
        }

        DataSet ds2 = ExecuteGetRepairHistory(headerPlu.ID, headerPlu.TYPE);
        DataTable dt3 = ds2.Tables[0];
        DataTable dt4 = ds2.Tables[1];

        List<IDetailHistoryRepair> detailHistoryRepair = new List<IDetailHistoryRepair>();
        foreach (DataRow dr in dt3.Rows)
        {
            detailHistoryRepair.Add(new IDetailHistoryRepair
            {
                IDDocRepair = (int)dr["IDDocRepair"],
                NomorRepair = (string)dr["NomorRepair"],
                SizeAwal = Convert.ToDecimal(dr["SizeAwal"]),
                SizeAkhir = Convert.ToDecimal(dr["SizeAkhir"]),
                BeratAwal = Convert.ToDecimal(dr["BeratAwal"]),
                BeratAkhir = Convert.ToDecimal(dr["BeratAkhir"]),
                Keterangan = (string)dr["Keterangan"]
            });
        }

        List<IPackageRepairHistory> packageRepair = new List<IPackageRepairHistory>();
        foreach (DataRow dr in dt4.Rows)
        {
            packageRepair.Add(new IPackageRepairHistory
            {
                IDDocRepair = (int)dr["IDDocRepair"],
                JenisReparasi = (string)dr["TypeRepair"]
            });
        }

        return new IItemPLUQC
        {
            Item = headerPlu,
            Stone = detailPlu,
            History = new IHistoryRepair
            {
                detailHistoryRepair = detailHistoryRepair,
                packageRepair = packageRepair
            }
        };
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static ISalesLocationResponse GetUserOutletAndDefaultSales()
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getDefaultLocationAndSalesOrAE", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = App.UserID });
                command.Parameters.Add(new SqlParameter("@IsPusat", SqlDbType.Int) { Value = 1 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];
                List<IDataLocation> dataLocation = Helper.GetDataFromDataTable<IDataLocation>(dt);
                List<IDataSalesWithLocation> dataSales = Helper.GetDataFromDataTable<IDataSalesWithLocation>(dt2);

                return new ISalesLocationResponse
                {
                    sales = dataSales,
                    location = dataLocation
                };
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IMapDataLocation> GetLocationTujuan(string kodeStore)
    {
        DataTable dt = Db.Rs("exec sp_mappingMasterServicePoint " + kodeStore, Db.Cnn_DB);

        List<IMapDataLocation> locations = new List<IMapDataLocation>();
        foreach (DataRow dr in dt.Rows)
        {
            IMapDataLocation location = new IMapDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            location.BrandOutlet = dr["Brand"].ToString();
            location.IDBrand = (int)dr["IDBrand"];
            location.Province = dr["Province"].ToString();
            locations.Add(location);
        }

        return locations;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static ILocAndPackageRepair GetDetailReparasi()
    {
        DataTable dt = Db.Rs("SELECT * FROM V_MasterPackageReparasi", Db.Cnn_DB);
        DataTable dt2 = Db.Rs("SELECT * FROM LocReparasi WHERE ID = 1", Db.Cnn_DB);

        List<ILocRepair> locRepair = new List<ILocRepair>();
        foreach (DataRow dr in dt2.Rows)
        {
            ILocRepair loc = new ILocRepair
            {
                ID = (int)dr["ID"],
                Kode = dr["Kode"].ToString(),
                Nama = dr["Nama"].ToString()
            };
            locRepair.Add(loc);
        }

        List<IPackageRepair> packageRepair = new List<IPackageRepair>();
        foreach (DataRow dr in dt.Rows)
        {
            IPackageRepair pkg = new IPackageRepair
            {
                IDLocReparasi = (int)dr["IDLocReparasi"],
                IDLocPackageReparasi = (int)dr["IDLocPackageReparasi"],
                IDPackage = (int)dr["IDPackage"],
                JenisReparasi = dr["JenisReparasi"].ToString(),
                Kode = dr["Kode"].ToString(),
                Nama = dr["Nama"].ToString(),
                NamaTipeReparasi = dr["NamaTipeReparasi"].ToString(),
                Ongkos = Convert.ToDecimal(dr["Ongkos"]),
                TipeReparasi = (int)dr["TipeReparasi"]
            };
            packageRepair.Add(pkg);
        }

        return new ILocAndPackageRepair
        {
            LocRepair = locRepair,
            PackageRepair = packageRepair
        };
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IDataCustomer> GetDataCustomer(string keyword, int searchby)
    {
        DataSet ds = ExecuteGetCustomer(keyword, searchby);
        DataTable dt = ds.Tables[0];

        List<IDataCustomer> dataCustomer = new List<IDataCustomer>();
        foreach (DataRow dr in dt.Rows)
        {
            IDataCustomer customer = new IDataCustomer
            {
                CustomerBirthday = dr["ST_CUST_BIRTHDAY"].ToString(),
                CustomerCode = dr["ST_CUST_MID_PRIMARY"].ToString(),
                CustomerMembership = dr["ST_CUST_MEMBERSHIP_STATUS"].ToString(),
                CustomerName = dr["ST_CUST_NAME"].ToString(),
                CustomerPhone = dr["ST_CUST_PHONE"].ToString(),
                CustomerStampsID = (int)dr["ST_CUST_ID"],
                CustomerEmail = dr["ST_CUST_EMAIL"].ToString(),
                ID = (int)dr["ID"]
            };
            dataCustomer.Add(customer);
        }

        return dataCustomer;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IDataCustomerItem> GetDataCustomerItem(int IDCustomer, string KodeCustLama)
    {
        DataSet ds = ExecuteGetCustomerItem(IDCustomer, KodeCustLama);
        DataTable dt = ds.Tables[0];

        List<IDataCustomerItem> dataCustomerItem = new List<IDataCustomerItem>();
        foreach (DataRow dr in dt.Rows)
        {
            IDataCustomerItem customer = new IDataCustomerItem
            {
                HargaBeli = Convert.ToDecimal(dr["HARGA_BELI"]),
                IDProduct = (int)dr["ID_PRODUCT"],
                IDProductTitipan = (int)dr["ID_PRODUCT_TITIPAN"],
                Image = dr["IMAGE"].ToString(),
                Item = dr["ITEM"].ToString(),
                Nomor = dr["NOMOR"].ToString(),
                StatusStock = (int)dr["STATUSSTOCK"]
            };
            dataCustomerItem.Add(customer);
        }

        return dataCustomerItem;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static IHistoryRepair GetRepairHistory(int IDProduct, string TipeProduct)
    {
        DataSet ds = ExecuteGetRepairHistory(IDProduct, TipeProduct);
        DataTable dt = ds.Tables[0];
        DataTable dt2 = ds.Tables[1];

        List<IDetailHistoryRepair> detailHistoryRepair = new List<IDetailHistoryRepair>();
        foreach (DataRow dr in dt.Rows)
        {
            detailHistoryRepair.Add(new IDetailHistoryRepair
            {
                IDDocRepair = (int)dr["IDDocRepair"],
                NomorRepair = (string)dr["NomorRepair"],
                SizeAwal = Convert.ToDecimal(dr["SizeAwal"]),
                SizeAkhir = Convert.ToDecimal(dr["SizeAkhir"]),
                BeratAwal = Convert.ToDecimal(dr["BeratAwal"]),
                BeratAkhir = Convert.ToDecimal(dr["BeratAkhir"]),
                Keterangan = (string)dr["Keterangan"]
            });
        }

        List<IPackageRepairHistory> packageRepair = new List<IPackageRepairHistory>();
        foreach (DataRow dr in dt2.Rows)
        {
            packageRepair.Add(new IPackageRepairHistory
            {
                IDDocRepair = (int)dr["IDDocRepair"],
                JenisReparasi = (string)dr["JenisReparasi"]
            });
        }

        return new IHistoryRepair
        {
            detailHistoryRepair = detailHistoryRepair,
            packageRepair = packageRepair
        };
    }

    [WebMethod]
    public static int CreateDocRepair(IInsertFormRepair args)
    {
        DataSet ds = ExecuteInsertRepair(args);
        DataTable dt = ds.Tables[0];

        int ID = 0;
        foreach (DataRow dr in dt.Rows)
        {
            ID = (int)dr["ID"];
        }
        return ID;
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

    public static DataSet ExecuteQCDetail(string keyword)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getDetailItemQc", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Keyword", SqlDbType.VarChar) { Value = keyword });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }
    public static DataSet ExecuteCheckPLUAvailability(string keyword)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_checkRepairAvailability", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Nomor", SqlDbType.VarChar) { Value = keyword });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }

    public static DataSet ExecuteGetCustomer(string keyword, int searchby)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_get_data_customer", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@Keyword", SqlDbType.VarChar) { Value = keyword });
                command.Parameters.Add(new SqlParameter("@SearchBy", SqlDbType.Int) { Value = searchby });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }

    public static DataSet ExecuteGetCustomerItem(int IDCustomer, string KodeCustLama)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_get_repair_item_by_customer_new", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID_CUSTOMER", SqlDbType.Int) { Value = IDCustomer });
                command.Parameters.Add(new SqlParameter("@KODE_CUSTOMER_LAMA", SqlDbType.VarChar) { Value = KodeCustLama });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }

    public static DataSet ExecuteGetRepairHistory(int IDProduct, string TipeProduct)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getHistoryRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDProduct", SqlDbType.Int) { Value = IDProduct });
                command.Parameters.Add(new SqlParameter("@TipeProduct", SqlDbType.VarChar) { Value = TipeProduct });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }


    public static DataSet ExecuteInsertRepair(IInsertFormRepair insertFormRepair)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            int ID = 0;
            using (SqlCommand command = new SqlCommand("sp_insertFormDocQC", connection))
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(insertFormRepair.insertDocQCParams.QC_Additional);
                DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);


                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@QCNama", SqlDbType.VarChar) { Value = App.UserNama });
                command.Parameters.Add(new SqlParameter("@Nomor", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.Nomor });
                command.Parameters.Add(new SqlParameter("@KodeCustLama", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.KodeCustLama });
                command.Parameters.Add(new SqlParameter("@IDProduct", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.IDProduct });
                command.Parameters.Add(new SqlParameter("@Blemish", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.Blemish });
                command.Parameters.Add(new SqlParameter("@JumlahButir", SqlDbType.Int) { Value = insertFormRepair.insertDocQCParams.JumlahButir });
                command.Parameters.Add(new SqlParameter("@Sertifikat", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.Sertifikat });
                command.Parameters.Add(new SqlParameter("@NonCZ", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.NonCZ });
                command.Parameters.Add(new SqlParameter("@CZ", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.CZ });
                command.Parameters.Add(new SqlParameter("@Logo", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.Logo });
                command.Parameters.Add(new SqlParameter("@KadarEmas", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.KadarEmas });
                command.Parameters.Add(new SqlParameter("@Merk", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.Merk });
                command.Parameters.Add(new SqlParameter("@NetWeight", SqlDbType.Decimal) { Value = insertFormRepair.insertDocQCParams.NetWeight });
                command.Parameters.Add(new SqlParameter("@WarnaEmas", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.WarnaEmas });
                command.Parameters.Add(new SqlParameter("@FisikKeseluruhan", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.FisikKeseluruhan });
                command.Parameters.Add(new SqlParameter("@OperatorNama", SqlDbType.VarChar) { Value = App.UserNama });
                command.Parameters.Add(new SqlParameter("@Draft", SqlDbType.Int) { Value = insertFormRepair.insertDocQCParams.Draft });
                command.Parameters.Add(new SqlParameter("@Keterangan", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.Keterangan });
                command.Parameters.Add(new SqlParameter("@GrossWeight", SqlDbType.Decimal) { Value = insertFormRepair.insertDocQCParams.GrossWeight });
                command.Parameters.Add(new SqlParameter("@QC_Additional", SqlDbType.Structured) { Value = pDt });
                command.Parameters.Add(new SqlParameter("@TipeProduct", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.TipeProduct });
                command.Parameters.Add(new SqlParameter("@KodeLokasi", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.KodeLokasi });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    ID = (int)dr["ID"];
                }
            }

            using (SqlCommand command = new SqlCommand("sp_insertFormRepairPusat", connection))
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(insertFormRepair.insertDocRepairParams.DocRepair_Package);
                string json2 = Newtonsoft.Json.JsonConvert.SerializeObject(insertFormRepair.insertDocRepairParams.QC_Additional);
                DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);
                DataTable pDt2 = JsonConvert.DeserializeObject<DataTable>(json2);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@PLU", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.PLU });
                command.Parameters.Add(new SqlParameter("@Nomor", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.Nomor });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = insertFormRepair.insertDocRepairParams.TipeLokasi });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = insertFormRepair.insertDocRepairParams.IDLokasi });
                command.Parameters.Add(new SqlParameter("@KodeLokasi", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.KodeLokasi });
                command.Parameters.Add(new SqlParameter("@IDCustomer", SqlDbType.Int) { Value = insertFormRepair.insertDocRepairParams.IDCustomer });
                command.Parameters.Add(new SqlParameter("@IDSales", SqlDbType.Int) { Value = insertFormRepair.insertDocRepairParams.IDSales });
                command.Parameters.Add(new SqlParameter("@SalesNama", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.SalesNama });
                command.Parameters.Add(new SqlParameter("@CustomerNama", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.CustomerNama });
                command.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar) { Value = App.UserNama });
                command.Parameters.Add(new SqlParameter("@Keterangan", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.Keterangan });
                command.Parameters.Add(new SqlParameter("@ImgPicture", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.ImgPicture });
                command.Parameters.Add(new SqlParameter("@IDProduct", SqlDbType.Int) { Value = insertFormRepair.insertDocRepairParams.IDProduct });
                command.Parameters.Add(new SqlParameter("@IDProductTitipan", SqlDbType.Int) { Value = insertFormRepair.insertDocRepairParams.IDProductTitipan });
                command.Parameters.Add(new SqlParameter("@EstimasiHarga", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.EstimasiHarga });
                command.Parameters.Add(new SqlParameter("@IDQC", SqlDbType.Int) { Value = ID });
                command.Parameters.Add(new SqlParameter("@IDQCTitipan", SqlDbType.Int) { Value = 0 });
                command.Parameters.Add(new SqlParameter("@TipeProduct", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.TipeProduct });
                command.Parameters.Add(new SqlParameter("@IDLocReparasi", SqlDbType.Int) { Value = insertFormRepair.insertDocRepairParams.IDLocReparasi });
                command.Parameters.Add(new SqlParameter("@DocRepair_Package", SqlDbType.Structured) { Value = pDt });
                command.Parameters.Add(new SqlParameter("@QC_Additional", SqlDbType.Structured) { Value = pDt2 });
                command.Parameters.Add(new SqlParameter("@SizeAwal", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.SizeAwal });
                command.Parameters.Add(new SqlParameter("@SizeAkhir", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.SizeAkhir });
                command.Parameters.Add(new SqlParameter("@Weight", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.Weight });
                command.Parameters.Add(new SqlParameter("@WeightData", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.WeightData });
                command.Parameters.Add(new SqlParameter("@WeightActual", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.WeightActual });
                command.Parameters.Add(new SqlParameter("@FrameColor", SqlDbType.Int) { Value = insertFormRepair.insertDocRepairParams.FrameColor });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds;
            }
        }
    }


    public class IQCAdditional
    {
        public int IDStone { get; set; }
        public string Keterangan { get; set; }
        public int SesuaiSertifikat { get; set; }
        public string StoneType { get; set; }
        public string TesKeaslian { get; set; }
        public string TesKualitas { get; set; }
        public int TotalButirActual { get; set; }
        public decimal TotalCaratActual { get; set; }
    }
    public class IInsertDocQCParams
    {
        public string QCNama { get; set; }
        public string KodeCustLama { get; set; }
        public string IDProduct { get; set; }
        public string Nomor { get; set; }
        public string Blemish { get; set; }
        public int JumlahButir { get; set; }
        public string Sertifikat { get; set; }
        public string NonCZ { get; set; }
        public string CZ { get; set; }
        public string Logo { get; set; }
        public string KadarEmas { get; set; }
        public string Merk { get; set; }
        public decimal NetWeight { get; set; }
        public string WarnaEmas { get; set; }
        public string FisikKeseluruhan { get; set; }
        public string OperatorNama { get; set; }
        public int Draft { get; set; }
        public string Keterangan { get; set; }
        public decimal GrossWeight { get; set; }
        public List<object> QC_Additional { get; set; }
        public string TipeProduct { get; set; }
        public string KodeLokasi { get; set; }
    }

    public class IInsertDocRepairParams
    {
        public string PLU { get; set; }
        public string Nomor { get; set; }
        public int TipeLokasi { get; set; }
        public int IDLokasi { get; set; }
        public string KodeLokasi { get; set; }
        public int IDSales { get; set; }
        public string SalesNama { get; set; }
        public string CustomerNama { get; set; }
        public int IDCustomer { get; set; }
        public string Operator { get; set; }
        public string Keterangan { get; set; }
        public string ImgPicture { get; set; }
        public int IDProduct { get; set; }
        public int IDProductTitipan { get; set; }
        public decimal EstimasiHarga { get; set; }
        public int IDQC { get; set; }
        public int IDQCTitipan { get; set; }
        public string TipeProduct { get; set; }
        public int IDLocReparasi { get; set; }
        public int FrameColor { get; set; }
        public decimal SizeAwal { get; set; }
        public decimal SizeAkhir { get; set; }
        public decimal Weight { get; set; }
        public decimal WeightData { get; set; }
        public decimal WeightActual { get; set; }
        public List<object> DocRepair_Package { get; set; }
        public List<object> QC_Additional { get; set; }
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
        public string TYPE { get; set; }
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
        public string CLARITY { get; set; }
        public string COLOR { get; set; }
    }
    public class IItemPLUQC
    {
        public IHeaderPLUQC Item { get; set; }
        public List<IDetailPLUQC> Stone { get; set; }
        public IHistoryRepair History { get; set; }
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
        public string BrandOutlet { get; set; }
        public int IDBrand { get; set; }
    }
    public class IDataLocation
    {
        public string NamaOutlet { get; set; }
        public string KodeOutlet { get; set; }
        public int IDLokasi { get; set; }
        public int TipeLokasi { get; set; }
        public string BrandOutlet { get; set; }
        public int IDBrand { get; set; }
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

    public class IDetailHistoryRepair
    {
        public int IDDocRepair { get; set; }
        public string NomorRepair { get; set; }
        public decimal SizeAwal { get; set; }
        public decimal SizeAkhir { get; set; }
        public decimal BeratAwal { get; set; }
        public decimal BeratAkhir { get; set; }
        public string Keterangan { get; set; }
    }

    public class IPackageRepairHistory
    {
        public int IDDocRepair { get; set; }
        public string JenisReparasi { get; set; }
    }

    public class IHistoryRepair
    {
        public List<IDetailHistoryRepair> detailHistoryRepair { get; set; }
        public List<IPackageRepairHistory> packageRepair { get; set; }
    }

    public class IMapDataLocation
    {
        public string NamaOutlet { get; set; }
        public string KodeOutlet { get; set; }
        public int IDLokasi { get; set; }
        public int TipeLokasi { get; set; }
        public string BrandOutlet { get; set; }
        public int IDBrand { get; set; }
        public string Province { get; set; }
    }
}