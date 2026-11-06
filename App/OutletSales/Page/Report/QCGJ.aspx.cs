using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Resell : System.Web.UI.Page
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

            fillFilter();

            if (App.Refresh(this)) fillTable(sender, e);  

        }
        
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {


        var rs = from p in db.DocQCGJ

                 select p;

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.StockProductGJ.Nomor == "" + keyword + "" || p.Nomor == "" + keyword + "");

        
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.StockProductGJ.OperatorTgl >= Convert.ToDateTime(dari.Text) && p.StockProductGJ.OperatorTgl < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        int index = 1;
        foreach (var r in rs)
        {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StockProductGJ.Nomor;
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.TglJam(r.OperatorTgl);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.QCNama;
            tr.Cells.Add(td);

            tb.Rows.Add(tr);

            index++;
        }
    }

    protected string TD_HyperLink(int IDDoc)
    {
        string x = string.Empty;

        x += "<div class=\"btn-group\">";
        x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"QCGJ_Print.aspx?id=" + IDDoc + "\">";
        x += "      <i class=\"icon-print\"></i> ";
        x += "      ";
        x += "  </a>";
        x += "</div>&nbsp;";
        x += "<div class=\"btn-group\">";
        x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"QCGJ_Detail.aspx?id=" + IDDoc + "\">";
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