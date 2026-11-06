using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class Page_SPK_DesignList : System.Web.UI.Page
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

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
            DLC.CharFrameFinishing(framefinishing);
            DLC.CharFrameColor(framecolor);
            DLC.CharProcessCons(processconstruction);

            //Generate
            fillFilter();
            if (App.Refresh(this)) fillTable(sender, e);
        }
    }

    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        var rs = from p in db.KodeKaretDJ
                 select p;


        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.Nomor == "" + keyword + "" || p.KodeKaretLama == "" +keyword+ "");
        int DesignCategory = Convert.ToInt32(designcategory.SelectedValue);
        if (DesignCategory > 0) rs = rs.Where(p => p.KodeKaretDJ_CharDesign.DesignCategory == DesignCategory);
        int DesignConcept = Convert.ToInt32(designconcept.SelectedValue);
        if (DesignConcept > 0) rs = rs.Where(p => p.KodeKaretDJ_CharDesign.DesignConcept == DesignConcept);
        int DesignProcess = Convert.ToInt32(designprocess.SelectedValue);
        if (DesignProcess > 0) rs = rs.Where(p => p.KodeKaretDJ_CharDesign.DesignProcess == DesignProcess);
        int TargetAge = Convert.ToInt32(targetage.SelectedValue);
        if (TargetAge > 0) rs = rs.Where(p => p.KodeKaretDJ_CharDesign.TargetAge == TargetAge);
        int TargetGender = Convert.ToInt32(targetgender.SelectedValue);
        if (TargetGender > 0) rs = rs.Where(p => p.KodeKaretDJ_CharDesign.TargetGender == TargetGender);
        int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        if (ProductItem > 0) rs = rs.Where(p => p.KodeKaretDJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        if (ProductCategory > 0) rs = rs.Where(p => p.KodeKaretDJ_CharProduct.ProductCategory == ProductCategory);
        int StoneDistribution = Convert.ToInt32(stonedistribution.SelectedValue);
        if (StoneDistribution > 0) rs = rs.Where(p => p.KodeKaretDJ_CharProduct.StoneDist == StoneDistribution);
        int FrameMaterial = Convert.ToInt32(framematerial.SelectedValue);
        if (FrameMaterial > 0) rs = rs.Where(p => p.KodeKaretDJ_CharProduct.FrameMaterial == FrameMaterial);
        int FrameFinishing = Convert.ToInt32(framefinishing.SelectedValue);
        if (FrameFinishing > 0) rs = rs.Where(p => p.KodeKaretDJ_CharProduct.FrameFinishing == FrameFinishing);
        int FrameColor = Convert.ToInt32(framecolor.SelectedValue);
        if (FrameColor > 0) rs = rs.Where(p => p.KodeKaretDJ_CharProduct.FrameColor == FrameColor);
        int ProcessConstruction = Convert.ToInt32(processconstruction.SelectedValue);
        if (ProcessConstruction > 0) rs = rs.Where(p => p.KodeKaretDJ_CharProduct.ProcessCons == ProcessConstruction);

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
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "top");
                tr.Cells.Add(td);

                td = new TableCell();
                td.Text = index++.ToString() + ".";
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "top");
                tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = "<p><img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" /></p>";
                //td.Style.Add("text-align", "center");
                //td.Style.Add("vertical-align", "top");
                //tr.Cells.Add(td);

                td = new TableCell();
                td.Text = TD_Data(r);
                td.Style.Add("text-align", "center");
                td.Style.Add("vertical-align", "top");
                tr.Cells.Add(td);

                if (r.KodeKaretDJ_CharProduct != null)
                {
                    td = new TableCell();
                    td.Text = r.DataHuman.Nama;
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_ProductChar(r.KodeKaretDJ_CharProduct.CharProductItem.Nama);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_ProductChar(r.KodeKaretDJ_CharProduct.CharProductCategory.Nama);
                    td.Style.Add("text-align", "center");
                    td.Style.Add("vertical-align", "top");
                    tr.Cells.Add(td);
                    
                    //td = new TableCell();
                    //td.Text = TD_Product01(r.KodeKaretDJ_CharProduct);
                    //tr.Cells.Add(td);

                    //td = new TableCell();
                    //td.Text = TD_Product02(r.KodeKaretDJ_CharProduct);
                    //td.CssClass = "text-center";
                    //tr.Cells.Add(td);

                    //td = new TableCell();
                    //td.Text = TD_Product03(r.KodeKaretDJ_CharProduct);
                    //td.CssClass = "text-center";
                    //tr.Cells.Add(td);
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
            designprocess.SelectedValue = Request.QueryString["designprocess"];
            targetage.SelectedValue = Request.QueryString["targetage"];
            targetgender.SelectedValue = Request.QueryString["targetgender"];
            productitem.SelectedValue = Request.QueryString["productitem"];
            productcategory.SelectedValue = Request.QueryString["productcategory"];
            productlevel.SelectedValue = Request.QueryString["productlevel"];
            stonedistribution.SelectedValue = Request.QueryString["stonedistribution"];
            framematerial.SelectedValue = Request.QueryString["framematerial"];
            framefinishing.SelectedValue = Request.QueryString["framefinishing"];
            framecolor.SelectedValue = Request.QueryString["framecolor"];
            processconstruction.SelectedValue = Request.QueryString["processconstruction"];
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
        href += "&framefinishing=" + framefinishing.Text;
        href += "&framecolor=" + framecolor.Text;
        href += "&processconstruction=" + processconstruction.Text;


        Response.Redirect(href);
    }
    protected string TD_HyperLink(KodeKaretDJ r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"SPK_Form.aspx?idkodekaret=" + r.ID + "&tipe=2\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }
    protected string TD_Data(KodeKaretDJ r)
    {
        string x = string.Empty;

        if (r.ID != null)
            x += "<p class=\"text-sm\">" + r.Nomor + "<br /><span class=\"text-muted\">"+r.KodeKaretLama+"</span></p>";

        return x;
    }
    protected string TD_Product01(KodeKaretDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(KodeKaretDJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductCategory.NamaKode;
        x += "</p>";

        return x;
    }
    protected string TD_Product03(KodeKaretDJ_CharProduct r)
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

        x += "<p class=\"text-md\">";
        x += text;
        x += "</p>";

        return x;
    }

}