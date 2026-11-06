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

            StockRetirePG x = new StockRetirePG();

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

            db.StockRetirePG.InsertOnSubmit(x);
            db.SubmitChanges();


            //Redirect
            App.ReloadClose(this);
            
            //Redirect
            App.ReloadClose(this);
        }
    }

    protected string Generate_Nomor(StockRetirePG j)
    {
        string x = string.Empty;

        int Tahun = DateTime.Now.Year;
        int Bulan = DateTime.Now.Month;

        //int Nomor = db.SalesOrder.Count(p => p.Tgl.Value.Year == Tahun && p.Tgl.Value.Month == Bulan );
        //Nomor = Nomor + 1;

        int Nomor = 1;

        try
        {
            var s = (from p in db.StockRetirePG
                     where p.OperatorTgl.Value.Year == Tahun && p.OperatorTgl.Value.Month == Bulan
                     orderby (p.Nomor.Substring(11, 4)) descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(11, 4)) + 1;
        }
        catch (Exception ex) { }

        return "SR/P" + "/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
    }
}