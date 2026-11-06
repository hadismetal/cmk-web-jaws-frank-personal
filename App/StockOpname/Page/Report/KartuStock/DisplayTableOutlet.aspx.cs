using System;
using System.Web;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Services;

public partial class Page_DisplayTableOutlet : System.Web.UI.Page
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
        // App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {            
            Ddl.Location(location, App.UserID);

            //datepicker.Text = Str.Tgl(DateTime.Now);
            //datepicker2.Text = Str.Tgl(DateTime.Now);

            fillFilter();
			// if (App.Refresh(this)) fillTable(sender, e);       
        }
    }
    
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static List<DataDisplayOutlet> GetData(int IDLokasi)
    {
        List<DataDisplayOutlet> DataTampung = new List<DataDisplayOutlet>();

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string query = @"
                SELECT a.ID, b.Nama AS NamaLokasi, a.NamaDisplay, FORMAT(a.Created_at, 'dddd, dd MMMM yyyy', 'id-ID') as Created_at, a.Created_by
                , Status
                FROM MasterDisplayOutlet a
                JOIN LocOutlet b ON a.IDLokasi = b.ID            
                WHERE a.IDLokasi = @IDLokasi
                ORDER BY b.ID ASC";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    int index = 1;
                    foreach (DataRow dr in dt.Rows)
                    {
                        DataDisplayOutlet Data = new DataDisplayOutlet
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            NamaLokasi = dr["NamaLokasi"].ToString(),
                            NamaDisplay = dr["NamaDisplay"].ToString(),
                            Status = Convert.ToInt32(dr["Status"]),
                            Created_at = dr["Created_at"].ToString(),
                            Created_by = dr["Created_by"].ToString()
                        };
                        DataTampung.Add(Data);
                    }
                }
            }
            return DataTampung;
        }
    }

    [WebMethod]
    public static object updateData(int ID, string tipe, string NamaDisplay = "")
    {      
        int Status  =0;
        string Message ="";
        var context = HttpContext.Current;
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {                                    
            HttpCookie UserNama= context.Request.Cookies["OperatorName"];
            string operatorName = App.UserNama == null ? UserNama.Value : App.UserNama;            
            
            string  query = @"
            DECLARE @STATUS INT, @MESSAGE VARCHAR(150)
            BEGIN TRY
                DECLARE @timezone varchar(100)
                select @timezone = CONVERT(DATETIME, SYSDATETIMEOFFSET() AT TIME ZONE ab.TimeZone)  from MasterDisplayOutlet a
                JOIN LocOutlet aa on aa.ID=a.IDLokasi
                JOIN TimeZoneLoc ab on ab.ID=aa.IDTimeZone
                where a.ID = @ID
                IF @tipe = 'update'
                BEGIN
                    IF EXISTS(SELECT 1 FROM MasterDisplayOutlet WHERE ID = @ID)
                    BEGIN
                        UPDATE MasterDisplayOutlet SET NamaDisplay = @NamaDisplay, Update_at = @timezone, Update_by = @Updated_by WHERE ID = @ID
                    END
                END
                ELSE IF @tipe = 'delete'
                BEGIN
                IF EXISTS(SELECT 1 FROM MasterDisplayOutlet WHERE ID = @ID)
                    BEGIN
                        UPDATE MasterDisplayOutlet SET Status = 0, Update_at = @timezone, Update_by = @Updated_by WHERE ID = @ID
                    END
                END
                ELSE IF @tipe = 'aktif'
                BEGIN            
                    IF EXISTS(SELECT 1 FROM MasterDisplayOutlet WHERE ID = @ID)
                    BEGIN
                        UPDATE MasterDisplayOutlet SET Status = 1, Update_at = @timezone, Update_by = @Updated_by WHERE ID = @ID
                    END
                END
            SELECT @STATUS = 200, @MESSAGE = 'Success'
            END TRY
            BEGIN CATCH
                SELECT @STATUS = 200, @MESSAGE = ERROR_MESSAGE()
            END CATCH
            SELECT @STATUS AS STATUS, @MESSAGE AS MESSAGE                        
            ";            
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = ID });
                command.Parameters.Add(new SqlParameter("@tipe", SqlDbType.VarChar) { Value = tipe });
                command.Parameters.Add(new SqlParameter("@Updated_by", SqlDbType.VarChar) { Value = operatorName });                
                command.Parameters.Add(new SqlParameter("@NamaDisplay", SqlDbType.VarChar) { Value = NamaDisplay });                                
                
                connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        Status = Convert.ToInt32(row["STATUS"]);
                        Message = row["MESSAGE"].ToString();
                    }                    
                connection.Close();
            }
        }
        return new
        {
            Status,Message
        };
    }
    // protected void fillTable(object sender, EventArgs e)
    // {
    //     int IDLokasi = Convert.ToInt32(location.SelectedValue);
    //     string query = @"
    //         SELECT a.ID, b.Nama AS NamaLokasi, a.NamaDisplay, FORMAT(a.Created_at, 'dddd, dd MMMM yyyy', 'id-ID') as Created_at, a.Created_by
    //         FROM MasterDisplayOutlet a
    //         JOIN LocOutlet b ON a.IDLokasi = b.ID            
    //         WHERE a.IDLokasi = @IDLokasi
    //         ORDER BY b.ID ASC";

    //     using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
    //     {
    //         using (SqlCommand command = new SqlCommand(query, connection))
    //         {
    //             command.CommandType = CommandType.Text;
    //             command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                
    //             SqlDataAdapter adapter = new SqlDataAdapter(command);
    //             DataSet ds = new DataSet();
    //             adapter.Fill(ds);
    //             DataTable dt = ds.Tables[0];

    //             if (dt.Rows.Count > 0)
    //             {
    //                 int index = 1;
    //                 foreach (DataRow dr in dt.Rows)
    //                 {
    //                     TableRow tr = new TableRow();
    //                     TableCell td;

    //                     td = new TableCell();
    //                     td.Text = "<p>" + index++.ToString() + ".</p>";
    //                     td.Style.Add("text-align", "center");
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<p>" + dr["NamaLokasi"].ToString() + ".</p>";
    //                     td.Style.Add("text-align", "center");
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<p>" + dr["NamaDisplay"].ToString() + ".</p>";
    //                     td.Style.Add("text-align", "center");
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<p>" + dr["Created_at"].ToString() + ".</p>";
    //                     td.Style.Add("text-align", "center");
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = "<p>" + dr["Created_by"].ToString() + ".</p>";
    //                     td.Style.Add("text-align", "center");
    //                     tr.Cells.Add(td);

    //                     td = new TableCell();
    //                     td.Text = @"
    //                         <div class='row'>
    //                             <div class='col-md-2'></div>
    //                             <div class='col-md-4'>
    //                                 <a href='#' data-toggle='modal' data-target='#modalUpdate' type='button' title='Edit Data'>
    //                                     <i class='icon-pencil'></i>
    //                                 </a>                    
    //                             </div>
    //                             <div class='col-md-4'>
    //                                 <a href='#' data-toggle='modal' data-target='#modalDelete' type='button' title='Disable Data'>
    //                                     <i class='icon-trash'></i>
    //                                 </a>                    
    //                             </div>
    //                             <div class='col-md-2'></div>
    //                         </div>

    //                         ";
    //                     td.Style.Add("text-align", "center");
    //                     tr.Cells.Add(td);

    //                     tb.Rows.Add(tr);
    //                 }
    //             }
    //         }
            
    //     }

    // }

    //Display Table
    protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr		
		href += "&location=" + location.SelectedValue;		

		Response.Redirect(href);
	}
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {

            location.SelectedValue = Request.QueryString["location"];
            //datepicker.Text = Request.QueryString["dari"];
            //datepicker2.Text = Request.QueryString["sampai"];
        }
    }

    public class DataDisplayOutlet
    {
        public int ID { get; set; }
        public string NamaLokasi { get; set; }
        public string NamaDisplay { get; set; }
        public int Status { get; set; }
        public string Created_at { get; set; }
        public string Created_by { get; set; }
    }

}