using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Settings_Size01 : System.Web.UI.Page {
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
		var rs = from p in db.Parcel102
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
			td.Text = TD_SizeMin(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_SizeMax(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Rapaport(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(Parcel102 r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Size01Update.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
		x += "</a>";

		return x;
	}
	protected string TD_NamaKode(Parcel102 r) {
		string x = string.Empty;

		x += "<p class=\"text-lg\">" + r.NamaKode + "</p>";

		return x;
	}
	protected string TD_Nama(Parcel102 r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";

		return x;
	}
	protected string TD_SizeMin(Parcel102 r) {
		string x = string.Empty;

		x += "<p>" + Convert.ToDecimal(r.SizeMin).ToString("N6") + "</p>";

		return x;
	}
	protected string TD_SizeMax(Parcel102 r) {
		string x = string.Empty;

		x += "<p>" + Convert.ToDecimal(r.SizeMax).ToString("N6") + "</p>";

		return x;
	}
	protected string TD_Rapaport(Parcel102 r) {
		string x = string.Empty;

		if (r.Rapaport.HasValue) {
			string css = r.Rapaport.Value ? "text-info" : "text-warning";
			x += "<p class=\"" + css + "\">" + DL.Rapaport(r.Rapaport.Value) + "</p>";
		}

		return x;
	}
}