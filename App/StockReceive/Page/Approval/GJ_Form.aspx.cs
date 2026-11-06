using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_GJ_Form : System.Web.UI.Page {
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
		var d = db.StockReceiveGJ.SingleOrDefault(p => p.ID == myID && p.StatusReceive == 0 && p.Draft == false);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/GJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Trans. Form
			warehouse.InnerHtml = d.LocWarehouse.Nama;
			brand.InnerHtml = d.LocWarehouse.CompanyBrand.Nama + " (" + d.LocWarehouse.CompanyBrand.NamaKode + ")";
			sumber.InnerHtml = DL.SumberStockReceive(d.Sumber);
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			tgl.InnerHtml = Str.Tgl(d.Tgl);

			//Trans. Data
			protonomor.InnerHtml = d.ProtoNomor;
			productordernomor.InnerHtml = d.ProductOrderNomor;
			suppliernama.InnerHtml = d.SupplierNama;
			suppliernomor.InnerHtml = d.SupplierNomor;
			requestoutletnama.InnerHtml = d.RequestOutletNama;
			requestcustomernama.InnerHtml = d.RequestCustomerNama;
			humandesignernama.InnerHtml = d.HumanDesignerNama;
			humanstocknama.InnerHtml = d.HumanStockNama;
			expiredproduct.InnerHtml = Str.Tgl(d.ExpiredProduct);
			expiredrequest.InnerHtml = Str.Tgl(d.ExpiredRequest);

			//Product Char.
			var product = d.StockReceiveGJ_CharProduct;
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
				grossweight.InnerHtml = Str.Number2(product.GrossWeight);
				nettoweight.InnerHtml = Str.Number2(product.NettoWeight);

				//Dimensi
				dimensid.InnerHtml = Str.Number2(product.DimensiD);
				dimensip.InnerHtml = Str.Number2(product.DimensiP);
				dimensil.InnerHtml = Str.Number2(product.DimensiL);
				dimensir.InnerHtml = Str.Number2(product.DimensiR);
			}

			//Design Char.
			var design = d.StockReceiveGJ_CharDesign;
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

			//Costing
			var costing = d.StockReceiveGJ_Costing;
			if (costing == null) save.Disabled = true;
			else {
				costing_unitprice.InnerHtml = Str.Number0(costing.UnitPrice);
                costing_frame.InnerHtml = Str.Number0(costing.Frame);
				costing_mounting.InnerHtml = Str.Number0(costing.MountingNO + costing.MountingRO);
				costing_setting.InnerHtml = Str.Number0(costing.Setting);
				costing_finishing.InnerHtml = Str.Number0(costing.Finishing);
                costing_ongkoslain.InnerHtml = Str.Number0(costing.OngkosLainCOGS);
				costing_totalbiaya.InnerHtml = Str.Number0(costing.TotalRupiah);
			}

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
	protected string designKet(StockReceiveGJ_CharDesign d) {
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
	protected string designFinishing(StockReceiveGJ d) {
		string x = string.Empty;

		var rs = from p in d.StockReceiveGJ_Finishing select p;
		foreach (var r in rs) {
			if (x != "") x += "<br />";
			x += r.CharProcessFinishing.Nama + " (" + r.CharProcessFinishing.NamaKode + ")";
		}

		return x;
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
	protected string TD_TotalCarat(decimal? TotalCarat, ref decimal ST) {
		string x = string.Empty;

		x += "<p class=\"text-lg text-info\">" + Str.Number3(TotalCarat) + "</p>";

		//Sub-Total
		if (TotalCarat.HasValue) ST += TotalCarat.Value;

		return x;
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.StockReceiveGJ.SingleOrDefault(p => p.ID == myID && p.StatusReceive == 0 && p.Draft == false);
		if (d == null) App.DataNull(this);
		else {
			//Redirect
			Response.Redirect("GJ_Pricing.aspx?id=" + myID);
		}
	}
}