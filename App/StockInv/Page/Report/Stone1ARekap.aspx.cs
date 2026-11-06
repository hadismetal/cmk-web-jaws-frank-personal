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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			kw.Focus(); //Keyword

            //DLC
            int Tipe = 1;
            if (Request.QueryString["tipelokasi"] != "")
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);

            DLC.LocationList(lokasi, Tipe);
            Ddl.Filter(parcel01,"parcel101");
			Ddl.Filter(parcel02,"parcel102");
			Ddl.Filter(parcel03,"parcel103");
			Ddl.Filter(parcel04,"parcel104");

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e)
    {
		string href = App.RefreshURL(this);

		//Customize-Qstr
		href += "&kw=" + kw.Text;
		href += "&parcel01=" + parcel01.SelectedValue;
		href += "&parcel02=" + parcel02.SelectedValue;
		href += "&parcel03=" + parcel03.SelectedValue;
		href += "&parcel04=" + parcel04.SelectedValue;
		href += "&order=" + order.SelectedValue;
		href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        Response.Redirect(href);
	}
    protected void Print(object sender, EventArgs e)
    {
        string href = "Stone1ARekap_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&order=" + order.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "Stone1ARekap_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedValue;
        href += "&parcel02=" + parcel02.SelectedValue;
        href += "&parcel03=" + parcel03.SelectedValue;
        href += "&parcel04=" + parcel04.SelectedValue;
        href += "&order=" + order.SelectedValue;
        href += "&orderby=" + orderby.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        Response.Redirect(href);
    }

    protected void fillFilter() {
        tipelokasi.Items.Clear();
        tipelokasi.Items.Add(new ListItem("All", "-1"));
        tipelokasi.Items.Add(new ListItem("Warehouse", "0"));
        tipelokasi.Items.Add(new ListItem("Outlet", "1"));
        tipelokasi.Items.Add(new ListItem("Exhibition", "2"));

        if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
            parcel01.SelectedValue = Request.QueryString["parcel01"];
            parcel02.SelectedValue = Request.QueryString["parcel02"];
            parcel03.SelectedValue = Request.QueryString["parcel03"];
            parcel04.SelectedValue = Request.QueryString["parcel04"];
			order.SelectedValue = Request.QueryString["order"];
			orderby.SelectedValue = Request.QueryString["orderby"];
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            lokasi.SelectedValue = Request.QueryString["lokasi"];
        }
        else
        {
            lokasi.Items.Clear();
            lokasi.Items.Add(new ListItem("All", "-1"));

        }
    }

	//Display Table
    protected void fillTable(object sender, EventArgs e)    
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int IDLokasi = Convert.ToInt32(lokasi.SelectedValue);
        int shape = Convert.ToInt32(parcel01.SelectedValue);
        int size = Convert.ToInt32(parcel02.SelectedValue);
        int colour = Convert.ToInt32(parcel03.SelectedValue);
        int clarity = Convert.ToInt32(parcel04.SelectedValue);                
        
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

                        param.Value = kw.Text;                            
                        param2.Value = Tipe;                            
                        param3.Value = IDLokasi;
                        param4.Value = shape;
                        param5.Value = size;
                        param6.Value = colour;
                        param7.Value = clarity;
                        param8.Value = order.SelectedValue;
                        param9.Value = orderby.SelectedValue ;
                        param10.Value = "regular";

                        DataTable dt = new DataTable();
    
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            conn.Open();
                            adapter.Fill(dt);
                            conn.Close();
                        }                                                
       
        

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

    // protected void fillTable(object sender, EventArgs e)
    // {
    //     int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
    //     int IDLokasi = Convert.ToInt32(lokasi.SelectedValue);
    //     //ORDER BY
    //     string o = order.SelectedValue + " " + orderby.SelectedValue;

    //     //SELECT
    //     /*var rs = from p in db.Stone1A.Where(w).OrderBy(o)
    //              where
    //                  (
    //                  p.Nama.Contains(kw.Text) ||
    //                  p.Keterangan.Contains(kw.Text) ||
    //                  p.KodeParcel.Contains(kw.Text)
    //                  )
    //              select p;*/

    //     //select* from StockProductDJ_Stone1A a
    //     //    join StockProductDJ b on a.IDForm = b.ID
    //     //    join StockActualDJ c on b.ID = c.IDProduct
    //     //    where c.IDLokasi = 45;

    //     var rs = (from p in db.StockProductDJ_Stone1A
    //               where !p.StockProductDJ.Draft.Value 
    //               select new
    //               {
    //                   ID = p.Stone1A.ID,
    //                   Nama = p.Stone1A.Nama,
    //                   KodeParcel = p.Stone1A.KodeParcel,
    //                   TotalButir = p.TotalButir,
    //                   TotalCarat = p.TotalCarat,
    //                   Nomor = p.StockProductDJ.Nomor,
    //                   Parcel01 = p.Stone1A.Parcel01,
    //                   Parcel02 = p.Stone1A.Parcel02,
    //                   Parcel03 = p.Stone1A.Parcel03,
    //                   Parcel04 = p.Stone1A.Parcel04,
    //                   Nama01 = p.Stone1A.Parcel101.Nama,
    //                   Nama02 = p.Stone1A.Parcel102.Nama,
    //                   Nama03 = p.Stone1A.Parcel103.Nama,
    //                   Nama04 = p.Stone1A.Parcel104.Nama,
    //                   Kodeparcel = p.Stone1A.KodeParcel,
    //                   StockProductDJ = p.StockProductDJ,
    //                   StockActualDJ = p.StockProductDJ.StockActualDJ,
    //                   Namalokasi = p.StockProductDJ.StockActualDJ.NamaLokasi       
    //               }).OrderBy(o).Where(z => z.Nama.Contains(kw.Text) ||
    //                  z.KodeParcel.Contains(kw.Text) || z.Nomor.Contains(kw.Text));

    //     if (Tipe >= 0)
    //     {
    //         rs = rs.Where(p => p.StockActualDJ.TipeLokasi == Tipe);

    //         if (IDLokasi >= 0)
    //         {
    //             rs = rs.Where(p => p.StockActualDJ.IDLokasi == IDLokasi);
    //         }
    //     }
       
    //         rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);
       

    //     if (Convert.ToInt32(parcel01.SelectedValue) > 0) rs = rs.Where(p => p.Parcel01 == Convert.ToInt32(parcel01.SelectedValue));
    //     if (Convert.ToInt32(parcel02.SelectedValue) > 0) rs = rs.Where(p => p.Parcel02 == Convert.ToInt32(parcel02.SelectedValue));
    //     if (Convert.ToInt32(parcel03.SelectedValue) > 0) rs = rs.Where(p => p.Parcel03 == Convert.ToInt32(parcel03.SelectedValue));
    //     if (Convert.ToInt32(parcel04.SelectedValue) > 0) rs = rs.Where(p => p.Parcel04 == Convert.ToInt32(parcel04.SelectedValue));

    //     var stone = rs.GroupBy(p => p.KodeParcel);

    //     //NO-RESULT
    //     if (rs.Count() == 0) App.TableNoResult(tb);

    //     else
    //     {
    //         decimal TotalButir = 0m;
    //         decimal TotalCarat = 0m;

    //         //RESULT
    //         int index = 1;
    //         foreach (var r in stone)
    //         {
    //             if (!Response.IsClientConnected) break;

    //             TableRow tr = new TableRow();
    //             TableCell td;

    //             td = new TableCell();
    //             td.Text = "<p>" + index++.ToString() + ".</p>";
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.First().Namalokasi;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.First().Nama;
    //             tr.Cells.Add(td);

             
    //             td = new TableCell();
    //             td.Text = r.First().Nama01;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.First().Nama02;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.First().Nama03;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.First().Nama04;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Convert.ToDecimal(r.Sum(p => p.TotalButir)).ToString("N0");
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = Convert.ToDecimal(r.Sum(p => p.TotalCarat)).ToString("N3");
    //             tr.Cells.Add(td);

    //             tb.Rows.Add(tr);

    //             TotalButir += Convert.ToDecimal(r.Sum(p => p.TotalButir));
    //             TotalCarat += Convert.ToDecimal(r.Sum(p => p.TotalCarat));
    //         }

    //         TableRow tr2 = new TableRow();
    //         TableCell td2;

    //         td2 = new TableCell();
    //         td2.Text = "<b>Total</b>";
    //         td2.ColumnSpan = 7;
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = TotalButir.ToString("N0");
    //         tr2.Cells.Add(td2);

    //         td2 = new TableCell();
    //         td2.Text = TotalCarat.ToString("N3");
    //         tr2.Cells.Add(td2);

    //         tb.Rows.Add(tr2);
    //     }
    // }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        lokasi.Items.Clear();
        if (Tipe == 0 || Tipe == 1 || Tipe == 2) { DLC.LocationList(lokasi, Tipe); }
        else { lokasi.Items.Add(new ListItem("All", "-1")); }
        lokasi.Focus();
    }
}