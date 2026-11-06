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
    public static ISalesLocationResponse GetUserOutletAndDefaultSales()
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getDefaultLocationAndSalesOrAE", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = App.UserID });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];
                List<IDataLocation> dataLocation = Helper.GetDataFromDataTable<IDataLocation>(dt);
                List<IDataSalesWithLocation> dataSales = Helper.GetDataFromDataTable<IDataSalesWithLocation>(dt2);

                return new ISalesLocationResponse
                {
                    sales = dataSales,
                    location = dataLocation
                };
            }
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<ILocRepair> GetLocationRepair()
    {
        var locationRepair = Db.Rs("SELECT ID_LokasiReparasi [ID], JenisLokasi [Nama], LokasiReparasi [Kode] FROM [DB_PROD_MANUFAKTUR].[dbmfg].[dbo].[msLokasiReparasi] WHERE StatusAktif = 'A'", Db.Cnn_DB);
        List<ILocRepair> locRepair = Helper.GetDataFromDataTable<ILocRepair>(locationRepair);
        return locRepair;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IDataLocation> GetLocationTujuan(string kodeStore)
    {
        DataTable dt = Db.Rs("exec sp_mappingMasterServicePoint " + kodeStore, Db.Cnn_DB);

        List<IDataLocation> locations = new List<IDataLocation>();
        foreach (DataRow dr in dt.Rows)
        {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            location.BrandOutlet = dr["Brand"].ToString();
            location.IDBrand = (int)dr["IDBrand"];
            location.Province = dr["Province"].ToString();
            locations.Add(location);
        }

        return locations;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IDeliveryOrderRepairReport> GetSendDeliveryOrderRepair(int idlokasi, int tipelokasi, int idlokasireparasi)
    {
        DataSet ds = ExecuteGetSendDeliveryOrderRepair(idlokasi, tipelokasi, idlokasireparasi);
        DataTable dt = ds.Tables[0];

        List<IDeliveryOrderRepairReport> doRepairReport = new List<IDeliveryOrderRepairReport>();
        foreach (DataRow dr in dt.Rows)
        {
            doRepairReport.Add(new IDeliveryOrderRepairReport
            {
                IDDocRepair = (int)dr["ID"],
                Image = RepairLib.GetPathImage(dr["Image"].ToString()),
                NamaCustomer = dr["NamaCustomer"].ToString(),
                NamaSales = dr["NamaSales"].ToString(),
                NomorReparasi = dr["NomorReparasi"].ToString(),
                PLU = dr["PLU"].ToString()
            });
        }
        return doRepairReport;
    }

    public static DataSet ExecutePostSendDeliveryOrderRepair(ISendDeliveryOrderParams sendDeliveryOrderParams)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_insertSendDeliveryOrderRepair", connection))
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(sendDeliveryOrderParams.DO_Detail);
                DataTable pDt = JsonConvert.DeserializeObject<DataTable>(json);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDBrandAsal", SqlDbType.Int) { Value = sendDeliveryOrderParams.IDBrandAsal });
                command.Parameters.Add(new SqlParameter("@IDAsal", SqlDbType.Int) { Value = sendDeliveryOrderParams.IDAsal });
                command.Parameters.Add(new SqlParameter("@TipeAsal", SqlDbType.Int) { Value = sendDeliveryOrderParams.TipeAsal });
                command.Parameters.Add(new SqlParameter("@NamaAsal", SqlDbType.VarChar) { Value = sendDeliveryOrderParams.NamaAsal });
                command.Parameters.Add(new SqlParameter("@IDBrandTujuan", SqlDbType.Int) { Value = sendDeliveryOrderParams.IDBrandTujuan });
                command.Parameters.Add(new SqlParameter("@IDTujuan", SqlDbType.Int) { Value = sendDeliveryOrderParams.IDTujuan });
                command.Parameters.Add(new SqlParameter("@TipeTujuan", SqlDbType.Int) { Value = sendDeliveryOrderParams.TipeTujuan });
                command.Parameters.Add(new SqlParameter("@NamaTujuan", SqlDbType.VarChar) { Value = sendDeliveryOrderParams.NamaTujuan });
                command.Parameters.Add(new SqlParameter("@IDLokasiReparasi", SqlDbType.Int) { Value = sendDeliveryOrderParams.IDLokasiReparasi });
                command.Parameters.Add(new SqlParameter("@Kurir", SqlDbType.VarChar) { Value = sendDeliveryOrderParams.Kurir });
                command.Parameters.Add(new SqlParameter("@Keterangan", SqlDbType.VarChar) { Value = sendDeliveryOrderParams.Keterangan });
                command.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar) { Value = App.UserNama });
                command.Parameters.Add(new SqlParameter("@DO_Detail", SqlDbType.Structured) { Value = pDt });
                command.Parameters.Add(new SqlParameter("@IsCrossBrand", SqlDbType.Int) { Value = sendDeliveryOrderParams.IsCrossBrand });

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

    public static DataSet ExecuteGetSendDeliveryOrderRepair(int idlokasi, int tipelokasi, int idlokasireparasi)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getSendDeliveryOrderRepair", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = idlokasi });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = tipelokasi });
                command.Parameters.Add(new SqlParameter("@IDLokasiReparasi", SqlDbType.Int) { Value = idlokasireparasi });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }

    public class ISendDeliveryOrderParams
    {
        public int IDBrandAsal { get; set; }
        public int IDAsal { get; set; }
        public int TipeAsal { get; set; }
        public string NamaAsal { get; set; }
        public int IDBrandTujuan { get; set; }
        public int IDTujuan { get; set; }
        public int TipeTujuan { get; set; }
        public string NamaTujuan { get; set; }
        public int IDLokasiReparasi { get; set; }
        public string Kurir { get; set; }
        public string Keterangan { get; set; }
        public string Operator { get; set; }
        public int IsCrossBrand { get; set; }
        public List<object> DO_Detail { get; set; }
    }

    public class IDeliveryOrderRepairReport
    {
        public int IDDocRepair { get; set; }
        public string NomorReparasi { get; set; }
        public string NamaCustomer { get; set; }
        public string NamaSales { get; set; }
        public string PLU { get; set; }
        public string Image { get; set; }
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
        public string NamaOutlet { get; set; }
        public string KodeOutlet { get; set; }
        public int IDLokasi { get; set; }
        public int TipeLokasi { get; set; }
        public string BrandOutlet { get; set; }
        public int IDBrand { get; set; }
        public string Province { get; set; }
    }
    public class ISalesLocationResponse
    {
        public List<IDataSalesWithLocation> sales { get; set; }
        public List<IDataLocation> location { get; set; }
    }
}