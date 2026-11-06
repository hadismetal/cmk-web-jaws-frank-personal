using System;
using System.Linq;

public partial class Page_Register_LD_Data02 : System.Web.UI.Page {
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

			//Form
			fillForm();
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("LD.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			//Header
			header_nomor.InnerHtml =
				"<a href=\"../History/LD_File.aspx?id=" + d.ID + "\" target=\"_blank\"><i class=\"icon-file-alt\"></i> "
				+ d.Nomor + "</a>";

			//Search-Button
			string qstr = "?id=" + myID;
			search_stock.HRef += qstr;

			//Expired
			expiredproduct.Text = Str.Tgl(d.Tgl.AddYears(1));
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
			var d = db.StockReceiveLD.SingleOrDefault(p => p.ID == myID && p.Draft);
			if (d == null) DataError();
			else {
				d.HumanStockNama = Str.CapTitle(humanstocknama.Text);

				d.ExpiredProduct = Convert.ToDateTime(expiredproduct.Text);
				if (expiredrequest.Text != "")
					d.ExpiredRequest = Convert.ToDateTime(expiredrequest.Text);
				else
					d.ExpiredRequest = null;

				//Commit
				db.SubmitChanges();

				//Redirect
				Response.Redirect("LD_Stone.aspx?id=" + myID);
			}
		}
	}
}