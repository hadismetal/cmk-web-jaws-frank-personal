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
    Map_DB db = new Map_DB();


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
        IDocRepairParcel docRepairParcel = new IDocRepairParcel();
        IDocRepairPackage docRepairPackage = new IDocRepairPackage();

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
                        BeratTimbang = Convert.ToDecimal(dt["BeratTimbang"]),
                        BeratTimbangAwal = Convert.ToDecimal(dt["BeratTimbangAwal"]),
                        FrameColor = dt["FrameColor"].ToString(),
                        FrameMaterial = dt["FrameMaterial"].ToString(),
                        GrafirKadarEmas = dt["GrafirKadarEmas"].ToString(),
                        GrafirLogo = dt["GrafirLogo"].ToString(),
                        IDDocRepair = (int)dt["IDDocRepair"],
                        ImgPicture = dt["ImgPicture"].ToString(),
                        Item = dt["Item"].ToString(),
                        KodeFrameMaterial = dt["KodeFrameMaterial"].ToString(),
                        KodeKaret = dt["KodeKaret"].ToString(),
                        NamaCustomer = dt["NamaCustomer"].ToString(),
                        NamaSales = dt["NamaSales"].ToString(),
                        NomorRepair = dt["NomorRepair"].ToString(),
                        PLU = dt["PLU"].ToString(),
                        SizeAkhir = (int)dt["SizeAkhir"],
                        SizeAwal = (int)dt["SizeAwal"],
                        TesKeaslian = dt["TesKeaslian"].ToString(),
                        TesSertifikat = dt["TesSertifikat"].ToString(),
                        TipeProduct = dt["TipeProduct"].ToString(),
                        TotalButir = (int)dt["TotalButir"],
                        TotalCarat = Convert.ToDecimal(dt["TotalCarat"])
                    }
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
                    docRepairDetail = new IDocRepairParcel {
                        CaratButir = Convert.ToDecimal(dt["CaratButir"]),
                        IDDocRepair = (int)dt["IDDocRepair"],
                        NomorRepair = dt["NomorRepair"].ToString(),
                        Parcel = dt["Parcel"].ToString(),
                        StoneType = dt["StoneType"].ToString(),
                        StoneTypeMap = dt["StoneTypeMap"].ToString(),
                        TotalButir = (int)dt["TotalButir"],
                        TotalButirActual = (int)dt["TotalButirActual"],
                        TotalCarat = Convert.ToDecimal(dt["TotalCarat"]),
                        TotalCaratActual = Convert.ToDecimal(dt["TotalCaratActual"])
                    }
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
                    docRepairDetail = new IDocRepairPackage {
                        IDPackage = (int)dt["IDPackage"],
                        IDDocRepair = (int)dt["IDDocRepair"],
                        NamaPackage = dt["NamaPackage"].ToString(),
                        NomorRepair = dt["NomorRepair"].ToString(),
                        TipePackage = dt["TipePackage"].ToString(),
                        NamaTipePackage = dt["NamaTipePackage"].ToString()
                    }
                }
            }
        }

        return new IDocRepair {
            docRepairDetail1 = docRepairDetail,
            docRepairPackage = docRepairPackage,
            docRepairParcel = docRepairParcel
        }
    }

    public class IDocRepair {
        public IDocRepairDetail docRepairDetail1;
        public IDocRepairPackage docRepairPackage;
        public IDocRepairParcel docRepairParcel;
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