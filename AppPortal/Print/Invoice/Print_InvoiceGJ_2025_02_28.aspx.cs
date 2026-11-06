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
        //App.Protect();
        //App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Fill();
            
        }
    }
    protected void Fill()
    {
        var d = db.SalesOrderGJ.SingleOrDefault(p => p.ID == myID);
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
			
			lit_ppn.Text = "2,2 %";
			lit_ppn2.Text = "2,2 %";
			lit_ppn3.Text = "2,2 %";
			lit_ppn4.Text = "2,2 %";
			lblppn.InnerText = "11%";
			decimal rateppn = 1.022m;
			int ppnunit = 11;
			if(d.SalesOrder.OperatorTgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
			{
				lit_ppn.Text = "2 %";
				lit_ppn2.Text = "2 %";
				lit_ppn3.Text = "2 %";
				lit_ppn4.Text = "2 %";
				lblppn.InnerText = "10%";
				rateppn = 1.02m;
				ppnunit = 10;
			}
			
			if(!IsUsePPN || d.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama == "LM")
			{
				lit_ppn.Text = "0 %";
				lit_ppn2.Text = "0 %";
				lit_ppn3.Text = "0 %";
				lit_ppn4.Text = "0 %";
				ppnunit = 0;
				
				divuseppn.Visible = false;
				lit_ppn_notice.Visible = false;
				lit_ppn_notice2.Visible = false;
				
				lblppn10.InnerHtml = "<em>Price</em> / Harga Jual ";
				lbl_ppn_notice.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
				lbl_ppn_notice2.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
			}
			else
			{
				divuseppn.Visible = true;
				lit_ppn_notice.Visible = true;
				lit_ppn_notice2.Visible = true;
				
				lblppn10.InnerHtml = "<em>Price Exclude VAT</em> / Harga Jual Tidak Termasuk PPN ";
				lbl_ppn_notice.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
				lbl_ppn_notice2.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
			}

            DPP = (!IsUsePPN) ? (d.TotalBayar.Value - LakuEmasNominal) : (d.TotalBayar.Value - LakuEmasNominal) / rateppn;
            PPN = d.TotalBayar.Value - DPP - LakuEmasNominal;
			
			try
			{
				Db.Execute("EXEC sp_update_ppn " + myID + ",3,'" + Convert.ToInt32(PPN) + "','" + ppnunit + "'" ,Db.Cnn_DB);
			}
			catch{}

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
            //adding firman 280818 user control untuk detail payment dari /UC/DetailPaymentControl.aspx
            if (TotalItem == 1)
            {
                detailPaymentControl.SetDetailPayment("GJ", d.SalesOrder.ID, Convert.ToDecimal(d.SalesOrder.Unpaid.Value), Convert.ToDecimal(d.SalesOrder.TotalBayar.Value));

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
            Item += "<td " + w1 + ">" + d.StockProductGJ.Nomor + "</td>";
            Item += "<td " + w2 + ">" + Str.CapUpper(d.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama) + " BERAT " + Convert.ToDecimal(d.StockProductGJ.StockProductGJ_CharProduct.GrossWeight).ToString("N2") + " KADAR " + Math.Floor(d.StockProductGJ.StockProductGJ_CharProduct.KadarLogam).ToString("N0") + "% WARNA " + Str.CapUpper(d.StockProductGJ.StockProductGJ_CharProduct.CharFrameColor.Nama) + "</td>";
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
				string query = "exec sp_get_asp_2024 '3','" + d.StockProductGJ.ID + "'";
				DataTable dt = Db.Rs(query, Db.Cnn_DB);
				
				string kodebrand = d.SalesOrder.DataSales.CompanyBrand.NamaKode;
				string namabrand = d.SalesOrder.DataSales.CompanyBrand.Nama;
				WriteASP(dt, kodebrand, namabrand);
			}
        }
    }
	
	protected void WriteASP(DataTable dt, string kodebrand, string namabrand)
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
			if(kodebrand == "P")
			{
				asp.Visible = false;
				thepalace.Visible = true;
				general.Visible = false;
			}
			else
			{
				asp.Visible = false;
				thepalace.Visible = false;
				general.Visible = true;
			}
		}
		else
		{
			asp.Visible = false;
			asp2.Visible = true;
		}
	}
}