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
    public static object GetDetailRepair(int IDKRM, int IsWarehouse, string Brand)
    {
        var docRepair = ExecuteDetailDocRepair(IDKRM, IsWarehouse, Brand);
        return docRepair;
    }

    public static object ExecuteDetailDocRepair(int IDKRM, int IsWarehouse, string Brand)
    {
        var KRM = new object();
        var KRMDetail = new List<object>();

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getDetailDeliveryOrderRepair_Krm_Print", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDKRM", SqlDbType.VarChar) { Value = IDKRM });
                command.Parameters.Add(new SqlParameter("@IsWarehouse", SqlDbType.VarChar) { Value = IsWarehouse });
                command.Parameters.Add(new SqlParameter("@Brand", SqlDbType.VarChar) { Value = Brand });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];

                foreach (DataRow dr in dt.Rows)
                {
                    KRM = new
                    {
                        NomorKRM = dr["NomorKRM"].ToString(),
                        NomorTRM = dr["NomorTRM"].ToString(),
                        AddrAlamat = dr["AddrAlamat"].ToString(),
                        KodeStore = dr["KodeStore"].ToString(),
                        Keterangan = dr["Keterangan"].ToString(),
                        NamaKurir = dr["NamaKurir"].ToString(),
                        TglKirim = dr["TglKirim"].ToString(),
                        TglTerima = dr["TglTerima"].ToString(),
                        NamaAsal = dr["NamaAsal"].ToString(),
                        NamaTujuan = dr["NamaTujuan"].ToString(),
                        ApproverTerima = dr["ApproverTerima"].ToString(),
                    };
                }

                foreach (DataRow dr in dt2.Rows)
                {
                    KRMDetail.Add(new
                    {
                        No = (int)dr["No"],
                        IDDocRepair = (int)dr["IDDocRepair"],
                        NomorRepair = dr["NomorRepair"].ToString(),
                        CustomerNama = dr["CustomerNama"].ToString(),
                        SalesNama = dr["SalesNama"].ToString(),
                        PLU = dr["PLU"].ToString(),
                        ImgPicture = dr["ImgPicture"].ToString(),
                        Item = dr["Item"].ToString(),
                        GrossWeight = Convert.ToDecimal(dr["GrossWeight"]),
                        NettWeight = Convert.ToDecimal(dr["NettWeight"]),
                        Qty = (int)dr["Qty"],
                    });
                }

            }

        }

        return new { KRM = KRM, KRMDetail = KRMDetail };
    }
}