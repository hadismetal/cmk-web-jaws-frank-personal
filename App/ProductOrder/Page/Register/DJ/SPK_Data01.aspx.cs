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

	protected void Page_Load(object sender, EventArgs e)
    {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack)
        {         
			search_customer.Focus();
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

			//Search-Button
            string qstr = "?id=" + myID + "&tipe=" + tipe;
            search_outlet.HRef += qstr;
			search_customer.HRef += qstr;
            search_po.HRef += qstr;
            //clear_customer.HRef += qstr;
            //clear_outlet.HRef += qstr;
            //clear_po.HRef += qstr;

            //Disabled-State
            //if (d.ProductDesign.RequestCustomer > 0) {
            //requestcustomernama.Enabled = false;

            if (d.RequestOutlet.HasValue)
            {
                if (d.RequestOutlet.Value > 0)
                {
                    requestoutletnama.Text = d.RequestOutletNama.ToString();
                    clearoutlet.Visible = true;
                }
            }

            if (d.RequestExhibition.HasValue)
            {
                if (d.RequestExhibition.Value > 0)
                {
                    requestoutletnama.Text = d.RequestExhibitionNama.ToString();
                    clearoutlet.Visible = true;
                }
            }

            //if (tipe == 1)
            //{

            if ((d.NoPO != null) && (d.NoPO != ""))
            {
                nopo.Text = d.NoPO;

                if ((d.RequestCustomerNama != null) && (d.RequestCustomerNama != ""))
                {
                    requestcustomernama.Text = d.RequestCustomerNama;
                }

                clearpo.Visible = true;
                clearoutlet.Visible = false;
                clearcustomer.Visible = false;
                search_outlet.Disabled = true;
                search_customer.Disabled = true;
                
            }
            else
            {
                if ((d.RequestCustomerNama != null) && (d.RequestCustomerNama != ""))
                {
                    requestcustomernama.Text = d.RequestCustomerNama;
                    //clear_customer.Visible = true;
                    clearcustomer.Visible = true;
                }
                else
                {
                    requestcustomernama.Text = "";
                    clearcustomer.Visible = false;
                }
            }
           
            //if (d.RequestCustomer > 0)
            //{
            //    var u = db.DataCustomer.SingleOrDefault(p => p.ID == d.RequestCustomer);
            //    requestcustomernama.Text = u.Nama;
            //}
			//}
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e)
    {
		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else
        {		
			d.RequestCustomerNama = Str.CapTitle(requestcustomernama.Text);
            d.RequestOutletNama = Str.CapTitle(requestoutletnama.Text);

			//Commit
			db.SubmitChanges();

            //Redirect
            //Response.Redirect("SPK_CharProduct01.aspx?id=" + myID + "&tipe=" + tipe);
            Response.Redirect("SPK_Data02.aspx?id=" + myID + "&tipe=" + tipe);
        }
	}

    protected void clearpo_Click(object sender, EventArgs e)
    {
        var po = db.ProductOrder.SingleOrDefault(p => p.ID == myID);
        if (po != null)
        {
            po.NoPO = null;
            po.RequestOutlet = 0;
            po.RequestOutletNama = null;
            po.RequestCustomer = 0;
            po.RequestCustomerNama = null;
            po.RequestExhibition = 0;
            po.RequestExhibitionNama = null;
            db.SubmitChanges();
        }
        Response.Redirect("SPK_Data01.aspx?id=" + myID + "&status=1&tipe=" + tipe + "");
    }
    protected void clearcustomer_Click(object sender, EventArgs e)
    {
        var cust = db.ProductOrder.SingleOrDefault(p => p.ID == myID);
        if (cust != null)
        {
            cust.RequestCustomer = 0;
            cust.RequestCustomerNama = null;
            db.SubmitChanges();

        }
        Response.Redirect("SPK_Data01.aspx?id=" + myID + "&status=1&tipe=" + tipe + "");
    }

    protected void clearoutlet_Click(object sender, EventArgs e)
    {
        var outlet = db.ProductOrder.SingleOrDefault(p => p.ID == myID);
        if (outlet != null)
        {
            outlet.RequestOutlet = 0;
            outlet.RequestOutletNama = null;
            outlet.RequestExhibition = 0;
            outlet.RequestExhibitionNama = null;
            db.SubmitChanges();

        }
        Response.Redirect("SPK_Data01.aspx?id=" + myID + "&status=1&tipe=" + tipe + "");
    }
}