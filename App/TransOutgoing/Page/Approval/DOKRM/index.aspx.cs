using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Web.Script.Services;


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

    protected string myV { get { return Request.QueryString["valid"]; } }

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
    public static object GetNomorKRM(string keyword, string start, string end, string loccode)
	{
		string[] start1 = start.Split('-');
		string[] end1 = end.Split('-');
		string xDateFrom = start1[2] + "-" + start1[1] + "-" + start1[0];
		string xDateTo = end1[2] + "-" + end1[1] + "-" + end1[0];
		
		string queryGetNomorKRM = "exec sp_report_dokrm_new '" + keyword.Trim() + "', '" + xDateFrom + "', " +
		"'" + xDateTo + "','" + loccode + "','1','1','20','1'";
		DataTable dt = Db.Rs(queryGetNomorKRM, Db.Cnn_DB);

		List<Krm> listkrm = new List<Krm>();
		int i = 0;
		foreach(DataRow dr in dt.Rows)
		{
			Krm krm = new Krm();
			krm.no = Convert.ToInt32(dr["NO"]);
			krm.id = Convert.ToInt32(dr["ID"]);
			krm.lokasi = dr["LOKASI"].ToString();
			krm.kurir = dr["NAMA_KURIR"].ToString();
			krm.statusdo = dr["STATUS_DO"].ToString();
			krm.operatornama = dr["OPERATOR_BUAT"].ToString();
			krm.operatortgl = Convert.ToDateTime(dr["TGL_BUAT"]).ToString("dddd, dd MMMM yyyy hh:mm");
			krm.etd = Convert.ToDateTime(dr["ETD"]).ToString("dddd, dd MMMM yyyy");
			krm.keterangan = dr["KETERANGAN"].ToString();
			listkrm.Add(krm);
		}
		return listkrm;
	}
	
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetDetailKRM(string IDF)
    {
        string queryGetNomorKRM = "exec sp_get_dokrm_new '" + IDF.ToString() + "'";
        DataSet ds = Ds(queryGetNomorKRM, Db.Cnn_DB);
        DataTable dtheader = ds.Tables[0];
        DataTable dtdetail = ds.Tables[1];

        int status = 0;
        List<KrmDetail> detail = new List<KrmDetail>();
        foreach (DataRow dr in dtdetail.Rows)
        {
            detail.Add(new KrmDetail()
            {
                nokrm = dr["NoKRM"].ToString(),
                cross_brand = Convert.ToBoolean(dr["IsCrossBrand"]) ? "Cross Brand" : "Not Cross Brand",
                keterangan = dr["Keterangan"].ToString(),
                lokasi_asal = dr["LokasiAsal"].ToString(),
                lokasi_tujuan = dr["LokasiTujuan"].ToString(),
                qty_outgoing = Convert.ToInt32(dr["QtyOutgoing"]),
                weight_outgoing = Convert.ToDecimal(dr["GrossWeightOutgoing"]),
                tipe_product = Convert.ToInt32(dr["TipeProduct"]) == 1 ? "DJ" : Convert.ToInt32(dr["TipeProduct"]) == 2 ? "PG" : Convert.ToInt32(dr["TipeProduct"]) == 3 ? "GJ" : "LD"
            });
        }
        if (dtheader.Rows.Count == 1)
        {
            KrmHeader header = new KrmHeader();
            header.namakurir = dtheader.Rows[0]["NamaKurir"].ToString();
            header.keterangan = dtheader.Rows[0]["Keterangan"].ToString();
			
			string listti = "<ol style=\"padding-left:15px\">";
			
			if(dtheader.Rows[0]["KeteranganTitipan"] != DBNull.Value || dtheader.Rows[0]["KeteranganTitipan"].ToString() != "")
			{
				string[] titipan = JsonConvert.DeserializeObject<string[]>(dtheader.Rows[0]["KeteranganTitipan"].ToString());
				
				for(int i = 0;i < titipan.Length; i++)
				{
					listti += "<li>" + titipan[i].ToString() + "</li>";
				}
			}
			
			listti += "</ol>";
			
			header.keterangan_titipan = listti.ToString();
            header.statusdo = Convert.ToInt32(dtheader.Rows[0]["StatusDO"]) == 1 ? "Waiting Approval" : Convert.ToInt32(dtheader.Rows[0]["StatusDO"]) == 2 ? "Approved" : Convert.ToInt32(dtheader.Rows[0]["StatusDO"]) == 3 ? "Rejected" : Convert.ToInt32(dtheader.Rows[0]["StatusDO"]) == 4 ? "Delivered" : "";
            header.operatornama = dtheader.Rows[0]["OperatorOutgoing"].ToString();
            header.operatortgl = Convert.ToDateTime(dtheader.Rows[0]["OperatorOutgoingTgl"]).ToString("dddd, dd MMMM yyyy hh:mm");
			header.etd = Convert.ToDateTime(dtheader.Rows[0]["ETD"]).ToString("dddd, dd MMMM yyyy");
            header.detail = detail;

            status = 1;

            return new { status, header };
        }
        else
        {
            return new { status };
        }
    }
	
	[WebMethod]
	[ScriptMethod(UseHttpGet = false)]
	public static object ApproveDO(List<int> ids)
	{
		string message = "";
		int status = 0;
		
		foreach(var c in ids){
			string queryValKRM = "EXEC sp_validate_approve_dokrm_new '" + c.ToString() + "'";
			DataTable dt = Db.Rs(queryValKRM, Db.Cnn_DB);
			int status_val = Convert.ToInt32(dt.Rows[0]["Status"]);
			string result_val = dt.Rows[0]["Message"].ToString();
			if(status_val == 0)
			{
				status = status_val;
				message = result_val;
				break;
			}
		}
		
		if(message == "")
		{
			try
			{
				string queryApproveKRM = "";
				foreach (var r in ids)
				{
					queryApproveKRM += "EXEC sp_approve_dokrm_new '" + r.ToString() + "','" + App.UserNama + "' ";
				}
				Db.Execute(queryApproveKRM, Db.Cnn_DB);
				message = "";
				status = 1;
			}
			catch(Exception ex)
			{
				message = ex.Message;
				status = 0;
			}
		}
		return new { status, message };
	}
	
	[WebMethod]
	[ScriptMethod(UseHttpGet = false)]
	public static object DeleteDO(int id)
	{
		string message = "";
		int status = 0;
		
		string queryValKRM = "EXEC sp_delete_dokrm_new '" + id.ToString() + "','" + App.UserNama + "'";
		DataTable dt = Db.Rs(queryValKRM, Db.Cnn_DB);
		int status_val = Convert.ToInt32(dt.Rows[0]["Status"]);
		string result_val = dt.Rows[0]["Message"].ToString();
		if(status_val == 0)
		{
			status = status_val;
			message = result_val;
		}
		else
		{
			status = status_val;
			message = "";
		}
		
		return new { status, message };
	}
		
	
	public static DataSet Ds(string strSql, string CnnString)
    {
        SqlConnection sqlCnn = new SqlConnection(CnnString);
        SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSql, sqlCnn);
        sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
        DataSet objDS = new DataSet();
        sqlAdapter.Fill(objDS, "data");
        sqlCnn.Close();
        return objDS;
    }

    public class Location
    {
        public int id { get; set; }
        public int tipe { get; set; }
        public string nama { get; set; }
        public string kode { get; set; }
    }
	
    public class Krm 
    {
        public int no { get; set; }
		public int id { get; set; }
        public string lokasi { get; set; }
        public string kurir { get; set; }
        public string statusdo { get; set; }
        public string operatornama { get; set; }
        public string operatortgl { get; set; }
        public string etd { get; set; }
		public string keterangan { get; set; }
    }
	
	public class KrmHeader
    {
        public string namakurir { get; set; }
        public string keterangan { get; set; }
		public string keterangan_titipan { get; set; }
        public string statusdo { get; set; }
        public string operatornama { get; set; }
        public string operatortgl { get; set; }
		public string etd { get;set; }
        public List<KrmDetail> detail { get; set; }
    }

    public class KrmDetail
    {
        public string nokrm { get; set; }
        public string tipe_product { get; set; }
        public string cross_brand { get; set; }
        public int qty_outgoing { get; set; }
        public decimal weight_outgoing { get; set; }
        public string keterangan { get; set; }
        public string lokasi_asal { get; set; }
        public string lokasi_tujuan { get; set; }
    }
}