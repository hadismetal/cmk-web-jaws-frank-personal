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
    public static int CreateDocRepair(IInsertFormRepair args, int IDBrandAsal)
    {
        ExecuteInsertRepair(args, IDBrandAsal);
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
    public static object GetDetailRepair(int IDDocRepair, int IDBrandAsal)
    {
        IDocRepair docRepair = ExecuteDetailDocRepair(IDDocRepair, IDBrandAsal);
        return docRepair;
    }

    public static IDocRepair ExecuteDetailDocRepair(int IDDocRepair, int IDBrandAsal)
    {
        IDocRepairDetail docRepairDetail = new IDocRepairDetail();
        List<IDocRepairParcel> docRepairParcel = new List<IDocRepairParcel>();
        List<IDocRepairPackage> docRepairPackage = new List<IDocRepairPackage>();
        List<IPengrajin> pengrajin = new List<IPengrajin>();
        string connectionString = Helper.GetConnectionByBrand(IDBrandAsal);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand("sp_getDetailRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.VarChar) { Value = IDDocRepair });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                docRepairDetail = Helper.GetDataFromDataTable<IDocRepairDetail>(dt, true)[0];
            }

            using (SqlCommand command = new SqlCommand("sp_getDetailParcelRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.VarChar) { Value = IDDocRepair });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                docRepairParcel = Helper.GetDataFromDataTable<IDocRepairParcel>(dt);
            }

            using (SqlCommand command = new SqlCommand("sp_getDetailPackageRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.VarChar) { Value = IDDocRepair });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                docRepairPackage = Helper.GetDataFromDataTable<IDocRepairPackage>(dt);
            }

        }


        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string query = "SELECT KodeLokasi FROM DocRepair_ServicePoint WHERE IDDocRepair = " + IDDocRepair;
            string kodeStore = Db.SingleString(query, connectionString);

            using (SqlCommand command = new SqlCommand("sp_getPengrajinRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@KODE", SqlDbType.VarChar) { Value = kodeStore });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                pengrajin = Helper.GetDataFromDataTable<IPengrajin>(dt);
            }
        }

        return new IDocRepair
        {
            docRepairDetail = docRepairDetail,
            docRepairPackage = docRepairPackage,
            docRepairParcel = docRepairParcel,
            pengrajin = pengrajin
        };
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetLocationPengrajin(string Kode)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            int ID = 0;
            using (SqlCommand command = new SqlCommand("sp_getPengrajinRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@KODE", SqlDbType.VarChar) { Value = Kode });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                List<IPengrajin> pengrajin = Helper.GetDataFromDataTable<IPengrajin>(dt);
                return pengrajin;
            }
        }
    }

    public static void ExecuteInsertRepair(IInsertFormRepair insertFormRepair, int IDBrandAsal)
    {
        string connectionString = Helper.GetConnectionByBrand(IDBrandAsal);
        using (SqlConnection connection = new SqlConnection(connectionString))
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
                command.Parameters.Add(new SqlParameter("@WeightActual", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.WeightActual });
                command.Parameters.Add(new SqlParameter("@WeightData", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.WeightData });
                command.Parameters.Add(new SqlParameter("@WeightPotong", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.WeightPotong });
                command.Parameters.Add(new SqlParameter("@WeightEnd", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.WeightEnd });
                command.Parameters.Add(new SqlParameter("@SizeAwal", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.SizeAwal });
                command.Parameters.Add(new SqlParameter("@SizeAkhir", SqlDbType.Decimal) { Value = insertFormRepair.insertDocRepairParams.SizeAkhir });

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
        public decimal SizeAwal { get; set; }
        public decimal SizeAkhir { get; set; }
        public decimal Weight { get; set; }
        public decimal WeightActual { get; set; }
        public decimal WeightData { get; set; }
        public decimal WeightPotong { get; set; }
        public decimal WeightEnd { get; set; }
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
        public string NamaItem { get; set; }
        public string KodeItem { get; set; }
        public string KlasifikasiItem { get; set; }
        public string JenisReparasi { get; set; }
        public decimal Ongkos { get; set; }
        public int TipeReparasi { get; set; }
        public string NamaTipeReparasi { get; set; }
        public string Nama { get; set; }
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
        public List<IPengrajin> pengrajin;
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
        public string GrafirLogo { get; set; }
        public string TesSertifikat { get; set; }
        public string GrafirKadarEmas { get; set; }
        public string TesKeaslian { get; set; }
        public string KodeKaret { get; set; }
        public string Item { get; set; }
        public string KodeItem { get; set; }
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
        public string ImgProduct { get; set; }
        public string Store { get; set; }
        public string Invoice { get; set; }
        public string FisikKeseluruhan { get; set; }
        public string Keterangan { get; set; }
        public string KeteranganRepair { get; set; }
        public string LocRepair { get; set; }
        public int Status { get; set; }
        public string KodeStore { get; set; }
    }

    public class IDocRepairParcel
    {
        public int IDDocQC { get; set; }
        public int IDDocRepair { get; set; }
        public int IDDocStone { get; set; }
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

    public class IPengrajin
    {
        public int ID { get; set; }
        public string NamaTukang { get; set; }
        public string NIK { get; set; }
        public string Tgl { get; set; }
        public string Keterangan { get; set; }
    }
}