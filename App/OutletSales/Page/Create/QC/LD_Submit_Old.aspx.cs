using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_LD_PricingSubmit : System.Web.UI.Page
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
        var d = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml = d.Nomor;

        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {

            DocQCLD x = new DocQCLD();

            x.QCNama = petugasqc.Text;
            x.Keterangan = keterangan.Text;
            x.Nomor = Document.Generate_Nomor_QCLD(x);
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

            db.DocQCLD.InsertOnSubmit(x);
            db.SubmitChanges();

            //Redirect
            App.ReloadClose(this);
        }
    }

}