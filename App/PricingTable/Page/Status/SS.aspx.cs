using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Status_SS : System.Web.UI.Page {
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
			//DLC
			DLC.CharProductCategory(category);
			DLC.CharProductLevel(level);
			DLC.CharStoneDist(dist);
			DLC.CharProductSegmen(segmen);
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
		href += "&segmen=" + segmen.SelectedIndex;
		href += "&status=" + status.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			category.SelectedIndex = Convert.ToInt32(Request.QueryString["category"]);
			level.SelectedIndex = Convert.ToInt32(Request.QueryString["level"]);
			dist.SelectedIndex = Convert.ToInt32(Request.QueryString["dist"]);
			segmen.SelectedIndex = Convert.ToInt32(Request.QueryString["segmen"]);
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
		if (segmen.SelectedIndex > 0) w += " AND IDSegmen = " + segmen.SelectedValue;
		if (status.SelectedIndex > 0) w += " AND StatusPricing = " + status.SelectedValue;

		//SELECT
		var rs = from p in db.PricingTableSS.Where(w)
				 orderby
					 p.CharProductCategory.SN,
					 p.CharProductLevel.SN,
					 p.CharStoneDist.SN,
					 p.CharProductSegmen.SN
				 select p;

		//RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		string prev = "";
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			//Sub-Header
			if (prev != r.IDCategory + " " + r.IDLevel + " " + r.IDDist) {
				Sub_Header(r);
				prev = r.IDCategory + " " + r.IDLevel + " " + r.IDDist;
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
			td.Text = TD_Approval(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Minimum(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Sistem(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected void Sub_Header(PricingTableSS r) {
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
		x += "<br />";
		x += "<span class=\"text-validator\">";
		x += r.CharStoneDist.Nama + " (" + r.CharStoneDist.NamaKode + ")";
		x += "</span>";

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(PricingTableSS r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../StatusLog/SS.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "  Detail";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(PricingTableSS r) {
		string x = string.Empty;

		x += "<p>";
		x += "<strong>";
		x += r.CharProductSegmen.Nama + " (" + r.CharProductSegmen.NamaKode + ")";
		x += "</strong>";
		x += "</p>";

		return x;
	}
	protected string TD_Approval(PricingTableSS r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Approval + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.ApprovalTgl) + "</p>";

		return x;
	}
	protected string TD_Minimum(PricingTableSS r) {
		string x = string.Empty;

		x += "<p class=\"text-success text-lg\">";
		if (r.SegmenSistem == 0)
			x += Str.Number2(r.Minimum);
		else
			x += Str.Number3(r.Minimum);
		x += "</p>";

		return x;
	}
	protected string TD_Sistem(PricingTableSS r) {
		string x = string.Empty;

		string css = r.SegmenSistem == 0 ? "text-success" : "text-orange";
		x += "<p class=\"" + css + "\">" + DL.SegmenSistem(r.SegmenSistem) + "</p>";

		return x;
	}
}