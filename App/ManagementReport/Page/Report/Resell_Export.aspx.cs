using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_Resell_Export : System.Web.UI.Page
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
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Resell_Export.xls");
            fillTable(sender, e);
        }
    }

    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.Resell
                 where !(from z in db.TradeIn
                         select z.IDResell).Contains(p.ID)
                 select p;

        string Status = "";
        Status = Request.QueryString["status"];
        if (Status == "1")
            rs = rs.Where(p => p.Draft == false);
        else if (Status == "2")
            rs = rs.Where(p => p.Draft == true);

        //check admin sales
        int Tipe = Convert.ToInt32(Request.QueryString["tipe"]);
        int Lokasi = Convert.ToInt32(Request.QueryString["tipelokasi"]);

        if ((Tipe == 1 || Tipe == 2) && Lokasi > 0)
        {
            rs = rs.Where(p => p.IDLokasi == Lokasi && p.TipeLokasi == Tipe);
        }

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = Request.QueryString["kw"];
        if (keyword != "") rs = rs.Where(p => p.Nomor == "" + keyword + "" || p.CustomerNama.Contains(keyword));

        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal Total = 0m;
            decimal TotalQtyTradeIn = 0m;
            decimal TotalBerat = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;

            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                decimal Harga = 0m;
                int QtyTradeIn = 0;


                QtyTradeIn = r.ResellDJ.Count() + r.ResellPG.Count();

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                /*td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);*/

                td = new TableCell();
                td.Text = "<p>" + index++.ToString().Replace(".", "").Replace(",", ".") + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CustomerNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = QtyTradeIn.ToString().Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight).ToString("N2").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty)).ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneWeight).ToString("N3").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                try
                {
                    Harga = Convert.ToDecimal(r.ResellDJ.Sum(p => p.HargaRupiah)) + Convert.ToDecimal(r.ResellPG.Sum(p => p.HargaRupiah));
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = Harga.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Total += Harga;
                TotalQtyTradeIn += QtyTradeIn;
                TotalBerat += r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight);
                TotalButir += Convert.ToDecimal(r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty));
                TotalCarat += r.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneWeight);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 4;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalQtyTradeIn.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBerat).ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalCarat).ToString("N3").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(Total).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected string TD_HyperLink(Resell r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Resell_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";
        return x;
    }
}