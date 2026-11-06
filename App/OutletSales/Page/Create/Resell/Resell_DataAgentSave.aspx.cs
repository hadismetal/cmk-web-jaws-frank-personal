using System;
using System.Linq;

public partial class Page_Register_Resell_DataAgentSave : System.Web.UI.Page {
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
	protected void DataError() { Response.Redirect("Resell.aspx"); }
	protected void LinkError() { Response.Redirect("Resell_DataAgent.aspx?id=" + myID); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.Resell.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			saveLink(d);

			//Redirect
			Response.Redirect("Resell_Form.aspx?id=" + myID + "&status=1");
		}
	}
	protected void saveLink(Resell data) {
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

		var d = db.DataSales.SingleOrDefault(
			p =>
			p.ID == myLink && !p.Disable &&
			p.IDBrand == IDBrand
			);

		if (d == null) LinkError();
		else {
			data.IDSales = d.ID;
			data.SalesNama = d.Nama;

			//Commit
			db.SubmitChanges();
		}
	}
}