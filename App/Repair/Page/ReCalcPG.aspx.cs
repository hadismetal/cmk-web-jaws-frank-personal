using System;
using System.Linq;

public partial class Page_Setup : System.Web.UI.Page {
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
    App price = new App();
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

        if(!IsPostBack)
        {
            if(Request.QueryString["d"] != "1")
                setup();

        }
		//Redirect
		Response.Redirect("ReCalcPG.aspx?d=1");
	}

	//Setup-Flag
	protected void setup() {
        var rs = from p in db.StockProductPG
                 where p.StatusPenjualan == 0 && p.ReturnProduct == false && p.StatusLebur == false
                 && p.Legacy == null && p.FixRate == true
                 select p;

        foreach (var r in rs)
        {
            decimal price1 = r.TotalHargaJual.Value;
            decimal price2 = Math.Floor(r.TotalHargaJual.Value);

            if (price1 - price2 > 0)
            {
                price1 = price.RoundTop(price1);
                r.TotalHargaJual = price1;
                db.SubmitChanges();
            }
        }
	}
}