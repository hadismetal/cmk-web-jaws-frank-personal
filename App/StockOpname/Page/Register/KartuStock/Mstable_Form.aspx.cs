using System;
using System.Web;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.Services;

public partial class Page_Mstable_Form : System.Web.UI.Page
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
    public static string Save(List<List<string>> data, int IDLokasi)
    {
        if (data == null || data.Count == 0)
            return "No Display Name received.";

        int totalInserted = 0;

        var context = HttpContext.Current;
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {                                    
            HttpCookie UserNama= context.Request.Cookies["OperatorName"];
            string operatorName = App.UserNama == "" ? UserNama.Value : App.UserNama;            
            connection.Open();
            int indeks = 0;
            string insertData = @"
                    IF NOT EXISTS(SELECT 1 FROM MasterDisplayOutlet WHERE NamaDisplay = @NamaDisplay AND IDLokasi = @IDLokasi AND Status = 1)
                    BEGIN
                        INSERT INTO MasterDisplayOutlet (IDJenisOpname,IDLokasi, KodeBrand, NamaDisplay, Status, Created_at, Created_by)
                        SELECT (select top 1 ID from MasterTableOpname where ID = 1) as IDJenisOpname, @IDLokasi, IIF(DB_NAME() LIKE '%frank%','FC',
                        IIF(DB_NAME() LIKE '%PALACE%','TP', IIF(DB_NAME() LIKE '%LEI%','PL','MD'))) as KodeBrand, @NamaDisplay, 1, GETDATE(), @CreatedBy
                    END
                    ELSE
                    BEGIN
                        UPDATE MasterDisplayOutlet 
                        SET Status = 1, Created_at = GETDATE(), Created_by = @CreatedBy
                        WHERE NamaDisplay = @NamaDisplay AND IDLokasi = @IDLokasi 
                    END
                ";
            foreach (var item in data)
            {
                string name = item[0];
                int qty = Convert.ToInt32(item[1]);

                for (int i = 1; i <= qty; i++)
                {
                    string generatedEmail = name + i.ToString();

                    using (SqlCommand command = new SqlCommand(insertData, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });                        
                        command.Parameters.Add(new SqlParameter("@NamaDisplay", SqlDbType.VarChar) { Value = generatedEmail });
                        command.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.VarChar) { Value = operatorName });
                        command.ExecuteNonQuery();  
                    }
                    totalInserted++;       
                }                        
            }
            connection.Close();
        }


        return "Successfully saved " + totalInserted + " Display Name.";
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
}