using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_RoleUserAdd : System.Web.UI.Page {
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
	protected string myKode { get { return Request.QueryString["km"]; } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			fillHeader();

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}
	protected void fillHeader() {
		var d = db.ModuleIndex.SingleOrDefault(p => p.Kode == myKode);
		if (d == null) App.DataNull(this);
		else {
			header_group.InnerHtml = d.ModuleGroup.Nama;
			header_module.InnerHtml = d.Nama;
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&km=" + myKode;

		//Customize-Qstr
		href += "&kw=" + kw.Text;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//SELECT
		var rs = from p in db.UserAccount
				 where
					p.Status == 1 && (p.Nama.Contains(kw.Text) || p.Email.Contains(kw.Text))
				 orderby p.Nama
				 select p;

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
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
			td.Text = TD_Nama(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Email(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Keterangan(r);
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(UserAccount r) {
		string x = string.Empty;

		int count = db.UserRole.Count(p => p.ID == r.ID && p.Module == myKode);
		if (count == 0) {
			x += "<a class=\"btn btn-info\" href=\"RoleUserAddSave.aspx?id=" + r.ID + "&km=" + myKode + "\">";
			x += "  <i class=\"icon-ok\"></i> ";
			x += "  Select";
			x += "</a>";
		}
		else {
			x += "<a class=\"btn btn-default\" disabled=\"disabled\">";
			x += "  <i class=\"icon-ok\"></i> ";
			x += "  Select";
			x += "</a>";
		}

		return x;
	}
	protected string TD_Nama(UserAccount r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";

		return x;
	}
	protected string TD_Email(UserAccount r) {
		string x = string.Empty;

		x += "<p>" + r.Email + "</p>";

		return x;
	}
	protected string TD_Keterangan(UserAccount r) {
		string x = string.Empty;

		if (r.Keterangan != "")
			x += "<p class=\"text-muted\">" + Str.HtmlBreak(r.Keterangan) + "</p>";

		return x;
	}
}