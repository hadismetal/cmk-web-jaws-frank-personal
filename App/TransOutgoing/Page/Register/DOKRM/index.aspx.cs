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

        tanggal_delivery_input.Value = DateTime.Today.ToString("dd/MM/yyyy");
        operator_delivery_input.Value = App.UserNama;

        if (!IsPostBack)
        {
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetLocationTujuan(int tipelokasi, string brand)
    {
		string query = "";
		string brand1 = ((brand == "F") ? "frank_db.dbo." : ((brand == "M") ? "mondial_db_real.dbo." : ((brand == "P") ? "palace_db_real.dbo." : "")));
		if(tipelokasi == 0)
		{
			query = "SELECT ID, Nama, Nama Kode FROM " + brand1 + "LocWarehouse WHERE Draft = 0 AND Disable = 0";
		}
		if(tipelokasi == 1)
		{
			query = "SELECT ID, Nama, Kode FROM " + brand1 + "LocOutlet WHERE Draft = 0 AND Disable = 0";
		}
		else if(tipelokasi == 2)
		{
			query = "SELECT ID, Nama, Kode FROM " + brand1 + "LocExhibition WHERE Draft = 0 AND Disable = 0";
		}
        DataTable dt = Db.Rs(query, Db.Cnn_DB); 
        List<Location> locations = new List<Location>();
        foreach(DataRow dr in dt.Rows) {
            Location location = new Location();
            location.id = (int)dr["ID"];
            location.tipe = tipelokasi;
            location.nama = dr["Nama"].ToString();
            location.kode = dr["Kode"].ToString();
            locations.Add(location);
        
        }
        return locations;
    }
	
	[WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetLocationAsal(int tipelokasi, string brand)
    {
        string query = "";
		string brand1 = ((brand == "F") ? "frank_db.dbo." : ((brand == "M") ? "mondial_db_real.dbo." : ((brand == "P") ? "palace_db_real.dbo." : "")));
		if(tipelokasi == 0)
		{
			query = "SELECT ID, Nama, Nama Kode FROM " + brand1 + "LocWarehouse WHERE Draft = 0 AND Disable = 0";
		}
		if(tipelokasi == 1)
		{
			query = "SELECT ID, Nama, Kode FROM " + brand1 + "LocOutlet WHERE Draft = 0 AND Disable = 0";
		}
		else if(tipelokasi == 2)
		{
			query = "SELECT ID, Nama, Kode FROM " + brand1 + "LocExhibition WHERE Draft = 0 AND Disable = 0";
		}
        DataTable dt = Db.Rs(query, Db.Cnn_DB); 
        List<Location> locations = new List<Location>();
        foreach(DataRow dr in dt.Rows) {
            Location location = new Location();
            location.id = (int)dr["ID"];
            location.tipe = tipelokasi;
            location.nama = dr["Nama"].ToString();
            location.kode = dr["Kode"].ToString();
            locations.Add(location);
        
        }
        return locations;
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
	public static object SaveDataDO(RequestDataKRM data)
	{
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
		
		if(ispass)
		{
			DataTable dt = new DataTable("DoKrmHeader");
			dt.Columns.Add("NamaKurir", typeof(string));
			dt.Columns.Add("Keterangan", typeof(string));
			dt.Columns.Add("StatusDO", typeof(int));
			dt.Columns.Add("OperatorOutgoingTgl", typeof(DateTime));
			dt.Columns.Add("OperatorOutgoing", typeof(string));
			dt.Columns.Add("ETD", typeof(DateTime));
			dt.Columns.Add("KeteranganTitipan", typeof(string));
			dt.Columns.Add("IDTipeTujuan", typeof(int));
			dt.Columns.Add("IDLokasiTujuan", typeof(int));
			dt.Columns.Add("BrandTujuan", typeof(string));

			DataRow dataRow = dt.NewRow();
			dataRow["NamaKurir"] = data.krm.NamaKurir;
			dataRow["Keterangan"] = data.krm.Keterangan;
			dataRow["StatusDO"] = 1;
			dataRow["OperatorOutgoingTgl"] = Convert.ToDateTime(DateTime.Now);
			dataRow["OperatorOutgoing"] = data.krm.OperatorOutgoingNama;
			dataRow["ETD"] = Convert.ToDateTime(data.krm.ETD);
			dataRow["KeteranganTitipan"] = data.krm.KeteranganTitipan;
			dataRow["IDTipeTujuan"] = Convert.ToInt32(data.krm.IDTipeTujuan);
			dataRow["IDLokasiTujuan"] = Convert.ToInt32(data.krm.IDLokasiTujuan);			
			dataRow["BrandTujuan"] = Convert.ToString(data.krm.BrandTujuan);
			dt.Rows.Add(dataRow);

			DataTable dtdetail = new DataTable("DoKrmDetail");
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

			string query = "sp_insert_dokrm_new";
			DataTable result = new DataTable();

			using (SqlConnection conn = new SqlConnection(Db.Cnn_DB))
			{
				try
				{										
					SqlCommand cmd = conn.CreateCommand();
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandText = query; //The name of the above mentioned stored procedure.  
					SqlParameter dokrm = cmd.Parameters.AddWithValue("@DoKrm", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.  
					SqlParameter dokrmdetail = cmd.Parameters.AddWithValue("@DoKrmDetail", dtdetail);					
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
		else
		{
			
		}
		return new { status, message };
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
    }
	
	public class RequestDataKRM
	{
		public DataKRM_Header krm { get; set; }
		public List<DataKrm_Detail> krm_detail { get; set; }
	}

	public class DataKrm_Detail
	{
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

    public class Product {
        public int id { get; set; }
        public string nama { get; set; }
    }

    public class Location {
        public int id {get; set;}
        public int tipe {get; set;}
        public string nama {get; set;}
        public string kode {get; set;}
    }
}