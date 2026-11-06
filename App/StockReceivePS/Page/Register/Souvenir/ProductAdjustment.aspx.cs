using System;
using System.Linq;

public partial class Page_Register_DJ_Form : System.Web.UI.Page {
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
	protected int myIDWarehouse { get { return App.QueryInt(this, "idwarehouse"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			tgl.Focus();

			//Form
			fillForm();

			//Form-Default
			tgl.Text = Str.Tgl(DateTime.Today);
		}
	}

	//Form
	protected void fillForm() {
		
	}

	//Save
	protected bool valid {
		get { return Fv.isDateTime(tgl, tgl_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
			AdjustmentSouvenir n = new AdjustmentSouvenir();

			//Trans. Form
            n.TipeLokasi = 0;
            n.IDLokasi = myIDWarehouse;
            //n.IDWarehouse = 1;
			n.Keterangan = Str.CapFirst(keterangan.Text);
            n.StatusAdjustment = Convert.ToInt32(adjtype.SelectedValue);

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;
            n.Status = 0;

			//User-Info
			n.Operator = App.UserNama;
			n.OperatorTgl = DateTime.Now;

			//Commit
			db.AdjustmentSouvenir.InsertOnSubmit(n);
			db.SubmitChanges();

            Response.Redirect("ProductAdjustmentInfo.aspx?id=" + n.ID);
		}
	}
}