using System;
using System.Linq;

public partial class Page_Register_DJ_CharDesign01 : System.Web.UI.Page
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
            designcategory.Focus();

            //DLC
            DLC.CharDesignCategory(designcategory);
            DLC.CharDesignConcept(designconcept);
            DLC.CharDesignProcess(designprocess);
            DLC.CharTargetAge(targetage);
            DLC.CharTargetGender(targetgender);

            //DLC-Control
            App.DropDown(save, designcategory, designcategory_ERR);
            App.DropDown(save, designconcept, designconcept_ERR);
            App.DropDown(save, designprocess, designprocess_ERR);
            App.DropDown(save, targetage, targetage_ERR);
            App.DropDown(save, targetgender, targetgender_ERR);

            //Form
            fillForm();
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
                    designcategory.SelectedValue = proto.ProductOrder.ProductOrder_CharDesign.DesignCategory.ToString();
                    designconcept.SelectedValue = proto.ProductOrder.ProductOrder_CharDesign.DesignConcept.ToString();
                    designprocess.SelectedValue = proto.ProductOrder.ProductOrder_CharDesign.DesignProcess.ToString();
                    targetage.SelectedValue = proto.ProductOrder.ProductOrder_CharDesign.TargetAge.ToString();
                    targetgender.SelectedValue = proto.ProductOrder.ProductOrder_CharDesign.TargetGender.ToString();
                }
            }
            if (d.IDMounting != null)
            {
                var mt = db.StockReceiveMounting.SingleOrDefault(p => p.ID == d.IDMounting);
                if (mt != null)
                {
                    designcategory.SelectedValue = mt.StockReceiveMounting_CharDesign.DesignCategory.ToString();
                    designconcept.SelectedValue = mt.StockReceiveMounting_CharDesign.DesignConcept.ToString();
                    designprocess.SelectedValue = mt.StockReceiveMounting_CharDesign.DesignProcess.ToString();
                    targetage.SelectedValue = mt.StockReceiveMounting_CharDesign.TargetAge.ToString();
                    targetgender.SelectedValue = mt.StockReceiveMounting_CharDesign.TargetGender.ToString();
                }
            }
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockReceiveDJ.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Process
            deleteProcess(d);
            insertProcess(d);

            //Redirect
            Response.Redirect("DJ_CharDesign02.aspx?id=" + myID);
        }
    }
    protected void deleteProcess(StockReceiveDJ ms)
    {
        var d = ms.StockReceiveDJ_CharDesign;
        if (d != null)
        {
            //Commit
            db.StockReceiveDJ_CharDesign.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void insertProcess(StockReceiveDJ ms)
    {
        StockReceiveDJ_CharDesign n = new StockReceiveDJ_CharDesign();

        n.ID = ms.ID;

        n.DesignCategory = Convert.ToInt32(designcategory.SelectedValue);
        n.DesignConcept = Convert.ToInt32(designconcept.SelectedValue);
        n.DesignProcess = Convert.ToInt32(designprocess.SelectedValue);
        n.TargetAge = Convert.ToInt32(targetage.SelectedValue);
        n.TargetGender = Convert.ToInt32(targetgender.SelectedValue);

        //Commit
        db.StockReceiveDJ_CharDesign.InsertOnSubmit(n);
        db.SubmitChanges();
    }
}