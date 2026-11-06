using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class DJ_List : System.Web.UI.Page
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
        var rs = from p in db.PromoDJ_Detail
                 where p.PromoDJ.Draft == false && p.PromoDJ.ApprovalNama != null && p.PromoDJ.ApprovalTgl != null
                 select p;

        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            rs = rs.Where(p => p.StockProductDJ.StockActualDJ.TipeLokasi == d.First().TipeLokasi && p.StockProductDJ.StockActualDJ.IDLokasi == d.First().IDLokasi);
        }

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.StockProductDJ.StockActualDJ.IDLokasi) && p.StockProductDJ.StockActualDJ.TipeLokasi == 1);
        }


        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => (p.PromoDJ.TglStart.Value <= Convert.ToDateTime(dari.Text) && p.PromoDJ.TglStart.Value <= Convert.ToDateTime(sampai.Text) && p.PromoDJ.TglExpire.Value >= Convert.ToDateTime(dari.Text) && p.PromoDJ.TglExpire.Value <= Convert.ToDateTime(sampai.Text))
                            || (p.PromoDJ.TglStart.Value >= Convert.ToDateTime(dari.Text) && p.PromoDJ.TglStart.Value <= Convert.ToDateTime(sampai.Text) && p.PromoDJ.TglExpire.Value >= Convert.ToDateTime(dari.Text) && p.PromoDJ.TglExpire.Value >= Convert.ToDateTime(sampai.Text))
                            || (p.PromoDJ.TglStart.Value >= Convert.ToDateTime(dari.Text) && p.PromoDJ.TglStart.Value <= Convert.ToDateTime(sampai.Text) && p.PromoDJ.TglExpire.Value >= Convert.ToDateTime(dari.Text) && p.PromoDJ.TglExpire.Value <= Convert.ToDateTime(sampai.Text))
                            || (p.PromoDJ.TglStart.Value <= Convert.ToDateTime(dari.Text) && p.PromoDJ.TglStart.Value <= Convert.ToDateTime(sampai.Text) && p.PromoDJ.TglExpire.Value >= Convert.ToDateTime(dari.Text) && p.PromoDJ.TglExpire.Value >= Convert.ToDateTime(sampai.Text))
                            );
        }
        if (kw.Text != "")
            rs = rs.Where(p => p.StockProductDJ.Nomor == (kw.Text));
        if (status.SelectedValue == "1")
            rs = rs.Where(p => (p.PromoDJ.StatusActive == null || p.PromoDJ.StatusActive == true) && p.StockProductDJ.StatusPenjualan == 0);
        else if (status.SelectedValue == "2")
            rs = rs.Where(p => p.StockProductDJ.StatusPenjualan == 2);
        else if (status.SelectedValue == "3")
            rs = rs.Where(p => p.PromoDJ.TglStart > System.DateTime.Now && (p.PromoDJ.StatusActive == null || p.PromoDJ.StatusActive == true) && p.StockProductDJ.StatusPenjualan == 0);
        else if (status.SelectedValue == "4")
            rs = rs.Where(p => p.PromoDJ.StatusActive == false);

        /// filter lokasi
        //if (Convert.ToInt32(tipelokasi.SelectedValue) == 0)
        //{
        //    rs = rs.Where(p => p.StockProductDJ.StockActualDJ.TipeLokasi.Equals(0));
        //    if (lokasi.SelectedIndex > 0) { rs = rs.Where(p => p.StockProductDJ.StockActualDJ.IDLokasi.Equals(lokasi.SelectedValue)); }
        //}
        //else if (Convert.ToInt32(tipelokasi.SelectedValue) == 1)
        //{
        //    rs = rs.Where(p => p.StockProductDJ.StockActualDJ.TipeLokasi.Equals(1));
        //    if (lokasi.SelectedIndex > 0) { rs = rs.Where(p => p.StockProductDJ.StockActualDJ.IDLokasi.Equals(lokasi.SelectedValue)); }
        //}
        //else if (Convert.ToInt32(tipelokasi.SelectedValue) == 2)
        //{
        //    rs = rs.Where(p => p.StockProductDJ.StockActualDJ.TipeLokasi.Equals(2));
        //    if (lokasi.SelectedIndex > 0) { rs = rs.Where(p => p.StockProductDJ.StockActualDJ.IDLokasi.Equals(lokasi.SelectedValue)); }
        //}

        rs = rs.OrderBy(p => p.PromoDJ.TglExpire).ThenBy(n => n.TipeLokasi).ThenBy(n => n.IDLokasi).ThenBy(n => n.StockProductDJ.Nomor);

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
            td.Text = TD_Nama(r.PromoDJ);
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductDJ.Nomor;
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            if (r.PromoDJ.StatusActive == false)
            {
                td.Text = "<span style=\"font-size:12px;\" class=\"label label-danger\">Cancel</span>";
            }
            else
            {
                if (r.StockProductDJ.StatusPenjualan == 2)
                {
                    td.Text = "<span style=\"font-size:12px;\" class=\"label label-warning\">Sold</span>";
                }
                else if (Convert.ToDateTime(r.PromoDJ.TglExpire).AddDays(1) < System.DateTime.Now)
                {
                    td.Text = "<span style=\"font-size:12px;\" class=\"label label-warning\">Expired</span>";
                }
                else if (r.PromoDJ.TglStart > System.DateTime.Now)
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
            if (r.StockProductDJ.StockActualDJ.TipeLokasi == 0) { NamaLokasi = rsWarehouse[r.StockProductDJ.StockActualDJ.IDLokasi]; }
            else if (r.StockProductDJ.StockActualDJ.TipeLokasi == 1) { NamaLokasi = rsOutlet[r.StockProductDJ.StockActualDJ.IDLokasi]; }
            else if (r.StockProductDJ.StockActualDJ.TipeLokasi == 2) { NamaLokasi = rsExhibition[r.StockProductDJ.StockActualDJ.IDLokasi]; }

            td = new TableCell();
            td.Text = NamaLokasi;
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TglStartExpire(r.PromoDJ);
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = DL.PromoStatus(r.PromoDJ.Tipe.Value);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_PromoHarga(r.PromoDJ);
            td.Style.Add("text-align", "right");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_DiscountNominal(r.PromoDJ);
            td.Style.Add("text-align", "right");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_DiscountPersen(r.PromoDJ);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Point(r.PromoDJ);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);


            tb.Rows.Add(tr);
        }
    }

    protected string TD_HyperLink(PromoDJ r)
    {
        string x = string.Empty;
        string href = string.Empty;

        if (r.StatusActive != false)
        {
            href = "DJ_Cancel.aspx?id=" + r.ID;
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
                href = "DJ_Print.aspx?id=" + r.ID;
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

        

        href = "DJ_Detail.aspx?id=" + r.ID;
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

    protected string TD_Nama(PromoDJ r)
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

    protected string TD_Operator(PromoDJ r)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + r.OperatorNama + "</strong></p>";
        x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

        return x;
    }

    protected string TD_DiscountPersen(PromoDJ r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += (r.DiscountPersen.HasValue) ? (r.DiscountPersen.Value != 0 ? Str.Number2(r.DiscountPersen.Value) : "") : "";
        x += "</p>";

        return x;
    }

    protected string TD_DiscountNominal(PromoDJ r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += (r.DiscountNominal.HasValue) ? (r.DiscountNominal.Value != 0 ? Str.Number0(r.DiscountNominal.Value) : "") : "";
        x += "</p>";

        return x;
    }
    protected string TD_Point(PromoDJ r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += (r.PoinReward.HasValue) ? (r.PoinReward.Value != 0 ? Str.Number2(r.PoinReward.Value) : "") : "";
        x += "</p>";

        return x;
    }
    protected string TD_PromoHarga(PromoDJ r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += (r.PromoHarga.HasValue) ? (r.PromoHarga.Value != 0 ? Str.Number0(r.PromoHarga.Value) : "") : "";
        x += "</p>";

        return x;
    }
    protected string TD_TglStartExpire(PromoDJ r)
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
        //tipelokasi.Items.Clear();
        //tipelokasi.Items.Add(new ListItem("All", "-1"));
        //tipelokasi.Items.Add(new ListItem("Warehouse", "0"));
        //tipelokasi.Items.Add(new ListItem("Outlet", "1"));
        //tipelokasi.Items.Add(new ListItem("Exhibition", "2"));

        if (App.Refresh(this))
        {
            //tipelokasi.SelectedValue = Request.QueryString["tipelokasi"];
            //DLC.LocationList(lokasi, Convert.ToInt32(Request.QueryString["tipelokasi"]));

            //lokasi.SelectedValue = Request.QueryString["lokasi"];

            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            kw.Text = Request.QueryString["kw"];
            status.Text = Request.QueryString["status"];
        }
        else
        {
            //lokasi.Items.Clear();
            //lokasi.Items.Add(new ListItem("All", "-1"));

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
        //href += "&tipelokasi=" + tipelokasi.SelectedValue;
        //href += "&lokasi=" + lokasi.SelectedValue;

        Response.Redirect(href);
    }

    protected void FillLokasi(object sender, EventArgs e)
    {
        //int Tipe = Convert.ToInt32(tipelokasi.SelectedValue);
        //lokasi.Items.Clear();
        //if (Tipe == 0 || Tipe == 1 || Tipe == 2) { DLC.LocationList(lokasi, Tipe); }
        //else { lokasi.Items.Add(new ListItem("All", "-1")); }
        //lokasi.Focus();
    }

    //bandroll_Click
    protected void bandroll_active_Click(object sender, EventArgs e)
    {
        string href = "";

        href = "DJ_List_Print.aspx?";
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.SelectedValue;
        //href += "&tipelokasi=" + tipelokasi.SelectedValue;
        //href += "&lokasi=" + lokasi.SelectedValue;
        href += "&mode=active";

        Response.Redirect(href);

    }

    protected void bandroll_upcoming_Click(object sender, EventArgs e)
    {
        string href = "";

        href = "DJ_List_Print.aspx?";
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.SelectedValue;
        //href += "&tipelokasi=" + tipelokasi.SelectedValue;
        //href += "&lokasi=" + lokasi.SelectedValue;
        href += "&mode=upcoming";

        Response.Redirect(href);

    }


    //bandroll_Click
    protected void bandroll_new_active_Click(object sender, EventArgs e)
    {
        string href = "";

        href = "DJ_List_Print_New.aspx?";
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.SelectedValue;
        //href += "&tipelokasi=" + tipelokasi.SelectedValue;
        //href += "&lokasi=" + lokasi.SelectedValue;
        href += "&mode=active";

        Response.Redirect(href);

    }

    protected void bandroll_new_upcoming_Click(object sender, EventArgs e)
    {
        string href = "";

        href = "DJ_List_Print_New.aspx?";
        href += "&kw=" + kw.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.SelectedValue;
        //href += "&tipelokasi=" + tipelokasi.SelectedValue;
        //href += "&lokasi=" + lokasi.SelectedValue;
        href += "&mode=upcoming";

        Response.Redirect(href);

    }

}