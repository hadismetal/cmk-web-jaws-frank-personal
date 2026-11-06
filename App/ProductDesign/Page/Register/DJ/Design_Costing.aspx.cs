using System;
using System.Linq;

public partial class Page_Register_Design_Costing : System.Web.UI.Page {
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
			frame.Focus();

			//Number-Format
			App.NumberFormat(frame);
			App.NumberFormat(mounting);
			App.NumberFormat(setting);
			App.NumberFormat(finishing);

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
		}
	}

	//Save
	protected bool valid {
		get {
			bool x = true;

			x = x && Fv.isDecimal(frame, frame_ERR);
			x = x && Fv.isDecimal(mounting, mounting_ERR);
			x = x && Fv.isDecimal(setting, setting_ERR);
			x = x && Fv.isDecimal(finishing, finishing_ERR);

			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.ProductDesign.SingleOrDefault(p => p.ID == myID && p.Draft);
			if (d == null) DataError();
			else {
				//Process
				//deleteProcess(d);
				//insertProcess(d);

				//Redirect
				//Response.Redirect("Design_Picture.aspx?id=" + myID);
                Response.Redirect("Design_Pricing.aspx?id=" + myID);
			}
		}
	}
	/*protected void deleteProcess(ProductDesign ms) {
		var d = ms.ProductDesign_Costing;
		if (d != null) {
			//Commit
			db.ProductDesign_Costing.DeleteOnSubmit(d);
			db.SubmitChanges();
		}
	}
	protected void insertProcess(ProductDesign ms) {
		ProductDesign_Costing n = new ProductDesign_Costing();

		n.ID = ms.ID;

		n.Frame = Convert.ToDecimal(frame.Text);
		n.Mounting = Convert.ToDecimal(mounting.Text);
		n.Setting = Convert.ToDecimal(setting.Text);
		n.Finishing = Convert.ToDecimal(finishing.Text);

		//Total
		n.TotalBiaya = n.Mounting + n.Setting + n.Finishing;

		//Commit
		db.ProductDesign_Costing.InsertOnSubmit(n);
		db.SubmitChanges();
	}*/
}