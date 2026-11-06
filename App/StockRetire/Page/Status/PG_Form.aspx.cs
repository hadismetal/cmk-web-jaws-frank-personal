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
            App.NumberFormat(susutbongkar);
            App.NumberFormat(susutlebur);
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

            susutbongkar.Text = Str.Number2(d.SusutBongkar);
            nettobongkar.Text = Str.Number2(d.NettoSetelahBongkar);
            susutlebur.Text = Str.Number2(d.SusutLebur);
            beratakhir.Text = Str.Number2(d.BeratAkhir);

            //Picture
            Upload.Image(imgpicture, d.StockProductPG.ImgPicture);

            fillProduct(d.StockProductPG);
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
        var d = db.StockRetirePG.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {

            d.NettoWeight = d.StockProductPG.Harga;
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