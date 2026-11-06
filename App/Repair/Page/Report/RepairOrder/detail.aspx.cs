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

    public static IDocRepair ExecuteDetailDocRepair(int IDDocRepair, int IDBrandAsal)
    {
        IDocRepairDetail docRepairDetail = new IDocRepairDetail();
        List<IDocRepairParcel> docRepairParcel = new List<IDocRepairParcel>();
        List<IDocRepairPackage> docRepairPackage = new List<IDocRepairPackage>();
        List<IDocRepairOther> docRepairOther = new List<IDocRepairOther>();


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
                        NamaTipePackage = dr["NamaTipePackage"].ToString()
                    });
                }
                // foreach (DataRow dr in dtOther.Rows)
                // {
                //     docRepairOther.Add(new IDocRepairOther
                //     {                                                
                //         NomorRepair = dr["NomorRPR"].ToString(),
                //         NamaPackage = dr["other_package_instruction"].ToString(),                        
                //         CostOther = Convert.ToDecimal(dr["other_cost_instruction"])
                //     });
                // }
            }
        }

        return new IDocRepair
        {
            docRepairDetail = docRepairDetail,
            docRepairPackage = docRepairPackage,
            docRepairParcel = docRepairParcel,
            docRepairOther = docRepairOther
        };
    }

    public class IDocRepair
    {
        public IDocRepairDetail docRepairDetail;
        public List<IDocRepairPackage> docRepairPackage;
        public List<IDocRepairParcel> docRepairParcel;
        public List<IDocRepairOther> docRepairOther;
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