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

        if ((From == "" || To == "" || From == null || To == null) && (ProductItem == null || ProductItem < 1)) DataError();

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                int YearFrom = 1;
                int YearTo = 1;
                int Start = 1;
                int End = 1;

                if (!From.Contains("PLU") && From.Length == 9)
                {
                    YearFrom = Convert.ToInt32(From.Substring(1, 2));
                    YearTo = Convert.ToInt32(To.Substring(1, 2));
                    Start = Convert.ToInt32(From.Substring(3, 6));
                    End = Convert.ToInt32(To.Substring(3, 6));

                }
                else if (From.Length == 6)
                {
                    if (From.Length == 6 && (From.Contains("P") || From.Contains("Q")))
                    {
                        Start = Convert.ToInt32(From.Substring(1, 5));
                        End = Convert.ToInt32(To.Substring(1, 5));
                    }
                    else
                    {
                        Start = Convert.ToInt32(From);
                        End = Convert.ToInt32(To);
                    }
                }
                else
                {
                    Start = Convert.ToInt32(From.Substring(3, 7));
                    End = Convert.ToInt32(To.Substring(3, 7));
                }

                if (YearFrom != YearTo) DataError();
                else
                {
                    if (Start > End) DataError();
                    else
                    {
                        var dj = from p in db.StockProductDJ
                                 where (p.Legacy == null || p.Legacy == 0)
                                 select p;

                        dj = from p in dj
                             where p.Nomor.Contains("PLU") ?
                                Convert.ToInt32(p.Nomor.Substring(3, 7)) >= Start &&
                                Convert.ToInt32(p.Nomor.Substring(3, 7)) <= End :
                                (p.Nomor.Length == 6 && !p.Nomor.Contains("P") && !p.Nomor.Contains("Q") ) ?
                                Convert.ToInt32(p.Nomor) >= Start &&
                                Convert.ToInt32(p.Nomor) <= End
                                :
                                (p.Nomor.Length == 6 && (p.Nomor.Contains("P") || p.Nomor.Contains("Q") )) ?
                                Convert.ToInt32(p.Nomor.Substring(1,5)) >= Start &&
                                Convert.ToInt32(p.Nomor.Substring(1, 5)) <= End
                                :
                                Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom &&
                                Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start &&
                                Convert.ToInt32(p.Nomor.Substring(3, 6)) <= End
                             select p;
                        
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


                        if (dj.Count() > 0)
                        {
                            StringBuilder builder = new StringBuilder();
                            string val = "";
                            int count = 1;
                            foreach (var rslt in dj)
                            {
                                string Kode = "";
                                if (rslt.SupplierNomor != "" && rslt.SupplierNomor != null)
                                    Kode += rslt.SupplierNomor.Substring(0, 1);
                                Kode += rslt.StockProductDJ_CharProduct.CharProductItem.NamaKode;
                                Kode += rslt.StockProductDJ_CharProduct.CharProductCategory.NamaKode;
                                Kode += Convert.ToDateTime(rslt.Tgl).ToString("MM");
                                Kode += Convert.ToDateTime(rslt.Tgl).ToString("yy");
                                Kode += "1";

                                var stone = (from p in db.StockProductDJ_Stone1A
                                             where p.IDForm == rslt.ID
                                             select new { GIA = "", Color = "", Clarity = p.Stone1A.Parcel104.Nama, Kode = p.Stone1A.Parcel104.NamaKode, Tipe = 1, Nama = p.Stone1A.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = "F" }).Concat(
                                             from p in db.StockProductDJ_Stone1B
                                             where p.IDForm == rslt.ID && p.StatusTerpasang == 1
                                             select new { GIA = p.GIA, Color = p.Stone1B.Parcel103.NamaKode, Clarity = p.Stone1B.Parcel104.Nama, Kode = p.Stone1B.Parcel104.NamaKode, Tipe = 2, Nama = p.Stone1B.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone1B.Parcel101.NamaKode }).Concat(
                                             from p in db.StockProductDJ_Stone2
                                             where p.IDForm == rslt.ID
                                             select new { GIA = "", Color = p.Stone2.Parcel203.NamaKode, Clarity = "", Kode = "", Tipe = 3, Nama = p.Stone2.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone2.Parcel201.NamaKode }).Concat(
                                             from p in db.StockProductDJ_Stone3
                                             where p.IDForm == rslt.ID
                                             select new { GIA = "", Color = p.Stone3.Parcel303.NamaKode, Clarity = "", Kode = "", Tipe = 4, Nama = p.Stone3.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone3.Parcel301.NamaKode }).Concat(
                                             from p in db.StockProductDJ_Stone4
                                             where p.IDForm == rslt.ID
                                             select new { GIA = "", Color = p.Stone4.Parcel403.NamaKode, Clarity = "", Kode = "", Tipe = 5, Nama = p.Stone4.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone4.Parcel401.NamaKode }).Concat(
                                             from p in db.StockProductDJ_Stone5
                                             where p.IDForm == rslt.ID
                                             select new { GIA = "", Color = p.Stone5.Parcel503.NamaKode, Clarity = "", Kode = "", Tipe = 6, Nama = p.Stone5.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone5.Parcel501.NamaKode });


                                decimal dimensiR = 0m;
                                try
                                {
                                    dimensiR = Math.Floor(rslt.StockProductDJ_CharProduct.DimensiR.Value);
                                }
                                catch (Exception ex) { }
                                decimal ringsize = 0m;
                                try
                                {
                                    //ringsize = Math.Floor(rslt.StockProductDJ_CharProduct.DimensiR.Value);
                                    ringsize = rslt.StockProductDJ_CharProduct.DimensiR.Value;
                                }
                                catch (Exception ex) { }

                                string print_ringsize = "";
                                if (ringsize - dimensiR > 0)
                                    print_ringsize = Convert.ToDecimal(rslt.StockProductDJ_CharProduct.DimensiR).ToString("N2");
                                else
                                    print_ringsize = Convert.ToDecimal(rslt.StockProductDJ_CharProduct.DimensiR).ToString("N0");

                                if (count > 1)
                                    builder.Append("\n");
                                builder.Append("\"" + enc.EncryptSha(rslt.Nomor) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Kode) + "\",");
                                builder.Append("\"" + enc.EncryptSha(rslt.StockProductDJ_CharProduct.CharProductItem.Nama) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0")) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Math.Floor(rslt.StockProductDJ_CharProduct.KadarLogam).ToString("N0") + "%") + "\",");
                                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StockProductDJ_CharProduct.StoneQty).ToString("N0")) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StockProductDJ_CharProduct.StoneCarat).ToString("N3")) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StockProductDJ_CharProduct.NettoWeight).ToString("N2")) + "\",");
                                builder.Append("\"" + enc.EncryptSha(print_ringsize) + "\",");
                                if (stone != null)
                                {
                                    if (stone.Count() < 5)
                                    {
                                        builder.Append("\"2\",");
                                        foreach (var st in stone)
                                        {
                                            string Clarity = "";
                                            string Colour = st.Color;
                                            string Shape = st.Shape;
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
                                                //if (st.Clarity == "SI")
                                                if (st.Nama.Contains("W4"))
                                                    Clarity = "VS";
                                                else if (st.Nama.Contains("B4") && rslt.LocWarehouse.CompanyBrand.NamaKode == "P")
                                                    Clarity = "VS";
                                                else
                                                    Clarity = "VVS";

                                                if (st.Nama.Contains("B4") && rslt.LocWarehouse.CompanyBrand.NamaKode == "P")
                                                {
                                                    Colour = "H";
                                                    Shape = "";
                                                }
                                            }
                                            else
                                                Clarity = st.Clarity;

                                            builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(st.TotalButir).ToString("N0")) + "\",");
                                            builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(st.TotalCarat).ToString("N3") + Shape + Colour + Clarity) + "\",");
                                        }
                                    }
                                    else
                                        builder.Append("\"1\",");
                                }
                                count++;
                            }
                            Response.Write(builder.ToString());
                        }
                    }
                }
            }
            else
            {
                
                if (ProductItem > 0)
                {

                    var dj = from p in db.StockProductDJ
                             where p.StockProductDJ_CharProduct.ProductItem == ProductItem
                             select p;

                    if (tipe > 0 && cabang > 0)
                    {
                        dj = dj.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == cabang);
                    }
                    if (clarity == 1)
                    {
                        dj = dj.Where(p => p.StockProductDJ_Stone1A.Any(s => s.Stone1A.Parcel104.Nama == "SI"));
                    }
                    else if (clarity == 2)
                    {
                        dj = dj.Where(p => p.StockProductDJ_Stone1A.Any(s => s.Stone1A.Parcel104.Nama != "SI"));
                    }

                    if (dj.Count() > 0)
                    {
                        StringBuilder builder = new StringBuilder();
                        string val = "";
                        int count = 1;
                        foreach (var rslt in dj)
                        {
                            string Kode = "";
                            if (rslt.SupplierNomor != "" && rslt.SupplierNomor != null)
                                Kode += rslt.SupplierNomor.Substring(0, 1);
                            Kode += rslt.StockProductDJ_CharProduct.CharProductItem.NamaKode;
                            Kode += rslt.StockProductDJ_CharProduct.CharProductCategory.NamaKode;
                            Kode += Convert.ToDateTime(rslt.Tgl).ToString("MM");
                            Kode += Convert.ToDateTime(rslt.Tgl).ToString("yy");
                            Kode += "1";

                            var stone = (from p in db.StockProductDJ_Stone1A
                                         where p.IDForm == rslt.ID
                                         select new { GIA = "", Color = "F", Clarity = p.Stone1A.Parcel104.Nama, Kode = p.Stone1A.Parcel104.NamaKode, Tipe = 1, Nama = p.Stone1A.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, /*Shape = "F"*/ Shape = p.Stone1A.Parcel101.NamaKode }).Concat(
                                             from p in db.StockProductDJ_Stone1B
                                             where p.IDForm == rslt.ID
                                             select new { GIA = "", Color = p.Stone1B.Parcel103.NamaKode, Clarity = p.Stone1B.Parcel104.Nama, Kode = p.Stone1B.Parcel104.NamaKode, Tipe = 2, Nama = p.Stone1B.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone1B.Parcel101.NamaKode }).Concat(
                                             from p in db.StockProductDJ_Stone2
                                             where p.IDForm == rslt.ID
                                             select new { GIA = "", Color = p.Stone2.Parcel203.NamaKode, Clarity = "", Kode = "", Tipe = 3, Nama = p.Stone2.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone2.Parcel201.NamaKode }).Concat(
                                             from p in db.StockProductDJ_Stone3
                                             where p.IDForm == rslt.ID
                                             select new { GIA = "", Color = p.Stone3.Parcel303.NamaKode, Clarity = "", Kode = "", Tipe = 4, Nama = p.Stone3.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone3.Parcel301.NamaKode }).Concat(
                                             from p in db.StockProductDJ_Stone4
                                             where p.IDForm == rslt.ID
                                             select new { GIA = "", Color = p.Stone4.Parcel403.NamaKode, Clarity = "", Kode = "", Tipe = 5, Nama = p.Stone4.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone4.Parcel401.NamaKode }).Concat(
                                             from p in db.StockProductDJ_Stone5
                                             where p.IDForm == rslt.ID
                                             select new { GIA = "", Color = p.Stone5.Parcel503.NamaKode, Clarity = "", Kode = "", Tipe = 6, Nama = p.Stone5.Nama, TotalButir = p.TotalButir, TotalCarat = p.TotalCarat, Shape = p.Stone5.Parcel501.NamaKode });

                            decimal dimensiR = 0m;
                            try
                            {
                                dimensiR = Math.Floor(rslt.StockProductDJ_CharProduct.DimensiR.Value);
                            }
                            catch (Exception ex) { }
                            decimal ringsize = 0m;
                            try
                            {
                                //ringsize = Math.Floor(rslt.StockProductDJ_CharProduct.DimensiR.Value);
                                ringsize = rslt.StockProductDJ_CharProduct.DimensiR.Value;
                            }
                            catch (Exception ex) { }

                            string print_ringsize = "";
                            if (ringsize - dimensiR > 0)
                                print_ringsize = Convert.ToDecimal(rslt.StockProductDJ_CharProduct.DimensiR).ToString("N2");
                            else
                                print_ringsize = Convert.ToDecimal(rslt.StockProductDJ_CharProduct.DimensiR).ToString("N0");

                            if (count > 1)
                                builder.Append("\n");
                            builder.Append("\"" + enc.EncryptSha(rslt.Nomor) + "\",");
                            builder.Append("\"" + enc.EncryptSha(Kode) + "\",");
                            builder.Append("\"" + enc.EncryptSha(rslt.StockProductDJ_CharProduct.CharProductItem.Nama) + "\",");
                            builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0")) + "\",");
                            builder.Append("\"" + enc.EncryptSha(Math.Floor(rslt.StockProductDJ_CharProduct.KadarLogam).ToString("N0") + "%") + "\",");
                            builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StockProductDJ_CharProduct.StoneQty).ToString("N0")) + "\",");
                            builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StockProductDJ_CharProduct.StoneCarat).ToString("N3")) + "\",");
                            builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StockProductDJ_CharProduct.NettoWeight).ToString("N2")) + "\",");
                            builder.Append("\"" + enc.EncryptSha(print_ringsize) + "\",");
                            if (stone != null)
                            {
                                if (stone.Count() < 5)
                                {
                                    builder.Append("\"2\",");
                                    foreach (var st in stone)
                                    {
                                        string Clarity = "";
                                        string Colour = st.Color;
                                        string Shape = st.Shape;
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
                                            //if (st.Clarity == "SI")
                                            if (st.Nama.Contains("W4"))
                                                Clarity = "VS";
                                            else if (st.Nama.Contains("B4") && rslt.LocWarehouse.CompanyBrand.NamaKode == "P")
                                                Clarity = "VS";
                                            else
                                                Clarity = "VVS";

                                            if (st.Nama.Contains("B4") && rslt.LocWarehouse.CompanyBrand.NamaKode == "P")
                                            {
                                                Colour = "H";
                                                Shape = "";
                                            }
                                        }
                                        else
                                            Clarity = st.Clarity;

                                        builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(st.TotalButir).ToString("N0")) + "\",");
                                        builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(st.TotalCarat).ToString("N3") + Shape + Colour + Clarity) + "\",");
                                        
                                    }
                                }
                                else
                                    builder.Append("\"1\",");
                            }
                            count++;
                        }
                        Response.Write(builder.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //DataError();
            Response.Write(ex);
        }
    }
    protected void DataError()
    {
        Response.Redirect("Export.aspx");
    }
}