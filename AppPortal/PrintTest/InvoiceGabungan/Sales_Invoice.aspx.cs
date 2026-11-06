using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

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

    static string UppercaseWords(string text)
    {
        
        
            return char.ToUpper(text[0]) +
                ((text.Length > 1) ? text.Substring(1).ToLower() : string.Empty);
       
    }

    protected void Fill()
    {
        var d = db.SalesOrder.SingleOrDefault(p => p.ID == myID && p.StatusPembayaran == true && p.StatusVoid == false);
        if (d == null)
        {
            Response.Redirect("/OutletSales/Page/Summary.aspx");
        }
        else
        {
            var py = from p in db.SalesReceiptDetail
                     where p.SalesReceipt.IDSalesOrder == myID
                     orderby p.OperatorTgl descending
                     select p;

            string BrandText = "CMK";
            string Cabang = "";
            string AlamatCabang = "";
            string KodeLokasi = "";
			bool IsUsePPN = false;

            if (d.TipeLokasi == 1)
            {
                var x = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);

                BrandText = x.CompanyBrand.Nama;
                Cabang = x.Nama;
                AlamatCabang = x.AddrAlamat;
                KodeLokasi = x.Kode;
				
				IsUsePPN = Db.SingleBool("SELECT TOP 1 IsUsePPN FROM LocOutlet WHERE ID = " + x.ID, Db.Cnn_DB);
				lblnpwp.InnerText = "NPWP : " + Db.SingleString("SELECT TOP 1 NPWP FROM LocOutlet WHERE ID = " + x.ID, Db.Cnn_DB);
            }
            else
            {
                var x = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);

                // corporate exhibition
                if (d.IDLokasi == 18 && x.Kode == "XPR")
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

            alamat.Text = Str.HtmlBreak(AlamatCabang);
            tgl.Text = Str.Tgl(d.Tgl.Value);

            noinvoice.Text = d.Nomor;
            customer.Text = Str.CapUpper(d.DataCustomer.Nama);
            nocustomer.Text = Str.CapUpper(d.DataCustomer.NoCustomer);
            customer2.Text = Str.CapUpper(d.DataCustomer.Nama);
			customerother.Text = Str.CapUpper(String.IsNullOrEmpty(d.CustomerReferenceNama) ? d.DataCustomer.Nama : d.CustomerReferenceNama);
            adminsales.Text = Str.CapUpper(d.SalesNama);

            decimal DPP = 0m;
            decimal PPN = 0m;
            decimal Nominal = 0m;
            decimal TotalDP = 0m;
            decimal Sisa = 0m;

            if (d.TradeIn == true)
            {
                decimal xTotalRupiah = d.HargaRupiah.Value;
                decimal xTotalDiscount = 0m;

                var yDiscDJ = db.SalesOrderDJ.Where(p => p.IDForm == d.ID);
                var yDiscPG = db.SalesOrderPG.Where(p => p.IDForm == d.ID);
                var yDiscGJ = db.SalesOrderGJ.Where(p => p.IDForm == d.ID);
                var yDiscLD = db.SalesOrderLD.Where(p => p.IDForm == d.ID);

                var xDiscDJ = (yDiscDJ.Count() > 0) 
                            ? (yDiscDJ.Sum(r => r.DiscountNominal.Value
                            + r.DiscountProgramNominal.Value
                            + r.DiscountGift.Value
                            + r.DiscountRound.Value
                            + (r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0m)
                            + r.DiscountBank
                            + r.DiscountOther))
                            : 0m
                            ;
                var xDiscPG = (yDiscPG.Count() > 0)
                            ? (yDiscPG.Sum(r => r.DiscountNominal.Value
                            + r.DiscountProgramNominal.Value
                            + r.DiscountGift.Value
                            + r.DiscountRound.Value
                            + (r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0m)
                            + r.DiscountBank
                            + r.DiscountOther))
                            : 0m
                            ;
                var xDiscGJ = (yDiscGJ.Count() > 0)
                            ? (yDiscGJ.Sum(r => r.DiscountNominal.Value
                            + r.DiscountProgramNominal.Value
                            + r.DiscountGift.Value
                            + r.DiscountRound.Value
                            + (r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0m)
                            + r.DiscountBank
                            + r.DiscountOther))
                            : 0m
                            ;
                var xDiscLD = (yDiscLD.Count() > 0)
                            ? (yDiscLD.Sum(r => r.DiscountNominal.Value
                            + r.DiscountProgramNominal.Value
                            + r.DiscountGift.Value
                            + r.DiscountRound.Value
                            + (r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0m)
                            + r.DiscountBank
                            + r.DiscountOther))
                            : 0m
                            ;
                xTotalDiscount = (xDiscDJ != null ? xDiscDJ : 0m)
                                + (xDiscPG != null ? xDiscPG : 0m)
                                + (xDiscGJ != null ? xDiscGJ : 0m)
                                + (xDiscLD != null ? xDiscLD : 0m)
                                ;

                //DPP = (xTotalRupiah - xTotalDiscount) / 1.02m;
				
				//Cetak Invoice Jika ada pembayaran laku emas
                decimal LakuEmasNominal = 0m;
				decimal DebitLakuEmasNominal = 0m;
                /*foreach (var i in py)
                {
                    if(i.IDPaymentType == 11)
                    {
                        LakuEmasNominal = LakuEmasNominal + Convert.ToDecimal(i.Nominal);
                    }
					if(((i.IDPaymentType == 4 || i.IDPaymentType == 2) && (i.IDEDC == 24 || i.IDEDC == 26 || i.IDEDC == 27 || i.IDEDC == 28)) || (i.IDPaymentType == 15 &&(i.IDEDC == 31 || i.IDEDC == 32)))
					{
						LakuEmasNominal = LakuEmasNominal + Convert.ToDecimal(i.Nominal);
					}
                }
				if (py.Count(p => p.IDPaymentType == 11) > 0)
				{
					le.Text = LakuEmasNominal.ToString("N0");
					paymentwithlakuemaspanel.Visible = true;
				}
				else if(py.Count(p => ((p.IDPaymentType == 4 || p.IDPaymentType == 2) && (p.IDEDC == 24 || p.IDEDC == 26 || p.IDEDC == 27 || p.IDEDC == 28) || (p.IDPaymentType == 15 && (p.IDEDC == 31 || p.IDEDC == 32)))) > 0)
				{
					try
					{
						decimal weight_pg_total = 0m;
						decimal weight_pg = 0m;
						decimal weight_lm_total = 0m;
						decimal weight_lm = 0m;
						decimal rate_lm = 0m;
						foreach (var j in py)
						{
							if(((j.IDPaymentType == 4 || j.IDPaymentType == 2) && (j.IDEDC == 24 || j.IDEDC == 26 || j.IDEDC == 27 || j.IDEDC == 28)) || (j.IDPaymentType == 15 &&(j.IDEDC == 31 || j.IDEDC == 32)))
							{
								rate_lm = Db.SingleDecimal("SELECT TOP 1 ISNULL(RateLM,0) FROM SalesReceiptDetailDebitLakuEmas WHERE IDSalesReceiptDetail = "+j.ID+" AND RateLM IS NOT NULL ", Db.Cnn_DB);
							}
						}
						var pg = d.SalesOrderPG;
						
						decimal harga_pg = 0m;
						decimal harga_pg_total = 0m;
						decimal harga_lm = 0m;
						decimal harga_lm_total = 0m;
						decimal ongkos = 0m;
						foreach(var item in pg)
						{
							weight_pg = item.StockProductPG.NettoWeight ?? 0m;
							weight_lm = (3 * weight_pg) / 4;
							int countisanyrate = Db.SingleInteger("SELECT COUNT('') FROM SalesOrderPGLEI WHERE IDProduct = " + item.IDProduct + " AND IDSalesOrder = " + item.IDForm, Db.Cnn_DB);
							if(countisanyrate > 0)
								rate_lm = Db.SingleDecimal("SELECT TOP 1 ISNULL(TGPJual,0) [TGPJual] FROM SalesOrderPGLEI WHERE IDProduct = " + item.IDProduct + " AND IDSalesOrder = " + item.IDForm ,Db.Cnn_DB);
							
							harga_lm = weight_lm * rate_lm;
							weight_pg_total += item.StockProductPG.NettoWeight ?? 0m;
							weight_lm_total += (item.StockProductPG.NettoWeight ?? 0m) * (0.75m);
							harga_pg_total += item.TotalBayar ?? 0m;
							harga_lm_total += harga_lm;
							
						}
						//weight_lm_total = (3 * weight_pg_total) / 4;
						//harga_lm = weight_lm_total * rate_lm;
						//harga_pg = d.TotalBayar ?? 0m;
						//ongkos = harga_pg - harga_lm;
						LakuEmasNominal = harga_lm_total;
						
						debitlepg.Text = (harga_pg_total).ToString("N0");
						debitlelm.Text = (harga_lm_total).ToString("N0");
						paymentwithdebitlakuemaspanel.Visible = true;
					}
					catch
					{
						LakuEmasNominal = 0m;
						//le.Text = (0m).ToString("N0");
						//paymentwithlakuemaspanel.Visible = false;
						paymentwithdebitlakuemaspanel.Visible = false;
					}
				}
				else
				{
					le.Text = (0m).ToString("N0");
					paymentwithlakuemaspanel.Visible = false;
				}*/
				
				lit_ppn.Text = "2,2 %";
				lit_ppn2.Text = "2,2 %";
				lit_ppn3.Text = "2,2 %";
				lit_ppn4.Text = "2,2 %";
				lit_ppn5.Text = "2,2 %";
				lit_ppn6.Text = "2,2 %";
				lblppn.InnerText = "11%";
				decimal rateppn = 1.022m;
				if(d.OperatorTgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
				{
					lit_ppn.Text = "2 %";
					lit_ppn2.Text = "2 %";
					lit_ppn3.Text = "2 %";
					lit_ppn4.Text = "2 %";
					lit_ppn5.Text = "2 %";
					lit_ppn6.Text = "2 %";
					lblppn.InnerText = "10%";
					rateppn = 1.02m;
				}
				
				if(!IsUsePPN)
				{
					lit_ppn.Text = "0 %";
					lit_ppn2.Text = "0 %";
					lit_ppn3.Text = "0 %";
					lit_ppn4.Text = "0 %";
					lit_ppn5.Text = "0 %";
					lit_ppn6.Text = "0 %";
					
					divuseppn.Visible = false;
					lit_ppn_notice.Visible = false;
					lit_ppn_notice2.Visible = false;
					lit_ppn_notice3.Visible = false;
					
					lblppn10.InnerHtml = "<em>Price</em> / Harga Jual ";
					lbl_ppn_notice.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
					lbl_ppn_notice2.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
					lbl_ppn_notice3.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
				}
				else
				{
					divuseppn.Visible = true;
					lit_ppn_notice.Visible = true;
					lit_ppn_notice2.Visible = true;
					lit_ppn_notice3.Visible = true;
					
					lblppn10.InnerHtml = "<em>Price Exclude VAT</em> / Harga Jual Tidak Termasuk PPN ";
					lbl_ppn_notice.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
					lbl_ppn_notice2.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
					lbl_ppn_notice3.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
				}
				
                DPP = (!IsUsePPN) ? (xTotalRupiah - xTotalDiscount - LakuEmasNominal - DebitLakuEmasNominal) : (xTotalRupiah - xTotalDiscount - LakuEmasNominal - DebitLakuEmasNominal) / rateppn;
                PPN = (xTotalRupiah - xTotalDiscount - LakuEmasNominal - DebitLakuEmasNominal) - DPP;
                Nominal = (xTotalRupiah - xTotalDiscount);
            }
            else
            {
                //DPP = d.TotalBayar.Value / 1.02m;
                //DPP = (KodeLokasi == "E46" || KodeLokasi == "P18" || KodeLokasi == "P45") ? d.TotalBayar.Value : d.TotalBayar.Value / 1.02m;
                //PPN = d.TotalBayar.Value - DPP;
                //Nominal = d.TotalBayar.Value;
				
                //Cetak Invoice Jika ada pembayaran laku emas
                decimal LakuEmasNominal = 0m;
				decimal DebitLakuEmasNominal = 0m;
				/*foreach (var i in py)
                {
                    if(i.IDPaymentType == 11)
                    {
                        LakuEmasNominal = LakuEmasNominal + Convert.ToDecimal(i.Nominal);
                    }
                }
				if (py.Count(p => p.IDPaymentType == 11) > 0)
				{
					foreach (var i in py)
					{
						if(i.IDPaymentType == 11)
						{
							LakuEmasNominal = LakuEmasNominal + Convert.ToDecimal(i.Nominal);
						}
					}
                    le.Text = LakuEmasNominal.ToString("N0");
					paymentwithlakuemaspanel.Visible = true;
				}
				else if(py.Count(p => ((p.IDPaymentType == 4 || p.IDPaymentType == 2) && (p.IDEDC == 24 || p.IDEDC == 26 || p.IDEDC == 27 || p.IDEDC == 28)) || (p.IDPaymentType == 15 && (p.IDEDC == 31 || p.IDEDC == 32))) > 0)
				{
					try
					{
						decimal weight_pg_total = 0m;
						decimal weight_pg = 0m;
						decimal weight_lm_total = 0m;
						decimal weight_lm = 0m;
						decimal rate_lm = 0m;
						
						var pg = d.SalesOrderPG;
						
						decimal harga_pg = 0m;
						decimal harga_pg_total = 0m;
						decimal harga_lm = 0m;
						decimal harga_lm_total = 0m;
						decimal ongkos = 0m;
						foreach(var item in pg)
						{
							weight_pg = item.StockProductPG.NettoWeight ?? 0m;
							weight_lm = (3 * weight_pg) / 4;
							int countisanyrate = Db.SingleInteger("SELECT COUNT('') FROM SalesOrderPGLEI WHERE IDProduct = " + item.IDProduct + " AND IDSalesOrder = " + item.IDForm, Db.Cnn_DB);
							if(countisanyrate > 0)
								rate_lm = Db.SingleDecimal("SELECT TOP 1 ISNULL(TGPJual,0) [TGPJual] FROM SalesOrderPGLEI WHERE IDProduct = " + item.IDProduct + " AND IDSalesOrder = " + item.IDForm ,Db.Cnn_DB);
							
							harga_lm = weight_lm * rate_lm;
							weight_pg_total += item.StockProductPG.NettoWeight ?? 0m;
							weight_lm_total += (item.StockProductPG.NettoWeight ?? 0m) * (0.75m);
							harga_pg_total += item.TotalBayar ?? 0m;
							harga_lm_total += harga_lm;
						}
						
						//weight_lm_total = (3 * weight_pg_total) / 4;
						//harga_lm = weight_lm_total * rate_lm;
						//harga_pg = d.TotalBayar ?? 0m;
						//ongkos = harga_pg - harga_lm;
						LakuEmasNominal = LakuEmasNominal + harga_lm_total;
						
						debitlepg.Text = (harga_pg_total).ToString("N0");
						debitlelm.Text = (harga_lm_total).ToString("N0");
						paymentwithdebitlakuemaspanel.Visible = true;
					}
					catch
					{
						LakuEmasNominal = LakuEmasNominal + 0m;
						paymentwithdebitlakuemaspanel.Visible = false;
					}
				}
				else
				{
					le.Text = (0m).ToString("N0");
					paymentwithlakuemaspanel.Visible = false;
				}*/
				
				lit_ppn.Text = "2,2 %";
				lit_ppn2.Text = "2,2 %";
				lit_ppn3.Text = "2,2 %";
				lit_ppn4.Text = "2,2 %";
				lit_ppn5.Text = "2,2 %";
				lit_ppn6.Text = "2,2 %";
				lblppn.InnerText = "11%";
				decimal rateppn = 1.022m;
				if(d.OperatorTgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
				{
					lit_ppn.Text = "2 %";
					lit_ppn2.Text = "2 %";
					lit_ppn3.Text = "2 %";
					lit_ppn4.Text = "2 %";
					lit_ppn5.Text = "2 %";
					lit_ppn6.Text = "2 %";
					lblppn.InnerText = "10%";
					rateppn = 1.02m;
				}
				
				if(!IsUsePPN)
				{
					lit_ppn.Text = "0 %";
					lit_ppn2.Text = "0 %";
					lit_ppn3.Text = "0 %";
					lit_ppn4.Text = "0 %";
					lit_ppn5.Text = "0 %";
					lit_ppn6.Text = "0 %";
					
					divuseppn.Visible = false;
					lit_ppn_notice.Visible = false;
					lit_ppn_notice2.Visible = false;
					lit_ppn_notice3.Visible = false;
					
					lblppn10.InnerHtml = "<em>Price</em> / Harga Jual ";
					lbl_ppn_notice.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
					lbl_ppn_notice2.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
					lbl_ppn_notice3.InnerHtml = "<em>Original Sales Amount = Price</em> / Nilai  Penjualan Asal = Harga Jual";
				}
				else
				{
					divuseppn.Visible = true;
					lit_ppn_notice.Visible = true;
					lit_ppn_notice2.Visible = true;
					lit_ppn_notice3.Visible = true;
					
					lblppn10.InnerHtml = "<em>Price Exclude VAT</em> / Harga Jual Tidak Termasuk PPN ";
					lbl_ppn_notice.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
					lbl_ppn_notice2.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
					lbl_ppn_notice3.InnerHtml = "<em>Original Sales Amount = Price Exclude VAT</em> / Nilai  Penjualan Asal = Harga Jual Tidak Termasuk PPN";
				}
				
                DPP = (!IsUsePPN) ? (d.TotalBayar.Value - LakuEmasNominal - DebitLakuEmasNominal) : (d.TotalBayar.Value - LakuEmasNominal - DebitLakuEmasNominal) / rateppn;
                PPN = (d.TotalBayar.Value - LakuEmasNominal - DebitLakuEmasNominal) - DPP;
                Nominal = d.TotalBayar.Value ;
            }
			
            //var pym = from p in db.SalesReceiptDetail where p.SalesReceipt.IDSalesOrder.Value == d.ID select p;
            //if (pym.Count() > 0)
            //{
            //    if (d.Unpaid < 0 && pym.Any(p => p.PaymentType.ID == 5 || p.PaymentType.ID == 8)) 
            //    {
            //        LebihBayar_DP_Voucher = true;
            //        LebihBayar_Pure_Voucher = !pym.Any(p => p.PaymentType.ID != 5);

            //    }

            //    string DetailPayment = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"noborder\"><tr><td style=\"vertical-align:top;\" colspan=\"4\"><em>Detail of payment</em></td></tr>";

            //    if (LebihBayar_DP_Voucher && LebihBayar_Pure_Voucher)
            //    {
            //        string vKodeVoucher = "";
            //        int count = 0;
            //        foreach (var r in pym)
            //        {
            //            if (count > 0) { vKodeVoucher += ", "; }
            //            vKodeVoucher += r.CCNumber;
            //            count++;
            //        }
            //        vKodeVoucher = "(" + vKodeVoucher + ")";
            //        DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">Voucher</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right;'>" + d.TotalBayar.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>" + vKodeVoucher + "</td></tr>";

            //        //terbilang.Text = Str.Terbilang(d.TotalBayar.Value);
            //    }
            //    else
            //    {

            //                foreach (var r in pym)
            //                {
            //                // keterangan debit card
            //                if (r.IDPaymentType == 2 && r.IDProgram.HasValue && r.IDBankIssuer.HasValue && r.IDCard.HasValue && !r.IDEDC.HasValue)
            //                {
            //                    DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">" + r.PaymentType.Nama + "</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + r.Nominal.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>(" + r.BankIssuer.Nama + " " + r.CardType.Nama + " " + r.ProgramCicilan.Nama + ")</td></tr>";
            //                }
            //                // keterangan credit card
            //                else if (r.IDPaymentType == 2 && r.IDProgram.HasValue && r.IDBankIssuer.HasValue && r.IDCard.HasValue && r.IDEDC.HasValue)
            //                {
            //                    DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">" + r.PaymentType.Nama + "</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + r.Nominal.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>(" + r.BankIssuer.Nama + " " + r.CardType.Nama + " " + r.ProgramCicilan.Nama + " / EDC : " + r.EDCList.Nama + ")</td></tr>";
            //                }
            //                else
            //                {
            //                    if (LebihBayar_DP_Voucher && pym.Count() == 1)
            //                    {
            //                        DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">" + r.PaymentType.Nama + "</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + d.TotalBayar.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>" + ((r.CCNumber) != null ? "( " + r.CCNumber + " )" : "") + "</td></tr>";
            //                    }
            //                    else if (r.IDPaymentType != 5)
            //                    {

            //                        DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">" + r.PaymentType.Nama + "</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + r.Nominal.Value.ToString("N0") + "</td><td></td></tr>";
            //                    }
            //                }

            //        }

            //        //firman add 27082018 grouping payment voucher pada sales invoice                  
            //        var pymVchr = from pyv in pym where pyv.IDPaymentType == 5 select pyv;


            //        if (pymVchr.Count() > 0)
            //        {
                        
            //            var group_payment_voucher = from gpym in pym                                                   
            //                                        group gpym by new { gpym.IDPaymentType, gpym.Nominal, gpym.CCName } into spym
            //                                        select new
            //                                        {
            //                                            spym.Key.IDPaymentType,
            //                                            spym.Key.CCName,
            //                                            spym.Key.Nominal,
            //                                            Qty = spym.Count(),
            //                                            Total = spym.Key.Nominal * spym.Count()
            //                                        };
            //            foreach (var pymVchrItem in group_payment_voucher)
            //            {
            //                if(pymVchrItem.IDPaymentType == 5)
            //                {
            //                    DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\"> Voucher </td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + pymVchrItem.Total.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>(" + pymVchrItem.Qty.ToString() + "x " + pymVchrItem.Nominal.Value.ToString("N0") + " " + pymVchrItem.CCName.ToString() + ")</td></tr>";
            //                }
            //            }
            //        }



            //    }
            //    DetailPayment += "</table>";
            //    detail_payment.Text = DetailPayment;
            //}

            // Detail Invoice (Kiri Bawah)
			decimal PPNItem = 0m;
            string DetailInvoice = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"noborder\"><tr><td colspan=\"3\"><em>Detail of invoice</em></td></tr>";
            foreach (var r in d.SalesOrderDJ) 
            { 
                DetailInvoice += "<tr><td width=\"300\">" + r.Invoice + "</td></td></tr>";
                //string pageurl = "/OutletSales/Page/Report/Print_InvoiceDJ.aspx?id=" + r.ID;
                //Response.Write("window.open('" + pageurl + "','_blank');");
            }
            foreach (var r in d.SalesOrderGJ) 
            { 
                DetailInvoice += "<tr><td width=\"300\">" + r.Invoice + "</td></td></tr>";
                //string pageurl = "/OutletSales/Page/Report/Print_InvoiceGJ.aspx?id=" + r.ID;
                //Response.Write("window.open('" + pageurl + "','_blank');");
            }
            foreach (var r in d.SalesOrderPG) 
            { 
                DetailInvoice += "<tr><td width=\"300\">" + r.Invoice + "</td></td></tr>";
                //string pageurl = "/OutletSales/Page/Report/Print_InvoicePG.aspx?id=" + r.ID;
                //Response.Write("window.open('" + pageurl + "','_blank');");
            }
            foreach (var r in d.SalesOrderLD) 
            { 
                DetailInvoice += "<tr><td width=\"300\">" + r.Invoice + "</td></td></tr>";
                //string pageurl = "/OutletSales/Page/Report/Print_InvoiceLD.aspx?id=" + r.ID;
                //Response.Write("window.open('" + pageurl + "','_blank');");
            }
            DetailInvoice += "</table>";
            detail_invoice.Text = DetailInvoice;

            if (d.DataSales.CompanyBrand.NamaKode == "F")
                frank_term.Visible = true;
            else if (d.DataSales.CompanyBrand.NamaKode == "M")
                mondial.Visible = true;
            else if (d.DataSales.CompanyBrand.NamaKode == "S")
                missmondial.Visible = true;

            string Header = "";
            string Item = "";
            int index = 1;
            decimal DP = 0m;

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
            Header += "<th " + w3 + ">" + "<em>Qty</em>" + "</th>";
            Header += "<th " + w4 + ">" + "<em>Price</em> / Harga" + "</th>";
            Header += "<th " + w5 + ">" + "<em>Discount</em>" + "</th>";
            Header += "<th " + w6 + ">" + "<em>Price</em> / Harga Jual (Rp)" + "</th>";
            Header += "</tr>";
            Header += "</table>";

            headerlist.Text = Header;

            Item += "<table cellspacing=\"0\" cellpadding=\"5\" border=\"0\" class=\"noborder\" style=\"width:1200px;\" >";

            foreach (var r in d.SalesOrderDJ)
            {
                decimal Discount = 0m;
                Discount = r.DiscountNominal.Value
                        + r.DiscountProgramNominal.Value
                        + r.DiscountGift.Value
                        + r.DiscountRound.Value
                        + (r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0m)
                        + r.DiscountBank
                        + r.DiscountOther
                        ;
                if (index == 1)
                {
                    if (py.Count() > 0)
                    {
                        DP = d.Paid.Value - py.Sum(p => p.Nominal.Value);
                    }
                    index++;
                }
                else DP = 0m;

                Item += "<tr>";
                Item += "<td " + w1 + ">" + r.StockProductDJ.Nomor + "</td>";
                Item += "<td " + w2 + ">" + Str.CapUpper(r.StockProductDJ.StockProductDJ_CharProduct.CharProductItem.Nama) + "</td>";
                Item += "<td " + w3 + ">" + Str.Number0(1) + "</td>";
                Item += "<td " + w4 + ">" + Str.Number0(r.HargaRupiah.Value) + "</td>";
                Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                Item += "<td " + w6 + ">" + Str.Number0(r.TotalBayar.Value) + "</td>";
                Item += "</tr>";
				
				PPNItem += CountPPNItem(IsUsePPN, Convert.ToDateTime(r.SalesOrder.Tgl), Convert.ToDecimal(r.TotalBayar), 1, r.IDProduct);

            }
            foreach (var r in d.SalesOrderGJ)
            {
                decimal Discount = 0m;
                Discount = r.DiscountNominal.Value
                        + r.DiscountProgramNominal.Value
                        + r.DiscountGift.Value
                        + r.DiscountRound.Value
                        + (r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0m)
                        + r.DiscountBank
                        + r.DiscountOther
                        ; 
                if (index == 1)
                {
                    if (py.Count() > 0)
                    {
                        DP = d.Paid.Value - py.Sum(p => p.Nominal.Value);
                    }
                    index++;
                }
                else DP = 0m;

                Item += "<tr>";
                Item += "<td " + w1 + ">" + r.StockProductGJ.Nomor + "</td>";
                Item += "<td " + w2 + ">" + Str.CapUpper(r.StockProductGJ.StockProductGJ_CharProduct.CharProductItem.Nama) + "</td>";
                Item += "<td " + w3 + ">" + Str.Number0(1) + "</td>";
                Item += "<td " + w4 + ">" + Str.Number0(r.HargaRupiah.Value) + "</td>";
                Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                Item += "<td " + w6 + ">" + Str.Number0(r.TotalBayar.Value) + "</td>";
                Item += "</tr>";
				
				PPNItem += CountPPNItem(IsUsePPN, Convert.ToDateTime(r.SalesOrder.Tgl), Convert.ToDecimal(r.TotalBayar), 3, r.IDProduct);
            }
            foreach (var r in d.SalesOrderPG)
            {
                decimal Discount = 0m;
                Discount = r.DiscountNominal.Value 
                        + r.DiscountProgramNominal.Value 
                        + r.DiscountGift.Value 
                        + r.DiscountRound.Value
                        + (r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0m)
                        + r.DiscountBank
                        + r.DiscountOther
                        ;
                if (index == 1)
                {
                    if (py.Count() > 0)
                    {
                        DP = d.Paid.Value - py.Sum(p => p.Nominal.Value);
                    }
                    index++;
                }
                else DP = 0m;

                Item += "<tr>";
                Item += "<td " + w1 + ">" + r.StockProductPG.Nomor + "</td>";
                Item += "<td " + w2 + ">" + Str.CapUpper(r.StockProductPG.CharProductItem.Nama) + " BERAT " + r.StockProductPG.GrossWeight + " KADAR " + Math.Floor(r.StockProductPG.KadarLogam.Value).ToString("N0") + "% WARNA " + Str.CapUpper(r.StockProductPG.CharFrameColor.Nama) + "</td>";
                Item += "<td " + w3 + ">" + Str.Number0(1) + "</td>";
                Item += "<td " + w4 + ">" + Str.Number0(r.HargaRupiah.Value) + "</td>";
                Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                Item += "<td " + w6 + ">" + Str.Number0(r.TotalBayar.Value) + "</td>";
                Item += "</tr>";
				
				PPNItem += CountPPNItem(IsUsePPN, Convert.ToDateTime(r.SalesOrder.Tgl), Convert.ToDecimal(r.TotalBayar), 2, r.IDProduct);
            }
            foreach (var r in d.SalesOrderLD)
            {
                decimal Discount = 0m;
                Discount = r.DiscountNominal.Value
                        + r.DiscountProgramNominal.Value
                        + r.DiscountGift.Value
                        + r.DiscountRound.Value
                        + (r.DiscountPromo.HasValue ? r.DiscountPromo.Value : 0m)
                        + r.DiscountBank
                        + r.DiscountOther
                        ;
                if (index == 1)
                {
                    if (py.Count() > 0)
                    {
                        DP = d.Paid.Value - py.Sum(p => p.Nominal.Value);
                    }
                    index++;
                }
                else DP = 0m;

                Item += "<tr>";
                Item += "<td " + w1 + ">" + r.StockProductLD_Stone1B.Nomor + "</td>";
                Item += "<td " + w2 + ">" + Str.Number3(r.StockProductLD_Stone1B.TotalCarat) + " Carat " + "</td>";
                Item += "<td " + w3 + ">" + Str.Number0(1) + "</td>";
                Item += "<td " + w4 + ">" + Str.Number0(r.HargaRupiah.Value) + "</td>";
                Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                Item += "<td " + w6 + ">" + Str.Number0(r.TotalBayar.Value) + "</td>";
                Item += "</tr>";
				
				PPNItem += CountPPNItem(IsUsePPN, Convert.ToDateTime(r.SalesOrder.Tgl), Convert.ToDecimal(r.TotalBayar), 4, r.IDProduct);
            }
            foreach (var r in d.SalesOrderPackaging)
            {
                decimal Discount = 0m;
                if (index == 1)
                {
                    if (py.Count() > 0)
                    {
                        DP = d.Paid.Value - py.Sum(p => p.Nominal.Value);
                    }
                    index++;
                }
                else DP = 0m;

                Item += "<tr>";
                Item += "<td " + w1 + ">" + r.Packaging.Kode + "</td>";
                Item += "<td " + w2 + ">" + Str.CapUpper(r.Packaging.Nama) + "</td>";
                Item += "<td " + w3 + ">" + Str.Number0(r.Qty) + "</td>";
                Item += "<td " + w4 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                Item += "<td " + w6 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                Item += "</tr>";

            }
            foreach (var r in d.SalesOrderSouvenir)
            {
                if (r.TipeProduct == 0 || r.TipeProduct == null)
                {
                    decimal Discount = 0m;
                    if (index == 1)
                    {
                        if (py.Count() > 0)
                        {
                            DP = d.Paid.Value - py.Sum(p => p.Nominal.Value);
                        }
                        index++;
                    }
                    else DP = 0m;

                    Item += "<tr>";
                    Item += "<td " + w1 + ">" + r.Souvenir.Kode + "</td>";
                    Item += "<td " + w2 + ">" + Str.CapUpper(r.Souvenir.Nama) + "</td>";
                    Item += "<td " + w3 + ">" + Str.Number0(r.Qty) + "</td>";
                    Item += "<td " + w4 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                    Item += "<td " + w6 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "</tr>";

                }
                else if (r.TipeProduct == 1)
                {
                    /*var rs = from p in db.SalesOrderSouvenir
                             join q in db.StockProductDJ on p.IDProductSouvenir equals q.ID
                             where p.IDForm == myID && p.TipeProduct == 1
                             select new { p, q };*/
                    var bs = db.StockProductDJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);

                    decimal Discount = 0m;
                    if (index == 1 && d.StatusPembayaran == false)
                    {
                        //DP = d.Paid.Value - py.Sum(p => p.TotalBayar.Value);
                        index++;
                    }
                    else DP = 0m;

                    Item += "<tr>";
                    Item += "<td " + w1 + ">" + bs.Nomor + "</td>";
                    Item += "<td " + w2 + ">" + Str.CapUpper(bs.StockProductDJ_CharProduct.CharProductItem.Nama) + "</td>";
                    Item += "<td " + w3 + ">" + Str.Number0(r.Qty) + "</td>";
                    Item += "<td " + w4 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                    Item += "<td " + w6 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "</tr>";

                }

                else if (r.TipeProduct == 2)
                {
                    var bs = db.StockProductGJ.SingleOrDefault(p => p.ID == r.IDProductSouvenir);

                    decimal Discount = 0m;
                    if (index == 1)
                    {
                        //DP = d.Paid.Value - py.Sum(p => p.TotalBayar.Value);
                        index++;
                    }
                    else DP = 0m;

                    Item += "<tr>";
                    Item += "<td " + w1 + ">" + bs.Nomor + "</td>";
                    Item += "<td " + w2 + ">" + Str.CapUpper(bs.StockProductGJ_CharProduct.CharProductItem.Nama) + "</td>";
                    Item += "<td " + w3 + ">" + Str.Number0(r.Qty) + "</td>";
                    Item += "<td " + w4 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                    Item += "<td " + w6 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "</tr>";

                }
                else if (r.TipeProduct == 3)
                {
                    var bs = db.StockProductPG.SingleOrDefault(p => p.ID == r.IDProductSouvenir);

                    decimal Discount = 0m;
                    if (index == 1)
                    {
                        //DP = d.Paid.Value - py.Sum(p => p.TotalBayar.Value);
                        index++;
                    }
                    else DP = 0m;

                    Item += "<tr>";
                    Item += "<td " + w1 + ">" + bs.Nomor + "</td>";
                    Item += "<td " + w2 + ">" + Str.CapUpper(bs.CharProductItem.Nama) + "</td>";
                    Item += "<td " + w3 + ">" + Str.Number0(r.Qty) + "</td>";
                    Item += "<td " + w4 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                    Item += "<td " + w6 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "</tr>";

                }

                else if (r.TipeProduct == 4)
                {
                    var bs = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == r.IDProductSouvenir);
                    decimal Discount = 0m;
                    if (index == 1)
                    {
                        //DP = d.Paid.Value - py.Sum(p => p.TotalBayar.Value);
                        index++;
                    }
                    else DP = 0m;

                    Item += "<tr>";
                    Item += "<td " + w1 + ">" + bs.Nomor + "</td>";
                    Item += "<td " + w2 + ">" + " - " + "</td>";
                    Item += "<td " + w3 + ">" + Str.Number0(r.Qty) + "</td>";
                    Item += "<td " + w4 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "<td " + w5 + ">" + Str.Number0(Discount) + "</td>";
                    Item += "<td " + w6 + ">" + Str.Number0(r.TotalRupiah.Value) + "</td>";
                    Item += "</tr>";

                }
            }
            Item += "</table>";
            itemlist.Text = Item;
			
			TotalDP = Convert.ToDecimal((from p in db.SalesReceiptDetail where p.SalesReceipt.IDSalesOrder == d.ID select p.Nominal).Sum());
            Sisa = Nominal - TotalDP;
			
			PPN = PPNItem; 
			DPP = d.TotalBayar.Value - PPNItem;

            dpp.Text = DPP.ToString("N0");
            ppn.Text = PPN.ToString("N0");
            nominal.Text = Nominal.ToString("N0");
            totaldp.Text = TotalDP.ToString("N0");
            sisa.Text = Sisa.ToString("N0");

            //adding firman 280818 
            totaldp.Text = d.TotalBayar.Value.ToString("N0");
            sisa.Text = 0.ToString("N0");
            terbilang.Text = Str.Terbilang(d.TotalBayar.Value);

            detailPaymentControl.SetDetailPayment("SO", d.ID, Convert.ToDecimal(d.Unpaid.Value), Convert.ToDecimal(d.TotalBayar.Value));
        }
    }
	
	protected decimal CountPPNItem(bool IsUsePPN, DateTime tgl, decimal TotalBuyAre, int tipeproduct, int idproduct)
	{
		decimal ppn = 0m;
		decimal rateppn = 0m;
		if(IsUsePPN)
		{
			if(tipeproduct == 1) //DJ
			{
				if(tgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
				{
					rateppn = 1.02m;
				}
				else if(tgl <= Convert.ToDateTime("2023-04-30 23:59:00.000"))
				{
					rateppn = 1.022m;
				}
				else
				{
					string query = "SELECT CAST(CASE WHEN C.Keterangan LIKE '%In%' THEN 1 WHEN C.Keterangan LIKE '%Out%' THEN 0 ELSE 1 END AS BIT) [IsInSource] FROM StockProductDJ A LEFT JOIN DataSupplier B ON A.Supplier = B.ID LEFT JOIN DataSupplierGroup C ON B.IDGroup = C.ID WHERE A.ID = " + idproduct;
					bool isinsource = Db.SingleBool(query,Db.Cnn_DB);
					if(isinsource)
					{
						rateppn = 1.0165m;
					}
					else
					{
						rateppn = 1.011m;
					}
				}
			}
			else if(tipeproduct == 2) //PG
			{
				if(tgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
				{
					rateppn = 1.02m;
				}
				else if(tgl <= Convert.ToDateTime("2023-04-30 23:59:00.000"))
				{
					rateppn = 1.022m;
				}
				else
				{
					string query = "SELECT CAST(CASE WHEN C.Keterangan LIKE '%In%' THEN 1 WHEN C.Keterangan LIKE '%Out%' THEN 0 ELSE 1 END AS BIT) [IsInSource] FROM StockProductPG A LEFT JOIN DataSupplier B ON A.IDSupplier = B.ID LEFT JOIN DataSupplierGroup C ON B.IDGroup = C.ID WHERE A.ID = " + idproduct;
					bool isinsource = Db.SingleBool(query,Db.Cnn_DB);
					if(isinsource)
					{
						rateppn = 1.0165m;
					}
					else
					{
						rateppn = 1.011m;
					}
				}
			}
			else if(tipeproduct == 3) //GJ
			{
				if(tgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
				{
					rateppn = 1.02m;
				}
				else if(tgl <= Convert.ToDateTime("2023-04-30 23:59:00.000"))
				{
					rateppn = 1.022m;
				}
				else
				{
					string query = "SELECT CAST(CASE WHEN C.Keterangan LIKE '%In%' THEN 1 WHEN C.Keterangan LIKE '%Out%' THEN 0 ELSE 1 END AS BIT) [IsInSource] FROM StockProductGJ A LEFT JOIN DataSupplier B ON A.Supplier = B.ID LEFT JOIN DataSupplierGroup C ON B.IDGroup = C.ID WHERE A.ID = " + idproduct;
					bool isinsource = Db.SingleBool(query,Db.Cnn_DB);
					if(isinsource)
					{
						rateppn = 1.0165m;
					}
					else
					{
						rateppn = 1.011m;
					}
				}
			}
			else if(tipeproduct == 4)
			{
				if(tgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
				{
					rateppn = 1.02m;
				}
				else if(tgl <= Convert.ToDateTime("2023-04-30 23:59:00.000"))
				{
					rateppn = 1.02m;
				}
				else
				{
					rateppn = 1.011m;
				}
			}
			ppn = (TotalBuyAre) - (TotalBuyAre / rateppn);
		}
		return ppn;
	}
}