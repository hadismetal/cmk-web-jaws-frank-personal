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
            
            status.SelectedValue = "1";
            fillFilter();
            
            if (App.Refresh(this)) fillTable(sender, e);

        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        var rs = from p in db.BundleItem
                 
                 //where p.Draft == false && p.ApprovalTgl == null && p.StatusActive == 1
                 select p;

        //rs = from p in rs
        //     where !(from z in db.Bundle_ItemMaster
        //             select z.IDForm).Contains(p.ID)
        //     select p;

        var rslt = from p in rs
             from q in db.Bundle_ItemPasangan.Where(q => q.IDForm == p.ID)
             from r in db.StockProductDJ.Where(r => r.ID == q.IDProduct && q.TipeProduct == 1).DefaultIfEmpty()
                from s in db.StockProductGJ.Where(s => s.ID == q.IDProduct && q.TipeProduct == 2).DefaultIfEmpty()
                from t in db.StockProductPG.Where(t => t.ID == q.IDProduct && q.TipeProduct == 3).DefaultIfEmpty()
                   from u in db.StockProductLD_Stone1B.Where(u => u.ID == q.IDProduct && q.TipeProduct == 4).DefaultIfEmpty()
             where !(from z in db.Bundle_ItemMaster
                     select z.IDForm).Contains(p.ID)
             select new
             {
                 p, q, r,s,t,u
             };

        rslt = rslt.OrderBy(p => p.p.TglStart);

        string Status = "";
        Status = Request.QueryString["status"];
        if (Status == "1")
            rslt = rslt.Where(p => p.p.StatusActive == 1);
        else if (Status == "2")
            rslt = rslt.Where(p => p.p.StatusActive == 2);
        if (kw.Text != "")
        {
            rslt = rslt.Where(p => p.r.Nomor.Contains(kw.Text) || p.s.Nomor.Contains(kw.Text) || p.t.Nomor.Contains(kw.Text) || p.u.Nomor.Contains(kw.Text));
        }
        if (dari.Text != "" && sampai.Text != "")
        {
            rslt = rslt.Where(p => p.p.TglStart.Value >= Convert.ToDateTime(dari.Text) && p.p.TglExpire.Value < Convert.ToDateTime(sampai.Text).AddDays(1));
        }

        var rst = rslt.GroupBy(p => p.p.ID);

        //NO-RESULT
        if(rslt.Count() == 0)
            App.TableNoResult(tb);

        //RESULT
        int index = 1;
        foreach (var r in rst)
        {
            var d = db.BundleItem.SingleOrDefault(p => p.ID == r.Key);
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(d);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(d);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Operator(d);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TglStart(d);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_TglExpire(d);
            tr.Cells.Add(td);


            tb.Rows.Add(tr);

        }
    }

    protected string TD_HyperLink(BundleItem r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Souvenir_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-pencil\"></i> ";
        x += "  Detail";
        x += "</a>";

        if (r.StatusActive == 1)
        {
            x += "&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"Souvenir_Cancel.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Cancel";
            x += "</a>";
        }

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

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            status.Text = Request.QueryString["status"];
            kw.Text = Request.QueryString["kw"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&status=" + status.Text;
        href += "&kw=" + kw.Text;

        Response.Redirect(href);
    }
}