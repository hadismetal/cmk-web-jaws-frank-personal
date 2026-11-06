using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_InvoicePG_Rekap_Export : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=InvoicePG_Rekap_Export.xls");
            fillTable(sender, e);
        }
    }
        
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesOrderPG
                 where p.SalesOrder.Draft == false && p.SalesOrder.IDSales != null
                 && p.SalesOrder.StatusPembayaran == true
                 select p;

        //check admin sales
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int Lokasi = Convert.ToInt32(Request.QueryString["lokasi"]);

        if (Tipe == 1 || Tipe == 2)
        {
            rs = rs.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
        }

        rs = rs.OrderByDescending(p => p.SalesOrder.Tgl);

        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        int GroupBy = Convert.ToInt32(Request.QueryString["groupby"]);
        if (GroupBy == 1)
        {
            var rslt = rs.GroupBy(p => p.StockProductPG.CharProductItem);
            if (rs.Count() > 0)
            {
                decimal TotalHarga = 0m;
                decimal TotalDiscount = 0m;
                decimal TotalDiscountProgram = 0m;
                decimal TotalDiscountGift = 0m;
                decimal TotalDiscountRound = 0m;
                decimal TotalWeight = 0m;
                int prev = 0;
                int index = 1;
                foreach (var r in rslt)
                {
                    decimal Weight = 0m;
                    int Qty = 1;

                    var st = (from p in db.SalesOrderPG
                              where p.StockProductPG.ProductItem == r.Key.ID
                              && p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1)
                              select p);

                    Weight = st.Sum(p => p.StockProductPG.GrossWeight.Value);

                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString().Replace(".", "").Replace(",", ".") + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Key.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Count().ToString().Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.TotalBayar.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.Discount.Value) / r.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.DiscountProgram.Value) / r.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountGift.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountRound.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Weight.ToString("N2").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);


                    tb.Rows.Add(tr);

                    TotalHarga += r.Sum(p => p.TotalBayar.Value);
                    TotalDiscount += r.Sum(p => p.Discount.Value);
                    TotalDiscountProgram += r.Sum(p => p.DiscountProgram.Value);
                    TotalDiscountGift += r.Sum(p => p.DiscountGift.Value);
                    TotalDiscountRound += r.Sum(p => p.DiscountRound.Value);
                    TotalWeight += Weight;
                }

                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = "<b>Total</b>";
                td2.ColumnSpan = 2;
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + rs.Count().ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount / rs.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalWeight).ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                tb.Rows.Add(tr2);
            }
        }
        else if (GroupBy == 2)
        {
            var rslt = rs.GroupBy(p => p.StockProductPG.CharGoldLevel);
            if (rs.Count() > 0)
            {
                decimal TotalHarga = 0m;
                decimal TotalDiscount = 0m;
                decimal TotalDiscountProgram = 0m;
                decimal TotalDiscountGift = 0m;
                decimal TotalDiscountRound = 0m;
                decimal TotalWeight = 0m;
                int prev = 0;
                int index = 1;
                foreach (var r in rslt)
                {
                    decimal Weight = 0m;
                    int Qty = 1;

                    var st = (from p in db.SalesOrderPG
                              where p.StockProductPG.ProductLevel == r.Key.ID
                              && p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1)
                              select p);

                    Weight = st.Sum(p => p.StockProductPG.GrossWeight.Value);

                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString().Replace(".", "").Replace(",", ".") + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Key.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Count().ToString().Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.TotalBayar.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.Discount.Value) / r.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.DiscountProgram.Value) / r.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountGift.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountRound.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Weight.ToString("N2").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);


                    tb.Rows.Add(tr);

                    TotalHarga += r.Sum(p => p.TotalBayar.Value);
                    TotalDiscount += r.Sum(p => p.Discount.Value);
                    TotalDiscountProgram += r.Sum(p => p.DiscountProgram.Value);
                    TotalDiscountGift += r.Sum(p => p.DiscountGift.Value);
                    TotalDiscountRound += r.Sum(p => p.DiscountRound.Value);
                    TotalWeight += Weight;
                }

                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = "<b>Total</b>";
                td2.ColumnSpan = 2;
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + rs.Count().ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount / rs.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalWeight).ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                tb.Rows.Add(tr2);
            }
        }
        else if (GroupBy == 3)
        {
            var rslt = rs.GroupBy(p => p.StockProductPG.CharGoldModel);
            if (rs.Count() > 0)
            {
                decimal TotalHarga = 0m;
                decimal TotalDiscount = 0m;
                decimal TotalDiscountProgram = 0m;
                decimal TotalDiscountGift = 0m;
                decimal TotalDiscountRound = 0m;
                decimal TotalWeight = 0m;
                int prev = 0;
                int index = 1;
                foreach (var r in rslt)
                {
                    decimal Weight = 0m;
                    int Qty = 1;

                    var st = (from p in db.SalesOrderPG
                              where p.StockProductPG.Model == r.Key.ID
                              && p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1)
                              select p);

                    Weight = st.Sum(p => p.StockProductPG.GrossWeight.Value);

                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString().Replace(".", "").Replace(",", ".") + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Key.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Count().ToString().Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.TotalBayar.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.Discount.Value) / r.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.DiscountProgram.Value) / r.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountGift.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountRound.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Weight.ToString("N2").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);


                    tb.Rows.Add(tr);

                    TotalHarga += r.Sum(p => p.TotalBayar.Value);
                    TotalDiscount += r.Sum(p => p.Discount.Value);
                    TotalDiscountProgram += r.Sum(p => p.DiscountProgram.Value);
                    TotalDiscountGift += r.Sum(p => p.DiscountGift.Value);
                    TotalDiscountRound += r.Sum(p => p.DiscountRound.Value);
                    TotalWeight += Weight;
                }

                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = "<b>Total</b>";
                td2.ColumnSpan = 2;
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + rs.Count().ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount / rs.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalWeight).ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                tb.Rows.Add(tr2);
            }
        }
        else if (GroupBy == 4)
        {
            var rslt = rs.GroupBy(p => p.SalesOrder.DataSales);
            if (rs.Count() > 0)
            {
                decimal TotalHarga = 0m;
                decimal TotalDiscount = 0m;
                decimal TotalDiscountProgram = 0m;
                decimal TotalDiscountGift = 0m;
                decimal TotalDiscountRound = 0m;
                decimal TotalWeight = 0m;
                int prev = 0;
                int index = 1;
                foreach (var r in rslt)
                {
                    decimal Weight = 0m;
                    int Qty = 1;

                    var st = (from p in db.SalesOrderPG
                              where p.StockProductPG.Model == r.Key.ID
                              && p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1)
                              select p);

                    Weight = st.Sum(p => p.StockProductPG.GrossWeight.Value);

                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString().Replace(".", "").Replace(",", ".") + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Key.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Count().ToString().Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.TotalBayar.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.Discount.Value) / r.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.DiscountProgram.Value) / r.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountGift.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountRound.Value).ToString("N0").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Weight.ToString("N2").Replace(".", "").Replace(",", ".");
                    tr.Cells.Add(td);


                    tb.Rows.Add(tr);

                    TotalHarga += r.Sum(p => p.TotalBayar.Value);
                    TotalDiscount += r.Sum(p => p.Discount.Value);
                    TotalDiscountProgram += r.Sum(p => p.DiscountProgram.Value);
                    TotalDiscountGift += r.Sum(p => p.DiscountGift.Value);
                    TotalDiscountRound += r.Sum(p => p.DiscountRound.Value);
                    TotalWeight += Weight;
                }

                TableRow tr2 = new TableRow();
                TableCell td2;

                td2 = new TableCell();
                td2.Text = "<b>Total</b>";
                td2.ColumnSpan = 2;
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + rs.Count().ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount / rs.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0").Replace(".", "").Replace(",", ".") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalWeight).ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
                tr2.Cells.Add(td2);

                tb.Rows.Add(tr2);
            }
        }
    }
}