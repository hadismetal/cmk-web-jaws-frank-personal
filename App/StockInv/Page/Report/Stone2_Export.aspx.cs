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
    
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Stone2_Export.xls");

            fillTable1(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        /// Master Brand
        var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocWarehouse
        var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocOutlet
        var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocExhibition
        var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

        // INIT
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);
        string keyword = Request.QueryString["kw"].ToString();

        //ORDER BY
        string o = Request.QueryString["order"] + " " + Request.QueryString["orderby"];

        var rs = (from p in db.StockProductDJ_Stone2
                  where !p.StockProductDJ.Draft.Value
                  && p.StockProductDJ.StockActualDJ.IDLokasi != -1
                  select new
                  {
                      TipeLokasi = p.StockProductDJ.StockActualDJ.TipeLokasi,
                      IDLokasi = p.StockProductDJ.StockActualDJ.IDLokasi,
                      ID = p.Stone2.ID,
                      Nama = p.Stone2.Nama,
                      KodeParcel = p.Stone2.KodeParcel,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      Nomor = p.StockProductDJ.Nomor,
                      Parcel01 = p.Stone2.Parcel01,
                      Parcel02 = p.Stone2.Parcel02,
                      Parcel03 = p.Stone2.Parcel03,
                      Parcel04 = p.Stone2.Parcel04,
                      Nama01 = p.Stone2.Parcel201.Nama,
                      Nama02 = p.Stone2.Parcel202.Nama,
                      Nama03 = p.Stone2.Parcel203.Nama,
                      Nama04 = p.Stone2.Parcel204.Nama,
                      StockProductDJ = p.StockProductDJ,
                      StockActualDJ = p.StockProductDJ.StockActualDJ,
                      HargaSatuan = p.HargaSatuan.Value,
                      HargaTotal = p.HargaTotal.Value
                  });
        //.OrderBy(o);
        if (keyword != "")
        {
            rs = rs.Where(z => z.Nama.Contains(keyword)
                       || z.KodeParcel.Contains(keyword)
                       || z.Nomor.Contains(keyword)
                       );
        }

        if (Tipe != -1)
            rs = rs.Where(p => p.TipeLokasi == Tipe);
        if (IDLokasi != -1)
            rs = rs.Where(p => p.IDLokasi == IDLokasi && p.TipeLokasi == Tipe);

        string Supplier = Request.QueryString["supplier"];
        if (Supplier != "" && Supplier != "Supplier")
            rs = rs.Where(p => p.StockProductDJ.SupplierNama.Equals(Supplier));

        rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

        if (Convert.ToInt32(Request.QueryString["parcel01"]) > 0) rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(Request.QueryString["parcel01"]));
        if (Convert.ToInt32(Request.QueryString["parcel02"]) > 0) rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(Request.QueryString["parcel02"]));
        if (Convert.ToInt32(Request.QueryString["parcel03"]) > 0) rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(Request.QueryString["parcel03"]));
        if (Convert.ToInt32(Request.QueryString["parcel04"]) > 0) rs = rs.Where(p => p.Parcel04 == Convert.ToInt32(Request.QueryString["parcel04"]));

        // PAGING (FOR DISPLAY ONLY)
        int pageNumber = 1;
        int numberOfObjectsPerPage = 30;
        decimal TotalPage = 0;

        if (action == "display")
        {
            if (Request.QueryString["page"] != "")
                pageNumber = Convert.ToInt32(Request.QueryString["page"]);
            if (pageNumber < 0)
                pageNumber = 1;

            TotalPage = Convert.ToDecimal(rs.Count()) / Convert.ToDecimal(numberOfObjectsPerPage);

            TotalPage = Math.Ceiling(TotalPage);

            rs = rs.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);
        }

        //NO-RESULT
        if (rs.Count() == 0) TableNoResult(tb);

        decimal TotalButir = 0m;
        decimal TotalCarat = 0m;
        decimal TotalHargaSatuan = 0m;
        decimal TotalHargaTotal = 0m;

        TableHeaderRow htr = new TableHeaderRow();
        TableHeaderCell htd;

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

        tb.Rows.Add(htr);

        //RESULT
        int index =  1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DL.LocationType(r.StockActualDJ.TipeLokasi);
            tr.Cells.Add(td);

            string Lokasi = "";

            if (r.StockActualDJ.TipeLokasi == 0)
                Lokasi = rsWarehouse[r.StockActualDJ.IDLokasi];
            else if (r.StockActualDJ.TipeLokasi == 1)
                Lokasi = rsOutlet[r.StockActualDJ.IDLokasi];
            else if (r.StockActualDJ.TipeLokasi == 2)
                Lokasi = rsExhibition[r.StockActualDJ.IDLokasi];

            td = new TableCell();
            td.Text = Lokasi;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama01;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama02;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama03;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (action == "excel") ? r.TotalButir.ToString("0").Replace(",", ".") : r.TotalButir.ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (action == "excel") ? r.TotalCarat.ToString("0.000").Replace(",", ".") : r.TotalCarat.ToString("N3");
            tr.Cells.Add(td);

            if (mode == "accounting")
            {
                td = new TableCell();
                td.Text = (action == "excel") ? r.HargaSatuan.ToString("0.00").Replace(",", ".") : r.HargaSatuan.ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (action == "excel") ? r.HargaTotal.ToString("0.00").Replace(",", ".") : r.HargaTotal.ToString("N2");
                tr.Cells.Add(td);
            }

            tb.Rows.Add(tr);

            TotalButir += r.TotalButir;
            TotalCarat += r.TotalCarat;
            TotalHargaSatuan += r.HargaSatuan;
            TotalHargaTotal += r.HargaTotal;
        }

        TableRow tr2 = new TableRow();
        TableCell td2;

        td2 = new TableCell();
        td2.Text = "<b>Total</b>";
        int cols = 9;
        td2.ColumnSpan = (mode == "accounting") ? cols : cols - 2;
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = (action == "excel") ? TotalButir.ToString("0").Replace(",", ".") : TotalButir.ToString("N0");
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = (action == "excel") ? TotalCarat.ToString("0.000").Replace(",", ".") : TotalCarat.ToString("N3");
        tr2.Cells.Add(td2);

        if (mode == "accounting")
        {

            td2 = new TableCell();
            td2.Text = (action == "excel") ? TotalHargaSatuan.ToString("0.00").Replace(",", ".") : TotalHargaSatuan.ToString("N2");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = (action == "excel") ? TotalHargaTotal.ToString("0.00").Replace(",", ".") : TotalHargaTotal.ToString("N2");
            tr2.Cells.Add(td2);
        }

        tb.Rows.Add(tr2);

    }
    protected void fillTable1(object sender, EventArgs e)
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
                td.Text = Convert.ToDecimal(row["Butir"]).ToString("0").Replace(",", ".");
                tr.Cells.Add(td);
                tb.Rows.Add(tr);

                td = new TableCell();
                td.Text = Convert.ToDecimal(row["Carat"]).ToString("0.000").Replace(",", ".");
                tr.Cells.Add(td);
                tb.Rows.Add(tr);

                td = new TableCell();
                td.Text = Convert.ToDecimal(row["HargaPerCaratUSD"]).ToString("0.00").Replace(",", ".");
                tr.Cells.Add(td);
                tb.Rows.Add(tr);

                td = new TableCell();
                td.Text = Convert.ToDecimal(row["TotalHargaUSD"]).ToString("0.00").Replace(",", ".");
                tr.Cells.Add(td);
                tb.Rows.Add(tr);
            }
            }
        }
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