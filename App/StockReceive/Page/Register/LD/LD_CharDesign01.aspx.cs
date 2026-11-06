using System;
using System.Linq;

public partial class Page_Register_LD_CharDesign01 : System.Web.UI.Page
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
    protected void DataError() { Response.Redirect("LD.aspx"); }

    //Form
    protected void fillForm()
    {
        var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Header
            header_nomor.InnerHtml =
                "<a href=\"../History/LD_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                + d.Nomor + "</a>";

        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (d == null) DataError();
        else
        {
            //Process
            deleteProcess(d);
            insertProcess(d);

            //Redirect
            Response.Redirect("LD_CharDesign02.aspx?id=" + myID);
        }
    }
    protected void deleteProcess(StockReceiveLD ms)
    {
        var d = ms.StockReceiveLD_CharDesign;
        if (d != null)
        {
            //Commit
            db.StockReceiveLD_CharDesign.DeleteOnSubmit(d);
            db.SubmitChanges();
        }
    }
    protected void insertProcess(StockReceiveLD ms)
    {
        StockReceiveLD_CharDesign n = new StockReceiveLD_CharDesign();

        n.ID = ms.ID;

        n.DesignCategory = Convert.ToInt32(designcategory.SelectedValue);
        n.DesignConcept = Convert.ToInt32(designconcept.SelectedValue);
        n.DesignProcess = Convert.ToInt32(designprocess.SelectedValue);
        n.TargetAge = Convert.ToInt32(targetage.SelectedValue);
        n.TargetGender = Convert.ToInt32(targetgender.SelectedValue);

        //Commit
        db.StockReceiveLD_CharDesign.InsertOnSubmit(n);
        db.SubmitChanges();
    }
}