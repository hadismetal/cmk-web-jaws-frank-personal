using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_Product : System.Web.UI.Page
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
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            //Role Protection
            App.TableRole(tb, Role.Operator);

            //Generate
            fillFilter();

            if (App.Refresh(this)) 
                fillTable(sender, e);
        }

    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        var rs = from p in db.PromoGJ

                 where p.Draft == false && p.ApprovalNama != null
                 select p;

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => (p.TglStart.Value <= Convert.ToDateTime(dari.Text) && p.TglStart.Value <= Convert.ToDateTime(sampai.Text) && p.TglExpire.Value >= Convert.ToDateTime(dari.Text) && p.TglExpire.Value <= Convert.ToDateTime(sampai.Text))
                            || (p.TglStart.Value >= Convert.ToDateTime(dari.Text) && p.TglStart.Value <= Convert.ToDateTime(sampai.Text) && p.TglExpire.Value >= Convert.ToDateTime(dari.Text) && p.TglExpire.Value >= Convert.ToDateTime(sampai.Text))
                            || (p.TglStart.Value >= Convert.ToDateTime(dari.Text) && p.TglStart.Value <= Convert.ToDateTime(sampai.Text) && p.TglExpire.Value >= Convert.ToDateTime(dari.Text) && p.TglExpire.Value <= Convert.ToDateTime(sampai.Text))
                            || (p.TglStart.Value <= Convert.ToDateTime(dari.Text) && p.TglStart.Value <= Convert.ToDateTime(sampai.Text) && p.TglExpire.Value >= Convert.ToDateTime(dari.Text) && p.TglExpire.Value >= Convert.ToDateTime(sampai.Text))
                            );
        }
        if (kw.Text != "") { rs = rs.Where(p => p.PromoGJ_Detail.Any(z => z.StockProductGJ.Nomor.Contains(kw.Text))); }
        if (status.SelectedValue == "1") { rs = rs.Where(p => (p.StatusActive == null || p.StatusActive == true) && p.TglExpire > System.DateTime.Now); }
        else if (status.SelectedValue == "2") { rs = rs.Where(p => (p.StatusActive == null || p.StatusActive == true) && p.TglStart > System.DateTime.Now); }
        else if (status.SelectedValue == "3") { rs = rs.Where(p => p.StatusActive == false); }

        /// filter lokasi
        if (Convert.ToInt32(tipelokasi.SelectedValue) == 0)
        {
            rs = rs.Where(p => p.PromoGJ_Detail.Any(z => z.StockProductGJ.StockActualGJ.TipeLokasi.Equals(0)));
            if (lokasi.SelectedIndex > 0) { rs = rs.Where(p => p.PromoGJ_Detail.Any(z => z.StockProductGJ.StockActualGJ.IDLokasi.Equals(lokasi.SelectedValue))); }
        }
        else if (Convert.ToInt32(tipelokasi.SelectedValue) == 1)
        {
            rs = rs.Where(p => p.PromoGJ_Detail.Any(z => z.StockProductGJ.StockActualGJ.TipeLokasi.Equals(1)));
            if (lokasi.SelectedIndex > 0) { rs = rs.Where(p => p.PromoGJ_Detail.Any(z => z.StockProductGJ.StockActualGJ.IDLokasi.Equals(lokasi.SelectedValue))); }
        }
        else if (Convert.ToInt32(tipelokasi.SelectedValue) == 2)
        {
            rs = rs.Where(p => p.PromoGJ_Detail.Any(z => z.StockProductGJ.StockActualGJ.TipeLokasi.Equals(2)));
            if (lokasi.SelectedIndex > 0) { rs = rs.Where(p => p.PromoGJ_Detail.Any(z => z.StockProductGJ.StockActualGJ.IDLokasi.Equals(lokasi.SelectedValue))); }
        }
        
        rs = rs.OrderBy(p => p.TglExpire);

        //NO-RESULT
        zero.Visible = rs.Count() == 0;
        tbl_result.Visible = !zero.Visible;

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(r, "cancel");
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_HyperLink(r, "detail");
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r);
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.PromoGJ_Detail.Count().ToString(); // jml
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            if (r.StatusActive == false)
            {
                td.Text = "<span style=\"font-size:12px;\" class=\"label label-danger\">Cancel</span>";
            }
            else
            {
                if (r.TglStart > System.DateTime.Now)
                {
                    td.Text = "<span style=\"font-size:12px;\" class=\"label label-warning\">Upcoming</span>";
                }
                else if (Convert.ToDateTime(r.TglExpire).AddDays(1) < System.DateTime.Now)
                {
                    td.Text = "<span style=\"font-size:12px;\" class=\"label label-warning\">Expired</span>";
                }
                else
                {
                    td.Text = "<span style=\"font-size:12px;\" class=\"label label-success\">Active</span>";
                }
            }

            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TglStartExpire(r);
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DL.PromoStatus(r.Tipe.Value);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_PromoHarga(r);
            td.Style.Add("text-align", "right");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_DiscountNominal(r);
            td.Style.Add("text-align", "right");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_DiscountPersen(r);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Point(r);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);


            tb.Rows.Add(tr);
        }
    }

    protected string TD_HyperLink(PromoGJ r, string Mode)
    {
        /// refreshPage=1&kw=&dari=01-11-2015&sampai=09-11-2015&status=0&tipelokasi=-1&lokasi=-1

        string x = string.Empty;
        string href = string.Empty;

        if (Mode == "cancel")
        {
            //fast-backward
            if (r.StatusActive == null && r.TglStart < System.DateTime.Now && (Convert.ToDateTime(r.TglExpire).AddDays(1) >= System.DateTime.Now)
                )
            {
                href = "GJ_Expire.aspx?id=" + r.ID;
                href += "&kw=" + kw.Text;
                href += "&dari=" + dari.Text;
                href += "&sampai=" + sampai.Text;
                href += "&status=" + status.SelectedValue;
                href += "&tipelokasi=" + tipelokasi.SelectedValue;
                href += "&lokasi=" + lokasi.SelectedValue;

                x += "<a class=\"btn btn-warning\" href=\"" + href + "\">";
                x += "  <span class=\"glyphicon glyphicon-time\" aria-hidden=\"true\"> </span> ";
                x += "  ";
                x += "</a>";
            }
            else if (r.StatusActive == null && r.TglStart > System.DateTime.Now && (Convert.ToDateTime(r.TglExpire).AddDays(1) >= System.DateTime.Now))
            {
                href = "GJ_Cancel.aspx?id=" + r.ID;
                href += "&kw=" + kw.Text;
                href += "&dari=" + dari.Text;
                href += "&sampai=" + sampai.Text;
                href += "&status=" + status.SelectedValue;
                href += "&tipelokasi=" + tipelokasi.SelectedValue;
                href += "&lokasi=" + lokasi.SelectedValue;

                x += "<a class=\"btn btn-danger\" href=\"" + href + "\">";
                x += "  <i class=\"icon-remove\"></i> ";
                x += "  ";
                x += "</a>";
            }
        }

        if (Mode == "bandroll")
        {
            if (r.StatusActive != false)
            {
                if (r.TglExpire > System.DateTime.Now)
                {
                    href = "GJ_Print.aspx?id=" + r.ID;
                    href += "&kw=" + kw.Text;
                    href += "&dari=" + dari.Text;
                    href += "&sampai=" + sampai.Text;
                    href += "&status=" + status.SelectedValue;
                    href += "&tipelokasi=" + tipelokasi.SelectedValue;
                    href += "&lokasi=" + lokasi.SelectedValue;

                    x += "<a class=\"btn btn-primary\" href=\"" + href + "\">";
                    x += "  <i class=\"icon-tags\"></i> ";
                    x += "  ";
                    x += "</a>";
                }
            }
        }

        if (Mode == "detail")
        {
            href = "GJ_Detail.aspx?id=" + r.ID;
            href += "&kw=" + kw.Text;
            href += "&dari=" + dari.Text;
            href += "&sampai=" + sampai.Text;
            href += "&status=" + status.SelectedValue;
            href += "&tipelokasi=" + tipelokasi.SelectedValue;
            href += "&lokasi=" + lokasi.SelectedValue;

            x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  ";
            x += "</a>";
        }

        return x;
    }

    protected string TD_Nama(PromoGJ r)
    {
        string xnama = r.Nama;

        if (xnama.Trim().Length > 25)
        {
            if (!(xnama.Contains(" "))) { xnama = xnama.Substring(0, 25) + "..."; }
        }

        string x = string.Empty;

        x += "<div><strong>" + xnama + "</strong></div>";
        x += "<div><strong>" + r.OperatorNama + "</strong>" + ", " + Str.FullDate(r.OperatorTgl) + "</div>";

        return x;
    }

    protected string TD_DiscountPersen(PromoGJ r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += (r.DiscountPersen.HasValue) ? (r.DiscountPersen.Value != 0 ? Str.Number2(r.DiscountPersen.Value) : "") : "";
        x += "</p>";

        return x;
    }

    protected string TD_DiscountNominal(PromoGJ r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += (r.DiscountNominal.HasValue) ? (r.DiscountNominal.Value != 0 ? Str.Number0(r.DiscountNominal.Value) : "") : "";
        x += "</p>";

        return x;
    }
    protected string TD_Point(PromoGJ r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += (r.PoinReward.HasValue) ? (r.PoinReward.Value != 0 ? Str.Number2(r.PoinReward.Value) : "") : "";
        x += "</p>";

        return x;
    }
    protected string TD_PromoHarga(PromoGJ r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += (r.PromoHarga.HasValue) ? (r.PromoHarga.Value != 0 ? Str.Number0(r.PromoHarga.Value) : "") : "";
        x += "</p>";

        return x;
    }
    protected string TD_TglStartExpire(PromoGJ r)
    {
        DateTime myDate1 = Convert.ToDateTime(r.TglExpire);
        DateTime myDate2 = DateTime.Now;
        TimeSpan timeSpanResult;
        timeSpanResult = myDate1 - myDate2;
        double days = Math.Ceiling(timeSpanResult.TotalDays);
        string result = days.ToString("#,##0");
        string color = (days) < 0 ? "red" : "blue";

        string x = string.Empty;

        x += "<div>" + Str.FullDate(r.TglStart) + "</div>";
        x += "<div>" + Str.FullDate(r.TglExpire) + " <span style='color:" + color + ";'>(" + result + ")</span>" + "</div>";

        return x;
    }

    protected void fillFilter()
    {
        tipelokasi.Items.Clear();
        tipelokasi.Items.Add(new ListItem("All", "-1"));
        tipelokasi.Items.Add(new ListItem("Warehouse", "0"));
        tipelokasi.Items.Add(new ListItem("Outlet", "1"));
        tipelokasi.Items.Add(new ListItem("Exhibition", "2"));

        if (App.Refresh(this))
        {
            tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            DLC.LocationList(lokasi, Convert.ToInt32(Request.QueryString["tipelokasi"]));

            lokasi.SelectedValue = Request.QueryString["lokasi"];

            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            kw.Text = Request.QueryString["kw"];
            status.Text = Request.QueryString["status"];
        }
        else
        {
            lokasi.Items.Clear();
            lokasi.Items.Add(new ListItem("All", "-1"));

            dari.Text = Str.Tgl(DateDrv.AwalBulan());
            sampai.Text = Str.Tgl(DateTime.Now);
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        lokasi.Items.Clear();
        if (Tipe == 0 || Tipe == 1 || Tipe == 2) { DLC.LocationList(lokasi, Tipe); }
        else { lokasi.Items.Add(new ListItem("All", "-1")); }
        lokasi.Focus();
    }

}