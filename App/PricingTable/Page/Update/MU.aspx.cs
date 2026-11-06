using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Update_MU : System.Web.UI.Page {
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
			//DLC
			DLC.CharProductCategory(category);
			DLC.CharProductLevel(level);
			DLC.CharStoneDist(dist);
			DLC.StatusPricing(status);

			//Generate
			fillFilter();
			fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
		href += "&category=" + category.SelectedIndex;
		href += "&level=" + level.SelectedIndex;
		href += "&dist=" + dist.SelectedIndex;
		href += "&status=" + status.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			category.SelectedIndex = Convert.ToInt32(Request.QueryString["category"]);
			level.SelectedIndex = Convert.ToInt32(Request.QueryString["level"]);
			dist.SelectedIndex = Convert.ToInt32(Request.QueryString["dist"]);
			status.SelectedIndex = Convert.ToInt32(Request.QueryString["status"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		string w = "1=1";
		if (category.SelectedIndex > 0) w += " AND IDCategory = " + category.SelectedValue;
		if (level.SelectedIndex > 0) w += " AND IDLevel = " + level.SelectedValue;
		if (dist.SelectedIndex > 0) w += " AND IDDist = " + dist.SelectedValue;
		if (status.SelectedIndex > 0) w += " AND StatusPricing = " + status.SelectedValue;

		//SELECT
		var rs = from p in db.PricingTableMU.Where(w)
				 orderby
					 p.CharProductCategory.SN,
					 p.CharProductLevel.SN,
					 p.CharStoneDist.SN
				 select p;

		//RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		string prev = "";
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			//Sub-Header
			if (prev != r.IDCategory + " " + r.IDLevel) {
				Sub_Header(r);
				prev = r.IDCategory + " " + r.IDLevel;
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
			td.Text = TD_Persen(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_PersenPending(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Mutasi(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected void Sub_Header(PricingTableMU r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += "<span class=\"text-orange\">";
		x += r.CharProductCategory.Nama + " (" + r.CharProductCategory.NamaKode + ")";
		x += "</span>";
		x += "<br />";
		x += r.CharProductLevel.Nama + " (" + r.CharProductLevel.NamaKode + ")";

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(PricingTableMU r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../UpdateProcess/MU.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(PricingTableMU r) {
		string x = string.Empty;

		x += "<p>";
		x += "<strong>";
		x += r.CharStoneDist.Nama + " (" + r.CharStoneDist.NamaKode + ")";
		x += "</strong>";
		x += "</p>";

		return x;
	}
	protected string TD_Operator(PricingTableMU r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
	protected string TD_Persen(PricingTableMU r) {
		string x = string.Empty;

		x += "<p class=\"text-success text-lg\">";
		x += Str.Number2(r.Persen);
		x += "</p>";

		return x;
	}
	protected string TD_PersenPending(PricingTableMU r) {
		string x = string.Empty;

		x += "<p class=\"text-danger text-lg\">";
		x += Str.Number2(r.PersenPending);
		x += "</p>";

		return x;
	}
	protected string TD_Mutasi(PricingTableMU r) {
		string x = string.Empty;

		if (r.Mutasi.HasValue) {
			string sgn = r.Mutasi > 0 ? "+" : "";
			string css = r.Mutasi > 10 || r.Mutasi < -10 ? "text-red" : "";

			x += "<p class=\"" + css + " text-lg\">";
			x += sgn;
			x += Str.Number2(r.Mutasi);
			x += "%";
			x += "</p>";
		}

		return x;
	}
}