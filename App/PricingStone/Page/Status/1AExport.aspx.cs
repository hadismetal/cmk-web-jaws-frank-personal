using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Status_1A : System.Web.UI.Page {
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

            Response.AppendHeader("content-disposition", "attachment; filename=DiamondReguler_ExportExcel.xls");

            fillTable(sender, e);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		string w = "1=1";
        if (Convert.ToInt32(Request.QueryString["parcel01"])>0) w += " AND Parcel01 = " + Convert.ToInt32(Request.QueryString["parcel01"]);
        if (Convert.ToInt32(Request.QueryString["parcel02"])>0) w += " AND Parcel02 = " + Convert.ToInt32(Request.QueryString["parcel02"]);
        if (Convert.ToInt32(Request.QueryString["parcel03"])>0) w += " AND Parcel03 = " + Convert.ToInt32(Request.QueryString["parcel03"]);
        if (Convert.ToInt32(Request.QueryString["parcel04"])>0) w += " AND Parcel04 = " + Convert.ToInt32(Request.QueryString["parcel04"]);
        if (Convert.ToInt32(Request.QueryString["status"])>0) w += " AND StatusPricing = " + Convert.ToInt32(Request.QueryString["status"]);

		//ORDER BY
        //string o = Request.QueryString["order"] + " " + Request.QueryString["orderby"];
        string o = "";
        string order = "";
        string orderby = "";
        if(Request.QueryString["order"]=="0")
            order = "Nama";
        else if(Request.QueryString["order"]=="1")
            order = "KodeParcel";
        else
            order = "ID";

        if(Request.QueryString["order"]=="0")
            orderby = "ASC";
        else
            orderby = "DESC";

        o = order + " " + orderby;

		//SELECT
		var rs = from p in db.Stone1A.Where(w).OrderBy(o)
				 where
                     (p.Nama.Contains(Request.QueryString["kw"]) || p.KodeParcel.Contains(Request.QueryString["kw"])) &&
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
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nama(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Approval(r.StonePricing1A);
			tr.Cells.Add(td);
			
			td = new TableCell();
			td.Text = "<p class=\"text-sm\">" + Str.FullDate(r.StonePricing1A.ApprovalTgl) + "</p>";
			tr.Cells.Add(td);
			
			td = new TableCell();
			td.Text = TD_Modal_New(r.StoneCosting1A);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Harga(r.StonePricing1A);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Selisih_New(r.StonePricing1A, r.StoneCosting1A, true);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Selisih_New(r.StonePricing1A, r.StoneCosting1A, false);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_Nama(Stone1A r) {
		string x = string.Empty;

		x += "<p><strong>" + r.Nama + "</strong></p>";
		

		return x;
	}
	protected string TD_Approval(StonePricing1A r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Approval + "</strong></p>";
		

		return x;
	}
	protected string TD_Modal(StonePricing1A r) {
		string x = string.Empty;

		x += "<p class=\"text-info text-lg\">";
		x += Str.Number2(r.Modal);
		x += "</p>";

		return x;
	}
    protected string TD_Modal_New(StoneCosting1A r)
    {
        string x = string.Empty;

        x += "<p class=\"text-info text-lg\">";
        x += Str.Number2(r.Harga);
        x += "</p>";

        return x;
    }
    protected string TD_Harga(StonePricing1A r) {
		string x = string.Empty;

		x += "<p class=\"text-success text-lg\">";
		x += Str.Number2(r.Harga);
		x += "</p>";

		return x;
	}
	protected string TD_Selisih(StonePricing1A r, bool Absolut) {
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
	protected string TD_Selisih_New(StonePricing1A r, StoneCosting1A s, bool Absolut)
	{
		string x = string.Empty;

		if (r != null && s != null && r.Harga.HasValue && s.Harga.HasValue)
		{
			decimal? Selisih = r.Harga - s.Harga;
			decimal? SelisihPersen = s.Harga.Value != 0 ? (Selisih / s.Harga.Value) * 100 : 100;

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