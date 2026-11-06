using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales_Item : System.Web.UI.Page
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
    App gp = new App();
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            //HyperLink

            //Form
            fillForm();

            //Generate

        }
        fillTable(sender, e);
    }

    //Exception
    protected void DataError() { Response.Redirect("Sales.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.TradeIn.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/Sales_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        decimal GT = 0m;
        decimal GT2 = 0m;

        decimal GTRO = 0m;

        var d = db.TradeIn.SingleOrDefault(p=>p.ID == myID);

        //Item
        fillItemDJ(ref GT, ref GT2, d);
        fillItemPG(ref GT, ref GT2, d);
        fillItemGJ(ref GT, ref GT2, d);

        fillItemDJRO(ref GTRO,d);
        fillItemPGRO(ref GTRO,d);
        fillItemGJRO(ref GTRO,d);

        //Grand-Total
        Sub_TotalSO(GT2, true);
        Sub_TotalRO(GTRO, true);
        tb.Focus();
    }

    //Item
    protected void fillItemDJ(ref decimal GT, ref decimal GT2, TradeIn d)
    {
        var rs = from p in db.SalesOrderDJ
                 where p.IDForm == d.IDSalesOrder
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0m;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderSO(0);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(0, r.ID, r.IDProduct);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Discount).ToString("N0") + " %";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgram).ToString("N0") + " %";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountRound).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb2.Rows.Add(tr);

            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GT2 += ST2;

            Sub_TotalSO( ST2, false);
        }
    }
    protected void fillItemPG(ref decimal GT, ref decimal GT2, TradeIn d)
    {
        var rs = from p in db.SalesOrderPG
                 where p.IDForm == d.IDSalesOrder
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderSO(1);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(1, r.ID, r.IDProduct);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductPG.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductPG.CharProductItem.Nama;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Discount).ToString("N0") + " %";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountProgram).ToString("N0") + " %";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountGift).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.DiscountRound).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.TotalBayar).ToString("N0");
            tr.Cells.Add(td);

            tb2.Rows.Add(tr);

            ST2 += Convert.ToDecimal(r.TotalBayar);

            index++;
        }

        //Total
        if (rs.Count() > 0)
        {
            GT2 += ST2;

            Sub_TotalSO(ST2, false);
        }
    }
    protected void fillItemGJ(ref decimal GT, ref decimal GT2, TradeIn d)
    {

    }

    //Custom Row
    protected void Sub_HeaderSO(int ItemIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.ItemIndex(ItemIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb2);
        tr.Cells.Add(td);

        tb2.Rows.Add(tr);
    }
    protected void Sub_TotalSO(decimal TotalBayar, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 5;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(TotalBayar) + "</p>";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);

        tb2.Rows.Add(tr);
    }

    //Display Column
    protected string TD_HyperLink(int ItemIndex, int ID, int IDProduct)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (ItemIndex == 0)
        {
            x += "&nbsp;<div class=\"btn-group\">";
            x += "  <a target=\"_blank\" class=\"btn btn-info\" href=\"DJ_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Info";
            x += "  </a>";
        }
        else if (ItemIndex == 1)
        {
            x += "&nbsp;<div class=\"btn-group\">";
            x += "  <a target=\"_blank\" class=\"btn btn-info\" href=\"PG_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Info";
            x += "  </a>";
        }
        x += "</div>";

        return x;
    }
    protected string TD_Nama(string Nama)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";

        return x;
    }
    protected string TD_Nama(string Nama, string GIA)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

        return x;
    }

    //Resell
    //Item
    protected void fillItemDJRO(ref decimal GTRO, TradeIn d)
    {
        var rs = from p in db.ResellDJ
                 where p.IDForm == d.IDResell
                 select p;

        int index = 1;

        decimal ST = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderRO(0);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLinkRO(0, r.ID, r.IDProduct);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
            tr.Cells.Add(td);

            decimal HargaJual = 0m;
            DateTime sales = r.SalesOrder.Tgl.Value;
            DateTime resell = r.Resell.Tgl.Value;

            HargaJual = Db.SingleDecimal("SELECT TotalBayar FROM SalesOrderDJ WHERE IDForm=" + r.SalesOrder.ID + " AND IDProduct = " + r.IDProduct, Db.Cnn_DB);

            td = new TableCell();
            td.Text = Convert.ToDecimal(HargaJual).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaAcuan.Value).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah.Value).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(100-((r.HargaRupiah.Value/HargaJual)*100)).ToString("N0") + "%";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (resell - sales).TotalDays.ToString() + " hari";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);

            index++;

        }
        //Total
        if (rs.Count() > 0)
        {
            GTRO += ST;

            Sub_TotalRO(ST, false);
        }
    }
    protected void fillItemPGRO(ref decimal GTRO, TradeIn d)
    {
        var rs = from p in db.ResellPG
                 where p.IDForm == d.IDResell
                 select p;

        int index = 1;

        decimal ST = 0;
        decimal ST2 = 0;

        //Sub-Header
        if (rs.Count() > 0) Sub_HeaderRO(1);

        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLinkRO(1, r.ID, r.IDProduct);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductPG.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductPG.CharProductItem.Nama;
            tr.Cells.Add(td);

            decimal HargaJual = 0m;
            DateTime sales = r.SalesOrder.Tgl.Value;
            DateTime resell = r.Resell.Tgl.Value;

            HargaJual = Db.SingleDecimal("SELECT TotalBayar FROM SalesOrderPG WHERE IDForm=" + r.SalesOrder.ID + " AND IDProduct = " + r.IDProduct, Db.Cnn_DB);

            td = new TableCell();
            td.Text = Convert.ToDecimal(HargaJual).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaAcuan.Value).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.HargaRupiah.Value).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(100 - ((r.HargaRupiah.Value / HargaJual) * 100)).ToString("N0") + "%";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = (resell - sales).TotalDays.ToString() + " hari";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            ST += Convert.ToDecimal(r.HargaRupiah);

            index++;
        }

        //Total
        if (rs.Count() > 0)
        {
            GTRO += ST;

            Sub_TotalRO(ST, false);
        }
    }
    protected void fillItemGJRO(ref decimal GT, TradeIn d)
    {

    }

    //Custom Row
    protected void Sub_HeaderRO(int ItemIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.ItemIndex(ItemIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void Sub_TotalRO(decimal Total, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 6;
        td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        td.Text += "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg\">" + Str.Number0(Total) + "</p>";
        //td.CssClass = "text-right";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected string TD_HyperLinkRO(int ItemIndex, int ID, int IDProduct)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (ItemIndex == 0)
        {
            x += "&nbsp;<div class=\"btn-group\">";
            x += "  <a target=\"_blank\" class=\"btn btn-info\" href=\"DJ_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Info";
            x += "  </a>";
        }
        else if (ItemIndex == 1)
        {
            x += "&nbsp;<div class=\"btn-group\">";
            x += "  <a target=\"_blank\" class=\"btn btn-info\" href=\"PG_Detail.aspx?id=" + IDProduct + "\">";
            x += "      <i class=\"icon-search\"></i> ";
            x += "      Info";
            x += "  </a>";
        }
        x += "</div>";

        return x;
    }
}