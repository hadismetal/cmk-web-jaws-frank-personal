using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Approval_PG_Retirement : System.Web.UI.Page
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
        var d = db.StockRetirePG.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            petugaslebur.Text = d.Nama;
            keterangan.Text = d.Keterangan;
            alasan.Text = d.Alasan;
            //Header
            header_nomor.InnerHtml = d.Nomor;

            //Picture
            Upload.Image(imgpicture, d.StockProductPG.ImgPicture);

            fillProduct(d.StockProductPG);

            susutbongkar.Text = Str.Number2(d.SusutBongkar);
            nettobongkar.Text = Str.Number2(d.NettoSetelahBongkar);
            susutlebur.Text = Str.Number2(d.SusutLebur);
            beratakhir.Text = Str.Number2(d.BeratAkhir);
        }
    }

    protected void fillProduct(StockProductPG ms)
    {
        netto.InnerHtml = Str.Number2(ms.NettoWeight);
        kadar.InnerHtml = Str.Number2(Math.Ceiling(ms.KadarLogam.Value));
        frame.InnerHtml = ms.CharFrameColor.Nama;
    }

}