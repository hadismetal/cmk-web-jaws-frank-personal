using System;
using System.Linq;

public partial class Page_Register_Repair_DataDesignerSave : System.Web.UI.Page {
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
	protected int myLink { get { return App.QueryInt(this, "link"); } }

	//Exception
	protected void DataError() { Response.Redirect("RepairInvoice.aspx"); }
	protected void LinkError() { Response.Redirect("RepairInvoice_DataCustomer.aspx?id=" + myID); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.DocRepairResult.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			saveLink(d);

			//Redirect
			Response.Redirect("RepairInvoice_Form.aspx?id=" + myID + "&status=1");
		}
	}
	protected void saveLink(DocRepairResult data) {

		var d = db.DocRepair.SingleOrDefault(
			p =>
			p.ID == myLink 
			);

		if (d == null) LinkError();
		else {
			data.IDWorkOrder = d.ID;
            if (d.Sumber == 0)
            {
                data.IDCustomer = d.IDCustomer;
                data.IDSales = d.IDSales;
                data.SalesNama = d.SalesNama;
                data.CustomerNama = d.CustomerNama;
            }
            data.IDLokasi = d.IDLokasi;
            data.IDProduct = d.IDProduct;
            data.ImgPicture = d.ImgPicture;
            data.Sumber = d.Sumber;

			//Commit
			db.SubmitChanges();
		}
	}
}