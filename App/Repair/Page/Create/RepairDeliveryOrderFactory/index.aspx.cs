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


public partial class RepairDeliveryOrder_Index : System.Web.UI.Page
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
    public static List<IDataLocation> GetLocPLURepair(string tglStart, string tglEnd)
    {
        List<IDataLocation> Location = new List<IDataLocation>();
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getListRepairFromFactory", connection))
			{
				command.CommandType = CommandType.StoredProcedure;				
				command.Parameters.Add(new SqlParameter("@tglStart", SqlDbType.VarChar) { Value = tglStart });
				command.Parameters.Add(new SqlParameter("@tglEnd", SqlDbType.VarChar) { Value = tglEnd });
				command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = 14 });
				command.Parameters.Add(new SqlParameter("@GroupBy", SqlDbType.VarChar) { Value = "GetLoc" });

				connection.Open();
				using (SqlDataReader reader = command.ExecuteReader())
				{
					bool found = false;
					do
					{
						var cols = Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)).ToList();
						if (cols.Contains("ID") && cols.Contains("Nama")) { found = true; break; }
					} while (reader.NextResult());

					if (found)
					{
						while (reader.Read())
						{
							Location.Add(new IDataLocation
							{
								IDLokasi = (int)reader["ID"],
								NamaOutlet = reader["Nama"].ToString()
							});
						}
					}
				}
				return Location;
			}
        }
    }
    
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IDeliveryOrderRepairReport> GetSendDeliveryOrderRepairFactory(string tglStart, string tglEnd, int IDLokasi)
    {
        DataSet ds = ExecuteGetSendDeliveryOrderRepairFactory(tglStart, tglEnd, IDLokasi);
        DataTable dt = ds.Tables[0];

        List<IDeliveryOrderRepairReport> doRepairReport = new List<IDeliveryOrderRepairReport>();
        foreach (DataRow dr in dt.Rows)
        {
            doRepairReport.Add(new IDeliveryOrderRepairReport
            {
                IDHeaderReparasi = (int)dr["idh_dobrj_reparasi"],
                NomorDO = dr["NomorDo"].ToString(),
                BeratAwal = Convert.ToDecimal(dr["beratAwal"]),
                BeratAkhir = Convert.ToDecimal(dr["beratAkhir"]),
                Status = dr["Status"].ToString().Trim(),
                Tanggal = dr["Tanggal"].ToString(),
                IsValid = (int)dr["IsValid"],
                NomorParcelArray = dr["NomorParcelArray"].ToString()
            });
        }
        return doRepairReport;
    }

    public static DataSet ExecutePostSendDeliveryOrderRepair(ISendDeliveryOrderParams sendDeliveryOrderParams)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_sendRepairByNomorFromFactoryToOutlet", connection))
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(sendDeliveryOrderParams.DO_Detail);
                DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@NamaKurir", SqlDbType.VarChar) { Value = sendDeliveryOrderParams.Kurir });
                command.Parameters.Add(new SqlParameter("@Keterangan", SqlDbType.VarChar) { Value = sendDeliveryOrderParams.Keterangan });
                command.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar) { Value = App.UserNama });
                command.Parameters.Add(new SqlParameter("@NomorDO", SqlDbType.Structured) { Value = pDt });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }


    [WebMethod]
    public static string PostSendDeliveryOrderRepair(ISendDeliveryOrderParams args)
    {
        DataSet ds = ExecutePostSendDeliveryOrderRepair(args);
        return "Success";
    }

    public static DataSet ExecuteGetSendDeliveryOrderRepairFactory(string TglAwal, string TglAkhir, int IDLokasi)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getListRepairFromFactory", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@tglStart", SqlDbType.VarChar) { Value = TglAwal });
                command.Parameters.Add(new SqlParameter("@tglEnd", SqlDbType.VarChar) { Value = TglAkhir });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.VarChar) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@GroupBy", SqlDbType.VarChar) { Value = "Report" });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }

    public class IDeliveryOrderRepairReport
    {
        public int IDHeaderReparasi { get; set; }
        public string NomorDO { get; set; }
        public decimal BeratAwal { get; set; }
        public decimal BeratAkhir { get; set; }
        public string Status { get; set; }
        public string Tanggal { get; set; }
        public int IsValid { get; set; }
        public string NomorParcelArray { get; set; }
    }

    public class ISendDeliveryOrderParams
    {
        public string Kurir { get; set; }
        public string Keterangan { get; set; }
        public List<object> DO_Detail { get; set; }
    }

    public class ILocRepair
    {
        public int ID { get; set; }
        public string Nama { get; set; }
        public string Kode { get; set; }
    }
    public class IDataSalesWithLocation
    {
        public int IDSales { get; set; }
        public string NamaSales { get; set; }
        public string NIK { get; set; }
        public string NamaOutlet { get; set; }
        public string KodeOutlet { get; set; }
        public int IDLokasi { get; set; }
        public int TipeLokasi { get; set; }
        public string BrandOutlet { get; set; }
        public int IDBrand { get; set; }
    }
    public class IDataLocation
    {
        public int IDLokasi { get; set; }
        public string NamaOutlet { get; set; }        
        
    }
    public class ISalesLocationResponse
    { 
        public List<IDataLocation> location { get; set; }
    }
}