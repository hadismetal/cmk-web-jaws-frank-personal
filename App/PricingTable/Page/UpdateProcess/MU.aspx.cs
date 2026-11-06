using System;
using System.Linq;

public partial class Page_UpdateProcess_MU : System.Web.UI.Page {
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
			persen.Focus();

			//Number-Format
			App.NumberFormat(persen);

			//Form
			fillForm();
		}
	}
	protected void fillForm() {
		var d = db.PricingTableMU.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_category.InnerHtml = d.CharProductCategory.Nama + " (" + d.CharProductCategory.NamaKode + ")";
			header_level.InnerHtml = d.CharProductLevel.Nama + " (" + d.CharProductLevel.NamaKode + ")";
			header_dist.InnerHtml = d.CharStoneDist.Nama + " (" + d.CharStoneDist.NamaKode + ")";

			persen.Text = d.Persen.HasValue ? Str.Number2(d.Persen) : persen.Text;
			persen.Text = d.PersenPending.HasValue ? Str.Number2(d.PersenPending) : persen.Text;
		}
	}

	//Save
	protected bool valid {
		get { return Fv.isDecimal(persen, persen_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.PricingTableMU.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
				d.PersenPending = Convert.ToDecimal(persen.Text);

				//Mutasi
				d.Mutasi = d.Persen.HasValue ? d.PersenPending - d.Persen : d.PersenPending;

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