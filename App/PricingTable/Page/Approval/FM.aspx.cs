using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_FM : System.Web.UI.Page {
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
		var rs = from p in db.PricingTableFM
				 where p.StatusPricing == 2
				 orderby
					 p.CompanyBrand.SN,
					 p.CharProductItem.SN,
					 p.CharProcessCons.SN
				 select p;

		//NO-RESULT
		zero.Visible = rs.Count() == 0;
		form_area.Visible = !zero.Visible;

		//RESULT
		string prev = "";
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			//Sub-Header
			if (prev != r.IDBrand + " " + r.IDItem) {
				Sub_Header(r);
				prev = r.IDBrand + " " + r.IDItem;
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
            td.Text = TD_Pembagi(r);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_PembagiPending(r);
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
	protected void Sub_Header(PricingTableFM r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += "<span class=\"text-orange\">";
		x += r.CompanyBrand.Nama + " (" + r.CompanyBrand.NamaKode + ")";
		x += "</span class=\"text-orange\">";
		x += "<br />";
		x += r.CharProductItem.Nama + " (" + r.CharProductItem.NamaKode + ")";

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(PricingTableFM r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../UpdateProcess/FM.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(PricingTableFM r) {
		string x = string.Empty;

		x += "<p>";
		x += "<strong>";
		x += r.CharProcessCons.Nama + " (" + r.CharProcessCons.NamaKode + ")";
		x += "</strong>";
		x += "</p>";

		return x;
	}
	protected string TD_Operator(PricingTableFM r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
    protected string TD_Pembagi(PricingTableFM r)
    {
        string x = string.Empty;

        x += "<p class=\"text-success\">" + Str.Number0(r.Pembagi) + "</p>";

        return x;
    }
    protected string TD_PembagiPending(PricingTableFM r)
    {
        string x = string.Empty;

        x += "<p class=\"text-danger\">" + Str.Number0(r.PendingPembagi) + "</p>";

        return x;
    }
	protected string TD_Biaya(PricingTableFM r) {
		string x = string.Empty;

		x += "<p class=\"text-success\">" + Str.Number0(r.InputNO) + "</p>";
		x += "<p class=\"text-success\">" + Str.Number0(r.InputRO) + "</p>";

		return x;
	}
	protected string TD_BiayaPending(PricingTableFM r) {
		string x = string.Empty;

		x += "<p class=\"text-danger\">" + Str.Number0(r.PendingNO) + "</p>";
		x += "<p class=\"text-danger\">" + Str.Number0(r.PendingRO) + "</p>";

		return x;
	}
	protected string TD_Mutasi(PricingTableFM r) {
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

			var d = db.PricingTableFM.SingleOrDefault(
				p =>
				p.ID == myID &&
				p.StatusPricing == 2
				);

			if (d != null) {
				decimal? BiayaLama = d.Biaya;
				decimal? BiayaBaru = d.BiayaPending;
                decimal? PembagiLama = d.Pembagi;
                decimal? PembagiBaru = d.PendingPembagi;

				d.Biaya = d.BiayaPending;
				d.BiayaPending = null;

                d.Pembagi = d.PendingPembagi;
                d.PendingPembagi = null;

				d.InputNO = d.PendingNO; d.PendingNO = null;
				d.InputRO = d.PendingRO; d.PendingRO = null;

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
				Log(d.ID, BiayaLama, BiayaBaru, PembagiLama, PembagiBaru);
			}
		}

		//Redirect
		App.Reload(this);
	}
	protected void Log(int IDMaster, decimal? BiayaLama, decimal? BiayaBaru, decimal? PembagiLama, decimal? PembagiBaru) {
		if (BiayaLama != BiayaBaru) {
			PricingLogFM n = new PricingLogFM();

			n.IDMaster = IDMaster;
			n.Tgl = DateTime.Now;
			n.Approval = App.UserNama;

			n.BiayaLama = BiayaLama.HasValue ? BiayaLama.Value : 0;
			n.BiayaBaru = BiayaBaru.HasValue ? BiayaBaru.Value : 0;
            n.PembagiLama = PembagiLama.HasValue ? PembagiLama.Value : 0;
            n.PembagiBaru = PembagiBaru.HasValue ? PembagiBaru.Value : 0;

			//Commit
			db.PricingLogFM.InsertOnSubmit(n);
			db.SubmitChanges();
		}
	}
}