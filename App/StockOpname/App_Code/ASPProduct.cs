using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ASP
/// </summary>
public partial class ASPProduct
{
    public static string[] getASPDJParameter(int idprod, decimal TotalBayar = 0m)
    {
        using (Map_DB db = new Map_DB())
        {

            string[] x = new string[20];
            int[] IDSolitaire = new int[20];
            int[] IDNonSolitaire = new int[20];
            bool Solitaire = false;
		
    		var s = db.StockProductDJ.SingleOrDefault(p => p.ID == idprod);

            // Kondisi Solitaire / NonSolitaire
            if (s.StockProductDJ_CharProduct.CharStoneDist.NamaKode == "SOL")
            {
                if (s.StockProductDJ_Stone1A.Any() && Solitaire == false)
                {
                    var maxValue = s.StockProductDJ_Stone1A.OrderByDescending(p => p.CaratButir).First().CaratButir;
                    Solitaire = (maxValue < 0.9m) ? true : false;
                }

                if (s.StockProductDJ_Stone1B.Any() && Solitaire == false)
                {
                    var maxValue = s.StockProductDJ_Stone1B.OrderByDescending(p => p.CaratButir).First().CaratButir;
                    Solitaire = (maxValue < 0.9m) ? true : false;
                }

                if (s.StockProductDJ_Stone2.Any() && Solitaire == false)
                {
                    var maxValue = s.StockProductDJ_Stone2.OrderByDescending(p => p.CaratButir).First().CaratButir;
                    Solitaire = (maxValue < 0.9m) ? true : false;
                }

                if (s.StockProductDJ_Stone3.Any() && Solitaire == false)
                {
                    var maxValue = s.StockProductDJ_Stone3.OrderByDescending(p => p.CaratButir).First().CaratButir;
                    Solitaire = (maxValue < 0.9m) ? true : false;
                }

                if (s.StockProductDJ_Stone4.Any() && Solitaire == false)
                {
                    var maxValue = s.StockProductDJ_Stone4.OrderByDescending(p => p.CaratButir).First().CaratButir;
                    Solitaire = (maxValue < 0.9m) ? true : false;
                }

                if (s.StockProductDJ_Stone5.Any() && Solitaire == false)
                {
                    var maxValue = s.StockProductDJ_Stone5.OrderByDescending(p => p.CaratButir).First().CaratButir;
                    Solitaire = (maxValue < 0.9m) ? true : false;
                }

            }

            bool NoExchange = false;

            // ASP Promo Active / Non-Active
            var rs = from p in db.PromoDJ_Detail
                     where p.PromoDJ.ApprovalNama != null && p.PromoDJ.TglStart.Value <= DateTime.Today
                     && p.PromoDJ.TglExpire.Value >= DateTime.Today && p.IDProduct.Value == s.ID
                     select p;
            if (rs.Count() > 0)
            {
                // promo dengan setting No Resell No Exchange tidak akan berlaku ASP apapun
                var disc = rs.First();
                if (disc.PromoDJ.StatusASP == 1)
                    NoExchange = true;
            }

            if (TotalBayar >= 3000000 && !NoExchange)
            {
                IDSolitaire[0] = 3;
                IDSolitaire[1] = 4;
                IDSolitaire[2] = 5;
                IDSolitaire[3] = 8;
                IDSolitaire[4] = 9;
                IDSolitaire[5] = 10;

                IDNonSolitaire[0] = 1;
                IDNonSolitaire[1] = 2;
                IDNonSolitaire[2] = 6;
                IDNonSolitaire[3] = 7;

                // Jika category Fashion dan Distribusi Stone Solitaire // 2017-04-12
                if (s.StockProductDJ_CharProduct.ProductCategory == 1 && s.StockProductDJ_CharProduct.CharStoneDist.NamaKode == "SOL")
                {
                    Solitaire = false;
                }

                // Jika category StatusInvestment dan level Semi Mounting // 2017-05-26
                // Disable - berdasarkan adendum Bu Mirah (10 Sep 2018)
                //if (s.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "SI" && s.StockProductDJ_CharProduct.CharProductLevel.NamaKode == "SMG")
                //{
                //    Solitaire = true;
                //}

                // Revisi ASP ID = 11 untuk category StatusInvestment dan level Semi Mounting // 2018-10-30
                if (s.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "SI" && s.StockProductDJ_CharProduct.CharProductLevel.NamaKode == "SMG")
                {
                    var d = db.ASPDJ.SingleOrDefault(p => p.ID == 11 && p.ProductCategory == s.StockProductDJ_CharProduct.ProductCategory);
                    if (d == null) { NoExchange = true; }
                    else
                    {
                        x[0] = d.MEPeriodeMin.ToString();
                        x[1] = d.MEPeriodeMax.ToString();
                        x[2] = d.MERumus.ToString("N2");
                        x[3] = (d.TI1_PeriodeMin).ToString();
                        x[4] = (d.TI1_PeriodeMax).ToString();
                        x[5] = d.TI1_Rumus.ToString("N2");
                        x[6] = (d.TI2_PeriodeMin).ToString();
                        x[7] = (d.TI2_PeriodeMax).ToString();
                        x[8] = d.TI2_Rumus.ToString("N2");
                        x[9] = (d.TI3_PeriodeMin).ToString();
                        x[10] = (d.TI3_PeriodeMax).ToString();
                        x[11] = d.TI3_Rumus.ToString("N2");
                        x[12] = (d.R1_PeriodeMin).ToString();
                        x[13] = (d.R1_PeriodeMax).ToString();
                        x[14] = d.R1_Rumus.ToString("N2");
                        x[15] = (d.R2_PeriodeMin).ToString();
                        x[16] = (d.R2_PeriodeMax).ToString();
                        x[17] = d.R2_Rumus.ToString("N2");
                        x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
                    }
                }
                //// Jika mengandung batu Loose Precious Stone (Stone3) // 2017-11-01
                else if (s.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "LPS" && s.StockProductDJ_Stone3.Any())
                {
                    if (TotalBayar >= 2500000000
                        && s.StockProductDJ_CharProduct.StoneCarat >= 3
                        )
                    {
                        // jika harga diatas 2,5M IDR
                        var d = db.ASPDJ.SingleOrDefault(p => p.ProductCategory == 16 && p.ID == 21); //16=LPS
                        if (d == null) { NoExchange = true; }
                        else
                        {
                            x[0] = d.MEPeriodeMin.ToString();
                            x[1] = d.MEPeriodeMax.ToString();
                            x[2] = d.MERumus.ToString("N2");
                            x[3] = (d.TI1_PeriodeMin).ToString();
                            x[4] = (d.TI1_PeriodeMax).ToString();
                            x[5] = d.TI1_Rumus.ToString("N2");
                            x[6] = (d.TI2_PeriodeMin).ToString();
                            x[7] = (d.TI2_PeriodeMax).ToString();
                            x[8] = d.TI2_Rumus.ToString("N2");
                            x[9] = (d.TI3_PeriodeMin).ToString();
                            x[10] = (d.TI3_PeriodeMax).ToString();
                            x[11] = d.TI3_Rumus.ToString("N2");
                            x[12] = (d.R1_PeriodeMin).ToString();
                            x[13] = (d.R1_PeriodeMax).ToString();
                            x[14] = d.R1_Rumus.ToString("N2");
                            x[15] = (d.R2_PeriodeMin).ToString();
                            x[16] = (d.R2_PeriodeMax).ToString();
                            x[17] = d.R2_Rumus.ToString("N2");
                            x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
                        }
                    }
                    else if (s.StockProductDJ_CharProduct.StoneCarat >= 3)
                    {
                        var d = db.ASPDJ.SingleOrDefault(p => p.ProductCategory == 16 && p.ID == 20); //16=LPS
                        if (d == null) { NoExchange = true; }
                        else
                        {
                            x[0] = d.MEPeriodeMin.ToString();
                            x[1] = d.MEPeriodeMax.ToString();
                            x[2] = d.MERumus.ToString("N2");
                            x[3] = (d.TI1_PeriodeMin).ToString();
                            x[4] = (d.TI1_PeriodeMax).ToString();
                            x[5] = d.TI1_Rumus.ToString("N2");
                            x[6] = (d.TI2_PeriodeMin).ToString();
                            x[7] = (d.TI2_PeriodeMax).ToString();
                            x[8] = d.TI2_Rumus.ToString("N2");
                            x[9] = (d.TI3_PeriodeMin).ToString();
                            x[10] = (d.TI3_PeriodeMax).ToString();
                            x[11] = d.TI3_Rumus.ToString("N2");
                            x[12] = (d.R1_PeriodeMin).ToString();
                            x[13] = (d.R1_PeriodeMax).ToString();
                            x[14] = d.R1_Rumus.ToString("N2");
                            x[15] = (d.R2_PeriodeMin).ToString();
                            x[16] = (d.R2_PeriodeMax).ToString();
                            x[17] = d.R2_Rumus.ToString("N2");
                            x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
                        }
                    }
                    else
                    {
                        x[0] = "";
                    }

                }
                //// Jika mengandung batu SNI Precious Stone (Stone3) // 2017-12-14
                else if (s.StockProductDJ_CharProduct.CharProductCategory.NamaKode == "SNIPS" && s.StockProductDJ_Stone3.Any())
                {
                    if (System.DateTime.Now <= Convert.ToDateTime("2017-11-26 23:59:59"))
                    {
                        // menggunakan ASP Fashion Non-Branded sebelum release ASP SNIPS / LPS yang baru
                        var d = db.ASPDJ.SingleOrDefault(p => p.ID == 16); // ASP Fashion
                        if (d == null) { NoExchange = true; }
                        else
                        {
                            x[0] = d.MEPeriodeMin.ToString();
                            x[1] = d.MEPeriodeMax.ToString();
                            x[2] = d.MERumus.ToString("N2");
                            x[3] = (d.TI1_PeriodeMin).ToString();
                            x[4] = (d.TI1_PeriodeMax).ToString();
                            x[5] = d.TI1_Rumus.ToString("N2");
                            x[6] = (d.TI2_PeriodeMin).ToString();
                            x[7] = (d.TI2_PeriodeMax).ToString();
                            x[8] = d.TI2_Rumus.ToString("N2");
                            x[9] = (d.TI3_PeriodeMin).ToString();
                            x[10] = (d.TI3_PeriodeMax).ToString();
                            x[11] = d.TI3_Rumus.ToString("N2");
                            x[12] = (d.R1_PeriodeMin).ToString();
                            x[13] = (d.R1_PeriodeMax).ToString();
                            x[14] = d.R1_Rumus.ToString("N2");
                            x[15] = (d.R2_PeriodeMin).ToString();
                            x[16] = (d.R2_PeriodeMax).ToString();
                            x[17] = d.R2_Rumus.ToString("N2");
                            x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
                        }
                    }
                    // Precious Stone EMERALD (Green / Hijau) / kode awalan "EM" atau persis "E"
                    else if (s.StockProductDJ_Stone3.Where(p => p.Stone3.Parcel301.NamaKode == "E" || p.Stone3.Parcel301.NamaKode.StartsWith("EM")).Any())
                    {
                        x[0] = "";
                    }
                    // Precious Stone bersertifikat GRS atau GIA dan jika lebih besar sama dengan total carat 3.00
                    else if (s.StockProductDJ_Stone3.Where(p => p.GIA != "" && p.GIA != null && p.CaratButir >= 3).Any())
                    {
                        var d = db.ASPDJ.SingleOrDefault(p => p.ProductCategory == 8 && p.ID == 19); //8=SNIPS
                        if (d == null) { NoExchange = true; }
                        else
                        {
                            x[0] = d.MEPeriodeMin.ToString();
                            x[1] = d.MEPeriodeMax.ToString();
                            x[2] = d.MERumus.ToString("N2");
                            x[3] = (d.TI1_PeriodeMin).ToString();
                            x[4] = (d.TI1_PeriodeMax).ToString();
                            x[5] = d.TI1_Rumus.ToString("N2");
                            x[6] = (d.TI2_PeriodeMin).ToString();
                            x[7] = (d.TI2_PeriodeMax).ToString();
                            x[8] = d.TI2_Rumus.ToString("N2");
                            x[9] = (d.TI3_PeriodeMin).ToString();
                            x[10] = (d.TI3_PeriodeMax).ToString();
                            x[11] = d.TI3_Rumus.ToString("N2");
                            x[12] = (d.R1_PeriodeMin).ToString();
                            x[13] = (d.R1_PeriodeMax).ToString();
                            x[14] = d.R1_Rumus.ToString("N2");
                            x[15] = (d.R2_PeriodeMin).ToString();
                            x[16] = (d.R2_PeriodeMax).ToString();
                            x[17] = d.R2_Rumus.ToString("N2");
                            x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
                        }
                    }
                    // Precious Stone bersertifikat GRS atau GIA dan jika lebih kecil dari total carat 3.00
                    else if (s.StockProductDJ_Stone3.Where(p => p.GIA != "" && p.GIA != null && p.CaratButir < 3).Any())
                    {
                        var d = db.ASPDJ.SingleOrDefault(p => p.ProductCategory == 8 && p.ID == 18); //8=SNIPS
                        if (d == null) { NoExchange = true; }
                        else
                        {
                            x[0] = d.MEPeriodeMin.ToString();
                            x[1] = d.MEPeriodeMax.ToString();
                            x[2] = d.MERumus.ToString("N2");
                            x[3] = (d.TI1_PeriodeMin).ToString();
                            x[4] = (d.TI1_PeriodeMax).ToString();
                            x[5] = d.TI1_Rumus.ToString("N2");
                            x[6] = (d.TI2_PeriodeMin).ToString();
                            x[7] = (d.TI2_PeriodeMax).ToString();
                            x[8] = d.TI2_Rumus.ToString("N2");
                            x[9] = (d.TI3_PeriodeMin).ToString();
                            x[10] = (d.TI3_PeriodeMax).ToString();
                            x[11] = d.TI3_Rumus.ToString("N2");
                            x[12] = (d.R1_PeriodeMin).ToString();
                            x[13] = (d.R1_PeriodeMax).ToString();
                            x[14] = d.R1_Rumus.ToString("N2");
                            x[15] = (d.R2_PeriodeMin).ToString();
                            x[16] = (d.R2_PeriodeMax).ToString();
                            x[17] = d.R2_Rumus.ToString("N2");
                            x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
                        }
                    }
                    // Precious Stone TANPA sertifikat GRS atau GIA
                    else if (s.StockProductDJ_Stone3.Where(p => p.GIA.Trim() == "" || p.GIA == null).Any())
                    {
                        var d = db.ASPDJ.SingleOrDefault(p => p.ProductCategory == 8 && p.ID == 17); //8=SNIPS
                        if (d == null) { NoExchange = true; }
                        else
                        {
                            x[0] = d.MEPeriodeMin.ToString();
                            x[1] = d.MEPeriodeMax.ToString();
                            x[2] = d.MERumus.ToString("N2");
                            x[3] = (d.TI1_PeriodeMin).ToString();
                            x[4] = (d.TI1_PeriodeMax).ToString();
                            x[5] = d.TI1_Rumus.ToString("N2");
                            x[6] = (d.TI2_PeriodeMin).ToString();
                            x[7] = (d.TI2_PeriodeMax).ToString();
                            x[8] = d.TI2_Rumus.ToString("N2");
                            x[9] = (d.TI3_PeriodeMin).ToString();
                            x[10] = (d.TI3_PeriodeMax).ToString();
                            x[11] = d.TI3_Rumus.ToString("N2");
                            x[12] = (d.R1_PeriodeMin).ToString();
                            x[13] = (d.R1_PeriodeMax).ToString();
                            x[14] = d.R1_Rumus.ToString("N2");
                            x[15] = (d.R2_PeriodeMin).ToString();
                            x[16] = (d.R2_PeriodeMax).ToString();
                            x[17] = d.R2_Rumus.ToString("N2");
                            x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
                        }
                    }
                    else
                    {
                        x[0] = "";
                    }

                }
                else
                {
                    if (Solitaire)
                    {
                        int status = 0;
                        if (DJCheckBrand(s)) /// Jika NonBranding
                            status = 1;
                        else /// Jika Brand
                            status = 0;
                        var d = db.ASPDJ.SingleOrDefault(p => IDSolitaire.Contains(p.ID) && p.ProductCategory == s.StockProductDJ_CharProduct.ProductCategory && p.StatusBrand == status);
    					if (DJCheckGeneric(s))
                        {
                            d = db.ASPDJ.SingleOrDefault(p => p.ID == 10);
                        }
                        if (d == null) { NoExchange = true; }
                        else
                        {
                            x[0] = d.MEPeriodeMin.ToString();
                            x[1] = d.MEPeriodeMax.ToString();
                            x[2] = d.MERumus.ToString("N2");
                            x[3] = (d.TI1_PeriodeMin).ToString();
                            x[4] = (d.TI1_PeriodeMax).ToString();
                            x[5] = d.TI1_Rumus.ToString("N2");
                            x[6] = (d.TI2_PeriodeMin).ToString();
                            x[7] = (d.TI2_PeriodeMax).ToString();
                            x[8] = d.TI2_Rumus.ToString("N2");
                            x[9] = (d.TI3_PeriodeMin).ToString();
                            x[10] = (d.TI3_PeriodeMax).ToString();
                            x[11] = d.TI3_Rumus.ToString("N2");
                            x[12] = (d.R1_PeriodeMin).ToString();
                            x[13] = (d.R1_PeriodeMax).ToString();
                            x[14] = d.R1_Rumus.ToString("N2");
                            x[15] = (d.R2_PeriodeMin).ToString();
                            x[16] = (d.R2_PeriodeMax).ToString();
                            x[17] = d.R2_Rumus.ToString("N2");
                            x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
                        }
                    }
                    else /// Selain Solitaire
                    {
                        int status = 0;
                        if (DJCheckBrand(s)) /// Jika NonBranding
                            status = 1;
                        else /// Jika Brand
                            status = 0;
                        var d = db.ASPDJ.SingleOrDefault(p => IDNonSolitaire.Contains(p.ID) && p.ProductCategory == s.StockProductDJ_CharProduct.ProductCategory && p.StatusBrand == status);
    					if (DJCheckGeneric(s))
                        {
                            d = db.ASPDJ.SingleOrDefault(p => p.ID == 10);
                        }
                        if (d == null) { NoExchange = true; }
                        else
                        {
                            x[0] = d.MEPeriodeMin.ToString();
                            x[1] = d.MEPeriodeMax.ToString();
                            x[2] = d.MERumus.ToString("N2");
                            x[3] = (d.TI1_PeriodeMin).ToString();
                            x[4] = (d.TI1_PeriodeMax).ToString();
                            x[5] = d.TI1_Rumus.ToString("N2");
                            x[6] = (d.TI2_PeriodeMin).ToString();
                            x[7] = (d.TI2_PeriodeMax).ToString();
                            x[8] = d.TI2_Rumus.ToString("N2");
                            x[9] = (d.TI3_PeriodeMin).ToString();
                            x[10] = (d.TI3_PeriodeMax).ToString();
                            x[11] = d.TI3_Rumus.ToString("N2");
                            x[12] = (d.R1_PeriodeMin).ToString();
                            x[13] = (d.R1_PeriodeMax).ToString();
                            x[14] = d.R1_Rumus.ToString("N2");
                            x[15] = (d.R2_PeriodeMin).ToString();
                            x[16] = (d.R2_PeriodeMax).ToString();
                            x[17] = d.R2_Rumus.ToString("N2");
                            x[18] = d.MEPeriodeMaxDay != null ? d.MEPeriodeMaxDay.ToString() : "0";
                        }
                    }
                }
            }
            else
            {
                NoExchange = true;
            }

            if (NoExchange) { x[0] = ""; }

            return x;
            }
}
    public static bool DJCheckBrand(StockProductDJ s)
    {
        // GN (NON 3X ROUND) , NM (GENERIC), NN (REGULER NON-BRAND) --> Parcel106 : 3 jenis batu non branded, selain dari 3 jenis ini berarti branded
        // default = false (branded)
        bool brand = false;

        brand = !Pricing.isDiamondBranded(s.Nomor);

        // return 0 atau false = mengandung batu branded
        return brand;
    }
	public static bool DJCheckGeneric(StockProductDJ s)
    {
        bool generic = false;

        generic = Pricing.isDiamondGeneric(s.Nomor);
        return generic;
    } 
    public static bool LDCheckBrand(StockProductLD_Stone1B s)
    {
        // GN (NON 3X ROUND) , NM (GENERIC), NN (REGULER NON-BRAND) --> Parcel106 : 3 jenis batu non branded, selain dari 3 jenis ini berarti branded
        // default = false (branded)
        bool brand = false;

        brand = !Pricing.isDiamondBranded(s.Nomor);

        // return 0 atau false = mengandung batu branded
        return brand;
    }
    public static string[] getASPPGParameter()
    {
        using (Map_DB db = new Map_DB())
        {
            string[] x = new string[20];

    
            var d = db.ASPPG.SingleOrDefault(p => p.ID == 1);
            if (d == null) x[0] = "";
            else
            {
                x[0] = d.MEPeriodeMin.ToString();
                x[1] = d.MEPeriodeMax.ToString();
                x[2] = d.MERumus.ToString("N2");
                x[3] = d.TI1_PeriodeMin.ToString();
                x[4] = d.TI1_Rumus.ToString("N2");
                x[5] = d.TI2_PeriodeMin.ToString();
                x[6] = d.TI2_Rumus.ToString("N2");
                x[7] = d.R_PeriodeMin.ToString();
                x[8] = d.R_Rumus.ToString("N2");
            }

            return x;
            }
}
    public static int CalcPeriod(DateTime from, bool week)
    {
        DateTime now = DateTime.Now;

        int Period = 0;
        if (week == false)
            Period = ((now.Year - from.Year) * 12) + now.Month - from.Month;
        else
            Period = Convert.ToInt32(Math.Floor((now - from).TotalDays / 7));

        return Period;
    }

