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
			
			if(KodeLokasi == "E46" || KodeLokasi == "P18" || d.StockProductLD_Stone1B.CharProductItem.Nama == "LM")
			{
				ppnunit = 0;
			}

            DPP = (KodeLokasi == "E46" || KodeLokasi == "P18" || d.StockProductLD_Stone1B.CharProductItem.Nama == "LM") ? (d.TotalBayar.Value - LakuEmasNominal) : (d.TotalBayar.Value -LakuEmasNominal) / rateppn;
            PPN = d.TotalBayar.Value - DPP - LakuEmasNominal;
			
			try
			{
				Db.Execute("EXEC sp_update_ppn " + myID + ",4,'" + Convert.ToInt32(PPN) + "','" + ppnunit + "'" ,Db.Cnn_DB);
			}
			catch{}

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

            bool Solitaire = false;
			
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
				string[] ASP = new string[20];
				ASP = DLC.getASPLDParameter(1, Solitaire);
				if(IsFancyDiamond(d.StockProductLD_Stone1B))
				{
					asp.Visible = false;
					asp2.Visible = true;
				}
				else
				{
					if (ASP[0] == "")
					{
						asp.Visible = false;
						asp2.Visible = true;
					}
					else
					{
						me1.Text = ASP[1];
						me2.Text = ASP[1];
						me_rumus1.Text = ASP[2];
						t1.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[4]) / 12)).ToString();
						ti_rumus1.Text = ASP[5];
						t2.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[7]) / 12)).ToString();
						ti_rumus2.Text = ASP[8];
						t3.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[9]) / 12)).ToString();
						r1.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[13]) / 12)).ToString();
						//r1_rumus.Text = ASP[14];
						r2.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[15]) / 12)).ToString();

						brand.Text = BrandText;
						brand2.Text = BrandText;
						brand3.Text = BrandText;
						brand4.Text = BrandText;
						brand5.Text = BrandText;
						brand6.Text = BrandText;

					}
				}
			}
        }
    }
	
	protected bool IsFancyDiamond(StockProductLD_Stone1B ld)
	{
		bool IsFancy = false;
		string NamaKodeParcel103 = ld.Stone1B.Parcel103.NamaKode;
		if(NamaKodeParcel103 == "VY" || NamaKodeParcel103 == "IY" || NamaKodeParcel103 == "FY")
			IsFancy = true;
		return IsFancy;
	}
}