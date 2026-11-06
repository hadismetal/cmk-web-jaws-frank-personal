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
        var d = db.StockProductGJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml = d.Nomor;

            //Trans. Data
            protonomor.InnerHtml = d.ProtoNomor;
            productordernomor.InnerHtml = d.ProductOrderNomor;
            suppliernama.InnerHtml = d.SupplierNama;
            suppliernomor.InnerHtml = d.SupplierNomor;
            requestoutletnama.InnerHtml = d.RequestOutletNama;
            requestcustomernama.InnerHtml = d.RequestCustomerNama;
            humanstocknama.InnerHtml = d.HumanStockNama;
            expiredproduct.InnerHtml = Str.Tgl(d.ExpiredProduct);
            expiredrequest.InnerHtml = Str.Tgl(d.ExpiredRequest);

            ////Product Char.
            var product = d.StockProductGJ_CharProduct;
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
            var design = d.StockProductGJ_CharDesign;
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
                finishing.InnerHtml = designFinishing(d);
            }

            //Picture
            Upload.Image(imgpicture, d.ImgPicture);

            fillProduct(d);

            fillMU(d);
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
        var d = db.StockProductGJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {

            StockRetireGJ x = new StockRetireGJ();

            x.Nama = petugaslebur.Text;
            x.Nomor = Generate_Nomor(x);
            x.IDProduct = d.ID;
            x.Keterangan = keterangan.Text;
            x.Alasan = alasan.Text;
            x.Tgl = DateTime.Now;
            x.Operator = App.UserNama;
            x.OperatorTgl = DateTime.Now;
            x.Draft = false;
            x.DraftDate = null;
            x.Status = 0;

            db.StockRetireGJ.InsertOnSubmit(x);
            db.SubmitChanges();


            //Redirect
            App.ReloadClose(this);
        }
    }

    protected string Generate_Nomor(StockRetireGJ j)
    {
        string x = string.Empty;

        int Tahun = DateTime.Now.Year;
        int Bulan = DateTime.Now.Month;

        //int Nomor = db.SalesOrder.Count(p => p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan );
        //Nomor = Nomor + 1;

        int Nomor = 1;

        try
        {
            var s = (from p in db.StockRetireGJ
                     where p.OperatorTgl.Value.Year == Tahun && p.OperatorTgl.Value.Month == Bulan
                     orderby (p.Nomor.Substring(11, 4)) descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(11, 4)) + 1;
        }
        catch (Exception ex) { }

        return "SR/G" + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
    }
}