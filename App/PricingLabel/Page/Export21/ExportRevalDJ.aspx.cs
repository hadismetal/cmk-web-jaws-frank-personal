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

    protected int limitdata = 750;

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
		db.CommandTimeout = 1000;
        int ProductItem = Convert.ToInt32(Request.QueryString["productitem"]);
        int ProductCategory = Convert.ToInt32(Request.QueryString["productcategory"]);
        int ProductLevel = Convert.ToInt32(Request.QueryString["productlevel"]);
        int tipe = Convert.ToInt32(Request.QueryString["tipe"]);
        int cabang = Convert.ToInt32(Request.QueryString["cabang"]);
        int Clarity = Convert.ToInt32(Request.QueryString["clarity"]);
        
        int IDProduct = 0;

        try
        {
            IDProduct = Convert.ToInt32(Request.QueryString["idproduct"]);
        }
        catch (Exception) { }

        string xTest = "0";

        try
        {

            if (1 == 1)
            {
                xTest = "4";

                var rv = from p in db.StockProductDJ
                         join q in db.StockActualDJ on p.ID equals q.IDProduct
                         join r in db.RevalItemDJ on q.IDProduct equals r.IDProduct
                         join s in db.RevalProductDJ on r.ID equals s.IDReval
                         where (p.Legacy == null || p.Legacy == 0) && p.Draft == false
                         select new { p, q, r, s };

                if (IDProduct > 0)
                {
                    rv = rv.Where(p => p.p.ID == IDProduct);
                }

                if (Clarity == 1)
                {
                    //rv = from p in rv
                    //     where !p.s.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("W4") && !z.Stone1A.Nama.Contains("B4"))
                    //     select p;

                    rv = rv.Where(p => !p.s.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("W4"))
                                && !p.s.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("B4"))
                                
                                );
                }
                if (Clarity == 2)
                {
                    rv = rv.Where(p => p.s.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("W4")));
                }
                else if (Clarity == 3)
                {
                    rv = rv.Where(p => p.s.RevalProductDJ_Stone1A.Any(z => z.Stone1A.Nama.Contains("B4")));
                }

                if (ProductItem > 0)
                {
                    rv = rv.Where(p => p.p.StockProductDJ_CharProduct.ProductItem == ProductItem);
                }

                if (ProductCategory > 0)
                {
                    rv = rv.Where(p => p.p.StockProductDJ_CharProduct.ProductCategory == ProductCategory);
                }

                if (ProductLevel > 0)
                {
                    rv = rv.Where(p => p.p.StockProductDJ_CharProduct.ProductLevel == ProductLevel);
                }

                xTest = "4C";

                if ((tipe >= 0 || tipe <= 2) && cabang > 0)
                {
                    rv = rv.Where(p => p.p.StockActualDJ.TipeLokasi == tipe && p.p.StockActualDJ.IDLokasi == cabang);
                }

                var w = (from p in db.PromoDJ_Detail
                         join q in db.PromoDJ on p.IDForm equals q.ID
                         where (from y in rv select y.p.ID).Contains(p.IDProduct.Value)
                             && p.PromoDJ.TglExpire >= System.DateTime.Now
                         select new { xPromoHarga = q.PromoHarga, xIDProduct = p.IDProduct })
                        ;

                var m = from p in rv
                        select new
                        {
                            SupplierNomor = p.p.SupplierNomor,
                            Supplier = p.p.Supplier,
                            ProductItem = p.p.StockProductDJ_CharProduct.CharProductItem.NamaKode,
                            ProductItemNama = p.p.StockProductDJ_CharProduct.CharProductItem.Nama,
                            ProductCategory = p.p.StockProductDJ_CharProduct.CharProductCategory.NamaKode,
                            Tgl = p.p.Tgl,
                            ID = p.p.ID,
                            ProductDimensiR = p.p.StockProductDJ_CharProduct.DimensiR,
                            Nomor = p.p.Nomor,
                            Harga = p.p.StockProductDJ_PricingMU.HargaRupiahRound,
                            KadarLogam = p.p.StockProductDJ_CharProduct.KadarLogam,
                            StoneQty = p.p.StockProductDJ_CharProduct.StoneQty,
                            StoneCarat = p.p.StockProductDJ_CharProduct.StoneCarat,
                            NettoWeight = p.p.StockProductDJ_CharProduct.NettoWeight,
                            CharFrameMaterial = p.p.StockProductDJ_CharProduct.CharFrameMaterial.NamaKode,
                            Keterangan = p.p.Keterangan,
                            ProductLevel = p.p.StockProductDJ_CharProduct.CharProductLevel.NamaKode,
                            CompanyBrand = p.p.LocWarehouse.CompanyBrand.NamaKode,

                            //IDProduct = p.r.IDProduct,
                            //PLU = p.p.Nomor,
                            //ProductItem = p.p.StockProductDJ_CharProduct.ProductItem,
                            //ProductCategory = p.p.StockProductDJ_CharProduct.ProductCategory,
                            //Location = p.q.NamaLokasi,
                            //Harga = p.p.StockProductDJ_PricingMU.HargaRupiahRound,
                            HargaReval = p.s.RevalProductDJ_PricingMU.HargaRupiahRound,
                            HargaPromo = (w.FirstOrDefault(q => q.xIDProduct == p.r.IDProduct).xPromoHarga),
                            StatusJual = p.p.StatusPenjualan
                        }
                        ;

                m = m.Where(p => p.StatusJual == 0 && p.HargaPromo == null && (decimal)p.HargaReval != (decimal)p.Harga);

                var x = from p in m
                        select new
                        {
                            SupplierNomor = p.SupplierNomor,
                            Supplier = p.Supplier,
                            ProductItem = p.ProductItem,
                            ProductItemNama = p.ProductItemNama,
                            ProductCategory = p.ProductCategory,
                            Tgl = p.Tgl,
                            ID = p.ID,
                            ProductDimensiR = p.ProductDimensiR,
                            Nomor = p.Nomor,
                            Harga = p.HargaReval,
                            KadarLogam = p.KadarLogam,
                            StoneQty = p.StoneQty,
                            StoneCarat = p.StoneCarat,
                            NettoWeight = p.NettoWeight,
                            CharFrameMaterial = p.CharFrameMaterial,
                            Keterangan = p.Keterangan,
                            ProductLevel = p.ProductLevel,
                            CompanyBrand = p.CompanyBrand
                        }
                        ;

                x = x.OrderBy(p => p.Nomor);
                
                xTest = "5B";

                if (x.Count() > 0 && x.Count() <= limitdata)
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
                        + "_" + x.Count().ToString() + ".csv");

                    StringBuilder builder = new StringBuilder();
                    int count = 1;

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
                        builder.Append("\"" + enc.EncryptSha(xRingSize) + "\",");
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

                    Response.Write(builder.ToString());

                }
                else if (rv.Count() > 0 && rv.Count() > limitdata)
                {
                    string xmsg = "";
                    xmsg = "ExportReval.aspx?msg=" + limitdata.ToString();
                    xmsg += "&v=" + "DJ";
                    xmsg += "&it=" + ProductItem;
                    xmsg += "&ct=" + ProductCategory;
                    xmsg += "&lv=" + ProductLevel;
                    xmsg += "&tp=" + tipe;
                    xmsg += "&cb=" + cabang;

                    Response.Redirect(xmsg);
                }
                else
                {
                    string xmsg = "";
                    xmsg = "ExportReval.aspx?msg=0";
                    xmsg += "&v=" + "DJ";
                    xmsg += "&it=" + ProductItem;
                    xmsg += "&ct=" + ProductCategory;
                    xmsg += "&lv=" + ProductLevel;
                    xmsg += "&tp=" + tipe;
                    xmsg += "&cb=" + cabang;

                    Response.Redirect(xmsg);
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
        Response.Redirect("ExportReval.aspx");
    }
}