using System;
using System.Linq;

public partial class Page_Register_GJ_Data01 : System.Web.UI.Page {
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
    protected int myIDKodeKaret { get { return App.QueryInt(this,"idkodekaret"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("GJ.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Approval==null);
		if (d == null) DataError();
		else {

            deleteProcess(d);

			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/GJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Search-Button
			string qstr = "?id=" + myID;
			search_customer.HRef += qstr;

			//Disabled-State
            
			if (d.RequestOutlet > 0) {
				requestoutletnama.Enabled = false;
				requestoutletnama.Text = d.RequestOutletNama;
			}
			if (d.RequestCustomer > 0) {
				requestcustomernama.Enabled = false;
				requestcustomernama.Text = d.RequestCustomerNama;
			}

            requestoutletnama.Text = d.RequestOutletNama;
            requestcustomernama.Text = d.RequestCustomerNama;

		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
        var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Approval == null);
		if (d == null) DataError();
		else {
			

			d.RequestOutletNama = Str.CapTitle(requestoutletnama.Text);
			d.RequestCustomerNama = Str.CapTitle(requestcustomernama.Text);

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("GJ_CharProduct01.aspx?id=" + myID);
		}
	}

    protected void deleteProcess(ProductDesignGJ d)
    {
        if (d.ProductDesignGJ_PricingProduct != null)
            db.ProductDesignGJ_PricingProduct.DeleteOnSubmit(
                d.ProductDesignGJ_PricingProduct);

        if (d.ProductDesignGJ_PricingBiaya != null)
            db.ProductDesignGJ_PricingBiaya.DeleteOnSubmit(
                d.ProductDesignGJ_PricingBiaya);

        if (d.ProductDesignGJ_PricingMU != null)
            db.ProductDesignGJ_PricingMU.DeleteOnSubmit(
                d.ProductDesignGJ_PricingMU);

        if (d.ProductDesignGJ_PricingSegmen != null)
            db.ProductDesignGJ_PricingSegmen.DeleteOnSubmit(
                d.ProductDesignGJ_PricingSegmen);

        //Commit
        db.SubmitChanges();
    }
}