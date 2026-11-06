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

            Response.AppendHeader("content-disposition", "attachment; filename=Stone1ARekap_Export.xls");
			fillTable1(sender, e);
		}
	}
    //Display Table
    protected void fillTable1(object sender, EventArgs e)    
    {
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);
        int shape = Convert.ToInt32(Request.QueryString["parcel01"]);
        int size = Convert.ToInt32(Request.QueryString["parcel02"]);
        int colour = Convert.ToInt32(Request.QueryString["parcel03"]);
        int clarity = Convert.ToInt32(Request.QueryString["parcel04"]);
        
        using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.sp_reportStone1A"; //The name of the above mentioned stored procedure.  
                        SqlParameter param = cmd.Parameters.Add("@keyword", SqlDbType.VarChar);
                        SqlParameter param2 = cmd.Parameters.Add("@TipeLokasi", SqlDbType.VarChar);
                        SqlParameter param3 = cmd.Parameters.Add("@Lokasi", SqlDbType.VarChar);
                        SqlParameter param4 = cmd.Parameters.Add("@Shape", SqlDbType.VarChar);
                        SqlParameter param5 = cmd.Parameters.Add("@Size", SqlDbType.VarChar);
                        SqlParameter param6 = cmd.Parameters.Add("@Colour", SqlDbType.VarChar);
                        SqlParameter param7 = cmd.Parameters.Add("@Clarity", SqlDbType.VarChar);
                        SqlParameter param8 = cmd.Parameters.Add("@Order", SqlDbType.VarChar);
                        SqlParameter param9 = cmd.Parameters.Add("@OrderBy", SqlDbType.VarChar);
                        SqlParameter param10 = cmd.Parameters.Add("@GroupBy", SqlDbType.VarChar);

                        param.Value = Request.QueryString["kw"];
                        param2.Value = Tipe;                            
                        param3.Value = IDLokasi;
                        param4.Value = shape;
                        param5.Value = size;
                        param6.Value = colour;
                        param7.Value = clarity;
                        param8.Value = Request.QueryString["order"];
                        param9.Value = Request.QueryString["orderby"];
                        param10.Value = "regular";

                        DataTable dt = new DataTable();
    
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            conn.Open();
                            adapter.Fill(dt);
                            conn.Close();
                        }                                                               
        
        TableHeaderRow htr = new TableHeaderRow();
        TableHeaderCell htd;

        htd = new TableHeaderCell();
        htd.Text = "No.";
        htd.Width = 40;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Nama Lokasi";
        htd.Width = 120;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Nama";
        htd.Width = 120;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Size";
        htd.Width = 120;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Shape";
        htd.Width = 120;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Colour";
        htd.Width = 120;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Clarity";
        htd.Width = 120;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Total Butir";
        htd.Width = 120;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Total Carat";
        htd.Width = 120;
        htr.Cells.Add(htd);

        tb.Rows.Add(htr);
        //NO-RESULT
        if (dt.Rows.Count == 0) App.TableNoResult(tb);

        else
        {
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;

            //RESULT
            int index = 1;
            foreach (DataRow row in dt.Rows)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (string)row["NamaLokasi"];
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (string)row["Nama"];
                tr.Cells.Add(td);

             
                td = new TableCell();
                td.Text = (string)row["Shape"];
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (string)row["Size"];
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (string)row["Colour"];
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = (string)row["Clarity"];
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(row["totButir"]).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(row["totCarat"]).ToString("N3");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalButir += Convert.ToDecimal(row["totButir"]);
                TotalCarat += Convert.ToDecimal(row["totCarat"]);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 7;
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

    protected void fillTable(object sender, EventArgs e)
    {
        //ORDER BY
        string o = Request.QueryString["order"] + " " + Request.QueryString["orderby"];
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);

        //SELECT
        /*var rs = from p in db.Stone1A.Where(w).OrderBy(o)
                 where
                     (
                     p.Nama.Contains(Request.QueryString["kw"]) ||
                     p.Keterangan.Contains(Request.QueryString["kw"]) ||
                     p.KodeParcel.Contains(Request.QueryString["kw"])
                     )
                 select p;*/

        var rs = (from p in db.StockProductDJ_Stone1A
                  where !p.StockProductDJ.Draft.Value
                  select new
                  {
                      ID = p.Stone1A.ID,
                      Nama = p.Stone1A.Nama,
                      KodeParcel = p.Stone1A.KodeParcel,
                      TotalButir = p.TotalButir,
                      TotalCarat = p.TotalCarat,
                      Nomor = p.StockProductDJ.Nomor,
                      Parcel01 = p.Stone1A.Parcel01,
                      Parcel02 = p.Stone1A.Parcel02,
                      Parcel03 = p.Stone1A.Parcel03,
                      Parcel04 = p.Stone1A.Parcel04,
                      Nama01 = p.Stone1A.Parcel101.Nama,
                      Nama02 = p.Stone1A.Parcel102.Nama,
                      Nama03 = p.Stone1A.Parcel103.Nama,
                      Nama04 = p.Stone1A.Parcel104.Nama,

                      StockProductDJ = p.StockProductDJ,
                      StockActualDJ = p.StockProductDJ.StockActualDJ,
                      Namalokasi = p.StockProductDJ.StockActualDJ.NamaLokasi
                  }).OrderBy(o).Where(z => z.Nama.Contains(Request.QueryString["kw"]) ||
                     z.KodeParcel.Contains(Request.QueryString["kw"]) || z.Nomor.Contains(Request.QueryString["kw"]));


        if (Tipe >= 0)
        {
            rs = rs.Where(p => p.StockActualDJ.TipeLokasi == Tipe);

            if (IDLokasi >= 0)
            {
                rs = rs.Where(p => p.StockActualDJ.IDLokasi == IDLokasi);
            }
        }

        rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

        if (Convert.ToInt32(Request.QueryString["parcel01"]) > 0) rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(Request.QueryString["parcel01"]));
        if (Convert.ToInt32(Request.QueryString["parcel02"]) > 0) rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(Request.QueryString["parcel02"]));
        if (Convert.ToInt32(Request.QueryString["parcel03"]) > 0) rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(Request.QueryString["parcel03"]));
        if (Convert.ToInt32(Request.QueryString["parcel04"]) > 0) rs = rs.Where(p => p.Parcel04 == Convert.ToInt32(Request.QueryString["parcel04"]));

        var stone = rs.GroupBy(p => p.KodeParcel);

        TableHeaderRow htr = new TableHeaderRow();
        TableHeaderCell htd;

        htd = new TableHeaderCell();
        htd.Text = "No.";
        htd.Width = 40;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Nama Lokasi";
        htd.Width = 120;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Nama";
        htd.Width = 120;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Size";
        htd.Width = 120;
        htr.Cells.Add(htd);

        htd = new TableHeaderCell();
        htd.Text = "Shape";
        htd.Width = 120;
        htr.Cells.Add(htd);


        htd = new TableHeaderCell();
        htd.Text = "Colour";
        htd.Width = 120;
        htr.Cells.Add(htd);


        htd = new TableHeaderCell();
        htd.Text = "Clarity";
        htd.Width = 120;
        htr.Cells.Add(htd);


        htd = new TableHeaderCell();
        htd.Text = "Total Butir";
        htd.Width = 120;
        htr.Cells.Add(htd);


        htd = new TableHeaderCell();
        htd.Text = "Total Carat";
        htd.Width = 120;
        htr.Cells.Add(htd);

        tb.Rows.Add(htr);
        

        //NO-RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        else
        {
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;

            //RESULT
            int index = 1;
            foreach (var r in stone)
            {
                if (!Response.IsClientConnected) break;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Namalokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama;
                tr.Cells.Add(td);



                td = new TableCell();
                td.Text = r.First().Nama01;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama02;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama03;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.First().Nama04;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Sum(p => p.TotalButir)).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Sum(p => p.TotalCarat)).ToString("N3");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalButir += Convert.ToDecimal(r.Sum(p => p.TotalButir));
                TotalCarat += Convert.ToDecimal(r.Sum(p => p.TotalCarat));
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 7;
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