using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Report_QCLD : System.Web.UI.Page
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
            if (App.Refresh(this))
			{
				fillFilter();
				fillTable(sender, e);
			}
        }
    }
    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
		string mode = "FINAL";
		string producttype = "LD";
        string keyword = kw.Text;
        string[] dari1 = dari.Text.Split('-');
        string[] sampai1 = sampai.Text.Split('-');
        string start = dari1[2] + "-" + dari1[1] + "-" + dari1[0] + " 00:00:00.000";
        string end = sampai1[2] + "-" + sampai1[1] + "-" + sampai1[0] + " 23:59:00.000";

        string query = "exec sp_get_qc_new '" + producttype + "','" + keyword + "', '" + start + "', '" + end + "', '" + mode + "' ";
        DataTable dt = Db.Rs(query, Db.Cnn_DB);

        int rowcount = dt.Rows.Count;
        int index = 1;
        if (rowcount == 0)
        {
			TableRow tr = new TableRow();
            TableCell td = new TableCell();
			td.ColumnSpan = 6;
			td.Text = "No Data";
			tb.Rows.Add(tr);
        }
        else
        {
            foreach (DataRow dr in dt.Rows)
            {
                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(Convert.ToInt32(dr["ID"]));
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index.ToString() + ".</p>";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["Nomor"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["PLU"].ToString();
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Convert.ToDateTime(dr["OperatorTgl"]).ToString("dd MMMM yyyy");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = dr["OperatorNama"].ToString();
                tr.Cells.Add(td);

                tb.Rows.Add(tr);

                index++;
            }
        }
    }

    protected string TD_HyperLink(int IDDoc)
    {
        string x = string.Empty;
		
        x += "<div class=\"btn-group\">";
        x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"QCLDFinal_Detail.aspx?id=" + IDDoc + "\">";
        x += "      <i class=\"icon-search\"></i> ";
        x += "      ";
        x += "  </a>";
        x += "</div>";

        return x;
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];

            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;


        Response.Redirect(href);
    }
}