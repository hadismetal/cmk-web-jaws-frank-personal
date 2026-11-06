using System;
using System.Linq;

public partial class Page_Register_SPK_Data01 : System.Web.UI.Page {
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
			search_customer.Focus();

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("SPK.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.ProductOrderGJ.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
            if (tipe == 1)
            {
                header_nomor.InnerHtml =
                    "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                    + d.ProductDesignGJ.Nomor + "</a>";
            }
            else
            {
                header_nomor.InnerHtml =
                    "<a href=\"../History/SPK_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
                    + d.KodeKaretGJ.Nomor + "</a>";
            }

			//Search-Button
			string qstr = "?id=" + myID;
			search_customer.HRef += qstr;

			//Disabled-State
			//if (d.ProductDesignGJ.RequestCustomer > 0) {
				//requestcustomernama.Enabled = false;
            if (tipe == 1)
            {
                requestcustomernama.Text = d.ProductDesignGJ.RequestCustomerNama;
                requestoutletnama.Text = d.ProductDesignGJ.RequestOutletNama;
            }
			//}
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.ProductOrderGJ.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			
			d.RequestCustomerNama = Str.CapTitle(requestcustomernama.Text);
            d.RequestOutletNama = Str.CapTitle(requestoutletnama.Text);

			//Commit
			db.SubmitChanges();

			//Redirect
            Response.Redirect("SPK_CharProduct01.aspx?id=" + myID + "&tipe=" + tipe);
		}
	}
}