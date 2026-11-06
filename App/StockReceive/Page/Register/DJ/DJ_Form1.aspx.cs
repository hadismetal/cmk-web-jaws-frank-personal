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
	protected int myIDWarehouse { get { return App.QueryInt(this, "IDWarehouse"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			keterangan.Focus();

			//Form

			//DLC

			//Form-Default
		}
	}

	//Exception
	//Form

	//Save
	protected void save_Click(object sender, EventArgs e) {
		string test = Db.SingleString(keterangan.Text, Db.Cnn_DB);

		result.Text = test;
	}
	
}