using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

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
        var d = db.SalesOrderLD.SingleOrDefault(p => p.ID == myID);
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
			if(d.SalesOrder.Tgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
			{
				lit_ppn.Text = "2 %";
				lit_ppn2.Text = "2 %";
				lblppn.InnerText = "10%";
				lblppn2.InnerText = "20%";
				rateppn = 1.02m;
				ppnunit = 10;
			}
            else if(d.SalesOrder.Tgl <= Convert.ToDateTime("2023-04-30 23:59:00.000"))
			{
				lit_ppn.Text = "2,2 %";
				lit_ppn2.Text = "2,2 %";
				lblppn.InnerText = "11%";
				lblppn2.InnerText = "20%";
				rateppn = 1.022m;
				ppnunit = 11;
			}
            else if(d.SalesOrder.Tgl <= Convert.ToDateTime("2024-12-30 23:59:00.000"))
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
                string getKeterangan = @"select CAST(ax.PPNPercentage AS Numeric(9,4)) PPNPercentage,CAST(ax.PPNPembagi AS Numeric(9,4)) PPNPembagi
										from (
											SELECT TOP 1 a.*
											FROM SalesOrderLD A 											
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
                            }
                        }
                        conn.Close();
                    }
                }
                lit_ppn.Text = "1,1 %";
                lit_ppn2.Text = "1,1 %";                
                lblppn2.InnerText = "10%";
                
				
			}
			
			if(!IsUsePPN || d.StockProductLD_Stone1B.CharProductItem.Nama == "LM")
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

            DPP = (!IsUsePPN) ? (d.TotalBayar.Value - LakuEmasNominal) : (d.TotalBayar.Value -LakuEmasNominal) / rateppn;
            PPN = d.TotalBayar.Value - DPP - LakuEmasNominal;
			
			// try
			// {
			// 	Db.Execute("EXEC sp_update_ppn " + myID + ",4,'" + Convert.ToInt32(PPN) + "','" + ppnunit + "'" ,Db.Cnn_DB);
			// }
			// catch{}

            dpp.Text = DPP.ToString("N0");
            ppn.Text = PPN.ToString("N0");
            nominal.Text = d.TotalBayar.Value.ToString("N0");
            totaldp.Text = (d.SalesOrder.StatusPembayaran == true) ? d.TotalBayar.Value.ToString("N0") : (d.TotalDP.HasValue ? d.TotalDP.Value.ToString("N0") : "0");
            sisa.Text = d.TotalDP.HasValue ? (d.TotalBayar.Value - ((d.SalesOrder.StatusPembayaran == true) ? d.TotalBayar.Value : d.TotalDP.Value)).ToString("N0") : d.TotalBayar.Value.ToString("N0");

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
                detailPaymentControl.SetDetailPayment("LD", d.SalesOrder.ID, Convert.ToDecimal(d.SalesOrder.Unpaid.Value), Convert.ToDecimal(d.SalesOrder.TotalBayar.Value));
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
            Item += "<td " + w1 + ">" + d.StockProductLD_Stone1B.Nomor + "</td>";
            Item += "<td " + w2 + ">" + Str.CapUpper(d.StockProductLD_Stone1B.TotalCarat.ToString("N3")) + " " + d.StockProductLD_Stone1B.Stone1B.Parcel103.Nama + " " + d.StockProductLD_Stone1B.Stone1B.Parcel104.Nama + "</td>";
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
			else
			{
				string query = "exec sp_get_asp_2024 '4','" + d.StockProductLD_Stone1B.ID + "'";
				DataTable dt = Db.Rs(query, Db.Cnn_DB);
				
				string kodebrand = d.SalesOrder.DataSales.CompanyBrand.NamaKode;
				string namabrand = d.SalesOrder.DataSales.CompanyBrand.Nama;
				bool isbranded = d.StockProductLD_Stone1B.Stone1B.Parcel106.IsBranded ?? false;
				WriteASP(dt, kodebrand, namabrand, isbranded);
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
            //x = val.ToString() + " % <br /> <em>Original sales amount</em> <br /> Nilai Penjualan Asal";
			x = val.ToString() + " % <br /> <em>Today Market Price</em>";
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
	
	protected void WriteASP(DataTable dt, string kodebrand, string namabrand, bool isbranded)
	{
		int rowscount = 0;
		string BrandText = "";
		if(kodebrand == "F")
			BrandText = "Frank & co";
		else if(kodebrand == "S" || kodebrand == "M")
			BrandText = "Mondial";
		else if(kodebrand == "P")
			BrandText = "The Palace";
		
		rowscount = dt.Rows.Count;
		
		if(rowscount == 0)
		{
			asp.Visible = false;
			asp2.Visible = true;
		}
		else if(rowscount == 1 && (dt.Rows[0][0].ToString() != ""))
		{
			asp.Visible = true;
			asp2.Visible = false;
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
			
			if(ti1aspperiodmax < 12)
			{
				ti1.Text = asp_text_period("<", ti1aspperiodmax, "m");
			}
			else
			{
				ti1.Text = asp_text_period("<", ti1aspperiodmax / 12, "y");
			}
			
			ti2.Text = asp_text_period("<", ti2aspperiodmax / 12, "y");
			ti3.Text = asp_text_period(">", ti3aspperiodmax / 12, "y");
			rs1.Text = asp_text_period("<", r1aspperiodmax / 12, "y");
			rs2.Text = asp_text_period(">", r2aspperiodmax / 12, "y");
			
			me_rumus1.Text = asp_text_value(BrandText, "me", measprumus);
			ti_rumus1.Text = asp_text_value(BrandText, "ti", ti1asprumus, (!isbranded) ? true : false);
			ti_rumus2.Text = asp_text_value(BrandText, "ti", ti2asprumus, (!isbranded) ? true : false);
			ti_rumus3.Text = asp_text_value(BrandText, "ti", ti3asprumus, true);
			rs_rumus1.Text = asp_text_value(BrandText, "rs", r1asprumus, true);
			rs_rumus2.Text = asp_text_value(BrandText, "rs", r2asprumus, true);
		}
		else
		{
			asp.Visible = false;
			asp2.Visible = true;
		}
	}
}