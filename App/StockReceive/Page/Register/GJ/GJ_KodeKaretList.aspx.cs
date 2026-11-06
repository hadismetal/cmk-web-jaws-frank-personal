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
    protected int myID { get { return App.QueryInt(this); } }

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
        var rs = from p in db.KodeKaretGJ_Detail
                 where p.ProductOrderGJ.StatusPenerimaan == 0
                 select p;


        string keyword = kw.Text;
        if (keyword != "") rs = rs.Where(p => p.KodeKaretDetail == "" + keyword + "");
        int DesignCategory = designcategory.SelectedIndex;
        if (DesignCategory > 0) rs = rs.Where(p => p.KodeKaretGJ.KodeKaretGJ_CharDesign.DesignCategory == DesignCategory);
        int DesignConcept = designconcept.SelectedIndex;
        if (DesignConcept > 0) rs = rs.Where(p => p.KodeKaretGJ.KodeKaretGJ_CharDesign.DesignConcept == DesignConcept);
        int DesignProcess = designprocess.SelectedIndex;
        if (DesignProcess > 0) rs = rs.Where(p => p.KodeKaretGJ.KodeKaretGJ_CharDesign.DesignProcess == DesignProcess);
        int TargetAge = targetage.SelectedIndex;
        if (TargetAge > 0) rs = rs.Where(p => p.KodeKaretGJ.KodeKaretGJ_CharDesign.TargetAge == TargetAge);
        int TargetGender = targetgender.SelectedIndex;
        if (TargetGender > 0) rs = rs.Where(p => p.KodeKaretGJ.KodeKaretGJ_CharDesign.TargetGender == TargetGender);
        int ProductItem = productitem.SelectedIndex;
        if (ProductItem > 0) rs = rs.Where(p => p.KodeKaretGJ.KodeKaretGJ_CharProduct.ProductItem == ProductItem);
        int ProductCategory = productcategory.SelectedIndex;
        if (ProductCategory > 0) rs = rs.Where(p => p.KodeKaretGJ.KodeKaretGJ_CharProduct.ProductCategory == ProductCategory);
        int StoneDistribution = stonedistribution.SelectedIndex;
        if (StoneDistribution > 0) rs = rs.Where(p => p.KodeKaretGJ.KodeKaretGJ_CharProduct.StoneDist == StoneDistribution);
        int FrameMaterial = framematerial.SelectedIndex;
        if (FrameMaterial > 0) rs = rs.Where(p => p.KodeKaretGJ.KodeKaretGJ_CharProduct.FrameMaterial == FrameMaterial);
        int FrameFinishing = framefinishing.SelectedIndex;
        if (FrameFinishing > 0) rs = rs.Where(p => p.KodeKaretGJ.KodeKaretGJ_CharProduct.FrameFinishing == FrameFinishing);
        int FrameColor = framecolor.SelectedIndex;
        if (FrameColor > 0) rs = rs.Where(p => p.KodeKaretGJ.KodeKaretGJ_CharProduct.FrameColor == FrameColor);
        int ProcessConstruction = processconstruction.SelectedIndex;
        if (ProcessConstruction > 0) rs = rs.Where(p => p.KodeKaretGJ.KodeKaretGJ_CharProduct.ProcessCons == ProcessConstruction);

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
                td.Text = TD_Data(r);
                tr.Cells.Add(td);

                if (r.KodeKaretGJ.KodeKaretGJ_CharProduct != null)
                {
                    td = new TableCell();
                    td.Text = TD_Product01(r.KodeKaretGJ.KodeKaretGJ_CharProduct);
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Product02(r.KodeKaretGJ.KodeKaretGJ_CharProduct);
                    td.CssClass = "text-center";
                    tr.Cells.Add(td);

                    td = new TableCell();
                    td.Text = TD_Product03(r.KodeKaretGJ.KodeKaretGJ_CharProduct);
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
        href += "&id=" + myID;
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
    protected string TD_HyperLink(KodeKaretGJ_Detail r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" href=\"GJ_InsertKodeKaret.aspx?id="+myID+"&idkodekaret=" + r.ID + "\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  Open";
        x += "</a>";

        return x;
    }
    protected string TD_Data(KodeKaretGJ_Detail r)
    {
        string x = string.Empty;

        if (r.ID != null)
            x += "<p class=\"text-sm\">" + r.KodeKaretDetail + "</p>";

        return x;
    }
    protected string TD_Product01(KodeKaretGJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-bold text-primary\">";
        x += r.CharProductItem.Nama;
        x += "</p>";

        return x;
    }
    protected string TD_Product02(KodeKaretGJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductCategory.NamaKode;
        x += "</p>";

        return x;
    }
    protected string TD_Product03(KodeKaretGJ_CharProduct r)
    {
        string x = string.Empty;

        x += "<p class=\"text-lg text-orange\">";
        x += r.CharProductLevel.NamaKode;
        x += "</p>";

        return x;
    }
}