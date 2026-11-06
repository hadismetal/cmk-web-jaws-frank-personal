using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_ProductAdd : System.Web.UI.Page {
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
	protected int myID { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			kw.Focus(); //Keyword

			//Form
			fillForm();

			//DLC
            //DLC
            DLC.CompanyBrand(brand);

			//Generate
			fillFilter();
			if (App.Refresh(this)) fillTable(sender, e);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Product.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.StockReceivePackaging.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			
		}
	}

	//Filter
	protected void refreshPage(object sender, EventArgs e) {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&id=" + myID;
        href += "&kw=" + kw.Text;
        href += "&brand=" + brand.SelectedValue;

        Response.Redirect(href);
	}
	protected void fillFilter() {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            brand.SelectedValue = Request.QueryString["brand"];
        }
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		fillProduct();

	}
	protected void fillProduct() {
		//SELECT
		var rs = from p in db.Packaging
				 select p;

        if (kw.Text != "")
            rs = rs.Where(p => p.Kode.Contains(kw.Text) || p.Nama.Contains(kw.Text));
        
        int Brand = Convert.ToInt32(brand.SelectedValue);
        if (Brand != null && Brand > 0)
            rs = rs.Where(p => p.IDBrand == Brand);

        rs = rs.OrderBy(p => p.Nama);

		//NO-RESULT
		if (rs.Count() == 0) App.TableNoResult(tb);

		//RESULT
		int index = 1;
		foreach (var r in rs) {
            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = TD_HyperLink(r.ID);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_KodePLU(r.Kode);
            tr.Cells.Add(td);

            tb.Rows.Add(tr);
		}
	}
	

	//Display Column
	protected string TD_HyperLink(int ID) {
		string x = string.Empty;
		string href = string.Empty;

		href += "ProductAddSave.aspx";
		href += "?id=" + myID;
		href += "&idproduct=" + ID;

		x += "<a class=\"btn btn-info\" href=\"" + href + "\">";
		x += "  <i class=\"icon-ok\"></i> ";
		x += "  Select";
		x += "</a>";

		return x;
	}
	protected string TD_Nama(string Nama) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";

		return x;
	}
	protected string TD_KodePLU(string KodePLU) 
    {
		string x = string.Empty;

		x += "<p class=\"text-muted\">" + KodePLU + "</p>";

		return x;
	}
}