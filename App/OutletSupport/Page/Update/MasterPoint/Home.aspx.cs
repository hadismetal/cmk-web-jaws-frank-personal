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
        }
        fillTable(sender, e);
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        var rs = from p in db.PointRedeem
                 where p.ApprovalTgl != null
                 select p;

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Point).ToString("N0");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Convert.ToDecimal(r.Rupiah).ToString("N2");
            tr.Cells.Add(td);
            
            tb.Rows.Add(tr);
        }
    }

    protected string TD_HyperLink(PointRedeem r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"MasterPoint.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-pencil\"></i> ";
        x += "  Update";
        x += "</a>";

        return x;
    }
}