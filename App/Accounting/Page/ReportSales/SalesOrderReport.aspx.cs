using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class SalesOrderReport : System.Web.UI.Page
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
    string Module = "acc"; // pos, acc

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

            dari.Text = Str.Tgl(DateTime.Today);
            sampai.Text = Str.Tgl(DateTime.Today);
            status.SelectedValue = "2";

            //Generate
            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);  
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.SalesOrder
                 where !(from z in db.SalesOrderDJ
                         where z.StockProductDJ.Legacy.Value == 1
                         select z.SalesOrder.ID).Contains(p.ID)
                         && !(from z in db.SalesOrderPG
                              where z.StockProductPG.Legacy.Value == 1
                              select z.SalesOrder.ID).Contains(p.ID)
                         && (p.SalesOrderDJ.Count() > 0 || p.SalesOrderGJ.Count() > 0 || p.SalesOrderLD.Count() > 0 || p.SalesOrderPG.Count() > 0)
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

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = kw.Text;
        if (keyword != "")
        {
            rs = from p in rs
                 where p.Nomor.Contains(keyword)
                        || (from q in db.SalesOrderDJ
                            where q.StockProductDJ.Nomor.Contains(kw.Text)
                            select q.IDForm).Contains(p.ID)
                        || (from q in db.SalesOrderGJ
                            where q.StockProductGJ.Nomor.Contains(kw.Text)
                            select q.IDForm).Contains(p.ID)
                        || (from q in db.SalesOrderPG
                            where q.StockProductPG.Nomor.Contains(kw.Text)
                            select q.IDForm).Contains(p.ID)
                        || (from q in db.SalesOrderLD
                            where q.StockProductLD_Stone1B.Nomor.Contains(kw.Text)
                            select q.IDForm).Contains(p.ID)
                        || p.CustomerNama.Contains(kw.Text)
                 select p;
        }
        
        if(status.SelectedValue=="1")
            rs = rs.Where(p=> p.StatusVoid == true);
        if (status.SelectedValue == "2")
            rs = rs.Where(p => p.StatusVoid == false);
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        if (rs.Count() > 0)
        {
            decimal TotalBayar = 0m;
            decimal TotalPaid = 0m;
            decimal TotalUnpaid = 0m;
            decimal TotalQty = 0m;

            string ti_nomor = "";
            string ti_keterangan = "";
            string ti_sisa_resell = "";

            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                ti_nomor = "";
                ti_keterangan = "";
                ti_sisa_resell = "";

                decimal nilaiResell = 0m;
                decimal nilaiHarusDibayar = 0m;

                if (r.TradeIn.HasValue)
                {
                    var ti = db.TradeIn.SingleOrDefault(p => p.IDSalesOrder == r.ID);
                    if (ti != null)
                    {
                        ti_nomor = "<br /><span style='color:blue;'>" + ti.Nomor + "</span>";
                        ti_keterangan = ti.Keterangan;

                        try
                        {
                            nilaiResell = r.TotalResell.HasValue ? r.TotalResell.Value : 0m;
                        }
                        catch (Exception) { }

                        try
                        {
                            nilaiHarusDibayar = r.TotalBayar.HasValue ? r.TotalBayar.Value : 0m;
                        }
                        catch (Exception) { }

                        ti_sisa_resell = "<br /><span style='color:blue;'>" + (nilaiHarusDibayar - nilaiResell).ToString("N0") + "</span>";
                    }
                }

                decimal Harga = 0m;
                decimal Paid = 0m;
                decimal Unpaid = 0m;
                int Qty = 0;

                Qty = r.SalesOrderDJ.Count() + r.SalesOrderPG.Count() + r.SalesOrderGJ.Count() + r.SalesOrderLD.Count() ;

                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                td.Style.Add("text-align","center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Nomor + ti_nomor;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDCustomer.HasValue ? r.CustomerNama : "";
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.IDSales.HasValue ? r.DataSales.Nama : "";
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Qty.ToString();
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                try
                {
                    Harga = Convert.ToDecimal(r.TotalBayar.Value);
                }
                catch (Exception) { }

                try
                {
                    Paid = Convert.ToDecimal(r.Paid.Value);
                }
                catch (Exception) { }

                try
                {
                    Unpaid = Convert.ToDecimal(r.Unpaid.Value);
                }
                catch (Exception) { }

                td = new TableCell();
                td.Text = Harga.ToString("N0") + ti_sisa_resell;
                td.Style.Add("text-align", "right");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Paid.ToString("N0");
                td.Style.Add("text-align", "right");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Unpaid.ToString("N0");
                td.Style.Add("text-align", "right");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.HtmlBreak(r.Keterangan) + Str.HtmlBreak(ti_keterangan);
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.KeteranganVoid != null ? Str.HtmlBreak(r.KeteranganVoid) : "";
                td.Style.Add("text-align", "left");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                TotalBayar += Harga;
                TotalUnpaid += Unpaid;
                TotalPaid += Paid;
                TotalQty += Qty;
            }

            TableRow tr2 = new TableRow();
            TableCell td2;

            td2 = new TableCell();
            td2.Text = "<b>Total</b>";
            td2.ColumnSpan = 6;
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalQty).ToString("N0") + "</b>";
            td2.Style.Add("text-align", "center");
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalBayar).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalPaid).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "<b>" + Convert.ToDecimal(TotalUnpaid).ToString("N0") + "</b>";
            tr2.Cells.Add(td2);

            td2 = new TableCell();
            td2.Text = "";
            td2.ColumnSpan = 2;
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


        Response.Redirect(href);
    }
    protected void Print(object sender, EventArgs e)
    {
        string href = "SalesOrderReport_Print.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;


        Response.Redirect(href);
    }
    protected string TD_HyperLink(SalesOrder r)
    {
        string x = string.Empty;

        if (!(r.StatusVoid == false && r.StatusPembayaran == false && r.Draft == true))
        {
            x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"SalesOrderReport_Detail.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  ";
            x += "</a>";
        }
		
		if (CheckTanggalLima(r))
		{
			if (r.StatusVoid == true && r.Draft == false && Payment.showVoidPaymentStatusGiftCard(r))
			{
				// Gift Card Payment Void Warning
				x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" href=\"SalesOrderReport_FormVoid.aspx?id=" + r.ID + "\">";
				x += "  <i class=\"icon-warning-sign\"></i> ";
				x += "  ";
				x += "</a>";
			}
			else if (VoidValidation.validVoidSalesOrderRole(r, Module))
			{
				x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Void Sales Order, Are you Sure?');\" href=\"SalesOrderReport_FormVoid.aspx?id=" + r.ID + "\">";
				x += "  <i class=\"icon-trash\"></i> ";
				x += "  ";
				x += "</a>";
			}
			else if (r.StatusVoid == false && r.StatusPembayaran == false && r.Draft == true)
			{
				x += "&nbsp;&nbsp;<a class=\"btn btn-warning\" onClick=\"return confirm('Cancel Sales Order, Are you Sure?');\" href=\"SalesOrderReport_Delete.aspx?id=" + r.ID + "\">";
				x += "  <i class=\"icon-remove\"></i> ";
				x += "  ";
				x += "</a>";
			}
		}
        return x;
    }
	
	protected bool CheckTanggalLima(SalesOrder r)
    {
        bool isactive = false;
        DateTime sodt = (r.Tgl.HasValue) ?  r.Tgl.Value : DateTime.Now.AddMonths(-2);
        DateTime now = DateTime.Now;

        if(now.Year > sodt.Year && now.AddYears(-1).Year == sodt.Year)
        {
            if(now.AddMonths(-1).Month == sodt.Month)
            {
                if(now.Day > 5)
                {
                    isactive = false;
                }
                else if(now.Day <= 5)
                {
                    isactive = true;
                }
            }
        }
        else if(now.Year == sodt.Year)
        {
            if(now.Month > sodt.Month)
            {
                if(now.Day > 5)
                {
                    isactive = false;
                }
                else if(now.Day <= 5)
                {
                    if(now.AddMonths(-1).Month == sodt.Month)
                    {
                        isactive = true;
                    }
                    else
                    {
                        isactive = false;
                    }
                }
            }
            else if(now.Month == sodt.Month)
            {
                isactive = true;
            }
        }
        return isactive;
    }
}