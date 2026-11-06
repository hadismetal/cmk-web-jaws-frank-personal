using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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
            
            if (d.SalesOrder.TipeLokasi == 1)
            {
                var x = db.LocOutlet.SingleOrDefault(p => p.ID == d.SalesOrder.IDLokasi);

                BrandText = x.CompanyBrand.Nama;
                Cabang = x.Nama;
                AlamatCabang = x.AddrAlamat;
                KodeLokasi = x.Kode;
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
                AlamatCabang = x.AddrAlamat;
                KodeLokasi = x.Kode;
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

            customer.Text = Str.CapUpper(d.SalesOrder.DataCustomer.Nama);
            nocustomer.Text = Str.CapUpper(d.SalesOrder.DataCustomer.NoCustomer);
            customer2.Text = Str.CapUpper(d.SalesOrder.DataCustomer.Nama);
			customerother.Text = Str.CapUpper(String.IsNullOrEmpty(d.SalesOrder.CustomerReferenceNama) ? d.SalesOrder.DataCustomer.Nama : d.SalesOrder.CustomerReferenceNama);
            adminsales.Text = Str.CapUpper(d.SalesOrder.SalesNama);

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
            foreach (var i in py)
            {
                if(i.IDPaymentType == 11)
                {
                    LakuEmasNominal = LakuEmasNominal + Convert.ToDecimal(i.Nominal);
                }
            }
			if (py.Count(p => p.IDPaymentType == 11) > 0)
			{
                le.Text = LakuEmasNominal.ToString("N0");
				paymentwithlakuemaspanel.Visible = true;
			}
			else
			{
				le.Text = (0m).ToString("N0");
				paymentwithlakuemaspanel.Visible = false;
			}
			
			decimal rateppn = 1.022m;
			int ppnunit = 11;
			lblppn.InnerText = "11%";
			if(d.SalesOrder.Tgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
			{
				rateppn = 1.02m;
				ppnunit = 10;
				lblppn.InnerText = "10%";
			}
	
			if(KodeLokasi == "E46" || KodeLokasi == "P18" || d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama == "LM")
			{
				ppnunit = 0;
			}
				
            DPP = (KodeLokasi == "E46" || KodeLokasi == "P18" || d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama == "LM") ? (d.TotalBayar.Value - LakuEmasNominal) : (d.TotalBayar.Value - LakuEmasNominal) / rateppn;
            PPN = d.TotalBayar.Value - DPP - LakuEmasNominal;
			
			try
			{
				Db.Execute("EXEC sp_update_ppn " + myID + ",1,'" + Convert.ToInt32(PPN) + "','" + ppnunit + "'" ,Db.Cnn_DB);
			}
			catch{}
			
            xtotal = d.TotalBayar.Value;
            xbayar = (d.SalesOrder.StatusPembayaran == true) ? d.TotalBayar.Value : (d.TotalDP.HasValue ? d.TotalDP.Value : 0m);
            //xsisa = d.TotalDP.HasValue ? (d.TotalBayar.Value - ((d.SalesOrder.StatusPembayaran == true) ? d.TotalBayar.Value : d.TotalDP.Value)) : d.TotalBayar.Value;
            xsisa = (xtotal - xbayar) > 0m ? (xtotal - xbayar) : 0m;

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
            if (TotalItem == 1 )
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
			if(d.SalesOrder.CustomerNama.ToUpper() == "CMK CLUB")
			{
				if(!String.IsNullOrEmpty(d.SalesOrder.CustomerReferenceNama))
				{
					if(d.SalesOrder.CustomerReferenceNama.ToUpper() == "CMK CLUB")
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
			else if(d.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.NamaKode == "M")
			{// No Resell No Buyback
				asp.Visible = false;
				asp2.Visible = true;
			}
			else
			{
				int category1 = d.StockProductDJ.StockProductDJ_CharProduct.ProductCategory;
				string categoryname1 = d.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode;
				int stonedist1 = d.StockProductDJ.StockProductDJ_CharProduct.StoneDist;
				int proccons1 = d.StockProductDJ.StockProductDJ_CharProduct.ProcessCons;
				int item1 = d.StockProductDJ.StockProductDJ_CharProduct.ProductItem;
				
				string[] ASP = new string[20];
				if (stonedist1 == 31 && proccons1 == 13)
				{
					ASP = ASPProductIntro();
				}
				else if (d.StockProductDJ.StockProductDJ_CharProduct.CharProductLevel.NamaKode == "MTG" && d.StockProductDJ.StockProductDJ_Stone1B.Any(p => !string.IsNullOrEmpty(p.GIA))) 
                {
                    ASP = ASPProductDossierMounting();
                }
				else if(CheckDossierGenericSMG(d))
				{
					/*if(category1 == 8 || category1 == 16)
					{
						decimal carat = d.StockProductDJ.StockProductDJ_Stone3.Sum(p => p.TotalCarat); 
						bool gia = d.StockProductDJ.StockProductDJ_Stone3.Any(p => !string.IsNullOrEmpty(p.GIA));
						decimal bayar = d.TotalBayar ?? 0m;
						ASP = ASPProductSNIPS(categoryname1, carat, (gia == false) ? "" : "Y", bayar);
					}
					else
					{*/
						ASP = ASPProductSemiMountingGeneric();
					//}
				}
				else if(CheckDossierBrandedSMG(d))
				{
					/*if(category1 == 8 || category1 == 16)
					{
						decimal carat = d.StockProductDJ.StockProductDJ_Stone3.Sum(p => p.TotalCarat); 
						bool gia = d.StockProductDJ.StockProductDJ_Stone3.Any(p => !string.IsNullOrEmpty(p.GIA));
						decimal bayar = d.TotalBayar ?? 0m;
						ASP = ASPProductSNIPS(categoryname1, carat, (gia == false) ? "" : "Y", bayar);
					}
					else
					{*/
						ASP = ASPProductSemiMountingBranded();
					//}
				}
				else
				{
					//ASP = ASPProduct.getASPDJParameter(d.StockProductDJ, d.TotalBayar.Value);
					ASP = DLC.getASPDJParameter(d.StockProductDJ, d.TotalBayar.Value);
				}
				//ASP = DLC.getASPDJParameter(d.StockProductDJ, d.TotalBayar.Value);
				general.Visible = false;

				bool isMountingOnly = false;

				if (!isMountingOnly && d.StockProductDJ.StockProductDJ_CharProduct.CharStoneDist.NamaKode == "SOL"
						&& d.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "SI"
						&& d.StockProductDJ.StockProductDJ_CharProduct.StoneQty == 0
						)
				{ isMountingOnly = true; }

				if (!isMountingOnly && d.StockProductDJ.StockProductDJ_CharProduct.CharProductLevel.NamaKode == "MTG"
					&& d.StockProductDJ.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "SI"
					&& d.StockProductDJ.StockProductDJ_CharProduct.StoneQty == 0
					)
				{ isMountingOnly = true; }

				if (isMountingOnly)
				{
					// Rangka only - PG TGP - Only Gold Value ...
					asp.Visible = false;
					asp2.Visible = false;
					general.Visible = true;
				}
				else if (ASP[0] == "")
				{
					// No Resell No Buyback
					asp.Visible = false;
					asp2.Visible = true;
				}
				else if(item1 == 64 || item1 == 91)
				{
					// No Resell No Buyback
					asp.Visible = false;
					asp2.Visible = true;
				}
				else
				{
					// ASP Normal
					me.Text = "";
					ti1.Text = "";
					ti2.Text = "";
					ti3.Text = "";
					rs1.Text = "";
					rs2.Text = "";
						
					if (ASP[18] != "" && ASP[18] != "0")
					{
						me.Text = asp_text_period("<", Convert.ToInt32(ASP[18]), "d");
					}
					else if (ASP[1] != "")
					{
						me.Text = asp_text_period("<", Convert.ToInt32(ASP[1]), "m");
					}

					ti1.Text = asp_text_period("<", Convert.ToInt32((ASP[4] == "0" ? ASP[6] : ASP[4])) / 12, "y");
					if(string.IsNullOrEmpty(ti1.Text))
						ti1.Text = asp_text_period("<", Convert.ToInt32((ASP[4] == "0" ? ASP[6] : ASP[4])), "m");																		   
					ti2.Text = asp_text_period("<", Convert.ToInt32((ASP[7] == "0" ? ASP[9] : ASP[7])) / 12, "y");
					ti3.Text = asp_text_period(">", Convert.ToInt32(ASP[9]) / 12, "y");
					rs1.Text = asp_text_period("<", Convert.ToInt32((ASP[13] == "0" ? ASP[15] : ASP[13])) / 12, "y");
					rs2.Text = asp_text_period(">", Convert.ToInt32(ASP[15]) / 12, "y");

					me_rumus1.Text = asp_text_value(BrandText, "me", Convert.ToDecimal(ASP[2]));
					ti_rumus1.Text = asp_text_value(BrandText, "ti", Convert.ToDecimal(ASP[5]), (ASP[4] == "0" ? true : false));
					ti_rumus2.Text = asp_text_value(BrandText, "ti", Convert.ToDecimal(ASP[8]), (ASP[7] == "0" ? true : false));
					ti_rumus3.Text = asp_text_value(BrandText, "ti", Convert.ToDecimal(ASP[11]), (ASP[10] == "0" ? true : false));
					rs_rumus1.Text = asp_text_value(BrandText, "rs", Convert.ToDecimal(ASP[14]), (ASP[13] == "0" ? true : false));
					rs_rumus2.Text = asp_text_value(BrandText, "rs", Convert.ToDecimal(ASP[17]), (ASP[16] == "0" ? true : false));
					
				}
            }
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
	protected string[] ASPProductIntro()
    {
        string[] x = new string[20];
        var d = db.ASPDJ.SingleOrDefault(p => p.ID == 23);
        if (d == null) { x[0] = ""; }
        else
        {
            x[0] = d.MEPeriodeMin.ToString();
            x[1] = d.MEPeriodeMax.ToString();
            x[2] = d.MERumus.ToString("N2");
            x[3] = (d.TI1_PeriodeMin).ToString();
            x[4] = (d.TI1_PeriodeMax).ToString();
            x[5] = d.TI1_Rumus.ToString("N2");
            x[6] = (d.TI2_PeriodeMin).ToString();
            x[7] = (d.TI2_PeriodeMax).ToString();
            x[8] = d.TI2_Rumus.ToString("N2");
            x[9] = (d.TI3_PeriodeMin).ToString();
            x[10] = (d.TI3_PeriodeMax).ToString();
            x[11] = d.TI3_Rumus.ToString("N2");
            x[12] = (d.R1_PeriodeMin).ToString();
            x[13] = (d.R1_PeriodeMax).ToString();
            x[14] = d.R1_Rumus.ToString("N2");
            x[15] = (d.R2_PeriodeMin).ToString();
            x[16] = (d.R2_PeriodeMax).ToString();
            x[17] = d.R2_Rumus.ToString("N2");
            x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
        }
        return x;
    }
	
	protected string[] ASPProductSNIPS(string kategori, decimal carat, string gia, decimal harga)
    {
        string[] x = new string[20];
		int ctgr = 17;
		if(kategori == "SNIPS" && gia == "" && carat <= 3)
			ctgr = 17;
		else if(kategori == "SNIPS" && gia != "" && carat <= 3)
			ctgr = 18;
		else if(kategori == "SNIPS" && gia != "" && carat >= 3)
			ctgr = 19;
		if(kategori == "LPS" &&  harga <= 2500000000)
			ctgr = 20;
		else if(kategori == "LPD" && harga > 2500000000)
			ctgr = 21;
        var d = db.ASPDJ.SingleOrDefault(p => p.ID == ctgr);
        if (d == null) { x[0] = ""; }
        else
        {
            x[0] = d.MEPeriodeMin.ToString();
            x[1] = d.MEPeriodeMax.ToString();
            x[2] = d.MERumus.ToString("N2");
            x[3] = (d.TI1_PeriodeMin).ToString();
            x[4] = (d.TI1_PeriodeMax).ToString();
            x[5] = d.TI1_Rumus.ToString("N2");
            x[6] = (d.TI2_PeriodeMin).ToString();
            x[7] = (d.TI2_PeriodeMax).ToString();
            x[8] = d.TI2_Rumus.ToString("N2");
            x[9] = (d.TI3_PeriodeMin).ToString();
            x[10] = (d.TI3_PeriodeMax).ToString();
            x[11] = d.TI3_Rumus.ToString("N2");
            x[12] = (d.R1_PeriodeMin).ToString();
            x[13] = (d.R1_PeriodeMax).ToString();
            x[14] = d.R1_Rumus.ToString("N2");
            x[15] = (d.R2_PeriodeMin).ToString();
            x[16] = (d.R2_PeriodeMax).ToString();
            x[17] = d.R2_Rumus.ToString("N2");
            x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
        }
        return x;
    }
	
	protected string[] ASPProductDossierMounting()
	{
		string[] x = new string[20];
		var d = db.ASPDJ.SingleOrDefault(p => p.ID == 2);
        if (d == null) { x[0] = ""; }
        else
        {
            x[0] = d.MEPeriodeMin.ToString();
            x[1] = d.MEPeriodeMax.ToString();
            x[2] = d.MERumus.ToString("N2");
            x[3] = (d.TI1_PeriodeMin).ToString();
            x[4] = (d.TI1_PeriodeMax).ToString();
            x[5] = d.TI1_Rumus.ToString("N2");
            x[6] = (d.TI2_PeriodeMin).ToString();
            x[7] = (d.TI2_PeriodeMax).ToString();
            x[8] = d.TI2_Rumus.ToString("N2");
            x[9] = (d.TI3_PeriodeMin).ToString();
            x[10] = (d.TI3_PeriodeMax).ToString();
            x[11] = d.TI3_Rumus.ToString("N2");
            x[12] = (d.R1_PeriodeMin).ToString();
            x[13] = (d.R1_PeriodeMax).ToString();
            x[14] = d.R1_Rumus.ToString("N2");
            x[15] = (d.R2_PeriodeMin).ToString();
            x[16] = (d.R2_PeriodeMax).ToString();
            x[17] = d.R2_Rumus.ToString("N2");
            x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
        }
		return x;
	}
	
	protected string[] ASPProductSemiMountingGeneric()
	{
		string[] x = new string[20];
        var d = db.ASPDJ.SingleOrDefault(p => p.ID == 10);
        if (d == null) { x[0] = ""; }
        else
        {
            x[0] = d.MEPeriodeMin.ToString();
            x[1] = d.MEPeriodeMax.ToString();
            x[2] = d.MERumus.ToString("N2");
            x[3] = (d.TI1_PeriodeMin).ToString();
            x[4] = (d.TI1_PeriodeMax).ToString();
            x[5] = d.TI1_Rumus.ToString("N2");
            x[6] = (d.TI2_PeriodeMin).ToString();
            x[7] = (d.TI2_PeriodeMax).ToString();
            x[8] = d.TI2_Rumus.ToString("N2");
            x[9] = (d.TI3_PeriodeMin).ToString();
            x[10] = (d.TI3_PeriodeMax).ToString();
            x[11] = d.TI3_Rumus.ToString("N2");
            x[12] = (d.R1_PeriodeMin).ToString();
            x[13] = (d.R1_PeriodeMax).ToString();
            x[14] = d.R1_Rumus.ToString("N2");
            x[15] = (d.R2_PeriodeMin).ToString();
            x[16] = (d.R2_PeriodeMax).ToString();
            x[17] = d.R2_Rumus.ToString("N2");
            x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
        }
        return x;
	}
	protected string[] ASPProductSemiMountingBranded()
	{
		string[] x = new string[20];
        var d = db.ASPDJ.SingleOrDefault(p => p.ID == 2);
        if (d == null) { x[0] = ""; }
        else
        {
            x[0] = d.MEPeriodeMin.ToString();
            x[1] = d.MEPeriodeMax.ToString();
            x[2] = d.MERumus.ToString("N2");
            x[3] = (d.TI1_PeriodeMin).ToString();
            x[4] = (d.TI1_PeriodeMax).ToString();
            x[5] = d.TI1_Rumus.ToString("N2");
            x[6] = (d.TI2_PeriodeMin).ToString();
            x[7] = (d.TI2_PeriodeMax).ToString();
            x[8] = d.TI2_Rumus.ToString("N2");
            x[9] = (d.TI3_PeriodeMin).ToString();
            x[10] = (d.TI3_PeriodeMax).ToString();
            x[11] = d.TI3_Rumus.ToString("N2");
            x[12] = (d.R1_PeriodeMin).ToString();
            x[13] = (d.R1_PeriodeMax).ToString();
            x[14] = d.R1_Rumus.ToString("N2");
            x[15] = (d.R2_PeriodeMin).ToString();
            x[16] = (d.R2_PeriodeMax).ToString();
            x[17] = d.R2_Rumus.ToString("N2");
            x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
        }
        return x;
	}
	protected bool CheckDossierGenericSMG(SalesOrderDJ dj) 
    {
        bool djancoeg = false;
        if (dj.StockProductDJ.StockProductDJ_Stone1B.Any()) 
        {
            if (CheckGeneric(dj.StockProductDJ) && 
                (dj.StockProductDJ.StockProductDJ_CharProduct.CharStoneDist.NamaKode == "SMF" || dj.StockProductDJ.StockProductDJ_CharProduct.CharStoneDist.NamaKode == "SMS"))
            {
                djancoeg = true;
            }
			else
			{
				if (CheckGeneric(dj.StockProductDJ) && (dj.StockProductDJ.StockProductDJ_CharProduct.CharProductLevel.NamaKode == "SMG"))
				{
					djancoeg = true;
				}
			}
			
        }
        return djancoeg;
    }
	protected bool CheckDossierBrandedSMG(SalesOrderDJ dj)
	{
		bool djancoeg = false;
		if (dj.StockProductDJ.StockProductDJ_Stone1B.Any()) 
        {
            if (CheckBranded(dj.StockProductDJ) && 
                (dj.StockProductDJ.StockProductDJ_CharProduct.CharStoneDist.NamaKode == "SMF" || dj.StockProductDJ.StockProductDJ_CharProduct.CharStoneDist.NamaKode == "SMS"))
            {
                djancoeg = true;
            }
			else
			{
				if (CheckBranded(dj.StockProductDJ) && (dj.StockProductDJ.StockProductDJ_CharProduct.CharProductLevel.NamaKode == "SMG"))
				{
					djancoeg = true;
				}
			}
			
        }
		return djancoeg;
	}

    protected bool CheckGeneric(StockProductDJ s)
    {
        bool generic = false;

        var stone1b = s.StockProductDJ_Stone1B;
        foreach(var x in stone1b)
        {
            if (x.Stone1B.Parcel106.NamaKode == "GN" || x.Stone1B.Parcel106.NamaKode == "C" || x.Stone1B.Parcel106.NamaKode == "NM")
                generic = true;
            break;
        }

        //generic = Pricing.isDiamondGeneric(s.Nomor);
        return generic;
    }
    protected bool CheckBranded(StockProductDJ s)
    {
        bool branded = false;

        var stone1b = s.StockProductDJ_Stone1B;
        foreach (var x in stone1b)
        {
            if (x.Stone1B.Parcel106.IsBranded == true)
                branded = true;
            break;
        }

        //branded = Pricing.isDiamondBranded(s.Nomor);
        return branded;
    }
}