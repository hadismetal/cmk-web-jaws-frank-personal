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
        string o = (Request.QueryString["order"] == "Nama" ? "c.Nama" : Request.QueryString["order"] == "ID" ? "c.ID" : Request.QueryString["order"] == "Nomor" ? "b.Nomor" : Request.QueryString["order"] == "KodeParcel" ? "c.KodeParcel":Request.QueryString["order"]) + " " + Request.QueryString["orderby"];

        

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
                            and ((@IDLokasi <= 0 or d.IDLokasi = @IDLokasi) and d.IDLokasi <> -1) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
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
                }                                
            }
        
        connection.Close();
        }
    }


}