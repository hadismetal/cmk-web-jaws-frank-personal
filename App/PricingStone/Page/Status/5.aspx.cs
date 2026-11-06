using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Status_5 : System.Web.UI.Page {
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
			DLC.Parcel501(parcel01);
			DLC.Parcel502(parcel02);
			DLC.Parcel503(parcel03);
			DLC.Parcel504(parcel04);
			DLC.Parcel505(parcel05);
			DLC.Parcel506(parcel06);
			DLC.Parcel507(parcel07);
			DLC.Parcel508(parcel08);
			DLC.Parcel509(parcel09);
			DLC.StatusPricing(status);

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
		href += "&parcel08=" + parcel08.SelectedIndex;
		href += "&parcel09=" + parcel09.SelectedIndex;
		href += "&status=" + status.SelectedIndex;
		href += "&order=" + order.SelectedIndex;
		href += "&orderby=" + orderby.SelectedIndex;

		Response.Redirect(href);
	}
    protected void Export(object sender, EventArgs e)
    {
        string href = "5Export.aspx?id=1";

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&parcel01=" + parcel01.SelectedIndex;
        href += "&parcel02=" + parcel02.SelectedIndex;
        href += "&parcel03=" + parcel03.SelectedIndex;
        href += "&parcel04=" + parcel04.SelectedIndex;
        href += "&parcel05=" + parcel05.SelectedIndex;
        href += "&parcel06=" + parcel06.SelectedIndex;
        href += "&parcel07=" + parcel07.SelectedIndex;
        href += "&parcel08=" + parcel08.SelectedIndex;
        href += "&parcel09=" + parcel09.SelectedIndex;
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
			parcel08.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel08"]);
			parcel09.SelectedIndex = Convert.ToInt32(Request.QueryString["parcel09"]);
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
		if (parcel08.SelectedIndex > 0) w += " AND Parcel08 = " + parcel08.SelectedValue;
		if (parcel09.SelectedIndex > 0) w += " AND Parcel09 = " + parcel09.SelectedValue;
		if (status.SelectedIndex > 0) w += " AND StatusPricing = " + status.SelectedValue;

		//ORDER BY
		string o = order.SelectedValue + " " + orderby.SelectedValue;

		//SELECT
		var rs = from p in db.Stone5.Where(w).OrderBy(o)
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
			td.Text = TD_Approval(r.StonePricing5);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Modal_New(r.StoneCosting5);
            td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Harga(r.StonePricing5);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Selisih_New(r.StonePricing5, r.StoneCosting5, true);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Selisih_New(r.StonePricing5, r.StoneCosting5, false);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(Stone5 r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Log_5.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "  Detail";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(Stone5 r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";
		if (r.KodeParcel != r.Nama)
			x += "<p class=\"text-muted\">" + r.KodeParcel + "</p>";

		return x;
	}
	protected string TD_Approval(StonePricing5 r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Approval + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.ApprovalTgl) + "</p>";

		return x;
	}
	protected string TD_Modal(StonePricing5 r) {
		string x = string.Empty;

		x += "<p class=\"text-info text-lg\">";
		x += Str.Number2(r.Modal);
		x += "</p>";

		return x;
	}
	protected string TD_Modal_New(StoneCosting5 r)
    {
        string x = string.Empty;
        x += "<p class=\"text-info text-lg\">";
        x += Str.Number2(r.Harga);
        x += "</p>";
        return x;
    }
    protected string TD_Harga(StonePricing5 r) {
		string x = string.Empty;

		x += "<p class=\"text-success text-lg\">";
		x += Str.Number2(r.Harga);
		x += "</p>";

		return x;
	}
	protected string TD_Selisih(StonePricing5 r, bool Absolut) {
		string x = string.Empty;

		if (r.Selisih.HasValue) {
			string sgn = r.Selisih > 0 ? "+" : "";
			string css = r.Selisih < 0 ? "text-red" : "text-orange";

			x += "<p class=\"" + css + " text-lg\">";

			if (Absolut) {
				x += sgn;
				x += Str.Number2(r.Selisih);
			}
			else {
				if (r.SelisihPersen.HasValue) {
					x += sgn;
					x += Str.Number2(r.SelisihPersen);
					x += "%";
				}
			}

			x += "</p>";
		}

		return x;
	}
	protected string TD_Selisih_New(StonePricing5 r, StoneCosting5 s, bool Absolut)
	{
		string x = string.Empty;
		if (r != null && s != null && r.Harga.HasValue && s.Harga.HasValue)
		{
			decimal? Selisih = r.Harga - s.Harga;
			decimal? SelisihPersen = (s.Harga > 0) ? (Selisih / s.Harga) * 100 : (decimal?)null;

			string sgn = Selisih > 0 ? "+" : "";
			string css = Selisih < 0 ? "text-red" : "text-orange";
			x += "<p class=\"" + css + " text-lg\">";
			if (Absolut)
			{
				x += sgn;
				x += Str.Number2(Selisih);
			}
			else
			{
				if (SelisihPersen.HasValue)
				{
					x += sgn;
					x += Str.Number2(SelisihPersen);
					x += "%";
				}
			}
			x += "</p>";
		}
		return x;
	}
}