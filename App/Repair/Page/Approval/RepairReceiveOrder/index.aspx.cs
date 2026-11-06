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
        DataTable dt = Db.Rs("SELECT * FROM LocReparasi", Db.Cnn_DB);

        List<ILocRepair> locRepair = new List<ILocRepair>();
        foreach (DataRow dr in dt.Rows)
        {
            ILocRepair loc = new ILocRepair
            {
                ID = (int)dr["ID"],
                Kode = dr["Kode"].ToString(),
                Nama = dr["Nama"].ToString()
            };
            locRepair.Add(loc);
        }

        return locRepair;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IDataLocation> GetLocationTujuan()
    {
        DataTable dtF = Db.Rs("SELECT ID, Nama, Kode, 1 [TipeLokasi], 'F' [Brand], IDBrand FROM [frank_db].[dbo].[LocOutlet] WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB);
        DataTable dtF2 = Db.Rs("SELECT ID, Nama, Kode, 2 [TipeLokasi], 'F' [Brand], IDBrand FROM [frank_db].[dbo].[LocExhibition] WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB);

        DataTable dtP = Db.Rs("SELECT ID, Nama, Kode, 1 [TipeLokasi], 'P' [Brand], IDBrand FROM [palace_db_real].[dbo].[LocOutlet] WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB);
        DataTable dtP2 = Db.Rs("SELECT ID, Nama, Kode, 2 [TipeLokasi], 'P' [Brand], IDBrand FROM [palace_db_real].[dbo].[LocExhibition] WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB);

        DataTable dtM = Db.Rs("SELECT ID, Nama, Kode, 1 [TipeLokasi], 'M' [Brand], IDBrand FROM [mondial_db_real].[dbo].[LocOutlet] WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB);
        DataTable dtM2 = Db.Rs("SELECT ID, Nama, Kode, 2 [TipeLokasi], 'M' [Brand], IDBrand FROM [mondial_db_real].[dbo].[LocExhibition] WHERE Disable = 0 ORDER BY ID", Db.Cnn_DB);

        List<IDataLocation> locations = new List<IDataLocation>();
        foreach (DataRow dr in dtF.Rows)
        {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            location.BrandOutlet = dr["Brand"].ToString();
            location.IDBrand = (int)dr["IDBrand"];
            locations.Add(location);
        }

        foreach (DataRow dr in dtF2.Rows)
        {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            location.BrandOutlet = dr["Brand"].ToString();
            location.IDBrand = (int)dr["IDBrand"];
            locations.Add(location);
        }

        foreach (DataRow dr in dtP.Rows)
        {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            location.BrandOutlet = dr["Brand"].ToString();
            location.IDBrand = (int)dr["IDBrand"];
            locations.Add(location);
        }

        foreach (DataRow dr in dtP2.Rows)
        {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            location.BrandOutlet = dr["Brand"].ToString();
            location.IDBrand = (int)dr["IDBrand"];
            locations.Add(location);
        }

        foreach (DataRow dr in dtM.Rows)
        {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            location.BrandOutlet = dr["Brand"].ToString();
            location.IDBrand = (int)dr["IDBrand"];
            locations.Add(location);
        }

        foreach (DataRow dr in dtM2.Rows)
        {
            IDataLocation location = new IDataLocation();
            location.IDLokasi = (int)dr["ID"];
            location.TipeLokasi = (int)dr["TipeLokasi"];
            location.NamaOutlet = dr["Nama"].ToString();
            location.KodeOutlet = dr["Kode"].ToString();
            location.BrandOutlet = dr["Brand"].ToString();
            location.IDBrand = (int)dr["IDBrand"];
            locations.Add(location);
        }

        return locations;
    }


    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IGetSendDeliveryOrderKRM> GetDeliveryOrderRepairKRM(int IDBrandAsal, int IDAsal, int TipeAsal, int IDBrandTujuan, int IDTujuan, int TipeTujuan, int IDLokasiReparasi, int Status, string TglStart, string TglEnd)
    {
        DataSet ds = ExecuteGetSendDeliveryOrderRepairKRM(new IGetSendDeliveryOrderKRMParams
        {
            IDBrandAsal = IDBrandAsal,
            IDAsal = IDAsal,
            TipeAsal = TipeAsal,
            IDBrandTujuan = IDBrandTujuan,
            IDTujuan = IDTujuan,
            TipeTujuan = TipeTujuan,
            IDLokasiReparasi = IDLokasiReparasi,
            Status = Status,
            TglStart = TglStart,
            TglEnd = TglEnd
        });

        DataTable dt = ds.Tables[0];

        List<IGetSendDeliveryOrderKRM> sendDeliveryOrderKRM = new List<IGetSendDeliveryOrderKRM>();

        foreach (DataRow dr in dt.Rows)
        {
            sendDeliveryOrderKRM.Add(new IGetSendDeliveryOrderKRM
            {
                ID = (int)dr["ID"],
                NomorKirim = dr["NomorKirim"].ToString(),
                IDBrandAsal = (int)dr["IDBrandAsal"],
                IDAsal = (int)dr["IDAsal"],
                TipeAsal = (int)dr["TipeAsal"],
                NamaAsal = dr["NamaAsal"].ToString(),
                IDBrandTujuan = (int)dr["IDBrandTujuan"],
                IDTujuan = (int)dr["IDTujuan"],
                TipeTujuan = (int)dr["TipeTujuan"],
                NamaTujuan = dr["NamaTujuan"].ToString(),
                IsWarehouse = (int)dr["IsWarehouse"],
                Operator = dr["Operator"].ToString(),
                OperatorTgl = dr["OperatorTgl"].ToString(),
                NamaKurir = dr["NamaKurir"].ToString(),
                Keterangan = dr["Keterangan"].ToString(),
            });
        }

        return sendDeliveryOrderKRM;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<IDetailDeliveryOrderRepairReport> GetDetailDeliveryOrder(int ID, int IsWarehouse, int IDBrandAsal)
    {
        string dbName = IDBrandAsal == 7 ? "use [frank_db]" : IDBrandAsal == 5 ? "use [palace_db_real]" : "use [mondial_db_real]";
        DataTable dt = Db.Rs(dbName + " exec sp_getDetailDeliveryOrderRepair_Trm " + ID + ", " + IsWarehouse, Db.Cnn_DB);


        List<IDetailDeliveryOrderRepairReport> detailDeliveryRepairReport = new List<IDetailDeliveryOrderRepairReport>();
        foreach (DataRow dr in dt.Rows)
        {
            detailDeliveryRepairReport.Add(new IDetailDeliveryOrderRepairReport
            {
                IDDocRepair = (int)dr["IDDocRepair"],
                Image = RepairLib.GetPathImage(dr["ImgPicture"].ToString()),
                NamaCustomer = dr["CustomerNama"].ToString(),
                NamaSales = dr["SalesNama"].ToString(),
                NomorReparasi = dr["NomorRepair"].ToString(),
                PLU = dr["PLU"].ToString()
            });
        }

        return detailDeliveryRepairReport;
    }

    [WebMethod]
    public static string ApproveDetailDeliveryOrder(int ID, int IsWarehouse, int IDBrandAsal)
    {
        string dbName = IDBrandAsal == 7 ? "use [frank_db]" : IDBrandAsal == 5 ? "use [palace_db_real]" : "use [mondial_db_real]";
        DataTable dt = Db.Rs(dbName + " exec sp_approveSendDeliveryOrderRepair_Trm " + ID + "," + IsWarehouse + ", '" + App.UserNama + "', " + "1, 0", Db.Cnn_DB);
        return "success";
    }



    public static DataSet ExecuteGetSendDeliveryOrderRepairKRM(IGetSendDeliveryOrderKRMParams sendDeliveryOrderKRMParams)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand("sp_getReceiveDeliveryOrderRepair_Krm", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDBrandAsal", SqlDbType.Int) { Value = sendDeliveryOrderKRMParams.IDBrandAsal });
                command.Parameters.Add(new SqlParameter("@IDAsal", SqlDbType.Int) { Value = sendDeliveryOrderKRMParams.IDAsal });
                command.Parameters.Add(new SqlParameter("@TipeAsal", SqlDbType.Int) { Value = sendDeliveryOrderKRMParams.TipeAsal });
                command.Parameters.Add(new SqlParameter("@IDBrandTujuan", SqlDbType.Int) { Value = sendDeliveryOrderKRMParams.IDBrandTujuan });
                command.Parameters.Add(new SqlParameter("@IDTujuan", SqlDbType.Int) { Value = sendDeliveryOrderKRMParams.IDTujuan });
                command.Parameters.Add(new SqlParameter("@TipeTujuan", SqlDbType.Int) { Value = sendDeliveryOrderKRMParams.TipeTujuan });
                command.Parameters.Add(new SqlParameter("@IDLokasiReparasi", SqlDbType.Int) { Value = sendDeliveryOrderKRMParams.IDLokasiReparasi });
                command.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int) { Value = sendDeliveryOrderKRMParams.Status });
                command.Parameters.Add(new SqlParameter("@TglStart", SqlDbType.VarChar) { Value = sendDeliveryOrderKRMParams.TglStart });
                command.Parameters.Add(new SqlParameter("@TglEnd", SqlDbType.VarChar) { Value = sendDeliveryOrderKRMParams.TglEnd });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }

    public class IGetSendDeliveryOrderKRMParams
    {
        public int IDBrandAsal { get; set; }
        public int IDAsal { get; set; }
        public int TipeAsal { get; set; }
        public int IDBrandTujuan { get; set; }
        public int IDTujuan { get; set; }
        public int TipeTujuan { get; set; }
        public int IDLokasiReparasi { get; set; }
        public string TglStart { get; set; }
        public string TglEnd { get; set; }
        public int Status { get; set; }
    }

    public class IGetSendDeliveryOrderKRM
    {
        public int ID { get; set; }
        public string NomorKirim { get; set; }
        public int IDBrandAsal { get; set; }
        public int IDAsal { get; set; }
        public int TipeAsal { get; set; }
        public string NamaAsal { get; set; }
        public int IDBrandTujuan { get; set; }
        public int IDTujuan { get; set; }
        public int TipeTujuan { get; set; }
        public string NamaTujuan { get; set; }
        public string Operator { get; set; }
        public string OperatorTgl { get; set; }
        public int IsWarehouse { get; set; }
        public string NamaKurir { get; set; }
        public string Keterangan { get; set; }
    }

    public class IDetailDeliveryOrderRepairReport
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
    }
    public class ISalesLocationResponse
    {
        public List<IDataSalesWithLocation> sales { get; set; }
        public List<IDataLocation> location { get; set; }
    }
}