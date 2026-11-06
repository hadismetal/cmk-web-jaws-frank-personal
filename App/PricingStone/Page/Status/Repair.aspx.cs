using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Status_Repair : System.Web.UI.Page {
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
			DLC.ParcelRepair01(parcel01);
			DLC.ParcelRepair02(parcel02);

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
		if (status.SelectedIndex > 0) w += " AND StatusPricing = " + status.SelectedValue;

		//ORDER BY
		string o = order.SelectedValue + " " + orderby.SelectedValue;

		//SELECT
		var rs = from p in db.StoneRepair.Where(w).OrderBy(o)
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
			td.Text = TD_Approval(r.StonePricingRepair);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Harga(r.StonePricingRepair);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Selisih(r.StonePricingRepair, true);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Selisih(r.StonePricingRepair, false);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(StoneRepair r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Log_Repair.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "  Detail";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(StoneRepair r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";
		if (r.KodeParcel != r.Nama)
			x += "<p class=\"text-muted\">" + r.KodeParcel + "</p>";

		return x;
	}
	protected string TD_Approval(StonePricingRepair r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Approval + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.ApprovalTgl) + "</p>";

		return x;
	}
	protected string TD_Harga(StonePricingRepair r) {
		string x = string.Empty;

		x += "<p class=\"text-success text-lg\">";
		x += Str.Number2(r.Harga);
		x += "</p>";

		return x;
	}
	protected string TD_Selisih(StonePricingRepair r, bool Absolut) {
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
}