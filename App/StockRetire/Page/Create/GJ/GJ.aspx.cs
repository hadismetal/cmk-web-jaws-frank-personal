using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Approval_GJ : System.Web.UI.Page {
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
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {

		//SELECT
		var rs = (from p in db.StockActualGJ
                  where p.StockProductGJ.Draft == false && p.StockProductGJ.ReturnProduct == false && p.StockProductGJ.StatusPenjualan == 0 && p.StockProductGJ.StatusLebur == false
                  && p.Substorage == 6
                 select p);
                  //select p).Where(p => db.DocQCGJ.Any(y => y.IDProduct != p.ID));

        rs = rs.OrderBy(p => p.StockProductGJ.LocWarehouse.Nama).ThenBy(p => p.StockProductGJ.ID);

        if (kw.Text != "")
            rs = rs.Where(p => p.Nomor == "" + kw.Text + "");

		//NO-RESULT
		//zero.Visible = rs.Count() == 0;
		//form_area.Visible = !zero.Visible;

		//RESULT
		int prev = 0;
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			//Sub-Header
            if (prev != r.StockProductGJ.IDWarehouse.Value)
            {
                Sub_Header(r.StockProductGJ);
                prev = r.StockProductGJ.IDWarehouse.Value;
			}

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
            td.Text = TD_HyperLink(r.StockProductGJ);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Image(r.StockProductGJ);
            tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Nomor(r.StockProductGJ);
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Data(r.StockProductGJ);
			tr.Cells.Add(td);

            if (r.StockProductGJ.StockProductGJ_CharProduct != null)
            {
				td = new TableCell();
                td.Text = TD_Product01(r.StockProductGJ.StockProductGJ_CharProduct);
				tr.Cells.Add(td);

				td = new TableCell();
                td.Text = TD_Product02(r.StockProductGJ.StockProductGJ_CharProduct);
				td.CssClass = "text-center";
				tr.Cells.Add(td);

				td = new TableCell();
                td.Text = TD_Product03(r.StockProductGJ.StockProductGJ_CharProduct);
				td.CssClass = "text-center";
				tr.Cells.Add(td);
			}

			tb.Rows.Add(tr);
		}
	}
	protected void Sub_Header(StockProductGJ r) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += r.LocWarehouse.Nama + " - " + r.LocWarehouse.CompanyBrand.Nama;

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected string TD_HyperLink(StockProductGJ r) {
		string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"GJ_Submit.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "  Open";
		x += "</a>";

		return x;
	}
	protected string TD_Nomor(StockProductGJ r) {
		string x = string.Empty;

		x += "<p class=\"text-md text-info\">" + r.Nomor + "</p>";

		return x;
	}
	protected string TD_Operator(StockProductGJ r) {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
	protected string TD_Data(StockProductGJ r) {
		string x = string.Empty;

		if (r.ProtoNomor != "" && r.ProtoNomor != null)
			x += "<p class=\"text-sm\">" + r.ProtoNomor + "</p>";

		if (r.ProductOrderNomor != "" && r.ProductOrderNomor != null)
			x += "<p class=\"text-sm\">" + r.ProductOrderNomor + "</p>";

		if (r.SupplierNomor != "" && r.SupplierNomor != null)
			x += "<p class=\"text-sm\">" + r.SupplierNomor + "</p>";

		return x;
	}
	protected string TD_Product01(StockProductGJ_CharProduct r) {
		string x = string.Empty;

		x += "<p class=\"text-bold text-primary\">";
		x += r.CharProductItem.Nama;
		x += "</p>";

		return x;
	}
	protected string TD_Product02(StockProductGJ_CharProduct r) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-orange\">";
		x += r.CharProductCategory.NamaKode;
		x += "</p>";

		return x;
	}
	protected string TD_Product03(StockProductGJ_CharProduct r) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-orange\">";
		x += r.CharProductLevel.NamaKode;
		x += "</p>";

		return x;
	}
    protected string TD_Image(StockProductGJ r)
    {
        string x = string.Empty;

        x += "<img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" />";

        return x;
    }

    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            
            kw.Text = Request.QueryString["kw"];
        }
    }
}