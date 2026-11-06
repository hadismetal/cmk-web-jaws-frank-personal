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
    public static object GetStone(string keyword, int IDDocRepair)
    {
        string queryGetStone = @"
                DECLARE @NomorRPR varchar(150), @IDLokasi int, @TipeLokasi int, @IsKlampis int  = 0
                SELECT @IsKlampis = IIF(IDLocReparasi=4,1,0) FROM DocRepair_Pivot WHERE IDDocRepair=@IDDocRepair
                
                select @NomorRPR = LTRIM(RTRIM(a.Nomor)), @TipeLokasi = TipeLokasi, @IDLokasi = IDLokasi from DocRepair a where a.ID=@IDDocRepair

                select * INTO #tempStoneSetting
                from (
                    select '1A' AS Tipe, IDForm, IDStone, TotalButir, TotalCarat, CaratButir, '' AS GIA  from StockProductDJ_Stone1A		
                    UNION
                    select '1B' AS Tipe, IDForm, IDStone, TotalButir, TotalCarat, CaratButir, GIA  from StockProductDJ_Stone1B
                    UNION
                    select '2' AS Tipe, IDForm, IDStone, TotalButir, TotalCarat, CaratButir, '' AS GIA  from StockProductDJ_Stone2
                    UNION
                    select '3' AS Tipe, IDForm, IDStone, TotalButir, TotalCarat, CaratButir, '' AS GIA  from StockProductDJ_Stone3
                    UNION
                    select '4' AS Tipe, IDForm, IDStone, TotalButir, TotalCarat, CaratButir, '' AS GIA  from StockProductDJ_Stone4
                    UNION
                    select '5' AS Tipe, IDForm, IDStone, TotalButir, TotalCarat, CaratButir, '' AS GIA  from StockProductDJ_Stone5
                )xa

                SELECT * INTO #tempStoneParcel
                FROM (
                    select '1A' AS Tipe, ID, Nama as Parcel FROM Stone1A
                    UNION
                    select '1B' AS Tipe, ID, Nama as Parcel FROM Stone1B
                    UNION
                    select '2' AS Tipe,  ID, Nama as Parcel FROM Stone2
                    UNION
                    select '3' AS Tipe,  ID, Nama as Parcel FROM Stone3
                    UNION		
                    select '4' AS Tipe,  ID, Nama as Parcel FROM Stone4
                    UNION			
                    select '5' AS Tipe,  ID, Nama as Parcel FROM Stone5
                )xaa

                select @IDDocRepair AS IDDocRepair, b.ID,a.Nomor,a.IDLokasi,a.TipeLokasi,a.IDBrand,a.Substorage
                ,b.SupplierNomor, d.Nama as ProductItem, bb.Parcel, ba.TotalButir,ba.TotalCarat,ba.CaratButir,GIA, ba.Tipe as TipeStone, bb.ID as IDStone
                from StockActualDJ a 
                JOIN StockProductDJ b on b.ID=a.IDProduct
                JOIN StockProductDJ_CharProduct c on c.ID=b.ID
                JOIN CharProductItem d on d.ID=c.ProductItem                
                JOIN #tempStoneSetting ba on ba.IDForm=b.ID	
                JOIN #tempStoneParcel bb on bb.ID=ba.IDStone AND ba.Tipe=bb.Tipe
                where (a.nomor = @keyword) and a.IDProduct NOT IN (select IDProduct from DocRepair_PLUSetting)
                and ((@IsKlampis = 1 and a.TipeLokasi = 0 and a.NamaLokasi like '%KLAMPIS%') or (LTRIM(RTRIM(b.SupplierNomor)) = @NomorRPR))
                
                --and a.IDLokasi = @IDLokasi and a.TipeLokasi = @TipeLokasi     

                DROP TABLE #tempStoneSetting, #tempStoneParcel;           
                ";

        List<IDocRepairParcel> docReportParcel = new List<IDocRepairParcel>();
        DataTable dt = new DataTable();
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(queryGetStone, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@keyword", SqlDbType.VarChar) { Value = keyword });
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.Int) { Value = IDDocRepair });


                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    docReportParcel.Add(new IDocRepairParcel
                    {
                        IDDocRepair = (int)dr["IDDocRepair"],
                        IDProduct = (int)dr["ID"],
                        IDLokasi = (int)dr["IDLokasi"],
                        TipeLokasi = (int)dr["TipeLokasi"],
                        PLU = dr["Nomor"].ToString(),
                        IDBrand = (int)dr["IDBrand"],
                        Substorage = (int)dr["Substorage"],
                        SupplierNomor = dr["SupplierNomor"].ToString(),
                        ProductItem = dr["ProductItem"].ToString(),
                        Parcel = dr["Parcel"].ToString(),
                        TipeStone = dr["TipeStone"].ToString(),
                        TotalButir = (int)dr["TotalButir"],
                        TotalCarat = Convert.ToDecimal(dr["TotalCarat"]),
                        CaratButir = Convert.ToDecimal(dr["CaratButir"]),
                        GIA = dr["GIA"].ToString(),
                        IDStone = (int)dr["IDStone"]
                    });
                }
            }

        }
        return docReportParcel;

    }
   
    [WebMethod]
    public static int insertStoneSetting(List<IDataStone> DataStone)
    {         
        DataSet ds = ExecuteInsertPLUSetting(DataStone);
        DataTable dt = ds.Tables[0];

        int ID = 0;
        foreach (DataRow dr in dt.Rows)
        {
            ID = (int)dr["IDDocRepair"];
        }
        return ID;
    }

    public static DataSet ExecuteInsertPLUSetting(List<IDataStone> DataStone)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            int ID = 0;            

            using (SqlCommand command = new SqlCommand("sp_insertPLUSetting_Repair", connection))
            {


                string json3 = Newtonsoft.Json.JsonConvert.SerializeObject(DataStone);
                DataTable pDt3 = JsonConvert.DeserializeObject<DataTable>(json3);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@DataStone", SqlDbType.Structured) { Value = pDt3 });
                command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = App.UserID });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds;
            }
        }
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
        
        string queryGetDetail = @"select a.ID AS IDDocRepair, a.Nomor as NomorRepair,  c.PLU, B.Nama AS Store
                                , IIF(c.IDLocReparasi = 1,'CS PUSAT',IIF(c.IDLocReparasi = 2, 'Service Point','Service Point Klampis')) LocRepair
                                from DocRepair a
                                JOIN LocOutlet b on b.ID=a.IDLokasi
                                JOIN DocRepair_Pivot c on c.IDDocRepair=a.ID
                                where a.id=@IDDocRepair and c.Status in (2,3)
        ";

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(queryGetDetail, connection))            
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.Int) { Value = IDDocRepair });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                docRepairDetail = Helper.GetDataFromDataTable<IDocRepairDetail>(dt, true)[0];
            }
        
        }

        return new IDocRepair
        {
            docRepairDetail = docRepairDetail
        };
    }

    public class IDocRepair
    {
        public IDocRepairDetail docRepairDetail;
    }

    public class IDocRepairDetail
    {
        public int IDDocRepair { get; set; }
        public string NomorRepair { get; set; }        
        public string PLU { get; set; }        
        public string Store { get; set; }       
        public string LocRepair { get; set; }
    }
    public class IDocRepairParcel
    {
        public int IDDocRepair { get; set; }
        public int IDProduct { get; set; }
        public int IDLokasi { get; set; }
        public int TipeLokasi { get; set; }
        public string PLU { get; set; }
        public int IDBrand { get; set; }
        public int Substorage { get; set; }
        public string SupplierNomor { get; set; }
        public string ProductItem { get; set; }
        public string Parcel { get; set; }
        public string TipeStone { get; set; }
        public int TotalButir { get; set; }
        public decimal TotalCarat { get; set; }
        public decimal CaratButir { get; set; }
        public string GIA { get; set; }  
        public int IDStone { get; set; }           
    }
    public class IInsertPLUSetting
    {        
        public IDataStone DataStone { get; set; }
    }

    public class IDataStone
    {
        public int IDDocRepair { get; set; }
        public int IDProduct { get; set; }
        public int IDLokasi { get; set; }
        public int TipeLokasi { get; set; }
        public string PLU { get; set; }
        public string TipeStone { get; set; }
        public int IDBrand { get; set; }
        public int Substorage { get; set; }
        public string SupplierNomor { get; set; }
        public string ProductItem { get; set; }
        public string Parcel { get; set; }
        public int TotalButir { get; set; }
        public decimal TotalCarat { get; set; }
        public decimal CaratButir { get; set; }
        public string GIA { get; set; }
        public int IDStone { get; set; }
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