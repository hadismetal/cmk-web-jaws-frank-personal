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

            Response.AppendHeader("content-disposition", "attachment; filename=Stone5_Export.xls");

			fillTable1(sender, e);
		}
	}

	
	//Display Table
    protected void fillTable1(object sender, EventArgs e)
    {              
        // INIT
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);
        string keyword = Request.QueryString["kw"].ToString();
        string Supplier = Request.QueryString["supplier"];            
        int Parcel01 = Convert.ToInt32(Request.QueryString["parcel01"]);
        int Parcel02 = Convert.ToInt32(Request.QueryString["parcel02"]);
        int Parcel03 = Convert.ToInt32(Request.QueryString["parcel03"]);
        int Parcel04 = Convert.ToInt32(Request.QueryString["parcel04"]);
        int Parcel05 = Convert.ToInt32(Request.QueryString["parcel05"]);
        int Parcel06 = Convert.ToInt32(Request.QueryString["parcel06"]);
        int Parcel07 = Convert.ToInt32(Request.QueryString["parcel07"]);
        int Parcel08 = Convert.ToInt32(Request.QueryString["parcel08"]);
        int Parcel09 = Convert.ToInt32(Request.QueryString["parcel09"]);

        //ORDER BY
        string o = (Request.QueryString["order"] == "Nama" ? "aaa.Nama" : Request.QueryString["order"] == "ID" ? "a.ID" :  Request.QueryString["order"] == "Nomor" ? "a.Nomor" : Request.QueryString["order"]) + " " + Request.QueryString["orderby"];

        string query = @"
                        SELECT a.Nomor as PLU, aaa.Nama as Item, IIF(C.TipeLokasi=0, 'Warehouse',IIF(C.TipeLokasi = 1,'Outlet','Exhibition')) TipeLokasi,
                        c.NamaLokasi, ba.Nama as Parcel, baa.Nama as Code ,bab.Nama as [Source], bac.Nama as Color, bad.Nama as Shape, bae.Nama as Overtone, baf.Nama as Diameter,
                        bag.Nama as Surface, bah.Nama as Luster, bai.Nama as Treatment, b.TotalButir, b.TotalCarat, ISNULL(b.HargaSatuan,0) HargaPerCaratUSD, ISNULL(B.HargaTotal,0) TotalHargaUSD
                        FROM StockProductDJ a
                        JOIN StockProductDJ_Stone5 b on b.IDForm=a.ID
                        JOIN Stone5 ba on ba.ID=b.IDStone
                        JOIN StockActualDJ c on c.IDProduct=a.ID
                        JOIN StockProductDJ_CharProduct aa on aa.ID=a.ID
                        JOIN CharProductItem aaa on aaa.ID=aa.ProductItem
                        JOIN Parcel501 baa on baa.ID=ba.Parcel01 /*parcel 01 & 02 */ JOIN Parcel502 bab on bab.ID=ba.Parcel02
                        JOIN Parcel503 bac on bac.ID=ba.Parcel03 /*parcel 03 & 04 */ JOIN Parcel504 bad on bad.ID=ba.Parcel04
                        JOIN Parcel505 bae on bae.ID=ba.Parcel05 /*parcel 05 & 06 */ JOIN Parcel506 baf on baf.ID=ba.Parcel06
                        JOIN Parcel507 bag on bag.ID=ba.Parcel07 /*parcel 07 & 08 */ JOIN Parcel508 bah on bah.ID=ba.Parcel08
                        JOIN Parcel509 bai on bai.ID=ba.Parcel09 /*parcel 07 & 08 */
                        WHERE a.Draft=0  and a.StatusLebur = 0 and a.ReturnProduct = 0 and a.StatusPenjualan = 0
                        and (@Parcel01=0 or ba.Parcel01=@Parcel01) and (@Parcel02=0 or ba.Parcel02=@Parcel02)
                        and (@Parcel03=0 or ba.Parcel03=@Parcel03) and (@Parcel04=0 or ba.Parcel04=@Parcel04)
                        and (@Parcel05=0 or ba.Parcel05=@Parcel05) and (@Parcel06=0 or ba.Parcel06=@Parcel06)
                        and (@Parcel07=0 or ba.Parcel07=@Parcel07) and (@Parcel08=0 or ba.Parcel08=@Parcel08)
                        and (@Parcel09=0 or ba.Parcel09=@Parcel09) and (@Supplier = 'Supplier' or a.SupplierNama = @Supplier)
                        and (@TipeLokasi < 0 or c.TipeLokasi = @TipeLokasi) and ((@IDLokasi <= 0 or c.IDLokasi=@IDLokasi) and c.IDLokasi <> -1)
                        and (@keyword = '' or ba.Nama like '%'+ @keyword +'%' or ba.KodeParcel like '%'+ @keyword +'%' or a.Nomor like '%'+ @keyword +'%')
                        ORDER BY "+ o +@"                        
                    ";

        // PAGING (FOR DISPLAY ONLY)        
                
        decimal TotalButir = 0m;
        decimal TotalCarat = 0m;
        decimal TotalHargaSatuan = 0m;
        decimal TotalHargaTotal = 0m;                
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {            
            using (SqlCommand command = new SqlCommand(query, connection)){
                command.CommandTimeout = 10;
                command.Parameters.Add(new SqlParameter("@keyword", SqlDbType.VarChar) { Value = Request.QueryString["kw"] });
                command.Parameters.Add(new SqlParameter("@TipeLokasi", SqlDbType.Int) { Value = Tipe }); 
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@Parcel01", SqlDbType.Int) { Value = Parcel01 }); 
                command.Parameters.Add(new SqlParameter("@Parcel02", SqlDbType.Int) { Value = Parcel02 });
                command.Parameters.Add(new SqlParameter("@Parcel03", SqlDbType.Int) { Value = Parcel03 }); 
                command.Parameters.Add(new SqlParameter("@Parcel04", SqlDbType.Int) { Value = Parcel04 });
                command.Parameters.Add(new SqlParameter("@Parcel05", SqlDbType.Int) { Value = Parcel05 }); 
                command.Parameters.Add(new SqlParameter("@Parcel06", SqlDbType.Int) { Value = Parcel06 });
                command.Parameters.Add(new SqlParameter("@Parcel07", SqlDbType.Int) { Value = Parcel07 }); 
                command.Parameters.Add(new SqlParameter("@Parcel08", SqlDbType.Int) { Value = Parcel08 });
                command.Parameters.Add(new SqlParameter("@Parcel09", SqlDbType.Int) { Value = Parcel09 }); 
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
                    td.Text = r["PLU"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["Item"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["TipeLokasi"].ToString();
                    tr.Cells.Add(td);            

                    td = new TableCell();
                    td.Text = r["NamaLokasi"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["Parcel"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["Code"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["Source"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["Color"].ToString();
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
                    td.Text = r["Surface"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["Luster"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r["Treatment"].ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (action == "excel") ? Convert.ToDecimal(r["TotalButir"]).ToString("0").Replace(",", ".") : Convert.ToDecimal(r["TotalButir"]).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (action == "excel") ? Convert.ToDecimal(r["TotalCarat"]).ToString("0.000").Replace(",", ".") : Convert.ToDecimal(r["TotalCarat"]).ToString("N3");
                    tr.Cells.Add(td);

                    if (mode == "accounting")
                    {
                        td = new TableCell();
                        td.Text = (action == "excel") ? Convert.ToDecimal(r["HargaPerCaratUSD"]).ToString("0.00").Replace(",", ".") : Convert.ToDecimal(r["HargaPerCaratUSD"]).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = (action == "excel") ? Convert.ToDecimal(r["TotalHargaUSD"]).ToString("0.00").Replace(",", ".") : Convert.ToDecimal(r["TotalHargaUSD"]).ToString("N2");
                        tr.Cells.Add(td);
                    }

                    tb.Rows.Add(tr);

                    TotalButir += Convert.ToDecimal(r["TotalButir"]);
                    TotalCarat += Convert.ToDecimal(r["TotalCarat"]);
                    TotalHargaSatuan += Convert.ToDecimal(r["HargaPerCaratUSD"]);
                    TotalHargaTotal += Convert.ToDecimal(r["TotalHargaUSD"]);   
                }
            
            }
        }                    
                                            
    }

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

        var rs = (from p in db.StockProductDJ_Stone5
                  where !p.StockProductDJ.Draft.Value
                  && p.StockProductDJ.StockActualDJ.IDLokasi != -1
                  select new
                  {
                      TipeLokasi = p.StockProductDJ.StockActualDJ.TipeLokasi,
                      IDLokasi = p.StockProductDJ.StockActualDJ.IDLokasi,
                      ID = p.Stone5.ID,
                      Nama = p.Stone5.Nama,
                      KodeParcel = p.Stone5.KodeParcel,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      Nomor = p.StockProductDJ.Nomor,
                      Parcel01 = p.Stone5.Parcel01,
                      Parcel02 = p.Stone5.Parcel02,
                      Parcel03 = p.Stone5.Parcel03,
                      Parcel04 = p.Stone5.Parcel04,
                      Parcel05 = p.Stone5.Parcel05,
                      Parcel06 = p.Stone5.Parcel06,
                      Parcel07 = p.Stone5.Parcel07,
                      Parcel08 = p.Stone5.Parcel08,
                      Parcel09 = p.Stone5.Parcel09,
                      Nama01 = p.Stone5.Parcel501.Nama,
                      Nama02 = p.Stone5.Parcel502.Nama,
                      Nama03 = p.Stone5.Parcel503.Nama,
                      Nama04 = p.Stone5.Parcel504.Nama,
                      Nama05 = p.Stone5.Parcel505.Nama,
                      Nama06 = p.Stone5.Parcel506.Nama,
                      Nama07 = p.Stone5.Parcel507.Nama,
                      Nama08 = p.Stone5.Parcel508.Nama,
                      Nama09 = p.Stone5.Parcel509.Nama,
                      StockProductDJ = p.StockProductDJ,
                      StockActualDJ = p.StockProductDJ.StockActualDJ,
                      HargaSatuan = p.HargaSatuan.Value,
                      HargaTotal = p.HargaTotal.Value,
                      Brand = p
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
        if (Convert.ToInt32(Request.QueryString["parcel05"]) > 0) rs = rs.Where(p => p.Parcel05 == Convert.ToInt32(Request.QueryString["parcel05"]));
        if (Convert.ToInt32(Request.QueryString["parcel06"]) > 0) rs = rs.Where(p => p.Parcel06 == Convert.ToInt32(Request.QueryString["parcel06"]));
        if (Convert.ToInt32(Request.QueryString["parcel07"]) > 0) rs = rs.Where(p => p.Parcel07 == Convert.ToInt32(Request.QueryString["parcel07"]));
        if (Convert.ToInt32(Request.QueryString["parcel08"]) > 0) rs = rs.Where(p => p.Parcel08 == Convert.ToInt32(Request.QueryString["parcel08"]));
        if (Convert.ToInt32(Request.QueryString["parcel09"]) > 0) rs = rs.Where(p => p.Parcel09 == Convert.ToInt32(Request.QueryString["parcel09"]));

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
        htd.Text = "Code";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Source";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Color";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Shape";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Overtone";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Diameter";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Surface";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Luster";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Treatment";
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
            td.Text = r.Nama04;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama05;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama06;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama07;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama08;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama09;
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
        int cols = 15;
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
        htd.Width = 150;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Code";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Source";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Color";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Shape";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Overtone";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Diameter";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Surface";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Luster";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Treatment";
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

    }
}