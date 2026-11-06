using System;
using System.Linq;

/// <summary>
/// Pricing System
/// </summary>
public partial class PricingStock {

    static App price = new App();

    static decimal Setting = 0m;

	// Repricing Repair Stock Final Stepw
    public static void RepairPricingDJ(int id)
    {
        using (Map_DB db = new Map_DB())
        {

            var d = db.StockProductDJ.SingleOrDefault(p => p.ID == id);

            //try { saveRevalStone(d); } catch (Exception ex) { }
            //try { saveRevalFinishing(d); } catch (Exception ex) { }
            //try { saveRevalTotalStone(d); } catch (Exception ex) { }
            //try { saveRevalProduct(d); } catch (Exception ex) { }
            //try { saveRevalBiaya(d); } catch (Exception ex) { }
            //try { saveRevalMU(d); } catch (Exception ex) { }
            //try { saveRevalSegmen(d); } catch (Exception ex) { }

            saveRevalStone(d);
            saveRevalFinishing(d);
            saveRevalTotalStone(d);
            saveRevalProduct(d);
            saveRevalBiaya(d);
            saveRevalMU(d);
            saveRevalSegmen(d);
            }
    }

    //saveRevalStone
    protected static void saveRevalStone(StockProductDJ d)
    {
        int Reval_M = 1;
        int Reval_R = 1;
        int Reval_G = 1;
        int Reval_SK = 1;
        int Reval_SB = 1;
        int Reval_MF = 1;
        int Reval_MU = 1;
        int Reval_Rate = 1;

        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SK == 1) { saveRevalStone1A(d); }
        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1) { saveRevalStone1B(d); }
        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1) { saveRevalStone2(d); }
        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1) { saveRevalStone3(d); }
        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1) { saveRevalStone4(d); }
        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_SB == 1) { saveRevalStone5(d); }
    }
    protected static void saveRevalStone1A(StockProductDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            int Reval_M = 1;
            int Reval_R = 1;
            int Reval_G = 1;
            int Reval_SK = 1;
            int Reval_SB = 1;
            int Reval_MF = 1;
            int Reval_MU = 1;
            int Reval_Rate = 1;

            //Exception-Cause
            string sebab = DL.StoneIndex(0);
            decimal OngkosPB = 0m;

            var rs = from p in db.StockProductDJ_Stone1A
                     where p.IDForm == d.ID
                     select p;

            foreach (var r in rs)
            {

                StonePricing1A prc = r.Stone1A.StonePricing1A;
                StoneCosting1A cost = r.Stone1A.StoneCosting1A;
                if (prc == null || cost == null)
                {
                }
                else
                {

                    if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                    else
                    {
                        //Harga
                        if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                        {
                            r.HargaSatuan = prc.Harga.Value;
                            r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                        }

                        if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                        {
                            r.HargaSatuanM = cost.Harga.Value;
                            r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                        }

                        //Tgl. Efektif
                        r.Efektif = prc.ApprovalTgl.Value;

                        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                        {
                            //Setting
                            OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone1A.Parcel101.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                            r.Setting = OngkosPB;

                            Setting += OngkosPB;
                            //db.SubmitChanges();
                        }

                        //Commit
                        db.SubmitChanges();
                    }
                }
            }
            }
    }
    protected static void saveRevalStone1B(StockProductDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            int Reval_M = 1;
            int Reval_R = 1;
            int Reval_G = 1;
            int Reval_SK = 1;
            int Reval_SB = 1;
            int Reval_MF = 1;
            int Reval_MU = 1;
            int Reval_Rate = 1;

            //Exception-Cause
            string sebab = DL.StoneIndex(1);

            decimal OngkosPB = 0m;

            var rs = from p in db.StockProductDJ_Stone1B
                     where
                         p.StatusTerpasang == 1
                         && p.IDForm == d.ID
                     select p;
            foreach (var r in rs)
            {

                StonePricing1B prc = r.Stone1B.StonePricing1B;
                StoneCosting1B cost = r.Stone1B.StoneCosting1B;
                if (prc == null || cost == null) { }
                else
                {

                    if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue || !prc.InputH.HasValue || !prc.InputP.HasValue) { }
                    else
                    {
                        //Harga
                        if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                        {
                            r.HargaSatuan = prc.Harga.Value;
                            r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                        }

                        //Rapaport
                        r.HargaInputH = prc.InputH.Value;
                        r.HargaInputP = prc.InputP.Value;

                        if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                        {
                            r.HargaSatuanM = cost.Harga.Value;
                            r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                        }

                        //Tgl. Efektif
                        r.Efektif = prc.ApprovalTgl.Value;

                        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                        {
                            //Setting
                            OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone1B.Parcel101.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                            r.Setting = OngkosPB;

                            Setting += OngkosPB;
                            db.SubmitChanges();
                        }

                        //Commit
                        db.SubmitChanges();
                    }
                }
            }
            }
    }
    protected static void saveRevalStone2(StockProductDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            int Reval_M = 1;
            int Reval_R = 1;
            int Reval_G = 1;
            int Reval_SK = 1;
            int Reval_SB = 1;
            int Reval_MF = 1;
            int Reval_MU = 1;
            int Reval_Rate = 1;

            //Exception-Cause
            string sebab = DL.StoneIndex(2);

            decimal OngkosPB = 0m;

            var rs = from p in db.StockProductDJ_Stone2
                     where p.IDForm == d.ID
                     select p;
            foreach (var r in rs)
            {

                StonePricing2 prc = r.Stone2.StonePricing2;
                StoneCosting2 cost = r.Stone2.StoneCosting2;
                if (prc == null || cost == null) { }
                else
                {

                    if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                    else
                    {
                        //Harga
                        if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                        {
                            r.HargaSatuan = prc.Harga.Value;
                            r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                        }

                        if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                        {
                            r.HargaSatuanM = cost.Harga.Value;
                            r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                        }

                        //Tgl. Efektif
                        r.Efektif = prc.ApprovalTgl.Value;

                        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                        {
                            //Setting
                            OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone2.Parcel201.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                            r.Setting = OngkosPB;

                            Setting += OngkosPB;
                            db.SubmitChanges();
                        }

                        //Commit
                        db.SubmitChanges();
                    }
                }
            }
            }
    }
    protected static void saveRevalStone3(StockProductDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            int Reval_M = 1;
            int Reval_R = 1;
            int Reval_G = 1;
            int Reval_SK = 1;
            int Reval_SB = 1;
            int Reval_MF = 1;
            int Reval_MU = 1;
            int Reval_Rate = 1;

            //Exception-Cause
            string sebab = DL.StoneIndex(3);

            decimal OngkosPB = 0m;

            var rs = from p in db.StockProductDJ_Stone3
                     where p.IDForm == d.ID
                     select p;

            foreach (var r in rs)
            {

                StonePricing3 prc = r.Stone3.StonePricing3;
                StoneCosting3 cost = r.Stone3.StoneCosting3;
                if (prc == null || cost == null) { }
                else
                {

                    if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                    else
                    {
                        //Harga
                        if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                        {
                            r.HargaSatuan = prc.Harga.Value;
                            r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                        }

                        if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                        {
                            r.HargaSatuanM = cost.Harga.Value;
                            r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                        }

                        //Tgl. Efektif
                        r.Efektif = prc.ApprovalTgl.Value;

                        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                        {
                            //Setting
                            OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone3.Parcel301.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                            r.Setting = OngkosPB;

                            Setting += OngkosPB;
                            db.SubmitChanges();
                        }

                        //Commit
                        db.SubmitChanges();
                    }
                }
            }
            }
    }
    protected static void saveRevalStone4(StockProductDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            int Reval_M = 1;
            int Reval_R = 1;
            int Reval_G = 1;
            int Reval_SK = 1;
            int Reval_SB = 1;
            int Reval_MF = 1;
            int Reval_MU = 1;
            int Reval_Rate = 1;

            //Exception-Cause
            string sebab = DL.StoneIndex(4);

            decimal OngkosPB = 0m;

            var rs = from p in db.StockProductDJ_Stone4
                     where p.IDForm == d.ID
                     select p;
            foreach (var r in rs)
            {

                StonePricing4 prc = r.Stone4.StonePricing4;
                StoneCosting4 cost = r.Stone4.StoneCosting4;
                if (prc == null || cost == null) { }
                else
                {

                    if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                    else
                    {
                        //Harga
                        if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                        {
                            r.HargaSatuan = prc.Harga.Value;
                            r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                        }

                        if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                        {
                            r.HargaSatuanM = cost.Harga.Value;
                            r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                        }

                        //Tgl. Efektif
                        r.Efektif = prc.ApprovalTgl.Value;

                        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                        {
                            //Setting
                            OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone4.Parcel401.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                            r.Setting = OngkosPB;

                            Setting += OngkosPB;
                            db.SubmitChanges();
                        }

                        //Commit
                        db.SubmitChanges();
                    }
                }
            }
            }
    }
    protected static void saveRevalStone5(StockProductDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            int Reval_M = 1;
            int Reval_R = 1;
            int Reval_G = 1;
            int Reval_SK = 1;
            int Reval_SB = 1;
            int Reval_MF = 1;
            int Reval_MU = 1;
            int Reval_Rate = 1;

            //Exception-Cause
            string sebab = DL.StoneIndex(5);

            decimal OngkosPB = 0m;

            var rs = from p in db.StockProductDJ_Stone5
                     where p.IDForm == d.ID
                     select p;
            foreach (var r in rs)
            {
                //StonePricing5 prc = r.Stone5.StonePricing5;
                //StoneCosting5 cost = r.Stone5.StoneCosting5;

                StonePricing5 prc = db.StonePricing5.SingleOrDefault(p => p.ID == r.IDStone);
                StoneCosting5 cost = db.StoneCosting5.SingleOrDefault(p => p.ID == r.IDStone);
                if (prc == null || cost == null) { }
                else
                {
                    if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) { }
                    else
                    {
                        //Harga
                        if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                        {
                            r.HargaSatuan = prc.Harga.Value;
                            r.HargaTotal = r.HargaSatuan * r.TotalCarat;
                        }

                        if ((Reval_M == 0 && Reval_R == 0) || Reval_M == 1)
                        {
                            r.HargaSatuanM = cost.Harga.Value;
                            r.HargaTotalM = r.HargaSatuanM * r.TotalCarat;
                        }

                        //Tgl. Efektif
                        r.Efektif = prc.ApprovalTgl.Value;

                        if ((Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 0) || Reval_MF == 1)
                        {
                            //Setting
                            try
                            {
                                OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone5.Parcel501.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                            }
                            catch (Exception ex) { }
                            r.Setting = OngkosPB;

                            Setting += OngkosPB;
                            db.SubmitChanges();
                        }

                        ////Commit
                        db.SubmitChanges();
                    }
                }
            }
            }
    }

    //saveRevalFinishing
    protected static void saveRevalFinishing(StockProductDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            int Reval_M = 1;
            int Reval_R = 1;
            int Reval_G = 1;
            int Reval_SK = 1;
            int Reval_SB = 1;
            int Reval_MF = 1;
            int Reval_MU = 1;
            int Reval_Rate = 1;

            var charproduct = d.StockProductDJ_CharProduct;
            var finishing = d.StockProductDJ_Finishing;
            foreach (var r in finishing)
            {
                bool ok = true;

                //Pricing
                var prc_ff = db.PricingTableFF.SingleOrDefault(p =>
                    p.IDItem == charproduct.ProductItem &&
                    p.IDFinishing == r.IDFinishing &&
                    p.Biaya.HasValue &&
                    p.ApprovalTgl.HasValue
                    );

                //Pricing - Validasi
                if (prc_ff == null) { ok = false; }

                //Process
                if (!ok) { }
                else
                {
                    if ((Reval_M == 0 && Reval_R == 0) || Reval_R == 1)
                    {
                        r.Biaya = prc_ff.Biaya.Value;
                        r.Efektif = prc_ff.ApprovalTgl.Value;

                        //Commit
                        db.SubmitChanges();
                    }
                }
            }
            }
    }

    //saveRevalTotalStone
    protected static void saveRevalTotalStone(StockProductDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            int Reval_M = 1;
            int Reval_R = 1;
            int Reval_G = 1;
            int Reval_SK = 1;
            int Reval_SB = 1;
            int Reval_MF = 1;
            int Reval_MU = 1;
            int Reval_Rate = 1;

            var charproduct = d.StockProductDJ_CharProduct;
            decimal total = 0;

            var rs_1A = from p in d.StockProductDJ_Stone1A select p.TotalButir;
            foreach (var r in rs_1A) total += r;

            var rs_1B = from p in d.StockProductDJ_Stone1B where p.StatusTerpasang == 1 select p.TotalButir;
            foreach (var r in rs_1B) total += r;

            var rs_2 = from p in d.StockProductDJ_Stone2 select p.TotalButir;
            foreach (var r in rs_2) total += r;

            var rs_3 = from p in d.StockProductDJ_Stone3 select p.TotalButir;
            foreach (var r in rs_3) total += r;

            var rs_4 = from p in d.StockProductDJ_Stone4 select p.TotalButir;
            foreach (var r in rs_4) total += r;

            var rs_5 = from p in d.StockProductDJ_Stone5 select p.TotalButir;
            foreach (var r in rs_5) total += r;

            charproduct.StoneQty = total;
            db.SubmitChanges();
            }
    }

    //saveRevalProduct
    protected static void saveRevalProduct(StockProductDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            int Reval_M = 1;
            int Reval_R = 1;
            int Reval_G = 1;
            int Reval_SK = 1;
            int Reval_SB = 1;
            int Reval_MF = 1;
            int Reval_MU = 1;
            int Reval_Rate = 1;

            var product = d.StockProductDJ_CharProduct;
            var pricingproduct = d.StockProductDJ_PricingProduct;
            var costingproduct = d.StockProductDJ_CostingProduct;
            bool ok = true;

            //Pricing

            int IDTG_Costing = CV.TG_PG1_Costing;
            int IDTG_Pricing = CV.TG_PG1_Pricing;
            if (product.CharFrameMaterial.NamaKode == "AU")
            {
                IDTG_Costing = CV.TG_PG1_Costing;
                IDTG_Pricing = CV.TG_PG1_Pricing;
            }
            else if (product.CharFrameMaterial.NamaKode == "PL")
            {
                IDTG_Costing = CV.TG_PG_Paladium;
                IDTG_Pricing = CV.TG_PG_Paladium2;
            }
            else if (product.CharFrameMaterial.NamaKode == "PT")
            {
                IDTG_Costing = CV.TG_PG_Platina;
                IDTG_Pricing = CV.TG_PG_Platina2;
            }
            else if (product.CharFrameMaterial.NamaKode == "SIL")
            {
                IDTG_Costing = CV.TG_PG_Silver;
                IDTG_Pricing = CV.TG_PG_Silver2;
            }

            var prc_tgM = db.PricingTableTG.SingleOrDefault(p =>
                p.ID == IDTG_Costing &&
                p.Harga.HasValue &&
                p.ApprovalTgl.HasValue);

            var prc_tg = db.PricingTableTG.SingleOrDefault(p =>
                p.ID == IDTG_Pricing &&
                p.Harga.HasValue &&
                p.ApprovalTgl.HasValue);

            //Pricing - Validasi
            if (prc_tg == null || prc_tgM == null) { ok = false; }

            //Process
            if (ok || Reval_R == 1 || Reval_M == 1)
            {
                /// Pricing (R)
                if (Reval_R == 1)
                {
                    var n = pricingproduct;

                    //Gold Price
                    if (Reval_G == 1)
                    {
                        n.GoldRate = prc_tg.Harga.Value;
                        n.EfektifGold = prc_tg.ApprovalTgl.Value;
                        db.SubmitChanges();
                    }

                    //Informasi Berat
                    n.Netto = product.NettoWeight;
                    n.Kadar = product.KadarTukaran;

                    //Total
                    n.TotalProduct = n.GoldRate * n.Netto * (n.Kadar / (decimal)100);
                    if (Reval_SK == 1) { n.TotalStone1A = Convert.ToDecimal(TotalRevalStone1A(d)); }
                    if (Reval_SB == 1) { n.TotalStone1B = Convert.ToDecimal(TotalRevalStone1B(d)); }
                    n.TotalStone2 = Convert.ToDecimal(TotalRevalStone2(d));
                    n.TotalStone3 = Convert.ToDecimal(TotalRevalStone3(d));
                    n.TotalStone4 = Convert.ToDecimal(TotalRevalStone4(d));
                    n.TotalStone5 = Convert.ToDecimal(TotalRevalStone5(d));
                    n.Total = n.TotalProduct + Convert.ToDecimal(n.TotalStone1A) + Convert.ToDecimal(n.TotalStone1B) + Convert.ToDecimal(n.TotalStone2) + Convert.ToDecimal(n.TotalStone3) + Convert.ToDecimal(n.TotalStone4) + Convert.ToDecimal(n.TotalStone5);

                    //Commit
                    db.SubmitChanges();
                }

                /// Costing (M)
                if (Reval_M == 1)
                {

                    var m = costingproduct;

                    //Gold Price
                    if (Reval_G == 1)
                    {
                        m.GoldRate = prc_tgM.Harga.Value;
                        m.EfektifGold = prc_tgM.ApprovalTgl.Value;
                        db.SubmitChanges();
                    }

                    //Informasi Berat
                    m.Netto = product.NettoWeight;
                    m.Kadar = product.KadarTukaran;

                    //Total
                    m.TotalProduct = m.GoldRate * m.Netto * (m.Kadar / (decimal)100);
                    if (Reval_SK == 1) { m.TotalStone1A = Convert.ToDecimal(TotalMRevalStone1A(d)); }
                    if (Reval_SB == 1) { m.TotalStone1B = Convert.ToDecimal(TotalMRevalStone1B(d)); }
                    m.TotalStone2 = Convert.ToDecimal(TotalMRevalStone2(d));
                    m.TotalStone3 = Convert.ToDecimal(TotalMRevalStone3(d));
                    m.TotalStone4 = Convert.ToDecimal(TotalMRevalStone4(d));
                    m.TotalStone5 = Convert.ToDecimal(TotalMRevalStone5(d));
                    m.Total = m.TotalProduct + Convert.ToDecimal(m.TotalStone1A) + Convert.ToDecimal(m.TotalStone1B) + Convert.ToDecimal(m.TotalStone2) + Convert.ToDecimal(m.TotalStone3) + Convert.ToDecimal(m.TotalStone4) + Convert.ToDecimal(m.TotalStone5);

                    //Commit
                    db.SubmitChanges();
                }
            }
            }
    }
    protected static decimal TotalRevalStone1A(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in d.StockProductDJ_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_1A) total += r;

        return total;
    }
    protected static decimal TotalRevalStone1B(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1B = from p in d.StockProductDJ_Stone1B where p.HargaTotal.HasValue && p.StatusTerpasang == 1 select p.HargaTotal.Value;
        foreach (var r in rs_1B) total += r;

        return total;
    }
    protected static decimal TotalRevalStone2(StockProductDJ d)
    {
        decimal total = 0;

        var rs_2 = from p in d.StockProductDJ_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_2) total += r;

        return total;
    }
    protected static decimal TotalRevalStone3(StockProductDJ d)
    {
        decimal total = 0;

        var rs_3 = from p in d.StockProductDJ_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_3) total += r;

        return total;
    }
    protected static decimal TotalRevalStone4(StockProductDJ d)
    {
        decimal total = 0;

        var rs_4 = from p in d.StockProductDJ_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_4) total += r;

        return total;
    }
    protected static decimal TotalRevalStone5(StockProductDJ d)
    {
        decimal total = 0;

        var rs_5 = from p in d.StockProductDJ_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }
    protected static decimal TotalMRevalStone1A(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1A = from p in d.StockProductDJ_Stone1A where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_1A) total += r;

        return total;
    }
    protected static decimal TotalMRevalStone1B(StockProductDJ d)
    {
        decimal total = 0;

        var rs_1B = from p in d.StockProductDJ_Stone1B where p.HargaTotalM.HasValue && p.StatusTerpasang == 1 select p.HargaTotalM.Value;
        foreach (var r in rs_1B) total += r;

        return total;
    }
    protected static decimal TotalMRevalStone2(StockProductDJ d)
    {
        decimal total = 0;

        var rs_2 = from p in d.StockProductDJ_Stone2 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_2) total += r;

        return total;
    }
    protected static decimal TotalMRevalStone3(StockProductDJ d)
    {
        decimal total = 0;

        var rs_3 = from p in d.StockProductDJ_Stone3 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_3) total += r;

        return total;
    }
    protected static decimal TotalMRevalStone4(StockProductDJ d)
    {
        decimal total = 0;

        var rs_4 = from p in d.StockProductDJ_Stone4 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_4) total += r;

        return total;
    }
    protected static decimal TotalMRevalStone5(StockProductDJ d)
    {
        decimal total = 0;

        var rs_5 = from p in d.StockProductDJ_Stone5 where p.HargaTotalM.HasValue select p.HargaTotalM.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }

    //saveRevalBiaya
    protected static void saveRevalBiaya(StockProductDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            int Reval_M = 1;
            int Reval_R = 1;
            int Reval_G = 1;
            int Reval_SK = 1;
            int Reval_SB = 1;
            int Reval_MF = 1;
            int Reval_MU = 1;
            int Reval_Rate = 1;

    
            var product = d.StockProductDJ_CharProduct;
            var pricingbiaya = d.StockProductDJ_PricingBiaya;
            bool ok = true;

            //Pricing
            var prc_fm = db.PricingTableFM.SingleOrDefault(p =>
                p.IDBrand == d.LocWarehouse.IDBrand &&
                p.IDItem == product.ProductItem &&
                p.IDCons == product.ProcessCons &&
                p.Biaya.HasValue &&
                p.Pembagi.HasValue &&
                p.ApprovalTgl.HasValue);

            var prc_fs = db.PricingTableFS.SingleOrDefault(p =>
                p.ID == FS_ID(d) &&
                p.Biaya.HasValue &&
                p.ApprovalTgl.HasValue);

            var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                p.ID == CV.US_General_Costing &&
                p.Harga.HasValue &&
                p.ApprovalTgl.HasValue);

            //Pricing - Validasi
            if (prc_fm == null) { ok = false; }
            if (prc_fs == null) { ok = false; }
            if (prc_us == null) { ok = false; }

            //Process
            if (ok)
            {

                var n = pricingbiaya;

                //n.ID = d.ID;

                //Informasi
                n.NamaBrand = d.LocWarehouse.CompanyBrand.Nama;
                n.NamaItem = product.CharProductItem.Nama;
                n.NamaCons = product.CharProcessCons.Nama;

                //Mounting
                if (Reval_MF == 1)
                {
                    n.Mounting = prc_fm.Biaya.Value;
                    n.MountingNO = prc_fm.InputNO.Value;
                    n.MountingRO = prc_fm.InputRO.Value;
                    n.EfektifMounting = prc_fm.ApprovalTgl.Value;

                    //Setting
                    //n.Setting = prc_fs.Biaya.Value;
                    /*Sementara di Hardcode*/

                    Setting = 0m;
                    Setting += n.StockProductDJ.StockProductDJ_Stone1A.Count() > 0 ? Convert.ToDecimal(n.StockProductDJ.StockProductDJ_Stone1A.Sum(p => p.Setting)) : 0m;
                    Setting += n.StockProductDJ.StockProductDJ_Stone1B.Count() > 0 ? Convert.ToDecimal(n.StockProductDJ.StockProductDJ_Stone1B.Sum(p => p.Setting)) : 0m;
                    Setting += n.StockProductDJ.StockProductDJ_Stone2.Count() > 0 ? Convert.ToDecimal(n.StockProductDJ.StockProductDJ_Stone2.Sum(p => p.Setting)) : 0m;
                    Setting += n.StockProductDJ.StockProductDJ_Stone3.Count() > 0 ? Convert.ToDecimal(n.StockProductDJ.StockProductDJ_Stone3.Sum(p => p.Setting)) : 0m;
                    Setting += n.StockProductDJ.StockProductDJ_Stone4.Count() > 0 ? Convert.ToDecimal(n.StockProductDJ.StockProductDJ_Stone4.Sum(p => p.Setting)) : 0m;
                    Setting += n.StockProductDJ.StockProductDJ_Stone5.Count() > 0 ? Convert.ToDecimal(n.StockProductDJ.StockProductDJ_Stone5.Sum(p => p.Setting)) : 0m;

                    n.Setting = Setting;
                    n.EfektifSetting = prc_fs.ApprovalTgl.Value;
                    db.SubmitChanges();
                }

                /*end hardcode*/

    
    
                //Finishing
                n.Finishing = TotalRevalFinishing(d);

                //Total
                n.TotalRupiah = n.Mounting + n.Setting + n.Finishing;

                if (Reval_Rate == 1)
                {
                    n.TotalRate = prc_us.Harga.Value;
                    n.EfektifRate = prc_us.ApprovalTgl.Value;
                    db.SubmitChanges();
                }

                n.Total = n.TotalRupiah / n.TotalRate;

                //Tgl. Efektif

    
    
                //Commit
                db.SubmitChanges();

            }
            }
    }
    protected static int FS_ID(StockProductDJ d)
    {
        // Master Biaya Setting ID = 1
        return 1;
    }
    protected static decimal TotalRevalFinishing(StockProductDJ d)
    {
        decimal total = 0;

        var rs = from p in d.StockProductDJ_Finishing where p.Biaya.HasValue select p.Biaya.Value;
        foreach (var r in rs) total += r;

        return total;
    }

    //saveRevalMU
    protected static void saveRevalMU(StockProductDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            int Reval_M = 1;
            int Reval_R = 1;
            int Reval_G = 1;
            int Reval_SK = 1;
            int Reval_SB = 1;
            int Reval_MF = 1;
            int Reval_MU = 1;
            int Reval_Rate = 1;

            var product = d.StockProductDJ_CharProduct;
            var costing = d.StockProductDJ_Costing;
            var pricingmu = d.StockProductDJ_PricingMU;
            bool ok = true;

            //Pricing
            var prc_mu = db.PricingTableMU.SingleOrDefault(p =>
                p.IDCategory == product.ProductCategory &&
                p.IDLevel == product.ProductLevel &&
                p.IDDist == product.StoneDist &&
                p.Persen.HasValue &&
                p.ApprovalTgl.HasValue);

            var prc_us = db.PricingTableUS.SingleOrDefault(p =>
                p.ID == CV.US_General_Pricing &&
                p.Harga.HasValue &&
                p.ApprovalTgl.HasValue);

            //Pricing - Validasi
            if (prc_mu == null) { ok = false; }
            if (prc_us == null) { ok = false; }

            //Process
            if (ok)
            {
                var n = pricingmu;

                //Informasi
                n.NamaCategory = product.CharProductCategory.Nama;
                n.NamaLevel = product.CharProductLevel.Nama;
                n.NamaDist = product.CharStoneDist.Nama;

                if (Reval_R == 1)
                {
                    /// R
                    n.ModalUSD = TotalRevalModal(d.ID);

                    n.ModalRupiah = n.ModalUSD * n.ModalRate;
                    db.SubmitChanges();
                }

                if (Reval_MU == 1)
                {
                    //Mark-Up
                    n.MU = prc_mu.Persen.Value;
                    db.SubmitChanges();
                }

                /// Ongkos
                decimal OngkosLain = 0m;

                try
                {
                    OngkosLain = costing.OngkosLain.Value;
                }
                catch (Exception ex)
                {
                    OngkosLain = 0m;
                }

                if (Reval_M == 0 && Reval_R == 0 && Reval_G == 0 && Reval_SK == 0 && Reval_SB == 0 && Reval_MF == 0 && Reval_MU == 0 && Reval_Rate == 1)
                {
                    n.ModalRate = prc_us.Harga.Value;
                    n.HargaRupiah = n.HargaUSD * n.ModalRate;
                    n.HargaRupiahRound = price.RoundTop(n.HargaRupiah);
                    n.EfektifRate = prc_us.ApprovalTgl.Value;

                }
                else if (Reval_M == 1 || Reval_R == 1)
                {
                    n.ModalRate = prc_us.Harga.Value;
                    n.HargaUSD = TotalRevalToMarkUp(d.ID) + (TotalRevalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalRevalStone1B(d.ID) + TotalRevalStone3NMU(d.ID) + TotalRevalStone2(d) + TotalRevalStone4(d) + TotalRevalStone5(d) + (OngkosLain / costing.TotalRate.Value);
                    n.HargaRupiah = (TotalRevalToMarkUp(d.ID) + (TotalRevalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalRevalStone1B(d.ID) + TotalRevalStone3NMU(d.ID) + TotalRevalStone2(d) + TotalRevalStone4(d) + TotalRevalStone5(d)) * n.ModalRate + OngkosLain;
                    n.HargaRupiahRound = price.RoundTop((TotalRevalToMarkUp(d.ID) + (TotalRevalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalRevalStone1B(d.ID) + TotalRevalStone3NMU(d.ID) + TotalRevalStone2(d) + TotalRevalStone4(d) + TotalRevalStone5(d)) * n.ModalRate + OngkosLain);

                }

                //Commit
                db.SubmitChanges();
            }
            }
    }
    protected static decimal TotalRevalModal(int ID)
    {
        using (Map_DB db = new Map_DB())
        {

            decimal total = 0;

            var product = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
            var biaya = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

            total += product.Total;
            total += biaya.Total;

            return total;
            }
    }
    protected static decimal TotalRevalToMarkUp(int ID)
    {
        using (Map_DB db = new Map_DB())
        {

            decimal total = 0;

            var product = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
            var biaya = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

            var stone3 = from p in db.StockProductDJ_Stone3
                         where p.IDForm == ID && (p.GIA == "" || p.GIA == null)
                         select p;
            decimal Stone3ToMarkUp = 0m;
            try
            {
                Stone3ToMarkUp = stone3.Sum(p => p.HargaTotal.Value);
            }
            catch (Exception ex) { }

            //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(product.TotalStone3) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
            //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(Stone3ToMarkUp) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
            total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(Stone3ToMarkUp);
            total += biaya.Total;

            return total;
            }
    }
    protected static decimal TotalRevalStone1B(int ID)
    {
        using (Map_DB db = new Map_DB())
        {

            decimal total = 0;

            var product = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == ID);
            var biaya = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == ID);

            total += Convert.ToDecimal(product.TotalStone1B);
            //total += biaya.Total;

            return total;
            }
    }
    protected static decimal TotalRevalStone3NMU(int ID)
    {
        using (Map_DB db = new Map_DB())
        {

            decimal total = 0;

            var stone3 = from p in db.StockProductDJ_Stone3
                         where p.IDForm == ID && p.GIA != ""
                         select p;
            try
            {
                total = stone3.Sum(p => p.HargaTotal.Value);
            }
            catch (Exception ex) { }

            return total;
            }
    }

    //saveRevalSegmen
    protected static void saveRevalSegmen(StockProductDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            var charproduct = d.StockProductDJ_CharProduct;
            var pricingmu = d.StockProductDJ_PricingMU;
            bool ok = true;

            //Pricing - Validasi
            ok = checkRevalSegmen(d);

            //Process
            if (!ok) { }
            else
            {
                //Product Value
                decimal harga = pricingmu.HargaUSD;

                decimal carat = 0m;
                try
                {
                    carat = (from p in db.StockProductDJ_Stone1A where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone1B where p.IDForm == d.ID && p.StatusTerpasang == 1 select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone2 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone3 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone4 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.StockProductDJ_Stone5 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Max(p => p.Carat);
                }
                catch (Exception ex) { }

                //Parameter
                int IDSegmen = 1;
                int SegmenSistem = 0;
                decimal SegmenMinimum = 0;
                DateTime EfektifSegmen = DateTime.Now;

                //SELECT
                var rs = from p in db.PricingTableSS
                         where
                             p.IDCategory == charproduct.ProductCategory &&
                             p.IDLevel == charproduct.ProductLevel &&
                             p.IDDist == charproduct.StoneDist && p.CharProductSegmen.NamaKode != "NN"
                         orderby p.Minimum
                         select p;

                foreach (var r in rs)
                {
                    if ((r.SegmenSistem == 0 && harga >= r.Minimum.Value) ||
                        (r.SegmenSistem == 1 && carat >= r.Minimum.Value))
                    {

                        IDSegmen = r.IDSegmen;
                        SegmenSistem = r.SegmenSistem;
                        SegmenMinimum = r.Minimum.Value;
                        EfektifSegmen = r.ApprovalTgl.Value;
                    }
                }

                var u = db.StockProductDJ_PricingSegmen.SingleOrDefault(p => p.ID == d.ID);
                db.StockProductDJ_PricingSegmen.DeleteOnSubmit(u);
                db.SubmitChanges();

                StockProductDJ_PricingSegmen n = new StockProductDJ_PricingSegmen();

                n.ID = d.ID;
                n.ProductSegmen = IDSegmen;

                //Informasi
                n.NamaCategory = charproduct.CharProductCategory.Nama;
                n.NamaLevel = charproduct.CharProductLevel.Nama;
                n.NamaDist = charproduct.CharStoneDist.Nama;

                //Segmen
                n.SegmenSistem = SegmenSistem;
                n.SegmenMinimum = SegmenMinimum;

                //Harga dan Carat
                n.HargaUSD = harga;
                n.TotalCarat = carat;

                //Tgl. Efektif
                n.EfektifSegmen = EfektifSegmen;

                ///Commit
                db.StockProductDJ_PricingSegmen.InsertOnSubmit(n);
                db.SubmitChanges();
            }
            }
    }
    protected static bool checkRevalSegmen(StockProductDJ d)
    {
        using (Map_DB db = new Map_DB())
        {

            var charproduct = d.StockProductDJ_CharProduct;
            bool ok = true;

            var rs = from p in db.PricingTableSS
                     where
                         p.IDCategory == charproduct.ProductCategory &&
                         p.IDLevel == charproduct.ProductLevel &&
                         p.IDDist == charproduct.StoneDist && p.CharProductSegmen.NamaKode != "NN"
                     select p;

            foreach (var r in rs)
            {
                if (!r.Minimum.HasValue || !r.ApprovalTgl.HasValue)
                {
                    ok = false;
                    break;
                }
            }

            return ok;
            }
    }
}
