using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Text;

public partial class Page_Report_SPK_PrintTest : System.Web.UI.Page
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
    protected void DataError() { Response.Redirect("ProductOrderList.aspx"); }

    //Form
    protected void fillForm()
    {
        decimal harga = (Convert.ToDecimal((from p in db.ProductOrder_PricingMU where p.ID == myID select p.HargaRupiahRound).SingleOrDefault()));
        var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            /// Header
            /// LOGO
            string CB = ""; /// mondial, franknco, missmondial, thepalace
            CB = d.CompanyBrand.Nama.ToLower();

            if (CB == "wholesale") { img_Logo.ImageUrl = "/Images/spk/logo_spk_wholesale.jpg"; }
            else if (CB == "mondial") { img_Logo.ImageUrl = "/Images/spk/logo_spk_mondial.jpg"; }
            else if (CB == "miss mondial") { img_Logo.ImageUrl = "/Images/spk/logo_spk_missmondial.jpg"; }
            else if (CB == "the palace") { img_Logo.ImageUrl = "/Images/spk/logo_spk_thepalace.jpg"; }
            else if (CB == "frank & co") { img_Logo.ImageUrl = "/Images/spk/logo_spk_franknco.jpg"; }
            else { img_Logo.ImageUrl = "/Images/spk/logo_spk_franknco.jpg"; }

            string kelas_harga = "";
            if (d.ProductOrder_CharProduct.CharStoneDist.NamaKode == "SOL"
                && (d.ProductOrder_CharProduct.CharProductItem.NamaKode.Equals("A")
                    || d.ProductOrder_CharProduct.CharProductItem.NamaKode.Equals("L")
                    || d.ProductOrder_CharProduct.CharProductItem.NamaKode.Equals("M")
                    || d.ProductOrder_CharProduct.CharProductItem.NamaKode.Equals("W")
                ))
            {
                // cetak kelas harga SOLITAIRE jika masuk diantara 4 item : 
                /*
                52	Earrings	A
                63	Pendant	L
                64	Wedding Ring	M
                73	Ladies Ring	W
                */

                kelas_harga = "SOLITAIRE";
            }
            else
            {
                if (harga <= 3000000)
                {
                    kelas_harga = "INTRO";
                }
                else if ((harga > 3000000) && (harga <= 10000000))
                {
                    kelas_harga = "LOW END";
                }
                else if ((harga > 10000000) && (harga <= 30000000))
                {
                    kelas_harga = "MIDDLE END";
                }
                else if ((harga > 30000000) && (harga <= 100000000))
                {
                    kelas_harga = "HIGH END";
                }
                else if ((harga > 100000000) && (harga <= 300000000))
                {
                    kelas_harga = "TOP END";
                }
                else if (harga > 300000000)
                {
                    kelas_harga = "SUPER END";
                }
            }

            if (CB == "wholesale") { kelas_harga = ""; } // untuk sementara di hiding dulu utk broker

            lbc1.Text = kelas_harga;

            lit_NamaCustomer.Text = d.RequestCustomerNama;
            lit_NomorLDKP.Text = d.NoLDKP;
            string namatoko = string.Empty;
            if (d.RequestOutlet > 0)
            {
                namatoko = d.RequestOutletNama;
                if ((namatoko.Length) > 28)
                {
                    namatoko = namatoko.Substring(0,28);
                }
            }
            else if (d.RequestExhibition > 0)
            {
                namatoko = d.RequestExhibitionNama;
                string cutnama = "";
                if ((namatoko.Length) > 28)
                {
                    namatoko = namatoko.Substring(0, 28);
                } 
            }

            lit_NamaToko.Text = namatoko;
                
            lit_NamaKoleksi.Text = d.ProductOrder_CharDesign.Naming;

            if (d.RequestCustomer > 0)
            {
                var u = db.DataCustomer.SingleOrDefault(p => p.ID == d.RequestCustomer);
                lit_NomorCustomer.Text = u.NoCustomer;
            }
            else
                if (d.NoPO != null)
            {
                var cust = (from p in db.ProductOrder
                            join p1 in db.SalesReceiptDPPO on p.NoPO equals p1.NomorDP
                            join p2 in db.DataCustomer on p1.IDCustomer equals p2.ID
                            select p2).FirstOrDefault();
                lit_NomorCustomer.Text = cust.NoCustomer;
            }

            lit_StatusBarang.Text = d.StatusBarang;
            lit_StatusBarangKet.Text = d.StatusBarangKeterangan.Trim() != "" ?  ("( " + d.StatusBarangKeterangan + " )") : "";

            lit_NomorSPK.Text = d.NoSPK != null ? d.NoSPK : "";

            //var myFonts = new PrivateFontCollection();
            //myFonts.AddFontFile(@"C:\path\to\BLACKR.TTF");
            //myFonts.AddFontFile(@"D:\App\jaws\App\ProductOrder\Font\free3of9.ttf");
            //var oFont = new Font(myFonts.Families[0], 20);

            string MoneyImg = "*" + d.NoSPK + "*";
            //Bitmap bitmap = new Bitmap(MoneyImg.Length * 40, 150);

            //using (Graphics graphics = Graphics.FromImage(bitmap))
            //{
            //    //Font oFont = new System.Drawing.Font("BLACKR", 20);
            //    PointF point = new PointF(2f, 2f);
            //    SolidBrush black = new SolidBrush(Color.Black);
            //    SolidBrush white = new SolidBrush(Color.White);
            //    graphics.FillRectangle(white, 0, 0, bitmap.Width, bitmap.Height);
            //    graphics.DrawString("$" + MoneyImg, oFont, black, point);
            //}

            //lit_NomorSPK.Text += "<br />" + MoneyImg;
            //MoneyImg = "*123123123*";
            //lit_SPKbarcode.Text = MoneyImg;
            lit_SPKbarcode2.Text = MoneyImg;

            //// 'PrivateFontCollection' is in the 'System.Drawing.Text' namespace
            //var foo = new PrivateFontCollection();
            //// Provide the path to the font on the filesystem
            //foo.AddFontFile("...");

            //var myCustomFont = new Font((FontFamily)foo.Families[0], 36f);

            lit_TanggalSPK.Text = d.Tgl != null ? Str.Tgl(d.Tgl) : "";
            lit_TanggalPersetujuanSPK.Text = ""; // d.ApprovalTgl != null ? Str.Tgl(d.ApprovalTgl) : "";
            lit_TanggalJatuhTempo.Text = (d.TanggalJatuhTempoDJ != null ? Str.Tgl(d.TanggalJatuhTempoDJ) : "");
            lit_NomorInvoiceDP.Text = d.NoInvoice;
            //lit_BebanSusut.Text = Convert.ToDecimal(d.ProductOrder_CharProduct.BebanSusut).ToString("N2");

            decimal bs = 0;
            if (d.ProductOrder_CharProduct.BebanSusut != null)
            {
                bs = Convert.ToDecimal(d.ProductOrder_CharProduct.BebanSusut.ToString());
            }
            if (bs == 0)
            {
                lit_BebanSusut.Text = "";
            }
            else if (bs != 0)
            {
                lit_BebanSusut.Text = bs.ToString("N2");
            }

            lit_Berat.Text = d.ProductOrder_CharProduct.GrossWeight.ToString("N2");
            lit_NomorPO.Text = d.NoPO;

            if ((d.NoPO != null) && (d.NoPO != ""))
            {
                var dp = db.SalesReceiptDPPO.SingleOrDefault(p => p.NomorDP == d.NoPO && p.DataCustomer.ID == d.RequestCustomer);
                if (dp != null)
                {
                    lit_namajp.InnerText = dp.SalesNama.ToString();
                }
            }
            else
            {
                lit_namajp.InnerText = "";
            }
            lit_StatusOrder.Text = (d.StatusOrder == 1) ? "New Order" : "Repeat Order";
            //lit_Komponen.Text = Convert.ToDecimal(d.ProductOrder_CharProduct.Komponen).ToString("N2");

            decimal kom = 0;
            if (d.ProductOrder_CharProduct.Komponen != null)
            {
                kom = Convert.ToDecimal(d.ProductOrder_CharProduct.Komponen.ToString());
            }
            if (d.StatusOrder == 1)
            {
                if (kom > 0 || kom < 0)
                {
                    lit_Komponen.Text = Str.Number2(kom);
                }
                else
                {
                    lit_Komponen.Text = " ";
                }
            }
            else if (d.StatusOrder == 2)
            {
                //lit_Komponen.Text = Convert.ToDecimal(d.ProductOrder_CharProduct.Komponen).ToString("N2");
                if (kom > 0 || kom < 0)
                {
                    lit_Komponen.Text = Str.Number2(kom);
                }
                else
                {
                    lit_Komponen.Text = " ";
                }
            }

            lit_TingkatKesulitan.Text = d.ProductOrder_CharProduct.CharProcessCons.Nama;
            //lit_PoinKomponen.Text = Convert.ToDecimal(d.ProductOrder_CharProduct.PoinKomponen).ToString("N2");
            decimal poi = 0;
            if (d.ProductOrder_CharProduct.PoinKomponen != null)
            {
                poi = Convert.ToDecimal(d.ProductOrder_CharProduct.PoinKomponen);
            }
            if (poi == 0)
            {
                lit_PoinKomponen.Text = "";
            }
            else if (poi != 0)
            {
                lit_PoinKomponen.Text = Str.Number2(poi);
            }

            productcategory.Text = d.ProductOrder_CharProduct.CharProductCategory.Nama;
            lit_Material.Text = d.ProductOrder_CharProduct.CharFrameMaterial.Nama;

            var kode = db.KodeKaret_Detail.SingleOrDefault(p => p.IDProductOrder == d.ID);

            string kodeKaretJAWS = "";
            string kodeKaretLama = "";

            try
            {
                kodeKaretJAWS = kode.KodeKaretDetail;
            }
            catch (Exception ex)
            {

            }
            try
            {
                kodeKaretLama = kode.KodeKaretDJ.KodeKaretLama + ((d.KodeKaretRepeat != null && d.KodeKaretRepeat != 0) ? ("/" + d.KodeKaretRepeat.ToString().PadLeft(4, '0')) : "");
            }
            catch (Exception ex)
            {

            }

            lit_KodeKaret.Text = "<div style='padding-bottom: 3px;'>" + kodeKaretJAWS + "</div>"
                               + ((kodeKaretLama != "") ? ("<div style='padding-top: 3px; border-top:solid 1px black;'>" + kodeKaretLama + "</div>") : "")
                               ;

            lit_Jenis.Text = d.ProductOrder_CharProduct.CharProductItem.Nama
                            + ((d.CompanyBrand.NamaKode == "W") ? ("<br />(" + d.ProductOrder_CharProduct.CharStoneDist.Nama + ")") : "")
                            ;
            lit_Jumlah.Text = (d.ProductOrder_CharProduct.CharProductItem.NamaKode == "A") ? "1 Psg" : "1"; /// selalu 1 jumlah barangnya (Qty)
            lit_RingSize.Text = d.ProductOrder_CharProduct.DimensiR != null ? Convert.ToDecimal(d.ProductOrder_CharProduct.DimensiR).ToString("N2") : "0,00";
            lit_RingSize2.Text = d.ProductOrder_CharProduct.DimensiR2 != null ? Convert.ToDecimal(d.ProductOrder_CharProduct.DimensiR2).ToString("N2") : "0,00";

            lit_Panjang.Text = "0.00";
            lit_Lebar.Text = "0.00";
            lit_Diameter.Text = "0.00";

            if (d.ProductOrder_CharProduct.CharProductItem.NamaKode == "E")
            {
                // Bangle
                string xP = d.ProductOrder_CharProduct.DimensiP != null ? d.ProductOrder_CharProduct.DimensiP.ToString("0.00") : "0.00";
                string xL = d.ProductOrder_CharProduct.DimensiL != null ? d.ProductOrder_CharProduct.DimensiL.ToString("0.00") : "0.00";

                lit_Diameter.Text = xP + " x " + xL;

            }
            else
            {
                lit_Panjang.Text = d.ProductOrder_CharProduct.DimensiP != null ? d.ProductOrder_CharProduct.DimensiP.ToString("0.00") : "0.00";
                lit_Lebar.Text = d.ProductOrder_CharProduct.DimensiL != null ? d.ProductOrder_CharProduct.DimensiL.ToString("0.00") : "0.00";
                lit_Diameter.Text = d.ProductOrder_CharProduct.DimensiD != null ? d.ProductOrder_CharProduct.DimensiD.ToString("0.00") : "0.00";

            }

            //lit_Berat.Text = rs_Berat.GrossWeight != null ? rs_Berat.GrossWeight.ToString("0.000") : "";
            lit_WarnaEmas.Text = (d.ProductOrder_CharProduct.CharFrameColor.Nama).Length > 15
                               ? "<span style='font-size:10px;'>" + (d.ProductOrder_CharProduct.CharFrameColor.Nama).Replace(" ", "<br />") + "</span>"
                               : (d.ProductOrder_CharProduct.CharFrameColor.Nama).Replace(" ", "<br />")
                               ;
            //lit_NomorCustomer.Text = d.RequestCustomer.ToString();
            lit_NamaCustomer.Text = (d.RequestCustomerNama != null) ? d.RequestCustomerNama : "";
            //lit_KelasHarga.Text = d.CharProductSegmen.Nama;

            /// List Parcel
            lit_Parcel.Text = "";

            decimal Jumlah_Butir = 0m;
            decimal Jumlah_CaratPerButir = 0m;
            decimal Jumlah_Carat = 0m;

            decimal Total_Butir = 0m;
            decimal Total_CaratPerButir = 0m;
            decimal Total_Carat = 0m;

            var rs_Parcel_1A = from p in db.ProductOrder_Stone1A
                               where p.IDForm == d.ID
                               select p;

            var rs_Parcel_1B = from p in db.ProductOrder_Stone1B
                               where p.IDForm == d.ID
                               select p;

            var rs_Parcel_2 = from p in db.ProductOrder_Stone2
                              where p.IDForm == d.ID
                              select p;

            var rs_Parcel_3 = from p in db.ProductOrder_Stone3
                              where p.IDForm == d.ID
                              select p;

            var rs_Parcel_4 = from p in db.ProductOrder_Stone4
                              where p.IDForm == d.ID
                              select p;

            var rs_Parcel_5 = from p in db.ProductOrder_Stone5
                              where p.IDForm == d.ID
                              select p;



            var stone = (
                    from p in db.ProductOrder_Stone1B
                    where p.IDForm == d.ID
                    select p).OrderByDescending(x => x.TotalCarat).FirstOrDefault();

            if (stone != null)
                lit_BrandBatu.Text = stone.Stone1B.Parcel106.Nama;

            if (rs_Parcel_1A.Count() > 0)
            {
                foreach (var rsp in rs_Parcel_1A)
                {
                    Jumlah_Butir = rsp.TotalButir;
                    Jumlah_CaratPerButir = rsp.CaratButir;
                    Jumlah_Carat = rsp.TotalCarat;

                    lit_Parcel.Text += "<tr>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.Stone1A.Nama + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.TotalButir.ToString("0") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + (rsp.CaratButir).ToString("0.000") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.TotalCarat.ToString("0.000") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.DimensiP.ToString("N3") + "</td>";
                    lit_Parcel.Text += "</tr>";

                    Total_Butir += Jumlah_Butir;
                    Total_CaratPerButir += Jumlah_CaratPerButir;
                    Total_Carat += Jumlah_Carat;
                }
            }

            if (rs_Parcel_1B.Count() > 0)
            {
                foreach (var rsp in rs_Parcel_1B)
                {
                    Jumlah_Butir = rsp.TotalButir;
                    Jumlah_CaratPerButir = rsp.CaratButir;
                    Jumlah_Carat = rsp.TotalCarat;

                    lit_Parcel.Text += "<tr>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.Stone1B.Nama + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.TotalButir.ToString("0") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + (rsp.CaratButir).ToString("0.000") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.TotalCarat.ToString("0.000") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.DimensiP.ToString("N3") + "</td>";
                    lit_Parcel.Text += "</tr>";

                    Total_Butir += Jumlah_Butir;
                    Total_CaratPerButir += Jumlah_CaratPerButir;
                    Total_Carat += Jumlah_Carat;
                }
            }

            if (rs_Parcel_2.Count() > 0)
            {
                foreach (var rsp in rs_Parcel_2)
                {
                    Jumlah_Butir = rsp.TotalButir;
                    Jumlah_CaratPerButir = rsp.CaratButir;
                    Jumlah_Carat = rsp.TotalCarat;

                    lit_Parcel.Text += "<tr>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.Stone2.Nama + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.TotalButir.ToString("0") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + (rsp.CaratButir).ToString("0.000") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.TotalCarat.ToString("0.000") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.DimensiP.ToString("N3") + "</td>";
                    lit_Parcel.Text += "</tr>";

                    Total_Butir += Jumlah_Butir;
                    Total_CaratPerButir += Jumlah_CaratPerButir;
                    Total_Carat += Jumlah_Carat;
                }
            }

            if (rs_Parcel_3.Count() > 0)
            {
                foreach (var rsp in rs_Parcel_3)
                {
                    Jumlah_Butir = rsp.TotalButir;
                    Jumlah_CaratPerButir = rsp.CaratButir;
                    Jumlah_Carat = rsp.TotalCarat;

                    lit_Parcel.Text += "<tr>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.Stone3.Nama + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.TotalButir.ToString("0") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + (rsp.CaratButir).ToString("0.000") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.TotalCarat.ToString("0.000") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.DimensiP.ToString("N3") + "</td>";
                    lit_Parcel.Text += "</tr>";

                    Total_Butir += Jumlah_Butir;
                    Total_CaratPerButir += Jumlah_CaratPerButir;
                    Total_Carat += Jumlah_Carat;
                }
            }

            if (rs_Parcel_4.Count() > 0)
            {
                foreach (var rsp in rs_Parcel_4)
                {
                    Jumlah_Butir = rsp.TotalButir;
                    Jumlah_CaratPerButir = rsp.CaratButir;
                    Jumlah_Carat = rsp.TotalCarat;

                    lit_Parcel.Text += "<tr>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.Stone4.Nama + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.TotalButir.ToString("0") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + (rsp.CaratButir).ToString("0.000") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.TotalCarat.ToString("0.000") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.DimensiP.ToString("N3") + "</td>";
                    lit_Parcel.Text += "</tr>";

                    Total_Butir += Jumlah_Butir;
                    Total_CaratPerButir += Jumlah_CaratPerButir;
                    Total_Carat += Jumlah_Carat;
                }
            }

            if (rs_Parcel_5.Count() > 0)
            {
                foreach (var rsp in rs_Parcel_5)
                {
                    Jumlah_Butir = rsp.TotalButir;
                    Jumlah_CaratPerButir = rsp.CaratButir;
                    Jumlah_Carat = rsp.TotalCarat;

                    lit_Parcel.Text += "<tr>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.Stone5.Nama + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.TotalButir.ToString("0") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + (rsp.CaratButir).ToString("0.000") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.TotalCarat.ToString("0.000") + "</td>";
                    lit_Parcel.Text += "<td style='border: 1px solid black;'>" + rsp.DimensiP.ToString("N3") + "</td>";
                    lit_Parcel.Text += "</tr>";

                    Total_Butir += Jumlah_Butir;
                    Total_CaratPerButir += Jumlah_CaratPerButir;
                    Total_Carat += Jumlah_Carat;
                }
            }

            lit_JumlahButir.Text = Total_Butir.ToString("0");
            lit_CaratPerButir.Text = Total_CaratPerButir.ToString("0.000");
            lit_TotalCarat.Text = Total_Carat.ToString("0.000");

            /// Kode Komponen & Jumlah
            //lit_KodeKomponen_1.Text = d.ProductOrder_CharProduct.KodeKomponen1 != null ? d.ProductOrder_CharProduct.KodeKomponen1 : "";
            //lit_KodeKomponen_2.Text = d.ProductOrder_CharProduct.KodeKomponen2 != null ? d.ProductOrder_CharProduct.KodeKomponen2 : "";
            //lit_KodeKomponen_3.Text = d.ProductOrder_CharProduct.KodeKomponen3 != null ? d.ProductOrder_CharProduct.KodeKomponen3 : "";
            //lit_KodeKomponen_4.Text = d.ProductOrder_CharProduct.KodeKomponen4 != null ? d.ProductOrder_CharProduct.KodeKomponen4 : "";
            //lit_KodeKomponen_5.Text = d.ProductOrder_CharProduct.KodeKomponen5 != null ? d.ProductOrder_CharProduct.KodeKomponen5 : "";
            //lit_KodeKomponen_6.Text = d.ProductOrder_CharProduct.KodeKomponen6 != null ? d.ProductOrder_CharProduct.KodeKomponen6 : "";
            //lit_JumlahKomponen_1.Text = d.ProductOrder_CharProduct.JlhKomponen1 != null ? Convert.ToDecimal(d.ProductOrder_CharProduct.JlhKomponen1).ToString("N2") : "";
            //lit_JumlahKomponen_2.Text = d.ProductOrder_CharProduct.JlhKomponen2 != null ? Convert.ToDecimal(d.ProductOrder_CharProduct.JlhKomponen2).ToString("N2") : "";
            //lit_JumlahKomponen_3.Text = d.ProductOrder_CharProduct.JlhKomponen3 != null ? Convert.ToDecimal(d.ProductOrder_CharProduct.JlhKomponen3).ToString("N2") : "";
            //lit_JumlahKomponen_4.Text = d.ProductOrder_CharProduct.JlhKomponen4 != null ? Convert.ToDecimal(d.ProductOrder_CharProduct.JlhKomponen4).ToString("N2") : "";
            //lit_JumlahKomponen_5.Text = d.ProductOrder_CharProduct.JlhKomponen5 != null ? Convert.ToDecimal(d.ProductOrder_CharProduct.JlhKomponen5).ToString("N2") : "";
            //lit_JumlahKomponen_6.Text = d.ProductOrder_CharProduct.JlhKomponen6 != null ? Convert.ToDecimal(d.ProductOrder_CharProduct.JlhKomponen6).ToString("N2") : "";

            /// Size & Grafir
            lit_Size_2.Text = d.ProductOrder_CharProduct.DimensiR != null ? Convert.ToDecimal(d.ProductOrder_CharProduct.DimensiR).ToString("N2") : "";
            lit_Size_2_2.Text = d.ProductOrder_CharProduct.DimensiR2 != null ? Convert.ToDecimal(d.ProductOrder_CharProduct.DimensiR2).ToString("N2") : "";

            string Grafir = d.ProductOrder_CharDesign.Grafir != null ? d.ProductOrder_CharDesign.Grafir : "";
            string[] Grafir_Array = Grafir.Select(c => c.ToString()).ToArray();

            string Grafir2 = d.ProductOrder_CharDesign.Grafir2 != null ? d.ProductOrder_CharDesign.Grafir2 : "";
            string[] Grafir2_Array = Grafir2.Select(c => c.ToString()).ToArray();

            if ((Grafir != "" || Grafir2 != "") && d.ProductOrder_CharDesign.GrafirType != "")
            {
                lit_fonttype.Text = "Jenis Huruf : " + d.ProductOrder_CharDesign.GrafirType;
            }

            //lit_Grafir.Text = Grafir;
            if (Grafir != "")
            {
                lit_Grafir_1.Text = Grafir_Array.Length >= 1 ? Grafir_Array[0] : "";
                lit_Grafir_2.Text = Grafir_Array.Length >= 2 ? Grafir_Array[1] : "";
                lit_Grafir_3.Text = Grafir_Array.Length >= 3 ? Grafir_Array[2] : "";
                lit_Grafir_4.Text = Grafir_Array.Length >= 4 ? Grafir_Array[3] : "";
                lit_Grafir_5.Text = Grafir_Array.Length >= 5 ? Grafir_Array[4] : "";
                lit_Grafir_6.Text = Grafir_Array.Length >= 6 ? Grafir_Array[5] : "";
                lit_Grafir_7.Text = Grafir_Array.Length >= 7 ? Grafir_Array[6] : "";
                lit_Grafir_8.Text = Grafir_Array.Length >= 8 ? Grafir_Array[7] : "";
                lit_Grafir_9.Text = Grafir_Array.Length >= 9 ? Grafir_Array[8] : "";
                lit_Grafir_10.Text = Grafir_Array.Length >= 10 ? Grafir_Array[9] : "";
                lit_Grafir_11.Text = Grafir_Array.Length >= 11 ? Grafir_Array[10] : "";
                lit_Grafir_12.Text = Grafir_Array.Length >= 12 ? Grafir_Array[11] : "";
                lit_Grafir_13.Text = Grafir_Array.Length >= 13 ? Grafir_Array[12] : "";
                lit_Grafir_14.Text = Grafir_Array.Length >= 14 ? Grafir_Array[13] : "";
                lit_Grafir_15.Text = Grafir_Array.Length >= 15 ? Grafir_Array[14] : "";
                lit_Grafir_16.Text = Grafir_Array.Length >= 16 ? Grafir_Array[15] : "";
                lit_Grafir_17.Text = Grafir_Array.Length >= 17 ? Grafir_Array[16] : "";
                lit_Grafir_18.Text = Grafir_Array.Length >= 18 ? Grafir_Array[17] : "";
                lit_Grafir_19.Text = Grafir_Array.Length >= 19 ? Grafir_Array[18] : "";
            }

            if (Grafir2 != "")
            {
                lit_Grafir2_1.Text = Grafir2_Array.Length >= 1 ? Grafir2_Array[0] : "";
                lit_Grafir2_2.Text = Grafir2_Array.Length >= 2 ? Grafir2_Array[1] : "";
                lit_Grafir2_3.Text = Grafir2_Array.Length >= 3 ? Grafir2_Array[2] : "";
                lit_Grafir2_4.Text = Grafir2_Array.Length >= 4 ? Grafir2_Array[3] : "";
                lit_Grafir2_5.Text = Grafir2_Array.Length >= 5 ? Grafir2_Array[4] : "";
                lit_Grafir2_6.Text = Grafir2_Array.Length >= 6 ? Grafir2_Array[5] : "";
                lit_Grafir2_7.Text = Grafir2_Array.Length >= 7 ? Grafir2_Array[6] : "";
                lit_Grafir2_8.Text = Grafir2_Array.Length >= 8 ? Grafir2_Array[7] : "";
                lit_Grafir2_9.Text = Grafir2_Array.Length >= 9 ? Grafir2_Array[8] : "";
                lit_Grafir2_10.Text = Grafir2_Array.Length >= 10 ? Grafir2_Array[9] : "";
                lit_Grafir2_11.Text = Grafir2_Array.Length >= 11 ? Grafir2_Array[10] : "";
                lit_Grafir2_12.Text = Grafir2_Array.Length >= 12 ? Grafir2_Array[11] : "";
                lit_Grafir2_13.Text = Grafir2_Array.Length >= 13 ? Grafir2_Array[12] : "";
                lit_Grafir2_14.Text = Grafir2_Array.Length >= 14 ? Grafir2_Array[13] : "";
                lit_Grafir2_15.Text = Grafir2_Array.Length >= 15 ? Grafir2_Array[14] : "";
                lit_Grafir2_16.Text = Grafir2_Array.Length >= 16 ? Grafir2_Array[15] : "";
                lit_Grafir2_17.Text = Grafir2_Array.Length >= 17 ? Grafir2_Array[16] : "";
                lit_Grafir2_18.Text = Grafir2_Array.Length >= 18 ? Grafir2_Array[17] : "";
                lit_Grafir2_19.Text = Grafir2_Array.Length >= 19 ? Grafir2_Array[18] : "";
            }

            /// Titipan
            lit_Titipan.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_EmasBerat.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_StoneCarat.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_StoneButir.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_ColourStoneCarat.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_ColourStoneButir.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_Cabang.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_Pusat.Text = ""; // kosongin dulu sementara karena belum konek pos
            lit_Factory.Text = ""; // kosongin dulu sementara karena belum konek pos

            /// Keterangan Perubahan SPK
            lit_KeteranganPerubahanSPK.Text = d.Keterangan != null ? d.Keterangan : "";

            /// Nomor Dokumen & Kode Karet Lama
            //lit_KodeKaretLama.Text = d.KodeKaretLama != null ? d.KodeKaretLama : "";

            //Picture
            Upload.Image(imgpicture, d.ImgPicture);
        }
    }
}