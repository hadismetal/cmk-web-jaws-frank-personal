using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_RoleDetail : System.Web.UI.Page {
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
			create.HRef += "?km=" + myKode;

			//Header
			fillHeader();

			//Generate
			fillFilter();
			fillTable(sender, e);
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
		href += "&role=" + role.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			role.SelectedIndex = Convert.ToInt32(Request.QueryString["role"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		string w = "1=1";
		if (role.SelectedIndex == 1) w += " AND Manager = True";
		if (role.SelectedIndex == 2) w += " AND Operator = True";
		if (role.SelectedIndex == 3) w += " AND DownloadFile = True";
		if (role.SelectedIndex == 4) w += " AND DownloadExcel = True";

		//SELECT
		var rs = from p in db.UserRole.Where(w)
				 where
					p.Module == myKode &&
					p.UserAccount.Status == 1
				 orderby p.UserAccount.Nama
				 select p;

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

        int index = 1;

		//RESULT
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
            td.Text = TD_HyperLink(r.UserAccount);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "" + index++ + ".";
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Nama(r.UserAccount);
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Email(r.UserAccount);
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Bool(r.Manager);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Bool(r.Operator);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Bool(r.DownloadFile);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Bool(r.DownloadExcel);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(UserAccount r) {
		string x = string.Empty;

		x += "<div class=\"btn-group\">";
		x += "	<a class=\"btn btn-info\" target=\"_blank\" href=\"RoleUserUpdate.aspx?id=" + r.ID + "&km=" + myKode + "\">";
		x += "		<i class=\"icon-pencil\"></i> ";
		x += "		Update";
		x += "	</a>";
		x += "	<a class=\"btn btn-danger\" href=\"javascript:goDelete('" + r.ID + "','" + myKode + "')\">";
		x += "		<i class=\"icon-trash\"></i> ";
		x += "		Delete";
		x += "	</a>";
		x += "</div>";

		return x;
	}
	protected string TD_Nama(UserAccount r) {
		string x = string.Empty;

        string color = "";
        if (r.UserNumber == null) { color = "style='color:red;'"; }

        x += "<span " + color + "><strong>" + r.Nama.ToUpper() + "</strong></span><br />";
        x += "<span class='text-muted'>" + r.UserNumber + "</span>";

		return x;
	}
	protected string TD_Email(UserAccount r) {
		string x = string.Empty;

        string color = "";
        if (!Email.validEmail(r.Email)) { color = "style='color:red;'"; }
        x += "<span " + color + ">" + r.Email + "</span>";

        if (r.Keterangan.Trim() != "")
        {
            x += "<br /><span class='text-muted'>" + r.Keterangan.Trim() + "</span>";
        }

		return x;
	}
	protected string TD_Bool(bool ok) {
		string x = string.Empty;

		string css = ok ? "icon-ok text-success" : "icon-remove text-silver";
		x += "<p><i class=\"icon-2x " + css + "\"></i></p>";

		return x;
	}
}