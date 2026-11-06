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

        if (!IsPostBack)
        {
            //fillTable(sender, e);
        }
    }

    protected void fillTable(object sender, EventArgs e)
    {
        fillOutstanding();
        fillOutstandingRepair();
        fillTitipan();
        fillOmset();

        DateTime start;
        start = Convert.ToDateTime("01-" + DateTime.Now.Month + "-" + DateTime.Now.Year);
        //start = Convert.ToDateTime("01-01-2014");

        var po = (from p in db.SalesOrderDJ
                  where p.SalesOrder.StatusPembayaran == true && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                  && p.StockProductDJ.Legacy.Value == null
                  select new
                  {
                      TotalBayar = p.TotalBayar.Value,
                      Invoice = p.Invoice,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      Butir = p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value,
                      Carat = p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat,
                      NetWeight = p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight,
                      Unpaid = p.SalesOrder.Unpaid.Value
                  }).Union(
                  from p in db.SalesOrderGJ
                  where p.SalesOrder.StatusPembayaran == true && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                  select new
                  {
                      TotalBayar = p.TotalBayar.Value,
                      Invoice = p.Invoice,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      Butir = 0m,
                      Carat = 0m,
                      NetWeight = p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight,
                      Unpaid = 0m
                  }).Union(
                  from p in db.SalesOrderPG
                  where p.SalesOrder.StatusPembayaran == true && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                  && p.StockProductPG.Legacy.Value == null
                  select new
                  {
                      TotalBayar = p.TotalBayar.Value,
                      Invoice = p.Invoice,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      Butir = 0m,
                      Carat = 0m,
                      NetWeight = p.StockProductPG.NettoWeight.Value,
                      Unpaid = 0m
                  }).Union(
                  from p in db.SalesOrderLD
                  where p.SalesOrder.StatusPembayaran == true && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                  select new
                  {
                      TotalBayar = p.TotalBayar.Value,
                      Invoice = p.Invoice,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      Butir = 1m,
                      Carat = p.StockProductLD_Stone1B.TotalCarat,
                      NetWeight = 0m,
                      Unpaid = 0m
                  })
                 ;

        po = po.Where(p => p.Tgl.Value.Month == DateTime.Today.Month && p.Tgl.Value.Year == DateTime.Today.Year);

        // label omset pusat
        Decimal Omset = 0m;
        try
        {
            if (po.Count() != 0)
                Omset += po.Sum(p => p.TotalBayar);
        }
        catch (Exception) { }

        var sog = po.GroupBy(p => new { p.Tgl.Value.Day, p.Tgl.Value.Month, p.Tgl.Value.Year });
        sog = sog.OrderBy(p => p.Key.Year).ThenBy(p => p.Key.Month).ThenBy(p => p.Key.Day);

        //grafik pusat
        int index = 1;
        foreach (var r in sog)
        {
            string d = (r.Sum(p => (p.TotalBayar))).ToString("N0").Replace(".", "");
            if (index == 1)
            {
                chart += "{category:\"" + r.Key.Day + "\",omset:" + d + "}";
            }
            else
                chart += ",{category:\"" + r.Key.Day + "\",omset:" + d + "}";
            index++;
        }

        var po2 = from p in db.SalesOrder
                  where p.StatusVoid == false
                  select p;

        po2 = po2.Where(p => p.Tgl.Value >= start && p.Tgl.Value <= DateTime.Today);

        // label omset pusat
        Decimal Omset2 = 0m;
        try
        {
            if (po2.Count() != 0)
                Omset2 += po2.Count();
        }
        catch (Exception) { }
        //lbl_pusat.Text = Str.Number0(Omset2);

        var sog2 = po2.GroupBy(p => new { p.Tgl.Value.Day, p.Tgl.Value.Month, p.Tgl.Value.Year });
        sog2 = sog2.OrderBy(p => p.Key.Year).ThenBy(p => p.Key.Month).ThenBy(p => p.Key.Day);

        //grafik pusat
        int index2 = 1;
        foreach (var r in sog2)
        {
            string d = (r.Count()).ToString("N0").Replace(".", "");
            if (index2 == 1)
            {
                chart2 += "{category:\"" + r.Key.Day + "\",qty:" + d + "}";
            }
            else
                chart2 += ",{category:\"" + r.Key.Day + "\",qty:" + d + "}";
            index2++;
        }
    }

    protected void fillOutstanding()
    {
        var rs = (from p in db.SalesOrderDJ
                  where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                  select new
                  {
                      TotalBayar = p.TotalBayar.Value,
                      Invoice = p.Invoice,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      Butir = p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value,
                      Carat = p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat,
                      NetWeight = p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight,
                      Unpaid = p.SalesOrder.Unpaid.Value
                  }).Union(
                  from p in db.SalesOrderGJ
                  where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                  select new
                  {
                      TotalBayar = p.TotalBayar.Value,
                      Invoice = p.Invoice,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      Butir = 0m,
                      Carat = 0m,
                      NetWeight = p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight,
                      Unpaid = 0m
                  }).Union(
                  from p in db.SalesOrderPG
                  where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                  select new
                  {
                      TotalBayar = p.TotalBayar.Value,
                      Invoice = p.Invoice,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      Butir = 0m,
                      Carat = 0m,
                      NetWeight = p.StockProductPG.NettoWeight.Value,
                      Unpaid = 0m
                  }).Union(
                  from p in db.SalesOrderLD
                  where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                  select new
                  {
                      TotalBayar = p.TotalBayar.Value,
                      Invoice = p.Invoice,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      Butir = 1m,
                      Carat = p.StockProductLD_Stone1B.TotalCarat,
                      NetWeight = 0m,
                      Unpaid = 0m
                  })
                 ;


        decimal OutstandingInvoice = 0m;
        try
        {
            OutstandingInvoice = rs.Sum(p => p.Unpaid);
        }
        catch (Exception) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Piutang Invoice Customer</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-red\">" + rs.Count().ToString() + "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-red\">" + Convert.ToDecimal(OutstandingInvoice).ToString("N0") + "</p>";
        tr.Cells.Add(td);

        //tb.Rows.Add(tr);
    }

    protected void fillOutstandingRepair()
    {
        var rs = from p in db.SalesOrderRepair
                 where p.StatusPembayaran == false && p.Draft == false
                 select p;

        decimal OutstandingRepair = 0m;
        try
        {
            OutstandingRepair = rs.Sum(p => p.Unpaid.Value);
        }
        catch (Exception) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Piutang Repair Customer</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-red\">" + rs.Count().ToString() + "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-red\">" + Convert.ToDecimal(OutstandingRepair).ToString("N0") + "</p>";
        tr.Cells.Add(td);

        //tb.Rows.Add(tr);
    }

    protected void fillOmset()
    {
        var rs = (from p in db.SalesOrderDJ
                  where p.SalesOrder.StatusPembayaran == true && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                  && p.StockProductDJ.Legacy.Value == null
                  select new
                  {
                      TotalBayar = p.TotalBayar.Value,
                      Invoice = p.Invoice,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      Butir = p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value,
                      Carat = p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat,
                      NetWeight = p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight,
                      Unpaid = p.SalesOrder.Unpaid.Value
                  }).Union(
                  from p in db.SalesOrderGJ
                  where p.SalesOrder.StatusPembayaran == true && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                  select new
                  {
                      TotalBayar = p.TotalBayar.Value,
                      Invoice = p.Invoice,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      Butir = 0m,
                      Carat = 0m,
                      NetWeight = p.StockProductGJ.StockProductGJ_CharProduct.NettoWeight,
                      Unpaid = 0m
                  }).Union(
                  from p in db.SalesOrderPG
                  where p.SalesOrder.StatusPembayaran == true && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                  && p.StockProductPG.Legacy.Value == null
                  select new
                  {
                      TotalBayar = p.TotalBayar.Value,
                      Invoice = p.Invoice,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      Butir = 0m,
                      Carat = 0m,
                      NetWeight = p.StockProductPG.NettoWeight.Value,
                      Unpaid = 0m
                  }).Union(
                  from p in db.SalesOrderLD
                  where p.SalesOrder.StatusPembayaran == true && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                  select new
                  {
                      TotalBayar = p.TotalBayar.Value,
                      Invoice = p.Invoice,
                      IDLokasi = p.SalesOrder.IDLokasi,
                      TipeLokasi = p.SalesOrder.TipeLokasi,
                      Tgl = p.SalesOrder.Tgl,
                      NoSO = p.SalesOrder.Nomor,
                      CustomerNama = p.SalesOrder.CustomerNama,
                      Butir = 1m,
                      Carat = p.StockProductLD_Stone1B.TotalCarat,
                      NetWeight = 0m,
                      Unpaid = 0m
                  })
                 ;

        rs = rs.Where(p => p.Tgl.Value.Month == DateTime.Today.Month && p.Tgl.Value.Year == DateTime.Today.Year);

        decimal Omset = 0m;
        try
        {
            Omset = rs.Sum(p => p.TotalBayar);
        }
        catch (Exception) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Omset dari tanggal 01 " +  Str.BulanTahun(DateTime.Today) + " sampai hari ini</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-red\">" + rs.Count().ToString() + "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-red\">" + Convert.ToDecimal(Omset).ToString("N0") + "</p>";
        tr.Cells.Add(td);

        //tb.Rows.Add(tr);
    }

    protected void fillTitipan()
    {
        var tt = from p in db.DocTitipan
                 where !p.Draft.Value && p.StatusTitipan.Value != 5 && p.StatusTitipan.HasValue
                 select p;

        TableRow tr2 = new TableRow();
        TableCell td2 = new TableCell();

        td2 = new TableCell();
        td2.Text = "<b>Titipan Customer</b>";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-lg text-red\">" + tt.Count().ToString() + "</p>";
        tr2.Cells.Add(td2);

        td2 = new TableCell();
        td2.Text = "<p class=\"text-lg text-red\">-</p>";
        tr2.Cells.Add(td2);

        //tb.Rows.Add(tr2);
    }
    public string chart = "";
    public string chart2 = "";
}