    public static string[] getASPGJParameter(int ProductCategory, bool Solitaire)
    {
        using (Map_DB db = new Map_DB())
        {
            string[] x = new string[20];

            if (Solitaire == false)
            {
                var d = db.ASPGJ.SingleOrDefault(p => p.ID > 2 && p.ProductCategory == ProductCategory);
                if (d == null) x[0] = "";
                else
                {
                    x[0] = d.MEPeriodeMin.ToString();
                    x[1] = d.MEPeriodeMax.ToString();
                    x[2] = d.MERumus.ToString("N2");
                    x[3] = (d.TI1_PeriodeMin).ToString();
                    x[4] = (d.TI1_PeriodeMax).ToString();
                    x[5] = d.TI1_Rumus.ToString("N2");
                    x[6] = (d.TI2_PeriodeMin).ToString();
                    x[7] = (d.TI2_PeriodeMax).ToString();
                    x[8] = d.TI2_Rumus.ToString("N2");
                    x[9] = (d.TI3_PeriodeMin).ToString();
                    x[10] = (d.TI3_PeriodeMax).ToString();
                    x[11] = d.TI3_Rumus.ToString("N2");
                    x[12] = (d.R1_PeriodeMin).ToString();
                    x[13] = (d.R1_PeriodeMax).ToString();
                    x[14] = d.R1_Rumus.ToString("N2");
                    x[15] = (d.R2_PeriodeMin).ToString();
                    x[16] = (d.R2_PeriodeMax).ToString();
                    x[17] = d.R2_Rumus.ToString("N2");
                }
            }
            else
            {
                var d = db.ASPGJ.SingleOrDefault(p => p.ID < 3 && p.ProductCategory == ProductCategory);
                if (d == null) x[0] = "";
                else
                {
                    x[0] = d.MEPeriodeMin.ToString();
                    x[1] = d.MEPeriodeMax.ToString();
                    x[2] = d.MERumus.ToString("N2");
                    x[3] = (d.TI1_PeriodeMin).ToString();
                    x[4] = (d.TI1_PeriodeMax).ToString();
                    x[5] = d.TI1_Rumus.ToString("N2");
                    x[6] = (d.TI2_PeriodeMin).ToString();
                    x[7] = (d.TI2_PeriodeMax).ToString();
                    x[8] = d.TI2_Rumus.ToString("N2");
                    x[9] = (d.TI3_PeriodeMin).ToString();
                    x[10] = (d.TI3_PeriodeMax).ToString();
                    x[11] = d.TI3_Rumus.ToString("N2");
                    x[12] = (d.R1_PeriodeMin).ToString();
                    x[13] = (d.R1_PeriodeMax).ToString();
                    x[14] = d.R1_Rumus.ToString("N2");
                    x[15] = (d.R2_PeriodeMin).ToString();
                    x[16] = (d.R2_PeriodeMax).ToString();
                    x[17] = d.R2_Rumus.ToString("N2");
                }
            }

            return x;
            }
}

