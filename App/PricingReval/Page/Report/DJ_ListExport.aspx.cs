using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_DJ_ListExport : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=PR_DJListExport.xls");
            fillTable(sender, e);
        }
    }
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var rs = from p in db.RevalLogProductDJ

                 select p;

        rs = rs.OrderByDescending(p => p.OperatorTgl);
        string plu = Request.QueryString["kw"].ToString();
        start = Convert.ToDateTime(Request.QueryString["dari"]);
        end = Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1);

            rs = rs.Where(p => p.OperatorTgl >= start && p.OperatorTgl < end);
     

        if (plu != "")
            rs = rs.Where(p => p.Nomor == "" + plu + "");

        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        int order = Convert.ToInt32(Request.QueryString["order"]);

        if (ProductLevel > 0) rs = rs.Where(p => p.RevalLogProductDJ_CharProduct.ProductLevel == ProductLevel);
   
        if (ProductItem > 0) rs = rs.Where(p => p.RevalLogProductDJ_CharProduct.ProductItem == ProductItem);
      
        if (ProductCategory > 0) rs = rs.Where(p => p.RevalLogProductDJ_CharProduct.ProductCategory == ProductCategory);

        else if (order == 1)
            rs.OrderBy(p => p.RevalLogProductDJ_CharProduct.CharProductLevel.Nama);
        else if (order == 2)
            rs.OrderBy(p => p.RevalLogProductDJ_CharProduct.CharProductCategory.Nama);
        else if (order == 3)
            rs.OrderBy(p => p.RevalLogProductDJ_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            int index = 1;

            foreach (var r in rs)
            {
                decimal AllM = 0m;
                decimal AllMOld = 0m;

                var x = r.RevalLogProductDJ_CostingProduct;
                var z = r.RevalLogProductDJ_Costing;

                AllMOld = (x.Total + z.TotalBiaya);

                var d = db.StockProductDJ.SingleOrDefault(p => p.Nomor.Equals(r.Nomor));

                var x2 = d.StockProductDJ_CostingProduct;
                var z2 = d.StockProductDJ_Costing;

                AllMOld = (x.Total + z.TotalBiaya);
                AllM = (x2.Total + z2.TotalBiaya);

                //var x = db.RevalLogProductDJ.SingleOrDefault(p => p.Nomor == "" + r.Nomor + "");

                TableRow tr = new TableRow();
                TableCell td;

                /*CheckBox cb = new CheckBox();

                cb.ID = "cb_po" + index.ToString();
                cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                HiddenField hf = new HiddenField();
                hf.ID = "hf" + index.ToString();
                hf.Value = r.ID.ToString();
                hf.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                td = new TableCell();
                td.Controls.Add(cb);
                td.Controls.Add(hf);
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.TglJam(r.OperatorTgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.RevalLogProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.HargaRupiahRoundOld).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Mutasi(Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.HargaRupiahRoundOld), Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.HargaRupiahRound));
                tr.Cells.Add(td);

                //td = new TableCell(); 
                //td.Text = Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.ModalUSDOld).ToString("N2");
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(AllMOld).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(AllM).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.ModalUSDOld).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductDJ_PricingMU.ModalUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductDJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductDJ_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                var det = (from p in db.RevalLogProductDJ_Stone1A
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalLogProductDJ_Stone1B
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalLogProductDJ_Stone2
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalLogProductDJ_Stone3
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalLogProductDJ_Stone4
                           where p.IDForm == r.ID
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Union(
                           from p in db.RevalLogProductDJ_Stone5
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

                td = new TableCell();
                td.Text = r.RevalLogProductDJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.RevalLogProductDJ_CharProduct.CharStoneDist.Nama;
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

        if (OldPrice == 0)
            mutasi = 0;
        else
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