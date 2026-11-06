using System;
using System.Linq;

public partial class Page_Register_Mounting_Data02 : System.Web.UI.Page {
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
			search_designer.Focus();

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Mounting.aspx"); }

	//Form
	protected void fillForm() {
        var d = db.StockReceiveMounting.SingleOrDefault(p => p.ID == myID && p.Approval == null);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Mounting_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Search-Button
			string qstr = "?id=" + myID;
			search_designer.HRef += qstr;
			search_stock.HRef += qstr;

			//Disabled-State
			if (d.HumanDesigner > 0) {
				humandesignernama.Enabled = false;
				humandesignernama.Text = d.HumanDesignerNama;
			}
			if (d.HumanStock > 0) {
				humanstocknama.Enabled = false;
				humanstocknama.Text = d.HumanStockNama;
			}

            if (d.Sumber == 1)
            {
                var x = db.KodeKaret_Detail.SingleOrDefault(p=>p.ID == d.Proto);
                if(x!=null)
                {
                    humandesignernama.Text = x.ProductOrder.DataHuman.Nama;
                }
            }

            humanstocknama.Text = d.HumanStockNama;
            humandesignernama.Text = d.HumanDesignerNama;


			//Expired
			expiredproduct.Text = Str.Tgl(d.Tgl);
            expiredrequest.Text = Str.Tgl(d.ExpiredRequest);
		}
	}

	//Save
	protected bool valid {
		get {
			bool x = true;

			x = x && Fv.isDateTime(expiredproduct, expiredproduct_ERR);
			x = x && Fv.isDateTime(expiredrequest, expiredrequest_ERR, true);

			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
            var d = db.StockReceiveMounting.SingleOrDefault(p => p.ID == myID && p.Approval == null);
			if (d == null) DataError();
			else {
				d.HumanDesignerNama = Str.CapTitle(humandesignernama.Text);
				d.HumanStockNama = Str.CapTitle(humanstocknama.Text);

				d.ExpiredProduct = Convert.ToDateTime(expiredproduct.Text);
				if (expiredrequest.Text != "")
					d.ExpiredRequest = Convert.ToDateTime(expiredrequest.Text);
				else
					d.ExpiredRequest = null;

				//Commit
				db.SubmitChanges();

				//Redirect
				Response.Redirect("Mounting_CharProduct01.aspx?id=" + myID);
			}
		}
	}
}