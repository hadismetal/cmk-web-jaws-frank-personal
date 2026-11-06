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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {

            DLC.CharProductItem(productitem);
            DLC.CharProductLevel(productlevel);
            DLC.CharProductCategory(productcategory);

            //DLC.LocationList(cabang, tipe);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

        var dj = from p in db.StockProductDJ
                 where p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value
                           && !(from s in db.SalesOrderDJ
                                where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false
                                select s.IDProduct).Contains(p.ID)
                 select p;
        var pg = from p in db.StockProductPG
                 where p.Draft == false && p.Approval != null && p.ReturnProduct == false && p.StatusPenjualan == 0 && p.StatusLebur == false
                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            var x = d.First();

            dj = dj.Where(p => p.StockActualDJ.TipeLokasi == x.TipeLokasi && p.StockActualDJ.IDLokasi == x.IDLokasi);
            pg = pg.Where(p => p.StockActualPG.TipeLokasi == x.TipeLokasi && p.StockActualPG.IDLokasi == x.IDLokasi);

        }

        /*int Cabang = Convert.ToInt32(cabang.SelectedValue);
        if (Cabang > 0)
        {
            dj = dj.Where(p => p.StockActualDJ.IDLokasi == Cabang && p.StockActualDJ.TipeLokasi == tipe);
            pg = pg.Where(p => p.StockActualPG.IDLokasi == Cabang && p.StockActualPG.TipeLokasi == tipe);
        }
        else
        {
            dj = dj.Where(p => p.StockActualDJ.TipeLokasi == tipe);
            pg = pg.Where(p => p.StockActualPG.TipeLokasi == tipe);
        }*/

        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0)
        {
            dj = dj.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
            pg = pg.Where(p => p.ProductItem == ProductItem);
        }

        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0)
        {
            dj = dj.Where(p => p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
            pg = pg.Where(p => p.ProductLevel == ProductLevel);
        }

        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0)
        {
            dj = dj.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
            //pg = pg.Where(p => p. == ProductItem);
        }

        if (dj.Count() == 0) App.TableNoResult(tb);
        else
        {
            decimal TotalStock = 0m;
            decimal TotalNetWeight = 0m;
            decimal TotalGrossWeight = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalM = 0m;
            decimal TotalR = 0m;

            var rs_dj = dj.GroupBy(p => new { p.StockActualDJ.TipeLokasi, p.StockActualDJ.IDLokasi });

            foreach (var r in rs_dj)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = DL.LocationType(r.First().StockActualDJ.TipeLokasi);
                tr.Cells.Add(td);

                string Lokasi = "";

                if (r.First().StockActualDJ.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID = " + r.First().StockActualDJ.IDLokasi, Db.Cnn_DB);
                else if (r.First().StockActualDJ.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + r.First().StockActualDJ.IDLokasi, Db.Cnn_DB);
                else if (r.First().StockActualDJ.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + r.First().StockActualDJ.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Count()).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Sum(p => p.StockProductDJ_CharProduct.NettoWeight)).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Sum(p => p.StockProductDJ_CharProduct.GrossWeight)).ToString("N2");
                tr.Cells.Add(td);

                decimal Butir = 0m;
                decimal Carat = 0m;

                Butir = Convert.ToDecimal(r.Sum(p => p.StockProductDJ_Stone1A.Sum(z => z.TotalButir))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ_Stone1B.Sum(z => z.TotalButir))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ_Stone2.Sum(z => z.TotalButir))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ_Stone3.Sum(z => z.TotalButir))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ_Stone4.Sum(z => z.TotalButir))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ_Stone5.Sum(z => z.TotalButir)));
                Carat = Convert.ToDecimal(r.Sum(p => p.StockProductDJ_Stone1A.Sum(z => z.TotalCarat))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ_Stone1B.Sum(z => z.TotalCarat))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ_Stone2.Sum(z => z.TotalCarat))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ_Stone3.Sum(z => z.TotalCarat))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ_Stone4.Sum(z => z.TotalCarat))) + Convert.ToDecimal(r.Sum(p => p.StockProductDJ_Stone5.Sum(z => z.TotalCarat)));

                td = new TableCell();
                td.Text = Butir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Carat.ToString("N3");
                tr.Cells.Add(td);

                decimal AllM = 0m;
                decimal AllMRupiah = 0m;
                try
                {

                    decimal x = r.Sum(p => p.StockProductDJ_CostingProduct.Total);
                    decimal z = r.Sum(p => p.StockProductDJ_Costing.TotalBiaya);

                    AllM = (x + z);

                    AllMRupiah = r.Sum(p => (p.StockProductDJ_CostingProduct.Total * p.StockProductDJ_Costing.TotalRate.Value));
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = AllMRupiah.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Sum(p=>p.StockProductDJ_PricingMU.HargaRupiahRound)).ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalStock += r.Count();
                TotalNetWeight += Convert.ToDecimal(r.Sum(p => p.StockProductDJ_CharProduct.NettoWeight));
                TotalGrossWeight += Convert.ToDecimal(r.Sum(p => p.StockProductDJ_CharProduct.GrossWeight));
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalM += AllMRupiah;
                TotalR += Convert.ToDecimal(r.Sum(p => p.StockProductDJ_PricingMU.HargaRupiahRound));
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.ColumnSpan = 2;
            td2.Text = "<b>Total</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalStock.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalButir.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalM.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalR.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }

        if (pg.Count() == 0) App.TableNoResult(tb2);
        else
        {
            decimal TotalStock = 0m;
            decimal TotalNetWeight = 0m;
            decimal TotalGrossWeight = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalM = 0m;
            decimal TotalR = 0m;

            //var rs_pg = pg.GroupBy(p => new { p.StockActualPG.TipeLokasi, p.StockActualPG.IDLokasi });
            var rs_pg = from p in pg
                        group p by new {p.StockActualPG.TipeLokasi, p.StockActualPG.IDLokasi } into g
                        select new
                        {
                            TipeLokasi = g.Key,
                            IDLokasi = g.First().StockActualPG.IDLokasi,
                            NettoWeight = g.Sum(s => s.NettoWeight),
                            GrossWeight = g.Sum(s => s.GrossWeight),
                            Harga = g.Sum(s => s.Harga),
                            TotalHarga24 = g.Sum(s => s.TotalHarga24),
                            TotalItem = g.Count(),
                            StockActualPG = g.First().StockActualPG
                        };

            foreach (var r in rs_pg)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = DL.LocationType(r.StockActualPG.TipeLokasi);
                tr.Cells.Add(td);

                string Lokasi = "";

                if (r.StockActualPG.TipeLokasi == 0)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocWarehouse WHERE ID = " + r.StockActualPG.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualPG.TipeLokasi == 1)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID = " + r.StockActualPG.IDLokasi, Db.Cnn_DB);
                else if (r.StockActualPG.TipeLokasi == 2)
                    Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID = " + r.StockActualPG.IDLokasi, Db.Cnn_DB);

                td = new TableCell();
                td.Text = Lokasi;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.TotalItem).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                decimal Butir = 0m;
                decimal Carat = 0m;

                td = new TableCell();
                td.Text = Butir.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Carat.ToString("N3");
                tr.Cells.Add(td);

                decimal AllM = 0m;
                decimal AllMRupiah = 0m;
                try
                {

                    AllMRupiah = r.Harga.Value;
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = AllMRupiah.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.TotalHarga24).ToString("N0");
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalStock += r.TotalItem;
                TotalNetWeight += Convert.ToDecimal(r.NettoWeight);
                TotalGrossWeight += Convert.ToDecimal(r.GrossWeight);
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalM += AllMRupiah;
                TotalR += Convert.ToDecimal(r.TotalHarga24);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.ColumnSpan = 2;
            td2.Text = "<b>Total</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalStock.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalNetWeight.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalGrossWeight.ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalButir.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalCarat.ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalM.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalR.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb2.Rows.Add(tr2);

        }


    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            //cabang.SelectedValue = Request.QueryString["cabang"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
        }
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        //href += "&cabang=" + cabang.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&tipe=" + tipe;

        Response.Redirect(href);
    }
}