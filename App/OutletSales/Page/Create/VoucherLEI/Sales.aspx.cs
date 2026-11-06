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
        var rs = from p in db.LocOutlet
                 orderby p.CompanyBrand.SN, p.Nama
                 where
                    !p.Disable &&
                    (p.Nama.Contains(kw.Text) || p.AddrAlamat.Contains(kw.Text))
                 select p;

        var am = from p in db.DataAreaManager
                 where p.IDUser == App.UserID
                 select p;
        if (am.Count() > 0)
        {
            int[] OutletID = DLC.AMOutlet(App.UserID);
            rs = rs.Where(p => OutletID.Contains(p.ID));
        }

        //SELECT
        var rs2 = from p in db.LocExhibition
                  orderby p.CompanyBrand.SN, p.Nama
                  where
                     !p.Disable &&
                     (p.Nama.Contains(kw.Text) || p.AddrAlamat.Contains(kw.Text))
                  select p;

        //NO-RESULT
        if (rs.Count() == 0 && rs2.Count() == 0) App.TableNoResult(tb);

        //RESULT
        int prev = 0;
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            //Sub-Header
            if (prev != r.IDBrand)
            {
                Sub_Header(r);
                prev = r.IDBrand;
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
            if (prev != r.IDBrand)
            {
                Sub_Header(r);
                prev = r.IDBrand;
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

	protected void Sub_Header(LocOutlet r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += r.CompanyBrand.Nama;

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(LocOutlet r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" href=\"Sales_Form.aspx?idlokasi=" + r.ID + "&tipe=1\">";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  Select";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(LocOutlet r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";

		return x;
	}
	protected string TD_Addr(LocOutlet r) {
		string x = string.Empty;

		if (r.AddrAlamat != "" && r.AddrAlamat != null)
			x += "<p class=\"text-sm\">" + Str.HtmlBreak(r.AddrAlamat) + "</p>";

		return x;
	}
	protected string TD_Group(LocOutlet r) {
		string x = string.Empty;

		x += "<p class=\"text-info\">" + r.LocOutletGroup.Nama + "</p>";

		return x;
	}

    protected void Sub_Header(LocExhibition r)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += r.CompanyBrand.Nama;

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected string TD_HyperLink(LocExhibition r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"Sales_Form.aspx?idlokasi=" + r.ID + "&tipe=2\">";
        x += "  <i class=\"icon-ok\"></i> ";
        x += "  Select";
        x += "</a>";

        return x;
    }
    protected string TD_Nama(LocExhibition r)
    {
        string x = string.Empty;

        x += "<p><strong>" + r.Nama + "</strong></p>";

        return x;
    }
    protected string TD_Addr(LocExhibition r)
    {
        string x = string.Empty;

        if (r.AddrAlamat != "" && r.AddrAlamat != null)
            x += "<p class=\"text-sm\">" + Str.HtmlBreak(r.AddrAlamat) + "</p>";

        return x;
    }
    protected string TD_Group(LocExhibition r)
    {
        string x = string.Empty;

        x += "<p class=\"text-info\">" + r.LocExhibitionGroup.Nama + "</p>";

        return x;
    }
    protected void CheckLokasi()
    {
        int UserID = App.UserID;
        var d = from p in db.DataAdminSales
                where p.IDUser == UserID
                select p;
        if (d.Count() > 0)
        {
            var x = d.First();
            string href = "";

            href = "Sales_Form.aspx?idlokasi=" + x.IDLokasi + "&tipe=" + x.TipeLokasi;
            Response.Redirect(href);
        }
    }
}