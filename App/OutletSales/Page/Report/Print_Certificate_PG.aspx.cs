using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class Page_Report_Print_Certificate_PG : System.Web.UI.Page
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

   // protected int myID { get { return App.QueryInt(this); } }

    protected string myPLU { get { return Request.QueryString["PLU"]; } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);
        Page.MaintainScrollPositionOnPostBack = true;
        if (!IsPostBack)
        {
            Fill();

        }
    }

    protected void Fill()
    {
        byte[] dataplu = Convert.FromBase64String(myPLU);
        string xplu = Encoding.UTF8.GetString(dataplu);

        var d = db.StockProductPG.SingleOrDefault(p => p.Nomor == xplu);
        if (d != null)
        {
            if (d.LocWarehouse.CompanyBrand.NamaKode == "F")
            {
                css.Text = "<style>body { color: #7c0545; font-weight: bold;}</style>";
            }
            else
            {
                css.Text = "<style>body { color: #000000; font-weight: bold;}</style>";
            }

            plu.Text = d.Nomor;

            kadar.Text = (Math.Floor(Convert.ToDecimal(d.KadarLogam)) == 37)
                       ? (Convert.ToDecimal(d.KadarLogam)).ToString("N1")
                       : Math.Floor(Convert.ToDecimal(d.KadarLogam)).ToString("N0")
                       ;
            weight.Text = Convert.ToDecimal(d.NettoWeight).ToString("N2");
            //kodekaret.Text = d.ProtoNomor;
            item.Text = d.CharProductItem.Nama;
            //img.Text = "<img src=\"/ImageCrop.aspx?path=" + d.ImgPicture + "\" />";
            img.Text = "<img src=\"/Images.aspx?path=" + d.ImgPicture + "\" style='max-width:90px; max-height:67px;' />";

            string stone_text = "";
            string shape = "";
            string color = "";
            string clarity = "";

           

            var rsCB = (from p in db.CompanyBrand
                        where p.Draft == false && p.Disable == false
                        select p.Nama).First();

            string CB = ""; /// mondial, franknco, missmondial, thepalace
            if (rsCB.Count() > 0) { CB = rsCB.ToLower(); }

            txcolor.Text = d.CharFrameColor.Nama;
        }
    }
}