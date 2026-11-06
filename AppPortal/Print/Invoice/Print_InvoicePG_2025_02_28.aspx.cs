using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Page_Summary : System.Web.UI.Page
{
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

	protected void Page_Load(object sender, EventArgs e)
	{
		//App.Protect();
		//App.ProtectRole(Role.Operator);

		if (!IsPostBack)
		{
			Fill();

		}
	}
	protected void Fill()
	{
		var d = db.SalesOrderPG.SingleOrDefault(p => p.ID == myID);
		var c = Db.SingleDecimal("Select top 1 isnull(BeratTimbang,0.00) BeratTimbang from SalesOrderPG WHERE ID = " + d.ID + " ", Db.Cnn_DB);
		if (d == null)
		{
			Response.Redirect("Sales.aspx");
		}
		else
		{
			//Picture
			//Upload.Image(imgpicture, d.StockProductPG.ImgPicture);
			imgpicture.ImageUrl = "https://api-frank.cmk.co.id/api/Image?img=" + d.StockProductPG.ImgPicture;

			string BrandText = "CMK";
			string Cabang = "";
			string AlamatCabang = "";
			string KodeLokasi = "";
			bool IsUsePPN = false;

			if (d.SalesOrder.TipeLokasi == 1)
			{
				var x = db.LocOutlet.SingleOrDefault(p => p.ID == d.SalesOrder.IDLokasi);

				BrandText = x.CompanyBrand.Nama;
				Cabang = x.Nama;
				AlamatCabang = x.AddrAlamat;
				KodeLokasi = x.Kode;

				IsUsePPN = Db.SingleBool("SELECT TOP 1 IsUsePPN FROM LocOutlet WHERE ID = " + x.ID, Db.Cnn_DB);
				lblnpwp.InnerText = "NPWP : " + Db.SingleString("SELECT TOP 1 NPWP FROM LocOutlet WHERE ID = " + x.ID, Db.Cnn_DB);
			}
			else
			{
				var x = db.LocExhibition.SingleOrDefault(p => p.ID == d.SalesOrder.IDLokasi);

				// corporate exhibition
				if (d.SalesOrder.IDLokasi == 18 && x.Kode == "XPR")
				{
					BrandText = "CMK";
				}
				else
				{
					BrandText = x.CompanyBrand.Nama;
				}
				Cabang = x.Nama;
				cabang.Text = Cabang;
				AlamatCabang = x.AddrAlamat;
				KodeLokasi = x.Kode;

				IsUsePPN = Db.SingleBool("SELECT TOP 1 IsUsePPN FROM LocExhibition WHERE ID = " + x.ID, Db.Cnn_DB);
				lblnpwp.InnerText = "NPWP : " + Db.SingleString("SELECT TOP 1 NPWP FROM LocExhibition WHERE ID = " + x.ID, Db.Cnn_DB);
			}

			BrandText = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(BrandText.ToLower());

			lit_brand1.Text = BrandText;
			lit_brand2.Text = BrandText;
			lit_brand3.Text = BrandText;
			lit_brand4.Text = BrandText;
			lit_brand5.Text = BrandText;
			lit_brand6.Text = BrandText;
			lit_brand7.Text = BrandText;
			lit_brand8.Text = BrandText;
			lit_brand9.Text = BrandText;
			lit_brand10.Text = BrandText;
			lit_brand11.Text = BrandText;
			lit_brand12.Text = BrandText;
			lit_brand13.Text = BrandText;
			lit_brand14.Text = BrandText;

			alamat.Text = Str.HtmlBreak(AlamatCabang);
			tgl.Text = Str.Tgl(d.SalesOrder.Tgl);
			terbilang.Text = Str.Terbilang(d.TotalBayar.Value);
			noinvoice.Text = d.Invoice;

			customer.Text = Str.CapUpper(d.SalesOrder.CustomerNama);
			nocustomer.Text = Str.CapUpper(d.SalesOrder.DataCustomer.NoCustomer);
			customer2.Text = Str.CapUpper(d.SalesOrder.CustomerNama);
			customerother.Text = Str.CapUpper(String.IsNullOrEmpty(d.SalesOrder.CustomerReferenceNama) ? d.SalesOrder.CustomerNama : d.SalesOrder.CustomerReferenceNama);
			string salescrossbrand = Db.SingleString("SELECT TOP 1 ISNULL(NamasalesCross,'') [SalesCross] FROM SalesOrder WHERE ID = " + d.SalesOrder.ID, Db.Cnn_DB);

			adminsales.Text = (!string.IsNullOrEmpty(salescrossbrand)) ? salescrossbrand : Str.CapUpper(d.SalesOrder.SalesNama);

			decimal DPP = 0m;
			decimal PPN = 0m;

			lit_ppn.Text = "2,2 %";
			lit_ppn2.Text = "2,2 %";
			lblppn.InnerText = "11%";
			decimal rateppn = 1.022m;
			int ppnunit = 11;
			if (d.SalesOrder.OperatorTgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
			{
				lit_ppn.Text = "2 %";
				lit_ppn2.Text = "2 %";
				rateppn = 1.02m;
				ppnunit = 10;
			}
			else if (d.SalesOrder.Tgl <= Convert.ToDateTime("2023-04-30 23:59:00.000"))
			{
				lit_ppn.Text = "2,2 %";
				lit_ppn2.Text = "2,2 %";
				rateppn = 1.022m;
				ppnunit = 11;
			}
			else if (d.SalesOrder.Tgl <= Convert.ToDateTime("2024-12-30 23:59:00.000"))
			{
				if (Db.SingleString("select b.Keterangan from DataSupplier a join DataSupplierGroup b on a.IDGroup = b.ID where a.ID = " + d.StockProductPG.IDSupplier, Db.Cnn_DB).Contains("In"))
				{
					lit_ppn.Text = "1,65 %";
					lit_ppn2.Text = "1,65 %";
					rateppn = 1.0165m;
					ppnunit = 11;
				}
				else if (Db.SingleString("select b.Keterangan from DataSupplier a join DataSupplierGroup b on a.IDGroup = b.ID where a.ID = " + d.StockProductPG.IDSupplier, Db.Cnn_DB).Contains("Out"))
				{
					lit_ppn.Text = "1,1 %";
					lit_ppn2.Text = "1,1 %";
					rateppn = 1.011m;
					ppnunit = 11;
				}
				else
				{
					lit_ppn.Text = "1,65 %";
					lit_ppn2.Text = "1,65 %";
					rateppn = 1.0165m;
					ppnunit = 11;
				}
			}
			else
			{
				string getKeterangan = @"select CAST(ax.PPNPercentage AS Numeric(9,4)) PPNPercentage,CAST(ax.PPNPembagi AS Numeric(9,4)) PPNPembagi,IIF(Source = 'Insource','15%','10%') AS [PPN2]
										from (
											SELECT TOP 1 a.*,IIF(LEFT(d.Keterangan,3) = 'Out','Outsource','Insource') Source
											FROM SalesOrderPG A 
											JOIN StockProductPG B ON A.IDProduct = B.ID 
											LEFT JOIN DataSupplier C ON B.IDSupplier = C.ID 
											LEFT JOIN DataSupplierGroup D ON C.IDGroup = D.ID
											WHERE a.ID = @ID
											)ax";
				using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString))
				{
					using (SqlCommand cmd = new SqlCommand(getKeterangan, conn))
					{
						cmd.Parameters.AddWithValue("@ID", myID);
						conn.Open();
						using (SqlDataReader reader = cmd.ExecuteReader())
						{
							// Loop through the results
							while (reader.Read())
							{
								rateppn = reader.GetDecimal(reader.GetOrdinal("PPNPembagi"));
								ppnunit = Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal("PPNPercentage")));
								lblppn.InnerText = Convert.ToString(reader.GetDecimal(reader.GetOrdinal("PPNPercentage")));
								lblppn2.InnerText = Convert.ToString(reader["PPN2"]);
							}
						}
						conn.Close();
					}
				}
				
			}

			if (!IsUsePPN || d.StockProductPG.CharProductItem.Nama == "LM")
			{
				lit_ppn.Text = "0 %";
				lit_ppn2.Text = "0 %";
				ppnunit = 0;

				divuseppn.Visible = false;
				lit_ppn_notice.Visible = false;
				lit_ppn_notice2.Visible = false;

				lblppn10.InnerHtml = "<em>Price</em> / Harga Jual ";
			}
			else
			{
				divuseppn.Visible = true;
				lit_ppn_notice.Visible = true;
				lit_ppn_notice2.Visible = true;

				lblppn10.InnerHtml = "<em>Price Exclude VAT</em> / Harga Jual Tidak Termasuk PPN ";
			}

			DPP = (!IsUsePPN || d.StockProductPG.CharProductItem.Nama == "LM") ? (d.TotalBayar.Value) : (d.TotalBayar.Value) / rateppn;
			PPN = d.TotalBayar.Value - DPP;

			// try
			// {
			// 	Db.Execute("EXEC sp_update_ppn " + myID + ",2,'" + Convert.ToInt32(PPN) + "','" + ppnunit + "'", Db.Cnn_DB);
			// }
			// catch { }

			dpp.Text = DPP.ToString("N0");
			ppn.Text = PPN.ToString("N0");
			nominal.Text = d.TotalBayar.Value.ToString("N0");
			totaldp.Text = (d.SalesOrder.StatusPembayaran == true) ? d.TotalBayar.Value.ToString("N0") : (d.TotalDP.HasValue ? d.TotalDP.Value.ToString("N0") : "0");
			sisa.Text = d.TotalDP.HasValue ? (d.TotalBayar.Value - ((d.SalesOrder.StatusPembayaran == true) ? d.TotalBayar.Value : d.TotalDP.Value)).ToString("N0") : (d.SalesOrder.StatusPembayaran == true) ? "0" : d.TotalBayar.Value.ToString("N0");

			// cetak detail of payment jika hanya 1 item plu
			int TotalItem = 0;
			TotalItem = d.SalesOrder.SalesOrderDJ.Count()
						+ d.SalesOrder.SalesOrderPG.Count()
						+ d.SalesOrder.SalesOrderGJ.Count()
						+ d.SalesOrder.SalesOrderLD.Count()
						;

			totaldp.Text = d.TotalBayar.Value.ToString("N0");
			sisa.Text = 0.ToString("N0");

			if (d.StockProductPG.CharProductItem.NamaKode == "LM")
			{
				pphpanel.Visible = true;

				decimal pphz = Db.SingleDecimal("SELECT TOP 1 CASE WHEN ((SELECT TOP 1 ISNULL(NPWP,'') [NPWP] FROM SalesOrder WHERE ID = " + d.IDForm + ") = '') THEN CAST((ISNULL(TotalBayar,0) / 1.009 * 0.009) AS DECIMAL(36,2)) ELSE CAST((ISNULL(TotalBayar,0) / 1.0045 * 0.0045) AS DECIMAL(36,2)) END [Pph22] FROM SalesOrder WHERE ID = " + d.IDForm, Db.Cnn_DB);

				pph.Text = pphz.ToString("N0");
			}

			//adding firman 280818 user control untuk detail payment dari /UC/DetailPaymentControl.aspx
			if (TotalItem == 1)
			{
				detailPaymentControl.SetDetailPayment("PG", d.SalesOrder.ID, Convert.ToDecimal(d.SalesOrder.Unpaid.Value), Convert.ToDecimal(d.SalesOrder.TotalBayar.Value));
			}

			string Header = "";
			string Item = "";

			string w1 = " style='border:none; vertical-align:top; text-align:left; width:180px;' ";
			string w2 = " style='border:none; vertical-align:top; text-align:left; ' ";
			string w3 = " style='border:none; vertical-align:top; text-align:center; width:60px;' ";
			string w4 = " style='border:none; vertical-align:top; text-align:right; width:120px;' ";
			string w5 = " style='border:none; vertical-align:top; text-align:right; width:120px;' ";
			string w6 = " style='border:none; vertical-align:top; text-align:right; width:180px;' ";

			Header += "<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" class=\"noborder\" style=\"width:1200px; \" >";
			Header += "<tr>";
			Header += "<th " + w1 + ">" + "<em>Code NO</em> / CODE" + "</th>";
			Header += "<th " + w2 + ">" + "<em>ITEM & Description</em> / Nama Barang & Keterangan" + "</th>";
			Header += "<th " + w3 + ">" + "Qty" + "</th>";
			Header += "<th " + w4 + ">" + "" + "</th>";
			Header += "<th " + w5 + ">" + "" + "</th>";
			Header += "<th " + w6 + ">" + "<em>Price</em> / Harga Jual (Rp)" + "</th>";
			Header += "</tr>";
			Header += "</table>";

			string querycollection = "SELECT TOP 1 ISNULL(B.m_koleksi,'') [Collection] FROM StockProductPG A " +
				"LEFT JOIN [DB_PROD_MYAPPS].[dbcmk].dbo.data_karet_cleansing B ON A.KodeBarang = B.m_karet_fix " +
				"WHERE A.ID = " + d.IDProduct;
			string collectionname = Db.SingleString(querycollection, Db.Cnn_DB);

			headerlist.Text = Header;

			Item += "<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" class=\"noborder\" style=\"width:1200px;\" >";

			Item += "<tr>";
			Item += "<td " + w1 + ">" + d.StockProductPG.Nomor + "</td>";
			Item += "<td " + w2 + ">" + Str.CapUpper(d.StockProductPG.CharProductItem.Nama) + (!string.IsNullOrEmpty(collectionname) ? " " + collectionname : "") + " BERAT " + (c != 0m ? c.ToString("N4") : Convert.ToDecimal(d.StockProductPG.GrossWeight).ToString("N4")) + " KADAR " + (d.StockProductPG.CharProductItem.Nama == "LM" ? d.StockProductPG.KadarLogam.Value.ToString("N2") : d.StockProductPG.KadarLogam.Value.ToString("N1")) + "% WARNA " + Str.CapUpper(d.StockProductPG.CharFrameColor.Nama) + "</td>";
			Item += "<td " + w3 + ">" + Str.Number0(1) + "</td>";
			Item += "<td " + w4 + ">" + "&nbsp;" + "</td>";
			Item += "<td " + w5 + ">" + "&nbsp;" + "</td>";
			Item += "<td " + w6 + ">" + Str.Number0(d.TotalBayar.Value) + "</td>";
			Item += "</tr>";

			Item += "</table>";
			itemlist.Text = Item;

			bool isCMKCLUB = false;
			if (d.SalesOrder.CustomerNama.ToUpper() == "CMK CLUB")
			{
				if (!String.IsNullOrEmpty(d.SalesOrder.CustomerReferenceNama))
				{
					if (d.SalesOrder.CustomerReferenceNama.ToUpper() == "CMK CLUB")
					{
						isCMKCLUB = true;
					}
				}
			}

			if (isCMKCLUB == true)
			{ // No Resell No Buyback
				asp.Visible = false;
				asp2.Visible = true;
			}
			else
			{
				string query = "exec sp_get_asp_2024 '2','" + d.StockProductPG.ID + "'";
				DataTable dt = Db.Rs(query, Db.Cnn_DB);

				string kodebrand = d.SalesOrder.DataSales.CompanyBrand.NamaKode;
				string namabrand = d.SalesOrder.DataSales.CompanyBrand.Nama;
				bool isfrankgold = false;
				bool isTinyTAN = false;
				if (d.StockProductPG.ProductLevel == 11)
					isfrankgold = true;
				if (d.StockProductPG.KodeBarang.Contains("TinyTAN"))
				{
					isfrankgold = true;
					isTinyTAN = true;
				}
				WriteASP(dt, kodebrand, namabrand, isfrankgold, isTinyTAN);
			}
		}
	}

	protected void WriteASP(DataTable dt, string kodebrand, string namabrand, bool isfrankgold, bool isTinyTAN)
	{
		int rowscount = 0;
		string BrandText = "";
		if (kodebrand == "F")
			BrandText = "Frank & co";
		else if (kodebrand == "S" || kodebrand == "M")
			BrandText = "Mondial";
		else if (kodebrand == "P")
			BrandText = "The Palace";

		rowscount = dt.Rows.Count;

		if (rowscount == 0)
		{
			asp.Visible = false;
			asp2.Visible = true;
		}
		else
		{
			if (kodebrand == "P")
			{
				if (isfrankgold)
				{
					asp.Visible = false;
					thepalace.Visible = false;
					general.Visible = true;
					asp2.Visible = false;

					lit_brand1.Text = isTinyTAN ? "The Palace" : "Frank & Co";
					lit_brand2.Text = isTinyTAN ? "The Palace" : "Frank & Co";
				}
				else
				{
					asp.Visible = false;
					thepalace.Visible = true;
					general.Visible = false;
					asp2.Visible = false;
				}
			}
			else
			{
				asp.Visible = false;
				thepalace.Visible = false;
				general.Visible = true;
				asp2.Visible = false;
			}
		}
	}
}