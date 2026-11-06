using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Report_InvoicePG_Export : System.Web.UI.Page
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }

    protected int idlokasi { get { return App.QueryInt(this, "idlokasi"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=InvoicePG_Export.xls");
            fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesOrderPG
                 where p.SalesOrder.StatusPembayaran == true
                 select p;

        //check admin sales
        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int Lokasi = Convert.ToInt32(Request.QueryString["lokasi"]);

        if (Tipe == 1 || Tipe == 2)
        {
            rs = rs.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
        }

        rs = rs.OrderByDescending(p => p.SalesOrder.Tgl);

        string keyword = Request.QueryString["kw"];
        if (keyword != "") rs = rs.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(Request.QueryString["kw"]) || p.Invoice.Contains(Request.QueryString["kw"]));

        if (Request.QueryString["status"] == "1")
            rs = rs.Where(p => p.SalesOrder.StatusVoid == true);
        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalNetWeight = 0m;
            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                int Qty = 1;

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString().Replace(".", "").Replace(",", ".") + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.SalesOrder.Tgl);
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.Invoice;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesOrder.CustomerNama;
                tr.Cells.Add(td);

                DataSales dat = r.SalesOrder.DataSales;
                string szSalesNm = string.Empty;
                if (dat != null)
                {
                    if (dat.Nama != null)
                        szSalesNm = dat.Nama;
                }

                td = new TableCell();
                td.Text = szSalesNm; // r.SalesOrder.DataSales.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.SupplierNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.SupplierSJ;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.KodeBarang;
                tr.Cells.Add(td);
                
                td = new TableCell();
                td.Text = Qty.ToString().Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.TotalBayar.Value.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Discount.Value.ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountProgram.Value.ToString("N0").Replace(".", "").Replace(",", ".") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountGift.Value.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountRound.Value.ToString("N0").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.GrossWeight.Value.ToString("N2").Replace(".", "").Replace(",", ".");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalHarga += r.TotalBayar.Value;
                TotalDiscount += r.Discount.Value;
                TotalDiscountProgram += r.DiscountProgram.Value;
                TotalDiscountGift += r.DiscountGift.Value;
                TotalDiscountRound += r.DiscountRound.Value;
                TotalNetWeight += r.StockProductPG.GrossWeight.Value;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 10;
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
            td2.Text = "<b>" + Convert.ToDecimal(TotalNetWeight).ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }
}