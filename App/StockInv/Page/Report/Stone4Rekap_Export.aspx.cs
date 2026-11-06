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

            Response.AppendHeader("content-disposition", "attachment; filename=Stone4Rekap_Export.xls");
			fillTable(sender, e);
		}
	}
    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int IDLokasi = Convert.ToInt32(Request.QueryString["lokasi"]);
        int Parcel1 = Convert.ToInt32(Request.QueryString["parcel01"]);
        int Parcel2 = Convert.ToInt32(Request.QueryString["parcel02"]);
        int Parcel3 = Convert.ToInt32(Request.QueryString["parcel03"]);
        string o1 = Request.QueryString["order"];
        string o2 = Request.QueryString["orderby"];

        using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
        {
            string query = @"
                            SELECT NamaLokasi,Nama,KodeParcel,sum(TotalButir) TotalButir,sum(TotalButir) TotalCarat
							,nmShape, nmColour, nmSize
							FROM (
								select c.ID,d.NamaLokasi,c.Nama,c.KodeParcel,a.TotalButir,a.TotalCarat
								,ca.nama nmColour,cb.nama nmShape,cc.nama nmSize
								from StockProductDJ_Stone4 a
								JOIN StockProductDJ b on b.id=a.IDForm
								JOIN Stone4 c on c.ID=a.IDStone
								JOIN StockActualDJ d on d.IDProduct=b.ID
								JOIN Parcel401 ca on ca.id=c.Parcel01
								JOIN Parcel402 cb on cb.id=c.Parcel02
								JOIN Parcel403 cc on cc.id=c.Parcel03
								where b.Draft=0 and d.IDLokasi <> -1
                                and b.StatusLebur=0 and b.StatusPenjualan=0 and b.ReturnProduct=0
                                and (@IDLokasi < 0 or d.IDLokasi = @IDLokasi) and (@Tipe < 0 or d.TipeLokasi=@Tipe)
                                and (@parcel1 = 0 or ca.id=@parcel1) and (@parcel2 = 0 or cb.id=@parcel2) and (@parcel3 = 0 or cc.id=@parcel3)
                                and (c.Nama like '%'+@kw+'%' or c.KodeParcel like '%'+@kw+'%')
							)AX
							GROUP BY Nama,KodeParcel ,nmColour,nmShape,nmSize, NamaLokasi,ID
                            order by " + o1 + " " + o2 + @"
                            ";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandTimeout = 10;
                command.Parameters.Add(new SqlParameter("@kw", SqlDbType.VarChar) { Value = Request.QueryString["kw"].Trim() });
                command.Parameters.Add(new SqlParameter("@Tipe", SqlDbType.Int) { Value = Tipe });
                command.Parameters.Add(new SqlParameter("@IDLokasi", SqlDbType.Int) { Value = IDLokasi });
                command.Parameters.Add(new SqlParameter("@parcel1", SqlDbType.Int) { Value = Parcel1 });
                command.Parameters.Add(new SqlParameter("@parcel2", SqlDbType.Int) { Value = Parcel2 });
                command.Parameters.Add(new SqlParameter("@parcel3", SqlDbType.Int) { Value = Parcel3 });
                command.Parameters.Add(new SqlParameter("@order", SqlDbType.VarChar) { Value = o1 });
                command.Parameters.Add(new SqlParameter("@orderby", SqlDbType.VarChar) { Value = o2 });

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
                        td.ColumnSpan = 7;
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
                        td.Text = row["nmColour"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmShape"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = row["nmSize"].ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        // Removed columns that are not in the SELECT statement

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
                    td.ColumnSpan = 7;
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