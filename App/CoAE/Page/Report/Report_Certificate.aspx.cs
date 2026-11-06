using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_Report_Certificate : System.Web.UI.Page
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
            datepicker.Text = DateTime.Now.ToString("MM-yyyy");
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];

            status.SelectedValue = Request.QueryString["status"];

            DateTime start;
            DateTime end;
            DateTime dchoose;
            dchoose = Convert.ToDateTime(Request.QueryString["dari"]);
            start = new DateTime(dchoose.Year, dchoose.Month, 1);
            end = new DateTime(dchoose.Year, dchoose.Month, DateTime.DaysInMonth(dchoose.Year, dchoose.Month));
            datepicker.Text = dchoose.ToString("MM-yyyy");
            //sampai.Text = Request.QueryString["sampai"];
        }
    }
    protected void fillTable(object sender, EventArgs e)
    {
        DateTime start;
        DateTime end;
        DateTime dchoose;
        dchoose = Convert.ToDateTime(Request.QueryString["dari"]);
        start = new DateTime(dchoose.Year, dchoose.Month, 1);
        end = new DateTime(dchoose.Year, dchoose.Month, DateTime.DaysInMonth(dchoose.Year, dchoose.Month));
        datepicker.Text = dchoose.ToString("MM-yyyy");
        string kode = kw.Text.Trim();

        //SELECT
        var rs = (from p in db.PrintCertificate where p.StatusFinish == 1 select p);

        if (datepicker.Text != "" && kode != "")
        {

            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end.AddDays(1) && ((p.Nama.Contains(kode) ||(p.Keterangan.Contains(kode))) ));
        }

        else if (datepicker.Text != "" && kode == "")
        {
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end.AddDays(1));
        }

        DateTime dta = DateTime.Today;
        int Status = Convert.ToInt32(status.SelectedValue);

        if (Status == 1)
        {   rs = rs.Where(p => p.TglExpire >= dta); }

        else if (Status == 2)
        {   rs = rs.Where(p => p.TglExpire < dta.AddDays(1));  }

        if (rs.Count() == 0) App.TableNoResult(tb);

        string st = "";
        int sta = 0;
        //RESULT
        int prev = 0;
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            //td.Text = (r.TglExpire >= dta) ? TD_Hyperlink(r.ID) : "";
            if (r.TglExpire < dta.AddDays(1))
            {
                sta = 1;
            }
            else
            {
                sta = 0;
            }
            td.Text = TD_Hyperlink(r.ID, sta);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nomor;
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama;
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Keterangan;
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.FullDate(r.Tgl);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.FullDate(r.TglExpire);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            if (sta == 1)
            {
                st = "Expired";
                td.Text = "<p style='color: red'>" + st + "</p>";
            }
            else
            {
                st = "Active";
                td.Text = "<p style='color: green'>" + st + "</p>";
            }
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            int qty = Db.SingleInteger(" select COUNT(*) from PrintCertificate_Detail where IDForm =" + r.ID, Db.Cnn_DB);

            td = new TableCell();
            td.Text = qty.ToString();
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Operator.ToString();
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);
            tb.Rows.Add(tr);
        }
    }

    protected string TD_Hyperlink(int id, int ex)
    {
        string x = string.Empty;

        if (ex == 0)
        {
            x += "<a class=\"btn btn-xs btn-info\" target=\"_blank\" href=\"Detail_Certificate.aspx?id=" + id + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "";
            x += "</a>";

            x += "&nbsp;";
            x += "<a class=\"btn btn-xs btn-info\" target=\"_blank\" href=\"Detail_Certificate_Export.aspx?id=" + id + "\">";
            x += "  <i class=\"icon-download-alt\"></i> ";
            x += "";
            x += "</a>";
        }
        else if (ex == 1)
        {
            x += "<a class=\"btn btn-xs btn-info\" target=\"_blank\" href=\"Detail_Certificate.aspx?id=" + id + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "";
            x += "</a>";
        }
        return x;
    }



    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        DateTime start;
        DateTime end;
        DateTime dchoose;

        dchoose = Convert.ToDateTime(datepicker.Text);
        start = new DateTime(dchoose.Year, dchoose.Month, 1);
        end = new DateTime(dchoose.Year, dchoose.Month, DateTime.DaysInMonth(dchoose.Year, dchoose.Month));
        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&status=" + status.SelectedValue;
        href += "&dari=" + start.ToShortDateString();
        href += "&sampai=" + end.ToShortDateString();
        Response.Redirect(href);
    }
}