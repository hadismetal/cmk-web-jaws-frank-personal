using System;
using System.Web;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Services;

public partial class Page_MasterJabatanPengganti : System.Web.UI.Page {
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
        Ddl.Location(locationtype, App.UserID);                
        namaJR.Value = App.UserNama;
		if (!IsPostBack) {
		}
	}

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = "KartuStock.aspx";

        href += "?loc=" + locationtype.Text;
        href += "&locName=" + locationtype.SelectedItem.Text;
        href += "&namaJR=" + App.UserNama;        
        Response.Redirect(href);
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<DataDisplay> GetDisplayFront(int IDLokasi)
    {

        List<DataDisplay> DataMeja = new List<DataDisplay>();

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string getDisplay = @"
                SELECT ID as IDTable, NamaDisplay FROM MasterDisplayOutlet 
                WHERE IDLokasi = @IDLokasi AND Status = 1
            ";
            using (SqlCommand command = new SqlCommand(getDisplay, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataDisplay display = new DataDisplay
                    {
                        ID = Convert.ToInt32(row["IDTable"]),
                        NamaDisplay = row["NamaDisplay"].ToString()
                    };
                    DataMeja.Add(display);
                }

            }
            return DataMeja;
        }
    }
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<DataJr> GetData(int IDLokasi, string TglStart, string TglEnd)
    {
        List<DataJr> DataTampung = new List<DataJr>();

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string getDisplay = @"                
                CREATE TABLE #tempLoc(
                    TipeLokasi INT,
                    IDLokasi INT ,
                    NamaLokasi varchar(100)
                )

                IF DB_NAME() LIKE '%PALACE%'
                BEGIN
                    INSERT INTO #tempLoc
                    SELECT 1, ID, Nama FROM palace_db_real.dbo.LocOutlet

                    INSERT INTO #tempLoc
                    SELECT 2, ID, Nama FROM palace_db_real.dbo.LocExhibition

                    select a.ID, A.IDUser, a.TipeLokasi, a.IDLokasi, NamaLokasi, aa.Nama, 
                    Convert(varchar(10), EfektifDate, 103) EfektifDate, ab.Nama as NamaShift, A.TipeJabatan,
                    IIF(CAST(GETDATE() AS DATE) > CAST(EfektifDate AS DATE), 0, 1) AS Status
                    from MasterJabatanPenggantiOpname A
                    JOIN #tempLoc b on b.IDLokasi=a.IDLokasi and b.TipeLokasi=a.TipeLokasi
                    JOIN palace_ac.dbo.UserAccount aa on aa.ID=a.IDUser
                    JOIN MasterShiftOpname ab on ab.id=a.TipeShift
                    WHERE (@IDLokasi = 0 OR A.IDLokasi = @IDLokasi) 
                    AND A.EfektifDate BETWEEN CAST(@TglStart AS DATE) AND CAST(@TglEnd AS DATE)
                END
                ELSE IF DB_NAME() LIKE '%FRANK%'
                BEGIN
                    INSERT INTO #tempLoc
                    SELECT 1, ID, Nama FROM frank_db.dbo.LocOutlet

                    INSERT INTO #tempLoc
                    SELECT 2, ID, Nama FROM frank_db.dbo.LocExhibition


                    select a.ID, A.IDUser, a.TipeLokasi, a.IDLokasi, NamaLokasi, aa.Nama, Convert(varchar(10), EfektifDate, 103) EfektifDate, ab.Nama as NamaShift, A.TipeJabatan,
                    IIF(CAST(GETDATE() AS DATE) > CAST(EfektifDate AS DATE), 0, 1) AS Status
                    from MasterJabatanPenggantiOpname A
                    JOIN #tempLoc b on b.IDLokasi=a.IDLokasi and b.TipeLokasi=a.TipeLokasi
                    JOIN frank_ac.dbo.UserAccount aa on aa.ID=a.IDUser
                    JOIN MasterShiftOpname ab on ab.id=a.TipeShift
                    WHERE (@IDLokasi = 0 OR A.IDLokasi = @IDLokasi) 
                    AND A.EfektifDate BETWEEN CAST(@TglStart AS DATE) AND CAST(@TglEnd AS DATE)
                END
                ELSE IF DB_NAME() LIKE '%MONDIAL%'
                BEGIN
                    INSERT INTO #tempLoc
                    SELECT 1, ID, Nama FROM mondial_db_real.dbo.LocOutlet

                    INSERT INTO #tempLoc
                    SELECT 2, ID, Nama FROM mondial_db_real.dbo.LocExhibition

                    select a.ID, A.IDUser, a.TipeLokasi, a.IDLokasi, NamaLokasi, aa.Nama, Convert(varchar(10), EfektifDate, 103) EfektifDate, ab.Nama as NamaShift, A.TipeJabatan,
                    IIF(CAST(GETDATE() AS DATE) > CAST(EfektifDate AS DATE), 0, 1) AS Status
                    from MasterJabatanPenggantiOpname A
                    JOIN #tempLoc b on b.IDLokasi=a.IDLokasi and b.TipeLokasi=a.TipeLokasi
                    JOIN mondial_ac.dbo.UserAccount aa on aa.ID=a.IDUser
                    JOIN MasterShiftOpname ab on ab.id=a.TipeShift
                    WHERE (@IDLokasi = 0 OR A.IDLokasi = @IDLokasi) 
                    AND A.EfektifDate BETWEEN CAST(@TglStart AS DATE) AND CAST(@TglEnd AS DATE)
                END
                ELSE IF DB_NAME() LIKE '%LEI%'
                BEGIN
                    INSERT INTO #tempLoc
                    SELECT 1, ID, Nama FROM lei_db.dbo.LocOutlet

                    INSERT INTO #tempLoc
                    SELECT 2, ID, Nama FROM lei_db.dbo.LocExhibition

                    select a.ID, A.IDUser, a.TipeLokasi, a.IDLokasi, NamaLokasi, aa.Nama, Convert(varchar(10), EfektifDate, 103) EfektifDate, ab.Nama as NamaShift, A.TipeJabatan,
                    IIF(CAST(GETDATE() AS DATE) > CAST(EfektifDate AS DATE), 0, 1) AS Status
                    from MasterJabatanPenggantiOpname A
                    JOIN #tempLoc b on b.IDLokasi=a.IDLokasi and b.TipeLokasi=a.TipeLokasi
                    JOIN lei_ac.dbo.UserAccount aa on aa.ID=a.IDUser
                    JOIN MasterShiftOpname ab on ab.id=a.TipeShift
                    WHERE (@IDLokasi = 0 OR A.IDLokasi = @IDLokasi) 
                    AND A.EfektifDate BETWEEN CAST(@TglStart AS DATE) AND CAST(@TglEnd AS DATE)
                END

                DROP TABLE #tempLoc
            ";
            
            using (SqlCommand command = new SqlCommand(getDisplay, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@TglStart", SqlDbType.Date) { Value = TglStart });
                command.Parameters.Add(new SqlParameter("@TglEnd", SqlDbType.Date) { Value = TglEnd });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataJr Data = new DataJr
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        IDUser = Convert.ToInt32(row["IDUser"]),
                        IDLokasi = Convert.ToInt32(row["IDLokasi"]),
                        NamaLokasi = row["NamaLokasi"].ToString(),
                        EfektifDate = row["EfektifDate"].ToString(),
                        NamaShift = row["NamaShift"].ToString(),
                        Nama = row["Nama"].ToString(),                        
                        TipeJabatan = row["TipeJabatan"].ToString(),
                        Status = Convert.ToInt32(row["Status"])
                    };
                    DataTampung.Add(Data);
                }

            }
            return DataTampung;
        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<DataDisplay> GetMasterTable(int IDLokasi)
    {
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(@"select ID, NamaDisplay from MasterDisplayOutlet WHERE IDLokasi = @IDLokasi", connection))
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
    public static List<ResponseMessage> UpdateData(int ID, int IDUser)
    {
        var list = new List<ResponseMessage>();
        DataSet ds = new DataSet();
        var context = HttpContext.Current;
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {                                    
            HttpCookie UserNama= context.Request.Cookies["OperatorName"];
            string operatorName = App.UserNama == null ? UserNama.Value : App.UserNama;            
            connection.Open();
            string query = @"
                DECLARE @TimeZone DATETIME, @STATUSOP INT = 0
                SELECT  @TimeZone = CONVERT(DATETIME, SYSDATETIMEOFFSET() AT TIME ZONE TimeZone) 
                FROM (
                    SELECT 1 AS TipeLokasi,ID AS IDLokasi,Nama,IDTimeZone FROM LocOutlet
                    UNION
                    SELECT 2 AS TipeLokasi,ID AS IDLokasi,Nama,1 AS IDTimeZone FROM LocExhibition
                )XA
                JOIN TimeZoneLoc x on x.ID=XA.IDTimeZone
                JOIN MasterJabatanPenggantiOpname ML ON ML.TipeLokasi=XA.TipeLokasi and ML.IDLokasi=XA.IDLokasi
                WHERE ML.ID=@ID

                select @STATUSOP = count(*) from masterjabatanpenggantiopname a
                JOIN StockOpname B ON CAST(b.Created_at as date) = A.EfektifDate AND B.IDLokasi=A.IDLokasi AND B.TipeLokasi=A.TipeLokasi AND B.KodeShift=A.TipeShift
                WHERE (b.NomorHeader is NULL AND CAST(a.EfektifDate as DATE) = CAST(GETDATE() AS DATE)) and a.ID=@ID


                IF @STATUSOP > 0
                BEGIN                
                    IF EXISTS(SELECT 1 FROM MasterJabatanPenggantiOpname WHERE ID=@ID)
                    BEGIN
                        UPDATE TOP (1) A SET IDUser=@IDUser, Updated_at=@TimeZone, Updated_by=@Operator
                        OUTPUT INSERTED.ID, DELETED.IDUser, INSERTED.Updated_at, INSERTED.Updated_by
                        INTO MasterJabatanPenggantiOpname_Log (IDMasterJabatanPenggantiOpname, IDUser, Created_at, Created_by)
                        FROM (
                            SELECT ID, IDUser, Updated_at, Updated_by FROM MasterJabatanPenggantiOpname WHERE ID=@ID
                        )A
                        SELECT 200 AS Status, 'Data updated successfully' AS Message
                    END
                    ELSE
                    BEGIN
                        SELECT 404 AS Status, 'Data not found' AS Message
                    END
                END
                ELSE
                BEGIN
                    SELECT 404 AS Status, 'Tugas JR on Duty sudah selesai!!' AS Message
                END
            ";
            try
            {
                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = ID });
                    command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = IDUser });
                    command.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar) { Value = operatorName });

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    adapter.Fill(ds);
                    DataTable dataJR = ds.Tables[0];                

                    foreach (DataRow row in dataJR.Rows)
                    {
                        list.Add(new ResponseMessage
                        {
                            Status = Convert.ToInt32(row["Status"]),
                            Message = Convert.ToString(row["Message"])
                        });
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                list.Add(new ResponseMessage
                {
                    Status = Convert.ToInt32(500),
                    Message = Convert.ToString(ex.Message)
                });                
            }
            connection.Close();
        }
        return list;
    }
    [WebMethod]
    public static List<DataListJR> GetListJR(int IDLokasi, int IDUser)
    {
        var list = new List<DataListJR>();
        DataSet ds = new DataSet();
        using(SqlConnection  connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            string query = @"select IDUser, a.Nama 
                            from DataAdminSales a
                            JOIN frank_ac.dbo.useraccount b on b.ID=A.IDUser                            
                            JOIN DB_PROD_MYAPPS.dbhrd.dbo.mskaryawan c on c.m_nik COLLATE SQL_Latin1_General_CP1_CI_AS = b.UserNumber COLLATE SQL_Latin1_General_CP1_CI_AS
                            where IDLokasi = @IDLokasi and TipeLokasi=@TipeLokasi and Disable=0 and draft=0 
                            and (IDUser <> 0  or IDUser is not null) 
                            and (C.m_jabatan LIKE '%JEWELLERY REPRESENTATIVE%')
                            GROUP BY IDUser, a.Nama";
            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = 1 });

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(ds);
                DataTable dataJR = ds.Tables[0];                

                foreach (DataRow row in dataJR.Rows)
                {
                    list.Add(new DataListJR
                    {
                        IDUser = Convert.ToInt32(row["IDUser"]),
                        Nama = Convert.ToString(row["Nama"])
                    });
                }

                return list;
            }
            connection.Close();
        }
        return list;
    }

    public class DataListJR
    {
        public int IDUser { get; set; }
        public string Nama { get; set; }
    }

    public class DataJr
    {
        public int ID { get; set; }
        public int IDUser { get; set; }
        public int IDLokasi { get; set; }
        public string Nama { get; set; }
        public string NamaLokasi { get; set; }
        public string EfektifDate { get; set; }        
        public string TipeJabatan { get; set; }
        public string NamaShift { get; set; }
        public int Status { get; set; }
    }
    public class DataDisplay
    {
        public int ID { get; set; }
        public string NamaDisplay { get; set; }
    }
    
    public class DataOpname
    {
        public int ID { get; set; }
        public string TipeLokasi { get; set; }
        public string NamaLokasi { get; set; }
        public string KodeShift { get; set; }
        public string tgl { get; set; }
        public int flagStats { get; set; }
        public int flagDetail { get; set; }
        public string Status { get; set; }
        public string tglApprove { get; set; }
        public string Approver { get; set; }
        public int IsLate { get; set; }
        public int IsSelisihDJ { get; set; }
        public int IsSelisihPG { get; set; }
        public int IsSelisihLD { get; set; }
        public string Keterangan { get; set; }
    }
    public class ResponseMessage
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }
}