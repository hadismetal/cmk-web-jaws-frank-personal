using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.IO;
using System.Web.Script.Services;
using System.Web.UI.HtmlControls;


public partial class Page_Register_Sales_Form : System.Web.UI.Page
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
    App gp = new App();
    
    
    string ID = "";

    protected int myID { get { return App.QueryInt(this, "id"); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected int myIDLokasi { get { return App.QueryInt(this, "idlokasi"); } }
    protected int status { get { return App.QueryInt(this, "status"); } }
    protected int myDP { get { return App.QueryInt(this, "dp"); } }

    int mode;

    decimal price_pg = 0m;
    decimal price_dj = 0m;
    decimal price_ld = 0m;
    decimal price_gj = 0m;


    decimal price_pg1 = 0m;
    decimal price_dj1 = 0m;
    decimal price_ld1 = 0m;
    decimal price_gj1 = 0m;
    int fc1 = 0;

    decimal price_pg2 = 0m;
    decimal price_dj2 = 0m;
    decimal price_ld2 = 0m;
    decimal price_gj2 = 0m;
    int fc2 = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);


        if (!IsPostBack)
        {
            lbmode.InnerText = "1";
            //lbmode2.InnerText = "1";
            
            price_estimates.Text = "0";
            price_estimates.Focus();
            //DLC.CharFrameColor(framecolour);
            DLC.CharFrameColor(framecolour);
            //DLC.FontType(dfont);
			FontType(dfont);

            //Form
            fillForm();

            if (myID > 0)
            {
                customerSearch(myID);
            }

            search_transaction.HRef += "?id=" + myID;

            //Form-Default
            tgl.Text = Str.Tgl(DateTime.Today);
            tgl.Enabled = false;

            txdod.Text = Str.Tgl(DateTime.Today.AddDays(30));

            ske_format.InnerHtml = Str.ComaExt(Upload.ImageFormat);

            lbid.Text = myID.ToString();
            lbstatus.Text = status.ToString();

        }
  
    }

    protected void customerSearch(int xID)
    {
        search_customer.HRef = (STAMPS.xActivate) ? "/OutletSales/Page/Create/DataCustomerStamps.aspx" : search_customer.HRef;
        search_customer.HRef += "?id=" + xID;
        search_customer.HRef += (STAMPS.xActivate) ? "&mod=DOWNPAYMENT" : "";
    }

    //Exception
    protected void DataError() { Response.Redirect("Sales.aspx"); }

    //Form

    protected void fillForm()
    {
        btn_cancel_plu_po1.Visible = false;
        btn_cancel_plu_po2.Visible = false;

        string tipeplu = "";
        div_dp_form.Visible = false;
        span_plu_added.Visible = false;
        span_plu_added_type.Visible = false;
        div_presentation_tab.Visible = false;
        div_previous_dp.Visible = false;
        keterangan.Enabled = false;
        price_estimates.Enabled = false;
        btn_cancel_previous_dp.Visible = false;
		txtletterwr.Enabled = false;

        string xplu = "";
        string xtipeplu = "";
        string xharga = "";
        string xcurrency = "";
        string ximage = "";
        string xproductinfo = "";

        //checkform2();
        if (status != 1)
        {
            if (tipe == 1)
            {
                var d = db.LocOutlet.SingleOrDefault(p => p.ID == myIDLokasi && !p.Disable);
                if (d == null) DataError();
                else
                {
                    warehouse.InnerHtml = d.Nama;
                    SalesReceiptDPPO n = new SalesReceiptDPPO();

                    //Location
                    n.IDLokasi = myIDLokasi;
                    n.TipeLokasi = tipe;

                    //Status
                    n.Draft = true;
                    n.DraftDate = DateTime.Now;

                    //User-Info
                    n.OperatorNama = App.UserNama;
                    n.OperatorTgl = DateTime.Now;
                    n.StatusDP = 0;

                    //Commit
                    db.SalesReceiptDPPO.InsertOnSubmit(n);
                    db.SubmitChanges();

                    string qstr = "?id=" + n.ID;
                    customerSearch(n.ID);
                    search_agent.HRef += qstr;
                    ID = n.ID.ToString();
                }
            }
            else if (tipe == 2)
            {
                var d = db.LocExhibition.SingleOrDefault(p => p.ID == myIDLokasi && !p.Disable);
                if (d == null) DataError();
                else
                {
                    warehouse.InnerHtml = d.Nama;
                    SalesReceiptDPPO n = new SalesReceiptDPPO();

                    //Location
                    n.IDLokasi = myIDLokasi;
                    n.TipeLokasi = tipe;

                    //Status
                    n.Draft = true;
                    n.DraftDate = DateTime.Now;

                    //User-Info
                    n.OperatorNama = App.UserNama;
                    n.OperatorTgl = DateTime.Now;
                    n.StatusDP = 0;
                    //Commit
                    db.SalesReceiptDPPO.InsertOnSubmit(n);
                    db.SubmitChanges();

                    string qstr = "?id=" + n.ID;
                    customerSearch(n.ID);
                    search_agent.HRef += qstr;
                    ID = n.ID.ToString();
                }
            }
           
        }
        else // status = 1  (Data already saved)
        {
            var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
            if (d == null) DataError();
            else
            {
                if (d.IDFrameColor != null)
                {
                    framecolour.SelectedValue = d.IDFrameColor.Value.ToString();
                }

                insf.Text = d.Inscription1;
                insm.Text = d.Inscription2;

                gwf.Text = (d.GoldWeight1 != null) ? d.GoldWeight1.Value.ToString("N2") : "0";
                gwm.Text = (d.GoldWeight2 != null) ? d.GoldWeight2.Value.ToString("N2") : "0";

                dwf.Text = (d.DiamondWeight1 != null) ? d.DiamondWeight1.Value.ToString("N2") : "0";
                dwm.Text = (d.DiamondWeight2 != null) ? d.DiamondWeight2.Value.ToString("N2") : "0";

                rsf.Text = (d.RingSize1 != null) ? d.RingSize1.Value.ToString("N2") : "0";
                rsm.Text = (d.RingSize2 != null) ? d.RingSize2.Value.ToString("N2") : "0";

                keterangan.Text = d.Keterangan;

                price_estimates.Text = d.EstimasiHarga.ToString("N0");

                if (d.IDFontType != null)
                {
                    dfont.SelectedValue = d.IDFontType.Value.ToString();
                }

                dpst.Attributes.Remove("class");
                dppo.Attributes.Remove("class");
                dpst.Attributes.Add("class", "tab-pane");
                dppo.Attributes.Add("class", "tab-pane");

                li_dp_stock.Attributes.Remove("class");
                li_dp_po.Attributes.Remove("class");
                save.Attributes.Remove("class");
                if ((d.TipeProduct != null && d.IDProduct != null))
                {
                    btn_cancel_plu_stock.Visible = true;
                    save.Attributes.Add("class", "btn btn-primary");
                    li_dp_stock.Visible = true;
                    li_dp_po.Visible = false;

                    li_dp_stock.Attributes.Add("class", "active");
                    dpst.Attributes.Add("class", "tab-pane active");
                }
                else
                {
                    if (myDP == 2)
                    {
                        li_dp_po.Attributes.Add("class", "active");
                        dppo.Attributes.Add("class", "tab-pane active");
                    }
                    else
                    {
                        li_dp_stock.Attributes.Add("class", "active");
                        dpst.Attributes.Add("class", "tab-pane active");
                    }

                    btn_cancel_plu_stock.Visible = false;
                    save.Attributes.Add("class", "btn btn-primary disabled");
                    li_dp_stock.Visible = true;
                    li_dp_po.Visible = true;
                }
                

                if (d.IDCustomer == null || d.IDSales == null)
                {
                    p_input1.Visible = true;
                    p_input2.Visible = true;
                    div_previous_dp.Visible = false;
                    div_presentation_tab.Visible = false;
                    keterangan.Enabled = false;
                    price_estimates.Enabled = false;
                    div_dp_form.Visible = false;
					txtletterwr.Enabled = false;
                }
                else
                {
                    p_input1.Visible = false;
                    p_input2.Visible = false;
                    div_previous_dp.Visible = true;
                    div_presentation_tab.Visible = true;
                    keterangan.Enabled = true;
                    price_estimates.Enabled = true;
                    div_dp_form.Visible = true;
					txtletterwr.Enabled = true;
                }

                string Brand = "";
                if (d.TipeLokasi == 1)
                {
                    var k = db.LocOutlet.SingleOrDefault(p => p.ID == d.IDLokasi);
                    Brand = k.Nama;
                }
                else if (d.TipeLokasi == 2)
                {
                    var k = db.LocExhibition.SingleOrDefault(p => p.ID == d.IDLokasi);
                    Brand = k.Nama;
                }

                warehouse.InnerHtml = Brand;
                if (d.IDCustomer != null)
                    customer.Text = d.DataCustomer.Nama;
                if (d.IDSales != null)
                    agent.Text = d.DataSales.Nama;
				
				int lettercount = Db.SingleInteger("SELECT COUNT('') Total FROM LetterWR WHERE IDSalesReceiptDPPO = '" + d.ID.ToString() + "' ", Db.Cnn_DB);
				if(lettercount == 1)
				{
					txtletterwr.Text = Db.SingleString("SELECT TOP 1 ISNULL(Letter,'') FROM LetterWR WHERE IDSalesReceiptDPPO = '" + myID + "' ", Db.Cnn_DB);
				}

                //while dp po
                if (d.IDProduct == null && (d.IDProductSample1 != null || d.IDProductSample2 != null))
                {
                    //Product type :
                    //  1 = DJ
                    //  2 = PG
                    //  3 = GJ
                    //  4 = LD
                    if (d.IDProductSample1 != null)
                    {
                        btn_cancel_plu_po1.Visible = true;

                        if (d.TipeProductSample1 == 1) // dj
                        {
                            tipeplu = "DJ";
                            var sample1 = (from p in db.StockProductDJ
                                           where p.ID == d.IDProductSample1 && p.Draft == false && p.Approval != null
                                           select p).SingleOrDefault();

                            xplu = sample1.Nomor;
                            xharga = Convert.ToDecimal(sample1.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                            xcurrency = "IDR";
                            ximage = (sample1.ImgPicture != null) ? sample1.ImgPicture : "";

                            cplu1.Text = sample1.Nomor;
                        }
                        else if (d.TipeProductSample1 == 2) //pg
                        {
                            tipeplu = "PG";
                            var sample1 = (from p in db.StockProductPG
                                           where p.ID == d.IDProductSample1 && p.Draft == false && p.Approval != null
                                           select p).SingleOrDefault();

                            decimal GoldPrice = gp.GetPGPricing(sample1, false);
                            if (sample1.FixRate == true)
                            {
                                GoldPrice = sample1.TotalHargaJual.Value;
                            }

                            xplu = sample1.Nomor;
                            xharga = "";
                            xcurrency = "";
                            ximage = (sample1.ImgPicture != null) ? sample1.ImgPicture : "";

                            cplu1.Text = xplu;
                        }
                        else if (d.TipeProductSample1 == 3) // gj
                        {
                            tipeplu = "GJ";
                            var sample1 = (from p in db.StockProductGJ
                                           where p.ID == d.IDProductSample1 && p.Draft == false && p.Approval != null
                                           select p).SingleOrDefault();

                            xplu = sample1.Nomor;
                            xharga = Convert.ToDecimal(sample1.StockProductGJ_PricingMU.HargaRupiahRound).ToString("N0");
                            xcurrency = "IDR";
                            ximage = (sample1.ImgPicture != null) ? sample1.ImgPicture : "";

                            cplu1.Text = xplu;
                        }
                        else if (d.TipeProductSample1 == 4) // ld
                        {
                            tipeplu = "LD";
                            var sample1 = (from p in db.StockProductLD_Stone1B
                                           where p.ID == d.IDProductSample1
                                           select p).SingleOrDefault();

                            xplu = sample1.Nomor;
                            xharga = Convert.ToDecimal(sample1.HargaTotal).ToString("N0");
                            xcurrency = "USD";
                            ximage = (sample1.ImgPicture != null) ? sample1.ImgPicture : "";

                            cplu1.Text = xplu;
                        }
                        lbmode.InnerText = "2";

                        if (tipeplu == "DJ") { xtipeplu = "Diamond Jewellery"; }
                        if (tipeplu == "PG") { xtipeplu = "Plain Gold"; }
                        if (tipeplu == "GJ") { xtipeplu = "Gold Jewellery"; }
                        if (tipeplu == "LD") { xtipeplu = "Loose Diamond"; }

                        xproductinfo = "<div class='row'>";
                        xproductinfo += "<div class='col-md-8 col-sm-8 col-xs-12'>";
                        xproductinfo += "<div class='info-box'>";
                        xproductinfo += "<span class='info-box-icon bg-silver'>";
                        if (ximage != "")
                        {
                            xproductinfo += "<img src=\"/Images.aspx?path=" + ximage + "\" />";
                        }
                        xproductinfo += "</span>";
                        xproductinfo += "<div class='info-box-content'>";
                        xproductinfo += "<span class='info-box-text'>" + xplu + "</span>";
                        xproductinfo += "<span class='info-box-number'>" + xharga + "<small class='text-muted'>&nbsp;" + xcurrency + "</small></span>";
                        xproductinfo += "<span class='info-box-descripton'>" + xtipeplu + "</span>";
                        xproductinfo += "</div>";
                        xproductinfo += "<!-- /.info-box-content -->";
                        xproductinfo += "</div>";
                        xproductinfo += "<!-- /.info-box -->";
                        xproductinfo += "</div>";
                        xproductinfo += "</div>";

                        lbprice1.Text = xproductinfo;
                    }
                    if (d.IDProductSample2 != null)
                    {
                        btn_cancel_plu_po2.Visible = true;

                        if (d.TipeProductSample2 == 1) // dj
                        {
                            tipeplu = "DJ";
                            var sample2 = (from p in db.StockProductDJ
                                           where p.ID == d.IDProductSample2 && p.Draft == false && p.Approval != null
                                           select p).SingleOrDefault();

                            xplu = sample2.Nomor;
                            xharga = Convert.ToDecimal(sample2.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                            xcurrency = "IDR";
                            ximage = (sample2.ImgPicture != null) ? sample2.ImgPicture : "";

                            cplu2.Text = sample2.Nomor;
                        }
                        else if (d.TipeProductSample2 == 2) // pg
                        {
                            tipeplu = "PG";
                            var sample2 = (from p in db.StockProductPG
                                           where p.ID == d.IDProductSample2 && p.Draft == false && p.Approval != null
                                           select p).SingleOrDefault();

                            decimal GoldPrice = gp.GetPGPricing(sample2, false);
                            if (sample2.FixRate == true)
                            {
                                GoldPrice = sample2.TotalHargaJual.Value;
                            }

                            xplu = sample2.Nomor;
                            xharga = "";
                            xcurrency = "";
                            ximage = (sample2.ImgPicture != null) ? sample2.ImgPicture : "";

                            cplu2.Text = xplu;
                        }
                        else if (d.TipeProductSample2 == 3) // gj
                        {
                            tipeplu = "GJ";
                            var sample2 = (from p in db.StockProductGJ
                                           where p.ID == d.IDProductSample2 && p.Draft == false && p.Approval != null
                                           select p).SingleOrDefault();

                            xplu = sample2.Nomor;
                            xharga = Convert.ToDecimal(sample2.StockProductGJ_PricingMU.HargaRupiahRound).ToString("N0");
                            xcurrency = "IDR";
                            ximage = (sample2.ImgPicture != null) ? sample2.ImgPicture : "";

                            cplu.Text = xplu;
                        }
                        else if (d.TipeProductSample2 == 4) // ld
                        {
                            tipeplu = "LD";
                            var sample2 = (from p in db.StockProductLD_Stone1B
                                           where p.ID == d.IDProductSample2
                                           select p).SingleOrDefault();

                            xplu = sample2.Nomor;
                            xharga = Convert.ToDecimal(sample2.HargaTotal).ToString("N0");
                            xcurrency = "USD";
                            ximage = (sample2.ImgPicture != null) ? sample2.ImgPicture : "";

                            cplu.Text = xplu;
                        }
                        lbmode.InnerText = "2";

                        if (tipeplu == "DJ") { xtipeplu = "Diamond Jewellery"; }
                        if (tipeplu == "PG") { xtipeplu = "Plain Gold"; }
                        if (tipeplu == "GJ") { xtipeplu = "Gold Jewellery"; }
                        if (tipeplu == "LD") { xtipeplu = "Loose Diamond"; }

                        xproductinfo = "<div class='row'>";
                        xproductinfo += "<div class='col-md-8 col-sm-8 col-xs-12'>";
                        xproductinfo += "<div class='info-box'>";
                        xproductinfo += "<span class='info-box-icon bg-silver'>";
                        if (ximage != "")
                        {
                            xproductinfo += "<img src=\"/Images.aspx?path=" + ximage + "\" />";
                        }
                        xproductinfo += "</span>";
                        xproductinfo += "<div class='info-box-content'>";
                        xproductinfo += "<span class='info-box-text'>" + xplu + "</span>";
                        xproductinfo += "<span class='info-box-number'>" + xharga + "<small class='text-muted'>&nbsp;" + xcurrency + "</small></span>";
                        xproductinfo += "<span class='info-box-descripton'>" + xtipeplu + "</span>";
                        xproductinfo += "</div>";
                        xproductinfo += "<!-- /.info-box-content -->";
                        xproductinfo += "</div>";
                        xproductinfo += "<!-- /.info-box -->";
                        xproductinfo += "</div>";
                        xproductinfo += "</div>";

                        lbprice2.Text = xproductinfo;
                    }


                }
                else if (d.TipeProduct != null && d.IDProductSample1 == null && d.IDProductSample2 == null)
                {
                    if (d.TipeProduct == 1) // dj
                    {
                        tipeplu = "DJ";
                        var stockplu = (from p in db.StockProductDJ
                                        where p.ID == d.IDProduct && p.Draft == false && p.Approval != null
                                        select p).SingleOrDefault();

                        span_plu_added.Visible = true;
                        span_plu_added_type.Visible = true;
                        lit_plu_added.Text = stockplu.Nomor;
                        lit_plu_added_type.Text = tipeplu;
                        price_estimates.Text = Convert.ToDecimal(stockplu.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                        isPLUFound(true);

                        xplu = stockplu.Nomor;
                        xharga = Convert.ToDecimal(stockplu.StockProductDJ_PricingMU.HargaRupiahRound).ToString("N0");
                        xcurrency = "IDR";
                        ximage = (stockplu.ImgPicture != null) ? stockplu.ImgPicture : "";

                        cplu.Text = xplu;
                        
                    }
                    else if (d.TipeProduct == 2) //pg
                    {
                        tipeplu = "PG";
                        var stockplu = (from p in db.StockProductPG
                                        where p.ID == d.IDProduct && p.Draft == false && p.Approval != null
                                        select p).SingleOrDefault();

                        span_plu_added.Visible = true;
                        span_plu_added_type.Visible = true;
                        lit_plu_added.Text = stockplu.Nomor;
                        lit_plu_added_type.Text = tipeplu;
                        price_estimates.Text = "";
                        isPLUFound(true);

                        decimal GoldPrice = gp.GetPGPricing(stockplu, false);
                        if (stockplu.FixRate == true)
                        {
                            GoldPrice = stockplu.TotalHargaJual.Value;
                        }

                        xplu = stockplu.Nomor;
                        xharga = "";
                        xcurrency = "";
                        ximage = (stockplu.ImgPicture != null) ? stockplu.ImgPicture : "";

                        cplu.Text = xplu;
                    }
                    else if (d.TipeProduct == 3) // gj
                    {
                        tipeplu = "GJ";
                        var stockplu = (from p in db.StockProductGJ
                                        where p.ID == d.IDProduct && p.Draft == false && p.Approval != null
                                        select p).SingleOrDefault();

                        span_plu_added.Visible = true;
                        span_plu_added_type.Visible = true;
                        lit_plu_added.Text = stockplu.Nomor;
                        lit_plu_added_type.Text = tipeplu;
                        price_estimates.Text = Convert.ToDecimal(stockplu.StockProductGJ_PricingMU.HargaRupiahRound).ToString("N0");
                        isPLUFound(true);

                        xplu = stockplu.Nomor;
                        xharga = Convert.ToDecimal(stockplu.StockProductGJ_PricingMU.HargaRupiahRound).ToString("N0");
                        xcurrency = "IDR";
                        ximage = (stockplu.ImgPicture != null) ? stockplu.ImgPicture : "";

                        cplu.Text = xplu;
                    }
                    else if (d.TipeProduct == 4) // ld
                    {
                        tipeplu = "LD";
                        var stockplu = (from p in db.StockProductLD_Stone1B
                                        where p.ID == d.IDProduct
                                        select p).SingleOrDefault();

                        span_plu_added.Visible = true;
                        span_plu_added_type.Visible = true;
                        lit_plu_added.Text = stockplu.Nomor;
                        lit_plu_added_type.Text = tipeplu;
                        price_estimates.Text = "";
                        isPLUFound(true);

                        xplu = stockplu.Nomor;
                        xharga = Convert.ToDecimal(stockplu.HargaTotal).ToString("N0");
                        xcurrency = "USD";
                        ximage = (stockplu.ImgPicture != null) ? stockplu.ImgPicture : "";

                        cplu.Text = xplu;
                    }

                    if (tipeplu == "DJ") { xtipeplu = "Diamond Jewellery"; }
                    if (tipeplu == "PG") { xtipeplu = "Plain Gold"; }
                    if (tipeplu == "GJ") { xtipeplu = "Gold Jewellery"; }
                    if (tipeplu == "LD") { xtipeplu = "Loose Diamond"; }

                    xproductinfo = "<div class='row'>";
                    xproductinfo += "<div class='col-md-6 col-sm-6 col-xs-12'>";
                    xproductinfo += "<div class='info-box'>";
                    xproductinfo += "<span class='info-box-icon bg-silver'>";
                    if (ximage != "")
                    {
                        xproductinfo += "<img src=\"/Images.aspx?path=" + ximage + "\" />";
                    }
                    xproductinfo += "</span>";
                    xproductinfo += "<div class='info-box-content'>";
                    xproductinfo += "<span class='info-box-text'>" + xplu + "</span>";
                    xproductinfo += "<span class='info-box-number'>" + xharga + "<small class='text-muted'>&nbsp;" + xcurrency + "</small></span>";
                    xproductinfo += "<span class='info-box-descripton'>" + xtipeplu + "</span>";
                    xproductinfo += "</div>";
                    xproductinfo += "<!-- /.info-box-content -->";
                    xproductinfo += "</div>";
                    xproductinfo += "<!-- /.info-box -->";
                    xproductinfo += "</div>";
                    xproductinfo += "</div>";

                    lbprice.Text = xproductinfo;
                    lbmode.InnerText = "1";
                }



                if (d.ImgSketch != null)
                {
                    Upload.Image(imgpicture, d.ImgSketch);
                    lbmode.InnerText = "2";
                    //lbmode2.InnerText = "2";

                }

                if (d.IDDPPOReference != null)
                {
                    btn_cancel_previous_dp.Visible = true;

                    var dref = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == d.IDDPPOReference);

                    if (dref != null)
                    {
                        price_estimates.Text = d.EstimasiHarga.ToString("N0");
                        keterangan.Text = d.Keterangan;
                        transac.Text = dref.NomorDP;

                        price_estimates.Enabled = false;
                        keterangan.Enabled = false;

                        btn_cancel_plu_po1.Visible = false;
                        btn_cancel_plu_po2.Visible = false;

                        btn_cancel_plu_stock.Visible = false;

                        btn_search_cplu1.Visible = false;
                        btn_search_cplu2.Visible = false;

                        btn_search_cplu.Visible = false;

                        keterangan.Enabled = false;
                        price_estimates.Enabled = false;

                        if (dref.IDProduct == null && dref.TipeProduct == null)
                        {
                            // po
                            li_dp_stock.Visible = false;
                            li_dp_po.Visible = true;

                            //framecolour.SelectedValue = dref.IDFrameColor.ToString();

                            framecolour.Enabled = false;
                            cplu1.Enabled = false;
                            cplu2.Enabled = false;

                            tr_upload.Visible = false;

                            gwf.Enabled = false;
                            gwm.Enabled = false;

                            dwf.Enabled = false;
                            dwm.Enabled = false;

                            rsf.Enabled = false;
                            rsm.Enabled = false;

                            insf.Enabled = false;
                            insm.Enabled = false;

                            dfont.Enabled = false;
                            txdod.Enabled = false;
                        }
                        else
                        {
                            // stock
                            li_dp_stock.Visible = true;
                            li_dp_po.Visible = false;

                            cplu.Attributes.Add("disabled","");
                        }
                    }

                }

                // show valid button

            }
            string qstr = "?id=" + myID;
            search_customer.HRef += qstr;
            search_agent.HRef += qstr;

            ID = myID.ToString();
        }

    }

    //Save
    protected bool valid
    {
        get { return Fv.isDateTime(tgl, tgl_ERR); }
    }

    protected bool valid_price
    {
        get { return Fv.isDecimal(price_estimates, price_estimates_ERR); }
    }

    protected bool validCustomer
    {
        get
        {
            if (customer.Text == "")
            {
                lbvalidcustomer.Text = "Please Input Customer";
                return false;
            }
            else
                return true;
        }
    }
    protected bool validJR
    {
        get
        {
            if (agent.Text == "")
            {
                lbvalidcustomer.Text = "Please Input Sales";
                return false;
            }
            else
                return true;
        }
    }
    protected void save_Click(object sender, EventArgs e)
    {
        //	if (valid && validCustomer && valid_price)
        if (valid & valid_price && validCustomer && validJR)
        {
            var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
            if (d != null)
            {
                if (d.IDCustomer != null && d.IDSales != null)
                {
                    d.Keterangan = Str.CapFirst(keterangan.Text);

                    d.Tgl = System.DateTime.Now;

                    //while dp stock
                    if (lbmode.InnerText == "1" && cplu.Text != "")
                    {
                        if (lbtipeplu.Text != "" && cplu.Text != "")
                        {
                            if (lbtipeplu.Text == "DJ")
                            {
                                var cekpr = (from p in db.StockProductDJ where p.Nomor == cplu.Text.Trim() select p).SingleOrDefault();
                                d.IDProduct = cekpr.ID;
                                d.TipeProduct = 1;
                                db.SubmitChanges();
                            }
                            else if (lbtipeplu.Text == "PG")
                            {
                                var cekpr = (from p in db.StockProductPG where p.Nomor == cplu.Text.Trim() select p).SingleOrDefault();
                                d.IDProduct = cekpr.ID;
                                d.TipeProduct = 2;
                                db.SubmitChanges();
                            }
                            else if (lbtipeplu.Text == "GJ")
                            {
                                var cekpr = (from p in db.StockProductGJ where p.Nomor == cplu.Text.Trim() select p).SingleOrDefault();
                                d.IDProduct = cekpr.ID;
                                d.TipeProduct = 3;
                                db.SubmitChanges();
                            }
                            else if (lbtipeplu.Text == "LD")
                            {
                                var cekpr = (from p in db.StockProductLD_Stone1B where p.Nomor == cplu.Text.Trim() select p).SingleOrDefault();
                                d.IDProduct = cekpr.ID;
                                d.TipeProduct = 4;
                                db.SubmitChanges();
                            }
                        }
                    }
                    //while dp po
                    else if (lbmode.InnerText == "2")
                    {
                        // sample plu is not mandatory
                        if (cplu1.Text != "")
                        {
                            if (lbtipeplu1.Text == "DJ")
                            {
                                var cekpr = (from p in db.StockProductDJ where p.Nomor == cplu1.Text.Trim() select p).SingleOrDefault();
                                d.IDProductSample1 = cekpr.ID;
                                d.TipeProductSample1 = 1;
                                db.SubmitChanges();
                            }
                            else if (lbtipeplu1.Text == "PG")
                            {
                                var cekpr = (from p in db.StockProductPG where p.Nomor == cplu1.Text.Trim() select p).SingleOrDefault();
                                d.IDProductSample1 = cekpr.ID;
                                d.TipeProductSample1 = 2;
                                db.SubmitChanges();
                            }
                            else if (lbtipeplu1.Text == "GJ")
                            {
                                var cekpr = (from p in db.StockProductGJ where p.Nomor == cplu1.Text.Trim() select p).SingleOrDefault();
                                d.IDProductSample1 = cekpr.ID;
                                d.TipeProductSample1 = 3;
                                db.SubmitChanges();
                            }
                            else if (lbtipeplu1.Text == "LD")
                            {
                                var cekpr = (from p in db.StockProductLD_Stone1B where p.Nomor == cplu1.Text.Trim() select p).SingleOrDefault();
                                d.IDProductSample1 = cekpr.ID;
                                d.TipeProductSample1 = 4;
                                db.SubmitChanges();
                            }
                        }
                        if (cplu2.Text != "")
                        {
                            if (lbtipeplu2.Text == "DJ")
                            {
                                var cekpr = (from p in db.StockProductDJ where p.Nomor == cplu2.Text.Trim() select p).SingleOrDefault();
                                d.IDProductSample2 = cekpr.ID;
                                d.TipeProductSample2 = 1;
                                db.SubmitChanges();
                            }
                            else if (lbtipeplu2.Text == "PG")
                            {
                                var cekpr = (from p in db.StockProductPG where p.Nomor == cplu2.Text.Trim() select p).SingleOrDefault();
                                d.IDProductSample2 = cekpr.ID;
                                d.TipeProductSample2 = 2;
                                db.SubmitChanges();
                            }
                            else if (lbtipeplu2.Text == "GJ")
                            {
                                var cekpr = (from p in db.StockProductGJ where p.Nomor == cplu2.Text.Trim() select p).SingleOrDefault();
                                d.IDProductSample2 = cekpr.ID;
                                d.TipeProductSample2 = 3;
                                db.SubmitChanges();
                            }
                            else if (lbtipeplu2.Text == "LD")
                            {
                                var cekpr = (from p in db.StockProductLD_Stone1B where p.Nomor == cplu2.Text.Trim() select p).SingleOrDefault();
                                d.IDProductSample2 = cekpr.ID;
                                d.TipeProductSample2 = 4;
                                db.SubmitChanges();
                            }
                        }
                        d.IDFrameColor = Convert.ToInt32(framecolour.SelectedValue);
                        d.GoldWeight1 = Convert.ToDecimal(gwf.Text);
                        d.GoldWeight2 = Convert.ToDecimal(gwm.Text);
                        d.DiamondWeight1 = Convert.ToDecimal(dwf.Text);
                        d.DiamondWeight2 = Convert.ToDecimal(dwm.Text);
                        d.RingSize1 = Convert.ToDecimal(rsf.Text);
                        d.RingSize2 = Convert.ToDecimal(rsm.Text);
                        d.Inscription1 = insf.Text;
                        d.Inscription2 = insm.Text;
                        d.IDFontType = Convert.ToInt32(dfont.SelectedValue);
                        d.DeliveryDate = Convert.ToDateTime(txdod.Text);
                        db.SubmitChanges();
                        // for sketch
                        //upload image sketch
                        //var img = 
                    }

                    //d.NomorDP = "";
                    d.EstimasiHarga = Convert.ToDecimal(price_estimates.Text);

                    db.SubmitChanges();
                }
                else
                {
                    if (customer.Text == "" && d.IDCustomer == null)
                    {
                        lbvalidcustomer.Text = "Please Input Customer";
                    }
                    if (agent.Text == "" && d.IDSales == null)
                    {
                        lbvalidcustomer.Text = "Please Input Sales";
                    }
                }
            }

            Response.Redirect("PaymentDP_ItemList.aspx?id=" + myID);
        }
    }

    protected void removeDataDPREF(SalesReceiptDPPO rs)
    {
        rs.IDDPPOReference = null;
        db.SubmitChanges();
    }

    protected void removeDataST(SalesReceiptDPPO rs)
    {
        rs.TipeProduct = null;
        rs.IDProduct = null;
        rs.IDDPPOReference = null;
        db.SubmitChanges();
    }

    protected void removeDataPO(SalesReceiptDPPO rs)
    {
        rs.EstimasiHarga = Convert.ToDecimal(price_estimates.Text);
        rs.Keterangan = keterangan.Text;
        rs.IDProductSample1 = null;
        rs.IDProductSample2 = null;
        rs.IDFrameColor = null;
        rs.GoldWeight1 = null;
        rs.GoldWeight2 = null;
        rs.DiamondWeight1 = null;
        rs.DiamondWeight2 = null;
        rs.RingSize1 = null;
        rs.RingSize2 = null;
        rs.Inscription1 = null;
        rs.Inscription2 = null;
        rs.IDFontType = null;
        rs.DeliveryDate = null;
        rs.ImgSketch = null;
        rs.TipeProductSample1 = null;
        rs.TipeProductSample2 = null;
        db.SubmitChanges();
    }

    protected void btn_search_cplu_Click(object sender, EventArgs e)
    {
        lbprice.Text = "";

        if (status == 1)
        {
            var rs = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
            removeDataPO(rs);

            string plu = cplu.Text.Trim();

            bool dj = cek_DJ(plu);
            bool pg = cek_PG(plu);
            bool gj = cek_GJ(plu);
            bool ld = cek_LD(plu);

            if (dj == false && pg == false && gj == false && ld == false)
            {
                rs.TipeProduct = null;
                rs.IDProduct = null;
                db.SubmitChanges();

                isPLUFound(false);
            }
            else
            {

                rs.Keterangan = keterangan.Text.Trim();

                if (dj == true)
                {
                    lbprice.Text = plu + "<br />" + "DJ" + "<br />" + "IDR " + price_dj.ToString("N0");
                    lbtipeplu.Text = "DJ";

                    var cekpr = db.StockProductDJ.SingleOrDefault(p => p.Nomor == plu);
                    if (cekpr != null)
                    {
                        rs.IDProduct = cekpr.ID;
                        rs.TipeProduct = 1;
                        db.SubmitChanges();
                    }
                }
                else if (pg == true)
                {
                    lbprice.Text = plu + "<br />" + "PG" + "<br />" + "available";
                    lbtipeplu.Text = "PG";

                    var cekpr = db.StockProductPG.SingleOrDefault(p => p.Nomor == plu);
                    if (cekpr != null)
                    {
                        rs.IDProduct = cekpr.ID;
                        rs.TipeProduct = 2;
                        db.SubmitChanges();
                    }
                }
                else if (gj == true)
                {
                    lbprice.Text = plu + "<br />" + "GJ" + "<br />" + "IDR " + price_gj.ToString("N0");
                    lbtipeplu.Text = "GJ";

                    var cekpr = db.StockProductGJ.SingleOrDefault(p => p.Nomor == plu);
                    if (cekpr != null)
                    {
                        rs.IDProduct = cekpr.ID;
                        rs.TipeProduct = 3;
                        db.SubmitChanges();
                    }
                }
                else if (ld == true)
                {
                    lbprice.Text = plu + "<br />" + "LD" + "<br />" + "USD " + price_ld.ToString("N2");
                    lbtipeplu.Text = "LD";

                    var cekpr = db.StockProductLD_Stone1B.SingleOrDefault(p => p.Nomor == plu);
                    if (cekpr != null)
                    {
                        rs.IDProduct = cekpr.ID;
                        rs.TipeProduct = 4;
                        db.SubmitChanges();
                    }
                }
                
            }
            lbmode.InnerText = "1";
            //((TextBox)cplu).Focus();
            ((LinkButton)sender).Focus();
            //cplu.Enabled = false;
            //btn_search_cplu.Visible = false;
            //btn_clear_cplu.Visible = true;

            fillForm();
        }
        Response.Redirect("PaymentDP_Form.aspx?id=" + myID + "&status=1");
    }



    protected bool cek_DJ(string plu)
    {
        var cek = (from p in db.SalesReceiptDPPO where p.ID == myID select p).SingleOrDefault();
        var rs = (from p in db.StockProductDJ
                  where p.Draft == false && p.StatusPenjualan == 0
                  && p.ReturnProduct == false
                  && p.StockActualDJ.IDLokasi == cek.IDLokasi
                  && p.StockActualDJ.TipeLokasi == cek.TipeLokasi
                  && p.StockActualDJ.Substorage == 1
                  && p.Nomor == plu
                  && !(from z in db.SalesOrderDJ
                       where z.SalesOrder.StatusVoid == false
                       select z.IDProduct).Contains(p.ID)
                  && !(from c in db.SalesReceiptDPPO
                       where c.StatusDP != 2 && c.TipeProduct == 1 && !c.Draft.Value
                       select c.IDProduct).Contains(p.ID)                        
                  select p).SingleOrDefault();

        if (rs != null)
        {
            price_dj = rs.StockProductDJ_PricingMU.HargaRupiahRound.Value;
            //price_dj1 = 0;
            //price_dj2 = 0;

            //price_pg = 0;
            //price_pg1 = 0;
            //price_pg2 = 0;

            //price_gj = 0;
            //price_gj1 = 0;
            //price_gj2 = 0;

            //price_ld = 0;
            //price_ld1 = 0;
            //price_ld2 = 0;
            return true;
        }
        else
        { return false; }
    }
    protected bool cek_PG(string plu)
    {
        var cek = (from p in db.SalesReceiptDPPO where p.ID == myID select p).SingleOrDefault();
        var rs = (from p in db.StockProductPG
                  where p.Draft == false && p.StatusPenjualan == 0
                     && p.StockActualPG.TipeLokasi == cek.TipeLokasi
                     && p.StockActualPG.IDLokasi == cek.IDLokasi
                     && p.Nomor == plu
                    && p.StockActualPG.Substorage == 1
                    && !(from z in db.SalesOrderPG
                         where z.SalesOrder.StatusVoid == false
                         select z.IDProduct).Contains(p.ID)
                    && !(from c in db.SalesReceiptDPPO
                         where c.StatusDP != 2 && c.TipeProduct == 2 && !c.Draft.Value
                         select c.IDProduct).Contains(p.ID)
                  select p).SingleOrDefault();


        if (rs != null)
        {
            decimal GoldPrice = gp.GetPGPricing(rs, false);
            if (rs.FixRate == true)
            {
                GoldPrice = rs.TotalHargaJual.Value;
            }
            price_pg = GoldPrice;
            //price_pg1 = 0;
            //price_pg2 = 0;

            //price_dj = 0;
            //price_dj1 = 0;
            //price_dj2 = 0;

            //price_gj = 0;
            //price_gj1 = 0;
            //price_gj2 = 0;

            //price_ld = 0;
            //price_ld1 = 0;
            //price_ld2 = 0;
            return true;
        }
        else
        { return false; }
    }
    protected bool cek_GJ(string plu)
    {
        var cek = (from p in db.SalesReceiptDPPO where p.ID == myID select p).SingleOrDefault();
        var rs = (from p in db.StockProductGJ
                  where p.Draft == false && p.StatusPenjualan == 0
                  && p.ReturnProduct == false
                  && p.StockActualGJ.IDLokasi == cek.IDLokasi
                  && p.StockActualGJ.TipeLokasi == cek.TipeLokasi
                  && p.StockActualGJ.Substorage == 1
                 && p.Nomor == plu
                 && !(from z in db.SalesOrderGJ
                      where z.SalesOrder.StatusVoid == false
                      select z.IDProduct).Contains(p.ID)
                 && !(from c in db.SalesReceiptDPPO
                      where c.StatusDP != 2 && c.TipeProduct == 3 && !c.Draft.Value
                      select c.IDProduct).Contains(p.ID)
                  select p).SingleOrDefault();

        if (rs != null)
        {
            price_gj = rs.StockProductGJ_PricingMU.HargaRupiahRound.Value;
            //price_gj1 = 0;
            //price_gj2 = 0;

            //price_dj = 0;
            //price_dj1 = 0;
            //price_dj2 = 0;

            //price_pg = 0;
            //price_pg1 = 0;
            //price_pg2 = 0;

            //price_ld = 0;
            //price_ld1 = 0;
            //price_ld2 = 0;
            return true;
        }
        else
        { return false; }
    }
    protected bool cek_LD(string plu)
    {
        var cek = (from p in db.SalesReceiptDPPO where p.ID == myID select p).SingleOrDefault();
        var rs = (from p in db.StockProductLD_Stone1B
                  where p.StatusPenjualan == 0
                  && p.ReturnProduct == false
                  && p.StockActualLD_Stone1B.IDLokasi == cek.IDLokasi
                  && p.StockActualLD_Stone1B.TipeLokasi == cek.TipeLokasi
                  && p.Nomor == plu
                 && p.StockActualLD_Stone1B.Substorage == 1
                 && !(from z in db.SalesOrderLD
                      where z.SalesOrder.StatusVoid == false
                      select z.IDProduct).Contains(p.ID)
                 && !(from c in db.SalesReceiptDPPO
                      where c.StatusDP != 2 && c.TipeProduct == 4 && !c.Draft.Value
                      select c.IDProduct).Contains(p.ID)
                  select p).SingleOrDefault();

        if (rs != null)
        {
            price_ld = rs.HargaTotal.Value;
            //price_ld1 = 0;
            //price_ld2 = 0;

            //price_dj = 0;
            //price_dj1 = 0;
            //price_dj2 = 0;

            //price_pg = 0;
            //price_pg1 = 0;
            //price_pg2 = 0;

            //price_gj = 0;
            //price_gj1 = 0;
            //price_gj2 = 0;
            return true;
        }
        else
        { return false; }
    }

    //btn_cancel_previous_dp_Click
    protected void btn_cancel_previous_dp_Click(object sender, EventArgs e)
    {
        var rs = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
        removeDataPO(rs);
        removeDataST(rs);
        removeDataDPREF(rs);

        Response.Redirect("PaymentDP_Form.aspx?id=" + myID + "&status=1");
    }

    //btn_cancel_plu_stock_Click
    protected void btn_cancel_plu_stock_Click(object sender, EventArgs e)
    {
        var rs = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
        removeDataPO(rs);
        removeDataST(rs);

        Response.Redirect("PaymentDP_Form.aspx?id=" + myID + "&status=1");
    }

    protected void btn_cancel_plu_po1_Click(object sender, EventArgs e)
    {
        var rs = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
        removeDataST(rs);

        rs.TipeProductSample1 = null;
        rs.IDProductSample1 = null;
        db.SubmitChanges();

        Response.Redirect("PaymentDP_Form.aspx?id=" + myID + "&status=1" + "&dp=2");
    }

    protected void btn_cancel_plu_po2_Click(object sender, EventArgs e)
    {
        var rs = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
        removeDataST(rs);

        rs.TipeProductSample2 = null;
        rs.IDProductSample2 = null;
        db.SubmitChanges();

        Response.Redirect("PaymentDP_Form.aspx?id=" + myID + "&status=1" + "&dp=2");
    }

    protected void btn_search_cplu1_Click(object sender, EventArgs e)
    {
        if (status == 1)
        {
            var rs = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
            rs.EstimasiHarga = Convert.ToDecimal(price_estimates.Text);
            rs.Keterangan = Str.CapUpper(keterangan.Text.Trim());
            rs.IDFrameColor = Convert.ToInt32(framecolour.SelectedValue);
            rs.GoldWeight1 = Convert.ToDecimal(gwf.Text);
            rs.GoldWeight2 = Convert.ToDecimal(gwm.Text);
            rs.DiamondWeight1 = Convert.ToDecimal(dwf.Text);
            rs.DiamondWeight2 = Convert.ToDecimal(dwm.Text);
            rs.RingSize1 = Convert.ToDecimal(rsf.Text);
            rs.RingSize2 = Convert.ToDecimal(rsm.Text);
            rs.Inscription1 = Str.CapUpper(insf.Text.Trim());
            rs.Inscription2 = Str.CapUpper(insm.Text.Trim());
            rs.IDFontType = Convert.ToInt32(dfont.SelectedValue);
            db.SubmitChanges();

            string plu1 = cplu1.Text.Trim();
            int cekdj = check_DJ(plu1, 1);
            int cekpg = check_PG(plu1, 1);
            int cekgj = check_GJ(plu1, 1);
            int cekld = check_LD(plu1, 1);

            //save2.Disabled = true;

            if (cekdj == 0 && cekpg == 0 && cekgj == 0 && cekld == 0)
            {
                lbprice1.Text = "PLU not found";
            }
            else
            {
                if (cekdj > 0)
                {
                    lbprice1.Text = plu1 + "<br />" + "DJ" + "<br />" + "IDR " + price_dj1.ToString("N0");
                    lbtipeplu1.Text = "DJ";
                    //framecolour.SelectedValue = fc1.ToString();

                    rs.TipeProductSample1 = 1;
                    rs.IDProductSample1 = cekdj;
                }
                else if (cekpg > 0)
                {
                    //lbprice1.Text = "IDR " + price_pg1.ToString("N0");
                    lbtipeplu1.Text = "PG";
                    lbprice1.Text = plu1 + "<br />" + "PG" + "<br />" + "Check Price !";
                    framecolour.SelectedValue = fc1.ToString();

                    rs.TipeProductSample1 = 2;
                    rs.IDProductSample1 = cekpg;
                }
                else if (cekgj > 0)
                {
                    lbprice1.Text = plu1 + "<br />" + "GJ" + "<br />" + "IDR " + price_gj1.ToString("N0");
                    lbtipeplu1.Text = "GJ";
                    framecolour.SelectedValue = fc1.ToString();

                    rs.TipeProductSample1 = 3;
                    rs.IDProductSample1 = cekgj;
                }
                else if (cekld > 0)
                {
                    lbprice1.Text = plu1 + "<br />" + "LD" + "<br />" + "USD " + price_ld1.ToString("N2");
                    lbtipeplu1.Text = "LD";

                    rs.TipeProductSample1 = 4;
                    rs.IDProductSample1 = cekld;
                }

                db.SubmitChanges();
                //save2.Disabled = false;
            }

            lbmode.InnerText = "2";
            ((LinkButton)sender).Focus();
        }
        Response.Redirect("PaymentDP_Form.aspx?id=" + myID + "&status=1" + "&dp=2");
    }

    protected void btn_search_cplu2_Click(object sender, EventArgs e)
    {
        if (status == 1)
        {
            var rs = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID);
            rs.EstimasiHarga = Convert.ToDecimal(price_estimates.Text);
            rs.Keterangan = Str.CapUpper(keterangan.Text.Trim());
            rs.IDFrameColor = Convert.ToInt32(framecolour.SelectedValue);
            rs.GoldWeight1 = Convert.ToDecimal(gwf.Text);
            rs.GoldWeight2 = Convert.ToDecimal(gwm.Text);
            rs.DiamondWeight1 = Convert.ToDecimal(dwf.Text);
            rs.DiamondWeight2 = Convert.ToDecimal(dwm.Text);
            rs.RingSize1 = Convert.ToDecimal(rsf.Text);
            rs.RingSize2 = Convert.ToDecimal(rsm.Text);
            rs.Inscription1 = Str.CapUpper(insf.Text.Trim());
            rs.Inscription2 = Str.CapUpper(insm.Text.Trim());
            rs.IDFontType = Convert.ToInt32(dfont.SelectedValue);
            db.SubmitChanges();

            string plu1 = cplu2.Text.Trim();
            int cekdj = check_DJ(plu1, 1);
            int cekpg = check_PG(plu1, 1);
            int cekgj = check_GJ(plu1, 1);
            int cekld = check_LD(plu1, 1);

            //save2.Disabled = true;

            if (cekdj == 0 && cekpg == 0 && cekgj == 0 && cekld == 0)
            {
                lbprice2.Text = "PLU not found";
            }
            else
            {
                if (cekdj > 0)
                {
                    lbprice2.Text = plu1 + "<br />" + "DJ" + "<br />" + "IDR " + price_dj1.ToString("N0");
                    lbtipeplu2.Text = "DJ";
                    //framecolour.SelectedValue = fc1.ToString();

                    rs.TipeProductSample2 = 1;
                    rs.IDProductSample2 = cekdj;
                }
                else if (cekpg > 0)
                {
                    //lbprice2.Text = "IDR " + price_pg1.ToString("N0");
                    lbtipeplu2.Text = "PG";
                    lbprice2.Text = plu1 + "<br />" + "PG" + "<br />" + "Check Price !";
                    framecolour.SelectedValue = fc1.ToString();

                    rs.TipeProductSample2 = 2;
                    rs.IDProductSample2 = cekpg;
                }
                else if (cekgj > 0)
                {
                    lbprice2.Text = plu1 + "<br />" + "GJ" + "<br />" + "IDR " + price_gj1.ToString("N0");
                    lbtipeplu2.Text = "GJ";
                    framecolour.SelectedValue = fc1.ToString();

                    rs.TipeProductSample2 = 3;
                    rs.IDProductSample2 = cekgj;
                }
                else if (cekld > 0)
                {
                    lbprice2.Text = plu1 + "<br />" + "LD" + "<br />" + "USD " + price_ld1.ToString("N2");
                    lbtipeplu2.Text = "LD";

                    rs.TipeProductSample2 = 4;
                    rs.IDProductSample2 = cekld;
                }

                db.SubmitChanges();
                //save2.Disabled = false;
            }

            lbmode.InnerText = "2";
            ((LinkButton)sender).Focus();
        }
        Response.Redirect("PaymentDP_Form.aspx?id=" + myID + "&status=1" + "&dp=2");
    }

    protected int check_DJ(string plu, int sequence)
    {
        int result = 0;
        var rs = (from p in db.StockProductDJ
                  where p.Approval != null
                     && p.Draft == false
                     && p.Nomor == plu
                  select p).SingleOrDefault();

        if (rs != null)
        {
            if (sequence == 1)
            {
                price_dj1 = rs.StockProductDJ_PricingMU.HargaRupiahRound.Value;
                fc1 = Convert.ToInt32(rs.StockProductDJ_CharProduct.FrameColor.ToString());
            }
            else { price_dj2 = rs.StockProductDJ_PricingMU.HargaRupiahRound.Value; fc2 = Convert.ToInt32(rs.StockProductDJ_CharProduct.FrameColor.ToString()); }
            
            result = rs.ID;
        }

        return result;
    }

    protected int check_PG(string plu, int sequence)
    {
        int result = 0;
        var rs = (from p in db.StockProductPG
                  where p.Draft == false
                  && p.Approval != null
                  && p.Nomor == plu
                  select p).SingleOrDefault();

        if (rs != null)
        {
            decimal GoldPrice = gp.GetPGPricing(rs, false);
            if (rs.FixRate == true)
            {
                GoldPrice = rs.TotalHargaJual.Value;
            }
            if (sequence == 1) { price_pg1 = GoldPrice; fc1 = Convert.ToInt32(rs.FrameColor.ToString()); }
            else { price_pg2 = GoldPrice; fc2 = Convert.ToInt32(rs.FrameColor.ToString()); }

            result = rs.ID;
        }

        return result;
    }

    protected int check_GJ(string plu, int sequence)
    {
        int result = 0;
        var rs = (from p in db.StockProductGJ
                  where p.Approval != null
                     && p.Draft == false
                     && p.Nomor == plu
                  select p).SingleOrDefault();

        if (rs != null)
        {
            if (sequence == 1)
            {
                price_gj1 = rs.StockProductGJ_PricingMU.HargaRupiahRound.Value;
                fc1 = Convert.ToInt32(rs.StockProductGJ_CharProduct.FrameColor.ToString());
            }
            else { price_gj2 = rs.StockProductGJ_PricingMU.HargaRupiahRound.Value; fc2 = Convert.ToInt32(rs.StockProductGJ_CharProduct.FrameColor.ToString()); }

            result = rs.ID;
        }

        return result;
    }

    protected int check_LD(string plu, int sequence)
    {
        int result = 0;
        var rs = (from p in db.StockProductLD_Stone1B
                  where p.Nomor == plu
                  select p).SingleOrDefault();

        if (rs != null)
        {
            if (sequence == 1) { price_ld1 = rs.HargaTotal.Value; }
            else { price_ld2 = rs.HargaTotal.Value; }

            result = rs.ID;
        }

        return result;
    }

    protected bool finishUpload(SalesReceiptDPPO d)
    {
        bool x = true;

        x = x && saveImage(d, ske_up, ske_ERR, "Picture");

        return x;
    }

    //Process
    protected bool saveImage(SalesReceiptDPPO d, FileUpload file, HtmlGenericControl lb, string name)
    {
        if (!Upload.HasFile(file))
        {
            //Commit
            d.ImgSketch = null;
            db.SubmitChanges();

            return true;
        }
        else
        {
            if (!Upload.FormatValid(file, lb, Upload.ImageFormat)) return false;
            else
            {
                string path = string.Empty;

                path += App.ImagesDir;
                path += "DPPO" + "\\";

                if (Upload.CreateDir(path))
                {
                    path += name + "_";
                    path += myID + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + Upload.GetExt(file.PostedFile.FileName);

                    //Upload
                    file.PostedFile.SaveAs(path);

                    //Commit
                    d.ImgSketch = path;
                    db.SubmitChanges();

                    return true;
                }
                else
                {
                    dir_ERR.InnerHtml += path;
                    dir_ERR.Visible = true;

                    return false;
                }
            }
        }
    }

    protected void uploadsketch_click(object sender, EventArgs e)
    {
        var d = db.SalesReceiptDPPO.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
        if (d == null) DataError();
        else
        {
            d.EstimasiHarga = Convert.ToDecimal(price_estimates.Text);
            d.Keterangan = Str.CapUpper(keterangan.Text.Trim());
            d.IDFrameColor = Convert.ToInt32(framecolour.SelectedValue);
            d.GoldWeight1 = Convert.ToDecimal(gwf.Text);
            d.GoldWeight2 = Convert.ToDecimal(gwm.Text);
            d.DiamondWeight1 = Convert.ToDecimal(dwf.Text);
            d.DiamondWeight2 = Convert.ToDecimal(dwm.Text);
            d.RingSize1 = Convert.ToDecimal(rsf.Text);
            d.RingSize2 = Convert.ToDecimal(rsm.Text);
            d.Inscription1 = Str.CapUpper(insf.Text.Trim());
            d.Inscription2 = Str.CapUpper(insm.Text.Trim());
            d.IDFontType = Convert.ToInt32(dfont.SelectedValue);

            bool finish = finishUpload(d);
            db.SubmitChanges();
            if (finish)
            {
                string href = App.RefreshURL(this);

                //Customize-Qstr
                href += "&id=" + myID;
                href += "&status=1";
                href += "&dp=2";
                Response.Redirect(href);
            }
        }

    }
    [WebMethod]
    public void UploadSketch(string imageData)
    {

        string path = string.Empty;

        path += App.ImagesDir;
        path += "DPPO" + "\\";

        if (Upload.CreateDir(path))
        {
            path += "Sketch_";
            path += myID + "-" + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".png";
            //    Upload
            //imageData.PostedFile.SaveAs(path);

            string fileNameWitPath = path;
            using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageData);
                    bw.Write(data);
                    bw.Close();
                }
            }

            File.WriteAllBytes(path, Convert.FromBase64String(imageData));
        }
        else
        {
            path += "Sketch_";
            path += myID + "-" + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".png";
            File.WriteAllBytes(path, Convert.FromBase64String(imageData));

        }
        //imgdata.ImageUrl = imageData;
        //lbdataimage.Text = imageData;
        //Response.Write("<script>");
        //Response.Write("alert('tes 123')");
        //Response.Write("</script>");

        //string href = App.RefreshURL(this);
        //href += "&id=" + myID;
        //href += "&status=1&tesimage=" + imageData;
        //Response.Redirect(href);
    }
    protected void canvas_upload_Click(object sender, EventArgs e)
    {
        string path = string.Empty;
        path += App.ImagesDir;
        path += "DPPO" + "\\";

        //string imageData = lbdataimage.Text;
        string imageData = lbdataimage.Text;
        //string imageData = Convert.ToBase64String(lbdataimage);

        if (Upload.CreateDir(path))
        {
            path += "Sketch_";
            path += myID + "-" + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".png";

            //File.WriteAllBytes(path, Convert.FromBase64String(imageData));

            string fileNameWitPath = path;
            using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageData);
                    bw.Write(data);
                    bw.Close();
                }
            }
        }
        else
        {
            path += "Sketch_";
            path += myID + "-" + DateTime.Now.ToString("ddMMyyyy-HHmmss") + ".png";

            //File.WriteAllBytes(path, Convert.FromBase64String(imageData));

            string fileNameWitPath = path;
            using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageData);
                    bw.Write(data);
                    bw.Close();
                }
            }
        }
    }

    protected void btn_clear_cplu_Click(object sender, EventArgs e)
    {
        cplu.Text = "";
        cplu.Enabled = true;
        btn_search_cplu.Visible = true;
        //btn_clear_cplu.Visible = false;
    }

    protected void btn_clear_cplu1_Click(object sender, EventArgs e)
    {
        cplu1.Text = "";
        cplu1.Enabled = true;
        btn_search_cplu1.Visible = true;
        //btn_clear_cplu1.Visible = false;
    }

    protected void btn_clear_cplu2_Click(object sender, EventArgs e)
    {
        cplu2.Text = "";
        cplu2.Enabled = true;
        btn_search_cplu2.Visible = true;
        //btn_clear_cplu2.Visible = false;
    }

    protected void isPLUFound(bool status)
    {
        lit_status_found.Text = "<span class='glyphicon glyphicon-" + (status ? "ok" : "remove") + "' style='color:" + (status ? "green" : "red") + "; font-size:20px;' aria-hidden='true'></span>";
    
    }
	protected void setLetterWR(object sender, EventArgs e)
    {
        var srdp = db.SalesReceiptDPPO.SingleOrDefault(r => r.ID == myID);
		//string message = "Jancuk";
        if (srdp != null)
        {
            int lettercount = Db.SingleInteger("SELECT COUNT('') Total FROM LetterWR WHERE IDSalesReceiptDPPO = '" + myID.ToString() + "' ", Db.Cnn_DB);
            string letterupdate = "";
            if (lettercount == 1)
            {
                letterupdate = Db.SingleString("SELECT TOP 1 ISNULL(Letter,'') FROM LetterWR WHERE IDSalesReceiptDPPO = '" + myID.ToString() + "' ", Db.Cnn_DB);
                Db.Execute("UPDATE LetterWR SET Letter = '" + txtletterwr.Text + "' WHERE IDSalesReceiptDPPO = '" + myID + "' ", Db.Cnn_DB);
            }
            else
            {
                Db.Execute("INSERT INTO LetterWR(IDSalesReceiptDPPO, Letter) VALUES('" + myID.ToString() + "','" + txtletterwr.Text + "') ", Db.Cnn_DB);
            }
        }
        txtletterwr.Text = Db.SingleString("SELECT TOP 1 ISNULL(Letter,'') FROM LetterWR WHERE IDSalesReceiptDPPO = '" + myID + "' ", Db.Cnn_DB);
		//ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + message + "');", true);
    }
	protected void FontType(DropDownList c)
    {
        using (Map_DB db = new Map_DB())
        {
        var rs = from p in db.FontType orderby p.Nama
                 where p.ID == 8 || p.ID == 9 || p.ID == 10 || p.ID == 11 || p.ID == 12
                 select p;
        foreach (var r in rs)
        {
            string t = r.Nama;
            string v = r.ID.ToString();
            c.Items.Add(new ListItem(t, v));
        }
        }
    }
}