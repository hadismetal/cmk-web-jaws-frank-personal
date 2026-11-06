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

        string xTest = "0";

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

                xTest = "1";

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

                xTest = "2";

                // Validasi Tahun agar tidak terlalu banyak bandroll
                if (YearFrom != YearTo) DataError();
                else
                {
                    xTest = "3";

                    if (Start > End) DataError();
                    else
                    {
                        xTest = "4";

                        var dj = from p in db.StockProductDJ
                                 where (p.Legacy == null || p.Legacy == 0) && p.Draft == false
                                    && p.Nomor.Length == From.Length
                                    && p.Nomor.Contains(From.Substring(0,3))
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

                        xTest = "5";

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
                            xTest = "6";

                            Response.ContentType = "text/csv";
                            Response.AppendHeader("content-disposition", "attachment; filename=DJ_"
                                + App.UserNama.Replace(" ", "") + "_"
                                + System.DateTime.Now.Year.ToString().Substring(2)
                                + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Day.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Hour.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Minute.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Second.ToString().PadLeft(2, '0')
                                + "_" + dj.Count().ToString() + ".csv");

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

                                //decimal dimensiR = 0m;
                                //try
                                //{
                                //    dimensiR = Math.Floor(rslt.ProductDimensiR.Value);
                                //}
                                //catch (Exception ex) { }
                                //decimal ringsize = 0m;
                                //try
                                //{
                                //    //ringsize = Math.Floor(rslt.StockProductDJ_CharProduct.DimensiR.Value);
                                //    ringsize = rslt.ProductDimensiR.Value;
                                //}
                                //catch (Exception ex) { }

                                string print_ringsize = "0";
                                try
                                {
                                    print_ringsize = Convert.ToDecimal(rslt.ProductDimensiR).ToString("N2");
                                }
                                catch (Exception ex) { }

                                //if (ringsize - dimensiR > 0)
                                //{
                                //    print_ringsize = Convert.ToDecimal(rslt.ProductDimensiR).ToString("N2");
                                //}
                                //else
                                //{
                                //    print_ringsize = "0";
                                //}

                                xTest = "7";

                                // Builder 1 : Start
                                if (count > 1) { builder.Append("\n"); }

                                builder.Append("\"" + enc.EncryptSha(rslt.Nomor) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Kode) + "\",");
                                builder.Append("\"" + enc.EncryptSha(rslt.ProductItemNama) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.Harga).ToString("N0")) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Math.Floor(rslt.KadarLogam).ToString("N0") + "%") + "\",");
                                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StoneQty).ToString("N0")) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.StoneCarat).ToString("N3")) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.NettoWeight).ToString("N2") + "gr") + "\",");
                                builder.Append("\"" + enc.EncryptSha(rslt.CharFrameMaterial) + "\",");
                                builder.Append("\"" + enc.EncryptSha(print_ringsize + "sz") + "\",");

                                // Builder 2 : NETT & DISC Condition
                                string strNETTDISC = "D";
                                if (Pricing.isNett(rslt.Nomor)) { strNETTDISC = "N"; }

                                builder.Append("\"" + enc.EncryptSha(strNETTDISC) + "\",");

                                xTest = "8";

                                // Builder 3 : Stone List
                                builder.Append("\"0\","); //sudah tidak terpakai tapi tetap harus dibuild

                                string plunumber = rslt.Nomor;
                                string stonemodelist = "b"; // b:bandroll / c:certificate
                                string plutype = "dj"; // dj or ld

                                string spname = "sp_get_stone_information_for_bandroll_certificate";
                                string query = " exec " + spname
                                             + " '" + stonemodelist + "'"
                                             + ",'" + plunumber + "'"
                                             + ",'" + plutype + "'"
                                             ;

                                DataTable dt = new DataTable();
                                dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

                                if (dt.Rows.Count > 0)
                                {
                                    string TotalButir = "";
                                    string TotalCarat = "";
                                    string Keterangan = "";

                                    foreach (DataRow dtRow in dt.Rows)
                                    {
                                        TotalButir = Convert.ToDecimal(dtRow["TotalButir"]).ToString("N0");
                                        TotalCarat = Convert.ToDecimal(dtRow["TotalCarat"]).ToString("N3");
                                        Keterangan = (dtRow["Keterangan"]).ToString();

                                        builder.Append("\"" + enc.EncryptSha(TotalButir) + "\",");
                                        builder.Append("\"" + enc.EncryptSha(TotalCarat + " " + Keterangan) + "\",");
                                    }
                                }

                                xTest = "9";

                                count++;
                            }
                            Response.Write(builder.ToString());
            
                        }
                        else
                        {
                            Response.Redirect("Export.aspx?msg=noplu");
                        }
                    }
                }
            }
            

        }
        catch (Exception ex)
        {
            //Response.Write(ex);
            Response.Write(xTest);
        }
    }
    protected void DataError()
    {
        Response.Redirect("Export.aspx");
    }
}