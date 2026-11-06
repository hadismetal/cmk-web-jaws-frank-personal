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

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            status.SelectedValue = "1";
            int Tipe = 1;
            if (tipelokasi.SelectedValue != "")
                Tipe = Convert.ToInt32(tipelokasi.SelectedValue);

            DLC.LocationList(lokasi, 1);
            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.TradeIn

                 select p;

        string Status = "";
        Status = Request.QueryString["status"];
        if (Status == "1")
            rs = rs.Where(p => p.Draft == false);
        else if (Status == "2")
            rs = rs.Where(p => p.Draft == true);

        //check admin sales
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int Lokasi = Convert.ToInt32(lokasi.SelectedValue);

        if ((Tipe == 1 || Tipe == 2) && Lokasi > 0)
        {
            rs = rs.Where(p => p.IDLokasi == Lokasi && p.TipeLokasi == Tipe);
        }

        rs = rs.OrderByDescending(p => p.Tgl);

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (groupby.SelectedValue == "0")
        {
            var t_dj = from p in db.ResellDJ
                       where (from z in db.TradeIn
                              where z.Draft == false
                              select z.IDResell).Contains(p.IDForm)

                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                t_dj = t_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            t_dj = t_dj.OrderByDescending(p => p.SalesOrder.Tgl);


            if (dari.Text != "" && sampai.Text != "")
            {
                t_dj = t_dj.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
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


            if (dari.Text != "" && sampai.Text != "")
            {
                t_pg = t_pg.Where(p => p.Resell.Tgl >= Convert.ToDateTime(dari.Text) && p.Resell.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            if (t_dj.Count() > 0)
            {
                decimal Qty = 0m;
                int prev = 0;
                int index = 1;

                Qty = t_dj.Count();

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Jewellery";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = t_dj.Count().ToString();
                tr.Cells.Add(td);

                decimal buyback = t_dj.Sum(p => p.HargaRupiah.Value);

                td = new TableCell();
                td.Text = Convert.ToDecimal(buyback).ToString("N0");
                tr.Cells.Add(td);

                decimal hrg = (from p in t_dj
                           select p.StockProductDJ.StockProductDJ_PricingMU.HargaRupiahRound.Value).Sum();

                td = new TableCell();
                td.Text = Convert.ToDecimal(hrg).ToString("N0");
                tr.Cells.Add(td);

                decimal depr = 0m;

                depr = (buyback/hrg)*100;

                td = new TableCell();
                td.Text = Convert.ToDecimal(depr).ToString("N2") + "%";
                tr.Cells.Add(td);

                decimal gross = 0m;
                gross = (from p in t_dj
                         select p.StockProductDJ.StockProductDJ_CharProduct.GrossWeight).Sum();

                td = new TableCell();
                td.Text = Convert.ToDecimal(gross).ToString("N2");
                tr.Cells.Add(td);

                decimal net = 0m;
                net = (from p in t_dj
                         select p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight).Sum();

                td = new TableCell();
                td.Text = Convert.ToDecimal(net).ToString("N2");
                tr.Cells.Add(td);

                decimal stoneqty = 0m;
                stoneqty = (from p in t_dj
                         select p.StockProductDJ.StockProductDJ_CharProduct.StoneQty.Value).Sum();

                td = new TableCell();
                td.Text = Convert.ToDecimal(gross).ToString("N0");
                tr.Cells.Add(td);

                decimal carat = 0m;
                carat = (from p in t_dj
                         select p.StockProductDJ.StockProductDJ_CharProduct.StoneCarat).Sum();

                td = new TableCell();
                td.Text = Convert.ToDecimal(carat).ToString("N3");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            dari.Text = Request.QueryString["dari"];
            groupby.SelectedValue = Request.QueryString["groupby"];
            sampai.Text = Request.QueryString["sampai"];
            status.Text = Request.QueryString["status"];
            tipelokasi.Text = Request.QueryString["tipelokasi"];
            lokasi.Text = Request.QueryString["lokasi"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&dari=" + dari.Text;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.Text;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;

        Response.Redirect(href);
    }
    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        if (Tipe == 1 || Tipe == 2)
        {
            lokasi.Items.Clear();
            DLC.LocationList(lokasi, Tipe);
        }
        lokasi.Focus();
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "TradeIn_Print.aspx?id=1";
        href += "&groupby=" + groupby.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        Response.Redirect(href);
    }
}