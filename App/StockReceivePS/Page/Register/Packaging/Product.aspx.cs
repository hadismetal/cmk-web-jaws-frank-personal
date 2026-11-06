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
			StockReceivePackaging n = new StockReceivePackaging();

			//Trans. Form
            n.IDWarehouse = myIDWarehouse;
            //n.IDWarehouse = 1;
			n.Keterangan = Str.CapFirst(keterangan.Text);
			n.Tgl = Convert.ToDateTime(tgl.Text);
            n.TglInput = DateTime.Now;
            n.NoPO = nopo.Text;
            n.NoDO = nodo.Text;

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;
            n.StatusReceive = false;

			//User-Info
			n.Operator = App.UserNama;
			n.OperatorTgl = DateTime.Now;

			//Commit
			db.StockReceivePackaging.InsertOnSubmit(n);
			db.SubmitChanges();

            Response.Redirect("ProductInfo.aspx?id=" + n.ID);
		}
	}
}