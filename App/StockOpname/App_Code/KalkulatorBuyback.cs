using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KalkulatorBuyback
/// </summary>
public class KalkulatorBuyback
{
    public static List<string[]> GetPriceBuyBack(int tipe, int idproduct, string nomorQC)
    {
        List<string[]> buybacks = new List<string[]>();
        using (Map_DB db = new Map_DB())
        {
            if (!string.IsNullOrEmpty(nomorQC) && tipe > 0)
            {
                if (tipe == 1) //DJ
                {
                    var docqc = db.DocQCDJ.SingleOrDefault(p => p.Nomor == nomorQC);
                    int[] getprice = GetIDBuyBack(tipe, docqc.IDProduct);
                    foreach(int y in getprice)
                    {
                        string[] buyback = new string[7];
                        buyback = PriceBuyBack(y);
                        buybacks.Add(buyback);
                    }
                }
            }
            else if (tipe > 0 && idproduct > 0)
            {
                if (tipe == 1) //DJ
                {
                    int[] getprice = GetIDBuyBack(tipe, idproduct);
                    foreach (int y in getprice)
                    {
                        string[] buyback = new string[7];
                        buyback = PriceBuyBack(y);
                        buybacks.Add(buyback);
                    }
                }
            }
        }
        return buybacks;
    }

    public static string[] GetListPriceBuyBack(int tipe, int idproduct, decimal hargajual)
    {
        using (Map_DB db = new Map_DB())
        {
            string[] aspproduct = new string[18];

            if (tipe == 1) // DJ
            {
                var p = db.StockProductDJ.SingleOrDefault(d => d.ID == idproduct);
                aspproduct = ASPProduct.getASPDJParameter(p.ID, hargajual);
            }
    		else if (tipe == 2) // PG
            {
                var p = db.StockProductPG.SingleOrDefault(d => d.ID == idproduct);
                aspproduct = ASPProduct.getASPPGParameter();
            }
    		else if (tipe == 3) // GJ
            {
                var p = db.StockProductGJ.SingleOrDefault(d => d.ID == idproduct);
                aspproduct = ASPProduct.getASPGJParameter(p.ID, false);
            }
    		else if (tipe == 4) // LD
            {
                var p = db.StockProductLD_Stone1B.SingleOrDefault(d => d.ID == idproduct);
                aspproduct = ASPProduct.getASPLDParameter(p.ID, false);
            }

            return aspproduct;
            }
}

    public static string[] PriceBuyBack(int id)
    {
        string[] pbb = new string[7];
        string query = "SELECT TOP 1 ID, " +
                        "TransctionType, " +
                        "MaxValue, " +
                        "Category, " +
                        "BuybackPercentage, " +
                        "MinPeriode, " +
                        "MaxPeriode " +
                        "FROM BuybackCalculator " +
                        "WHERE ID = " + id.ToString();
        DataTable dt = Db.Rs(query, Db.Cnn_DB);
        if (dt.Rows.Count == 1)
        {
            pbb[0] = dt.Rows[0]["ID"].ToString();
            pbb[1] = (dt.Rows[0]["TransctionType"].ToString() == "TI") ? "TRADE IN" : "RESELL";
            pbb[2] = dt.Rows[0]["MaxValue"].ToString();
            pbb[3] = dt.Rows[0]["Category"].ToString();
            pbb[4] = dt.Rows[0]["BuybackPercentage"].ToString();
            pbb[5] = dt.Rows[0]["MinPeriode"].ToString();
            pbb[6] = dt.Rows[0]["MaxPeriode"].ToString();
        }
        return pbb;
    }

    public static int[] GetIDBuyBack(int tipe, int idproduct)
    {
        int[] retVal = new int[2]; //TradeIn dan Resell
        using (Map_DB db = new Map_DB())
        {
            if (tipe == 1)
            {
                var dy = db.StockProductDJ.SingleOrDefault(p => p.ID == idproduct);
                if(dy.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "SNIPS")
                {
                    if (dy.StockProductDJ_Stone3.Any(p => !string.IsNullOrEmpty(p.GIA) && p.CaratButir < 3)) //SNIPS with gia < 3 carat
                    {
                        retVal[0] = 5; //TI
                        retVal[1] = 11; //RO
                    }
                    else if (dy.StockProductDJ_Stone3.Any(p => !string.IsNullOrEmpty(p.GIA) && p.CaratButir >= 3)) //SNIPS with gia >= 3 carat
                    {
                        retVal[0] = 6; //TI
                        retVal[1] = 12; //RO
                    }
                    else if(dy.StockProductDJ_Stone3.Any(p => string.IsNullOrEmpty(p.GIA))) //SNIPS non gia
                    {
                        retVal[0] = 4; //TI
                        retVal[1] = 10; //RO
                    }
                }
                else if(dy.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "FAS" || dy.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "SNI") //FASHION / SNI DJ
                {
                    retVal[0] = 3; //TI
                    retVal[1] = 9; //RO
                }
                else
                {
                    if (KalkulatorCheckBranded(dy.Nomor)) // Branded
                    {
                        retVal[0] = 1; //TI
                        retVal[1] = 7; //RO
                    }
                    if (KalkulatorCheckGeneric(dy.Nomor)) //Generic
                    {
                        retVal[0] = 2; //TI
                        retVal[1] = 8; //RO
                    }
                    else
                    {
                        retVal[0] = 0;
                        retVal[1] = 0;
                    }
                }
            }
        }
        return retVal;
    }

    public static bool KalkulatorCheckBranded(string PLU)
    {
        bool status = false;
        using (Map_DB db = new Map_DB())
        {
            var d = (
                    (from p in db.StockProductDJ_Stone1B where p.StockProductDJ.Nomor == PLU select p.Stone1B.Parcel06).Union
                    (from p in db.StockProductLD_Stone1B where p.Nomor == PLU select p.Stone1B.Parcel06)
                    );

            if (d.Count() > 0)
            {
                /// return batu branded, selain dari 3 jenis batu (NM:GENERIC NON PARAMETER, GN:NON 3X ROUND, NN:REGULER NON BRAND)
                /// revisi 25 Apr 2017, Flag Stone Branded by database table
                var stone = from p in db.Stone1B
                            where d.Contains(p.Parcel06)
                            && p.Parcel106.IsBranded == true
                            && !(p.Parcel106.NamaKode.Contains("GENERIC"))
                            && !(p.Parcel106.NamaKode.Contains("REGULER"))
                            && !(p.Parcel106.NamaKode.Contains("NON 3X"))
                            select p;

                /// jika jumlah batu branding lebih dari 0 maka return false
                if (stone.Count() > 0) { status = true; }
            }
        }
        return status;
    }

    public static bool KalkulatorCheckGeneric(string PLU)
    {
        bool status = false;
        using (Map_DB db = new Map_DB()) {
            var d = (
                    (from p in db.StockProductDJ_Stone1B where p.StockProductDJ.Nomor == PLU select p.Stone1B.Parcel06).Union
                    (from p in db.StockProductLD_Stone1B where p.Nomor == PLU select p.Stone1B.Parcel06)
                    );

            if (d.Count() > 0)
            {
                var stone = from p in db.Stone1B
                            where d.Contains(p.Parcel06)
                            && ((p.Parcel106.Nama.Contains("GENERIC"))
                                || (p.Parcel106.Nama.Contains("NON 3X")))
                            select p;

                if (stone.Count() > 0) { status = true; }
            }
        }
        return status;
    }
}