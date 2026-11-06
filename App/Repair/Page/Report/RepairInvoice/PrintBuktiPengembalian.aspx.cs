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

    protected int myID { get { return App.QueryInt(this); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);
        
        decimal paid = Db.SingleDecimal("SELECT B.TotalBayar FROM DocRepairResult A JOIN SalesReceipt B ON A.ID = B.IDRepair", Db.Cnn_DB);
        // var DocRepairResult = from p in db.DocRepairResult where p.IDWorkOrder == myID select p;
        // var idDocRepairResult = DocRepairResult.Max()
        // var idResult = db.DocRepairResult.OrderByDescending(u => u.ID).FirstOrDefault(u => u.IDWorkOrder == myID);
        // detailPaymentControlR.SetDetailPayment("SO", idResult.ID,  0, paid);


        if (!IsPostBack)
        {
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetDetailInvoice(int IDDocRepair) {
        DataSet ds = ExecuteDocRepair(IDDocRepair);

        DataTable dt = ds.Tables[0];


        List<object> Data = new List<object>();

        foreach(DataRow dr in dt.Rows) {
            Data.Add(new {
                NomorRepair = (string)dr["NomorRepair"],
                TglRepair = dr["tanggalPengajuan"].ToString(),
                TglPengembalian = dr["tanggalPengembalian"].ToString(),                                
                SalesNama = (string)dr["SalesNama"],
                CustomerNama = (string)dr["CustomerNama"],
                Item = (string)(dr["Item"]),
                BeratBarang = Convert.ToDecimal(dr["BeratBarang"]).ToString("N3"),
                BeratPotongan = Convert.ToDecimal(dr["BeratPotongan"]).ToString("N3"),
                Store = (string)dr["Store"],
                AddrAlamat = (string)dr["AddrAlamat"],
                ImgPicture = (string)dr["ImgPicture"],
                ImgPictureItem = (string)dr["ImgPictureItem"],
                PLU = (string)dr["PLU"]
            });
        }

        return Data[0];
    }

    public static DataSet ExecuteDocRepair(int IDDocRepair)
    {
        string queryGetDetailPengembalian = @"
                        select A.* INTO #TEMPLOC 
                            FROM (SELECT ID as IDLokasi, Nama as NamaLokasi, 1 as TipeLokasi, AddrAlamat from LocOutlet 
                            UNION
                            SELECT ID as IDLokasi, Nama as NamaLokasi, 2 as TipeLokasi, AddrAlamat from LocExhibition
                        )A
																
						SELECT * 
						FROM (
						    SELECT a.ID, a.Nomor as NomorRepair,  CONVERT(varchar(12),a.OperatorTgl, 105) as tanggalPengajuan, CONVERT(varchar(12),ad.CreatedAt, 105) as tanggalPengembalian
							,ae.NamaLokasi as Store, ae.AddrAlamat, a.CustomerNama, a.SalesNama, aa.Nomor as PLU
							, aba.Nama as Item, ac.WeightEnd as BeratBarang, ac.WeightPiece as BeratPotongan, aa.ImgPicture, a.ImgPicture as ImgPictureItem
							from DocRepair a 
							JOIN DocRepairResult b on b.IDWorkOrder=a.ID
							JOIN StockProductDJ aa on aa.ID=a.IDProduct
							JOIN DocRepair_CharProduct ab on ab.ID=a.ID
							JOIN DocRepair_Pivot ac on ac.IDDocRepair=a.ID
							JOIN CharProductItem aba on aba.ID=ab.ProductItem
							JOIN StockLedgerReparasi ad on ad.IDDocRepair=a.ID
							JOIN #TEMPLOC ae on ae.IDLokasi=a.IDLokasi and ae.TipeLokasi=a.TipeLokasi
							where ad.Status=10
							UNION
							select a.ID,a.Nomor as NomorRepair,  CONVERT(varchar(12),a.OperatorTgl, 105) as tanggalPengajuan, CONVERT(varchar(12),ad.CreatedAt, 105) as tanggalPengembalian
							,ae.NamaLokasi as Store, ae.AddrAlamat, a.CustomerNama, a.SalesNama, aa.Nomor as PLU
							, aba.Nama as Item, ab.GrossWeight as BeratBarang, ac.WeightPiece as BeratPotongan, aa.ImgPicture, a.ImgPicture as ImgPictureItem
							from DocRepair a 
							LEFT JOIN DocRepairResult b on b.IDWorkOrder=a.ID
							JOIN StockProductDJ aa on aa.ID=a.IDProduct
							JOIN DocRepair_CharProduct ab on ab.ID=a.ID
							JOIN DocRepair_Pivot ac on ac.IDDocRepair=a.ID
							JOIN CharProductItem aba on aba.ID=ab.ProductItem
							JOIN StockLedgerReparasi ad on ad.IDDocRepair=a.ID
							JOIN #TEMPLOC ae on ae.IDLokasi=a.IDLokasi and ae.TipeLokasi=a.TipeLokasi
							where ad.Status=10 and b.ID IS NULL
						) xxx WHERE xxx.ID=@IDDocRepair
					

						DROP TABLE #TEMPLOC
";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(queryGetDetailPengembalian, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.Int) { Value = IDDocRepair });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }
}