    public static string[] getASPGJParameter(StockProductGJ s, bool Solitaire)
    {
        using (Map_DB db = new Map_DB())
        {
            string[] x = new string[20];

            if (s.StockProductGJ_CharProduct.CharProductCategory.NamaKode != "WDR")
            {
                if (Solitaire == false)
                {
                    var d = db.ASPGJ.SingleOrDefault(p => p.ID > 2 && p.ProductCategory == s.StockProductGJ_CharProduct.ProductCategory);
                    if (d == null) x[0] = "";
                    else
                    {
                        x[0] = d.MEPeriodeMin.ToString();
                        x[1] = d.MEPeriodeMax.ToString();
                        x[2] = d.MERumus.ToString("N2");
                        x[3] = (d.TI1_PeriodeMin).ToString();
                        x[4] = (d.TI1_PeriodeMax).ToString();
                        x[5] = d.TI1_Rumus.ToString("N2");
                        x[6] = (d.TI2_PeriodeMin).ToString();
                        x[7] = (d.TI2_PeriodeMax).ToString();
                        x[8] = d.TI2_Rumus.ToString("N2");
                        x[9] = (d.TI3_PeriodeMin).ToString();
                        x[10] = (d.TI3_PeriodeMax).ToString();
                        x[11] = d.TI3_Rumus.ToString("N2");
                        x[12] = (d.R1_PeriodeMin).ToString();
                        x[13] = (d.R1_PeriodeMax).ToString();
                        x[14] = d.R1_Rumus.ToString("N2");
                        x[15] = (d.R2_PeriodeMin).ToString();
                        x[16] = (d.R2_PeriodeMax).ToString();
                        x[17] = d.R2_Rumus.ToString("N2");
                    }
                }
                else
                {
                    var d = db.ASPGJ.SingleOrDefault(p => p.ID < 3 && p.ProductCategory == s.StockProductGJ_CharProduct.ProductCategory);
                    if (d == null) x[0] = "";
                    else
                    {
                        x[0] = d.MEPeriodeMin.ToString();
                        x[1] = d.MEPeriodeMax.ToString();
                        x[2] = d.MERumus.ToString("N2");
                        x[3] = (d.TI1_PeriodeMin).ToString();
                        x[4] = (d.TI1_PeriodeMax).ToString();
                        x[5] = d.TI1_Rumus.ToString("N2");
                        x[6] = (d.TI2_PeriodeMin).ToString();
                        x[7] = (d.TI2_PeriodeMax).ToString();
                        x[8] = d.TI2_Rumus.ToString("N2");
                        x[9] = (d.TI3_PeriodeMin).ToString();
                        x[10] = (d.TI3_PeriodeMax).ToString();
                        x[11] = d.TI3_Rumus.ToString("N2");
                        x[12] = (d.R1_PeriodeMin).ToString();
                        x[13] = (d.R1_PeriodeMax).ToString();
                        x[14] = d.R1_Rumus.ToString("N2");
                        x[15] = (d.R2_PeriodeMin).ToString();
                        x[16] = (d.R2_PeriodeMax).ToString();
                        x[17] = d.R2_Rumus.ToString("N2");
                    }
                }
            }

            return x;
            }
}

