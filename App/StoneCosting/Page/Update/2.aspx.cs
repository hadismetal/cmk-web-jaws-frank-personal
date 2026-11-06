using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Update_2 : System.Web.UI.Page {
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
		App.ProtectRole(Role.Operator);

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
			DLC.StatusCosting(status);

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
		href += "&status=" + status.SelectedIndex;
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
			status.SelectedIndex = Convert.ToInt32(Request.QueryString["status"]);
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
		if (status.SelectedIndex > 0) w += " AND StatusCosting = " + status.SelectedValue;

		//ORDER BY
		string o = order.SelectedValue + " " + orderby.SelectedValue;

		//SELECT
		var rs = from p in db.Stone2.Where(w).OrderBy(o)
				 where
					 (p.Nama.Contains(kw.Text) || p.KodeParcel.Contains(kw.Text)) &&
					 !p.Disable
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
			td.Text = TD_Operator(r.StoneCosting2);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Harga(r.StoneCosting2);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_HargaPending(r.StoneCosting2);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Mutasi(r.StoneCosting2);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(Stone2 r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Process_2.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(Stone2 r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";
		if (r.KodeParcel != r.Nama)
			x += "<p class=\"text-muted\">" + r.KodeParcel + "</p>";

		return x;
	}
	protected string TD_Operator(StoneCosting2 r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
	protected string TD_Harga(StoneCosting2 r) {
		string x = string.Empty;

		x += "<p class=\"text-success text-lg\">";
		x += Str.Number2(r.Harga);
		x += "</p>";

		return x;
	}
	protected string TD_HargaPending(StoneCosting2 r) {
		string x = string.Empty;

		x += "<p class=\"text-danger text-lg\">";
		x += Str.Number2(r.HargaPending);
		x += "</p>";

		return x;
	}
	protected string TD_Mutasi(StoneCosting2 r) {
		string x = string.Empty;

		if (r.MutasiPersen.HasValue) {
			string sgn = r.MutasiPersen > 0 ? "+" : "";
			string css = r.MutasiPersen > 10 || r.MutasiPersen < -10 ? "text-red" : "";

			x += "<p class=\"" + css + " text-lg\">";
			x += sgn;
			x += Str.Number2(r.MutasiPersen);
			x += "%";
			x += "</p>";
		}
		else if (r.Mutasi.HasValue) {
			string sgn = r.Mutasi > 0 ? "+" : "";

			x += "<p class=\"text-lg\">";
			x += sgn;
			x += Str.Number2(r.Mutasi);
			x += "</p>";
		}

		return x;
	}
}