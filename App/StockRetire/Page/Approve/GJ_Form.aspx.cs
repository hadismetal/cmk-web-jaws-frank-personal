using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_GJ_Retirement : System.Web.UI.Page
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
        var d = db.StockRetireGJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            petugaslebur.Text = d.Nama;
            keterangan.Text = d.Keterangan;
            alasan.Text = d.Alasan;
            //Header
            header_nomor.InnerHtml = d.Nomor;

            susutbongkar.Text = Str.Number2(d.SusutBongkar);
            nettobongkar.Text = Str.Number2(d.NettoSetelahBongkar);
            susutlebur.Text = Str.Number2(d.SusutLebur);
            beratakhir.Text = Str.Number2(d.BeratAkhir);

            //Trans. Data
            protonomor.InnerHtml = d.StockProductGJ.ProtoNomor;
            productordernomor.InnerHtml = d.StockProductGJ.ProductOrderNomor;
            suppliernama.InnerHtml = d.StockProductGJ.SupplierNama;
            suppliernomor.InnerHtml = d.StockProductGJ.SupplierNomor;
            requestoutletnama.InnerHtml = d.StockProductGJ.RequestOutletNama;
            requestcustomernama.InnerHtml = d.StockProductGJ.RequestCustomerNama;
            humanstocknama.InnerHtml = d.StockProductGJ.HumanStockNama;
            expiredproduct.InnerHtml = Str.Tgl(d.StockProductGJ.ExpiredProduct);
            expiredrequest.InnerHtml = Str.Tgl(d.StockProductGJ.ExpiredRequest);

            ////Product Char.
            var product = d.StockProductGJ.StockProductGJ_CharProduct;
            if (product == null) save.Disabled = true;
            else
            {
                productitem.InnerHtml = product.CharProductItem.Nama + " (" + product.CharProductItem.NamaKode + ")";
                productcategory.InnerHtml = product.CharProductCategory.Nama + " (" + product.CharProductCategory.NamaKode + ")";
                productlevel.InnerHtml = product.CharProductLevel.Nama + " (" + product.CharProductLevel.NamaKode + ")";
                stonedist.InnerHtml = product.CharStoneDist.Nama + " (" + product.CharStoneDist.NamaKode + ")";
                framematerial.InnerHtml = product.CharFrameMaterial.Nama + " (" + product.CharFrameMaterial.NamaKode + ")";
                framefinishing.InnerHtml = product.CharFrameFinishing.Nama + " (" + product.CharFrameFinishing.NamaKode + ")";
                framecolor.InnerHtml = product.CharFrameColor.Nama + " (" + product.CharFrameColor.NamaKode + ")";
                processcons.InnerHtml = product.CharProcessCons.Nama + " (" + product.CharProcessCons.NamaKode + ")";

                //Weight
                grossweight.InnerHtml = Str.Number3(product.GrossWeight);
                stonecarat.InnerHtml = Str.Number3(product.StoneCarat);
                stoneweight.InnerHtml = Str.Number3(product.StoneWeight);
                nettoweight.InnerHtml = Str.Number3(product.NettoWeight);

                //Dimensi
                dimensid.InnerHtml = Str.Number2(product.DimensiD);
                dimensip.InnerHtml = Str.Number2(product.DimensiP);
                dimensil.InnerHtml = Str.Number2(product.DimensiL);
                dimensir.InnerHtml = Str.Number2(product.DimensiR);
            }

            //Design Char.
            var design = d.StockProductGJ.StockProductGJ_CharDesign;
            if (design == null) save.Disabled = true;
            else
            {
                designcategory.InnerHtml = design.CharDesignCategory.Nama + " (" + design.CharDesignCategory.NamaKode + ")";
                designconcept.InnerHtml = design.CharDesignConcept.Nama + " (" + design.CharDesignConcept.NamaKode + ")";
                designprocess.InnerHtml = design.CharDesignProcess.Nama + " (" + design.CharDesignProcess.NamaKode + ")";
                targetage.InnerHtml = design.CharTargetAge.Nama + " (" + design.CharTargetAge.NamaKode + ")";
                targetgender.InnerHtml = design.CharTargetGender.Nama + " (" + design.CharTargetGender.NamaKode + ")";

                naming.InnerHtml = design.Naming;
                designket.InnerHtml = designKet(design);
                grafir.InnerHtml = design.Grafir;
                finishing.InnerHtml = designFinishing(d.StockProductGJ);
            }

            //Picture
            Upload.Image(imgpicture, d.StockProductGJ.ImgPicture);

            fillProduct(d.StockProductGJ);

            fillMU(d.StockProductGJ);

            if (d.Status != 0)
            {
                reject.Visible = false;
                save.Visible = false;
            }
        }
    }

    protected void fillProduct(StockProductGJ ms)
    {
        var d = ms.StockProductGJ_PricingProduct;
        var x = ms.StockProductGJ_CostingProduct;
        if (d != null && x != null)
        {
            netto.InnerHtml = Str.Number2(x.Netto);
            kadar.InnerHtml = Str.Number2(Math.Ceiling(x.Kadar));
        }
    }
    protected void fillMU(StockProductGJ ms)
    {
        decimal AllM = 0m;

        var d = ms.StockProductGJ_PricingMU;
        if (d != null)
        {

            hargaidr.InnerHtml = Str.Number0(d.HargaRupiahRound);
        }
    }

    //Design Info
    protected string designKet(StockProductGJ_CharDesign d)
    {
        string x = string.Empty;

        if (d.SetItem)
        {
            if (x != "") x += ", ";
            x += "Set Item";
        }
        if (d.Special)
        {
            if (x != "") x += ", ";
            x += "Special Design";
        }

        return x;
    }
    protected string designFinishing(StockProductGJ d)
    {
        string x = string.Empty;

        var rs = from p in d.StockProductGJ_Finishing select p;
        foreach (var r in rs)
        {
            if (x != "") x += "<br />";
            x += r.CharProcessFinishing.Nama + " (" + r.CharProcessFinishing.NamaKode + ")";
        }

        return x;
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockRetireGJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {

            d.Approval = App.UserNama;
            d.ApprovalTgl = DateTime.Now;
            d.Status = 1;
            d.StockProductGJ.StatusLebur = true;
            db.SubmitChanges();

            StockLedgerGJ l = new StockLedgerGJ();

            // Value (See Library DB - Ledger for Value and Remark)
            decimal Value = -1; // 1 : in , -1 : out
            int Remark = 20; // See Library DB - Ledger
            string Keterangan = d.Nomor; // Commonly filled by document number or id 
            if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

            string Lokasi = "";
            if (d.StockProductGJ.StockActualGJ.TipeLokasi == 1)
                Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockProductGJ.StockActualGJ.IDLokasi, Db.Cnn_DB);
            else if (d.StockProductGJ.StockActualGJ.TipeLokasi == 2)
                Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockProductGJ.StockActualGJ.IDLokasi, Db.Cnn_DB);

            //Input
            l.IDProduct = d.IDProduct;
            l.TipeLokasi = d.StockProductGJ.StockActualGJ.TipeLokasi;
            l.IDLokasi = d.StockProductGJ.StockActualGJ.IDLokasi;
            l.NamaLokasi = Lokasi;
            l.Value = Value;
            l.Remark = Remark;
            l.Keterangan = Keterangan;

            //User-Info
            l.Operator = App.UserNama;
            l.OperatorTgl = DateTime.Now;

            //Commit
            db.StockLedgerGJ.InsertOnSubmit(l);
            db.SubmitChanges();

            //Redirect
            App.ReloadClose(this);
        }
    }

    protected void reject_Click(object sender, EventArgs e)
    {
        var d = db.StockRetireGJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {

            d.Approval = App.UserNama;
            d.ApprovalTgl = DateTime.Now;
            d.Status = 2;

            db.SubmitChanges();


            //Redirect
            App.ReloadClose(this);
        }
    }
}