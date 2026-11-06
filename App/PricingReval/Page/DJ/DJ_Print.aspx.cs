using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;

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
    App price = new App();

    decimal Setting = 0m;

    int Reval_M = 0;
    int Reval_R = 0;
    int Reval_G = 0;
    int Reval_SK = 0;
    int Reval_SB = 0;
    int Reval_MF = 0;
    int Reval_MU = 0;
    int Reval_Rate = 0;

    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected int act { get { return App.QueryInt(this, "act"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
           
        }
        fillTable(sender, e);
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        

        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        string o = "1=1";
        if (ProductItem > 0)
            o += " AND RevalProductDJ_CharProduct.ProductItem=" + ProductItem;
        if (ProductLevel > 0)
            o += " AND RevalProductDJ_CharProduct.ProductLevel=" + ProductLevel;
        if (ProductCategory > 0)
            o += " AND RevalProductDJ_CharProduct.ProductCategory=" + ProductCategory;

        var rs = from p in db.RevalProductDJ.Where(o)
                 select p;

        if (Request.QueryString["orderby"] == "1")
            rs.OrderBy(p => p.LocWarehouse.Nama);
        else if (Request.QueryString["orderby"] == "2")
            rs.OrderBy(p => p.RevalProductDJ_CharProduct.CharProductLevel.Nama);
        else if (Request.QueryString["orderby"] == "3")
            rs.OrderBy(p => p.RevalProductDJ_CharProduct.CharProductCategory.Nama);
        else if (Request.QueryString["orderby"] == "4")
            rs.OrderBy(p => p.RevalProductDJ_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            int index = 1;

            foreach (var r in rs)
            {
                var x = db.StockProductDJ.SingleOrDefault(p => p.Nomor == "" + r.Nomor + "");

                decimal AllM1 = 0m;

                var y = x.StockProductDJ_CostingProduct;
                var z = x.StockProductDJ_Costing;

                AllM1 = (y.Total + z.TotalBiaya);

                decimal AllM2 = 0m;

                var y2 = r.RevalProductDJ_CostingProduct;
                var z2 = r.RevalProductDJ_Costing;

                AllM2 = (y2.Total + z2.TotalBiaya);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                string KodeKaret;
                KodeKaret = (r.ProtoNomor != null) ? r.ProtoNomor : "";



                td = new TableCell();
                td.Text = "<small>" + KodeKaret + "</small>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.RevalProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(x.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0") + " / " + Convert.ToDecimal(r.RevalProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Mutasi(Convert.ToDecimal(x.StockProductDJ_PricingMU.HargaRupiahRound), Convert.ToDecimal(r.RevalProductDJ_PricingMU.HargaRupiahRound));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(AllM2).ToString("N2") + " / " + Convert.ToDecimal(AllM1).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(x.StockProductDJ_PricingMU.ModalUSD).ToString("N2") + " / " + Convert.ToDecimal(r.RevalProductDJ_PricingMU.ModalUSD).ToString("N2");
                tr.Cells.Add(td);

                

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalProductDJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalProductDJ_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                var det = (from p in db.RevalProductDJ_Stone1A
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalProductDJ_Stone1B
                           where p.IDForm == r.ID && p.StatusTerpasang == 1
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalProductDJ_Stone2
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalProductDJ_Stone3
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalProductDJ_Stone4
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalProductDJ_Stone5
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat });

                decimal TotalButir = 0m;
                try
                {
                    TotalButir = det.Sum(p => p.TotalButir);
                }
                catch (Exception ex) { }
                decimal TotalCarat = 0m;
                try
                {
                    TotalCarat = det.Sum(p => p.TotalCarat);
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = TotalButir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TotalCarat.ToString("N3");
                tr.Cells.Add(td);

                string Segmen = "None";
                try
                {
                    Segmen = r.RevalProductDJ_PricingSegmen.CharProductSegmen.Nama;
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = Segmen;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.RevalProductDJ_CharProduct.CharStoneDist.Nama;
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