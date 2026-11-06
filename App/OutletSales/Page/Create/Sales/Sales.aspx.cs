using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Sales : System.Web.UI.Page {
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

	// carries CompanyBrand/LocOutletGroup alongside the row so Sub_Header/TD_Group
	// don't lazy-load those nav properties per row (was a query per distinct value per row)
	// (protected, not private: used as a parameter type by protected methods below)
	protected class LocOutletRow
	{
	    public LocOutlet Entity { get; set; }
	    public string BrandNama { get; set; }
	    public string GroupNama { get; set; }
	}
	protected class LocExhibitionRow
	{
	    public LocExhibition Entity { get; set; }
	    public string BrandNama { get; set; }
	    public string GroupNama { get; set; }
	}

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			//Generate
			fillFilter();
            CheckLokasi();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) 
    {
		string href = App.RefreshURL(this);

		//Customize-Qstr
		href += "&kw=" + kw.Text;

		Response.Redirect(href);
	}
	protected void fillFilter() 
    {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) 
    {
        //SELECT
        // project CompanyBrand/LocOutletGroup into the query so LINQ-to-SQL resolves
        // them via JOIN in the same round-trip, instead of lazy-loading them per row
        // later in Sub_Header/TD_Group (was N extra queries per row)
        var rsQuery = from p in db.LocOutlet
                 orderby p.CompanyBrand.SN, p.Nama
                 where
                    !p.Disable &&
                    (p.Nama.Contains(kw.Text) || p.AddrAlamat.Contains(kw.Text))
                 select new LocOutletRow { Entity = p, BrandNama = p.CompanyBrand.Nama, GroupNama = p.LocOutletGroup.Nama };

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rsQuery = rsQuery.Where(p => OutletID.Contains(p.Entity.ID));
        }

        //SELECT
        var rs2Query = from p in db.LocExhibition
                  orderby p.CompanyBrand.SN, p.Nama
                  where
                     !p.Disable &&
                     (p.Nama.Contains(kw.Text) || p.AddrAlamat.Contains(kw.Text))
                  select new LocExhibitionRow { Entity = p, BrandNama = p.CompanyBrand.Nama, GroupNama = p.LocExhibitionGroup.Nama };

        // materialize once - previously each query re-executed for Count() and again for foreach
        var rs = rsQuery.ToList();
        var rs2 = rs2Query.ToList();

        //NO-RESULT
        if (rs.Count == 0 && rs2.Count == 0) App.TableNoResult(tb);

        //RESULT
        int prev = 0;
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            //Sub-Header
            if (prev != r.Entity.IDBrand)
            {
                Sub_Header(r);
                prev = r.Entity.IDBrand;
            }

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(r);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<span class='pull-left' style='text-align:left;'>" + TD_Nama(r) + "</span>"
                    + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
                    + "<span class='text-muted pull-left' style='text-align:left;'>" + "STORE" + "</span>";
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Addr(r);
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Group(r);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

        //RESULT
        prev = 0;
        foreach (var r in rs2)
        {
            if (!Response.IsClientConnected) break;

            //Sub-Header
            if (prev != r.Entity.IDBrand)
            {
                Sub_Header(r);
                prev = r.Entity.IDBrand;
            }

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(r);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<span class='pull-left' style='text-align:left;'>" + TD_Nama(r) + "</span>"
                    + "<div style='clear:both; border-bottom:solid 1px silver;'></div>"
                    + "<span class='text-muted pull-left' style='text-align:left;'>" + "EXHIBITION" + "</span>";
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Addr(r);
            td.Style.Add("text-align", "left");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Group(r);
            td.Style.Add("text-align", "center");
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
        }

	}

	protected void Sub_Header(LocOutletRow r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += r.BrandNama;

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(LocOutletRow r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" href=\"Sales_Form.aspx?idlokasi=" + r.Entity.ID + "&tipe=1\">";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  Select";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(LocOutletRow r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Entity.Nama + "</strong></p>";

		return x;
	}
	protected string TD_Addr(LocOutletRow r) {
		string x = string.Empty;

		if (r.Entity.AddrAlamat != "" && r.Entity.AddrAlamat != null)
			x += "<p class=\"text-sm\">" + Str.HtmlBreak(r.Entity.AddrAlamat) + "</p>";

		return x;
	}
	protected string TD_Group(LocOutletRow r) {
		string x = string.Empty;

		x += "<p class=\"text-info\">" + r.GroupNama + "</p>";

		return x;
	}

    protected void Sub_Header(LocExhibitionRow r)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += r.BrandNama;

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected string TD_HyperLink(LocExhibitionRow r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"Sales_Form.aspx?idlokasi=" + r.Entity.ID + "&tipe=2\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(LocExhibitionRow r)
    {
        string x = string.Empty;

        x += "<p><strong>" + r.Entity.Nama + "</strong></p>";

        return x;
    }
    protected string TD_Addr(LocExhibitionRow r)
    {
        string x = string.Empty;

        if (r.Entity.AddrAlamat != "" && r.Entity.AddrAlamat != null)
            x += "<p class=\"text-sm\">" + Str.HtmlBreak(r.Entity.AddrAlamat) + "</p>";

        return x;
    }
    protected string TD_Group(LocExhibitionRow r)
    {
        string x = string.Empty;

        x += "<p class=\"text-info\">" + r.GroupNama + "</p>";

        return x;
    }
    protected void CheckLokasi()
    {
        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        // materialize once via FirstOrDefault instead of Count()+First() (2 round-trips -> 1)
        var x = d.FirstOrDefault();
        if (x != null)
        {
            string href = "";

            href = "Sales_Form.aspx?idlokasi=" + x.IDLokasi + "&tipe=" + x.TipeLokasi;
            Response.Redirect(href);
        }
    }
}