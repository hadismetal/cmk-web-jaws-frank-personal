using System;
using System.Web;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Services;

public partial class Page_MasterJabatanPengganti : System.Web.UI.Page
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
    protected int idlokasi { get { return App.QueryInt(this, "idlokasi"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        // App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            // Ddl.Location(location, App.UserID);
            IDLokasi.Value = idlokasi.ToString();

            //datepicker.Text = Str.Tgl(DateTime.Now);
            //datepicker2.Text = Str.Tgl(DateTime.Now);

            //Generate            
        }
    }

    //Display Table
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
                        
            // location.SelectedValue = Request.QueryString["location"];            
            //datepicker.Text = Request.QueryString["dari"];
            //datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    [WebMethod]
    public static List<DataShift> GetMasterShift()
    {
        var list = new List<DataShift>();
        DataSet ds = new DataSet();
        using(SqlConnection  connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            string query = @"
            SELECT ID, Nama, MaxShift FROM MasterShiftOpname";
            using(SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(ds);
                DataTable dataShift = ds.Tables[0];                

                foreach (DataRow row in dataShift.Rows)
                {
                    list.Add(new DataShift
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Nama = Convert.ToString(row["Nama"]),
                        MaxShift = Convert.ToString(row["MaxShift"])
                    });
                }

                return list;
            }
            connection.Close();
        }
        return list;
    }

    [WebMethod]
    public static List<DataJR> GetListJR(int IDLokasi)
    {
        var list = new List<DataJR>();
        DataSet ds = new DataSet();
        using(SqlConnection  connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            string query = @"
                            select * into #tempLoc
                            from (
                                SELECT 1 AS TipeLokasi, ID AS IDLokasi, Nama as NamaLokasi, Kode FROM LocOutlet where Disable = 0
                                union
                                SELECT 2 AS TipeLokasi, ID AS IDLokasi, Nama as NamaLokasi, Kode FROM LocExhibition where Disable = 0
                            )a


                            select aa.ID as IDUser,a.Nama                            
                            from DataSales a
                            JOIN DataSalesGroup b on b.id = a.IDGroup
                            JOIN frank_ac.dbo.UserAccount aa on aa.UserNumber = a.Keterangan
                            JOIN #tempLoc c on c.Kode = b.NamaKode 
                            where A.Draft=0 AND A.Disable=0 and c.IDLokasi = @IDLokasi and c.TipeLokasi=@TipeLokasi and aa.status = 1

                            drop table #tempLoc";
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
                    list.Add(new DataJR
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

    [WebMethod]
    public static int CheckData(int IDUser, string Nama, string tanggalAwal,  int IDLokasi, int TipeShift)
    {

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            string query = @"
                SELECT COUNT(*) FROM MasterJabatanPenggantiOpname
                WHERE IDUser = @IDUser            
                AND EfektifDate = CAST(@TanggalAwal AS DATE)                 
                AND TipeShift = @TipeShift 
                AND IDLokasi = @IDLokasi";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = IDUser });
                command.Parameters.Add(new SqlParameter("@Nama", SqlDbType.VarChar) { Value = Nama });
                command.Parameters.Add(new SqlParameter("@TanggalAwal", SqlDbType.DateTime) { Value = tanggalAwal });                
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@TipeShift", SqlDbType.Int) { Value = TipeShift });

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count;
            }
        }
    }

    [WebMethod]
    public static string SaveData(int IDUser,  string tanggalAwal, string TipeJabatan, int IDLokasi, int TipeShift)
    {
        // Example: simulate saving (you can insert to DB here)
        var context = HttpContext.Current;
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {                                    
            HttpCookie UserNama= context.Request.Cookies["OperatorName"];
            string operatorName = App.UserNama == "" ? UserNama.Value : App.UserNama;            
            connection.Open();          
            string insertData = @"    
                DECLARE @TimeZone DATETIME
                SELECT  @TimeZone = CONVERT(DATETIME, SYSDATETIMEOFFSET() AT TIME ZONE TimeZone) 
                FROM (
                    SELECT 1 AS TipeLokasi,ID AS IDLokasi,Nama,IDTimeZone FROM LocOutlet
                    UNION
                    SELECT 2 AS TipeLokasi,ID AS IDLokasi,Nama,1 AS IDTimeZone FROM LocExhibition
                )XA
                JOIN TimeZoneLoc x on x.ID=XA.IDTimeZone
                WHERE IDLokasi = @IDLokasi AND TipeLokasi=@TipeLokasi

                INSERT INTO MasterJabatanPenggantiOpname (IDUser, TipeLokasi, IDLokasi, TipeShift, TipeJabatan, EfektifDate, Created_at, Created_by)
                OUTPUT INSERTED.ID, INSERTED.IDUser, INSERTED.Created_at, INSERTED.Created_by
                INTO MasterJabatanPenggantiOpname_Log (IDMasterJabatanPenggantiOpname, IDUser, Created_at, Created_by)
                VALUES(@IDUser, @TipeLokasi, @IDLokasi, @TipeShift, @TipeJabatan, CAST(@TanggalAwal AS DATE), @TimeZone, @CreatedBy)
            ";
            using (SqlCommand command = new SqlCommand(insertData, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = Convert.ToInt32(1) });
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = IDUser });
                command.Parameters.Add(new SqlParameter("@TipeJabatan", SqlDbType.VarChar) { Value = TipeJabatan });
                command.Parameters.Add(new SqlParameter("@TanggalAwal", SqlDbType.DateTime) { Value = tanggalAwal });
                command.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.VarChar) { Value = operatorName });
                command.Parameters.Add(new SqlParameter("@TipeShift", SqlDbType.Int) { Value = TipeShift });
                
                command.ExecuteNonQuery();  
            }            
            connection.Close();
        }


        return "Successfully saved ";
    }


    protected void NextStep(object sender, EventArgs e)
    {
        // Response.Write(location.SelectedValue);
        // string href = "RegisterList.aspx?IDLokasi=1";

        // Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "DJ_DetailPrint.aspx?id=1";

        //Customize-Qstr        
        Response.Redirect(href);
    }

    public class DataJR
    {
        public int IDUser { get; set; }
        public string Nama { get; set; }
    }
    public class DataShift
    {
        public int ID { get; set; }
        public string Nama { get; set; }
        public string MaxShift { get; set; }        
    }
}