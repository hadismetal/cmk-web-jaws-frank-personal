using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            status.SelectedValue = "1";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);  
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.TradeIn
                 where (p.StatusVoid!=1 || p.StatusVoid == null)
                 select p;

        string Status = "";
        Status = Request.QueryString["status"];
        if (Status == "1")
            rs = rs.Where(p => p.Draft == false);
        else if (Status == "2")
            rs = rs.Where(p => p.Draft == true);

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.IDLokasi) && p.TipeLokasi == 1);
        }

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = kw.Text;
        if (keyword != "")
        {
            rs = rs.Where(p => p.Nomor == "" + keyword + "" || p.CustomerNama.Contains(kw.Text) || p.Resell.ResellDJ.Any(z => z.StockProductDJ.Nomor == "" + keyword + "") || p.Resell.ResellGJ.Any(z => z.StockProductGJ.Nomor == "" + keyword + "") || p.Resell.ResellPG.Any(z => z.StockProductPG.Nomor == "" + keyword + "") || p.Resell.ResellLD.Any(z => z.StockProductLD_Stone1B.Nomor == "" + keyword + "") || p.SalesOrder.SalesOrderDJ.Any(z=>z.StockProductDJ.Nomor == "" + keyword + "")  || p.SalesOrder.SalesOrderGJ.Any(z => z.StockProductGJ.Nomor == "" + keyword + "") || p.SalesOrder.SalesOrderPG.Any(z => z.StockProductPG.Nomor == "" + keyword + "") || p.SalesOrder.SalesOrderLD.Any(z => z.StockProductLD_Stone1B.Nomor == "" + keyword + ""));
        }

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
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
                

                QtyTradeIn = r.Resell.ResellDJ.Count() + r.Resell.ResellPG.Count();

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
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
                td.Text = r.SalesNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = QtyTradeIn.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Resell.ResellDJ.Sum(p=>p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty)).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneWeight).ToString("N3");
                tr.Cells.Add(td);

                try
                {
                    Harga = Convert.ToDecimal(r.Resell.ResellDJ.Sum(p => p.HargaRupiah)) + Convert.ToDecimal(r.Resell.ResellPG.Sum(p => p.HargaRupiah));
                }
                catch (Exception ex) { }

                td = new TableCell();
                //td.Text = Harga.ToString("N0");
                td.Text = (r.SalesOrder.TotalBayar != null) ? r.SalesOrder.TotalBayar.Value.ToString("N0") : "";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Total += (r.SalesOrder.TotalBayar != null) ? r.SalesOrder.TotalBayar.Value : 0;
                TotalQtyTradeIn += QtyTradeIn;
                TotalBerat += r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight);
                TotalButir += Convert.ToDecimal(r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty));
                TotalCarat += r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneWeight);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 6;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBerat).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalCarat).ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(Total).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            status.Text = Request.QueryString["status"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.Text;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(TradeIn r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"TradeIn_Data.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        decimal ttlByr = 0m;
        ttlByr = (r.SalesOrder.TotalBayar != null) ? r.SalesOrder.TotalBayar.Value : 0 ;

        if ((r.StatusVoid == null || r.StatusVoid == 0) && r.Draft == false && r.SalesOrder.StatusPembayaran == false && (r.SalesOrder.Unpaid.Value) == ttlByr)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"TradeIn_Void.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Void";
            x += "</a>";
        }
        else if (r.Draft == true && r.StatusVoid != 1)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"TradeIn_Delete.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Delete";
            x += "</a>";
        }
        return x;
    }
}