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

	protected void Page_Load(object sender, EventArgs e){
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {            
			point.Focus();
            
			//Number-Format
			App.NumberFormat(rupiah);
		}
	}
	//Save
	protected bool valid {
		get { return Fv.isDecimal(rupiah, rupiah_ERR); }
	}

	protected void save_Click(object sender, EventArgs e) {
        PointRedeem n = new PointRedeem();

        n.PointPending = Convert.ToDecimal(point.Text);
        n.RupiahPending = Convert.ToDecimal(rupiah.Text);

        n.Operator = App.UserNama;
        n.OperatorTgl = DateTime.Now;        

        db.PointRedeem.InsertOnSubmit(n);
        db.SubmitChanges();

        App.Reload(this);        
	}
}