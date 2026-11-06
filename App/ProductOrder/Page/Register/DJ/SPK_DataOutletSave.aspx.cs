using System;
using System.Linq;

public partial class Page_Register_Design_DataOutletSave : System.Web.UI.Page {
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
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }

    protected int loctipe { get { return App.QueryInt(this, "loctip"); } }

    //Exception
    protected void DataError() { Response.Redirect("SPK.aspx"); }
    protected void LinkError() { Response.Redirect("SPK_DataOutlet.aspx?id=" + myID +"&err=" + err); }

    string err = string.Empty;
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID && p.Draft);
		if (d == null) DataError();
		else {
			saveLink(d);

			//Redirect
            Response.Redirect("SPK_Data01.aspx?id=" + myID + "&tipe=" + tipe );
		}
	}
	protected void saveLink(ProductOrder data)
    {
       // var idbrand = from p in db.CompanyBrand where !p.Disable select p.ID;
        if (loctipe == 1)
        {
            var d = db.LocOutlet.SingleOrDefault(p =>p.ID == myLink && !p.Disable);
            err += "tipe1";
            if (d == null) LinkError();
            else
            {
                data.RequestOutlet = d.ID;
                data.RequestOutletNama = d.Nama;
                data.RequestExhibition = 0;
                data.RequestExhibitionNama = null;
                //Commit
                db.SubmitChanges();
            }
        }
        else if (loctipe == 2)
        {
            var d = db.LocExhibition.SingleOrDefault(p => p.ID == myLink && !p.Disable);
            err += "tipe2";
            if (d == null) LinkError();
            else
            {
                data.RequestOutlet = 0;
                data.RequestOutletNama = null;
                data.RequestExhibition = d.ID;
                data.RequestExhibitionNama = d.Nama;
                //Commit
                db.SubmitChanges();
            }
        }
    }
}