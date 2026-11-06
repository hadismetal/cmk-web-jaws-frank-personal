using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Account_1B : System.Web.UI.Page {
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
			Response.ContentType = "application/vnd.ms-excel";

			Response.AppendHeader("content-disposition", "attachment; filename=Stone_Export.xls");
			fillTable(sender, e);
		}
	}

	//Filter

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		string w = "1=1";
		if (Convert.ToInt32(Request.QueryString["parcel01"]) > 0) w += " AND Parcel01 = " + Request.QueryString["parcel01"];
		if (Convert.ToInt32(Request.QueryString["parcel02"]) > 0) w += " AND Parcel02 = " + Request.QueryString["parcel02"];
		if (Convert.ToInt32(Request.QueryString["parcel03"]) > 0) w += " AND Parcel03 = " + Request.QueryString["parcel03"];
		if (Convert.ToInt32(Request.QueryString["parcel04"]) > 0) w += " AND Parcel04 = " + Request.QueryString["parcel04"];
		if (Convert.ToInt32(Request.QueryString["parcel05"]) > 0) w += " AND Parcel05 = " + Request.QueryString["parcel05"];
		if (Convert.ToInt32(Request.QueryString["parcel06"]) > 0) w += " AND Parcel06 = " + Request.QueryString["parcel06"];
		if (Convert.ToInt32(Request.QueryString["parcel07"]) > 0) w += " AND Parcel07 = " + Request.QueryString["parcel07"];
		if (Convert.ToInt32(Request.QueryString["disable"]) == 1) w += " AND Draft = False AND Disable = False";
		if (Convert.ToInt32(Request.QueryString["disable"]) == 2) w += " AND Draft = False AND Disable = True";
		if (Convert.ToInt32(Request.QueryString["disable"]) == 3) w += " AND Draft = True";

		//ORDER BY
		string o = Request.QueryString["order"] + " " + Request.QueryString["orderby"];

		//SELECT
		var rs = from p in db.Stone1B.Where(w).OrderBy(o)
				 where
					 (
					 p.Nama.Contains(Request.QueryString["kw"]) ||
					 p.Keterangan.Contains(Request.QueryString["kw"]) ||
					 p.KodeParcel.Contains(Request.QueryString["kw"])
					 )
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
            td.Text = "<p>" + index++.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.Nama;
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Tgl(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Status(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_KodeParcel(r);
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Parcel101.Nama;
			td.Style.Add("text-align", "left");
			td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Parcel102.Nama;
			td.Style.Add("text-align", "left");
			td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Parcel103.Nama;
			td.Style.Add("text-align", "left");
			td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Parcel104.Nama;
			td.Style.Add("text-align", "left");
			td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Parcel105.Nama;
			td.Style.Add("text-align", "left");
			td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Parcel106.Nama;
			td.Style.Add("text-align", "left");
			td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Parcel107.Nama;
			td.Style.Add("text-align", "left");
			td.Style.Add("vertical-align", "middle");
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(Stone1B r) {
		string x = string.Empty;

		x += "<div class=\"btn-group\">";
		x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"1B_File.aspx?id=" + r.ID + "\">";
		x += "      <i class=\"icon-file\"></i> ";
		x += "      Open";
		x += "  </a>";

		if (Role.Manager) {
			x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"1B_Update.aspx?id=" + r.ID + "\">";
			x += "      <i class=\"icon-pencil\"></i> ";
			x += "      Update";
			x += "  </a>";
		}
        //else {
        //    x += "  <a class=\"btn btn-default\" target=\"_blank\" disabled=\"disabled\">";
        //    x += "      <i class=\"icon-pencil\"></i> ";
        //    x += "      Update";
        //    x += "  </a>";
        //}

		return x;
	}
	protected string TD_Nama(Stone1B r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";
		if (r.Keterangan != "")
			x += "<p class=\"text-muted\">" + Str.HtmlBreak(r.Keterangan) + "</p>";

		return x;
	}
	protected string TD_Tgl(Stone1B r) {
		string x = string.Empty;

		x += "<p>" + Str.Tgl(r.Tgl) + "</p>";

		return x;
	}
	protected string TD_Status(Stone1B r) {
		string x = string.Empty;

		if (r.Draft)
			x += "<p class=\"text-draft\">" + DL.DisableDraft() + "</p>";
		else if (r.Disable)
			x += "<p class=\"text-warning\">" + DL.Disable(r.Disable) + "</p>";

		return x;
	}
	protected string TD_KodeParcel(Stone1B r) {
		string x = string.Empty;

		x += "<p>" + r.KodeParcel + "</p>";

		return x;
	}
}