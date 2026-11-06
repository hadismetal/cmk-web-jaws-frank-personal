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

            datepicker.Text = Str.Tgl(DateTime.Now);
            datepicker2.Text = Str.Tgl(DateTime.Now);

			//Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
        DateTime start;
        DateTime end;

        

		//SELECT
		var rs = from p in db.StockRetireGJ
                 where p.Draft == false
				 select p;

        if (datepicker.Text != "" && datepicker2.Text != "")
        {
            start = Convert.ToDateTime(datepicker.Text);
            end = Convert.ToDateTime(datepicker2.Text);
            rs = rs.Where(p => p.Tgl >= start && p.Tgl < end.AddDays(1));
        }

        int Status = Convert.ToInt32(status.SelectedValue);
        if (Status == 1)
            rs = rs.Where(p => p.Status == 1);
        else if (Status == 2)
            rs = rs.Where(p => p.Status == 0);
        else if (Status == 3)
            rs = rs.Where(p => p.Status == 2);

        rs = rs.OrderBy(p=> p.StockProductGJ.LocWarehouse.Nama).ThenBy(p => p.ID);

		//NO-RESULT
		//zero.Visible = rs.Count() == 0;
		//form_area.Visible = !zero.Visible;

        if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		int prev = 0;
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			//Sub-Header
			if (prev != r.StockProductGJ.IDWarehouse) {
                Sub_Header(r.StockProductGJ);
				prev = r.StockProductGJ.IDWarehouse.Value;
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
            td.Text = TD_Image(r.StockProductGJ);
            tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Nomor(r.StockProductGJ);
			tr.Cells.Add(td);

			td = new TableCell();
            td.Text = TD_Operator(r.StockProductGJ);
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
    protected string TD_HyperLink(StockRetireGJ r)
    {
		string x = string.Empty;

        
		x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"GJ_Form.aspx?id=" + r.ID + "\">";
		x += "  <i class=\"icon-search\"></i> ";
		x += "  Open";
		x += "</a>";

		return x;
	}
    protected string TD_Nomor(StockProductGJ r)
    {
		string x = string.Empty;

		x += "<p class=\"text-md text-info\">" + r.Nomor + "</p>";

		return x;
	}
    protected string TD_Operator(StockProductGJ r)
    {
		string x = string.Empty;

		x += "<p class=\"text-sm\"><strong>" + r.Operator + "</strong></p>";
		x += "<p class=\"text-sm\">" + Str.FullDate(r.OperatorTgl) + "</p>";

		return x;
	}
    protected string TD_Data(StockProductGJ r)
    {
		string x = string.Empty;

		if (r.ProtoNomor != "" && r.ProtoNomor != null)
			x += "<p class=\"text-sm\">" + r.ProtoNomor + "</p>";

		if (r.ProductOrderNomor != "" && r.ProductOrderNomor != null)
			x += "<p class=\"text-sm\">" + r.ProductOrderNomor + "</p>";

		if (r.SupplierNomor != "" && r.SupplierNomor != null)
			x += "<p class=\"text-sm\">" + r.SupplierNomor + "</p>";

		return x;
	}
    protected string TD_Product01(StockProductGJ_CharProduct r)
    {
		string x = string.Empty;

		x += "<p class=\"text-bold text-primary\">";
		x += r.CharProductItem.Nama;
		x += "</p>";

		return x;
	}
    protected string TD_Product02(StockProductGJ_CharProduct r)
    {
		string x = string.Empty;

		x += "<p class=\"text-lg text-orange\">";
		x += r.CharProductCategory.NamaKode;
		x += "</p>";

		return x;
	}
    protected string TD_Product03(StockProductGJ_CharProduct r)
    {
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
        href += "&status=" + status.SelectedValue;
        href += "&dari=" + datepicker.Text;
        href += "&sampai=" + datepicker2.Text;

        Response.Redirect(href);
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            
            kw.Text = Request.QueryString["kw"];
            status.SelectedValue = Request.QueryString["status"];
            datepicker.Text = Request.QueryString["dari"];
            datepicker2.Text = Request.QueryString["sampai"];
        }
    }
}