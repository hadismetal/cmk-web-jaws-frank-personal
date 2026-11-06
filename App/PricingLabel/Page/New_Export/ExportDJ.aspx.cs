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
        string From = Request.QueryString["from"];
        string To = Request.QueryString["to"];
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int tipe = Convert.ToInt32(Request.QueryString["tipe"]);
        int cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        int clarity = Convert.ToInt32(Request.QueryString["clarity"]);

        if (To == "" && From != "") { To = From; }
        if (From == "" && To != "") { From = To; }
        if ((From == "" || To == "" || From == null || To == null) && (ProductItem == null || ProductItem < 1)) DataError();
        if ((From.Length != To.Length)) DataError();

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                int YearFrom = 1;
                int YearTo = 1;
                int Start = 1;
                int End = 1;

                // (Old / Avoris) : with plu, 10 digit
                if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                {
                    Start = Convert.ToInt32(From.Substring(3, 7));
                    End = Convert.ToInt32(To.Substring(3, 7));
                }
                // (Old / Speedware) : with first character letter, 6 digit
                else if (( From.Substring(0, 1) == "A" 
                        || From.Substring(0, 1) == "B" 
                        || From.Substring(0, 1) == "G" 
                        || From.Substring(0, 1) == "P"
                        || From.Substring(0, 1) == "R"
                        || From.Substring(0, 1) == "Q"
                    ) && From.Length == 6)
                {
                    Start = Convert.ToInt32(From.Substring(1,5));
                    End = Convert.ToInt32(To.Substring(1, 5));
                }
                // (Old / Speedware) : without letter / all number, 6 digit
                else if (From.Length == 6)
                {
                    Start = Convert.ToInt32(From);
                    End = Convert.ToInt32(To);
                }
                // (New / JAWS) : with first letter, 9 digit
                else if (From.Length == 9)
                {
                    YearFrom = Convert.ToInt32(From.Substring(1, 2));
                    YearTo = Convert.ToInt32(To.Substring(1, 2));
                    Start = Convert.ToInt32(From.Substring(3, 6));
                    End = Convert.ToInt32(To.Substring(3, 6));
                }

                // Validasi Tahun agar tidak terlalu banyak bandroll
                if (YearFrom != YearTo) DataError();
                else
                {
                    if (Start > End) DataError();
                    else
                    {
                        var dj = from p in db.StockProductDJ
                                 where (p.Legacy == null || p.Legacy == 0) && p.Draft == false
                                    && p.Nomor.Length == From.Length
                                    && p.Nomor.Contains(From.Substring(0, 3))
                                 select p;

                        // (Old / Avoris) : with plu, 10 digit
                        if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                        {
                            // PLU0023827
                            dj = dj.Where(p => p.Nomor.Length == 10);
                            dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 7)) >= Start);
                            dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 7)) <= End);
                            dj = dj.Where(p => p.Nomor.Substring(0, 3) == "PLU");
                        }
                        // (Old / Speedware) : with first character letter, 6 digit
                        else if ((From.Substring(0, 1) == "A" 
                               || From.Substring(0, 1) == "B" 
                               || From.Substring(0, 1) == "G" 
                               || From.Substring(0, 1) == "P"
                               || From.Substring(0, 1) == "R"
                               || From.Substring(0, 1) == "Q"
                                ) && From.Length == 6)
                        {
                            // P43172
                            dj = dj.Where(p => p.Nomor.Length == 6);
                            dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(1, 5)) >= Start);
                            dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(1, 5)) <= End);
                            dj = dj.Where(p => ( p.Nomor.Substring(0, 1) == "A" // frank
                                              || p.Nomor.Substring(0, 1) == "B" // frank
                                              || p.Nomor.Substring(0, 1) == "G" // frank
                                              || p.Nomor.Substring(0, 1) == "P" // frank, palace, mondial
                                              || p.Nomor.Substring(0, 1) == "R" // frank
                                              || p.Nomor.Substring(0, 1) == "Q" // mondial
                                             ));
                        }
                        // (Old / Speedware) : without letter / all number, 6 digit
                        else if (From.Length == 6)
                        {
                            // 555698
                            dj = dj.Where(p => p.Nomor.Length == 6);
                            dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(0, 6)) >= Start);
                            dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(0, 6)) <= End);
                            dj = dj.Where(p => !(p.Nomor.Substring(0, 1) == "A" // frank
                                              || p.Nomor.Substring(0, 1) == "B" // frank
                                              || p.Nomor.Substring(0, 1) == "G" // frank
                                              || p.Nomor.Substring(0, 1) == "P" // frank, palace, mondial
                                              || p.Nomor.Substring(0, 1) == "R" // frank
                                              || p.Nomor.Substring(0, 1) == "Q" // mondial
                                              ));
                        }
                        // (New / JAWS) : with first letter, 9 digit
                        else if (From.Length == 9)
                        {
                            // H15002681
                            dj = dj.Where(p => p.Nomor.Substring(1, 2) == YearFrom.ToString()); // A17020031
                            dj = dj.Where(p => p.Nomor.Length == 9);
                            dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start);
                            dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 6)) <= End);
                            dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom); // jaga agar tidak salah range tahun
                        }

                        if (ProductItem > 0)
                        {
                            dj = dj.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
                        }
                        if (tipe > 0 && cabang > 0)
                        {
                            dj = dj.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == cabang);
                        }
                        if (clarity == 1)
                        {
                            dj = dj.Where(p => p.StockProductDJ_Stone1A.Any(s => s.Stone1A.Parcel104.Nama == "SI"));
                        }
                        else if(clarity == 2)
                        {
                            dj = dj.Where(p => p.StockProductDJ_Stone1A.Any(s => s.Stone1A.Parcel104.Nama != "SI"));
                        }

                        // urut by nomor plu
                        dj = dj.OrderBy(p => p.Nomor);

                        var x = from p in dj
                                select new
                                {
                                    SupplierNomor = p.SupplierNomor,
                                    Supplier = p.Supplier,
                                    ProductItem = p.StockProductDJ_CharProduct.CharProductItem.NamaKode,
                                    ProductItemNama = p.StockProductDJ_CharProduct.CharProductItem.Nama,
                                    ProductCategory = p.StockProductDJ_CharProduct.CharProductCategory.NamaKode,
                                    Tgl = p.Tgl,
                                    ID = p.ID,
                                    ProductDimensiR = p.StockProductDJ_CharProduct.DimensiR,
                                    Nomor = p.Nomor,
                                    //Harga = q.PromoDJ.PromoHarga,
                                    Harga = p.StockProductDJ_PricingMU.HargaRupiahRound,
                                    KadarLogam = p.StockProductDJ_CharProduct.KadarLogam,
                                    StoneQty = p.StockProductDJ_CharProduct.StoneQty,
                                    StoneCarat = p.StockProductDJ_CharProduct.StoneCarat,
                                    NettoWeight = p.StockProductDJ_CharProduct.NettoWeight,
                                    CharFrameMaterial = p.StockProductDJ_CharProduct.CharFrameMaterial.NamaKode,
                                    Keterangan = p.Keterangan,
                                    ProductLevel = p.StockProductDJ_CharProduct.CharProductLevel.NamaKode,
                                    CompanyBrand = p.LocWarehouse.CompanyBrand.NamaKode
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
                        else
                        {
                            Response.Redirect("Export.aspx");
                        }
                    }
                }
            }
            

        }
        catch (Exception ex)
        {
            Response.Write(ex);
        }
    }
    protected void DataError()
    {
        Response.Redirect("Export.aspx");
    }
}