using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Account_AdminSales : System.Web.UI.Page {
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
			DLC.CompanyBrand(brand);
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
		href += "&brand=" + brand.SelectedIndex;
		href += "&disable=" + disable.SelectedIndex;
		href += "&order=" + order.SelectedIndex;
		href += "&orderby=" + orderby.SelectedIndex;

		Response.Redirect(href);
	}
	protected void fillFilter() {
		if (App.Refresh(this)) {
			kw.Text = Request.QueryString["kw"];
			brand.SelectedIndex = Convert.ToInt32(Request.QueryString["brand"]);
			disable.SelectedIndex = Convert.ToInt32(Request.QueryString["disable"]);
			order.SelectedIndex = Convert.ToInt32(Request.QueryString["order"]);
			orderby.SelectedIndex = Convert.ToInt32(Request.QueryString["orderby"]);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//WHERE
		string w = "1=1";
		if (brand.SelectedIndex > 0) w += " AND IDBrand = " + brand.SelectedValue;
		if (disable.SelectedIndex == 1) w += " AND Draft = False AND Disable = False";
		if (disable.SelectedIndex == 2) w += " AND Draft = False AND Disable = True";
		if (disable.SelectedIndex == 3) w += " AND Draft = True";

		//ORDER BY
		string o = order.SelectedValue + " " + orderby.SelectedValue;

		//SELECT
		var rs = from p in db.DataAdminSales.Where(w).OrderBy(o)
				 where
					(
					p.Nama.Contains(kw.Text) ||
					p.Keterangan.Contains(kw.Text) ||
					p.AddrAlamat.Contains(kw.Text)
					)
				 select p;

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		int index = 1;
		foreach (var r in rs)
        {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = TD_HyperLink(r);
            td.Style.Add("text-align","center");
            td.Style.Add("vertical-align", "top");
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "top");
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nama(r);
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "top");
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Tgl(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "top");
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Status(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "top");
            tr.Cells.Add(td);
            
            td = new TableCell();
            td.Text = TD_Location(r);
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "top");
            tr.Cells.Add(td);
            
            tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(DataAdminSales r) {
		string x = string.Empty;

		x += "<div class=\"btn-group\">";
		x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"AdminSalesFile.aspx?id=" + r.ID + "\">";
		x += "      <i class=\"icon-file\"></i> ";
		x += "      Open";
		x += "  </a>";

		if (Role.Operator || Role.Manager) {
			x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"AdminSalesUpdate.aspx?id=" + r.ID + "\">";
			x += "      <i class=\"icon-pencil\"></i> ";
			x += "      Update";
			x += "  </a>";
		}
		else {
			x += "  <a class=\"btn btn-default\" target=\"_blank\" disabled=\"disabled\">";
			x += "      <i class=\"icon-pencil\"></i> ";
			x += "      Update";
			x += "  </a>";
		}

		x += "</div>";

		return x;
	}
	protected string TD_Nama(DataAdminSales r) {
		string x = string.Empty;

		x += "<p><strong>" + Str.CapUpper(r.Nama) + "</strong></p>";
		if (r.Keterangan != "")
			x += "<p class=\"text-muted\">" + Str.HtmlBreak(r.Keterangan) + "</p>";

		return x;
	}
	protected string TD_Tgl(DataAdminSales r) {
		string x = string.Empty;

		x += "<p>" + Str.Tgl(r.Tgl) + "</p>";

		return x;
	}
	protected string TD_Status(DataAdminSales r) {
		string x = string.Empty;

		if (r.Draft)
			x += "<p class=\"text-draft\">" + DL.DisableDraft() + "</p>";
		else if (r.Disable)
			x += "<p class=\"text-warning\">" + DL.Disable(r.Disable) + "</p>";

		return x;
	}
	protected string TD_Addr(DataAdminSales r) {
		string x = string.Empty;

		if (r.AddrAlamat != "" && r.AddrAlamat != null)
			x += "<p class=\"text-sm\">" + Str.HtmlBreak(r.AddrAlamat) + "</p>";

		return x;
	}
    protected string TD_Location(DataAdminSales r)
    {
        string x = string.Empty;


        string TipeLokasi = "";
        string NamaLokasi = "";
        string KodeLokasi = "";
        string Brand = "";

        if (r.TipeLokasi == 0)
        {
            TipeLokasi = "WAREHOUSE";
            var l = db.LocWarehouse.SingleOrDefault(p => p.ID == r.IDLokasi);
            NamaLokasi = l.Nama;
            KodeLokasi = "";
            Brand = l.CompanyBrand.Nama;
        }
        else if (r.TipeLokasi == 1)
        {
            TipeLokasi = "STORE";
            var l = db.LocOutlet.SingleOrDefault(p => p.ID == r.IDLokasi);
            NamaLokasi = l.Nama;
            KodeLokasi = l.Kode;
            Brand = l.CompanyBrand.Nama;
        }
        else
        {
            TipeLokasi = "EXHIBITION";
            var l = db.LocExhibition.SingleOrDefault(p => p.ID == r.IDLokasi);
            NamaLokasi = l.Nama;
            KodeLokasi = l.Kode;
            Brand = l.CompanyBrand.Nama;
        }

        TipeLokasi = Str.CapUpper(TipeLokasi);
        NamaLokasi = Str.CapUpper(NamaLokasi);
        KodeLokasi = KodeLokasi != "" ? Str.CapUpper(KodeLokasi) + " - " : "";
        Brand = Str.CapUpper(Brand);

        x += "<p class=\"text-bold\">" + NamaLokasi + "</p>";
        x += "<p><span class=\"text-info\">" + KodeLokasi + "</span><span class=\"text-muted\">" + Brand + " - " + TipeLokasi + "</span></p>";

        return x;
    }
}