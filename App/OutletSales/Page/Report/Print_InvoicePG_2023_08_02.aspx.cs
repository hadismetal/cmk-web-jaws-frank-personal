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
        App.Protect();
        App.ProtectRole(Role.Operator);

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
            Upload.Image(imgpicture, d.StockProductPG.ImgPicture);

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
            decimal DebitLakuEmasNominal = 0m;
            var py = from p in db.SalesReceiptDetail
                     where p.SalesReceipt.IDSalesOrder == d.IDForm
                     orderby p.OperatorTgl descending
                     select p;
            /*foreach (var i in py)
            {
                if(i.IDPaymentType == 11)
                {
                    LakuEmasNominal = LakuEmasNominal + Convert.ToDecimal(i.Nominal);
                }
            }*/
            if (py.Count(p => p.IDPaymentType == 11) > 0)
            {
                //le.Text = LakuEmasNominal.ToString("N0");
                //paymentwithlakuemaspanel.Visible = true;
            }
            /*else if(py.Count(p => p.IDPaymentType == 4 && p.IDEDC == 24) > 0)
			{
				decimal weight_pg = 0m;
				decimal weight_lm = 0m;
				decimal rate_lm = 0m;
				foreach (var j in py){
					if(j.IDPaymentType == 4 && j.IDEDC == 24 ){
						rate_lm = Db.SingleDecimal("SELECT TOP 1 ISNULL(RateLM,0) FROM SalesReceiptDetailDebitLakuEmas WHERE IDSalesReceiptDetail = "+j.ID+" AND RateLM IS NOT NULL ", Db.Cnn_DB);
					}
				}
				
				decimal harga_pg = 0m;
				decimal harga_lm = 0m;
				decimal ongkos = 0m;
				weight_pg += d.StockProductPG.NettoWeight ?? 0m;
				weight_lm = (3 * weight_pg) / 4;
				harga_lm = weight_lm * rate_lm;
				harga_pg = d.SalesOrder.TotalBayar ?? 0m;
				ongkos = harga_pg - harga_lm;
				DebitLakuEmasNominal = harga_lm;
				
				debitlepg.Text = (harga_pg).ToString("N0");
				debitlelm.Text = (harga_lm).ToString("N0");
				paymentwithdebitlakuemaspanel.Visible = true;
			}*/
            else
            {
                le.Text = (0m).ToString("N0");
                paymentwithlakuemaspanel.Visible = false;
                paymentwithdebitlakuemaspanel.Visible = false;
            }

            lit_ppn.Text = "2,2 %";
            lit_ppn2.Text = "2,2 %";
            lit_ppn3.Text = "2,2 %";
            lit_ppn4.Text = "2,2 %";
            decimal rateppn = 1.022m;
            int ppnunit = 11;
            if (d.SalesOrder.OperatorTgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
            {
                lit_ppn.Text = "2 %";
                lit_ppn2.Text = "2 %";
                lit_ppn3.Text = "2 %";
                lit_ppn4.Text = "2 %";
                rateppn = 1.02m;
                ppnunit = 10;

            }
            else if (d.SalesOrder.Tgl <= Convert.ToDateTime("2023-04-30 23:59:00.000"))
            {
                lit_ppn.Text = "2,2 %";
                lit_ppn2.Text = "2,2 %";
                lit_ppn3.Text = "2,2 %";
                lit_ppn4.Text = "2,2 %";
                rateppn = 1.022m;
                ppnunit = 11;
            }
            else
            {
                if (Db.SingleString("select b.Keterangan from DataSupplier a join DataSupplierGroup b on a.IDGroup = b.ID where a.ID = " + d.StockProductPG.IDSupplier, Db.Cnn_DB).Contains("In"))
                {
                    lit_ppn.Text = "1,65 %";
                    lit_ppn2.Text = "1,65 %";
                    lit_ppn3.Text = "1,65 %";
                    lit_ppn4.Text = "1,65 %";
                    rateppn = 1.0165m;
                    ppnunit = 11;
                }
                else if (Db.SingleString("select b.Keterangan from DataSupplier a join DataSupplierGroup b on a.IDGroup = b.ID where a.ID = " + d.StockProductPG.IDSupplier, Db.Cnn_DB).Contains("Out"))
                {
                    lit_ppn.Text = "1,1 %";
                    lit_ppn2.Text = "1,1 %";
                    lit_ppn3.Text = "1,1 %";
                    lit_ppn4.Text = "1,1 %";
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

            if (!IsUsePPN || d.StockProductPG.CharProductItem.Nama == "LM")
            {
                lit_ppn.Text = "0 %";
                lit_ppn2.Text = "0 %";
                lit_ppn3.Text = "0 %";
                lit_ppn4.Text = "0 %";
                ppnunit = 0;

                lit_ppn_notice.Visible = false;
                lit_ppn_notice2.Visible = false;

            }
            else
            {
                lit_ppn_notice.Visible = true;
                lit_ppn_notice2.Visible = true;

            }

            DPP = (!IsUsePPN || d.StockProductPG.CharProductItem.Nama == "LM") ? (d.TotalBayar.Value - LakuEmasNominal - DebitLakuEmasNominal) : (d.TotalBayar.Value - LakuEmasNominal - DebitLakuEmasNominal) / rateppn;
            PPN = d.TotalBayar.Value - DPP - LakuEmasNominal - DebitLakuEmasNominal;

            try
            {
                Db.Execute("EXEC sp_update_ppn " + myID + ",2,'" + Convert.ToInt32(PPN) + "','" + ppnunit + "'", Db.Cnn_DB);
            }
            catch { }

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
            else
            {
            }

            //adding firman 280818 user control untuk detail payment dari /UC/DetailPaymentControl.aspx
            if (TotalItem == 1)
            {


                detailPaymentControl.SetDetailPayment("PG", d.SalesOrder.ID, Convert.ToDecimal(d.SalesOrder.Unpaid.Value), Convert.ToDecimal(d.SalesOrder.TotalBayar.Value));

            }

            //if (TotalItem == 1)
            //{
            //    var py = from p in db.SalesReceiptDetail
            //             where p.SalesReceipt.IDSalesOrder.Value == d.SalesOrder.ID
            //             select p;

            //    bool LebihBayar_DP_Voucher = false;
            //    bool LebihBayar_Pure_Voucher = false;

            //    totaldp.Text = d.TotalBayar.Value.ToString("N0");
            //    sisa.Text = 0.ToString("N0");

            //    if (d.SalesOrder.Unpaid < 0 && py.Any(p => p.PaymentType.ID == 5 || p.PaymentType.ID == 8))
            //    {
            //        LebihBayar_DP_Voucher = true;
            //        totaldp.Text = d.TotalBayar.Value.ToString("N0");
            //        sisa.Text = 0.ToString("N0");

            //        LebihBayar_Pure_Voucher = !py.Any(p => p.PaymentType.ID != 5);

            //    }

            //    string DetailPayment = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"noborder\"><tr><td colspan=\"4\"><em>Detail of payment</em></td></tr>";

            //    if (LebihBayar_DP_Voucher && LebihBayar_Pure_Voucher)
            //    {
            //        string vKodeVoucher = "";
            //        int count = 0;
            //        foreach (var r in py)
            //        {
            //            if (count > 0) { vKodeVoucher += ", "; }
            //            vKodeVoucher += r.CCNumber;
            //            count++;
            //        }
            //        vKodeVoucher = "(" + vKodeVoucher + ")";
            //        DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">Voucher</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right;'>" + d.TotalBayar.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>" + vKodeVoucher + "</td></tr>";
            //    }
            //    else
            //    {
            //        foreach (var r in py)
            //        {
            //            if (LebihBayar_DP_Voucher && py.Count() == 1)
            //            {
            //                DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">" + r.PaymentType.Nama + "</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right;'>" + d.TotalBayar.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>" + ((r.CCNumber) != null ? "( " + r.CCNumber + " )" : "") + "</td></tr>";
            //            }
            //            else if (r.IDPaymentType != 5)
            //            {
            //                DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\">" + r.PaymentType.Nama + "</td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right;'>" + r.Nominal.Value.ToString("N0") + "</td><td></td></tr>";
            //            }
            //        }
            //    }

            //    //firman add 27082018 grouping payment voucher pada print invoice
            //    var pymVchr = from pyv in py where pyv.IDPaymentType == 5 select pyv;

            //    if (pymVchr.Count() > 0)
            //    {

            //        var group_payment_voucher = from gpym in py
            //                                    group gpym by new { gpym.IDPaymentType, gpym.Nominal, gpym.CCName } into spym
            //                                    select new
            //                                    {
            //                                        spym.Key.IDPaymentType,
            //                                        spym.Key.CCName,
            //                                        spym.Key.Nominal,
            //                                        Qty = spym.Count(),
            //                                        Total = spym.Key.Nominal * spym.Count()
            //                                    };

            //        foreach (var pymVchrItem in group_payment_voucher)
            //        {
            //            if (pymVchrItem.IDPaymentType == 5)
            //            {
            //                DetailPayment += "<tr><td style='vertical-align:top;' width=\"120\"> Voucher </td><td style='vertical-align:top;' width=\"20\">:</td><td style='vertical-align:top; text-align:right; padding-left:8px;'>" + pymVchrItem.Total.Value.ToString("N0") + "</td><td style='vertical-align:top; text-align:left; padding-left:8px;'>(" + pymVchrItem.Qty.ToString() + "x " + pymVchrItem.Nominal.Value.ToString("N0") + " " + pymVchrItem.CCName.ToString() + ")</td></tr>";
            //            }
            //        }
            //    }

            //    DetailPayment += "</table>";
            //    detail_payment.Text = DetailPayment;
            //}

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
                string query = "exec sp_get_asp_new '2','" + d.StockProductPG.ID + "'";
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
    }
}