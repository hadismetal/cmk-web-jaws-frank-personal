using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Report_Omset_Export : System.Web.UI.Page
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
            Response.ContentType = "application/vnd.ms-excel";

            Response.AppendHeader("content-disposition", "attachment; filename=Omset_Export.xls");
            fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesOrderDJ
                 where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false
                 && !(from z in db.SalesOrderDJ
                      where z.StockProductDJ.Legacy.Value == 1
                      select z.SalesOrder.ID).Contains(p.ID)
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
        
        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            rs = rs.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
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
            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                decimal Butir = 0m;
                decimal Carat = 0m;
                int Qty = 1;

                Butir = r.StockProductDJ.StockProductDJ_Stone1A.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone1B.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone2.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone3.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone4.Sum(p => p.TotalButir) + r.StockProductDJ.StockProductDJ_Stone5.Sum(p => p.TotalButir);
                Carat = r.StockProductDJ.StockProductDJ_Stone1A.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone1B.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone2.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone3.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone4.Sum(p => p.TotalCarat) + r.StockProductDJ.StockProductDJ_Stone5.Sum(p => p.TotalCarat);
 
                TotalHarga += r.HargaRupiah.Value;
                TotalBayar += r.TotalBayar.Value;
                TotalDiscount += r.Discount.Value;
                TotalDiscountProgram += r.DiscountProgram.Value;
                TotalDiscountGift += r.DiscountGift.Value;
                TotalDiscountRound += r.DiscountRound.Value;
                TotalButir += Butir;
                TotalCarat += Carat;
                TotalNetWeight += r.StockProductDJ.StockProductDJ_CharProduct.NettoWeight;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Jewellery</b>";
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
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalCarat).ToString("N3").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalNetWeight).ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }

        //PG

        var pg = from p in db.SalesOrderPG
                 where  /*p.StatusResell == 0 &&*/ p.SalesOrder.StatusVoid == false && p.SalesOrder.Draft == false
                 select p;

        //check admin sales
        var s = from p in db.DataAdminSales
                 where p.IDUser == App.UserID
                 select p;

        if (Tipe == 1 || Tipe == 2)
        {
            pg = pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
        }

        if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
        {
            pg = pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
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

                TotalHarga += r.HargaRupiah.Value;
                TotalBayar += r.TotalBayar.Value;
                TotalDiscount += r.Discount.Value;
                TotalDiscountProgram += r.DiscountProgram.Value;
                TotalDiscountGift += r.DiscountGift.Value;
                TotalDiscountRound += r.DiscountRound.Value;
                TotalNetWeight += r.StockProductPG.GrossWeight.Value;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Plain Gold</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + pg.Count().ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
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
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar).ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>0</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>0</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalNetWeight).ToString("N2").Replace(".", "").Replace(",", ".") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }

        decimal TotalQtySales = 0m;
        decimal TotalOmzetSales = 0m;
        decimal TotalQtyResell = 0m;
        decimal TotalOmzetResell = 0m;
        decimal TotalQtyTradeIn = 0m;
        decimal TotalOmzetTradeIn = 0m;
        decimal TotalQtyExchange = 0m;
        decimal TotalOmzetExchange = 0m;
        decimal TotalQtyAll = 0m;
        decimal TotalOmzetAll = 0m;
        decimal TotalGrossOmset = 0m;
        decimal TotalOmsetResellOutlet = 0m;
        decimal TotalQtyResellOutlet = 0m;
        decimal TotalOmsetResellOther = 0m;
        decimal TotalQtyResellOther = 0m;

        if (Request.QueryString["groupby"] == "0")
        {

            var o_dj = from p in db.SalesOrderDJ
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false
                       && p.SalesOrder.Draft == false
                       && !(from z in db.SalesOrderDJ
                            where z.StockProductDJ.Legacy.Value == 1
                            select z.SalesOrder.ID).Contains(p.ID)
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_dj = o_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_dj = o_dj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                o_dj = o_dj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var r_dj = from p in db.ResellDJ
                       where !(from z in db.TradeIn
                               select z.IDResell).Contains(p.IDForm)
                       && p.Resell.Draft == false
                       select p;

            var ro_dj = from p in db.ResellDJ
                        where !(from z in db.TradeIn
                                select z.IDResell).Contains(p.IDForm)
                        && p.Resell.Draft == false
                        
                        select p;

            var rot_dj = from p in db.ResellDJ
                        where !(from z in db.TradeIn
                                select z.IDResell).Contains(p.IDForm)
                        && p.Resell.Draft == false

                        select p;

            if (Tipe == 1 || Tipe == 2)
            {
                r_dj = r_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);

                ro_dj = ro_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi == Lokasi);
                rot_dj = rot_dj.Where(p => p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi != Lokasi);

            }
            else
            {
                ro_dj = ro_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
                rot_dj = rot_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
            }

            //r_dj = r_dj.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                r_dj = r_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var e_dj = from p in db.ResellDJ
                       where (from z in db.TradeIn
                              where z.Tgl.Value <= p.SalesOrder.Tgl.Value.AddDays(30)
                              && z.Draft == false
                              select z.IDResell).Contains(p.IDForm)

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                e_dj = e_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            e_dj = e_dj.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                e_dj = e_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var t_dj = from p in db.ResellDJ
                       where (from z in db.TradeIn
                              where z.Tgl.Value > p.SalesOrder.Tgl.Value.AddDays(30)
                              && z.Draft == false
                              select z.IDResell).Contains(p.IDForm)

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                t_dj = t_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            t_dj = t_dj.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                t_dj = t_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var o_pg = from p in db.SalesOrderPG
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false && p.TotalBayar.HasValue
                       && p.SalesOrder.Draft == false
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_pg = o_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_pg = o_pg.OrderByDescending(p => p.SalesOrder.Tgl);

            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                o_pg = o_pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var r_pg = from p in db.ResellPG
                       where !(from z in db.TradeIn
                               select z.IDResell).Contains(p.IDForm)
                       && p.Resell.Draft == false && p.HargaRupiah.HasValue
                       select p;

            var ro_pg = from p in db.ResellPG
                        where !(from z in db.TradeIn
                                select z.IDResell).Contains(p.IDForm)
                        && p.Resell.Draft == false

                        select p;

            var rot_pg = from p in db.ResellPG
                         where !(from z in db.TradeIn
                                 select z.IDResell).Contains(p.IDForm)
                         && p.Resell.Draft == false

                         select p;

            if (Tipe == 1 || Tipe == 2)
            {
                r_pg = r_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);

                ro_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi == Lokasi);
                rot_pg = ro_pg.Where(p => p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi != Lokasi);

            }
            else
            {
                ro_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
                rot_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
            }

            r_pg = r_pg.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                r_pg = r_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var e_pg = from p in db.ResellPG
                       where (from z in db.TradeIn
                              where z.Tgl.Value < p.SalesOrder.Tgl.Value.AddDays(30)
                              select z.IDResell).Contains(p.IDForm)

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                e_pg = e_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            e_pg = e_pg.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                e_pg = e_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var t_pg = from p in db.ResellPG
                       where (from z in db.TradeIn
                              where z.Draft == false
                              select z.IDResell).Contains(p.IDForm) && p.HargaRupiah.HasValue

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                t_pg = t_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            t_pg = t_pg.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                t_pg = t_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            if (o_dj.Count() > 0)
            {
                decimal NetOmzet = 0m;
                decimal GrossOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;

                Qty = o_dj.Count();
                GrossOmzet = Convert.ToDecimal(o_dj.Sum(p => p.TotalBayar)) - Convert.ToDecimal(e_dj.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(t_dj.Sum(p => p.HargaRupiah));
                NetOmzet = Convert.ToDecimal(o_dj.Sum(p => p.TotalBayar)) - Convert.ToDecimal(e_dj.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(t_dj.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(r_dj.Sum(p => p.HargaRupiah));

                //Header
                TableRow tr_h = new TableRow();
                TableCell td_h;

                td_h = new TableCell();
                td_h.Text = "";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 160;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 150;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 150;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 150;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Jewellery";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_dj.Count().ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(o_dj.Sum(p => p.TotalBayar)).ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = e_dj.Count().ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(e_dj.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = t_dj.Count().ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(t_dj.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = GrossOmzet.ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r_dj.Count().ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r_dj.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rot_dj.Count().ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(rot_dj.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ro_dj.Count().ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(ro_dj.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NetOmzet.ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalOmzetSales += Convert.ToDecimal(o_dj.Sum(p => p.TotalBayar));
                TotalQtySales += Convert.ToDecimal(o_dj.Count());
                TotalOmzetResell += Convert.ToDecimal(r_dj.Sum(p => p.HargaRupiah));
                TotalQtyResell += Convert.ToDecimal(r_dj.Count());
                TotalOmzetExchange += Convert.ToDecimal(e_dj.Sum(p => p.HargaRupiah));
                TotalQtyExchange += Convert.ToDecimal(e_dj.Count());
                TotalOmzetTradeIn += Convert.ToDecimal(t_dj.Sum(p => p.HargaRupiah));
                TotalQtyTradeIn += Convert.ToDecimal(t_dj.Count());
                TotalOmsetResellOutlet += Convert.ToDecimal(ro_dj.Sum(p => p.HargaRupiah));
                TotalQtyResellOutlet += Convert.ToDecimal(ro_dj.Count());
                TotalOmsetResellOther += Convert.ToDecimal(rot_dj.Sum(p => p.HargaRupiah));
                TotalQtyResellOther += Convert.ToDecimal(rot_dj.Count());

                TotalQtyAll += Qty;
                TotalOmzetAll += NetOmzet;
                TotalGrossOmset += GrossOmzet;
            }

            if (o_pg.Count() > 0)
            {
                decimal NetOmzet = 0m;
                decimal GrossOmzet = 0m;
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;
                decimal Exchange = 0m;
                try
                {
                    Exchange = Convert.ToDecimal(e_pg.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }
                decimal TradeIn = 0m;
                try
                {
                    TradeIn = Convert.ToDecimal(t_pg.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                decimal Resell = 0m;
                try
                {
                    Resell = Convert.ToDecimal(r_pg.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                decimal ResellOther = 0m;
                try
                {
                    ResellOther = Convert.ToDecimal(rot_pg.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                decimal ResellOutlet = 0m;
                try
                {
                    ResellOutlet = Convert.ToDecimal(ro_pg.Sum(p => p.HargaRupiah.Value));
                }
                catch (Exception ex) { }

                Qty = o_pg.Count() - r_pg.Count() - t_pg.Count();
                GrossOmzet = Convert.ToDecimal(o_pg.Sum(p => p.TotalBayar.Value)) - Exchange - TradeIn;
                NetOmzet = Convert.ToDecimal(o_pg.Sum(p => p.TotalBayar.Value)) - Exchange - TradeIn - Resell;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Plain Gold";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_pg.Count().ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(o_pg.Sum(p => p.TotalBayar)).ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = e_pg.Count().ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Exchange.ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = t_pg.Count().ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TradeIn.ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = GrossOmzet.ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r_pg.Count().ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Resell.ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = rot_pg.Count().ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ResellOther.ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ro_pg.Count().ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = ResellOutlet.ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString().Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = NetOmzet.ToString("N0").Replace(".", "").Replace(",", ".");
                td.CssClass = "text-right";
                tr.Cells.Add(td);

                tb2.Rows.Add(tr);

                TotalOmzetSales += Convert.ToDecimal(o_pg.Sum(p => p.TotalBayar));
                TotalQtySales += Convert.ToDecimal(o_pg.Count());
                TotalOmzetResell += Resell;
                TotalQtyResell += Convert.ToDecimal(r_pg.Count());
                TotalOmzetExchange += Exchange;
                TotalQtyExchange += Convert.ToDecimal(e_pg.Count());
                TotalOmzetTradeIn += TradeIn;
                TotalQtyTradeIn += Convert.ToDecimal(t_pg.Count());
                TotalOmsetResellOutlet += Convert.ToDecimal(ro_pg.Sum(p => p.HargaRupiah));
                TotalQtyResellOutlet += Convert.ToDecimal(ro_pg.Count());
                TotalOmsetResellOther += Convert.ToDecimal(rot_pg.Sum(p => p.HargaRupiah));
                TotalQtyResellOther += Convert.ToDecimal(rot_pg.Count());

                TotalQtyAll += Qty;
                TotalOmzetAll += NetOmzet;
                TotalGrossOmset += GrossOmzet;
            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyExchange.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetExchange.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalGrossOmset.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOther.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOther.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOutlet.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOutlet.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
        else if (Request.QueryString["groupby"] == "1")
        {
            var item = from p in db.CharProductItem
                       orderby p.Nama
                       select p;

            var o_dj = from p in db.SalesOrderDJ
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false
                       && p.SalesOrder.Draft == false
                       && !(from z in db.SalesOrderDJ
                            where z.StockProductDJ.Legacy.Value == 1
                            select z.SalesOrder.ID).Contains(p.ID)
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_dj = o_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_dj = o_dj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                o_dj = o_dj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var r_dj = from p in db.ResellDJ
                       where !(from z in db.TradeIn
                               select z.IDResell).Contains(p.IDForm)
                       && p.Resell.Draft == false
                       select p;

            var ro_dj = from p in db.ResellDJ
                        where !(from z in db.TradeIn
                                select z.IDResell).Contains(p.IDForm)
                        && p.Resell.Draft == false

                        select p;

            var rot_dj = from p in db.ResellDJ
                         where !(from z in db.TradeIn
                                 select z.IDResell).Contains(p.IDForm)
                         && p.Resell.Draft == false

                         select p;

            if (Tipe == 1 || Tipe == 2)
            {
                r_dj = r_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);

                ro_dj = ro_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi == Lokasi);
                rot_dj = rot_dj.Where(p => p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi != Lokasi);

            }
            else
            {
                ro_dj = ro_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
                rot_dj = rot_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
            }

            //r_dj = r_dj.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                r_dj = r_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var e_dj = from p in db.ResellDJ
                       where (from z in db.TradeIn
                              where z.Tgl.Value <= p.SalesOrder.Tgl.Value.AddDays(30)
                              && z.Draft == false
                              select z.IDResell).Contains(p.IDForm)

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                e_dj = e_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            e_dj = e_dj.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                e_dj = e_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var t_dj = from p in db.ResellDJ
                       where (from z in db.TradeIn
                              where z.Tgl.Value > p.SalesOrder.Tgl.Value.AddDays(30)
                              && z.Draft == false
                              select z.IDResell).Contains(p.IDForm)

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                t_dj = t_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            t_dj = t_dj.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                t_dj = t_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var o_pg = from p in db.SalesOrderPG
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false && p.TotalBayar.HasValue
                       && p.SalesOrder.Draft == false
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_pg = o_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_pg = o_pg.OrderByDescending(p => p.SalesOrder.Tgl);

            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                o_pg = o_pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var r_pg = from p in db.ResellPG
                       where !(from z in db.TradeIn
                               select z.IDResell).Contains(p.IDForm)
                       && p.Resell.Draft == false && p.HargaRupiah.HasValue
                       select p;

            var ro_pg = from p in db.ResellPG
                        where !(from z in db.TradeIn
                                select z.IDResell).Contains(p.IDForm)
                        && p.Resell.Draft == false

                        select p;

            var rot_pg = from p in db.ResellPG
                         where !(from z in db.TradeIn
                                 select z.IDResell).Contains(p.IDForm)
                         && p.Resell.Draft == false

                         select p;

            if (Tipe == 1 || Tipe == 2)
            {
                r_pg = r_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);

                ro_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi == Lokasi);
                rot_pg = ro_pg.Where(p => p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi != Lokasi);

            }
            else
            {
                ro_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
                rot_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
            }

            r_pg = r_pg.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                r_pg = r_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var e_pg = from p in db.ResellPG
                       where (from z in db.TradeIn
                              where z.Tgl.Value < p.SalesOrder.Tgl.Value.AddDays(30)
                              select z.IDResell).Contains(p.IDForm)

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                e_pg = e_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            e_pg = e_pg.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                e_pg = e_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var t_pg = from p in db.ResellPG
                       where (from z in db.TradeIn
                              where z.Draft == false
                              select z.IDResell).Contains(p.IDForm) && p.HargaRupiah.HasValue

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                t_pg = t_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            t_pg = t_pg.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                t_pg = t_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
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
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 160;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                foreach (var r in item)
                {
                    decimal NetOmzet = 0m;
                    decimal GrossOmzet = 0m;
                    decimal Qty = 0m;
                    int prev = 0;
                    int index = 1;

                    var d = from p in o_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                            select p;

                    var d2 = from p in o_pg
                             where p.StockProductPG.ProductItem == r.ID
                             select p;

                    var f = from p in t_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                            select p;

                    var f2 = from p in t_pg
                             where p.StockProductPG.ProductItem == r.ID
                             select p;

                    var h = from p in e_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                            select p;

                    var h2 = from p in e_pg
                             where p.StockProductPG.ProductItem == r.ID
                             select p;

                    var g = from p in r_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                            select p;

                    var g2 = from p in r_pg
                             where p.StockProductPG.ProductItem == r.ID
                             select p;

                    var i = from p in rot_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                            select p;

                    var i2 = from p in rot_pg
                             where p.StockProductPG.ProductItem == r.ID
                             select p;

                    var j = from p in ro_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                            select p;

                    var j2 = from p in ro_pg
                             where p.StockProductPG.ProductItem == r.ID
                             select p;

                    Qty = d.Count() + d2.Count();
                    GrossOmzet = Convert.ToDecimal(d.Sum(p => p.TotalBayar)) + Convert.ToDecimal(d2.Sum(p => p.TotalBayar));
                    NetOmzet = Convert.ToDecimal(d.Sum(p => p.TotalBayar)) + Convert.ToDecimal(d2.Sum(p => p.TotalBayar)) - Convert.ToDecimal(f.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(f2.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(h.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(h2.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(g.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(g2.Sum(p => p.HargaRupiah));

                    if (Qty > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = r.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(d.Sum(p => p.TotalBayar)).ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = h.Count().ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(h.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = f.Count().ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(f.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = GrossOmzet.ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = g.Count().ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(g.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = i.Count().ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(i.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = j.Count().ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(j.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = NetOmzet.ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        tb2.Rows.Add(tr);

                        TotalOmzetSales += Convert.ToDecimal(d.Sum(p => p.TotalBayar));
                        TotalQtySales += Convert.ToDecimal(Qty);
                        TotalOmzetExchange += Convert.ToDecimal(h.Sum(p => p.HargaRupiah));
                        TotalQtyExchange += Convert.ToDecimal(h.Count());
                        TotalOmzetTradeIn += Convert.ToDecimal(f.Sum(p => p.HargaRupiah));
                        TotalQtyTradeIn += Convert.ToDecimal(f.Count());
                        TotalOmzetResell += Convert.ToDecimal(g.Sum(p => p.HargaRupiah));
                        TotalQtyResell += Convert.ToDecimal(g.Count());
                        TotalOmsetResellOther += Convert.ToDecimal(i.Sum(p => p.HargaRupiah));
                        TotalQtyResellOther += Convert.ToDecimal(i.Count());
                        TotalOmsetResellOutlet += Convert.ToDecimal(j.Sum(p => p.HargaRupiah));
                        TotalQtyResellOutlet += Convert.ToDecimal(j.Count());

                        TotalQtyAll += Qty;
                        TotalOmzetAll += NetOmzet;
                        TotalGrossOmset += GrossOmzet;
                    }
                }

            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyExchange.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetExchange.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalGrossOmset.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOther.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOther.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOutlet.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOutlet.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }

        else if (Request.QueryString["groupby"] == "2")
        {
            var item = (from p in db.SalesOrder
                        orderby p.SalesNama
                        select p).GroupBy(p => p.IDSales);

            var o_dj = from p in db.SalesOrderDJ
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false
                       && p.SalesOrder.Draft == false
                       && !(from z in db.SalesOrderDJ
                            where z.StockProductDJ.Legacy.Value == 1
                            select z.SalesOrder.ID).Contains(p.ID)
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_dj = o_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_dj = o_dj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                o_dj = o_dj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var r_dj = from p in db.ResellDJ
                       where !(from z in db.TradeIn
                               select z.IDResell).Contains(p.IDForm)
                       && p.Resell.Draft == false
                       select p;

            var ro_dj = from p in db.ResellDJ
                        where !(from z in db.TradeIn
                                select z.IDResell).Contains(p.IDForm)
                        && p.Resell.Draft == false

                        select p;

            var rot_dj = from p in db.ResellDJ
                         where !(from z in db.TradeIn
                                 select z.IDResell).Contains(p.IDForm)
                         && p.Resell.Draft == false

                         select p;

            if (Tipe == 1 || Tipe == 2)
            {
                r_dj = r_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);

                ro_dj = ro_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi == Lokasi);
                rot_dj = rot_dj.Where(p => p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi != Lokasi);

            }
            else
            {
                ro_dj = ro_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
                rot_dj = rot_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
            }

            //r_dj = r_dj.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                r_dj = r_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var e_dj = from p in db.ResellDJ
                       where (from z in db.TradeIn
                              where z.Tgl.Value <= p.SalesOrder.Tgl.Value.AddDays(30)
                              && z.Draft == false
                              select z.IDResell).Contains(p.IDForm)

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                e_dj = e_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            e_dj = e_dj.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                e_dj = e_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var t_dj = from p in db.ResellDJ
                       where (from z in db.TradeIn
                              where z.Tgl.Value > p.SalesOrder.Tgl.Value.AddDays(30)
                              && z.Draft == false
                              select z.IDResell).Contains(p.IDForm)

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                t_dj = t_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            t_dj = t_dj.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                t_dj = t_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var o_pg = from p in db.SalesOrderPG
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false && p.TotalBayar.HasValue
                       && p.SalesOrder.Draft == false
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_pg = o_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_pg = o_pg.OrderByDescending(p => p.SalesOrder.Tgl);

            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                o_pg = o_pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var r_pg = from p in db.ResellPG
                       where !(from z in db.TradeIn
                               select z.IDResell).Contains(p.IDForm)
                       && p.Resell.Draft == false && p.HargaRupiah.HasValue
                       select p;

            var ro_pg = from p in db.ResellPG
                        where !(from z in db.TradeIn
                                select z.IDResell).Contains(p.IDForm)
                        && p.Resell.Draft == false

                        select p;

            var rot_pg = from p in db.ResellPG
                         where !(from z in db.TradeIn
                                 select z.IDResell).Contains(p.IDForm)
                         && p.Resell.Draft == false

                         select p;

            if (Tipe == 1 || Tipe == 2)
            {
                r_pg = r_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);

                ro_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi == Lokasi);
                rot_pg = ro_pg.Where(p => p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi != Lokasi);

            }
            else
            {
                ro_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
                rot_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
            }

            r_pg = r_pg.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                r_pg = r_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var e_pg = from p in db.ResellPG
                       where (from z in db.TradeIn
                              where z.Tgl.Value < p.SalesOrder.Tgl.Value.AddDays(30)
                              select z.IDResell).Contains(p.IDForm)

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                e_pg = e_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            e_pg = e_pg.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                e_pg = e_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var t_pg = from p in db.ResellPG
                       where (from z in db.TradeIn
                              where z.Draft == false
                              select z.IDResell).Contains(p.IDForm) && p.HargaRupiah.HasValue

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                t_pg = t_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            t_pg = t_pg.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                t_pg = t_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
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
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 160;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                td_h.Width = 60;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                td_h.Width = 100;
                td_h.CssClass = "text-right";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                foreach (var r in item)
                {
                    decimal NetOmzet = 0m;
                    decimal GrossOmzet = 0m;
                    decimal Qty = 0m;
                    int prev = 0;
                    int index = 1;

                    var d = from p in o_dj
                            where p.SalesOrder.IDSales == r.Key
                            select p;

                    var d2 = from p in o_pg
                             where p.SalesOrder.IDSales == r.Key
                             select p;

                    var f = from p in t_dj
                            where p.SalesOrder.IDSales == r.Key
                            select p;

                    var f2 = from p in t_pg
                             where p.SalesOrder.IDSales == r.Key
                             select p;

                    var h = from p in e_dj
                            where p.SalesOrder.IDSales == r.Key
                            select p;

                    var h2 = from p in e_pg
                             where p.SalesOrder.IDSales == r.Key
                             select p;

                    var g = from p in r_dj
                            where p.SalesOrder.IDSales == r.Key
                            select p;

                    var g2 = from p in r_pg
                             where p.SalesOrder.IDSales == r.Key
                             select p;

                    var i = from p in rot_dj
                            where p.SalesOrder.IDSales == r.Key
                            select p;

                    var i2 = from p in rot_pg
                             where p.SalesOrder.IDSales == r.Key
                             select p;

                    var j = from p in ro_dj
                            where p.SalesOrder.IDSales == r.Key
                            select p;

                    var j2 = from p in ro_pg
                             where p.SalesOrder.IDSales == r.Key
                             select p;

                    Qty = d.Count() + d2.Count();
                    GrossOmzet = Convert.ToDecimal(d.Sum(p => p.TotalBayar)) + Convert.ToDecimal(d2.Sum(p => p.TotalBayar));
                    NetOmzet = Convert.ToDecimal(d.Sum(p => p.TotalBayar)) + Convert.ToDecimal(d2.Sum(p => p.TotalBayar)) - Convert.ToDecimal(f.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(f2.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(h.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(h2.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(g.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(g2.Sum(p => p.HargaRupiah));

                    if (Qty > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = r.First().SalesNama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(d.Sum(p => p.TotalBayar)).ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = h.Count().ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(h.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = f.Count().ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(f.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = GrossOmzet.ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = g.Count().ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(g.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = i.Count().ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(i.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = j.Count().ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(j.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString().Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = NetOmzet.ToString("N0").Replace(".", "").Replace(",", ".");
                        td.CssClass = "text-right";
                        tr.Cells.Add(td);

                        tb2.Rows.Add(tr);

                        TotalOmzetSales += Convert.ToDecimal(d.Sum(p => p.TotalBayar));
                        TotalQtySales += Convert.ToDecimal(Qty);
                        TotalOmzetExchange += Convert.ToDecimal(h.Sum(p => p.HargaRupiah));
                        TotalQtyExchange += Convert.ToDecimal(h.Count());
                        TotalOmzetTradeIn += Convert.ToDecimal(f.Sum(p => p.HargaRupiah));
                        TotalQtyTradeIn += Convert.ToDecimal(f.Count());
                        TotalOmzetResell += Convert.ToDecimal(g.Sum(p => p.HargaRupiah));
                        TotalQtyResell += Convert.ToDecimal(g.Count());
                        TotalOmsetResellOther += Convert.ToDecimal(i.Sum(p => p.HargaRupiah));
                        TotalQtyResellOther += Convert.ToDecimal(i.Count());
                        TotalOmsetResellOutlet += Convert.ToDecimal(j.Sum(p => p.HargaRupiah));
                        TotalQtyResellOutlet += Convert.ToDecimal(j.Count());

                        TotalQtyAll += Qty;
                        TotalOmzetAll += NetOmzet;
                        TotalGrossOmset += GrossOmzet;
                    }
                }

            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyExchange.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetExchange.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalGrossOmset.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOther.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOther.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOutlet.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOutlet.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            td_all.CssClass = "text-right";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }

        else if (Request.QueryString["groupby"] == "3")
        {
            var item = (from p in db.SalesOrder
                        orderby p.SalesNama
                        select p).GroupBy(p => p.IDCustomer);

            var o_dj = from p in db.SalesOrderDJ
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false
                       && p.SalesOrder.Draft == false
                       && !(from z in db.SalesOrderDJ
                            where z.StockProductDJ.Legacy.Value == 1
                            select z.SalesOrder.ID).Contains(p.ID)
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_dj = o_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_dj = o_dj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                o_dj = o_dj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var r_dj = from p in db.ResellDJ
                       where !(from z in db.TradeIn
                               select z.IDResell).Contains(p.IDForm)
                       && p.Resell.Draft == false
                       select p;

            var ro_dj = from p in db.ResellDJ
                        where !(from z in db.TradeIn
                                select z.IDResell).Contains(p.IDForm)
                        && p.Resell.Draft == false

                        select p;

            var rot_dj = from p in db.ResellDJ
                         where !(from z in db.TradeIn
                                 select z.IDResell).Contains(p.IDForm)
                         && p.Resell.Draft == false

                         select p;

            if (Tipe == 1 || Tipe == 2)
            {
                r_dj = r_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);

                ro_dj = ro_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi == Lokasi);
                rot_dj = rot_dj.Where(p => p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi != Lokasi);

            }
            else
            {
                ro_dj = ro_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
                rot_dj = rot_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
            }

            //r_dj = r_dj.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                r_dj = r_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var e_dj = from p in db.ResellDJ
                       where (from z in db.TradeIn
                              where z.Tgl.Value <= p.SalesOrder.Tgl.Value.AddDays(30)
                              && z.Draft == false
                              select z.IDResell).Contains(p.IDForm)

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                e_dj = e_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            e_dj = e_dj.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                e_dj = e_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var t_dj = from p in db.ResellDJ
                       where (from z in db.TradeIn
                              where z.Tgl.Value > p.SalesOrder.Tgl.Value.AddDays(30)
                              && z.Draft == false
                              select z.IDResell).Contains(p.IDForm)

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                t_dj = t_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            t_dj = t_dj.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                t_dj = t_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var o_pg = from p in db.SalesOrderPG
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false && p.TotalBayar.HasValue
                       && p.SalesOrder.Draft == false
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_pg = o_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_pg = o_pg.OrderByDescending(p => p.SalesOrder.Tgl);

            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                o_pg = o_pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.SalesOrder.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var r_pg = from p in db.ResellPG
                       where !(from z in db.TradeIn
                               select z.IDResell).Contains(p.IDForm)
                       && p.Resell.Draft == false && p.HargaRupiah.HasValue
                       select p;

            var ro_pg = from p in db.ResellPG
                        where !(from z in db.TradeIn
                                select z.IDResell).Contains(p.IDForm)
                        && p.Resell.Draft == false

                        select p;

            var rot_pg = from p in db.ResellPG
                         where !(from z in db.TradeIn
                                 select z.IDResell).Contains(p.IDForm)
                         && p.Resell.Draft == false

                         select p;

            if (Tipe == 1 || Tipe == 2)
            {
                r_pg = r_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);

                ro_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi == Lokasi);
                rot_pg = ro_pg.Where(p => p.Resell.TipeLokasi == Tipe && p.Resell.IDLokasi != Lokasi);

            }
            else
            {
                ro_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
                rot_pg = ro_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe && p.Resell.TipeLokasi == 0 && p.Resell.IDLokasi == 0);
            }

            r_pg = r_pg.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                r_pg = r_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var e_pg = from p in db.ResellPG
                       where (from z in db.TradeIn
                              where z.Tgl.Value < p.SalesOrder.Tgl.Value.AddDays(30)
                              select z.IDResell).Contains(p.IDForm)

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                e_pg = e_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            e_pg = e_pg.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                e_pg = e_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
            }

            var t_pg = from p in db.ResellPG
                       where (from z in db.TradeIn
                              where z.Draft == false
                              select z.IDResell).Contains(p.IDForm) && p.HargaRupiah.HasValue

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                t_pg = t_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            t_pg = t_pg.OrderByDescending(p => p.SalesOrder.Tgl);


            if (Request.QueryString["dari"] != "" && Request.QueryString["sampai"] != "")
            {
                t_pg = t_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(Request.QueryString["dari"]) && p.Resell.Tgl <= Convert.ToDateTime(Request.QueryString["sampai"]).AddDays(1));
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

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Qty</b>";
                tr_h.Cells.Add(td_h);

                td_h = new TableCell();
                td_h.Text = "<b>Total</b>";
                tr_h.Cells.Add(td_h);

                tb2.Rows.Add(tr_h);

                foreach (var r in item)
                {
                    decimal NetOmzet = 0m;
                    decimal GrossOmzet = 0m;
                    decimal Qty = 0m;
                    int prev = 0;
                    int index = 1;

                    var d = from p in o_dj
                            where p.SalesOrder.IDCustomer == r.Key
                            select p;

                    var d2 = from p in o_pg
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    var f = from p in t_dj
                            where p.SalesOrder.IDCustomer == r.Key
                            select p;

                    var f2 = from p in t_pg
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    var h = from p in e_dj
                            where p.SalesOrder.IDCustomer == r.Key
                            select p;

                    var h2 = from p in e_pg
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    var g = from p in r_dj
                            where p.SalesOrder.IDCustomer == r.Key
                            select p;

                    var g2 = from p in r_pg
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    var i = from p in rot_dj
                            where p.SalesOrder.IDCustomer == r.Key
                            select p;

                    var i2 = from p in rot_pg
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    var j = from p in ro_dj
                            where p.SalesOrder.IDCustomer == r.Key
                            select p;

                    var j2 = from p in ro_pg
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    Qty = d.Count() + d2.Count();
                    GrossOmzet = Convert.ToDecimal(d.Sum(p => p.TotalBayar)) + Convert.ToDecimal(d2.Sum(p => p.TotalBayar));
                    NetOmzet = Convert.ToDecimal(d.Sum(p => p.TotalBayar)) + Convert.ToDecimal(d2.Sum(p => p.TotalBayar)) - Convert.ToDecimal(f.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(f2.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(h.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(h2.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(g.Sum(p => p.HargaRupiah)) - Convert.ToDecimal(g2.Sum(p => p.HargaRupiah));

                    if (Qty > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = r.First().CustomerNama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString().Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(d.Sum(p => p.TotalBayar)).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = h.Count().ToString().Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(h.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = f.Count().ToString().Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(f.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = GrossOmzet.ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = g.Count().ToString().Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(g.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = i.Count().ToString().Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(i.Sum(p => p.HargaRupiah)).ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = j.Count().ToString().Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Convert.ToDecimal(j.Sum(p => p.HargaRupiah)).ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString().Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = NetOmzet.ToString("N0").Replace(".", "").Replace(",", ".");
                        tr.Cells.Add(td);

                        tb2.Rows.Add(tr);

                        TotalOmzetSales += Convert.ToDecimal(d.Sum(p => p.TotalBayar));
                        TotalQtySales += Convert.ToDecimal(Qty);
                        TotalOmzetExchange += Convert.ToDecimal(h.Sum(p => p.HargaRupiah));
                        TotalQtyExchange += Convert.ToDecimal(h.Count());
                        TotalOmzetTradeIn += Convert.ToDecimal(f.Sum(p => p.HargaRupiah));
                        TotalQtyTradeIn += Convert.ToDecimal(f.Count());
                        TotalOmzetResell += Convert.ToDecimal(g.Sum(p => p.HargaRupiah));
                        TotalQtyResell += Convert.ToDecimal(g.Count());
                        TotalOmsetResellOther += Convert.ToDecimal(i.Sum(p => p.HargaRupiah));
                        TotalQtyResellOther += Convert.ToDecimal(i.Count());
                        TotalOmsetResellOutlet += Convert.ToDecimal(j.Sum(p => p.HargaRupiah));
                        TotalQtyResellOutlet += Convert.ToDecimal(j.Count());

                        TotalQtyAll += Qty;
                        TotalOmzetAll += NetOmzet;
                        TotalGrossOmset += GrossOmzet;
                    }
                }

            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtySales.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetSales.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyExchange.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetExchange.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyTradeIn.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetTradeIn.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalGrossOmset.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResell.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetResell.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOther.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOther.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyResellOutlet.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmsetResellOutlet.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQtyAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalOmzetAll.ToString("N0").Replace(".", "").Replace(",", ".") + "</b>";
            tr_all.Cells.Add(td_all);

            tb2.Rows.Add(tr_all);
        }
    }
}