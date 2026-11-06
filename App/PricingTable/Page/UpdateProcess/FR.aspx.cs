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
			biaya.Focus();

			//Number-Format
			App.NumberFormat(biaya);
            App.NumberFormat(costing);

			//Form
			fillForm();
		}
	}
	protected void fillForm() {
		var d = db.PricingTableRepair.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_repair.InnerHtml = d.CharProcessRepair.Nama + " (" + d.CharProcessRepair.NamaKode + ")";

			biaya.Text = d.Biaya.HasValue ? Str.Number0(d.Biaya) : biaya.Text;
			biaya.Text = d.BiayaPending.HasValue ? Str.Number0(d.BiayaPending) : biaya.Text;
            costing.Text = d.Costing.HasValue ? Str.Number0(d.Costing) : costing.Text;
            costing.Text = d.CostingPending.HasValue ? Str.Number0(d.CostingPending) : costing.Text;
		}
	}

	//Save
	protected bool valid {
		get { return Fv.isDecimal(biaya, biaya_ERR); }
	}
    protected bool validCosting
    {
        get { return Fv.isDecimal(costing, costing_ERR); }
    }
	protected void save_Click(object sender, EventArgs e) {
		if (valid && validCosting) {
			var d = db.PricingTableRepair.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
				d.BiayaPending = Convert.ToDecimal(biaya.Text);
                d.CostingPending = Convert.ToDecimal(costing.Text);

				//Mutasi
				d.Mutasi = d.Biaya.HasValue ? d.BiayaPending - d.Biaya : d.BiayaPending;
				d.MutasiPersen = d.Biaya != 0 ? (d.Mutasi / d.Biaya) * 100 : null;

				//Status
				d.StatusPricing = 2;

				//User-Info
				d.Operator = App.UserNama;
				d.OperatorTgl = DateTime.Now;

				//Commit
				db.SubmitChanges();

				//Redirect
				App.ReloadClose(this);
			}
		}
	}
}