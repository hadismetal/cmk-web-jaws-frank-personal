using System;
using System.Linq;

public partial class Page_Register_TradeIn_DataAgentSave : System.Web.UI.Page {
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
    protected int myTipe { get { return App.QueryInt(this, "tipe"); } }

	//Exception
	protected void DataError() { Response.Redirect("TradeIn.aspx"); }
	protected void LinkError() { Response.Redirect("TradeIn_DataAgent.aspx?id=" + myID + "&tipe=" + myTipe); }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.TradeIn.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			saveLink(d);

			//Redirect
			Response.Redirect("TradeIn_Form.aspx?id=" + myID + "&status=1");
		}
	}
	protected void saveLink(TradeIn data) {
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
            if (myTipe == 2)
            {
                data.IDSales2 = d.ID;
                data.SalesNama2 = d.Nama;
            }
            else
            {
                data.IDSales = d.ID;
                data.SalesNama = d.Nama;
            }

			//Commit
			db.SubmitChanges();
		}
	}
}