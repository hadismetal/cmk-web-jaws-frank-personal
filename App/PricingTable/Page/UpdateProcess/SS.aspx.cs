using System;
using System.Linq;

public partial class Page_UpdateProcess_SS : System.Web.UI.Page {
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
			minimum.Focus();

			//Number-Format
			App.NumberFormat(minimum);

			//Form
			fillForm();
		}
	}
	protected void fillForm() {
		var d = db.PricingTableSS.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_category.InnerHtml = d.CharProductCategory.Nama + " (" + d.CharProductCategory.NamaKode + ")";
			header_level.InnerHtml = d.CharProductLevel.Nama + " (" + d.CharProductLevel.NamaKode + ")";
			header_dist.InnerHtml = d.CharStoneDist.Nama + " (" + d.CharStoneDist.NamaKode + ")";
			header_segmen.InnerHtml = d.CharProductSegmen.Nama + " (" + d.CharProductSegmen.NamaKode + ")";
			header_sistem.InnerHtml = DL.SegmenSistem(d.SegmenSistem);

			if (d.SegmenSistem == 0) {
				header_sistem.Attributes["class"] += " text-success";
				satuan.InnerHtml = "USD";

				minimum.Text = d.Minimum.HasValue ? Str.Number2(d.Minimum) : minimum.Text;
				minimum.Text = d.MinimumPending.HasValue ? Str.Number2(d.MinimumPending) : minimum.Text;
			}
			else {
				header_sistem.Attributes["class"] += " text-orange";
				satuan.InnerHtml = "Carat";

				minimum.Text = d.Minimum.HasValue ? Str.Number3(d.Minimum) : minimum.Text;
				minimum.Text = d.MinimumPending.HasValue ? Str.Number3(d.MinimumPending) : minimum.Text;
			}
		}
	}

	//Save
	protected bool valid {
		get { return Fv.isDecimal(minimum, minimum_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.PricingTableSS.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
				d.MinimumPending = Convert.ToDecimal(minimum.Text);

				//Mutasi
				d.Mutasi = d.Minimum.HasValue ? d.MinimumPending - d.Minimum : d.MinimumPending;
				d.MutasiPersen = d.Minimum != 0 ? (d.Mutasi / d.Minimum) * 100 : null;

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