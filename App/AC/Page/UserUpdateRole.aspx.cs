using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_UserUpdateRole : System.Web.UI.Page {
	private Map_AC _db;

	private Map_AC db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_AC();
	        return _db;
	    }
	}

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    base.Dispose();
	}
	protected int myID { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			create.HRef += "?id=" + myID;

			fillForm();
			fillModule();
		}
	}
	protected void fillForm() {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);
		}
	}
	protected void fillModule() {
		//SELECT
		var rs = from p in db.ModuleIndex
				 orderby p.ID
				 select p;

		//RESULT
		int prev = 0;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			var d = db.UserRole.SingleOrDefault(p => p.ID == myID && p.Module == r.Kode);
			if (d != null) {
				//Sub-Header
				if (prev != r.IDGroup) {
					Sub_Header(r);
					prev = r.IDGroup;
				}

				TableRow tr = new TableRow();
				TableCell td;

				td = new TableCell();
                td.Text = TD_HyperLink(r);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "middle");
				tr.Cells.Add(td);

				td = new TableCell();
                td.Text = TD_Nama(r);
                td.Style.Add("text-align", "left");
                td.Style.Add("vertical-align", "middle");
				tr.Cells.Add(td);

				td = new TableCell();
                td.Text = TD_Bool(d.Manager);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "middle");
				tr.Cells.Add(td);

				td = new TableCell();
                td.Text = TD_Bool(d.Operator);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "middle");
				tr.Cells.Add(td);

				td = new TableCell();
                td.Text = TD_Bool(d.DownloadFile);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "middle");
				tr.Cells.Add(td);

				td = new TableCell();
                td.Text = TD_Bool(d.DownloadExcel);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "middle");
				tr.Cells.Add(td);

				tb.Rows.Add(tr);
			}
		}
	}
	protected void Sub_Header(ModuleIndex r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += r.ModuleGroup.Nama;

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(ModuleIndex r) {
		string x = string.Empty;

		x += "<div class=\"btn-group\">";
		x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"RoleUserUpdate.aspx?id=" + myID + "&km=" + r.Kode + "\">";
		x += "      <i class=\"icon-pencil\"></i> ";
		x += "      Update";
		x += "  </a>";
		x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + myID + "','" + r.Kode + "')\">";
		x += "      <i class=\"icon-trash\"></i> ";
		x += "      Delete";
		x += "  </a>";
		x += "</div>";

		return x;
	}
	protected string TD_Nama(ModuleIndex r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";

		return x;
	}
	protected string TD_Bool(bool ok) {
		string x = string.Empty;

		string css = ok ? "icon-ok text-success" : "icon-remove text-silver";
		x += "<p><i class=\"icon-2x " + css + "\"></i></p>";

		return x;
	}
}