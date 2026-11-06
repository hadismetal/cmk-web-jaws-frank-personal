using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using System.Web.UI;
using System.Globalization;
using System.Data.SqlClient;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;
using System.Net.Cache;
using Newtonsoft.Json;
using System.Data.SqlClient;

public partial class Page_kartustock : System.Web.UI.Page {
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

        DateTime today = DateTime.Now.Date;
        // fillTable1(loc);
        // App.Protect();        

		if (!IsPostBack) 
        {            
            fillFilter();			
		}


	}


    protected void fillFilter()
    {
        if (App.Refresh(this))
        {

        }
    }

    [WebMethod]
    public static List<ResultInsert> PostUpdateData(IIDataOpname IDataOpname)
    {
        List<ResultInsert> result = new List<ResultInsert>();
        
        DataSet ds = ExecuteInsertOpname(IDataOpname);
        DataTable dt = ds.Tables[0];


        foreach (DataRow datas  in dt.Rows)
        {
            ResultInsert resulted = new ResultInsert
            {
                Status = Convert.ToInt32(datas["Status"]),
                Message = datas["Message"].ToString()
            };
            result.Add(resulted);
        }
      
        return result;
    }

    public static DataSet ExecuteInsertOpname(IIDataOpname InsertDataOpname)
    {               
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {                        
            
            using (SqlCommand command = new SqlCommand("sp_UpdateDataKartuStock", connection))
            {

                var list = new List<IIDataOpname>();
                list.Add(InsertDataOpname);
                string Data = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                
                DataTable pDt1 = JsonConvert.DeserializeObject<DataTable>(Data);                

                command.CommandType = CommandType.StoredProcedure;                
                command.Parameters.Add(new SqlParameter("@DataOpname", SqlDbType.Structured) { Value = pDt1 });
                command.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar) { Value = App.UserNama});                

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds;
            }
        }
    }



    private void AddFooterCell(TableRow tr,string idElement, int value)
    {
        TableCell cell = new TableCell();            
        cell.ID = idElement;
        cell.ClientIDMode = ClientIDMode.Static;
        cell.Text = value.ToString();  // Convert value to string        
        tr.Cells.Add(cell);  // Add the cell to the row
    }
    private void AddFooterCellTotal(TableRow tr,string idElement, int value)
    {
        TableCell cell = new TableCell();
        cell.ColumnSpan = 1; 
        cell.Text = @"<div class='text-center' id='"+ idElement +"'>"+value+"</div>";
        cell.HorizontalAlign = HorizontalAlign.Center;  // Align text to the center
        tr.Cells.Add(cell);  // Add the cell to the row
    }
    private void AddFooterCellSL(TableRow tr,string idElement, int value,int totExst)
    {
        TableCell cell = new TableCell();
        cell.CssClass="text-center";
        cell.ID = idElement;
        cell.ClientIDMode = ClientIDMode.Static;
        cell.Text = (totExst-value).ToString();  // Convert value to string        
        tr.Cells.Add(cell);  // Add the cell to the row
    }
    private void AddFooterCellTotalSL(TableRow tr,string idElement, int value,int totExst)
    {
        TableCell cell = new TableCell();
        cell.ColumnSpan = 1; 
        cell.Text = @"<div class='text-center' id='"+ idElement +"'>"+(totExst-value)+"</div>";
        cell.HorizontalAlign = HorizontalAlign.Center;  // Align text to the center
        tr.Cells.Add(cell);  // Add the cell to the row
    }

    private void AddFooterCell1(TableRow tr, string idElement)
    {
        TableCell cell = new TableCell();
        cell.ColumnSpan = 1;
        cell.Text = @"<div class='text-center'>
                <input type='text' value='0' class='form-control text-center' readonly id='" + idElement + @"'/>
              </div>";
        cell.HorizontalAlign = HorizontalAlign.Center;  // Align text to the center
        tr.Cells.Add(cell);  // Add the cell to the row
    }    
    private void AddCelltb1(TableRow tr,string idElement, string dataTipe = ""
    , string dataTipeRow="", string dataTipeColumn="", string attributes="")
    {
        string keypress = "";
        if (attributes == "")
        {
            keypress = "onkeypress='return onlyNumbersAndCommas(event)'";
        }
        TableCell cell = new TableCell();
        cell.ColumnSpan = 1; 
        cell.Style.Add("text-align","center");
        cell.Text = @"<div class='text-center'>
                <input type='text' value='0' class='form-control text-center' "+ keypress  +"  onchange='kalkulasiBarangDalam(`" + dataTipeColumn + "`,`"+ dataTipeRow + @"`)'  
                id='" + idElement + "' data-tipe='" + dataTipe + "' data-row='" + dataTipeRow + "' data-column='"+ dataTipeColumn + "' "+ attributes +@"/>
              </div>";
        cell.HorizontalAlign = HorizontalAlign.Center;  // Align text to the center
        tr.Cells.Add(cell);  // Add the cell to the row
    }
    private void AddCelltb2(TableRow tr,string idElement, string dataTipe = ""
    , string dataTipeRow="", string dataTipeColumn="", string attributes="")
    {
        string keypress = "";
        if (attributes == "")
        {
            keypress = "onkeypress='return onlyNumbersAndCommas(event)'";
        }
        TableCell cell = new TableCell();
        cell.ColumnSpan = 1; 
        cell.Style.Add("text-align","center");
        cell.Text = @"<div class='text-center'>
                <input type='text' value='0' class='form-control text-center' "+ keypress  +"  onchange='kalkulasiBarangTitipan(`" + dataTipeColumn + "`,`"+ dataTipeRow + @"`)'  
                id='" + idElement + "' data-tipe='" + dataTipe + "' data-row='" + dataTipeRow + "' data-column='"+ dataTipeColumn + "' "+ attributes +@"/>
              </div>";
        cell.HorizontalAlign = HorizontalAlign.Center;  // Align text to the center
        tr.Cells.Add(cell);  // Add the cell to the row
    }
    private void AddCelltb3(TableRow tr,string idElement, string dataTipe = ""
    , string dataTipeRow="", string dataTipeColumn="", string attributes="")
    {
        string keypress = "";
        if (attributes == "")
        {
            keypress = "onkeypress='return onlyNumbersAndCommas(event)'";
        }
        TableCell cell = new TableCell();
        cell.ColumnSpan = 1; 
        cell.Style.Add("text-align","center");
        cell.Text = @"<div class='text-center'>
                <input type='text' value='0' class='form-control text-center' "+ keypress  +"  onchange='kalkulasiBarangLuar(`" + dataTipeColumn + "`,`"+ dataTipeRow + @"`)'  
                id='" + idElement + "' data-tipe='" + dataTipe + "' data-row='" + dataTipeRow + "' data-column='"+ dataTipeColumn + "' "+ attributes +@"/>
              </div>";
        cell.HorizontalAlign = HorizontalAlign.Center;  // Align text to the center
        tr.Cells.Add(cell);  // Add the cell to the row
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr		
        // href += "&parcel01=" + parcel01.SelectedValue;
        // href += "&parcel02=" + parcel02.SelectedValue;
        // href += "&parcel03=" + parcel03.SelectedValue;
        // href += "&parcel04=" + parcel04.SelectedValue;
        // href += "&parcel05=" + parcel05.SelectedValue;
        // href += "&parcel06=" + parcel06.SelectedValue;		
        // href += "&order=" + order.SelectedValue;
        // href += "&orderby=" + orderby.SelectedValue;

        Response.Redirect(href);
    }

    
    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]

    public static DDataOpname GetData(int ID, string KodeShift)
    {        
        List<DataDisplay> DataDis = new List<DataDisplay>();
        List<DataDJ> dataDJ = new List<DataDJ>();        
        List<DataPG> dataPG = new List<DataPG>();        
        List<DataLD> dataLD = new List<DataLD>();        
        List<DataSummary> dataSummary = new List<DataSummary>();        
        List<DataSistemDJ> dataSistemDJ = new List<DataSistemDJ>();        
        List<DataSistemPG> dataSistemPG = new List<DataSistemPG>();        
        List<DataSistemLD> dataSistemLD = new List<DataSistemLD>();        

        DataSet ds = new DataSet();


        string query = "sp_GetDataKartuStock";
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = ID });

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(ds);
                DataDis = Helper.GetDataFromDataTable<DataDisplay>(ds.Tables[0]);                
                dataDJ = Helper.GetDataFromDataTable<DataDJ>(ds.Tables[1]);                
                dataPG = Helper.GetDataFromDataTable<DataPG>(ds.Tables[1]);                
                dataLD = Helper.GetDataFromDataTable<DataLD>(ds.Tables[1]);                
                dataSummary = Helper.GetDataFromDataTable<DataSummary>(ds.Tables[2]);

                DataTable SD = ds.Tables[3];

                foreach (DataRow SData in SD.Rows)
                {
                    if(Convert.ToString(SData["TipeProduct"]) == "DJ")
                    {
                        dataSistemDJ.Add(new DataSistemDJ
                        {
                            IDLokasi = Convert.ToInt32(SData["IDLokasi"]),
                            TipeLokasi = Convert.ToInt32(SData["TipeLokasi"]),
                            TipeProduct = Convert.ToString(SData["TipeProduct"]),                                                                        
                            BGLDJ = Convert.ToInt32(SData["BGL"]),
                            BRCDJ = Convert.ToInt32(SData["BRC"]),
                            EARDJ = Convert.ToInt32(SData["EAR"]),
                            LRDJ = Convert.ToInt32(SData["LR"]),
                            BUCKDJ = Convert.ToInt32(SData["BUCK"]),
                            MJDJ = Convert.ToInt32(SData["MJ"]),
                            NECKDJ = Convert.ToInt32(SData["NECK"]),
                            BRODJ = Convert.ToInt32(SData["BRO"]),
                            PDTDJ = Convert.ToInt32(SData["PDT"]),
                            PDTCDJ = Convert.ToInt32(SData["PDTC"]),
                            WRDJ = Convert.ToInt32(SData["WR"]),
                            OTHDJ = Convert.ToInt32(SData["Others"]),
                        });

                    }else if (Convert.ToString(SData["TipeProduct"]) == "PG")
                    {
                        dataSistemPG.Add(new DataSistemPG
                        {
                            IDLokasi = Convert.ToInt32(SData["IDLokasi"]),
                            TipeLokasi = Convert.ToInt32(SData["TipeLokasi"]),
                            TipeProduct = Convert.ToString(SData["TipeProduct"]),                                                                        
                            BGLPG = Convert.ToInt32(SData["BGL"]),
                            BRCPG = Convert.ToInt32(SData["BRC"]),
                            EARPG = Convert.ToInt32(SData["EAR"]),
                            LRPG = Convert.ToInt32(SData["LR"]),
                            BUCKPG = Convert.ToInt32(SData["BUCK"]),
                            MJPG = Convert.ToInt32(SData["MJ"]),
                            NECKPG = Convert.ToInt32(SData["NECK"]),
                            BROPG = Convert.ToInt32(SData["BRO"]),
                            PDTPG = Convert.ToInt32(SData["PDT"]),
                            PDTCPG = Convert.ToInt32(SData["PDTC"]),
                            WRPG = Convert.ToInt32(SData["WR"]),
                            OTHPG = Convert.ToInt32(SData["Others"]),
                        });                
                        
                    }
                    else
                    {
                        dataSistemLD.Add(new DataSistemLD
                        {
                            IDLokasi = Convert.ToInt32(SData["IDLokasi"]),
                            TipeLokasi = Convert.ToInt32(SData["TipeLokasi"]),
                            TipeProduct = Convert.ToString(SData["TipeProduct"]),                                                                        
                            OTHLD = Convert.ToInt32(SData["Others"]),
                        });
                    }
                                
                }
            }
        }
        return new DDataOpname
        {
            ID = ID,
            DataDisplay  = DataDis,
            DataDJ = dataDJ,
            DataPG = dataPG,
            DataLD = dataLD,
            DataSummary = dataSummary,
            DataSistemDJ = dataSistemDJ,
            DataSistemPG = dataSistemPG,
            DataSistemLD = dataSistemLD
        };
    }

    [WebMethod]    
    public static List<ResultInsert> ApproveDataOpname(int ID, string KETERANGAN, int ISSELISIHDJ, int ISSELISIHPG, int ISSELISIHLD)
    {
        List<ResultInsert> result = new List<ResultInsert>();        

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {                        
            using (SqlCommand command = new SqlCommand("sp_ApproveKartuStock", connection))
            {
                command.CommandType = CommandType.StoredProcedure;                
                command.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = ID });
                command.Parameters.Add(new SqlParameter("@KETERANGAN", SqlDbType.VarChar) { Value = KETERANGAN });
                command.Parameters.Add(new SqlParameter("@ISSELISIHDJ", SqlDbType.Int) { Value = ISSELISIHDJ });
                command.Parameters.Add(new SqlParameter("@ISSELISIHPG", SqlDbType.Int) { Value = ISSELISIHPG });
                command.Parameters.Add(new SqlParameter("@ISSELISIHLD", SqlDbType.Int) { Value = ISSELISIHLD });
                command.Parameters.Add(new SqlParameter("@Operator", SqlDbType.VarChar) { Value = App.UserNama});                

                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];


                foreach (DataRow datas  in dt.Rows)
                {
                    ResultInsert resulted = new ResultInsert
                    {
                        Status = Convert.ToInt32(datas["Status"]),
                        Message = datas["Message"].ToString()
                    };
                    result.Add(resulted);
                }
            
                return result;
            }
        }        
    }

    public class DDataOpname
    {
        public int ID { get; set; }
        public List<DataDisplay> DataDisplay { get; set; }
        public List<DataDJ> DataDJ { get; set; }
        public List<DataPG> DataPG { get; set; }
        public List<DataLD> DataLD { get; set; }        
        public List<DataSummary> DataSummary { get; set; }
        public List<DataSistemDJ> DataSistemDJ { get; set; }
        public List<DataSistemPG> DataSistemPG { get; set; }
        public List<DataSistemLD> DataSistemLD { get; set; }
    }
    
    public class DataDJ
    {
        public int ID { get; set; }
        public int TipeUser { get; set; }
        public int IDHeader { get; set; }
        public int IDOpname { get; set; }
        public int IDDisplay { get; set; }        
        public int BGLDJ { get; set; }
        public int BRCDJ { get; set; }
        public int EARDJ { get; set; }
        public int LRDJ { get; set; }
        public int BUCKDJ { get; set; }
        public int MJDJ { get; set; }        
        public int NECKDJ { get; set; }
        public int BRODJ { get; set; }
        public int PDTDJ { get; set; }
        public int PDTCDJ { get; set; }
        public int WRDJ { get; set; }
        public int OTHDJ { get; set; }
        public string Created_by { get; set; }
        public string Created_at { get; set; }
    }
    
    public class DataPG
    {
        public int ID { get; set; }
        public int TipeUser { get; set; }
        public int IDHeader { get; set; }
        public int IDOpname { get; set; }
        public int IDDisplay { get; set; }        
        public int BGLPG { get; set; }
        public int BRCPG { get; set; }
        public int EARPG { get; set; }
        public int LRPG { get; set; }
        public int BUCKPG { get; set; }
        public int MJPG { get; set; }        
        public int NECKPG { get; set; }
        public int BROPG { get; set; }
        public int PDTPG { get; set; }
        public int PDTCPG { get; set; }
        public int WRPG { get; set; }
        public int OTHPG { get; set; }
        public string Created_by { get; set; }
        public string Created_at { get; set; }
    }
    public class DataLD
    {
        public int ID { get; set; }
        public int TipeUser { get; set; }
        public int IDHeader { get; set; }
        public int IDOpname { get; set; }
        public int IDDisplay { get; set; }                
        public int OTHLD { get; set; }
        public string Created_by { get; set; }
        public string Created_at { get; set; }
    }


    public class DataDisplay
    {
        public int ID { get; set; }
        public string NamaDisplay { get; set; }
    }    

    public class DataSistemDJ
    {
        public int IDLokasi { get; set; }               
        public int TipeLokasi { get; set; }               
        public string TipeProduct { get; set; }               
        public int BGLDJ { get; set; }
        public int BRCDJ { get; set; }
        public int EARDJ { get; set; }
        public int LRDJ { get; set; }
        public int BUCKDJ { get; set; }
        public int MJDJ { get; set; }        
        public int NECKDJ { get; set; }
        public int BRODJ { get; set; }
        public int PDTDJ { get; set; }
        public int PDTCDJ { get; set; }
        public int WRDJ { get; set; }
        public int OTHDJ { get; set; }
        public string Created_by { get; set; }
        public string Created_at { get; set; }
    }
    public class DataSistemPG
    {
        public int IDLokasi { get; set; }               
        public int TipeLokasi { get; set; }               
        public string TipeProduct { get; set; }        
        public int BGLPG { get; set; }
        public int BRCPG { get; set; }
        public int EARPG { get; set; }
        public int LRPG { get; set; }
        public int BUCKPG { get; set; }
        public int MJPG { get; set; }        
        public int NECKPG { get; set; }
        public int BROPG { get; set; }
        public int PDTPG { get; set; }
        public int PDTCPG { get; set; }
        public int WRPG { get; set; }
        public int OTHPG { get; set; }
        public string Created_by { get; set; }
        public string Created_at { get; set; }
    }
    public class DataSistemLD
    {
        public int IDLokasi { get; set; }               
        public int TipeLokasi { get; set; }               
        public string TipeProduct { get; set; }        
        public int OTHLD { get; set; }
        public string Created_by { get; set; }
        public string Created_at { get; set; }
    }
    public class DataSummary
    {        
        public int TipeUser { get; set; }
        public int IDHeader { get; set; }
        public int IDOpname { get; set; }     
        public int BGLDJ { get; set; }
        public int BRCDJ { get; set; }
        public int EARDJ { get; set; }
        public int LRDJ { get; set; }
        public int BUCKDJ { get; set; }
        public int MJDJ { get; set; }        
        public int NECKDJ { get; set; }
        public int BRODJ { get; set; }
        public int PDTDJ { get; set; }
        public int PDTCDJ { get; set; }
        public int WRDJ { get; set; }
        public int OTHDJ { get; set; }
        public int BGLPG { get; set; }
        public int BRCPG { get; set; }
        public int EARPG { get; set; }
        public int LRPG { get; set; }
        public int BUCKPG { get; set; }
        public int MJPG { get; set; }        
        public int NECKPG { get; set; }
        public int BROPG { get; set; }
        public int PDTPG { get; set; }
        public int PDTCPG { get; set; }
        public int WRPG { get; set; }
        public int OTHPG { get; set; }
        public int OTHLD { get; set; }
    }

    public class IIDataOpname
    {

        public int BGL { get; set; }
        public int BRC { get; set; }
        public int EAR { get; set; }
        public int LR { get; set; }
        public int BUCK { get; set; }
        public int MJ { get; set; }
        public int NECK { get; set; }
        public int BRO { get; set; }
        public int PDT { get; set; }
        public int PDTC { get; set; }
        public int WR { get; set; }
        public int OTH { get; set; }
        public string Jenis { get; set; }
        public int ID { get; set; }        

    }
    public class ResultInsert
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }

}