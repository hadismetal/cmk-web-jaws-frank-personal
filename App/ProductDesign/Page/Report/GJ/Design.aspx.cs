using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;

public partial class Page_Design : System.Web.UI.Page {
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
            DLC.CharProductLevel(productlevel);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductCategory(productcategory);
            DLC.Designer(designer);
            DLC.CharStoneDist(stonedistribution);
            DLC.CharProductSegmen(segmen);
            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.ProductDesignGJ
                 where p.Draft == false
                 && (from z in db.ProductDesignGJ_CharDesign
                     select z.ID).Contains(p.ID)
                 && (from z in db.ProductDesignGJ_CharProduct
                     select z.ID).Contains(p.ID)
                 && (from z in db.ProductDesignGJ_PricingSegmen
                     select z.ID).Contains(p.ID)

                 select p;

        rs = rs.OrderByDescending(p => p.Tgl);

        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.Nomor == "" + keyword + "");
        int DesignCategory = Convert.ToInt32(designcategory.SelectedValue);
        if (DesignCategory > 0) rs = rs.Where(p => p.ProductDesignGJ_CharDesign.DesignCategory == DesignCategory);
        int DesignConcept = Convert.ToInt32(designconcept.SelectedValue);
        if (DesignConcept > 0) rs = rs.Where(p => p.ProductDesignGJ_CharDesign.DesignConcept == DesignConcept);
        int TargetAge = Convert.ToInt32(targetage.SelectedValue);
        if (TargetAge > 0) rs = rs.Where(p => p.ProductDesignGJ_CharDesign.TargetAge == TargetAge);
        int TargetGender = Convert.ToInt32(targetgender.SelectedValue);
        if (TargetGender > 0) rs = rs.Where(p => p.ProductDesignGJ_CharDesign.TargetGender == TargetGender);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.ProductDesignGJ_CharProduct.ProductItem == ProductItem);
        int ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        if (ProductLevel > 0) rs = rs.Where(p => p.ProductDesignGJ_CharProduct.ProductLevel == ProductLevel);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.ProductDesignGJ_CharProduct.ProductCategory == ProductCategory);
        int Designer = Convert.ToInt32(designer.SelectedValue);
        if (Designer > 0) rs = rs.Where(p => p.DataHuman.ID == Designer);

        int StoneDist = Convert.ToInt32(stonedistribution.SelectedValue);
        if (StoneDist > 0) rs = rs.Where(p => p.ProductDesignGJ_CharProduct.StoneDist == StoneDist);
        int ProductSegmen = Convert.ToInt32(segmen.SelectedValue);
        if (ProductSegmen > 0) rs = rs.Where(p => p.ProductDesignGJ_PricingSegmen.ProductSegmen == ProductSegmen);

        if(status.SelectedValue=="1")
            rs = rs.Where(p=> p.Approval !=null);
        else if (status.SelectedValue == "2")
            rs = rs.Where(p => p.Approval == null);
        if (dari.Text != "" && sampai.Text != "")
        {
            rs = rs.Where(p => p.Tgl >= Convert.ToDateTime(dari.Text) && p.Tgl < Convert.ToDateTime(sampai.Text));
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

                td = new TableCell();
                td.Text = r.ProductDesignGJ_CharProduct.CharProductItem.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProductDesignGJ_CharProduct.CharProductLevel.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProductDesignGJ_CharProduct.CharProductCategory.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProductDesignGJ_CharProduct.CharStoneDist.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProductDesignGJ_CharDesign.CharDesignCategory.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProductDesignGJ_CharDesign.CharDesignConcept.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProductDesignGJ_PricingSegmen.CharProductSegmen.Nama;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.ProductDesignGJ_CharDesign.Naming;
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = r.HumanDesignerNama;
                tr.Cells.Add(td);/**/

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
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            status.SelectedValue = Request.QueryString["status"];
            stonedistribution.SelectedValue = Request.QueryString["stonedistribution"];
            segmen.SelectedValue = Request.QueryString["segmen"];
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
        href += "&productlevel=" + productlevel.Text;
        href += "&productcategory=" + productcategory.Text;
        href += "&stonedistribution=" + stonedistribution.Text;
        href += "&segmen=" + segmen.Text;
        href += "&status=" + status.Text;
        href += "&dari=" + dari.Text;
        href += "&sampai=" + sampai.Text;
        href += "&designer=" + designer.Text;

        Response.Redirect(href);
    }
    protected string TD_HyperLink(ProductDesignGJ r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"Design_Detail.aspx?id=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";
        return x;
    }
    protected string TD_Data(ProductDesignGJ r)
    {
        string x = string.Empty;

        if (r.ID != null && r.Nomor != null)
            x += "<p class=\"text-sm\">" + r.Nomor + "</p>";

        return x;
    }
    protected string TD_Product01(ProductDesignGJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(ProductDesignGJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductCategory.NamaKode;
        x += "</p>";

        return x;
    }
    protected string TD_Product03(ProductDesignGJ_CharProduct r)
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