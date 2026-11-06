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

    //private decimal p_ForceConvertToDecimal(object objValue)
    //{
    //    decimal decReturn = decimal.Zero;

    //    try
    //    {
    //        decReturn = Convert.ToDecimal(objValue);
    //    }
    //    catch
    //    {
    //        //  do nothing.
    //        //  return 0;
    //    }

    //    return decReturn;
    //}

    //private string p_ForceConvertToString(object objValue)
    //{
    //    string szReturn = string.Empty;

    //    try
    //    {
    //        szReturn = Convert.ToString(objValue);
    //    }
    //    catch
    //    {
    //        //  do nothing.
    //        //  return 0;
    //    }

    //    return szReturn;
    //}

    //Display Table

    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesOrderPG
                 where p.SalesOrder.StatusPembayaran == true
                 select p;

        //check admin sales
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        int Lokasi = Convert.ToInt32(lokasi.SelectedValue);

        if (Tipe == 1 || Tipe == 2)
        {
            rs = rs.Where(p => p.SalesOrder.IDLokasi == Lokasi && p.SalesOrder.TipeLokasi == Tipe);
        }

        rs = rs.OrderByDescending(p => p.SalesOrder.Tgl);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.SalesOrder.Nomor == "" + keyword + "" || p.SalesOrder.CustomerNama.Contains(kw.Text) || p.Invoice.Contains(kw.Text));
        
        if(status.SelectedValue=="1")
            rs = rs.Where(p=> p.SalesOrder.StatusVoid == true);

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.SalesOrder.Tgl >= Convert.ToDateTime(dari.Text) && p.SalesOrder.Tgl <= Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalHarga = 0m;
            decimal TotalDiscount = 0m;
            decimal TotalDiscountProgram = 0m;
            decimal TotalDiscountGift = 0m;
            decimal TotalDiscountRound = 0m;
            decimal TotalNetWeight = 0m;
            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                int Qty = 1;

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.FullDate(r.SalesOrder.Tgl);
                tr.Cells.Add(td);
                
                td = new TableCell();
                td.Text = r.Invoice;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.Nomor;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.SalesOrder.CustomerNama;
                tr.Cells.Add(td);

                DataSales dat = r.SalesOrder.DataSales;
                string szSalesNm = string.Empty;
                if (dat != null)
                {
                    if (dat.Nama != null)
                        szSalesNm = dat.Nama;
                }

                td = new TableCell();
                td.Text = szSalesNm; // r.SalesOrder.DataSales.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.SupplierNama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.SupplierSJ;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.KodeBarang;
                tr.Cells.Add(td);
                
                td = new TableCell();
                td.Text = Qty.ToString();
                tr.Cells.Add(td);                

                td = new TableCell();
                td.Text = r.TotalBayar.Value.ToString("N0"); // p_ForceConvertToDecimal(r.TotalBayar).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Discount.Value.ToString("N0") + "%"; // p_ForceConvertToDecimal(r.Discount).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountProgram.Value.ToString("N0") + "%"; // p_ForceConvertToDecimal(r.DiscountProgram).ToString("N0") + "%";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountGift.Value.ToString("N0"); // p_ForceConvertToDecimal(r.DiscountGift).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.DiscountRound.Value.ToString("N0"); // p_ForceConvertToDecimal(r.DiscountRound).ToString("N0");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.StockProductPG.GrossWeight.Value.ToString("N2"); // p_ForceConvertToDecimal(r.StockProductPG.GrossWeight).ToString("N2");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalHarga += r.TotalBayar.Value;
                TotalDiscount += r.Discount.Value;
                TotalDiscountProgram += r.DiscountProgram.Value;
                TotalDiscountGift += r.DiscountGift.Value;
                TotalDiscountRound += r.DiscountRound.Value;
                TotalNetWeight += r.StockProductPG.GrossWeight.Value;

                //TotalHarga += p_ForceConvertToDecimal(r.TotalBayar);
                //TotalDiscount += p_ForceConvertToDecimal(r.Discount);
                //TotalDiscountProgram += p_ForceConvertToDecimal(r.DiscountProgram);
                //TotalDiscountGift += p_ForceConvertToDecimal(r.DiscountGift);
                //TotalDiscountRound += p_ForceConvertToDecimal(r.DiscountRound);
                //TotalNetWeight += p_ForceConvertToDecimal(r.StockProductPG.GrossWeight);
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 10;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + rs.Count().ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalHarga).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscount/rs.Count()).ToString("N0") + " %</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountProgram / rs.Count()).ToString("N0") + " %</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountGift).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalDiscountRound).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalNetWeight).ToString("N2") + "</b>";
            tr2.Cells.Add(td2);

            tb.Rows.Add(tr2);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            
            status.SelectedValue = Request.QueryString["status"];
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
        
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;

        Response.Redirect(href);
    }

    protected void Export(object sender, EventArgs e)
    {
        string href = "InvoicePG_Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;

        Response.Redirect(href);
    }

    protected void Print(object sender, EventArgs e)
    {
        string href = "InvoicePG_Print.aspx?id=1";

        href += "&kw=" + kw.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&tipelokasi=" + tipelokasi.Text;
        href += "&lokasi=" + lokasi.Text;
        Response.Redirect(href);
    }

    protected string TD_HyperLink(SalesOrder r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Sales_Detail.aspx?id=" + r.ID + "\">";
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
}