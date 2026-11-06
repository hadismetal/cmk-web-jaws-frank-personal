using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_Design_Form : System.Web.UI.Page {
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
	// protected void fillForm() {
	// 	var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Approval == null && p.Draft == false);
	// 	if (d == null) App.DataNull(this);
	// 	else {
	// 		//Header
	// 		header_nomor.InnerHtml =
	// 			"<a href=\"../History/Design_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
	// 			+ d.Nomor + "</a>";

	// 		//Trans. Form
	// 		designer.InnerHtml = d.DataHuman.Nama;
    //         brand.InnerHtml = d.DataHuman.CompanyBrand.Nama + " (" + d.DataHuman.CompanyBrand.NamaKode + ")";
			
	// 		keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
	// 		tgl.InnerHtml = Str.Tgl(d.Tgl);

	// 		//Trans. Data
	// 		designnomor.InnerHtml = d.DesignKode;
			
	// 		requestcustomernama.InnerHtml = d.RequestCustomerNama;
	// 		humandesignernama.InnerHtml = d.HumanDesignerNama;
			

	// 		//Product Char.
	// 		var product = d.ProductDesign_CharProduct;
	// 		if (product == null) save.Disabled = true;
	// 		else {
	// 			productitem.InnerHtml = product.CharProductItem.Nama + " (" + product.CharProductItem.NamaKode + ")";
	// 			productcategory.InnerHtml = product.CharProductCategory.Nama + " (" + product.CharProductCategory.NamaKode + ")";
	// 			productlevel.InnerHtml = product.CharProductLevel.Nama + " (" + product.CharProductLevel.NamaKode + ")";
	// 			stonedist.InnerHtml = product.CharStoneDist.Nama + " (" + product.CharStoneDist.NamaKode + ")";
	// 			framematerial.InnerHtml = product.CharFrameMaterial.Nama + " (" + product.CharFrameMaterial.NamaKode + ")";
	// 			framefinishing.InnerHtml = product.CharFrameFinishing.Nama + " (" + product.CharFrameFinishing.NamaKode + ")";
	// 			framecolor.InnerHtml = product.CharFrameColor.Nama + " (" + product.CharFrameColor.NamaKode + ")";
	// 			processcons.InnerHtml = product.CharProcessCons.Nama + " (" + product.CharProcessCons.NamaKode + ")";

	// 			//Weight
	// 			grossweight.InnerHtml = Str.Number3(product.GrossWeight);
	// 			stonecarat.InnerHtml = Str.Number3(product.StoneCarat);
	// 			stoneweight.InnerHtml = Str.Number3(product.StoneWeight);
	// 			nettoweight.InnerHtml = Str.Number3(product.NettoWeight);

	// 			//Dimensi
	// 			dimensid.InnerHtml = Str.Number2(product.DimensiD);
	// 			dimensip.InnerHtml = Str.Number2(product.DimensiP);
	// 			dimensil.InnerHtml = Str.Number2(product.DimensiL);
	// 			dimensir.InnerHtml = Str.Number2(product.DimensiR);
	// 		}

	// 		//Design Char.
	// 		var design = d.ProductDesign_CharDesign;
	// 		if (design == null) save.Disabled = true;
	// 		else {
	// 			designcategory.InnerHtml = design.CharDesignCategory.Nama + " (" + design.CharDesignCategory.NamaKode + ")";
	// 			designconcept.InnerHtml = design.CharDesignConcept.Nama + " (" + design.CharDesignConcept.NamaKode + ")";
	// 			designprocess.InnerHtml = design.CharDesignProcess.Nama + " (" + design.CharDesignProcess.NamaKode + ")";
	// 			targetage.InnerHtml = design.CharTargetAge.Nama + " (" + design.CharTargetAge.NamaKode + ")";
	// 			targetgender.InnerHtml = design.CharTargetGender.Nama + " (" + design.CharTargetGender.NamaKode + ")";

	// 			naming.InnerHtml = design.Naming;
	// 			designket.InnerHtml = designKet(design);
	// 			grafir.InnerHtml = design.Grafir;
	// 			finishing.InnerHtml = designFinishing(d);
	// 		}

	// 		//Stone
	// 		fillStone();

	// 		//Costing

	// 		//Picture
	// 		Upload.Image(imgpicture, d.ImgPicture);

	// 		//Pricing Error
	// 		if (Request.QueryString["prcerror"] != null) {
	// 			sebab.InnerHtml = Request.QueryString["prcerror"];
	// 			pricing_ERR.Visible = true;
	// 		}
	// 	}
	// }

	protected void fillForm() {
		var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Approval == null && p.Draft == false);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Design_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Trans. Form
			designer.InnerHtml = d.DataHuman.Nama;
			brand.InnerHtml = d.DataHuman.CompanyBrand.Nama + " (" + d.DataHuman.CompanyBrand.NamaKode + ")";

			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			tgl.InnerHtml = Str.Tgl(d.Tgl);

			//Trans. Data
			designnomor.InnerHtml = d.DesignKode;

			requestcustomernama.InnerHtml = d.RequestCustomerNama;
			humandesignernama.InnerHtml = d.HumanDesignerNama;

			//Product Char.
			var product = d.ProductDesign_CharProduct;
			if (product == null) save.Disabled = true;
			else {
				productitem.InnerHtml = product.CharProductItem.Nama + " (" + product.CharProductItem.NamaKode + ")";
				productcategory.InnerHtml = product.CharProductCategory.Nama + " (" + product.CharProductCategory.NamaKode + ")";
				productlevel.InnerHtml = product.CharProductLevel.Nama + " (" + product.CharProductLevel.NamaKode + ")";
				stonedist.InnerHtml = product.CharStoneDist.Nama + " (" + product.CharStoneDist.NamaKode + ")";
				framematerial.InnerHtml = product.CharFrameMaterial.Nama + " (" + product.CharFrameMaterial.NamaKode + ")";
				framefinishing.InnerHtml = product.CharFrameFinishing.Nama + " (" + product.CharFrameFinishing.NamaKode + ")";
				framecolor.InnerHtml = product.CharFrameColor.Nama + " (" + product.CharFrameColor.NamaKode + ")";
				processcons.InnerHtml = product.CharProcessCons.Nama + " (" + product.CharProcessCons.NamaKode + ")";

				//Weight
				grossweight.InnerHtml = Str.Number3(product.GrossWeight);
				stonecarat.InnerHtml = Str.Number3(product.StoneCarat);
				stoneweight.InnerHtml = Str.Number3(product.StoneWeight);
				nettoweight.InnerHtml = Str.Number3(product.NettoWeight);

				//Dimensi
				dimensid.InnerHtml = Str.Number2(product.DimensiD);
				dimensip.InnerHtml = Str.Number2(product.DimensiP);
				dimensil.InnerHtml = Str.Number2(product.DimensiL);
				dimensir.InnerHtml = Str.Number2(product.DimensiR);

				var dimR2 = db.ExecuteQuery<decimal?>(
					"SELECT DimensiR2 FROM ProductDesign_CharProduct WHERE ID = {0}",
					product.ID
				).FirstOrDefault();

				dimensir2.InnerHtml = dimR2.HasValue ? Str.Number2(dimR2.Value) : "0";
			}

			//Design Char.
			var design = d.ProductDesign_CharDesign;
			if (design == null) save.Disabled = true;
			else {
				designcategory.InnerHtml = design.CharDesignCategory.Nama + " (" + design.CharDesignCategory.NamaKode + ")";
				designconcept.InnerHtml = design.CharDesignConcept.Nama + " (" + design.CharDesignConcept.NamaKode + ")";
				designprocess.InnerHtml = design.CharDesignProcess.Nama + " (" + design.CharDesignProcess.NamaKode + ")";
				targetage.InnerHtml = design.CharTargetAge.Nama + " (" + design.CharTargetAge.NamaKode + ")";
				targetgender.InnerHtml = design.CharTargetGender.Nama + " (" + design.CharTargetGender.NamaKode + ")";

				naming.InnerHtml = design.Naming;
				designket.InnerHtml = designKet(design);
				grafir.InnerHtml = design.Grafir;
				finishing.InnerHtml = designFinishing(d);
			}

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
	protected string designKet(ProductDesign_CharDesign d) {
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
	protected string designFinishing(ProductDesign d) {
		string x = string.Empty;

		var rs = from p in d.ProductDesign_Finishing select p;
		foreach (var r in rs) {
			if (x != "") x += "<br />";
			x += r.CharProcessFinishing.Nama + " (" + r.CharProcessFinishing.NamaKode + ")";
		}

		return x;
	}

	//Stone
	protected void fillStone() {
		int GT_Butir = 0;
		decimal GT_Carat = 0;

		//Stone
		fillStone1A(0, ref GT_Butir, ref GT_Carat);
		fillStone1B(1, ref GT_Butir, ref GT_Carat);
		fillStone2(2, ref GT_Butir, ref GT_Carat);
		fillStone3(3, ref GT_Butir, ref GT_Carat);
		fillStone4(4, ref GT_Butir, ref GT_Carat);
		fillStone5(5, ref GT_Butir, ref GT_Carat);

		//Grand-Total
		Sub_Total(GT_Butir, GT_Carat, true);
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
			td.Text = TD_Nama(r.Stone1A.Nama);
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
	protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat) {
		//SELECT
		var rs = from p in db.ProductDesign_Stone1B
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
			td.Text = TD_Nama(r.Stone1B.Nama, r.GIA, Convert.ToInt32(r.StatusTerpasang));
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
	protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat) {
		//SELECT
		var rs = from p in db.ProductDesign_Stone2
				 where p.IDForm == myID
				 orderby p.Stone2.Nama
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
			td.Text = TD_Nama(r.Stone2.Nama);
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
	protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat) {
		//SELECT
		var rs = from p in db.ProductDesign_Stone3
				 where p.IDForm == myID
				 orderby p.Stone3.Nama
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
			td.Text = TD_Nama(r.Stone3.Nama);
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
	protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat) {
		//SELECT
		var rs = from p in db.ProductDesign_Stone4
				 where p.IDForm == myID
				 orderby p.Stone4.Nama
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
			td.Text = TD_Nama(r.Stone4.Nama);
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
	protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat) {
		//SELECT
		var rs = from p in db.ProductDesign_Stone5
				 where p.IDForm == myID
				 orderby p.Stone5.Nama
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
			td.Text = TD_Nama(r.Stone5.Nama);
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
    protected string TD_Nama(string Nama, string GIA, int Status)
    {
        string x = string.Empty;

        x += "<p><strong>" + Nama + "</strong></p>";
        if (GIA != "")
            x += "<p class=\"text-muted text-sm\"> " + GIA + "</p>";
        if (Status == 1)
            x += "<p class=\"text-muted text-sm\">Terpasang</p>";
        else
            x += "<p class=\"text-muted text-sm\">Tidak Terpasang</p>";

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
		var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Approval == null && p.Draft == false);
		if (d == null) App.DataNull(this);
		else {
			//Redirect
			Response.Redirect("Design_Pricing.aspx?id=" + myID);
		}
	}
}