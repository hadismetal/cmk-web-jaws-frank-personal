using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Role : System.Web.UI.Page {
	private Map_AC _db;

	private Map_AC db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_AC();
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
			DLC.ModuleGroup(group);

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
		href += "&group=" + group.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			group.SelectedIndex = Convert.ToInt32(Request.QueryString["group"]);
		}
	}

	//Generate
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		string w = "1=1";
		if (group.SelectedIndex > 0) w += " AND IDGroup = " + group.SelectedValue;

		//SELECT
		var rs = from p in db.ModuleIndex.Where(w)
				 orderby p.IDGroup
				 select p;

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		int prev = 0;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			//Sub-Header
			if (prev != r.IDGroup) {
				Sub_Header(r);
				prev = r.IDGroup;
			}

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
            td.Text = TD_HyperLink(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Nama(r);
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Manager(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Operator(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Visitor(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Total(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected void Sub_Header(ModuleIndex r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += r.ModuleGroup.Nama;

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(ModuleIndex r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" href=\"RoleDetail.aspx?km=" + r.Kode + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "  Detail";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(ModuleIndex r) {
		string x = string.Empty;

        x += "<p><strong>" + r.Nama + "</strong></p>";

		return x;
	}
	protected string TD_Manager(ModuleIndex r) {
		string x = string.Empty;

		int count = db.UserRole.Count(p => p.UserAccount.Status == 1 && p.Module == r.Kode && p.Manager);
		if (count > 0)
			x += "<p class=\"text-lg text-danger\">" + Str.Integer(count) + "</p>";

		return x;
	}
	protected string TD_Operator(ModuleIndex r) {
		string x = string.Empty;

		int count = db.UserRole.Count(p => p.UserAccount.Status == 1 && p.Module == r.Kode && p.Operator);
		if (count > 0)
			x += "<p class=\"text-lg text-orange\">" + Str.Integer(count) + "</p>";

		return x;
	}
	protected string TD_Visitor(ModuleIndex r) {
		string x = string.Empty;

		int count = db.UserRole.Count(p => p.UserAccount.Status == 1 && p.Module == r.Kode && !p.Operator && !p.Manager);
		if (count > 0)
			x += "<p class=\"text-lg text-info\">" + Str.Integer(count) + "</p>";

		return x;
	}
	protected string TD_Total(ModuleIndex r) {
		string x = string.Empty;

		int count = db.UserRole.Count(p => p.UserAccount.Status == 1 && p.Module == r.Kode);
		if (count > 0)
			x += "<p class=\"text-lg text-success\">" + Str.Integer(count) + "</p>";
		else
			x += "<p class=\"text-red\">Tanpa User</p>";

		return x;
	}
}