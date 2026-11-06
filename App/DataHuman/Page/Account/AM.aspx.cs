using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Account_AM : System.Web.UI.Page {
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

    private Map_AC _ac;

    private Map_AC ac
    {
        get
        {
            if (_ac == null)
                _ac = new Map_AC();
            return _ac;
        }
    }

	public override void Dispose()
	{
	    if (db != null)
	        db.Dispose();
	    if (ac != null)
	        ac.Dispose();
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
		var rs = from p in db.DataAreaManager.Where(w).OrderBy(o)
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
		foreach (var r in rs) {
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

            //td = new TableCell();
            //td.Text = TD_Addr(r);
            //tr.Cells.Add(td);

            //         string NamaLokasi = "";
            //         string Brand = "";

            //         foreach(var t in r.DataAreaManager_Lokasi)
            //         {
            //             NamaLokasi += Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + t.IDLokasi, Db.Cnn_DB) + "<br />";
            //         }

            //         td = new TableCell();
            //         td.Text = Brand;
            //         td.CssClass = "text-center";
            //         tr.Cells.Add(td);

            string[] OutletName = DLC.AMOutletGroup(r.IDUser.Value);

            string xinfo = "";
            foreach (var l in OutletName)
            {
                xinfo += (l != null) ? l.ToString() + "<br />" : "";
            }

            td = new TableCell();
            td.Text = xinfo;
            td.Style.Add("text-align", "left");
            td.Style.Add("vertical-align", "top");
            tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}
	}
	protected string TD_HyperLink(DataAreaManager r) {
		string x = string.Empty;

		x += "<div class=\"btn-group\">";
		//x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"AMFile.aspx?id=" + r.ID + "\">";
		//x += "      <i class=\"icon-file\"></i> ";
		//x += "      Open";
		//x += "  </a>";

		if (Role.Operator || Role.Manager) {
			x += "  <a class=\"btn btn-info\" target=\"_blank\" href=\"AMUpdate.aspx?id=" + r.ID + "\">";
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
	protected string TD_Nama(DataAreaManager r) {
		string x = string.Empty;
        int iUserID = r.IDUser.Value;
        var d = ac.UserAccount.SingleOrDefault(p => p.ID == iUserID);

        if (d != null)
        {
            x += "<p><strong>" + Str.CapUpper(d.Nama) + "</strong></p>";
            x += "<p class=\"text-muted\">" + Str.HtmlBreak(d.UserNumber) + "</p>";
        }
        else
        {
            x += "<p class='text-red'><strong>UNASSIGNED !!!</strong></p>";
        }

        x += "<div style='clear: both; border-bottom:solid 1px silver;'></div>";
        x += "<p style=\"color:blue;\"><strong>" + Str.CapUpper(r.Nama) + "</strong></p>";
        if (r.Keterangan != "")
            x += "<p class=\"text-muted\">" + Str.HtmlBreak(r.Keterangan) + "</p>";

        return x;
	}
	protected string TD_Tgl(DataAreaManager r) {
		string x = string.Empty;

		x += "<p>" + Str.Tgl(r.Tgl) + "</p>";

		return x;
	}
	protected string TD_Status(DataAreaManager r) {
		string x = string.Empty;

		if (r.Draft)
			x += "<p class=\"text-draft\">" + DL.DisableDraft() + "</p>";
		else if (r.Disable)
			x += "<p class=\"text-warning\">" + DL.Disable(r.Disable) + "</p>";

		return x;
	}
	protected string TD_Addr(DataAreaManager r) {
		string x = string.Empty;

		if (r.AddrAlamat != "" && r.AddrAlamat != null)
			x += "<p class=\"text-sm\">" + Str.HtmlBreak(r.AddrAlamat) + "</p>";

		return x;
	}
}