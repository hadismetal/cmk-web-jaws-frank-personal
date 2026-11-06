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
    public static List<IGetListRepairServicePoint> GetListRepairServicePoint(int IDLokasi, int TipeLokasi, string KodeBrand, string Keywords)
    {
        DataSet ds = ExecuteGetListRepairServicePoint(IDLokasi, TipeLokasi, KodeBrand, Keywords);
        DataTable dt = ds.Tables[0];
        List<IGetListRepairServicePoint> getListRepairServicePoint = Helper.GetDataFromDataTable<IGetListRepairServicePoint>(dt);
        return getListRepairServicePoint;
    }

    [WebMethod]
    public static string ReturnCancelRepair(int IDDocRepair, int IDBrandAsal)
    {
        string connectionString = Helper.GetConnectionByBrand(IDBrandAsal);

        var status = Db.SingleInteger("SELECT Status FROM DocRepair_Pivot WHERE IDDocRepair = " + IDDocRepair, connectionString);
        if (status != 3)
        {
            return "failed";
        }

        var query = Db.Rs("UPDATE DocRepair_Pivot SET Status = 4, IsCancelRepair = 1 WHERE IDDocRepair = " + IDDocRepair, connectionString);

        string query2 = "EXEC sp_insertLedgerRepair " + IDDocRepair + ", '" + App.UserNama + "'";
        Db.Execute(query2, Db.Cnn_DB);

        return "success";
    }

    public static DataSet ExecuteGetListRepairServicePoint(int IDLokasi, int TipeLokasi, string KodeBrand, string Keywords)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getListRepairServicePoint", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = TipeLokasi });
                command.Parameters.Add(new SqlParameter("@KodeBrand", SqlDbType.VarChar) { Value = KodeBrand });
                command.Parameters.Add(new SqlParameter("@Keywords", SqlDbType.VarChar) { Value = Keywords });
                command.Parameters.Add(new SqlParameter("@IsReport", SqlDbType.VarChar) { Value = 0 });

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
    }
    public class ISalesLocationResponse
    {
        public List<IDataSalesWithLocation> sales { get; set; }
        public List<IDataLocation> location { get; set; }
    }
}