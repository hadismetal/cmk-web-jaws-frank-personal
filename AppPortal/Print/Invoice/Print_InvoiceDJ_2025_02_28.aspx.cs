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
		string xtest = "";

		var d = db.SalesOrderDJ.SingleOrDefault(p => p.ID == myID);
		if (d == null)
		{
			Response.Redirect("Sales.aspx");
		}
		else
		{
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
				if (Cabang.Contains("BLIBLI"))
				{
					onlinePolicy.Visible = true;
				}

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

			decimal xtotal = 0m;
			decimal xbayar = 0m;
			decimal xsisa = 0m;

			decimal LakuEmasNominal = 0m;
			var py = from p in db.SalesReceiptDetail
					 where p.SalesReceipt.IDSalesOrder == d.IDForm
					 orderby p.OperatorTgl descending
					 select p;

			lit_ppn.Text = "2,2 %";
			lit_ppn2.Text = "2,2 %";
			lblppn.InnerText = "11%";
			decimal rateppn = 1.022m;
			int ppnunit = 11;
			if (d.SalesOrder.Tgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
			{
				lit_ppn.Text = "2 %";
				lit_ppn2.Text = "2 %";
				lblppn.InnerText = "10%";
				rateppn = 1.02m;
				ppnunit = 10;
			}
			else if (d.SalesOrder.Tgl <= Convert.ToDateTime("2023-04-30 23:59:00.000"))
			{
				lit_ppn.Text = "2,2 %";
				lit_ppn2.Text = "2,2 %";
				lblppn.InnerText = "11%";
				lblppn2.InnerText = "20%";
				rateppn = 1.022m;
				ppnunit = 11;
			}
			else if (d.SalesOrder.Tgl <= Convert.ToDateTime("2024-12-30 23:59:00.000"))
			{
				if ((Db.SingleString("select ISNULL(b.Keterangan, '-') from StockProductDJ c left join DataSupplier a on c.Supplier = a.ID left join DataSupplierGroup b on a.IDGroup = b.ID where c.ID = " + d.StockProductDJ.ID, Db.Cnn_DB).Contains("In") || Db.SingleString("select ISNULL(b.Keterangan, '-') from StockProductDJ c left join DataSupplier a  on c.Supplier = a.ID left join DataSupplierGroup b on a.IDGroup = b.ID where c.ID = " + d.StockProductDJ.ID, Db.Cnn_DB).Equals("-")))
				{
					lit_ppn.Text = "1,65 %";
					lit_ppn2.Text = "1,65 %";
					lblppn.InnerText = "11%";
					lblppn2.InnerText = "15%";
					rateppn = 1.0165m;
					ppnunit = 11;
				}
				else if (Db.SingleString("select b.Keterangan from DataSupplier a join DataSupplierGroup b on a.IDGroup = b.ID where a.ID = " + d.StockProductDJ.Supplier, Db.Cnn_DB).Contains("Out"))
				{
					lit_ppn.Text = "1,1 %";
					lit_ppn2.Text = "1,1 %";
					lblppn.InnerText = "11%";
					lblppn2.InnerText = "10%";
					rateppn = 1.011m;
					ppnunit = 11;
				}
				else
				{
					lit_ppn.Text = "1,65 %";
					lit_ppn2.Text = "1,65 %";
					lblppn.InnerText = "11%";
					lblppn2.InnerText = "15%";
					rateppn = 1.0165m;
					ppnunit = 11;
				}
			}
			else
			{
				string getKeterangan = @"select CAST(ax.PPNPercentage AS Numeric(9,4)) PPNPercentage,CAST(ax.PPNPembagi AS Numeric(9,4)) PPNPembagi,IIF(Source = 'Insource','15%','10%') AS [PPN2]
										from (
											SELECT TOP 1 a.*,IIF(LEFT(d.Keterangan,3) = 'Out','Outsource','Insource') Source
											FROM SalesOrderDJ A 
											JOIN StockProductDJ B ON A.IDProduct = B.ID 
											LEFT JOIN DataSupplier C ON B.Supplier = C.ID 
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
								lblppn.InnerText = Convert.ToString(reader.GetDecimal(reader.GetOrdinal("PPNPercentage"))) + " %";
								lblppn2.InnerText = Convert.ToString(reader["PPN2"]);
							}
						}
						conn.Close();
					}
				}
				
			}

			if (!IsUsePPN || d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama == "LM")
			{
				lit_ppn.Text = "0 %";
				lit_ppn2.Text = "0 %";
				ppnunit = 0;

				divuseppn.Visible = false;
				lit_ppn_notice.Visible = false;

				lblppn10.InnerHtml = "<em>Price</em> / Harga Jual ";
				lbl_ppn_notice.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
			}
			else
			{
				divuseppn.Visible = true;
				lit_ppn_notice.Visible = true;

				lblppn10.InnerHtml = "<em>Price Exclude VAT</em> / Harga Jual Tidak Termasuk PPN ";
				lbl_ppn_notice.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
			}

			DPP = (!IsUsePPN) ? (d.TotalBayar.Value - LakuEmasNominal) : (d.TotalBayar.Value - LakuEmasNominal) / rateppn;
			PPN = d.TotalBayar.Value - DPP - LakuEmasNominal;
			xtotal = d.TotalBayar.Value;
			xbayar = (d.SalesOrder.StatusPembayaran == true) ? d.TotalBayar.Value : (d.TotalDP.HasValue ? d.TotalDP.Value : 0m);
			//xsisa = d.TotalDP.HasValue ? (d.TotalBayar.Value - ((d.SalesOrder.StatusPembayaran == true) ? d.TotalBayar.Value : d.TotalDP.Value)) : d.TotalBayar.Value;
			xsisa = (xtotal - xbayar) > 0m ? (xtotal - xbayar) : 0m;

			// try
			// {
			// 	Db.Execute("EXEC sp_update_ppn " + myID + ",1,'" + Convert.ToInt32(PPN) + "','" + ppnunit + "'", Db.Cnn_DB);
			// }
			// catch { }

			dpp.Text = DPP.ToString("N0");
			ppn.Text = PPN.ToString("N0");
			nominal.Text = xtotal.ToString("N0");
			totaldp.Text = xbayar.ToString("N0");
			sisa.Text = xsisa.ToString("N0");

			// cetak detail of payment jika hanya 1 item plu
			int TotalItem = 0;
			TotalItem = d.SalesOrder.SalesOrderDJ.Count()
						+ d.SalesOrder.SalesOrderPG.Count()
						+ d.SalesOrder.SalesOrderGJ.Count()
						+ d.SalesOrder.SalesOrderLD.Count()
						;

			totaldp.Text = d.TotalBayar.Value.ToString("N0");
			sisa.Text = 0.ToString("N0");

			//adding firman 280818 user control untuk detail payment dari /UC/DetailPaymentControl.aspx
			if (TotalItem == 1)
			{
				detailPaymentControl.SetDetailPayment("DJ", d.SalesOrder.ID, Convert.ToDecimal(d.SalesOrder.Unpaid.Value), Convert.ToDecimal(d.SalesOrder.TotalBayar.Value));
				// detailPaymentControl.SetDetailPayment(myID);
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

			headerlist.Text = Header;

			Item += "<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" class=\"noborder\" style=\"width:1200px;\" >";

			Item += "<tr>";
			Item += "<td " + w1 + ">" + d.StockProductDJ.Nomor + "</td>";
			Item += "<td " + w2 + ">" + Str.CapUpper(d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama) + "</td>";
			Item += "<td " + w3 + ">" + Str.Number0(1) + "</td>";
			Item += "<td " + w4 + ">" + "&nbsp;" + "</td>";
			Item += "<td " + w5 + ">" + "&nbsp;" + "</td>";
			Item += "<td " + w6 + ">" + Str.Number0(d.TotalBayar.Value) + "</td>";
			Item += "</tr>";

			Item += "</table>";
			itemlist.Text = Item;

			bool isCMKCLUB = false;
			bool isWeddingRing = false;
			bool isPlatinum = false;
			bool isMounting = false;
			bool isDossier = false;
			string namaitem = d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama.ToLower();
			string namamaterial = d.StockProductDJ.StockProductDJ_CharProduct.CharFrameMaterial.Nama.ToLower();
			string level = d.StockProductDJ.StockProductDJ_CharProduct.CharProductLevel.NamaKode.ToLower();
			if (namaitem.Contains("wedding ring") || namaitem.Contains("aureola") || namaitem.Contains("aretino"))
			{
				isWeddingRing = true;
				if (namamaterial.Contains("platinum"))
				{
					isPlatinum = true;
				}
			}
			if (level.Contains("mtg"))
			{
				isMounting = true;
			}
			if (d.StockProductDJ.StockProductDJ_Stone1B.Any())
			{
				isDossier = true;
			}
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
				string query = "exec sp_get_asp_2024 '" + 1 + "','" + d.StockProductDJ.ID + "'";
				DataTable dt = Db.Rs(query, Db.Cnn_DB);

				string kodebrand = d.SalesOrder.DataSales.CompanyBrand.NamaKode;
				string namabrand = d.SalesOrder.DataSales.CompanyBrand.Nama;
				bool istinytan = false; //aspbts
				int counttinytan = Db.SingleInteger("SELECT COUNT('') FROM [DB_PROD_MYAPPS].[dbcmk].dbo.mstinytan A " +
									"JOIN StockProductDJ B ON A.m_karet = B.ProtoNomor " +
									"WHERE B.ID = " + d.StockProductDJ.ID, Db.Cnn_DB);
				if (counttinytan > 0)
					istinytan = true;
				WriteASP(dt, kodebrand, namabrand, istinytan, Convert.ToDateTime(d.SalesOrder.Tgl), isWeddingRing, isPlatinum, isMounting, isDossier);
			}

			/*InvoicePDF a = new InvoicePDF();
            string FilePath = a.writeInvoice(d.IDForm, 1, d.IDProduct, App.UserID.ToString(), Server.MapPath("~/App_Data"));
            System.Net.WebClient User = new System.Net.WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath);
            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.BinaryWrite(FileBuffer);
            }*/
		}
	}

	protected string asp_text_period(string mode, int val, string time)
	{
		string x = "";
		string xtime_eng = "";
		string xtime_ina = "";

		if (val > 0)
		{
			if (time == "d")
			{
				xtime_eng = val > 1 ? "Days" : "Day";
				xtime_ina = "Hari";
			}
			else if (time == "m")
			{
				xtime_eng = val > 1 ? "Months" : "Month";
				xtime_ina = "Bulan";
			}
			else if (time == "y")
			{
				xtime_eng = val > 1 ? "Years" : "Year";
				xtime_ina = "Tahun";
			}

			x = " " + mode + " " + val.ToString() + " <em>" + xtime_eng + "</em> - " + val.ToString() + " " + xtime_ina;
		}


		return x;
	}

	protected string asp_text_value(string sbu, string mode, decimal val, bool buyingprice = true)
	{
		string x = "";

		int mode2 = 1;

		if (mode == "me") { mode2 = 1; }
		else if (val == 100m && buyingprice && (mode == "ti" || mode == "rs")) { mode2 = 2; }
		else if (val == 0m) { mode2 = 3; }

		if (mode2 == 1)
		{
			x = val.ToString() + " % <br /> <em>Original sales amount</em> <br /> Nilai Penjualan Asal";
		}
		else if (mode2 == 2)
		{
			x = sbu + " <em>buying price</em> <br /> Harga beli " + sbu;
		}
		else if (mode2 == 3)
		{
			if (mode == "ti")
			{
				x = "<em>No Trade In</em> <br /> Tidak berlaku Tukar Tambah ";
			}
			else if (mode == "rs")
			{
				x = "<em>No Option to Resell</em> <br /> Tidak ada Opsi Penjualan Kembali ";
			}
		}

		return x;
	}

	protected void WriteASP(DataTable dt, string kodebrand, string namabrand, bool istinytan, DateTime tgl, bool isweddingring, bool isplatinum, bool ismounting, bool isdossier)
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
			aspbts.Visible = false;
		}
		else if (rowscount == 1 && (dt.Rows[0][0].ToString() == "PG FIXED"))
		{
			asp.Visible = false;
			asp2.Visible = false;
			aspbts.Visible = false;
			general.Visible = true;
		}
		/*else if (ismounting && isweddingring && !isplatinum)
		{
			
		}*/
		else if (isweddingring)
		{
			asp.Visible = false;
			aspbts.Visible = false;
			if (isplatinum)
			{
				asp2.Visible = true;
				aspweddingring.Visible = false;
			}
			else
			{
				if (ismounting)
				{
					if (isdossier)
					{
						asp2.Visible = false;
						aspweddingring.Visible = true;
					}
					else
					{
						asp.Visible = false;
						asp2.Visible = false;
						aspbts.Visible = false;
						general.Visible = true;
					}
				}
				else
				{
					asp2.Visible = false;
					aspweddingring.Visible = true;
				}
			}
		}
		else if (rowscount == 1 && (dt.Rows[0][0].ToString() != ""))
		{
			string namaasp = dt.Rows[0]["Nama"].ToString();
			if (istinytan == false)
			{
				if (namaasp != "DOS BRANDED")
				{
					asp.Visible = true;
					aspdossier.Visible = false;
					asp2.Visible = false;
					aspbts.Visible = false;
					// ASP Normal
					string idasp = dt.Rows[0][0].ToString();
					int measpperiodmax = Convert.ToInt32(dt.Rows[0][4]);
					int measprumus = Convert.ToInt32(dt.Rows[0][5]);

					int ti1aspperiodmax = Convert.ToInt32(dt.Rows[0][7]);
					int ti1asprumus = Convert.ToInt32(dt.Rows[0][8]);

					int ti2aspperiodmax = Convert.ToInt32(dt.Rows[0][10]);
					int ti2asprumus = Convert.ToInt32(dt.Rows[0][11]);

					int ti3aspperiodmax = Convert.ToInt32(dt.Rows[0][10]);
					int ti3asprumus = Convert.ToInt32(dt.Rows[0][14]);

					int r1aspperiodmax = Convert.ToInt32(dt.Rows[0][16]);
					int r1asprumus = Convert.ToInt32(dt.Rows[0][17]);

					int r2aspperiodmax = Convert.ToInt32(dt.Rows[0][16]);
					int r2asprumus = Convert.ToInt32(dt.Rows[0][20]);

					int memchasprumus = Convert.ToInt32(dt.Rows[0][21]);

					if (memchasprumus != 0)
					{
						me.Text = asp_text_period("<", memchasprumus, "d");
					}
					else
					{
						me.Text = asp_text_period("<", measpperiodmax, "m");
					}

					if (ti1aspperiodmax < 12)
						ti1.Text = asp_text_period("<", ti1aspperiodmax, "m");
					else
						ti1.Text = asp_text_period("<", ti1aspperiodmax / 12, "y");
					ti2.Text = asp_text_period("<", ti2aspperiodmax / 12, "y");
					ti3.Text = asp_text_period(">", ti3aspperiodmax / 12, "y");
					rs1.Text = asp_text_period("<", r1aspperiodmax / 12, "y");
					rs2.Text = asp_text_period(">", r2aspperiodmax / 12, "y");

					me_rumus1.Text = asp_text_value(BrandText, "me", measprumus);
					if (ti1asprumus == 100)
						ti_rumus1.Text = asp_text_value(BrandText, "ti", ti1asprumus, true);
					else
						ti_rumus1.Text = asp_text_value(BrandText, "ti", ti1asprumus, false);

					if (ti2asprumus == 100)
						ti_rumus2.Text = asp_text_value(BrandText, "ti", ti2asprumus, true);
					else
						ti_rumus2.Text = asp_text_value(BrandText, "ti", ti2asprumus, false);

					ti_rumus3.Text = asp_text_value(BrandText, "ti", ti3asprumus, true);

					if (r1asprumus == 100)
						rs_rumus1.Text = asp_text_value(BrandText, "rs", r1asprumus, true);
					else
						rs_rumus1.Text = asp_text_value(BrandText, "rs", r1asprumus, false);

					rs_rumus2.Text = asp_text_value(BrandText, "rs", r2asprumus, true);
				}
				else
				{
					asp.Visible = false;
					aspdossier.Visible = true;
					asp2.Visible = false;
					aspbts.Visible = false;
					// ASP Normal
					string idasp = dt.Rows[0][0].ToString();
					int ti0aspperiodmax = Convert.ToInt32(dt.Rows[0][4]);
					int ti0asprumus = Convert.ToInt32(dt.Rows[0][5]);

					int ti1aspperiodmax = Convert.ToInt32(dt.Rows[0][7]);
					int ti1asprumus = Convert.ToInt32(dt.Rows[0][8]);

					int ti2aspperiodmax = Convert.ToInt32(dt.Rows[0][10]);
					int ti2asprumus = Convert.ToInt32(dt.Rows[0][11]);

					int ti3aspperiodmax = Convert.ToInt32(dt.Rows[0][10]);
					int ti3asprumus = Convert.ToInt32(dt.Rows[0][14]);

					int r1aspperiodmax = Convert.ToInt32(dt.Rows[0][16]);
					int r1asprumus = Convert.ToInt32(dt.Rows[0][17]);

					int r2aspperiodmax = Convert.ToInt32(dt.Rows[0][16]);
					int r2asprumus = Convert.ToInt32(dt.Rows[0][20]);

					int ti0mchasprumus = Convert.ToInt32(dt.Rows[0][21]);

					medoss.Text = asp_text_period("<", 1, "m");

					tidoss01.Text = asp_text_period("<", 3, "m");

					if (ti0mchasprumus != 0)
					{
						tidoss0.Text = asp_text_period("<", ti0mchasprumus, "d");
					}
					else
					{
						tidoss0.Text = asp_text_period("<", ti0aspperiodmax, "m");
					}

					if (ti1aspperiodmax < 12)
						tidoss1.Text = asp_text_period("<", ti1aspperiodmax, "m");
					else
						tidoss1.Text = asp_text_period("<", ti1aspperiodmax / 12, "y");
					tidoss2.Text = asp_text_period("<", ti2aspperiodmax / 12, "y");
					tidoss3.Text = asp_text_period(">", ti3aspperiodmax / 12, "y");
					rsdoss1.Text = asp_text_period("<", r1aspperiodmax / 12, "y");
					rsdoss2.Text = asp_text_period(">", r2aspperiodmax / 12, "y");

					medoss_rumus.Text = asp_text_value(BrandText, "m", 100);
					tidoss_rumus01.Text = asp_text_value(BrandText, "ti", 100, false);

					tidoss_rumus0.Text = asp_text_value(BrandText, "m", 95);
					if (ti1asprumus == 100)
						tidoss_rumus1.Text = asp_text_value(BrandText, "ti", 90, true);
					else
						tidoss_rumus1.Text = asp_text_value(BrandText, "ti", 90, false);

					if (ti2asprumus == 100)
						tidoss_rumus2.Text = asp_text_value(BrandText, "ti", ti2asprumus, true);
					else
						tidoss_rumus2.Text = asp_text_value(BrandText, "ti", ti2asprumus, false);

					tidoss_rumus3.Text = asp_text_value(BrandText, "ti", ti3asprumus, true);

					if (r1asprumus == 100)
						rsdoss_rumus1.Text = asp_text_value(BrandText, "rs", r1asprumus, true);
					else
						rsdoss_rumus1.Text = asp_text_value(BrandText, "rs", r1asprumus, false);

					rsdoss_rumus2.Text = asp_text_value(BrandText, "rs", r2asprumus, true);
				}
			}
			else
			{
				asp.Visible = false;
				asp2.Visible = false;
				aspbts.Visible = true;

				// ASP Normal
				string idasp = dt.Rows[0][0].ToString();
				int measpperiodmax = Convert.ToInt32(dt.Rows[0][4]);
				int measprumus = Convert.ToInt32(dt.Rows[0][5]);

				int ti1aspperiodmax = Convert.ToInt32(dt.Rows[0][7]);
				int ti1asprumus = Convert.ToInt32(dt.Rows[0][8]);

				int ti2aspperiodmax = Convert.ToInt32(dt.Rows[0][10]);
				int ti2asprumus = Convert.ToInt32(dt.Rows[0][11]);

				int ti3aspperiodmax = Convert.ToInt32(dt.Rows[0][10]);
				int ti3asprumus = Convert.ToInt32(dt.Rows[0][14]);

				int memchasprumus = Convert.ToInt32(dt.Rows[0][21]);

				if (memchasprumus != 0)
				{
					mebts.Text = asp_text_period("<", memchasprumus, "d");
				}
				else
				{
					mebts.Text = asp_text_period("<", measpperiodmax, "m");
				}

				if (ti1aspperiodmax < 12)
					tibts1.Text = asp_text_period("<", ti1aspperiodmax, "m");
				else
					tibts1.Text = asp_text_period("<", ti1aspperiodmax / 12, "y");
				tibts2.Text = asp_text_period("<", ti2aspperiodmax / 12, "y");
				tibts3.Text = asp_text_period(">", ti3aspperiodmax / 12, "y");

				mebts_rumus1.Text = asp_text_value(BrandText, "me", measprumus);
				if (ti1asprumus == 100)
					tibts_rumus1.Text = asp_text_value(BrandText, "ti", ti1asprumus, true);
				else
					tibts_rumus1.Text = asp_text_value(BrandText, "ti", ti1asprumus, false);

				if (ti2asprumus == 100)
					tibts_rumus2.Text = asp_text_value(BrandText, "ti", ti2asprumus, true);
				else
					tibts_rumus2.Text = asp_text_value(BrandText, "ti", ti2asprumus, false);

				tibts_rumus3.Text = asp_text_value(BrandText, "ti", ti3asprumus, true);

				Literalbts1.Text = BrandText;
				Literalbts2.Text = BrandText;
			}
		}
		else
		{
			asp.Visible = false;
			asp2.Visible = true;
			aspbts.Visible = false;
		}
	}
}