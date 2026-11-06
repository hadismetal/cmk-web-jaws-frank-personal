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
			fillTable1(sender, e);
		}
	}

	//Display Table
    protected void fillTable1(object sender, EventArgs e)
    {
        
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);
        int parcel1 = Convert.ToInt32(Request.QueryString["parcel01"]);
        int parcel2 = Convert.ToInt32(Request.QueryString["parcel02"]);
        int parcel3 = Convert.ToInt32(Request.QueryString["parcel03"]);
        int parcel4 = Convert.ToInt32(Request.QueryString["parcel04"]);
        int parcel5 = Convert.ToInt32(Request.QueryString["parcel05"]);
        int parcel6 = Convert.ToInt32(Request.QueryString["parcel06"]);
        int parcel7 = Convert.ToInt32(Request.QueryString["parcel07"]);
        int Supplier = Convert.ToInt32(Request.QueryString["supplier"]);

        int pageNumber = 1;
        int numberOfObjectsPerPage = 30;
        decimal TotalPage = 0;

       decimal TotalButir = 0m;
        decimal TotalCarat = 0m;
        decimal TotalHarga = 0m;
        decimal TotalHargaSatuan = 0m;
        decimal TotalHargaTotal = 0m;
        decimal TotalHargaTotalM = 0m;


        //Header
        TableHeaderRow htr = new TableHeaderRow();        
        TableHeaderCell htd = new TableHeaderCell();
        tableBuilder(htr, htd, mode);

        tb.Rows.Add(htr);

        // string o = (Request.QueryString["order"] == "Nama" ? "c.Nama" : Request.QueryString["order"] == "ID" ? "c.ID" : Request.QueryString["order"]) + " " + Request.QueryString["orderby"];
        string o = (Request.QueryString["order"] == "Nama" ? "c.Nama" : Request.QueryString["order"] == "ID" ? "c.ID" : Request.QueryString["order"] == "Nomor" ? "b.Nomor" : Request.QueryString["order"] == "KodeParcel" ? "c.KodeParcel":Request.QueryString["order"]) + " " + Request.QueryString["orderby"];


        string query = @"	select b.Nomor as PLU, baa.Nama as ProductItem, IIF(d.TipeLokasi=0,'Warehouse',IIF(d.TipeLokasi = 1,'Outlet','Exhibition')) tipelokasi,
							D.NamaLokasi, c.Nama, ca.nama nmShape, cc.nama nmColor, cd.nama nmClarity, ce.Nama as nmCutting, cf.Nama as nmBrandSt,a.GIA,ISNULL(e.m_report,'') noSarine, 
							CONVERT(varchar,b.OperatorTgl,105) TanggalPenerimaan, DATEDIFF(day, b.OperatorTgl, GETDATE()) AS UsiaStock,
                            ISNULL(a.TotalButir,0) TotalButir,ISNULL(a.TotalCarat,0) TotalCarat,ISNULL(a.HargaSatuan,0) HargaSatuan,ISNULL(a.HargaTotal,0) HargaTotal,
                            ISNULL(a.HargaSatuanM,0) HargaSatuanM,ISNULL(a.HargaTotalM,0) HargaTotalM
                            from StockProductDJ_Stone1B a
                            JOIN StockProductDJ b on b.id=a.IDForm
                            JOIN StockProductDJ_CharProduct ba on ba.ID=b.ID
							JOIN CharProductItem baa on baa.id=ba.ProductItem
                            JOIN Stone1B c on c.ID=a.IDStone
                            JOIN StockActualDJ d on d.IDProduct=b.ID
                            JOIN Parcel101 ca on ca.id=c.Parcel01                            
                            JOIN Parcel103 cc on cc.id=c.Parcel03
                            JOIN Parcel104 cd on cd.id=c.Parcel04
							JOIN Parcel105 ce on ce.id=c.Parcel05
							JOIN Parcel106 cf on cf.id=c.Parcel06							
							LEFT JOIN [DB_PROD_MYAPPS].dbcmk.dbo.table_sarin e on e.m_gia=a.GIA
                            where b.Draft=0 and a.StatusTerpasang=1 
                            and b.StatusLebur=0 and b.StatusPenjualan=0 and b.ReturnProduct=0
                            and ((@IDLokasi <= 0  or d.IDLokasi = @IDLokasi) and d.IDLokasi <> -1) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
                            and (@Parcel1 = 0 or c.Parcel01=@Parcel1) and (@Parcel2 = 0 or c.Parcel02=@Parcel2) 
                            and (@Parcel3 = 0 or c.parcel03=@Parcel3) and (@Parcel4 = 0 or c.Parcel04=@Parcel4) 
                            and (@Parcel5 = 0 or c.Parcel05=@Parcel5) and (@Parcel6 = 0 or c.parcel06=@Parcel6)
							and (@Parcel7 = 0 or c.Parcel07=@Parcel7) and (@Supplier=0 or b.Supplier=@Supplier)
							and (@kw = '' or c.Nama like '%'+@kw+'%' or c.KodeParcel like '%'+@kw+'%' or b.Nomor like '%'+@kw+'%' )
                            order by " + o ;                        

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {                       
            

            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // command.CommandTimeout = 30;
                command.Parameters.Add(new SqlParameter("@kw", SqlDbType.VarChar) { Value = Request.QueryString["kw"].Trim() });
                command.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.Int) { Value = Tipe });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@parcel1", SqlDbType.Int) { Value = parcel1 });
                command.Parameters.Add(new SqlParameter("@parcel2", SqlDbType.Int) { Value = parcel2 });
                command.Parameters.Add(new SqlParameter("@parcel3", SqlDbType.Int) { Value = parcel3 });
                command.Parameters.Add(new SqlParameter("@parcel4", SqlDbType.Int) { Value = parcel4 });                
                command.Parameters.Add(new SqlParameter("@parcel5", SqlDbType.Int) { Value = parcel5 });                
                command.Parameters.Add(new SqlParameter("@parcel6", SqlDbType.Int) { Value = parcel6 });                
                command.Parameters.Add(new SqlParameter("@parcel7", SqlDbType.Int) { Value = parcel7 });                
                command.Parameters.Add(new SqlParameter("@Supplier", SqlDbType.Int) { Value = Supplier });

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                
                int no  = 1;
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count == 0)
                    {
                        App.TableNoResult(tb);                                              
                    }                                        

                   
                    foreach (DataRow row in dt.Rows)
                    {                        

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = no++.ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["PLU"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["ProductItem"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["tipelokasi"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["NamaLokasi"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["Nama"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmShape"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmColor"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmClarity"].ToString();
                        tr.Cells.Add(td);
                        
                        td = new TableCell();
                        td.Text = row["nmCutting"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmBrandSt"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["GIA"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["noSarine"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["TanggalPenerimaan"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["UsiaStock"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["TotalButir"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["TotalCarat"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = ((decimal)row["HargaSatuan"]).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = ((decimal)row["HargaTotal"]).ToString("N2");
                        tr.Cells.Add(td);



                        tb.Rows.Add(tr);
                                    
                    }

                        // TableRow tr2 = new TableRow();
                        // TableCell td2;

                        // td2 = new TableCell();
                        // td2.Text = "<b>Total</b>";
                        // td2.ColumnSpan = 10;
                        // tr2.Cells.Add(td2);

                        // td2 = new TableCell();
                        // td2.Text = TotalButir.ToString("N0");
                        // td2.CssClass = "text-right";
                        // tr2.Cells.Add(td2);

                        // td2 = new TableCell();
                        // td2.Text = TotalCarat.ToString("N3");
                        // td2.CssClass = "text-right";
                        // tr2.Cells.Add(td2);

                        // td2 = new TableCell();
                        // td2.Text = "-";
                        // td2.CssClass = "text-right";
                        // tr2.Cells.Add(td2);

                        // td2 = new TableCell();
                        // td2.Text = TotalHarga.ToString("N2");
                        // td2.CssClass = "text-right";
                        // tr2.Cells.Add(td2);

                        // td2 = new TableCell();
                        // td2.Text = "-";
                        // td2.CssClass = "text-right";
                        // tr2.Cells.Add(td2);

                        // td2 = new TableCell();
                        // td2.Text = TotalHargaTotalM.ToString("N2");
                        // td2.CssClass = "text-right";
                        // tr2.Cells.Add(td2);

                        // td2 = new TableCell();
                        // td2.Text = "<b></b>";
                        // td2.ColumnSpan = 4;
                        // tr2.Cells.Add(td2);

                        // tb.Rows.Add(tr2);
                }                  
            }
        
        connection.Close();
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
        htd.Width = 150;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Tipe Lokasi";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Lokasi";
        htd.Width = 100;
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
        htd.Text = "Cutting";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Brand";
        htd.Width = 200;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "GIA";
        htd.Width = 100;
        htr.Cells.Add(htd);
        
        htd = new TableHeaderCell();
        htd.Text = "No.Sarine";
        htd.Width = 150;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Tanggal Penerimaan";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Usia Stock";
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

    protected void fillTable(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);
        int parcel1 = Convert.ToInt32(Request.QueryString["parcel01"]);
        int parcel2 = Convert.ToInt32(Request.QueryString["parcel02"]);
        int parcel3 = Convert.ToInt32(Request.QueryString["parcel03"]);
        int parcel4 = Convert.ToInt32(Request.QueryString["parcel04"]);


        decimal TotalButir = 0m;
        decimal TotalCarat = 0m;
        decimal TotalHarga = 0m;

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
        htd.Text = "Total Butir";
        htd.Width = 100;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Total Carat ";
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
        int no = 1;
        // string o = (Request.QueryString["order"] == "Nama" ? "c.Nama" : Request.QueryString["order"] == "ID" ? "c.ID" : Request.QueryString["order"]) + " " + Request.QueryString["orderby"];
        string o = (Request.QueryString["order"] == "Nama" ? "c.Nama" : Request.QueryString["order"] == "ID" ? "c.ID" : Request.QueryString["order"] == "Nomor" ? "b.Nomor" : Request.QueryString["order"]) + " " + Request.QueryString["orderby"];

        

        string query = @"select b.Nomor PLU,baa.nama nmItem,IIF(d.TipeLokasi=0,'Warehouse',IIF(d.TipeLokasi = 1,'Outlet','Exhibition')) tipelokasi
							,d.NamaLokasi,c.Nama,ca.nama nmShape,cc.nama nmColor,
                            cd.nama nmClarity,SUM(ISNULL(a.TotalButir,0)) TotalButir,SUM(ISNULL(a.TotalCarat,0))TotalCarat,ISNULL(a.HargaSatuan,0) HargaSatuan,ISNULL(HargaTotal,0) HargaTotal
                            from StockProductDJ_Stone1A a
                            JOIN StockProductDJ b on b.id=a.IDForm
                            JOIN StockProductDJ_CharProduct ba on ba.ID=b.ID
							JOIN CharProductItem baa on baa.id=ba.ProductItem
                            JOIN Stone1A c on c.ID=a.IDStone
                            JOIN StockActualDJ d on d.IDProduct=b.ID
                            JOIN Parcel101 ca on ca.id=c.Parcel01
                            JOIN Parcel102 cb on cb.id=c.Parcel02
                            JOIN Parcel103 cc on cc.id=c.Parcel03
                            JOIN Parcel104 cd on cd.id=c.Parcel04
                            where b.Draft=0
                            and b.StatusLebur=0 and b.StatusPenjualan=0 and b.ReturnProduct=0
                            and (@IDLokasi < 0 or d.IDLokasi = @IDLokasi) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
                            and (@parcel1 = 0 or ca.id=@parcel1) and (@parcel2 = 0 or cb.id=@parcel2) and (@parcel3 = 0 or cc.id=@parcel3)
                            and (@parcel4 = 0 or cd.id=@parcel4)                            
                            and (@kw = '' or c.Nama like '%'+@kw+'%' or c.KodeParcel like '%'+@kw+'%' or b.Nomor like '%'+@kw+'%' )
                            group by b.Nomor,c.id,c.nama,c.kodeparcel,ca.nama,cb.nama,cc.nama,cd.nama,d.NamaLokasi,a.HargaSatuan,a.HargaTotal,baa.nama,d.TipeLokasi
                            order by " + o ;
        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {                                    
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandTimeout = 10;
                command.Parameters.Add(new SqlParameter("@kw", SqlDbType.VarChar) { Value = Request.QueryString["kw"].Trim() });
                command.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.Int) { Value = Tipe });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@parcel1", SqlDbType.Int) { Value = parcel1 });
                command.Parameters.Add(new SqlParameter("@parcel2", SqlDbType.Int) { Value = parcel2 });
                command.Parameters.Add(new SqlParameter("@parcel3", SqlDbType.Int) { Value = parcel3 });
                command.Parameters.Add(new SqlParameter("@parcel4", SqlDbType.Int) { Value = parcel4 });                
             

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                                
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count == 0)
                    {
                        App.TableNoResult(tb);                                                
                    }                                        

                   
                    foreach (DataRow row in dt.Rows)
                    {                        

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = no++.ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["PLU"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmItem"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["tipelokasi"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["NamaLokasi"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["Nama"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmShape"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmColor"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmClarity"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["TotalButir"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["TotalCarat"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = ((decimal)row["HargaSatuan"]).ToString("N2");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = ((decimal)row["HargaTotal"]).ToString("N2");
                        tr.Cells.Add(td);



                        tb.Rows.Add(tr);
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
                }                                
            }
        
        connection.Close();
        }
    }


}