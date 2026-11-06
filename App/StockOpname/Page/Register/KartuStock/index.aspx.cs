using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Services;
using System.Reflection.Metadata;

public partial class Page_Summary : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
        Ddl.Location1(locationtype, App.UserID);
        namaJR.Value = App.UserNama;            
        // Ddl.ShiftOpname(shift);

		if (!IsPostBack) {
		}
	}

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = "KartuStock.aspx";

        // var dj = from p in db.StockProductDJ
        // where p.ReturnProduct == false && p.StatusPenjualan == 0
        // select p;

        // Response.Write(App.UserNama);

        href += "?loc=" + locationtype.Text;
        href += "&locName=" + locationtype.SelectedItem.Text;
        href += "&namaJR=" + App.UserNama;
        // href += "&shift=" + shift.Text;
        Response.Redirect(href);
    }
    // [WebMethod]
    // [ScriptMethod(UseHttpGet = true)]
    // public static List<DataDisplay> GetDisplayFront(int IDLokasi)
    // {

    //     List<DataDisplay> DataMeja = new List<DataDisplay>();

    //     using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
    //     {
    //         string getDisplay = @"
    //             SELECT ID as IDTable, NamaDisplay 
    //             FROM MasterDisplayOutlet 
    //             WHERE IDLokasi = @IDLokasi AND Status = 1
    //         ";
    //         using (SqlCommand command = new SqlCommand(getDisplay, connection))
    //         {
    //             command.CommandType = CommandType.Text;
    //             command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });

    //             SqlDataAdapter adapter = new SqlDataAdapter(command);
    //             DataSet ds = new DataSet();
    //             adapter.Fill(ds);

    //             foreach (DataRow row in ds.Tables[0].Rows)
    //             {
    //                 DataDisplay display = new DataDisplay
    //                 {
    //                     ID = Convert.ToInt32(row["IDTable"]),
    //                     NamaDisplay = row["NamaDisplay"].ToString()
    //                 };
    //                 DataMeja.Add(display);
    //             }

    //         }
    //         return DataMeja;
    //     }
    // }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static int CheckUser(int Shift)
    {
        int typeUser = 0;
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(@"
            declare @IsUserValid int = 0;
                IF EXISTS(SELECT 1 TipeJabatan FROM MasterJabatanPenggantiOpname WHERE CAST(GETDATE() AS DATE) = EfektifDate AND IDUser = @IDUser and TipeShift = @Shift)
                BEGIN
                    SELECT @IsUserValid = 1;
                END
                IF EXISTS (SELECT 1 FROM frank_ac.dbo.UserAccount a
                    JOIN [DB_PROD_MYAPPS].dbhrd.dbo.mskaryawan b on b.m_nik COLLATE DATABASE_DEFAULT = a.UserNumber COLLATE DATABASE_DEFAULT
                    JOIN DataAdminSales c on c.IDUser=a.ID where a.ID = @IDUser and a.status = 1 
                    and (m_jabatan LIKE '%STORE MANAGER%' OR m_jabatan like '%ADMINISTRATIVE EXECUTIVE%' or m_jabatan like '%JEWELLERY COORDINATOR%'))
                BEGIN
                   SELECT @IsUserValid = 1;
                END
                SELECT @IsUserValid as IsUserValid;
            ", connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = App.UserID });            
                command.Parameters.Add(new SqlParameter("@Shift", SqlDbType.Int) { Value = Shift });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);                
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    typeUser = Convert.ToInt32(dt.Rows[0]["IsUserValid"]);
                }
            }
        }
        return typeUser;
    }
    
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<DataDisplay> GetMasterTable(int IDLokasi)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(@"            
            SELECT 
                MIN(ID) AS ID,
                CASE 
                    WHEN PATINDEX('%[0-9]%', NamaDisplay) > 0
                    THEN LEFT(NamaDisplay, PATINDEX('%[0-9]%', NamaDisplay) - 1)
                    ELSE NamaDisplay
                END AS NamaDisplay
            FROM MasterDisplayOutlet
            WHERE IDLokasi = @IDLokasi 
            AND Status = 1
            GROUP BY
                CASE 
                    WHEN PATINDEX('%[0-9]%', NamaDisplay) > 0
                    THEN LEFT(NamaDisplay, PATINDEX('%[0-9]%', NamaDisplay) - 1)
                    ELSE NamaDisplay
                END    
            ", connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });            

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                List<DataDisplay> DataDis = Helper.GetDataFromDataTable<DataDisplay>(ds.Tables[0]);
                return DataDis;
            }
        }
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<DataShift> GetMasterShift(int IDLokasi)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(@"
            DECLARE @IsShift int =0 --0 pagi buka, 1 siang buka, 2 malam buka
            
            SELECT @IsShift = COUNT(*) FROM StockOpname 
            where IDLokasi = @IDLokasi and Approval_at IS NOT NULL
            AND CAST(Created_at AS DATE) = CAST(GETDATE() AS DATE)


            select ID, Nama from MasterShiftOpname            
             WHERE ((@IsShift = 0 and ID=1) OR (@IsShift = 1 and ID=2) OR (@IsShift = 2 and ID=3))"
             
             , connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });            

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                List<DataShift> DataDis = Helper.GetDataFromDataTable<DataShift>(ds.Tables[0]);
                return DataDis;
            }
        }
    }


    public class DataDisplay
    {        
        public int ID { get; set; }
        public string NamaDisplay { get; set; }
    }
    public class DataShift
    {
        public int ID { get; set; }
        public string Nama { get; set; }
    }
}