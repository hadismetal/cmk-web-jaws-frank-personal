using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_PG_PricingSubmit : System.Web.UI.Page
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
            
        }
        fillForm();
    }

    //Form
    protected void fillForm()
    {
        var d = db.DocQCPG.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml = d.Nomor;

            petugasqc.Text = d.QCNama;
            logo.Text = d.Logo;
            merk.Text = d.Merk;
            fisik.Text = d.FisikKeseluruhan;
            netweight.Text = d.NetWeight;
            framecolor.Text = d.WarnaEmas;

            fillProduct(d.StockProductPG);
        }
    }

    protected void fillProduct(StockProductPG ms)
    {
        netto.InnerHtml = Str.Number2(ms.NettoWeight);
        kadar.InnerHtml = Str.Number2(Math.Ceiling(ms.KadarLogam.Value));
        framecolor.Text = ms.CharFrameColor.Nama;
    }
}