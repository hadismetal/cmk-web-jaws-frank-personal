using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Report_GJ_ListExport : System.Web.UI.Page
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

            Response.AppendHeader("content-disposition", "attachment; filename=PR_GJListExport.xls");
            fillTable(sender, e);
        }
    }

    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var rs = from p in db.RevalLogProductGJ

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


        if (ProductLevel > 0) rs = rs.Where(p => p.RevalLogProductGJ_CharProduct.ProductLevel == ProductLevel);
      
        if (ProductItem > 0) rs = rs.Where(p => p.RevalLogProductGJ_CharProduct.ProductItem == ProductItem);
      
        if (ProductCategory > 0) rs = rs.Where(p => p.RevalLogProductGJ_CharProduct.ProductCategory == ProductCategory);

        else if (order == 1)
            rs.OrderBy(p => p.RevalLogProductGJ_CharProduct.CharProductLevel.Nama);
        else if (order == 2)
            rs.OrderBy(p => p.RevalLogProductGJ_CharProduct.CharProductCategory.Nama);
        else if (order == 3)
            rs.OrderBy(p => p.RevalLogProductGJ_CharProduct.CharProductItem.Nama);

        if (rs != null)
        {
            int index = 1;

            foreach (var r in rs)
            {
                //var x = db.RevalLogProductGJ.SingleOrDefault(p => p.Nomor == "" + r.Nomor + "");

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
                td.Text = r.RevalLogProductGJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductGJ_PricingMU.HargaRupiahRoundOld).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductGJ_PricingMU.HargaRupiahRound).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Mutasi(Convert.ToDecimal(r.RevalLogProductGJ_PricingMU.HargaRupiahRoundOld), Convert.ToDecimal(r.RevalLogProductGJ_PricingMU.HargaRupiahRound));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductGJ_PricingMU.ModalUSDOld).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductGJ_PricingMU.ModalUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductGJ_PricingMU.HargaUSDOld).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductGJ_PricingMU.HargaUSD).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductGJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.RevalLogProductGJ_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.RevalLogProductGJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.RevalLogProductGJ_CharProduct.CharStoneDist.Nama;
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