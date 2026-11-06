using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class AP_Diary : System.Web.UI.Page {
	private Map_AP _db;

	private Map_AP db
	{
	    get
	    {
	        if (_db == null)
	            _db = new Map_AP();
	        return _db;
	    }
	}

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    base.Dispose();
	}
	protected string myModule { get { return Request.QueryString["module"]; } }
	protected string myDataTable { get { return Request.QueryString["datatable"]; } }
	protected int myDataID { get { return App.QueryInt(this, "dataid"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(RoleAP.Open(myModule));

		if (!IsPostBack) {
			//Button-Create
			fillCreate();
			div_create.Visible = RoleAP.Create(myModule);

			//Generate
			fillFilter();
			fillTable(sender, e);
		}
	}
	protected void fillCreate() {
		string href = string.Empty;

		href += "?module=" + myModule;
		href += "&datatable=" + myDataTable;
		href += "&dataid=" + myDataID;

		create.HRef += href;
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&module=" + myModule;
		href += "&datatable=" + myDataTable;
		href += "&dataid=" + myDataID;

		//Customize-Qstr
		href += "&order=" + order.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			order.SelectedIndex = Convert.ToInt32(Request.QueryString["order"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//ORDER BY
		string o = string.Empty;
		if (order.SelectedIndex == 0) o = "ID ASC";
		if (order.SelectedIndex == 1) o = "ID Desc";

		//SELECT
		var rs = from p in db.Diary.OrderBy(o)
				 where
					p.Module == myModule &&
					p.DataTable == myDataTable &&
					p.DataID == myDataID
				 select p;

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//Role Protection
		bool CanDelete = RoleAP.Delete(myModule);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			tr.CssClass = "vertical-top";

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Catatan(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Author(r);
			tr.Cells.Add(td);

			td = new TableCell();
			if (CanDelete) td.Text = TD_Delete(r);
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_Catatan(Diary r) {
		string x = string.Empty;

		x += "<p>" + Str.HtmlBreak(r.Catatan) + "</p>";

		return x;
	}
	protected string TD_Author(Diary r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Author + "</strong></p>";
		x += "<p class=\"text-muted\">Tgl. " + Str.Tgl(r.Tgl) + "</p>";

		return x;
	}
	protected string TD_Delete(Diary r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-danger\" href=\"javascript:goDelete('" + r.ID + "')\">";
		x += "	<i class=\"icon-trash\"></i> ";
		x += "	Delete";
		x += "</a>";

		return x;
	}
}