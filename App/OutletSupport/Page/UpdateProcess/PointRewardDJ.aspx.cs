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
			rupiah.Focus();

			//Number-Format
			App.NumberFormat(rupiah);

            //Form
            fillForm();
        }
    }
    protected void fillForm()
    {
        var d = db.PointRewardDJ.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            category.Text = "<span class=\"text-lg text-danger\">" + d.CharProductCategory.Nama + "</span>";

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
            var s = db.PointRewardDJ.SingleOrDefault(p => p.ID == myID);
            if (s != null)
            {

                PointRewardLogDJ n = new PointRewardLogDJ();

                n.IDMaster = s.ID;
                n.RupiahLama = s.Rupiah.Value;
                n.RupiahBaru = Convert.ToDecimal(rupiah.Text);
                n.Approval = App.UserNama;
                n.Tgl = DateTime.Now;

                db.PointRewardLogDJ.InsertOnSubmit(n);
                db.SubmitChanges();

                //Commit
                db.SubmitChanges();

                s.Rupiah = Convert.ToDecimal(rupiah.Text);
                s.Operator = App.UserNama;
                s.OperatorTgl = DateTime.Now;
                s.Approval = App.UserNama;
                s.ApprovalTgl = DateTime.Now;

                db.SubmitChanges();

                

                //Redirect
                App.ReloadClose(this);
            }
        }
        else
        {
            err_tr.Visible = true;
            err.Text = "<span class=\"text-danger\">Please complete the form.</span>";
        }
	}
}