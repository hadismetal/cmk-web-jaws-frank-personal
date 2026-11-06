using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class GJ_List : System.Web.UI.Page
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
    EncryptDecrypt enc = new EncryptDecrypt();

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
        var rs = from p in db.PromoGJ_Detail
                 where p.PromoGJ.Draft == false && p.PromoGJ.ApprovalNama != null && p.PromoGJ.ApprovalTgl != null
                 select p;

        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => (p.PromoGJ.TglStart.Value <= Convert.ToDateTime(dari.Text) && p.PromoGJ.TglStart.Value <= Convert.ToDateTime(sampai.Text) && p.PromoGJ.TglExpire.Value >= Convert.ToDateTime(dari.Text) && p.PromoGJ.TglExpire.Value <= Convert.ToDateTime(sampai.Text))
                            || (p.PromoGJ.TglStart.Value >= Convert.ToDateTime(dari.Text) && p.PromoGJ.TglStart.Value <= Convert.ToDateTime(sampai.Text) && p.PromoGJ.TglExpire.Value >= Convert.ToDateTime(dari.Text) && p.PromoGJ.TglExpire.Value >= Convert.ToDateTime(sampai.Text))
                            || (p.PromoGJ.TglStart.Value >= Convert.ToDateTime(dari.Text) && p.PromoGJ.TglStart.Value <= Convert.ToDateTime(sampai.Text) && p.PromoGJ.TglExpire.Value >= Convert.ToDateTime(dari.Text) && p.PromoGJ.TglExpire.Value <= Convert.ToDateTime(sampai.Text))
                            || (p.PromoGJ.TglStart.Value <= Convert.ToDateTime(dari.Text) && p.PromoGJ.TglStart.Value <= Convert.ToDateTime(sampai.Text) && p.PromoGJ.TglExpire.Value >= Convert.ToDateTime(dari.Text) && p.PromoGJ.TglExpire.Value >= Convert.ToDateTime(sampai.Text))
                            );
        }
        if (kw.Text != "")
            rs = rs.Where(p => p.StockProductGJ.Nomor == (kw.Text));
        if (status.SelectedValue == "1")
            rs = rs.Where(p => (p.PromoGJ.StatusActive == null || p.PromoGJ.StatusActive == true) && p.StockProductGJ.StatusPenjualan == 0);
        else if (status.SelectedValue == "2")
            rs = rs.Where(p => p.StockProductGJ.StatusPenjualan == 2);
        else if (status.SelectedValue == "3")
            rs = rs.Where(p => p.PromoGJ.TglStart > System.DateTime.Now && (p.PromoGJ.StatusActive == null || p.PromoGJ.StatusActive == true) && p.StockProductGJ.StatusPenjualan == 0);
        else if (status.SelectedValue == "4")
            rs = rs.Where(p => p.PromoGJ.StatusActive == false);

        /// filter lokasi
        if (Convert.ToInt32(tipelokasi.SelectedValue) == 0)
        {
            rs = rs.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi.Equals(0));
            if (lokasi.SelectedIndex > 0) { rs = rs.Where(p => p.StockProductGJ.StockActualGJ.IDLokasi.Equals(lokasi.SelectedValue)); }
        }
        else if (Convert.ToInt32(tipelokasi.SelectedValue) == 1)
        {
            rs = rs.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi.Equals(1));
            if (lokasi.SelectedIndex > 0) { rs = rs.Where(p => p.StockProductGJ.StockActualGJ.IDLokasi.Equals(lokasi.SelectedValue)); }
        }
        else if (Convert.ToInt32(tipelokasi.SelectedValue) == 2)
        {
            rs = rs.Where(p => p.StockProductGJ.StockActualGJ.TipeLokasi.Equals(2));
            if (lokasi.SelectedIndex > 0) { rs = rs.Where(p => p.StockProductGJ.StockActualGJ.IDLokasi.Equals(lokasi.SelectedValue)); }
        }

        rs = rs.OrderBy(p => p.PromoGJ.TglExpire).ThenBy(n => n.TipeLokasi).ThenBy(n => n.IDLokasi).ThenBy(n => n.StockProductGJ.Nomor);

        //NO-RESULT
        zero.Visible = rs.Count() == 0;
        tbl_result.Visible = !zero.Visible;

        /// Master Brand
        var rsBrand = (from p in db.CompanyBrand select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocWarehouse
        var rsWarehouse = (from p in db.LocWarehouse select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocOutlet
        var rsOutlet = (from p in db.LocOutlet select p).ToDictionary(p => p.ID, p => p.Nama);

        /// Master Lokasi LocExhibition
        var rsExhibition = (from p in db.LocExhibition select p).ToDictionary(p => p.ID, p => p.Nama);

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            td.Style.Add("text-align","center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.PromoGJ);
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductGJ.Nomor;
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            if (r.PromoGJ.StatusActive == false)
            {
                td.Text = "<span style=\"font-size:12px;\" class=\"label label-danger\">Cancel</span>";
            }
            else
            {
                if (r.StockProductGJ.StatusPenjualan == 2)
                {
                    td.Text = "<span style=\"font-size:12px;\" class=\"label label-warning\">Sold</span>";
                }
                else if (Convert.ToDateTime(r.PromoGJ.TglExpire).AddDays(1) < System.DateTime.Now)
                {
                    td.Text = "<span style=\"font-size:12px;\" class=\"label label-warning\">Expired</span>";
                }
                else if (r.PromoGJ.TglStart > System.DateTime.Now)
                {
                    td.Text = "<span style=\"font-size:12px;\" class=\"label label-success\">Upcoming</span>";
                }
                else
                {
                    td.Text = "<span style=\"font-size:12px;\" class=\"label label-info\">Active</span>";
                }
            }

            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            string NamaLokasi = "";
            if (r.StockProductGJ.StockActualGJ.TipeLokasi == 0) { NamaLokasi = rsWarehouse[r.StockProductGJ.StockActualGJ.IDLokasi]; }
            else if (r.StockProductGJ.StockActualGJ.TipeLokasi == 1) { NamaLokasi = rsOutlet[r.StockProductGJ.StockActualGJ.IDLokasi]; }
            else if (r.StockProductGJ.StockActualGJ.TipeLokasi == 2) { NamaLokasi = rsExhibition[r.StockProductGJ.StockActualGJ.IDLokasi]; }

            td = new TableCell();
            td.Text = NamaLokasi;
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TglStartExpire(r.PromoGJ);
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DL.PromoStatus(r.PromoGJ.Tipe.Value);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_PromoHarga(r.PromoGJ);
            td.Style.Add("text-align", "right");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_DiscountNominal(r.PromoGJ);
            td.Style.Add("text-align", "right");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_DiscountPersen(r.PromoGJ);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Point(r.PromoGJ);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);


            tb.Rows.Add(tr);
        }
    }

    protected string TD_HyperLink(PromoGJ r)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (r.StatusActive != false)
        {
            href = "GJ_Cancel.aspx?id=" + r.ID;
            href += "&kw=" + kw.Text;
            href += "&dari=" + dari.Text;
            href += "&sampai=" + sampai.Text;
            href += "&status=" + status.SelectedValue;

            x += "&nbsp;<a class=\"btn btn-danger\" href=\"" + href + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Cancel";
            x += "</a>";

            if (Convert.ToDateTime(r.TglExpire).AddDays(1) > System.DateTime.Now)
            {
                href = "GJ_Print.aspx?id=" + r.ID;
                href += "&kw=" + kw.Text;
                href += "&dari=" + dari.Text;
                href += "&sampai=" + sampai.Text;
                href += "&status=" + status.SelectedValue;

                x += "&nbsp;<a class=\"btn btn-primary\" href=\"" + href + "\">";
                //x += "  <i class=\"icon-tags\"></i> ";
                x += "  Bandroll";
                x += "</a>";
            }
        }

        

        href = "GJ_Detail.aspx?id=" + r.ID;
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.SelectedValue;

        x += "&nbsp;<a class=\"btn btn-info\" href=\"" + href + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Detail";
        x += "</a>";


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

    protected string TD_Operator(PromoGJ r)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + r.OperatorNama + "</strong></p>";
        x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

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

    //bandroll_Click
    protected void bandroll_active_Click(object sender, EventArgs e)
    {
        string href = "";

        href = "GJ_List_Print.aspx?";
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&mode=active";

        Response.Redirect(href);

    }

    protected void bandroll_upcoming_Click(object sender, EventArgs e)
    {
        string href = "";

        href = "GJ_List_Print.aspx?";
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&mode=upcoming";

        Response.Redirect(href);

    }


    //bandroll_Click
    protected void bandroll_new_active_Click(object sender, EventArgs e)
    {
        string href = "";

        href = "GJ_List_Print_New.aspx?";
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&mode=active";

        Response.Redirect(href);

    }

    protected void bandroll_new_upcoming_Click(object sender, EventArgs e)
    {
        string href = "";

        href = "GJ_List_Print_New.aspx?";
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.SelectedValue;
        href += "&tipelokasi=" + tipelokasi.SelectedValue;
        href += "&lokasi=" + lokasi.SelectedValue;
        href += "&mode=upcoming";

        Response.Redirect(href);

    }

}