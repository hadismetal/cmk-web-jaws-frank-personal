using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web;
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
        var context = HttpContext.Current;        
        HttpCookie UserNama= context.Request.Cookies["OperatorName"];
        string operatorName = App.UserNama == null ? UserNama.Value : App.UserNama;
        int UserID = App.UserID == null ? Convert.ToInt32(context.Request.Cookies["UserID"].Value) : App.UserID;
        Ddl.Location1(locationtype, UserID);        
        Ddl.ShiftOpname(shift);
        namaJR.Value = operatorName;
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
        var context = HttpContext.Current;
        HttpCookie UserNama= context.Request.Cookies["OperatorName"];
        string operatorName = App.UserNama == null ? UserNama.Value : App.UserNama;
        int UserID = App.UserID == null ? Convert.ToInt32(context.Request.Cookies["UserID"].Value) : App.UserID;

        href += "?loc=" + locationtype.Text;
        href += "&locName=" + locationtype.SelectedItem.Text;
        href += "&namaJR=" + operatorName;
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
        int Total = 0;
        var context = HttpContext.Current;
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {                                    
            HttpCookie UserNama= context.Request.Cookies["OperatorName"];
            string operatorName = App.UserNama == null ? UserNama.Value : App.UserNama;
            int UserID = App.UserID == null ? Convert.ToInt32(context.Request.Cookies["UserID"].Value) : App.UserID;

            string getDisplay = @"            
                DECLARE @IDLokasi1 INT = 0,
				@ONDUTY INT = 0, 
				@TANGGALDUTY DATE, 
				@SHIFTDUTY INT

		SELECT @IDLokasi1 = ISNULL(IDLokasi,0) 
		FROM DataAdminSales 
		WHERE IDUser = @IDUser

		SELECT XA.*, 
			   CONVERT(DATETIME, SYSDATETIMEOFFSET() AT TIME ZONE TimeZone) AS TimeLine
		INTO #TempLoc
		FROM (
			SELECT 1 AS TipeLokasi, ID AS IDLokasi, Nama, IDTimeZone FROM LocOutlet
			UNION
			SELECT 2 AS TipeLokasi, ID AS IDLokasi, Nama, 1 AS IDTimeZone FROM LocExhibition
		) XA
		JOIN TimeZoneLoc x ON x.ID = XA.IDTimeZone

		SELECT IDHeader, COUNT(TipeUser) AS total
		INTO #TempAx
		FROM (
			SELECT IDHeader, TipeUser 
			FROM StockOpname_Display 
			GROUP BY IDHeader, TipeUser
		) axx
		GROUP BY IDHeader

		SELECT
			a.ID,
			IIF(a.TipeLokasi = 0,'Warehouse',
				IIF(a.TipeLokasi = 1, 'Outlet', 'Exhibition')) AS TipeLokasi,
			aa.Nama AS NamaLokasi,
			c.Nama AS KodeShift,
			FORMAT(a.Created_at, 'dddd, dd MMMM yyyy', 'id-ID') AS tgl,
			a.Status AS flagStats,
			IIF(ax.total > 1, 1, 0) AS flagDetail,
			IIF(a.Status = 1, 'Waiting for Approval',
				IIF(a.Status = 2, 'Approved','Draft')) AS Status,
			ISNULL(a.IsSelisihDJ,0) AS IsSelisihDJ,
			ISNULL(a.IsSelisihPG,0) AS IsSelisihPG,
			ISNULL(a.IsSelisihLD,0) AS IsSelisihLD,
			FORMAT(a.Approval_at, 'dddd, dd MMMM yyyy hh\:mm tt', 'id-ID') AS tglApprove,
			a.Approval_by AS Approver,
			IIF(a.Approval_at IS NULL, 2,
				IIF(CAST(a.Approval_at AS DATE) > CAST(a.Created_at AS DATE), 2,
					IIF(FORMAT(a.Approval_at, 'HH:mm') > c.MaxShift, 1, 0)
				)
			) AS IsLate,
            A.Created_at AS Created_at,
			a.KeteranganApprove AS Keterangan
            INTO #ResultData
            FROM StockOpname a
            JOIN #TempLoc aa 
                ON aa.IDLokasi = a.IDLokasi 
            AND aa.TipeLokasi = a.TipeLokasi
            JOIN MasterShiftOpname c ON c.ID = a.KodeShift
            JOIN #TempAx ax ON ax.IDHeader = a.ID
            WHERE
            (
                (@IDLokasi = 0 AND @IDLokasi1 = 0)
                OR (@IDLokasi1 <> 0 AND a.IDLokasi = @IDLokasi1)
                OR (a.IDLokasi = @IDLokasi)
            )
            AND (@TipeLokasi = 9 OR a.TipeLokasi = @TipeLokasi)
            AND (
                    (
                        @ONDUTY = 1 AND a.KodeShift = @SHIFTDUTY AND CAST(a.Created_at AS DATE) = @TANGGALDUTY
                    )
                OR
                    (
                        @ONDUTY = 0 AND (@KodeShift = '0' OR a.KodeShift = @KodeShift)
                    )
                )
            AND CAST(a.Created_at AS DATE) BETWEEN @start AND @end

            SELECT *
            FROM #ResultData
            ORDER BY Created_at DESC
            OFFSET @OFFSET ROWS
            FETCH NEXT @PAGESIZE ROWS ONLY

            SELECT COUNT(*) AS Total 
            FROM #ResultData

            DROP TABLE #TempLoc
            DROP TABLE #TempAx
            DROP TABLE #ResultData
            ";
            using (SqlCommand command = new SqlCommand(getDisplay, connection))
            {

                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = TipeLokasi });
                command.Parameters.Add(new SqlParameter("@KodeShift", SqlDbType.VarChar) { Value = KodeShift });
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = UserID });
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
                        IsSelisihLD = Convert.ToInt32(row["IsSelisihLD"]),
                        Keterangan = row["Keterangan"].ToString()
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
        public string Keterangan { get; set; }
    }
}