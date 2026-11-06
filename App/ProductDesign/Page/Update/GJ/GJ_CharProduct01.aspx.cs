using System;
using System.Linq;

public partial class Page_Register_GJ_CharProduct01 : System.Web.UI.Page
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
            productitem.Focus();

            //DLC
            DLC.CharProductItem(productitem);
            DLC.CharProductCategory(productcategory);
            DLC.CharProductLevel(productlevel);
            DLC.CharFrameMaterial(framematerial);
            DLC.CharFrameFinishing(framefinishing);
            DLC.CharFrameColor(framecolor);
            DLC.CharProcessCons(processcons);

            //DLC-Control
            App.DropDown(save, productitem, productitem_ERR);
            App.DropDown(save, productcategory, productcategory_ERR);
            App.DropDown(save, productlevel, productlevel_ERR);
            App.DropDown(save, framematerial, framematerial_ERR);
            App.DropDown(save, framefinishing, framefinishing_ERR);
            App.DropDown(save, framecolor, framecolor_ERR);
            App.DropDown(save, processcons, processcons_ERR);

            //Form
            fillForm();
            fillDist(sender, e);
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("GJ.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.ProductDesignGJ_CharProduct.SingleOrDefault(p => p.ID == myID && p.ProductDesignGJ.Approval == null);
        if (d == null)
        {
            var x = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Approval == null);
            if (x == null) DataError();
            header_nomor.InnerHtml =
                "<a href=\"../History/GJ_File.aspx?id=" + x.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + x.Nomor + "</a>";
        }
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/GJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.ProductDesignGJ.Nomor + "</a>";

            productitem.SelectedValue = d.ProductItem.ToString();
            productcategory.SelectedValue = d.ProductCategory.ToString();
            productlevel.SelectedValue = d.ProductLevel.ToString();
            framematerial.SelectedValue = d.FrameMaterial.ToString();
            framefinishing.SelectedValue = d.FrameFinishing.ToString();
            framecolor.SelectedValue = d.FrameColor.ToString();
            processcons.SelectedValue = d.ProcessCons.ToString();
        }
    }
    protected void fillDist(object sender, EventArgs e)
    {
        //Clear-Selection
        stonedist_ERR.Visible = false;
        stonedist.ClearSelection();
        stonedist.Items.Clear();

        //DLC
        if (productcategory.Items.Count > 0 && productlevel.Items.Count > 0)
        {
            int IDCategory = Convert.ToInt32(productcategory.SelectedValue);
            int IDLevel = Convert.ToInt32(productlevel.SelectedValue);
            DLC.SettingsStoneDist(stonedist, IDCategory, IDLevel);
        }
    }

    //Save
    protected bool valid_Dist
    {
        get
        {
            bool x = stonedist.Items.Count > 0;
            stonedist_ERR.Visible = !x;
            return x;
        }
    }
    protected void save_Click(object sender, EventArgs e)
    {
        if (valid_Dist)
        {
            var d = db.ProductDesignGJ_CharProduct.SingleOrDefault(p => p.ID == myID && p.ProductDesignGJ.Approval == null);
            if (d == null)
            {
                var ms = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Approval == null);
                insertProcess(ms);
                Response.Redirect("GJ_CharProduct02.aspx?id=" + myID);
            }
            else
            {
                //Process

                d.ProductItem = Convert.ToInt32(productitem.SelectedValue);
                d.ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
                d.ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
                d.StoneDist = Convert.ToInt32(stonedist.SelectedValue);
                d.FrameMaterial = Convert.ToInt32(framematerial.SelectedValue);
                d.FrameFinishing = Convert.ToInt32(framefinishing.SelectedValue);
                d.FrameColor = Convert.ToInt32(framecolor.SelectedValue);
                d.ProcessCons = Convert.ToInt32(processcons.SelectedValue);

                //Commit
                db.SubmitChanges();

                //Redirect
                Response.Redirect("GJ_CharProduct02.aspx?id=" + myID);
            }
        }
    }

    protected void insertProcess(ProductDesignGJ ms)
    {
        ProductDesignGJ_CharProduct n = new ProductDesignGJ_CharProduct();

        n.ID = ms.ID;

        n.ProductItem = Convert.ToInt32(productitem.SelectedValue);
        n.ProductCategory = Convert.ToInt32(productcategory.SelectedValue);
        n.ProductLevel = Convert.ToInt32(productlevel.SelectedValue);
        n.StoneDist = Convert.ToInt32(stonedist.SelectedValue);
        n.FrameMaterial = Convert.ToInt32(framematerial.SelectedValue);
        n.FrameFinishing = Convert.ToInt32(framefinishing.SelectedValue);
        n.FrameColor = Convert.ToInt32(framecolor.SelectedValue);
        n.ProcessCons = Convert.ToInt32(processcons.SelectedValue);

        //Commit
        db.ProductDesignGJ_CharProduct.InsertOnSubmit(n);
        db.SubmitChanges();
    }
}