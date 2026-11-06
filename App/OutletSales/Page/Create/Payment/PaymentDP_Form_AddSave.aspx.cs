using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_Payment_PaymentDP_Form_AddSave : System.Web.UI.Page
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

    protected int myID { get { return App.QueryInt(this,"id"); } }

    protected int IDRef { get { return App.QueryInt(this, "idref"); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);
        if (!IsPostBack)
        {
            Save();
        }
    }

    protected void Save()
    {
        var gr = (from p in db.SalesReceiptDPPO where p.ID == IDRef select p).SingleOrDefault();
        decimal estimate_before = 0m;
        decimal estimate_now = 0m;
        decimal paid = 0m;
        int refid = 0;

        refid = gr.ID;
        estimate_before = gr.EstimasiHarga;

        var qr = (from p in db.SalesReceiptDPPO where p.ID == myID select p).SingleOrDefault();

        qr.IDDPPOReference = refid;

        qr.EstimasiHarga = gr.EstimasiHarga;
        qr.Keterangan = gr.Keterangan;

        bool isDPPO = false;

        if (gr.TipeProduct != null && gr.IDProduct != null)
        {
            qr.IDFrameColor = null;
            qr.GoldWeight1 = null;
            qr.GoldWeight2 = null;
            qr.TipeProductSample1 = null;
            qr.TipeProductSample2 = null;
            qr.IDProductSample1 = null;
            qr.IDProductSample2 = null;
            qr.DiamondWeight1 = null;
            qr.DiamondWeight2 = null;
            qr.RingSize1 = null;
            qr.RingSize2 = null;
            qr.Inscription1 = null;
            qr.Inscription2 = null;
            qr.IDFontType = null;
            qr.ImgSketch = null;

            qr.TipeProduct = gr.TipeProduct;
            qr.IDProduct = gr.IDProduct;
        }
        else
        {
            isDPPO = true;

            qr.IDFrameColor = gr.IDFrameColor;
            qr.GoldWeight1 = gr.GoldWeight1;
            qr.GoldWeight2 = gr.GoldWeight2;
            qr.TipeProductSample1 = gr.TipeProductSample1;
            qr.TipeProductSample2 = gr.TipeProductSample2;
            qr.IDProductSample1 = gr.IDProductSample1;
            qr.IDProductSample2 = gr.IDProductSample2;
            qr.DiamondWeight1 = gr.DiamondWeight1;
            qr.DiamondWeight2 = gr.DiamondWeight2;
            qr.RingSize1 = gr.RingSize1;
            qr.RingSize2 = gr.RingSize2;
            qr.Inscription1 = gr.Inscription1;
            qr.Inscription2 = gr.Inscription2;
            qr.IDFontType = gr.IDFontType;
            qr.ImgSketch = gr.ImgSketch;

            qr.TipeProduct = null;
            qr.IDProduct = null;
        }
        

        db.SubmitChanges();

        Response.Redirect("PaymentDP_Form.aspx?id=" + myID + "&status=1" + (isDPPO ? "&dp=2" : ""));
    }


}