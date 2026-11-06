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
    string action = "display"; // display, excel

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			fillTable(sender, e);
		}
	}

	//Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        // INIT
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);                        
        int parcel01 = Convert.ToInt32(Request.QueryString["parcel01"]);        
        int parcel02 = Convert.ToInt32(Request.QueryString["parcel02"]);        
        int parcel03 = Convert.ToInt32(Request.QueryString["parcel03"]);        
        int parcel04 = Convert.ToInt32(Request.QueryString["parcel04"]);
        int parcel05 = Convert.ToInt32(Request.QueryString["parcel05"]);
        int parcel06 = Convert.ToInt32(Request.QueryString["parcel06"]);
        int parcel07 = Convert.ToInt32(Request.QueryString["parcel07"]);    
        string keyword = Request.QueryString["kw"].ToString();

        //ORDER BY
        string o = (Request.QueryString["order"] == "Nama" ? "aaa.Nama" : Request.QueryString["order"] == "ID" ? "a.ID" :  Request.QueryString["order"] == "Nomor" ? "a.Nomor" : Request.QueryString["order"] == "KodeParcel" ? "aaa.KodeParcel":Request.QueryString["order"]) + " " + Request.QueryString["orderby"];        
        string query=@"        
                    select a.Nomor,aba.Nama Item
                    ,IIF(b.TipeLokasi=0,'Warehouse',IIF(b.TipeLokasi = 1,'Outlet','Exhibition')) AS TipeLokasi
                    ,b.NamaLokasi,aaa.Nama as Parcel,aaaa.Nama as Shape,aaab.Nama as Colour,aaac.Nama as Clarity
                    ,aa.TotalButir as Butir,cast((aa.TotalButir*aa.CaratButir) as numeric(9,3)) Carat, 
                    cast(ISNULL(HargaSatuan,0) as numeric(9,3)) HargaPerCaratUSD,cast(ISNULL(HargaTotal,0) as numeric(9,3)) TotalHargaUSD
                    from StockProductDJ a 
                    JOIN StockProductDJ_Stone2 aa on aa.IDForm=a.ID
                    JOIN Stone2 aaa on aaa.ID=aa.IDStone
                    JOIN Parcel201 aaaa on aaaa.ID=aaa.Parcel01
                    JOIN Parcel202 aaab on aaab.ID=aaa.Parcel02
                    JOIN Parcel205 aaac on aaac.ID=aaa.Parcel05
                    JOIN StockProductDJ_CharProduct ab on ab.ID=a.ID
                    JOIN CharProductItem aba on aba.ID=ab.ProductItem
                    JOIN StockActualDJ b on b.IDProduct=a.ID
                    where a.Draft = 0 and (b.IDLokasi <> -1 and (@IDLokasi <= 0 OR b.IDLokasi = @IDLokasi)) 
                    and (@TipeLokasi < 0 or b.TipeLokasi = @TipeLokasi) and InHand=1
                    and (@Parcel01 = 0 or aaa.Parcel01 = @Parcel01)
                    and (@Parcel02 = 0 or aaa.Parcel02 = @Parcel02)
                    and (@Parcel03 = 0 or aaa.Parcel03 = @Parcel03)
                    and (@Parcel04 = 0 or aaa.Parcel04 = @Parcel04)
                    and (@Parcel05 = 0 or aaa.Parcel05 = @Parcel05)
                    and (@Parcel06 = 0 or aaa.Parcel06 = @Parcel06)
                    and (@Parcel07 = 0 or aaa.Parcel07 = @Parcel07)
                    and (@Supplier = 'Supplier' or a.SupplierNama = @Supplier)
                    and (@Keyword = '' or a.Nomor like '%' + @Keyword + '%' or aaa.KodeParcel like '%' + @Keyword + '%' or aaa.Nama like '%' + @Keyword + '%')
                    and a.StatusLebur = 0 and a.StatusPenjualan=0 and a.ReturnProduct=0 
                    ORDER BY "+ o +@"
            
                ";
                    

        // PAGING (FOR DISPLAY ONLY)
        int pageNumber = Convert.ToInt32(Request.QueryString["startROW"]);
        int numberOfObjectsPerPage = 30;
        int totalData = 0;
        decimal TotalPage = 0;
        decimal TotalButir = 0m;
        decimal TotalCarat = 0m;
        decimal TotalHargaSatuan = 0m;
        decimal TotalHargaTotal = 0m;       



        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {            
            using (SqlCommand command = new SqlCommand(query, connection)){
                command.CommandTimeout = 10;
                command.Parameters.Add(new SqlParameter("@Keyword", SqlDbType.VarChar) { Value = Request.QueryString["kw"].Trim() });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = Tipe });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@Parcel01", SqlDbType.Int) { Value = parcel01 });
                command.Parameters.Add(new SqlParameter("@Parcel02", SqlDbType.Int) { Value = parcel02 });
                command.Parameters.Add(new SqlParameter("@Parcel03", SqlDbType.Int) { Value = parcel03 });
                command.Parameters.Add(new SqlParameter("@Parcel04", SqlDbType.Int) { Value = parcel04 });                
                command.Parameters.Add(new SqlParameter("@Parcel05", SqlDbType.Int) { Value = parcel05 });                
                command.Parameters.Add(new SqlParameter("@Parcel06", SqlDbType.Int) { Value = parcel06 });                
                command.Parameters.Add(new SqlParameter("@Parcel07", SqlDbType.Int) { Value = parcel07 });                
                command.Parameters.Add(new SqlParameter("@Supplier", SqlDbType.VarChar) { Value = Request.QueryString["supplier"] });                
                connection.Open();
                SqlDataAdapter adapter1 = new SqlDataAdapter(command);
                DataSet ds1 = new DataSet();
                adapter1.Fill(ds1);
                

                DataTable data = ds1.Tables[0];                
                if (data.Rows.Count == 0 ) 
                {                    
                    TableNoResult(tb);
                }            
        

        //Build Header     
        TableHeaderRow htr = new TableHeaderRow();        
        TableHeaderCell htd = new TableHeaderCell();            
        tableBuilder(htr, htd, mode);        

        tb.Rows.Add(htr);

        //RESULT
            int index = (pageNumber > 0) ? ((pageNumber-1) * 30) + 1 : 1;                        
            foreach (DataRow row in data.Rows)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;                            

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["Nomor"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["Item"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["TipeLokasi"].ToString();
                tr.Cells.Add(td);

                string Lokasi = row["NamaLokasi"].ToString();

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["Parcel"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["Shape"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["Colour"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = row["Clarity"].ToString();
                tr.Cells.Add(td);
                tb.Rows.Add(tr);

                td = new TableCell();
                td.Text = Convert.ToDecimal(row["Butir"]).ToString();
                tr.Cells.Add(td);
                tb.Rows.Add(tr);

                td = new TableCell();
                td.Text = Convert.ToDecimal(row["Carat"]).ToString("N3");
                tr.Cells.Add(td);
                tb.Rows.Add(tr);

                td = new TableCell();
                td.Text = Convert.ToDecimal(row["HargaPerCaratUSD"]).ToString("N2");
                tr.Cells.Add(td);
                tb.Rows.Add(tr);

                td = new TableCell();
                td.Text = Convert.ToDecimal(row["TotalHargaUSD"]).ToString("N2");
                tr.Cells.Add(td);
                tb.Rows.Add(tr);
            }
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
    protected void tableBuilder(TableHeaderRow htr, TableHeaderCell htd, string mode){
        htd = new TableHeaderCell();
        htd.Text = "No.";
        htd.Width = 50;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "PLU";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Product Item";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Tipe Lokasi";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Lokasi";
        htd.Width = 150;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Nama";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Shape";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Colour";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Clarity";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Butir";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Carat";
        htd.Width = 100;
        htr.Cells.Add(htd);

        if (mode == "accounting")
        {
            htd = new TableHeaderCell();
            htd.Text = "Harga Per Karat USD";
            htd.Width = 100;
            htr.Cells.Add(htd);

            htd = new TableHeaderCell();
            htd.Text = "Total Harga USD";
            htd.Width = 100;
            htr.Cells.Add(htd);
        }
    }    


}