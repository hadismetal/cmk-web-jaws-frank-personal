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
            //DLC.CharGoldModelAllFilter(ddl_Model);
            //DLC.CharGoldLevel(ddl_ProductLevel);
            //DLC.CharFrameColor(ddl_RangkaWarna);
            //DLC.CharProductItem(ddl_ProductItem);
            //DLC.CharTargetAge(ddl_KelompokUsia);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        string xsearch = kw.Text.Trim();

        var rs = from p in db.StockProductPG
                 where p.Draft == false && (p.Nomor.Contains(xsearch))
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        //int ProductLevel = Convert.ToInt32(ddl_ProductLevel.SelectedValue);
        //if (ProductLevel > 0) rs = rs.Where(p => p.ProductLevel == ProductLevel);

        //int ProductItem = Convert.ToInt32(ddl_ProductItem.SelectedValue);
        //if (ProductItem > 0) rs = rs.Where(p => p.ProductItem == ProductItem);

        //int TargetAge = Convert.ToInt32(ddl_KelompokUsia.SelectedValue);
        //if (TargetAge > 0) rs = rs.Where(p => p.TargetAge == TargetAge);

        //int Model = Convert.ToInt32(ddl_Model.SelectedValue);
        //if (Model > 0) rs = rs.Where(p => p.Model == Model);

        //int RangkaWarna = Convert.ToInt32(ddl_RangkaWarna.SelectedValue);
        //if (RangkaWarna > 0) rs = rs.Where(p => p.FrameColor == RangkaWarna);

        if (rs != null)
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

                td = new TableCell();
                td.Text = TD_Product01(r);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "middle");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Product02(r);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "middle");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Product03(r);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "middle");
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            //ddl_ProductLevel.SelectedValue = Request.QueryString["productlevel"];
            //ddl_ProductItem.SelectedValue = Request.QueryString["productitem"];
            //ddl_KelompokUsia.SelectedValue = Request.QueryString["targetage"];
            //ddl_Model.SelectedValue = Request.QueryString["model"];
            //ddl_RangkaWarna.SelectedValue = Request.QueryString["framecolor"];
            
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        //href += "&productlevel=" + ddl_ProductLevel.SelectedValue;
        //href += "&productitem=" + ddl_ProductItem.SelectedValue;
        //href += "&targetage=" + ddl_KelompokUsia.SelectedValue;
        //href += "&model=" + ddl_Model.SelectedValue;
        //href += "&framecolor=" + ddl_RangkaWarna.SelectedValue;


        Response.Redirect(href);
    }
    protected string TD_HyperLink(StockProductPG r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info btn-xs\" target=\"_blank\" href=\"PG_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }
    protected string TD_DataLegacy(StockProductPG r)
    {
        string x = string.Empty;

        if (r.Legacy == 1)
        { x += "<i class='icon-warning-sign text-red'>&nbsp;</i><span class='text-bold text-red'>Legacy Product</span>"; }

        return x;
    }
    protected string TD_Data(StockProductPG r, string xsearch = "")
    {
        string x = string.Empty;

        string rsearch = (xsearch != "") ? "<span style='color:blue;'>" + xsearch + "</span>" : "";
        string result = (xsearch != "") ? r.Nomor.Replace(xsearch, rsearch) : r.Nomor;

        if (r.ID != null)
        { x += "<p class=\"text-bold\">" + result + "</p>"; }

        return x;
    }
    protected string TD_Product01(StockProductPG r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-info\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(StockProductPG r)
    {
        string x = string.Empty;

        x += "<p class=\"text-orange\">";
        x += r.CharGoldModel.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product03(StockProductPG r)
    {
        string x = string.Empty;

        x += "<p class=\"text-orange\">";
        x += r.CharGoldLevel.Nama;
        x += "</p>";

        return x;
    }
}