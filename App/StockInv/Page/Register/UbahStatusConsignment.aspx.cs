using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Linq.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Linq.Dynamic;
using System.Web.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Script.Serialization;



public partial class Page_Register_UbahStatusConsignment: System.Web.UI.Page
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

    private Map_CS _d;

    private Map_CS d
    {
        get
        {
            if (_d == null)
                _d = new Map_CS();
            return _d;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (d != null)
            d.Dispose();
        base.Dispose();
    }
    
    App price = new App();

    decimal GoldPrice = 0m;
    static int i = 1;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        
        App.Protect();
        App.ProtectRole(Role.Operator);


        if (!IsPostBack)
        {                        
            
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("PG.aspx"); }
  

    public class SupplierSJ
    {
        public string Supplier { get; set; }
    }

    public class NonExistingItem
    {
        public string Item { get; set; }
        public int Count { get; set; }
    }
    public class itemExist
    {
        public string Item { get; set; }
        public int Count { get; set; }
    }
    [WebMethod]
    public static object CheckDataExistence(List<string> items)
    {
        // List to hold values with their counts
        List<NonExistingItem> itemCounts = new List<NonExistingItem>();
        List<itemExist> itemExists = new List<itemExist>();
        List<SupplierSJ> SJSupp = new List<SupplierSJ>();

        string suppler = "";
            foreach (string item in items)
            {

                string Remarks = "SELECT COUNT(a.Nomor) AS Total FROM StockReceivePG a JOIN StockProductPG b on b.nomor=a.nomor WHERE a.Nomor ='"+item.Trim()+"' and a.Consignment=1 and b.statusPenjualan=0";
                string Status = "SELECT COUNT(a.plu) AS Total FROM (select top 1 plu,max(OperatorTgl) tgl,status1 from ChangesStatusConsignment where plu='"+item.Trim()+"' group by plu,status1) a where plu='"+item.Trim()+"' and (status1=0 or status1=1)";
                string querySupplier = "SELECT a.supplierSJ FROM StockReceivePG a JOIN StockProductPG b on b.nomor=a.nomor WHERE a.Nomor ='"+item.Trim()+"' and a.Consignment=1 and b.statusPenjualan=0 group by a.SupplierSJ";
                int remark = Db.SingleInteger(Remarks, Db.Cnn_DB);                                
                int status = Db.SingleInteger(Status, Db.Cnn_DB);                                
                string supplierSJ = Db.SingleString(querySupplier,Db.Cnn_DB);
                
                itemCounts.Add(new NonExistingItem { Item = item.Trim(), Count = remark});                
                itemExists.Add(new itemExist { Item = item.Trim(), Count = status});                

                if (suppler != supplierSJ)
                {
                    SJSupp.Add(new SupplierSJ {Supplier = supplierSJ});                    
                }                

                suppler = supplierSJ;
            }
         
        var result = new 
        {
            plu=itemCounts,
            SJ=SJSupp,
            IE = itemExists
        };
        // Serialize the result to JSON
        return new JavaScriptSerializer().Serialize(result);
    }


    protected void uploadData(object sender, EventArgs e)    
    {
        error.Visible = false;
        success.Visible = false;       
        
        if (!error.Visible)
        {
            if (Path.GetExtension(FileUpload1.PostedFile.FileName) == ".csv")
            {
                
                DataTable dt = new DataTable();
                dt.Columns.Add("PLU");               

                int index = 0;
                try
                {
                    using (var reader = new StreamReader(FileUpload1.FileContent, Encoding.Default))
                    {
                        
                        string[] stringSeparators = new string[] { "\r\n" };
                        string[] read = reader.ReadToEnd().Split(stringSeparators, StringSplitOptions.None);

                        // Debugging output to check the total number of rows
                        Response.Write("Total Rows: " + read.Length);

                        for (int i = 1; i < read.Length; i++)  // Starting from 1 if first row is header
                        {
                            if (read.Length > 1)  // Ensure there's a second column before accessing it
                            {
                                DataRow dr = dt.NewRow();
                                dr["PLU"] = read[i];  // Set the "PLU" value from the second column
                                dt.Rows.Add(dr);     // Add the DataRow to the DataTable
                            }
                        }
                    }
                   
                        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
                        {
                            SqlCommand cmd = conn.CreateCommand();
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "dbo.sp_ChangesStatusConsignment"; //The name of the above mentioned stored procedure.  
                            SqlParameter param = cmd.Parameters.AddWithValue("@Data", dt); //Here"@MyUDTableType" is the User-defined Table Type as a parameter.                                                                                     
                            SqlParameter param2 = cmd.Parameters.Add("@OperatorNama", SqlDbType.VarChar);                            
                            param2.Value = App.UserNama;

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    
                }
                catch (Exception ex)
                {
                    error.Text = ex.Message;
                    error.Visible = true;
                }              
            }
            else
            {
                error.Text = "File tidak sesuai, gunakan file extensi .csv";
                error.Visible = true;
            }
        }
        
        if (!error.Visible)
        {            

            string href = App.CurrentPath;            
            href += "?d=1&sj=";
            Response.Redirect(href);
        }
    }
    

    protected void btnTemplate_Click(object sender, EventArgs e)    
    {
        StringBuilder builder = new StringBuilder();
		string[] namakolom = new string[] {"PLU"};
		string result = "";
		foreach (var column in namakolom)
		{
			result += "\"" + column + "\";";
		}

		builder.Append(result.Remove(result.Length - 1));

		Response.Clear();
		Response.ContentType = "text/csv";
		Response.AddHeader("Content-Disposition", "attachment;filename=TemplateUbahStatusConsignment.csv");
		Response.Write(builder.ToString());
		Response.End();
    }
}