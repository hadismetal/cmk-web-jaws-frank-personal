using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_GJ_Submit : System.Web.UI.Page {
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
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("GJ.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/GJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Trans. Form
            brand.InnerHtml = d.DataHuman.CompanyBrand.Nama;
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			tgl.InnerHtml = Str.Tgl(d.Tgl);

			//Trans. Data
			
			requestcustomernama.InnerHtml = d.RequestCustomerNama;
			humandesignernama.InnerHtml = d.HumanDesignerNama;

			////Product Char.
			var product = d.ProductDesignGJ_CharProduct;
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
				nettoweight.InnerHtml = Str.Number3(product.NettoWeight);

				//Dimensi
				dimensid.InnerHtml = Str.Number2(product.DimensiD);
				dimensip.InnerHtml = Str.Number2(product.DimensiP);
				dimensil.InnerHtml = Str.Number2(product.DimensiL);
				dimensir.InnerHtml = Str.Number2(product.DimensiR);
			}

			//Design Char.
			var design = d.ProductDesignGJ_CharDesign;
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

			

			//Picture
			Upload.Image(imgpicture, d.ImgPicture);
		}
	}

	//Design Info
	protected string designKet(ProductDesignGJ_CharDesign d) {
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
	protected string designFinishing(ProductDesignGJ d) {
		string x = string.Empty;

		var rs = from p in d.ProductDesignGJ_Finishing select p;
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
	protected string TD_CaratButir(decimal? CaratButir) {
		string x = string.Empty;

		x += "<p class=\"text-muted\">" + Str.Number3(CaratButir) + "</p>";

		return x;
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
		if (d == null) DataError();
		else {
			//Status
			d.Draft = false;
			d.DraftDate = null;

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("GJ.aspx?d=1");
		}
        //Response.Redirect("GJ_Pricing.aspx?id=" + myID);
	}
}