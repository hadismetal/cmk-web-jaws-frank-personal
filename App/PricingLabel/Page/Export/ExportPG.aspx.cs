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
            Response.ContentType = "text/csv";

            Response.AppendHeader("content-disposition", "attachment; filename=PLU_PG.csv");
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

        if ((From == "" || To == "" || From == null || To == null) && (ProductItem == null || ProductItem < 1)) DataError();

        try
        {
            if (From != "" && From != null && To != "" && To != null)
            {
                int YearFrom = 1;
                int YearTo = 1;
                int Start = 1;
                int End = 1;

                if (From.Length == 9)
                {
                    YearFrom = Convert.ToInt32(From.Substring(1, 2));
                    YearTo = Convert.ToInt32(To.Substring(1, 2));
                    Start = Convert.ToInt32(From.Substring(3, 6));
                    End = Convert.ToInt32(To.Substring(3, 6));
                }
                else if(From.Contains("PLU"))
                {
                    Start = Convert.ToInt32(From.Substring(3, 7));
                    End = Convert.ToInt32(To.Substring(3, 7));
                }
                else if(From.Length == 6)
                {
                    Start = Convert.ToInt32(From.Substring(1, 5));
                    End = Convert.ToInt32(To.Substring(1, 5));
                }
                //B18121
                //B14000420

                if (YearFrom != YearTo) DataError();
                else
                {
                    if (Start > End) DataError();
                    else
                    {
                        var pg = from p in db.StockProductPG
                                 where (p.Nomor.Contains("PLU") && p.Nomor.Substring(0,3) == "PLU") ?
                                 Convert.ToInt32(p.Nomor.Substring(3, 7)) >= Start &&
                                 Convert.ToInt32(p.Nomor.Substring(3, 7)) <= End :
                                 p.Nomor.Length == 6 ?
                                     Convert.ToInt32(p.Nomor.Substring(1, 5)) >= Start &&
                                     Convert.ToInt32(p.Nomor.Substring(1, 5)) <= End &&
                                     p.Nomor.Substring(0, 1) == From.Substring(0,1) : 
                                (p.Nomor.Length == 9 && p.Legacy == null) ?
                                 Convert.ToInt32(p.Nomor.Substring(1, 2)) == YearFrom &&
                                 Convert.ToInt32(p.Nomor.Substring(3, 6)) >= Start &&
                                 Convert.ToInt32(p.Nomor.Substring(3, 6)) <= End
                                 : 1!=1
                                 select p;
                        if (ProductItem > 0)
                        {
                            pg = pg.Where(p => p.ProductItem == ProductItem);
                        }
                        if (tipe > 0 && cabang > 0)
                        {
                            pg = pg.Where(p => p.StockActualPG.TipeLokasi == tipe && p.StockActualPG.IDLokasi == cabang);
                        }


                        if (pg.Count() > 0)
                        {
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
                                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.GrossWeight).ToString("N2")) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal((rslt.FixRate == true) ? prc.RoundTop(rslt.TotalHargaJual.Value) : rslt.MountingR).ToString("N0")) + "\",");
                                builder.Append("\"" + enc.EncryptSha(Math.Floor(Convert.ToDecimal(rslt.KadarLogam)).ToString("N0") + "%") + "\",");
                                builder.Append("\"" + enc.EncryptSha(Kode) + "\",");
                                builder.Append("\"" + enc.EncryptSha(rslt.CharFrameColor.Nama) + "\",");
                                builder.Append("\"" + enc.EncryptSha((rslt.DimensiR.HasValue ? rslt.DimensiR.Value : 0).ToString("N2")) + "\",");
                                count++;
                            }
                            Response.Write(builder.ToString());
                        }
                    }
                }
            }
            else
            {
                var pg = from p in db.StockProductPG
                         where p.ProductItem == ProductItem
                         select p;
                if (ProductItem > 0)
                {
                    pg = pg.Where(p => p.ProductItem == ProductItem);
                }
                if (tipe > 0 && cabang > 0)
                {
                    pg = pg.Where(p => p.StockActualPG.TipeLokasi == tipe && p.StockActualPG.IDLokasi == cabang);
                }


                if (pg.Count() > 0)
                {
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
                        builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal(rslt.GrossWeight).ToString("N2")) + "\",");
                        builder.Append("\"" + enc.EncryptSha(Convert.ToDecimal((rslt.FixRate == true) ? prc.RoundTop(rslt.TotalHargaJual.Value) : rslt.MountingR).ToString("N0")) + "\",");
                        builder.Append("\"" + enc.EncryptSha(Math.Floor(Convert.ToDecimal(rslt.KadarLogam)).ToString("N0") + "%") + "\",");
                        builder.Append("\"" + enc.EncryptSha(Kode) + "\",");
                        builder.Append("\"" + enc.EncryptSha(rslt.CharFrameColor.Nama) + "\",");
                        builder.Append("\"" + enc.EncryptSha((rslt.DimensiR.HasValue ? rslt.DimensiR.Value : 0).ToString("N2")) + "\",");
                        count++;
                    }
                    Response.Write(builder.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.ToString());
            //DataError();
        }
    }
    protected void DataError()
    {
        Response.Redirect("Export.aspx");
    }
}