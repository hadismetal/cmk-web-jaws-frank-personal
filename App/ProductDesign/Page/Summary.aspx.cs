using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Summary : System.Web.UI.Page {
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            DLC.CharDesignCategory(designcategory);
            DLC.CharDesignConcept(designconcept);
            DLC.CharTargetAge(targetage);
            DLC.CharTargetGender(targetgender);
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductCategory(productcategory);
            DLC.Designer(designer);
            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.ProductDesign
                 /*where p.Draft == false*/
                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.Nomor == "" + keyword + "");
        int DesignCategory = Convert.ToInt32(designcategory.SelectedValue);
        if (DesignCategory > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.DesignCategory == DesignCategory);
        int DesignConcept = Convert.ToInt32(designconcept.SelectedValue);
        if (DesignConcept > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.DesignConcept == DesignConcept);
        int TargetAge = Convert.ToInt32(targetage.SelectedValue);
        if (TargetAge > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.TargetAge == TargetAge);
        int TargetGender = Convert.ToInt32(targetgender.SelectedValue);
        if (TargetGender > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.TargetGender == TargetGender);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProductCategory == ProductCategory);
        int Designer = Convert.ToInt32(designer.SelectedValue);
        if (Designer > 0) rs = rs.Where(p => p.DataHuman.ID == Designer);
        if(status.SelectedValue=="1")
            rs = rs.Where(p=> p.Approval !=null);
        else if (status.SelectedValue == "2")
            rs = rs.Where(p => p.Approval == null);
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text));
        }

        int UserID = App.UserID;
        var ad = from p in db.DataHuman
                 where p.IDUser == UserID && !p.Disable
                 select p;
        if (ad.Count() > 0)
        {
            var x = ad.First();
            rs = rs.Where(p => p.DataHuman.CompanyBrand.NamaKode == x.CompanyBrand.NamaKode);
        }

        if (rs != null)
        {
            int prev = 0;
            int index = 1;
            foreach (var r in rs)
            {
                if (!Response.IsClientConnected) break;

                //Sub-Header

                TableRow tr = new TableRow();
                TableCell td;

                td = new TableCell();
                td.Text = TD_HyperLink(r);
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = "<p>" + index++.ToString() + ".</p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = "<p><img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" /></p>";
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = Str.FullDate(r.Tgl);
                tr.Cells.Add(td);


                td = new TableCell();
                td.Text = TD_Data(r);
                tr.Cells.Add(td);

                if (r.ProductDesign_CharProduct != null)
                {
                    td = new TableCell();
                    td.Text = TD_Product01(r.ProductDesign_CharProduct);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Product02(r.ProductDesign_CharProduct);
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_ProductChar(r.ProductDesign_CharProduct.CharProductCategory.Nama);
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    string Segmen = "-";
                    try
                    {
                        Segmen = r.ProductDesign_PricingSegmen.CharProductSegmen.Nama;
                    }
                    catch (Exception ex) { }

                    td = new TableCell();
                    td.Text = TD_ProductChar(Segmen);
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    string Harga = "-";
                    try
                    {
                        Harga = Convert.ToDecimal(r.ProductDesign_PricingMU.HargaRupiahRound).ToString("N0");
                    }
                    catch(Exception ex)
                    {
                    }

                    td = new TableCell();
                    td.Text = TD_ProductChar(Harga);
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_ProductChar(r.DataHuman.Nama);
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);
                }

                tb.Rows.Add(tr);
            }
        }
    }

    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            kw.Text = Request.QueryString["kw"];
            designcategory.SelectedValue = Request.QueryString["designcategory"];
            designconcept.SelectedValue = Request.QueryString["designconcept"];
            targetage.SelectedValue = Request.QueryString["targetage"];
            targetgender.SelectedValue = Request.QueryString["targetgender"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            status.SelectedValue = Request.QueryString["status"];
            dari.Text = Request.QueryString["dari"];
            sampai.Text = Request.QueryString["sampai"];
            designer.Text = Request.QueryString["designer"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        href += "&kw=" + kw.Text;
        href += "&designcategory=" + designcategory.Text;
        href += "&designconcept=" + designconcept.Text;
        href += "&targetage=" + targetage.Text;
        href += "&targetgender=" + targetgender.Text;
        href += "&productitem=" + productitem.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&designer=" + designer.Text;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(ProductDesign r)
    {
        string x = string.Empty;

        if (r.Draft == false)
        {
            x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Design_Detail.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-search\"></i> ";
            x += "  Open";
            x += "</a>";
        }

        if (r.Approval == null && Role.Manager)
        {
            x += "&nbsp;&nbsp;<a class=\"btn btn-danger\" onClick=\"return confirm('Are you Sure');\" href=\"Design_Delete.aspx?id=" + r.ID + "\">";
            x += "  <i class=\"icon-trash\"></i> ";
            x += "  Delete";
            x += "</a>";
        }
        return x;
    }
    protected string TD_Data(ProductDesign r)
    {
        string x = string.Empty;

        if (r.ID != null)
            x += "<p class=\"text-sm\">" + r.Nomor + "</p>";

        return x;
    }
    protected string TD_Product01(ProductDesign_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(ProductDesign_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductCategory.NamaKode;
        x += "</p>";

        return x;
    }
    protected string TD_Product03(ProductDesign_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductLevel.NamaKode;
        x += "</p>";

        return x;
    }

    protected string TD_ProductChar(string text)
    {
        string x = string.Empty;

        x += "<p class=\"text-orange\">";
        x += text;
        x += "</p>";

        return x;
    }
}