using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;

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
        else
        {
            if (d.Legacy == 1) { legacy_ERR.Visible = true; }

			//Header
            header_nomor.InnerHtml = d.Nomor;

			//Trans. Form
            brand.InnerHtml = (d.LocWarehouse != null) ? d.LocWarehouse.CompanyBrand.Nama : "";
			//keterangan.InnerHtml = Str.HtmlBreak(d.Keterangan);

			//Trans. Data
            designnomor.InnerHtml = d.Nomor;
            suppliernomor.InnerHtml = d.SupplierSJ;
            kodebarangnomor.InnerHtml = d.KodeBarang;

			////Product Char.
            productitem.InnerHtml = d.CharProductItem.Nama;
            productlevel.InnerHtml = d.CharGoldLevel.Nama;
            model.InnerHtml = d.CharGoldModel.Nama;
            kelompokusia.InnerHtml = d.CharTargetAge.Nama;
            framecolor.InnerHtml = d.CharFrameColor.Nama;

                //Weight
                grossweight.InnerHtml = Str.Number3(d.GrossWeight);
                nettoweight.InnerHtml = Str.Number3(d.NettoWeight);
				dimensir.InnerHtml = Str.Number2(d.DimensiR);

			//Estimasi Harga
            decimal GoldPrice = GoldPrice = Db.SingleDecimal("SELECT TOP 1 Harga FROM PricingTableTG WHERE ID=3", Db.Cnn_DB);
            decimal Rate = Db.SingleDecimal("SELECT TOP 1 Harga FROM PricingTableUS WHERE ID=2", Db.Cnn_DB);

            decimal MarginPG = Db.SingleDecimal("SELECT TOP 1 Margin FROM CharGoldModel WHERE ID=" + d.Model, Db.Cnn_DB);

            decimal TotalBerat = 1 * d.GrossWeight.Value;
            //decimal Harga = (((d.KadarLogam.Value / 100) * GoldPrice) * Rate) * TotalBerat;
            decimal Harga = d.TotalHargaJual.Value;
			
			decimal HargaPromo = 0m;

            var rs = from p in db.PromoPG_Detail
                     where p.PromoPG.ApprovalNama != null && p.PromoPG.TglStart.Value <= DateTime.Today
                     && p.PromoPG.TglExpire.Value >= DateTime.Today && p.IDProduct.Value == d.ID
                     && (p.PromoPG.StatusActive != false || p.PromoPG.StatusActive == null)
                     select p;
            if (rs.Count() > 0)
            {
                var disc = rs.First();
                if (disc.PromoPG.Tipe == 1)
                {
                    decimal DiscountHarga = 0m;
                    decimal DiscountPersen = 0m;
                    decimal Discount = 0m;
                    decimal TotalBayar = d.TotalHargaJual.Value;

                    DiscountHarga = disc.PromoPG.DiscountNominal.Value;
                    DiscountPersen = (TotalBayar * (disc.PromoPG.DiscountPersen.Value / 100));
                    Discount = (DiscountHarga + DiscountPersen);
                    HargaPromo = TotalBayar - Discount;
                }
                if (disc.PromoPG.Tipe == 3)
                {
                    HargaPromo = disc.PromoPG.PromoHarga.Value;

                }

                //Tambah kondisi Bu Mirah (SK) untuk harga promo lebih besar dari harga Stock
                if (HargaPromo >= Harga)
                {
                    Harga = HargaPromo;
                    HargaPromo = 0m;
                }
            }

            using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
            {
                string queryCheckTerjual = @"SELECT COUNT('') [Total] FROM StockProductPG A 
                                            JOIN SalesOrderPG B ON A.ID = B.IDProduct 
                                            JOIN SalesOrder C ON B.IDForm = C.ID 
                                            WHERE C.StatusVoid = 0 AND C.StatusPembayaran = 1 AND A.ID = @IDProduct";
                connection.Open();

                int isTerjual = 0;
                int isCrossBrand = 0;

                using (SqlCommand command = new SqlCommand(queryCheckTerjual, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("IDProduct", SqlDbType.Int) { Value = myID });
                    isTerjual = Convert.ToInt32(command.ExecuteScalar());
                }

                string queryCheckCrossbrand = @"SELECT COUNT('') [Total] FROM CrossBrand_StockOutgoingPG A
                                                JOIN CrossBrand_StockOutgoingPG_Product B ON A.ID = B.IDForm
                                                JOIN StockProductPG C ON B.IDProduct = C.ID
                                                JOIN StockActualPG D ON C.ID = D.IDProduct
                                                WHERE C.ID = @IDProduct AND D.IDLokasi = -1 AND D.InHand = 0";

                using (SqlCommand command = new SqlCommand(queryCheckCrossbrand, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("IDProduct", SqlDbType.Int) { Value = myID });
                    isCrossBrand = Convert.ToInt32(command.ExecuteScalar());
                }

                if (isTerjual > 0)
                {
                    harga.InnerHtml = "<p class=\"text-lg text-danger\">Barang sudah terjual. Pastikan ke SK untuk harga terbaru!</p>";
                    hargapromo.InnerHtml = "<p class=\"text-lg text-danger\">-</p>";
                }
                else
                {
                    if (isCrossBrand > 0)
                    {
                        harga.InnerHtml = "<p class=\"text-lg text-danger\">Barang tidak ditemukan (Cross Brand).</p>";
                        hargapromo.InnerHtml = "<p class=\"text-lg text-danger\">-</p>";
                    }
                    else
                    {
                        if (d.FixRate == false || !d.FixRate.HasValue)
                        {
                            harga.InnerHtml = "<p class=\"text-lg text-danger\">Harga barang sesuai TGP hari ini.</p>";
                            hargapromo.InnerHtml = "";
                        }
                        else
                        {
                            harga.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(Harga).ToString("N0") + "</p>";
                            hargapromo.InnerHtml = "<p class=\"text-lg text-info\">" + ((HargaPromo > 0) ? Convert.ToDecimal(HargaPromo).ToString("N0") : "-") + "</p>";
                        }
                    }
                }
            }

            //harga.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(Harga).ToString("N0") + "</p>";
            //hargapromo.InnerHtml = "<p class=\"text-lg text-info\">" + ((HargaPromo > 0) ? Convert.ToDecimal(HargaPromo).ToString("N0") : "-") + "</p>";
            //Picture
            Upload.Image(imgpicture, d.ImgPicture);
		}
	}
}