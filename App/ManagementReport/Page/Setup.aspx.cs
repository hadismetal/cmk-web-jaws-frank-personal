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

		setup();

		//Redirect
		Response.Redirect("Summary.aspx?d=1");
	}

	//Setup-Flag
	protected void setup() {
        var pl = from p in db.CharProductLevel
                 where !p.Disable
                 select p;
        var sd = from p in db.CharStoneDist
                 where !p.Disable
                 select p;
        var pi = from p in db.CharProductItem
                 where !p.Disable
                 select p;
        var sg = from p in db.CharProductSegmen
                 where !p.Disable
                 select p;
        foreach (var r in pl)
        {
            foreach (var s in sd)
            {
                foreach (var t in pi)
                {
                    foreach (var u in sg)
                    {
                        var item = from p in db.BudgetStock
                                   where p.ProductLevel == r.ID //&& p.StoneDist == s.ID
                                   && p.ProductItem == t.ID && p.Segmen == u.ID
                                   select p;
                        if (item.Count() == 0)
                        {
                            BudgetStock d = new BudgetStock();

                            d.ProductLevel = r.ID;
                            //d.StoneDist = s.ID;
                            d.ProductItem = t.ID;
                            d.Segmen = u.ID;

                            db.BudgetStock.InsertOnSubmit(d);
                            db.SubmitChanges();

                        }
                    }
                }
            }
        }
        Response.Redirect("Summary.aspx?d=1");
	}
}