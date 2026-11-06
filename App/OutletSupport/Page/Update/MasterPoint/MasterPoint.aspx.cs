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
            fillForm();            
		}
	}

    protected void fillForm()
    {
        var d = db.PointRedeem.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            point.Text = d.Point.HasValue ? Str.Number0(d.Point) : point.Text;

            rupiah.Text = d.Rupiah.HasValue ? Str.Number0(d.Rupiah) : rupiah.Text;
        }
    }
	//Save
	protected bool valid {
		get { return Fv.isDecimal(rupiah, rupiah_ERR); }
	}

	protected void save_Click(object sender, EventArgs e) {
        if (valid)
        {
            var s = db.PointRedeem.SingleOrDefault(p => p.ID == myID);
            if (s != null)
            {
                s.PointPending = Convert.ToDecimal(point.Text);
                s.RupiahPending = Convert.ToDecimal(rupiah.Text);
                s.Operator = App.UserNama;
                s.OperatorTgl = DateTime.Now;
                s.Approval = null;
                s.ApprovalTgl = null;

                db.SubmitChanges();               

                //Redirect
                Response.Redirect("Home.aspx?d=1");
            }
        }
        else
        {
            err_tr.Visible = true;
            err.Text = "<span class=\"text-danger\">Please complete the form.</span>";
        }
	   
	}

}