using System;
using System.Linq;

public partial class Page_Approval_SPK_Pricing : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }

    //Exception
    protected void PricingError(string sebab)
    {
        Response.Redirect("SPK_Form.aspx?id=" + myID + "&prcerror=" + sebab);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Manager);

        string Bulan = DateTime.Now.ToString("MM");
        string Tahun = DateTime.Now.ToString("yy");

        int LastKodeKaret = 1;

        int LastSPK = 1;
        int Counter = 1;
        int SPKID = 1;
        int LastSPKID = 1;
        string KodeKaret = "";

        var d = db.ProductOrderGJ.SingleOrDefault(p => p.ID == myID && p.Draft == false && p.StatusPenerimaan < 2 && p.Approval == null);
        if (d == null) App.DataNull(this);
        else
        {
            string companycode = d.CompanyBrand.NamaKode;
                    
            //Process
            var product = d.ProductOrderGJ_CharProduct;
            if (product == null) PricingError("Product Char.");
            else
            {

                //edit suwandi
                for (int i = 0; i < d.JlhCopy; i++)
                {
                    try
                    {
                        LastSPK = Db.SingleInteger("SELECT COUNT(*) FROM ProductOrderGJ WHERE SUBSTRING(NoSPK,5,2) = CONVERT(VARCHAR(2), GETDATE(), 2) AND SUBSTRING(NoSPK,8,2) = CONVERT(VARCHAR(2), GETDATE(), 1)", Db.Cnn_DB);
                    }
                    catch (Exception ex)
                    {
                        LastSPK = 0;
                    }

                    string SPKNumber = Str.KodeFormat((LastSPK + 1), 4);
                    string KodeBrand,KodeItem,KodeProductCategory,KodeSet;
                    if(d.IDProductDesign!=null)
                        KodeBrand = d.ProductDesignGJ.DataHuman.CompanyBrand.NamaKode;
                    else
                        KodeBrand = d.CompanyBrand.NamaKode;

                        KodeItem = d.ProductOrderGJ_CharProduct.CharProductItem.NamaKode;
                        KodeProductCategory = d.ProductOrderGJ_CharProduct.CharProductCategory.KodeProto.Substring(0,1);
                        KodeSet = (d.ProductOrderGJ_CharDesign.SetItem == true) ? "S" : "T";

                    int SeqProductCategory = 1;
                    try
                    {
                        SeqProductCategory = Db.SingleInteger("SELECT COUNT(*) FROM KodeKaretGJ_CharProduct WHERE ProductCategory=" + d.ProductOrderGJ_CharProduct.ProductCategory + " AND ProductItem=" + d.ProductOrderGJ_CharProduct.ProductItem, Db.Cnn_DB);
                        SeqProductCategory = SeqProductCategory + 1;
                    }
                    catch (Exception ex)
                    {
                        SeqProductCategory = 1;
                    }

                    if (Counter == 1 && d.IDKodeKaret == null)
                    {
                        KodeKaretGJ kd = new KodeKaretGJ();

                        KodeKaret = KodeBrand + KodeItem + KodeProductCategory + KodeSet + Tahun + Str.KodeFormat(SeqProductCategory, 4);

                        kd.Nomor = KodeKaret;
                        kd.IDProductOrder = d.ID;
                        kd.Tgl = d.Tgl;
                        kd.Keterangan = d.Keterangan;
                        kd.Operator = d.Operator;
                        kd.OperatorTgl = d.OperatorTgl;
                        kd.ImgPicture = d.ImgPicture;
                        kd.IDBrand = d.ProductDesignGJ.DataHuman.IDBrand;
                        kd.IDSegmen = d.ProductDesignGJ.ProductDesignGJ_PricingSegmen.ProductSegmen;
                        kd.IDDesigner = d.ProductDesignGJ.DataHuman.ID;
                        kd.KodeKaretLama = d.KodeKaretLama;

                        db.KodeKaretGJ.InsertOnSubmit(kd);
                        db.SubmitChanges();

                        try
                        {
                            LastKodeKaret = Db.SingleInteger("SELECT TOP 1 ID FROM KodeKaretGJ ORDER BY ID DESC", Db.Cnn_DB);
                        }
                        catch (Exception ex)
                        {
                            LastKodeKaret = 1;
                        }

                        KodeKaretGJ_CharDesign dsg = new KodeKaretGJ_CharDesign();
                        dsg.ID = LastKodeKaret;
                        dsg.DesignCategory = d.ProductOrderGJ_CharDesign.DesignCategory;
                        dsg.DesignConcept = d.ProductOrderGJ_CharDesign.DesignConcept;
                        dsg.DesignProcess = d.ProductOrderGJ_CharDesign.DesignProcess;
                        dsg.TargetAge = d.ProductOrderGJ_CharDesign.TargetAge;
                        dsg.TargetGender = d.ProductOrderGJ_CharDesign.TargetGender;
                        dsg.Naming = d.ProductOrderGJ_CharDesign.Naming;
                        dsg.SetItem = d.ProductOrderGJ_CharDesign.SetItem;
                        dsg.Special = d.ProductOrderGJ_CharDesign.Special;

                        db.KodeKaretGJ_CharDesign.InsertOnSubmit(dsg);
                        db.SubmitChanges();

                        KodeKaretGJ_CharProduct prod = new KodeKaretGJ_CharProduct();
                        prod.ID = LastKodeKaret;
                        prod.FrameColor = d.ProductOrderGJ_CharProduct.FrameColor;
                        prod.FrameFinishing = d.ProductOrderGJ_CharProduct.FrameFinishing;
                        prod.FrameMaterial = d.ProductOrderGJ_CharProduct.FrameMaterial;
                        prod.ProcessCons = d.ProductOrderGJ_CharProduct.ProcessCons;
                        prod.ProductCategory = d.ProductOrderGJ_CharProduct.ProductCategory;
                        prod.ProductItem = d.ProductOrderGJ_CharProduct.ProductItem;
                        prod.ProductLevel = d.ProductOrderGJ_CharProduct.ProductLevel;
                        prod.StoneDist = d.ProductOrderGJ_CharProduct.StoneDist;
                        prod.GrossWeight = d.ProductOrderGJ_CharProduct.GrossWeight;
                        prod.StoneCarat = d.ProductOrderGJ_CharProduct.StoneCarat;
                        prod.StoneWeight = d.ProductOrderGJ_CharProduct.StoneWeight;
                        prod.NettoWeight = d.ProductOrderGJ_CharProduct.NettoWeight;
                        prod.KadarLogam = d.ProductOrderGJ_CharProduct.KadarLogam;
                        prod.KadarTukaran = d.ProductOrderGJ_CharProduct.KadarTukaran;
                        prod.DimensiD = d.ProductOrderGJ_CharProduct.DimensiD;
                        prod.DimensiL = d.ProductOrderGJ_CharProduct.DimensiL;
                        prod.DimensiP = d.ProductOrderGJ_CharProduct.DimensiP;
                        prod.DimensiR = d.ProductOrderGJ_CharProduct.DimensiR;
                        prod.KodeKomponen1 = d.ProductOrderGJ_CharProduct.KodeKomponen1;
                        prod.JlhKomponen1 = d.ProductOrderGJ_CharProduct.JlhKomponen1;
                        prod.KodeKomponen2 = d.ProductOrderGJ_CharProduct.KodeKomponen2;
                        prod.JlhKomponen2 = d.ProductOrderGJ_CharProduct.JlhKomponen2;
                        prod.KodeKomponen3 = d.ProductOrderGJ_CharProduct.KodeKomponen3;
                        prod.JlhKomponen3 = d.ProductOrderGJ_CharProduct.JlhKomponen3;
                        prod.KodeKomponen4 = d.ProductOrderGJ_CharProduct.KodeKomponen4;
                        prod.JlhKomponen4 = d.ProductOrderGJ_CharProduct.JlhKomponen4;
                        prod.KodeKomponen5 = d.ProductOrderGJ_CharProduct.KodeKomponen5;
                        prod.JlhKomponen5 = d.ProductOrderGJ_CharProduct.JlhKomponen5;
                        prod.KodeKomponen6 = d.ProductOrderGJ_CharProduct.KodeKomponen6;
                        prod.JlhKomponen6 = d.ProductOrderGJ_CharProduct.JlhKomponen6;
                        prod.Komponen = d.ProductOrderGJ_CharProduct.Komponen;
                        prod.PoinKomponen = d.ProductOrderGJ_CharProduct.PoinKomponen;
                        prod.BebanSusut = d.ProductOrderGJ_CharProduct.BebanSusut;

                        db.KodeKaretGJ_CharProduct.InsertOnSubmit(prod);
                        db.SubmitChanges();



                        var finishing = from p in db.ProductOrderGJ_Finishing
                                        where p.IDForm == d.ID
                                        select p;
                        if (finishing != null)
                        {

                            foreach (var r in finishing)
                            {
                                KodeKaretGJ_Finishing fns = new KodeKaretGJ_Finishing();
                                fns.IDForm = LastKodeKaret;
                                fns.IDFinishing = r.IDFinishing;
                                fns.Biaya = r.Biaya;
                                fns.Efektif = r.Efektif;
                                db.KodeKaretGJ_Finishing.InsertOnSubmit(fns);
                                db.SubmitChanges();
                            }

                        }
                    }
                    else
                    {
                        LastKodeKaret = Convert.ToInt32(d.IDKodeKaret);

                        var dsg = db.KodeKaretGJ_CharDesign.SingleOrDefault(p => p.ID == LastKodeKaret);
 
                        dsg.DesignCategory = d.ProductOrderGJ_CharDesign.DesignCategory;
                        dsg.DesignConcept = d.ProductOrderGJ_CharDesign.DesignConcept;
                        dsg.DesignProcess = d.ProductOrderGJ_CharDesign.DesignProcess;
                        dsg.TargetAge = d.ProductOrderGJ_CharDesign.TargetAge;
                        dsg.TargetGender = d.ProductOrderGJ_CharDesign.TargetGender;
                        dsg.Naming = d.ProductOrderGJ_CharDesign.Naming;
                        dsg.SetItem = d.ProductOrderGJ_CharDesign.SetItem;
                        dsg.Special = d.ProductOrderGJ_CharDesign.Special;

                        db.SubmitChanges();

                        var prod = db.KodeKaretGJ_CharProduct.SingleOrDefault(p => p.ID == LastKodeKaret);
                        prod.FrameColor = d.ProductOrderGJ_CharProduct.FrameColor;
                        prod.FrameFinishing = d.ProductOrderGJ_CharProduct.FrameFinishing;
                        prod.FrameMaterial = d.ProductOrderGJ_CharProduct.FrameMaterial;
                        prod.ProcessCons = d.ProductOrderGJ_CharProduct.ProcessCons;
                        prod.ProductCategory = d.ProductOrderGJ_CharProduct.ProductCategory;
                        prod.ProductItem = d.ProductOrderGJ_CharProduct.ProductItem;
                        prod.ProductLevel = d.ProductOrderGJ_CharProduct.ProductLevel;
                        prod.StoneDist = d.ProductOrderGJ_CharProduct.StoneDist;
                        prod.GrossWeight = d.ProductOrderGJ_CharProduct.GrossWeight;
                        prod.StoneCarat = d.ProductOrderGJ_CharProduct.StoneCarat;
                        prod.StoneWeight = d.ProductOrderGJ_CharProduct.StoneWeight;
                        prod.NettoWeight = d.ProductOrderGJ_CharProduct.NettoWeight;
                        prod.KadarLogam = d.ProductOrderGJ_CharProduct.KadarLogam;
                        prod.KadarTukaran = d.ProductOrderGJ_CharProduct.KadarTukaran;
                        prod.DimensiD = d.ProductOrderGJ_CharProduct.DimensiD;
                        prod.DimensiL = d.ProductOrderGJ_CharProduct.DimensiL;
                        prod.DimensiP = d.ProductOrderGJ_CharProduct.DimensiP;
                        prod.DimensiR = d.ProductOrderGJ_CharProduct.DimensiR;
                        prod.KodeKomponen1 = d.ProductOrderGJ_CharProduct.KodeKomponen1;
                        prod.JlhKomponen1 = d.ProductOrderGJ_CharProduct.JlhKomponen1;
                        prod.KodeKomponen2 = d.ProductOrderGJ_CharProduct.KodeKomponen2;
                        prod.JlhKomponen2 = d.ProductOrderGJ_CharProduct.JlhKomponen2;
                        prod.KodeKomponen3 = d.ProductOrderGJ_CharProduct.KodeKomponen3;
                        prod.JlhKomponen3 = d.ProductOrderGJ_CharProduct.JlhKomponen3;
                        prod.KodeKomponen4 = d.ProductOrderGJ_CharProduct.KodeKomponen4;
                        prod.JlhKomponen4 = d.ProductOrderGJ_CharProduct.JlhKomponen4;
                        prod.KodeKomponen5 = d.ProductOrderGJ_CharProduct.KodeKomponen5;
                        prod.JlhKomponen5 = d.ProductOrderGJ_CharProduct.JlhKomponen5;
                        prod.KodeKomponen6 = d.ProductOrderGJ_CharProduct.KodeKomponen6;
                        prod.JlhKomponen6 = d.ProductOrderGJ_CharProduct.JlhKomponen6;
                        prod.Komponen = d.ProductOrderGJ_CharProduct.Komponen;
                        prod.PoinKomponen = d.ProductOrderGJ_CharProduct.PoinKomponen;
                        prod.BebanSusut = d.ProductOrderGJ_CharProduct.BebanSusut;

                        db.SubmitChanges();

                        var z = db.KodeKaretGJ_Finishing.Where(p => p.IDForm == LastKodeKaret);
                        if (z != null)
                        {
                            db.KodeKaretGJ_Finishing.DeleteAllOnSubmit(z);
                            db.SubmitChanges();
                        }

                        var finishing = from p in db.ProductOrderGJ_Finishing
                                        where p.IDForm == d.ID
                                        select p;
                        if (finishing != null)
                        {
                            
                            foreach (var r in finishing)
                            {
                                KodeKaretGJ_Finishing fns = new KodeKaretGJ_Finishing();
                                fns.IDForm = LastKodeKaret;
                                fns.IDFinishing = r.IDFinishing;
                                fns.Biaya = r.Biaya;
                                fns.Efektif = r.Efektif;
                                db.KodeKaretGJ_Finishing.InsertOnSubmit(fns);
                                db.SubmitChanges();
                            }

                        }
                    }

                    if (Counter == 1)
                    {
                        d.NoSPK = "SPK/" + companycode + "/" + Tahun + "/" + Bulan + "/" + SPKNumber;
                        d.IDKodeKaret = (d.IDKodeKaret != null) ? d.IDKodeKaret : LastKodeKaret;
                        d.StatusFPS = 0;
                        d.StatusPenerimaan = 0;
                        d.Approval = App.UserNama;
                        d.ApprovalTgl = DateTime.Today;
                        d.IDDesigner = d.IDDesigner;
                        d.IDSegmen = d.IDSegmen;
                        db.SubmitChanges();

                        LastSPKID = d.ID;
                    }
                    else
                    {
                        ProductOrderGJ kd = new ProductOrderGJ();

                        /*if (d.IDKodeKaret == null)
                            KodeKaret = KodeBrand + KodeItem + KodeProductCategory + KodeSet + Tahun + Str.KodeFormat(SeqProductCategory, 4);
                        else
                        {
                            var karet = db.KodeKaretGJ.SingleOrDefault(p => p.ID == d.IDKodeKaret);
                            KodeKaret = karet.Nomor;
                        }*/

                        kd.IDBrand = d.IDBrand;
                        kd.KodeKaret = KodeKaret;
                        kd.IDKodeKaret = LastKodeKaret;
                        kd.NoSPK = "SPK/" + companycode + "/" + Tahun + "/" + Bulan + "/" + SPKNumber;
                        kd.Tgl = d.Tgl;
                        kd.Keterangan = d.Keterangan;
                        kd.Operator = d.Operator;
                        kd.OperatorTgl = d.OperatorTgl;
                        kd.ImgPicture = d.ImgPicture;
                        kd.StatusFPS = 0;
                        kd.StatusPenerimaan = 0;
                        kd.Draft = false;
                        kd.DraftDate = d.Tgl;
                        kd.RequestCustomer = d.RequestCustomer;
                        kd.HumanDesigner = d.HumanDesigner;
                        kd.HumanDesignerNama = d.HumanDesignerNama;
                        kd.TanggalJatuhTempoDJ = d.TanggalJatuhTempoDJ;
                        kd.TanggalJatuhTempoKonsumen = d.TanggalJatuhTempoKonsumen;
                        kd.TanggalJatuhTempoRangka = d.TanggalJatuhTempoRangka;
                        kd.TanggalJatuhTempoSetting = d.TanggalJatuhTempoSetting;
                        kd.Operator = d.Operator;
                        kd.OperatorTgl = d.OperatorTgl;
                        kd.Approval = d.Approval;
                        kd.ApprovalTgl = d.ApprovalTgl;
                        kd.JlhCopy = 1;
                        kd.IDDesigner = d.IDDesigner;
                        kd.IDSegmen = d.IDSegmen;

                        db.ProductOrderGJ.InsertOnSubmit(kd);
                        db.SubmitChanges();

                        try
                        {
                            LastSPKID = Db.SingleInteger("SELECT TOP 1 ID FROM ProductOrderGJ ORDER BY ID DESC", Db.Cnn_DB);
                        }
                        catch (Exception ex)
                        {
                            LastSPKID = 1;
                        }

                        ProductOrderGJ_CharDesign dsg = new ProductOrderGJ_CharDesign();
                        dsg.ID = LastSPKID;
                        dsg.DesignCategory = d.ProductOrderGJ_CharDesign.DesignCategory;
                        dsg.DesignConcept = d.ProductOrderGJ_CharDesign.DesignConcept;
                        dsg.DesignProcess = d.ProductOrderGJ_CharDesign.DesignProcess;
                        dsg.TargetAge = d.ProductOrderGJ_CharDesign.TargetAge;
                        dsg.TargetGender = d.ProductOrderGJ_CharDesign.TargetGender;
                        dsg.Naming = d.ProductOrderGJ_CharDesign.Naming;
                        dsg.SetItem = d.ProductOrderGJ_CharDesign.SetItem;
                        dsg.Special = d.ProductOrderGJ_CharDesign.Special;

                        db.ProductOrderGJ_CharDesign.InsertOnSubmit(dsg);
                        db.SubmitChanges();

                        ProductOrderGJ_CharProduct prod = new ProductOrderGJ_CharProduct();
                        prod.ID = LastSPKID;
                        prod.FrameColor = d.ProductOrderGJ_CharProduct.FrameColor;
                        prod.FrameFinishing = d.ProductOrderGJ_CharProduct.FrameFinishing;
                        prod.FrameMaterial = d.ProductOrderGJ_CharProduct.FrameMaterial;
                        prod.ProcessCons = d.ProductOrderGJ_CharProduct.ProcessCons;
                        prod.ProductCategory = d.ProductOrderGJ_CharProduct.ProductCategory;
                        prod.ProductItem = d.ProductOrderGJ_CharProduct.ProductItem;
                        prod.ProductLevel = d.ProductOrderGJ_CharProduct.ProductLevel;
                        prod.StoneDist = d.ProductOrderGJ_CharProduct.StoneDist;
                        prod.GrossWeight = d.ProductOrderGJ_CharProduct.GrossWeight;
                        prod.StoneCarat = d.ProductOrderGJ_CharProduct.StoneCarat;
                        prod.StoneWeight = d.ProductOrderGJ_CharProduct.StoneWeight;
                        prod.NettoWeight = d.ProductOrderGJ_CharProduct.NettoWeight;
                        prod.KadarLogam = d.ProductOrderGJ_CharProduct.KadarLogam;
                        prod.KadarTukaran = d.ProductOrderGJ_CharProduct.KadarTukaran;
                        prod.DimensiD = d.ProductOrderGJ_CharProduct.DimensiD;
                        prod.DimensiL = d.ProductOrderGJ_CharProduct.DimensiL;
                        prod.DimensiP = d.ProductOrderGJ_CharProduct.DimensiP;
                        prod.DimensiR = d.ProductOrderGJ_CharProduct.DimensiR;

                        db.ProductOrderGJ_CharProduct.InsertOnSubmit(prod);
                        db.SubmitChanges();

                        

                        var finishing = from p in db.ProductOrderGJ_Finishing
                                        where p.IDForm == d.ID
                                        select p;
                        if (finishing != null)
                        {
                            foreach (var r in finishing)
                            {
                                ProductOrderGJ_Finishing fns = new ProductOrderGJ_Finishing();

                                fns.IDForm = LastSPKID;
                                fns.IDFinishing = r.IDFinishing;
                                fns.Biaya = r.Biaya;
                                fns.Efektif = r.Efektif;
                                db.ProductOrderGJ_Finishing.InsertOnSubmit(fns);
                                db.SubmitChanges();
                            }
                            
                        }

                    }

                    string KodeKaretDetail = "";
                    int SeqKodeKaretDetail = 1;

                    try
                    {
                        SeqKodeKaretDetail = Db.SingleInteger("SELECT MAX(Increment) FROM KodeKaretGJ_Detail WHERE IDKodeKaret=" + LastKodeKaret, Db.Cnn_DB);
                        SeqKodeKaretDetail = SeqKodeKaretDetail + 1;
                    }
                    catch (Exception ex)
                    {
                        SeqKodeKaretDetail = 1;
                    }

                    if (d.IDKodeKaret == null)
                        KodeKaret = KodeBrand + KodeItem + KodeProductCategory + KodeSet + Tahun + Str.KodeFormat(SeqProductCategory, 4);
                    else
                    {
                        var karet = db.KodeKaretGJ.SingleOrDefault(p => p.ID == d.IDKodeKaret);
                        KodeKaret = karet.Nomor;
                    }

                    KodeKaretDetail = KodeKaret + "-" + Str.KodeFormat(SeqKodeKaretDetail, 4);

                    KodeKaretGJ_Detail det = new KodeKaretGJ_Detail();
                    det.IDKodeKaret = LastKodeKaret;
                    det.IDProductOrder = LastSPKID;
                    det.Increment = SeqKodeKaretDetail;
                    det.KodeKaretDetail = KodeKaretDetail;
                    det.NoSPK = (Counter == 1) ? d.NoSPK : "SPK/" + companycode + "/" + Bulan + "/" + Tahun + "/" + SPKNumber;
                    det.StatusOrder = (Counter == 1) ? 1 : 2;

                    db.KodeKaretGJ_Detail.InsertOnSubmit(det);
                    db.SubmitChanges();

                    Counter++;
                }

                //end edit
            }

            //Redirect
            //Response.Redirect("SPK.aspx?id=" + myID);
            App.ReloadClose(this);
        }
    }

    
    //Finishing
    protected void saveFinishing(ProductOrderGJ d, ProductOrderGJ_CharProduct product)
    {
        var rs = from p in d.ProductOrderGJ_Finishing select p;
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

            //Pricing - Validasi
            if (prc_ff == null) { ok = false; sebab = "Finishing Fee"; }

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

    protected int FS_ID(ProductOrderGJ d, ProductOrderGJ_CharProduct product)
    {
        return 1; //Masih di-hardcoded. BELUM SELESAI !
    }

    //Sub-Total
    protected decimal TotalStone1A(ProductOrderGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone1B(ProductOrderGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone2(ProductOrderGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone3(ProductOrderGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone4(ProductOrderGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalStone5(ProductOrderGJ d)
    {
        decimal total = 0;

        return total;
    }
    protected decimal TotalFinishing(ProductOrderGJ d)
    {
        decimal total = 0;

        var rs = from p in d.ProductOrderGJ_Finishing where p.Biaya.HasValue select p.Biaya.Value;
        foreach (var r in rs) total += r;

        return total;
    }
}