using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_Register_DJ_Stone : System.Web.UI.Page {
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

    decimal GT_Qty = 0;
    decimal GT_Harga = 0;
    decimal GT_Ongkos = 0;

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			//HyperLink
			add.HRef += "?id=" + myID;

			//Form
			fillForm();

			//Generate
			fillTable(sender, e);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("ProductAdjusment.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.AdjustmentSouvenir.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {

		}
	}

	//Display Table
	protected void fillTable(object sender, EventArgs e) {
		

		//Stone
		fillProduct();

        //Grand-Total
        Sub_Total(GT_Qty, true);
	}

	//Stone
	protected void fillProduct() {
		//SELECT
		var rs = from p in db.AdjustmentSouvenir_Product
				 where p.IDForm == myID
				 orderby p.Souvenir.Nama
				 select p;


		//RESULT
		int index = 1;
		foreach (var r in rs) {
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = TD_HyperLink(r.ID);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nama(r.Souvenir.Nama);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Info(r.Qty);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

            GT_Qty += r.Qty.Value;

			tb.Rows.Add(tr);
		}
	}

    protected void Sub_Total(decimal TotalQty, bool GrandTotal)
    {
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

        tb.Rows.Add(tr);
    }

	//Display Column
	protected string TD_HyperLink(int TableID) {
		string x = string.Empty;
		string href = string.Empty;

		href += "ProductAdjustmentUpdate.aspx";
		href += "?id=" + myID;
		href += "&tid=" + TableID;

		x += "<div class=\"btn-group\">";
		x += "  <a class=\"btn btn-info\" href=\"" + href + "\">";
		x += "      <i class=\"icon-pencil\"></i> ";
		x += "      Update";
		x += "  </a>";
		x += "  <a class=\"btn btn-danger\" href=\"javascript:goDelete('" + myID + "', '" + TableID + "')\">";
		x += "      <i class=\"icon-trash\"></i> ";
		x += "      Delete";
		x += "  </a>";
		x += "</div>";

		return x;
	}
	protected string TD_Nama(string Nama) {
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";

		return x;
	}
    protected string TD_Info(decimal? Dimensi)
    {
        string x = string.Empty;

        x += "<p>" + Str.Number0(Dimensi) + "</p>";

        return x;
    }

	//Save
	protected void save_Click(object sender, EventArgs e) {
		Response.Redirect("ProductAdjustment_Submit.aspx?id=" + myID);
	}
}