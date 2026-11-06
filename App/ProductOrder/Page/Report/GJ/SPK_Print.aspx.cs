using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_SPK_Print : System.Web.UI.Page {
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
    protected void DataError() { Response.Redirect("ProductOrderList.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.ProductOrderGJ.SingleOrDefault(p => p.ID == myID);
		if (d == null) DataError();
		else {
			//Header

            if (d.CompanyBrand.NamaKode == "S")
                img_Logo.ImageUrl = "/Images/Logo_The_Palace-4002.jpg";
            else
                img_Logo.ImageUrl = "/Images/Logo_The_Palace-400.jpg";

            lit_NamaCustomer.Text = d.RequestCustomerNama;
            lit_NomorLDKP.Text = d.NoLDKP;
            lit_NamaToko.Text = d.RequestOutletNama;
            lit_NamaKoleksi.Text = d.ProductOrderGJ_CharDesign.Naming;
            if (d.RequestCustomer > 0)
            {
                var u = db.DataCustomer.SingleOrDefault(p => p.ID == d.RequestCustomer);
                lit_NomorCustomer.Text = u.NoCustomer;
                lit_StatusBarang.Text = "Pesanan";
            }
            else
                lit_StatusBarang.Text = "Stock";
            

            if(d.CompanyBrand.NamaKode == "S")
                img_Logo.ImageUrl = "/Images/Logo_The_Palace-4002.jpg";
            else
                img_Logo.ImageUrl = "/Images/Logo_The_Palace-400.jpg";

            lit_NomorSPK.Text = d.NoSPK != null ? d.NoSPK : "";
            lit_TanggalSPK.Text = d.Tgl != null ? Str.Tgl(d.Tgl) : "";
            lit_TanggalPersetujuanSPK.Text = d.ApprovalTgl != null ? Str.Tgl(d.ApprovalTgl) : "";
            lit_TanggalJatuhTempo.Text = (d.TanggalJatuhTempoDJ != null ? Str.Tgl(d.TanggalJatuhTempoDJ) : "");
            lit_NomorInvoiceDP.Text = d.NoInvoice;
            lit_BebanSusut.Text = Convert.ToDecimal(d.ProductOrderGJ_CharProduct.BebanSusut).ToString("N2");
            lit_Berat.Text = d.ProductOrderGJ_CharProduct.GrossWeight.ToString("N2");
            lit_NomorPO.Text = d.NoPO;
            lit_StatusOrder.Text = (d.StatusOrder == 1) ? "New Order" : "Repeat Order";
            lit_Komponen.Text = Convert.ToDecimal(d.ProductOrderGJ_CharProduct.Komponen).ToString("N2");
            lit_TingkatKesulitan.Text = d.ProductOrderGJ_CharProduct.CharProcessCons.Nama;
            lit_PoinKomponen.Text = Convert.ToInt32(d.ProductOrderGJ_CharProduct.PoinKomponen).ToString("N2");
            productcategory.Text = d.ProductOrderGJ_CharProduct.CharProductCategory.Nama;

            var kode = db.KodeKaretGJ_Detail.SingleOrDefault(p => p.IDProductOrder == d.ID);

            try
            {
                lit_KodeKaret.Text = kode.KodeKaretDetail;
            }
            catch (Exception ex)
            {

            }
            try
            {
                kodekaretlama.Text = "<p style=\"font-size: 10px;\">" + kode.KodeKaretGJ.KodeKaretLama + "</p>";
            }
            catch (Exception ex)
            {

            }

            lit_Jenis.Text = d.ProductOrderGJ_CharProduct.CharProductItem.Nama;
            lit_Jumlah.Text = (d.ProductOrderGJ_CharProduct.CharProductItem.NamaKode == "A") ? "1 Psg" : "1"; /// selalu 1 jumlah barangnya (Qty)
            lit_Panjang.Text = d.ProductOrderGJ_CharProduct.DimensiP != null ? d.ProductOrderGJ_CharProduct.DimensiP.ToString("0.00") : "";
            lit_RingSize.Text = d.ProductOrderGJ_CharProduct.DimensiR != null ? Convert.ToDecimal(d.ProductOrderGJ_CharProduct.DimensiR).ToString("N2") : "";
            lit_Lebar.Text = d.ProductOrderGJ_CharProduct.DimensiL != null ? d.ProductOrderGJ_CharProduct.DimensiL.ToString("0.00") : "";
            lit_Diameter.Text = d.ProductOrderGJ_CharProduct.DimensiD != null ? d.ProductOrderGJ_CharProduct.DimensiD.ToString("0.00") : "";
            //lit_Berat.Text = rs_Berat.GrossWeight != null ? rs_Berat.GrossWeight.ToString("0.000") : "";
            lit_WarnaEmas.Text = d.ProductOrderGJ_CharProduct.CharFrameColor.Nama;
            //lit_NomorCustomer.Text = d.RequestCustomer.ToString();
            lit_NamaCustomer.Text = (d.RequestCustomerNama!=null) ? d.RequestCustomerNama : "";
            lit_KelasHarga.Text = d.CharProductSegmen.Nama;

            /// List Parcel
            lit_Parcel.Text = "";

            decimal Jumlah_Butir = 0m;
            decimal Jumlah_CaratPerButir = 0m;
            decimal Jumlah_Carat = 0m;

            decimal Total_Butir = 0m;
            decimal Total_CaratPerButir = 0m;
            decimal Total_Carat = 0m;


            lit_JumlahButir.Text = Total_Butir.ToString("0");
            lit_CaratPerButir.Text = Total_CaratPerButir.ToString("0.000");
            lit_TotalCarat.Text = Total_Carat.ToString("0.000");

            /// Kode Komponen & Jumlah
            //lit_KodeKomponen_1.Text = d.ProductOrderGJ_CharProduct.KodeKomponen1 != null ? d.ProductOrderGJ_CharProduct.KodeKomponen1 : "";
            //lit_KodeKomponen_2.Text = d.ProductOrderGJ_CharProduct.KodeKomponen2 != null ? d.ProductOrderGJ_CharProduct.KodeKomponen2 : "";
            //lit_KodeKomponen_3.Text = d.ProductOrderGJ_CharProduct.KodeKomponen3 != null ? d.ProductOrderGJ_CharProduct.KodeKomponen3 : "";
            //lit_KodeKomponen_4.Text = d.ProductOrderGJ_CharProduct.KodeKomponen4 != null ? d.ProductOrderGJ_CharProduct.KodeKomponen4 : "";
            //lit_KodeKomponen_5.Text = d.ProductOrderGJ_CharProduct.KodeKomponen5 != null ? d.ProductOrderGJ_CharProduct.KodeKomponen5 : "";
            //lit_KodeKomponen_6.Text = d.ProductOrderGJ_CharProduct.KodeKomponen6 != null ? d.ProductOrderGJ_CharProduct.KodeKomponen6 : "";
            //lit_JumlahKomponen_1.Text = d.ProductOrderGJ_CharProduct.JlhKomponen1 != null ? Convert.ToDecimal(d.ProductOrderGJ_CharProduct.JlhKomponen1).ToString("N2") : "";
            //lit_JumlahKomponen_2.Text = d.ProductOrderGJ_CharProduct.JlhKomponen2 != null ? Convert.ToDecimal(d.ProductOrderGJ_CharProduct.JlhKomponen2).ToString("N2") : "";
            //lit_JumlahKomponen_3.Text = d.ProductOrderGJ_CharProduct.JlhKomponen3 != null ? Convert.ToDecimal(d.ProductOrderGJ_CharProduct.JlhKomponen3).ToString("N2") : "";
            //lit_JumlahKomponen_4.Text = d.ProductOrderGJ_CharProduct.JlhKomponen4 != null ? Convert.ToDecimal(d.ProductOrderGJ_CharProduct.JlhKomponen4).ToString("N2") : "";
            //lit_JumlahKomponen_5.Text = d.ProductOrderGJ_CharProduct.JlhKomponen5 != null ? Convert.ToDecimal(d.ProductOrderGJ_CharProduct.JlhKomponen5).ToString("N2") : "";
            //lit_JumlahKomponen_6.Text = d.ProductOrderGJ_CharProduct.JlhKomponen6 != null ? Convert.ToDecimal(d.ProductOrderGJ_CharProduct.JlhKomponen6).ToString("N2") : "";

            /// Size & Grafir
            lit_Size_2.Text = d.ProductOrderGJ_CharProduct.DimensiR != null ? Convert.ToDecimal(d.ProductOrderGJ_CharProduct.DimensiR).ToString("N2") : "";
            

            string Grafir = d.ProductOrderGJ_CharDesign.Grafir != null ? d.ProductOrderGJ_CharDesign.Grafir : "";
            string[] Grafir_Array = Grafir.Select(c => c.ToString()).ToArray();

            //lit_Grafir.Text = Grafir;
            if (Grafir != "")
            {
                lit_Grafir_1.Text = Grafir_Array.Length >= 1 ? Grafir_Array[0] : "";
                lit_Grafir_2.Text = Grafir_Array.Length >= 2 ? Grafir_Array[1] : "";
                lit_Grafir_3.Text = Grafir_Array.Length >= 3 ? Grafir_Array[2] : "";
                lit_Grafir_4.Text = Grafir_Array.Length >= 4 ? Grafir_Array[3] : "";
                lit_Grafir_5.Text = Grafir_Array.Length >= 5 ? Grafir_Array[4] : "";
                lit_Grafir_6.Text = Grafir_Array.Length >= 6 ? Grafir_Array[5] : "";
                lit_Grafir_7.Text = Grafir_Array.Length >= 7 ? Grafir_Array[6] : "";
                lit_Grafir_8.Text = Grafir_Array.Length >= 8 ? Grafir_Array[7] : "";
                lit_Grafir_9.Text = Grafir_Array.Length >= 9 ? Grafir_Array[8] : "";
                lit_Grafir_10.Text = Grafir_Array.Length >= 10 ? Grafir_Array[9] : "";
                lit_Grafir_11.Text = Grafir_Array.Length >= 11 ? Grafir_Array[10] : "";
                lit_Grafir_12.Text = Grafir_Array.Length >= 12 ? Grafir_Array[11] : "";
                lit_Grafir_13.Text = Grafir_Array.Length >= 13 ? Grafir_Array[12] : "";
                lit_Grafir_14.Text = Grafir_Array.Length >= 14 ? Grafir_Array[13] : "";
                lit_Grafir_15.Text = Grafir_Array.Length >= 15 ? Grafir_Array[14] : "";
                lit_Grafir_16.Text = Grafir_Array.Length >= 16 ? Grafir_Array[15] : "";
                lit_Grafir_17.Text = Grafir_Array.Length >= 17 ? Grafir_Array[16] : "";
                lit_Grafir_18.Text = Grafir_Array.Length >= 18 ? Grafir_Array[17] : "";
                lit_Grafir_19.Text = Grafir_Array.Length >= 19 ? Grafir_Array[18] : "";
            }

            /// Titipan
            lit_Titipan.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_EmasBerat.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_StoneCarat.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_StoneButir.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_ColourStoneCarat.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_ColourStoneButir.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_Cabang.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_Pusat.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_Factory.Text = ""; // kosongin dulu sementara karena belum konek pos

            /// Keterangan Perubahan SPK
            lit_KeteranganPerubahanSPK.Text = d.Keterangan != null ? d.Keterangan : "";

            /// Nomor Dokumen & Kode Karet Lama
            //lit_KodeKaretLama.Text = d.KodeKaretLama != null ? d.KodeKaretLama : "";

            //Picture
            Upload.Image(imgpicture, d.ImgPicture);
		}
	}

	
}