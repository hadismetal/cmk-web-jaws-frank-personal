using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_DJ : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        string xsearch = kw.Text.Trim();

        var rs = from p in db.StockProductDJ
                 where 1==1
                    && p.Draft == false
                    && p.Nomor.Contains(xsearch)
                 select p;

        rs = rs.OrderBy(p => p.Nomor);

        if (rs.Count() > 0)
        {
            if (rs.Count() > 100) { lit_datacount.Text = "Showing 100 / " + rs.Count() + " data"; rs = rs.Take(100); } else { lit_datacount.Text = "Showing " + rs.Count() + " data"; }

            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "middle");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "middle");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Data(r, xsearch);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "middle");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_DataLegacy(r);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "middle");
                tr.Cells.Add(td);

                if (r.StockProductDJ_CharProduct != null)
                {
                    td = new TableCell();
                    td.Text = TD_Product01(r.StockProductDJ_CharProduct);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Product02(r.StockProductDJ_CharProduct);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Product03(r.StockProductDJ_CharProduct);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "middle");
                    tr.Cells.Add(td);
                }

                tb.Rows.Add(tr);
            }
        }
        else
            err.Text = "<p class=\"text-danger\">Kode PLU tidak ditemukan.</p>";
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];

        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;


        Response.Redirect(href);
    }
    protected string TD_HyperLink(StockProductDJ r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info btn-xs\" target=\"_blank\" href=\"DJ_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }
    protected string TD_Data(StockProductDJ r, string xsearch = "")
    {
        string x = string.Empty;

        string rsearch = (xsearch != "") ? "<span style='color:blue;'>" + xsearch + "</span>" : "";
        string result = (xsearch != "") ? r.Nomor.Replace(xsearch, rsearch) : r.Nomor;

        if (r.ID != null)
        { x += "<p class=\"text-bold\">" + result + "</p>"; }

        return x;
    }
    protected string TD_DataLegacy(StockProductDJ r)
    {
        string x = string.Empty;
        
        if (r.Legacy == 1)
        { x += "<i class='icon-warning-sign text-red'>&nbsp;</i><span class='text-bold text-red'>Legacy Product</span>"; }

        return x;
    }
    protected string TD_Product01(StockProductDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-info\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(StockProductDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-orange\">";
        x += r.CharProductCategory.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product03(StockProductDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-orange\">";
        x += r.CharProductLevel.Nama;
        x += "</p>";

        return x;
    }
}