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

		//Redirect
		Response.Redirect("Summary.aspx?d=1");
	}
	protected void stone1A() {
		var rs = from p in db.Stone1A select p;
		foreach (var r in rs) {
			if (r.StoneCosting1A == null) {
				StoneCosting1A n = new StoneCosting1A();

				n.ID = r.ID;

				//Commit
				db.StoneCosting1A.InsertOnSubmit(n);
				db.SubmitChanges();
			}
		}
	}
	protected void stone1B() {
		var rs = from p in db.Stone1B select p;
		foreach (var r in rs) {
			if (r.StoneCosting1B == null) {
				StoneCosting1B n = new StoneCosting1B();

				n.ID = r.ID;

				//Commit
				db.StoneCosting1B.InsertOnSubmit(n);
				db.SubmitChanges();
			}
		}
	}
	protected void stone2() {
		var rs = from p in db.Stone2 select p;
		foreach (var r in rs) {
			if (r.StoneCosting2 == null) {
				StoneCosting2 n = new StoneCosting2();

				n.ID = r.ID;

				//Commit
				db.StoneCosting2.InsertOnSubmit(n);
				db.SubmitChanges();
			}
		}
	}
	protected void stone3() {
		var rs = from p in db.Stone3 select p;
		foreach (var r in rs) {
			if (r.StoneCosting3 == null) {
				StoneCosting3 n = new StoneCosting3();

				n.ID = r.ID;

				//Commit
				db.StoneCosting3.InsertOnSubmit(n);
				db.SubmitChanges();
			}
		}
	}
	protected void stone4() {
		var rs = from p in db.Stone4 select p;
		foreach (var r in rs) {
			if (r.StoneCosting4 == null) {
				StoneCosting4 n = new StoneCosting4();

				n.ID = r.ID;

				//Commit
				db.StoneCosting4.InsertOnSubmit(n);
				db.SubmitChanges();
			}
		}
	}
	protected void stone5() {
		var rs = from p in db.Stone5 select p;
		foreach (var r in rs) {
			if (r.StoneCosting5 == null) {
				StoneCosting5 n = new StoneCosting5();

				n.ID = r.ID;

				//Commit
				db.StoneCosting5.InsertOnSubmit(n);
				db.SubmitChanges();
			}
		}
	}
}