using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;

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
        var rs = (from p in db.StockProductWhitelist select p);

        if (datepicker.Text != "" && kode != "")
        {

            rs = rs.Where(p => p.OperatorTgl >= start && p.OperatorTgl < end.AddDays(1) && ((p.NomorPLU.Contains(kode) || (p.Keterangan.Contains(kode)))));
        }

        else if (datepicker.Text != "" && kode == "")
        {
            rs = rs.Where(p => p.OperatorTgl >= start && p.OperatorTgl < end.AddDays(1));
        }

        DateTime dta = DateTime.Today;
        int Status = Convert.ToInt32(status.SelectedValue);
        string xStatus = "";

        if (Status == 1)
        { 
            rs = rs.Where(p => p.ApprovalTgl != null && p.Approval != null); 
        }
        else if (Status == 2)
        { 
            rs = rs.Where(p => p.ApprovalTgl == null && p.Approval == null);; 
        }

        if (rs.Count() == 0) App.TableNoResult(tb);

        string st = "";
        int sta = 0;
        //RESULT
        int prev = 0;
        int index = 1;

        rs = rs.OrderByDescending(p => p.OperatorTgl);

        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = (r.Approval == null && r.ApprovalTgl == null) ? TD_Hyperlink(r.ID) : "";
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.NomorPLU;
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            xStatus = (r.Approval != null && r.ApprovalTgl != null)
                    ? "<span style='color:green;'>Approved</span>"
                    : "<span style='color:red;'>Pending</span>"
                    ;

            td = new TableCell();
            td.Text = xStatus;
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Operator.ToString() + "<br />" + Str.FullDateJam(r.OperatorTgl);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Approval != null ? (r.Approval.ToString() + "<br />" + Str.FullDateJam(r.ApprovalTgl)) : "";
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);
            tb.Rows.Add(tr);

            td = new TableCell();
            td.Text = r.Keterangan;
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

        }
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


    protected string TD_Hyperlink(int id)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-xs btn-success\" href=\"WhitelistTradeIn_Approve.aspx?id=" + id + "\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "";
        x += "</a>";

        return x;
    }


}