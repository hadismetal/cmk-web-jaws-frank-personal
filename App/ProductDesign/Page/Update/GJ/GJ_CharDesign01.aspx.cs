using System;
using System.Linq;

public partial class Page_Register_GJ_CharDesign01 : System.Web.UI.Page
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
    protected void DataError() { Response.Redirect("GJ.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.ProductDesignGJ_CharDesign.SingleOrDefault(p => p.ID == myID && p.ProductDesignGJ.Approval==null);
        if (d == null)
        {
            var x = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
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

            
            designcategory.SelectedValue = d.DesignCategory.ToString();
            designconcept.SelectedValue = d.DesignConcept.ToString();
            designprocess.SelectedValue = d.DesignProcess.ToString();
            targetage.SelectedValue = d.TargetAge.ToString();
            targetgender.SelectedValue = d.TargetGender.ToString();
                
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.ProductDesignGJ_CharDesign.SingleOrDefault(p => p.ID == myID && p.ProductDesignGJ.Approval==null);
        if (d == null)
        {
            var ms = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
            insertProcess(ms);
            Response.Redirect("GJ_CharDesign02.aspx?id=" + myID);
        }
        else
        {
            //Process
            d.DesignCategory = Convert.ToInt32(designcategory.SelectedValue);
            d.DesignConcept = Convert.ToInt32(designconcept.SelectedValue);
            d.DesignProcess = Convert.ToInt32(designprocess.SelectedValue);
            d.TargetAge = Convert.ToInt32(targetage.SelectedValue);
            d.TargetGender = Convert.ToInt32(targetgender.SelectedValue);

            //Commit
            db.SubmitChanges();

            //Redirect
            Response.Redirect("GJ_CharDesign02.aspx?id=" + myID);
        }
    }
    protected void insertProcess(ProductDesignGJ ms)
    {
        ProductDesignGJ_CharDesign n = new ProductDesignGJ_CharDesign();

        n.ID = ms.ID;

        n.DesignCategory = Convert.ToInt32(designcategory.SelectedValue);
        n.DesignConcept = Convert.ToInt32(designconcept.SelectedValue);
        n.DesignProcess = Convert.ToInt32(designprocess.SelectedValue);
        n.TargetAge = Convert.ToInt32(targetage.SelectedValue);
        n.TargetGender = Convert.ToInt32(targetgender.SelectedValue);

        //Commit
        db.ProductDesignGJ_CharDesign.InsertOnSubmit(n);
        db.SubmitChanges();
    }
}