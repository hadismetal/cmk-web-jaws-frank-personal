using System;
using System.Linq;

public partial class Page_Register_SPK_CharDesign01 : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
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
	protected void DataError() { Response.Redirect("SPK.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
            //if (tipe == 1)
            //{
            //    header_nomor.InnerHtml =
            //        "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //        + d.ProductDesign.Nomor + "</a>";
            //}
            //else
            //{
            //    header_nomor.InnerHtml =
            //        "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
            //        + d.KodeKaretDJ.Nomor + "</a>";
            //}
		}
        if (tipe == 1)
        {
            var dsg = db.ProductDesign_CharDesign.SingleOrDefault(p => p.ID == d.IDProductDesign);
            if (dsg == null) DataError();
            else
            {
                designcategory.SelectedValue = dsg.DesignCategory.ToString();
                designconcept.SelectedValue = dsg.DesignConcept.ToString();
                designprocess.SelectedValue = dsg.DesignProcess.ToString();
                targetage.SelectedValue = dsg.TargetAge.ToString();
                targetgender.SelectedValue = dsg.TargetGender.ToString();
            }
        }
        else if (tipe == 2)
        {
            var ro = db.KodeKaretDJ_CharDesign.SingleOrDefault(p => p.ID == d.IDKodeKaret);
            if (ro == null) DataError();
            else
            {
                //designcategory.SelectedValue = ro.DesignCategory.ToString();
                //designconcept.SelectedValue = ro.DesignConcept.ToString();
                //designprocess.SelectedValue = ro.DesignProcess.ToString();
                //targetage.SelectedValue = ro.TargetAge.ToString();
                //targetgender.SelectedValue = ro.TargetGender.ToString();

                var lastro = db.ProductOrder.OrderByDescending(p => p.ID).FirstOrDefault(p => p.IDKodeKaret == d.IDKodeKaret && p.Approval != null && p.StatusOrder != null && p.KodeKaretLama != null);

                designcategory.SelectedValue = lastro.ProductOrder_CharDesign.DesignCategory.ToString();
                designconcept.SelectedValue = lastro.ProductOrder_CharDesign.DesignConcept.ToString();
                designprocess.SelectedValue = lastro.ProductOrder_CharDesign.DesignProcess.ToString();
                targetage.SelectedValue = lastro.ProductOrder_CharDesign.TargetAge.ToString();
                targetgender.SelectedValue = lastro.ProductOrder_CharDesign.TargetGender.ToString();
            }
        }
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Process
			deleteProcess(d);
			insertProcess(d);

			//Redirect
			Response.Redirect("SPK_CharDesign02.aspx?id=" + myID + "&tipe=" + tipe);
		}
	}
	protected void deleteProcess(ProductOrder ms) {
		var d = ms.ProductOrder_CharDesign;
		if (d != null) {
			//Commit
			db.ProductOrder_CharDesign.DeleteOnSubmit(d);
			db.SubmitChanges();
		}
	}
	protected void insertProcess(ProductOrder ms) {
		ProductOrder_CharDesign n = new ProductOrder_CharDesign();

		n.ID = ms.ID;

		n.DesignCategory = Convert.ToInt32(designcategory.SelectedValue);
		n.DesignConcept = Convert.ToInt32(designconcept.SelectedValue);
		n.DesignProcess = Convert.ToInt32(designprocess.SelectedValue);
		n.TargetAge = Convert.ToInt32(targetage.SelectedValue);
		n.TargetGender = Convert.ToInt32(targetgender.SelectedValue);
        n.GrafirType = "";

		//Commit
		db.ProductOrder_CharDesign.InsertOnSubmit(n);
		db.SubmitChanges();
	}
}