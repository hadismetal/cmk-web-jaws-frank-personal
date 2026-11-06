using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_SPK_Form : System.Web.UI.Page {
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
		var d = db.ProductOrderGJ.SingleOrDefault(p => p.ID == myID && p.StatusPenerimaan == 0 && p.Draft == false);
		if (d == null) App.DataNull(this);
		else {
			//Header
            if (d.ProductDesignGJ != null)
            {
                header_nomor.InnerHtml =
                    "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                    + d.ProductDesignGJ.Nomor + "</a>";
            }
            else
            {
                header_nomor.InnerHtml =
                    "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                    + d.ID.ToString() + "</a>";
            }
			
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);
			tgl.InnerHtml = Str.Tgl(d.Tgl);

			//Trans. Data
            if (d.ProductDesignGJ != null)
            {
                designnomor.InnerHtml = d.ProductDesignGJ.Nomor;
                humandesignernama.InnerHtml = d.ProductDesignGJ.DataHuman.Nama;
            }
            else if (d.IDKodeKaret != null)
                designnomor.InnerHtml = d.KodeKaretGJ.Nomor;
			
			requestcustomernama.InnerHtml = d.RequestCustomerNama;

			//Product Char.
			var product = d.ProductOrderGJ_CharProduct;
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

			//SPK Char.
			var design = d.ProductOrderGJ_CharDesign;
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

			//Picture
			Upload.Image(imgpicture, d.ImgPicture);

			//Pricing Error
			if (Request.QueryString["prcerror"] != null) {
				sebab.InnerHtml = Request.QueryString["prcerror"];
				pricing_ERR.Visible = true;
			}
		}
	}

	//SPK Info
    protected string designKet(ProductOrderGJ_CharDesign d)
    {
		string x = string.Empty;

		if (d.SetItem) {
			if (x != "") x += ", ";
			x += "Set Item";
		}
		if (d.Special) {
			if (x != "") x += ", ";
			x += "Special SPK";
		}

		return x;
	}
	protected string designFinishing(ProductOrderGJ d) {
		string x = string.Empty;

		var rs = from p in d.ProductOrderGJ_Finishing select p;
		foreach (var r in rs) {
			if (x != "") x += "<br />";
			x += r.CharProcessFinishing.Nama + " (" + r.CharProcessFinishing.NamaKode + ")";
		}

		return x;
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.ProductOrderGJ.SingleOrDefault(p => p.ID == myID && p.StatusPenerimaan == 0 && p.Draft == false);
		if (d == null) App.DataNull(this);
		else {
			//Redirect
			Response.Redirect("SPK_Approve.aspx?id=" + myID);
		}
	}
}