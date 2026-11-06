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
			DLC.Parcel501(parcel01);
			DLC.Parcel502(parcel02);
			DLC.Parcel503(parcel03);
			DLC.Parcel504(parcel04);
            DLC.Parcel505(parcel05);
            DLC.Parcel506(parcel06);
            DLC.Parcel507(parcel07);

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
		href += "&kw=" + kw.Text;
		href += "&parcel01=" + parcel01.SelectedValue;
		href += "&parcel02=" + parcel02.SelectedValue;
		href += "&parcel03=" + parcel03.SelectedValue;
		href += "&parcel04=" + parcel04.SelectedValue;
        href += "&parcel05=" + parcel05.SelectedValue;
        href += "&parcel06=" + parcel06.SelectedValue;
        href += "&parcel07=" + parcel07.SelectedValue;
		href += "&order=" + order.SelectedValue;
		href += "&orderby=" + orderby.SelectedValue;

		Response.Redirect(href);
	}

    protected void Export(object sender, EventArgs e)
    {
        string href = "Stone5Rekap_Export.aspx?id=1";

        //Customize-Qstr        		
		href += "&kw=" + kw.Text;
		href += "&parcel01=" + parcel01.SelectedValue;
		href += "&parcel02=" + parcel02.SelectedValue;
		href += "&parcel03=" + parcel03.SelectedValue;
		href += "&parcel04=" + parcel04.SelectedValue;
        href += "&parcel05=" + parcel05.SelectedValue;
        href += "&parcel06=" + parcel06.SelectedValue;
        href += "&parcel07=" + parcel07.SelectedValue;
		href += "&order=" + order.SelectedValue;
		href += "&orderby=" + orderby.SelectedValue;

		Response.Redirect(href);
    }

	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
			parcel01.SelectedValue = Request.QueryString["parcel01"];
			parcel02.SelectedValue = Request.QueryString["parcel02"];
			parcel03.SelectedValue = Request.QueryString["parcel03"];
			parcel04.SelectedValue = Request.QueryString["parcel04"];
            parcel05.SelectedValue = Request.QueryString["parcel05"];
            parcel06.SelectedValue = Request.QueryString["parcel06"];
            parcel07.SelectedValue = Request.QueryString["parcel07"];
			order.SelectedValue = Request.QueryString["order"];
			orderby.SelectedValue = Request.QueryString["orderby"];
		}
	}

	//Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //ORDER BY
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);
        string keyword = Request.QueryString["kw"].ToString();        
        string P1 = parcel01.SelectedValue == "Code :" ? "0" : parcel01.SelectedValue;
        string P2 = parcel02.SelectedValue == "Source :"? "0" : parcel02.SelectedValue;
        string P3 = parcel03.SelectedValue == "Colour :" ? "0" : parcel03.SelectedValue;
        string P4 = parcel04.SelectedValue == "Shape :"? "0" : parcel04.SelectedValue;
        string P5 = parcel05.SelectedValue == "Overtone :" ? "0" : parcel05.SelectedValue;
        string P6 = parcel06.SelectedValue == "Diameter :" ? "0" : parcel06.SelectedValue;
        string P7 = parcel07.SelectedValue == "Surface :" ? "0" : parcel07.SelectedValue;
        string o = order.SelectedValue + " " + orderby.SelectedValue;        
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
                command.Parameters.Add(new SqlParameter("@keyword", SqlDbType.VarChar) { Value = kw.Text.Trim(charsToTrim) });                
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

            //RESULT
            
        }  
    // protected void fillTable(object sender, EventArgs e)
    // {
    //     //ORDER BY
    //     string o = order.SelectedValue + " " + orderby.SelectedValue;

    //     //SELECT
    //     /*var rs = from p in db.Stone5.Where(w).OrderBy(o)
    //              where
    //                  (
    //                  p.Nama.Contains(kw.Text) ||
    //                  p.Keterangan.Contains(kw.Text) ||
    //                  p.KodeParcel.Contains(kw.Text)
    //                  )
    //              select p;*/

    //     var rs = (from p in db.StockProductDJ_Stone5
    //               where !p.StockProductDJ.Draft.Value
    //               && p.StockProductDJ.StockActualDJ.IDLokasi != -1
    //               select new
    //               {
    //                   ID = p.Stone5.ID,
    //                   Nama = p.Stone5.Nama,
    //                   KodeParcel = p.Stone5.KodeParcel,
    //                   TotalButir = p.TotalButir,
    //                   TotalCarat = p.TotalCarat,
    //                   Nomor = p.StockProductDJ.Nomor,
    //                   Parcel01 = p.Stone5.Parcel01,
    //                   Parcel02 = p.Stone5.Parcel02,
    //                   Parcel03 = p.Stone5.Parcel03,
    //                   Parcel04 = p.Stone5.Parcel04,
    //                   Parcel05 = p.Stone5.Parcel05,
    //                   Parcel06 = p.Stone5.Parcel06,
    //                   Parcel07 = p.Stone5.Parcel07,
    //                   Nama01 = p.Stone5.Parcel501.Nama,
    //                   Nama02 = p.Stone5.Parcel502.Nama,
    //                   Nama03 = p.Stone5.Parcel503.Nama,
    //                   Nama04 = p.Stone5.Parcel504.Nama,
    //                   Nama05 = p.Stone5.Parcel505.Nama,
    //                   Nama06 = p.Stone5.Parcel506.Nama,
    //                   Nama07 = p.Stone5.Parcel507.Nama,
    //                   StockProductDJ = p.StockProductDJ,
    //                   StockActualDJ = p.StockProductDJ.StockActualDJ
    //               }).OrderBy(o).Where(z => z.Nama.Contains(kw.Text) ||
    //                  z.KodeParcel.Contains(kw.Text) || z.Nomor.Contains(kw.Text));

    //     rs = rs.Where(p => p.StockProductDJ.StatusLebur == false && p.StockProductDJ.ReturnProduct == false && p.StockProductDJ.StatusPenjualan == 0);

    //     if (parcel01.SelectedIndex > 0) rs = rs.Where(p => p.Parcel01 == parcel01.SelectedIndex);
    //     if (parcel02.SelectedIndex > 0) rs = rs.Where(p => p.Parcel02 == parcel02.SelectedIndex);
    //     if (parcel03.SelectedIndex > 0) rs = rs.Where(p => p.Parcel03 == parcel03.SelectedIndex);
    //     if (parcel04.SelectedIndex > 0) rs = rs.Where(p => p.Parcel04 == parcel04.SelectedIndex);
    //     if (parcel05.SelectedIndex > 0) rs = rs.Where(p => p.Parcel05 == parcel05.SelectedIndex);
    //     if (parcel06.SelectedIndex > 0) rs = rs.Where(p => p.Parcel06 == parcel06.SelectedIndex);
    //     if (parcel07.SelectedIndex > 0) rs = rs.Where(p => p.Parcel07 == parcel07.SelectedIndex);

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
    //             td.Text = r.First().Nama;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.First().Nama01;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.First().Nama03;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.First().Nama04;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.First().Nama05;
    //             tr.Cells.Add(td);

    //             td = new TableCell();
    //             td.Text = r.First().Nama06;
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

}