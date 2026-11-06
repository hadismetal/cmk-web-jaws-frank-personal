using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_FF : System.Web.UI.Page {
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

			//Generate
			fillTable(sender, e);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//SELECT
		var rs = from p in db.PricingTableRepair
				 where p.StatusPricing == 2
				 orderby
					 p.CharProcessRepair.SN
				 select p;

		//NO-RESULT
		zero.Visible = rs.Count() == 0;
		form_area.Visible = !zero.Visible;

		//RESULT
		int prev = 0;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			if (Role.Operator) {
				td = new TableCell();
				td.Text = TD_HyperLink(r);
				tr.Cells.Add(td);
			}

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

			//Process-ID
			process_id.Items.Add(r.ID.ToString());
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

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var rs = from p in process_id.Items.Cast<ListItem>()
				 select p.Value;

		foreach (var r in rs) {
			int myID = Convert.ToInt32(r);

			var d = db.PricingTableRepair.SingleOrDefault(
				p =>
				p.ID == myID &&
				p.StatusPricing == 2
				);

			if (d != null) {
				decimal? BiayaLama = d.Biaya;
				decimal? BiayaBaru = d.BiayaPending;
                decimal? CostingLama = d.Costing;
                decimal? CostingBaru = d.CostingPending;

				d.Biaya = d.BiayaPending;
				d.BiayaPending = null;
                d.Costing = d.CostingPending;
                d.CostingPending = null;

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
				Log(d.ID, BiayaLama, BiayaBaru, CostingLama, CostingBaru);
			}
		}

		//Redirect
		App.Reload(this);
	}
	protected void Log(int IDMaster, decimal? BiayaLama, decimal? BiayaBaru, decimal? CostingLama, decimal? CostingBaru) {
		if (BiayaLama != BiayaBaru || CostingLama != CostingBaru) {
			PricingLogRepair n = new PricingLogRepair();

			n.IDMaster = IDMaster;
			n.Tgl = DateTime.Now;
			n.Approval = App.UserNama;

			n.BiayaLama = BiayaLama.HasValue ? BiayaLama.Value : 0;
			n.BiayaBaru = BiayaBaru.HasValue ? BiayaBaru.Value : 0;
            n.CostingLama = CostingLama.HasValue ? CostingLama.Value : 0;
            n.CostingBaru = CostingBaru.HasValue ? CostingBaru.Value : 0;

			//Commit
			db.PricingLogRepair.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
}