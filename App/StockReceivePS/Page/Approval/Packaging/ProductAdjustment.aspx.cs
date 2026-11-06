using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_Packaging : System.Web.UI.Page {
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
			//Generate
			fillTable(sender, e);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//SELECT
		var rs = from p in db.AdjustmentPackaging
				 where p.Draft.Value == false && p.Approval == null
				 orderby p.OperatorTgl descending
				 select p;

		//NO-RESULT
		zero.Visible = rs.Count() == 0;
		form_area.Visible = !zero.Visible;

		//RESULT
		int prev = 0;
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;


			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = TD_HyperLink(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Operator(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Form(r.ID.ToString());
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.StatusAdjustment == 0 ? "In Hand" : r.StatusAdjustment == 1 ? "Broken" : "Lost";
            tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(AdjustmentPackaging r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"ProductAdjustment_Form.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "  Open";
		x += "</a>";

		return x;
	}
	protected string TD_Operator(AdjustmentPackaging r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
	protected string TD_Form(string Text) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + Text + "</strong></p>";

		return x;
	}
}