using System;
using System.Linq;

public partial class Page_Approval_DJ_Pricing : System.Web.UI.Page
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
    App price = new App();

    decimal Setting = 0m;

    protected int myID { get { return App.QueryInt(this); } }

    //Exception
    protected void PricingError(string sebab)
    {
        Response.Redirect("Design_Picture.aspx?id=" + myID + "&prcerror=" + sebab);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Delete
            deleteProcess(d);

            //Process
            var product = d.ProductDesign_CharProduct;
            if (product == null) PricingError("Product Char.");
            else
            {
                saveStone(d);
                saveFinishing(d, product);

                saveProduct(d, product);
                saveBiaya(d, product);

                saveMU(d, product);
                saveSegmen(d, product);
            }

            //Redirect
            Response.Redirect("Design_Submit.aspx?id=" + myID);
        }
    }

    //Delete
    protected void deleteProcess(ProductDesign d)
    {
        if (d.ProductDesign_PricingProduct != null)
            db.ProductDesign_PricingProduct.DeleteOnSubmit(
                d.ProductDesign_PricingProduct);

        if (d.ProductDesign_PricingBiaya != null)
            db.ProductDesign_PricingBiaya.DeleteOnSubmit(
                d.ProductDesign_PricingBiaya);

        if (d.ProductDesign_PricingMU != null)
            db.ProductDesign_PricingMU.DeleteOnSubmit(
                d.ProductDesign_PricingMU);

        if (d.ProductDesign_PricingSegmen != null)
            db.ProductDesign_PricingSegmen.DeleteOnSubmit(
                d.ProductDesign_PricingSegmen);

        //Commit
        db.SubmitChanges();
    }

    //Stone Pricing
    protected void saveStone(ProductDesign d)
    {
        saveStone1A(d);
        saveStone1B(d);
        saveStone2(d);
        saveStone3(d);
        saveStone4(d);
        saveStone5(d);
    }
    protected void saveStone1A(ProductDesign d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(0);
        decimal OngkosPB = 0m;

        var rs = from p in d.ProductDesign_Stone1A select p;
        foreach (var r in rs)
        {

            StonePricing1A prc = r.Stone1A.StonePricing1A;
            if (prc == null) PricingError(sebab);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.ProductDesign_CharProduct.CharProductItem.NamaKode, r.Stone1A.Parcel101.NamaKode, d.ProductDesign_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone1B(ProductDesign d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(1);

        decimal OngkosPB = 0m;

        var rs = from p in d.ProductDesign_Stone1B where p.StatusTerpasang == 1 select p;
        foreach (var r in rs)
        {

            StonePricing1B prc = r.Stone1B.StonePricing1B;
            if (prc == null) PricingError(sebab);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue || !prc.InputH.HasValue || !prc.InputP.HasValue) PricingError(sebab);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    //Rapaport
                    r.HargaInputH = prc.InputH.Value;
                    r.HargaInputP = prc.InputP.Value;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.ProductDesign_CharProduct.CharProductItem.NamaKode, r.Stone1B.Parcel101.NamaKode, d.ProductDesign_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone2(ProductDesign d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(2);

        decimal OngkosPB = 0m;

        var rs = from p in d.ProductDesign_Stone2 select p;
        foreach (var r in rs)
        {

            StonePricing2 prc = r.Stone2.StonePricing2;
            if (prc == null) PricingError(sebab);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.ProductDesign_CharProduct.CharProductItem.NamaKode, r.Stone2.Parcel201.NamaKode, d.ProductDesign_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone3(ProductDesign d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(3);

        decimal OngkosPB = 0m;

        var rs = from p in d.ProductDesign_Stone3 select p;
        foreach (var r in rs)
        {

            StonePricing3 prc = r.Stone3.StonePricing3;
            if (prc == null) PricingError(sebab);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.ProductDesign_CharProduct.CharProductItem.NamaKode, r.Stone3.Parcel301.NamaKode, d.ProductDesign_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone4(ProductDesign d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(4);

        decimal OngkosPB = 0m;

        var rs = from p in d.ProductDesign_Stone4 select p;
        foreach (var r in rs)
        {

            StonePricing4 prc = r.Stone4.StonePricing4;
            if (prc == null) PricingError(sebab);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.ProductDesign_CharProduct.CharProductItem.NamaKode, r.Stone4.Parcel401.NamaKode, d.ProductDesign_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }
    protected void saveStone5(ProductDesign d)
    {
        //Exception-Cause
        string sebab = DL.StoneIndex(5);

        decimal OngkosPB = 0m;

        var rs = from p in d.ProductDesign_Stone5 select p;
        foreach (var r in rs)
        {

            StonePricing5 prc = r.Stone5.StonePricing5;
            if (prc == null) PricingError(sebab);
            else
            {

                if (!prc.Harga.HasValue || !prc.ApprovalTgl.HasValue) PricingError(sebab);
                else
                {
                    //Harga
                    r.HargaSatuan = prc.Harga.Value;
                    r.HargaTotal = r.HargaSatuan * r.TotalCarat;

                    //Tgl. Efektif
                    r.Efektif = prc.ApprovalTgl.Value;

                    //Setting
                    OngkosPB = price.CalculateOngkosPB(d.ProductDesign_CharProduct.CharProductItem.NamaKode, r.Stone5.Parcel501.NamaKode, d.ProductDesign_CharProduct.CharStoneDist.NamaKode, Convert.ToDecimal(r.TotalCarat), Convert.ToDecimal(r.TotalButir));
                    r.Setting = OngkosPB;

                    Setting += OngkosPB;

                    //Commit
                    db.SubmitChanges();
                }
            }
        }
    }

    //Finishing
    protected void saveFinishing(ProductDesign d, ProductDesign_CharProduct product)
    {
        var rs = from p in d.ProductDesign_Finishing select p;
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

            bool SAPActivate = false;
            SAPActivate = SAP.xActivate;

            if (SAPActivate == false)
            {

                //Pricing - Validasi
                if (prc_ff == null) { ok = false; sebab = "Finishing Fee"; }
            }

            //Process
            if (!ok) PricingError(sebab);
            else
            {
                r.Biaya = prc_ff.Biaya.Value;
                r.Efektif = prc_ff.ApprovalTgl.Value;

                //Commit
                db.SubmitChanges();
            }
        }
    }

    //Product
    protected void saveProduct(ProductDesign d, ProductDesign_CharProduct product)
    {
        string sebab = "";
        bool ok = true;

        int IDTG_Pricing = CVV.TG_PG1_Pricing;
        if (d.ProductDesign_CharProduct.CharFrameMaterial.NamaKode == "AU")
        {
            IDTG_Pricing = CVV.TG_PG1_Pricing;
        }
        else if (d.ProductDesign_CharProduct.CharFrameMaterial.NamaKode == "PL")
        {
            IDTG_Pricing = CVV.TG_PG_Paladium2;            
        }
        else if (d.ProductDesign_CharProduct.CharFrameMaterial.NamaKode == "PT")
        {
            IDTG_Pricing = CVV.TG_PG_Platina2;
        }
        else if (d.ProductDesign_CharProduct.CharFrameMaterial.NamaKode == "SIL")
        {
            IDTG_Pricing = CVV.TG_PG_Silver2;
        }

        //Pricing
        var prc_tg = db.PricingTableTG.SingleOrDefault(p =>
            p.ID == IDTG_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);

        bool SAPActivate = false;
        SAPActivate = SAP.xActivate;

        if (SAPActivate == false)
        {

            //Pricing - Validasi
            if (prc_tg == null) { ok = false; sebab = "Today Gold Price"; }
        }

        //Process
        if (!ok) PricingError(sebab);
        else
        {
            ProductDesign_PricingProduct n = new ProductDesign_PricingProduct();

            n.ID = d.ID;

            //Gold Price
            n.GoldRate = prc_tg.Harga.Value;
            n.EfektifGold = prc_tg.ApprovalTgl.Value;

            //Informasi Berat
            n.Netto = product.NettoWeight;
            n.Kadar = product.KadarTukaran;

            //Total
            n.TotalProduct = n.GoldRate * n.Netto * (n.Kadar / (decimal)100);
            n.TotalStone1A = Convert.ToDecimal(TotalStone1A(d));
            n.TotalStone1B = Convert.ToDecimal(TotalStone1B(d));
            n.TotalStone2 = Convert.ToDecimal(TotalStone2(d));
            n.TotalStone3 = Convert.ToDecimal(TotalStone3(d));
            n.TotalStone4 = Convert.ToDecimal(TotalStone4(d));
            n.TotalStone5 = Convert.ToDecimal(TotalStone5(d));
            n.Total = n.TotalProduct + Convert.ToDecimal(n.TotalStone1A) + Convert.ToDecimal(n.TotalStone1B) + Convert.ToDecimal(n.TotalStone2) + Convert.ToDecimal(n.TotalStone3) + Convert.ToDecimal(n.TotalStone4) + Convert.ToDecimal(n.TotalStone5);

            //Commit
            db.ProductDesign_PricingProduct.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }

    //Biaya
    protected void saveBiaya(ProductDesign d, ProductDesign_CharProduct product)
    {
        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_fm = db.PricingTableFM.SingleOrDefault(p =>
            p.IDBrand == d.DataHuman.IDBrand &&
            p.IDItem == product.ProductItem &&
            p.IDCons == product.ProcessCons &&
            p.Biaya.HasValue &&
            p.Pembagi.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_fs = db.PricingTableFS.SingleOrDefault(p =>
            p.ID == FS_ID(d, product) &&
            p.Biaya.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CVV.US_General_Costing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);


        //bool SAPActivate = false;
        //SAPActivate = SAP.xActivate;

        //if (SAPActivate == false)
        //{
            //Pricing - Validasi
            if (prc_fm == null) { ok = false; sebab = "Mounting Fee"; }
            if (prc_fs == null) { ok = false; sebab = "Setting Fee"; }
            if (prc_us == null) { ok = false; sebab = "USD Rate"; }
        //}

        //Process
        if (!ok) PricingError(sebab);
        else
        {
            ProductDesign_PricingBiaya n = new ProductDesign_PricingBiaya();

            n.ID = d.ID;

            //Informasi
            n.NamaBrand = d.DataHuman.CompanyBrand.Nama;
            n.NamaItem = product.CharProductItem.Nama;
            n.NamaCons = product.CharProcessCons.Nama;

            

           
                //Mounting
                n.Mounting = prc_fm.Biaya.Value;
                n.MountingNO = prc_fm.InputNO.Value;
                n.MountingRO = prc_fm.InputRO.Value;

                //Setting
                //n.Setting = prc_fs.Biaya.Value;
                /*Sementara di Hardcode*/
                n.Setting = Setting;

                /*end hardcode*/



                //Finishing
                n.Finishing = TotalFinishing(d);

                //Total
                n.TotalRupiah = n.Mounting + n.Setting + n.Finishing;
                n.TotalRate = prc_us.Harga.Value;
                n.Total = n.TotalRupiah / n.TotalRate;

                //Tgl. Efektif
                n.EfektifMounting = prc_fm.ApprovalTgl.Value;
                n.EfektifSetting = prc_fs.ApprovalTgl.Value;
                n.EfektifRate = prc_us.ApprovalTgl.Value;

                //Commit
                db.ProductDesign_PricingBiaya.InsertOnSubmit(n);
                db.SubmitChanges();
            
         


          
        }
    }
    protected int FS_ID(ProductDesign d, ProductDesign_CharProduct product)
    {
        return 1; //Masih di-hardcoded. BELUM SELESAI !
    }

    //Mark-Up
    protected void saveMU(ProductDesign d, ProductDesign_CharProduct product)
    {
        string sebab = "";
        bool ok = true;

        //Pricing
        var prc_mu = db.PricingTableMU.SingleOrDefault(p =>
            p.IDCategory == product.ProductCategory &&
            p.IDLevel == product.ProductLevel &&
            p.IDDist == product.StoneDist &&
            p.Persen.HasValue &&
            p.ApprovalTgl.HasValue);

        var prc_us = db.PricingTableUS.SingleOrDefault(p =>
            p.ID == CVV.US_General_Pricing &&
            p.Harga.HasValue &&
            p.ApprovalTgl.HasValue);


        bool SAPActivate = false;
        SAPActivate = SAP.xActivate;

        if (SAPActivate == false)
        {
            //Pricing - Validasi
            if (prc_mu == null) { ok = false; sebab = "Diamond Jewelry MU"; }
            if (prc_us == null) { ok = false; sebab = "USD Rate"; }
        }

        //Process
        if (!ok) PricingError(sebab);
        else
        {
           

            ProductDesign_PricingMU n = new ProductDesign_PricingMU();

            n.ID = d.ID;

            //Informasi
            n.NamaCategory = product.CharProductCategory.Nama;
            n.NamaLevel = product.CharProductLevel.Nama;
            n.NamaDist = product.CharStoneDist.Nama;

           

            //Modal
            n.ModalUSD = TotalModal(d.ID);
            decimal rate = prc_us.Harga.Value;
            DateTime efektif_rate = prc_us.ApprovalTgl.Value;
            if (Db.SingleBool("SELECT dbo.fx_isFashionRate(" + product.ID + ", 2) ", Db.Cnn_DB))
            {
                rate = Db.SingleDecimal("SELECT Harga From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
                efektif_rate = Db.SingleTime("SELECT ApprovalTgl From PricingTableUS WHERE ID = 5", Db.Cnn_DB);
            }
            n.ModalRate = rate;
            n.ModalRupiah = n.ModalUSD * n.ModalRate;

            //Mark-Up
            n.MU = prc_mu.Persen.Value;

            //Harga Final
            //n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID);
            n.HargaUSD = TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d);
            //n.HargaRupiah = (TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID)) * n.ModalRate;
            n.HargaRupiah = (TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d)) * n.ModalRate;
            //n.HargaRupiahRound = price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID)) * n.ModalRate);
            n.HargaRupiahRound = price.RoundTop((TotalToMarkUp(d.ID) + (TotalToMarkUp(d.ID) * (n.MU / (decimal)100)) + TotalStone1B(d.ID) + TotalStone3NMU(d.ID) + TotalStone2(d) + TotalStone4(d) + TotalStone5(d)) * n.ModalRate);


            //Tgl. Efektif
            //n.EfektifRate = prc_us.ApprovalTgl.Value;
            n.EfektifRate = efektif_rate;

            //Commit
            db.ProductDesign_PricingMU.InsertOnSubmit(n);
            db.SubmitChanges();
            
          
        }
    }

    //Segmen System
    protected void saveSegmen(ProductDesign d, ProductDesign_CharProduct product)
    {
        string sebab = "Segmen System";
        bool ok = true;

        //Pricing - Validasi
        ok = checkSegmen(product);

        //Process
        if (!ok) PricingError(sebab);
        else
        {
            //Product Value
            decimal harga = HargaUSD(d.ID);
            //decimal carat = product.StoneCarat;

            decimal carat = 0m;

            try
            {
                carat = (from p in db.ProductDesign_Stone1A where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductDesign_Stone1B where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductDesign_Stone2 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductDesign_Stone3 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductDesign_Stone4 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Union
                            (from p in db.ProductDesign_Stone5 where p.IDForm == d.ID select new { Carat = p.CaratButir }).Max(p => p.Carat);
            }
            catch (Exception ex) { }

            //Parameter
            int IDSegmen = 0;
            int SegmenSistem = 0;
            decimal SegmenMinimum = 0;
            DateTime EfektifSegmen = DateTime.Now;

            //SELECT
            var rs = from p in db.PricingTableSS
                     where
                         p.IDCategory == product.ProductCategory &&
                         p.IDLevel == product.ProductLevel &&
                         p.IDDist == product.StoneDist && p.CharProductSegmen.NamaKode != "NN"
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

            ProductDesign_PricingSegmen n = new ProductDesign_PricingSegmen();

            n.ID = d.ID;
            n.ProductSegmen = IDSegmen;

            //Informasi
            n.NamaCategory = product.CharProductCategory.Nama;
            n.NamaLevel = product.CharProductLevel.Nama;
            n.NamaDist = product.CharStoneDist.Nama;

            //Segmen
            n.SegmenSistem = SegmenSistem;
            n.SegmenMinimum = SegmenMinimum;

            //Harga dan Carat
            n.HargaUSD = harga;
            n.TotalCarat = carat;

            //Tgl. Efektif
            n.EfektifSegmen = EfektifSegmen;

            ///Commit
            db.ProductDesign_PricingSegmen.InsertOnSubmit(n);
            db.SubmitChanges();
        }
    }
    protected bool checkSegmen(ProductDesign_CharProduct product)
    {
        bool ok = true;

        var rs = from p in db.PricingTableSS
                 where
                     p.IDCategory == product.ProductCategory &&
                     p.IDLevel == product.ProductLevel &&
                     p.IDDist == product.StoneDist && p.CharProductSegmen.NamaKode != "NN"
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

    //Sub-Total
    protected decimal TotalStone(ProductDesign d)
    {
        decimal total = 0;

        var rs_1A = from p in d.ProductDesign_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_1A) total += r;

        var rs_1B = from p in d.ProductDesign_Stone1B where p.HargaTotal.HasValue && p.StatusTerpasang == 1 select p.HargaTotal.Value;
        foreach (var r in rs_1B) total += r;

        var rs_2 = from p in d.ProductDesign_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_2) total += r;

        var rs_3 = from p in d.ProductDesign_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_3) total += r;

        var rs_4 = from p in d.ProductDesign_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_4) total += r;

        var rs_5 = from p in d.ProductDesign_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
        foreach (var r in rs_5) total += r;

        return total;
    }
    //Sub-Total
    protected decimal TotalStone1A(ProductDesign d)
    {
        decimal total = 0;

        try
        {
            var rs_1A = from p in d.ProductDesign_Stone1A where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_1A) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalStone1B(ProductDesign d)
    {
        decimal total = 0;

        try
        {
            var rs_1B = from p in d.ProductDesign_Stone1B where p.HargaTotal.HasValue && p.StatusTerpasang == 1 select p.HargaTotal.Value;
            foreach (var r in rs_1B) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalStone2(ProductDesign d)
    {
        decimal total = 0;

        try
        {
            var rs_2 = from p in d.ProductDesign_Stone2 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_2) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalStone3(ProductDesign d)
    {
        decimal total = 0;

        try
        {
            var rs_3 = from p in d.ProductDesign_Stone3 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_3) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalStone4(ProductDesign d)
    {
        decimal total = 0;

        try
        {
            var rs_4 = from p in d.ProductDesign_Stone4 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_4) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalStone5(ProductDesign d)
    {

        decimal total = 0;
        try
        {
            var rs_5 = from p in d.ProductDesign_Stone5 where p.HargaTotal.HasValue select p.HargaTotal.Value;
            foreach (var r in rs_5) total += r;
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalFinishing(ProductDesign d)
    {
        decimal total = 0;

        var rs = from p in d.ProductDesign_Finishing where p.Biaya.HasValue select p.Biaya.Value;
        foreach (var r in rs) total += r;

        return total;
    }
    protected decimal TotalModal(int ID)
    {
        decimal total = 0;

        var product = db.ProductDesign_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.ProductDesign_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += product.Total;
        total += biaya.Total;

        return total;
    }
    protected decimal TotalToMarkUp(int ID)
    {
        decimal total = 0;

        var product = db.ProductDesign_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.ProductDesign_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        var stone3 = from p in db.ProductDesign_Stone3
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
        //total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(Stone3ToMarkUp) + Convert.ToDecimal(product.TotalStone4) + Convert.ToDecimal(product.TotalStone5);
        total += Convert.ToDecimal(product.TotalProduct) + Convert.ToDecimal(product.TotalStone1A) + Convert.ToDecimal(Stone3ToMarkUp);
        total += biaya.Total;

        return total;
    }
    protected decimal TotalStone3NMU(int ID)
    {
        decimal total = 0;

        var stone3 = from p in db.ProductDesign_Stone3
                     where p.IDForm == ID && p.GIA != ""
                     select p;
        try
        {
            total = stone3.Sum(p => p.HargaTotal.Value);
        }
        catch (Exception ex) { }

        return total;
    }
    protected decimal TotalStone1B(int ID)
    {
        decimal total = 0;

        var product = db.ProductDesign_PricingProduct.SingleOrDefault(p => p.ID == ID);
        var biaya = db.ProductDesign_PricingBiaya.SingleOrDefault(p => p.ID == ID);

        total += Convert.ToDecimal(product.TotalStone1B);
        //total += biaya.Total;

        return total;
    }
    protected decimal HargaUSD(int ID)
    {
        var mu = db.ProductDesign_PricingMU.SingleOrDefault(p => p.ID == ID);
        return mu.HargaUSD;
    }
}