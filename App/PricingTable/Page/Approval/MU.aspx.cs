using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_MU : System.Web.UI.Page {
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

        var rs = from p in db.PricingTableMU.Where(w)
				 where p.StatusPricing == 2
				 orderby
					 p.CharProductCategory.SN,
					 p.CharProductLevel.SN,
					 p.CharStoneDist.SN
				 select p;

		//NO-RESULT
		zero.Visible = rs.Count() == 0;
		form_area.Visible = !zero.Visible;

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

			//Process-ID
			process_id.Items.Add(r.ID.ToString());
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

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var rs = from p in process_id.Items.Cast<ListItem>()
				 select p.Value;

		foreach (var r in rs) {
			int myID = Convert.ToInt32(r);

			var d = db.PricingTableMU.SingleOrDefault(
				p =>
				p.ID == myID &&
				p.StatusPricing == 2
				);

			if (d != null) {
				decimal? PersenLama = d.Persen;
				decimal? PersenBaru = d.PersenPending;

				d.Persen = d.PersenPending;
				d.PersenPending = null;

				//Mutasi
				d.Mutasi = null;

				//Status
				d.StatusPricing = 0;

				//User-Info
				d.Approval = App.UserNama;
				d.ApprovalTgl = DateTime.Now;

				//Commit
				db.SubmitChanges();

				//Post-Process
				Log(d.ID, PersenLama, PersenBaru);
			}
		}

		//Redirect
		App.Reload(this);
	}
	protected void Log(int IDMaster, decimal? PersenLama, decimal? PersenBaru) {
		if (PersenLama != PersenBaru) {
			PricingLogMU n = new PricingLogMU();

			n.IDMaster = IDMaster;
			n.Tgl = DateTime.Now;
			n.Approval = App.UserNama;

			n.PersenLama = PersenLama.HasValue ? PersenLama.Value : 0;
			n.PersenBaru = PersenBaru.HasValue ? PersenBaru.Value : 0;

			//Commit
			db.PricingLogMU.InsertOnSubmit(n);
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