using System;
using System.Linq;

public partial class Page_UpdateProcess_FM : System.Web.UI.Page {
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
			inputno.Focus();

			//Number-Format
			App.NumberFormat(inputno);
			App.NumberFormat(inputro);
            App.NumberFormat(pembagi);

			//Form
			fillForm();
		}
	}
	protected void fillForm() {
		var d = db.PricingTableFM.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_brand.InnerHtml = d.CompanyBrand.Nama + " (" + d.CompanyBrand.NamaKode + ")";
			header_item.InnerHtml = d.CharProductItem.Nama + " (" + d.CharProductItem.NamaKode + ")";
			header_cons.InnerHtml = d.CharProcessCons.Nama + " (" + d.CharProcessCons.NamaKode + ")";

			inputno.Text = d.InputNO.HasValue ? Str.Number0(d.InputNO) : inputno.Text;
			inputno.Text = d.PendingNO.HasValue ? Str.Number0(d.PendingNO) : inputno.Text;

            pembagi.Text = d.Pembagi.HasValue ? Str.Number0(d.Pembagi) : pembagi.Text;
            pembagi.Text = d.PendingPembagi.HasValue ? Str.Number0(d.PendingPembagi) : pembagi.Text;

			inputro.Text = d.InputRO.HasValue ? Str.Number0(d.InputRO) : inputro.Text;
			inputro.Text = d.PendingRO.HasValue ? Str.Number0(d.PendingRO) : inputro.Text;
		}
	}

	//Save
	protected bool valid {
        get { return Fv.isDecimal(inputno, inputno_ERR) && Fv.isDecimal(inputro, inputro_ERR) && Fv.isDecimal(pembagi, pembagi_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			var d = db.PricingTableFM.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
				d.PendingNO = Convert.ToDecimal(inputno.Text);
				d.PendingRO = Convert.ToDecimal(inputro.Text);
				d.BiayaPending = d.PendingNO + d.PendingRO;
                d.PendingPembagi = Convert.ToDecimal(pembagi.Text);

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