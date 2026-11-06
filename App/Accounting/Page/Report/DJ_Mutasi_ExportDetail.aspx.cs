using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_DJ_Mutasi_Export : System.Web.UI.Page
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
	
	protected int product { get { return App.QueryInt(this, "product"); } }

    protected int brand { get { return App.QueryInt(this, "brand"); } }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Mutasi_DJ_Export.xls");
            getData();
            //getFilter();
        }
    }

    private void getData()
    {
        string spname = "sp_mutasi_product";
        string query = " exec " + spname
                     + "'" + Request.QueryString["from"] + "', "
                     + "'" + Request.QueryString["to"] + "', "
                     + locationType + ", "
                     + location + ", "
                     + productItem + ", "
                     + productCategory + ", "
                     + productLevel + ", "
                     + stoneDist + ", "
                     + "'" + Request.QueryString["plu"] + "', "
                     + groupBy + ", "
					 + product + ", "
                     + brand + ", 2";

        var dataFrame = (from p in db.CharFrameMaterial select p).ToDictionary(p => p.ID, p => p.Nama);

        DataSet ds = new DataSet();
        ds = customExec(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

        DataTable dt = ds.Tables["data"];

        if (dt.Rows.Count > 0)
        {
            TableRow tr;
            TableCell tc;
            int Qty = 0;
            decimal totalNetto24K = 0;
            decimal totalGross24K = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if(dr["Groups"].ToString() == "")
                {
                    tr = new TableRow();

                    tc = new TableCell();
                    tc.Text = dr["Lokasi"].ToString();
                    tc.BorderWidth = 1;
                    tc.ColumnSpan = 11;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Qty.ToString();
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["GrossWeight"].ToString();
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["NettoWeight"].ToString();
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    // decimal kadarLogam1 = Convert.ToDecimal(dr["KadarLogam"]);
                    // decimal nettoWeight1 = Convert.ToDecimal(dr["NettoWeight"]);
                    // decimal grossWeight1 = Convert.ToDecimal(dr["GrossWeight"]);
                    decimal kadarLogam1 = 0;
                    if (dr["KadarLogam"] != null && dr["KadarLogam"].ToString() != "")
                    {
                        kadarLogam1 = Convert.ToDecimal(dr["KadarLogam"]);
                    }

                    decimal nettoWeight1 = 0;
                    if (dr["NettoWeight"] != null && dr["NettoWeight"].ToString() != "")
                    {
                        nettoWeight1 = Convert.ToDecimal(dr["NettoWeight"]);
                    }

                    decimal grossWeight1 = 0;
                    if (dr["GrossWeight"] != null && dr["GrossWeight"].ToString() != "")
                    {
                        grossWeight1 = Convert.ToDecimal(dr["GrossWeight"]);
                    }

                    decimal calculatedNetto1 = (kadarLogam1 / 99.99m) * nettoWeight1;
                    decimal calculatedGross1 = (kadarLogam1 / 99.99m) * grossWeight1;

                    tc = new TableCell();
                    tc.Text = calculatedGross1.ToString("N2");
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = calculatedNetto1.ToString("N2");
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);
                    
                    tc = new TableCell();
                    tc.Text = "";
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["TotalButir"].ToString();
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["TotalCarat"].ToString();
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["Butir"].ToString();
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["Carat"].ToString();
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["Emas"].ToString();
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["Stone"].ToString();
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["Ongkos"].ToString();
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = dr["HargaM"].ToString();
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = Convert.ToDecimal(dr["HargaRupiah"]).ToString("N2");
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);

                    tc = new TableCell();
                    tc.Text = "";
                    tc.BorderWidth = 1;
                    tc.BorderColor = Color.Gray;
                    tc.Style.Add("font-weight", "bold");
                    tc.BackColor = Color.Aquamarine;
                    tr.Cells.Add(tc);
                    
                    tb.Rows.Add(tr);

                    tr = new TableRow();

                    tc = new TableCell();
                    tc.Text = "";
                    tr.Cells.Add(tc);

                    tb.Rows.Add(tr);
                    
                    Qty = 0;
                    continue;
                }

                tr = new TableRow();

                tc = new TableCell();
                tc.Text = dr["Brand"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Groups"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["TipeLokasi"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Lokasi"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Nomor"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);
				
				tc = new TableCell();
                tc.Text = dr["KodeKaret"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["ProductItem"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["ProductCategory"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                // int frameMaterialId = Convert.ToInt32(dr["FrameMaterial"]);
                int frameMaterialId = 0;
                if (dr["FrameMaterial"] != DBNull.Value && !string.IsNullOrEmpty(dr["FrameMaterial"].ToString()))
                {
                    frameMaterialId = Convert.ToInt32(dr["FrameMaterial"]);
                }

                tc.Text = dataFrame.ContainsKey(frameMaterialId) ? dataFrame[frameMaterialId] : "-";
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["ProductLevel"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["StoneDist"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = "1";
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["GrossWeight"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["NettoWeight"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                // tc = new TableCell();
                // tc.Text = dr["GrossWeight"].ToString();
                // tc.BorderWidth = 1;
                // tc.BorderColor = Color.Gray;
                // tr.Cells.Add(tc);
                // decimal kadarLogam = string.IsNullOrEmpty(dr["KadarLogam"]?.ToString()) ? 0 : Convert.ToDecimal(dr["KadarLogam"]);
                // decimal nettoWeight = string.IsNullOrEmpty(dr["NettoWeight"]?.ToString()) ? 0 : Convert.ToDecimal(dr["NettoWeight"]);
                // decimal grossWeight = string.IsNullOrEmpty(dr["GrossWeight"]?.ToString()) ? 0 : Convert.ToDecimal(dr["GrossWeight"]);
                
                decimal kadarLogam = 0;
                if (dr["KadarLogam"] != null && dr["KadarLogam"].ToString() != "")
                {
                    kadarLogam = Convert.ToDecimal(dr["KadarLogam"]);
                }

                decimal nettoWeight = 0;
                if (dr["NettoWeight"] != null && dr["NettoWeight"].ToString() != "")
                {
                    nettoWeight = Convert.ToDecimal(dr["NettoWeight"]);
                }

                decimal grossWeight = 0;
                if (dr["GrossWeight"] != null && dr["GrossWeight"].ToString() != "")
                {
                    grossWeight = Convert.ToDecimal(dr["GrossWeight"]);
                }

                decimal KadarTukaranJual = 0;
                if (dr["KadarTukaranJual"] != null && dr["KadarTukaranJual"].ToString() != "")
                {
                    KadarTukaranJual = Convert.ToDecimal(dr["KadarTukaranJual"]);
                }

                decimal KadarTukaranBeli = 0;
                if (dr["KadarTukaranBeli"] != null && dr["KadarTukaranBeli"].ToString() != "")
                {
                    KadarTukaranBeli = Convert.ToDecimal(dr["KadarTukaranBeli"]);
                }


                decimal calculatedNetto = (kadarLogam / 99.99m) * nettoWeight;
                decimal calculatedGross = (kadarLogam / 99.99m) * grossWeight;

                tc = new TableCell();
                tc.Text = calculatedGross.ToString("N2");
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = calculatedNetto.ToString("N2");
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = kadarLogam.ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = KadarTukaranJual.ToString("N2");
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = KadarTukaranBeli.ToString("N2");
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["TotalButir"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["TotalCarat"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Parcel"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Butir"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Carat"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Emas"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Stone"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Ongkos"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["HargaM"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Convert.ToDecimal(dr["HargaRupiah"]).ToString("N2");
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = dr["Supplier"].ToString();
                tc.BorderWidth = 1;
                tc.BorderColor = Color.Gray;
                tr.Cells.Add(tc);

                tb.Rows.Add(tr);
                totalNetto24K += calculatedNetto;
                totalGross24K += calculatedGross;
                Qty += 1;
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
                lokasi = location == 0 ? "ALL" : db.LocWarehouse.SingleOrDefault(x => x.ID == location).Nama;
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