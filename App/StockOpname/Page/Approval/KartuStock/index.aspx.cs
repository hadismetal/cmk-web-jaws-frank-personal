using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Services;

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
        Ddl.ShiftOpname(shift);
        namaJR.Value = App.UserNama;
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
        href += "&shift=" + shift.Text;
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
    public static object GetDataOpname(int IDLokasi, int TipeLokasi, string KodeShift,string start, string end,  int PageSize, int PageStart)
    {
        List<DataOpname> DataTampung = new List<DataOpname>();
        int OFFSET = (PageStart <= 0 ? 1 : PageStart -1) * PageSize;
         
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string getDisplay = @"                            
                declare @IDLokasi1 int = 0, @ONDUTY INT = 0, @TANGGALDUTY DATE, @SHIFTDUTY INT
                
                SELECT @ONDUTY = 1, @TANGGALDUTY = EfektifDate, @SHIFTDUTY = TipeShift FROM MasterJabatanPenggantiOpname where IDUser = @IDUser AND EfektifDate between @start and @end
                SELECT @IDLokasi1 = ISNULL(IDLokasi,0) FROM DataAdminSales where IDUser = @IDUser

                SELECT XA.*, CONVERT(DATETIME, SYSDATETIMEOFFSET() AT TIME ZONE TimeZone) AS TimeLine
                INTO #TempLoc 
                FROM (
                    SELECT 1 AS TipeLokasi,ID AS IDLokasi,Nama,IDTimeZone FROM LocOutlet
                    UNION
                    SELECT 2 AS TipeLokasi,ID AS IDLokasi,Nama,1 AS IDTimeZone FROM LocExhibition
                )XA
                JOIN TimeZoneLoc x on x.ID=XA.IDTimeZone

                select a.ID, IIF(a.TipeLokasi = 0,'Warehouse', IIF(a.TipeLokasi = 1, 'Outlet', 'Exhibition')) TipeLokasi, aa.Nama as NamaLokasi, c.Nama KodeShift
                ,FORMAT(a.Created_at, 'dddd, dd MMMM yyyy', 'id-ID') tgl,a.Status as flagStats, IIF(ax.total > 1, 1, 0) flagDetail
                ,IIF(Status = 1, 'Waiting for Approval', IIF(Status = 2, 'Approved','Draft')) Status
                , ISNULL(a.IsSelisihDJ,0) IsSelisihDJ, ISNULL(a.IsSelisihPG, 0) IsSelisihPG, ISNULL(a.IsSelisihLD, 0) IsSelisihLD
                , FORMAT(a.Approval_at, 'dddd, dd MMMM yyyy hh\:mm tt', 'id-ID') tglApprove, a.Approval_by as Approver                
                , IIF(a.Approval_at is null, 2, IIF(CAST(a.Approval_at AS DATE) > CAST(a.Created_at as date), 2,IIF(FORMAT(A.Approval_at, 'HH:mm') > c.MaxShift, 1, 0))) AS IsLate
                from StockOpname a 
                JOIN #TempLoc aa on aa.IDLokasi=a.IDLokasi and aa.TipeLokasi=a.TipeLokasi                
                JOIN StockOpname_Display b on b.IDHeader = a.ID
				JOIN MasterShiftOpname c on c.ID=a.KodeShift
				JOIN (select IDHeader, COUNT(TipeUser) total from (SELECT IDHeader, TipeUser from StockOpname_Display group by IDHeader, TipeUser)axx GROUP BY IDHeader) ax ON AX.IDHeader=A.ID
                WHERE (@IDLokasi = 0 and @IDLokasi1 = 0 OR @IDLokasi1 <> 0 and a.IDLokasi = @IDLokasi1 OR a.IDLokasi = @IDLokasi) and (@TipeLokasi = 9 or a.TipeLokasi = @TipeLokasi)
                AND ((@ONDUTY = 1 AND a.KodeShift in (select TipeShift FROM MasterJabatanPenggantiOpname where IDUser = @IDUser AND EfektifDate between @start and @end) AND CAST(A.Created_at as date) = @TANGGALDUTY) OR (@ONDUTY = 0 AND (@KodeShift = '0' OR a.KodeShift = @KodeShift)))
                and CAST(a.Created_at AS DATE) BETWEEN @start and @end
                GROUP BY a.ID,a.TipeLokasi, a.IDLokasi, aa.nama,a.Created_at,a.Status, a.Approval_at, a.Approval_by, c.Nama, c.MaxShift, a.IsSelisihDJ, a.IsSelisihPG, a.IsSelisihLD, ax.total
                ORDER BY a.Created_at DESC
                OFFSET @OFFSET ROWS
                FETCH NEXT @PAGESIZE ROWS ONLY
                


                select count(*) Total from (
                    select a.ID, IIF(a.TipeLokasi = 0,'Warehouse', IIF(a.TipeLokasi = 1, 'Outlet', 'Exhibition')) TipeLokasi, aa.Nama as NamaLokasi, c.Nama KodeShift
                    ,FORMAT(a.Created_at, 'dddd, dd MMMM yyyy', 'id-ID') tgl,a.Status as flagStats, IIF(ax.total > 1, 1, 0) flagDetail
                    ,IIF(Status = 1, 'Waiting for Approval', IIF(Status = 2, 'Approved','Draft')) Status
                    , ISNULL(a.IsSelisihDJ,0) IsSelisihDJ, ISNULL(a.IsSelisihPG, 0) IsSelisihPG, ISNULL(a.IsSelisihLD, 0) IsSelisihLD
                    , FORMAT(a.Approval_at, 'dddd, dd MMMM yyyy hh\:mm tt', 'id-ID') tglApprove, a.Approval_by as Approver                
                    , IIF(a.Approval_at is null, 2, IIF(CAST(a.Approval_at AS DATE) > CAST(a.Created_at as date), 2,IIF(FORMAT(A.Approval_at, 'HH:mm') > c.MaxShift, 1, 0))) AS IsLate
                    from StockOpname a 
                    JOIN #TempLoc aa on aa.IDLokasi=a.IDLokasi and aa.TipeLokasi=a.TipeLokasi                
                    JOIN StockOpname_Display b on b.IDHeader = a.ID
                    JOIN MasterShiftOpname c on c.ID=a.KodeShift
                    JOIN (select IDHeader, COUNT(TipeUser) total from (SELECT IDHeader, TipeUser from StockOpname_Display group by IDHeader, TipeUser)axx GROUP BY IDHeader) ax ON AX.IDHeader=A.ID
                    WHERE (@IDLokasi = 0 and @IDLokasi1 = 0 OR @IDLokasi1 <> 0 and a.IDLokasi = @IDLokasi1 OR a.IDLokasi = @IDLokasi) and (@TipeLokasi = 9 or a.TipeLokasi = @TipeLokasi)
                    AND ((@ONDUTY = 1 AND a.KodeShift in (select TipeShift FROM MasterJabatanPenggantiOpname where IDUser = @IDUser AND EfektifDate between @start and @end) AND CAST(A.Created_at as date) = @TANGGALDUTY) OR (@ONDUTY = 0 AND (@KodeShift = '0' OR a.KodeShift = @KodeShift)))
                    and CAST(a.Created_at AS DATE) BETWEEN @start and @end
                    GROUP BY a.ID,a.TipeLokasi, a.IDLokasi, aa.nama,a.Created_at,a.Status, a.Approval_at, a.Approval_by, c.Nama, c.MaxShift, a.IsSelisihDJ, a.IsSelisihPG, a.IsSelisihLD, ax.total
                )xxa

                DROP TABLE #TempLoc
            ";
            int Total = 0;
            using (SqlCommand command = new SqlCommand(getDisplay, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = TipeLokasi });
                command.Parameters.Add(new SqlParameter("@KodeShift", SqlDbType.VarChar) { Value = KodeShift });
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = App.UserID });
                command.Parameters.Add(new SqlParameter("@start", SqlDbType.VarChar) { Value = start });
                command.Parameters.Add(new SqlParameter("@end", SqlDbType.VarChar) { Value = end });                
                command.Parameters.Add(new SqlParameter("@OFFSET", SqlDbType.Int) { Value = OFFSET });
                command.Parameters.Add(new SqlParameter("@PAGESIZE", SqlDbType.Int) { Value = PageSize });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable tot = ds.Tables[1];
                foreach (DataRow item in tot.Rows)
                {
                    Total = Convert.ToInt32(item["Total"]);
                }                

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DataOpname Data = new DataOpname
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        TipeLokasi = row["TipeLokasi"].ToString(),
                        NamaLokasi = row["NamaLokasi"].ToString(),
                        KodeShift = row["KodeShift"].ToString(),
                        tgl = row["tgl"].ToString(),
                        flagStats = Convert.ToInt32(row["flagStats"]),
                        flagDetail = Convert.ToInt32(row["flagDetail"]),
                        Status = row["Status"].ToString(),
                        tglApprove = row["tglApprove"].ToString(),
                        Approver = row["Approver"].ToString(),
                        IsLate = Convert.ToInt32(row["IsLate"]),
                        IsSelisihDJ = Convert.ToInt32(row["IsSelisihDJ"]),
                        IsSelisihPG = Convert.ToInt32(row["IsSelisihPG"]),
                        IsSelisihLD = Convert.ToInt32(row["IsSelisihLD"])
                    };
                    DataTampung.Add(Data);
                }

            }
            return new
            {
                DataTampung,
                Total
            };
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
    }
}