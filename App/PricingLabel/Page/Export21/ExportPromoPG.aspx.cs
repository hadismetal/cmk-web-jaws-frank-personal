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
        string xPromoID = Request.QueryString["id"];
        string xProductID = Request.QueryString["pid"];
        string xLocID = Request.QueryString["setlocid"];

        int PromoID = 0;
        int ProductID = 0;
        int TipeLocationID = -1;
        int LocationID = 0;

        try
        {
            PromoID = Convert.ToInt32(xPromoID);
        }
        catch (Exception) { }

        try
        {
            ProductID = Convert.ToInt32(xProductID);
        }
        catch (Exception) { }

        try
        {
            TipeLocationID = Convert.ToInt32(xLocID.Substring(0, 1));
        }
        catch (Exception) { }

        try
        {
            LocationID = Convert.ToInt32(xLocID.Substring(1));
        }
        catch (Exception) { }

        string xTest = "0";

        try
        {
            if (PromoID > 0)
            {

                xTest = "1";

                // (Old / Avoris) : with plu, 10 digit
                //if (From.Substring(0, 3) == "PLU" && From.Length == 10)
                //{
                //    Start = Convert.ToInt32(From.Substring(3, 7));
                //    End = Convert.ToInt32(To.Substring(3, 7));
                //}
                //// (Old / Speedware) : with first character letter, 6 digit
                //else if (( From.Substring(0, 1) == "A" 
                //        || From.Substring(0, 1) == "B" 
                //        || From.Substring(0, 1) == "G" 
                //        || From.Substring(0, 1) == "P"
                //        || From.Substring(0, 1) == "R"
                //        || From.Substring(0, 1) == "Q"
                //    ) && From.Length == 6)
                //{
                //    Start = Convert.ToInt32(From.Substring(1,5));
                //    End = Convert.ToInt32(To.Substring(1, 5));
                //}
                //// (Old / Speedware) : without letter / all number, 6 digit
                //else if (From.Length == 6)
                //{
                //    Start = Convert.ToInt32(From);
                //    End = Convert.ToInt32(To);
                //}
                //// (New / JAWS) : with first letter, 9 digit
                //else if (From.Length == 9)
                //{
                //    YearFrom = Convert.ToInt32(From.Substring(1, 2));
                //    YearTo = Convert.ToInt32(To.Substring(1, 2));
                //    Start = Convert.ToInt32(From.Substring(3, 6));
                //    End = Convert.ToInt32(To.Substring(3, 6));
                //}

                xTest = "2";

                // Validasi Tahun agar tidak terlalu banyak bandroll
                if (!(PromoID > 0)) DataError();
                else
                {
                    xTest = "3";

                    if (!(PromoID > 0)) DataError();
                    else
                    {
                        xTest = "4";

                        var promoPG = from p in db.PromoPG_Detail
                                      where (p.StockProductPG.Legacy == null || p.StockProductPG.Legacy == 0)
                                      && p.IDForm == PromoID && p.PromoPG.ApprovalNama != null
                                      select p;

                        if (ProductID > 0) { promoPG = promoPG.Where(p => p.IDProduct.Value == ProductID); }

                        xTest = "4A";

                        var PG = from p in db.StockProductPG
                                 join q in promoPG on p.ID equals q.IDProduct
                                 select new { p , q }
                                 ;

                        if (TipeLocationID >= 0 && TipeLocationID <= 2)
                        {
                            PG = PG.Where(p => p.p.StockActualPG.TipeLokasi == TipeLocationID && p.p.StockActualPG.IDLokasi == LocationID);
                        }

                        xTest = "4B";

                        xTest = "5";

                        // urut by nomor plu
                        PG = PG.OrderBy(p => p.p.Nomor);

                        xTest = "5A";

                        var x = PG;

                        xTest = "5B";


                        if (PG.Count() > 0)
                        {
                            xTest = "6";

                            Response.ContentType = "text/csv";
                            Response.AppendHeader("content-disposition", "attachment; filename=PG_"
                                + App.UserNama.Replace(" ", "") + "_"
                                + System.DateTime.Now.Year.ToString().Substring(2)
                                + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Day.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Hour.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Minute.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Second.ToString().PadLeft(2, '0')
                                + "_" + PG.Count().ToString() + ".csv");

                            StringBuilder builder = new StringBuilder();
                            string val = "";
                            int count = 1;

                            xTest = "6A";

                            foreach (var rslt in x)
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

                                xNomor = rslt.p.Nomor;
                                xItem = rslt.p.CharProductItem.Nama;
                                xGross = Convert.ToDecimal(rslt.p.GrossWeight).ToString("N2") + "gr";
                                xHarga = rslt.q.PromoPG.PromoHarga.Value.ToString("N0");
                                xKadar = Math.Floor(Convert.ToDecimal(rslt.p.KadarLogam)).ToString("N0") + "%";
                                if (rslt.p.SupplierNama != "" && rslt.p.SupplierNama != null)
                                {
                                    xKode += rslt.p.SupplierNama.Substring(0, 1);
                                }
                                xKode += rslt.p.CharProductItem.NamaKode;
                                xKode += Convert.ToDateTime(rslt.p.Tgl).ToString("MM");
                                xKode += Convert.ToDateTime(rslt.p.Tgl).ToString("yy");
                                xKode += "1";
                                xColor = rslt.p.CharFrameColor.Nama;
                                xDimR = ((rslt.p.DimensiR.HasValue ? rslt.p.DimensiR.Value.ToString("N2") : 0.ToString("N0"))) + "sz";
                                xMaterial = db.CharFrameMaterial.Where(p => p.Nama == "Emas").FirstOrDefault().NamaKode;
                                xRingSize = "";
								
								ExtraRingSize = 0;//Db.SingleInteger("DECLARE @COUNT INT = (SELECT COUNT('') FROM PromoPG A JOIN PromoPG_Detail B ON A.ID = B.IDForm WHERE /*TglStart <= GETDATE() AND*/ TglExpire >= GETDATE() AND B.IDProduct = '" + rslt.ID + "') IF(@COUNT = 1) BEGIN SELECT TOP 1 CAST(ISNULL(ExtraRingSize,0) AS INT) FROM PromoPG A JOIN PromoPG_Detail B ON A.ID = B.IDForm WHERE /*TglStart <= GETDATE() AND*/ TglExpire >= GETDATE() AND B.IDProduct = '" + rslt.ID + "' END ELSE BEGIN SELECT 0 END ",Db.Cnn_DB);
						
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
                        else
                        {
                            Response.Redirect("ExportPromo.aspx?msg=noplu");
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
        Response.Redirect("ExportPromo.aspx");
    }
}