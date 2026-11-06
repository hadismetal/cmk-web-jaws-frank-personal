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
			qty.Focus();

			//Number-Format
			App.NumberFormat(qty);
            App.NumberFormat(omset);

			//Form
			fillForm();
		}
	}
	protected void fillForm() {
		var d = db.BudgetStock.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_level.InnerHtml = d.CharProductLevel.Nama + " (" + d.CharProductLevel.NamaKode + ")";	
			//header_dist.InnerHtml = d.CharStoneDist.Nama + " (" + d.CharStoneDist.NamaKode + ")";
            header_segmen.InnerHtml = d.CharProductSegmen.Nama + " (" + d.CharProductSegmen.NamaKode + ")";

			qty.Text = d.TargetQty.HasValue ? Str.Number0(d.TargetQty) : qty.Text;
			omset.Text = d.TargetRupiah.HasValue ? Str.Number0(d.TargetRupiah) : omset.Text;
		}
	}

	//Save
	protected bool valid {
		get { return Fv.isDecimal(qty, qty_ERR); }
	}
    protected bool validomset
    {
        get { return Fv.isDecimal(omset, omset_ERR); }
    }
	protected void save_Click(object sender, EventArgs e) {
		if (valid && validomset) {
			var d = db.BudgetStock.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {

                d.TargetQty = Convert.ToDecimal(qty.Text);
                d.TargetRupiah = Convert.ToDecimal(omset.Text);

				//User-Info
				d.OperatorNama = App.UserNama;
				d.OperatorTgl = DateTime.Now;

				//Commit
				db.SubmitChanges();

				//Redirect
				App.ReloadClose(this);
			}
		}
	}
}