using System;
using System.Linq;

public partial class Page_UpdateProcess_FF : System.Web.UI.Page {
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
			me_min.Focus();

			//Number-Format
			App.NumberFormat(me_min);
            App.NumberFormat(me_max);
            App.NumberFormat(me);


			//Form
			fillForm();
		}
	}
	protected void fillForm() {
		var d = db.ASPPG.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {

			me_min.Text = d.MEPeriodeMin.ToString("N0");
            me_max.Text = d.MEPeriodeMax.ToString("N0");
            me.Text = d.MERumus.ToString("N0");
		}
	}

	
	protected void save_Click(object sender, EventArgs e) {
			var d = db.ASPPG.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
                ASPPGLog n = new ASPPGLog();
                n.IDMaster = d.ID;
                n.MEPeriodeMinLama = d.MEPeriodeMin;
                n.MEPeriodeMin = Convert.ToInt32(me_min.Text);
                n.MEPeriodeMaxLama = d.MEPeriodeMax;
                n.MEPeriodeMax = Convert.ToInt32(me_max.Text);
                n.MERumusLama = d.MERumus;
                n.MERumus = Convert.ToInt32(me.Text);
                n.Tgl = DateTime.Now;

                db.ASPPGLog.InsertOnSubmit(n);
                db.SubmitChanges();


                d.MEPeriodeMin = Convert.ToInt32(me_min.Text);
                d.MEPeriodeMax = Convert.ToInt32(me_max.Text);
                d.MERumus = Convert.ToDecimal(me.Text);

				//Commit
				db.SubmitChanges();

				//Redirect
				App.ReloadClose(this);
			}
	}
}