using System;
using System.Linq;

public partial class Page_Register_Design_Data01 : System.Web.UI.Page {
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

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			search_customer.Focus();

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Design.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Design_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Search-Button
			string qstr = "?id=" + myID;
			search_customer.HRef += qstr;

			//Disabled-State
			if (d.RequestCustomer > 0) {
				requestcustomernama.Enabled = false;
				requestcustomernama.Text = d.RequestCustomerNama;
			}

            if (d.RequestOutlet > 0)
            {
                requestoutletnama.Enabled = false;
                requestoutletnama.Text = d.RequestOutletNama;
            }
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.ProductDesignGJ.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			
			d.RequestCustomerNama = Str.CapTitle(requestcustomernama.Text);
            d.RequestOutletNama = Str.CapTitle(requestoutletnama.Text);

			//Commit
			db.SubmitChanges();

			//Redirect
            Response.Redirect("Design_CharProduct01.aspx?id=" + myID);
		}
	}
}