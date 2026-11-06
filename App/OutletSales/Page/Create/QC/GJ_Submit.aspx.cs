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

        if (!IsPostBack)
        {
            
        }
        fillForm();
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

            fillProduct(d);

            fillMU(d);
        }
    }

    protected void fillProduct(StockProductGJ ms)
    {
        var d = ms.StockProductGJ_PricingProduct;
        var x = ms.StockProductGJ_CharProduct;
        if (d != null && x != null)
        {
            netto.InnerHtml = Str.Number2(x.NettoWeight);
            kadar.InnerHtml = Str.Number2(x.KadarLogam);
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

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockProductGJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {

            DocQCGJ x = new DocQCGJ();

            x.QCNama = petugasqc.Text;
            x.Keterangan = keterangan.Text;
            x.Nomor = Document.Generate_Nomor_QCGJ(x);
            x.IDProduct = d.ID;
            x.Blemish = blemish.Text;
            x.JumlahButir = jlhbutir.Text;
            x.Sertifikat = sertifikat.Text;
            x.NonCZ = noncz.Text;
            x.CZ = cz.Text;
            x.Logo = logo.Text;
            x.KadarEmas = kadaremas.Text;
            x.Merk = merk.Text;
            x.FisikKeseluruhan = fisik.Text;
            x.NetWeight = netweight.Text;
            x.WarnaEmas = framecolor.Text;
            x.OperatorNama = App.UserNama;
            x.OperatorTgl = DateTime.Now;
            x.Draft = false;
            x.DraftDate = null;

            db.DocQCGJ.InsertOnSubmit(x);
            db.SubmitChanges();

            //Redirect
            App.ReloadClose(this);
        }
    }

}