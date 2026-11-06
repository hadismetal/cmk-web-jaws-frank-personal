using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_SS : System.Web.UI.Page {
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
		App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
			//Role Protection
			App.TableRole(tb, Role.Operator);

            DLC.CharProductCategory(category);
            DLC.CharProductLevel(level);
            DLC.CharStoneDist(dist);

			//Generate
            fillFilter();
			fillTable(sender, e);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
        //SELECT
        string w = "1=1";
        if (category.SelectedIndex > 0) w += " AND IDCategory = " + category.SelectedValue;
        if (level.SelectedIndex > 0) w += " AND IDLevel = " + level.SelectedValue;
        if (dist.SelectedIndex > 0) w += " AND IDDist = " + dist.SelectedValue;

        var rs = from p in db.PricingTableSS.Where(w)
				 where p.StatusPricing == 2
				 orderby
					 p.CharProductCategory.SN,
					 p.CharProductLevel.SN,
					 p.CharStoneDist.SN,
					 p.CharProductSegmen.SN
				 select p;

		//NO-RESULT
		zero.Visible = rs.Count() == 0;
		form_area.Visible = !zero.Visible;

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

			if (Role.Operator) {
				td = new TableCell();
				td.Text = TD_HyperLink(r);
				tr.Cells.Add(td);
			}

			td = new TableCell();
			td.Text = TD_Nama(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Operator(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Minimum(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_MinimumPending(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Mutasi(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Sistem(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);

			//Process-ID
			process_id.Items.Add(r.ID.ToString());
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

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../UpdateProcess/SS.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
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
	protected string TD_Operator(PricingTableSS r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

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
	protected string TD_MinimumPending(PricingTableSS r) {
		string x = string.Empty;

		x += "<p class=\"text-danger text-lg\">";
		if (r.SegmenSistem == 0)
			x += Str.Number2(r.MinimumPending);
		else
			x += Str.Number3(r.MinimumPending);
		x += "</p>";

		return x;
	}
	protected string TD_Mutasi(PricingTableSS r) {
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

			if (r.SegmenSistem == 0)
				x += Str.Number2(r.Mutasi);
			else
				x += Str.Number3(r.Mutasi);

			x += "</p>";
		}

		return x;
	}
	protected string TD_Sistem(PricingTableSS r) {
		string x = string.Empty;

		string css = r.SegmenSistem == 0 ? "text-success" : "text-orange";
		x += "<p class=\"" + css + "\">" + DL.SegmenSistem(r.SegmenSistem) + "</p>";

		return x;
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var rs = from p in process_id.Items.Cast<ListItem>()
				 select p.Value;

		foreach (var r in rs) {
			int myID = Convert.ToInt32(r);

			var d = db.PricingTableSS.SingleOrDefault(
				p =>
				p.ID == myID &&
				p.StatusPricing == 2
				);

			if (d != null) {
				decimal? MinimumLama = d.Minimum;
				decimal? MinimumBaru = d.MinimumPending;

				d.Minimum = d.MinimumPending;
				d.MinimumPending = null;

				//Mutasi
				d.Mutasi = null;
				d.MutasiPersen = null;

				//Status
				d.StatusPricing = 0;

				//User-Info
				d.Approval = App.UserNama;
				d.ApprovalTgl = DateTime.Now;

				//Commit
				db.SubmitChanges();

				//Post-Process
				Log(d.ID, MinimumLama, MinimumBaru);
			}
		}

		//Redirect
		App.Reload(this);
	}
	protected void Log(int IDMaster, decimal? MinimumLama, decimal? MinimumBaru) {
		if (MinimumLama != MinimumBaru) {
			PricingLogSS n = new PricingLogSS();

			n.IDMaster = IDMaster;
			n.Tgl = DateTime.Now;
			n.Approval = App.UserNama;

			n.MinimumLama = MinimumLama.HasValue ? MinimumLama.Value : 0;
			n.MinimumBaru = MinimumBaru.HasValue ? MinimumBaru.Value : 0;

			//Commit
			db.PricingLogSS.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            category.SelectedIndex = Convert.ToInt32(Request.QueryString["category"]);
            level.SelectedIndex = Convert.ToInt32(Request.QueryString["level"]);
            dist.SelectedIndex = Convert.ToInt32(Request.QueryString["dist"]);
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr

        href += "&category=" + category.SelectedIndex;
        href += "&level=" + level.SelectedIndex;
        href += "&dist=" + dist.SelectedIndex;

        Response.Redirect(href);
    }
}