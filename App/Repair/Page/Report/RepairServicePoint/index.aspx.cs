
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
        List<string> kodeLokasiList = new List<string>();
        string getLokasiSC = @"select a.Kode as KodeOutlet from RepairMasterLocationProvince a
                                JOIN RepairMasterLocationProvinceChild b on b.IDRepairMasterLocationProvince = a.ID
                                GROUP BY a.Kode";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            using (SqlCommand cmd = new SqlCommand(getLokasiSC,connection))
            {
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter adapterSC = new SqlDataAdapter(cmd);                
                adapterSC.Fill(ds1);
                DataTable dtSC = ds1.Tables[0];
                kodeLokasiList = dtSC.AsEnumerable()
                .Select(r => r.Field<string>("KodeOutlet"))
                .ToList();
                
            }

            using (SqlCommand command = new SqlCommand("sp_getDefaultLocationAndSalesOrAE", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = App.UserID });
                command.Parameters.Add(new SqlParameter("@IsReport", SqlDbType.Int) { Value = 1 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);
                
                DataTable dt = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];

                var filteredRows = dt.AsEnumerable()
                .Where(r => kodeLokasiList.Contains(r.Field<string>("KodeOutlet")));
                DataTable filteredDt = filteredRows.Any() ? filteredRows.CopyToDataTable() : dt.Clone();

                List<IDataLocation> dataLocation = Helper.GetDataFromDataTable<IDataLocation>(filteredDt);
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
    public static List<IGetListRepairServicePoint> GetListRepairServicePoint(int IDLokasi, int TipeLokasi, string KodeBrand, string Keywords, string TglAwal, string TglHingga)
    {
        DataSet ds = ExecuteGetListRepairServicePoint(IDLokasi, TipeLokasi, KodeBrand, Keywords, TglAwal,TglHingga);

        DataTable dt = ds.Tables[0];

        List<IGetListRepairServicePoint> getListRepairServicePoint = new List<IGetListRepairServicePoint>();

        foreach (DataRow dr in dt.Rows)
        {
            getListRepairServicePoint.Add(new IGetListRepairServicePoint
            {
                No = (int)dr["No"],
                ID = (int)dr["ID"],
                NamaCustomer = dr["NamaCustomer"].ToString(),
                IDBrandAsal = (int)dr["IDBrandAsal"],
                NamaAsalOutlet = dr["NamaAsal"].ToString(),
                NamaSales = dr["NamaSales"].ToString(),
                Nomor = dr["Nomor"].ToString(),
                PLU = dr["PLU"].ToString(),
                Finish = dr["Finish"].ToString(),
                FinishTgl = dr["FinishTgl"].ToString(),
                Edit = dr["Edit"].ToString(),
                EditTgl = dr["EditTgl"].ToString()
            });
        }

        return getListRepairServicePoint;
    }


    public static DataSet ExecuteGetListRepairServicePoint(int IDLokasi, int TipeLokasi, string KodeBrand, string Keywords, string TglAwal, string TglHingga)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getListRepairServicePoint_Report", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = TipeLokasi });
                command.Parameters.Add(new SqlParameter("@KodeBrand", SqlDbType.VarChar) { Value = KodeBrand });
                command.Parameters.Add(new SqlParameter("@Keywords", SqlDbType.VarChar) { Value = Keywords });
                command.Parameters.Add(new SqlParameter("@IsReport", SqlDbType.Int) { Value = 1 });
                command.Parameters.Add(new SqlParameter("@TglAwal", SqlDbType.VarChar) { Value = TglAwal });
                command.Parameters.Add(new SqlParameter("@TglHingga", SqlDbType.VarChar) { Value = TglHingga });

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
        public string NamaAsalOutlet { get; set; }        
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