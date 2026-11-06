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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
            fillTable(sender, e);
		}
	}

    protected void fillTable(object sender, EventArgs e)
    {
        //fillOutstanding();
        //fillOutstandingRepair();
        //fillTitipan();
        //fillOmset();
        //fillDJ();
        //fillGJ();
        //fillPG();
        //fillLD();
        //fillMonting();

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
            rs = rs.Where(p=> OutletID.Contains(p.IDLokasi.Value) && p.TipeLokasi == 1);
        }


        decimal OutstandingInvoice = 0m;
        try
        {
            OutstandingInvoice = rs.Sum(p => p.Unpaid);
        }
        catch (Exception ex) { }

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

        tb.Rows.Add(tr);
    }

    protected void fillOutstandingRepair()
    {
        
        var rs = from p in db.SalesOrderRepair
                        where p.StatusPembayaran == false && p.Draft == false
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

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.IDLokasi.Value) && p.TipeLokasi == 1);
        }

        decimal OutstandingRepair = 0m;
        try
        {
            OutstandingRepair = rs.Sum(p => p.Unpaid.Value);
        }
        catch (Exception ex) { }

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

        tb.Rows.Add(tr);
    }

    protected void fillOmset()
    {
        //var rs = from p in db.SalesOrder
        //         where p.Draft == false && p.StatusVoid == false && p.Tgl.Value.Month == DateTime.Today.Month
        //         && p.StatusPembayaran == true
        //         select p;

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

        rs = rs.Where(p=>p.Tgl.Value.Month == DateTime.Today.Month);

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
            rs = rs.Where(p => OutletID.Contains(p.IDLokasi.Value) && p.TipeLokasi == 1);
        }

        rs = rs.Where(p=>p.Tgl >= Convert.ToDateTime(DateTime.Today.Year + "-" + DateTime.Today.Month.ToString() + "-01") && p.Tgl < DateTime.Today.AddDays(1));

        decimal Omset = 0m;
        try
        {
            Omset = rs.Sum(p => p.TotalBayar);
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Omset</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-red\">" + rs.Count().ToString() + "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-red\">" + Convert.ToDecimal(Omset).ToString("N0") + "</p>";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void fillTitipan()
    {
        

        var tt = from p in db.DocTitipan
                 where !p.Draft.Value && p.StatusTitipan.Value != 5 && p.StatusTitipan.HasValue
                 select p;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {

            var c = ad.First();
            tt = tt.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);
        }

        if (ad.Count() > 0)
        {

            var c = ad.First();
            tt = tt.Where(p => p.IDLokasi == c.IDLokasi && p.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            tt = tt.Where(p => OutletID.Contains(p.IDLokasi.Value) && p.TipeLokasi == 1);
        }

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

        tb.Rows.Add(tr2);
    }
    protected void fillDJ()
    {
        var rs = from p in db.StockProductDJ
                 where p.ReturnProduct == false && p.StatusLebur == false
       && !(from s in db.SalesOrderDJ
            where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
            select s.IDProduct).Contains(p.ID)
                 select p;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.StockActualDJ.IDLokasi == c.IDLokasi && p.StockActualDJ.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.StockActualDJ.IDLokasi) && p.StockActualDJ.TipeLokasi == 1);
        }

        decimal StockDJ = 0m;
        try
        {
            StockDJ = rs.Count();
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Diamond Jewellery</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-red\">" + StockDJ.ToString("N0") + "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);

    }

    protected void fillGJ()
    {
        var rs = from p in db.StockProductGJ
                 where p.ReturnProduct == false && p.StatusLebur == false
       && !(from s in db.SalesOrderGJ
            where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
            select s.IDProduct).Contains(p.ID)
                 select p;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.StockActualGJ.IDLokasi == c.IDLokasi && p.StockActualGJ.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.StockActualGJ.IDLokasi) && p.StockActualGJ.TipeLokasi == 1);
        }

        decimal StockGJ = 0m;
        try
        {
            StockGJ = rs.Count();
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Gold Jewellery</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-red\">" + StockGJ.ToString("N0") + "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void fillPG()
    {
        var rs = from p in db.StockProductPG
                 where p.ReturnProduct == false
       && !(from s in db.SalesOrderPG
            where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
            select s.IDProduct).Contains(p.ID)
                 select p;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.StockActualPG.IDLokasi == c.IDLokasi && p.StockActualPG.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.StockActualPG.IDLokasi) && p.StockActualPG.TipeLokasi == 1);
        }

        decimal StockPG = 0m;
        try
        {
            StockPG = rs.Count();
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Plain Gold</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-red\">" + StockPG.ToString("N0") + "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void fillLD()
    {
        var rs = from p in db.StockProductLD_Stone1B
                 where p.ReturnProduct == false
       && !(from s in db.SalesOrderLD
            where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
            select s.IDProduct).Contains(p.ID)
                 select p;

        //check admin sales
        var ad = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (ad.Count() > 0)
        {
            var c = ad.First();
            rs = rs.Where(p => p.StockActualLD_Stone1B.IDLokasi == c.IDLokasi && p.StockActualLD_Stone1B.TipeLokasi == c.TipeLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.IDLokasi.Value) && p.TipeLokasi == 1);
        }

        decimal StockLD = 0m;
        try
        {
            StockLD = rs.Count();
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Loose Diamond</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-lg text-red\">" + StockLD.ToString("N0") + "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }

    protected void fillMonting()
    {
        var rs = from p in db.StockProductMounting
                 where p.StatusReceive == false
                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            var x = d.First();

            rs = rs.Where(p => p.StockActualMounting.TipeLokasi == x.TipeLokasi && p.StockActualMounting.IDLokasi == x.IDLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.StockActualMounting.IDLokasi) && p.StockActualMounting.TipeLokasi == 1);
        }

        decimal StockMounting = 0m;
        try
        {
            StockMounting = rs.Count();
        }
        catch (Exception ex) { }

        decimal HargaJual = 0m;
        try
        {
            HargaJual = rs.Sum(p => p.StockProductMounting_PricingMU.HargaRupiahRound.Value);
        }
        catch (Exception ex) { }

        decimal HargaM = 0m;
        try
        {
            decimal CostingProduct = rs.Sum(p => p.StockProductMounting_CostingProduct.Total);
            decimal Costing = rs.Sum(p => p.StockProductMounting_Costing.TotalBiaya);
            HargaM = CostingProduct + Costing;
        }
        catch (Exception ex) { }

        decimal HargaR = 0m;
        try
        {
            HargaR = rs.Sum(p => p.StockProductMounting_PricingMU.HargaUSD);
        }
        catch (Exception ex) { }

        decimal GrossWeight = 0m;
        try
        {
            GrossWeight = rs.Sum(p => p.StockProductMounting_CharProduct.GrossWeight);
        }
        catch (Exception ex) { }

        decimal NetWeight = 0m;
        try
        {
            NetWeight = rs.Sum(p => p.StockProductMounting_CharProduct.NettoWeight);
        }
        catch (Exception ex) { }

        TableRow tr = new TableRow();
        TableCell td = new TableCell();

        td = new TableCell();
        td.Text = "<b>Stock Mounting</b>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-red text-lg\">" + StockMounting.ToString("N0") + "</p>";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = "<p class=\"text-red text-lg\">" + HargaJual.ToString("N0") + "</p>";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);

    }
}