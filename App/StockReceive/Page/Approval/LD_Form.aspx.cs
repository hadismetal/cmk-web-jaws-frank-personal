using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_LD_Form : System.Web.UI.Page {
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
		var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.StatusReceive == 0 && p.Draft == false);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/LD_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Trans. Form
			warehouse.InnerHtml = d.LocWarehouse.Nama;
			brand.InnerHtml = d.LocWarehouse.CompanyBrand.Nama + " (" + d.LocWarehouse.CompanyBrand.NamaKode + ")";
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			tgl.InnerHtml = Str.Tgl(d.Tgl);

			//Trans. Data
			suppliernama.InnerHtml = d.SupplierNama;
			suppliernomor.InnerHtml = d.SupplierNomor;
			requestoutletnama.InnerHtml = d.RequestOutletNama;
			requestcustomernama.InnerHtml = d.RequestCustomerNama;
			humanstocknama.InnerHtml = d.HumanStockNama;
			expiredproduct.InnerHtml = Str.Tgl(d.ExpiredProduct);
			expiredrequest.InnerHtml = Str.Tgl(d.ExpiredRequest);

			
			//Stone
			fillStone();

			//Picture
			Upload.Image(imgpicture, d.ImgPicture);

			//Pricing Error
			if (Request.QueryString["prcerror"] != null) {
				sebab.InnerHtml = Request.QueryString["prcerror"];
				pricing_ERR.Visible = true;
			}
		}
	}

	//Design Info
	protected string designKet(StockReceiveLD_CharDesign d) {
		string x = string.Empty;

		if (d.SetItem) {
			if (x != "") x += ", ";
			x += "Set Item";
		}
		if (d.Special) {
			if (x != "") x += ", ";
			x += "Special Design";
		}

		return x;
	}
	
	//Stone
	protected void fillStone() {
		int GT_Butir = 0;
		decimal GT_Carat = 0;

		//Stone
		fillStone1B(1, ref GT_Butir, ref GT_Carat);

		//Grand-Total
		Sub_Total(GT_Butir, GT_Carat, true);
	}

	//Stone
	protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat) {
		//SELECT
		var rs = from p in db.StockReceiveLD_Stone1B
				 where p.IDForm == myID
				 orderby p.Stone1B.Nama
				 select p;

		//Sub-Total
		int ST_Butir = 0;
		decimal ST_Carat = 0;

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
			td.Text = TD_Nama(r.Stone1B.Nama, r.GIA);
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Dimensi(r.DimensiP);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Dimensi(r.DimensiL);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Dimensi(r.DimensiT);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_CaratButir(r.CaratButir);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_TotalButir(r.TotalButir, ref ST_Butir);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_TotalCarat(r.TotalCarat, ref ST_Carat);
			td.CssClass = "text-right";
			tr.Cells.Add(td);

			tb.Rows.Add(tr);
		}

		//Total
		if (rs.Count() > 0) {
			GT_Butir += ST_Butir;
			GT_Carat += ST_Carat;

			Sub_Total(ST_Butir, ST_Carat, false);
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
	protected void Sub_Total(int TotalButir, decimal TotalCarat, bool GrandTotal) {
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

		td = new TableCell();
		td.ColumnSpan = 6;
		td.Text += "<p><i class=\"icon-plus-sign-alt\"></i> ";
		td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
		td.Text += "</p>";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = "<p class=\"text-lg\">" + Str.Integer(TotalButir) + "</p>";
		td.CssClass = "text-center";
		tr.Cells.Add(td);

		td = new TableCell();
		td.Text = "<p class=\"text-lg\">" + Str.Number3(TotalCarat) + "</p>";
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
	protected string TD_Dimensi(decimal? Dimensi) {
		string x = string.Empty;

		x += "<p>" + Str.Number2(Dimensi) + "</p>";

		return x;
	}
	protected string TD_CaratButir(decimal? CaratButir) {
		string x = string.Empty;

		x += "<p class=\"text-muted\">" + Str.Number3(CaratButir) + "</p>";

		return x;
	}
	protected string TD_TotalButir(int? TotalButir, ref int ST) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-success\">" + Str.Integer(TotalButir) + "</p>";

		//Sub-Total
		if (TotalButir.HasValue) ST += TotalButir.Value;

		return x;
	}
	protected string TD_TotalCarat(decimal? TotalCarat, ref decimal ST) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-info\">" + Str.Number3(TotalCarat) + "</p>";

		//Sub-Total
		if (TotalCarat.HasValue) ST += TotalCarat.Value;

		return x;
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.StatusReceive == 0 && p.Draft == false);
		if (d == null) App.DataNull(this);
		else {
			//Redirect
			Response.Redirect("LD_Pricing.aspx?id=" + myID);
		}
	}
}