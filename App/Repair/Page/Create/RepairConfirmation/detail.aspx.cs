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
    public static object GetDetailRepair(int IDDocRepair, int IDBrandAsal)
    {
        IDocRepair docRepair = ExecuteDetailDocRepair(IDDocRepair, IDBrandAsal);
        return docRepair;
    }

    [WebMethod]    
    public static int submitChanges(IDataConfirmation DataConfirmation)
    {  
        int ID = 0;
        int rowsInserted = 0;
        string queryTesting = @"
        
            DECLARE @json NVARCHAR(MAX) = @jsonParam, @NomorRPR varchar(100), @IDBrand int, @IDAsal int, @Status varchar(100)
            CREATE TABLE #tempPricingLog (
                IDDocRepair INT,
                TipeEstimasi varchar(100),
                HargaMinIDR MONEY,
                HargaMaxIDR MONEY
            );            

            INSERT INTO #tempPricingLog
            SELECT *
            FROM OPENJSON(@json)            
            WITH (
                IDDocRepair INT,
                TipeEstimasi varchar(100),
                HargaMinIDR MONEY,
                HargaMaxIDR MONEY
            )

            SELECT @NomorRPR = Nomor, @IDAsal = IDLokasi FROM DocRepair WHERE ID = (SELECT TOP 1 IDDocRepair FROM #tempPricingLog);

            IF @TipeKonfirmasi = 'Approve'
            BEGIN
                INSERT INTO DocRepair_LogPricingEstimation(IDDocRepair, TipeEstimasi, HargaMinIDR, HargaMaxIDR, Created_at)
                SELECT IDDocRepair, TipeEstimasi, HargaMinIDR, HargaMaxIDR, GETDATE() FROM #tempPricingLog
            END            

            IF @TipeKonfirmasi = 'Approve'
            BEGIN
                 SET @Status = 'Disetujui'
            END               
            ELSE 
            BEGIN
                SET @Status = 'Ditolak'
            END
            exec [DB_PROD_MANUFAKTUR].dbmfg.dbo.sp_update_status_konfirmasi_toko_reparasi @NomorRPR, @Status; 

            DROP TABLE #tempPricingLog
        ";

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(DataConfirmation.estimasi);

        string connectionString = Helper.GetConnectionByBrand(7);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {

            connection.Open();

            using (SqlCommand cmd = new SqlCommand(queryTesting, connection))
            {
                cmd.Parameters.AddWithValue("@jsonParam", json);
                cmd.Parameters.AddWithValue("@TipeKonfirmasi", DataConfirmation.TipeKonfirmasi);


                rowsInserted = cmd.ExecuteNonQuery();

            }

            connection.Close();

        }


        // DataSet ds = ExecuteConfirmationRepair(DataConfirmation);
        // DataTable dt = ds.Tables[0];

        // int ID = 0;
        // foreach (DataRow dr in dt.Rows)
        // {
        //     ID = (int)dr["IDDocRepair"];
        // }
         
        return rowsInserted;
    }
    public static DataSet ExecuteConfirmationRepair(List<IDataConfirmation> DataConfirmation)
    {        
        string connectionString = Helper.GetConnectionByBrand(DataConfirmation[0].IDBrandAsal);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string data = @"select * from tablea";
            using (SqlCommand command = new SqlCommand(data, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@DataConfirmation", SqlDbType.NVarChar) { Value = JsonConvert.SerializeObject(DataConfirmation) });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds;
            }
        }        
    }

    public static IDocRepair ExecuteDetailDocRepair(int IDDocRepair, int IDBrandAsal)
    {
        IDocRepairDetail docRepairDetail = new IDocRepairDetail();
        List<IDocRepairDetailStone> docRepairDetailStone = new List<IDocRepairDetailStone>();
        List<IDocRepairDetailPackage> docRepairDetailPackage = new List<IDocRepairDetailPackage>();
        List<IDocRepairDetailEstimasi> docRepairDetailEstimasi = new List<IDocRepairDetailEstimasi>();


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


            string querygetEstmasi = @"DECLARE @NomorRPR varchar(100), @IDAsal int                                                        
                                    select @NomorRPR = Nomor, @IDAsal = IDLokasi FROM DocRepair WHERE ID = @IDDocRepair;
                                    exec sp_CheckEstimatedPrice_Repair @NomorRPR, @IDBrand, @IDAsal, @Type";
            using (SqlCommand command = new SqlCommand(querygetEstmasi, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.Int) { Value = IDDocRepair });
                command.Parameters.Add(new SqlParameter("@IDBrand", SqlDbType.Int) { Value = IDBrandAsal });
                command.Parameters.Add(new SqlParameter("@Type", SqlDbType.Int) { Value = 1 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                DataTable dt1 = ds.Tables[1];
                DataTable dt2 = ds.Tables[2];


                docRepairDetailStone = Helper.GetDataFromDataTable<IDocRepairDetailStone>(dt);
                docRepairDetailPackage = Helper.GetDataFromDataTable<IDocRepairDetailPackage>(dt1);
                docRepairDetailEstimasi = Helper.GetDataFromDataTable<IDocRepairDetailEstimasi>(dt2);
            }

        }

        return new IDocRepair
        {
            docRepairDetail = docRepairDetail,
            docRepairDetailStone = docRepairDetailStone,
            docRepairDetailPackage = docRepairDetailPackage,
            docRepairDetailEstimasi = docRepairDetailEstimasi
        };
    }

    public class IDataConfirmation
    { 
        public int IDDocRepair { get; set; }
        public int IDBrandAsal { get; set; }
        public string TipeKonfirmasi { get; set; }
        public List<IDocRepairMainPackage> PaketUtama { get; set; }
        public List<IDocRepairAdditionalPackage> satuanTambahan { get; set; }        
        public List<IDocRepairDetailEstimasi> estimasi { get; set; }
    }

    public class IDocRepairMainPackage
    {
        public int IDDocRepair { get; set; }
        public int TipeReparasi { get; set; }
        public string JenisReparasi { get; set; }
        
    }
    public class IDocRepairAdditionalPackage
    {
        public int IDDocRepair { get; set; }
        public int TipeReparasi { get; set; }
        public string JenisReparasi { get; set; }
        
    }

    public class IDocRepair
    {
        public IDocRepairDetail docRepairDetail;
        public List<IDocRepairDetailStone> docRepairDetailStone;
        public List<IDocRepairDetailPackage> docRepairDetailPackage;
        public List<IDocRepairDetailEstimasi> docRepairDetailEstimasi;
    }

    public class IDocRepairDetail
    {
        public int IDDocRepair { get; set; }
        public string NomorRepair { get; set; }
        public string NamaCustomer { get; set; }
        public string NamaSales { get; set; }
        public string PLU { get; set; }
        public string TipeProduct { get; set; }
        public string Blemish { get; set; }
        public string ImgPicture { get; set; }
        public string GrafirLogo { get; set; }
        public string TesSertifikat { get; set; }
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
        public decimal BeratTimbangPotong { get; set; }
        public decimal BeratTimbangActual { get; set; }
        public decimal BeratTimbang { get; set; }
        public decimal BeratTimbangAkhir { get; set; }
        public decimal SizeBRJ { get; set; }
        public decimal SizeAwal { get; set; }
        public decimal SizeAkhir { get; set; }
        public string ImgProduct { get; set; }
        public string Store { get; set; }
        public string Invoice { get; set; }
        public string FisikKeseluruhan { get; set; }
        public string Keterangan { get; set; }
        public string KeteranganRepair { get; set; }
        public string LocRepair { get; set; }
    }

    public class IDocRepairDetailStone
    {
        public int IDStone { get; set; }
        public string Tipe { get; set; }
        public string KodeParcel { get; set; }
        public string Parcel { get; set; }
        public decimal TotalButir { get; set; }
        public decimal TotalCarat { get; set; }        
    }
    public class IDocRepairDetailPackage
    {
        public int IDDocRepair { get; set; }
        public int TipeReparasi { get; set; }
        public string JenisReparasi { get; set; }
        
    }
    public class IDocRepairDetailEstimasi
    {
        public int No { get; set; }
        public int IDDocRepair { get; set; }
        public string TipeEstimasi { get; set; }
        public decimal HargaMinIDR { get; set; }
        public decimal HargaMaxIDR { get; set; }
        
    }
    public class IDocRepairParcel    
    {
        public int IDDocRepair { get; set; }
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
    public class IDocRepairOther
    {        
        public string NomorRepair { get; set; }
        public string NamaPackage { get; set; }        
        public decimal CostOther { get; set; }        
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