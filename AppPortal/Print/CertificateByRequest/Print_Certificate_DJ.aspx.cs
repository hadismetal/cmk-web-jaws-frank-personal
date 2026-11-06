using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
public partial class Page_Report_Print_Certificate_DJ : System.Web.UI.Page
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

    protected string myPrint { get { return Request.QueryString["ID"]; } }
    protected string myPLU { get { return Request.QueryString["PLU"]; } }
    protected string myPrintDetail { get { return Request.QueryString["ID2"]; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        if (!IsPostBack)
        {
            Fill();
        }
    }

    protected void Fill()
    {
        byte[] dataprint = Convert.FromBase64String(myPrint);
        int xprint = Convert.ToInt32(Encoding.UTF8.GetString(dataprint));

        byte[] dataplu = Convert.FromBase64String(myPLU);
        string xplu = Encoding.UTF8.GetString(dataplu);

        byte[] datadetil = Convert.FromBase64String(myPrintDetail);
        int xdetil = Convert.ToInt32(Encoding.UTF8.GetString(datadetil));

        //lbwarning.InnerText = xprint.ToString() + "<br />" + xplu + "<br />" + xdetil.ToString();


        var d1 = db.PrintCertificate_Detail.SingleOrDefault(p => p.ID == xdetil && p.IDForm == xprint && p.PLU == xplu
        && p.TypeProduct == 1 && p.PrintCertificate.StatusFinish == 1);
        if (d1 != null)
        {
            if (d1.PrintCertificate.TglExpire >= System.DateTime.Now)
            {
                var d = db.StockProductDJ.SingleOrDefault(p => p.Nomor == d1.PLU);
                if (d != null)
                {
                    if (d.LocWarehouse.CompanyBrand.NamaKode == "F")
                    {
                        css.Text = "<style>body { color: #7c0545; font-weight: bold;}</style>";
                    }
                    else
                    {
                        css.Text = "<style>body { color: #000000; font-weight: bold;}</style>";
                    }

                    plu.Text = d.Nomor;
                    kadar.Text = (Math.Floor(d.StockProductDJ_CharProduct.KadarLogam) == 37)
                               ? (d.StockProductDJ_CharProduct.KadarLogam).ToString("N1")
                               : Math.Floor(d.StockProductDJ_CharProduct.KadarLogam).ToString("N0")
                               ;
                    weight.Text = d.StockProductDJ_CharProduct.NettoWeight.ToString("N2");
                    kodekaret.Text = d.ProtoNomor;
                    item.Text = d.StockProductDJ_CharProduct.CharProductItem.Nama;
                    //img.Text = "<img src=\"/ImageCrop.aspx?path=" + d.ImgPicture + "\" />";
                    img.Text = "<img src=\"/Images.aspx?path=" + d.ImgPicture + "\" style='width:90px; height:67px;' />";

                    string stone_text = "";
                    string shape = "";
                    string color = "";
                    string clarity = "";

                    var stone = (from p in d.StockProductDJ_Stone1A
                                 select new { Shape = p.Stone1A.Parcel101.NamaKode, Color = p.Stone1A.Parcel103.NamaKode, Clarity = p.Stone1A.Parcel104.Nama, Kode = p.Stone1A.Parcel104.NamaKode, Tipe = 1, Nama = p.Stone1A.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                                    from p in d.StockProductDJ_Stone1B
                                    select new { Shape = p.Stone1B.Parcel101.NamaKode, Color = p.Stone1B.Parcel103.NamaKode, Clarity = p.Stone1B.Parcel104.Nama, Kode = p.Stone1B.Parcel104.NamaKode, Tipe = 2, Nama = p.Stone1B.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                                    from p in d.StockProductDJ_Stone2
                                    select new { Shape = p.Stone2.Parcel201.NamaKode, Color = p.Stone2.Parcel203.NamaKode, Clarity = "", Kode = "", Tipe = 3, Nama = p.Stone2.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                                    from p in d.StockProductDJ_Stone3
                                    select new { Shape = p.Stone3.Parcel301.NamaKode, Color = p.Stone3.Parcel303.NamaKode, Clarity = "", Kode = "", Tipe = 4, Nama = p.Stone3.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                                    from p in d.StockProductDJ_Stone4
                                    select new { Shape = p.Stone4.Parcel401.NamaKode, Color = p.Stone4.Parcel403.NamaKode, Clarity = "", Kode = "", Tipe = 5, Nama = p.Stone4.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat }).Concat(
                                    from p in d.StockProductDJ_Stone5
                                    select new { Shape = p.Stone5.Parcel501.NamaKode, Color = p.Stone5.Parcel503.NamaKode, Clarity = "", Kode = "", Tipe = 6, Nama = p.Stone5.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat });

                    var rsCB = (from p in db.CompanyBrand
                                where p.Draft == false && p.Disable == false
                                select p.Nama).First();

                    string CB = ""; /// mondial, franknco, missmondial, thepalace
                    if (rsCB.Count() > 0) { CB = rsCB.ToLower(); }

                    if (stone.Count() < 5)
                    {
                        stone_text = "";
                        int index = 1;
                        foreach (var st in stone)
                        {
                            shape = st.Shape;
                            color = st.Color;
                            clarity = st.Clarity;

                            /// JIKA STONE 1A : Stone Kecil / Reguler
                            if (st.Tipe == 1)
                            {
                                if ((st.Nama).Substring((st.Nama).Length - 1) != "4")
                                {
                                    /// 1st class
                                    color = "F";
                                    clarity = "VVS";
                                }
                                else if ((st.Nama).Substring((st.Nama).Length - 2) == "B4")
                                {
                                    /// 3rd class
                                    color = "H";
                                    clarity = "VS";
                                }
                                else
                                {
                                    /// 2nd class
                                    color = "F";
                                    clarity = "VS";
                                }

                            }

                            /// CETAK
                            if (index == 1)
                            {
                                stone_text += Convert.ToDecimal(st.TotalButir).ToString("N0") + " : ";
                                stone_text += Convert.ToDecimal(st.TotalCarat).ToString("N3") + " " + shape + " " + color + " " + clarity;
                                index++;
                            }
                            else
                            {
                                stone_text += "<br />" + Convert.ToDecimal(st.TotalButir).ToString("N0") + " : ";
                                stone_text += Convert.ToDecimal(st.TotalCarat).ToString("N3") + " " + shape + " " + color + " " + clarity;
                            }
                        }
                        txtstone.Text = stone_text;
                    }
                    else
                    {
                        string infobatu = "";
                        infobatu = d.StockProductDJ_CharProduct.StoneQty.Value.ToString("N0") + " = " + d.StockProductDJ_CharProduct.StoneCarat.ToString("N3");

                        txtstone.Text = infobatu;
                    }
                }
            }
            else
            {
                tsertif.Visible = false;
                lbwarning.InnerText = "Mohon registrasi ulang dikarena masa aktif Sertifikat untuk PLU ini sudah habis";
            }
        }
        else
        {
            tsertif.Visible = false;
            lbwarning.InnerText = "Data yang diinput tidak ada !";
        }
    }
}