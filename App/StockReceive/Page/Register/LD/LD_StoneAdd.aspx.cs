using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_LD_StoneAdd : System.Web.UI.Page {
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
	protected int myID { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			//Form
			fillForm();

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("LD.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/LD_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Cancel-Link
			cancel.HRef += "?id=" + myID;
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Page-Qstr
		href += "&id=" + myID;

		//Customize-Qstr
		href += "&stoneindex=1";
		href += "&kw=" + kw.Text;

		//Create-Session
		App.Session("StoneIndex", "1");

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		fill_1B();
	}
	protected void fill_1B() {
		//SELECT
		var rs = from p in db.Stone1B
				 where
					!p.Disable &&
					(p.Nama.Contains(kw.Text) || p.KodeParcel.Contains(kw.Text))
				 orderby p.Nama
				 select p;

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;
			fill(index, r.ID, r.Nama, r.KodeParcel);
		}
	}

	//Display Row
	protected void fill(int index, int ID, string Nama, string KodeParcel) {
		TableRow tr = new TableRow();
		TableCell td;

		td = new TableCell();
		td.Text = TD_HyperLink(ID);
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = "<p>" + index++.ToString() + ".</p>";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = TD_Nama(Nama);
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = TD_KodeParcel(KodeParcel);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}

	//Display Column
	protected string TD_HyperLink(int ID) {
		string x = string.Empty;
		string href = string.Empty;

		href += "LD_StoneAddSave.aspx";
		href += "?id=" + myID;
        href += "&si=1";
		href += "&idstone=" + ID;

		x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  Select";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(string Nama) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";

		return x;
	}
	protected string TD_KodeParcel(string KodeParcel) {
		string x = string.Empty;

		x += "<p class=\"text-muted\">" + KodeParcel + "</p>";

		return x;
	}
}