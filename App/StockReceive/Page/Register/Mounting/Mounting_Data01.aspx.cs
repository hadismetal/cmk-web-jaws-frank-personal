using System;
using System.Linq;

public partial class Page_Register_Mounting_Data01 : System.Web.UI.Page {
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
			search_proto.Focus();

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Mounting.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.StockReceiveMounting.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/Mounting_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Search-Button
			string qstr = "?id=" + myID;
			search_proto.HRef += qstr;
			search_productorder.HRef += qstr;
			search_supplier.HRef += qstr;
			search_outlet.HRef += qstr;
			search_customer.HRef += qstr;

			//Disabled-State
			if (d.Proto > 0) {
				protonomor.Enabled = false;
				protonomor.Text = d.ProtoNomor;

                var proto = db.KodeKaret_Detail.SingleOrDefault(p=>p.ID == d.Proto);
                if(proto==null) DataError();
                else
                {
                    requestcustomernama.Text = proto.ProductOrder.RequestCustomerNama;
                    requestoutletnama.Text = proto.ProductOrder.RequestOutletNama;
                    productordernomor.Enabled = false;
                    productordernomor.Text = proto.NoSPK;
                }
			}
			if (d.ProductOrder > 0) {
				productordernomor.Enabled = false;
				productordernomor.Text = d.ProductOrderNomor;
			}
			if (d.Supplier > 0) {
				suppliernama.Enabled = false;
				suppliernama.Text = d.SupplierNama;
			}
			if (d.RequestOutlet > 0) {
				requestoutletnama.Enabled = false;
				requestoutletnama.Text = d.RequestOutletNama;
			}
			if (d.RequestCustomer > 0) {
				requestcustomernama.Enabled = false;
				requestcustomernama.Text = d.RequestCustomerNama;
			}
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.StockReceiveMounting.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			d.ProtoNomor = Str.CapUpper(protonomor.Text);
			d.ProductOrderNomor = Str.CapUpper(productordernomor.Text);

            var sp = (from p in db.DataSupplier
                      where p.Nama.Equals(suppliernama.Text)
                      select p);
            if (sp.Count() > 0)
            {
                var spl = sp.First();
                d.Supplier = spl.ID;
            }

			d.SupplierNama = Str.CapTitle(suppliernama.Text);
			d.SupplierNomor = Str.CapUpper(suppliernomor.Text);

            int UserID = App.UserID;
            var f = from p in db.DataSK
                    where p.IDUser == UserID
                    select p;
            if (f.Count() > 0)
            {
                var sk = f.First();
                d.HumanStock = sk.ID;
                d.HumanStockNama = sk.Nama;
            }

			d.RequestOutletNama = Str.CapTitle(requestoutletnama.Text);
			d.RequestCustomerNama = Str.CapTitle(requestcustomernama.Text);

			//Commit
			db.SubmitChanges();

			//Redirect
			Response.Redirect("Mounting_Data02.aspx?id=" + myID);
		}
	}
}