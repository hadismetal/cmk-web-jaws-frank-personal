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
                if (i.IDPaymentType == 11)
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
            if (d.SalesOrder.Tgl <= Convert.ToDateTime("2022-03-31 23:59:00.000"))
            {
                rateppn = 1.02m;
                ppnunit = 10;
                lblppn.InnerText = "10%";
            }

            if (KodeLokasi == "E46" || KodeLokasi == "P18" || d.StockProductPG.CharProductItem.Nama == "LM")
            {
                ppnunit = 0;
            }

            DPP = (KodeLokasi == "E46" || KodeLokasi == "P18" || d.StockProductPG.CharProductItem.Nama == "LM") ? (d.TotalBayar.Value - LakuEmasNominal) : (d.TotalBayar.Value - LakuEmasNominal) / rateppn;
            PPN = d.TotalBayar.Value - DPP - LakuEmasNominal;

            try
            {
                Db.Execute("EXEC sp_update_ppn " + myID + ",2,'" + Convert.ToInt32(PPN) + "','" + ppnunit + "'", Db.Cnn_DB);
            }
            catch { }

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

            if (d.StockProductPG.CharProductItem.Nama == "LM")
            {
                ppnpanel.Visible = false;
                pphpanel.Visible = true;
                labelpajak.InnerText = " PPH";

                decimal pph1 = Db.SingleDecimal("SELECT TOP 1 CASE WHEN ((SELECT TOP 1 ISNULL(NPWP,'') [NPWP] FROM SalesOrder WHERE ID = " + d.IDForm + ") = '') THEN CAST((ISNULL(TotalBayar,0) / 1.009 * 0.009) AS DECIMAL(36,2)) ELSE CAST((ISNULL(TotalBayar,0) / 1.0045 * 0.0045) AS DECIMAL(36,2)) END [Pph22] FROM SalesOrderPG WHERE ID = " + d.ID, Db.Cnn_DB);

                dpp.Text = (d.TotalBayar.Value - pph1).ToString("N0");
                pph.Text = pph1.ToString("N0");
            }
            else
            {
                labelpajak.InnerText = " PPN";
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
            Item += "<td " + w2 + ">" + Str.CapUpper(d.StockProductPG.CharProductItem.Nama) + (!string.IsNullOrEmpty(collectionname) ? " " + collectionname : "") + " BERAT " + Convert.ToDecimal(d.StockProductPG.GrossWeight).ToString("N2") + " KADAR " + Math.Floor(d.StockProductPG.KadarLogam.Value).ToString("N0") + "% WARNA " + Str.CapUpper(d.StockProductPG.CharFrameColor.Nama) + "</td>";
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
                string[] ASP = new string[10];
                ASP = DLC.getASPPGParameter();

                if (d.SalesOrder.DataCustomer.CompanyBrand.NamaKode == "F"
                    || d.SalesOrder.DataSales.CompanyBrand.NamaKode == "M"
                    || d.SalesOrder.DataSales.CompanyBrand.NamaKode == "S"
                    || d.SalesOrder.DataSales.CompanyBrand.NamaKode == "P"
                    )
                {
                    asp.Visible = false;
                    asp2.Visible = false;

                    // catatan setelah kotak ASP
                    if (d.SalesOrder.DataSales.CompanyBrand.NamaKode == "P") { thepalace.Visible = true; }
                    else { general.Visible = true; }

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
                        /*t1.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[1]) / 12)).ToString();
						t2.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[1]) / 12)).ToString();*/
                        t1.Text = ASP[1];
                        t2.Text = ASP[1];

                        //me1.Text = d.MEPeriodeMax.Value.ToString();
                        //me2.Text = d.MEPeriodeMax.Value.ToString();
                        //me_rumus1.Text = d.MERumus.Value.ToString("N2");
                        ///*t1.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[1]) / 12)).ToString();
                        //t2.Text = Math.Floor(Convert.ToDecimal(Convert.ToInt32(ASP[1]) / 12)).ToString();*/
                        //t1.Text = d.MEPeriodeMax.Value.ToString();
                        //t2.Text = d.MEPeriodeMax.Value.ToString();
                    }
                }
            }
        }
    }
}