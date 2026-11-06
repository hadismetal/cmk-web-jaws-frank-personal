using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_Mounting_PricingSubmit : System.Web.UI.Page
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
        var d = db.StockReceiveMounting.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/Mounting_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            fillProduct(d);
            
            fillBiaya(d);
            fillFinishing();
            fillMU(d);
            fillSegmen(d);
        }
    }

    protected void fillProduct(StockReceiveMounting ms)
    {
        var d = ms.StockReceiveMounting_PricingProduct;
        var x = ms.StockReceiveMounting_CostingProduct;
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
    protected void fillBiaya(StockReceiveMounting ms)
    {
        var d = ms.StockReceiveMounting_PricingBiaya;
        var x = ms.StockReceiveMounting_Costing;
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
    protected void fillMU(StockReceiveMounting ms)
    {
        decimal AllM = 0m;

        var d = ms.StockReceiveMounting_PricingMU;
        var x = ms.StockReceiveMounting_CostingProduct;
        var z = ms.StockReceiveMounting_Costing;

        AllM = (x.Total + z.TotalBiaya);

        if (d != null)
        {
            cat1.InnerHtml = d.NamaCategory;
            lvl1.InnerHtml = d.NamaLevel;
            dist1.InnerHtml = d.NamaDist;

            modal.InnerHtml = Str.Number2(AllM) + " / " + Str.Number2(d.ModalUSD);
            modalrate.InnerHtml = Str.Number2(z.TotalRate) + " / " + Str.Number2(d.ModalRate);
            modalrupiah.InnerHtml = Str.Number2(AllM * z.TotalRate) + " / " + Str.Number2(d.ModalRupiah);

            murate.InnerHtml = Str.Tgl(d.EfektifRate);

            mu_persen.InnerHtml = Str.Number2(d.MU);
            hargaidr.InnerHtml = Str.Number0(d.HargaRupiahRound);
            hargausd.InnerHtml = Str.Number2(d.HargaUSD);
        }
    }
    protected void fillSegmen(StockReceiveMounting ms)
    {
        var d = ms.StockReceiveMounting_PricingSegmen;
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
        var rs = from p in db.StockReceiveMounting_Finishing
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
        var d = db.StockReceiveMounting.SingleOrDefault(p => p.ID == myID && p.Approval==null);
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

                    var kd = db.KodeKaretDJ.SingleOrDefault(p => p.ID == s.IDKodeKaret);
                    if (kd != null)
                    {
                        //kd.IDBrand = mt.LocWarehouse.IDBrand;
                        //kd.IDDesigner = mt.HumanDesigner;
                        kd.IDSegmen = d.StockReceiveMounting_PricingSegmen.ProductSegmen;
                        kd.Keterangan = d.Keterangan;
                        kd.KodeKaretDJ_CharDesign.DesignCategory = d.StockReceiveMounting_CharDesign.DesignCategory;
                        kd.KodeKaretDJ_CharDesign.DesignConcept = d.StockReceiveMounting_CharDesign.DesignConcept;
                        kd.KodeKaretDJ_CharDesign.DesignProcess = d.StockReceiveMounting_CharDesign.DesignProcess;
                        kd.KodeKaretDJ_CharDesign.TargetAge = d.StockReceiveMounting_CharDesign.TargetAge;
                        kd.KodeKaretDJ_CharDesign.TargetGender = d.StockReceiveMounting_CharDesign.TargetGender;
                        kd.KodeKaretDJ_CharDesign.Naming = d.StockReceiveMounting_CharDesign.Naming;
                        kd.KodeKaretDJ_CharDesign.SetItem = d.StockReceiveMounting_CharDesign.SetItem;
                        kd.KodeKaretDJ_CharDesign.Special = d.StockReceiveMounting_CharDesign.Special;
                        kd.KodeKaretDJ_CharDesign.Grafir = d.StockReceiveMounting_CharDesign.Grafir;
                        kd.KodeKaretDJ_CharProduct.ProductItem = d.StockReceiveMounting_CharProduct.ProductItem;
                        kd.KodeKaretDJ_CharProduct.ProductCategory = d.StockReceiveMounting_CharProduct.ProductCategory;
                        kd.KodeKaretDJ_CharProduct.ProductLevel = d.StockReceiveMounting_CharProduct.ProductLevel;
                        kd.KodeKaretDJ_CharProduct.StoneDist = d.StockReceiveMounting_CharProduct.StoneDist;
                        kd.KodeKaretDJ_CharProduct.FrameMaterial = d.StockReceiveMounting_CharProduct.FrameMaterial;
                        kd.KodeKaretDJ_CharProduct.FrameFinishing = d.StockReceiveMounting_CharProduct.FrameFinishing;
                        kd.KodeKaretDJ_CharProduct.FrameColor = d.StockReceiveMounting_CharProduct.FrameColor;
                        kd.KodeKaretDJ_CharProduct.ProcessCons = d.StockReceiveMounting_CharProduct.ProcessCons;
                        kd.KodeKaretDJ_CharProduct.GrossWeight = d.StockReceiveMounting_CharProduct.GrossWeight;
                        kd.KodeKaretDJ_CharProduct.StoneCarat = d.StockReceiveMounting_CharProduct.StoneCarat;
                        kd.KodeKaretDJ_CharProduct.StoneWeight = d.StockReceiveMounting_CharProduct.StoneWeight;
                        kd.KodeKaretDJ_CharProduct.NettoWeight = d.StockReceiveMounting_CharProduct.NettoWeight;
                        kd.KodeKaretDJ_CharProduct.KadarLogam = d.StockReceiveMounting_CharProduct.KadarLogam;
                        kd.KodeKaretDJ_CharProduct.KadarTukaran = d.StockReceiveMounting_CharProduct.KadarTukaran;
                        kd.KodeKaretDJ_CharProduct.DimensiD = d.StockReceiveMounting_CharProduct.DimensiD;
                        kd.KodeKaretDJ_CharProduct.DimensiP = d.StockReceiveMounting_CharProduct.DimensiP;
                        kd.KodeKaretDJ_CharProduct.DimensiL = d.StockReceiveMounting_CharProduct.DimensiL;
                        kd.KodeKaretDJ_CharProduct.DimensiR = d.StockReceiveMounting_CharProduct.DimensiR;

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

            StockProductMounting prod = new StockProductMounting();
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
            prod.StatusReceive = false;

            db.StockProductMounting.InsertOnSubmit(prod);
            db.SubmitChanges();

            int ProdID = Db.SingleInteger("SELECT TOP 1 ID FROM StockProductMounting ORDER BY ID DESC",Db.Cnn_DB);

            StockProductMounting_CharDesign dsg = new StockProductMounting_CharDesign();
            dsg.ID = ProdID;
            dsg.DesignCategory = d.StockReceiveMounting_CharDesign.DesignCategory;
            dsg.DesignConcept = d.StockReceiveMounting_CharDesign.DesignConcept;
            dsg.DesignProcess = d.StockReceiveMounting_CharDesign.DesignProcess;
            dsg.TargetAge = d.StockReceiveMounting_CharDesign.TargetAge;
            dsg.TargetGender = d.StockReceiveMounting_CharDesign.TargetGender;
            dsg.Naming = d.StockReceiveMounting_CharDesign.Naming;
            dsg.SetItem = d.StockReceiveMounting_CharDesign.SetItem;
            dsg.Special = d.StockReceiveMounting_CharDesign.Special;
            dsg.Grafir = d.StockReceiveMounting_CharDesign.Grafir;

            db.StockProductMounting_CharDesign.InsertOnSubmit(dsg);
            db.SubmitChanges();

            StockProductMounting_CharProduct pd = new StockProductMounting_CharProduct();
            pd.ID = ProdID;
            pd.ProductItem = d.StockReceiveMounting_CharProduct.ProductItem;
            pd.ProductCategory = d.StockReceiveMounting_CharProduct.ProductCategory;
            pd.ProductLevel = d.StockReceiveMounting_CharProduct.ProductLevel;
            pd.StoneDist = d.StockReceiveMounting_CharProduct.StoneDist;
            pd.FrameColor = d.StockReceiveMounting_CharProduct.FrameColor;
            pd.FrameFinishing = d.StockReceiveMounting_CharProduct.FrameFinishing;
            pd.FrameMaterial = d.StockReceiveMounting_CharProduct.FrameMaterial;
            pd.ProcessCons = d.StockReceiveMounting_CharProduct.ProcessCons;
            pd.GrossWeight = d.StockReceiveMounting_CharProduct.GrossWeight;
            pd.StoneCarat = d.StockReceiveMounting_CharProduct.StoneCarat;
            pd.StoneWeight = d.StockReceiveMounting_CharProduct.StoneWeight;
            pd.NettoWeight = d.StockReceiveMounting_CharProduct.NettoWeight;
            pd.KadarLogam = d.StockReceiveMounting_CharProduct.KadarLogam;
            pd.KadarTukaran = d.StockReceiveMounting_CharProduct.KadarTukaran;
            pd.DimensiD = d.StockReceiveMounting_CharProduct.DimensiD;
            pd.DimensiL = d.StockReceiveMounting_CharProduct.DimensiL;
            pd.DimensiP = d.StockReceiveMounting_CharProduct.DimensiP;
            pd.DimensiR = d.StockReceiveMounting_CharProduct.DimensiR;
            pd.StoneQty = d.StockReceiveMounting_CharProduct.StoneQty;

            db.StockProductMounting_CharProduct.InsertOnSubmit(pd);
            db.SubmitChanges();

            StockProductMounting_Costing cost = new StockProductMounting_Costing();
            cost.ID = ProdID;
            cost.UnitPrice = d.StockReceiveMounting_Costing.UnitPrice;
            cost.Frame = d.StockReceiveMounting_Costing.Frame;
            cost.MountingNO = d.StockReceiveMounting_Costing.MountingNO;
            cost.MountingRO = d.StockReceiveMounting_Costing.MountingRO.Value;
            cost.Pembagi = d.StockReceiveMounting_Costing.Pembagi.Value;
            cost.Setting = d.StockReceiveMounting_Costing.Setting;
            cost.Finishing = d.StockReceiveMounting_Costing.Finishing;
            cost.OngkosLainCOGS = d.StockReceiveMounting_Costing.OngkosLainCOGS;
            cost.OngkosLain = d.StockReceiveMounting_Costing.OngkosLain;
            cost.TotalRate = d.StockReceiveMounting_Costing.TotalRate.Value;
            cost.TotalRupiah = d.StockReceiveMounting_Costing.TotalRupiah.Value;
            cost.TotalBiaya = d.StockReceiveMounting_Costing.TotalBiaya;

            db.StockProductMounting_Costing.InsertOnSubmit(cost);
            db.SubmitChanges();

            StockProductMounting_CostingProduct cp = new StockProductMounting_CostingProduct();
            cp.ID = ProdID;
            cp.GoldRate = d.StockReceiveMounting_CostingProduct.GoldRate;
            cp.Netto = d.StockReceiveMounting_CostingProduct.Netto;
            cp.Kadar = d.StockReceiveMounting_CostingProduct.Kadar;
            cp.TotalProduct = d.StockReceiveMounting_CostingProduct.TotalProduct;
            cp.Total = d.StockReceiveMounting_CostingProduct.Total;
            cp.EfektifGold = d.StockReceiveMounting_CostingProduct.EfektifGold;

            db.StockProductMounting_CostingProduct.InsertOnSubmit(cp);
            db.SubmitChanges();

            var l_fns = from p in db.StockReceiveMounting_Finishing
                        where p.IDForm == d.ID
                        select p;
            if (l_fns != null)
            {
                foreach (var r in l_fns)
                {
                    StockProductMounting_Finishing fns = new StockProductMounting_Finishing();
                    fns.IDForm = ProdID;
                    fns.IDFinishing = r.IDFinishing;
                    fns.Biaya = r.Biaya;
					fns.BiayaModal = r.BiayaModal;
                    fns.Efektif = r.Efektif;
                    db.StockProductMounting_Finishing.InsertOnSubmit(fns);
                    db.SubmitChanges();
                }
            }

            StockProductMounting_PricingBiaya by = new StockProductMounting_PricingBiaya();
            by.ID = ProdID;
            by.NamaBrand = d.StockReceiveMounting_PricingBiaya.NamaBrand;
            by.NamaItem = d.StockReceiveMounting_PricingBiaya.NamaItem;
            by.NamaCons = d.StockReceiveMounting_PricingBiaya.NamaCons;
            by.Mounting = d.StockReceiveMounting_PricingBiaya.Mounting;
            by.MountingNO = d.StockReceiveMounting_PricingBiaya.MountingNO;
            by.MountingRO = d.StockReceiveMounting_PricingBiaya.MountingRO;
            by.Setting = d.StockReceiveMounting_PricingBiaya.Setting;
            by.Finishing = d.StockReceiveMounting_PricingBiaya.Finishing;
            by.TotalRupiah = d.StockReceiveMounting_PricingBiaya.TotalRupiah;
            by.TotalRate = d.StockReceiveMounting_PricingBiaya.TotalRate;
            by.Total = d.StockReceiveMounting_PricingBiaya.Total;
            by.EfektifRate = d.StockReceiveMounting_PricingBiaya.EfektifRate;
            by.EfektifMounting = d.StockReceiveMounting_PricingBiaya.EfektifMounting;
            by.EfektifSetting = d.StockReceiveMounting_PricingBiaya.EfektifSetting;

            db.StockProductMounting_PricingBiaya.InsertOnSubmit(by);
            db.SubmitChanges();

            StockProductMounting_PricingMU mu = new StockProductMounting_PricingMU();
            mu.ID = ProdID;
            mu.NamaCategory = d.StockReceiveMounting_PricingMU.NamaCategory;
            mu.NamaLevel = d.StockReceiveMounting_PricingMU.NamaLevel;
            mu.NamaDist = d.StockReceiveMounting_PricingMU.NamaDist;
            mu.ModalUSD = d.StockReceiveMounting_PricingMU.ModalUSD;
            mu.ModalRate = d.StockReceiveMounting_PricingMU.ModalRate;
            mu.ModalRupiah = d.StockReceiveMounting_PricingMU.ModalRupiah;
            mu.MU = d.StockReceiveMounting_PricingMU.MU;
            mu.HargaUSD = d.StockReceiveMounting_PricingMU.HargaUSD;
            mu.HargaRupiah = d.StockReceiveMounting_PricingMU.HargaRupiah;
            mu.HargaRupiahRound = d.StockReceiveMounting_PricingMU.HargaRupiahRound;
            mu.EfektifRate = d.StockReceiveMounting_PricingMU.EfektifRate;

            db.StockProductMounting_PricingMU.InsertOnSubmit(mu);
            db.SubmitChanges();

            StockProductMounting_PricingProduct prc = new StockProductMounting_PricingProduct();
            prc.ID = ProdID;
            prc.GoldRate = d.StockReceiveMounting_PricingProduct.GoldRate;
            prc.Netto = d.StockReceiveMounting_PricingProduct.Netto;
            prc.Kadar = d.StockReceiveMounting_PricingProduct.Kadar;
            prc.TotalProduct = d.StockReceiveMounting_PricingProduct.TotalProduct;
            prc.Total = d.StockReceiveMounting_PricingProduct.Total;
            prc.EfektifGold = d.StockReceiveMounting_PricingProduct.EfektifGold;

            db.StockProductMounting_PricingProduct.InsertOnSubmit(prc);
            db.SubmitChanges();

            StockProductMounting_PricingSegmen sg = new StockProductMounting_PricingSegmen();
            sg.ID = ProdID;
            sg.ProductSegmen = d.StockReceiveMounting_PricingSegmen.ProductSegmen;
            sg.NamaCategory = d.StockReceiveMounting_PricingSegmen.NamaCategory;
            sg.NamaLevel = d.StockReceiveMounting_PricingSegmen.NamaLevel;
            sg.NamaDist = d.StockReceiveMounting_PricingSegmen.NamaDist;
            sg.SegmenSistem = d.StockReceiveMounting_PricingSegmen.SegmenSistem;
            sg.SegmenMinimum = d.StockReceiveMounting_PricingSegmen.SegmenMinimum;
            sg.TotalCarat = d.StockReceiveMounting_PricingSegmen.TotalCarat;
            sg.HargaUSD = d.StockReceiveMounting_PricingSegmen.HargaUSD;
            sg.EfektifSegmen = d.StockReceiveMounting_PricingSegmen.EfektifSegmen;

            db.StockProductMounting_PricingSegmen.InsertOnSubmit(sg);
            db.SubmitChanges();

            StockActualMounting stock = new StockActualMounting();
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
            db.StockActualMounting.InsertOnSubmit(stock);
            db.SubmitChanges();


            //Redirect
            App.ReloadClose(this);
        }
    }

    protected void updateFinishing(int IDKodeKaret, StockReceiveMounting mt)
    {
        db.ExecuteCommand("DELETE FROM KodeKaretDJ_Finishing WHERE IDForm = " + IDKodeKaret);
        db.SubmitChanges();

        var rs = mt.StockReceiveMounting_Finishing;

        foreach (var r in rs)
        {
            KodeKaretDJ_Finishing fns = new KodeKaretDJ_Finishing();
            fns.IDForm = IDKodeKaret;
            fns.IDFinishing = r.IDFinishing;
            fns.Biaya = r.Biaya;
            fns.Efektif = r.Efektif;
            db.KodeKaretDJ_Finishing.InsertOnSubmit(fns);
            db.SubmitChanges();
        }
    }
    protected string Generate_Nomor(StockReceiveMounting d)
    {
        string x = string.Empty;

        int Tahun = d.Tgl.Year;
        int Bulan = d.Tgl.Month;

        /*int Nomor = db.StockReceiveMounting.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
        Nomor = Nomor + 1;

        return Str.KodeFormat(Tahun, Bulan, Nomor, 4);*/
        int Nomor = 1;

        try
        {
            var s = (from p in db.StockReceiveMounting
                     where p.Tgl.Year == Tahun /*&& p.Tgl.Month == Bulan*/
                     && p.Nomor != null
                     orderby (p.Nomor.Substring(3, 6)) descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(3, 6)) + 1;
        }
        catch (Exception ex) { }

        string Kode = "M";
        try
        {
            Kode = App.KodeMounting;
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