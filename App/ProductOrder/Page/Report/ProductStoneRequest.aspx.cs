using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_ProductStoneRequest : System.Web.UI.Page {
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
            fillTable(sender, e);
        }
	}

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        var rs = from p in db.ProductStoneRequest
                 where p.Approval != null
                 orderby p.Tgl descending
                 select p;

        //RESULT
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
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Operator(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_FPS(r);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.TglJatuhTempo);
            tr.Cells.Add(td);


            tb.Rows.Add(tr);
        }
    }


    protected string TD_HyperLink(ProductStoneRequest r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"ProductStoneRequest_Print.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }

    protected string TD_FPS(ProductStoneRequest r)
    {
        string x = string.Empty;

        x = r.NomorDokumen;

        return x;
    }
    protected string TD_Operator(ProductStoneRequest r)
    {
        string x = string.Empty;

        x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
        x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

        return x;
    }
}