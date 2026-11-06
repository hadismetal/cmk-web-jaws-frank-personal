using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_Mutasi_Export_Sales : System.Web.UI.Page
{
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
    #region Parameter
    protected int locationType { get { return App.QueryInt(this, "locationType"); } }

    protected int location { get { return App.QueryInt(this, "location"); } }

    protected int productItem { get { return App.QueryInt(this, "productItem"); } }

    protected int productCategory { get { return App.QueryInt(this, "productCategory"); } }

    protected int productLevel { get { return App.QueryInt(this, "productLevel"); } }

    protected int stoneDist { get { return App.QueryInt(this, "stoneDist"); } }

    protected int groupBy { get { return App.QueryInt(this, "groupBy"); } }

    protected int brand { get { return App.QueryInt(this, "brand"); } }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Mutasi_Export_Lebur.xls");
            getData();
            getFilter();
        }
    }

    private void getData()
    {
        string toDate = Request.QueryString["to"] + " 23:59:59";

        string spname = "sp_report_lebur";

        string query = "exec " + spname
                     + "'FC', "
                     + "'" + Request.QueryString["from"] + "', "
                     + "'" + toDate + "'";

        DataSet ds = new DataSet();
        ds = customExec(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_CS"].ConnectionString);

        DataTable dt = ds.Tables["data"];

        if (dt.Rows.Count > 0)
        {
            TableRow tr;
            TableCell tc;

            foreach (DataRow dr in dt.Rows)
            {
                tr = new TableRow();

                tc = new TableCell();
                tc.Text = dr["vf_brand"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_store"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_nama"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_keterangan"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_group"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_nomor"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_tgl_receive"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_supplier_receive"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_tanggal"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_productid"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_item"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_netweight"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_grossweight"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_butir"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_carat"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_hargajual"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_MGold"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_MStone"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_MOngkos"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_totalM"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_rate"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_goldrate"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
				tc.Text = dr["vf_goldrateactual"].ToString();
				tc.BorderWidth = 1;
				tc.BorderColor = Color.Gray;
				tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_kadartukaran"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_hargamrupiah"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_kadarlogam"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["vf_framematerial"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tb.Rows.Add(tr);
            }


        }
    }

    private void getFilter()
    {
        TableRow tr;
        TableCell tc;

        string tipeLokasi = string.Empty;
        string lokasi = string.Empty;

        switch (locationType)
        {
            case 0:
                tipeLokasi = "Warehouse";
                lokasi = location == 0 ? "ALL" : db.LocWarehouse.SingleOrDefault(x => x.ID == location).Nama;
                break;
            case 1:
                tipeLokasi = "Outlet";
                lokasi = location == 0 ? "ALL" : db.LocOutlet.SingleOrDefault(x => x.ID == location).Nama;
                break;
            case 2:
                tipeLokasi = "Exhibition";
                lokasi = location == 0 ? "ALL" : db.LocExhibition.SingleOrDefault(x => x.ID == location).Nama;
                break;
            default:
                tipeLokasi = "ALL";
                lokasi = "ALL";
                break;
        }

        #region Keterangan Filter Lokasi
        tr = new TableRow();

        tc = new TableCell();
        tc.Text = "Location Type :";
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = tipeLokasi;
        tr.Cells.Add(tc);

        tc = new TableCell();
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = "Cabang :";
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = lokasi;
        tr.Cells.Add(tc);

        Table1.Rows.Add(tr);
        #endregion

        #region Keterangan Filter Characteristic
        tr = new TableRow();

        tc = new TableCell();
        tc.Text = "Product Item :";
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = productItem == 0 ? "ALL" : db.CharProductItem.Single(x => x.ID == productItem).Nama;
        tr.Cells.Add(tc);

        tc = new TableCell();
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = "Product Category :";
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = productCategory == 0 ? "ALL" : db.CharProductCategory.Single(x => x.ID == productCategory).Nama;
        tr.Cells.Add(tc);

        tc = new TableCell();
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = "Product Level :";
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = productLevel == 0 ? "ALL" : db.CharProductLevel.Single(x => x.ID == productLevel).Nama;
        tr.Cells.Add(tc);

        tc = new TableCell();
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = "Stone Dist :";
        tr.Cells.Add(tc);

        tc = new TableCell();
        tc.Text = stoneDist == 0 ? "ALL" : db.CharStoneDist.Single(x => x.ID == stoneDist).Nama;
        tr.Cells.Add(tc);

        Table1.Rows.Add(tr);
        #endregion

        tr = new TableRow();
        Table1.Rows.Add(tr);
    }
    private DataSet customExec(string strSql, string CnnString)
    {
        SqlConnection sqlCnn = new SqlConnection(CnnString);
        SqlDataAdapter sqlAdapter = new SqlDataAdapter(strSql, sqlCnn);
        sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
        DataSet objDS = new DataSet();
        sqlAdapter.Fill(objDS, "data");
        sqlCnn.Close();

        return objDS;
    }

}