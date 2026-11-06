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
            //DLC.CharDesignCategory(designcategory);
            //DLC.CharDesignConcept(designconcept);
            //DLC.CharDesignProcess(designprocess);
            //DLC.CharTargetAge(targetage);
            //DLC.CharTargetGender(targetgender);
            //DLC.CharProductItem(productitem);
            //DLC.CharProductCategory(productcategory);
            //DLC.CharProductLevel(productlevel);
            //DLC.CharStoneDist(stonedistribution);
            //DLC.CharFrameMaterial(framematerial);
            //DLC.CharFrameFinishing(framefinishing);
            //DLC.CharFrameColor(framecolor);
            //DLC.CharProcessCons(processconstruction);

            //Generate
            fillFilter();
            //if (App.Refresh(this)) fillTable(sender, e);
            fillTable(sender, e);
        }
    }

    //back_Click
    protected void back_Click(object sender, EventArgs e)
    {
        Response.Redirect("SPK.aspx");
    }
    
    //Display Table
    protected void fillTable(object sender, EventArgs e)
    {
        //SELECT
        var rs = from p in db.ProductDesign
                 where p.StatusSPK == false && p.Draft == false && p.Approval != null
                 && p.Tgl >= System.DateTime.Now.AddMonths(-12)
                 select p;

        //string keyword = kw.Text;
        //if (keyword != "") rs = rs.Where(p => p.Nomor == "" + keyword + "");
        //int DesignCategory = Convert.ToInt32(designcategory.SelectedValue);
        //if (DesignCategory > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.DesignCategory == DesignCategory);
        //int DesignConcept = Convert.ToInt32(designconcept.SelectedValue);
        //if (DesignConcept > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.DesignConcept == DesignConcept);
        //int DesignProcess = Convert.ToInt32(designprocess.SelectedValue);
        //if (DesignProcess > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.DesignProcess == DesignProcess);
        //int TargetAge = Convert.ToInt32(targetage.SelectedValue);
        //if (TargetAge > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.TargetAge == TargetAge);
        //int TargetGender = Convert.ToInt32(targetgender.SelectedValue);
        //if (TargetGender > 0) rs = rs.Where(p => p.ProductDesign_CharDesign.TargetGender == TargetGender);
        //int ProductItem = Convert.ToInt32(productitem.SelectedValue);
        //if (ProductItem > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProductItem == ProductItem);
        //int ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        //if (ProductCategory > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProductCategory == ProductCategory);
        //int StoneDistribution = Convert.ToInt32(stonedistribution.SelectedValue);
        //if (StoneDistribution > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.StoneDist == StoneDistribution);
        //int FrameMaterial = Convert.ToInt32(framematerial.SelectedValue);
        //if (FrameMaterial > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.FrameMaterial == FrameMaterial);
        //int FrameFinishing = Convert.ToInt32(framefinishing.SelectedValue);
        //if (FrameFinishing > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.FrameFinishing == FrameFinishing);
        //int FrameColor = Convert.ToInt32(framecolor.SelectedValue);
        //if (FrameColor > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.FrameColor == FrameColor);
        //int ProcessConstruction = Convert.ToInt32(processconstruction.SelectedValue);
        //if (ProcessConstruction > 0) rs = rs.Where(p => p.ProductDesign_CharProduct.ProcessCons == ProcessConstruction);

        rs = rs.OrderByDescending(p => p.Tgl);

        //NO-RESULT
        //zero.Visible = rs.Count() == 0;
        //form_area.Visible = !zero.Visible;

        //RESULT
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
            
            td = new TableCell();
            //td.Text = "<p><img src=\"/Images.aspx?path=" + r.ImgPicture + "\" width=\"100\" /></p>";
            td.Text = Img.ResizeImageToBase64Html(r.ImgPicture, 30, 30);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "top");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Data(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "top");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Operator(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "top");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Approval(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "top");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Product01(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "top");
            tr.Cells.Add(td);

            td = new TableCell();
            td.Text = TD_Product02(r);
            td.Style.Add("text-align", "center");
            td.Style.Add("vertical-align", "top");
            tr.Cells.Add(td);

            if (r.ProductDesign_CharProduct != null)
            {


                //td = new TableCell();
                //td.Text = TD_Product01(r.ProductDesign_CharProduct);
                //td.Style.Add("text-align", "center");
                //td.Style.Add("vertical-align", "top");
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = TD_Product02(r.ProductDesign_CharProduct);
                //td.Style.Add("text-align", "center");
                //td.Style.Add("vertical-align", "top");
                //tr.Cells.Add(td);

                //td = new TableCell();
                //td.Text = TD_Product03(r.ProductDesign_CharProduct);
                //td.Style.Add("text-align", "center");
                //td.Style.Add("vertical-align", "top");
                //tr.Cells.Add(td);
            }

            tb.Rows.Add(tr);
        }
    }
    protected void fillFilter()
    {
        if (App.Refresh(this))
        {
            //kw.Text = Request.QueryString["kw"];
            //designcategory.SelectedValue = Request.QueryString["designcategory"];
            //designconcept.SelectedValue = Request.QueryString["designconcept"];
            //designprocess.SelectedValue = Request.QueryString["designprocess"];
            //targetage.SelectedValue = Request.QueryString["targetage"];
            //targetgender.SelectedValue = Request.QueryString["targetgender"];
            //productitem.SelectedValue = Request.QueryString["productitem"];
            //productcategory.SelectedValue = Request.QueryString["productcategory"];
            //productlevel.SelectedValue = Request.QueryString["productlevel"];
            //stonedistribution.SelectedValue = Request.QueryString["stonedistribution"];
            //framematerial.SelectedValue = Request.QueryString["framematerial"];
            //framefinishing.SelectedValue = Request.QueryString["framefinishing"];
            //framecolor.SelectedValue = Request.QueryString["framecolor"];
            //processconstruction.SelectedValue = Request.QueryString["processconstruction"];
        }
    }
    protected void refreshPage(object sender, EventArgs e)
    {
        string href = App.RefreshURL(this);

        //Customize-Qstr
        //href += "&kw=" + kw.Text;
        //href += "&designcategory=" + designcategory.Text;
        //href += "&designconcept=" + designconcept.Text;
        //href += "&designprocess=" + designprocess.Text;
        //href += "&targetage=" + targetage.Text;
        //href += "&targetgender=" + targetgender.Text;
        //href += "&productitem=" + productitem.Text;
        //href += "&productcategory=" + productcategory.Text;
        //href += "&productlevel=" + productlevel.Text;
        //href += "&stonedistribution=" + stonedistribution.Text;
        //href += "&framematerial=" + framematerial.Text;
        //href += "&framefinishing=" + framefinishing.Text;
        //href += "&framecolor=" + framecolor.Text;
        //href += "&processconstruction=" + processconstruction.Text;


        Response.Redirect(href);
    }

    protected string TD_HyperLink(ProductDesign r)
    {
        string x = string.Empty;

        x += "<a class=\"btn btn-info\" target=\"_blank\" href=\"SPK_Form.aspx?idproductdesign=" + r.ID + "&tipe=1\">";
        x += "  <i class=\"icon-search\"></i> ";
        x += "  ";
        x += "</a>";

        return x;
    }
    protected string TD_Data(ProductDesign r)
    {
        string x = string.Empty;

        x += r.Nomor
            + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
            + r.DataHuman.Nama
            ;

        return x;
    }
    protected string TD_Operator(ProductDesign r)
    {
        string x = string.Empty;

        string x_Operator = "";
        string x_OperatorTgl = "";

        x_Operator = (r.Operator != null) ? r.Operator : "";
        x_OperatorTgl = (r.Operator != null) ? Str.FullDateJam(r.OperatorTgl) : "";

        x += x_Operator
            + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
            + x_OperatorTgl
            ;

        return x;
    }
    protected string TD_Approval(ProductDesign r)
    {
        string x = string.Empty;

        string x_Approval = "";
        string x_ApprovalTgl = "";

        x_Approval = (r.Approval != null) ? r.Approval : "";
        x_ApprovalTgl = (r.Approval != null) ? Str.FullDateJam(r.ApprovalTgl) : "";

        x += x_Approval
            + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
            + x_ApprovalTgl
            ;

        return x;
    }
    protected string TD_Product01(ProductDesign r)
    {
        string x = string.Empty;

        if (r.ProductDesign_CharProduct != null)
        {
            x += r.ProductDesign_CharProduct.CharProductItem.Nama
                + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
                + r.ProductDesign_CharProduct.CharProductCategory.Nama
                ;
        }

        return x;
    }
    protected string TD_Product02(ProductDesign r)
    {
        string x = string.Empty;

        if (r.ProductDesign_CharProduct != null)
        {
            x += r.ProductDesign_CharProduct.CharProductLevel.Nama
                + "<div style='clear: both; border-bottom:solid 1px silver;'></div>"
                + r.ProductDesign_CharProduct.CharStoneDist.Nama
                ;
        }

        return x;
    }

}