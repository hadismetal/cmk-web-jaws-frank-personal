using System;
using System.Linq;

public partial class Page_Register_DJ_CharProduct01 : System.Web.UI.Page
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
    protected void DataError() { Response.Redirect("DJ.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

            if (d.Proto != null)
            {
                var proto = db.KodeKaret_Detail.SingleOrDefault(p => p.ID == d.Proto);
                if (proto != null)
                {
                    productitem.SelectedValue = proto.ProductOrder.ProductOrder_CharProduct.ProductItem.ToString();
                    productcategory.SelectedValue = proto.ProductOrder.ProductOrder_CharProduct.ProductCategory.ToString();
                    productlevel.SelectedValue = proto.ProductOrder.ProductOrder_CharProduct.ProductLevel.ToString();
                    framematerial.SelectedValue = proto.ProductOrder.ProductOrder_CharProduct.FrameMaterial.ToString();
                    framefinishing.SelectedValue = proto.ProductOrder.ProductOrder_CharProduct.FrameFinishing.ToString();
                    framecolor.SelectedValue = proto.ProductOrder.ProductOrder_CharProduct.FrameColor.ToString();
                    processcons.SelectedValue = proto.ProductOrder.ProductOrder_CharProduct.ProcessCons.ToString();
                }
            }
            if (d.IDMounting != null)
            {
                var mt = db.StockReceiveMounting.SingleOrDefault(p => p.ID == d.IDMounting);
                if (mt != null)
                {
                    productitem.SelectedValue = mt.StockReceiveMounting_CharProduct.ProductItem.ToString();
                    productcategory.SelectedValue = mt.StockReceiveMounting_CharProduct.ProductCategory.ToString();
                    productlevel.SelectedValue = mt.StockReceiveMounting_CharProduct.ProductLevel.ToString();
                    framematerial.SelectedValue = mt.StockReceiveMounting_CharProduct.FrameMaterial.ToString();
                    framefinishing.SelectedValue = mt.StockReceiveMounting_CharProduct.FrameFinishing.ToString();
                    framecolor.SelectedValue = mt.StockReceiveMounting_CharProduct.FrameColor.ToString();
                    processcons.SelectedValue = mt.StockReceiveMounting_CharProduct.ProcessCons.ToString();
                }
            }
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
            var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
            if (d == null) DataError();
            else
            {
                //Process
                deleteProcess(d);
                insertProcess(d);

                //Redirect
                Response.Redirect("DJ_CharProduct02.aspx?id=" + myID);
            }
        }
    }
    protected void deleteProcess(StockReceiveDJ ms)
    {
        var d = ms.StockReceiveDJ_CharProduct;
        if (d != null)
        {
            //Commit
            db.StockReceiveDJ_CharProduct.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void insertProcess(StockReceiveDJ ms)
    {
        StockReceiveDJ_CharProduct n = new StockReceiveDJ_CharProduct();

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
        db.StockReceiveDJ_CharProduct.InsertOnSubmit(n);
        db.SubmitChanges();
    }
}