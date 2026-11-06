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
    public static string GetNomorDoc()
    {
        string query = @"
            select m_nodoc from dbinfo.dbo.t_sop where m_nomor='26000001'
        ";
        string nodoc = "";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_CS))
        {
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];
                foreach (DataRow item in dt.Rows)
                {
                    nodoc = item["m_nodoc"].ToString();
                }
            }
        }
        return nodoc;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static DDataOpname GetDataOpname(int ID)
    {
        List<DataSumDisplay> dataSumDisplay = new List<DataSumDisplay>();
        List<DataSumStock> dataSumStock = new List<DataSumStock>();
        List<DataDetailDalam> dataDetailDalam = new List<DataDetailDalam>();
        List<DataDetailTitipan> dataDetailTitipan = new List<DataDetailTitipan>();
        List<DataDetailLuarSistem> dataDetailLuarSistem = new List<DataDetailLuarSistem>();

        List<DataSistem> dataSistem = new List<DataSistem>();

        string queryGetData = "sp_GetDataBAST_Opname";
         
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
           
            using (SqlCommand command = new SqlCommand(queryGetData, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = ID });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                dataSumDisplay = Helper.GetDataFromDataTable<DataSumDisplay>(ds.Tables[0]);                
                dataSumStock = Helper.GetDataFromDataTable<DataSumStock>(ds.Tables[1]);                
                dataDetailDalam = Helper.GetDataFromDataTable<DataDetailDalam>(ds.Tables[2]);                
                dataDetailTitipan = Helper.GetDataFromDataTable<DataDetailTitipan>(ds.Tables[3]);                
                dataDetailLuarSistem = Helper.GetDataFromDataTable<DataDetailLuarSistem>(ds.Tables[4]);                
                dataSistem = Helper.GetDataFromDataTable<DataSistem>(ds.Tables[5]);                
                

            }           
        }
        return new DDataOpname
        {          
            DataSumDisplay  = dataSumDisplay,            
            DataSumStock  = dataSumStock,
            DataDetailDalam  = dataDetailDalam,            
            DataDetailTitipan  = dataDetailTitipan,            
            DataDetailLuarSistem  = dataDetailLuarSistem,
            DataSistem  = dataSistem
        };

    }

   public class DDataOpname
    {        
        public List<DataSumDisplay> DataSumDisplay { get; set; }
        public List<DataSumStock> DataSumStock { get; set; }        
        public List<DataDetailDalam> DataDetailDalam { get; set; }
        public List<DataDetailTitipan> DataDetailTitipan { get; set; }
        public List<DataDetailLuarSistem> DataDetailLuarSistem { get; set; }
        public List<DataSistem> DataSistem { get; set; }

    }


    public class DataSumDisplay
    {
        public int IDHeader { get; set; }
        public int IDOpname { get; set; }        
        public string Nama { get; set; }
        public int Data { get; set; }
    }    
    public class DataSumStock
    {
        public int ID { get; set; }
        public string KodeLokasi { get; set; }
        public string NamaLokasi { get; set; }
        public string NamaShift { get; set; }
        public string Approval_at { get; set; }
        public string TimeLine { get; set; }
        public int Data { get; set; }

    }   

    public class DataDetailDalam
    {
        public int IDHeader { get; set; }
        public int IDOpname { get; set; }        
        public int IDTipeOpname { get; set; }        
        public string NamaOpname { get; set; }
        public string NamaDetailOpname { get; set; }
        public int Data { get; set; }

    }
    public class DataDetailTitipan
    {
        public int IDHeader { get; set; }
        public int IDOpname { get; set; }        
        public int IDTipeOpname { get; set; }        
        public string NamaOpname { get; set; }
        public string NamaDetailOpname { get; set; }
        public int Data { get; set; }

    }
    public class DataDetailLuarSistem
    {
        public int IDHeader { get; set; }
        public int IDOpname { get; set; }        
        public int IDTipeOpname { get; set; }        
        public string NamaOpname { get; set; }
        public string NamaDetailOpname { get; set; }
        public int Data { get; set; }
    }
    public class DataSistem
    {
        public int IDLokasi { get; set;}
        public int TipeLokasi { get; set;}
        public int Data { get; set; }

    }

}