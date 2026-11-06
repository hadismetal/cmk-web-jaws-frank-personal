using System;
using System.Linq;

public partial class Page_Settings_Shape05Update : System.Web.UI.Page {
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
			roundshape.Focus();

			//DLC
			DLC.RoundShape(roundshape);

			//Form
			fillForm();
		}
	}
	protected void fillForm() {
		var d = db.Parcel504.SingleOrDefault(p => p.ID == myID && !p.Disable);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_nama.InnerHtml = d.Nama + " (" + d.NamaKode + ")";

			if (!d.RoundShape.HasValue)
				roundshape.SelectedIndex = 0;
			else
				roundshape.SelectedIndex = d.RoundShape.Value ? 1 : 0;
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.Parcel504.SingleOrDefault(p => p.ID == myID && !p.Disable);
		if (d == null) App.DataNull(this);
		else {
			d.RoundShape = roundshape.SelectedIndex == 1;

			//Commit
			db.SubmitChanges();

			//Redirect
			App.ReloadClose(this);
		}
	}
}