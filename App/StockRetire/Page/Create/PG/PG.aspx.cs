using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_PG : System.Web.UI.Page {
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
		App.ProtectRole(Role.Manager);

		if (!IsPostBack) {

			//Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {

		//SELECT
		var rs = (from p in db.StockActualPG
                  where p.StockProductPG.Draft == false && p.StockProductPG.ReturnProduct == false && p.StockProductPG.StatusPenjualan == 0 && p.StockProductPG.StatusLebur == false
                  && p.Substorage == 6
                 select p);
                  //select p).Where(p => db.DocQCPG.Any(y => y.IDProduct != p.ID));

        rs = rs.OrderBy(p => p.StockProductPG.LocWarehouse.Nama).ThenBy(p => p.StockProductPG.ID);

        if (kw.Text != "")
            rs = rs.Where(p => p.Nomor == "" + kw.Text + "");

		//NO-RESULT
		//zero.Visible = rs.Count() == 0;
		//form_area.Visible = !zero.Visible;

		//RESULT
		int prev = 0;
		int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            //Sub-Header
            if (prev != r.StockProductPG.IDWarehouse.Value)
            {
                Sub_Header(r.StockProductPG);
                prev = r.StockProductPG.IDWarehouse.Value;
            }

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(r.StockProductPG);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Image(r.StockProductPG);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nomor(r.StockProductPG);
            tr.Cells.Add(td);

            if (r != null)
            {
                td = new TableCell();
                td.Text = TD_Product01(r.StockProductPG);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Product02(r.StockProductPG);
                td.CssClass = "text-center";
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Product03(r.StockProductPG);
                td.CssClass = "text-center";
                tr.Cells.Add(td);
            }

            tb.Rows.Add(tr);
        }
	}
    protected void Sub_Header(StockProductPG r)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += r.LocWarehouse.Nama + " - " + r.LocWarehouse.CompanyBrand.Nama;

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected string TD_HyperLink(StockProductPG r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"PG_Submit.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }
    protected string TD_Nomor(StockProductPG r)
    {
        string x = string.Empty;

        x += "<p class=\"text-md text-info\">" + r.Nomor + "</p>";

        return x;
    }
    protected string TD_Operator(StockProductPG r)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
        x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

        return x;
    }
    protected string TD_Product01(StockProductPG r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(StockProductPG r)
    {
        string x = string.Empty;


        x += r.CharFrameColor.Nama;

        return x;
    }
    protected string TD_Product03(StockProductPG r)
    {
        string x = string.Empty;


        x += r.CharGoldLevel.Nama;

        return x;
    }
    protected string TD_Image(StockProductPG r)
    {
        string x = string.Empty;

        x += "<img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" />";

        return x;
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {

            kw.Text = Request.QueryString["kw"];
        }
    }
}