using System;
using System.Linq;

public partial class Page_Group_WarehouseCreate : System.Web.UI.Page {
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
	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			nama.Focus();
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		LocWarehouseGroup n = new LocWarehouseGroup();

		n.Nama = Str.CapTitle(nama.Text);
		n.NamaKode = Str.CapUpper(namakode.Text);
		n.Keterangan = Str.CapFirst(keterangan.Text);
		n.SN = db.LocWarehouseGroup.Count() + 1;

		//Commit
		db.LocWarehouseGroup.InsertOnSubmit(n);
		db.SubmitChanges();

		//Redirect
		App.Reload(this);
	}
}