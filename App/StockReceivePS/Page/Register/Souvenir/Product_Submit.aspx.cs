using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_DJ_Submit : System.Web.UI.Page {
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

    decimal GT_Qty = 0;
    decimal GT_Harga = 0m;
    decimal GT_Ongkos = 0m;

	protected int myID { get { return App.QueryInt(this); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Product.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.StockReceiveSouvenir.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header

            nopo.InnerText = d.NoPO;
            nodo.InnerText = d.NoDO;
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			tgl.InnerHtml = Str.Tgl(d.Tgl);

			//Product
			fillProduct();
		}
	}


	//Product
	protected void fillProduct() {
        //SELECT
        var rs = from p in db.StockReceiveSouvenir_Product
                 where p.IDForm == myID
                 orderby p.Souvenir.Nama
                 select p;

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = "<p>" + index++.ToString() + ".</p>";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Souvenir.Nama);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Nama(r.Souvenir.Kode);
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Info(r.Qty);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Info(r.Harga);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            GT_Qty += r.Qty.Value;
            GT_Harga += r.Harga.Value;

            tb.Rows.Add(tr);
        }

		//Grand-Total
		Sub_Total(GT_Qty, GT_Harga, GT_Ongkos, true);
	}
	
	protected void Sub_Total(decimal TotalQty, decimal TotalHarga, decimal Ongkos, bool GrandTotal) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

		td = new TableCell();
		td.ColumnSpan = 3;
		td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
		td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
		td.Text += "</p>";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = "<p class=\"text-lg\">" + Str.Number0(TotalQty) + "</p>";
		td.CssClass = "text-center";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = "<p class=\"text-lg\">" + Str.Number0(TotalHarga) + "</p>";
		td.CssClass = "text-right";
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}

	//Display Column
	protected string TD_Nama(string Nama) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";

		return x;
	}
	protected string TD_Nama(string Nama, string GIA) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";
		if (GIA != "")
			x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";

		return x;
	}
	protected string TD_Info(decimal? Value) {
		string x = string.Empty;

		x += "<p>" + Str.Number0(Value) + "</p>";

		return x;
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.StockReceiveSouvenir.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Status
			d.Draft = false;
			d.DraftDate = null;

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("Product.aspx?d=1");
		}
        //Response.Redirect("DJ_Pricing.aspx?id=" + myID);
	}
}