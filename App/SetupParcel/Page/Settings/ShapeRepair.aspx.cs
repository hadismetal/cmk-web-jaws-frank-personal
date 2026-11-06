using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Settings_Shape01 : System.Web.UI.Page {
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
			//Role Protection
			App.TableRole(tb, Role.Operator);

			//Generate
			fillTable(sender, e);
		}
	}

	//Display-Table
	protected void fillTable(object sender, EventArgs e) {
		//SELECT
		var rs = from p in db.ParcelRepair01
				 where
					!p.Disable
				 orderby p.SN
				 select p;

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
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
			td.Text = TD_NamaKode(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nama(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_RoundShape(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(ParcelRepair01 r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"ShapeRepairUpdate.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
		x += "</a>";

		return x;
	}
	protected string TD_NamaKode(ParcelRepair01 r) {
		string x = string.Empty;

		x += "<p class=\"text-lg\">" + r.NamaKode + "</p>";

		return x;
	}
	protected string TD_Nama(ParcelRepair01 r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";

		return x;
	}
	protected string TD_RoundShape(ParcelRepair01 r) {
		string x = string.Empty;

		if (r.RoundShape.HasValue) {
			string css = r.RoundShape.Value ? "text-info" : "text-warning";
			x += "<p class=\"" + css + "\">" + DL.RoundShape(r.RoundShape.Value) + "</p>";
		}

		return x;
	}
}