using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Update_TG : System.Web.UI.Page {
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
			//DLC
			DLC.StatusPricing(status);

			//Generate
			fillFilter();
			fillTable(sender, e);
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
		string href = App.RefreshURL(this);

		//Customize-Qstr
		href += "&status=" + status.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			status.SelectedIndex = Convert.ToInt32(Request.QueryString["status"]);
		}
	}

    protected int[] Source()
    {
        int[] hasil = new int[30];
        
        hasil[0] = 5;
        hasil[1] = 6;
        hasil[2] = 7;
        hasil[3] = 8;
        hasil[4] = 9;
        hasil[5] = 10;
        hasil[6] = 11;
        hasil[7] = 12;
        hasil[8] = 13;
        hasil[9] = 14;
        hasil[10] = 15;
        hasil[11] = 16;
        hasil[12] = 17;
        hasil[13] = 18;
        hasil[14] = 19;
        hasil[15] = 20;
        hasil[16] = 21;
        hasil[17] = 22;
        hasil[18] = 34;
        hasil[19] = 35;
        hasil[20] = 36;

        return hasil;
    }

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		string w = "1=1";
		if (status.SelectedIndex > 0) w += " AND StatusPricing = " + status.SelectedValue;

		//SELECT
		var rs = from p in db.PricingTableTG.Where(w)
                 where Source().Contains(p.ID)
				 orderby p.ID
				 select p;

		//RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = TD_HyperLink(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nama(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Operator(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Harga(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_HargaPending(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Mutasi(r);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(PricingTableTG r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"../UpdateProcess/TGP.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-pencil\"></i> ";
		x += "  Update";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(PricingTableTG r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";
		if (r.Keterangan != "")
			x += "<p class=\"text-muted\">" + Str.HtmlBreak(r.Keterangan) + "</p>";

		return x;
	}
	protected string TD_Operator(PricingTableTG r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
	protected string TD_Harga(PricingTableTG r) {
		string x = string.Empty;

		x += "<p class=\"text-success text-lg\">";
		x += Str.Number2(r.Harga);
		x += "</p>";

		return x;
	}
	protected string TD_HargaPending(PricingTableTG r) {
		string x = string.Empty;

		x += "<p class=\"text-danger text-lg\">";
		x += Str.Number2(r.HargaPending);
		x += "</p>";

		return x;
	}
	protected string TD_Mutasi(PricingTableTG r) {
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