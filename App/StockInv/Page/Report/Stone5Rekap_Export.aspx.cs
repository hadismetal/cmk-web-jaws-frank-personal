using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_Account_Stone : System.Web.UI.Page {
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
    string mode = "accounting"; // accounting, merchandise
    string action = "excel"; // display, excel

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Stone5Rekap_Export.xls");
			fillTable(sender, e);
		}
	}
    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //ORDER BY
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);
        string keyword = Request.QueryString["kw"].ToString();        
        string P1 = Request.QueryString["parcel01"] == "Code :" ? "0" : Request.QueryString["parcel01"];
        string P2 = Request.QueryString["parcel02"] == "Source :"? "0" : Request.QueryString["parcel02"];
        string P3 = Request.QueryString["parcel03"] == "Colour :" ? "0" : Request.QueryString["parcel03"];
        string P4 = Request.QueryString["parcel04"] == "Shape :"? "0" : Request.QueryString["parcel04"];
        string P5 = Request.QueryString["parcel05"] == "Overtone :" ? "0" : Request.QueryString["parcel05"];
        string P6 = Request.QueryString["parcel06"] == "Diameter :" ? "0" : Request.QueryString["parcel06"];
        string P7 = Request.QueryString["parcel07"] == "Surface :" ? "0" : Request.QueryString["parcel07"];
        string o = Request.QueryString["order"] + " " + Request.QueryString["orderby"];        
        string query = @"
                	SELECT Nama,KodeParcel,sum(TotalButir) TotalButir,sum(TotalButir) TotalCarat
					,Code, Source, Colour, Shape, Overtone, Diameter, Surface
					FROM (
						select c.ID,c.Nama,c.KodeParcel,a.TotalButir,a.TotalCarat
						, baa.Nama as Code, bab.Nama [Source], bac.Nama Colour,
						bad.Nama Shape, bae.Nama Overtone,baf.Nama Diameter, bag.Nama Surface
						from StockProductDJ_Stone5 a
						JOIN StockProductDJ b on b.id=a.IDForm
						JOIN Stone5 c on c.ID=a.IDStone
						JOIN StockActualDJ d on d.IDProduct=b.ID
						JOIN Parcel501 baa on baa.ID=c.Parcel01 /*parcel 01 & 02 */ JOIN Parcel502 bab on bab.ID=c.Parcel02
						JOIN Parcel503 bac on bac.ID=c.Parcel03 /*parcel 03 & 04 */ JOIN Parcel504 bad on bad.ID=c.Parcel04
						JOIN Parcel505 bae on bae.ID=c.Parcel05 /*parcel 05 & 06 */ JOIN Parcel506 baf on baf.ID=c.Parcel06
						JOIN Parcel507 bag on bag.ID=c.Parcel07  
						WHERE b.Draft=0  and b.StatusLebur = 0 and b.ReturnProduct = 0 and b.StatusPenjualan = 0 and d.IDLokasi <> -1                        
                        and (@Parcel01=0 or baa.ID=@Parcel01) and (@Parcel02=0 or bab.ID=@Parcel02)
                        and (@Parcel03=0 or bac.ID=@Parcel03) and (@Parcel04=0 or bad.ID=@Parcel04)
                        and (@Parcel05=0 or bae.ID=@Parcel05) and (@Parcel06=0 or baf.ID=@Parcel06)
                        and (@Parcel07=0 or baf.ID=@Parcel07)                         
                        and (@keyword = '' or c.Nama like '%'+ @keyword +'%' or c.KodeParcel like '%'+ @keyword +'%' or b.Nomor like '%'+ @keyword +'%')
					)AX
					GROUP BY Nama,KodeParcel ,Code, Source, Colour, Shape, Overtone, Diameter, Surface, ID
                    ORDER BY "+ o +@"                    
                ";        
        
    
            char[] charsToTrim = { '*', ' ', '\''};            
            decimal TotalPage = 0;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalHargaSatuan = 0m;
            decimal TotalHargaTotal = 0m;

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {            
            using (SqlCommand command = new SqlCommand(query, connection)){
                command.CommandTimeout = 10;
                command.Parameters.Add(new SqlParameter("@keyword", SqlDbType.VarChar) { Value = Request.QueryString["kw"].Trim(charsToTrim) });                
                command.Parameters.Add(new SqlParameter("@Parcel01", SqlDbType.VarChar) { Value = P1 }); 
                command.Parameters.Add(new SqlParameter("@Parcel02", SqlDbType.VarChar) { Value = P2 });
                command.Parameters.Add(new SqlParameter("@Parcel03", SqlDbType.VarChar) { Value = P3 }); 
                command.Parameters.Add(new SqlParameter("@Parcel04", SqlDbType.VarChar) { Value = P4 });
                command.Parameters.Add(new SqlParameter("@Parcel05", SqlDbType.VarChar) { Value = P5 }); 
                command.Parameters.Add(new SqlParameter("@Parcel06", SqlDbType.VarChar) { Value = P6 });
                command.Parameters.Add(new SqlParameter("@Parcel07", SqlDbType.VarChar) { Value = P7 });                
                
                connection.Open();
                SqlDataAdapter adapter1 = new SqlDataAdapter(command);
                DataSet ds1 = new DataSet();                                
                adapter1.Fill(ds1);                                    
                

                DataTable data = ds1.Tables[0];                

                if (data.Rows.Count == 0 ) 
                {                    
                    App.TableNoResult(tb);
                }

                int index = 1;
                foreach (DataRow r in data.Rows)
                {
                    if (!Response.IsClientConnected) break;
    
                    TableRow tr = new TableRow();
                    TableCell td;
    
                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);
    
                    td = new TableCell();
                    td.Text = r["Nama"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["Code"].ToString();
                    tr.Cells.Add(td);
    
                    td = new TableCell();
                    td.Text = r["Shape"].ToString();
                    tr.Cells.Add(td);
    
                    td = new TableCell();
                    td.Text = r["Overtone"].ToString();
                    tr.Cells.Add(td);
                
    
                    td = new TableCell();
                    td.Text = r["Diameter"].ToString();
                    tr.Cells.Add(td);
    
                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["TotalButir"]).ToString("N0");
                    tr.Cells.Add(td);
    
                    td = new TableCell();
                    td.Text = Convert.ToDecimal(r["TotalCarat"]).ToString("N3");
                    tr.Cells.Add(td);
    
                    tb.Rows.Add(tr);
    
                    TotalButir += Convert.ToDecimal(r["TotalButir"]);
                    TotalCarat += Convert.ToDecimal(r["TotalCarat"]);
                }
    
                TableRow tr2 = new TableRow();
                TableCell td2;
    
                td2 = new TableCell();
                td2.Text = "<b>Total</b>";
                td2.ColumnSpan = 6;
                tr2.Cells.Add(td2);
    
                td2 = new TableCell();
                td2.Text = TotalButir.ToString("N0");
                tr2.Cells.Add(td2);
    
                td2 = new TableCell();
                td2.Text = TotalCarat.ToString("N3");
                tr2.Cells.Add(td2);
    
                tb.Rows.Add(tr2);
            }
        }
    }  


    protected static void TableNoResult(Table tb)
    {
        tb.Width = new Unit("100%");

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td.Text = "No Data";
        //td.ColumnSpan = TableColspan(tb);
        //td.Width = new Unit("100%");
        tr.Cells.Add(td);

        tr.CssClass = "no-result";
        tb.Rows.Add(tr);
    }
}