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
            rs = rs.Where(p => OutletID.Contains(p.IDLokasi.Value) && p.TipeLokasi == 1);
        }

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.NoSO == "" + keyword + "" || p.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text));
        
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalBayar = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalNetWeight = 0m;
            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                decimal Butir = 0m;
                decimal Carat = 0m;
                int Qty = 1;

                Butir = r.Butir;
                Carat = r.Carat;
 
                TotalHarga += r.TotalBayar;
                TotalBayar += r.TotalBayar;
                
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalNetWeight += r.NetWeight;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Item</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalCarat).ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalNetWeight).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }

        //PG

        //var pg = from p in db.SalesOrderPG
        //         where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false && p.StockProductPG.StatusPenjualan == 1
        //         select p;

        ////check admin sales
        //var s = from p in db.DataAdminSales
        //         where p.IDUser == App.UserID
        //         select p;

        //if (s.Count() > 0)
        //{
        //    var c = s.First();
        //    pg = pg.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
        //}

        //if (dari.Text != "" && sampai.Text != "")
        //{
        //    pg = pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        //}

        //if (pg.Count() > 0)
        //{
        //    decimal TotalHarga = 0m;
        //    decimal TotalBayar = 0m;
        //    decimal TotalNetWeight = 0m;
        //    int prev = 0;
        //    int index = 1;
        //    foreach (var r in pg)
        //    {
        //        int Qty = 1;

        //        TotalHarga += r.HargaRupiah.Value;
        //        TotalBayar += r.TotalBayar.Value;
        //        TotalNetWeight += r.StockProductPG.GrossWeight.Value;
        //    }

        //    TableRow tr2 = new TableRow();
        //    TableCell td2;

        //    td2 = new TableCell();
        //    td2.Text = "<b>Plain Gold</b>";
        //    tr2.Cells.Add(td2);

        //    td2 = new TableCell();
        //    td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
        //    tr2.Cells.Add(td2);

        //    td2 = new TableCell();
        //    td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
        //    tr2.Cells.Add(td2);

        //    td2 = new TableCell();
        //    td2.Text = "<b>" + Convert.ToDecimal(TotalBayar).ToString("N0") + "</b>";
        //    tr2.Cells.Add(td2);

        //    td2 = new TableCell();
        //    td2.Text = "<b>0</b>";
        //    tr2.Cells.Add(td2);

        //    td2 = new TableCell();
        //    td2.Text = "<b>0</b>";
        //    tr2.Cells.Add(td2);

        //    td2 = new TableCell();
        //    td2.Text = "<b>" + Convert.ToDecimal(TotalNetWeight).ToString("N2") + "</b>";
        //    tr2.Cells.Add(td2);

        //    tb.Rows.Add(tr2);
        //}

        decimal TotalQtySales = 0m;

        if (groupby.SelectedValue == "0")
        {

            var o_dj = from p in db.SalesOrderDJ
                       where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                       select p;

            if (ad.Count() > 0)
            {
                var c = ad.First();
                o_dj = o_dj.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
            }

            o_dj = o_dj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_dj = o_dj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            var o_pg = from p in db.SalesOrderPG
                       where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                       select p;

            if (ad.Count() > 0)
            {
                var c = ad.First();
                o_pg = o_pg.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
            }

            o_pg = o_pg.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_pg = o_pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            var o_gj = from p in db.SalesOrderGJ
                       where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                       select p;

            if (ad.Count() > 0)
            {
                var c = ad.First();
                o_gj = o_gj.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
            }

            o_gj = o_gj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_gj = o_gj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            var o_ld = from p in db.SalesOrderLD
                       where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                       select p;

            if (ad.Count() > 0)
            {
                var c = ad.First();
                o_ld = o_ld.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
            }

            o_ld = o_ld.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_ld = o_ld.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            if (o_dj.Count() > 0)
            {
                decimal NetOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;

                Qty = o_dj.Count();

                //Header
                TableRow tr_h = new TableRow();
                TableCell td_h;

                td_h = new TableCell();
                td_h.Text = "";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Jewellery";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_dj.Count().ToString();
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalQtySales += Convert.ToDecimal(o_dj.Count());
            }

            if (o_pg.Count() > 0)
            {
                decimal NetOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;
                
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Plain Gold";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_pg.Count().ToString();
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalQtySales += Convert.ToDecimal(o_pg.Count());
            }

            if (o_gj.Count() > 0)
            {
                decimal NetOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Gold Jewellery";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_gj.Count().ToString();
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalQtySales += Convert.ToDecimal(o_gj.Count());
            }

            if (o_ld.Count() > 0)
            {
                decimal NetOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Loose Diamond";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_ld.Count().ToString();
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalQtySales += Convert.ToDecimal(o_ld.Count());
            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
        else if (groupby.SelectedValue == "1")
        {
            var item = from p in db.CharProductItem
                       orderby p.Nama
                       select p;

            var o_dj = from p in db.SalesOrderDJ
                       where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                       select p;

            if (ad.Count() > 0)
            {
                var c = ad.First();
                o_dj = o_dj.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
            }

            o_dj = o_dj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_dj = o_dj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            var o_pg = from p in db.SalesOrderPG
                       where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                       select p;

            if (ad.Count() > 0)
            {
                var c = ad.First();
                o_pg = o_pg.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
            }

            o_pg = o_pg.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_pg = o_pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            var o_gj = from p in db.SalesOrderGJ
                       where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                       select p;

            if (ad.Count() > 0)
            {
                var c = ad.First();
                o_gj = o_gj.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
            }

            o_gj = o_gj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_gj = o_gj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            var o_ld = from p in db.SalesOrderLD
                       where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                       select p;

            if (ad.Count() > 0)
            {
                var c = ad.First();
                o_ld = o_ld.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
            }

            o_ld = o_ld.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_ld = o_ld.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            if (o_dj.Count() > 0)
            {
                //Header
                TableRow tr_h = new TableRow();
                TableCell td_h;

                td_h = new TableCell();
                td_h.Text = "";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                foreach (var r in item)
                {
                    decimal NetOmzet = 0m;
                    decimal Qty = 0m;
                    int prev = 0;
                    int index = 1;

                    var d = from p in o_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                            select p;

                    var d2 = from p in o_pg
                            where p.StockProductPG.ProductItem == r.ID
                            select p;

                    var d3 = from p in o_gj
                             where p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                             select p;

                    var d4 = from p in o_ld
                             where p.StockProductLD_Stone1B.ProductItem == r.ID
                             select p;

                    Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                    
                    if (Qty > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = r.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        tr.Cells.Add(td);

                        tb2.Rows.Add(tr);

                        TotalQtySales += Qty;
                    }
                }
                
            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }

        else if (groupby.SelectedValue == "2")
        {
            var item = (from p in db.SalesOrder
                        orderby p.SalesNama
                        select p).GroupBy(p => p.IDSales);

            var o_dj = from p in db.SalesOrderDJ
                       where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                       select p;

            if (ad.Count() > 0)
            {
                var c = ad.First();
                o_dj = o_dj.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
            }

            o_dj = o_dj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_dj = o_dj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            var o_pg = from p in db.SalesOrderPG
                       where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                       select p;

            if (ad.Count() > 0)
            {
                var c = ad.First();
                o_pg = o_pg.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
            }

            o_pg = o_pg.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_pg = o_pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            var o_gj = from p in db.SalesOrderGJ
                       where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                       select p;

            if (ad.Count() > 0)
            {
                var c = ad.First();
                o_gj = o_gj.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
            }

            o_gj = o_gj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_gj = o_gj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            var o_ld = from p in db.SalesOrderLD
                       where p.SalesOrder.StatusPembayaran == false && p.SalesOrder.Draft == false && p.SalesOrder.StatusVoid == false
                       select p;

            if (ad.Count() > 0)
            {
                var c = ad.First();
                o_ld = o_ld.Where(p => p.SalesOrder.IDLokasi == c.IDLokasi && p.SalesOrder.TipeLokasi == c.TipeLokasi);
            }

            o_ld = o_ld.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_ld = o_ld.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            if (o_dj.Count() > 0)
            {
                //Header
                TableRow tr_h = new TableRow();
                TableCell td_h;

                td_h = new TableCell();
                td_h.Text = "";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                foreach (var r in item)
                {
                    decimal NetOmzet = 0m;
                    decimal Qty = 0m;
                    int prev = 0;
                    int index = 1;

                    try
                    {
                        var d = from p in o_dj
                                where p.SalesOrder.IDSales == r.Key
                                select p;

                        var d2 = from p in o_pg
                                 where p.SalesOrder.IDSales == r.Key
                                 select p;

                        var d3 = from p in o_gj
                                 where p.SalesOrder.IDSales == r.Key
                                 select p;

                        var d4 = from p in o_ld
                                 where p.SalesOrder.IDSales == r.Key
                                 select p;

                        Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                    }
                    catch { Qty = 0; }

                    if (Qty > 0)
                    {
                        string SalesNama = Db.SingleString("SELECT Nama FROM DataSales WHERE ID=" + r.Key, Db.Cnn_DB);
                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = SalesNama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        tr.Cells.Add(td);

                        tb2.Rows.Add(tr);

                        TotalQtySales += Convert.ToDecimal(Qty);
                    }
                }

            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            groupby.SelectedValue = Request.QueryString["groupby"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;


        Response.Redirect(href);
    }
}