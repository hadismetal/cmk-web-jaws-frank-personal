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

            Response.AppendHeader("content-disposition", "attachment; filename=Stone3Rekap_Export.xls");
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
        int parcel8 = Convert.ToInt32(Request.QueryString["parcel08"]);
        string o1 = Request.QueryString["order"];
        string o2 = Request.QueryString["orderby"];

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string query = @"
                            

                            select NamaLokasi,Nama as Parcel,KodeParcel, SUM(TotalButir) TotalButir, SUM(TotalCarat) TotalCarat,nmItem,nmShape,nmSize,nmColor,colorGrade,clarity,nmCategory,countryOForigin
                            from (
                            	select c.ID,d.NamaLokasi,c.Nama,c.KodeParcel,a.TotalButir,a.TotalCarat
                                ,ca.nama nmItem,cb.nama nmShape,cc.nama nmSize,cd.nama nmColor,ce.nama colorGrade,
                            	cf.nama clarity,cg.nama nmCategory,ch.Nama as countryOForigin
                                from StockProductDJ_Stone3 a
                                JOIN StockProductDJ b on b.id=a.IDForm
                                JOIN Stone3 c on c.ID=a.IDStone
                                JOIN StockActualDJ d on d.IDProduct=b.ID
                                JOIN Parcel301 ca on ca.id=c.Parcel01
                                JOIN Parcel302 cb on cb.id=c.Parcel02
                                JOIN Parcel303 cc on cc.id=c.Parcel03
                                JOIN Parcel304 cd on cd.id=c.Parcel04
                                JOIN Parcel305 ce on ce.id=c.Parcel05
                                JOIN Parcel306 cf on cf.id=c.Parcel06
                                JOIN Parcel307 cg on cg.id=c.Parcel07
                                JOIN Parcel308 ch on ch.id=c.Parcel08
                                where b.Draft=0
                                and b.StatusLebur=0 and b.StatusPenjualan=0 and b.ReturnProduct=0 and d.IDLokasi <> -1	
                                and (@IDLokasi < 0 or d.IDLokasi = @IDLokasi) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
                                and (@parcel1 = 0 or ca.id=@parcel1) and (@parcel2 = 0 or cb.id=@parcel2) and (@parcel3 = 0 or cc.id=@parcel3)
                                and (@parcel4 = 0 or cd.id=@parcel4) and (@parcel5 = 0 or ce.id=@parcel5) and (@parcel6 = 0 or cf.id=@parcel6)
                                and (@parcel7 = 0 or cg.id=@parcel7) and (@parcel8 = 0 or ch.id=@parcel8)
                                and (c.Nama like '%'+@kw+'%' or c.KodeParcel like '%'+@kw+'%')
                            )ax
                            GROUP BY  Nama ,KodeParcel,nmItem,nmShape,nmSize,nmColor,colorGrade,clarity,nmCategory,countryOForigin,NamaLokasi,ID
                            order by " + o1 + " " + o2;
                            
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
                command.Parameters.Add(new SqlParameter("@parcel8", SqlDbType.Int) { Value = parcel8 });                                              

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                int no = 0;                
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
                        td.Text = row["Parcel"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmItem"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmShape"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmSize"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmColor"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        // Ensure the column names match those in the DataTable
                        td.Text = row["colorGrade"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["clarity"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmCategory"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["countryOForigin"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(row["TotalButir"]).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(row["TotalCarat"]).ToString("N3");
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);                        
                    }

                }
                else
                {
                    TableRow tr = new TableRow();
                    TableCell td;
                    td = new TableCell();
                    td.Text = "No data available";
                    td.ColumnSpan = 9;
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