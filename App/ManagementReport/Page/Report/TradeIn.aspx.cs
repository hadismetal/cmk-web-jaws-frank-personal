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

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.Nomor == "" + keyword + "" || p.CustomerNama.Contains(kw.Text));

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal Total = 0m;
            decimal TotalQtyTradeIn = 0m;
            decimal TotalBerat = 0m;
            decimal TotalButir = 0m;
            decimal TotalCarat = 0m;

            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                decimal Harga = 0m;
                int QtyTradeIn = 0;


                QtyTradeIn = r.Resell.ResellDJ.Count() + r.Resell.ResellPG.Count();

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = r.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.CustomerNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = QtyTradeIn.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight).ToString("N2");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDecimal(r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty)).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneWeight).ToString("N3");
                tr.Cells.Add(td);

                try
                {
                    Harga = Convert.ToDecimal(r.Resell.ResellDJ.Sum(p => p.HargaRupiah)) + Convert.ToDecimal(r.Resell.ResellPG.Sum(p => p.HargaRupiah));
                }
                catch (Exception ex) { }

                td = new TableCell();
                td.Text = Harga.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                Total += Harga;
                TotalQtyTradeIn += QtyTradeIn;
                TotalBerat += r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.NettoWeight);
                TotalButir += Convert.ToDecimal(r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneQty));
                TotalCarat += r.Resell.ResellDJ.Sum(p => p.StockProductDJ.StockProductDJ_CharProduct.StoneWeight);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 6;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + TotalQtyTradeIn.ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBerat).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalButir).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalCarat).ToString("N3") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(Total).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            dari.Text = Request.QueryString["dari"];
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
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.Text;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;

        Response.Redirect(href);
    }

    protected string TD_HyperLink(TradeIn r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"TradeIn_Data.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";
        return x;
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
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "TradeIn_Export.aspx?id=1";
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        Response.Redirect(href);
    }
}