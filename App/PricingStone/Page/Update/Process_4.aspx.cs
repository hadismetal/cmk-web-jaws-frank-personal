using System;
using System.Linq;

public partial class Page_Update_Process_4 : System.Web.UI.Page {
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
			harga.Focus();

			//Number-Format
			App.NumberFormat(harga);

			//Form
			fillForm();
		}
	}
	protected void fillForm() {
		var d = db.StonePricing4.SingleOrDefault(p => p.ID == myID && !p.Stone4.Disable);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_nama.InnerHtml = d.Stone4.Nama;

			harga.Text = d.Harga.HasValue ? Str.Number2(d.Harga) : harga.Text;
			harga.Text = d.HargaPending.HasValue ? Str.Number2(d.HargaPending) : harga.Text;
		}
	}

	//Save
	protected bool valid {
		get { return Fv.isDecimal(harga, harga_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.StonePricing4.SingleOrDefault(p => p.ID == myID && !p.Stone4.Disable);
			if (d == null) App.DataNull(this);
			else {
				d.HargaPending = Convert.ToDecimal(harga.Text);

				//Mutasi
				d.Mutasi = d.Harga.HasValue ? d.HargaPending - d.Harga : d.HargaPending;
				d.MutasiPersen = d.Harga != 0 ? (d.Mutasi / d.Harga) * 100 : null;

				//Status
				d.Stone4.StatusPricing = 2;

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