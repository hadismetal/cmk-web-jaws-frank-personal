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

            Response.AppendHeader("content-disposition", "attachment; filename=Stone2Rekap_Export.xls");
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
        int parcel5 = Convert.ToInt32(Request.QueryString["parcel05"]);
        int parcel6 = Convert.ToInt32(Request.QueryString["parcel06"]);
        int parcel7 = Convert.ToInt32(Request.QueryString["parcel07"]);
         string o = (Request.QueryString["order"] == "Nomor" ? "AX." + Request.QueryString["order"] : "AX." + Request.QueryString["order"]) + " " + Request.QueryString["orderby"];
        // string o = (Request.QueryString["order"] == "Nomor" ? "b." + Request.QueryString["order"] : "c." + Request.QueryString["order"]) + " " + Request.QueryString["orderby"];


        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string query = @"SELECT ID,NamaLokasi,Nama,KodeParcel,SUM(TotalButir) TotalButir,SUM(TotalCarat)TotalCarat
                            ,nmShape,nmColor,nmSize,colorGrade,nmClarity,nmMaterial,nmCategory
                         FROM (
                            select c.ID,d.NamaLokasi,c.Nama,c.KodeParcel,SUM(a.TotalButir) TotalButir,SUM(a.TotalCarat)TotalCarat
                            ,ca.nama nmShape,cb.nama nmColor,cc.nama nmSize,cd.nama colorGrade,ce.nama nmClarity,cf.nama nmMaterial,cg.nama nmCategory                            
                            from StockProductDJ_Stone2 a
                            JOIN StockProductDJ b on b.id=a.IDForm
                            JOIN Stone2 c on c.ID=a.IDStone
                            JOIN StockActualDJ d on d.IDProduct=b.ID
                            JOIN Parcel201 ca on ca.id=c.Parcel01
                            JOIN Parcel202 cb on cb.id=c.Parcel02
                            JOIN Parcel203 cc on cc.id=c.Parcel03
                            JOIN Parcel204 cd on cd.id=c.Parcel04
                            JOIN Parcel205 ce on ce.id=c.Parcel05
                            JOIN Parcel206 cf on cf.id=c.Parcel06
                            JOIN Parcel207 cg on cg.id=c.Parcel07
                            where b.Draft=0
                            and b.StatusLebur=0 and b.StatusPenjualan=0 and b.ReturnProduct=0 and d.IDLokasi <> -1
                            and (@IDLokasi <= 0 or d.IDLokasi = @IDLokasi) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
                            and (@parcel1 = 0 or ca.id=@parcel1) and (@parcel2 = 0 or cb.id=@parcel2) and (@parcel3 = 0 or cc.id=@parcel3)
                            and (@parcel4 = 0 or cd.id=@parcel4) and (@parcel5 = 0 or ce.id=@parcel5) and (@parcel6 = 0 or cf.id=@parcel6)
                            and (@parcel7 = 0 or cg.id=@parcel7) and (c.Nama like '%'+@kw+'%' or c.KodeParcel like '%'+@kw+'%')
                            group by c.id,c.nama,c.kodeparcel,ca.nama,cb.nama,cc.nama,cd.nama,ce.nama,cf.nama,cg.nama,d.NamaLokasi
                            )AX
                            group by AX.id,AX.nama,AX.kodeparcel,nmShape,nmColor,nmSize,colorGrade,nmClarity,nmMaterial,nmCategory,NamaLokasi
                            order by " + o ;
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
                command.Parameters.Add(new SqlParameter("@parcel5", SqlDbType.Int) { Value = parcel5 });
                command.Parameters.Add(new SqlParameter("@parcel6", SqlDbType.Int) { Value = parcel6 });
                command.Parameters.Add(new SqlParameter("@parcel7", SqlDbType.Int) { Value = parcel7 });


                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                int no = 0;
                decimal TotalButir = 0m;
                decimal TotalCarat = 0m;
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count == 0)
                    {
                        TableRow tr = new TableRow();
                        TableCell td;
                        td = new TableCell();
                        td.Text = "No data available";
                        td.ColumnSpan = 12;
                        td.Style.Add("text-align", "center");
                        tr.Cells.Add(td);
                        tb.Rows.Add(tr);
                        return;
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        no++;                        

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = no.ToString();
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
                        td.Text = row["nmSize"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        // Ensure the column names match those in the DataTable
                        td.Text = row["colorGrade"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmClarity"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmMaterial"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmCategory"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(row["TotalButir"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(row["TotalCarat"]).ToString("N3");
                        tr.Cells.Add(td);

                        TotalButir += Convert.ToDecimal(row["TotalButir"]);
                        TotalCarat += Convert.ToDecimal(row["TotalCarat"]);




                        tb.Rows.Add(tr);
                    }

                    TableRow tr2 = new TableRow();
                        TableCell td2;
            
                        td2 = new TableCell();
                        td2.Text = "<b>Total</b>";
                        td2.ColumnSpan = 10;
                        tr2.Cells.Add(td2);
            
                        td2 = new TableCell();
                        td2.Text = TotalButir.ToString("N0");
                        tr2.Cells.Add(td2);
            
                        td2 = new TableCell();
                        td2.Text = TotalCarat.ToString("N3");
                        tr2.Cells.Add(td2);
            
                        tb.Rows.Add(tr2);

                }
                else
                {
                    TableRow tr = new TableRow();
                    TableCell td;
                    td = new TableCell();
                    td.Text = "No data available";
                    td.ColumnSpan = 12;
                    td.HorizontalAlign = HorizontalAlign.Center;
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
}