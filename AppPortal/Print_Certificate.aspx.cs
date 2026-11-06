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
    protected void Fill()
    {
        var d = db.StockProductDJ.SingleOrDefault(p => p.ID == myID);
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
            kadar.Text = Math.Floor(d.StockProductDJ_CharProduct.KadarLogam).ToString("N0");
            weight.Text = d.StockProductDJ_CharProduct.NettoWeight.ToString("N2");
            kodekaret.Text = d.ProtoNomor;
            item.Text = d.StockProductDJ_CharProduct.CharProductItem.Nama;
            img.Text = "<img src=\"/ImageCrop.aspx?path=" + d.ImgPicture + "\" />";

            string stone_text = "";

            var stone = (from p in d.StockProductDJ_Stone1A
                         select new { Color = "", Clarity = p.Stone1A.Parcel104.Nama, Kode = p.Stone1A.Parcel104.NamaKode, Tipe = 1, Nama = p.Stone1A.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = "F" }).Concat(
                from p in d.StockProductDJ_Stone1B
                select new { Color = p.Stone1B.Parcel103.NamaKode, Clarity = p.Stone1B.Parcel104.Nama, Kode = p.Stone1B.Parcel104.NamaKode, Tipe = 2, Nama = p.Stone1B.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone1B.Parcel101.NamaKode }).Concat(
                from p in d.StockProductDJ_Stone2
                select new { Color = p.Stone2.Parcel203.NamaKode, Clarity = "", Kode = "", Tipe = 3, Nama = p.Stone2.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone2.Parcel201.NamaKode }).Concat(
                from p in d.StockProductDJ_Stone3
                select new { Color = p.Stone3.Parcel303.NamaKode, Clarity = "", Kode = "", Tipe = 4, Nama = p.Stone3.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone3.Parcel301.NamaKode }).Concat(
                from p in d.StockProductDJ_Stone4
                select new { Color = p.Stone4.Parcel403.NamaKode, Clarity = "", Kode = "", Tipe = 5, Nama = p.Stone4.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone4.Parcel401.NamaKode }).Concat(
                from p in d.StockProductDJ_Stone5
                select new { Color = p.Stone5.Parcel503.NamaKode, Clarity = "", Kode = "", Tipe = 6, Nama = p.Stone5.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone5.Parcel501.NamaKode });

            if (stone.Count() < 5)
            {
                stone_text = "";
                int index = 1;
                foreach (var st in stone)
                {
                    string Clarity = "";
                    if (st.Tipe == 1)
                    {
                        /*if (Regex.Matches(st.Nama, "A4").Count > 0 || Regex.Matches(st.Nama, "S4").Count > 0)
                        {
                            Clarity = "VS";
                        }
                        else
                        {
                            Clarity = "VVS";
                        }*/
                        if (st.Clarity == "SI")
                            Clarity = "VS";
                        else
                            Clarity = "VVS";
                    }
                    else
                        Clarity = st.Clarity;

                    if (index == 1)
                    {
                        stone_text += Convert.ToDecimal(st.TotalButir).ToString("N0") + " : ";
                        index++;
                    }
                    else
                        stone_text += "<br />" + Convert.ToDecimal(st.TotalButir).ToString("N0") + " : ";
                    stone_text += Convert.ToDecimal(st.TotalCarat).ToString("N3") + " " + st.Shape + " " + st.Color + " " + Clarity;
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
}