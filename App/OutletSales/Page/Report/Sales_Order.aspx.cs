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
            status.SelectedValue = "2";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);  
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesOrder
                 where !(from z in db.SalesOrderDJ
                         where z.StockProductDJ.Legacy.Value == 1
                         select z.SalesOrder.ID).Contains(p.ID)
                         && !(from z in db.SalesOrderPG
                              where z.StockProductPG.Legacy.Value == 1
                              select z.SalesOrder.ID).Contains(p.ID)
                         && (p.SalesOrderDJ.Count() > 0 || p.SalesOrderGJ.Count() > 0 || p.SalesOrderLD.Count() > 0 || p.SalesOrderPG.Count() > 0)
                 select p;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);
        }

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = kw.Text;
        if (keyword != "")
        {
            rs = from p in rs
                 where p.Nomor.Contains(keyword)
                        || (from q in db.SalesOrderDJ
                            where q.StockProductDJ.Nomor.Contains(kw.Text)
                            select q.IDForm).Contains(p.ID)
                        || (from q in db.SalesOrderGJ
                            where q.StockProductGJ.Nomor.Contains(kw.Text)
                            select q.IDForm).Contains(p.ID)
                        || (from q in db.SalesOrderPG
                            where q.StockProductPG.Nomor.Contains(kw.Text)
                            select q.IDForm).Contains(p.ID)
                        || (from q in db.SalesOrderLD
                            where q.StockProductLD_Stone1B.Nomor.Contains(kw.Text)
                            select q.IDForm).Contains(p.ID)
                        || p.CustomerNama.Contains(kw.Text)
                 select p;
        }
        
        if(status.SelectedValue=="1")
            rs = rs.Where(p=> p.StatusVoid == true);
        if (status.SelectedValue == "2")
            rs = rs.Where(p => p.StatusVoid == false);
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalBayar = 0m;
            decimal TotalPaid = 0m;
            decimal TotalUnpaid = 0m;
            decimal TotalQty = 0m;

            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                decimal Harga = 0m;
                decimal Paid = 0m;
                decimal Unpaid = 0m;
                int Qty = 0;

                Qty = r.SalesOrderDJ.Count() + r.SalesOrderPG.Count() + r.SalesOrderGJ.Count() + r.SalesOrderLD.Count() ;

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
                td.Text = r.IDCustomer.HasValue ? r.CustomerNama : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDSales.HasValue ? r.DataSales.Nama : "";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                tr.Cells.Add(td);

                try
                {
                    Harga = Convert.ToDecimal(r.TotalBayar.Value);
                }
                catch (Exception ex) { }

                try
                {
                    Paid = Convert.ToDecimal(r.Paid.Value);
                }
                catch (Exception ex) { }

                try
                {
                    Unpaid = Convert.ToDecimal(r.Unpaid.Value);
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = Harga.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Paid.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Unpaid.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(r.Keterangan);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.KeteranganVoid != null ? Str.HtmlBreak(r.KeteranganVoid) : "-";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.TglVoid.HasValue ? Str.FullDate(r.TglVoid) : "-";
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalBayar += Harga;
                TotalUnpaid += Unpaid;
                TotalPaid += Paid;
                TotalQty += Qty;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 6;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalQty).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalPaid).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalUnpaid).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            
            status.SelectedValue = Request.QueryString["status"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;


        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "Sales_OrderPrint.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;


        Response.Redirect(href);
    }
    protected string TD_HyperLink(SalesOrder r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Sales_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        //if (r.Tgl >= DateTime.Today && r.StatusVoid == false && r.StatusPembayaran == false && r.Draft == false && r.Unpaid.Value == r.TotalBayar.Value)
        if (r.Tgl >= DateTime.Today && r.StatusVoid == false && r.Draft == false)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"Sales_FormVoid.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Void";
            x += "</a>";
        }
        //else if (r.StatusVoid == false && r.StatusPembayaran == false && r.Draft == true)
        else if (r.StatusVoid == false && r.StatusPembayaran == false && r.Draft == true)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"Sales_Delete.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Delete";
            x += "</a>";
        }
        return x;
    }
}