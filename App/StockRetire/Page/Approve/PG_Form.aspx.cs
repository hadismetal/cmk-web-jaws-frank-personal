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


            if (d.Status != 0)
            {
                reject.Visible = false;
                save.Visible = false;
            }
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

            d.Approval = App.UserNama;
            d.ApprovalTgl = DateTime.Now;
            d.Status = 1;
            d.StockProductPG.StatusLebur = true;
            db.SubmitChanges();

            StockLedgerPG l = new StockLedgerPG();

            // Value (See Library DB - Ledger for Value and Remark)
            decimal Value = -1; // 1 : in , -1 : out
            int Remark = 20; // See Library DB - Ledger
            string Keterangan = d.Nomor; // Commonly filled by document number or id 
            if (Keterangan.Trim() == "") Keterangan = DL.LedgerRemark(Remark); // dont change!

            string Lokasi = "";
            if (d.StockProductPG.StockActualPG.TipeLokasi == 1)
                Lokasi = Db.SingleString("SELECT Nama FROM LocOutlet WHERE ID=" + d.StockProductPG.StockActualPG.IDLokasi, Db.Cnn_DB);
            else if (d.StockProductPG.StockActualPG.TipeLokasi == 2)
                Lokasi = Db.SingleString("SELECT Nama FROM LocExhibition WHERE ID=" + d.StockProductPG.StockActualPG.IDLokasi, Db.Cnn_DB);

            //Input
            l.IDProduct = d.IDProduct;
            l.TipeLokasi = d.StockProductPG.StockActualPG.TipeLokasi;
            l.IDLokasi = d.StockProductPG.StockActualPG.IDLokasi;
            l.NamaLokasi = Lokasi;
            l.Value = Value;
            l.Remark = Remark;
            l.Keterangan = Keterangan;

            //User-Info
            l.Operator = App.UserNama;
            l.OperatorTgl = DateTime.Now;

            //Commit
            db.StockLedgerPG.InsertOnSubmit(l);
            db.SubmitChanges();

            //Redirect
            App.ReloadClose(this);
        }
    }

    protected void reject_Click(object sender, EventArgs e)
    {
        var d = db.StockRetirePG.SingleOrDefault(p => p.ID == myID);
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