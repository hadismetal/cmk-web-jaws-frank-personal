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

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);  
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesOrderPG
                 where p.SalesOrder.Draft == false && p.SalesOrder.IDSales != null
                 select p;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.SalesOrder.IDLokasi.Value) && p.SalesOrder.TipeLokasi == 1);
        }

        rs = rs.OrderByDescending(p => p.SalesOrder.Tgl);

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        int GroupBy = Convert.ToInt32(groupby.SelectedValue);
        if(GroupBy == 1)
        {
            var rslt = rs.GroupBy(p=>p.StockProductPG.CharProductItem);
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
                               && p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1)
                               select p);

                    Weight = st.Sum(p => p.StockProductPG.GrossWeight.Value);

                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Key.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Count().ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.TotalBayar.Value).ToString("N0");
                    tr.Cells.Add(td);     

                    td = new TableCell();
                    td.Text = (r.Sum(p=>p.Discount.Value)/r.Count()).ToString("N0") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.DiscountProgram.Value) / r.Count()).ToString("N0") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountGift.Value).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountRound.Value).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Weight.ToString("N2");
                    tr.Cells.Add(td);


                    tb.Rows.Add(tr);

                    TotalHarga += r.Sum(p=>p.TotalBayar.Value);
                    TotalDiscount += r.Sum(p => p.Discount.Value);
                    TotalDiscountProgram += r.Sum(p => p.DiscountProgram.Value);
                    TotalDiscountGift += r.Sum(p=>p.DiscountGift.Value);
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
                td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount / rs.Count()).ToString("N0") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalWeight).ToString("N2") + "</b>";
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
                              && p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1)
                              select p);

                    Weight = st.Sum(p => p.StockProductPG.GrossWeight.Value);

                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Key.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Count().ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.TotalBayar.Value).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.Discount.Value) / r.Count()).ToString("N0") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.DiscountProgram.Value) / r.Count()).ToString("N0") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountGift.Value).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountRound.Value).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Weight.ToString("N2");
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
                td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount / rs.Count()).ToString("N0") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalWeight).ToString("N2") + "</b>";
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
                              && p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1)
                              select p);

                    Weight = st.Sum(p => p.StockProductPG.GrossWeight.Value);

                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Key.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Count().ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.TotalBayar.Value).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.Discount.Value) / r.Count()).ToString("N0") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.DiscountProgram.Value) / r.Count()).ToString("N0") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountGift.Value).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountRound.Value).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Weight.ToString("N2");
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
                td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount / rs.Count()).ToString("N0") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalWeight).ToString("N2") + "</b>";
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
                              && p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1)
                              select p);

                    Weight = st.Sum(p => p.StockProductPG.GrossWeight.Value);

                    if (!Response.IsClientConnected) break;

                    //Sub-Header

                    TableRow tr = new TableRow();
                    TableCell td;

                    td = new TableCell();
                    td.Text = "<p>" + index++.ToString() + ".</p>";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Key.Nama;
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Count().ToString();
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.TotalBayar.Value).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.Discount.Value) / r.Count()).ToString("N0") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = (r.Sum(p => p.DiscountProgram.Value) / r.Count()).ToString("N0") + "%";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountGift.Value).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = r.Sum(p => p.DiscountRound.Value).ToString("N0");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = Weight.ToString("N2");
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
                td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount / rs.Count()).ToString("N0") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0") + " %</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
                tr2.Cells.Add(td2);

                td2 = new TableCell();
                td2.Text = "<b>" + Convert.ToDecimal(TotalWeight).ToString("N2") + "</b>";
                tr2.Cells.Add(td2);

                tb.Rows.Add(tr2);
            }
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            groupby.SelectedValue = Request.QueryString["groupby"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&groupby=" + groupby.SelectedValue;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "InvoicePG_RekapExport.aspx?id=1";
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&groupby=" + groupby.SelectedValue;

        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "InvoicePG_RekapPrint.aspx?id=1";
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&groupby=" + groupby.SelectedValue;

        Response.Redirect(href);
    }
}