using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Account_2 : System.Web.UI.Page {
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
			kw.Focus(); //Keyword

			//DLC
			DLC.Parcel201(parcel01);
			DLC.Parcel202(parcel02);
			DLC.Parcel203(parcel03);
			DLC.Parcel204(parcel04);
			DLC.Parcel205(parcel05);
			DLC.Parcel206(parcel06);
			DLC.Parcel207(parcel07);
			DLC.DisableDraft(disable);

			//Form-Default
			disable.SelectedIndex = 1;

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
		href += "&kw=" + kw.Text;
		href += "&parcel01=" + parcel01.SelectedIndex;
		href += "&parcel02=" + parcel02.SelectedIndex;
		href += "&parcel03=" + parcel03.SelectedIndex;
		href += "&parcel04=" + parcel04.SelectedIndex;
		href += "&parcel05=" + parcel05.SelectedIndex;
		href += "&parcel06=" + parcel06.SelectedIndex;
		href += "&parcel07=" + parcel07.SelectedIndex;
		href += "&disable=" + disable.SelectedIndex;
		href += "&order=" + order.SelectedIndex;
		href += "&orderby=" + orderby.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
			parcel01.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel01"]);
			parcel02.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel02"]);
			parcel03.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel03"]);
			parcel04.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel04"]);
			parcel05.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel05"]);
			parcel06.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel06"]);
			parcel07.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel07"]);
			disable.SelectedIndex = Convert.ToInt32(Request.QueryString["disable"]);
			order.SelectedIndex = Convert.ToInt32(Request.QueryString["order"]);
			orderby.SelectedIndex = Convert.ToInt32(Request.QueryString["orderby"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		string w = "1=1";
		if (parcel01.SelectedIndex > 0) w += " AND Parcel01 = " + parcel01.SelectedValue;
		if (parcel02.SelectedIndex > 0) w += " AND Parcel02 = " + parcel02.SelectedValue;
		if (parcel03.SelectedIndex > 0) w += " AND Parcel03 = " + parcel03.SelectedValue;
		if (parcel04.SelectedIndex > 0) w += " AND Parcel04 = " + parcel04.SelectedValue;
		if (parcel05.SelectedIndex > 0) w += " AND Parcel05 = " + parcel05.SelectedValue;
		if (parcel06.SelectedIndex > 0) w += " AND Parcel06 = " + parcel06.SelectedValue;
		if (parcel07.SelectedIndex > 0) w += " AND Parcel07 = " + parcel07.SelectedValue;
		if (disable.SelectedIndex == 1) w += " AND Draft = False AND Disable = False";
		if (disable.SelectedIndex == 2) w += " AND Draft = False AND Disable = True";
		if (disable.SelectedIndex == 3) w += " AND Draft = True";

		//ORDER BY
		string o = order.SelectedValue + " " + orderby.SelectedValue;

		//SELECT
		var rs = from p in db.Stone2.Where(w).OrderBy(o)
				 where
					 (
					 p.Nama.Contains(kw.Text) ||
					 p.Keterangan.Contains(kw.Text) ||
					 p.KodeParcel.Contains(kw.Text)
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
            td.Text = TD_HyperLink(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "middle");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r);
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

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(Stone2 r) {
		string x = string.Empty;

		x += "<div class=\"btn-group\">";
		x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"2_File.aspx?id=" + r.ID + "\">";
		x += "      <i class=\"icon-file\"></i> ";
		x += "      Open";
		x += "  </a>";

		if (Role.Manager) {
			x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"2_Update.aspx?id=" + r.ID + "\">";
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
	protected string TD_Nama(Stone2 r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";
		if (r.Keterangan != "")
			x += "<p class=\"text-muted\">" + Str.HtmlBreak(r.Keterangan) + "</p>";

		return x;
	}
	protected string TD_Tgl(Stone2 r) {
		string x = string.Empty;

		x += "<p>" + Str.Tgl(r.Tgl) + "</p>";

		return x;
	}
	protected string TD_Status(Stone2 r) {
		string x = string.Empty;

		if (r.Draft)
			x += "<p class=\"text-draft\">" + DL.DisableDraft() + "</p>";
		else if (r.Disable)
			x += "<p class=\"text-warning\">" + DL.Disable(r.Disable) + "</p>";

		return x;
	}
	protected string TD_KodeParcel(Stone2 r) {
		string x = string.Empty;

		x += "<p>" + r.KodeParcel + "</p>";

		return x;
	}
}