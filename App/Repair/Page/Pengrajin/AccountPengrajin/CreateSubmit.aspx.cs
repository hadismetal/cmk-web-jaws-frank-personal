using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_CreateSubmit : System.Web.UI.Page {
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
			fillForm();
			fillModule();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Create.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID && p.Status == 0);
		if (d == null) DataError();
		else {
			//Header
			header_nama.InnerHtml =
				"<a href=\"UserFile.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nama + "</a>";

            //Account
            nik.InnerHtml = (d.UserNumber != null ? d.UserNumber : "");
            nama.InnerHtml = d.Nama;
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			email.InnerHtml = d.Email;
			sms.InnerHtml = d.SMS;

			//Password
			pass.InnerHtml = App.Session("GeneratedPass");
		}
	}

	//Module List
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
	protected string TD_Nama(ModuleIndex r) {
		string x = string.Empty;

		x += "<p class=\"hang-30\"><strong>" + r.Nama + "</strong></p>";

		return x;
	}
	protected string TD_Bool(bool ok) {
		string x = string.Empty;

		string css = ok ? "icon-ok text-success" : "icon-remove text-silver";
		x += "<p><i class=\"icon-2x " + css + "\"></i></p>";

		return x;
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.UserAccount.SingleOrDefault(p => p.ID == myID && p.Status == 0);
		if (d == null) DataError();
		else {
			//Status
			d.Status = 1;

			//Commit
			db.SubmitChanges();

			//Session-Destroy
			Session["GeneratedPass"] = null;

			//Redirect
			Response.Redirect("Create.aspx?d=1");
		}
	}
}