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
    public static object ReportKRM(string keyword, string start, string end, string loccode, string statusdo)    
    {
        string[] start1 = start.Split('-');
        string[] end1 = end.Split('-');
        string xDateFrom = start1[2] + "-" + start1[1] + "-" + start1[0];
        string xDateTo = end1[2] + "-" + end1[1] + "-" + end1[0];

        int xUserID = App.UserID;
        int xisAM = 0;
        int xisAE = 0;
        string listStore = "";
        //check area manager
		using (Map_DB db = new Map_DB())
		{
			var am = from p in db.DataAreaManager where p.IDUser == App.UserID select p;
			int amcount = am.Count();
			if (amcount > 0)
			{
				int z = 0;
				foreach (var x in am)
				{
					int j = 0;
					var lokasi = x.DataAreaManager_Lokasi
						.GroupBy(p => new { p.IDForm, p.TipeLokasi, p.IDLokasi })
						.Select(c => new { c.Key.IDForm, c.Key.TipeLokasi, c.Key.IDLokasi });
					foreach (var y in lokasi)
					{						
						if (y.TipeLokasi == 1)
							listStore += db.LocOutlet.Single(p => p.ID == y.IDLokasi).Kode;
						else
							listStore += db.LocExhibition.Single(p => p.ID == y.IDLokasi).Kode;
						j++;
						listStore += ",";
					}
					z++;
				}
				xisAM = 1;
			}

			if (xisAM == 0)
			{
				//check admin sales
				var ad = from p in db.DataAdminSales where p.IDUser == App.UserID select p;
				if (ad.Count() > 0)
				{
					int k = 0;
					foreach (var x in ad)
					{						
						if (x.TipeLokasi == 1)
							listStore = db.LocOutlet.Single(p => p.ID == x.IDLokasi).Kode;
						else
							listStore = db.LocExhibition.Single(p => p.ID == x.IDLokasi).Kode;
						k++;
						listStore += ",";
					}
					xisAE = 1;
				}
			}
		}

        string xKeyloc = (listStore.Length > 0) ? listStore.Remove(listStore.Length - 1) : listStore;
        loccode = xKeyloc;    

        string query = "exec sp_report_dokrm_detail_new '" + keyword.Trim() + "', '" + xDateFrom + "', " +
        "'" + xDateTo + "','" + loccode + "','" + statusdo + "','1','20','1'";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        Krm[] listkrm = new Krm[dt.Rows.Count];
        int i = 0;
		foreach (DataRow dr in dt.Rows)
		{
			Krm krm = new Krm();
			krm.no = Convert.ToInt32(dr["NO"]);
			krm.id = Convert.ToInt32(dr["ID"]);
			krm.nodokrm =  dr["NOMOR_DO_KRM"].ToString();
			krm.nokrm = dr["NO_KRM"].ToString().Replace(",","<br />");
			krm.kurir = dr["NAMA_KURIR"].ToString();
			krm.statusdo = dr["STATUS_DO"].ToString();
			krm.operatornama = dr["OPERATOR_BUAT"].ToString();
			krm.operatortgl = Convert.ToDateTime(dr["TGL_BUAT"]).ToString("dddd, dd MMMM yyyy hh:mm");
			krm.approvalnama = dr["OPERATOR_APPROVED"].ToString();
			krm.approvaltgl = (dr["TGL_APPROVED"] == DBNull.Value) ? "" : Convert.ToDateTime(dr["TGL_APPROVED"]).ToString("dddd, dd MMMM yyyy hh:mm");
			krm.etd = Convert.ToDateTime(dr["ETD"]).ToString("dddd, dd MMMM yyyy");
			krm.keterangan = dr["KETERANGAN"].ToString();
			krm.tujuan = dr["TUJUAN"].ToString();
			krm.qty = Convert.ToInt32(dr["QTY"]).ToString();
			krm.estimasi_harga = Convert.ToDecimal(dr["ESTIMASI_HARGA"]).ToString("N0");
			krm.estimasi_harga_int = Convert.ToInt64(dr["ESTIMASI_HARGA"]);
			krm.TipeTujuan = Convert.ToInt32(dr["TipeTujuan"]);
			krm.IDLokasiTujuan = Convert.ToInt32(dr["IDLokasiTujuan"]);
			krm.IDBrandTujuan = Convert.ToInt32(dr["IDBrandTujuan"]);
			listkrm[i] = krm;            
			
			i++;
		}
        return listkrm;
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]    
    public static object GetNomorKRM(string nomorKRM, string brandtujuan, int idlokasitujuan, int tipelokasitujuan)
    {
		string brandasal = "";
		int idlokasiasal = 0;
		int tipelokasiasal = 0;
		
		string cekval = ValidateSaveDO(nomorKRM.Trim());
		ResponseDataKRM response = new ResponseDataKRM();
		if(string.IsNullOrEmpty(cekval))
		{
			string queryGetNomorKRM = "exec sp_getKRMByNomor '" + nomorKRM.Trim() + "', '" + brandasal + "', " +
			"'" + tipelokasiasal + "','" + idlokasiasal + "','" + brandtujuan + "','" + tipelokasitujuan + "','" + idlokasitujuan + "'" ;
			DataTable dt = Db.Rs(queryGetNomorKRM, Db.Cnn_DB);
			
			int rowcount = dt.Rows.Count;
			
			if(rowcount == 0)
			{
				response.error_message = "Data Tidak Ditemukan.";
			}
			else if(rowcount > 1)
			{
				response.error_message = "Data Duplikat.";
			}
			else
			{
				response.error_message = "";
			}

			DataKRM dataKRM = new DataKRM();
			try
			{
				dataKRM.Tipe_Product = dt.Rows[0]["Tipe_Product"].ToString();
				dataKRM.Tipe_Lokasi_Tujuan = Convert.ToInt32(dt.Rows[0]["Tipe_Lokasi_Tujuan"]);
				dataKRM.ID_Lokasi_Tujuan = Convert.ToInt32(dt.Rows[0]["ID_Lokasi_Tujuan"]);
				dataKRM.Is_Cross_Brand = Convert.ToBoolean(dt.Rows[0]["Is_Cross_Brand"]) ? "Cross Brand" : "Same Brand";
				dataKRM.Nomor = dt.Rows[0]["Nomor"].ToString();
				dataKRM.Keterangan = dt.Rows[0]["Keterangan"].ToString();
				dataKRM.JumlahBarang = Convert.ToInt32(dt.Rows[0]["JumlahBarang"]);
				dataKRM.GrossWeight = Convert.ToDecimal(dt.Rows[0]["GrossWeight"]);
				dataKRM.LokasiAsal = dt.Rows[0]["LokasiAsal"].ToString();
				dataKRM.LokasiTujuan = dt.Rows[0]["LokasiTujuan"].ToString();
				dataKRM.PathToDetail = dt.Rows[0]["PathToDetail"].ToString();
				dataKRM.EstimasiHarga = Convert.ToDecimal(dt.Rows[0]["EstimasiHarga"]);
				dataKRM.Brand_Asal = Convert.ToString(dt.Rows[0]["Brand_Asal"]);
				dataKRM.ID_Tipe_Asal = Convert.ToInt32(dt.Rows[0]["Tipe_Lokasi_Asal"]);
				dataKRM.ID_Lokasi_Asal = Convert.ToInt32(dt.Rows[0]["ID_Lokasi_Asal"]);
				dataKRM.Brand_Tujuan = Convert.ToString(dt.Rows[0]["Brand_Tujuan"]);
			}
			catch{}
			
			response.krm = dataKRM;
		}
		else
		{
			response.error_message = cekval;
		}

        return response;
    }

	[WebMethod]
    [ScriptMethod(UseHttpGet = false)]
	public static object DeleteDO(List<int> ids){
		int status=0;
		string message="";
		if(message == "")
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
				{
					conn.Open();
					foreach (var r in ids)
					{
						string QueryDelete = "DELETE FROM DoKrmDetail where ID = '" + r.ToString() + "' ";
						using (SqlCommand cmd = new SqlCommand(QueryDelete, conn))
						{
							cmd.ExecuteNonQuery();
						}
						message = r.ToString();
					}
					conn.Close();
				}
				
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
	public static object UpdateDO(RequestDataKRM data){
		string message = "";
		int status = 0;
		bool ispass = true;
		
		foreach(var x in data.krm_detail)
		{
			string cekval = ValidateSaveDO(x.NoKRM);
			if(!string.IsNullOrEmpty(cekval))
			{
				ispass = false;
				message = cekval;
				break;
			}
		}

		if(ispass){
			DataTable dtdetail = new DataTable("DoKrmDetailUpdate");			
			dtdetail.Columns.Add("IDDOHead", typeof(int));
			dtdetail.Columns.Add("NoKRM", typeof(string));
			dtdetail.Columns.Add("TipeProduct", typeof(int));
			dtdetail.Columns.Add("IsCrossBrand", typeof(bool));
			dtdetail.Columns.Add("QtyOutgoing", typeof(int));
			dtdetail.Columns.Add("GrossWeightOutgoing", typeof(decimal));
			dtdetail.Columns.Add("Keterangan", typeof(string));
			dtdetail.Columns.Add("NamaAsal", typeof(string));
			dtdetail.Columns.Add("NamaTujuan", typeof(string));
			dtdetail.Columns.Add("EstimasiHarga", typeof(decimal));
			dtdetail.Columns.Add("BrandAsal", typeof(string));
			dtdetail.Columns.Add("IDTipeAsal", typeof(int));
			dtdetail.Columns.Add("IDLokasiAsal", typeof(int));
			dtdetail.Columns.Add("BrandTujuan", typeof(string));			
			dtdetail.Columns.Add("IDTipeTujuan", typeof(int));
			dtdetail.Columns.Add("IDLokasiTujuan", typeof(int));			

			foreach(var x in data.krm_detail)
			{
				DataRow dataRow2 = dtdetail.NewRow();
				dataRow2["IDDOHead"] = x.IDDOHead;
				dataRow2["NoKrm"] = x.NoKRM;
				dataRow2["TipeProduct"] = Convert.ToInt32(x.TipeProduct);
				dataRow2["IsCrossBrand"] = Convert.ToBoolean(x.IsCrossBrand);
				dataRow2["QtyOutgoing"] = Convert.ToInt32(x.QtyOutgoing);
				dataRow2["GrossWeightOutgoing"] = Convert.ToDecimal(x.GrossWeightOutgoing);
				dataRow2["Keterangan"] = x.Keterangan;
				dataRow2["NamaAsal"] = x.Asal;
				dataRow2["NamaTujuan"] = x.Tujuan;
				dataRow2["EstimasiHarga"] = x.EstimasiHarga;
				dataRow2["BrandAsal"] = Convert.ToString(x.BrandAsal);
				dataRow2["IDTipeAsal"] = Convert.ToInt32(x.IDTipeAsal);
				dataRow2["IDLokasiAsal"] = Convert.ToInt32(x.IDLokasiAsal);
				dataRow2["BrandTujuan"] = Convert.ToString(x.BrandTujuan);			
				dataRow2["IDTipeTujuan"] = Convert.ToInt32(x.IDTipeTujuan);
				dataRow2["IDLokasiTujuan"] = Convert.ToInt32(x.IDLokasiTujuan);
				dtdetail.Rows.Add(dataRow2);
			}
			string query = "sp_update_add_dokrm";
			DataTable result = new DataTable();
			using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
			{
				try
				{
					
					SqlCommand cmd = conn.CreateCommand();
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandText = query; //The name of the above mentioned stored procedure.  					
					SqlParameter dokrmdetail = cmd.Parameters.AddWithValue("@DoKrmDetailUpdate", dtdetail);													
					conn.Open();
					// create data adapter
					SqlDataAdapter da = new SqlDataAdapter(cmd);
					da.Fill(result);
					da.Dispose();
					conn.Close();
					message = result.Rows[0]["Message"].ToString();
					status = Convert.ToInt32(result.Rows[0]["Status"]);
				}
				catch (Exception ex)
				{
					message = ex.Message;
					if (conn != null && conn.State == ConnectionState.Open)
						conn.Close();
				}
			}
		}
		return new { message, status };
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
				idDetailKRM = Convert.ToInt32(dr["ID"])

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
	
	protected static string ValidateSaveDO(string krm)
	{
		string result = string.Empty;
		string query = "sp_check_dokrm_new";
		using(SqlConnection conn = new SqlConnection(Db.Cnn_DB))
		{
			try
			{
				SqlCommand cmd = conn.CreateCommand();
				cmd.CommandText = query;
				cmd.CommandType = CommandType.StoredProcedure;
				SqlParameter param = cmd.Parameters.AddWithValue("@NOKRM", krm);
				conn.Open();

				object r = cmd.ExecuteScalar();
				result = r != null ? r.ToString() : "Data Error.";
			}
			catch (Exception ex) 
			{ 
				result = ex.Message;
			}
		}

		return result;
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

	public class ResponseDataKRM
	{
		public string error_message { get; set; }
		public DataKRM krm { get; set; }
	}

    public class DataKRM {
		public string Tipe_Product { get; set; }
		public int Tipe_Lokasi_Tujuan { get; set; }
		public int ID_Lokasi_Tujuan { get; set; }
		public string Is_Cross_Brand { get; set; }
        public string Nomor { get; set; }
        public string Keterangan { get; set; }
        public int JumlahBarang { get; set; }
		public decimal GrossWeight { get;set; }
		public string LokasiAsal { get; set; }
        public string LokasiTujuan { get; set; } 
        public string PathToDetail { get; set; }
		public decimal EstimasiHarga { get; set; }
		public string Brand_Asal { get; set; }
		public int ID_Tipe_Asal { get; set; }
		public int ID_Lokasi_Asal { get; set; }
		public string Brand_Tujuan { get; set; }
    }
	
	public class RequestDataKRM
	{		
		public List<DataKrm_Detail> krm_detail { get; set; }
	}

	public class DataKrm_Detail
	{
		public int IDDOHead { get; set; }
		public string NoKRM { get; set; }
		public int TipeProduct { get; set; }
		public int IsCrossBrand { get; set; }
		public int QtyOutgoing { get; set; }
		public decimal GrossWeightOutgoing { get; set; }
		public string Keterangan { get; set; }
		public string Asal { get; set; }
		public string Tujuan { get; set; }
		public decimal EstimasiHarga { get; set; }
		public string BrandAsal { get; set; }
		public int IDTipeAsal { get; set; }
		public int IDLokasiAsal { get; set; }
		public string BrandTujuan { get; set; }
		public int IDTipeTujuan { get; set; }
		public int IDLokasiTujuan { get; set; }
	}

	public class DataKRM_Header
	{
		public string NamaKurir { get; set; }
		public string Keterangan { get; set; }
		public int StatusDO { get; set; }
		public string OperatorOutgoingTgl { get; set;}
		public string OperatorOutgoingNama { get; set; }
		public string ETD { get; set; }
		public string KeteranganTitipan { get; set; }
		public int IDTipeTujuan { get; set; }
		public int IDLokasiTujuan { get; set; }
		public string BrandTujuan { get; set; }

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
		public int idDetailKRM { get; set; }
    }
    public class Krm
    {
        public int no { get; set; }
        public int id { get; set; }
        public string nodokrm { get; set; }
        public string nokrm { get; set; }
        public string lokasiasal { get; set; }
        public string lokasitujuan { get; set; }
        public string kurir { get; set; }
        public string statusdo { get; set; }
        public string operatornama { get; set; }
        public string operatortgl { get; set; }
        public string approvalnama { get; set; }
        public string approvaltgl { get; set; }
        public string etd { get; set; }
        public string keterangan { get; set; }
		public string tujuan { get; set; }
		public string qty { get;set; }
		public string estimasi_harga { get; set; }
		public long estimasi_harga_int { get; set; }
		public int TipeTujuan { get; set; }
		public int IDLokasiTujuan { get; set; }
		public int IDBrandTujuan { get; set; }		

    }    
}