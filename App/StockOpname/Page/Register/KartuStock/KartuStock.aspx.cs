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
       
       if (Session["UserID"] == null)
        {
            // Session belum ada atau sudah expired
            // Response.Redirect("~/Login.aspx");
            App.Protect();
        }


        IDUser.Value = Convert.ToString(App.UserID);

        string loc = Request.QueryString["loc"];
        string locName = Request.QueryString["locName"];
        string shift = Request.QueryString["shift"];
        string namaJR = Request.QueryString["namaJR"];
        string kodeLoc = locName.Substring(6, 3);
        string tipelokasi = Request.QueryString["tipeLokasi"];
        string dataDisplay = Request.QueryString["display"];
        string KodeShift = "";
        
        string query = "SELECT ID, Nama FROM MasterShiftOpname where ID = " + shift ;
        DataTable dt = Db.Rs(query, Db.Cnn_DB);        
        foreach (System.Data.DataRow item in dt.Rows)
        {
            KodeShift = item["Nama"].ToString();
        }


        DateTime today = DateTime.Now.Date;
        fillTable1(loc);
        // App.Protect();        

		if (!IsPostBack) 
        {
            IDLokasi.Value = loc;
            IDDisplay.Value = dataDisplay;
            locType.Value = tipelokasi;                        

            kodetoko.InnerText = kodeLoc;
            lokasi.InnerText = locName;
            NamaAE.InnerText = namaJR;
            Shift.InnerText = KodeShift;
            tanggal.InnerText = today.ToString("dd-MM-yyyy");



            
            fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}


	}


    protected void fillFilter()
    {
        if (App.Refresh(this))
        {

        }
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true)]
    public static int  GetUserData(string UserID, int shift)
    {
        string query = @"
            DECLARE @ONDUTY INT = 0

            SELECT @ONDUTY = 1
            FROM MasterJabatanPenggantiOpname a
            WHERE a.IDUser = @IDUser and EfektifDate = CAST(GETDATE() AS DATE)
            and a.TipeShift = @shift

            IF(@ONDUTY = 0)
            BEGIN
                SELECT @ONDUTY = 
                CASE 
                    WHEN b.m_jabatan LIKE '%ASSISTANT STORE MANAGER%' OR b.m_jabatan LIKE '%STORE MANAGER%' OR b.m_jabatan LIKE '%JEWELLERY COORDINATOR%' OR b.m_jabatan LIKE '%ADMINISTRATIVE EXECUTIVE%' THEN 1                     
                    WHEN b.m_jabatan LIKE '%JEWELLERY REPRESENTATIVE%' OR b.m_jabatan LIKE '%GOLD REPRESENTATIVE%' THEN 2
                ELSE 1 END
                FROM frank_ac.dbo.UserAccount a 
                JOIN [DB_PROD_MYAPPS].dbhrd.DBO.mskaryawan b on b.m_nik COLLATE SQL_Latin1_General_CP1_CI_AS = a.UserNumber COLLATE SQL_Latin1_General_CP1_CI_AS
                WHERE a.ID = @IDUser AND b.m_tglkeluar = '1900-01-01 00:00:00.000'
            END

            SELECT @ONDUTY AS TipeUser
        ";
        int TipeUser = 0;

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = Convert.ToInt32(UserID) });
                command.Parameters.Add(new SqlParameter("@shift", SqlDbType.Int) { Value = shift });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    TipeUser = Convert.ToInt32(dr["TipeUser"]);
                }
            }
        }
        return TipeUser;
    }


    [WebMethod]
    // [ScriptMethod(UseHttpGet = true)]
    public static List<DataDisplay> GetDisplayFront(string IDLokasi, string Display, int UserType)
    {

        List<DataDisplay> DataMeja = new List<DataDisplay>();

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string getDisplay = @"
                

                    IF (@UserType > 0)
                    BEGIN
                        SELECT ID as IDTable, NamaDisplay FROM MasterDisplayOutlet 
                        WHERE IDLokasi = @IDLokasi AND Status = 1 and (@UserType > 0 OR ID in (SELECT value FROM STRING_SPLIT(@Display, ',')))
                        ORDER BY ID DESC
                    END
                    ELSE
                    BEGIN
                        SELECT ID as IDTable, NamaDisplay 
                        FROM MasterDisplayOutlet M
                        WHERE IDLokasi = @IDLokasi 
                        AND Status = 1 
                        AND (
                                @UserType > 0
                                OR EXISTS (
                                    SELECT 1 
                                    FROM STRING_SPLIT(@Display, ',') S
                                    WHERE M.NamaDisplay LIKE S.value + '%'
                                )
                            )
                        ORDER BY ID DESC
                    END

                    

            ";
            using (SqlCommand command = new SqlCommand(getDisplay, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = Convert.ToInt32(IDLokasi) });
                command.Parameters.Add(new SqlParameter("@Display", SqlDbType.VarChar) { Value = Display});
                command.Parameters.Add(new SqlParameter("@UserType", SqlDbType.Int) { Value = UserType});

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
    public static List<ResultInsert> InsertDataOpname(IIDataOpname IDataOpname)
    {
        List<ResultInsert> result = new List<ResultInsert>();
        
        try
        {
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
        }
        catch (Exception ex)
        {
                ResultInsert resulted = new ResultInsert
                {
                    Status = 500,
                    Message = ex.Message
                };
                result.Add(resulted);            
        }
      
        return result;
    }
    

    public static DataSet ExecuteInsertOpname(IIDataOpname InsertDataOpname)
    {             
        int IDHeader = 0;   
        int TipeUser = 0;   
        DataSet ds = new DataSet();               

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {                                                            
            int UserID = App.UserID == 0 ? InsertDataOpname.IDUser : App.UserID;
            
            using (SqlCommand command = new SqlCommand("sp_insertDailyOpname", connection))
            {
                string BD = Newtonsoft.Json.JsonConvert.SerializeObject(InsertDataOpname.BarangDisplay);                
                string BS = Newtonsoft.Json.JsonConvert.SerializeObject(InsertDataOpname.BarangDalam);                
                string BT = Newtonsoft.Json.JsonConvert.SerializeObject(InsertDataOpname.BarangTitipan);
                string BL = Newtonsoft.Json.JsonConvert.SerializeObject(InsertDataOpname.BarangLuarSistem);                
                
                DataTable pDt1 = JsonConvert.DeserializeObject<DataTable>(BD);
                DataTable pDt2 = JsonConvert.DeserializeObject<DataTable>(BS);
                DataTable pDt3 = JsonConvert.DeserializeObject<DataTable>(BT);
                DataTable pDt4 = JsonConvert.DeserializeObject<DataTable>(BL);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = InsertDataOpname.TipeLokasi });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = InsertDataOpname.IDLokasi});
                command.Parameters.Add(new SqlParameter("@KodeShift", SqlDbType.VarChar) { Value = InsertDataOpname.KodeShift});
                command.Parameters.Add(new SqlParameter("@IDUser", SqlDbType.Int) { Value = UserID });
                command.Parameters.Add(new SqlParameter("@BarangDisplay", SqlDbType.Structured) { Value = pDt1 });
                command.Parameters.Add(new SqlParameter("@BarangDalam", SqlDbType.Structured) { Value = pDt2 });
                command.Parameters.Add(new SqlParameter("@BarangTitipan", SqlDbType.Structured) { Value = pDt3 });                
                command.Parameters.Add(new SqlParameter("@BarangLuarSistem", SqlDbType.Structured) { Value = pDt4 });                

                SqlDataAdapter adapter = new SqlDataAdapter(command);                
                adapter.Fill(ds);

                DataTable dtt =  ds.Tables[0];
                // foreach (DataRow datas  in dtt.Rows)
                // {
                //     IDHeader = Convert.ToInt32(datas["IDHeader"]);
                //     TipeUser = Convert.ToInt32(datas["TipeUser"]);
                // }

                // return ds;
            }

        }        
        // using (SqlConnection connection1 = new SqlConnection(Db.Cnn_DB))        
        // {
        //     string querySistem = @"
        //         IF NOT EXISTS(SELECT 1 FROM StockOpname_DataSistem where IDOpname = @IDHeader) and @TipeUser in (1,9)
        //         BEGIN
        //             exec [dbo].[sp_RekapStockOpname] @IDLokasi, @TipeLokasi, 1, @IDHeader
        //         END
        //         else
        //         begin
        //             select
        //         end
        //     ";
        //     using (SqlCommand command = new SqlCommand(querySistem, connection1))
        //     {

        //         command.CommandType = CommandType.Text;
        //         command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = InsertDataOpname.TipeLokasi });
        //         command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = InsertDataOpname.IDLokasi});
        //         command.Parameters.Add(new SqlParameter("@IDHeader", SqlDbType.Int) { Value = IDHeader });
        //         command.Parameters.Add(new SqlParameter("@TipeUser", SqlDbType.Int) { Value = TipeUser });

        //         SqlDataAdapter adapter = new SqlDataAdapter(command);                                
        //     }
        // }
            return ds;
    }


    protected void fillTable1(string loc)
    {

        //data dj
        char tipeLokasi = loc[loc.Length - 1];
        string kodeLokasi = loc.Substring(0, loc.Length - 1);
        string tipeProduct = "";
        string statusRejected = "";
        int IDSO = 0;
        int DJBGL = 0;
        int DJBRC = 0;
        int DJEAR = 0;
        int DJLR = 0;
        int DJBUCK = 0;
        int DJMJ = 0;
        int DJNECK = 0;
        int DJBRO = 0;
        int DJPDT = 0;
        int DJPDTC = 0;
        int DJWR = 0;
        int DJOTH = 0;

        int PGBGL = 0;
        int PGBRC = 0;
        int PGEAR = 0;
        int PGLR = 0;
        int PGBUCK = 0;
        int PGMJ = 0;
        int PGNECK = 0;
        int PGBRO = 0;
        int PGPDT = 0;
        int PGPDTC = 0;
        int PGWR = 0;
        int PGOTH = 0;

        int LDBGL = 0;
        int LDBRC = 0;
        int LDEAR = 0;
        int LDLR = 0;
        int LDBUCK = 0;
        int LDMJ = 0;
        int LDNECK = 0;
        int LDBRO = 0;
        int LDPDT = 0;
        int LDPDTC = 0;
        int LDWR = 0;
        int LDOTH = 0;

        int totalDJ = 0;
        int totalPG = 0;
        int totalLD = 0;
        int totalALL = 0;



        totalALL = totalDJ + totalPG + totalALL;
        TableRow tr = new TableRow();
        tr.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter        
        tr.CssClass = "freeze-footer";

        TableCell footerFirstRow = new TableCell();
        footerFirstRow.Text = "Total";
        footerFirstRow.ColumnSpan = 1;
        footerFirstRow.CssClass = "freeze-column text-center";
        tr.Cells.Add(footerFirstRow);

        AddFooterCell1(tr, "totalDJBGL");
        AddFooterCell1(tr, "totalDJBRC");
        AddFooterCell1(tr, "totalDJEAR");
        AddFooterCell1(tr, "totalDJLR");
        AddFooterCell1(tr, "totalDJBUCK");
        AddFooterCell1(tr, "totalDJMJ");
        AddFooterCell1(tr, "totalDJNECK");
        AddFooterCell1(tr, "totalDJBRO");
        AddFooterCell1(tr, "totalDJPDT");
        AddFooterCell1(tr, "totalDJPDTC");
        AddFooterCell1(tr, "totalDJWR");
        AddFooterCell1(tr, "totalDJOTH");
        AddFooterCell1(tr, "totalDJ");

        AddFooterCell1(tr, "totalPGBGL");
        AddFooterCell1(tr, "totalPGBRC");
        AddFooterCell1(tr, "totalPGEAR");
        AddFooterCell1(tr, "totalPGLR");
        AddFooterCell1(tr, "totalPGBUCK");
        AddFooterCell1(tr, "totalPGMJ");
        AddFooterCell1(tr, "totalPGNECK");
        AddFooterCell1(tr, "totalPGBRO");
        AddFooterCell1(tr, "totalPGPDT");
        AddFooterCell1(tr, "totalPGPDTC");
        AddFooterCell1(tr, "totalPGWR");
        AddFooterCell1(tr, "totalPGOTH");
        AddFooterCell1(tr, "totalPG");

        AddFooterCell1(tr, "totalLDOTH");
        AddFooterCell1(tr, "totalLD");

        AddFooterCell1(tr, "totalALL");


        // AddFooterCellTotal(tr2,"totalDJ");  

        tb.Rows.Add(tr);

        //start generate tabel barang dalam
        generateTabelBarangDalam1(Convert.ToInt32(loc));
        //end generate tabel barang dalam

        //start generate tabel barang titipan
        generateTabelBarangTitipan();
        //end generate tabel barang titipan

        //start generate tabel barang luar
        generateTabelBarangluar();         
    }
    // private void AddFooterCell(TableRow tr,string idElement, int value)
    // {
    //     TableCell cell = new TableCell();            
    //     cell.ID = idElement;
    //     cell.ClientIDMode = ClientIDMode.Static;
    //     cell.Text = value.ToString();  // Convert value to string        
    //     tr.Cells.Add(cell);  // Add the cell to the row
    // }
    // private void AddFooterCellTotal(TableRow tr,string idElement, int value)
    // {
    //     TableCell cell = new TableCell();
    //     cell.ColumnSpan = 1; 
    //     cell.Text = @"<div class='text-center' id='"+ idElement +"'>"+value+"</div>";
    //     cell.HorizontalAlign = HorizontalAlign.Center;  // Align text to the center
    //     tr.Cells.Add(cell);  // Add the cell to the row
    // }
    // private void AddFooterCellSL(TableRow tr,string idElement, int value,int totExst)
    // {
    //     TableCell cell = new TableCell();
    //     cell.CssClass="text-center";
    //     cell.ID = idElement;
    //     cell.ClientIDMode = ClientIDMode.Static;
    //     cell.Text = (totExst-value).ToString();  // Convert value to string        
    //     tr.Cells.Add(cell);  // Add the cell to the row
    // }
    // private void AddFooterCellTotalSL(TableRow tr,string idElement, int value,int totExst)
    // {
    //     TableCell cell = new TableCell();
    //     cell.ColumnSpan = 1; 
    //     cell.Text = @"<div class='text-center' id='"+ idElement +"'>"+(totExst-value)+"</div>";
    //     cell.HorizontalAlign = HorizontalAlign.Center;  // Align text to the center
    //     tr.Cells.Add(cell);  // Add the cell to the row
    // }

    private void AddFooterCell1(TableRow tr, string idElement)
    {
        TableCell cell = new TableCell();
        cell.ColumnSpan = 1;
        cell.Text = @"<div class='text-center'>
                <input type='number' value='0' class='input-table text-center' readonly id='" + idElement + @"'/>
              </div>";
        cell.HorizontalAlign = HorizontalAlign.Center;  // Align text to the center
        tr.Cells.Add(cell);  // Add the cell to the row
    }    
    private void AddCelltb1(TableRow tr,string idElement, string dataTipe = ""
    , string dataTipeRow="", string dataTipeColumn="", string attributes="", string DataColumnProduct = "", Dictionary<string,int> dict = null)
    {     
        string keypress = "";
        string attributes1 = idElement.StartsWith("OT") ? "readonly onkeypress='return false'" : "";
        int values = 0;
        
        string bgColor = dataTipeColumn.StartsWith("DJ") ? "#FCDCE1" 
               : dataTipeColumn.StartsWith("PG") ? "#E9ECCE" 
               : "#CDE9DC";


        if (dict != null)
        {
            if (dict.ContainsKey(dataTipeColumn))
            {
                values = dict[dataTipeColumn];
            }
            else if (idElement.Equals("TotalDJOT", StringComparison.OrdinalIgnoreCase))
            {                
                values = dict
                        .Where(x => x.Key.StartsWith("DJ"))
                        .Sum(x => x.Value);
            }else if (idElement.Equals("TotalPGOT", StringComparison.OrdinalIgnoreCase))
            {
                values = dict
                        .Where(x => x.Key.StartsWith("PG"))
                        .Sum(x => x.Value);
            }else if (idElement.Equals("TotalLDOT", StringComparison.OrdinalIgnoreCase))
            {                
                values = dict
                        .Where(x => x.Key.StartsWith("LD"))
                        .Sum(x => x.Value);
            }else if (idElement.Equals("TotalALLOT", StringComparison.OrdinalIgnoreCase))
            {             
                values = dict.Values.Sum();
            }else if (dict.ContainsKey(dataTipe.Replace("totalBDDJ","DJ")))
            {                                             
                values = dict[dataTipe.Replace("totalBDDJ","DJ")];
            }else if (dict.ContainsKey(dataTipe.Replace("totalBDPG","PG")))
            {                                             
                values = dict[dataTipe.Replace("totalBDPG","PG")];
            }else if (dict.ContainsKey(dataTipe.Replace("totalBDLD","LD")))
            {                                             
                values = dict[dataTipe.Replace("totalBDLD","LD")];
            }else if (dataTipe.StartsWith("totalBDDJALL"))
            {                                             
                values = dict
                        .Where(x => x.Key.StartsWith("DJ"))
                        .Sum(x => x.Value);
            }else if (dataTipe.StartsWith("totalBDPGALL"))
            {                                             
                values = dict
                        .Where(x => x.Key.StartsWith("PG"))
                        .Sum(x => x.Value);
            }else if (dataTipe.StartsWith("totalBDLDALL"))
            {                                             
                values = dict
                        .Where(x => x.Key.StartsWith("LD"))
                        .Sum(x => x.Value);
            }else if (idElement.Equals("totalBDALL", StringComparison.OrdinalIgnoreCase))
            {             
                values = dict.Values.Sum();
            }
        }


        TableCell cell = new TableCell();
        cell.ColumnSpan = 1;         
        cell.Style.Add("text-align","center");            
        if (attributes == "readonly")
        {
            keypress = "onkeypress='return false'";
        }
        else
        {
            keypress = "oninput='sanitize(this)'";
            cell.Style.Add("background-color", bgColor);            
        }
         
        cell.Text = @"<div class='text-center'>
                <input type='text' inputmode='numeric' min='0' value='"+values+"' class='input-table text-center' "+ keypress  +"  onchange='kalkulasiBarangDalam(`" + dataTipeColumn + "`,`"+ dataTipeRow +  @"`)'  onpaste='return false;'  
                id='" + idElement+ "' data-tipe='" + dataTipe + "' data-row='" + dataTipeRow + "' data-column='"+ dataTipeColumn + "' data-product='"+ DataColumnProduct + "' " + attributes1 + " " + attributes +@" />
              </div>";
        cell.HorizontalAlign = HorizontalAlign.Center;  // Align text to the center
        tr.Cells.Add(cell);  // Add the cell to the row
    }
    private void AddCelltb2(TableRow tr,string idElement, string dataTipe = ""
    , string dataTipeRow="", string dataTipeColumn="", string attributes="")
    {
        string keypress = ""; 
        string bgColor = dataTipeColumn.StartsWith("DJ") ? "#FCDCE1" 
               : dataTipeColumn.StartsWith("PG") ? "#E9ECCE" 
               : "#CDE9DC";       
        TableCell cell = new TableCell();
        cell.ColumnSpan = 1; 
        cell.Style.Add("text-align","center");
         if (attributes == "readonly")
        {
            keypress = "onkeypress='return false'";
        }
        else
        {
            keypress = "oninput='sanitize(this)'";
            cell.Style.Add("background-color", bgColor);            
        }
        cell.Text = @"<div class='text-center'>
                <input type='text' inputmode='numeric' value='0' min='0' class='input-table text-center' "+ keypress  +"  onchange='kalkulasiBarangTitipan(`" + dataTipeColumn + "`,`"+ dataTipeRow + @"`)' onpaste='return false;'
                id='" + idElement + "' data-tipe='" + dataTipe + "' data-row='" + dataTipeRow + "' data-column='"+ dataTipeColumn + "' "+ attributes +@"/>
              </div>";
        cell.HorizontalAlign = HorizontalAlign.Center;  // Align text to the center
        tr.Cells.Add(cell);  // Add the cell to the row
    }
    private void AddCelltb3(TableRow tr,string idElement, string dataTipe = ""
    , string dataTipeRow="", string dataTipeColumn="", string attributes="")
    {
        string keypress = "";        
        TableCell cell = new TableCell();
        string bgColor = dataTipeColumn.StartsWith("DJ") ? "#FCDCE1" 
               : dataTipeColumn.StartsWith("PG") ? "#E9ECCE" 
               : "#CDE9DC";
        cell.ColumnSpan = 1; 
        cell.Style.Add("text-align","center");
            if (attributes == "readonly")
            {
                keypress = "onkeypress='return false'";
            }
            else
            {
                keypress = "oninput='sanitize(this)'";
                cell.Style.Add("background-color", bgColor);            
            }
        cell.Text = @"<div class='text-center'>
                <input type='text' inputmode='numeric' min='0' value='0' class='input-table text-center' "+ keypress  +"  onchange='kalkulasiBarangLuar(`" + dataTipeColumn + "`,`"+ dataTipeRow + @"`)' onpaste='return false;'
                id='" + idElement + "' data-tipe='" + dataTipe + "' data-row='" + dataTipeRow + "' data-column='"+ dataTipeColumn + "' "+ attributes +@"/>
              </div>";
        cell.HorizontalAlign = HorizontalAlign.Center;  // Align text to the center
        tr.Cells.Add(cell);  // Add the cell to the row
    }

    private void generateTabelBarangTitipan(){
        //START RPR1
        TableRow RR = new TableRow();
        RR.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellRR = new TableCell();
        cellRR.Text = "BRG CSTMR AKAN DIREPARASI";        
        cellRR.CssClass = "freeze-column";        
        cellRR.Style.Add("text-align","center");
        RR.Cells.Add(cellRR);

        AddCelltb2(RR, "RRDJBGL","BTDJ","DJRR","DJBGL");
        AddCelltb2(RR, "RRDJBRC","BTDJ","DJRR","DJBRC");
        AddCelltb2(RR, "RRDJEAR","BTDJ","DJRR","DJEAR");
        AddCelltb2(RR, "RRDJLR","BTDJ","DJRR","DJLR");
        AddCelltb2(RR, "RRDJBUCK","BTDJ","DJRR","DJBUCK");
        AddCelltb2(RR, "RRDJMJ","BTDJ","DJRR","DJMJ");
        AddCelltb2(RR, "RRDJNECK","BTDJ","DJRR","DJNECK");
        AddCelltb2(RR, "RRDJBRO","BTDJ","DJRR","DJBRO");
        AddCelltb2(RR, "RRDJPDT","BTDJ","DJRR","DJPDT");
        AddCelltb2(RR, "RRDJPDTC","BTDJ","DJRR","DJPDTC");
        AddCelltb2(RR, "RRDJWR","BTDJ","DJRR","DJWR");
        AddCelltb2(RR, "RRDJOTH","BTDJ","DJRR","DJOTH");
        AddCelltb2(RR, "TotalDJRR","TotalDJBT","TotalDJRR","TotalDJBT","readonly");

        AddCelltb2(RR, "RRPGBGL","BTPG","PGRR","PGBGL");
        AddCelltb2(RR, "RRPGBRC","BTPG","PGRR","PGBRC");
        AddCelltb2(RR, "RRPGEAR","BTPG","PGRR","PGEAR");
        AddCelltb2(RR, "RRPGLR","BTPG","PGRR","PGLR");
        AddCelltb2(RR, "RRPGBUCK","BTPG","PGRR","PGBUCK");
        AddCelltb2(RR, "RRPGMJ","BTPG","PGRR","PGMJ");
        AddCelltb2(RR, "RRPGNECK","BTPG","PGRR","PGNECK");
        AddCelltb2(RR, "RRPGBRO","BTPG","PGRR","PGBRO");
        AddCelltb2(RR, "RRPGPDT","BTPG","PGRR","PGPDT");
        AddCelltb2(RR, "RRPGPDTC","BTPG","PGRR","PGPDTC");
        AddCelltb2(RR, "RRPGWR","BTPG","PGRR","PGWR");
        AddCelltb2(RR, "RRPGOTH","BTPG","PGRR","PGOTH");
        AddCelltb2(RR, "TotalPGRR","TotalPGBT","TotalPGRR","TotalPGBT","readonly");

        AddCelltb2(RR, "RRLDOTH","BTLD","LDRR","LDOTH");
        AddCelltb2(RR, "TotalLDRR","TotalLDBT","TotalLDRR","TotalLDBT","readonly");
        AddCelltb2(RR, "totalALLRR","ALLRR","TotalALLRR","TotalALLBT","readonly");

        tb2.Rows.Add(RR);        
        //END RPR1

        //START RPR2
        TableRow RE = new TableRow();
        RE.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellRE = new TableCell();
        cellRE.Text = "BRG CSTMR DIREPARASI DI PENGRAJIN";        
        cellRE.CssClass = "freeze-column";        
        cellRE.Style.Add("text-align","center");
        RE.Cells.Add(cellRE);
                
        AddCelltb2(RE, "REDJBGL","BTDJ","DJRE","DJBGL");
        AddCelltb2(RE, "REDJBRC","BTDJ","DJRE","DJBRC");
        AddCelltb2(RE, "REDJEAR","BTDJ","DJRE","DJEAR");
        AddCelltb2(RE, "REDJLR","BTDJ","DJRE","DJLR");
        AddCelltb2(RE, "REDJBUCK","BTDJ","DJRE","DJBUCK");
        AddCelltb2(RE, "REDJMJ","BTDJ","DJRE","DJMJ");
        AddCelltb2(RE, "REDJNECK","BTDJ","DJRE","DJNECK");
        AddCelltb2(RE, "REDJBRO","BTDJ","DJRE","DJBRO");
        AddCelltb2(RE, "REDJPDT","BTDJ","DJRE","DJPDT");
        AddCelltb2(RE, "REDJPDTC","BTDJ","DJRE","DJPDTC");
        AddCelltb2(RE, "REDJWR","BTDJ","DJRE","DJWR");
        AddCelltb2(RE, "REDJOTH","BTDJ","DJRE","DJOTH");
        AddCelltb2(RE, "TotalDJRE","TotalDJBT","TotalDJRE","TotalDJBT","readonly");

        AddCelltb2(RE, "REPGBGL","BTPG","PGRE","PGBGL");
        AddCelltb2(RE, "REPGBRC","BTPG","PGRE","PGBRC");
        AddCelltb2(RE, "REPGEAR","BTPG","PGRE","PGEAR");
        AddCelltb2(RE, "REPGLR","BTPG","PGRE","PGLR");
        AddCelltb2(RE, "REPGBUCK","BTPG","PGRE","PGBUCK");
        AddCelltb2(RE, "REPGMJ","BTPG","PGRE","PGMJ");
        AddCelltb2(RE, "REPGNECK","BTPG","PGRE","PGNECK");
        AddCelltb2(RE, "REPGBRO","BTPG","PGRE","PGBRO");
        AddCelltb2(RE, "REPGPDT","BTPG","PGRE","PGPDT");
        AddCelltb2(RE, "REPGPDTC","BTPG","PGRE","PGPDTC");
        AddCelltb2(RE, "REPGWR","BTPG","PGRE","PGWR");
        AddCelltb2(RE, "REPGOTH","BTPG","PGRE","PGOTH");
        AddCelltb2(RE, "TotalPGRE","TotalPGBT","TotalPGRE","TotalPGBT","readonly");

        AddCelltb2(RE, "RELDOTH","BTLD","LDRE","LDOTH");
        AddCelltb2(RE, "TotalLDRE","TotalLDBT","TotalLDRE","TotalLDBT","readonly");
        AddCelltb2(RE, "totalALLRE","ALLRE","TotalALLRE","TotalALLBT","readonly");
    
        tb2.Rows.Add(RE);        
        //END RPR2

        //START RPRDONE
        TableRow RD = new TableRow();
        RD.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellRD = new TableCell();
        cellRD.Text = "BRG CSTMR SUDAH SELESAI REPARASI";        
        cellRD.CssClass = "freeze-column";        
        cellRD.Style.Add("text-align","center");
        RD.Cells.Add(cellRD);
                
        AddCelltb2(RD, "RDDJBGL","BTDJ","DJRD","DJBGL");
        AddCelltb2(RD, "RDDJBRC","BTDJ","DJRD","DJBRC");
        AddCelltb2(RD, "RDDJEAR","BTDJ","DJRD","DJEAR");
        AddCelltb2(RD, "RDDJLR","BTDJ","DJRD","DJLR");
        AddCelltb2(RD, "RDDJBUCK","BTDJ","DJRD","DJBUCK");
        AddCelltb2(RD, "RDDJMJ","BTDJ","DJRD","DJMJ");
        AddCelltb2(RD, "RDDJNECK","BTDJ","DJRD","DJNECK");
        AddCelltb2(RD, "RDDJBRO","BTDJ","DJRD","DJBRO");
        AddCelltb2(RD, "RDDJPDT","BTDJ","DJRD","DJPDT");
        AddCelltb2(RD, "RDDJPDTC","BTDJ","DJRD","DJPDTC");
        AddCelltb2(RD, "RDDJWR","BTDJ","DJRD","DJWR");
        AddCelltb2(RD, "RDDJOTH","BTDJ","DJRD","DJOTH");
        AddCelltb2(RD, "TotalDJRD","TotalDJBT","TotalDJRD","TotalDJBT","readonly");

        AddCelltb2(RD, "RDPGBGL","BTPG","PGRD","PGBGL");
        AddCelltb2(RD, "RDPGBRC","BTPG","PGRD","PGBRC");
        AddCelltb2(RD, "RDPGEAR","BTPG","PGRD","PGEAR");
        AddCelltb2(RD, "RDPGLR","BTPG","PGRD","PGLR");
        AddCelltb2(RD, "RDPGBUCK","BTPG","PGRD","PGBUCK");
        AddCelltb2(RD, "RDPGMJ","BTPG","PGRD","PGMJ");
        AddCelltb2(RD, "RDPGNECK","BTPG","PGRD","PGNECK");
        AddCelltb2(RD, "RDPGBRO","BTPG","PGRD","PGBRO");
        AddCelltb2(RD, "RDPGPDT","BTPG","PGRD","PGPDT");
        AddCelltb2(RD, "RDPGPDTC","BTPG","PGRD","PGPDTC");
        AddCelltb2(RD, "RDPGWR","BTPG","PGRD","PGWR");
        AddCelltb2(RD, "RDPGOTH","BTPG","PGRD","PGOTH");
        AddCelltb2(RD, "TotalPGRD","TotalPGBT","TotalPGRD","TotalPGBT","readonly");

        AddCelltb2(RD, "RDLDOTH","BTLD","LDRD","LDOTH");
        AddCelltb2(RD, "TotalLDRD","TotalLDBT","TotalLDRD","TotalLDBT","readonly");
        AddCelltb2(RD, "totalALLRD","ALLRD","TotalALLRD","TotalALLBT","readonly");
    
        tb2.Rows.Add(RD);        
        //END RPRDONE

        //START TCSTMR
        TableRow TC = new TableRow();
        TC.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellTC = new TableCell();
        cellTC.Text = "TITIPAN CUSTOMER";        
        cellTC.CssClass = "freeze-column";        
        cellTC.Style.Add("text-align","center");
        TC.Cells.Add(cellTC);
                
        AddCelltb2(TC, "TCDJBGL","BTDJ","DJTC","DJBGL");
        AddCelltb2(TC, "TCDJBRC","BTDJ","DJTC","DJBRC");
        AddCelltb2(TC, "TCDJEAR","BTDJ","DJTC","DJEAR");
        AddCelltb2(TC, "TCDJLR","BTDJ","DJTC","DJLR");
        AddCelltb2(TC, "TCDJBUCK","BTDJ","DJTC","DJBUCK");
        AddCelltb2(TC, "TCDJMJ","BTDJ","DJTC","DJMJ");
        AddCelltb2(TC, "TCDJNECK","BTDJ","DJTC","DJNECK");
        AddCelltb2(TC, "TCDJBRO","BTDJ","DJTC","DJBRO");
        AddCelltb2(TC, "TCDJPDT","BTDJ","DJTC","DJPDT");
        AddCelltb2(TC, "TCDJPDTC","BTDJ","DJTC","DJPDTC");
        AddCelltb2(TC, "TCDJWR","BTDJ","DJTC","DJWR");
        AddCelltb2(TC, "TCDJOTH","BTDJ","DJTC","DJOTH");
        AddCelltb2(TC, "TotalDJTC","TotalDJBT","TotalDJTC","TotalDJBT","readonly");

        AddCelltb2(TC, "TCPGBGL","BTPG","PGTC","PGBGL");
        AddCelltb2(TC, "TCPGBRC","BTPG","PGTC","PGBRC");
        AddCelltb2(TC, "TCPGEAR","BTPG","PGTC","PGEAR");
        AddCelltb2(TC, "TCPGLR","BTPG","PGTC","PGLR");
        AddCelltb2(TC, "TCPGBUCK","BTPG","PGTC","PGBUCK");
        AddCelltb2(TC, "TCPGMJ","BTPG","PGTC","PGMJ");
        AddCelltb2(TC, "TCPGNECK","BTPG","PGTC","PGNECK");
        AddCelltb2(TC, "TCPGBRO","BTPG","PGTC","PGBRO");
        AddCelltb2(TC, "TCPGPDT","BTPG","PGTC","PGPDT");
        AddCelltb2(TC, "TCPGPDTC","BTPG","PGTC","PGPDTC");
        AddCelltb2(TC, "TCPGWR","BTPG","PGTC","PGWR");
        AddCelltb2(TC, "TCPGOTH","BTPG","PGTC","PGOTH");
        AddCelltb2(TC, "TotalPGTC","TotalPGBT","TotalPGTC","TotalPGBT","readonly");

        AddCelltb2(TC, "TCLDOTH","BTLD","LDTC","LDOTH");
        AddCelltb2(TC, "TotalLDTC","TotalLDBT","TotalLDTC","TotalLDBT","readonly");
        AddCelltb2(TC, "totalALLTC","ALLTC","TotalALLTC","TotalALLBT","readonly");
    
        tb2.Rows.Add(TC);        
        //END TCSTMR

        //START LMLEI
        TableRow LM = new TableRow();
        LM.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellLM = new TableCell();
        cellLM.Text = "Barang Stock LM LEI";        
        cellLM.CssClass = "freeze-column";        
        cellLM.Style.Add("text-align","center");
        LM.Cells.Add(cellLM);
                
        AddCelltb2(LM, "LMDJBGL","BTDJ","DJLM","DJBGL");
        AddCelltb2(LM, "LMDJBRC","BTDJ","DJLM","DJBRC");
        AddCelltb2(LM, "LMDJEAR","BTDJ","DJLM","DJEAR");
        AddCelltb2(LM, "LMDJLR","BTDJ","DJLM","DJLR");
        AddCelltb2(LM, "LMDJBUCK","BTDJ","DJLM","DJBUCK");
        AddCelltb2(LM, "LMDJMJ","BTDJ","DJLM","DJMJ");
        AddCelltb2(LM, "LMDJNECK","BTDJ","DJLM","DJNECK");
        AddCelltb2(LM, "LMDJBRO","BTDJ","DJLM","DJBRO");
        AddCelltb2(LM, "LMDJPDT","BTDJ","DJLM","DJPDT");
        AddCelltb2(LM, "LMDJPDTC","BTDJ","DJLM","DJPDTC");
        AddCelltb2(LM, "LMDJWR","BTDJ","DJLM","DJWR");
        AddCelltb2(LM, "LMDJOTH","BTDJ","DJLM","DJOTH");
        AddCelltb2(LM, "TotalDJLM","TotalDJBT","TotalDJLM","TotalDJBT","readonly");

        AddCelltb2(LM, "LMPGBGL","BTPG","PGLM","PGBGL");
        AddCelltb2(LM, "LMPGBRC","BTPG","PGLM","PGBRC");
        AddCelltb2(LM, "LMPGEAR","BTPG","PGLM","PGEAR");
        AddCelltb2(LM, "LMPGLR","BTPG","PGLM","PGLR");
        AddCelltb2(LM, "LMPGBUCK","BTPG","PGLM","PGBUCK");
        AddCelltb2(LM, "LMPGMJ","BTPG","PGLM","PGMJ");
        AddCelltb2(LM, "LMPGNECK","BTPG","PGLM","PGNECK");
        AddCelltb2(LM, "LMPGBRO","BTPG","PGLM","PGBRO");
        AddCelltb2(LM, "LMPGPDT","BTPG","PGLM","PGPDT");
        AddCelltb2(LM, "LMPGPDTC","BTPG","PGLM","PGPDTC");
        AddCelltb2(LM, "LMPGWR","BTPG","PGLM","PGWR");
        AddCelltb2(LM, "LMPGOTH","BTPG","PGLM","PGOTH");
        AddCelltb2(LM, "TotalPGLM","TotalPGBT","TotalPGLM","TotalPGBT","readonly");

        AddCelltb2(LM, "LMLDOTH","BTLD","LDLM","LDOTH");
        AddCelltb2(LM, "TotalLDLM","TotalLDBT","TotalLDLM","TotalLDBT","readonly");
        AddCelltb2(LM, "totalALLLM","ALLLM","TotalALLLM","TotalALLBT","readonly");
    
        tb2.Rows.Add(LM);        
        //END LMLEI

        //START LMLEICSTMR
        TableRow LC = new TableRow();
        LC.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellLC = new TableCell();
        cellLC.Text = "Titipan CSTMR LM LEI";        
        cellLC.CssClass = "freeze-column";        
        cellLC.Style.Add("text-align","center");
        LC.Cells.Add(cellLC);
                
        AddCelltb2(LC, "LCDJBGL","BTDJ","DJLC","DJBGL");
        AddCelltb2(LC, "LCDJBRC","BTDJ","DJLC","DJBRC");
        AddCelltb2(LC, "LCDJEAR","BTDJ","DJLC","DJEAR");
        AddCelltb2(LC, "LCDJLR","BTDJ","DJLC","DJLR");
        AddCelltb2(LC, "LCDJBUCK","BTDJ","DJLC","DJBUCK");
        AddCelltb2(LC, "LCDJMJ","BTDJ","DJLC","DJMJ");
        AddCelltb2(LC, "LCDJNECK","BTDJ","DJLC","DJNECK");
        AddCelltb2(LC, "LCDJBRO","BTDJ","DJLC","DJBRO");
        AddCelltb2(LC, "LCDJPDT","BTDJ","DJLC","DJPDT");
        AddCelltb2(LC, "LCDJPDTC","BTDJ","DJLC","DJPDTC");
        AddCelltb2(LC, "LCDJWR","BTDJ","DJLC","DJWR");
        AddCelltb2(LC, "LCDJOTH","BTDJ","DJLC","DJOTH");
        AddCelltb2(LC, "TotalDJLC","TotalDJBT","TotalDJLC","TotalDJBT","readonly");

        AddCelltb2(LC, "LCPGBGL","BTPG","PGLC","PGBGL");
        AddCelltb2(LC, "LCPGBRC","BTPG","PGLC","PGBRC");
        AddCelltb2(LC, "LCPGEAR","BTPG","PGLC","PGEAR");
        AddCelltb2(LC, "LCPGLR","BTPG","PGLC","PGLR");
        AddCelltb2(LC, "LCPGBUCK","BTPG","PGLC","PGBUCK");
        AddCelltb2(LC, "LCPGMJ","BTPG","PGLC","PGMJ");
        AddCelltb2(LC, "LCPGNECK","BTPG","PGLC","PGNECK");
        AddCelltb2(LC, "LCPGBRO","BTPG","PGLC","PGBRO");
        AddCelltb2(LC, "LCPGPDT","BTPG","PGLC","PGPDT");
        AddCelltb2(LC, "LCPGPDTC","BTPG","PGLC","PGPDTC");
        AddCelltb2(LC, "LCPGWR","BTPG","PGLC","PGWR");
        AddCelltb2(LC, "LCPGOTH","BTPG","PGLC","PGOTH");
        AddCelltb2(LC, "TotalPGLC","TotalPGBT","TotalPGLC","TotalPGBT","readonly");

        AddCelltb2(LC, "LCLDOTH","BTLD","LDLC","LDOTH");
        AddCelltb2(LC, "TotalLDLC","TotalLDBT","TotalLDLC","TotalLDBT","readonly");
        AddCelltb2(LC, "totalALLLC","ALLLC","TotalALLLC","TotalALLBT","readonly");
    
        tb2.Rows.Add(LC);        
        //END LMLEICSTMR       

        // Footer Total barang titipan
        TableRow BT = new TableRow();
        BT.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter             
        BT.CssClass = "freeze-footer";   
        TableCell cellBT = new TableCell();
        cellBT.Text = "Total Barang Titipan";        
        cellBT.CssClass = "freeze-column text-center";            
        BT.Cells.Add(cellBT);
        //dj        
        AddCelltb2(BT, "TotalBTDJBGL","totalBTDJBGL", "", "", "readonly");
        AddCelltb2(BT, "TotalBTDJBRC","totalBTDJBRC", "", "", "readonly");
        AddCelltb2(BT, "TotalBTDJEAR","totalBTDJEAR", "", "", "readonly");
        AddCelltb2(BT, "TotalBTDJLR","totalBTDJLR", "", "", "readonly");
        AddCelltb2(BT, "TotalBTDJBUCK","totalBTDJBUCK", "", "", "readonly");
        AddCelltb2(BT, "TotalBTDJMJ","totalBTDJMJ", "", "", "readonly");
        AddCelltb2(BT, "TotalBTDJNECK","totalBTDJNECK", "", "", "readonly");
        AddCelltb2(BT, "TotalBTDJBRO","totalBTDJBRO", "", "", "readonly");
        AddCelltb2(BT, "TotalBTDJPDT","totalBTDJPDT", "", "", "readonly");
        AddCelltb2(BT, "TotalBTDJPDTC","totalBTDJPDTC", "", "", "readonly");
        AddCelltb2(BT, "TotalBTDJWR","totalBTDJWR", "", "", "readonly");
        AddCelltb2(BT, "TotalBTDJOTH","totalBTDJOTH", "", "", "readonly"); 
        AddCelltb2(BT,"TotalBTDJ","totalBTDJALL", "", "", "readonly");
                                                        
    
        AddCelltb2(BT, "TotalBTPGBGL","totalBTPGBGL", "", "", "readonly");
        AddCelltb2(BT, "TotalBTPGBRC","totalBTPGBRC", "", "", "readonly");
        AddCelltb2(BT, "TotalBTPGEAR","totalBTPGEAR", "", "", "readonly");
        AddCelltb2(BT, "TotalBTPGLR","totalBTPGLR", "", "", "readonly");
        AddCelltb2(BT, "TotalBTPGBUCK","totalBTPGBUCK", "", "", "readonly");
        AddCelltb2(BT, "TotalBTPGMJ","totalBTPGMJ", "", "", "readonly");
        AddCelltb2(BT, "TotalBTPGNECK","totalBTPGNECK", "", "", "readonly");
        AddCelltb2(BT, "TotalBTPGBRO","totalBTPGBRO", "", "", "readonly");
        AddCelltb2(BT, "TotalBTPGPDT","totalBTPGPDT", "", "", "readonly");
        AddCelltb2(BT, "TotalBTPGPDTC","totalBTPGPDTC", "", "", "readonly");
        AddCelltb2(BT, "TotalBTPGWR","totalBTPGWR", "", "", "readonly");
        AddCelltb2(BT, "TotalBTPGOTH","totalBTPGOTH", "", "", "readonly");       
        AddCelltb2(BT,"TotalBTPG","totalBTPGALL", "", "", "readonly");
    
        AddCelltb2(BT, "TotalBTLDOTH","totalBTLDOTH", "", "", "readonly");     
        AddCelltb2(BT,"TotalBTLD","totalBTLDALL", "", "", "readonly"); 

        AddCelltb2(BT,"TotalBTALL","totalBTALL", "", "", "readonly");     
        
        tb2.Rows.Add(BT);   
    }
    
    private void generateTabelBarangluar(){        
        //START PTLAIN
        TableRow TL = new TableRow();
        TL.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellTL = new TableCell();
        cellTL.Text = "Pinjaman Dari Toko Lain";        
        cellTL.CssClass = "freeze-column";        
        cellTL.Style.Add("text-align","center");
        TL.Cells.Add(cellTL);

        AddCelltb3(TL, "TLDJBGL","BLDJ","DJTL","DJBGL");
        AddCelltb3(TL, "TLDJBRC","BLDJ","DJTL","DJBRC");
        AddCelltb3(TL, "TLDJEAR","BLDJ","DJTL","DJEAR");
        AddCelltb3(TL, "TLDJLR","BLDJ","DJTL","DJLR");
        AddCelltb3(TL, "TLDJBUCK","BLDJ","DJTL","DJBUCK");
        AddCelltb3(TL, "TLDJMJ","BLDJ","DJTL","DJMJ");
        AddCelltb3(TL, "TLDJNECK","BLDJ","DJTL","DJNECK");
        AddCelltb3(TL, "TLDJBRO","BLDJ","DJTL","DJBRO");
        AddCelltb3(TL, "TLDJPDT","BLDJ","DJTL","DJPDT");
        AddCelltb3(TL, "TLDJPDTC","BLDJ","DJTL","DJPDTC");
        AddCelltb3(TL, "TLDJWR","BLDJ","DJTL","DJWR");
        AddCelltb3(TL, "TLDJOTH","BLDJ","DJTL","DJOTH");
        AddCelltb3(TL, "TotalDJTL","TotalDJBL","TotalDJTL","TotalDJBL","readonly");

        AddCelltb3(TL, "TLPGBGL","BLPG","PGTL","PGBGL");
        AddCelltb3(TL, "TLPGBRC","BLPG","PGTL","PGBRC");
        AddCelltb3(TL, "TLPGEAR","BLPG","PGTL","PGEAR");
        AddCelltb3(TL, "TLPGLR","BLPG","PGTL","PGLR");
        AddCelltb3(TL, "TLPGBUCK","BLPG","PGTL","PGBUCK");
        AddCelltb3(TL, "TLPGMJ","BLPG","PGTL","PGMJ");
        AddCelltb3(TL, "TLPGNECK","BLPG","PGTL","PGNECK");
        AddCelltb3(TL, "TLPGBRO","BLPG","PGTL","PGBRO");
        AddCelltb3(TL, "TLPGPDT","BLPG","PGTL","PGPDT");
        AddCelltb3(TL, "TLPGPDTC","BLPG","PGTL","PGPDTC");
        AddCelltb3(TL, "TLPGWR","BLPG","PGTL","PGWR");
        AddCelltb3(TL, "TLPGOTH","BLPG","PGTL","PGOTH");
        AddCelltb3(TL, "TotalPGTL","TotalPGBL","TotalPGTL","TotalPGBL","readonly");

        AddCelltb3(TL, "TLLDOTH","BLLD","LDTL","LDOTH");
        AddCelltb3(TL, "TotalLDTL","TotalLDBL","TotalLDTL","TotalLDBL","readonly");
        AddCelltb3(TL, "totalALLTL","ALLTL","TotalALLTL","TotalALLBL","readonly");

        tb3.Rows.Add(TL);        
        //END PTLAIN

        //START OTB
        TableRow GT = new TableRow();
        GT.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellGT = new TableCell();
        cellGT.Text = "RESELL / GOLD TUNAI";
        cellGT.CssClass = "freeze-column";        
        cellGT.Style.Add("text-align","center");
        GT.Cells.Add(cellGT);
                
        AddCelltb3(GT, "GTDJBGL","BLDJ","DJGT","DJBGL");
        AddCelltb3(GT, "GTDJBRC","BLDJ","DJGT","DJBRC");
        AddCelltb3(GT, "GTDJEAR","BLDJ","DJGT","DJEAR");
        AddCelltb3(GT, "GTDJLR","BLDJ","DJGT","DJLR");
        AddCelltb3(GT, "GTDJBUCK","BLDJ","DJGT","DJBUCK");
        AddCelltb3(GT, "GTDJMJ","BLDJ","DJGT","DJMJ");
        AddCelltb3(GT, "GTDJNECK","BLDJ","DJGT","DJNECK");
        AddCelltb3(GT, "GTDJBRO","BLDJ","DJGT","DJBRO");
        AddCelltb3(GT, "GTDJPDT","BLDJ","DJGT","DJPDT");
        AddCelltb3(GT, "GTDJPDTC","BLDJ","DJGT","DJPDTC");
        AddCelltb3(GT, "GTDJWR","BLDJ","DJGT","DJWR");
        AddCelltb3(GT, "GTDJOTH","BLDJ","DJGT","DJOTH");
        AddCelltb3(GT, "TotalDJGT","TotalDJBL","TotalDJGT","TotalDJBL","readonly");

        AddCelltb3(GT, "GTPGBGL","BLPG","PGGT","PGBGL");
        AddCelltb3(GT, "GTPGBRC","BLPG","PGGT","PGBRC");
        AddCelltb3(GT, "GTPGEAR","BLPG","PGGT","PGEAR");
        AddCelltb3(GT, "GTPGLR","BLPG","PGGT","PGLR");
        AddCelltb3(GT, "GTPGBUCK","BLPG","PGGT","PGBUCK");
        AddCelltb3(GT, "GTPGMJ","BLPG","PGGT","PGMJ");
        AddCelltb3(GT, "GTPGNECK","BLPG","PGGT","PGNECK");
        AddCelltb3(GT, "GTPGBRO","BLPG","PGGT","PGBRO");
        AddCelltb3(GT, "GTPGPDT","BLPG","PGGT","PGPDT");
        AddCelltb3(GT, "GTPGPDTC","BLPG","PGGT","PGPDTC");
        AddCelltb3(GT, "GTPGWR","BLPG","PGGT","PGWR");
        AddCelltb3(GT, "GTPGOTH","BLPG","PGGT","PGOTH");
        AddCelltb3(GT, "TotalPGGT","TotalPGBL","TotalPGGT","TotalPGBL","readonly");

        AddCelltb3(GT, "GTLDOTH","BLLD","LDGT","LDOTH");
        AddCelltb3(GT, "TotalLDGT","TotalLDBL","TotalLDGT","TotalLDBL","readonly");
        AddCelltb3(GT, "totalALLGT","ALLGT","TotalALLGT","TotalALLBL","readonly");
    
        tb3.Rows.Add(GT);        
        //END OTB

        //START GPLEI
        TableRow GP = new TableRow();
        GP.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellGP = new TableCell();
        cellGP.Text = "GOLD PRIORITY LEI";        
        cellGP.CssClass = "freeze-column";        
        cellGP.Style.Add("text-align","center");
        GP.Cells.Add(cellGP);
                
        AddCelltb3(GP, "GPDJBGL","BLDJ","DJGP","DJBGL");
        AddCelltb3(GP, "GPDJBRC","BLDJ","DJGP","DJBRC");
        AddCelltb3(GP, "GPDJEAR","BLDJ","DJGP","DJEAR");
        AddCelltb3(GP, "GPDJLR","BLDJ","DJGP","DJLR");
        AddCelltb3(GP, "GPDJBUCK","BLDJ","DJGP","DJBUCK");
        AddCelltb3(GP, "GPDJMJ","BLDJ","DJGP","DJMJ");
        AddCelltb3(GP, "GPDJNECK","BLDJ","DJGP","DJNECK");
        AddCelltb3(GP, "GPDJBRO","BLDJ","DJGP","DJBRO");
        AddCelltb3(GP, "GPDJPDT","BLDJ","DJGP","DJPDT");
        AddCelltb3(GP, "GPDJPDTC","BLDJ","DJGP","DJPDTC");
        AddCelltb3(GP, "GPDJWR","BLDJ","DJGP","DJWR");
        AddCelltb3(GP, "GPDJOTH","BLDJ","DJGP","DJOTH");
        AddCelltb3(GP, "TotalDJGP","TotalDJBL","TotalDJGP","TotalDJBL","readonly");

        AddCelltb3(GP, "GPPGBGL","BLPG","PGGP","PGBGL");
        AddCelltb3(GP, "GPPGBRC","BLPG","PGGP","PGBRC");
        AddCelltb3(GP, "GPPGEAR","BLPG","PGGP","PGEAR");
        AddCelltb3(GP, "GPPGLR","BLPG","PGGP","PGLR");
        AddCelltb3(GP, "GPPGBUCK","BLPG","PGGP","PGBUCK");
        AddCelltb3(GP, "GPPGMJ","BLPG","PGGP","PGMJ");
        AddCelltb3(GP, "GPPGNECK","BLPG","PGGP","PGNECK");
        AddCelltb3(GP, "GPPGBRO","BLPG","PGGP","PGBRO");
        AddCelltb3(GP, "GPPGPDT","BLPG","PGGP","PGPDT");
        AddCelltb3(GP, "GPPGPDTC","BLPG","PGGP","PGPDTC");
        AddCelltb3(GP, "GPPGWR","BLPG","PGGP","PGWR");
        AddCelltb3(GP, "GPPGOTH","BLPG","PGGP","PGOTH");
        AddCelltb3(GP, "TotalPGGP","TotalPGBL","TotalPGGP","TotalPGBL","readonly");

        AddCelltb3(GP, "GPLDOTH","BLLD","LDGP","LDOTH");
        AddCelltb3(GP, "TotalLDGP","TotalLDBL","TotalLDGP","TotalLDBL","readonly");
        AddCelltb3(GP, "totalALLGP","ALLGP","TotalALLGP","TotalALLBL","readonly");
    
        tb3.Rows.Add(GP);        
        //END GPLEI

        // Footer Total barang titipan
        TableRow BL = new TableRow();
        BL.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter             
        BL.CssClass = "freeze-footer";   
        TableCell cellBL = new TableCell();
        cellBL.Text = "Total Barang Luar Sistem";        
        cellBL.CssClass = "freeze-column text-center";
        BL.Cells.Add(cellBL);
        //dj        
        AddCelltb3(BL, "TotalBLDJBGL","totalBLDJBGL", "", "", "readonly");
        AddCelltb3(BL, "TotalBLDJBRC","totalBLDJBRC", "", "", "readonly");
        AddCelltb3(BL, "TotalBLDJEAR","totalBLDJEAR", "", "", "readonly");
        AddCelltb3(BL, "TotalBLDJLR","totalBLDJLR", "", "", "readonly");
        AddCelltb3(BL, "TotalBLDJBUCK","totalBLDJBUCK", "", "", "readonly");
        AddCelltb3(BL, "TotalBLDJMJ","totalBLDJMJ", "", "", "readonly");
        AddCelltb3(BL, "TotalBLDJNECK","totalBLDJNECK", "", "", "readonly");
        AddCelltb3(BL, "TotalBLDJBRO","totalBLDJBRO", "", "", "readonly");
        AddCelltb3(BL, "TotalBLDJPDT","totalBLDJPDT", "", "", "readonly");
        AddCelltb3(BL, "TotalBLDJPDTC","totalBLDJPDTC", "", "", "readonly");
        AddCelltb3(BL, "TotalBLDJWR","totalBLDJWR", "", "", "readonly");
        AddCelltb3(BL, "TotalBLDJOTH","totalBLDJOTH", "", "", "readonly"); 
        AddCelltb3(BL,"TotalBLDJ","totalBLDJALL", "", "", "readonly");
                                                        
    
        AddCelltb3(BL, "TotalBLPGBGL","totalBLPGBGL", "", "", "readonly");
        AddCelltb3(BL, "TotalBLPGBRC","totalBLPGBRC", "", "", "readonly");
        AddCelltb3(BL, "TotalBLPGEAR","totalBLPGEAR", "", "", "readonly");
        AddCelltb3(BL, "TotalBLPGLR","totalBLPGLR", "", "", "readonly");
        AddCelltb3(BL, "TotalBLPGBUCK","totalBLPGBUCK", "", "", "readonly");
        AddCelltb3(BL, "TotalBLPGMJ","totalBLPGMJ", "", "", "readonly");
        AddCelltb3(BL, "TotalBLPGNECK","totalBLPGNECK", "", "", "readonly");
        AddCelltb3(BL, "TotalBLPGBRO","totalBLPGBRO", "", "", "readonly");
        AddCelltb3(BL, "TotalBLPGPDT","totalBLPGPDT", "", "", "readonly");
        AddCelltb3(BL, "TotalBLPGPDTC","totalBLPGPDTC", "", "", "readonly");
        AddCelltb3(BL, "TotalBLPGWR","totalBLPGWR", "", "", "readonly");
        AddCelltb3(BL, "TotalBLPGOTH","totalBLPGOTH", "", "", "readonly");       
        AddCelltb3(BL,"TotalBLPG","totalBLPGALL", "", "", "readonly");
    
        AddCelltb3(BL, "TotalBLLDOTH","totalBLLDOTH", "", "", "readonly");     
        AddCelltb3(BL,"TotalBLLD","totalBLLDALL", "", "", "readonly"); 

        AddCelltb3(BL,"TotalBLALL","totalBLALL", "", "", "readonly");       
        
        tb3.Rows.Add(BL);   
    }
    
    private void generateTabelBarangDalam(int locationId){
        //START DP
        TableRow DP = new TableRow();
        DP.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter        
        TableCell cellDP = new TableCell();
        cellDP.Text = "DP";        
        cellDP.CssClass = "freeze-column ";        
        cellDP.Style.Add("text-align","center");
        DP.Cells.Add(cellDP);
        //AddCelltb1(DP, "DATAELEMENT","DATATIPE","DATAROW","DATACOLUMN");

        AddCelltb1(DP, "DPDJBGL","BDDJ","DJDP","DJBGL","","BDDJBGL");
        AddCelltb1(DP, "DPDJBRC","BDDJ","DJDP","DJBRC","","BDDJBRC");
        AddCelltb1(DP, "DPDJEAR","BDDJ","DJDP","DJEAR","","BDDJEAR");
        AddCelltb1(DP, "DPDJLR","BDDJ","DJDP","DJLR","","BDDJLR");
        AddCelltb1(DP, "DPDJBUCK","BDDJ","DJDP","DJBUCK","","BDDJBUCK");
        AddCelltb1(DP, "DPDJMJ","BDDJ","DJDP","DJMJ","","BDDJMJ");
        AddCelltb1(DP, "DPDJNECK","BDDJ","DJDP","DJNECK","","BDDJNECK");
        AddCelltb1(DP, "DPDJBRO","BDDJ","DJDP","DJBRO","","BDDJBRO");
        AddCelltb1(DP, "DPDJPDT","BDDJ","DJDP","DJPDT","","BDDJPDT");
        AddCelltb1(DP, "DPDJPDTC","BDDJ","DJDP","DJPDTC","","BDDJPDTC");
        AddCelltb1(DP, "DPDJWR","BDDJ","DJDP","DJWR","","BDDJWR");
        AddCelltb1(DP, "DPDJOTH","BDDJ","DJDP","DJOTH","","BDDJOTH");
        AddCelltb1(DP, "TotalDJDP","TotalDJBD","TotalDJDP","TotalDJBD","readonly");

        AddCelltb1(DP, "DPPGBGL","BDPG","PGDP","PGBGL","","BDPGBGL");
        AddCelltb1(DP, "DPPGBRC","BDPG","PGDP","PGBRC","","BDPGBRC");
        AddCelltb1(DP, "DPPGEAR","BDPG","PGDP","PGEAR","","BDPGEAR");
        AddCelltb1(DP, "DPPGLR","BDPG","PGDP","PGLR","","BDPGLR");
        AddCelltb1(DP, "DPPGBUCK","BDPG","PGDP","PGBUCK","","BDPGBUCK");
        AddCelltb1(DP, "DPPGMJ","BDPG","PGDP","PGMJ","","BDPGMJ");
        AddCelltb1(DP, "DPPGNECK","BDPG","PGDP","PGNECK","","BDPGNECK");
        AddCelltb1(DP, "DPPGBRO","BDPG","PGDP","PGBRO","","BDPGBRO");
        AddCelltb1(DP, "DPPGPDT","BDPG","PGDP","PGPDT","","BDPGPDT");
        AddCelltb1(DP, "DPPGPDTC","BDPG","PGDP","PGPDTC","","BDPGPDTC");
        AddCelltb1(DP, "DPPGWR","BDPG","PGDP","PGWR","","BDPGWR");
        AddCelltb1(DP, "DPPGOTH","BDPG","PGDP","PGOTH","","BDPGOTH");
        AddCelltb1(DP, "TotalPGDP","TotalPGBD","TotalPGDP","TotalPGBD","readonly");

        AddCelltb1(DP, "DPLDOTH","BDLD","LDDP","LDOTH","","BDPLDOTH");
        AddCelltb1(DP, "TotalLDDP","TotalLDBD","TotalLDDP","TotalLDBD","readonly");
        AddCelltb1(DP, "totalALLDP","ALLDP","TotalALLDP","TotalALLBD","readonly");

        tb1.Rows.Add(DP);        
        //END DP        



        //START BRANKAS(BK)
        TableRow BK = new TableRow();
        BK.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellBK = new TableCell();
        cellBK.Text = "BRANKAS (BUFFER STOK)";        
        cellBK.CssClass = "freeze-column";        
        cellBK.Style.Add("text-align","center");
        BK.Cells.Add(cellBK);
                
        AddCelltb1(BK, "BKDJBGL","BDDJ","DJBK","DJBGL","","BDDJBGL");
        AddCelltb1(BK, "BKDJBRC","BDDJ","DJBK","DJBRC","","BDDJBRC");
        AddCelltb1(BK, "BKDJEAR","BDDJ","DJBK","DJEAR","","BDDJEAR");
        AddCelltb1(BK, "BKDJLR","BDDJ","DJBK","DJLR","","BDDJLR");
        AddCelltb1(BK, "BKDJBUCK","BDDJ","DJBK","DJBUCK","","BDDJBUCK");
        AddCelltb1(BK, "BKDJMJ","BDDJ","DJBK","DJMJ","","BDDJMJ");
        AddCelltb1(BK, "BKDJNECK","BDDJ","DJBK","DJNECK","","BDDJNECK");
        AddCelltb1(BK, "BKDJBRO","BDDJ","DJBK","DJBRO","","BDDJBRO");
        AddCelltb1(BK, "BKDJPDT","BDDJ","DJBK","DJPDT","","BDDJPDT");
        AddCelltb1(BK, "BKDJPDTC","BDDJ","DJBK","DJPDTC","","BDDJPDTC");
        AddCelltb1(BK, "BKDJWR","BDDJ","DJBK","DJWR","","BDDJWR");
        AddCelltb1(BK, "BKDJOTH","BDDJ","DJBK","DJOTH","","BDDJOTH");
        AddCelltb1(BK, "TotalDJBK","TotalDJBD","TotalDJBK","TotalDJBD","readonly");

        AddCelltb1(BK, "BKPGBGL","BDPG","PGBK","PGBGL","","BDPGBGL");
        AddCelltb1(BK, "BKPGBRC","BDPG","PGBK","PGBRC","","BDPGBRC");
        AddCelltb1(BK, "BKPGEAR","BDPG","PGBK","PGEAR","","BDPGEAR");
        AddCelltb1(BK, "BKPGLR","BDPG","PGBK","PGLR","","BDPGLR");
        AddCelltb1(BK, "BKPGBUCK","BDPG","PGBK","PGBUCK","","BDPGBUCK");
        AddCelltb1(BK, "BKPGMJ","BDPG","PGBK","PGMJ","","BDPGMJ");
        AddCelltb1(BK, "BKPGNECK","BDPG","PGBK","PGNECK","","BDPGNECK");
        AddCelltb1(BK, "BKPGBRO","BDPG","PGBK","PGBRO","","BDPGBRO");
        AddCelltb1(BK, "BKPGPDT","BDPG","PGBK","PGPDT","","BDPGPDT");
        AddCelltb1(BK, "BKPGPDTC","BDPG","PGBK","PGPDTC","","BDPGPDTC");
        AddCelltb1(BK, "BKPGWR","BDPG","PGBK","PGWR","","BDPGWR");
        AddCelltb1(BK, "BKPGOTH","BDPG","PGBK","PGOTH","","BDPGOTH");
        AddCelltb1(BK, "TotalPGBK","TotalPGBD","TotalPGBK","TotalPGBD","readonly");

        AddCelltb1(BK, "BKLDOTH","BDLD","LDBK","LDOTH","","BDLDOTH");
        AddCelltb1(BK, "TotalLDBK","TotalLDBD","TotalLDBK","TotalLDBD","readonly");
        AddCelltb1(BK, "totalALLBK","ALLBK","TotalALLBK","TotalALLBD","readonly");
    
        tb1.Rows.Add(BK);        
        //END VAULT

        //START SIMPANAN(SV)
        TableRow SV = new TableRow();
        SV.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellSV = new TableCell();
        cellSV.Text = "Simpanan";      
        cellSV.CssClass = "freeze-column";        
        cellSV.Style.Add("text-align","center");
        SV.Cells.Add(cellSV);
                
        AddCelltb1(SV, "SVDJBGL","BDDJ","DJSV","DJBGL","","BDDJBGL");
        AddCelltb1(SV, "SVDJBRC","BDDJ","DJSV","DJBRC","","BDDJBRC");
        AddCelltb1(SV, "SVDJEAR","BDDJ","DJSV","DJEAR","","BDDJEAR");
        AddCelltb1(SV, "SVDJLR","BDDJ","DJSV","DJLR","","BDDJLR");
        AddCelltb1(SV, "SVDJBUCK","BDDJ","DJSV","DJBUCK","","BDDJBUCK");
        AddCelltb1(SV, "SVDJMJ","BDDJ","DJSV","DJMJ","","BDDJMJ");
        AddCelltb1(SV, "SVDJNECK","BDDJ","DJSV","DJNECK","","BDDJNECK");
        AddCelltb1(SV, "SVDJBRO","BDDJ","DJSV","DJBRO","","BDDJBRO");
        AddCelltb1(SV, "SVDJPDT","BDDJ","DJSV","DJPDT","","BDDJPDT");
        AddCelltb1(SV, "SVDJPDTC","BDDJ","DJSV","DJPDTC","","BDDJPDTC");
        AddCelltb1(SV, "SVDJWR","BDDJ","DJSV","DJWR","","BDDJWR");
        AddCelltb1(SV, "SVDJOTH","BDDJ","DJSV","DJOTH","","BDDJOTH");
        AddCelltb1(SV, "TotalDJSV","TotalDJBD","TotalDJSV","TotalDJBD","readonly");

        AddCelltb1(SV, "SVPGBGL","BDPG","PGSV","PGBGL","","BDPGBGL");
        AddCelltb1(SV, "SVPGBRC","BDPG","PGSV","PGBRC","","BDPGBRC");
        AddCelltb1(SV, "SVPGEAR","BDPG","PGSV","PGEAR","","BDPGEAR");
        AddCelltb1(SV, "SVPGLR","BDPG","PGSV","PGLR","","BDPGLR");
        AddCelltb1(SV, "SVPGBUCK","BDPG","PGSV","PGBUCK","","BDPGBUCK");
        AddCelltb1(SV, "SVPGMJ","BDPG","PGSV","PGMJ","","BDPGMJ");
        AddCelltb1(SV, "SVPGNECK","BDPG","PGSV","PGNECK","","BDPGNECK");
        AddCelltb1(SV, "SVPGBRO","BDPG","PGSV","PGBRO","","BDPGBRO");
        AddCelltb1(SV, "SVPGPDT","BDPG","PGSV","PGPDT","","BDPGPDT");
        AddCelltb1(SV, "SVPGPDTC","BDPG","PGSV","PGPDTC","","BDPGPDTC");
        AddCelltb1(SV, "SVPGWR","BDPG","PGSV","PGWR","","BDPGWR");
        AddCelltb1(SV, "SVPGOTH","BDPG","PGSV","PGOTH","","BDPGOTH");
        AddCelltb1(SV, "TotalPGSV","TotalPGBD","TotalPGSV","TotalPGBD","readonly");

        AddCelltb1(SV, "SVLDOTH","BDLD","LDSV","LDOTH","","BDLDOTH");
        AddCelltb1(SV, "TotalLDSV","TotalLDBD","TotalLDSV","TotalLDBD","readonly");
        AddCelltb1(SV, "totalALLSV", "ALLSV", "TotalALLSV", "TotalALLBD", "readonly");
        
        tb1.Rows.Add(SV);  
        //END SIMPANAN

        //START GWP (GF)
        TableRow GF = new TableRow();
        GF.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellGF = new TableCell();
        cellGF.Text = "Hadiah / GWP";                
        cellGF.CssClass = "freeze-column";        
        cellGF.Style.Add("text-align","center");
        GF.Cells.Add(cellGF);
                
        AddCelltb1(GF, "GFDJBGL","BDDJ","DJGF","DJBGL","","BDDJBGL");
        AddCelltb1(GF, "GFDJBRC","BDDJ","DJGF","DJBRC","","BDDJBRC");
        AddCelltb1(GF, "GFDJEAR","BDDJ","DJGF","DJEAR","","BDDJEAR");
        AddCelltb1(GF, "GFDJLR","BDDJ","DJGF","DJLR","","BDDJLR");
        AddCelltb1(GF, "GFDJBUCK","BDDJ","DJGF","DJBUCK","","BDDJBUCK");
        AddCelltb1(GF, "GFDJMJ","BDDJ","DJGF","DJMJ","","BDDJMJ");
        AddCelltb1(GF, "GFDJNECK","BDDJ","DJGF","DJNECK","","BDDJNECK");
        AddCelltb1(GF, "GFDJBRO","BDDJ","DJGF","DJBRO","","BDDJBRO");
        AddCelltb1(GF, "GFDJPDT","BDDJ","DJGF","DJPDT","","BDDJPDT");
        AddCelltb1(GF, "GFDJPDTC","BDDJ","DJGF","DJPDTC","","BDDJPDTC");
        AddCelltb1(GF, "GFDJWR","BDDJ","DJGF","DJWR","","BDDJWR");
        AddCelltb1(GF, "GFDJOTH","BDDJ","DJGF","DJOTH","","BDDJOTH");
        AddCelltb1(GF, "TotalDJGF","TotalDJBD","TotalDJGF","TotalDJBD","readonly");

        AddCelltb1(GF, "GFPGBGL","BDPG","PGGF","PGBGL","","BDPGBGL");
        AddCelltb1(GF, "GFPGBRC","BDPG","PGGF","PGBRC","","BDPGBRC");
        AddCelltb1(GF, "GFPGEAR","BDPG","PGGF","PGEAR","","BDPGEAR");
        AddCelltb1(GF, "GFPGLR","BDPG","PGGF","PGLR","","BDPGLR");
        AddCelltb1(GF, "GFPGBUCK","BDPG","PGGF","PGBUCK","","BDPGBUCK");
        AddCelltb1(GF, "GFPGMJ","BDPG","PGGF","PGMJ","","BDPGMJ");
        AddCelltb1(GF, "GFPGNECK","BDPG","PGGF","PGNECK","","BDPGNECK");
        AddCelltb1(GF, "GFPGBRO","BDPG","PGGF","PGBRO","","BDPGBRO");
        AddCelltb1(GF, "GFPGPDT","BDPG","PGGF","PGPDT","","BDPGPDT");
        AddCelltb1(GF, "GFPGPDTC","BDPG","PGGF","PGPDTC","","BDPGPDTC");
        AddCelltb1(GF, "GFPGWR","BDPG","PGGF","PGWR","","BDPGWR");
        AddCelltb1(GF, "GFPGOTH","BDPG","PGGF","PGOTH","","BDPGOTH");
        AddCelltb1(GF, "TotalPGGF","TotalPGBD","TotalPGGF","TotalPGBD","readonly");

        AddCelltb1(GF, "GFLDOTH","BDLD","LDGF","LDOTH","","BDLDOTH");
        AddCelltb1(GF, "TotalLDGF","TotalLDBD","TotalLDGF","TotalLDBD","readonly");
        AddCelltb1(GF, "totalALLGF","ALLGF","TotalALLGF","TotalALLBD","readonly");
    
        tb1.Rows.Add(GF);        
        //END GWP

        //START DIPINJAMKAN(LN)
        TableRow LN = new TableRow();
        LN.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellLN = new TableCell();
        cellLN.Text = "DIPINJAMKAN";        
        cellLN.CssClass = "freeze-column";        
        cellLN.Style.Add("text-align","center");
        LN.Cells.Add(cellLN);
                
        AddCelltb1(LN, "LNDJBGL","BDDJ","DJLN","DJBGL","","BDDJBGL");
        AddCelltb1(LN, "LNDJBRC","BDDJ","DJLN","DJBRC","","BDDJBRC");
        AddCelltb1(LN, "LNDJEAR","BDDJ","DJLN","DJEAR","","BDDJEAR");
        AddCelltb1(LN, "LNDJLR","BDDJ","DJLN","DJLR","","BDDJLR");
        AddCelltb1(LN, "LNDJBUCK","BDDJ","DJLN","DJBUCK","","BDDJBUCK");
        AddCelltb1(LN, "LNDJMJ","BDDJ","DJLN","DJMJ","","BDDJMJ");
        AddCelltb1(LN, "LNDJNECK","BDDJ","DJLN","DJNECK","","BDDJNECK");
        AddCelltb1(LN, "LNDJBRO","BDDJ","DJLN","DJBRO","","BDDJBRO");
        AddCelltb1(LN, "LNDJPDT","BDDJ","DJLN","DJPDT","","BDDJPDT");
        AddCelltb1(LN, "LNDJPDTC","BDDJ","DJLN","DJPDTC","","BDDJPDTC");
        AddCelltb1(LN, "LNDJWR","BDDJ","DJLN","DJWR","","BDDJWR");
        AddCelltb1(LN, "LNDJOTH","BDDJ","DJLN","DJOTH","","BDDJOTH");
        AddCelltb1(LN, "TotalDJLN","TotalDJBD","TotalDJLN","TotalDJBD","readonly");

        AddCelltb1(LN, "LNPGBGL","BDPG","PGLN","PGBGL","","BDPGBGL");
        AddCelltb1(LN, "LNPGBRC","BDPG","PGLN","PGBRC","","BDPGBRC");
        AddCelltb1(LN, "LNPGEAR","BDPG","PGLN","PGEAR","","BDPGEAR");
        AddCelltb1(LN, "LNPGLR","BDPG","PGLN","PGLR","","BDPGLR");
        AddCelltb1(LN, "LNPGBUCK","BDPG","PGLN","PGBUCK","","BDPGBUCK");
        AddCelltb1(LN, "LNPGMJ","BDPG","PGLN","PGMJ","","BDPGMJ");
        AddCelltb1(LN, "LNPGNECK","BDPG","PGLN","PGNECK","","BDPGNECK");
        AddCelltb1(LN, "LNPGBRO","BDPG","PGLN","PGBRO","","BDPGBRO");
        AddCelltb1(LN, "LNPGPDT","BDPG","PGLN","PGPDT","","BDPGPDT");
        AddCelltb1(LN, "LNPGPDTC","BDPG","PGLN","PGPDTC","","BDPGPDTC");
        AddCelltb1(LN, "LNPGWR","BDPG","PGLN","PGWR","","BDPGWR");
        AddCelltb1(LN, "LNPGOTH","BDPG","PGLN","PGOTH","","BDPGOTH");
        AddCelltb1(LN, "TotalPGLN","TotalPGBD","TotalPGLN","TotalPGBD","readonly");

        AddCelltb1(LN, "LNLDOTH","BDLD","LDLN","LDOTH","","BDLDOTH");
        AddCelltb1(LN, "TotalLDLN","TotalLDBD","TotalLDLN","TotalLDBD","readonly");
        AddCelltb1(LN, "totalALLLN","ALLLN","TotalALLLN","TotalALLBD","readonly");
    
        tb1.Rows.Add(LN);        
        //END DIPINJAMKAN

        //START BRG BARU
        TableRow NW = new TableRow();
        NW.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellNW = new TableCell();
        cellNW.Text = "BRG BARU, AKAN DIPROSES QC";      
        cellNW.CssClass = "freeze-column";        
        cellNW.Style.Add("text-align","center");
        NW.Cells.Add(cellNW);
                
        AddCelltb1(NW, "NWDJBGL","BDDJ","DJNW","DJBGL","","BDDJBGL");
        AddCelltb1(NW, "NWDJBRC","BDDJ","DJNW","DJBRC","","BDDJBRC");
        AddCelltb1(NW, "NWDJEAR","BDDJ","DJNW","DJEAR","","BDDJEAR");
        AddCelltb1(NW, "NWDJLR","BDDJ","DJNW","DJLR","","BDDJLR");
        AddCelltb1(NW, "NWDJBUCK","BDDJ","DJNW","DJBUCK","","BDDJBUCK");
        AddCelltb1(NW, "NWDJMJ","BDDJ","DJNW","DJMJ","","BDDJMJ");
        AddCelltb1(NW, "NWDJNECK","BDDJ","DJNW","DJNECK","","BDDJNECK");
        AddCelltb1(NW, "NWDJBRO","BDDJ","DJNW","DJBRO","","BDDJBRO");
        AddCelltb1(NW, "NWDJPDT","BDDJ","DJNW","DJPDT","","BDDJPDT");
        AddCelltb1(NW, "NWDJPDTC","BDDJ","DJNW","DJPDTC","","BDDJPDTC");
        AddCelltb1(NW, "NWDJWR","BDDJ","DJNW","DJWR","","BDDJWR");
        AddCelltb1(NW, "NWDJOTH","BDDJ","DJNW","DJOTH","","BDDJOTH");
        AddCelltb1(NW, "TotalDJNW","TotalDJBD","TotalDJNW","TotalDJBD","readonly");

        AddCelltb1(NW, "NWPGBGL","BDPG","PGNW","PGBGL","","BDPGBGL");
        AddCelltb1(NW, "NWPGBRC","BDPG","PGNW","PGBRC","","BDPGBRC");
        AddCelltb1(NW, "NWPGEAR","BDPG","PGNW","PGEAR","","BDPGEAR");
        AddCelltb1(NW, "NWPGLR","BDPG","PGNW","PGLR","","BDPGLR");
        AddCelltb1(NW, "NWPGBUCK","BDPG","PGNW","PGBUCK","","BDPGBUCK");
        AddCelltb1(NW, "NWPGMJ","BDPG","PGNW","PGMJ","","BDPGPMJ");
        AddCelltb1(NW, "NWPGNECK","BDPG","PGNW","PGNECK","","BDPGNECK");
        AddCelltb1(NW, "NWPGBRO","BDPG","PGNW","PGBRO","","BDPGBRO");
        AddCelltb1(NW, "NWPGPDT","BDPG","PGNW","PGPDT","","BDPGPDT");
        AddCelltb1(NW, "NWPGPDTC","BDPG","PGNW","PGPDTC","","BDPGPDTC");
        AddCelltb1(NW, "NWPGWR","BDPG","PGNW","PGWR","","BDPGWR");
        AddCelltb1(NW, "NWPGOTH","BDPG","PGNW","PGOTH","","BDPGOTH");
        AddCelltb1(NW, "TotalPGNW","TotalPGBD","TotalPGNW","TotalPGBD","readonly");

        AddCelltb1(NW, "NWLDOTH","BDLD","LDNW","LDOTH","","BDLDOTH");
        AddCelltb1(NW, "TotalLDNW","TotalLDBD","TotalLDNW","TotalLDBD","readonly");
        AddCelltb1(NW, "totalALLNW","ALLNW","TotalALLNW","TotalALLBD","readonly");
    
        tb1.Rows.Add(NW);        
        //END BRG BARU

        //START RETUR
        TableRow RT = new TableRow();
        RT.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellRT = new TableCell();
        cellRT.Text = "RETUR";        
        cellRT.CssClass = "freeze-column";        
        cellRT.Style.Add("text-align","center");
        RT.Cells.Add(cellRT);
                
        AddCelltb1(RT, "RTDJBGL","BDDJ","DJRT","DJBGL","","BDDJBGL");
        AddCelltb1(RT, "RTDJBRC","BDDJ","DJRT","DJBRC","","BDDJBRC");
        AddCelltb1(RT, "RTDJEAR","BDDJ","DJRT","DJEAR","","BDDJEAR");
        AddCelltb1(RT, "RTDJLR","BDDJ","DJRT","DJLR","","BDDJLR");
        AddCelltb1(RT, "RTDJBUCK","BDDJ","DJRT","DJBUCK","","BDDJBUCK");
        AddCelltb1(RT, "RTDJMJ","BDDJ","DJRT","DJMJ","","BDDJMJ");
        AddCelltb1(RT, "RTDJNECK","BDDJ","DJRT","DJNECK","","BDDJNECK");
        AddCelltb1(RT, "RTDJBRO","BDDJ","DJRT","DJBRO","","BDDJBRO");
        AddCelltb1(RT, "RTDJPDT","BDDJ","DJRT","DJPDT","","BDDJPDT");
        AddCelltb1(RT, "RTDJPDTC","BDDJ","DJRT","DJPDTC","","BDDJPDTC");
        AddCelltb1(RT, "RTDJWR","BDDJ","DJRT","DJWR","","BDDJWR");
        AddCelltb1(RT, "RTDJOTH","BDDJ","DJRT","DJOTH","","BDDJOTH");
        AddCelltb1(RT, "TotalDJRT","TotalDJBD","TotalDJRT","TotalDJBD","readonly");

        AddCelltb1(RT, "RTPGBGL","BDPG","PGRT","PGBGL","","BDPGBGL");
        AddCelltb1(RT, "RTPGBRC","BDPG","PGRT","PGBRC","","BDPGBRC");
        AddCelltb1(RT, "RTPGEAR","BDPG","PGRT","PGEAR","","BDPGEAR");
        AddCelltb1(RT, "RTPGLR","BDPG","PGRT","PGLR","","BDPGLR");
        AddCelltb1(RT, "RTPGBUCK","BDPG","PGRT","PGBUCK","","BDPGBUCK");
        AddCelltb1(RT, "RTPGMJ","BDPG","PGRT","PGMJ","","BDPGPMJ");
        AddCelltb1(RT, "RTPGNECK","BDPG","PGRT","PGNECK","","BDPGNECK");
        AddCelltb1(RT, "RTPGBRO","BDPG","PGRT","PGBRO","","BDPGBRO");
        AddCelltb1(RT, "RTPGPDT","BDPG","PGRT","PGPDT","","BDPGPDT");
        AddCelltb1(RT, "RTPGPDTC","BDPG","PGRT","PGPDTC","","BDPGPDTC");
        AddCelltb1(RT, "RTPGWR","BDPG","PGRT","PGWR","","BDPGWR");
        AddCelltb1(RT, "RTPGOTH","BDPG","PGRT","PGOTH","","BDPGOTH");
        AddCelltb1(RT, "TotalPGRT","TotalPGBD","TotalPGRT","TotalPGBD","readonly");

        AddCelltb1(RT, "RTLDOTH","BDLD","LDRT","LDOTH","","BDLDOTH");
        AddCelltb1(RT, "TotalLDRT","TotalLDBD","TotalLDRT","TotalLDBD","readonly");
        AddCelltb1(RT, "totalALLRT","ALLRT","TotalALLRT","TotalALLBD","readonly");
    
        tb1.Rows.Add(RT);        
        //END RETUR

        //START BUYBACK/RESELL
        TableRow RS = new TableRow();
        RS.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellRS = new TableCell();
        cellRS.Text = "BUYBACK / RESELL CMK";         
        cellRS.CssClass = "freeze-column";        
        cellRS.Style.Add("text-align","center");
        RS.Cells.Add(cellRS);
                
        AddCelltb1(RS, "RSDJBGL","BDDJ","DJRS","DJBGL","","BDDJBGL");
        AddCelltb1(RS, "RSDJBRC","BDDJ","DJRS","DJBRC","","BDDJBRC");
        AddCelltb1(RS, "RSDJEAR","BDDJ","DJRS","DJEAR","","BDDJEAR");
        AddCelltb1(RS, "RSDJLR","BDDJ","DJRS","DJLR","","BDDJLR");
        AddCelltb1(RS, "RSDJBUCK","BDDJ","DJRS","DJBUCK","","BDDJBUCK");
        AddCelltb1(RS, "RSDJMJ","BDDJ","DJRS","DJMJ","","BDDJMJ");
        AddCelltb1(RS, "RSDJNECK","BDDJ","DJRS","DJNECK","","BDDJNECK");
        AddCelltb1(RS, "RSDJBRO","BDDJ","DJRS","DJBRO","","BDDJBRO");
        AddCelltb1(RS, "RSDJPDT","BDDJ","DJRS","DJPDT","","BDDJPDT");
        AddCelltb1(RS, "RSDJPDTC","BDDJ","DJRS","DJPDTC","","BDDJPDTC");
        AddCelltb1(RS, "RSDJWR","BDDJ","DJRS","DJWR","","BDDJWR");
        AddCelltb1(RS, "RSDJOTH","BDDJ","DJRS","DJOTH","","BDDJOTH");
        AddCelltb1(RS, "TotalDJRS","TotalDJBD","TotalDJRS","TotalDJBD","readonly");

        AddCelltb1(RS, "RSPGBGL","BDPG","PGRS","PGBGL","","BDPGBGL");
        AddCelltb1(RS, "RSPGBRC","BDPG","PGRS","PGBRC","","BDPGBRC");
        AddCelltb1(RS, "RSPGEAR","BDPG","PGRS","PGEAR","","BDPGEAR");
        AddCelltb1(RS, "RSPGLR","BDPG","PGRS","PGLR","","BDPGLR");
        AddCelltb1(RS, "RSPGBUCK","BDPG","PGRS","PGBUCK","","BDPGBUCK");
        AddCelltb1(RS, "RSPGMJ","BDPG","PGRS","PGMJ","","BDPGPMJ");
        AddCelltb1(RS, "RSPGNECK","BDPG","PGRS","PGNECK","","BDPGNECK");
        AddCelltb1(RS, "RSPGBRO","BDPG","PGRS","PGBRO","","BDPGBRO");
        AddCelltb1(RS, "RSPGPDT","BDPG","PGRS","PGPDT","","BDPGPDT");
        AddCelltb1(RS, "RSPGPDTC","BDPG","PGRS","PGPDTC","","BDPGPDTC");
        AddCelltb1(RS, "RSPGWR","BDPG","PGRS","PGWR","","BDPGWR");
        AddCelltb1(RS, "RSPGOTH","BDPG","PGRS","PGOTH","","BDPGOTH");
        AddCelltb1(RS, "TotalPGRS","TotalPGBD","TotalPGRS","TotalPGBD","readonly");

        AddCelltb1(RS, "RSLDOTH","BDLD","LDRS","LDOTH","","BDLDOTH");
        AddCelltb1(RS, "TotalLDRS","TotalLDBD","TotalLDRS","TotalLDBD","readonly");
        AddCelltb1(RS, "totalALLRS","ALLRS","TotalALLRS","TotalALLBD","readonly");
    
        tb1.Rows.Add(RS);        
        //END BUYBACK / RESELL

        //START REPAIR
        TableRow RP = new TableRow();
        RP.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellRP = new TableCell();
        cellRP.Text = "BARANG STOCK YG DI-REPAIR";           
        cellRP.CssClass = "freeze-column";        
        cellRP.Style.Add("text-align","center");
        RP.Cells.Add(cellRP);
                
        AddCelltb1(RP, "RPDJBGL","BDDJ","DJRP","DJBGL","","BDDJBGL");
        AddCelltb1(RP, "RPDJBRC","BDDJ","DJRP","DJBRC","","BDDJBRC");
        AddCelltb1(RP, "RPDJEAR","BDDJ","DJRP","DJEAR","","BDDJEAR");
        AddCelltb1(RP, "RPDJLR","BDDJ","DJRP","DJLR","","BDDJLR");
        AddCelltb1(RP, "RPDJBUCK","BDDJ","DJRP","DJBUCK","","BDDJBUCK");
        AddCelltb1(RP, "RPDJMJ","BDDJ","DJRP","DJMJ","","BDDJMJ");
        AddCelltb1(RP, "RPDJNECK","BDDJ","DJRP","DJNECK","","BDDJNECK");
        AddCelltb1(RP, "RPDJBRO","BDDJ","DJRP","DJBRO","","BDDJBRO");
        AddCelltb1(RP, "RPDJPDT","BDDJ","DJRP","DJPDT","","BDDJPDT");
        AddCelltb1(RP, "RPDJPDTC","BDDJ","DJRP","DJPDTC","","BDDJPDTC");
        AddCelltb1(RP, "RPDJWR","BDDJ","DJRP","DJWR","","BDDJWR");
        AddCelltb1(RP, "RPDJOTH","BDDJ","DJRP","DJOTH","","BDDJOTH");
        AddCelltb1(RP, "TotalDJRP","TotalDJBD","TotalDJRP","TotalDJBD","readonly");
    
        AddCelltb1(RP, "RPPGBGL","BDPG","PGRP","PGBGL","","BDPGBGL");
        AddCelltb1(RP, "RPPGBRC","BDPG","PGRP","PGBRC","","BDPGBRC");
        AddCelltb1(RP, "RPPGEAR","BDPG","PGRP","PGEAR","","BDPGEAR");
        AddCelltb1(RP, "RPPGLR","BDPG","PGRP","PGLR","","BDPGLR");
        AddCelltb1(RP, "RPPGBUCK","BDPG","PGRP","PGBUCK","","BDPGBUCK");
        AddCelltb1(RP, "RPPGMJ","BDPG","PGRP","PGMJ","","BDPGPMJ");
        AddCelltb1(RP, "RPPGNECK","BDPG","PGRP","PGNECK","","BDPGNECK");
        AddCelltb1(RP, "RPPGBRO","BDPG","PGRP","PGBRO","","BDPGBRO");
        AddCelltb1(RP, "RPPGPDT","BDPG","PGRP","PGPDT","","BDPGPDT");
        AddCelltb1(RP, "RPPGPDTC","BDPG","PGRP","PGPDTC","","BDPGPDTC");
        AddCelltb1(RP, "RPPGWR","BDPG","PGRP","PGWR","","BDPGWR");
        AddCelltb1(RP, "RPPGOTH","BDPG","PGRP","PGOTH","","BDPGOTH");
        AddCelltb1(RP, "TotalPGRP","TotalPGBD","TotalPGRP","TotalPGBD","readonly");

        AddCelltb1(RP, "RPLDOTH","BDLD","LDRP","LDOTH","","BDLDOTH");
        AddCelltb1(RP, "TotalLDRP","TotalLDBD","TotalLDRP","TotalLDBD","readonly");
        AddCelltb1(RP, "totalALLRP","ALLRP","TotalALLRP","TotalALLBD","readonly");
    
        tb1.Rows.Add(RP);        
        //END REPAIR

        //START OTW
        TableRow OT = new TableRow();
        OT.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter
        TableCell cellOT = new TableCell();
        cellOT.Text = "OTW";        
        cellOT.CssClass = "freeze-column";        
        cellOT.Style.Add("text-align","center");
        OT.Cells.Add(cellOT);        
        DataTable dtOTW = new DataTable();
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("sp_getBarangOtwOpname", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = locationId});
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtOTW);        
                }
            }
        }
        Dictionary<string, int> dictOTW = new Dictionary<string, int>();

        foreach (DataRow dr in dtOTW.Rows)
        {
            string tipe = dr["TipeProduct"].ToString();   // DJ / PG / LD
            string item = dr["ProductItem"].ToString();   // BGL / BRC / EAR dll
            int qty = Convert.ToInt32(dr["Qty"]);

            string key = tipe + item;   // contoh: DJBGL

            if (!dictOTW.ContainsKey(key))
                dictOTW.Add(key, qty);
        }
                
        AddCelltb1(OT, "OTDJBGL","BDDJ","DJOT","DJBGL", "","BDDJBGL", dictOTW);
        AddCelltb1(OT, "OTDJBRC","BDDJ","DJOT","DJBRC", "","BDDJBRC", dictOTW);
        AddCelltb1(OT, "OTDJEAR","BDDJ","DJOT","DJEAR", "","BDDJEAR", dictOTW);
        AddCelltb1(OT, "OTDJLR","BDDJ","DJOT","DJLR", "","BDDJLR", dictOTW);
        AddCelltb1(OT, "OTDJBUCK","BDDJ","DJOT","DJBUCK", "","BDDJBUCK", dictOTW);
        AddCelltb1(OT, "OTDJMJ","BDDJ","DJOT","DJMJ", "","BDDJMJ", dictOTW);
        AddCelltb1(OT, "OTDJNECK","BDDJ","DJOT","DJNECK", "","BDDJNECK", dictOTW);
        AddCelltb1(OT, "OTDJBRO","BDDJ","DJOT","DJBRO", "","BDDJBRO", dictOTW);
        AddCelltb1(OT, "OTDJPDT","BDDJ","DJOT","DJPDT", "","BDDJPDT", dictOTW);
        AddCelltb1(OT, "OTDJPDTC","BDDJ","DJOT","DJPDTC", "","BDDJPDTC", dictOTW);
        AddCelltb1(OT, "OTDJWR","BDDJ","DJOT","DJWR", "","BDDJWR", dictOTW);
        AddCelltb1(OT, "OTDJOTH","BDDJ","DJOT","DJOTH", "","BDDJOTH", dictOTW);
        AddCelltb1(OT, "TotalDJOT","TotalDJBD","TotalDJOT","TotalDJBD","readonly","",dictOTW);

        AddCelltb1(OT, "OTPGBGL","BOTG","PGOT","PGBGL", "","BDPGBGL",dictOTW);
        AddCelltb1(OT, "OTPGBRC","BOTG","PGOT","PGBRC", "","BDPGBRC",dictOTW);
        AddCelltb1(OT, "OTPGEAR","BOTG","PGOT","PGEAR", "","BDPGEAR",dictOTW);
        AddCelltb1(OT, "OTPGLR","BOTG","PGOT","PGLR", "","BDPGLR",dictOTW);
        AddCelltb1(OT, "OTPGBUCK","BOTG","PGOT","PGBUCK", "","BDPGBUCK",dictOTW);
        AddCelltb1(OT, "OTPGMJ","BOTG","PGOT","PGMJ", "","BDPGMJ",dictOTW);
        AddCelltb1(OT, "OTPGNECK","BOTG","PGOT","PGNECK", "","BDPGNECK",dictOTW);
        AddCelltb1(OT, "OTPGBRO","BOTG","PGOT","PGBRO", "","BDPGBRO",dictOTW);
        AddCelltb1(OT, "OTPGPDT","BOTG","PGOT","PGPDT", "","BDPGPDT",dictOTW);
        AddCelltb1(OT, "OTPGPDTC","BOTG","PGOT","PGPDTC", "","BDPGPDTC",dictOTW);
        AddCelltb1(OT, "OTPGWR","BOTG","PGOT","PGWR", "","BDPGWR",dictOTW);
        AddCelltb1(OT, "OTPGOTH","BOTG","PGOT","PGOTH", "","BDPGOTH",dictOTW);
        AddCelltb1(OT, "TotalPGOT","TotalPGBD","TotalPGOT","TotalPGBD","readonly","",dictOTW);

        AddCelltb1(OT, "OTLDOTH","BDLD","LDOT","LDOTH", "","BDLDOTH",dictOTW);
        AddCelltb1(OT, "TotalLDOT","TotalLDBD","TotalLDOT","TotalLDBD","readonly","",dictOTW);
        AddCelltb1(OT, "totalALLOT","ALLOT","TotalALLOT","TotalALLBD","readonly","",dictOTW);
    
        tb1.Rows.Add(OT);     
        //END OTW   


        // Footer Total System

        TableRow BD = new TableRow();
        BD.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter             
        BD.CssClass = "freeze-footer";   
        TableCell cellBD = new TableCell();
        cellBD.Text = "Total Barang Dalam";        
        cellBD.CssClass = "freeze-column text-center";            
        BD.Cells.Add(cellBD);
        //dj        
            AddCelltb1(BD, "TotalDJBGL","totalBDDJBGL", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJBRC","totalBDDJBRC", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJEAR","totalBDDJEAR", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJLR","totalBDDJLR", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJBUCK","totalBDDJBUCK", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJMJ","totalBDDJMJ", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJNECK","totalBDDJNECK", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJBRO","totalBDDJBRO", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJPDT","totalBDDJPDT", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJPDTC","totalBDDJPDTC", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJWR","totalBDDJWR", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJOTH","totalBDDJOTH", "", "", "readonly","",dictOTW); 
            AddCelltb1(BD,"totalBDDJ","totalBDDJALL", "", "", "readonly","",dictOTW);
                                                         
        
            AddCelltb1(BD, "TotalPGBGL","totalBDPGBGL", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGBRC","totalBDPGBRC", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGEAR","totalBDPGEAR", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGLR","totalBDPGLR", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGBUCK","totalBDPGBUCK", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGMJ","totalBDPGMJ", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGNECK","totalBDPGNECK", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGBRO","totalBDPGBRO", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGPDT","totalBDPGPDT", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGPDTC","totalBDPGPDTC", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGWR","totalBDPGWR", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGOTH","totalBDPGOTH", "", "", "readonly","",dictOTW);       
            AddCelltb1(BD,"totalBDPG","totalBDPGALL", "", "", "readonly","",dictOTW);
        
            AddCelltb1(BD, "TotalLDOTH","totalBDLDOTH", "", "", "readonly","",dictOTW);     
            AddCelltb1(BD,"totalBDLD","totalBDLDALL", "", "", "readonly","",dictOTW); 

            AddCelltb1(BD,"totalBDALL","totalBDALL", "", "", "readonly","",dictOTW);               
        
        tb1.Rows.Add(BD);   
    }
    
    private void generateTabelBarangDalam1(int locationId){        
        //START DP
        DataTable dtBarangDalem = new DataTable();
        DataTable dtOTW = new DataTable();
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            connection.Open();
            string queryGetMaster = @"
                                    select IIF(B.Kode = 'BS', 'BD',b.Kode) as KodeHead,a.*  from MasterTableOpname_Detail A
                                    JOIN MasterTableOpname b on b.ID=a.IDMasterTableOpname where B.ID=2";
            using (SqlCommand cmd = new SqlCommand(queryGetMaster, connection))
            {
                cmd.CommandType = CommandType.Text;                
                using (SqlDataAdapter da1 = new SqlDataAdapter(cmd))
                {
                    da1.Fill(dtBarangDalem);        
                }
            }

            using (SqlCommand cmd = new SqlCommand("sp_getBarangOtwOpname", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = locationId});
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtOTW);        
                }
            }

            Dictionary<string, int> dictOTW = new Dictionary<string, int>();

            foreach (DataRow dr in dtOTW.Rows)
            {
                string tipe = dr["TipeProduct"].ToString();   // DJ / PG / LD
                string item = dr["ProductItem"].ToString();   // BGL / BRC / EAR dll
                int qty = Convert.ToInt32(dr["Qty"]);

                string key = tipe + item;   // contoh: DJBGL

                if (!dictOTW.ContainsKey(key))
                    dictOTW.Add(key, qty);
            }

            foreach (DataRow row in dtBarangDalem.Rows)
            {
                TableRow DP = new TableRow();
                DP.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter        
                TableCell cellDP = new TableCell();
                cellDP.Text = row["Nama"].ToString();        
                cellDP.CssClass = "freeze-column ";        
                cellDP.Style.Add("text-align","center");
                DP.Cells.Add(cellDP);
                //AddCelltb1(DP, "DATAELEMENT","DATATIPE","DATAROW","DATACOLUMN");

                AddCelltb1(DP, row["Kode"]+"DJBGL",row["KodeHead"]+"DJ","DJ"+row["Kode"],"DJBGL","",row["KodeHead"]+"DJBGL",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"DJBRC",row["KodeHead"]+"DJ","DJ"+row["Kode"],"DJBRC","",row["KodeHead"]+"DJBRC",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"DJEAR",row["KodeHead"]+"DJ","DJ"+row["Kode"],"DJEAR","",row["KodeHead"]+"DJEAR",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"DJLR",row["KodeHead"]+"DJ","DJ"+row["Kode"],"DJLR","",row["KodeHead"]+"DJLR",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"DJBUCK",row["KodeHead"]+"DJ","DJ"+row["Kode"],"DJBUCK","",row["KodeHead"]+"DJBUCK",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"DJMJ",row["KodeHead"]+"DJ","DJ"+row["Kode"],"DJMJ","",row["KodeHead"]+"DJMJ",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"DJNECK",row["KodeHead"]+"DJ","DJ"+row["Kode"],"DJNECK","",row["KodeHead"]+"DJNECK",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"DJBRO",row["KodeHead"]+"DJ","DJ"+row["Kode"],"DJBRO","",row["KodeHead"]+"DJBRO",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"DJPDT",row["KodeHead"]+"DJ","DJ"+row["Kode"],"DJPDT","",row["KodeHead"]+"DJPDT",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"DJPDTC",row["KodeHead"]+"DJ","DJ"+row["Kode"],"DJPDTC","",row["KodeHead"]+"DJPDTC",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"DJWR",row["KodeHead"]+"DJ","DJ"+row["Kode"],"DJWR","",row["KodeHead"]+"DJWR",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"DJOTH",row["KodeHead"]+"DJ","DJ"+row["Kode"],"DJOTH","",row["KodeHead"]+"DJOTH",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, "TotalDJ"+row["Kode"],"TotalDJ"+row["KodeHead"],"TotalDJ"+row["Kode"],"TotalDJ"+row["KodeHead"],"readonly","",row["Kode"].ToString() =="OT"?dictOTW:null);

                AddCelltb1(DP, row["Kode"]+"PGBGL",row["KodeHead"]+"PG","PG"+row["Kode"],"PGBGL","",row["KodeHead"]+"PGBGL",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"PGBRC",row["KodeHead"]+"PG","PG"+row["Kode"],"PGBRC","",row["KodeHead"]+"PGBRC",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"PGEAR",row["KodeHead"]+"PG","PG"+row["Kode"],"PGEAR","",row["KodeHead"]+"PGEAR",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"PGLR",row["KodeHead"]+"PG","PG"+row["Kode"],"PGLR","",row["KodeHead"]+"PGLR",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"PGBUCK",row["KodeHead"]+"PG","PG"+row["Kode"],"PGBUCK","",row["KodeHead"]+"PGBUCK",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"PGMJ",row["KodeHead"]+"PG","PG"+row["Kode"],"PGMJ","",row["KodeHead"]+"PGMJ",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"PGNECK",row["KodeHead"]+"PG","PG"+row["Kode"],"PGNECK","",row["KodeHead"]+"PGNECK",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"PGBRO",row["KodeHead"]+"PG","PG"+row["Kode"],"PGBRO","",row["KodeHead"]+"PGBRO",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"PGPDT",row["KodeHead"]+"PG","PG"+row["Kode"],"PGPDT","",row["KodeHead"]+"PGPDT",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"PGPDTC",row["KodeHead"]+"PG","PG"+row["Kode"],"PGPDTC","",row["KodeHead"]+"PGPDTC",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"PGWR",row["KodeHead"]+"PG","PG"+row["Kode"],"PGWR","",row["KodeHead"]+"PGWR",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, row["Kode"]+"PGOTH",row["KodeHead"]+"PG","PG"+row["Kode"],"PGOTH","",row["KodeHead"]+"PGOTH",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, "TotalPG"+row["Kode"],"TotalPG"+row["KodeHead"],"TotalPG"+row["Kode"],"TotalPG"+row["KodeHead"],"readonly","",row["Kode"].ToString() =="OT"?dictOTW:null);

                AddCelltb1(DP, row["Kode"]+"LDOTH",row["KodeHead"]+"LD","LD"+row["Kode"],"LDOTH","",row["KodeHead"]+"PLDOTH",row["Kode"].ToString()=="OT"?dictOTW:null);
                AddCelltb1(DP, "TotalLD"+row["Kode"],"TotalLD"+row["KodeHead"],"TotalLD"+row["Kode"],"TotalLD"+row["KodeHead"],"readonly","",row["Kode"].ToString() =="OT"?dictOTW:null);
                AddCelltb1(DP, "totalALL"+row["Kode"],"ALL"+row["Kode"],"TotalALL"+row["Kode"],"TotalALL"+row["KodeHead"],"readonly","",row["Kode"].ToString() =="OT"?dictOTW:null);

                tb1.Rows.Add(DP);
                
            }
        
        

   
            // Footer Total System

            TableRow BD = new TableRow();
            BD.TableSection = TableRowSection.TableFooter;  // Set the section to TableFooter             
            BD.CssClass = "freeze-footer";   
            TableCell cellBD = new TableCell();
            cellBD.Text = "Total Barang Dalam";        
            cellBD.CssClass = "freeze-column text-center";            
            BD.Cells.Add(cellBD);
            //dj        
            AddCelltb1(BD, "TotalDJBGL","totalBDDJBGL", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJBRC","totalBDDJBRC", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJEAR","totalBDDJEAR", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJLR","totalBDDJLR", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJBUCK","totalBDDJBUCK", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJMJ","totalBDDJMJ", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJNECK","totalBDDJNECK", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJBRO","totalBDDJBRO", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJPDT","totalBDDJPDT", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJPDTC","totalBDDJPDTC", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJWR","totalBDDJWR", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalDJOTH","totalBDDJOTH", "", "", "readonly","",dictOTW); 
            AddCelltb1(BD,"totalBDDJ","totalBDDJALL", "", "", "readonly","",dictOTW);
                                                         
        
            AddCelltb1(BD, "TotalPGBGL","totalBDPGBGL", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGBRC","totalBDPGBRC", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGEAR","totalBDPGEAR", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGLR","totalBDPGLR", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGBUCK","totalBDPGBUCK", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGMJ","totalBDPGMJ", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGNECK","totalBDPGNECK", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGBRO","totalBDPGBRO", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGPDT","totalBDPGPDT", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGPDTC","totalBDPGPDTC", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGWR","totalBDPGWR", "", "", "readonly","",dictOTW);
            AddCelltb1(BD, "TotalPGOTH","totalBDPGOTH", "", "", "readonly","",dictOTW);       
            AddCelltb1(BD,"totalBDPG","totalBDPGALL", "", "", "readonly","",dictOTW);
        
            AddCelltb1(BD, "TotalLDOTH","totalBDLDOTH", "", "", "readonly","",dictOTW);     
            AddCelltb1(BD,"totalBDLD","totalBDLDALL", "", "", "readonly","",dictOTW); 

            AddCelltb1(BD,"totalBDALL","totalBDALL", "", "", "readonly","",dictOTW);                       
            tb1.Rows.Add(BD);   
        }
    }
    
    protected void fillTable(object sender, EventArgs e) {}
    
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        Response.Redirect(href);
    }


    public class DataDisplay
    {
        public int ID { get; set; }
        public string NamaDisplay { get; set; }
    }
    public class ResultInsert
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }


    public class IInsertFormOpname
    {        
        public IIDataOpname IDataOpname { get; set; }
    }

    public class IIDataOpname
    {
        public int TipeLokasi { get; set; }
        public int IDLokasi { get; set; }
        public string KodeShift { get; set; }
        public int IDUser { get; set; }
        public List<object> BarangDisplay { get; set; }
        public List<object> BarangDalam { get; set; }
        public List<object> BarangTitipan { get; set; }
        public List<object> BarangLuarSistem { get; set; }

    }
}