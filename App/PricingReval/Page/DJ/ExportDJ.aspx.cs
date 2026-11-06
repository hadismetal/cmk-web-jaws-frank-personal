using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

public partial class Page_Export : System.Web.UI.Page
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
    EncryptDecrypt enc = new EncryptDecrypt();
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Response.ContentType = "text/csv";

            Response.AppendHeader("content-disposition", "attachment; filename=PLU_DJ.csv");
            Set();
        }
    }
    protected void Set()
    {

        var dj = from p in db.RevalProductDJ
                 join q in db.StockActualDJ on p.Nomor equals q.Nomor
                 join r in db.StockProductDJ on q.Nomor equals r.Nomor
                 select new { p, q, r };

        if (Request.QueryString["parcel104a"] == "1")
        {
            dj = from p in dj
                 where !p.p.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("W4") && !z.Stone1A.Nama.Contains("B4"))
                 select p;
        }
        else if (Request.QueryString["parcel104a"] == "2")
        {
            dj = dj.Where(p => p.p.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("W4")));
        }
        else if (Request.QueryString["parcel104a"] == "3")
        {
            dj = dj.Where(p => p.p.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("B4")));
        }

        int Tipe = Convert.ToInt32(Request.QueryString["tipelokasi"]);
        int Lokasi = Convert.ToInt32(Request.QueryString["lokasi"]);
        if (Tipe == 0 || Tipe == 1 || Tipe == 2)
        {
            dj = dj.Where(p => ((Lokasi > 0) ? p.q.IDLokasi == Lokasi : 1 == 1) && p.q.TipeLokasi == Tipe);
        }

        string kw = Request.QueryString["kw"];
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        string orderby = Request.QueryString["orderby"];

        if (kw != "")
            dj = dj.Where(p => p.p.Nomor == "" + kw + "");

        if (ProductLevel > 0) dj = dj.Where(p => p.p.RevalProductDJ_CharProduct.ProductLevel == ProductLevel);

        if (ProductItem > 0) dj = dj.Where(p => p.p.RevalProductDJ_CharProduct.ProductItem == ProductItem);

        if (ProductCategory > 0) dj = dj.Where(p => p.p.RevalProductDJ_CharProduct.ProductCategory == ProductCategory);

        var x = from p in dj
                select new
                {
                    SupplierNomor = p.r.SupplierNomor,
                    Supplier = p.r.Supplier,
                    ProductItem = p.r.StockProductDJ_CharProduct.CharProductItem.NamaKode,
                    ProductItemNama = p.r.StockProductDJ_CharProduct.CharProductItem.Nama,
                    ProductCategory = p.r.StockProductDJ_CharProduct.CharProductCategory.NamaKode,
                    Tgl = p.r.Tgl,
                    ID = p.r.ID,
                    ProductDimensiR = p.r.StockProductDJ_CharProduct.DimensiR,
                    Nomor = p.r.Nomor,
                    //Harga = q.PromoDJ.PromoHarga,
                    Harga = p.p.RevalProductDJ_PricingMU.HargaRupiahRound,
                    KadarLogam = p.r.StockProductDJ_CharProduct.KadarLogam,
                    StoneQty = p.r.StockProductDJ_CharProduct.StoneQty,
                    StoneCarat = p.r.StockProductDJ_CharProduct.StoneCarat,
                    NettoWeight = p.r.StockProductDJ_CharProduct.NettoWeight,
                    CharFrameMaterial = p.r.StockProductDJ_CharProduct.CharFrameMaterial.NamaKode,
                    Keterangan = p.r.Keterangan,
                    ProductLevel = p.r.StockProductDJ_CharProduct.CharProductLevel.NamaKode,
                    CompanyBrand = p.r.LocWarehouse.CompanyBrand.NamaKode
                }
                ;

        if (dj.Count() > 0)
        {
            StringBuilder builder = new StringBuilder();
            string val = "";
            int count = 1;

            foreach (var rslt in x)
            {
                string Kode = "";
                if (rslt.Keterangan == "Import Stock")
                    Kode = rslt.SupplierNomor;
                else
                {
                    if (rslt.Supplier.HasValue && rslt.Supplier.Value != 0)
                    {

                        //Kode += rslt.SupplierNomor.Substring(0, 1);
                        int IDGroup = Db.SingleInteger("SELECT IDGroup FROM DataSupplier WHERE ID=" + rslt.Supplier, Db.Cnn_DB);
                        var sp = Db.SingleString("SELECT NamaKode FROM DataSupplierGroup WHERE ID=" + IDGroup, Db.Cnn_DB);
                        Kode = sp.Substring(0, 1);
                    }
                    Kode += rslt.ProductItem;
                    Kode += rslt.ProductCategory;
                    Kode += Convert.ToDateTime(rslt.Tgl).ToString("MM");
                    Kode += Convert.ToDateTime(rslt.Tgl).ToString("yy");
                    Kode += "1";
                }

                var stone = (from p in db.StockProductDJ_Stone1A
                             where p.IDForm == rslt.ID
                             select new { Brands = "", GIA = "", Color = "F", Clarity = p.Stone1A.Parcel104.Nama, Kode = p.Stone1A.Parcel104.NamaKode, Tipe = 1, Nama = p.Stone1A.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, /*Shape = "F"*/ Shape = p.Stone1A.Parcel101.NamaKode }).Concat(
                            from p in db.StockProductDJ_Stone1B
                            where p.IDForm == rslt.ID
                            select new { Brands = p.Stone1B.Parcel106.NamaKode, GIA = "", Color = p.Stone1B.Parcel103.NamaKode, Clarity = p.Stone1B.Parcel104.NamaKode, Kode = p.Stone1B.Parcel104.Nama, Tipe = 2, Nama = p.Stone1B.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone1B.Parcel101.NamaKode }).Concat(
                            from p in db.StockProductDJ_Stone2
                            where p.IDForm == rslt.ID
                            select new { Brands = "", GIA = "", Color = p.Stone2.Parcel203.NamaKode, Clarity = "", Kode = "", Tipe = 3, Nama = p.Stone2.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone2.Parcel201.NamaKode }).Concat(
                            from p in db.StockProductDJ_Stone3
                            where p.IDForm == rslt.ID
                            select new { Brands = "", GIA = "", Color = p.Stone3.Parcel303.NamaKode, Clarity = "", Kode = "", Tipe = 4, Nama = p.Stone3.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone3.Parcel301.NamaKode }).Concat(
                            from p in db.StockProductDJ_Stone4
                            where p.IDForm == rslt.ID
                            select new { Brands = "", GIA = "", Color = p.Stone4.Parcel403.NamaKode, Clarity = "", Kode = "", Tipe = 5, Nama = p.Stone4.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone4.Parcel401.NamaKode }).Concat(
                            from p in db.StockProductDJ_Stone5
                            where p.IDForm == rslt.ID
                            select new { Brands = "", GIA = "", Color = p.Stone5.Parcel503.NamaKode, Clarity = "", Kode = "", Tipe = 6, Nama = p.Stone5.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone5.Parcel501.NamaKode });

                decimal dimensiR = 0m;
                try
                {
                    dimensiR = Math.Floor(rslt.ProductDimensiR.Value);
                }
                catch (Exception ex) { }
                decimal ringsize = 0m;
                try
                {
                    //ringsize = Math.Floor(rslt.StockProductDJ_CharProduct.DimensiR.Value);
                    ringsize = rslt.ProductDimensiR.Value;
                }
                catch (Exception ex) { }

                string print_ringsize = "";
                if (ringsize - dimensiR > 0)
                    print_ringsize = Convert.ToDecimal(rslt.ProductDimensiR).ToString("N2");
                else
                    print_ringsize = Convert.ToDecimal(rslt.ProductDimensiR).ToString("N0");

                // Builder 1 : Start
                if (count > 1) { builder.Append("\n"); }

                builder.Append("\"" + enc.EncryptSha(rslt.Nomor) + "\",");
                builder.Append("\"" + enc.EncryptSha(Kode) + "\",");
                builder.Append("\"" + enc.EncryptSha(rslt.ProductItemNama) + "\",");
                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.Harga).ToString("N0")) + "\",");
                builder.Append("\"" + enc.EncryptSha(Math.Floor(rslt.KadarLogam).ToString("N0") + "%") + "\",");
                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StoneQty).ToString("N0")) + "\",");
                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StoneCarat).ToString("N3")) + "\",");
                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.NettoWeight).ToString("N2")) + "\",");
                builder.Append("\"" + enc.EncryptSha(rslt.CharFrameMaterial) + "\",");
                builder.Append("\"" + enc.EncryptSha(print_ringsize) + "\",");

                // Builder 2 : NETT & DISC Condition
                if (stone.Where(p => p.Tipe == 2).Count() > 0
                    && (rslt.ProductLevel == "SMG" || rslt.ProductLevel == "MTG")
                    && (rslt.ProductItem == "M" || rslt.ProductItem == "14"))
                    builder.Append("\"" + enc.EncryptSha("NETT") + "\",");
                else if (stone.Where(p => p.Tipe == 2).Count() > 0 && rslt.ProductCategory == "SNI")
                    builder.Append("\"" + enc.EncryptSha("NETT") + "\",");
                else
                    builder.Append("\"" + enc.EncryptSha("DISC") + "\",");

                if (stone != null)
                {
                    if (stone.Count() < 5)
                    {
                        #region stone<5
                        builder.Append("\"2\",");
                        foreach (var st in stone)
                        {
                            string Clarity = "";
                            string Colour = st.Color;
                            string Shape = st.Shape;
                            string Brand = st.Brands;
                            if (st.Tipe == 1)
                            {
                                if (st.Nama.Contains("W4"))
                                    Clarity = "VS";
                                else if (st.Nama.Contains("B4") && rslt.CompanyBrand == "P")
                                    Clarity = "VS";
                                else
                                    Clarity = "VVS";

                                if (st.Nama.Contains("B4") && rslt.CompanyBrand == "P")
                                {
                                    Colour = "H";
                                    Shape = "";
                                }
                            }
                            else
                                Clarity = st.Clarity;

                            builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(st.TotalButir).ToString("N0")) + "\",");
                            builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(st.TotalCarat).ToString("N3") + Shape + Colour + Clarity + Brand) + "\",");
                        }
                        #endregion
                    }
                    else
                    {
                        #region stone>=5

                        builder.Append("\"1\",");

                        builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StoneQty).ToString("N0")) + "\",");
                        builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StoneCarat).ToString("N3")) + "\"");

                        #endregion
                    }
                }

                count++;
            }
            Response.Write(builder.ToString());

        }
                            
    }
    protected void DataError()
    {
        Response.Redirect("Export.aspx");
    }
}