using System;
using System.Linq;

public partial class Page_Settings_Size01Update : System.Web.UI.Page {
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
			sizemin.Focus();

			//DLC
			DLC.Rapaport(rapaport);

			//Number-Format
			App.NumberFormat(sizemin);
			App.NumberFormat(sizemax);

			//Form
			fillForm();
		}
	}
	protected void fillForm() {
		var d = db.ParcelRepair02.SingleOrDefault(p => p.ID == myID && !p.Disable);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_nama.InnerHtml = d.Nama + " (" + d.NamaKode + ")";

			sizemin.Text = Str.Number3(d.SizeMin);
			sizemax.Text = Str.Number3(d.SizeMax);

			if (!d.Rapaport.HasValue)
				rapaport.SelectedIndex = 1;
			else
				rapaport.SelectedIndex = d.Rapaport.Value ? 1 : 0;
		}
	}

	//Save
	protected bool valid {
		get {
			bool x = true;

			x = x && Fv.isDecimal(sizemin, sizemin_ERR);
			x = x && Fv.isDecimal(sizemax, sizemax_ERR);

			return x;
		}
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.ParcelRepair02.SingleOrDefault(p => p.ID == myID && !p.Disable);
			if (d == null) App.DataNull(this);
			else {
				d.SizeMin = Convert.ToDecimal(sizemin.Text);
				d.SizeMax = Convert.ToDecimal(sizemax.Text);
				d.Rapaport = rapaport.SelectedIndex == 1;

				//Commit
				db.SubmitChanges();

				//Redirect
				App.ReloadClose(this);
			}
		}
	}
}