using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

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
    protected int id { get { return App.QueryInt(this, "id"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            //Generate
            //fillFilter();

            //if (App.Refresh(this))
            fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.DocRepairResult
                 where //p.StatusTitipan != 5 && p.StatusTitipan.HasValue
                 p.Draft.Value == false && p.DocRepair.IDProduct == id
                 select p;

        var s = db.StockProductDJ.SingleOrDefault(p => p.ID == id);

        

        rs = rs.OrderByDescending(p => p.Tgl);

        if (rs.Count() > 0)
        {
            //Header
            header_plu.InnerHtml = s.Nomor;
                //"<a href=\"../History/DJ_File.aspx?id=" + s.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                //+ s.Nomor + "</a>";

            int index = 1;
            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                //td = new TableCell();
                //td.Text = TD_HyperLink(r);
                //td.Style.Add("text-align", "center");
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Hyperlink_Invoice(r);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.Operator;
                td.Style.Add("text-align", "center");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }

    protected void fillFilter()
    {

    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&id=" + id;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(DocRepairResult r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Repair_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }


    protected string TD_Hyperlink_Invoice(DocRepairResult d)
    {
        string x = string.Empty;

        x += "<a target='_blank' href='Repair_Detail.aspx?id=" + d.ID + "'>" + d.Nomor + "</a>";

        return x;
    }
}