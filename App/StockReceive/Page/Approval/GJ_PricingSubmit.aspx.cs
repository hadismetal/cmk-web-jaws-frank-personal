using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_GJ_PricingSubmit : System.Web.UI.Page
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
    decimal GT_HargaM = 0;

    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        if (!IsPostBack)
        {
            fillForm();
        }
    }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveGJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/GJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            fillProduct(d);
            
            fillBiaya(d);
            fillFinishing();
            fillMU(d);
            fillSegmen(d);
        }
    }

    protected void fillProduct(StockReceiveGJ ms)
    {
        var d = ms.StockReceiveGJ_PricingProduct;
        var x = ms.StockReceiveGJ_CostingProduct;
        if (d != null && x != null)
        {
            goldrate.InnerHtml = Str.Number2(x.GoldRate) + " / " + Str.Number2(d.GoldRate);
            netto.InnerHtml = Str.Number2(x.Netto) + " / " + Str.Number2(d.Netto);
            kadar.InnerHtml = Str.Number2(x.Kadar) + " / " + Str.Number2(d.Kadar);
            totalproduct.InnerHtml = Str.Number2(x.TotalProduct) + " / " + Str.Number2(d.TotalProduct);

            sum_frame.InnerHtml = Str.Number2(x.TotalProduct)  + " / " + Str.Number2(d.TotalProduct);
            sum_total.InnerHtml = Str.Number2(x.Total) + " / " + Str.Number2(d.Total);

            efektifgold.InnerHtml = Str.Tgl(d.EfektifGold);
        }
    }
    protected void fillBiaya(StockReceiveGJ ms)
    {
        var d = ms.StockReceiveGJ_PricingBiaya;
        var x = ms.StockReceiveGJ_Costing;
        if (d != null)
        {
            brand1.InnerHtml = d.NamaBrand;
            item1.InnerHtml = d.NamaItem;
            cons1.InnerHtml = d.NamaCons;

            no1.InnerHtml = Str.Number0(x.MountingNO) + " / " + Str.Number0(d.MountingNO);
            ro1.InnerHtml = Str.Number0(x.MountingRO) + " / " + Str.Number0(d.MountingRO);
            total1.InnerHtml = Str.Number0(x.MountingNO + x.MountingRO) + " / " + Str.Number0(d.Mounting);

            efektifmounting.InnerHtml = Str.Tgl(d.EfektifMounting);

            fee10.InnerHtml = Str.Number0(x.MountingNO + x.MountingRO) + " / " + Str.Number0(d.Mounting);
            fee20.InnerHtml = Str.Number0(x.Setting) + " / " + Str.Number0(d.Setting);
            fee30.InnerHtml = Str.Number0(x.Finishing) + " / " + Str.Number0(d.Finishing);
            fee31.InnerHtml = Str.Number0(x.OngkosLainCOGS) + " / " + Str.Number0(x.OngkosLain);

            fee11.InnerHtml = Str.Tgl(d.EfektifMounting);
            fee21.InnerHtml = Str.Tgl(d.EfektifSetting);

            feerupiah.InnerHtml = Str.Number0(x.TotalRupiah) + " / " + Str.Number0(d.TotalRupiah + x.OngkosLain);
            feerate.InnerHtml = Str.Number2(x.TotalRate)  + " / " + Str.Number2(d.TotalRate);
            feetotal.InnerHtml = Str.Number2(x.TotalBiaya) + " / " + Str.Number2(d.Total);

            efektifrate.InnerHtml = Str.Tgl(d.EfektifRate);
        }
    }
    protected void fillMU(StockReceiveGJ ms)
    {
        decimal AllM = 0m;

        var d = ms.StockReceiveGJ_PricingMU;
        var x = ms.StockReceiveGJ_CostingProduct;
        var z = ms.StockReceiveGJ_Costing;

        AllM = (x.Total + z.TotalBiaya);

        if (d != null)
        {
            cat1.InnerHtml = d.NamaCategory;
            lvl1.InnerHtml = d.NamaLevel;
            dist1.InnerHtml = d.NamaDist;

            modal.InnerHtml = Str.Number2(AllM) + " / " + Str.Number2(d.ModalUSD);
            //modalrate.InnerHtml = Str.Number2(z.TotalRate) + " / " + Str.Number2(d.ModalRate);
            modalrate.InnerHtml = Str.Number2(d.ModalRate);
            //modalrupiah.InnerHtml = Str.Number2(AllM * z.TotalRate) + " / " + Str.Number2(d.ModalRupiah);

            murate.InnerHtml = Str.Tgl(d.EfektifRate);

            mu_persen.InnerHtml = Str.Number2(d.MU);
            hargaidr.InnerHtml = Str.Number0(d.HargaRupiahRound);
            hargausd.InnerHtml = Str.Number2(d.HargaUSD);
        }
    }
    protected void fillSegmen(StockReceiveGJ ms)
    {
        var d = ms.StockReceiveGJ_PricingSegmen;
        if (d != null)
        {
            cat2.InnerHtml = d.NamaCategory;
            lvl2.InnerHtml = d.NamaLevel;
            dist2.InnerHtml = d.NamaDist;

            segmensistem.InnerHtml = DL.SegmenSistem(d.SegmenSistem);
            if (d.SegmenSistem == 0)
                minimum.InnerHtml = Str.Number2(d.SegmenMinimum);
            else
                minimum.InnerHtml = Str.Number3(d.SegmenMinimum);
            efektifsegmen.InnerHtml = Str.Tgl(d.EfektifSegmen);

            segmen.InnerHtml = d.CharProductSegmen.Nama;

            carat.InnerHtml = Str.Number3(d.TotalCarat);
            hargasegmen.InnerHtml = Str.Number2(d.HargaUSD);
        }
    }

    protected void fillFinishing()
    {
        //SELECT
        var rs = from p in db.StockReceiveGJ_Finishing
                 where p.IDForm == myID
                 orderby p.CharProcessFinishing.SN
                 select p;

        //RESULT
        int index = 1;
        foreach (var r in rs)
        {
            if (!Response.IsClientConnected) break;

            TableRow tr = new TableRow();
            TableCell td;

            td = new TableCell();
            td.Text = index++.ToString() + ".";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = r.CharProcessFinishing.Nama;
            td.CssClass = "text-bold";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Tgl(r.Efektif);
            td.CssClass = "text-center";
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = Str.Number2(r.BiayaModal) + " / " + Str.Number2(r.Biaya);
            td.CssClass = "text-right";
            tr.Cells.Add(td);

            tbff.Rows.Add(tr);
        }
    }


    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockReceiveGJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
        if (d == null) App.DataNull(this);
        else
        {
            //Status
            if (d.ProtoNomor != null)
            {
                var s = db.KodeKaret_Detail.SingleOrDefault(p => p.KodeKaretDetail == "" + d.ProtoNomor + "");
                if (s != null)
                {
                    var x = db.ProductOrder.SingleOrDefault(p => p.ID == s.IDProductOrder);
                    if (x != null)
                        x.StatusPenerimaan = 1;

                    var kd = db.KodeKaretGJ.SingleOrDefault(p => p.ID == s.IDKodeKaret);
                    if (kd != null)
                    {
                        //kd.IDBrand = mt.LocWarehouse.IDBrand;
                        //kd.IDDesigner = mt.HumanDesigner;
                        kd.IDSegmen = d.StockReceiveGJ_PricingSegmen.ProductSegmen;
                        kd.Keterangan = d.Keterangan;
                        kd.KodeKaretGJ_CharDesign.DesignCategory = d.StockReceiveGJ_CharDesign.DesignCategory;
                        kd.KodeKaretGJ_CharDesign.DesignConcept = d.StockReceiveGJ_CharDesign.DesignConcept;
                        kd.KodeKaretGJ_CharDesign.DesignProcess = d.StockReceiveGJ_CharDesign.DesignProcess;
                        kd.KodeKaretGJ_CharDesign.TargetAge = d.StockReceiveGJ_CharDesign.TargetAge;
                        kd.KodeKaretGJ_CharDesign.TargetGender = d.StockReceiveGJ_CharDesign.TargetGender;
                        kd.KodeKaretGJ_CharDesign.Naming = d.StockReceiveGJ_CharDesign.Naming;
                        kd.KodeKaretGJ_CharDesign.SetItem = d.StockReceiveGJ_CharDesign.SetItem;
                        kd.KodeKaretGJ_CharDesign.Special = d.StockReceiveGJ_CharDesign.Special;
                        kd.KodeKaretGJ_CharDesign.Grafir = d.StockReceiveGJ_CharDesign.Grafir;
                        kd.KodeKaretGJ_CharProduct.ProductItem = d.StockReceiveGJ_CharProduct.ProductItem;
                        kd.KodeKaretGJ_CharProduct.ProductCategory = d.StockReceiveGJ_CharProduct.ProductCategory;
                        kd.KodeKaretGJ_CharProduct.ProductLevel = d.StockReceiveGJ_CharProduct.ProductLevel;
                        kd.KodeKaretGJ_CharProduct.StoneDist = d.StockReceiveGJ_CharProduct.StoneDist;
                        kd.KodeKaretGJ_CharProduct.FrameMaterial = d.StockReceiveGJ_CharProduct.FrameMaterial;
                        kd.KodeKaretGJ_CharProduct.FrameFinishing = d.StockReceiveGJ_CharProduct.FrameFinishing;
                        kd.KodeKaretGJ_CharProduct.FrameColor = d.StockReceiveGJ_CharProduct.FrameColor;
                        kd.KodeKaretGJ_CharProduct.ProcessCons = d.StockReceiveGJ_CharProduct.ProcessCons;
                        kd.KodeKaretGJ_CharProduct.GrossWeight = d.StockReceiveGJ_CharProduct.GrossWeight;
                        kd.KodeKaretGJ_CharProduct.StoneCarat = d.StockReceiveGJ_CharProduct.StoneCarat;
                        kd.KodeKaretGJ_CharProduct.StoneWeight = d.StockReceiveGJ_CharProduct.StoneWeight;
                        kd.KodeKaretGJ_CharProduct.NettoWeight = d.StockReceiveGJ_CharProduct.NettoWeight;
                        kd.KodeKaretGJ_CharProduct.KadarLogam = d.StockReceiveGJ_CharProduct.KadarLogam;
                        kd.KodeKaretGJ_CharProduct.KadarTukaran = d.StockReceiveGJ_CharProduct.KadarTukaran;
                        kd.KodeKaretGJ_CharProduct.DimensiD = d.StockReceiveGJ_CharProduct.DimensiD;
                        kd.KodeKaretGJ_CharProduct.DimensiP = d.StockReceiveGJ_CharProduct.DimensiP;
                        kd.KodeKaretGJ_CharProduct.DimensiL = d.StockReceiveGJ_CharProduct.DimensiL;
                        kd.KodeKaretGJ_CharProduct.DimensiR = d.StockReceiveGJ_CharProduct.DimensiR;

                        db.SubmitChanges();

                        updateFinishing(s.IDKodeKaret, d);
                    }
                }
            }
            

            //User-Info
            d.Nomor = Generate_Nomor(d);
            d.Approval = App.UserNama;
            d.ApprovalTgl = DateTime.Now;

            //Commit
            db.SubmitChanges();

            StockProductGJ prod = new StockProductGJ();
            prod.IDWarehouse = d.IDWarehouse;
            prod.Sumber = d.Sumber;
            prod.Keterangan = d.Keterangan;
            prod.Tgl = d.Tgl;
            prod.Nomor = d.Nomor;
            prod.Draft = d.Draft;
            prod.DraftDate = d.DraftDate;
            prod.Proto = d.Proto;
            prod.ProtoNomor = d.ProtoNomor;
            prod.ProductOrder = d.ProductOrder;
            prod.ProductOrderNomor = d.ProductOrderNomor;
            prod.Supplier = d.Supplier;
            prod.SupplierNama = d.SupplierNama;
            prod.SupplierNomor = d.SupplierNomor;
            prod.RequestOutlet = d.RequestOutlet;
            prod.RequestOutletNama = d.RequestOutletNama;
            prod.RequestCustomer = d.RequestCustomer;
            prod.RequestCustomerNama = d.RequestCustomerNama;
            prod.HumanDesigner = d.HumanDesigner;
            prod.HumanDesginerNama = d.HumanDesignerNama;
            prod.HumanStock = d.HumanStock;
            prod.HumanStockNama = d.HumanStockNama;
            prod.ExpiredProduct = d.ExpiredProduct;
            prod.ExpiredRequest = d.ExpiredRequest;
            prod.Operator = d.Operator;
            prod.OperatorTgl = d.OperatorTgl;
            prod.Approval = d.Approval;
            prod.ApprovalTgl = d.ApprovalTgl;
            prod.CatatanManager = d.CatatanManager;
            prod.ImgPicture = d.ImgPicture;
            prod.ReturnProduct = false;
            prod.StatusPenjualan = 0;
            prod.StatusLebur = false;

            db.StockProductGJ.InsertOnSubmit(prod);
            db.SubmitChanges();

            int ProdID = Db.SingleInteger("SELECT TOP 1 ID FROM StockProductGJ ORDER BY ID DESC",Db.Cnn_DB);

            StockProductGJ_CharDesign dsg = new StockProductGJ_CharDesign();
            dsg.ID = ProdID;
            dsg.DesignCategory = d.StockReceiveGJ_CharDesign.DesignCategory;
            dsg.DesignConcept = d.StockReceiveGJ_CharDesign.DesignConcept;
            dsg.DesignProcess = d.StockReceiveGJ_CharDesign.DesignProcess;
            dsg.TargetAge = d.StockReceiveGJ_CharDesign.TargetAge;
            dsg.TargetGender = d.StockReceiveGJ_CharDesign.TargetGender;
            dsg.Naming = d.StockReceiveGJ_CharDesign.Naming;
            dsg.SetItem = d.StockReceiveGJ_CharDesign.SetItem;
            dsg.Special = d.StockReceiveGJ_CharDesign.Special;
            dsg.Grafir = d.StockReceiveGJ_CharDesign.Grafir;

            db.StockProductGJ_CharDesign.InsertOnSubmit(dsg);
            db.SubmitChanges();

            StockProductGJ_CharProduct pd = new StockProductGJ_CharProduct();
            pd.ID = ProdID;
            pd.ProductItem = d.StockReceiveGJ_CharProduct.ProductItem;
            pd.ProductCategory = d.StockReceiveGJ_CharProduct.ProductCategory;
            pd.ProductLevel = d.StockReceiveGJ_CharProduct.ProductLevel;
            pd.StoneDist = d.StockReceiveGJ_CharProduct.StoneDist;
            pd.FrameColor = d.StockReceiveGJ_CharProduct.FrameColor;
            pd.FrameFinishing = d.StockReceiveGJ_CharProduct.FrameFinishing;
            pd.FrameMaterial = d.StockReceiveGJ_CharProduct.FrameMaterial;
            pd.ProcessCons = d.StockReceiveGJ_CharProduct.ProcessCons;
            pd.GrossWeight = d.StockReceiveGJ_CharProduct.GrossWeight;
            pd.StoneCarat = d.StockReceiveGJ_CharProduct.StoneCarat;
            pd.StoneWeight = d.StockReceiveGJ_CharProduct.StoneWeight;
            pd.NettoWeight = d.StockReceiveGJ_CharProduct.NettoWeight;
            pd.KadarLogam = d.StockReceiveGJ_CharProduct.KadarLogam;
            pd.KadarTukaran = d.StockReceiveGJ_CharProduct.KadarTukaran;
            pd.DimensiD = d.StockReceiveGJ_CharProduct.DimensiD;
            pd.DimensiL = d.StockReceiveGJ_CharProduct.DimensiL;
            pd.DimensiP = d.StockReceiveGJ_CharProduct.DimensiP;
            pd.DimensiR = d.StockReceiveGJ_CharProduct.DimensiR;
            pd.StoneQty = d.StockReceiveGJ_CharProduct.StoneQty;

            db.StockProductGJ_CharProduct.InsertOnSubmit(pd);
            db.SubmitChanges();

            StockProductGJ_Costing cost = new StockProductGJ_Costing();
            cost.ID = ProdID;
            cost.UnitPrice = d.StockReceiveGJ_Costing.UnitPrice;
            cost.Frame = d.StockReceiveGJ_Costing.Frame;
            cost.MountingNO = d.StockReceiveGJ_Costing.MountingNO;
            cost.MountingRO = d.StockReceiveGJ_Costing.MountingRO.Value;
            cost.Pembagi = d.StockReceiveGJ_Costing.Pembagi.Value;
            cost.Setting = d.StockReceiveGJ_Costing.Setting;
            cost.Finishing = d.StockReceiveGJ_Costing.Finishing;
            cost.OngkosLainCOGS = d.StockReceiveGJ_Costing.OngkosLainCOGS;
            cost.OngkosLain = d.StockReceiveGJ_Costing.OngkosLain;
            cost.TotalRate = d.StockReceiveGJ_Costing.TotalRate.Value;
            cost.TotalRupiah = d.StockReceiveGJ_Costing.TotalRupiah.Value;
            cost.TotalBiaya = d.StockReceiveGJ_Costing.TotalBiaya;

            db.StockProductGJ_Costing.InsertOnSubmit(cost);
            db.SubmitChanges();

            StockProductGJ_CostingProduct cp = new StockProductGJ_CostingProduct();
            cp.ID = ProdID;
            cp.GoldRate = d.StockReceiveGJ_CostingProduct.GoldRate;
            cp.Netto = d.StockReceiveGJ_CostingProduct.Netto;
            cp.Kadar = d.StockReceiveGJ_CostingProduct.Kadar;
            cp.TotalProduct = d.StockReceiveGJ_CostingProduct.TotalProduct;
            cp.Total = d.StockReceiveGJ_CostingProduct.Total;
            cp.EfektifGold = d.StockReceiveGJ_CostingProduct.EfektifGold;

            db.StockProductGJ_CostingProduct.InsertOnSubmit(cp);
            db.SubmitChanges();

            var l_fns = from p in db.StockReceiveGJ_Finishing
                        where p.IDForm == d.ID
                        select p;
            if (l_fns != null)
            {
                foreach (var r in l_fns)
                {
                    StockProductGJ_Finishing fns = new StockProductGJ_Finishing();
                    fns.IDForm = ProdID;
                    fns.IDFinishing = r.IDFinishing;
                    fns.Biaya = r.Biaya;
					fns.BiayaModal = r.BiayaModal;
                    fns.Efektif = r.Efektif;
                    db.StockProductGJ_Finishing.InsertOnSubmit(fns);
                    db.SubmitChanges();
                }
            }

            StockProductGJ_PricingBiaya by = new StockProductGJ_PricingBiaya();
            by.ID = ProdID;
            by.NamaBrand = d.StockReceiveGJ_PricingBiaya.NamaBrand;
            by.NamaItem = d.StockReceiveGJ_PricingBiaya.NamaItem;
            by.NamaCons = d.StockReceiveGJ_PricingBiaya.NamaCons;
            by.Mounting = d.StockReceiveGJ_PricingBiaya.Mounting;
            by.MountingNO = d.StockReceiveGJ_PricingBiaya.MountingNO;
            by.MountingRO = d.StockReceiveGJ_PricingBiaya.MountingRO;
            by.Setting = d.StockReceiveGJ_PricingBiaya.Setting;
            by.Finishing = d.StockReceiveGJ_PricingBiaya.Finishing;
            by.TotalRupiah = d.StockReceiveGJ_PricingBiaya.TotalRupiah;
            by.TotalRate = d.StockReceiveGJ_PricingBiaya.TotalRate;
            by.Total = d.StockReceiveGJ_PricingBiaya.Total;
            by.EfektifRate = d.StockReceiveGJ_PricingBiaya.EfektifRate;
            by.EfektifMounting = d.StockReceiveGJ_PricingBiaya.EfektifMounting;
            by.EfektifSetting = d.StockReceiveGJ_PricingBiaya.EfektifSetting;

            db.StockProductGJ_PricingBiaya.InsertOnSubmit(by);
            db.SubmitChanges();

            StockProductGJ_PricingMU mu = new StockProductGJ_PricingMU();
            mu.ID = ProdID;
            mu.NamaCategory = d.StockReceiveGJ_PricingMU.NamaCategory;
            mu.NamaLevel = d.StockReceiveGJ_PricingMU.NamaLevel;
            mu.NamaDist = d.StockReceiveGJ_PricingMU.NamaDist;
            mu.ModalUSD = d.StockReceiveGJ_PricingMU.ModalUSD;
            mu.ModalRate = d.StockReceiveGJ_PricingMU.ModalRate;
            mu.ModalRupiah = d.StockReceiveGJ_PricingMU.ModalRupiah;
            mu.MU = d.StockReceiveGJ_PricingMU.MU;
            mu.HargaUSD = d.StockReceiveGJ_PricingMU.HargaUSD;
            mu.HargaRupiah = d.StockReceiveGJ_PricingMU.HargaRupiah;
            mu.HargaRupiahRound = d.StockReceiveGJ_PricingMU.HargaRupiahRound;
            mu.EfektifRate = d.StockReceiveGJ_PricingMU.EfektifRate;

            db.StockProductGJ_PricingMU.InsertOnSubmit(mu);
            db.SubmitChanges();

            StockProductGJ_PricingProduct prc = new StockProductGJ_PricingProduct();
            prc.ID = ProdID;
            prc.GoldRate = d.StockReceiveGJ_PricingProduct.GoldRate;
            prc.Netto = d.StockReceiveGJ_PricingProduct.Netto;
            prc.Kadar = d.StockReceiveGJ_PricingProduct.Kadar;
            prc.TotalProduct = d.StockReceiveGJ_PricingProduct.TotalProduct;
            prc.Total = d.StockReceiveGJ_PricingProduct.Total;
            prc.EfektifGold = d.StockReceiveGJ_PricingProduct.EfektifGold;

            db.StockProductGJ_PricingProduct.InsertOnSubmit(prc);
            db.SubmitChanges();

            StockProductGJ_PricingSegmen sg = new StockProductGJ_PricingSegmen();
            sg.ID = ProdID;
            sg.ProductSegmen = d.StockReceiveGJ_PricingSegmen.ProductSegmen;
            sg.NamaCategory = d.StockReceiveGJ_PricingSegmen.NamaCategory;
            sg.NamaLevel = d.StockReceiveGJ_PricingSegmen.NamaLevel;
            sg.NamaDist = d.StockReceiveGJ_PricingSegmen.NamaDist;
            sg.SegmenSistem = d.StockReceiveGJ_PricingSegmen.SegmenSistem;
            sg.SegmenMinimum = d.StockReceiveGJ_PricingSegmen.SegmenMinimum;
            sg.TotalCarat = d.StockReceiveGJ_PricingSegmen.TotalCarat;
            sg.HargaUSD = d.StockReceiveGJ_PricingSegmen.HargaUSD;
            sg.EfektifSegmen = d.StockReceiveGJ_PricingSegmen.EfektifSegmen;

            db.StockProductGJ_PricingSegmen.InsertOnSubmit(sg);
            db.SubmitChanges();

            // Ledger 
            StockLedgerGJ l = new StockLedgerGJ();

            // Value (See Library DB - Ledger for Value and Remark)
            decimal Value = 1; // 1 : in , -1 : out
            int Remark = 4; // See Library DB - Ledger
            string Keterangan = d.Nomor; // Commonly filled by document number or id 
            if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

            //Input
            l.IDProduct = ProdID;
            l.TipeLokasi = 0;
            l.IDLokasi = d.IDWarehouse;
            l.NamaLokasi = d.LocWarehouse.Nama;
            l.Value = Value;
            l.Remark = Remark;
            l.Keterangan = Keterangan;

            //User-Info
            l.Operator = App.UserNama;
            l.OperatorTgl = DateTime.Now;

            //Commit
            db.StockLedgerGJ.InsertOnSubmit(l);
            db.SubmitChanges();

            StockActualGJ stock = new StockActualGJ();
            stock.IDProduct = ProdID;
            stock.NamaLokasi = d.LocWarehouse.Nama;
            stock.IDBrand = d.LocWarehouse.CompanyBrand.ID;
            stock.TipeLokasi = 0;
            stock.IDLokasi = d.IDWarehouse;
            stock.Substorage = 1;
            stock.Keterangan = d.Keterangan;
            stock.Tgl = d.Tgl;
            stock.Nomor = d.Nomor;
            stock.InHand = 1;
            stock.Operator = d.Operator;
            stock.OperatorTgl = d.OperatorTgl;
            db.StockActualGJ.InsertOnSubmit(stock);
            db.SubmitChanges();


            //Redirect
            App.ReloadClose(this);
        }
    }

    protected void updateFinishing(int IDKodeKaret, StockReceiveGJ mt)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretGJ_Finishing WHERE IDForm = " + IDKodeKaret);
        db.SubmitChanges();

        var rs = mt.StockReceiveGJ_Finishing;

        foreach (var r in rs)
        {
            KodeKaretGJ_Finishing fns = new KodeKaretGJ_Finishing();
            fns.IDForm = IDKodeKaret;
            fns.IDFinishing = r.IDFinishing;
            fns.Biaya = r.Biaya;
            fns.Efektif = r.Efektif;
            db.KodeKaretGJ_Finishing.InsertOnSubmit(fns);
            db.SubmitChanges();
        }
    }

    protected string Generate_Nomor(StockReceiveGJ d)
    {
        string x = string.Empty;

        int Tahun = d.Tgl.Year;
        int Bulan = d.Tgl.Month;

        /*int Nomor = db.StockReceiveGJ.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
        Nomor = Nomor + 1;

        return Str.KodeFormat(Tahun, Bulan, Nomor, 4);*/
        int Nomor = 1;

        try
        {
            var s = (from p in db.StockReceiveGJ
                     where p.Tgl.Year == Tahun /*&& p.Tgl.Month == Bulan*/
                     && p.Nomor != null
                     orderby (p.Nomor.Substring(1, 8)) descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(3, 6)) + 1;
        }
        catch (Exception ex) { }

        string Kode = "G";
        try
        {
            Kode = App.KodeGJ;
        }
        catch (Exception ex) { }

        string Kode2 = convertCharPLU(Tahun.ToString().Remove(0, 2));

        string PLU = Kode + Kode2 + Str.KodeFormat(Nomor, 6);

        return PLU;

    }

    private string convertCharPLU(string number)
    {

        char alph1 = Convert.ToInt32(number.Substring(0, 1)) == 0 ? '0' : (char)(Convert.ToInt32(number.Substring(0, 1)) + 64);
        char alph2 = Convert.ToInt32(number.Substring(1, 1)) == 0 ? '0' : (char)(Convert.ToInt32(number.Substring(1, 1)) + 64);
        return alph1.ToString() + alph2.ToString();
    }
}