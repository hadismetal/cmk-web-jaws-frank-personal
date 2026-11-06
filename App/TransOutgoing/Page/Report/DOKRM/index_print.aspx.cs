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

public partial class DOKRM_index_print : System.Web.UI.Page
{
	
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
                tipe_product = Convert.ToInt32(dr["TipeProduct"]) == 1 ? "DJ" : Convert.ToInt32(dr["TipeProduct"]) == 2 ? "PG" : Convert.ToInt32(dr["TipeProduct"]) == 3 ? "GJ" : "LD",
				selling_price = Convert.ToDecimal(dr["SellingPrice"]).ToString("N0")
            });
        }
        if (dtheader.Rows.Count == 1)
        {
            KrmHeader header = new KrmHeader();
			header.nomordo = dtheader.Rows[0]["Nomor"].ToString();
            header.namakurir = dtheader.Rows[0]["NamaKurir"].ToString();
            header.keterangan = dtheader.Rows[0]["Keterangan"].ToString();
			
			string listti = "<ol style=\"padding-left:15px\">";
			//string[] titipan = dtheader.Rows[0]["KeteranganTitipan"].ToString().Replace("\"","").Replace("[","").Replace("]","").Split(',');
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
            header.operatornama = dtheader.Rows[0]["ApprovalOutgoing"].ToString();
            header.operatortgl = Convert.ToDateTime(dtheader.Rows[0]["ApprovalOutgoingTgl"]).ToString("dddd, dd MMMM yyyy hh:mm");
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
	
	public class KrmHeader
    {
		public string nomordo { get;set; }
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
		public string selling_price { get;set; }
    }
}
