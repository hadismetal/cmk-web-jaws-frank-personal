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
			DLC.CharProductItem(item);
			DLC.CharProductLevel(level);
			DLC.CharStoneDist(dist);
			DLC.CharProductSegmen(segmen);

			//Generate
			fillFilter();
			fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
		href += "&item=" + item.SelectedIndex;
		href += "&level=" + level.SelectedIndex;
		href += "&dist=" + dist.SelectedIndex;
		href += "&segmen=" + segmen.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			item.SelectedIndex = Convert.ToInt32(Request.QueryString["item"]);
			level.SelectedIndex = Convert.ToInt32(Request.QueryString["level"]);
			dist.SelectedIndex = Convert.ToInt32(Request.QueryString["dist"]);
			segmen.SelectedIndex = Convert.ToInt32(Request.QueryString["segmen"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		string w = "1=1";
		if (item.SelectedIndex > 0) w += " AND ProductItem = " + item.SelectedValue;
		if (level.SelectedIndex > 0) w += " AND ProductLevel = " + level.SelectedValue;
		if (dist.SelectedIndex > 0) w += " AND StineDist = " + dist.SelectedValue;
		if (status.SelectedIndex > 0) w += " AND Segmen = " + segmen.SelectedValue;

		//SELECT
		var rs = from p in db.BudgetStock.Where(w)
				 orderby
					 p.CharProductLevel.SN,
					 //p.CharStoneDist.SN,
                     p.CharProductSegmen.SN,
                     p.CharProductItem.SN
				 select p;

		//RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		string prev = "";
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			//Sub-Header
			//if (prev != r.ProductLevel + " " + r.StoneDist + " "  +  r.Segmen) {
			//	Sub_Header(r);
            //    prev = r.ProductLevel + " " + r.StoneDist + " " + r.Segmen;
			//}

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
			td.Text = r.TargetQty.HasValue ? r.TargetQty.Value.ToString("N0") : "" ;
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = r.TargetRupiah.HasValue ? r.TargetRupiah.Value.ToString("N0") : "";
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected void Sub_Header(BudgetStock r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += "<span class=\"text-orange\">";
		x += r.CharProductLevel.Nama + " (" + r.CharProductLevel.NamaKode + ")";
		x += "</span>";
		x += "<br />";
		//x += r.CharStoneDist.Nama + " (" + r.CharStoneDist.NamaKode + ")";
        x += "<br />";
        x += r.CharProductItem.Nama + " (" + r.CharProductItem.NamaKode + ")";

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(BudgetStock r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"BudgetStock_Update.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(BudgetStock r) {
		string x = string.Empty;

		x += "<p>";
		x += "<strong>";
        x += r.CharProductItem.Nama + " (" + r.CharProductItem.NamaKode + ")";
		x += "</strong>";
		x += "</p>";

		return x;
	}
	protected string TD_Operator(BudgetStock r) {
		string x = string.Empty;

		//x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		//x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
}