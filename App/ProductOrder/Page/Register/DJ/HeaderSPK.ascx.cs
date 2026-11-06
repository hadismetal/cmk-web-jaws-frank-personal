using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Register_HeaderSPK : System.Web.UI.UserControl
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
    protected int myID { get { return Convert.ToInt32(Request.QueryString["id"]); } }
    protected int tipe { get { return Convert.ToInt32(Request.QueryString["tipe"]); } }
    protected int idkodekaret { get { return Convert.ToInt32(Request.QueryString["idkodekaret"]); } }
    protected int idproductdesign { get { return Convert.ToInt32(Request.QueryString["idproductdesign"]); } }

	protected void Page_Load(object sender, EventArgs e) 
    {
		if (!IsPostBack) 
        {
            fillHeader();
		}
	}

    protected void fillHeader()
    {
        string xtipespk = "";
        string xdesigner = "";
        string xkodekaretjaws = "";
        string xkodekaretlama = "";
        string xreferensinomor = "";
        string xreferensitanggal = "";
        bool isNO = false;

        var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d != null)
        {
            xtipespk = (d.StatusOrder == 1) ? "New Order" : "Repeat Order";
            xdesigner = d.DataHuman.Nama + " (" + d.DataHuman.CompanyBrand.Nama + ")";

            if (d.StatusOrder == 1)
            {
                isNO = true;
                
                xreferensinomor = d.ProductDesign.Nomor;
                xreferensitanggal = Str.Tgl(d.ProductDesign.Tgl);

            }
            else if (d.StatusOrder == 2)
            {
                xkodekaretjaws = (d.StatusOrder == 2) ? d.KodeKaretDJ.Nomor : "";
                xkodekaretlama = (d.KodeKaretDJ != null) ? d.KodeKaretDJ.KodeKaretLama : "";
            }
            

        }
        else
        {
            // front page menu

            xtipespk = (tipe == 1) ? "New Order" : "Repeat Order";

            if (idproductdesign > 0 && tipe == 1)
            {
                isNO = true;
                var x = db.ProductDesign.SingleOrDefault(p => p.ID == idproductdesign && p.Draft == false && p.StatusSPK == false);
                if (x != null)
                {
                    xdesigner = x.DataHuman.Nama + " (" + x.DataHuman.CompanyBrand.Nama + ")";
                    
                    xreferensinomor = " <span style='color:blue;'>(" + x.Nomor + ")</span>";
                    xreferensitanggal = Str.Tgl(x.Tgl);

                }
            }

            if (idkodekaret > 0 && tipe == 2)
            {
                var x = db.KodeKaretDJ.SingleOrDefault(p => p.ID == idkodekaret);
                if (x != null)
                {
                    xdesigner = x.DataHuman.Nama + " (" + x.DataHuman.CompanyBrand.Nama + ")";
                    
                    xkodekaretjaws = (x.Nomor != null) ? x.Nomor : "";
                    xkodekaretlama = x.KodeKaretLama;
                }

            }

        }

        div_no_1.Visible = isNO;
        div_no_2.Visible = isNO;
        div_ro_1.Visible = !isNO;
        div_ro_2.Visible = !isNO;

        tipespk.InnerHtml = xtipespk;
        designer.InnerText = xdesigner;
        kodekaretjaws.InnerText = xkodekaretjaws;
        kodekaretlama.InnerText = xkodekaretlama;
        referensinomor.InnerHtml = xreferensinomor;
        referensitanggal.InnerText = xreferensitanggal;

    }
}