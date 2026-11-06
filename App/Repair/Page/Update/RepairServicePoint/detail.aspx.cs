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
                foreach(DataRow dr in dt.Rows) {
                    docRepairDetail = new IDocRepairDetail {
                        BeratTimbang = Convert.ToDecimal(dr["BeratTimbang"]),
                        BeratTimbangAwal = Convert.ToDecimal(dr["BeratTimbangAwal"]),
                        FrameColor = dr["FrameColor"].ToString(),
                        FrameMaterial = dr["FrameMaterial"].ToString(),
                        GrafirKadarEmas = dr["GrafirKadarEmas"].ToString(),
                        GrafirLogo = dr["GrafirLogo"].ToString(),
                        IDDocRepair = (int)dr["IDDocRepair"],
                        ImgPicture = dr["ImgPicture"].ToString(),
                        Item = dr["Item"].ToString(),
                        KodeFrameMaterial = dr["KodeFrameMaterial"].ToString(),
                        KodeKaret = dr["KodeKaret"].ToString(),
                        NamaCustomer = dr["NamaCustomer"].ToString(),
                        NamaSales = dr["NamaSales"].ToString(),
                        NomorRepair = dr["NomorRepair"].ToString(),
                        PLU = dr["PLU"].ToString(),
                        SizeAkhir = (int)dr["SizeAkhir"],
                        SizeAwal = (int)dr["SizeAwal"],
                        TesKeaslian = dr["TesKeaslian"].ToString(),
                        TesSertifikat = dr["TesSertifikat"].ToString(),
                        TipeProduct = dr["TipeProduct"].ToString(),
                        TotalButir = (int)dr["TotalButir"],
                        TotalCarat = Convert.ToDecimal(dr["TotalCarat"]),
                        ImgProduct = dr["ImgProduct"].ToString(),
                        Store = dr["Store"].ToString(),
                        Invoice = dr["Invoice"].ToString(),
                        FisikKeseluruhan = dr["FisikKeseluruhan"].ToString(),
                        Keterangan = dr["Keterangan"].ToString(),
                        LocRepair = dr["LocRepair"].ToString()
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
                foreach(DataRow dr in dt.Rows) {
                    docRepairParcel.Add(new IDocRepairParcel {
                        CaratButir = Convert.ToDecimal(dr["CaratButir"]),
                        IDDocRepair = (int)dr["IDDocRepair"],
                        NomorRepair = dr["NomorRepair"].ToString(),
                        Parcel = dr["Parcel"].ToString(),
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
                foreach(DataRow dr in dt.Rows) {
                    docRepairPackage.Add(new IDocRepairPackage {
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

        return new IDocRepair {
            docRepairDetail = docRepairDetail,
            docRepairPackage = docRepairPackage,
            docRepairParcel = docRepairParcel
        };
    }

    public class IDocRepair {
        public IDocRepairDetail docRepairDetail;
        public List<IDocRepairPackage> docRepairPackage;
        public List<IDocRepairParcel> docRepairParcel;
    }

    public class IDocRepairDetail {
        public int IDDocRepair {get;set;}
        public string NomorRepair {get;set;}
        public string NamaCustomer {get;set;}
        public string NamaSales {get;set;}
        public string PLU {get;set;}
        public string TipeProduct {get;set;}
        public string ImgPicture {get;set;}
        public string GrafirLogo {get;set;}
        public string TesSertifikat {get;set;}
        public string GrafirKadarEmas {get;set;}
        public string TesKeaslian {get;set;}
        public string KodeKaret {get;set;}
        public string Item {get;set;}
        public string FrameColor {get;set;}
        public string FrameMaterial {get;set;}
        public string KodeFrameMaterial {get;set;}
        public int TotalButir {get;set;}
        public decimal TotalCarat {get;set;}
        public decimal BeratTimbangAwal {get;set;}
        public decimal BeratTimbang {get;set;}
        public int SizeAwal {get;set;}
        public int SizeAkhir {get;set;}
        public string ImgProduct {get;set;}
        public string Store {get;set;}
        public string Invoice {get;set;}
        public string FisikKeseluruhan {get;set;}
        public string Keterangan {get;set;}
        public string LocRepair {get;set;}
    }

       public class IDocRepairParcel {
        public int IDDocRepair {get;set;}
        public string NomorRepair {get;set;}
        public string Parcel {get;set;}
        public decimal CaratButir {get;set;}
        public decimal TotalCarat {get;set;}
        public decimal TotalCaratActual {get;set;}
        public int TotalButir {get;set;}
        public int TotalButirActual {get;set;}
        public string StoneType {get;set;}
        public string StoneTypeMap {get;set;}   
        public string SesuaiSertifikat {get;set;}
        public string TesKeaslian {get;set;}
        public string TesKualitas {get;set;}
        public string Keterangan {get;set;}
    }

    public class IDocRepairPackage {
        public int IDDocRepair {get;set;}
        public string NomorRepair {get;set;}
        public string NamaPackage {get;set;}
        public int IDPackage {get;set;}
        public int TipePackage {get;set;}
        public string NamaTipePackage {get;set;}

    }
}