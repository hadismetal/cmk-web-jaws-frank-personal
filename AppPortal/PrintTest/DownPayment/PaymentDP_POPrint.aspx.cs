using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_Payment_PaymentDP_POPrint : System.Web.UI.Page
{
    private Map_CS _cs;
    private Map_DB _db;

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

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
        if (cs != null)
            cs.Dispose();
        if (db != null)
            db.Dispose();
        base.Dispose();
    }
    App gp = new App();
    protected int myID { get { return App.QueryInt(this); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        //App.Protect();
        //App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            Fill();

        }
    }

    protected void Fill()
    {
        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
        if (d == null)
            Response.Redirect("PaymentDP.aspx");
        else
        {
            no_po.Text = d.NomorDP;
            string lokasi = "";
            string phnumber = "";
            int brand = 0;
            if (d.TipeLokasi == 1)
            {
                var cek = (from p in db.LocOutlet where p.ID == d.IDLokasi select p).SingleOrDefault();
                lokasi = cek.Nama;
                phnumber = cek.AddrNoTelp;
                brand = cek.IDBrand;
            }
            else 
            {
                var cek = (from p in db.LocExhibition where p.ID == d.IDLokasi select p).SingleOrDefault();
                lokasi = cek.Nama;
                phnumber = cek.AddrNoTelp;
                brand = cek.IDBrand;
            }

            location.Text = lokasi;
            phnum.Text = phnumber;
            customer.Text = d.DataCustomer.Nama;
            lbcustomer.Text = d.DataCustomer.Nama;
			lbketerangan.Text = "<small>" + d.Keterangan + "</small>";
            //phcust.Text = (d.DataCustomer.AddrNoTelp != null)? d.DataCustomer.AddrNoTelp: "";
            var phcustomer = (from p in cs.Mscustomer where p.M_kode == d.DataCustomer.NoCustomer select p).SingleOrDefault();
            phcust.Text = (phcustomer.M_telepon1 != null) ? phcustomer.M_telepon1 : ((phcustomer.M_telepon2 != null) ? phcustomer.M_telepon2 : "");
            lbjr.Text = d.SalesNama;
			
			int freegwpcount = Db.SingleInteger("SELECT COUNT('') FROM LetterWR WHERE IDSalesReceiptDPPO = " + myID.ToString(), Db.Cnn_DB);
            if (freegwpcount == 1)
            {
                rowgwp.Visible = true;
                freegwp.Text = Db.SingleString("SELECT TOP 1 Letter FROM LetterWR WHERE IDSalesReceiptDPPO = " + myID.ToString(), Db.Cnn_DB);
            }
            else
            {
                rowgwp.Visible = false;
                freegwp.Text = "";
            }

            string company = (from p in db.CompanyBrand where p.ID == brand select p.Nama).SingleOrDefault();
            company = company.ToLower();

            if (company == "mondial") { img_Logo.ImageUrl = "/Images/spk/logo_spk_mondial.jpg"; }
            else if (company == "miss mondial") { img_Logo.ImageUrl = "/Images/spk/logo_spk_mondial.jpg"; }
            else if (company == "the palace") { img_Logo.ImageUrl = "/Images/spk/logo_spk_thepalace.jpg"; }
            else if (company == "frank & co") { img_Logo.ImageUrl = "/Images/spk/logo_spk_franknco.jpg"; }
            else { img_Logo.ImageUrl = "/Images/spk/logo_spk_franknco.jpg"; }

            if (d.IDProductSample1 != null)
            {
                if (d.TipeProductSample1 == 1) // dj
                {
                    var cek = db.StockProductDJ.SingleOrDefault(p => p.ID == d.IDProductSample1);
                    lbitem.Text = (from p in db.CharProductItem where p.ID == cek.StockProductDJ_CharProduct.ProductItem && p.Disable == false select p.Nama).SingleOrDefault();
                    lbplu1.Text = cek.Nomor;
                    brplu1.Text = "IDR " + Convert.ToDecimal(cek.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                    lbframe.Text = cek.StockProductDJ_CharProduct.CharFrameColor.Nama;
                }
                else if (d.TipeProductSample1 == 2) // pg
                {
                    var cek = db.StockProductPG.SingleOrDefault(p => p.ID == d.IDProductSample1);
                    lbitem.Text = (from p in db.CharProductItem where p.ID == cek.ProductItem && p.Disable == false select p.Nama).SingleOrDefault();
                    lbplu1.Text = cek.Nomor;
                    decimal goldprice = (cek.FixRate == false) ? gp.GetPGPricing(cek, false) : cek.TotalHargaJual.Value;
                    //brplu1.Text = "IDR " + goldprice.ToString("N0");
                    brplu1.Text = "";
                    lbframe.Text = cek.CharFrameColor.Nama;
                }
                else if (d.TipeProductSample1 == 3) //gj
                {
                    var cek = db.StockProductGJ.SingleOrDefault(p => p.ID == d.IDProductSample1);
                    lbitem.Text = (from p in db.CharProductItem where p.ID == cek.StockProductGJ_CharProduct.ProductItem && p.Disable == false select p.Nama).SingleOrDefault();
                    lbplu1.Text = cek.Nomor;
                    brplu1.Text = "IDR " + Convert.ToDecimal(cek.StockProductGJ_PricingMU.HargaRupiahRound).ToString("N0");
                    lbframe.Text = cek.StockProductGJ_CharProduct.CharFrameColor.Nama;
                }
                else if (d.TipeProductSample1 == 4) //ld
                {
                    var cek = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == d.IDProductSample1);
                    lbplu1.Text = cek.Nomor;
                    brplu1.Text = "USD " + Convert.ToDecimal(cek.HargaTotal).ToString("N0");
                }
            }
            if (d.IDProductSample2 != null)
            {
                if (d.TipeProductSample2 == 1)// dj
                {
                    var cek = db.StockProductDJ.SingleOrDefault(p => p.ID == d.IDProductSample2);
                    lbitem.Text = (from p in db.CharProductItem where p.ID == cek.StockProductDJ_CharProduct.ProductItem && p.Disable == false select p.Nama).SingleOrDefault();
                    lbplu2.Text = cek.Nomor;
                    brplu2.Text = "IDR " + Convert.ToDecimal(cek.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                }
                else if (d.TipeProductSample1 == 2) //pg
                {
                    var cek = db.StockProductPG.SingleOrDefault(p => p.ID == d.IDProductSample2);
                    lbitem.Text = (from p in db.CharProductItem where p.ID == cek.ProductItem && p.Disable == false select p.Nama).SingleOrDefault();
                    lbplu2.Text = cek.Nomor;
                    decimal goldprice = (cek.FixRate == false) ? gp.GetPGPricing(cek, false) : cek.TotalHargaJual.Value;
                    //brplu2.Text = "IDR " + goldprice.ToString("N0");
                    brplu2.Text = "";
                }
                else if (d.TipeProductSample1 == 3) //gj
                {
                    var cek = db.StockProductGJ.SingleOrDefault(p => p.ID == d.IDProductSample2);
                    lbitem.Text = (from p in db.CharProductItem where p.ID == cek.StockProductGJ_CharProduct.ProductItem && p.Disable == false select p.Nama).SingleOrDefault();
                    lbplu2.Text = cek.Nomor;
                    brplu2.Text = "IDR " + Convert.ToDecimal(cek.StockProductGJ_PricingMU.HargaRupiahRound).ToString("N0");
                }
                else if (d.TipeProductSample1 == 4) //ld
                {
                    var cek = db.StockProductLD_Stone1B.SingleOrDefault(p => p.ID == d.IDProductSample2);
                    lbplu2.Text = cek.Nomor;
                    brplu2.Text = "USD " + Convert.ToDecimal(cek.HargaTotal).ToString("N0");
                }
            }
            if (d.GoldWeight1 != null) { lbgwf.Text = Convert.ToDecimal(d.GoldWeight1).ToString("N2"); }
            if (d.GoldWeight2 != null) { lbgwm.Text = Convert.ToDecimal(d.GoldWeight2).ToString("N2"); }
            if (d.DiamondWeight1 != null) { lbdwf.Text = Convert.ToDecimal(d.DiamondWeight1).ToString("N3"); }
            if (d.DiamondWeight2 != null) { lbdwm.Text = Convert.ToDecimal(d.DiamondWeight2).ToString("N3"); }
            if (d.RingSize1 != null) { lbrsf.Text = Convert.ToDecimal(d.RingSize1).ToString("N1"); }
            if (d.RingSize2 != null) { lbrsm.Text = Convert.ToDecimal(d.RingSize2).ToString("N1"); }
            if (d.Inscription1 != null) { inscf.Text = d.Inscription1; }
            if (d.Inscription2 != null) { inscm.Text = d.Inscription2; }
            if (d.IDFontType != null) { lbfont.Text = ((from p in db.FontType where p.ID == d.IDFontType select p.Nama).SingleOrDefault()).ToString(); }
            if (d.Tgl != null) { lbdorder.Text = Convert.ToDateTime(d.Tgl).ToString("dd MMMM yyyy"); }
            if (d.DeliveryDate != null) { lbddeliver.Text = Convert.ToDateTime(d.DeliveryDate).ToString("dd MMMM yyyy"); }
        }// end display


    }
}