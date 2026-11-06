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
        int limitdata = 200;

        string From = Request.QueryString["from"];
        string To = Request.QueryString["to"];
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int tipe = Convert.ToInt32(Request.QueryString["tipe"]);
        int cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        int supplier = Convert.ToInt32(Request.QueryString["supplier"]);
        string kodeBarang = Request.QueryString["kobar"];
        int ProductLevel = Convert.ToInt32(Request.QueryString["level"]);

        string xTest = "0";

        if (To == "" && From != "") { To = From; }
        if (From == "" && To != "") { From = To; }
        //if ((From == "" || To == "" || From == null || To == null) && (ProductItem == null || ProductItem < 1)) DataError();
        if ((From.Length != To.Length)) DataError();

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                string YearFrom = "";
                string YearTo = "";
                int Start = 1;
                int End = 1;
                if (From.Substring(0, 2).ToUpper() != "ZN" && From.Substring(0, 2).ToUpper() != "GP" && From.Substring(0, 3).ToUpper() != "VRS")
                {
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
                }

                if (YearFrom != YearTo) DataError();
                else
                {
                    if (Start > End) DataError();
                    else
                    {
                        var pg = from p in db.StockProductPG
                                 where 1 == 1 && p.KodeBarang.Contains(kodeBarang)  && p.ReturnProduct == false && p.StatusLebur == false && !p.Draft.Value
                                 select p;

                        if (From.Substring(0, 2).ToUpper() != "ZN" && From.Substring(0, 2).ToUpper() != "GP" && From.Substring(0, 3).ToUpper() != "VRS")
                        {
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
                                xTest = "4A2";

                                // 555698
                                pg = pg.Where(p => p.Nomor.Length == 6);

                                pg = pg.Where(p => !p.Nomor.Contains("A")); // frank
                                pg = pg.Where(p => !p.Nomor.Contains("B")); // frank
                                pg = pg.Where(p => !p.Nomor.Contains("G")); // frank
                                pg = pg.Where(p => !p.Nomor.Contains("P")); // frank, palace, mondial
                                pg = pg.Where(p => !p.Nomor.Contains("R")); // frank
                                pg = pg.Where(p => !p.Nomor.Contains("Q")); // mondial

                                pg = pg.Where(p => Convert.ToInt32("1" + p.Nomor.Substring(0, 6)) >= Convert.ToInt32("1" + Start.ToString().PadLeft(6, '0')));
                                pg = pg.Where(p => Convert.ToInt32("1" + p.Nomor.Substring(0, 6)) <= Convert.ToInt32("1" + End.ToString().PadLeft(6, '0')));

                            }
                            // (New / JAWS) : with first letter, 9 digit
                            else if (From.Length == 9)
                            {
                                // H15002681
                                pg = pg.Where(p => p.Nomor.Substring(1, 2) == YearFrom.ToString()); // A17020031
                                pg = pg.Where(p => p.Nomor.Length == 9);
                                pg = pg.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start);
                                pg = pg.Where(p => Convert.ToInt32(p.Nomor.Substring(3, 6)) <= End);
                                //pg = pg.Where(p => Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom); // jaga agar tidak salah range tahun
                            }
                        }
                        else if (From.Substring(0, 2).ToUpper() == "ZN" || From.Substring(0, 2).ToUpper() == "GP" || From.Substring(0, 3).ToUpper() == "VRS")
                        {
                            pg = pg.Where(p => p.Nomor.Contains(From));
                        }

                        if (ProductItem > 0)
                        {
                            pg = pg.Where(p => p.ProductItem == ProductItem);
                        }

                        if (supplier > 0)
                        {
                            pg = pg.Where(p => p.IDSupplier == supplier);
                        }
                        if(ProductLevel > 0)
                        {
                            pg = pg.Where(p => p.ProductLevel == ProductLevel);
                        }
                        
                        if ((tipe >= 0 || tipe <= 2) && cabang > 0)
                        {
                            pg = pg.Where(p => p.StockActualPG.TipeLokasi == tipe && p.StockActualPG.IDLokasi == cabang);
                        }

                        // urut by nomor plu
                        pg = pg.OrderBy(p => p.Nomor);

                        if (pg.Count() > 0 && pg.Count() <= limitdata)
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

                            int count = 1;
                            foreach (var rslt in pg)
                            {
                                string xNomor = "";
                                string xItem = "";
                                string xGross = "";
                                string xHarga = "";
                                string xKadar = "";
                                string xKode = "";
                                string xColor = "";
                                string xDimR = "";
                                string xMaterial = "";
                                string xRingSize = "";

                                xNomor = rslt.Nomor;
                                xItem = rslt.CharProductItem.Nama;
                                // if (rslt.FixRate == true) {
                                //     xGross = Convert.ToDecimal(rslt.GrossWeight).ToString("N2") + "gr";
                                // }
                                xGross = Convert.ToDecimal(rslt.GrossWeight).ToString("N2") + "gr";
                                xHarga = (rslt.FixRate == true)
                                       ? prc.RoundTop(Convert.ToDecimal(rslt.TotalHargaJual.Value)).ToString("N0")
                                       : (Convert.ToDecimal(rslt.MountingR) == 0 ? "" : Convert.ToDecimal(rslt.MountingR).ToString("N0"))
                                       ;
                                xKadar = Math.Floor(Convert.ToDecimal(rslt.KadarLogam)).ToString("N0") + "%";
                                if (From.Substring(0, 2).ToUpper() == "ZN" || From.Substring(0, 2).ToUpper() == "GP" || From.Substring(0, 3).ToUpper() == "VRS")
                                {
                                    xKode = rslt.KodeBarang;
                                }   
                                else
                                {
                                    if (rslt.SupplierNama != "" && rslt.SupplierNama != null)
                                    {
                                        xKode += rslt.SupplierNama.Substring(0, 1);
                                    }
                                    xKode += rslt.CharProductItem.NamaKode;
                                    xKode += Convert.ToDateTime(rslt.Tgl).ToString("MM");
                                    xKode += Convert.ToDateTime(rslt.Tgl).ToString("yy");
                                    xKode += "1";
                                }
                                xColor = rslt.CharFrameColor.Nama;
                                xDimR = ((rslt.DimensiR.HasValue ? rslt.DimensiR.Value.ToString("N2") : 0.ToString("N0"))) + "sz";
                                xMaterial = db.CharFrameMaterial.Where(p => p.Nama == "Emas").FirstOrDefault().NamaKode;
                                xRingSize = "";

                                if (count > 1) { builder.Append("\n"); }

                                builder.Append("\"" + enc.EncryptSha(xNomor) + "\",");
                                builder.Append("\"" + enc.EncryptSha(xItem) + "\",");
                                builder.Append("\"" + enc.EncryptSha(xGross) + "\",");
                                builder.Append("\"" + enc.EncryptSha(xHarga) + "\",");
                                builder.Append("\"" + enc.EncryptSha(xKadar) + "\",");
                                builder.Append("\"" + enc.EncryptSha(xKode) + "\",");
                                builder.Append("\"" + enc.EncryptSha(xColor) + "\",");
                                builder.Append("\"" + enc.EncryptSha(xDimR) + "\",");
                                builder.Append("\"" + enc.EncryptSha(xMaterial) + "\",");
                                builder.Append("\"" + enc.EncryptSha(xRingSize) + "\",");

                                count++;
                            }

                            Response.Write(builder.ToString());

                        }
                        else if (pg.Count() > 0 && pg.Count() > limitdata)
                        {
                            string xmsg = "";
                            xmsg = "Export.aspx?msg=" + limitdata;
                            xmsg += "&v=" + "PG";
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
                            xmsg += "&v=" + "PG";
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
            else
            {
                var pg = from p in db.StockProductPG
                         join q in db.StockActualPG on p.ID equals q.IDProduct
                         where (p.Legacy == null || p.Legacy == 0) && p.Draft == false && q.StockProductPG.ReturnProduct == false && q.StockProductPG.StatusLebur == false && !q.StockProductPG.Draft.Value
                            && !(from s in db.SalesOrderPG
                                 where s.StatusResell.Value == 0 && s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true
                                 select s.IDProduct).Contains(q.IDProduct)
                            && !(from s in db.SalesOrderSouvenir
                                 where s.SalesOrder.StatusVoid == false && s.SalesOrder.StatusPembayaran == true && s.TipeProduct == 3
                                 select s.IDProductSouvenir).Contains(q.IDProduct)
                            && 1 == 1
                         //&& p.Nomor.Length == From.Length
                         select p;
                if (ProductItem > 0)
                {
                    pg = pg.Where(p => p.ProductItem == ProductItem);
                }

                if(ProductLevel > 0)
                {
                    pg = pg.Where(p => p.ProductLevel == ProductLevel);
                }
                if ((tipe >= 0 || tipe <= 2) && cabang > 0)
                {
                    pg = pg.Where(p => p.StockActualPG.TipeLokasi == tipe && p.StockActualPG.IDLokasi == cabang);
                }

                // urut by nomor plu
                pg = pg.OrderBy(p => p.Nomor);

                if (pg.Count() > 0 /*&& pg.Count() <= limitdata*/)
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

                    int count = 1;
                    foreach (var rslt in pg)
                    {
                        string xNomor = "";
                        string xItem = "";
                        string xGross = "";
                        string xHarga = "";
                        string xKadar = "";
                        string xKode = "";
                        string xColor = "";
                        string xDimR = "";
                        string xMaterial = "";
                        string xRingSize = "";
						int ExtraRingSize = 0;

                        xNomor = rslt.Nomor;
                        xItem = rslt.CharProductItem.Nama;
                        // if (rslt.FixRate == true) {
                        //     xGross = Convert.ToDecimal(rslt.GrossWeight).ToString("N2") + "gr";
                        // }
                        xGross = Convert.ToDecimal(rslt.GrossWeight).ToString("N2") + "gr";
                        xHarga = (rslt.FixRate == true)
                               ? prc.RoundTop(Convert.ToDecimal(rslt.TotalHargaJual.Value)).ToString("N0")
                               : (Convert.ToDecimal(rslt.MountingR) == 0 ? "" : Convert.ToDecimal(rslt.MountingR).ToString("N0"))
                               ;
                        xKadar = Math.Floor(Convert.ToDecimal(rslt.KadarLogam)).ToString("N0") + "%";

                        if (xNomor.Substring(0, 2).ToUpper() == "ZN" || xNomor.Substring(0, 2).ToUpper() == "GP" || xNomor.Substring(0, 3).ToUpper() == "VRS")
                        {
                            xKode = rslt.KodeBarang;
                        }   
                        else
                        {
                            if (rslt.SupplierNama != "" && rslt.SupplierNama != null)
                            {
                                xKode += rslt.SupplierNama.Substring(0, 1);
                            }
                            xKode += rslt.CharProductItem.NamaKode;
                            xKode += Convert.ToDateTime(rslt.Tgl).ToString("MM");
                            xKode += Convert.ToDateTime(rslt.Tgl).ToString("yy");
                            xKode += "1";
                        }
                        xColor = rslt.CharFrameColor.Nama;
                        xDimR = ((rslt.DimensiR.HasValue ? rslt.DimensiR.Value.ToString("N2") : 0.ToString("N0"))) + "sz";
                        xMaterial = db.CharFrameMaterial.Where(p => p.Nama == "Emas").FirstOrDefault().NamaKode;
                        xRingSize = "";

						ExtraRingSize = Db.SingleInteger("DECLARE @COUNT INT = (SELECT COUNT('') FROM PromoPG A JOIN PromoPG_Detail B ON A.ID = B.IDForm WHERE TglStart <= GETDATE() AND TglExpire >= GETDATE() AND B.IDProduct = '" + rslt.ID + "') IF(@COUNT = 1) BEGIN SELECT TOP 1 CAST(ISNULL(ExtraRingSize,0) AS INT) FROM PromoPG A JOIN PromoPG_Detail B ON A.ID = B.IDForm WHERE TglStart <= GETDATE() AND TglExpire >= GETDATE() AND B.IDProduct = '" + rslt.ID + "' END ELSE BEGIN SELECT 0 END ",Db.Cnn_DB);
						
						string xExtraRingSize = (ExtraRingSize > 0) ? " (+" + ExtraRingSize + " sz)" : "";																						  
                        if (count > 1) { builder.Append("\n"); }

                        builder.Append("\"" + enc.EncryptSha(xNomor) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xItem) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xGross) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xHarga) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xKadar) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xKode) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xColor) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xDimR) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xMaterial) + "\",");
                        builder.Append("\"" + enc.EncryptSha(xRingSize + xExtraRingSize) + "\",");


                        count++;
                    }

                    Response.Write(builder.ToString());

                }
                /*else if (pg.Count() > 0 && pg.Count() > limitdata)
				{
					string xmsg = "";
					xmsg = "Export.aspx?msg=" + limitdata;
					xmsg += "&v=" + "PG";
					xmsg += "&it=" + ProductItem;
					xmsg += "&fr=" + From;
					xmsg += "&to=" + To;
					xmsg += "&tp=" + tipe;
					xmsg += "&cb=" + cabang;

					Response.Redirect(xmsg);
				}*/
                else
                {

                    string xmsg = "";
                    xmsg = "Export.aspx?msg=0";
                    xmsg += "&v=" + "PG";
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