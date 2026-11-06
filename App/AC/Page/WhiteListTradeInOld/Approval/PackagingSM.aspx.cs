using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Approval_PackagingSM : System.Web.UI.Page
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

        string query = "EXEC sp_get_list_form_approval_packaging '" + Convert.ToDateTime(start).ToString("yyyy-MM-dd") + "', '" + Convert.ToDateTime(end).ToString("yyyy-MM-dd") + "', '" + kw.Text + "'";
        DataTable dt = new DataTable();
        dt = Db.Rs(query, Db.Cnn_DB);

        int index = 1;
        foreach(DataRow dr in dt.Rows)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_Hyperlink(dr["NomorPackaging"].ToString(), dr["ApprovalPackagingSM"].ToString());
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = index.ToString();
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = dr["NomorPackaging"].ToString();
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = dr["LinkPackaging"].ToString();
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = dr["ApprovalPackagingSM"].ToString();
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = dr["ApprovalPackagingAccounting"].ToString();
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = dr["ApprovalPackagingPusat"].ToString();
			td.Style.Add("text-align", "center");
            tr.Cells.Add(td);
			
			td = new TableCell();
            td.Text = TD_Status(dr["ApprovalPackagingSM"].ToString(), dr["ApprovalPackagingAccounting"].ToString(), dr["ApprovalPackagingPusat"].ToString());
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);
			
			td = new TableCell();
            td.Text = TD_Keterangan(dr["ApprovalPackagingSM"].ToString(), dr["ApprovalPackagingAccounting"].ToString(), dr["ApprovalPackagingPusat"].ToString());
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
			
			index++;
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


    protected string TD_Hyperlink(string nopck, string appsm)
    {
        string x = string.Empty;
		
		int id = 0;
		int sid = 0;
		
		var d = db.SalesOrder.SingleOrDefault(p => p.Nomor == nopck);
		
		id = d.ID;
		sid = d.SalesReceipt.FirstOrDefault().ID;
		
		if(string.IsNullOrEmpty(appsm))
		{
			x += "<a target=\"_blank\" class=\"btn btn-xs btn-success\" href=\"PackagingSM_Approve.aspx?nopck=" + nopck + "\">";
			x += "  <i class=\"icon-ok\"></i> ";
			x += "";
			x += "</a>";
		}
		
		x += "<a target=\"_blank\" class=\"btn btn-xs btn-info\" style=\"margin-left:2px\" href=\"Sales_Packaging.aspx?id=" + id + "&sid=" + sid + "\">";
		x += "  <i class=\"icon-print\"></i> ";
		x += "";
		x += "</a>";

        return x;
    }
	
	protected string TD_Status(string appsm, string appaccounting, string applsp)
	{
		string x = string.Empty;
		
		if(string.IsNullOrEmpty(appsm))
		{
			x = "<label style=\"color:black\">Draft</label>";
		}
		if(!string.IsNullOrEmpty(appsm))
		{
			x = "<label style=\"color:lime\">Approved By SM</label>";
		}
		if(!string.IsNullOrEmpty(applsp))
		{
			x = "<label style=\"color:orange\">Approved By Finance</label>";
		}
		if(!string.IsNullOrEmpty(appaccounting))
		{
			x = "<label style=\"color:lightblue\">Approved By Accounting</label>";
		}
		
		return x;
	}
	
	protected string TD_Keterangan(string appsm, string appaccounting, string applsp)
	{
		string x = string.Empty;
		
		if(string.IsNullOrEmpty(appsm))
		{
			x = "<label style=\"color:black\">Belum Approve SM</label>";
		}
		else if(!string.IsNullOrEmpty(appsm) && string.IsNullOrEmpty(applsp))
		{
			x = "<label style=\"color:orange\">Belum Approve Finance</label>";
		}
		else if(!string.IsNullOrEmpty(applsp) && string.IsNullOrEmpty(appaccounting))
		{
			x = "<label style=\"color:lime\">Belum Approve Accounting</label>";
		}
		else if(!string.IsNullOrEmpty(appsm) && !string.IsNullOrEmpty(appaccounting) && !string.IsNullOrEmpty(applsp))
		{
			x = "<label style=\"color:blue\">Done</label>";
		}
		
		return x;
	}
}