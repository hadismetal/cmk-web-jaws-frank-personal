using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_Design_PricingSubmit : System.Web.UI.Page {
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
		App.ProtectRole(Role.Manager);

		if (!IsPostBack) {
			fillForm();
		}
	}

	//Form
	protected void fillForm() {
		var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.StatusReceive == 0 && p.Draft == false);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Design_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Stone
			fillStone();
		}
	}

	//Stone
	protected void fillStone() {
		int GT_Butir = 0;
		decimal GT_Harga = 0;

		//Stone
		fillStone1A(0, ref GT_Butir, ref GT_Harga);
		

		//Grand-Total
		Sub_Total(GT_Butir, GT_Harga, true);
	}

	//Stone
	protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat) {
		//SELECT
		var rs = from p in db.ProductDesign_Stone1A
				 where p.IDForm == myID
				 orderby p.Stone1A.Nama
				 select p;

		//Sub-Total
		int ST_Butir = 0;
		decimal ST_Harga = 0;

		//Sub-Header
		if (rs.Count() > 0) Sub_Header(StoneIndex);

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
			td.Text = TD_Nama(r.Stone1A.Nama);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Tgl(r.Efektif);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Harga(r.HargaSatuan);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Harga(r.HargaTotal, ref ST_Harga);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}

		//Total
		if (rs.Count() > 0) {
			GT_Butir += ST_Butir;
			GT_Carat += ST_Harga;

			Sub_Total(ST_Butir, ST_Harga, false);
		}
	}

	//Custom Row
	protected void Sub_Header(int StoneIndex) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = "sub-header";
		string x = string.Empty;

		x += "<br />";
		x += DL.StoneIndex(StoneIndex);

		td = new TableCell();
		td.Text = x;
		td.CssClass = "text-lg text-warning";
		td.ColumnSpan = App.TableColspan(tb);
		tr.Cells.Add(td);

		tb.Rows.Add(tr);
	}
	protected void Sub_Total(int TotalButir, decimal TotalHarga, bool GrandTotal) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

		td = new TableCell();
		td.ColumnSpan = 4;
		td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
		td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
		td.Text += "</p>";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = "<p class=\"text-lg\">" + Str.Integer(TotalButir) + "</p>";
		td.CssClass = "text-center";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = "<p class=\"text-lg\">" + Str.Number2(TotalHarga) + "</p>";
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
	protected string TD_Tgl(DateTime? Tgl) {
		string x = string.Empty;

		x += "<p>" + Str.Tgl(Tgl) + "</p>";

		return x;
	}
	protected string TD_TotalButir(int? TotalButir, ref int ST) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-success\">" + Str.Integer(TotalButir) + "</p>";

		//Sub-Total
		if (TotalButir.HasValue) ST += TotalButir.Value;

		return x;
	}
	protected string TD_Harga(decimal? Harga) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-info\">" + Str.Number2(Harga) + "</p>";

		return x;
	}
	protected string TD_Harga(decimal? Harga, ref decimal ST) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-info\">" + Str.Number2(Harga) + "</p>";

		//Sub-Total
		if (Harga.HasValue) ST += Harga.Value;

		return x;
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.StatusReceive == 0 && p.Draft == false);
		if (d == null) App.DataNull(this);
		else {
			//Redirect
            d.Draft = true;
            db.SubmitChanges();
            App.ReloadClose(this);
			//Response.Redirect("Design_ProductPosting.aspx?id=" + myID);
		}
	}
}