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
            TipeLocationID = Convert.ToInt32(xLocID.Substring(0,1));
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

                        var promoGJ = from p in db.PromoGJ_Detail
                                      where 1==1 //(p.StockProductGJ.Legacy == null || p.StockProductGJ.Legacy == 0)
                                      && p.IDForm == PromoID && p.PromoGJ.ApprovalNama != null
                                      select p;

                        if (ProductID > 0) { promoGJ = promoGJ.Where(p => p.IDProduct.Value == ProductID); }

                        xTest = "4A";

                        var GJ = from p in db.StockProductGJ
                                 join q in promoGJ on p.ID equals q.IDProduct
                                 select new
                                 {
                                     SupplierNomor = p.SupplierNomor,
                                     Supplier = p.Supplier,
                                     ProductItem = p.StockProductGJ_CharProduct.CharProductItem.NamaKode,
                                     ProductItemNama = p.StockProductGJ_CharProduct.CharProductItem.Nama,
                                     ProductCategory = p.StockProductGJ_CharProduct.CharProductCategory.NamaKode,
                                     Tgl = p.Tgl,
                                     ID = p.ID,
                                     ProductDimensiR = p.StockProductGJ_CharProduct.DimensiR,
                                     Nomor = p.Nomor,
                                     Harga = q.PromoGJ.PromoHarga,
                                     //Harga = p.StockProductGJ_PricingMU.HargaRupiahRound,
                                     KadarLogam = p.StockProductGJ_CharProduct.KadarLogam,
                                     StoneQty = p.StockProductGJ_CharProduct.StoneQty,
                                     StoneCarat = p.StockProductGJ_CharProduct.StoneCarat,
                                     NettoWeight = p.StockProductGJ_CharProduct.NettoWeight,
                                     CharFrameMaterial = p.StockProductGJ_CharProduct.CharFrameMaterial.NamaKode,
                                     Keterangan = p.Keterangan,
                                     ProductLevel = p.StockProductGJ_CharProduct.CharProductLevel.NamaKode,
                                     CompanyBrand = p.LocWarehouse.CompanyBrand.NamaKode,
                                     TipeLokasi = p.StockActualGJ.TipeLokasi,
                                     IDLokasi = p.StockActualGJ.IDLokasi
                                 }
                                 ;

                        if (TipeLocationID >= 0 && TipeLocationID <= 2)
                        {
                            GJ = GJ.Where(p => p.TipeLokasi == TipeLocationID && p.IDLokasi == LocationID);
                        }

                        xTest = "4B";


                        xTest = "5";

                        // urut by nomor plu
                        GJ = GJ.OrderBy(p => p.Nomor);

                        xTest = "5A";

                        var x = GJ;

                        xTest = "5B";


                        if (GJ.Count() > 0)
                        {
                            xTest = "6";

                            Response.ContentType = "text/csv";
                            Response.AppendHeader("content-disposition", "attachment; filename=GJ_"
                                + App.UserNama.Replace(" ", "") + "_"
                                + System.DateTime.Now.Year.ToString().Substring(2)
                                + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Day.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Hour.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Minute.ToString().PadLeft(2, '0')
                                + System.DateTime.Now.Second.ToString().PadLeft(2, '0')
                                + "_" + GJ.Count().ToString() + ".csv");

                            StringBuilder builder = new StringBuilder();
                            int count = 1;

                            xTest = "6A";

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
                                xNettDisc = (Pricing.isNett(rslt.Nomor, PromoID)) ? "N" : "D";

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

                                xTest = "9";

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