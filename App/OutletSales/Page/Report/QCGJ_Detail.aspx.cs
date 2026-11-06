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
            
        }
        fillForm();
    }

    //Form
    protected void fillForm()
    {
        var d = db.DocQCGJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            //Header
            header_nomor.InnerHtml = d.Nomor;

            petugasqc.Text = d.QCNama;
            blemish.Text = d.Blemish;
            jlhbutir.Text = d.JumlahButir;
            sertifikat.Text = d.Sertifikat;
            noncz.Text = d.NonCZ;
            cz.Text = d.CZ;
            logo.Text = d.Logo;
            kadaremas.Text = d.KadarEmas;
            merk.Text = d.Merk;
            fisik.Text = d.FisikKeseluruhan;
            netweight.Text = d.NetWeight;
            framecolor.Text = d.WarnaEmas;

            fillStone();
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

    //Stone
    protected void fillStone()
    {
        int GT_Butir = 0;
        decimal GT_Carat = 0;

        //Grand-Total
        Sub_Total(GT_Butir, GT_Carat, true);
    }

    
    protected void Sub_Header(int StoneIndex)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = "sub-header";
        string x = string.Empty;

        x += "<br />";
        x += DL.StoneIndex(StoneIndex);

        td = new TableCell();
        td.Text = x;
        td.CssClass = "text-lg text-warning";
        td.ColumnSpan = App.TableColspan(tb);
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
    protected void Sub_Total(int TotalButir, decimal TotalCarat, bool GrandTotal)
    {
        TableRow tr = new TableRow();
        TableCell td;

        tr.CssClass = GrandTotal ? "text-grandtotal" : "text-subtotal";

        td = new TableCell();
        td.ColumnSpan = 3;
        td.Text += GrandTotal ? "Grand Total" : "Sub-Total";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Integer(TotalButir);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        td = new TableCell();
        td.Text = Str.Number3(TotalCarat);
        td.CssClass = "text-center";
        tr.Cells.Add(td);

        tb.Rows.Add(tr);
    }
}