using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

public partial class Page_Register_Resell_ItemAddSave : System.Web.UI.Page
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
    App gp = new App();
    App price = new App();

    decimal Setting = 0m;

    protected int myID { get { return App.QueryInt(this); } }
    protected int myItemIndex { get { return App.QueryInt(this, "item"); } }
    protected int myIDItem { get { return App.QueryInt(this, "iditem"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {

        }

        Save();
    }

    //Exception
    protected void DataError() { Response.Redirect("Resell.aspx"); }


    protected void Save()
    {
        var d = db.Resell.SingleOrDefault(p => p.ID == myID);
        if (d == null) DataError();
        else
        {
            if (myItemIndex == 1)
            {
                var s = db.SalesOrderDJ.SingleOrDefault(p => p.IDProduct == myIDItem && p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                if (s == null) DataError();
                else
                {
                    var c = db.ResellDJ.SingleOrDefault(p => p.IDProduct == myIDItem && p.SalesOrder.StatusVoid == false && (p.Resell.StatusVoid == null || p.Resell.StatusVoid == 0));
                    if (c == null)
                    {
                        ResellDJ n = new ResellDJ();

                        n.IDForm = myID;
                        n.IDProduct = s.IDProduct;
                        n.IDSalesOrder = s.IDForm;
                        n.Nomor = s.StockProductDJ.Nomor;

                        decimal HargaBuyBack = 0m;

                        decimal PPN = 0m;
                        string queryPPN = "SELECT ISNULL(PPN,0) [PPN] FROM SalesOrderDJ WHERE ID = @ID";
                        List<SqlParameter> parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@ID", SqlDbType.Int){ Value = s.ID }
                        };
                        PPN = Convert.ToDecimal(Rs(queryPPN, Db.Cnn_DB, parameters, CommandType.Text).Rows[0][0]);
                        HargaBuyBack = s.TotalBayar.HasValue ? s.TotalBayar.Value - PPN : 0;

                        n.HargaAcuan = HargaBuyBack;
                        n.HargaRupiah = HargaBuyBack;
                        n.ModalUSD = TotalCurrPriceM(s.StockProductDJ);

                        db.ResellDJ.InsertOnSubmit(n);
                        db.SubmitChanges();
                    }
                }
            }
            else if (myItemIndex == 2)
            {
                var s = db.SalesOrderPG.SingleOrDefault(p => p.IDProduct == myIDItem && p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                if (s == null) DataError();
                else
                {
                    var c = db.ResellPG.SingleOrDefault(p => p.IDProduct == myIDItem && p.SalesOrder.StatusVoid == false && (p.Resell.StatusVoid == null || p.Resell.StatusVoid == 0) && p.Resell.Draft);
                    if (c == null)
                    {
                        decimal GoldPrice = 0m;

                        ResellPG n = new ResellPG();

                        n.IDForm = myID;
                        n.IDProduct = s.IDProduct;
                        n.IDSalesOrder = s.IDForm;
                        n.Nomor = s.StockProductPG.Nomor;
                        n.TglResell = DateTime.Now;

                        decimal HargaBuyBack = 0m;

                        decimal TgpBeli = GetTGPPGBeliNew(s.IDProduct);
                        GoldPrice = gp.GetPGPricing(s.StockProductPG, true);
                        //HargaBuyBack = GoldPrice;
                        HargaBuyBack = TgpBeli * Convert.ToDecimal(s.StockProductPG.GrossWeight);

                        n.HargaAcuan = HargaBuyBack;
                        n.HargaRupiah = HargaBuyBack;

                        db.ResellPG.InsertOnSubmit(n);
                        db.SubmitChanges();
                    }
                }
            }
            else if (myItemIndex == 3)
            {
                var s = db.SalesOrderGJ.SingleOrDefault(p => p.IDProduct == myIDItem && p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                if (s == null) DataError();
                else
                {
                    var c = db.ResellGJ.SingleOrDefault(p => p.IDProduct == myIDItem && p.SalesOrder.StatusVoid == false && (p.Resell.StatusVoid == null || p.Resell.StatusVoid == 0));
                    if (c == null)
                    {
                        ResellGJ n = new ResellGJ();

                        n.IDForm = myID;
                        n.IDProduct = s.IDProduct;
                        n.IDSalesOrder = s.IDForm;
                        n.Nomor = s.StockProductGJ.Nomor;
                        n.TglResell = DateTime.Now;

                        decimal HargaBuyBack = 0m;

                        decimal PPN = 0m;
                        string queryPPN = "SELECT ISNULL(PPN,0) [PPN] FROM SalesOrderGJ WHERE ID = @ID";
                        List<SqlParameter> parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@ID", SqlDbType.Int){ Value = s.ID }
                        };
                        PPN = Convert.ToDecimal(Rs(queryPPN, Db.Cnn_DB, parameters, CommandType.Text).Rows[0][0]);
                        HargaBuyBack = s.TotalBayar.HasValue ? s.TotalBayar.Value - PPN : 0;

                        n.HargaAcuan = HargaBuyBack;
                        n.HargaRupiah = HargaBuyBack;

                        db.ResellGJ.InsertOnSubmit(n);
                        db.SubmitChanges();
                    }
                }
            }
            else if (myItemIndex == 4)
            {
                var s = db.SalesOrderLD.SingleOrDefault(p => p.IDProduct == myIDItem && p.StatusResell == 0 && p.SalesOrder.StatusVoid == false);
                if (s == null) DataError();
                else
                {
                    var c = db.ResellLD.SingleOrDefault(p => p.IDProduct == myIDItem && p.SalesOrder.StatusVoid == false && (p.Resell.StatusVoid == null || p.Resell.StatusVoid == 0));
                    if (c == null)
                    {
                        ResellLD n = new ResellLD();

                        n.IDForm = myID;
                        n.IDProduct = s.IDProduct;
                        n.IDSalesOrder = s.IDForm;
                        n.Nomor = s.StockProductLD_Stone1B.Nomor;
                        n.TglResell = DateTime.Now;

                        decimal HargaBuyBack = 0m;

                        int month = DLC.CalcPeriod(Convert.ToDateTime(s.SalesOrder.Tgl), true);

                        var stone = from p in db.StoneCosting1B
                                    where p.ID == s.StockProductLD_Stone1B.IDStone
                                    select p;
                        var stonepricing = from p in db.StonePricing1B
                                           where p.ID == s.StockProductLD_Stone1B.IDStone
                                           select p;
                        var us = from p in db.PricingTableUS
                                 where p.ID == CV.US_General_Costing
                                 select p;
                        var uspricing = from p in db.PricingTableUS
                                        where p.ID == CV.US_General_Pricing
                                        select p;

                        decimal mu = Pricing.ResellMarkUp();

                        if (s.StockProductLD_Stone1B.TotalCarat > 3.0m)
                        {
                            HargaBuyBack = s.StockProductLD_Stone1B.TotalCarat * stone.First().Harga.Value * us.First().Harga.Value;
                            HargaBuyBack = HargaBuyBack + (HargaBuyBack * (mu / 100));
                        }
                        else
                        {
                            if (month >= s.MEPeriodeMin && month < s.MEPeriodeMax)
                                HargaBuyBack = s.MENominal.Value;
                            else if (month >= s.R1_PeriodeMin && month < s.R1_PeriodeMax)
                            {
                                HargaBuyBack = (s.R1_Rumus.Value / 100) * s.StockProductLD_Stone1B.TotalCarat * stonepricing.First().Harga.Value * uspricing.First().Harga.Value;
                                //HargaBuyBack = HargaBuyBack + (HargaBuyBack * (mu/100));
                            }
                            else if (month >= s.R2_PeriodeMin)
                            {
                                HargaBuyBack = s.StockProductLD_Stone1B.TotalCarat * stone.First().Harga.Value * us.First().Harga.Value;
                                HargaBuyBack = HargaBuyBack + (HargaBuyBack * (mu / 100));
                            }
                        }


                        n.HargaAcuan = HargaBuyBack;
                        n.HargaRupiah = HargaBuyBack;

                        n.HargaAcuan = HargaBuyBack;
                        n.HargaRupiah = HargaBuyBack;

                        db.ResellLD.InsertOnSubmit(n);
                        db.SubmitChanges();
                    }
                }
            }
            Response.Redirect("Resell_Data.aspx?id=" + myID);
        }
    }

    protected decimal CalcResellPrice(StockProductDJ d)
    {
        decimal HargaAcuan = 0m;
        HargaAcuan = TotalModal(d);

        return HargaAcuan;
    }
    protected decimal CalcResellPriceCurr(StockProductDJ d)
    {
        decimal HargaAcuan = 0m;
        HargaAcuan = TotalCurrPrice(d);

        return HargaAcuan;
    }
    protected decimal CalcResellPriceCurrM(StockProductDJ d)
    {
        decimal HargaM = 0m;
        HargaM = TotalCurrPriceM(d);

        return HargaM;
    }
    protected decimal CalcResellPriceCurr(StockProductGJ d)
    {
        decimal HargaAcuan = 0m;
        HargaAcuan = TotalCurrPrice(d);

        return HargaAcuan;
    }
    protected decimal TotalCurrPrice(StockProductDJ d)
    {
        decimal Total = 0m;
        decimal TotalStone = 0m;
        decimal TotalFinishing = 0m;
        decimal TotalBiaya = 0m;
        decimal TotalToMU = 0m;
        decimal mu = 0m;
        decimal Rate = 0m;

        var prc_mu = db.PricingTableMU.SingleOrDefault(p =>
            p.IDCategory == d.StockProductDJ_CharProduct.ProductCategory &&
            p.IDLevel == d.StockProductDJ_CharProduct.ProductLevel &&
            p.IDDist == d.StockProductDJ_CharProduct.StoneDist &&
            p.Persen.HasValue &&
            p.ApprovalTgl.HasValue);

        mu = prc_mu.Persen.Value;

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        Rate = prc_us.Harga.Value;

        TotalStone = TotalStone1A(d) + TotalStone1B(d) + TotalStone2(d) + TotalStone3(d) + TotalStone4(d) + TotalStone5(d);
        TotalFinishing = CalcFinishing(d);
        TotalBiaya = CalculateTotalBiaya(d);

        Total = (TotalToMarkUp(d) + (TotalToMarkUp(d) * (mu / (decimal)100)) + TotalStone1B(d) + TotalStone3NMU(d) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d)) * Rate;

        return Total;
    }
    protected decimal TotalCurrPriceM(StockProductDJ d)
    {
        decimal Total = 0m;
        decimal TotalStone = 0m;
        decimal TotalFinishing = 0m;
        decimal TotalBiaya = 0m;
        decimal TotalToMU = 0m;
        decimal mu = 0m;
        decimal Rate = 1m;

        var prc_mu = db.PricingTableMU.SingleOrDefault(p =>
            p.IDCategory == d.StockProductDJ_CharProduct.ProductCategory &&
            p.IDLevel == d.StockProductDJ_CharProduct.ProductLevel &&
            p.IDDist == d.StockProductDJ_CharProduct.StoneDist &&
            p.Persen.HasValue &&
            p.ApprovalTgl.HasValue);

        if (prc_mu != null)
            mu = prc_mu.Persen.Value;

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        if (prc_us != null)
            Rate = prc_us.Harga.Value;

        TotalStone = TotalStone1A(d) + TotalStone1B(d) + TotalStone2(d) + TotalStone3(d) + TotalStone4(d) + TotalStone5(d);
        TotalFinishing = CalcFinishing(d);
        TotalBiaya = CalculateTotalBiaya(d);

        Total = (TotalToMarkUp(d) + (TotalToMarkUp(d) * (mu / (decimal)100)) + TotalStone1B(d) + TotalStone3NMU(d) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d));

        return Total;
    }
    protected decimal TotalCurrPrice(StockProductGJ d)
    {
        decimal Total = 0m;
        decimal TotalStone = 0m;
        decimal TotalFinishing = 0m;
        decimal TotalBiaya = 0m;
        decimal TotalToMU = 0m;
        decimal mu = 100m;
        decimal Rate = 0m;

        var prc_mu = db.PricingTableMU.SingleOrDefault(p =>
            p.IDCategory == d.StockProductGJ_CharProduct.ProductCategory &&
            p.IDLevel == d.StockProductGJ_CharProduct.ProductLevel &&
            p.IDDist == d.StockProductGJ_CharProduct.StoneDist &&
            p.Persen.HasValue &&
            p.ApprovalTgl.HasValue);

        if (prc_mu != null)
        {
            mu = prc_mu.Persen.Value;
        }

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        Rate = prc_us.Harga.Value;

        TotalFinishing = CalcFinishing(d);
        TotalBiaya = CalculateTotalBiaya(d);

        Total = (TotalToMarkUp(d) + (TotalToMarkUp(d) * (mu / (decimal)100))) * Rate;

        return Total;
    }
    protected decimal TotalStone3NMU(StockProductDJ d)
    {
        decimal total = 0;

        var stone3 = from p in db.StockProductDJ_Stone3
                     where p.IDForm == d.ID && p.GIA != ""
                     select p;
        try
        {
            total = stone3.Sum(p => p.Stone3.StonePricing3.Harga.Value * p.TotalCarat);
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalToMarkUp(StockProductDJ d)
    {
        decimal total = 0;
        decimal totalstone1a = 0m;

        var product = db.StockProductDJ_PricingProduct.SingleOrDefault(p => p.ID == d.ID);
        var biaya = db.StockProductDJ_PricingBiaya.SingleOrDefault(p => p.ID == d.ID);

        var stone3 = from p in db.StockProductDJ_Stone3
                     where p.IDForm == d.ID && (p.GIA == "" || p.GIA == null)
                     select p;
        var stone1a = from p in db.StockProductDJ_Stone1A
                      where p.IDForm == d.ID
                      select p;
        decimal Stone3ToMarkUp = 0m;
        try
        {
            Stone3ToMarkUp = stone3.Sum(p => p.Stone3.StonePricing3.Harga.Value * p.TotalCarat);
        }
        catch (Exception ex) { }

        try
        {
            totalstone1a = stone1a.Sum(p => p.Stone1A.StonePricing1A.Harga.Value * p.TotalCarat);
        }
        catch (Exception ex)
        {
            totalstone1a = 0m;
        }


        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(product.TotalStone3) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(Stone3ToMarkUp) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(product.TotalStone2) + Convert.ToDecimal(Stone3ToMarkUp);
        total += CalculateTotalProduct(d) + totalstone1a + Convert.ToDecimal(Stone3ToMarkUp);
        total += CalculateTotalBiaya(d);

        return total;
    }
    protected decimal TotalToMarkUp(StockProductGJ d)
    {
        decimal total = 0;

        total += CalculateTotalProduct(d);
        total += CalculateTotalBiaya(d);

        return total;
    }
    protected decimal CalcResellPrice(StockProductGJ d)
    {
        decimal HargaAcuan = 0m;
        HargaAcuan = TotalModal(d);

        return HargaAcuan;
    }
    protected decimal TotalModal(StockProductDJ d)
    {
        decimal total = 0;
        decimal TotalProduct = 0m;
        decimal TotalBiaya = 0m;
        decimal rate = 0m;

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        rate = prc_us.Harga.Value;

        TotalProduct = CalculateTotalProduct(d);
        //TotalBiaya = CalculateTotalBiaya(d);

        total += TotalProduct;
        //total += TotalBiaya;

        total = total * rate;

        return total;
    }
    protected decimal TotalModal(StockProductGJ d)
    {
        decimal total = 0;
        decimal TotalProduct = 0m;
        decimal TotalBiaya = 0m;
        decimal rate = 0m;

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        rate = prc_us.Harga.Value;

        TotalProduct = CalculateTotalProduct(d);
        //TotalBiaya = CalculateTotalBiaya(d);

        total += TotalProduct;
        //total += TotalBiaya;

        total = total * rate;

        return total;
    }
    protected decimal CalculateTotalProduct(StockProductDJ d)
    {
        decimal Total = 0m;

        //Pricing
        var prc_tg = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == CV.TG_PG_Resell &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //decimal GoldRate = prc_tg.Harga.Value;
        decimal GoldRate = d.StockProductDJ_CostingProduct.GoldRate;
        decimal TotalProduct = 0m;

        TotalProduct = GoldRate * d.StockProductDJ_CharProduct.NettoWeight * (d.StockProductDJ_CharProduct.KadarTukaran / (decimal)100);
        Total = TotalProduct + Convert.ToDecimal(TotalStone1A(d)) + Convert.ToDecimal(TotalStone1B(d)) + Convert.ToDecimal(TotalStone2(d)) + Convert.ToDecimal(TotalStone3(d)) + Convert.ToDecimal(TotalStone4(d)) + Convert.ToDecimal(TotalStone5(d));
        //Total = TotalStone1A(d);

        //Total = GoldRate;

        return Total;
    }
    protected decimal CalculateTotalProduct(StockProductGJ d)
    {
        decimal Total = 0m;

        //Pricing
        var prc_tg = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == CV.TG_PG_Resell &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        //decimal GoldRate = prc_tg.Harga.Value;
        decimal GoldRate = d.StockProductGJ_CostingProduct.GoldRate;
        decimal TotalProduct = 0m;

        TotalProduct = GoldRate * d.StockProductGJ_CharProduct.NettoWeight * (d.StockProductGJ_CharProduct.KadarTukaran / (decimal)100);
        Total = TotalProduct;
        //Total = TotalStone1A(d);

        //Total = GoldRate;

        return Total;
    }
    protected decimal TotalStone1A(StockProductDJ d)
    {
        decimal total = 0m;
        foreach (var r in d.StockProductDJ_Stone1A)
        {
            decimal HargaSatuanM = 0m;
            decimal HargaTotalM = 0m;

            StoneCosting1A prc = r.Stone1A.StoneCosting1A;
            HargaSatuanM = prc.Harga.Value;
            HargaTotalM = HargaSatuanM * r.TotalCarat;


            decimal OngkosPB = 0m;
            OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone1A.Parcel101.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
            r.Setting = OngkosPB;

            Setting += OngkosPB;

            total += HargaTotalM;
        }

        return total;
    }
    protected decimal TotalStone1B(StockProductDJ d)
    {
        decimal total = 0m;
        foreach (var r in d.StockProductDJ_Stone1B)
        {
            decimal HargaSatuanM = 0m;
            decimal HargaTotalM = 0m;

            StoneCosting1B prc = r.Stone1B.StoneCosting1B;
            HargaSatuanM = prc.Harga.HasValue ? prc.Harga.Value : 0;
            HargaTotalM = HargaSatuanM * r.TotalCarat;

            decimal OngkosPB = 0m;
            OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone1B.Parcel101.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
            r.Setting = OngkosPB;

            Setting += OngkosPB;

            total += HargaTotalM;
        }

        return total;
    }
    protected decimal TotalStone2(StockProductDJ d)
    {
        decimal total = 0m;
        foreach (var r in d.StockProductDJ_Stone2)
        {
            decimal HargaSatuanM = 0m;
            decimal HargaTotalM = 0m;

            StoneCosting2 prc = r.Stone2.StoneCosting2;
            HargaSatuanM = prc.Harga.Value;
            HargaTotalM = HargaSatuanM * r.TotalCarat;

            decimal OngkosPB = 0m;
            OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone2.Parcel201.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
            r.Setting = OngkosPB;

            Setting += OngkosPB;

            total += HargaTotalM;
        }

        return total;
    }
    protected decimal TotalStone3(StockProductDJ d)
    {
        decimal total = 0m;
        foreach (var r in d.StockProductDJ_Stone3)
        {
            decimal HargaSatuanM = 0m;
            decimal HargaTotalM = 0m;

            StoneCosting3 prc = r.Stone3.StoneCosting3;
            HargaSatuanM = prc.Harga.Value;
            HargaTotalM = HargaSatuanM * r.TotalCarat;

            decimal OngkosPB = 0m;
            OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone3.Parcel301.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
            r.Setting = OngkosPB;

            Setting += OngkosPB;

            total += HargaTotalM;
        }

        return total;
    }
    protected decimal TotalStone4(StockProductDJ d)
    {
        decimal total = 0m;
        foreach (var r in d.StockProductDJ_Stone4)
        {
            decimal HargaSatuanM = 0m;
            decimal HargaTotalM = 0m;

            StoneCosting4 prc = r.Stone4.StoneCosting4;
            HargaSatuanM = prc.Harga.Value;
            HargaTotalM = HargaSatuanM * r.TotalCarat;

            decimal OngkosPB = 0m;
            OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone4.Parcel401.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
            r.Setting = OngkosPB;

            Setting += OngkosPB;

            total += HargaTotalM;
        }

        return total;
    }
    protected decimal TotalStone5(StockProductDJ d)
    {
        decimal total = 0m;
        foreach (var r in d.StockProductDJ_Stone5)
        {
            decimal HargaSatuanM = 0m;
            decimal HargaTotalM = 0m;

            StoneCosting5 prc = r.Stone5.StoneCosting5;
            HargaSatuanM = prc.Harga.Value;
            HargaTotalM = HargaSatuanM * r.TotalCarat;

            decimal OngkosPB = 0m;
            OngkosPB = price.CalculateOngkosPB(d.StockProductDJ_CharProduct.CharProductItem.NamaKode, r.Stone5.Parcel501.NamaKode, d.StockProductDJ_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
            r.Setting = OngkosPB;

            Setting += OngkosPB;

            total += HargaTotalM;
        }

        return total;
    }

    protected decimal CalculateTotalBiaya(StockProductDJ d)
    {
        decimal total = 0m;
        decimal finishing = 0m;
        decimal mounting = 0m;
        decimal rate = 1m;
        decimal TotalRupiah = 0m;

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        if (prc_us != null)
            rate = prc_us.Harga.Value;

        finishing = CalcFinishing(d);
        mounting = CalcMounting(d);

        //Total
        TotalRupiah = mounting + Setting + finishing;
        total = TotalRupiah / rate;

        return total;
    }
    protected decimal CalculateTotalBiaya(StockProductGJ d)
    {
        decimal total = 0m;
        decimal finishing = 0m;
        decimal mounting = 0m;
        decimal rate = 0m;
        decimal TotalRupiah = 0m;

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CV.US_General_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        rate = prc_us.Harga.Value;

        finishing = CalcFinishing(d);
        mounting = CalcMounting(d);

        //Total
        TotalRupiah = mounting + Setting + finishing;
        total = TotalRupiah / rate;

        return total;
    }
    protected decimal CalcFinishing(StockProductDJ d)
    {
        decimal total = 0m;
        var rs = from p in d.StockProductDJ_Finishing select p;
        var product = d.StockProductDJ_CharProduct;
        foreach (var r in rs)
        {
            string sebab = "";
            bool ok = true;

            //Pricing
            var prc_ff = db.PricingTableFF.SingleOrDefault(p =>
                p.IDItem == product.ProductItem &&
                p.IDFinishing == r.IDFinishing &&
                p.Biaya.HasValue &&
                p.ApprovalTgl.HasValue
                );

            total += prc_ff.Biaya.Value;
        }

        return total;
    }
    protected decimal CalcFinishing(StockProductGJ d)
    {
        decimal total = 0m;
        var rs = from p in d.StockProductGJ_Finishing select p;
        var product = d.StockProductGJ_CharProduct;
        foreach (var r in rs)
        {
            string sebab = "";
            bool ok = true;

            //Pricing
            var prc_ff = db.PricingTableFF.SingleOrDefault(p =>
                p.IDItem == product.ProductItem &&
                p.IDFinishing == r.IDFinishing &&
                p.Biaya.HasValue &&
                p.ApprovalTgl.HasValue
                );

            total += prc_ff.Biaya.Value;
        }

        return total;
    }
    protected decimal CalcMounting(StockProductDJ d)
    {
        decimal total = 0m;
        var product = d.StockProductDJ_CharProduct;

        //Pricing
        if (d.IDWarehouse.HasValue)
        {
            var prc_fm = db.PricingTableFM.SingleOrDefault(p =>
                p.IDBrand == d.LocWarehouse.IDBrand &&
                p.IDItem == product.ProductItem &&
                p.IDCons == product.ProcessCons &&
                p.Biaya.HasValue &&
                p.Pembagi.HasValue &&
                p.ApprovalTgl.HasValue);

            if (prc_fm != null)
                total = prc_fm.Biaya.Value;
        }

        return total;
    }
    protected decimal CalcMounting(StockProductGJ d)
    {
        decimal total = 0m;
        var product = d.StockProductGJ_CharProduct;

        //Pricing
        var prc_fm = db.PricingTableFM.SingleOrDefault(p =>
            p.IDBrand == d.LocWarehouse.IDBrand &&
            p.IDItem == product.ProductItem &&
            p.IDCons == product.ProcessCons &&
            p.Biaya.HasValue &&
            p.Pembagi.HasValue &&
            p.ApprovalTgl.HasValue);

        total = prc_fm.Biaya.Value;

        return total;
    }

    protected decimal GetTGPPGBeliNew(int IDProduct)
    {
        decimal GoldPrice = 0m;

        string query = "sp_get_tgp_jual_beli_plu";
        List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@ID_PRODUCT", SqlDbType.Int){ Value = IDProduct },
            new SqlParameter("@TIPE_PRODUCT", SqlDbType.VarChar){ Value = "PG" }
        };

        DataTable dt = Rs(query, Db.Cnn_DB, parameters, CommandType.StoredProcedure);
        if (dt.Rows.Count == 1)
        {
            GoldPrice = Convert.ToDecimal(dt.Rows[0]["HARGA_BELI"]);
        }

        return GoldPrice;
    }

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