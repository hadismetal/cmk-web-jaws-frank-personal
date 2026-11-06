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
        var rs = from p in db.BundleItem
                 orderby p.DraftDate
                 //where p.Draft == false && p.ApprovalTgl != null
                 where !(from z in db.Bundle_ItemMaster
                         select z.IDForm).Contains(p.ID)
                 select p;

        //rs = from p in rs
        //     where !(from z in db.Bundle_ItemMaster
        //             select z.IDForm).Contains(p.ID)
        //     select p;

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
            td.Text = TD_Nama(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Operator(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TglStart(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TglExpire(r);
            tr.Cells.Add(td);
            
            tb.Rows.Add(tr);

        }
    }

    protected string TD_HyperLink(BundleItem r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Souvenir.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-pencil\"></i> ";
        x += "  Update";
        x += "</a>";

        return x;
    }

    protected string TD_Nama(BundleItem r)
    {
        string x = string.Empty;

        x += "<p><strong>" + r.Nama + "</strong></p>";

        return x;
    }

    protected string TD_Operator(BundleItem r)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
        x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

        return x;
    }

    
    protected string TD_TglStart(BundleItem r)
    {

        string x = string.Empty;

        x += "<p>" + Str.Tgl(r.TglStart) + "</p>";

        return x;
    }

    protected string TD_TglExpire(BundleItem r)
    {

        string x = string.Empty;

        x += "<p>" + Str.Tgl(r.TglExpire) + "</p>";

        return x;
    }
}