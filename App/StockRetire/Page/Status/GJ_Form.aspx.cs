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
            App.NumberFormat(susutbongkar);
            App.NumberFormat(susutlebur);
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

            d.NettoWeight = d.StockProductGJ.StockProductGJ_CharProduct.NettoWeight;
            d.SusutBongkar = Convert.ToDecimal(susutbongkar.Text);
            d.NettoSetelahBongkar = Convert.ToDecimal(nettobongkar.Text);
            d.SusutLebur = Convert.ToDecimal(susutlebur.Text);
            d.BeratAkhir = Convert.ToDecimal(beratakhir.Text);
            d.Nama = petugaslebur.Text;
            d.Alasan = alasan.Text;
            d.Keterangan = keterangan.Text;
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
    protected void CalcSusutBongkar(object sender, EventArgs e)
    {
        ((TextBox)sender).Focus();
        CalcAll();
    }

    protected void CalcSusutLebur(object sender, EventArgs e)
    {
        ((TextBox)sender).Focus();
        CalcAll();
    }
    protected void CalcAll()
    {
        decimal NettoWeight = 0m;
        decimal SusutBongkar = 0m;
        decimal NettoBongkar = 0m;
        decimal SusutLebur = 0m;
        decimal BeratAkhir = 0m;

        try
        {
            NettoWeight = Convert.ToDecimal(netto.InnerText);
        }
        catch (Exception ex) { }
        try
        {
            SusutBongkar = Convert.ToDecimal(susutbongkar.Text);
        }
        catch (Exception ex) { }
        try
        {
            SusutLebur = Convert.ToDecimal(susutlebur.Text);
        }
        catch (Exception ex) { }

        NettoBongkar = NettoWeight - SusutBongkar;
        BeratAkhir = NettoBongkar - SusutLebur;

        nettobongkar.Text = NettoBongkar.ToString("N2");
        beratakhir.Text = BeratAkhir.ToString("N2");
    }
}