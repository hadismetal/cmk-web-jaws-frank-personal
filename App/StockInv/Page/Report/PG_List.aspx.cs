using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_Summary : System.Web.UI.Page
{
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    //Map_DB db = new Map_DB();
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            DLC.CharProductItem(productitem);
            DLC.CharGoldLevel(productlevel);
            DLC.CharFrameColor(framecolor);
            DLC.CompanyBrand(brand);
            DLC.CharGoldModelAll2(model);

            DLC.LocationList(cabang, tipe);
			Ddl.Substorage(substorage);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        decimal TotalHarga = 0m;
        decimal TotalBerat = 0m;
        decimal TotalQty = 0m;
        decimal TotalWeight = 0m;
        decimal TotalOngkos = 0m;
        decimal TotalTotal = 0m;
        decimal TtlHargaJual = 0m;

		int pageNumber = 1;
		int numberOfObjectsPerPage = 30;

		if (Request.QueryString["page"] != "")
			pageNumber = Convert.ToInt32(Request.QueryString["page"]);
		if (Request.QueryString["page"] == "0" || Request.QueryString["page"] == null || Request.QueryString["page"] == "")
			pageNumber = 1;
					
		string query = "EXEC sp_report_stock_inventory_pg_new '" + kw.Text + "','" + tipe + "','" + cabang.SelectedValue + "','" + productitem.SelectedValue + "','" + productlevel.SelectedValue + "','" + model.SelectedValue + "','" + framecolor.SelectedValue + "','" + substorage.SelectedValue + "','" + consignment.SelectedValue + "', '" + pageNumber.ToString() + "', '" + numberOfObjectsPerPage.ToString() + "', 0";
		
		DataSet ds = QuerySPDataSet(query, Db.Cnn_DB);
		
		DataTable dt = ds.Tables[0];
		int countdata = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
		
		int TotalPage = (countdata / numberOfObjectsPerPage) + 1;
		
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
                td.Text = Convert.ToDecimal(rslt["KadarLogam"]).ToString("N2") + "%";
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
            string href = App.RefreshURL(this);

            //Customize-Qstr
            href += "&kw=" + kw.Text;
            href += "&brand=" + brand.Text;
            href += "&productitem=" + productitem.Text;
            href += "&productlevel=" + productlevel.Text;
            href += "&framecolor=" + framecolor.Text;
            href += "&model=" + model.Text;
            href += "&tipe=" + tipe;
            href += "&cabang=" + cabang.Text;
            href += "&substorage=" + substorage.SelectedValue;
            href += "&consignment=" + consignment.SelectedValue;
            href += "&page=" + (pageNumber + 1).ToString();

            string href2 = App.RefreshURL(this);

            //Customize-Qstr
            href2 += "&kw=" + kw.Text;
            href2 += "&brand=" + brand.Text;
            href2 += "&productitem=" + productitem.Text;
            href2 += "&productlevel=" + productlevel.Text;
            href2 += "&framecolor=" + framecolor.Text;
            href2 += "&model=" + model.Text;
            href2 += "&tipe=" + tipe;
            href2 += "&cabang=" + cabang.Text;
            href2 += "&substorage=" + substorage.SelectedValue;
            href2 += "&consignment=" + consignment.SelectedValue;
            href2 += "&page=" + (pageNumber - 1).ToString();

            string href3 = App.RefreshURL(this);

            //Customize-Qstr
            href3 += "&kw=" + kw.Text;
            href3 += "&brand=" + brand.Text;
            href3 += "&productitem=" + productitem.Text;
            href3 += "&productlevel=" + productlevel.Text;
            href3 += "&framecolor=" + framecolor.Text;
            href3 += "&model=" + model.Text;
            href3 += "&tipe=" + tipe;
            href3 += "&cabang=" + cabang.Text;
            href3 += "&substorage=" + substorage.SelectedValue;
            href3 += "&consignment=" + consignment.SelectedValue;
            href3 += "&page=1";

            string href4 = App.RefreshURL(this);

            //Customize-Qstr
            href4 += "&kw=" + kw.Text;
            href4 += "&brand=" + brand.Text;
            href4 += "&productitem=" + productitem.Text;
            href4 += "&productlevel=" + productlevel.Text;
            href4 += "&framecolor=" + framecolor.Text;
            href4 += "&model=" + model.Text;
            href4 += "&tipe=" + tipe;
            href4 += "&cabang=" + cabang.Text;
            href4 += "&substorage=" + substorage.SelectedValue;
            href4 += "&consignment=" + consignment.SelectedValue;
            href4 += "&page=" + (TotalPage);

            string paging = "";

            paging += "<div style=\"clear: both; width: 100%; text-align: center;\">";
			if(pageNumber > 1)
			{
				paging += "<a href=\"" + href3 + "\">First</a>&nbsp;&nbsp;";
				paging += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
			}
			if(pageNumber < (Convert.ToInt32(countdata / numberOfObjectsPerPage) + 1))
			{
				paging += "<a href=\"" + href + "\">Next</a>&nbsp;&nbsp;";
				paging += "<a href=\"" + href4 + "\">Last</a>";
			}
            paging += "<br /> Page " + (pageNumber) + " of " + TotalPage + " </div>";

            paging_text.Text = paging;
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            
            productitem.SelectedValue = Request.QueryString["productitem"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            framecolor.SelectedValue = Request.QueryString["framecolor"];
            brand.SelectedValue = Request.QueryString["brand"];
            model.SelectedValue = Request.QueryString["model"];
            cabang.SelectedValue = Request.QueryString["cabang"];
            substorage.SelectedValue = Request.QueryString["substorage"];
            consignment.SelectedValue = Request.QueryString["consignment"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&brand=" + brand.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;
        href += "&tipe=" + tipe;
        href += "&cabang=" + cabang.Text;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&consignment=" + consignment.SelectedValue;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "PG_ListPrint.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&brand=" + brand.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;
        href += "&tipe=" + tipe;
        href += "&cabang=" + cabang.Text;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&consignment=" + consignment.SelectedValue;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "PG_ListExport.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&brand=" + brand.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&model=" + model.Text;
        href += "&tipe=" + tipe;
        href += "&cabang=" + cabang.Text;
        href += "&substorage=" + substorage.SelectedValue;
        href += "&consignment=" + consignment.SelectedValue;

        Response.Redirect(href);
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