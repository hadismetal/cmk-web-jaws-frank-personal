using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Settings_Segmen : System.Web.UI.Page {
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

			//Role Protection
			App.TableRole(tb, Role.Operator);

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

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			category.SelectedIndex = Convert.ToInt32(Request.QueryString["category"]);
			level.SelectedIndex = Convert.ToInt32(Request.QueryString["level"]);
		}
	}

	//Display-Table
	protected void fillTable(object sender, EventArgs e) {
		startLooping();
	}
	protected void startLooping() {
		//WHERE
		string w = "1=1";
		if (category.SelectedIndex > 0) w += " AND ID = " + category.SelectedValue;

		//SELECT
		var rs = from p in db.CharProductCategory.Where(w)
				 where
					 !p.Disable
				 orderby p.SN
				 select p;

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		foreach (var r_category in rs) {
			if (!Response.IsClientConnected) break;
			Sub_Header(r_category);
			startLooping(r_category);
		}
	}
	protected void startLooping(CharProductCategory r_category) {
		//WHERE
		string w = "1=1";
		if (level.SelectedIndex > 0) w += " AND ID = " + level.SelectedValue;

		//SELECT
		var rs = from p in db.CharProductLevel.Where(w)
				 where
					!p.Disable
				 orderby p.SN
				 select p;

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		foreach (var r_level in rs) {
			if (!Response.IsClientConnected) break;
			fillTable(r_category, r_level);
		}
	}

	//Display-Row
	protected void Sub_Header(CharProductCategory r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += r.Nama + " (" + r.NamaKode + ")";

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected void fillTable(CharProductCategory r_category, CharProductLevel r_level) {
		TableRow tr = new TableRow();
		TableCell td;

		if (Role.Operator) {
			td = new TableCell();
			td.Text = TD_HyperLink(r_category, r_level);
			tr.Cells.Add(td);
		}

		td = new TableCell();
		td.Text = TD_Nama(r_level);
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = TD_Sistem(r_category, r_level);
		td.CssClass = "text-right";
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(CharProductCategory r_category, CharProductLevel r_level) {
		string x = string.Empty;
		string href = string.Empty;

		href += "SegmenUpdate.aspx";
		href += "?idcategory=" + r_category.ID;
		href += "&idlevel=" + r_level.ID;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"" + href + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(CharProductLevel r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + " (" + r.NamaKode + ")</strong></p>";

		return x;
	}
	protected string TD_Sistem(CharProductCategory r_category, CharProductLevel r_level) {
		string x = string.Empty;

		int? SS = Pricing.SegmenSistem(r_category.ID, r_level.ID);
		if (SS.HasValue) {
			string css = SS.Value == 0 ? "text-success" : "text-orange";
			x += "<p class=\"" + css + "\">" + DL.SegmenSistem(SS.Value) + "</p>";
		}

		return x;
	}
}