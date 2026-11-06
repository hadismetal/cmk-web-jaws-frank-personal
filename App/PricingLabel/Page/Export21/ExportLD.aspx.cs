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
        int limitdata = 200;

        string From = Request.QueryString["from"];
        string To = Request.QueryString["to"];
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int tipe = Convert.ToInt32(Request.QueryString["tipe"]);
        int cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        //int clarity = Convert.ToInt32(Request.QueryString["clarity"]);

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
                    YearFrom = From.Substring(1, 2);
                    YearTo = To.Substring(1, 2);
                    Start = Convert.ToInt32(From.Substring(3, 6));
                    End = Convert.ToInt32(To.Substring(3, 6));
                }

                if (YearFrom != YearTo) DataError();
                else
                {
                    if (Start > End) DataError();
                    else
                    {
                        var ld = from p in db.StockProductLD_Stone1B
                                 where 1 == 1
                                    && p.Nomor.Length == From.Length
                                 select p;

                        // (Old / Avoris) : with plu, 10 digit
                        if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                        {
                            // PLU0023827
                            ld = ld.Where(p => p.Nomor.Length == 10);
                            ld = ld.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 7)) >= Start);
                            ld = ld.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 7)) <= End);
                            ld = ld.Where(p => p.Nomor.Substring(0, 3) == "PLU");
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
                            ld = ld.Where(p => p.Nomor.Length == 6);
                            ld = ld.Where(p => Convert.ToInt32(p.Nomor.Substring(1, 5)) >= Start);
                            ld = ld.Where(p => Convert.ToInt32(p.Nomor.Substring(1, 5)) <= End);
                            ld = ld.Where(p => (p.Nomor.Substring(0, 1) == "A" // frank
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
                            xTest = "4A2";

                            // 555698
                            ld = ld.Where(p => p.Nomor.Length == 6);

                            ld = ld.Where(p => !p.Nomor.Contains("A")); // frank
                            ld = ld.Where(p => !p.Nomor.Contains("B")); // frank
                            ld = ld.Where(p => !p.Nomor.Contains("G")); // frank
                            ld = ld.Where(p => !p.Nomor.Contains("P")); // frank, palace, mondial
                            ld = ld.Where(p => !p.Nomor.Contains("R")); // frank
                            ld = ld.Where(p => !p.Nomor.Contains("Q")); // mondial

                            ld = ld.Where(p => Convert.ToInt32("1" + p.Nomor.Substring(0, 6)) >= Convert.ToInt32("1" + Start.ToString().PadLeft(6, '0')));
                            ld = ld.Where(p => Convert.ToInt32("1" + p.Nomor.Substring(0, 6)) <= Convert.ToInt32("1" + End.ToString().PadLeft(6, '0')));
                        
                        }
                        // (New / JAWS) : with first letter, 9 digit
                        else if (From.Length == 9)
                        {
                            // H15002681
			    ld = ld.Where(p => p.Nomor.Substring(1, 2) == YearFrom.ToString());
                            ld = ld.Where(p => p.Nomor.Length == 9);
                            ld = ld.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start);
                            ld = ld.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 6)) <= End);
                            //ld = ld.Where(p => Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom); // jaga agar tidak salah range tahun
                        }

                        if ((tipe >= 0 || tipe <= 2) && cabang > 0)
                        {
                            ld = ld.Where(p => p.StockActualLD_Stone1B.TipeLokasi == tipe && p.StockActualLD_Stone1B.IDLokasi == cabang);
                        }

                        // urut by nomor plu
                        ld = ld.OrderBy(p => p.Nomor);

                        if (ld.Count() > 0 && ld.Count() <= limitdata)
                        {
                            Response.ContentType = "text/csv";
                            Response.AppendHeader("content-disposition", "attachment; filename=LD_"
                                + App.UserNama.Replace(" ", "") + "_"
                                + System.DateTime.Now.Year.ToString().Substring(2)
                                + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Day.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Hour.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Minute.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Second.ToString().PadLeft(2, '0')
                                + "_" + ld.Count().ToString() + ".csv");

                            StringBuilder builder = new StringBuilder();
                            int count = 1;
                            
                            foreach (var rslt in ld)
                            {
                                string xNomor = rslt.Nomor;
                                string xGIA = rslt.GIA;
                                string xSCCB = rslt.Stone1B.Parcel101.NamaKode + rslt.Stone1B.Parcel103.NamaKode + rslt.Stone1B.Parcel104.NamaKode + rslt.Stone1B.Parcel106.NamaKode;

                                // Builder : Start
                                if (count > 1) { builder.Append("\n"); }

                                builder.Append("\"" + enc.EncryptSha(xNomor) + "\",");
                                builder.Append("\"" + enc.EncryptSha(xGIA) + "\",");
                                builder.Append("\"" + enc.EncryptSha(xSCCB) + "\",");
                                
                                // Builder : Stone List

                                string plunumber = xNomor;
                                string stonemodelist = "b"; // b:bandroll / c:certificate
                                string plutype = "ld"; // dj or ld

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


                                builder.Append("\"" + enc.EncryptSha(rslt.Stone1B.Parcel106.Nama.ToString()) + "\",");

                                count++;
                            }

                            Response.Write(builder.ToString());

                        }
                        else if (ld.Count() > 0 && ld.Count() > limitdata)
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=" + limitdata.ToString();
                            xmsg += "&v=" + "LD";
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
                            xmsg += "&v=" + "LD";
                            xmsg += "&it=" + ProductItem;
                            xmsg += "&fr=" + From;
                            xmsg += "&to=" + To;
                            xmsg += "&tp=" + tipe;
                            xmsg += "&cb=" + cabang;

                            Response.Redirect(xmsg);

                            //Response.Redirect("Export.aspx?msg=0");
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
}