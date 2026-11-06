using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_PG : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
        App.Protect();
        App.ProtectRole(Role.Operator);

        string CB = ""; /// mondial, franknco, missmondial, thepalace
        CB = System.Configuration.ConfigurationManager.AppSettings["LicensedTo"];

      
       
       

        if (!IsPostBack)
        {
            DLC.CharDesignCategory(designcategory);
            DLC.CharDesignConcept(designconcept);
            DLC.CharDesignProcess(designprocess);
            DLC.CharTargetAge(targetage);
            DLC.CharTargetGender(targetgender);
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);
            DLC.CharStoneDist(stonedistribution);
            DLC.CharFrameMaterial(framematerial);
            DLC.CharFrameColor(framecolor);
            DLC.CharGoldLevel(goldlevel);
            DLC.CharGoldModelAll2(goldmodel);
            DLC.DDLSupplier(supplier);
            DLC.Parcel106(stonebrand);

            if (CB.ToLower().Contains("THE PALACE".ToLower()))
            {
               
                ratetype.SelectedValue = "0";
                LoadFixRated(ratetype, new EventArgs());

            }

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
       
    }

 


    protected void LoadFixRated(object sender, EventArgs e)
    {
        ((RadioButtonList)sender).Focus();
        if (ratetype.SelectedValue == "0")
        {
            // divDari.Visible = false;
            // divSampai.Visible = false;
            dari.ReadOnly = true;
            sampai.ReadOnly = true;
            ratetype.SelectedValue = "0";
            dari.Text = string.Empty;
            sampai.Text = string.Empty;
            // ratetype.SelectedIndex = 0;
            // ratetype.SelectedIndex = 0;
            //DLC.CharGoldModel(ddl_Model);
            //((DataControlField)EmasGridView.Columns
            //.Cast<DataControlField>()
            //.Where(fld => fld.HeaderText == "Total Harga Jual")
            //.SingleOrDefault()).Visible = false;

        }
        else
        {
            
            dari.ReadOnly = false;
            sampai.ReadOnly = false;
           // dari.Text = string.Empty;
          //  sampai.Text = string.Empty;
            //divDari.Visible = true;
            //divSampai.Visible = true;
            ratetype.SelectedValue = "1";
            //  ratetype.SelectedIndex = 1;
            //  ratetype.SelectedIndex = 1;
            //DLC.CharGoldModelGJ(ddl_Model);
            //((DataControlField)EmasGridView.Columns
            //.Cast<DataControlField>()
            //.Where(fld => fld.HeaderText == "Total Harga Jual")
            //.SingleOrDefault()).Visible = true;
        }


        //CalcHarga();
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {

       
        string xkw = kw.Text.Trim();
        /////////////////////////////////////////////
        decimal xDari = 0m;
        try
        {
            xDari = Convert.ToDecimal(dari.Text.Trim());
        }
        catch (Exception ex) { }
        decimal xSampai = 0m;
        try
        {
            xSampai = Convert.ToDecimal(sampai.Text.Trim());
        }
        catch (Exception ex) { }
        /////////////////////////////////////////////
        decimal xDariBerat = 0m;
        try
        {
            xDariBerat = decimal.Parse(dariBerat.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture); // Convert.ToDecimal("9.5");
        }
        catch (Exception ex) { }
        decimal xSampaiBerat = 0m;
        try
        {
            xSampaiBerat = decimal.Parse(sampaiBerat.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture);  //Convert.ToDecimal(sampaiBerat.Text.Trim());
        }
        catch (Exception ex) { }

        /////////////////////////////////////////////
        decimal xDariUkuran = 0m;
        try
        {
            xDariUkuran = decimal.Parse(dariUkuran.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture); // Convert.ToDecimal("9.5");
        }
        catch (Exception ex) { }
        decimal xSampaiUkuran = 0m;
        try
        {
            xSampaiUkuran = decimal.Parse(sampaiUkuran.Text.Trim(), System.Globalization.CultureInfo.InvariantCulture);  //Convert.ToDecimal(sampaiBerat.Text.Trim());
        }
        catch (Exception ex) { }


        /////////////////////////////////////////////
        int xBasic = 0;
        try
        {
            xBasic = Convert.ToInt32(statusbasic.SelectedValue);
        }
        catch (Exception ex) { }
        //stockstatus
        int xStockstatus = 0;
        try
        {
            xStockstatus = Convert.ToInt32(stockstatus.SelectedValue);
        }
        catch (Exception ex) { }
        /////////////////////////////////////////////
        int xProductItem = -1;
        try
        {
            xProductItem = Convert.ToInt32(productitem.SelectedValue);
        }
        catch (Exception ex) { }
        int xProductCategory = -1;
        try
        {
            xProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        }
        catch (Exception ex) { }
        int xProductLevel = -1;
        try
        {
            xProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        }
        catch (Exception ex) { }
        int xStoneDistribution = -1;
        try
        {
            xStoneDistribution = Convert.ToInt32(stonedistribution.SelectedValue);
        }
        catch (Exception ex) { }
        int xDesignCategory = -1;
        try
        {
            xDesignCategory = Convert.ToInt32(designcategory.SelectedValue);
        }
        catch (Exception ex) { }
        int xDesignConcept = -1;
        try
        {
            xDesignConcept = Convert.ToInt32(designconcept.SelectedValue);
        }
        catch (Exception ex) { }
        int xDesignProcess = -1;
        try
        {
            xDesignProcess = Convert.ToInt32(designprocess.SelectedValue);
        }
        catch (Exception ex) { }
        int xTargetAge = -1;
        try
        {
            xTargetAge = Convert.ToInt32(targetage.SelectedValue);
        }
        catch (Exception ex) { }
        int xTargetGender = -1;
        try
        {
            xTargetGender = Convert.ToInt32(targetgender.SelectedValue);
        }
        catch (Exception ex) { }
        int xFrameMaterial = -1;
        try
        {
            xFrameMaterial = Convert.ToInt32(framematerial.SelectedValue);
        }
        catch (Exception ex) { }
        int xFrameColor = -1;
        try
        {
            xFrameColor = Convert.ToInt32(framecolor.SelectedValue);
        }
        catch (Exception ex) { }
        int xGoldLevel = -1;
        try
        {
            xGoldLevel = Convert.ToInt32(goldlevel.SelectedValue);
        }
        catch (Exception ex) { }
        /////////////////////////////////////////////
        int xStoneBrand = -1;
        try
        {
            xStoneBrand = Convert.ToInt32(stonebrand.SelectedValue);
        }
        catch (Exception ex) { }
        /////////////////////////////////////////////
        int xGoldModel = -1;
        try
        {
            xGoldModel = Convert.ToInt32(goldmodel.SelectedValue);
        }
        catch (Exception ex) { }
        /////////////////////////////////////////////
        int xSupplier = -1;
        try
        {
            xSupplier = Convert.ToInt32(supplier.SelectedValue);
        }
        catch (Exception ex) { }

        int pageNumber = 1;
        int numberOfObjectsPerPage = 20;

        if (Request.QueryString["page"] != "")
            pageNumber = Convert.ToInt32(Request.QueryString["page"]);
        if (pageNumber < 1)
            pageNumber = 1;

        decimal TotalPage = 0m;// Convert.ToDecimal(rslt.Count()) / Convert.ToDecimal(numberOfObjectsPerPage);

        string xrate = "1";
        xrate = ratetype.SelectedValue.ToString();
        LoadFixRated(ratetype, new EventArgs() );
        
      

        //TotalPage = Math.Ceiling(TotalPage);

        //rslt = rslt.Skip(numberOfObjectsPerPage * (pageNumber-1)).Take(numberOfObjectsPerPage);

        string xproduct = "";

        //if (rslt != null)
        //{
        //    decimal xQty = 0m;

        //    int prev = 0;
        //    int index = 1;
        //    foreach (var r in rslt)
        //    {
        //        xQty = 0m;

        //        //decimal xQty = (from p in r where p.StatusPenjualan == 0 && p.StatusReturn == false && p.StatusLebur == false select p.Qty).Sum();
        //        try
        //        {
        //            xQty = (qtym != null) ? (qtym.SingleOrDefault(p => p.ProtoNomor == r.Key).QtyM) : 0m;
        //        }
        //        catch (Exception ex) { }

        //        xproduct += "<div class='col-xs-6 col-sm-4 col-md-3' style='text-align:center;'>";
        //        xproduct += "<a class='aproduct' href='DJ_View.aspx?proto=" + r.First().ProtoNomor + "&min=" + Dari + "&max=" + Sampai + " '>";
        //        xproduct += "<div style='padding:15px; margin:5px;'>";
        //        xproduct += "<div style='height:150px; display:block !important;' class='img-thumbnail'><img style='max-height:140px; margin:0 auto;' src=\"/Images.aspx?path=" + r.OrderByDescending(p => p.HargaRupiahRound).First().ImgPicture + "\" class='img-responsive' /></div>";
        //        xproduct += "<div style='height:30px;' >";
        //        xproduct += r.First().ProtoNomor;
        //        xproduct += "</div>";
        //        xproduct += "<div style='height:30px;' > Qty : " + Str.Number0((xQty != null ? xQty : 0m)) + "</div>";
        //        xproduct += "</div>";
        //        xproduct += "</a>";
        //        xproduct += "</div>";
                
        //    }
        //}

        /// new logic
        string zpage = pageNumber.ToString();
        string zpagelimit = numberOfObjectsPerPage.ToString();
        string zproducttype = "PG";
        string zkeyword = xkw;
        string zpricefrom = xDari.ToString();
        string zpriceuntil = xSampai.ToString();
        string zstatusbasic = xBasic.ToString();
        string zproductitem = xProductItem.ToString();
        string zproductcategory = xProductCategory.ToString();
        string zproductlevel = xProductLevel.ToString();
        string zproductstonedist = xStoneDistribution.ToString();
        string zdsgcategory = xDesignCategory.ToString();
        string zdsgconcept = xDesignConcept.ToString();
        string zdsgprocess = xDesignProcess.ToString();
        string ztargetage = xTargetAge.ToString();
        string ztargetgender = xTargetGender.ToString();
        string zframematerial = xFrameMaterial.ToString();
        string zframecolour = xFrameColor.ToString();
        string zstockstatus = xStockstatus.ToString();
        string zstonebrand = xStoneBrand.ToString();
        string zberatfrom = xDariBerat.ToString().Replace(",", ".");
        string zberatuntil = xSampaiBerat.ToString().Replace(",", ".");
        string zgoldlevel = xGoldLevel.ToString();
        string zgoldmodel = xGoldModel.ToString();
        string zsupplier = xSupplier.ToString();
        string zrate = xrate.ToString();
        string zukuranfrom = xDariUkuran.ToString().Replace(",", ".");
        string zukuranuntil = xSampaiUkuran.ToString().Replace(",", ".");
        
        int isStore = 0;

        if (db.DataAdminSales.Any(p => p.IDUser == App.UserID)) isStore = 1;
        if (db.DataSales.Any(p => p.IDUser == App.UserID)) isStore = 1;


        string spname = "sp_report_product_catalogue";
        string query = " exec " + spname
                     + " " + zpage + ""
                     + ", " + zpagelimit + ""
                     + ", '" + zproducttype + "'"
                     + ", '" + zkeyword + "'"
                     + ", " + zpricefrom + ""
                     + ", " + zpriceuntil + ""
                     + ", " + zstatusbasic + ""
                     + ", " + zproductitem + ""
                     + ", " + zproductcategory + ""
                     + ", " + zproductlevel + ""
                     + ", " + zproductstonedist + ""
                     + ", " + zdsgcategory + ""
                     + ", " + zdsgconcept + ""
                     + ", " + zdsgprocess + ""
                     + ", " + ztargetage + ""
                     + ", " + ztargetgender + ""
                     + ", " + zframematerial + ""
                     + ", " + zframecolour + ""
                     + ", " + zstockstatus + ""
                     + ", " + zstonebrand + ""
                     + ", " + isStore.ToString() 
                     + ", " + zgoldlevel + ""
                     + ", " + zgoldmodel + ""
                     + ", " + zsupplier + ""
                     + ", " + zrate + ""
                     + ", " + zberatfrom + ""
                     + ", " + zberatuntil + ""
                     + ", " + zukuranfrom + ""
                     + ", " + zukuranuntil + ""
                     ;

        // dari.Text = xDariBerat.ToString();
        //literalsp.Text = query;

        //dariBerat.Text = ratetype.SelectedValue.ToString();

        try
        {
            DataTable dt = new DataTable();
            dt = Db.Rs(query, System.Configuration.ConfigurationManager.ConnectionStrings["Cnn_DB"].ConnectionString);

            if (dt.Rows.Count > 0)
            {
                 

                int idx = 1;

                foreach (DataRow dtRow in dt.Rows)
                {
                  //  string msg = dt.Rows.Count.ToString();
                 //   System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AlertBox", "alert('" + msg + "');", true);


                    if (idx == 1) { TotalPage = Convert.ToDecimal(dtRow["MaxPaging"].ToString()); }

                    string kdplustr = string.Empty;
                    if (kwDDL.Text.Trim() == "0"){ kdplustr = "&kdplu=" + zkeyword + ""; }



                    xproduct += "<div class='col-xs-6 col-sm-4 col-md-3' style='text-align:center;'>";
                    xproduct += "<a class='aproduct' href='PG_View.aspx?proto=" + dtRow["ProtoNomor"].ToString() + "&min=" + zpricefrom + "&max=" + zpriceuntil + "&weightmin=" + zberatfrom + "&weightmax=" + zberatuntil + "&sizemin=" + zukuranfrom + "&sizemax=" + zukuranuntil + "&kwddl=" + kwDDL.Text.Trim() + kdplustr + " '>";
                    xproduct += "<div style='padding:15px; margin:5px;'>";
                    xproduct += "<div style='height:150px; display:block !important;' class='img-thumbnail'><img style='max-height:140px; margin:0 auto;' src=\"/Images.aspx?path=" + dtRow["ImgPicture"].ToString() + "\" class='img-responsive' /></div>";
                    xproduct += "<div style='height:30px;' >";
                    xproduct += dtRow["ProtoNomor"].ToString();
                    xproduct += "</div>";
                    xproduct += "<div style='height:30px;' >Qty : " + dtRow["QtyReadyToSell"].ToString() + " (" + dtRow["QtyReadyToSellWithPriceFilter"].ToString() + ")</div>";
                    xproduct += "</div>";
                    xproduct += "</a>";
                    xproduct += "</div>";

                    idx++;
                }

            }
            else
            {
                xproduct = "<span>There were no items matching your search. Try again with different keywords.</span>";
            }
        }
        catch (Exception ex) { }

        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&designcategory=" + designcategory.Text;
        href += "&designconcept=" + designconcept.Text;
        href += "&designprocess=" + designprocess.Text;
        href += "&targetage=" + targetage.Text;
        href += "&targetgender=" + targetgender.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&stonedistribution=" + stonedistribution.Text;
        href += "&framematerial=" + framematerial.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&goldlevel=" + goldlevel.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&basic=" + statusbasic.Text;
        href += "&page=" + (pageNumber + 1).ToString();
        href += "&stk=" + stockstatus.Text;
        href += "&stonebrand=" + stonebrand.Text;
        href += "&ratetype=" + ratetype.SelectedValue.ToString();
        href += "&dariberat=" + dariBerat.Text;
        href += "&sampaiberat=" + sampaiBerat.Text;
        href += "&kwddl=" + kwDDL.SelectedValue.ToString();
        href += "&supplier=" + supplier.SelectedValue.ToString();
        href += "&dariukuran=" + dariUkuran.Text;
        href += "&sampaiukuran=" + sampaiUkuran.Text;

        string href2 = App.RefreshURL(this);

        //Customize-Qstr
        href2 += "&kw=" + kw.Text;
        href2 += "&designcategory=" + designcategory.Text;
        href2 += "&designconcept=" + designconcept.Text;
        href2 += "&designprocess=" + designprocess.Text;
        href2 += "&targetage=" + targetage.Text;
        href2 += "&targetgender=" + targetgender.Text;
        href2 += "&productitem=" + productitem.Text;
        href2 += "&productcategory=" + productcategory.Text;
        href2 += "&productlevel=" + productlevel.Text;
        href2 += "&stonedistribution=" + stonedistribution.Text;
        href2 += "&framematerial=" + framematerial.Text;
        href2 += "&framecolor=" + framecolor.Text;
        href2 += "&goldlevel=" + goldlevel.Text;
        href2 += "&dari=" + dari.Text;
        href2 += "&sampai=" + sampai.Text;
        href2 += "&basic=" + statusbasic.Text;
        href2 += "&page=" + (pageNumber - 1).ToString();
        href2 += "&stk=" + stockstatus.Text;
        href2 += "&stonebrand=" + stonebrand.Text;
        href2 += "&ratetype=" + ratetype.SelectedValue.ToString();
        href2 += "&dariberat=" + dariBerat.Text;
        href2 += "&sampaiberat=" + sampaiBerat.Text;
        href2 += "&kwddl=" + kwDDL.SelectedValue.ToString();
        href2 += "&supplier=" + supplier.SelectedValue.ToString();
        href2 += "&dariukuran=" + dariUkuran.Text;
        href2 += "&sampaiukuran=" + sampaiUkuran.Text;

        string href3 = App.RefreshURL(this);

        //Customize-Qstr
        href3 += "&kw=" + kw.Text;
        href3 += "&designcategory=" + designcategory.Text;
        href3 += "&designconcept=" + designconcept.Text;
        href3 += "&designprocess=" + designprocess.Text;
        href3 += "&targetage=" + targetage.Text;
        href3 += "&targetgender=" + targetgender.Text;
        href3 += "&productitem=" + productitem.Text;
        href3 += "&productcategory=" + productcategory.Text;
        href3 += "&productlevel=" + productlevel.Text;
        href3 += "&stonedistribution=" + stonedistribution.Text;
        href3 += "&framematerial=" + framematerial.Text;
        href3 += "&framecolor=" + framecolor.Text;
        href3 += "&goldlevel=" + goldlevel.Text;
        href3 += "&dari=" + dari.Text;
        href3 += "&sampai=" + sampai.Text;
        href3 += "&basic=" + statusbasic.Text;
        href3 += "&page=1";
        href3 += "&stk=" + stockstatus.Text;
        href3 += "&stonebrand=" + stonebrand.Text;
        href3 += "&ratetype=" + ratetype.SelectedValue.ToString();
        href3 += "&dariberat=" + dariBerat.Text;
        href3 += "&sampaiberat=" + sampaiBerat.Text;
        href3 += "&kwddl=" + kwDDL.SelectedValue.ToString();
        href3 += "&supplier=" + supplier.SelectedValue.ToString();
        href3 += "&dariukuran=" + dariUkuran.Text;
        href3 += "&sampaiukuran=" + sampaiUkuran.Text;

        string href4 = App.RefreshURL(this);

        //Customize-Qstr
        href4 += "&kw=" + kw.Text;
        href4 += "&designcategory=" + designcategory.Text;
        href4 += "&designconcept=" + designconcept.Text;
        href4 += "&designprocess=" + designprocess.Text;
        href4 += "&targetage=" + targetage.Text;
        href4 += "&targetgender=" + targetgender.Text;
        href4 += "&productitem=" + productitem.Text;
        href4 += "&productcategory=" + productcategory.Text;
        href4 += "&productlevel=" + productlevel.Text;
        href4 += "&stonedistribution=" + stonedistribution.Text;
        href4 += "&framematerial=" + framematerial.Text;
        href4 += "&framecolor=" + framecolor.Text;
        href4 += "&goldlevel=" + goldlevel.Text;
        href4 += "&dari=" + dari.Text;
        href4 += "&sampai=" + sampai.Text;
        href4 += "&basic=" + statusbasic.Text;
        href4 += "&page=" + (TotalPage);
        href4 += "&stk=" + stockstatus.Text;
        href4 += "&stonebrand=" + stonebrand.Text;
        href4 += "&ratetype=" + ratetype.SelectedValue.ToString();
        href4 += "&dariberat=" + dariBerat.Text;
        href4 += "&sampaiberat=" + sampaiBerat.Text;
        href4 += "&kwddl=" + kwDDL.SelectedValue.ToString();
        href4 += "&supplier=" + supplier.SelectedValue.ToString();
        href4 += "&dariukuran=" + dariUkuran.Text;
        href4 += "&sampaiukuran=" + sampaiUkuran.Text;


        string curl = "";
        curl += "&kw=" + kw.Text;
        curl += "&designcategory=" + designcategory.Text;
        curl += "&designconcept=" + designconcept.Text;
        curl += "&designprocess=" + designprocess.Text;
        curl += "&targetage=" + targetage.Text;
        curl += "&targetgender=" + targetgender.Text;
        curl += "&productitem=" + productitem.Text;
        curl += "&productcategory=" + productcategory.Text;
        curl += "&productlevel=" + productlevel.Text;
        curl += "&stonedistribution=" + stonedistribution.Text;
        curl += "&framematerial=" + framematerial.Text;
        curl += "&framecolor=" + framecolor.Text;
        curl += "&goldlevel=" + goldlevel.Text;
        curl += "&dari=" + dari.Text;
        curl += "&sampai=" + sampai.Text;
        curl += "&basic=" + statusbasic.Text;
        curl += "&stk=" + stockstatus.Text;
        curl += "&stonebrand=" + stonebrand.Text;
        curl += "&ratetype=" + ratetype.SelectedValue.ToString();
        curl += "&dariberat=" + dariBerat.Text;
        curl += "&sampaiberat=" + sampaiBerat.Text;
        curl += "&kwddl=" + kwDDL.SelectedValue.ToString();
        curl += "&supplier=" + supplier.SelectedValue.ToString();
        curl += "&dariukuran=" + dariUkuran.Text;
        curl += "&sampaiukuran=" + sampaiUkuran.Text;

        string gotopage = "<input type=\"text\" size=\"3\" style=\"text-align: center;\" name=\"goto\" value=\"" + (pageNumber) + "\" onKeydown=\"gotoPage(event,this.value,'" + curl + "')\" />";

        string paging1 = "";
        string paging2 = "";

        if (TotalPage > 1)
        {
            div_paging.Visible = true;

            paging1 += "<div style=\"clear: both; width: 100%; text-align: center;\">";
            paging1 += "<a href=\"" + href3 + "\">First</a>&nbsp;&nbsp;";
            paging1 += "<a href=\"" + href2 + "\">Prev</a>&nbsp;&nbsp;";
            //paging += gotopage + "&nbsp;&nbsp;";
            paging2 += "<a href=\"" + href + "\">Next</a>&nbsp;&nbsp;";
            paging2 += "<a href=\"" + href4 + "\">Last</a>";
            paging2 += "<br /> Page " + (pageNumber) + " of " + TotalPage + " </div>";

            txt_totalpaging.Text = TotalPage.ToString();
            txt_paging.Text = pageNumber.ToString();
            paging_text_1.Text = paging1;
            paging_text_2.Text = paging2;
        }

        prod.Text = xproduct;

    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            designcategory.SelectedValue = Request.QueryString["designcategory"];
            designconcept.SelectedValue = Request.QueryString["designconcept"];
            designprocess.SelectedValue = Request.QueryString["designprocess"];
            targetage.SelectedValue = Request.QueryString["targetage"];
            targetgender.SelectedValue = Request.QueryString["targetgender"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            stonedistribution.SelectedValue = Request.QueryString["stonedistribution"];
            framematerial.SelectedValue = Request.QueryString["framematerial"];
            framecolor.SelectedValue = Request.QueryString["framecolor"];
            goldlevel.SelectedValue = Request.QueryString["goldlevel"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            statusbasic.SelectedValue = Request.QueryString["basic"];
            stockstatus.SelectedValue = Request.QueryString["stk"];
            stonebrand.SelectedValue = Request.QueryString["stonebrand"];
            ratetype.SelectedValue = Request.QueryString["ratetype"];
            dariBerat.Text = Request.QueryString["dariberat"];
            sampaiBerat.Text = Request.QueryString["sampaiberat"];
            kwDDL.SelectedValue = Request.QueryString["kwddl"];
            supplier.SelectedValue = Request.QueryString["supplier"];
            dariUkuran.Text = Request.QueryString["dariUkuran"];
            sampaiUkuran.Text = Request.QueryString["sampaiUkuran"];
            dariUkuran.Text = Request.QueryString["dariUkuran"];
            sampaiUkuran.Text = Request.QueryString["sampaiUkuran"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&designcategory=" + designcategory.Text;
        href += "&designconcept=" + designconcept.Text;
        href += "&designprocess=" + designprocess.Text;
        href += "&targetage=" + targetage.Text;
        href += "&targetgender=" + targetgender.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&productlevel=" + productlevel.Text;
        href += "&stonedistribution=" + stonedistribution.Text;
        href += "&framematerial=" + framematerial.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&goldlevel=" + goldlevel.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&basic=" + statusbasic.Text;
        href += "&stk=" + stockstatus.Text;
        href += "&stonebrand=" + stonebrand.Text;
        href += "&ratetype=" + ratetype.SelectedValue.ToString();
        href += "&dariberat=" + dariBerat.Text;
        href += "&sampaiberat=" + sampaiBerat.Text;
        href += "&kwddl=" + kwDDL.SelectedValue.ToString();
        href += "&supplier=" + supplier.SelectedValue.ToString();
        href += "&dariUkuran=" + dariUkuran.Text;
        href += "&sampaiUkuran=" + sampaiUkuran.Text;

        Response.Redirect(href);
    }
  

    protected void btn_paging_Click(object sender, EventArgs e)
    {
        string curl = "";

        int page = 0;
        int totalpage = 0;

        try
        {
            page = Convert.ToInt32(txt_paging.Text.Trim());
            totalpage = Convert.ToInt32(txt_totalpaging.Text.Trim());
        }
        catch (Exception ex) { }

        try
        {
            if (page > totalpage) { page = totalpage; }
        }
        catch (Exception ex) { }

        curl += "&kw=" + kw.Text;
        curl += "&designcategory=" + designcategory.Text;
        curl += "&designconcept=" + designconcept.Text;
        curl += "&designprocess=" + designprocess.Text;
        curl += "&targetage=" + targetage.Text;
        curl += "&targetgender=" + targetgender.Text;
        curl += "&productitem=" + productitem.Text;
        curl += "&productcategory=" + productcategory.Text;
        curl += "&productlevel=" + productlevel.Text;
        curl += "&stonedistribution=" + stonedistribution.Text;
        curl += "&framematerial=" + framematerial.Text;
        curl += "&framecolor=" + framecolor.Text;
        curl += "&goldlevel=" + goldlevel.Text;
        curl += "&dari=" + dari.Text;
        curl += "&sampai=" + sampai.Text;
        curl += "&basic=" + statusbasic.Text;
        curl += "&stk=" + stockstatus.Text;
        curl += "&stonebrand=" + stonebrand.Text;
        curl += "&ratetype=" + ratetype.SelectedValue.ToString();
        curl += "&dariberat=" + dariBerat.Text;
        curl += "&sampaiberat=" + sampaiBerat.Text;
        curl += "&kwddl=" + kwDDL.SelectedValue.ToString();
        curl += "&supplier=" + supplier.SelectedValue.ToString();
        curl += "&dariUkuran=" + dariUkuran.Text;
        curl += "&sampaiUkuran=" + sampaiUkuran.Text;

        if (page > 0 && totalpage > 0) { curl += "&page=" + page.ToString(); }

        Response.Redirect("PG_List.aspx?refreshPage=1" + curl);

    }
}