    public static string[] getASPLDParameter(int ProductCategory, bool Solitaire)
    {
        using (Map_DB db = new Map_DB())
        {
            string[] x = new string[20];

            var d = db.ASPLD.SingleOrDefault(p => p.ID == 1);
            if (d == null) x[0] = "";
            else
            {
                x[0] = d.MEPeriodeMin.ToString();
                x[1] = d.MEPeriodeMax.ToString();
                x[2] = d.MERumus.ToString("N2");
                x[3] = (d.TI1_PeriodeMin).ToString();
                x[4] = (d.TI1_PeriodeMax).ToString();
                x[5] = d.TI1_Rumus.ToString("N2");
                x[6] = (d.TI2_PeriodeMin).ToString();
                x[7] = (d.TI2_PeriodeMax).ToString();
                x[8] = d.TI2_Rumus.ToString("N2");
                x[9] = (d.TI3_PeriodeMin).ToString();
                x[10] = (d.TI3_PeriodeMax).ToString();
                x[11] = d.TI3_Rumus.ToString("N2");
                x[12] = (d.R1_PeriodeMin).ToString();
                x[13] = (d.R1_PeriodeMax).ToString();
                x[14] = d.R1_Rumus.ToString("N2");
                x[15] = (d.R2_PeriodeMin).ToString();
                x[16] = (d.R2_PeriodeMax).ToString();
                x[17] = d.R2_Rumus.ToString("N2");
            }

    
            return x;
            }
}
}