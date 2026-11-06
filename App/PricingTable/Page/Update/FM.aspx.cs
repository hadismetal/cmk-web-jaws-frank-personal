using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Update_FM : System.Web.UI.Page
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
            //DLC
            DLC.CompanyBrand(brand);
            DLC.CharProductItem(item);
            DLC.CharProcessCons(cons);
            DLC.StatusPricing(status);

            //Generate
            fillFilter();
            fillTable(sender, e);
        }
    }

    //Filter
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&brand=" + brand.SelectedIndex;
        href += "&item=" + item.SelectedIndex;
        href += "&cons=" + cons.SelectedIndex;
        href += "&status=" + status.SelectedIndex;
        href += "&display=" + display.SelectedIndex;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            brand.SelectedIndex = Convert.ToInt32(Request.QueryString["brand"]);
            item.SelectedIndex = Convert.ToInt32(Request.QueryString["item"]);
            cons.SelectedIndex = Convert.ToInt32(Request.QueryString["cons"]);
            status.SelectedIndex = Convert.ToInt32(Request.QueryString["status"]);
            display.SelectedIndex = Convert.ToInt32(Request.QueryString["display"]);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //WHERE
        string w = "1=1";
        if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;
        if (item.SelectedIndex > 0) w += " AND IDItem = " + item.SelectedValue;
        if (cons.SelectedIndex > 0) w += " AND IDCons = " + cons.SelectedValue;
        if (status.SelectedIndex > 0) w += " AND StatusPricing = " + status.SelectedValue;

        //SELECT
        var rs = from p in db.PricingTableFM.Where(w)
                 orderby
                     p.CompanyBrand.SN,
                     p.CharProductItem.SN,
                     p.CharProcessCons.SN
                 select p;

        //RESULT
        if (rs.Count() == 0) App.TableNoResult(tb);

        //RESULT
        string prev = "";
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            //Sub-Header
            if (prev != r.IDBrand + " " + r.IDItem)
            {
                Sub_Header(r);
                prev = r.IDBrand + " " + r.IDItem;
            }

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Operator(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Pembagi(r);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_PendingPembagi(r);
            td.CssClass = "text-right";
            tr.Cells.Add(td);


            td = new TableCell();
            td.Text = TD_Biaya(r);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_BiayaPending(r);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Mutasi(r);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }
    }
    protected void Sub_Header(PricingTableFM r)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += "<span class=\"text-orange\">";
        x += r.CompanyBrand.Nama + " (" + r.CompanyBrand.NamaKode + ")";
        x += "</span class=\"text-orange\">";
        x += "<br />";
        x += r.CharProductItem.Nama + " (" + r.CharProductItem.NamaKode + ")";

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected string TD_HyperLink(PricingTableFM r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../UpdateProcess/FM.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-pencil\"></i> ";
        x += "  Update";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(PricingTableFM r)
    {
        string x = string.Empty;

        x += "<p>";
        x += "<strong>";
        x += r.CharProcessCons.Nama + " (" + r.CharProcessCons.NamaKode + ")";
        x += "</strong>";
        x += "</p>";

        return x;
    }
    protected string TD_Operator(PricingTableFM r)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
        x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

        return x;
    }
    protected string TD_Biaya(PricingTableFM r)
    {
        string x = string.Empty;

        if (display.SelectedIndex == 0)
        {
            x += "<p class=\"text-success text-lg\">";
            x += Str.Number0(r.Biaya);
            x += "</p>";
        }
        else
        {
            x += "<p class=\"text-success\">" + Str.Number0(r.InputNO) + "</p>";
            x += "<p class=\"text-success\">" + Str.Number0(r.InputRO) + "</p>";
        }

        return x;
    }
    protected string TD_BiayaPending(PricingTableFM r)
    {
        string x = string.Empty;

        if (display.SelectedIndex == 0)
        {
            x += "<p class=\"text-danger text-lg\">";
            x += Str.Number0(r.BiayaPending);
            x += "</p>";
        }
        else
        {
            x += "<p class=\"text-danger\">" + Str.Number0(r.PendingNO) + "</p>";
            x += "<p class=\"text-danger\">" + Str.Number0(r.PendingRO) + "</p>";
        }

        return x;
    }
    protected string TD_Pembagi(PricingTableFM r)
    {
        string x = string.Empty;


        x += "<p class=\"text-success text-lg\">";
        x += Str.Number0(r.Pembagi);
        x += "</p>";


        return x;
    }
    protected string TD_PendingPembagi(PricingTableFM r)
    {
        string x = string.Empty;


        x += "<p class=\"text-danger text-lg\">";
        x += Str.Number0(r.PendingPembagi);
        x += "</p>";


        return x;
    }
    protected string TD_Mutasi(PricingTableFM r)
    {
        string x = string.Empty;

        if (r.MutasiPersen.HasValue)
        {
            string sgn = r.MutasiPersen > 0 ? "+" : "";
            string css = r.MutasiPersen > 10 || r.MutasiPersen < -10 ? "text-red" : "";

            x += "<p class=\"" + css + " text-lg\">";
            x += sgn;
            x += Str.Number2(r.MutasiPersen);
            x += "%";
            x += "</p>";
        }
        else if (r.Mutasi.HasValue)
        {
            string sgn = r.Mutasi > 0 ? "+" : "";

            x += "<p class=\"text-lg\">";
            x += sgn;
            x += Str.Number0(r.Mutasi);
            x += "</p>";
        }

        return x;
    }
}