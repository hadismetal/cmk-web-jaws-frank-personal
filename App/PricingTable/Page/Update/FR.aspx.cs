using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Update_FF : System.Web.UI.Page {
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
        href += "&status=" + status.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			status.SelectedIndex = Convert.ToInt32(Request.QueryString["status"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		string w = "1=1";
		if (status.SelectedIndex > 0) w += " AND StatusPricing = " + status.SelectedValue;

		//SELECT
		var rs = from p in db.PricingTableRepair.Where(w)
				 orderby
					 p.CharProcessRepair.SN
				 select p;

		//RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		int prev = 0;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

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
            td.Text = TD_M(r);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_MPending(r);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Biaya(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_BiayaPending(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Mutasi(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected void Sub_Header(PricingTableRepair r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += r.CharProcessRepair.Nama + " (" + r.CharProcessRepair.NamaKode + ")";

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(PricingTableRepair r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../UpdateProcess/FR.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(PricingTableRepair r) {
		string x = string.Empty;

		x += "<p>";
		x += "<strong>";
        x += r.CharProcessRepair.Nama + " (" + r.CharProcessRepair.NamaKode + ")";
		x += "</strong>";
		x += "</p>";

		return x;
	}
	protected string TD_Operator(PricingTableRepair r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
	protected string TD_Biaya(PricingTableRepair r) {
		string x = string.Empty;

		x += "<p class=\"text-success text-lg\">";
		x += Str.Number0(r.Biaya);
		x += "</p>";

		return x;
	}
	protected string TD_BiayaPending(PricingTableRepair r) {
		string x = string.Empty;

		x += "<p class=\"text-danger text-lg\">";
		x += Str.Number0(r.BiayaPending);
		x += "</p>";

		return x;
	}
    protected string TD_M(PricingTableRepair r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success text-lg\">";
        x += Str.Number0(r.Costing.HasValue ? r.Costing.Value : 0);
        x += "</p>";

        return x;
    }
    protected string TD_MPending(PricingTableRepair r)
    {
        string x = string.Empty;

        x += "<p class=\"text-danger text-lg\">";
        x += Str.Number0(r.CostingPending.HasValue ? r.CostingPending.Value : 0);
        x += "</p>";

        return x;
    }
	protected string TD_Mutasi(PricingTableRepair r) {
		string x = string.Empty;

		if (r.MutasiPersen.HasValue) {
			string sgn = r.MutasiPersen > 0 ? "+" : "";
			string css = r.MutasiPersen > 10 || r.MutasiPersen < -10 ? "text-red" : "";

			x += "<p class=\"" + css + " text-lg\">";
			x += sgn;
			x += Str.Number2(r.MutasiPersen);
			x += "%";
			x += "</p>";
		}
		else if (r.Mutasi.HasValue) {
			string sgn = r.Mutasi > 0 ? "+" : "";

			x += "<p class=\"text-lg\">";
			x += sgn;
			x += Str.Number0(r.Mutasi);
			x += "</p>";
		}

		return x;
	}
}