using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_Design : System.Web.UI.Page {
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
		App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
			//Generate
			fillTable(sender, e);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		//SELECT
		var rs = from p in db.ProductDesignGJ
				 where p.Approval == null && p.Draft == false
				 orderby p.DataHuman.Nama, p.ID
				 select p;

		//NO-RESULT
		zero.Visible = rs.Count() == 0;
		form_area.Visible = !zero.Visible;

		//RESULT
		int prev = 0;
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			//Sub-Header
			if (prev != r.IDDesigner) {
				Sub_Header(r);
				prev = r.IDDesigner;
			}

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = TD_HyperLink(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p><img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" /></p>";
            tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nomor(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Operator(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Data(r);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Human(r);
			tr.Cells.Add(td);

			if (r.ProductDesignGJ_CharProduct != null) {
				td = new TableCell();
				td.Text = TD_Product01(r.ProductDesignGJ_CharProduct);
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = TD_Product02(r.ProductDesignGJ_CharProduct);
				td.CssClass = "text-center";
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = TD_Product03(r.ProductDesignGJ_CharProduct);
				td.CssClass = "text-center";
				tr.Cells.Add(td);
			}

			tb.Rows.Add(tr);
		}
	}
	protected void Sub_Header(ProductDesignGJ r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += r.DataHuman.Nama + " - " + r.DataHuman.CompanyBrand.Nama;

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(ProductDesignGJ r) {
		string x = string.Empty;

		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Design_Form.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "  Open";
		x += "</a>";

		return x;
	}
	protected string TD_Nomor(ProductDesignGJ r) {
		string x = string.Empty;

		x += "<p class=\"text-md text-info\">" + r.Nomor + "</p>";

		return x;
	}
	protected string TD_Operator(ProductDesignGJ r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
	protected string TD_Data(ProductDesignGJ r) {
		string x = string.Empty;

		if (r.DesignKode != "" && r.DesignKode != null)
			x += "<p class=\"text-sm\">" + r.DesignKode + "</p>";

		return x;
	}
	protected string TD_Human(ProductDesignGJ r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\">" + r.HumanDesignerNama + "</p>";

		return x;
	}
	protected string TD_Product01(ProductDesignGJ_CharProduct r) {
		string x = string.Empty;

		x += "<p class=\"text-bold text-primary\">";
		x += r.CharProductItem.Nama;
		x += "</p>";

		return x;
	}
	protected string TD_Product02(ProductDesignGJ_CharProduct r) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-orange\">";
		x += r.CharProductCategory.NamaKode;
		x += "</p>";

		return x;
	}
	protected string TD_Product03(ProductDesignGJ_CharProduct r) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-orange\">";
		x += r.CharProductLevel.NamaKode;
		x += "</p>";

		return x;
	}
}