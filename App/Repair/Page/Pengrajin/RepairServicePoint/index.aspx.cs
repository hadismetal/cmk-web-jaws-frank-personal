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
        // App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static object GetLocationPengrajin()
    {
        DataSet ds = ExecuteGetLocationPengrajin();
        DataTable dt = ds.Tables[0];
        DataTable dt2 = ds.Tables[1];

        List<IDataLocation> locations = new List<IDataLocation>();
        foreach (DataRow dr in dt.Rows)
        {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            location.IDBrand = (int)dr["IDBrand"];
            locations.Add(location);
        }



        string Message = "";
        foreach (DataRow dr in dt2.Rows)
        {
            Message = dr["Message"].ToString();
        }

        return new
        {
            location = locations,
            message = Message
        };
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IGetListRepairServicePoint> GetListRepairServicePoint(int IDLokasi, int TipeLokasi, string KodeBrand, string Keywords)
    {
        DataSet ds = ExecuteGetListRepairServicePoint(IDLokasi, TipeLokasi, KodeBrand, Keywords);
        DataTable dt = ds.Tables[0];
        List<IGetListRepairServicePoint> getListRepairServicePoint = Helper.GetDataFromDataTable<IGetListRepairServicePoint>(dt);
        return getListRepairServicePoint;
    }

    [WebMethod]
    public static string UpdateStatusRepairServicePoint(int IDDocRepair, int Status, int IDBrandAsal)
    {

        string connectionString = Helper.GetConnectionByBrand(IDBrandAsal);
        using (SqlConnection connection = new SqlConnection(connectionString))
        {

            using (SqlCommand command = new SqlCommand("sp_updateProsesPengrajin", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = App.UserID });
                command.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int) { Value = Status });
                command.Parameters.Add(new SqlParameter("@IDDocRepair", SqlDbType.Int) { Value = IDDocRepair });
                command.Parameters.Add(new SqlParameter("@BrandCode", SqlDbType.VarChar) { Value = "F" });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return "Success mengubah status!";
            }
        }
    }


    public static DataSet ExecuteGetListRepairServicePoint(int IDLokasi, int TipeLokasi, string KodeBrand, string Keywords)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getListRepairServicePointPengrajin", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = TipeLokasi });
                command.Parameters.Add(new SqlParameter("@KodeBrand", SqlDbType.VarChar) { Value = KodeBrand });
                command.Parameters.Add(new SqlParameter("@Keywords", SqlDbType.VarChar) { Value = Keywords });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }
    public static DataSet ExecuteGetLocationPengrajin()
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getPengrajinLocation", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = App.UserID });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }

    public class IGetListRepairServicePoint
    {
        public int No { get; set; }
        public int ID { get; set; }
        public string Nomor { get; set; }
        public int IDBrandAsal { get; set; }
        public string PLU { get; set; }
        public string NamaCustomer { get; set; }
        public string NamaSales { get; set; }
        public string Finish { get; set; }
        public string FinishTgl { get; set; }
        public string Edit { get; set; }
        public string EditTgl { get; set; }
        public string OperatorTerima { get; set; }
        public string NIKTerima { get; set; }
        public string TglTerima { get; set; }
        public string OperatorProses { get; set; }
        public string NIKProses { get; set; }
        public string TglProses { get; set; }
        public string OperatorFinish { get; set; }
        public string NIKFinish { get; set; }
        public string TglFinish { get; set; }

        public int Status { get; set; }
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
        public int IDBrand { get; set; }
    }
    public class ISalesLocationResponse
    {
        public List<IDataSalesWithLocation> sales { get; set; }
        public List<IDataLocation> location { get; set; }
    }
}