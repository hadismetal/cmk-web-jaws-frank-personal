using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_Packaging : System.Web.UI.Page {
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

			//DLC
			DLC.CompanyBrand(brand);

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
		href += "&kw=" + kw.Text;
		href += "&brand=" + brand.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
			brand.SelectedIndex = Convert.ToInt32(Request.QueryString["brand"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		string w = "1=1";
		if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;

		//SELECT
		var rs = from p in db.LocWarehouse.Where(w)
				 orderby p.CompanyBrand.SN, p.Nama
				 where
					!p.Disable &&
					(p.Nama.Contains(kw.Text) || p.AddrAlamat.Contains(kw.Text))
				 select p;

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		int prev = 0;
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			//Sub-Header
			if (prev != r.IDBrand) {
				Sub_Header(r);
				prev = r.IDBrand;
			}

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
			td.Text = TD_Addr(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Group(r);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected void Sub_Header(LocWarehouse r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += r.CompanyBrand.Nama + " (" + r.CompanyBrand.NamaKode + ")";

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(LocWarehouse r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" href=\"Product.aspx?idwarehouse=" + r.ID + "\">";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  Select";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(LocWarehouse r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";

		return x;
	}
	protected string TD_Addr(LocWarehouse r) {
		string x = string.Empty;

		if (r.AddrAlamat != "" && r.AddrAlamat != null)
			x += "<p class=\"text-sm\">" + Str.HtmlBreak(r.AddrAlamat) + "</p>";

		return x;
	}
	protected string TD_Group(LocWarehouse r) {
		string x = string.Empty;

		x += "<p class=\"text-info\">" + r.LocWarehouseGroup.NamaKode + "</p>";

		return x;
	}
}