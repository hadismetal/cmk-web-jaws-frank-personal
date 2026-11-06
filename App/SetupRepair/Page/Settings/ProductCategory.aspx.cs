using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Settings_ProductCategory : System.Web.UI.Page {
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
		var rs = from p in db.CharProductCategory
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
			td.Text = TD_Nama(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_KodeProto(r);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(CharProductCategory r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"ProductCategoryUpdate.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(CharProductCategory r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + " (" + r.NamaKode + ")</strong></p>";

		return x;
	}
	protected string TD_KodeProto(CharProductCategory r) {
		string x = string.Empty;

		x += "<p class=\"text-lg\">" + r.KodeProto + "</p>";

		return x;
	}
}