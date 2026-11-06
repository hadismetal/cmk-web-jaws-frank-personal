using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ_List_V2_Export : System.Web.UI.Page
{
    App price = new App();

    //protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    //protected int act { get { return App.QueryInt(this, "act"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=DJ_Reval_Export.xls");
        }
        fillTable(sender, e);
    }

    protected void fillTable(object sender, EventArgs e)
    {

        /*var rs = from p in db.RevalProductDJ

                 select p;

        rs = rs.OrderBy(p => p.RevalProductDJ_CharProduct.ProductItem);

        if (kw.Text != "")
            rs = rs.Where(p => p.Nomor == "" + kw.Text + "");
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.RevalProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.RevalProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.RevalProductDJ_CharProduct.ProductCategory == ProductCategory);*/

        DataTable dt = new DataTable();
		
		string tipelokasi = Request.QueryString["tipelokasi"];
		string lokasi = Request.QueryString["lokasi"];
		string productitem = Request.QueryString["productitem"];
		string productcategory = Request.QueryString["productcategory"];
		string productlevel = Request.QueryString["productlevel"];
		string parcel104a = Request.QueryString["parcel104a"];
		string ordby = Request.QueryString["order"];
		string kw = Request.QueryString["kw"];

        try
        {
            string spname = "sp_getExecuteReval ";
            string query = " exec " + spname
                         + " " + tipelokasi + ", "
                         + lokasi + ", "
                         + productitem + ", "
                         + productcategory + ", "
                         + productlevel + ", "
                         + parcel104a + ", "
                         + ordby + ", '"
                         + kw + "'";

            dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
        }

        if (dt.Rows.Count != 0)
        {
            int index = 1;

            foreach (DataRow r in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["Nomor"].ToString();
                tr.Cells.Add(td);

                string KodeKaret;
                KodeKaret = r["ProtoNomor"].ToString();
                td = new TableCell();
                td.Text = KodeKaret;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["ProductItem"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["HargaRupiahRound"]).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["HargaRupiahRound_Reval"]).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Mutasi(Convert.ToDecimal(r["HargaRupiahRound"]), Convert.ToDecimal(r["HargaRupiahRound_Reval"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["M_Awal"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["M"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["M_Reval"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["R"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["R_Reval"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["NettoWeight"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["GrossWeight"]).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["TotalButir"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r["TotalCarat"]).ToString("N3");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["Segment"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["StoneDist"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r["ProductLevel"].ToString();
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;

            }


        }


    }

    protected string TD_Mutasi(decimal OldPrice, decimal NewPrice)
    {
        string x = string.Empty;
        decimal mutasi = 0m;

        mutasi = ((NewPrice - OldPrice) / OldPrice) * 100;

        string sgn = mutasi > 0 ? "+" : "";
        string css = mutasi > 10 || mutasi < -10 ? "text-red" : "";

        x += "<p class=\"" + css + " text-lg\">";
        x += sgn;
        x += Str.Number2(mutasi);
        x += "%";
        x += "</p>";

        return x;
    }
}