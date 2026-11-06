using System;
using System.Linq;

public partial class Page_Register_DJ_Data01 : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("DJ.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Approval==null);
		if (d == null) DataError();
		else {

            deleteProcess(d);

			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/DJ_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Search-Button
			string qstr = "?id=" + myID;
			search_outlet.HRef += qstr;
			search_customer.HRef += qstr;

            requestoutletnama.Text = d.RequestOutletNama;
            requestcustomernama.Text = d.RequestCustomerNama;

		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
        var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Approval == null);
		if (d == null) DataError();
		else {
			

			d.RequestOutletNama = Str.CapTitle(requestoutletnama.Text);
			d.RequestCustomerNama = Str.CapTitle(requestcustomernama.Text);

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("DJ_CharProduct01.aspx?id=" + myID);
		}
	}

    protected void deleteProcess(ProductDesign d)
    {
        if (d.ProductDesign_PricingProduct != null)
            db.ProductDesign_PricingProduct.DeleteOnSubmit(
                d.ProductDesign_PricingProduct);

        if (d.ProductDesign_PricingBiaya != null)
            db.ProductDesign_PricingBiaya.DeleteOnSubmit(
                d.ProductDesign_PricingBiaya);

        if (d.ProductDesign_PricingMU != null)
            db.ProductDesign_PricingMU.DeleteOnSubmit(
                d.ProductDesign_PricingMU);

        if (d.ProductDesign_PricingSegmen != null)
            db.ProductDesign_PricingSegmen.DeleteOnSubmit(
                d.ProductDesign_PricingSegmen);

        //Commit
        db.SubmitChanges();
    }
}