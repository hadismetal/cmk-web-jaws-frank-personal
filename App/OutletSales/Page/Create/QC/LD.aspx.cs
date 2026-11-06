using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_LD : System.Web.UI.Page {
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

		if (!IsPostBack) {

			//Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {

		//SELECT
		var rs = (from p in db.StockProductLD_Stone1B
				  where !p.StockProductLD.Draft.Value
                    && p.Nomor.Contains(kw.Text.Trim())
                    && p.Nomor != null
                    && p.Nomor != ""
                  select p);
        
		var so = (from p in db.SalesOrderLD
				  where !p.StockProductLD_Stone1B.StockProductLD.Draft.Value
                    && p.StockProductLD_Stone1B.Nomor.Contains(kw.Text.Trim())
					&& p.SalesOrder.StatusPembayaran == true
					&& p.SalesOrder.StatusVoid == false
				  select p);
		
		//RESULT
		int prev = 0;
		int index = 1;
        if (rs.Count() > 0 && so.Count() > 0)
        {
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
                td.Text = TD_Image(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Nomor(r);
                tr.Cells.Add(td);

                tb.Rows.Add(tr);
            }
        }
	}
    protected string TD_HyperLink(StockProductLD_Stone1B r)
    {
		string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"LD_Submit.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "  Open";
		x += "</a>";

		return x;
	}
    protected string TD_Nomor(StockProductLD_Stone1B r)
    {
		string x = string.Empty;

		x += "<p class=\"text-md text-info\">" + r.Nomor + "</p>";

		return x;
	}


    protected string TD_Image(StockProductLD_Stone1B r)
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