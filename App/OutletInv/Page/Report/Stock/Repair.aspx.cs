using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page
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

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        err.InnerHtml = "PLU Tidak ditemukan.";
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;

        var rs = from p in db.DocRepairResult
                 join q in db.StockProductDJ on p.IDProduct equals q.ID
                 where p.IDProduct != null && !p.Draft.Value
                 select new { p,q};

        rs = rs.OrderByDescending(p => p.p.Tgl);

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.p.Tgl >= start && p.p.Tgl < end.AddDays(1));
        }

        if (kw.Text != "")
            rs = rs.Where(p => p.q.Nomor.Contains(kw.Text));
        
        
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == ProductCategory);

        if (rs.Count() > 0)
        {
            int index = 1;
            foreach (var r in rs)
            {
                

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = index.ToString() + ".";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Hyperlink(r.p);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.p.StockProductDJ.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.Tgl(r.p.Tgl);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.p.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                var det = (from p in r.p.StockProductDJ.StockProductDJ_Stone1A
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in r.p.StockProductDJ.StockProductDJ_Stone1B
                           where p.StatusTerpasang == 1
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in r.p.StockProductDJ.StockProductDJ_Stone2
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in r.p.StockProductDJ.StockProductDJ_Stone3
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in r.p.StockProductDJ.StockProductDJ_Stone4
                           select new { TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                           from p in r.p.StockProductDJ.StockProductDJ_Stone5
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

                tb.Rows.Add(tr);

                index++;
            }
        }
    }

    protected string TD_Hyperlink(DocRepairResult d)
    {
        string x = string.Empty;

        x += "<a target='_blank' href='Repair_Item.aspx?id=" + d.StockProductDJ.ID  + "'>" + d.Nomor + "</a>";

        return x;
    }
}