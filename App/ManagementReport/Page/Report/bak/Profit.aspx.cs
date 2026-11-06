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
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int Lokasi = Convert.ToInt32(lokasi.SelectedValue);


        decimal TotalJual = 0m;
        decimal TotalModal = 0m;
        decimal TotalProfit = 0m;
        decimal TotalQty = 0m;

        decimal Jual = 0m;
        decimal Modal = 0m;
        decimal Profit = 0m;
        decimal Qty = 0m;

        if (groupby.SelectedValue == "0")
        {

            var o_dj = from p in db.SalesOrderDJ
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false
                       && p.SalesOrder.Draft == false
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_dj = o_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_dj = o_dj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_dj = o_dj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            var o_pg = from p in db.SalesOrderPG
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false && p.TotalBayar.HasValue
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_pg = o_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_pg = o_pg.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_pg = o_pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
            }
            
            if (o_dj.Count() > 0)
            {
                
                int prev = 0;
                int index = 1;

                Qty = o_dj.Count();
                Jual = Convert.ToDecimal(o_dj.Sum(p => p.TotalBayar));
                Modal = Convert.ToDecimal(o_dj.Sum(p => p.ModalRupiah));
                Profit = Jual - Modal;

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Jewellery";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = o_dj.Count().ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Jual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Modal.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Profit.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalJual += Jual;
                TotalModal += Modal;
                TotalProfit += Profit;
                TotalQty += o_dj.Count();
            }

            if (o_pg.Count() > 0)
            {

                Qty = o_pg.Count();
                Jual = Convert.ToDecimal(o_pg.Sum(p => p.TotalBayar));
                Modal = Convert.ToDecimal(o_pg.Sum(p => p.ModalRupiah));
                Profit = Jual - Modal;

                int prev = 0;
                int index = 1;
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "Plain Gold";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Jual.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Modal.ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Profit.ToString("N0");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalJual += Jual;
                TotalModal += Modal;
                TotalProfit += Profit;
                TotalQty += o_pg.Count();
            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalJual.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalModal.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalProfit.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            tb.Rows.Add(tr_all);
        }
        else if (groupby.SelectedValue == "1")
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
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_dj = o_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_dj = o_dj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_dj = o_dj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            
            var o_pg = from p in db.SalesOrderPG
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false
                       && p.SalesOrder.Draft == false
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_pg = o_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_pg = o_pg.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_pg = o_pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            if (o_dj.Count() > 0)
            {

                foreach (var r in item)
                {
                    
                    int prev = 0;
                    int index = 1;

                    var d = from p in o_dj
                            where p.StockProductDJ.StockProductDJ_CharProduct.ProductItem == r.ID
                            select p;

                    var d2 = from p in o_pg
                             where p.StockProductPG.ProductItem == r.ID
                             select p;

                    Qty = d.Count() + d2.Count();
                    Jual = Convert.ToDecimal(d.Sum(p => p.TotalBayar)) + Convert.ToDecimal(d2.Sum(p => p.TotalBayar));
                    Modal = Convert.ToDecimal(d.Sum(p => p.ModalRupiah)) + Convert.ToDecimal(d2.Sum(p => p.ModalRupiah));
                    Profit = Jual - Modal;

                    if (d.Count() > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = r.Nama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Jual.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Modal.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Profit.ToString("N0");
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);

                        TotalJual += Jual;
                        TotalModal += Modal;
                        TotalProfit += Profit;
                        TotalQty += Qty;
                    }
                }

            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalJual.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalModal.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalProfit.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            tb.Rows.Add(tr_all);
        }

        else if (groupby.SelectedValue == "2")
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
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_dj = o_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_dj = o_dj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_dj = o_dj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            var o_pg = from p in db.SalesOrderPG
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false
                       && p.SalesOrder.Draft == false
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_pg = o_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_pg = o_pg.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_pg = o_pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            if (o_dj.Count() > 0)
            {

                foreach (var r in item)
                {
                    int prev = 0;
                    int index = 1;

                    var d = from p in o_dj
                            where p.SalesOrder.IDSales == r.Key
                            select p;

                    var d2 = from p in o_pg
                             where p.SalesOrder.IDSales == r.Key
                             select p;

                    Qty = d.Count() + d2.Count();
                    Jual = Convert.ToDecimal(d.Sum(p => p.TotalBayar)) + Convert.ToDecimal(d2.Sum(p => p.TotalBayar));
                    Modal = Convert.ToDecimal(d.Sum(p => p.ModalRupiah)) + Convert.ToDecimal(d2.Sum(p => p.ModalRupiah));
                    Profit = Jual - Modal;

                    if (d.Count() > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = d.First().SalesOrder.SalesNama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Jual.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Modal.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Profit.ToString("N0");
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);

                        TotalJual += Jual;
                        TotalModal += Modal;
                        TotalProfit += Profit;
                        TotalQty += Qty;
                    }
                }

            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalJual.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalModal.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalProfit.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            tb.Rows.Add(tr_all);
        }

        else if (groupby.SelectedValue == "3")
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
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_dj = o_dj.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_dj = o_dj.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_dj = o_dj.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            var o_pg = from p in db.SalesOrderPG
                       where /*!(from z in db.TradeIn
                               select z.IDSalesOrder).Contains(p.IDForm)
                       && p.StatusResell == 0 &&*/
                       p.SalesOrder.StatusVoid == false
                       && p.SalesOrder.Draft == false
                       select p;

            if (Tipe == 1 || Tipe == 2)
            {
                o_pg = o_pg.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
            }

            o_pg = o_pg.OrderByDescending(p => p.SalesOrder.Tgl);

            if (dari.Text != "" && sampai.Text != "")
            {
                o_pg = o_pg.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
            }

            if (o_dj.Count() > 0)
            {

                foreach (var r in item)
                {
                    int prev = 0;
                    int index = 1;

                    var d = from p in o_dj
                            where p.SalesOrder.IDCustomer == r.Key
                            select p;

                    var d2 = from p in o_pg
                             where p.SalesOrder.IDCustomer == r.Key
                             select p;

                    Qty = d.Count() + d2.Count();
                    Jual = Convert.ToDecimal(d.Sum(p => p.TotalBayar)) + Convert.ToDecimal(d2.Sum(p => p.TotalBayar));
                    Modal = Convert.ToDecimal(d.Sum(p => p.ModalRupiah)) + Convert.ToDecimal(d2.Sum(p => p.ModalRupiah));
                    Profit = Jual - Modal;

                    if (d.Count() > 0)
                    {

                        TableRow tr = new TableRow();
                        TableCell td;

                        td = new TableCell();
                        td.Text = d.First().SalesOrder.CustomerNama;
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Qty.ToString();
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Jual.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Modal.ToString("N0");
                        tr.Cells.Add(td);

                        td = new TableCell();
                        td.Text = Profit.ToString("N0");
                        tr.Cells.Add(td);

                        tb.Rows.Add(tr);

                        TotalJual += Jual;
                        TotalModal += Modal;
                        TotalProfit += Profit;
                        TotalQty += Qty;
                    }
                }

            }

            TableRow tr_all = new TableRow();
            TableCell td_all;

            td_all = new TableCell();
            td_all.Text = "";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalQty.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalJual.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalModal.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            td_all = new TableCell();
            td_all.Text = "<b>" + TotalProfit.ToString("N0") + "</b>";
            tr_all.Cells.Add(td_all);

            tb.Rows.Add(tr_all);
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
        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "Profit_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&groupby=" + groupby.SelectedValue;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;
        href += "&brand=" + brand.Text;

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
}