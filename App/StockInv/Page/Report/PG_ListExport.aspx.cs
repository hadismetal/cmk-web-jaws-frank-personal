using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_Report_PG_ListExport : System.Web.UI.Page
{
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {

        }
        Response.ContentType = "application/vnd.ms-excel";

        Response.AppendHeader("content-disposition", "attachment; filename=PG_List_ExportExcel.xls");
        fillTable(sender, e);
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        decimal TotalHarga = 0m;
        decimal TotalBerat = 0m;
        decimal TotalQty = 0m;
        decimal TotalWeight = 0m;
        decimal TotalOngkos = 0m;
        decimal TotalTotal = 0m;
        decimal TtlHargaJual = 0m;

        string query = "EXEC sp_report_stock_inventory_pg_new '" + Request.QueryString["kw"] + "'," +
            "'" + Request.QueryString["tipe"] + "'," +
            "'" + Request.QueryString["cabang"] + "'," +
            "'" + Request.QueryString["productitem"] + "'," +
            "'" + Request.QueryString["productlevel"] + "'," +
            "'" + Request.QueryString["model"] + "'," +
            "'" + Request.QueryString["framecolor"] + "'," +
            "'" + Request.QueryString["substorage"] + "'," +
            "'" + Request.QueryString["consignment"] + "'," +
            "'1'," +
            "'1', 1";

        DataSet ds = QuerySPDataSet(query, Db.Cnn_DB);

        DataTable dt = ds.Tables[0];
        int countdata = Convert.ToInt32(ds.Tables[1].Rows[0][0]);

        int rowcount = dt.Rows.Count;

        if (rowcount > 0)
        {
            foreach (DataRow rslt in dt.Rows)
            {
                if (!Response.IsClientConnected) break;
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + rslt["Nomor"].ToString() + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<span class=\"text-sm\">" + Str.FullDate(Convert.ToDateTime(rslt["Tgl"])) + "</span>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt["Substorage"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt["Consignment"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt["Lokasi"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt["ProductItem"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt["GoldLevel"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt["Model"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt["KadarLogam"]).ToString("N0") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rslt["FrameColor"].ToString();
                tr.Cells.Add(td);

                //ring size
                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt["DimensiR"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToInt32(rslt["Qty"]).ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt["TotalWeight"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt["Ongkos"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt["Harga"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt["TotalBerat"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt["TotalHarga"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rslt["HargaJual"]).ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalQty += 1;
                TotalWeight += Convert.ToDecimal(rslt["TotalWeight"]);
                TotalOngkos += Convert.ToDecimal(rslt["Ongkos"]);
                TotalHarga += Convert.ToDecimal(rslt["Harga"]);
                TotalBerat += Convert.ToDecimal(rslt["TotalBerat"]);
                TotalTotal += Convert.ToDecimal(rslt["TotalHarga"]);
                TtlHargaJual += Convert.ToDecimal(rslt["HargaJual"]);
            }

            if (rowcount > 0)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<b>Total</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalQty.ToString() + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalWeight.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalOngkos.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalHarga.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalBerat.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TotalTotal.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<b>" + TtlHargaJual.ToString("N2") + "</b>";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }
	
	protected DataSet QuerySPDataSet(string query, string connectionString)
	{
		DataSet ds = new DataSet();
		using (SqlConnection conn = new SqlConnection(connectionString))
		{
			conn.Open();
			SqlTransaction trans;
			trans = conn.BeginTransaction();
			try
			{
				SqlDataAdapter sda = new SqlDataAdapter(query, connectionString);
				sda.SelectCommand.CommandTimeout = 3000;
				sda.Fill(ds);
				sda.Dispose();
				trans.Commit();
				conn.Close();
			}
			catch(Exception ex)
			{
				string ErrorMessage = ex.Message.ToString();
				trans.Rollback();
				conn.Close();
			}
		}
		return ds;
	}
}