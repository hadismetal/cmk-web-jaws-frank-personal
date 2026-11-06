using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class AP_LogAudit : System.Web.UI.Page {
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
		App.ProtectRole(RoleAP.OpenLog(myModule));

		if (!IsPostBack) {
			//Generate
			fillFilter();
			fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&module=" + myModule;
		href += "&datatable=" + myDataTable;
		href += "&dataid=" + myDataID;

		//Customize-Qstr
		href += "&format=" + format.SelectedIndex;
		href += "&order=" + order.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			format.SelectedIndex = Convert.ToInt32(Request.QueryString["format"]);
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
		var rs = from p in db.LogAudit.OrderBy(o)
				 where
					p.Module == myModule &&
					p.DataTable == myDataTable &&
					p.DataID == myDataID
				 select p;

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//ALTER-CSS
		tb.CssClass = format.SelectedIndex == 0 ? "table-report" : "table-list";

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			if (format.SelectedIndex == 0) {
				tr.CssClass = "";

				td = new TableCell();
				td.Text = index++.ToString() + ".";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = Str.Tgl(r.Tgl);
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = r.UserAccount;
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = r.Aktivitas;
				tr.Cells.Add(td);
			}
			else {
				tr.CssClass = "vertical-top";

				td = new TableCell();
				td.Text = "<p>" + index++.ToString() + ".</p>";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = TD_Tgl(r);
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = TD_UserAccount(r);
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = TD_Keterangan(r);
				tr.Cells.Add(td);
			}

			tb.Rows.Add(tr);
		}
	}
	protected string TD_Tgl(LogAudit r) {
		string x = string.Empty;

		x += "<p>" + Str.Tgl(r.Tgl) + "</p>";
		x += "<p>" + Str.Jam(r.Tgl) + "</p>";

		return x;
	}
	protected string TD_UserAccount(LogAudit r) {
		string x = string.Empty;

		x += "<p><strong>" + r.UserAccount + "</strong></p>";

		return x;
	}
	protected string TD_Keterangan(LogAudit r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Aktivitas + "</strong></p>";
		if (r.MutasiData != "") {
			x += "<hr />";
			x += "<p>" + Str.HtmlBreak(r.MutasiData) + "</p>";
		}

		return x;
	}
}