using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_PG_Submit : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("PG.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.StockProductPG.SingleOrDefault(p => p.ID == myID);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Trans. Form
            brand.InnerHtml = d.StockActualPG.CompanyBrand.Nama;
			keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);

			//Trans. Data
            designnomor.InnerHtml = d.Nomor;
            suppliernomor.InnerHtml = d.SupplierSJ;

			////Product Char.
            productitem.InnerHtml = d.CharProductItem.Nama;
            productlevel.InnerHtml = d.CharGoldLevel.Nama;
            model.InnerHtml = d.CharGoldModel.Nama;
            kelompokusia.InnerHtml = d.CharTargetAge.Nama;
            framecolor.InnerHtml = d.CharFrameColor.Nama;

                //Weight
                grossweight.InnerHtml = Str.Number3(d.GrossWeight);
                nettoweight.InnerHtml = Str.Number3(d.NettoWeight);

			//Estimasi Harga
            decimal GoldPrice = GoldPrice = Db.SingleDecimal("SELECT TOP 1 Harga FROM PricingTableTG WHERE ID=3", Db.Cnn_DB);
            decimal Rate = Db.SingleDecimal("SELECT TOP 1 Harga FROM PricingTableUS WHERE ID=2", Db.Cnn_DB);

            decimal MarginPG = Db.SingleDecimal("SELECT TOP 1 Margin FROM CharGoldModel WHERE ID=" + d.Model, Db.Cnn_DB);

            decimal TotalBerat = 1 * d.GrossWeight.Value;
            //decimal Harga = (((d.KadarLogam.Value / 100) * GoldPrice) * Rate) * TotalBerat;
            decimal Harga = d.TotalHargaJual.Value;

            harga.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(Harga).ToString("N0") + "</p>";

			//Picture
			Upload.Image(imgpicture, d.ImgPicture);
		}
	}
}