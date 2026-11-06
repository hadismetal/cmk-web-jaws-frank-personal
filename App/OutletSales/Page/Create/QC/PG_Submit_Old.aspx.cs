using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_DJ_PricingSubmit : System.Web.UI.Page
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
        var d = db.StockProductPG.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml = d.Nomor;

            fillProduct(d);

        }
    }

    protected void fillProduct(StockProductPG ms)
    {
        netto.InnerHtml = Str.Number2(ms.NettoWeight);
        kadar.InnerHtml = Str.Number2(Math.Ceiling(ms.KadarLogam.Value));
        frame.InnerHtml = ms.CharFrameColor.Nama;
    }



    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockProductPG.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {

            DocQCPG x = new DocQCPG();

            x.QCNama = petugasqc.Text;
            x.Keterangan = keterangan.Text;
            x.Nomor = Document.Generate_Nomor_QCPG(x);
            x.IDProduct = d.ID;
            x.Logo = logo.Text;
            x.KadarLogam = kadaremas.Text;
            //x.Merk = merk.Text;
            x.FisikKeseluruhan = fisik.Text;
            x.NetWeight = netweight.Text;
            x.WarnaEmas = framecolor.Text;
            x.OperatorNama = App.UserNama;
            x.OperatorTgl = DateTime.Now;
            x.Draft = false;
            x.DraftDate = null;

            db.DocQCPG.InsertOnSubmit(x);
            db.SubmitChanges();

            //Redirect
            App.ReloadClose(this);
        }
    }

}