using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Serialization;

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

            int Tipe = 1;
            if (Request.QueryString["tipelokasi"] != "All" && Request.QueryString["tipelokasi"] != "0")
                Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);

            DLC.LocationList(lokasi, Tipe);

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);  
        }
    }

    private decimal p_ForceConvertToDecimal(object objValue)
    {
        decimal decReturn = decimal.Zero;

        try
        {
            decReturn = Convert.ToDecimal(objValue);
        }
        catch
        {
            //  do nothing.
            //  return 0;
        }

        return decReturn;
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //var rs = from p in db.SalesOrderDJ
        //         where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false
        //         && !(from z in db.SalesOrderDJ
        //              where z.StockProductDJ.Legacy.Value == 1
        //              select z.SalesOrder.ID).Contains(p.ID)
        //         && p.SalesOrder.StatusPembayaran == true
        //         select p;

        var rs = from p in db.SalesOrderDJ
                 where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false
                 && p.StockProductDJ.Legacy.Value == null
                 && p.SalesOrder.StatusPembayaran == true
                 select p;

        //check admin sales
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int Lokasi = Convert.ToInt32(lokasi.SelectedValue);

        if (Tipe == 1 || Tipe == 2)
        {
            
            rs = rs.Where(p => ((Lokasi > 0) ? p.SalesOrder.IDLokasi == Lokasi : 1 == 1) && p.SalesOrder.TipeLokasi == Tipe);
        }

        int Basic = Convert.ToInt32(basic.SelectedValue);
        if (Basic > 0)
        {
            rs = from p in rs
                 where (from x in db.StockBasic
                        select x.ProtoNomor).Contains(p.StockProductDJ.ProtoNomor)
                 select p;
        }

        rs = rs.OrderByDescending(p => p.SalesOrder.Tgl);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text));
        
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalBayar = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalNetWeight = 0m;
            //int prev = 0;
            //int index = 1;
            foreach (var r in rs)
            {
                decimal Butir = 0m;
                decimal Carat = 0m;
                //int Qty = 1;

                Butir = r.StockProductDJ.StockProductDJ_Stone1A.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone1B.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone2.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone3.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone4.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone5.Sum(p => p.TotalButir);
                Carat = r.StockProductDJ.StockProductDJ_Stone1A.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone1B.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone2.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone3.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone4.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone5.Sum(p => p.TotalCarat);
 
                //TotalHarga += r.HargaRupiah.Value;
                //TotalBayar += r.TotalBayar.Value;
                //TotalDiscount += r.Discount.Value;
                //TotalDiscountProgram += r.DiscountProgram.Value;
                //TotalDiscountGift += r.DiscountGift.Value;
                //TotalDiscountRound += r.DiscountRound.Value;
                //TotalButir += Butir;
                //TotalCarat += Carat;
                //TotalNetWeight += r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight;

                TotalHarga += p_ForceConvertToDecimal(r.HargaRupiah);
                TotalBayar += p_ForceConvertToDecimal(r.TotalBayar);
                TotalDiscount += p_ForceConvertToDecimal(r.Discount);
                TotalDiscountProgram += p_ForceConvertToDecimal(r.DiscountProgram);
                TotalDiscountGift += p_ForceConvertToDecimal(r.DiscountGift);
                TotalDiscountRound += p_ForceConvertToDecimal(r.DiscountRound);
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalNetWeight += r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Diamond Jewellery</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalHarga).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscount / ((rs.Count() > 0) ? rs.Count() : 1) ).ToString("N0") + " %</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountProgram / ((rs.Count() > 0) ? rs.Count() : 1)).ToString("N0") + " %</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalBayar).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalButir).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalCarat).ToString("N3") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalNetWeight).ToString("N2") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }

        //PG

        var pg = from p in db.SalesOrderPG
                 where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false
                 && p.StockProductPG.Legacy.Value != 1
                 && p.SalesOrder.StatusPembayaran == true
                 select p;

        //check admin sales
        var s = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (Tipe == 1 || Tipe == 2)
        {
            pg = pg.Where(p => ((Lokasi > 0) ? p.SalesOrder.IDLokasi == Lokasi : 1 == 1) && p.SalesOrder.TipeLokasi == Tipe);
        }

        if (dari.Text != "" && sampai.Text != "")
        {
            pg = pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl <  Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (pg.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalBayar = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalNetWeight = 0m;
            int prev = 0;
            int index = 1;
            foreach (var r in pg)
            {
                int Qty = 1;

                //TotalHarga += r.HargaRupiah.Value;
                //TotalBayar += r.TotalBayar.Value;
                //TotalDiscount += r.Discount.Value;
                //TotalDiscountProgram += r.DiscountProgram.Value;
                //TotalDiscountGift += r.DiscountGift.Value;
                //TotalDiscountRound += r.DiscountRound.Value;
                //TotalNetWeight += r.StockProductPG.GrossWeight.Value;

                TotalHarga += p_ForceConvertToDecimal(r.HargaRupiah);
                TotalBayar += p_ForceConvertToDecimal(r.TotalBayar);
                TotalDiscount += p_ForceConvertToDecimal(r.Discount);
                TotalDiscountProgram += p_ForceConvertToDecimal(r.DiscountProgram);
                TotalDiscountGift += p_ForceConvertToDecimal(r.DiscountGift);
                TotalDiscountRound += p_ForceConvertToDecimal(r.DiscountRound);
                TotalNetWeight += p_ForceConvertToDecimal(r.StockProductPG.GrossWeight);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Plain Gold</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + pg.Count().ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalHarga).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscount / ((pg.Count() > 0) ? pg.Count() : 1)).ToString("N0") + " %</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountProgram / ((pg.Count() > 0) ? pg.Count() : 1)).ToString("N0") + " %</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalBayar).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>0</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>0</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalNetWeight).ToString("N2") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }

        var rsgj = from p in db.SalesOrderGJ
                 where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false
                 && p.SalesOrder.StatusPembayaran == true
                 select p;

        if (Tipe == 1 || Tipe == 2)
        {

            rsgj = rsgj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
        }

        if (Basic > 0)
        {
            rsgj = from p in rsgj
                 where (from x in db.StockBasic
                        select x.ProtoNomor).Contains(p.StockProductGJ.ProtoNomor)
                 select p;
        }

        rsgj = rsgj.OrderByDescending(p => p.SalesOrder.Tgl);

        if (keyword != "") rsgj = rsgj.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text));

        if (dari.Text != "" && sampai.Text != "")
        {
            rsgj = rsgj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rsgj.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalBayar = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalNetWeight = 0m;
            //int prev = 0;
            //int index = 1;
            foreach (var r in rsgj)
            {
                decimal Butir = 0m;
                decimal Carat = 0m;
                //int Qty = 1;

                Butir = 0m;
                Carat = 0m;

                //TotalHarga += r.HargaRupiah.Value;
                //TotalBayar += r.TotalBayar.Value;
                //TotalDiscount += r.Discount.Value;
                //TotalDiscountProgram += r.DiscountProgram.Value;
                //TotalDiscountGift += r.DiscountGift.Value;
                //TotalDiscountRound += r.DiscountRound.Value;
                //TotalButir += Butir;
                //TotalCarat += Carat;
                //TotalNetWeight += r.StockProductGJ.StockProductGJ_CharProduct.NettoWeight;

                TotalHarga += p_ForceConvertToDecimal(r.HargaRupiah);
                TotalBayar += p_ForceConvertToDecimal(r.TotalBayar);
                TotalDiscount += p_ForceConvertToDecimal(r.Discount);
                TotalDiscountProgram += p_ForceConvertToDecimal(r.DiscountProgram);
                TotalDiscountGift += p_ForceConvertToDecimal(r.DiscountGift);
                TotalDiscountRound += p_ForceConvertToDecimal(r.DiscountRound);
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalNetWeight += r.StockProductGJ.StockProductGJ_CharProduct.NettoWeight;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Gold Jewellery</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rsgj.Count().ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalHarga).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscount / ((rsgj.Count() > 0) ? rsgj.Count() : 1)).ToString("N0") + " %</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountProgram / ((rsgj.Count() > 0) ? rsgj.Count() : 1)).ToString("N0") + " %</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalBayar).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalButir).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalCarat).ToString("N3") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalNetWeight).ToString("N2") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }

        //Loose Diamond
        var rsld = from p in db.SalesOrderLD
                   where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false
                   && p.SalesOrder.StatusPembayaran == true
                   select p;

        if (Tipe == 1 || Tipe == 2)
        {

            rsld = rsld.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
        }

        rsld = rsld.OrderByDescending(p => p.SalesOrder.Tgl);

        if (keyword != "") rsld = rsld.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text));

        if (dari.Text != "" && sampai.Text != "")
        {
            rsld = rsld.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rsld.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalBayar = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;
            decimal TotalNetWeight = 0m;
            //int prev = 0;
            //int index = 1;
            foreach (var r in rsld)
            {
                decimal Butir = 0m;
                decimal Carat = 0m;
                //int Qty = 1;

                Butir = 1m;
                Carat = r.StockProductLD_Stone1B.CaratButir;

                //TotalHarga += r.HargaRupiah.Value;
                //TotalBayar += r.TotalBayar.Value;
                //TotalDiscount += r.Discount.Value;
                //TotalDiscountProgram += r.DiscountProgram.Value;
                //TotalDiscountGift += r.DiscountGift.Value;
                //TotalDiscountRound += r.DiscountRound.Value;
                //TotalButir += Butir;
                //TotalCarat += Carat;
                //TotalNetWeight += r.StockProductLD.StockProductLD_CharProduct.NettoWeight;

                TotalHarga += p_ForceConvertToDecimal(r.HargaRupiah);
                TotalBayar += p_ForceConvertToDecimal(r.TotalBayar);
                TotalDiscount += p_ForceConvertToDecimal(r.Discount);
                TotalDiscountProgram += p_ForceConvertToDecimal(r.DiscountProgram);
                TotalDiscountGift += p_ForceConvertToDecimal(r.DiscountGift);
                TotalDiscountRound += p_ForceConvertToDecimal(r.DiscountRound);
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalNetWeight += 0m; ;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Loose Diamond</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rsld.Count().ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalHarga).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscount / ((rsld.Count() > 0) ? rsld.Count() : 1)).ToString("N0") + " %</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountProgram / ((rsld.Count() > 0) ? rsld.Count() : 1)).ToString("N0") + " %</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalBayar).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalButir).ToString("N0") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalCarat).ToString("N3") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + p_ForceConvertToDecimal(TotalNetWeight).ToString("N2") + "</b>";
            td2.CssClass = "text-right";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }

        decimal TotalQtySales = 0m;
        decimal TotalOmzetSales = 0m;
        decimal TotalQtyResell = 0m;
        decimal TotalOmzetResell = 0m;
        decimal TotalQtyTradeIn = 0m;
        decimal TotalOmzetTradeIn = 0m;
        decimal TotalQtyAll = 0m;
        decimal TotalOmzetAll = 0m;

        var o_dj = from p in db.SalesOrderDJ
                   where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                   p.SalesOrder.StatusVoid == false
                   && p.SalesOrder.Draft == false
                   && p.StockProductDJ.Legacy.Value == null
                   && p.SalesOrder.StatusPembayaran == true
                   select p;

        if (Basic > 0)
        {
            o_dj = from p in o_dj
                   where (from x in db.StockBasic
                          select x.ProtoNomor).Contains(p.StockProductDJ.ProtoNomor)
                   select p;
        }

        if (Tipe == 1 || Tipe == 2)
        {
            o_dj = o_dj.Where(p => ((Lokasi > 0) ? p.SalesOrder.IDLokasi == Lokasi : 1 == 1) && p.SalesOrder.TipeLokasi == Tipe);
        }

        o_dj = o_dj.OrderByDescending(p => p.SalesOrder.Tgl);

        if (dari.Text != "" && sampai.Text != "")
        {
            o_dj = o_dj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var r_dj = from p in db.ResellDJ
                   where !(from z in db.TradeIn
                           select z.IDResell).Contains(p.IDForm)
                   && p.Resell.StatusVoid == null && p.Resell.Draft == false
                   select p;

        var ro_dj = from p in db.ResellDJ
                    where !(from z in db.TradeIn
                            select z.IDResell).Contains(p.IDForm)
                    && p.Resell.StatusVoid == null && p.Resell.Draft == false
                    select p;

        var rot_dj = from p in db.ResellDJ
                     where !(from z in db.TradeIn
                             select z.IDResell).Contains(p.IDForm)
                     && p.Resell.StatusVoid == null && p.Resell.Draft == false
                     select p;

        if (Basic > 0)
        {
            r_dj = from p in r_dj
                   where (from x in db.StockBasic
                          select x.ProtoNomor).Contains(p.StockProductDJ.ProtoNomor)
                   select p;
            ro_dj = from p in ro_dj
                    where (from x in db.StockBasic
                           select x.ProtoNomor).Contains(p.StockProductDJ.ProtoNomor)
                    select p;
            rot_dj = from p in rot_dj
                     where (from x in db.StockBasic
                            select x.ProtoNomor).Contains(p.StockProductDJ.ProtoNomor)
                     select p;
        }

        if ((Tipe == 1 || Tipe == 2))
        {
            r_dj = r_dj.Where(p => ((Lokasi > 0) ? p.Resell.IDLokasi == Lokasi : 1 == 1) && p.Resell.TipeLokasi == Tipe);
            ro_dj = ro_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi == Lokasi);
            //ro_dj = ro_dj.Where(p => p.SalesOrder.IDLokasi == p.Resell.IDLokasi && p.SalesOrder.TipeLokasi == p.Resell.TipeLokasi);
            rot_dj = rot_dj.Where(p => p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi != Lokasi);
            //rot_dj = rot_dj.Where(p => p.SalesOrder.IDLokasi != p.Resell.IDLokasi || p.SalesOrder.TipeLokasi != p.Resell.TipeLokasi);
        }
        //else
        //{
        //    r_dj = r_dj.Where(p => ((Lokasi > 0) ? p.Resell.IDLokasi == Lokasi : 1 == 1) && p.Resell.TipeLokasi == Tipe);
        //    ro_dj = ro_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
        //    rot_dj = rot_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
        //    //ro_dj = ro_dj.Where(p => p.SalesOrder.IDLokasi == p.Resell.IDLokasi && p.SalesOrder.TipeLokasi == p.Resell.TipeLokasi && p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
        //    //rot_dj = rot_dj.Where(p => (p.SalesOrder.IDLokasi != p.Resell.IDLokasi || p.SalesOrder.TipeLokasi != p.Resell.TipeLokasi) && p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
        //}

        r_dj = r_dj.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            r_dj = r_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            ro_dj = ro_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            ro_dj = rot_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var e_dj = from p in db.ResellDJ
                   where (from z in db.TradeIn
                          where z.Tgl.Value <= p.SalesOrder.Tgl.Value.AddDays(30)
                          && z.Draft == false
                          select z.IDResell).Contains(p.IDForm)

                   select p;

        if (Tipe == 1 || Tipe == 2)
        {
            e_dj = e_dj.Where(p => ((Lokasi > 0) ? p.SalesOrder.IDLokasi == Lokasi : 1 == 1) && p.SalesOrder.TipeLokasi == Tipe);
        }

        e_dj = e_dj.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            e_dj = e_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        //var t_dj = from p in db.ResellDJ
        //           where (from z in db.TradeIn
        //                  where z.Tgl.Value > p.SalesOrder.Tgl.Value.AddDays(30)
        //                  && z.Draft == false
        //                  select z.IDResell).Contains(p.IDForm)
        //                  && p.Resell.StatusVoid == null && p.Resell.Draft == false
        //           select p;

        //var t_dj = from p in db.ResellDJ
        //           where (from z in db.TradeIn
        //                  where z.Draft == false
        //                  select z.IDResell).Contains(p.IDForm)
        //                  && p.Resell.StatusVoid == null && p.Resell.Draft == false
        //           select p;

        var t_dj = from p in db.ResellDJ
                   where (from z in db.TradeIn
                          where z.Draft == false
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.StatusVoid == null && p.Resell.Draft == false
                   select p;

        if (Basic > 0)
        {
            t_dj = from p in t_dj
                   where (from x in db.StockBasic
                          select x.ProtoNomor).Contains(p.StockProductDJ.ProtoNomor)
                   select p;
        }

        if (Tipe == 1 || Tipe == 2)
        {
            t_dj = t_dj.Where(p => ((Lokasi > 0) ? p.Resell.IDLokasi == Lokasi : 1 == 1) && p.Resell.TipeLokasi == Tipe);
        }

        t_dj = t_dj.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            t_dj = t_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }
        //var o_pg = from p in db.SalesOrderPG
        //           where /*!(from z in db.TradeIn
        //                   select z.IDSalesOrder).Contains(p.IDForm)
        //           && p.StatusResell == 0 &&*/
        //           p.SalesOrder.StatusVoid == false && p.TotalBayar.HasValue
        //           && p.SalesOrder.Draft == false
        //           select p;

        //var o_pg = from p in db.SalesOrderPG
        //           where /*!(from z in db.TradeIn
        //                       select z.IDSalesOrder).Contains(p.IDForm)
        //               && p.StatusResell == 0 &&*/
        //           p.SalesOrder.StatusVoid == false && p.TotalBayar.HasValue
        //           && p.SalesOrder.Draft == false
        //           && p.StockProductPG.Legacy.Value != 1
        //           && p.SalesOrder.StatusPembayaran == true
        //           select new
        //           {
        //               SalesOrder = p.SalesOrder,
        //               ProductItem = DLC.ProductItem(),
        //               ProductCategory = DLC.ProductCategory(),
        //               ProductLevel = DLC.ProductLevel(),
        //               ProductSegmen = DLC.ProductSegmen(),
        //               TotalBayar = p.TotalBayar,
        //           };

        var o_pg = from p in db.SalesOrderPG
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false
                       && p.SalesOrder.Draft == false
                       && p.StockProductPG.Legacy.Value != 1
                       && p.SalesOrder.StatusPembayaran == true
                       select new
                   {
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       TotalBayar = p.TotalBayar,
                   };

        if (Tipe == 1 || Tipe == 2)
        {
            o_pg = o_pg.Where(p => ((Lokasi > 0) ? p.SalesOrder.IDLokasi == Lokasi : 1 == 1) && p.SalesOrder.TipeLokasi == Tipe);
        }

        o_pg = o_pg.OrderByDescending(p => p.SalesOrder.Tgl);

        if (dari.Text != "" && sampai.Text != "")
        {
            o_pg = o_pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var r_pg = from p in db.ResellPG
                   where !(from z in db.TradeIn
                           where !z.Draft
                           select z.IDResell).Contains(p.IDForm)
                           && p.Resell.StatusVoid == null && !p.Resell.Draft
                   select new
                   {
                       Resell = p.Resell,
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       HargaRupiah = p.HargaRupiah
                   };

        var ro_pg = from p in db.ResellPG
                    where !(from z in db.TradeIn
                            select z.IDResell).Contains(p.IDForm)
                    && p.Resell.StatusVoid == null && !p.Resell.Draft

                    select new
                    {
                        Resell = p.Resell,
                        SalesOrder = p.SalesOrder,
                        ProductItem = DLC.ProductItem(),
                        ProductCategory = DLC.ProductCategory(),
                        ProductLevel = DLC.ProductLevel(),
                        ProductSegmen = DLC.ProductSegmen(),
                        HargaRupiah = p.HargaRupiah
                    };

        var rot_pg = from p in db.ResellPG
                     where !(from z in db.TradeIn
                             select z.IDResell).Contains(p.IDForm)
                     && p.Resell.StatusVoid == null && !p.Resell.Draft

                     select new
                     {
                         Resell = p.Resell,
                         SalesOrder = p.SalesOrder,
                         ProductItem = DLC.ProductItem(),
                         ProductCategory = DLC.ProductCategory(),
                         ProductLevel = DLC.ProductLevel(),
                         ProductSegmen = DLC.ProductSegmen(),
                         HargaRupiah = p.HargaRupiah
                     };

        if ((Tipe == 1 || Tipe == 2) && Lokasi > 0)
        {
            r_pg = r_pg.Where(p => ((Lokasi > 0) ? p.Resell.IDLokasi == Lokasi : 1 == 1) && p.Resell.TipeLokasi == Tipe);
            //ro_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi == Lokasi);
            //rot_pg = rot_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && (p.Resell.TipeLokasi != Tipe || p.Resell.IDLokasi != Lokasi));

            ro_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi == Lokasi);
            rot_pg = rot_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && (p.Resell.TipeLokasi != Tipe || p.Resell.IDLokasi != Lokasi));
        }
        //else
        //{
        //    r_pg = r_pg.Where(p => ((Lokasi > 0) ? p.Resell.IDLokasi == Lokasi : 1 == 1) && p.Resell.TipeLokasi == Tipe);
        //    ro_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
        //    rot_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
        //}

        r_pg = r_pg.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            r_pg = r_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));

        }

        var e_pg = from p in db.ResellPG
                   where (from z in db.TradeIn
                          where z.Tgl.Value < p.SalesOrder.Tgl.Value.AddDays(30)
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.StatusVoid == null && !p.Resell.Draft
                   select p;

        if (Tipe == 1 || Tipe == 2)
        {
            e_pg = e_pg.Where(p => ((Lokasi > 0) ? p.SalesOrder.IDLokasi == Lokasi : 1 == 1) && p.SalesOrder.TipeLokasi == Tipe);
        }

        e_pg = e_pg.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            e_pg = e_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var t_pg = from p in db.ResellPG
                   where (from z in db.TradeIn
                          where z.Draft == false
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.StatusVoid == null && !p.Resell.Draft
                   select new
                   {
                       Resell = p.Resell,
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       HargaRupiah = p.HargaRupiah
                   };

        if (Tipe == 1 || Tipe == 2)
        {
            t_pg = t_pg.Where(p => ((Lokasi > 0) ? p.Resell.IDLokasi == Lokasi : 1 == 1) && p.Resell.TipeLokasi == Tipe);
        }

        t_pg = t_pg.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            t_pg = t_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var o_gj = from p in db.SalesOrderGJ
                   where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                   p.SalesOrder.StatusVoid == false
                   && p.SalesOrder.Draft == false
                   && p.SalesOrder.StatusPembayaran == true
                   select p;

        if (Basic > 0)
        {
            o_gj = from p in o_gj
                   where (from x in db.StockBasic
                          select x.ProtoNomor).Contains(p.StockProductGJ.ProtoNomor)
                   select p;
        }

        if (Tipe == 1 || Tipe == 2)
        {
            o_gj = o_gj.Where(p => ((Lokasi > 0) ? p.SalesOrder.IDLokasi == Lokasi : 1 == 1) && p.SalesOrder.TipeLokasi == Tipe);
        }

        o_gj = o_gj.OrderByDescending(p => p.SalesOrder.Tgl);

        if (dari.Text != "" && sampai.Text != "")
        {
            o_gj = o_gj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var r_gj = from p in db.ResellGJ
                   where !(from z in db.TradeIn
                           select z.IDResell).Contains(p.IDForm)
                   && p.Resell.StatusVoid == null && p.Resell.Draft == false
                   select p;

        if (Basic > 0)
        {
            r_gj = from p in r_gj
                   where (from x in db.StockBasic
                          select x.ProtoNomor).Contains(p.StockProductGJ.ProtoNomor)
                   select p;
        }

        if (Tipe == 1 || Tipe == 2)
        {
            r_gj = r_gj.Where(p => ((Lokasi > 0) ? p.Resell.IDLokasi == Lokasi : 1 == 1) && p.Resell.TipeLokasi == Tipe);
        }

        r_gj = r_gj.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            r_gj = r_gj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var t_gj = from p in db.ResellGJ
                   where (from z in db.TradeIn
                          where z.Draft == false
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.StatusVoid == null && p.Resell.Draft == false
                   select p;

        if (Basic > 0)
        {
            t_gj = from p in t_gj
                   where (from x in db.StockBasic
                          select x.ProtoNomor).Contains(p.StockProductGJ.ProtoNomor)
                   select p;
        }

        if (Tipe == 1 || Tipe == 2)
        {
            t_gj = t_gj.Where(p => ((Lokasi > 0) ? p.Resell.IDLokasi == Lokasi : 1 == 1) && p.Resell.TipeLokasi == Tipe);
        }

        t_gj = t_gj.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            t_gj = t_gj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var o_ld = from p in db.SalesOrderLD
                   where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                   p.SalesOrder.StatusVoid == false && p.TotalBayar.HasValue
                   && p.SalesOrder.Draft == false
                   && p.SalesOrder.StatusPembayaran == true
                   select new
                   {
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       TotalBayar = p.TotalBayar,
                   };

        if (Tipe == 1 || Tipe == 2)
        {
            o_ld = o_ld.Where(p => ((Lokasi > 0) ? p.SalesOrder.IDLokasi == Lokasi : 1 == 1) && p.SalesOrder.TipeLokasi == Tipe);
        }

        o_ld = o_ld.OrderByDescending(p => p.SalesOrder.Tgl);

        if (dari.Text != "" && sampai.Text != "")
        {
            o_ld = o_ld.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var r_ld = from p in db.ResellLD
                   where !(from z in db.TradeIn
                           select z.IDResell).Contains(p.IDForm)
                   && p.Resell.Draft == false && p.HargaRupiah.HasValue
                   select new
                   {
                       Resell = p.Resell,
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       HargaRupiah = p.HargaRupiah
                   };

        if (Tipe == 1 || Tipe == 2)
        {
            r_ld = r_ld.Where(p => ((Lokasi > 0) ? p.Resell.IDLokasi == Lokasi : 1 == 1) && p.Resell.TipeLokasi == Tipe);
        }

        r_ld = r_ld.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            r_ld = r_ld.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var t_ld = from p in db.ResellLD
                   where (from z in db.TradeIn
                          where z.Draft == false
                          select z.IDResell).Contains(p.IDForm)
                          && p.Resell.StatusVoid == null && p.Resell.Draft == false
                   select new
                   {
                       Resell = p.Resell,
                       SalesOrder = p.SalesOrder,
                       ProductItem = DLC.ProductItem(),
                       ProductCategory = DLC.ProductCategory(),
                       ProductLevel = DLC.ProductLevel(),
                       ProductSegmen = DLC.ProductSegmen(),
                       HargaRupiah = p.HargaRupiah
                   };

        if (Tipe == 1 || Tipe == 2)
        {
            t_ld = t_ld.Where(p => ((Lokasi > 0) ? p.Resell.IDLokasi == Lokasi : 1 == 1) && p.Resell.TipeLokasi == Tipe);
        }

        t_ld = t_ld.OrderByDescending(p => p.SalesOrder.Tgl);


        if (dari.Text != "" && sampai.Text != "")
        {
            t_ld = t_ld.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (groupby.SelectedValue == "0")
        {

            if (1==1)
            {
                decimal NetOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;

                Qty = o_dj.Count() - r_dj.Count() - t_dj.Count();
                NetOmzet = p_ForceConvertToDecimal(o_dj.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(t_dj.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(r_dj.Sum(p => p.HargaRupiah));

                //Header
                TableRow tr_h = new TableRow();
                TableCell td_h;

                td_h = new TableCell();
                td_h.Text = "";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Jewellery";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_dj.Count().ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(o_dj.Sum(p => p.TotalBayar)).ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = t_dj.Count().ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(t_dj.Sum(p => p.HargaRupiah)).ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r_dj.Count().ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r_dj.Sum(p => p.HargaRupiah)).ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NetOmzet.ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalOmzetSales += p_ForceConvertToDecimal(o_dj.Sum(p => p.TotalBayar));
                TotalQtySales += p_ForceConvertToDecimal(o_dj.Count());
                TotalOmzetResell += p_ForceConvertToDecimal(r_dj.Sum(p => p.HargaRupiah));
                TotalQtyResell += p_ForceConvertToDecimal(r_dj.Count());
                TotalOmzetTradeIn += p_ForceConvertToDecimal(t_dj.Sum(p => p.HargaRupiah));
                TotalQtyTradeIn += p_ForceConvertToDecimal(t_dj.Count());

                TotalQtyAll += Qty;
                TotalOmzetAll += NetOmzet;
            }

            if (1==1)
            {
                decimal NetOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;
                decimal TradeIn = 0m;
                try
                {
                    TradeIn = p_ForceConvertToDecimal(t_pg.Sum(p => p.HargaRupiah));
                }
                catch (Exception ex) { }

                decimal Resell = 0m;
                try
                {
                    Resell = p_ForceConvertToDecimal(r_pg.Sum(p => p.HargaRupiah));
                }
                catch (Exception ex) { }

                Qty = o_pg.Count() - r_pg.Count() - t_pg.Count();
                //NetOmzet = p_ForceConvertToDecimal(o_pg.Sum(p => p.TotalBayar)) - TradeIn - Resell;
                NetOmzet = p_ForceConvertToDecimal(o_pg.Sum(p => p.TotalBayar));

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Plain Gold";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_pg.Count().ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(o_pg.Sum(p => p.TotalBayar)).ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = t_pg.Count().ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TradeIn.ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r_pg.Count().ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NetOmzet.ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalOmzetSales += p_ForceConvertToDecimal(o_pg.Sum(p => p.TotalBayar));
                TotalQtySales += p_ForceConvertToDecimal(o_pg.Count());
                TotalOmzetResell += Resell;
                TotalQtyResell += p_ForceConvertToDecimal(r_pg.Count());
                TotalOmzetTradeIn += TradeIn;
                TotalQtyTradeIn += p_ForceConvertToDecimal(t_pg.Count());

                TotalQtyAll += Qty;
                TotalOmzetAll += NetOmzet;
            }

            if (1==1)
            {
                decimal NetOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;

                Qty = o_gj.Count();
                NetOmzet = p_ForceConvertToDecimal(o_gj.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(t_gj.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(r_gj.Sum(p => p.HargaRupiah));

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Gold Jewellery";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_gj.Count().ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(o_gj.Sum(p => p.TotalBayar)).ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = t_gj.Count().ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(t_gj.Sum(p => p.HargaRupiah)).ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r_gj.Count().ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(r_gj.Sum(p => p.HargaRupiah)).ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NetOmzet.ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalOmzetSales += p_ForceConvertToDecimal(o_gj.Sum(p => p.TotalBayar));
                TotalQtySales += p_ForceConvertToDecimal(o_gj.Count());
                TotalOmzetResell += p_ForceConvertToDecimal(r_gj.Sum(p => p.HargaRupiah));
                TotalQtyResell += p_ForceConvertToDecimal(r_gj.Count());
                TotalOmzetTradeIn += p_ForceConvertToDecimal(t_gj.Sum(p => p.HargaRupiah));
                TotalQtyTradeIn += p_ForceConvertToDecimal(t_gj.Count());

                TotalQtyAll += Qty;
                TotalOmzetAll += NetOmzet;
            }

            if ( 1 == 1)
            {
                decimal NetOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;
                decimal TradeIn = 0m;
                try
                {
                    TradeIn = p_ForceConvertToDecimal(t_ld.Sum(p => p.HargaRupiah));
                }
                catch (Exception ex) { }

                decimal Resell = 0m;
                try
                {
                    Resell = p_ForceConvertToDecimal(r_ld.Sum(p => p.HargaRupiah));
                }
                catch (Exception ex) { }

                //Qty = o_ld.Count() - r_ld.Count() - t_ld.Count();
                Qty = o_ld.Count();
                //NetOmzet = p_ForceConvertToDecimal(o_ld.Sum(p => p.TotalBayar)) - TradeIn - Resell;
                NetOmzet = p_ForceConvertToDecimal(o_ld.Sum(p => p.TotalBayar));

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Loose Diamond";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_ld.Count().ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = p_ForceConvertToDecimal(o_ld.Sum(p => p.TotalBayar)).ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = t_ld.Count().ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TradeIn.ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r_ld.Count().ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NetOmzet.ToString("N0");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalOmzetSales += p_ForceConvertToDecimal(o_ld.Sum(p => p.TotalBayar));
                TotalQtySales += p_ForceConvertToDecimal(o_ld.Count());
                TotalOmzetResell += Resell;
                TotalQtyResell += p_ForceConvertToDecimal(r_ld.Count());
                TotalOmzetTradeIn += TradeIn;
                TotalQtyTradeIn += p_ForceConvertToDecimal(t_ld.Count());

                TotalQtyAll += Qty;
                TotalOmzetAll += NetOmzet;
            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
        else if (groupby.SelectedValue == "1")
        {
            var item = from p in db.CharProductItem
                       orderby p.Nama
                       select p;

            if ( 1 == 1)
            {
                //Header
                TableRow tr_h = new TableRow();
                TableCell td_h;

                td_h = new TableCell();
                td_h.Text = "";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                foreach (var r in item)
                {
                    decimal NetOmzet = 0m;
                    decimal TotalOmset = 0m;
                    decimal TotalResell = 0m;
                    decimal TotalTradeIn = 0m;
                    decimal Qty = 0m;
                    decimal QtyResell = 0m;
                    decimal QtyTradeIn = 0m;
                    int prev = 0;
                    int index = 1;

                    var d = from p in o_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                            select p;

                    var d2 = from p in o_pg
                             where p.ProductItem == r.ID
                             select p;

                    var d3 = from p in o_gj
                             where p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                             select p;

                    var d4 = from p in o_ld
                             where p.ProductItem == r.ID
                             select p;

                    var f = from p in t_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                            select p;

                    var f2 = from p in t_pg
                             where p.ProductItem == r.ID
                             select p;

                    var f3 = from p in t_gj
                            where p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                            select p;

                    var f4 = from p in t_ld
                             where p.ProductItem == r.ID
                             select p;

                    var g = from p in r_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                            select p;

                    var g2 = from p in r_pg
                             where p.ProductItem == r.ID
                             select p;

                    var g3 = from p in r_gj
                            where p.StockProductGJ.StockProductGJ_CharProduct.ProductItem == r.ID
                            select p;

                    var g4 = from p in r_ld
                             where p.ProductItem == r.ID
                             select p;

                    Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                    QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                    QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                    //NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah))- p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                    TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                    
                    if (Qty > 0 || QtyResell > 0 || QtyTradeIn > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = r.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalOmset.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyTradeIn.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalTradeIn.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyResell.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalResell.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = NetOmzet.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        tb2.Rows.Add(tr);

                        TotalOmzetSales += p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                        TotalQtySales += p_ForceConvertToDecimal(Qty);
                        //TotalOmzetTradeIn += p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah) + f2.Sum(p => p.HargaRupiah) + f3.Sum(p => p.HargaRupiah) + f4.Sum(p => p.HargaRupiah));
                        TotalOmzetTradeIn += TotalTradeIn;
                        TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                        //TotalOmzetResell += p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah) + g2.Sum(p => p.HargaRupiah) + g3.Sum(p => p.HargaRupiah) + g4.Sum(p => p.HargaRupiah));
                        TotalOmzetResell += TotalResell;
                        TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                        TotalQtyAll += Qty;
                        TotalOmzetAll += NetOmzet;
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
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }

        else if (groupby.SelectedValue == "2")
        {
            var item = ((from p in db.SalesOrder
                        orderby p.SalesNama
                        select new {
                            IDSales = p.IDSales
                        }).Union(
                        from p in db.Resell
                        orderby p.SalesNama
                        select new {
                            IDSales = p.IDSales
                        })).GroupBy(p => p.IDSales);

            if ( 1 == 1)
            {
                //Header
                TableRow tr_h = new TableRow();
                TableCell td_h;

                td_h = new TableCell();
                td_h.Text = "";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                foreach (var r in item)
                {
                    decimal NetOmzet = 0m;
                    decimal TotalOmset = 0m;
                    decimal TotalResell = 0m;
                    decimal TotalTradeIn = 0m;
                    decimal Qty = 0m;
                    decimal QtyResell = 0m;
                    decimal QtyTradeIn = 0m;
                    int prev = 0;
                    int index = 1;

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

                    //var f = from p in t_dj
                    //        where p.Resell.IDSales == r.Key
                    //        select p;

                    var f = from p in t_dj
                            join q in db.TradeIn on p.Resell.ID equals q.IDResell
                            where q.IDSales == r.Key
                            select p;

                    var f2 = from p in t_pg
                             join q in db.TradeIn on p.Resell.ID equals q.IDResell
                             where q.IDSales == r.Key
                             select p;

                    var f3 = from p in t_gj
                             join q in db.TradeIn on p.Resell.ID equals q.IDResell
                             where q.IDSales == r.Key
                             select p;

                    var f4 = from p in t_ld
                             join q in db.TradeIn on p.Resell.ID equals q.IDResell
                             where q.IDSales == r.Key
                             select p;

                    var g = from p in r_dj
                            where p.Resell.IDSales == r.Key
                            select p;

                    var g2 = from p in r_pg
                             where p.Resell.IDSales == r.Key
                             select p;

                    var g3 = from p in r_gj
                             where p.Resell.IDSales == r.Key
                             select p;

                    var g4 = from p in r_ld
                             where p.Resell.IDSales == r.Key
                             select p;

                    Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                    QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                    QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                    //NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah))- p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                    TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                    if (Qty > 0 || QtyResell > 0 || QtyTradeIn > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        var sn = db.DataSales.SingleOrDefault(p => p.ID == r.Key);

                        try
                        {
                            td = new TableCell();
                            td.Text = sn.Nama;
                            tr.Cells.Add(td);
                        }
                        catch (Exception ex)
                        {
                            td = new TableCell();
                            td.Text = "";
                            tr.Cells.Add(td);
                        }

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalOmset.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyTradeIn.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalTradeIn.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyResell.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalResell.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = NetOmzet.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        tb2.Rows.Add(tr);

                        TotalOmzetSales += p_ForceConvertToDecimal(TotalOmset);
                        TotalQtySales += p_ForceConvertToDecimal(Qty);
                        TotalOmzetTradeIn += p_ForceConvertToDecimal(TotalTradeIn);
                        TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                        TotalOmzetResell += p_ForceConvertToDecimal(TotalResell);
                        TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                        TotalQtyAll += Qty;
                        TotalOmzetAll += NetOmzet;
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
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }

        else if (groupby.SelectedValue == "3")
        {
            var sales = (from p in db.SalesOrder
                         where
                         p.StatusVoid == false
                         && p.Draft == false
                         && p.StatusPembayaran == true
                         select new
                         {
                             IDCustomer = p.IDCustomer.Value,
                             Tgl = p.Tgl.Value,
                             IDLokasi = p.IDLokasi.Value,
                             TipeLokasi = p.TipeLokasi.Value
                         }).Union
                        (from p in db.Resell
                         where
                         p.StatusVoid == null
                         && p.Draft == false
                         && !(from z in db.TradeIn
                              select z.IDResell).Contains(p.ID)
                         select new
                         {
                             IDCustomer = p.IDCustomer.Value,
                             Tgl = p.Tgl.Value,
                             IDLokasi = p.IDLokasi,
                             TipeLokasi = p.TipeLokasi
                         }).Union
                        (from p in db.Resell
                         where
                         p.StatusVoid == null
                         && p.Draft == false
                         && (from z in db.TradeIn
                             select z.IDResell).Contains(p.ID)
                         select new
                         {
                             IDCustomer = p.IDCustomer.Value,
                             Tgl = p.Tgl.Value,
                             IDLokasi = p.IDLokasi,
                             TipeLokasi = p.TipeLokasi
                         });

            if (dari.Text != "" && sampai.Text != "")
            {
                sales = sales.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            if (Tipe == 1 || Tipe == 2)
            {
                sales = sales.Where(p => p.IDLokasi == Lokasi && p.TipeLokasi == Tipe);
            }

            var item = (from p in db.DataCustomer
                        join q in sales on p.ID equals q.IDCustomer
                        orderby p.Nama
                        select new { ID = p.ID }).GroupBy(z => z.ID);
            
            if ( 1 == 1)
            {
                //Header
                TableRow tr_h = new TableRow();
                TableCell td_h;

                td_h = new TableCell();
                td_h.Text = "";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                foreach (var r in item)
                {
                    decimal NetOmzet = 0m;
                    decimal TotalOmset = 0m;
                    decimal TotalResell = 0m;
                    decimal TotalTradeIn = 0m;
                    decimal Qty = 0m;
                    decimal QtyResell = 0m;
                    decimal QtyTradeIn = 0m;
                    int prev = 0;
                    int index = 1;

                    var d = from p in o_dj
                            where p.SalesOrder.IDCustomer == r.Key
                            select p;

                    var d2 = from p in o_pg
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    var d3 = from p in o_gj
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    var d4 = from p in o_ld
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    var f = from p in t_dj
                            where p.SalesOrder.IDCustomer == r.Key
                            select p;

                    var f2 = from p in t_pg
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    var f3 = from p in t_gj
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    var f4 = from p in t_ld
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    var g = from p in r_dj
                            where p.SalesOrder.IDCustomer == r.Key
                            select p;

                    var g2 = from p in r_pg
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    var g3 = from p in r_gj
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    var g4 = from p in r_ld
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                    QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                    QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                    //NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah))- p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                    TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                    if (Qty > 0 || QtyResell > 0 || QtyTradeIn > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        var sn = db.DataCustomer.SingleOrDefault(p => p.ID == r.Key);

                        try
                        {
                            td = new TableCell();
                            td.Text = sn.Nama;
                            tr.Cells.Add(td);
                        }
                        catch (Exception ex)
                        {
                            td = new TableCell();
                            td.Text = "";
                            tr.Cells.Add(td);
                        }

                        td = new TableCell();
                        td.Text =   Qty.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalOmset.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyTradeIn.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalTradeIn.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyResell.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalResell.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = NetOmzet.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        tb2.Rows.Add(tr);

                        TotalOmzetSales += p_ForceConvertToDecimal(TotalOmset);
                        TotalQtySales += p_ForceConvertToDecimal(Qty);
                        TotalOmzetTradeIn += p_ForceConvertToDecimal(TotalTradeIn);
                        TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                        TotalOmzetResell += p_ForceConvertToDecimal(TotalResell);
                        TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                        TotalQtyAll += Qty;
                        TotalOmzetAll += NetOmzet;
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
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }

        else if (groupby.SelectedValue == "4")
        {
            var item = from p in db.CharProductCategory
                       orderby p.Nama
                       select p;

            if ( 1 == 1)
            {
                //Header
                TableRow tr_h = new TableRow();
                TableCell td_h;

                td_h = new TableCell();
                td_h.Text = "";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                foreach (var r in item)
                {
                    decimal NetOmzet = 0m;
                    decimal TotalOmset = 0m;
                    decimal TotalResell = 0m;
                    decimal TotalTradeIn = 0m;
                    decimal Qty = 0m;
                    decimal QtyResell = 0m;
                    decimal QtyTradeIn = 0m;
                    int prev = 0;
                    int index = 1;

                    var d = from p in o_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.ID
                            select p;

                    var d2 = from p in o_pg
                             where p.ProductCategory == r.ID
                             select p;

                    var d3 = from p in o_gj
                             where p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                             select p;

                    var d4 = from p in o_ld
                             where p.ProductCategory == r.ID
                             select p;

                    var f = from p in t_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.ID
                            select p;

                    var f2 = from p in t_pg
                             where p.ProductCategory == r.ID
                             select p;

                    var f3 = from p in t_gj
                             where p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                             select p;

                    var f4 = from p in t_ld
                             where p.ProductCategory == r.ID
                             select p;

                    var g = from p in r_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductCategory == r.ID
                            select p;

                    var g2 = from p in r_pg
                             where p.ProductCategory == r.ID
                             select p;

                    var g3 = from p in r_gj
                             where p.StockProductGJ.StockProductGJ_CharProduct.ProductCategory == r.ID
                             select p;

                    var g4 = from p in r_ld
                             where p.ProductCategory == r.ID
                             select p;

                    Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                    QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                    QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                    //NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah))- p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                    TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                    if (Qty > 0 || QtyResell > 0 || QtyTradeIn > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = r.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalOmset.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyTradeIn.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalTradeIn.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyResell.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalResell.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = NetOmzet.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        tb2.Rows.Add(tr);

                        TotalOmzetSales += p_ForceConvertToDecimal(TotalOmset);
                        TotalQtySales += p_ForceConvertToDecimal(Qty);
                        TotalOmzetTradeIn += p_ForceConvertToDecimal(TotalTradeIn);
                        TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                        TotalOmzetResell += p_ForceConvertToDecimal(TotalResell);
                        TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                        TotalQtyAll += Qty;
                        TotalOmzetAll += NetOmzet;
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
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }

        else if (groupby.SelectedValue == "5")
        {
            var item = from p in db.CharProductLevel
                       orderby p.Nama
                       select p;

            if (1 == 1)
            {
                //Header
                TableRow tr_h = new TableRow();
                TableCell td_h;

                td_h = new TableCell();
                td_h.Text = "";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                foreach (var r in item)
                {
                    decimal NetOmzet = 0m;
                    decimal TotalOmset = 0m;
                    decimal TotalResell = 0m;
                    decimal TotalTradeIn = 0m;
                    decimal Qty = 0m;
                    decimal QtyResell = 0m;
                    decimal QtyTradeIn = 0m;
                    int prev = 0;
                    int index = 1;

                    var d = from p in o_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.ID
                            select p;

                    var d2 = from p in o_pg
                             where p.ProductLevel == r.ID
                             select p;

                    var d3 = from p in o_gj
                             where p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                             select p;

                    var d4 = from p in o_ld
                             where p.ProductLevel == r.ID
                             select p;

                    var f = from p in t_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.ID
                            select p;

                    var f2 = from p in t_pg
                             where p.ProductLevel == r.ID
                             select p;

                    var f3 = from p in t_gj
                             where p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                             select p;

                    var f4 = from p in t_ld
                             where p.ProductLevel == r.ID
                             select p;

                    var g = from p in r_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductLevel == r.ID
                            select p;

                    var g2 = from p in r_pg
                             where p.ProductLevel == r.ID
                             select p;

                    var g3 = from p in r_gj
                             where p.StockProductGJ.StockProductGJ_CharProduct.ProductLevel == r.ID
                             select p;

                    var g4 = from p in r_ld
                             where p.ProductLevel == r.ID
                             select p;

                    Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                    QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                    QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                    //NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                    TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                    if (Qty > 0 || QtyResell > 0 || QtyTradeIn > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = r.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalOmset.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyTradeIn.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalTradeIn.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyResell.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalResell.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = NetOmzet.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        tb2.Rows.Add(tr);

                        TotalOmzetSales += p_ForceConvertToDecimal(TotalOmset);
                        TotalQtySales += p_ForceConvertToDecimal(Qty);
                        TotalOmzetTradeIn += p_ForceConvertToDecimal(TotalTradeIn);
                        TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                        TotalOmzetResell += p_ForceConvertToDecimal(TotalResell);
                        TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                        TotalQtyAll += Qty;
                        TotalOmzetAll += NetOmzet;
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
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }

        //Segmen
        else if (groupby.SelectedValue == "6")
        {
            var item = from p in db.CharProductSegmen
                       orderby p.Nama
                       select p;

            if (1 == 1)
            {
                //Header
                TableRow tr_h = new TableRow();
                TableCell td_h;

                td_h = new TableCell();
                td_h.Text = "";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                foreach (var r in item)
                {
                    decimal NetOmzet = 0m;
                    decimal TotalOmset = 0m;
                    decimal TotalResell = 0m;
                    decimal TotalTradeIn = 0m;
                    decimal Qty = 0m;
                    decimal QtyResell = 0m;
                    decimal QtyTradeIn = 0m;
                    int prev = 0;
                    int index = 1;

                    var d = from p in o_dj
                            where p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen == r.ID
                            select p;

                    var d2 = from p in o_pg
                             where p.ProductSegmen == r.ID
                             select p;

                    var d3 = from p in o_gj
                             where p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                             select p;

                    var d4 = from p in o_ld
                             where p.ProductSegmen == r.ID
                             select p;

                    var f = from p in t_dj
                            where p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen == r.ID
                            select p;

                    var f2 = from p in t_pg
                             where p.ProductSegmen == r.ID
                             select p;

                    var f3 = from p in t_gj
                             where p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                             select p;

                    var f4 = from p in t_ld
                             where p.ProductSegmen == r.ID
                             select p;

                    var g = from p in r_dj
                            where p.StockProductDJ.StockProductDJ_PricingSegmen.ProductSegmen == r.ID
                            select p;

                    var g2 = from p in r_pg
                             where p.ProductSegmen == r.ID
                             select p;

                    var g3 = from p in r_gj
                             where p.StockProductGJ.StockProductGJ_PricingSegmen.ProductSegmen == r.ID
                             select p;

                    var g4 = from p in r_ld
                             where p.ProductSegmen == r.ID
                             select p;

                    Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                    QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                    QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                    //NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                    TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                    if (Qty > 0 || QtyResell > 0 || QtyTradeIn > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = r.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalOmset.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyTradeIn.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalTradeIn.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyResell.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalResell.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = NetOmzet.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        tb2.Rows.Add(tr);

                        TotalOmzetSales += p_ForceConvertToDecimal(TotalOmset);
                        TotalQtySales += p_ForceConvertToDecimal(Qty);
                        TotalOmzetTradeIn += p_ForceConvertToDecimal(TotalTradeIn);
                        TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                        TotalOmzetResell += p_ForceConvertToDecimal(TotalResell);
                        TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                        TotalQtyAll += Qty;
                        TotalOmzetAll += NetOmzet;
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
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }


        //Outlet
        else if (groupby.SelectedValue == "7")
        {
            var item = (from p in db.LocOutlet
                        orderby p.Nama
                        select new { 
                        TipeLokasi = 1,
                        IDLokasi = p.ID,
                        Nama = p.Nama
                        }).Union(
                        from p in db.LocExhibition
                        orderby p.Nama
                        select new
                        {
                            TipeLokasi = 2,
                            IDLokasi = p.ID,
                            Nama = p.Nama
                        });

            if (1 == 1)
            {
                //Header
                TableRow tr_h = new TableRow();
                TableCell td_h;

                td_h = new TableCell();
                td_h.Text = "";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 70;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Net Omzet</b>";
                td_h.Width = 130;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                foreach (var r in item)
                {
                    decimal NetOmzet = 0m;
                    decimal TotalOmset = 0m;
                    decimal TotalResell = 0m;
                    decimal TotalTradeIn = 0m;
                    decimal Qty = 0m;
                    decimal QtyResell = 0m;
                    decimal QtyTradeIn = 0m;
                    int prev = 0;
                    int index = 1;

                    var d = from p in o_dj
                            where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                            select p;

                    var d2 = from p in o_pg
                             where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                             select p;

                    var d3 = from p in o_gj
                             where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                             select p;

                    var d4 = from p in o_ld
                             where p.SalesOrder.TipeLokasi == r.TipeLokasi && p.SalesOrder.IDLokasi == r.IDLokasi
                             select p;

                    var f = from p in t_dj
                            where p.Resell.TipeLokasi == r.TipeLokasi && p.Resell.IDLokasi == r.IDLokasi
                            select p;

                    var f2 = from p in t_pg
                             where p.Resell.TipeLokasi == r.TipeLokasi && p.Resell.IDLokasi == r.IDLokasi
                             select p;

                    var f3 = from p in t_gj
                             where p.Resell.TipeLokasi == r.TipeLokasi && p.Resell.IDLokasi == r.IDLokasi
                             select p;

                    var f4 = from p in t_ld
                             where p.Resell.TipeLokasi == r.TipeLokasi && p.Resell.IDLokasi == r.IDLokasi
                             select p;

                    var g = from p in r_dj
                            where p.Resell.TipeLokasi == r.TipeLokasi && p.Resell.IDLokasi == r.IDLokasi
                            select p;

                    var g2 = from p in r_pg
                             where p.Resell.TipeLokasi == r.TipeLokasi && p.Resell.IDLokasi == r.IDLokasi
                             select p;

                    var g3 = from p in r_gj
                             where p.Resell.TipeLokasi == r.TipeLokasi && p.Resell.IDLokasi == r.IDLokasi
                             select p;

                    var g4 = from p in r_ld
                             where p.Resell.TipeLokasi == r.TipeLokasi && p.Resell.IDLokasi == r.IDLokasi
                             select p;

                    Qty = d.Count() + d2.Count() + d3.Count() + d4.Count();
                    QtyResell = g.Count() + g2.Count() + g3.Count() + g4.Count();
                    QtyTradeIn = f.Count() + f2.Count() + f3.Count() + f4.Count();
                    ////NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    NetOmzet = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar)) - p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) - p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah));
                    TotalOmset = p_ForceConvertToDecimal(d.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d2.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d3.Sum(p => p.TotalBayar)) + p_ForceConvertToDecimal(d4.Sum(p => p.TotalBayar));
                    TotalResell = p_ForceConvertToDecimal(g.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(g4.Sum(p => p.HargaRupiah));
                    TotalTradeIn = p_ForceConvertToDecimal(f.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f2.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f3.Sum(p => p.HargaRupiah)) + p_ForceConvertToDecimal(f4.Sum(p => p.HargaRupiah));
                    if (Qty > 0 || QtyResell > 0 || QtyTradeIn > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = r.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalOmset.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyTradeIn.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalTradeIn.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = QtyResell.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = TotalResell.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = NetOmzet.ToString("N0");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        tb2.Rows.Add(tr);

                        TotalOmzetSales += p_ForceConvertToDecimal(TotalOmset);
                        TotalQtySales += p_ForceConvertToDecimal(Qty);
                        TotalOmzetTradeIn += p_ForceConvertToDecimal(TotalTradeIn);
                        TotalQtyTradeIn += p_ForceConvertToDecimal(QtyTradeIn);
                        TotalOmzetResell += p_ForceConvertToDecimal(TotalResell);
                        TotalQtyResell += p_ForceConvertToDecimal(QtyResell);


                        TotalQtyAll += Qty;
                        TotalOmzetAll += NetOmzet;
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
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            basic.SelectedValue = Request.QueryString["basic"];
            kw.Text = Request.QueryString["kw"];
            groupby.SelectedValue = Request.QueryString["groupby"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            tipelokasi.Text = Request.QueryString["tipelokasi"];
            lokasi.Text = Request.QueryString["lokasi"];
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
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;
        href += "&basic=" + basic.Text;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "Sales_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;
        href += "&basic=" + basic.Text;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "Sales_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;
        href += "&basic=" + basic.Text;
        Response.Redirect(href);
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            string t = "All";
            string v = "0";
            ListItem item = new ListItem(t, v);
            lokasi.Items.Add(item);
            DLC.LocationList(lokasi, Tipe);
        }
        lokasi.Focus();
    }
}