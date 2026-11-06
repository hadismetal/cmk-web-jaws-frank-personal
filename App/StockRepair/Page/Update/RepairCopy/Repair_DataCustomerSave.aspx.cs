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
	protected void DataError() { Response.Redirect("Repair.aspx"); }
	protected void LinkError() { Response.Redirect("Repair_DataCustomer.aspx?id=" + myID); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.DocRepair.SingleOrDefault(p => p.ID == myID && p.Draft.Value);
		if (d == null) DataError();
		else {
			saveLink(d);

			//Redirect
			Response.Redirect("Repair_Form.aspx?id=" + myID + "&status=1");
		}
	}
	protected void saveLink(DocRepair data) {
        int IDBrand = 1;
        if (data.TipeLokasi == 1)
        {
            var Brand = db.LocOutlet.SingleOrDefault(p => p.ID == data.IDLokasi);
            IDBrand = Brand.IDBrand;
        }
        else if (data.TipeLokasi == 2)
        {
            var Brand = db.LocExhibition.SingleOrDefault(p => p.ID == data.IDLokasi);
            IDBrand = Brand.IDBrand;
        }

		var d = db.DataCustomer.SingleOrDefault(
			p =>
			p.ID == myLink && !p.Disable &&
			p.IDBrand == IDBrand
			);

		if (d == null) LinkError();
		else {
			data.IDCustomer = d.ID;
			data.CustomerNama = d.Nama;

			//Commit
			db.SubmitChanges();
		}
	}
}