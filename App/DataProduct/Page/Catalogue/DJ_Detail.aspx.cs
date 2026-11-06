using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

public partial class Page_Register_DJ_Submit : System.Web.UI.Page
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
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack)
		{

			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("DJ.aspx"); }

	//Form
	protected void fillForm()
	{
		var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myID
				//&& p.ReturnProduct == false
				);
		if (d == null) DataError();
		else
		{

			if (d.Legacy == 1) { legacy_ERR.Visible = true; }

			//Header
			header_nomor.InnerHtml = d.Nomor;

			//Trans. Form
			brand.InnerHtml = (d.LocWarehouse != null ? d.LocWarehouse.CompanyBrand.Nama : "");
			tgl.InnerHtml = Str.Tgl(d.ApprovalTgl);

			//Trans. Data
			designnomor.InnerHtml = d.Nomor + "<br />" + d.ProtoNomor;


			////Product Char.
			var product = d.StockProductDJ_CharProduct;
			if (product == null) DataError();
			else
			{
				proto.InnerHtml = d.ProtoNomor;
				productitem.InnerHtml = product.CharProductItem.Nama + " (" + product.CharProductItem.NamaKode + ")";
				productcategory.InnerHtml = Db.SingleString("SELECT B.Nama + ' (' + B.NamaKode + ')' FROM StockProductDJ_CharProduct A JOIN CharProductCategory2 B ON A.ProductCategory2 = B.ID WHERE A.ID = " + d.ID, Db.Cnn_DB);
				//productcategory.InnerHtml = product.CharProductCategory.Nama + " (" + product.CharProductCategory.NamaKode + ")";
				productlevel.InnerHtml = product.CharProductLevel.Nama + " (" + product.CharProductLevel.NamaKode + ")";
				//stonedist.InnerHtml = product.CharStoneDist.Nama + " (" + product.CharStoneDist.NamaKode + ")";
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
				DataTable dt2 = Db.Rs(
					"SELECT ISNULL(DimensiR2,0) AS DimensiR2 FROM StockProductDJ_CharProduct WHERE ID = " + d.ID,
					Db.Cnn_DB
				);

				decimal val = 0;

				if (dt2 != null && dt2.Rows.Count > 0)
				{
					if (dt2.Rows[0]["DimensiR2"] != DBNull.Value)
					{
						val = Convert.ToDecimal(dt2.Rows[0]["DimensiR2"]);
					}
				}

				dimensir2.InnerHtml = Str.Number2((decimal?)val);

				//NamingKoleksi
				string qry = "SELECT TOP 100 ISNULL(m_kodemat,'') [KodeAmat], ISNULL(m_koleksi,'') [Koleksi], ISNULL(m_naming,'') [Naming] FROM [DB_PROD_MYAPPS].[dbcmk].dbo.data_cleansing_karet_final_old A" +
						" JOIN StockProductDJ B ON A.m_karet_jaws = B.ProtoNomor" +
						" WHERE B.ID = " + d.ID;
				DataTable dt = Db.Rs(qry, Db.Cnn_DB);
				if (dt.Rows.Count == 1)
				{
					kodeamat.InnerHtml = dt.Rows[0]["KodeAmat"].ToString();
					naming2.InnerHtml = dt.Rows[0]["Naming"].ToString();
					koleksi.InnerHtml = dt.Rows[0]["Koleksi"].ToString();
				}
			}

			//DJ Char.
			var design = d.StockProductDJ_CharDesign;
			if (design == null) DataError();
			else
			{
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
			fillStone(d);

			decimal Harga = 0m;
			Harga = d.StockProductDJ_PricingMU.HargaRupiahRound.Value;

			decimal HargaPromo = 0m;

			var rs = from p in db.PromoDJ_Detail
					 where p.PromoDJ.ApprovalNama != null && p.PromoDJ.TglStart.Value <= DateTime.Today
					 && p.PromoDJ.TglExpire.Value >= DateTime.Today && p.IDProduct.Value == d.ID
					 && (p.PromoDJ.StatusActive != false || p.PromoDJ.StatusActive == null)
					 select p;
			if (rs.Count() > 0)
			{
				var disc = rs.First();
				if (disc.PromoDJ.Tipe == 1)
				{
					decimal DiscountHarga = 0m;
					decimal DiscountPersen = 0m;
					decimal Discount = 0m;
					decimal TotalBayar = d.StockProductDJ_PricingMU.HargaRupiahRound.Value;

					DiscountHarga = disc.PromoDJ.DiscountNominal.Value;
					DiscountPersen = (TotalBayar * (disc.PromoDJ.DiscountPersen.Value / 100));
					Discount = (DiscountHarga + DiscountPersen);
					HargaPromo = TotalBayar - Discount;
				}
				if (disc.PromoDJ.Tipe == 3)
				{
					HargaPromo = disc.PromoDJ.PromoHarga.Value;

				}

				//Tambah kondisi Bu Mirah (SK) untuk harga promo lebih besar dari harga Stock
				if (HargaPromo >= Harga)
				{
					Harga = HargaPromo;
					HargaPromo = 0m;
				}
			}
            //Estimasi Harga


            using (SqlConnection connection = new SqlConnection(Db.Cnn_DB))
            {
                string queryCheckTerjual = @"SELECT COUNT('') [Total] FROM StockProductDJ A 
                                            JOIN SalesOrderDJ B ON A.ID = B.IDProduct 
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

                string queryCheckCrossbrand = @"SELECT COUNT('') [Total] FROM CrossBrand_StockOutgoingDJ A
                                                JOIN CrossBrand_StockOutgoingDJ_Product B ON A.ID = B.IDForm
                                                JOIN StockProductDJ C ON B.IDProduct = C.ID
                                                JOIN StockActualDJ D ON C.ID = D.IDProduct
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
                    hargapwp.InnerHtml = "<p class=\"text-lg text-danger\">-</p>";
                    table_harga_promo.Visible = false;
                    table_harga_pwp.Visible = false;
                }
                else
                {
                    if (isCrossBrand > 0)
                    {
                        harga.InnerHtml = "<p class=\"text-lg text-danger\">Barang tidak ditemukan (Cross Brand).</p>";
                        hargapromo.InnerHtml = "<p class=\"text-lg text-danger\">-</p>";
                        hargapwp.InnerHtml = "<p class=\"text-lg text-danger\">-</p>";
                        table_harga_promo.Visible = false;
                        table_harga_pwp.Visible = false;
                    }
                    else
                    {
                        decimal Harga_PWP = GetHargaPWP();
                        harga.InnerHtml = "<p class=\"text-lg text-info\">" + Convert.ToDecimal(Harga).ToString("N0") + "</p>";
                        hargapromo.InnerHtml = "<p class=\"text-lg text-info\">" + ((HargaPromo > 0) ? Convert.ToDecimal(HargaPromo).ToString("N0") : "-") + "</p>";
                        hargapwp.InnerHtml = "<p class=\"text-lg text-info\">" + ((Harga_PWP > 0m) ? Convert.ToDecimal(Harga_PWP).ToString("N0") : "-") + "</p>";
                        table_harga_promo.Visible = true;
                        table_harga_pwp.Visible = true;
                    }
                }
            }

            //Picture
            // if (File.Exists(d.ImgPicture))
			// {
			// 	Upload.Image(imgpicture, d.ImgPicture);
			// }
			// else if (string.IsNullOrEmpty(d.ImgPicture) && File.Exists(d.ImgPicture.Replace(Path.GetFileName(d.ImgPicture), "") + d.ProtoNomor + d.StockProductDJ_CharProduct.CharFrameColor.NamaKode + ".jpg"))
			// {
			// 	string path = d.ImgPicture.Replace(Path.GetFileName(d.ImgPicture), "") + d.ProtoNomor + d.StockProductDJ_CharProduct.CharFrameColor.NamaKode + ".jpg";
			// 	Upload.Image(imgpicture, path);
			// }
			// else
			// {
			// 	string path = d.ImgPicture.Replace(Path.GetFileName(d.ImgPicture), "") + d.ProtoNomor + ".jpg";
			// 	Upload.Image(imgpicture, path);
			// }
			if (!string.IsNullOrEmpty(d.ImgPicture) && File.Exists(d.ImgPicture))
			{
				Upload.Image(imgpicture, d.ImgPicture);
			}
			else
			{
				string basePath = "";

				if (!string.IsNullOrEmpty(d.ImgPicture))
					basePath = d.ImgPicture.Replace(Path.GetFileName(d.ImgPicture), "");

				string colorCode = "";
				if (d.StockProductDJ_CharProduct != null && 
					d.StockProductDJ_CharProduct.CharFrameColor != null)
				{
					colorCode = d.StockProductDJ_CharProduct.CharFrameColor.NamaKode;
				}

				string pathWithColor = basePath + d.ProtoNomor + colorCode + ".jpg";
				string pathDefault = basePath + d.ProtoNomor + ".jpg";

				if (!string.IsNullOrEmpty(colorCode) && File.Exists(pathWithColor))
				{
					Upload.Image(imgpicture, pathWithColor);
				}
				else if (File.Exists(pathDefault))
				{
					Upload.Image(imgpicture, pathDefault);
				}
				else
				{
					Upload.Image(imgpicture, d.ImgPicture); 
				}
			}
		}
	}

	//DJ Info
	protected string designKet(StockProductDJ_CharDesign d)
	{
		string x = string.Empty;

		if (d.SetItem)
		{
			if (x != "") x += ", ";
			x += "Set Item";
		}
		if (d.Special)
		{
			if (x != "") x += ", ";
			x += "Special DJ";
		}

		return x;
	}
	protected string designFinishing(StockProductDJ d)
	{
		string x = string.Empty;

		var rs = from p in d.StockProductDJ_Finishing select p;
		foreach (var r in rs)
		{
			if (x != "") x += "<br />";
			x += r.CharProcessFinishing.Nama + " (" + r.CharProcessFinishing.NamaKode + ")";
		}

		return x;
	}

	//Stone
	protected void fillStone(StockProductDJ d)
	{
		int GT_Butir = 0;
		decimal GT_Carat = 0;


		string plunumber = d.Nomor;
		string stonemodelist = "i"; // b:bandroll / c:certificate
		string plutype = "dj"; // dj or ld

		string spname = "sp_get_stone_information_for_bandroll_certificate";
		string query = " exec " + spname
					 + " '" + stonemodelist + "'"
					 + ",'" + plunumber + "'"
					 + ",'" + plutype + "'"
					 ;

		DataTable dt = new DataTable();
		dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

		if (dt.Rows.Count > 0)
		{
			int TotalButir = 0;
			decimal TotalCarat = 0m;
			decimal CaratButir = 0m;
			//string Keterangan = "";
			string ShapeName = "";
			string Colour = "";
			string Clarity = "";
			string GIA = "";
			string IC = "";
			string StoneBrand = "";

			int ST_Butir = 0;
			decimal ST_Carat = 0;

			string xStoneTypeCursor = "";
			string xStoneType = "";

			//RESULT
			int index = 1;

			foreach (DataRow dtRow in dt.Rows)
			{
				xStoneType = (dtRow["StoneType"]).ToString();
				if (xStoneType != xStoneTypeCursor)
				{
					GT_Butir += ST_Butir;
					GT_Carat += ST_Carat;

					if (index > 1)
					{
						Sub_Total(ST_Butir, ST_Carat, false);
					}

					ST_Butir = 0;
					ST_Carat = 0;

					xStoneTypeCursor = xStoneType;
					if (xStoneTypeCursor == "1A") { Sub_Header(0); }
					if (xStoneTypeCursor == "1B") { Sub_Header(1); }
					if (xStoneTypeCursor == "2") { Sub_Header(2); }
					if (xStoneTypeCursor == "3") { Sub_Header(3); }
					if (xStoneTypeCursor == "4") { Sub_Header(4); }
					if (xStoneTypeCursor == "5") { Sub_Header(5); }
				}

				TotalButir = Convert.ToInt32(dtRow["TotalButir"]);
				TotalCarat = Convert.ToDecimal(dtRow["TotalCarat"]);
				CaratButir = Convert.ToDecimal(dtRow["CaratButir"]);
				ShapeName = (dtRow["ShapeName"]).ToString();
				Colour = (dtRow["Colour"]).ToString();
				Clarity = (dtRow["Clarity"]).ToString();
				GIA = (dtRow["GIA"]).ToString();
				IC = (dtRow["IC"]).ToString();
				StoneBrand = (dtRow["BrandName"]).ToString();

				TableRow tr = new TableRow();
				TableCell td;
				string sarine = string.Empty;
				string linkSarine = string.Empty;
				string grade = string.Empty;
				if (GIA != null)
				{
					try
					{
						string q = "SELECT m_report, m_digital, m_grade FROM table_sarin WHERE m_gia = '" + GIA + "'";
						DataTable dts = Db.Rs(q, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_CS"].ConnectionString);

						if (dts.Rows.Count > 0)
						{
							DataRow row = dts.Rows[0];
							sarine = row["m_report"].ToString();
							linkSarine = row["m_digital"].ToString();
							grade = row["m_grade"].ToString();
						}
					}
					catch
					{
						string q = "SELECT m_report, m_digital, m_grade FROM table_sarin WHERE m_gia = '" + GIA + "'";
						DataTable dts = Db.Rs(q, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_CS"].ConnectionString);

						if (dts.Rows.Count > 0)
						{
							DataRow row = dts.Rows[0];
							sarine = row["m_report"].ToString();
							linkSarine = row["m_digital"].ToString();
							grade = row["m_grade"].ToString();
						}
					}
				}

				td = new TableCell();
				td.Text = "<p>" + index++.ToString() + ".</p>";
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = TD_Nama(ShapeName, ((GIA.Trim() != "") ? GIA.Trim() : ""), StoneBrand, sarine, linkSarine, grade, IC);
				td.Style.Add("text-align", "left");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = Colour;
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = Clarity;
				td.Style.Add("text-align", "center");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = TD_CaratButir(CaratButir);
				td.Style.Add("text-align", "right");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = TD_TotalButir(TotalButir, ref ST_Butir);
				td.Style.Add("text-align", "right");
				tr.Cells.Add(td);

				td = new TableCell();
				td.Text = TD_TotalCarat(TotalCarat, ref ST_Carat);
				td.Style.Add("text-align", "right");
				tr.Cells.Add(td);

				tb.Rows.Add(tr);

				if (dt.Rows.Count == index - 1)
				{
					GT_Butir += ST_Butir;
					GT_Carat += ST_Carat;

					Sub_Total(ST_Butir, ST_Carat, false);
				}


			}


		}

		//Stone
		//fillStone1A(0, ref GT_Butir, ref GT_Carat, d);
		//fillStone1B(1, ref GT_Butir, ref GT_Carat, d);
		//fillStone2(2, ref GT_Butir, ref GT_Carat, d);
		//fillStone3(3, ref GT_Butir, ref GT_Carat, d);
		//fillStone4(4, ref GT_Butir, ref GT_Carat, d);
		//fillStone5(5, ref GT_Butir, ref GT_Carat, d);

		//Grand-Total
		Sub_Total(GT_Butir, GT_Carat, true);
	}

	//Stone
	protected void fillStone1A(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
	{
		//SELECT
		var rs = from p in db.StockProductDJ_Stone1A
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
		foreach (var r in rs)
		{
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			string Parcel103 = "";
			string Parcel104 = "";

			/*if (Regex.Matches(r.Stone1A.Nama, "A4").Count > 0 || Regex.Matches(r.Stone1A.Nama, "S4").Count > 0)
            {
                Parcel103 = "F";
                Parcel104 = "VS";
            }
            else
            {
                Parcel103 = "F";
                Parcel104 = "VVS";
            }*/

			Parcel103 = "F";
			if (r.Stone1A.Parcel104.NamaKode == "23" || r.Stone1A.Parcel104.NamaKode == "05")
				Parcel104 = "VS";
			else
				Parcel104 = "VVS";

			td = new TableCell();
			td.Text = r.Stone1A.Parcel101.Nama;
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Parcel103;
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = Parcel104;
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
		if (rs.Count() > 0)
		{
			GT_Butir += ST_Butir;
			GT_Carat += ST_Carat;

			Sub_Total(ST_Butir, ST_Carat, false);
		}
	}
	protected void fillStone1B(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
	{
		//SELECT
		var rs = from p in db.StockProductDJ_Stone1B
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
		foreach (var r in rs)
		{
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nama(r.Stone1B.Parcel101.Nama, (r.GIA != null ? r.GIA : ""), Convert.ToInt32(r.StatusTerpasang));
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone1B.Parcel103.Nama;
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone1B.Parcel104.Nama;
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
		if (rs.Count() > 0)
		{
			GT_Butir += ST_Butir;
			GT_Carat += ST_Carat;

			Sub_Total(ST_Butir, ST_Carat, false);
		}
	}
	protected void fillStone2(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
	{
		//SELECT
		var rs = from p in db.StockProductDJ_Stone2
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
		foreach (var r in rs)
		{
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nama(r.Stone2.Parcel201.Nama, (r.GIA != null ? r.GIA : ""));
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone2.Parcel203.Nama;
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone2.Parcel204.Nama;
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
		if (rs.Count() > 0)
		{
			GT_Butir += ST_Butir;
			GT_Carat += ST_Carat;

			Sub_Total(ST_Butir, ST_Carat, false);
		}
	}
	protected void fillStone3(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
	{
		//SELECT
		var rs = from p in db.StockProductDJ_Stone3
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
		foreach (var r in rs)
		{
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nama(r.Stone3.Parcel301.Nama, (r.GIA != null ? r.GIA : ""));
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone3.Parcel303.Nama;
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone3.Parcel304.Nama;
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
		if (rs.Count() > 0)
		{
			GT_Butir += ST_Butir;
			GT_Carat += ST_Carat;

			Sub_Total(ST_Butir, ST_Carat, false);
		}
	}
	protected void fillStone4(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
	{
		//SELECT
		var rs = from p in db.StockProductDJ_Stone4
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
		foreach (var r in rs)
		{
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);



			td = new TableCell();
			td.Text = TD_Dimensi(r.DimensiP);
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nama(r.Stone4.Parcel401.Nama, (r.GIA != null ? r.GIA : ""));
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone4.Parcel403.Nama;
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone4.Parcel403.Nama;
			td.CssClass = "text-center";
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
		if (rs.Count() > 0)
		{
			GT_Butir += ST_Butir;
			GT_Carat += ST_Carat;

			Sub_Total(ST_Butir, ST_Carat, false);
		}
	}
	protected void fillStone5(int StoneIndex, ref int GT_Butir, ref decimal GT_Carat)
	{
		//SELECT
		var rs = from p in db.StockProductDJ_Stone5
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
		foreach (var r in rs)
		{
			if (!Response.IsClientConnected) break;

			TableRow tr = new TableRow();
			TableCell td;

			td = new TableCell();
			td.Text = "<p>" + index++.ToString() + ".</p>";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = TD_Nama(r.Stone5.Parcel501.Nama, (r.GIA != null ? r.GIA : ""));
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone5.Parcel503.Nama;
			td.CssClass = "text-center";
			tr.Cells.Add(td);

			td = new TableCell();
			td.Text = r.Stone5.Parcel504.Nama;
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
		if (rs.Count() > 0)
		{
			GT_Butir += ST_Butir;
			GT_Carat += ST_Carat;

			Sub_Total(ST_Butir, ST_Carat, false);
		}
	}

	//Custom Row
	protected void Sub_Header(int StoneIndex)
	{
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
	protected void Sub_Total(int TotalButir, decimal TotalCarat, bool GrandTotal)
	{
		TableRow tr = new TableRow();
		TableCell td;

		tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

		td = new TableCell();
		td.ColumnSpan = 5;
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
	protected string TD_Nama(string Nama)
	{
		string x = string.Empty;

		x += "<p><strong>" + Nama + "</strong></p>";

		return x;
	}
	protected string TD_Nama(string Nama, string GIA, string BrandStone = "", string sarine = "", string linkSarine = "", string grade = "", string IC = "")
	{
		string x = string.Empty;
        string UrlGIA = ConfigurationManager.AppSettings["UrlGIA"];
        UrlGIA = UrlGIA + "locale=en_US&reportno=";

        x += "<p><strong>" + Nama + "</strong></p>";

		if (BrandStone != null && BrandStone.Trim() != "")
			x += "<p class=\"text-sm\" style=\"color:purple; font-weight:bold;\">" + BrandStone + "</p>";

		if (GIA != null && GIA.Trim() != "")
		{
			string ICTrim = IC.Contains(" ") ? IC.Split(' ')[1] : IC;
			x += "<p class=\"text-muted text-sm\">IC : <a target=\"_blank\" class=\"text-muted text-sm\" href=\"" + UrlGIA + ICTrim.Replace("IC", "") + "\">" + IC + "</a></p>";
			x += "<p class=\"text-muted text-sm\">GIA : <a target=\"_blank\" class=\"text-muted text-sm\" href=\"" + UrlGIA + GIA.Replace("GIA", "") + "\">" + GIA + "</a></p>";
		}

		if (sarine != null && sarine.Trim() != "")
			x += "<p class=\"text-muted text-sm\">Sarine : <a target=\"_blank\" class=\"text-muted text-sm\" href=\"" + linkSarine + "\">" + sarine + "</a></p>";

		if (grade != null && grade.Trim() != "")
			x += "<p class=\"text-muted text-sm\">Grade : <label>" + grade + "</label></p>";
		return x;
	}

	protected string TD_Nama(string Nama, string GIA, int Status)
	{
		string x = string.Empty;
        string UrlGIA = ConfigurationManager.AppSettings["UrlGIA"];
        UrlGIA = UrlGIA + "locale=en_US&reportno=";

        x += "<p><strong>" + Nama + "</strong></p>";
		if (GIA != null && GIA.Trim() != "")
			x += "<p class=\"text-muted text-sm\"> <a target=\"_blank\" class=\"text-muted text-sm\" href=\"" + UrlGIA + GIA.Replace("GIA", "") + "\">" + GIA + "</a></p>";
		if (Status == 0)
			x += "<p class=\"text-muted text-sm\">Batu belum terpasang.</p>";

		return x;
	}
	protected string TD_Nama(string Nama, string GIA)
	{
		string x = string.Empty;
        string UrlGIA = ConfigurationManager.AppSettings["UrlGIA"];
        UrlGIA = UrlGIA + "locale=en_US&reportno=";

        x += "<p><strong>" + Nama + "</strong></p>";
		if (GIA != null && GIA.Trim() != "")
			x += "<p class=\"text-muted text-sm\"> <a target=\"_blank\" class=\"text-muted text-sm\" href=\"" + UrlGIA + GIA.Replace("GIA", "") + "\">" + GIA + "</a></p>";

		return x;
	}
	protected string TD_Dimensi(decimal? Dimensi)
	{
		string x = string.Empty;

		x += "<p>" + Str.Number2(Dimensi) + "</p>";

		return x;
	}
	protected string TD_CaratButir(decimal? CaratButir)
	{
		string x = string.Empty;

		x += "<p class=\"text-muted\">" + Str.Number3(CaratButir) + "</p>";

		return x;
	}
	protected string TD_TotalButir(int? TotalButir, ref int ST)
	{
		string x = string.Empty;

		x += "<p class=\"text-lg text-success\">" + Str.Integer(TotalButir) + "</p>";

		//Sub-Total
		if (TotalButir.HasValue) ST += TotalButir.Value;

		return x;
	}
	protected string TD_TotalCarat(decimal? TotalCarat, ref decimal ST)
	{
		string x = string.Empty;

		x += "<p class=\"text-lg text-info\">" + Str.Number3(TotalCarat) + "</p>";

		//Sub-Total
		if (TotalCarat.HasValue) ST += TotalCarat.Value;

		return x;
	}

    protected decimal GetHargaPWP()
    {
        string query = "DECLARE @HARGA_PWP MONEY; " +
            "SELECT TOP (1) @HARGA_PWP = B.HargaPWP " +
            "FROM PromoPWP A " +
            "INNER JOIN PromoPWP_Detail B ON A.ID = B.IDForm " +
            "WHERE A.TglStart <= CAST(GETDATE() AS DATE)" +
            "AND A.TglExpired >= CAST(GETDATE() AS DATE) " +
            "AND B.IDProduct = @IDPRODUCT; " +
            "SELECT COALESCE(@HARGA_PWP, 0) AS HARGA_PWP;";
        List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@IDPRODUCT", SqlDbType.Int){ Value = myID }
        };
        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.Text);
        return Convert.ToDecimal(dt.Rows[0]["HARGA_PWP"]);
    }

    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }
}