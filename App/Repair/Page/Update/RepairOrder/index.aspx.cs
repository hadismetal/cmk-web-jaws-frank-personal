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
            return ex.Message.ToString();
        }
    }

    public static List<IDataSalesWithLocation> CheckSales(int IDUser = 0, string KodeLokasi = "")
    {
        string query = "SELECT" +
        " B.ID [IDSales], B.Nama [NamaSales], B.Keterangan [NIK], C.Nama [NamaOutlet], C.NamaKode [KodeOutlet], CASE WHEN D.ID IS NULL THEN E.ID ELSE D.ID END [IDLokasi], CASE WHEN D.ID IS NULL THEN 2 ELSE 1 END [TipeLokasi] " +
        " FROM frank_ac.dbo.UserAccount A" +
        " JOIN DataSales B ON A.UserNumber = B.Keterangan" +
        " JOIN DataSalesGroup C ON B.IDGroup = C.ID" +
        " LEFT JOIN LocOutlet D ON C.NamaKode = D.Kode" +
        " LEFT JOIN LocExhibition E ON C.NamaKode = E.Kode" +
        " WHERE B.Disable = 0 AND C.Disable = 0 AND ((D.ID IS NOT NULL AND D.Disable = 0) OR (E.ID IS NOT NULL AND E.Disable = 0))";

        if (IDUser != 0)
        {
            query += " AND A.ID = " + IDUser;
        }

        if (KodeLokasi != "")
        {
            query += " AND C.NamaKode = '" + KodeLokasi + "'";
        }

        string queryAdd = "SELECT * FROM ( " + query + " ) A ORDER BY A.TipeLokasi, A.IDLokasi";

        DataTable dt = Db.Rs(queryAdd, Db.Cnn_DB);
        List<IDataSalesWithLocation> dataSales = new List<IDataSalesWithLocation>();
        foreach (DataRow dr in dt.Rows)
        {
            IDataSalesWithLocation sales = new IDataSalesWithLocation();
            sales.IDSales = (int)dr["IDSales"];
            sales.NamaSales = dr["NamaSales"].ToString();
            sales.NIK = dr["NIK"].ToString();
            sales.NamaOutlet = dr["NamaOutlet"].ToString();
            sales.IDLokasi = (int)dr["IDLokasi"];
            sales.TipeLokasi = (int)dr["TipeLokasi"];
            sales.NamaOutlet = dr["NamaOutlet"].ToString();
            sales.KodeOutlet = dr["KodeOutlet"].ToString();
            dataSales.Add(sales);
        }

        return dataSales;
    }

    public static List<IDataSalesWithLocation> CheckAdminSales(int IDUser = 0)
    {
        string query = "SELECT " +
        " B.ID [IDSales], B.Nama [NamaSales], B.Keterangan [NIK], CASE WHEN B.TipeLokasi = 1 THEN C.Nama ELSE D.Nama END [NamaOutlet], CASE WHEN B.TipeLokasi = 1 THEN C.Kode ELSE D.Kode END [KodeOutlet], B.IDLokasi [IDLokasi], B.TipeLokasi [TipeLokasi]" +
        " FROM frank_ac.dbo.UserAccount A" +
        " JOIN DataAdminSales B ON A.UserNumber = B.Keterangan" +
        " LEFT JOIN LocOutlet C ON B.TipeLokasi = 1 AND C.ID = B.IDLokasi" +
        " LEFT JOIN LocExhibition D ON B.TipeLokasi = 2 AND C.ID = B.IDLokasi" +
        " WHERE B.Disable = 0 AND ((B.TipeLokasi = 1 AND C.Disable = 0) OR (B.TipeLokasi = 2 AND D.Disable = 0))";


        if (IDUser != 0)
        {
            query += " AND A.ID = " + IDUser;
        }

        string queryAdd = query + " ORDER BY B.TipeLokasi, B.IDLokasi";

        DataTable dt = Db.Rs(queryAdd, Db.Cnn_DB);

        List<IDataSalesWithLocation> dataSales = new List<IDataSalesWithLocation>();
        foreach (DataRow dr in dt.Rows)
        {
            IDataSalesWithLocation sales = new IDataSalesWithLocation();
            sales.IDSales = (int)dr["IDSales"];
            sales.NamaSales = dr["NamaSales"].ToString();
            sales.NIK = dr["NIK"].ToString();
            sales.NamaOutlet = dr["NamaOutlet"].ToString();
            sales.IDLokasi = (int)dr["IDLokasi"];
            sales.TipeLokasi = (int)dr["TipeLokasi"];
            sales.NamaOutlet = dr["NamaOutlet"].ToString();
            sales.KodeOutlet = dr["KodeOutlet"].ToString();
            dataSales.Add(sales);
        }

        return dataSales;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static ISalesLocationResponse GetUserOutletAndDefaultSales()
    {
        List<IDataSalesWithLocation> dataSalesLocation = CheckSales(App.UserID);
        List<IDataSalesWithLocation> dataAdminSalesLocation = CheckAdminSales(App.UserID);


        List<IDataSalesWithLocation> dataSales = new List<IDataSalesWithLocation>();
        List<IDataLocation> dataLocation = new List<IDataLocation>();

        if (dataSalesLocation.Count() == 0 && dataAdminSalesLocation.Count() == 0)
        {
            List<IDataSalesWithLocation> dataSalesLocationAll = CheckSales();

            foreach (IDataSalesWithLocation salesLocation in dataSalesLocationAll)
            {
                dataSales.Add(salesLocation);
            }

            List<IDataLocation> locations = Location();

            foreach (IDataLocation location in locations)
            {
                dataLocation.Add(location);
            }

            return new ISalesLocationResponse
            {
                sales = dataSales,
                location = dataLocation
            };
        }

        if (dataSalesLocation.Count() == 0 && dataAdminSalesLocation.Count() > 0)
        {
            foreach (IDataSalesWithLocation adminSalesLocation in dataAdminSalesLocation)
            {
                IDataLocation location = new IDataLocation
                {
                    NamaOutlet = adminSalesLocation.NamaOutlet,
                    KodeOutlet = adminSalesLocation.KodeOutlet,
                    IDLokasi = adminSalesLocation.IDLokasi,
                    TipeLokasi = adminSalesLocation.TipeLokasi
                };

                dataLocation.Add(location);
            }

            foreach (IDataLocation location in dataLocation)
            {
                List<IDataSalesWithLocation> dataSalesLocationAll = CheckSales(0, location.KodeOutlet);

                foreach (IDataSalesWithLocation dataSalesWithLocation in dataSalesLocationAll)
                {
                    dataSales.Add(dataSalesWithLocation);
                }
            }



            return new ISalesLocationResponse
            {
                sales = dataSales,
                location = dataLocation
            };
        }

        foreach (IDataSalesWithLocation salesLocation in dataSalesLocation)
        {
            IDataLocation location = new IDataLocation
            {
                NamaOutlet = salesLocation.NamaOutlet,
                KodeOutlet = salesLocation.KodeOutlet,
                IDLokasi = salesLocation.IDLokasi,
                TipeLokasi = salesLocation.TipeLokasi
            };

            dataSales.Add(salesLocation);
            dataLocation.Add(location);
        }

        return new ISalesLocationResponse
        {
            sales = dataSales,
            location = dataLocation
        };
    }

    public static List<IDataLocation> Location()
    {
        DataTable dt = Db.Rs("SELECT ID, Nama, Kode, 1 [TipeLokasi] FROM LocOutlet WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB);
        DataTable dt2 = Db.Rs("SELECT ID, Nama, Kode, 2 [TipeLokasi] FROM LocExhibition WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB);

        List<IDataLocation> locations = new List<IDataLocation>();
        foreach (DataRow dr in dt.Rows)
        {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            locations.Add(location);
        }

        foreach (DataRow dr in dt2.Rows)
        {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            locations.Add(location);
        }

        return locations;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static ILocAndPackageRepair GetDetailReparasi()
    {
        DataTable dt = Db.Rs("SELECT * FROM V_MasterPackageReparasi", Db.Cnn_DB);
        DataTable dt2 = Db.Rs("SELECT * FROM LocReparasi", Db.Cnn_DB);

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

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getDetailRepair", connection))
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
                        IDDocQC = (int)dr["IDDocQC"],
                        BeratTimbang = Convert.ToDecimal(dr["BeratTimbang"]),
                        BeratTimbangAwal = Convert.ToDecimal(dr["BeratTimbangAwal"]),
                        BeratTimbangPotong = Convert.ToDecimal(dr["BeratTimbangPotong"]),
                        BeratTimbangActual = Convert.ToDecimal(dr["BeratTimbangActual"]),
                        FrameColor = dr["FrameColor"].ToString(),
                        FrameMaterial = dr["FrameMaterial"].ToString(),
                        GrafirKadarEmas = dr["GrafirKadarEmas"].ToString(),
                        GrafirLogo = dr["GrafirLogo"].ToString(),
                        IDDocRepair = (int)dr["IDDocRepair"],
                        ImgPicture = RepairLib.GetPathImage(dr["ImgPicture"].ToString()),
                        ImgPictureRaw = dr["ImgPicture"].ToString(),
                        Item = dr["Item"].ToString(),
                        KodeFrameMaterial = dr["KodeFrameMaterial"].ToString(),
                        KodeKaret = dr["KodeKaret"].ToString(),
                        NamaCustomer = dr["NamaCustomer"].ToString(),
                        NamaSales = dr["NamaSales"].ToString(),
                        NomorRepair = dr["NomorRepair"].ToString(),
                        PLU = dr["PLU"].ToString(),
                        SizeBRJ = Convert.ToDecimal(dr["SizeBRJ"]),
                        SizeAkhir = Convert.ToDecimal(dr["SizeAkhir"]),
                        SizeAwal = Convert.ToDecimal(dr["SizeAwal"]),
                        TesKeaslian = dr["TesKeaslian"].ToString(),
                        TesSertifikat = dr["TesSertifikat"].ToString(),
                        Blemish = dr["Blemish"].ToString(),
                        TipeProduct = dr["TipeProduct"].ToString(),
                        TotalButir = (int)dr["TotalButir"],
                        TotalCarat = Convert.ToDecimal(dr["TotalCarat"]),
                        ImgProduct = dr["ImgProduct"].ToString(),
                        Store = dr["Store"].ToString(),
                        Invoice = dr["Invoice"].ToString(),
                        FisikKeseluruhan = dr["FisikKeseluruhan"].ToString(),
                        Keterangan = dr["Keterangan"].ToString(),
                        KeteranganRepair = dr["KeteranganRepair"].ToString(),
                        LocRepair = dr["LocRepair"].ToString(),
                        Status = (int)dr["Status"],
                        IDLocRepair = (int)dr["IDLocRepair"],
                    };
                }
            }

            using (SqlCommand command = new SqlCommand("sp_getDetailParcelRepair", connection))
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
                        IDDocQC = (int)dr["IDDocQC"],
                        IDDocStone = (int)dr["IDDocStone"],
                        IDProductStone = (int)dr["IDProductStone"],
                        IDDocRepair = (int)dr["IDDocRepair"],
                        NomorRepair = dr["NomorRepair"].ToString(),
                        Parcel = dr["Parcel"].ToString(),
                        CaratButir = Convert.ToDecimal(dr["CaratButir"]),
                        StoneType = dr["StoneType"].ToString(),
                        StoneTypeMap = dr["StoneTypeMap"].ToString(),
                        TotalButir = (int)dr["TotalButir"],
                        TotalButirActual = (int)dr["TotalButirActual"],
                        TotalCarat = Convert.ToDecimal(dr["TotalCarat"]),
                        TotalCaratActual = Convert.ToDecimal(dr["TotalCaratActual"]),
                        SesuaiSertifikat = dr["SesuaiSertifikat"].ToString(),
                        TesKeaslian = dr["TesKeaslian"].ToString(),
                        TesKualitas = dr["TesKualitas"].ToString(),
                        Keterangan = dr["Keterangan"].ToString(),
                        Stone = dr["Stone"].ToString(),
                        Clarity = dr["Clarity"].ToString(),
                        Color = dr["Color"].ToString(),
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
                        NamaTipePackage = dr["NamaTipePackage"].ToString()
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


    public static void ExecuteInsertRepair(IInsertFormRepair insertFormRepair)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            int ID = 0;
            using (SqlCommand command = new SqlCommand("sp_editFormDocQC_Stone", connection))
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(insertFormRepair.insertDocQCParams.QC_Additional);
                DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocQC", SqlDbType.Int) { Value = insertFormRepair.insertDocQCParams.IDDocQC });
                command.Parameters.Add(new SqlParameter("@Sertifikat", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.Sertifikat });
                command.Parameters.Add(new SqlParameter("@Blemish", SqlDbType.VarChar) { Value = insertFormRepair.insertDocQCParams.Blemish });
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
                command.Parameters.Add(new SqlParameter("@QC_Additional", SqlDbType.Structured) { Value = pDt });


                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

            }

            using (SqlCommand command = new SqlCommand("sp_editFormRepair", connection))
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(insertFormRepair.insertDocRepairParams.DocRepair_Package);
                DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                string json2 = Newtonsoft.Json.JsonConvert.SerializeObject(insertFormRepair.insertDocRepairParams.QC_Additional);
                DataTable pDt2 = JsonConvert.DeserializeObject<DataTable>(json2);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.IDDocRepair });
                command.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar) { Value = App.UserNama });
                command.Parameters.Add(new SqlParameter("@Keterangan", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.Keterangan });
                command.Parameters.Add(new SqlParameter("@ImgPicture", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.ImgPicture });
                command.Parameters.Add(new SqlParameter("@TipeProduct", SqlDbType.VarChar) { Value = insertFormRepair.insertDocRepairParams.TipeProduct });
                command.Parameters.Add(new SqlParameter("@DocRepair_Package", SqlDbType.Structured) { Value = pDt });
                command.Parameters.Add(new SqlParameter("@Weight", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.Weight });
                command.Parameters.Add(new SqlParameter("@WeightActual", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.WeightActual });
                command.Parameters.Add(new SqlParameter("@WeightData", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.WeightData });
                command.Parameters.Add(new SqlParameter("@SizeAwal", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.SizeAwal });
                command.Parameters.Add(new SqlParameter("@SizeAkhir", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.SizeAkhir });
                command.Parameters.Add(new SqlParameter("@QC_Additional", SqlDbType.Structured) { Value = pDt2 });
                command.Parameters.Add(new SqlParameter("@FrameColor", SqlDbType.Int) { Value = insertFormRepair.insertDocRepairParams.FrameColor });


                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

            }
        }
    }


    public class IInsertDocQCParams
    {
        public int IDDocQC { get; set; }
        public string Sertifikat { get; set; }
        public string Logo { get; set; }
        public string KadarEmas { get; set; }
        public string Blemish { get; set; }
        public string Merk { get; set; }
        public decimal NetWeight { get; set; }
        public string WarnaEmas { get; set; }
        public string FisikKeseluruhan { get; set; }
        public string OperatorNama { get; set; }
        public string Keterangan { get; set; }
        public decimal GrossWeight { get; set; }
        public string TipeProduct { get; set; }
        public List<object> QC_Additional { get; set; }
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
        public decimal SizeAwal { get; set; }
        public decimal SizeAkhir { get; set; }
        public decimal Weight { get; set; }
        public decimal WeightActual { get; set; }
        public decimal WeightData { get; set; }
        public List<object> QC_Additional { get; set; }
        public int FrameColor { get; set; }

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
    }

    public class IDocRepairDetail
    {
        public int IDDocRepair { get; set; }
        public int IDDocQC { get; set; }
        public string NomorRepair { get; set; }
        public string NamaCustomer { get; set; }
        public string NamaSales { get; set; }
        public string PLU { get; set; }
        public string TipeProduct { get; set; }
        public string ImgPicture { get; set; }
        public string ImgPictureRaw { get; set; }
        public string GrafirLogo { get; set; }
        public string TesSertifikat { get; set; }
        public string Blemish { get; set; }
        public string GrafirKadarEmas { get; set; }
        public string TesKeaslian { get; set; }
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
        public decimal BeratTimbangPotong { get; set; }
        public decimal SizeAwal { get; set; }
        public decimal SizeAkhir { get; set; }
        public decimal SizeBRJ { get; set; }
        public string ImgProduct { get; set; }
        public string Store { get; set; }
        public string Invoice { get; set; }
        public string FisikKeseluruhan { get; set; }
        public string Keterangan { get; set; }
        public string KeteranganRepair { get; set; }
        public string LocRepair { get; set; }
        public int IDLocRepair { get; set; }
        public int Status { get; set; }
    }

    public class IDocRepairParcel
    {
        public int IDDocQC { get; set; }
        public int IDDocRepair { get; set; }
        public int IDDocStone { get; set; }
        public int IDProductStone { get; set; }
        public string NomorRepair { get; set; }
        public string Parcel { get; set; }
        public decimal CaratButir { get; set; }
        public decimal TotalCarat { get; set; }
        public decimal TotalCaratActual { get; set; }
        public int TotalButir { get; set; }
        public int TotalButirActual { get; set; }
        public string StoneType { get; set; }
        public string StoneTypeMap { get; set; }
        public string SesuaiSertifikat { get; set; }
        public string TesKeaslian { get; set; }
        public string TesKualitas { get; set; }
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

    }
}