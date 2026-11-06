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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Manager);

		//Process
		stone1A();
		stone1B();
		stone2();
		stone3();
		stone4();
		stone5();
        stoneRepair();

		//Redirect
		Response.Redirect("Summary.aspx?d=1");
	}
	protected void stone1A() {
		var rs = from p in db.Stone1A select p;
		foreach (var r in rs) {
			if (r.StonePricing1A == null) {
				StonePricing1A n = new StonePricing1A();

				n.ID = r.ID;

				//Commit
				db.StonePricing1A.InsertOnSubmit(n);
				db.SubmitChanges();
			}
		}
	}
	protected void stone1B() {
		var rs = from p in db.Stone1B select p;
		foreach (var r in rs) {
			if (r.StonePricing1B == null) {
				StonePricing1B n = new StonePricing1B();

				n.ID = r.ID;

				//Commit
				db.StonePricing1B.InsertOnSubmit(n);
				db.SubmitChanges();
			}
		}
	}
	protected void stone2() {
		var rs = from p in db.Stone2 select p;
		foreach (var r in rs) {
			if (r.StonePricing2 == null) {
				StonePricing2 n = new StonePricing2();

				n.ID = r.ID;

				//Commit
				db.StonePricing2.InsertOnSubmit(n);
				db.SubmitChanges();
			}
		}
	}
	protected void stone3() {
		var rs = from p in db.Stone3 select p;
		foreach (var r in rs) {
			if (r.StonePricing3 == null) {
				StonePricing3 n = new StonePricing3();

				n.ID = r.ID;

				//Commit
				db.StonePricing3.InsertOnSubmit(n);
				db.SubmitChanges();
			}
		}
	}
	protected void stone4() {
		var rs = from p in db.Stone4 select p;
		foreach (var r in rs) {
			if (r.StonePricing4 == null) {
				StonePricing4 n = new StonePricing4();

				n.ID = r.ID;

				//Commit
				db.StonePricing4.InsertOnSubmit(n);
				db.SubmitChanges();
			}
		}
	}
	protected void stone5() {
		var rs = from p in db.Stone5 select p;
		foreach (var r in rs) {
			if (r.StonePricing5 == null) {
				StonePricing5 n = new StonePricing5();

				n.ID = r.ID;

				//Commit
				db.StonePricing5.InsertOnSubmit(n);
				db.SubmitChanges();
			}
		}
	}
    protected void stoneRepair()
    {
        var rs = from p in db.StoneRepair select p;
        foreach (var r in rs)
        {
            if (r.StonePricingRepair == null)
            {
                StonePricingRepair n = new StonePricingRepair();

                n.ID = r.ID;

                //Commit
                db.StonePricingRepair.InsertOnSubmit(n);
                db.SubmitChanges();
            }
        }
    }
}