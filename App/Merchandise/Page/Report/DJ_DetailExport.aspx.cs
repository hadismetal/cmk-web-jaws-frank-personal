using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

public partial class Page_DJ : System.Web.UI.Page
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=DJ_Detail_ExportExcel.xls");
            //Generate
            fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

        string cabang = Request.QueryString["cabang"] == "-1" ? "0" : Request.QueryString["cabang"];
        string spname = "detailDJ_StockInventory ";
        string query = " exec " + spname
                     + Request.QueryString["substorage"] + ", "
                     + cabang + ", "
                     + Request.QueryString["tipe"] + ", "
                     + Request.QueryString["brand"] + ", "
                     + Request.QueryString["productitem"] + ", "
                     + Request.QueryString["productlevel"] + ", "
                     + Request.QueryString["productcategory"] + ", "
                     + "'" + Request.QueryString["kw"] + "' ";

        DataSet ds = customExec(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        DataTable dt = ds.Tables["data"];
        DataTable total = ds.Tables["data1"];


        if (dt.Rows.Count > 0)
        {
            TableRow tr;
            TableCell tc;
            int ID = 0;
            int index = 1;
            foreach (DataRow dr in dt.Rows)
            {
                tr = new TableRow();

                if (ID == Convert.ToInt32(dr["ID"]))
                {
                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);
                    
                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                }
                else
                {
                    tc = new TableCell();
                    tc.Text =index.ToString();
                    tr.Cells.Add(tc);
                    index++;

                    tc = new TableCell();
                    tc.Text = dr["Nomor"].ToString();
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["ApprovalTgl"].ToString();
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["Approval"].ToString();
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["Brand"].ToString();
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["NamaLokasi"].ToString();
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["ProtoNomor"].ToString();
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["SupplierNomor"].ToString();
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["ProductItem"].ToString();
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["ProductCategory"].ToString();
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["StoneDist"].ToString();
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["Segment"].ToString();
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToInt32(dr["InHand"]).ToString();
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["NettoWeight"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["GrossWeight"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["FrameM"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["FrameR"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["NOM"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["NOR"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["ROM"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["ROR"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["BiayaModal"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["Biaya"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["SettingM"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["SettingR"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["HargaMUSD"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["HargaUSD"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["HargaRupiahRound"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["PromoHarga"]).ToString("N2", nfi);
                    tr.Cells.Add(tc);
                }

                tc = new TableCell();
                tc.Text = dr["Stone"].ToString();
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToInt32(dr["TotalButir"]).ToString();
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["TotalCarat"]).ToString("N2", nfi);
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["TotalHargaM"]).ToString("N2", nfi);
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["TotalHargaR"]).ToString("N2", nfi);
                tr.Cells.Add(tc);

                tb.Rows.Add(tr);

                ID = Convert.ToInt32(dr["ID"]);
            }

            tr = new TableRow();

            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "Total";
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            DataRow tot = total.Rows[0];
            tc = new TableCell();
            tc.Text = Convert.ToInt32(tot["InHand"]).ToString();
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["NettoWeight"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["GrossWeight"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["FrameM"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["FrameR"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["NOM"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["NOR"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["ROM"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["ROR"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["BiayaModal"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["Biaya"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["SettingM"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["SettingR"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["HargaMUSD"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["HargaUSD"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["HargaRupiahRound"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["PromoHarga"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = "";
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToInt32(tot["TotalButir"]).ToString();
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["TotalCarat"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["TotalHargaM"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tc = new TableCell();
            tc.Text = Convert.ToDecimal(tot["TotalHargaR"]).ToString("N2", nfi);
            tc.Style.Add("font-weight", "bold");
            tr.Cells.Add(tc);

            tb.Rows.Add(tr);

        }


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