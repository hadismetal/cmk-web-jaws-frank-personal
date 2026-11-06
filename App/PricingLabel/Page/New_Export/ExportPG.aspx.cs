using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

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
    App prc = new App();
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
                else if ((From.Substring(0, 1) == "A"
                        || From.Substring(0, 1) == "B"
                        || From.Substring(0, 1) == "G"
                        || From.Substring(0, 1) == "P"
                        || From.Substring(0, 1) == "R"
                        || From.Substring(0, 1) == "Q"
                    ) && From.Length == 6)
                {
                    Start = Convert.ToInt32(From.Substring(1, 5));
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

                if (YearFrom != YearTo) DataError();
                else
                {
                    if (Start > End) DataError();
                    else
                    {
                        var pg = from p in db.StockProductPG
                                 where 1 == 1
                                    && p.Nomor.Length == From.Length
                                    && p.Nomor.Contains(From.Substring(0, 3))
                                 select p;


                        // (Old / Avoris) : with plu, 10 digit
                        if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                        {
                            // PLU0023827
                            pg = pg.Where(p => p.Nomor.Length == 10);
                            pg = pg.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 7)) >= Start);
                            pg = pg.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 7)) <= End);
                            pg = pg.Where(p => p.Nomor.Substring(0, 3) == "PLU");
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
                            pg = pg.Where(p => p.Nomor.Length == 6);
                            pg = pg.Where(p => Convert.ToInt32(p.Nomor.Substring(1, 5)) >= Start);
                            pg = pg.Where(p => Convert.ToInt32(p.Nomor.Substring(1, 5)) <= End);
                            pg = pg.Where(p => (p.Nomor.Substring(0, 1) == "A" // frank
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
                            pg = pg.Where(p => p.Nomor.Length == 6);
                            pg = pg.Where(p => Convert.ToInt32(p.Nomor.Substring(0, 6)) >= Start);
                            pg = pg.Where(p => Convert.ToInt32(p.Nomor.Substring(0, 6)) <= End);
                            pg = pg.Where(p => !(p.Nomor.Substring(0, 1) == "A" // frank
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
                            pg = pg.Where(p => p.Nomor.Substring(1, 2) == YearFrom.ToString()); // A17020031
                            pg = pg.Where(p => p.Nomor.Length == 9);
                            pg = pg.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start);
                            pg = pg.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 6)) <= End);
                            pg = pg.Where(p => Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom); // jaga agar tidak salah range tahun
                        }


                        if (ProductItem > 0)
                        {
                            pg = pg.Where(p => p.ProductItem == ProductItem);
                        }
                        if (tipe > 0 && cabang > 0)
                        {
                            pg = pg.Where(p => p.StockActualPG.TipeLokasi == tipe && p.StockActualPG.IDLokasi == cabang);
                        }

                        // urut by nomor plu
                        pg = pg.OrderBy(p => p.Nomor);

                        if (pg.Count() > 0)
                        {
                            Response.ContentType = "text/csv";
                            Response.AppendHeader("content-disposition", "attachment; filename=PG_"
                                + App.UserNama.Replace(" ", "") + "_"
                                + System.DateTime.Now.Year.ToString().Substring(2)
                                + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Day.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Hour.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Minute.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Second.ToString().PadLeft(2, '0')
                                + "_" + pg.Count().ToString() + ".csv");

                            StringBuilder builder = new StringBuilder();
                            string val = "";
                            int count = 1;
                            foreach (var rslt in pg)
                            {
                                string Kode = "";
                                if (rslt.SupplierNama != "" && rslt.SupplierNama != null)
                                    Kode += rslt.SupplierNama.Substring(0, 1);
                                Kode += rslt.CharProductItem.NamaKode;
                                //Kode += rslt.StockProductDJ_CharProduct.CharProductCategory.NamaKode;
                                Kode += Convert.ToDateTime(rslt.Tgl).ToString("MM");
                                Kode += Convert.ToDateTime(rslt.Tgl).ToString("yy");
                                Kode += "1";
                                if (count > 1)
                                    builder.Append("\n");

                                builder.Append("\"" + enc.EncryptSha(rslt.Nomor) + "\",");
                                builder.Append("\"" + enc.EncryptSha(rslt.CharProductItem.Nama) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.GrossWeight).ToString("N2") + "gr") + "\",");
                                builder.Append("\"" + enc.EncryptSha((rslt.FixRate == true) ? prc.RoundTop(Convert.ToDecimal(rslt.TotalHargaJual.Value)).ToString("N0") 
                                                    : (Convert.ToDecimal(rslt.MountingR) == 0 ? "" : Convert.ToDecimal(rslt.MountingR).ToString("N0"))) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Math.Floor(Convert.ToDecimal(rslt.KadarLogam)).ToString("N0") + "%") + "\",");
                                builder.Append("\"" + enc.EncryptSha(Kode) + "\",");
                                builder.Append("\"" + enc.EncryptSha(rslt.CharFrameColor.Nama) + "\",");
                                builder.Append("\"" + enc.EncryptSha(((rslt.DimensiR.HasValue ? rslt.DimensiR.Value.ToString("N2") : 0.ToString("N0"))) + "sz") + "\",");
                                builder.Append("\"" + enc.EncryptSha(db.CharFrameMaterial.Where(p => p.Nama == "Emas").FirstOrDefault().NamaKode) + "\",");
                                builder.Append("\"" + enc.EncryptSha("") + "\","); // STATUS : DISC / NETT (sementara hanya untuk DJ)

                                //builder.Append("\"" + rslt.Nomor + "\",");
                                //builder.Append("\"" + rslt.CharProductItem.Nama + "\",");
                                //builder.Append("\"" + Convert.ToDecimal(rslt.GrossWeight).ToString("N2") + "\",");
                                //builder.Append("\"" + Convert.ToDecimal((rslt.FixRate == true) ? prc.RoundTop(rslt.TotalHargaJual.Value) : rslt.MountingR).ToString("N0") + "\",");
                                //builder.Append("\"" + Math.Floor(Convert.ToDecimal(rslt.KadarLogam)).ToString("N0") + "%" + "\",");
                                //builder.Append("\"" + Kode + "\",");
                                //builder.Append("\"" + rslt.CharFrameColor.Nama + "\",");
                                //builder.Append("\"" + (rslt.DimensiR.HasValue ? rslt.DimensiR.Value : 0).ToString("N2") + "\",");
                                //builder.Append("\"" + (db.CharFrameMaterial.Where(p => p.Nama == "Emas").FirstOrDefault().NamaKode) + "\",");

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