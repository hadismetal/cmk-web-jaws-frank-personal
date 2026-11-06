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
		var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Design_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Search-Button
			string qstr = "?id=" + myID;
			search_customer.HRef += qstr;
            search_outlet.HRef += qstr;

			//Disabled-State
			if (d.RequestCustomer > 0) {
				requestcustomernama.Enabled = false;
				requestcustomernama.Text = d.RequestCustomerNama;
                clearcustomer.Visible = true;
			}

            if (d.RequestOutlet > 0)
            {
                requestoutletnama.Enabled = false;
                requestoutletnama.Text = d.RequestOutletNama;
                clearoutlet.Visible = true;
            }
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Draft);
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

    protected void clearoutlet_Click(object sender, EventArgs e)
    {
        var outlet = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (outlet == null) DataError();
        else
        {
            outlet.RequestOutlet = null;
            outlet.RequestOutletNama = null;
            db.SubmitChanges();
        }
        Response.Redirect("Design_Data01.aspx?id=" + myID);
    }

    protected void clearcustomer_Click(object sender, EventArgs e)
    {
        var cust = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Draft);
        if (cust == null) DataError();
        else
        {
            cust.RequestCustomer = 0;
            cust.RequestCustomerNama = null;
            db.SubmitChanges();
        }
        Response.Redirect("Design_Data01.aspx?id=" + myID);
    }
}