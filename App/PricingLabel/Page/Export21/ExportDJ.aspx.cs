using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Collections.Generic;

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
        int limitdata = 200;

        string From = Request.QueryString["from"];
        string To = Request.QueryString["to"];
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int tipe = Convert.ToInt32(Request.QueryString["tipe"]);
        int cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        //int clarity = Convert.ToInt32(Request.QueryString["clarity"]);
        int SkipPWP = Convert.ToInt32(Request.QueryString["skip_pwp"]);

        string xTest = "0";

        if (To == "" && From != "") { To = From; }
        if (From == "" && To != "") { From = To; }
        if ((From == "" || To == "" || From == null || To == null) && (ProductItem == null || ProductItem < 1)) DataError();
        if ((From.Length != To.Length)) DataError();

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                string YearFrom = "";
                string YearTo = "";
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
                    YearFrom = From.Substring(1, 2);
                    YearTo = To.Substring(1, 2);
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

                        var dj = (from p in db.StockProductDJ
                                 where (p.Legacy == null || p.Legacy == 0) && p.Draft == false
                                    && p.Nomor.Length == From.Length
                                 select p);

                        xTest = "4A";

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
                            xTest = "4A1";

                            //throw new Exception();

                            // P43172
                            dj = dj.Where(p => p.Nomor.Length == 6);
                            dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(1, 5)) >= Start);
                            dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(1, 5)) <= End);
                            dj = dj.Where(p => (p.Nomor.Substring(0, 1) == "A" // frank
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
                            xTest = "4A2";

                            // 555698
                            dj = dj.Where(p => p.Nomor.Length == 6);

                            dj = dj.Where(p => !p.Nomor.Contains("A")); // frank
                            dj = dj.Where(p => !p.Nomor.Contains("B")); // frank
                            dj = dj.Where(p => !p.Nomor.Contains("G")); // frank
                            dj = dj.Where(p => !p.Nomor.Contains("P")); // frank, palace, mondial
                            dj = dj.Where(p => !p.Nomor.Contains("R")); // frank
                            dj = dj.Where(p => !p.Nomor.Contains("Q")); // mondial

                            dj = dj.Where(p => p.Nomor.Equals(Start.ToString()));
                        }
                        // (New / JAWS) : with first letter, 9 digit
                        else if (From.Length == 9)
                        {
                            // H15002681
                            dj = dj.Where(p => p.Nomor.Substring(1, 2) == YearFrom.ToString()); // A17020031
                            dj = dj.Where(p => p.Nomor.Length == 9);
                            dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start);
                            dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 6)) <= End);
                            if(From == To)
                            {
                                dj = dj.Where(p => p.Nomor.Equals(From));
                            }
                            //dj = dj.Where(p => Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom); // jaga agar tidak salah range tahun
                        }

                        xTest = "4B";

                        if (ProductItem > 0)
                        {
                            dj = dj.Where(p => p.StockProductDJ_CharProduct.ProductItem == ProductItem);
                        }

                        if (ProductCategory > 0)
                        {
                            dj = dj.Where(p => p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
                        }

                        xTest = "4C";

                        if ((tipe >= 0 || tipe <= 2) && cabang > 0)
                        {
                            dj = dj.Where(p => p.StockActualDJ.TipeLokasi == tipe && p.StockActualDJ.IDLokasi == cabang);
                        }

                        xTest = "4D";

                        //if (clarity == 1)
                        //{
                        //    dj = dj.Where(p => p.StockProductDJ_Stone1A.Any(s => s.Stone1A.Parcel104.Nama == "SI"));
                        //}
                        //else if(clarity == 2)
                        //{
                        //    dj = dj.Where(p => p.StockProductDJ_Stone1A.Any(s => s.Stone1A.Parcel104.Nama != "SI"));
                        //}

                        xTest = "5";

                        // urut by nomor plu
                        dj = dj.OrderBy(p => p.Nomor);

                        xTest = "5A";

                        var x = (from p in dj
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
                                    CompanyBrand = p.LocWarehouse.CompanyBrand.NamaKode,
									ExtraRingSize = (Db.SingleInteger("DECLARE @COUNT INT = (SELECT COUNT('') FROM PromoDJ A JOIN PromoDJ_Detail B ON A.ID = B.IDForm WHERE TglStart <= GETDATE() AND TglExpire >= GETDATE() AND B.IDProduct = '" + p.ID + "') IF(@COUNT = 1) BEGIN SELECT TOP 1 CAST(ISNULL(ExtraRingSize,0) AS INT) FROM PromoDJ A JOIN PromoDJ_Detail B ON A.ID = B.IDForm WHERE TglStart <= GETDATE() AND TglExpire >= GETDATE() AND B.IDProduct = '" + p.ID + "' END ELSE BEGIN SELECT 0 END ",Db.Cnn_DB))
                                }).OrderBy(p => p.Nomor);

                        xTest = "5B";

                        if (dj.Count() > 0 && dj.Count() <= limitdata)
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
                            int count = 1;
                            int countskip = 1;
                            bool xIsPWP = false;
                            decimal xHargaPWP = 0m;

                            foreach (var rslt in x)
                            {
                                string xNomor = "";
                                string xKode = "";
                                string xItem = "";
                                string xHarga = "";
                                string xKadar = "";
                                string xStoneQty = "";
                                string xStoneCrt = "";
                                string xNetto = "";
                                string xMaterial = "";
                                string xRingSize = "";
                                string xNettDisc = "";

                                xNomor = rslt.Nomor;
                                if (rslt.Keterangan == "Import Stock")
                                {
                                    xKode = rslt.SupplierNomor;
                                }
                                else
                                {
                                    if (rslt.Supplier.HasValue && rslt.Supplier.Value != 0)
                                    {

                                        //Kode += rslt.SupplierNomor.Substring(0, 1);
                                        int IDGroup = Db.SingleInteger("SELECT IDGroup FROM DataSupplier WHERE ID=" + rslt.Supplier, Db.Cnn_DB);
                                        var sp = Db.SingleString("SELECT NamaKode FROM DataSupplierGroup WHERE ID=" + IDGroup, Db.Cnn_DB);
                                        xKode = sp.Substring(0, 1);
                                    }
                                    xKode += rslt.ProductItem;
                                    xKode += rslt.ProductCategory;
                                    xKode += Convert.ToDateTime(rslt.Tgl).ToString("MM");
                                    xKode += Convert.ToDateTime(rslt.Tgl).ToString("yy");
                                    xKode += "1";
                                }
                                xItem = rslt.ProductItemNama;
                                xHarga = Convert.ToDecimal(rslt.Harga).ToString("N0");
                                xKadar = Math.Floor(rslt.KadarLogam).ToString("N0") + "%";
                                xStoneQty = Convert.ToDecimal(rslt.StoneQty).ToString("N0");
                                xStoneCrt = Convert.ToDecimal(rslt.StoneCarat).ToString("N3");
                                xNetto = Convert.ToDecimal(rslt.NettoWeight).ToString("N2") + "gr";
                                xMaterial = rslt.CharFrameMaterial;
                                xRingSize = "0";
                                try
                                {
                                    xRingSize = Convert.ToDecimal(rslt.ProductDimensiR).ToString("N2");
                                }
                                catch (Exception ex) { }
                                xRingSize += "sz";
                                xNettDisc = (Pricing.isNett(rslt.Nomor)) ? "N" : "D";

                                xTest = "7";
								
								string xExtraRingSize = (rslt.ExtraRingSize > 0) ? " (+" + rslt.ExtraRingSize + " sz)" : "";

                                string querypwp = "DECLARE @ISPWP BIT = 0 " +
                                    "DECLARE @HARGAPWP MONEY = 0 " +
                                    "IF EXISTS (SELECT 1 FROM PromoPWP A JOIN PromoPWP_Detail B ON A.ID = B.IDForm " +
                                    "WHERE B.IDProduct = @IDPRODUCT AND B.TipeProduct = 1 AND A.Status = 1 " +
                                    "AND A.TglStart <= CAST(GETDATE() AS DATE) AND A.TglExpired >= CAST(GETDATE() AS DATE)) " +
                                    "BEGIN " +
                                    "SET @ISPWP = 1 " +
                                    "SET @HARGAPWP = (SELECT TOP 1 ISNULL(B.HargaPWP,0) FROM PromoPWP A JOIN PromoPWP_Detail B ON A.ID = B.IDForm " +
                                    "WHERE B.IDProduct = @IDPRODUCT AND B.TipeProduct = 1 AND A.Status = 1 " +
                                    "AND A.TglStart <= CAST(GETDATE() AS DATE) AND A.TglExpired >= CAST(GETDATE() AS DATE)) " +
                                    "END " +
                                    "SELECT @ISPWP [ISPWP], @HARGAPWP [HARGAPWP] ";
                                List<SqlParameter> parameters = new List<SqlParameter>()
                                {
                                    new SqlParameter("@IDPRODUCT", SqlDbType.Int){ Value = rslt.ID }
                                };
                                DataTable dt1 = Rs(querypwp, Db.Cnn_DB, parameters, CommandType.Text);
                                xIsPWP = Convert.ToBoolean(dt1.Rows[0]["ISPWP"]);
                                xHargaPWP = Convert.ToDecimal(dt1.Rows[0]["HARGAPWP"]);

                                if (SkipPWP == 1)
                                {
                                    if (!xIsPWP)
                                    {
                                        // Builder : Start
                                        if (countskip > 1) { builder.Append("\n"); }

                                        builder.Append("\"" + enc.EncryptSha(xNomor) + "\",");
                                        builder.Append("\"" + enc.EncryptSha(xKode) + "\",");
                                        builder.Append("\"" + enc.EncryptSha(xItem) + "\",");
                                        builder.Append("\"" + enc.EncryptSha(xHarga) + "\",");
                                        builder.Append("\"" + enc.EncryptSha(xKadar) + "\",");
                                        builder.Append("\"" + enc.EncryptSha(xStoneQty) + "\",");
                                        builder.Append("\"" + enc.EncryptSha(xStoneCrt) + "\",");
                                        builder.Append("\"" + enc.EncryptSha(xNetto) + "\",");
                                        builder.Append("\"" + enc.EncryptSha(xMaterial) + "\",");
                                        builder.Append("\"" + enc.EncryptSha(xRingSize + xExtraRingSize) + "\",");
                                        builder.Append("\"" + enc.EncryptSha(xNettDisc) + "\",");

                                        xTest = "8";

                                        // Builder : Stone List
                                        builder.Append("\"0\","); //sudah tidak terpakai tapi tetap harus dibuild

                                        string plunumber = rslt.Nomor;
                                        string stonemodelist = "b"; // b:bandroll / c:certificate
                                        string plutype = "dj"; // dj or ld

                                        /*string spname = "sp_get_stone_information_for_bandroll_certificate";
                                        string query = " exec " + spname
                                                     + " '" + stonemodelist + "'"
                                                     + ",'" + plunumber + "'"
                                                     + ",'" + plutype + "'"
                                                     ;*/

                                        //DataTable dt = new DataTable();
                                        //dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

                                        string query = "sp_get_stone_information_for_bandroll_certificate_report_new";
                                        List<SqlParameter> sqlParameters = new List<SqlParameter>()
                                        {
                                            new SqlParameter { Value = plunumber, ParameterName = "@PLU", SqlDbType = SqlDbType.VarChar },
                                            new SqlParameter { Value = plutype, ParameterName = "@TYPE", SqlDbType = SqlDbType.VarChar },
                                            new SqlParameter { Value = stonemodelist, ParameterName = "@MODE", SqlDbType = SqlDbType.VarChar }
                                        };

                                        DataTable dt = DbAccess.Rs(query, Db.Cnn_DB, sqlParameters);

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

                                        countskip++;
                                    }
                                }
                                else
                                {
                                    // Builder : Start
                                    if (count > 1) { builder.Append("\n"); }

                                    builder.Append("\"" + enc.EncryptSha(xNomor) + "\",");
                                    builder.Append("\"" + enc.EncryptSha(xKode) + "\",");
                                    builder.Append("\"" + enc.EncryptSha(xItem) + "\",");
                                    builder.Append("\"" + enc.EncryptSha(xHarga) + "\",");
                                    builder.Append("\"" + enc.EncryptSha(xKadar) + "\",");
                                    builder.Append("\"" + enc.EncryptSha(xStoneQty) + "\",");
                                    builder.Append("\"" + enc.EncryptSha(xStoneCrt) + "\",");
                                    builder.Append("\"" + enc.EncryptSha(xNetto) + "\",");
                                    builder.Append("\"" + enc.EncryptSha(xMaterial) + "\",");
                                    builder.Append("\"" + enc.EncryptSha(xRingSize + xExtraRingSize) + "\",");
                                    builder.Append("\"" + enc.EncryptSha(xNettDisc) + "\",");

                                    xTest = "8";

                                    // Builder : Stone List
                                    builder.Append("\"0\","); //sudah tidak terpakai tapi tetap harus dibuild

                                    string plunumber = rslt.Nomor;
                                    string stonemodelist = "b"; // b:bandroll / c:certificate
                                    string plutype = "dj"; // dj or ld

                                    /*string spname = "sp_get_stone_information_for_bandroll_certificate";
                                    string query = " exec " + spname
                                                 + " '" + stonemodelist + "'"
                                                 + ",'" + plunumber + "'"
                                                 + ",'" + plutype + "'"
                                                 ;*/

                                    //DataTable dt = new DataTable();
                                    //dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

                                    string query = "sp_get_stone_information_for_bandroll_certificate_report_new";
                                    List<SqlParameter> sqlParameters = new List<SqlParameter>()
                                    {
                                        new SqlParameter { Value = plunumber, ParameterName = "@PLU", SqlDbType = SqlDbType.VarChar },
                                        new SqlParameter { Value = plutype, ParameterName = "@TYPE", SqlDbType = SqlDbType.VarChar },
                                        new SqlParameter { Value = stonemodelist, ParameterName = "@MODE", SqlDbType = SqlDbType.VarChar }
                                    };

                                    DataTable dt = DbAccess.Rs(query, Db.Cnn_DB, sqlParameters);

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
                            }

                            Response.Write(builder.ToString());

                        }
                        else if (dj.Count() > 0 && dj.Count() > limitdata)
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=" + limitdata.ToString();
                            xmsg += "&v=" + "DJ";
                            xmsg += "&it=" + ProductItem;
                            xmsg += "&fr=" + From;
                            xmsg += "&to=" + To;
                            xmsg += "&tp=" + tipe;
                            xmsg += "&cb=" + cabang;

                            Response.Redirect(xmsg);
                        }
                        else
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=0";
                            xmsg += "&v=" + "DJ";
                            xmsg += "&it=" + ProductItem;
                            xmsg += "&fr=" + From;
                            xmsg += "&to=" + To;
                            xmsg += "&tp=" + tipe;
                            xmsg += "&cb=" + cabang;

                            Response.Redirect(xmsg);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            xTest = "Error Code : 0x" + xTest.PadLeft(3, '0')
                  + "<br /><br />Error Message : <span>" + ex.Message.ToString() + "</span>"
                  + "<br /><br />Please contact IT Administrator within the error code number and message."
                  ;

            Response.Write(xTest);
        }
    }
    protected void DataError()
    {
        Response.Redirect("Export.aspx");
    }

    //Data Table Builder
    protected DataTable Rs(string strSql, string CnnString, List<SqlParameter> param, CommandType cmdType)
    {
        DataTable rs = new DataTable();
        using (SqlConnection sqlCnn = new SqlConnection(CnnString))
        {
            sqlCnn.Open();
            using (SqlCommand cmd = new SqlCommand(strSql, sqlCnn))
            {
                cmd.CommandType = cmdType;
                if (param != null)
                {
                    foreach (var x in param)
                    {
                        cmd.Parameters.Add(x);
                    }
                }
                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                {
                    sqlAdapter.SelectCommand.CommandTimeout = 300; // 300 sec or 5 mins for long time query
                    sqlAdapter.Fill(rs);
                }
            }
        }
        return rs;
    }
}