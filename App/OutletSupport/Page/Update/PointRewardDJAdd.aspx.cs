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

            DLC.CharProductCategory(category);

			//Number-Format
			App.NumberFormat(rupiah);
		}
	}

	//Save
	protected bool valid {
		get { return Fv.isDecimal(rupiah, rupiah_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
        if (valid && category.SelectedValue != "0")
        {
            var s = db.PointRewardDJ.SingleOrDefault(p => p.IDCategory == Convert.ToInt32(category.SelectedValue));
            if (s == null)
            {
                PointRewardDJ d = new PointRewardDJ();

                d.IDCategory = Convert.ToInt32(category.SelectedValue);
                d.Rupiah = Convert.ToDecimal(rupiah.Text);
                d.Operator = App.UserNama;
                d.OperatorTgl = DateTime.Now;
                d.Approval = App.UserNama;
                d.ApprovalTgl = DateTime.Now;

                db.PointRewardDJ.InsertOnSubmit(d);
                db.SubmitChanges();

                PointRewardLogDJ n = new PointRewardLogDJ();

                n.IDMaster = d.ID;
                n.RupiahLama = 0m;
                n.RupiahBaru = Convert.ToDecimal(rupiah.Text);
                n.Approval = App.UserNama;
                n.Tgl = DateTime.Now;

                db.PointRewardLogDJ.InsertOnSubmit(n);
                db.SubmitChanges();

                //Commit
                db.SubmitChanges();

                //Redirect
                App.ReloadClose(this);
            }
            else
            {
                err_tr.Visible = true;
                err.Text = "<span class=\"text-danger\">Product Category Already Exists</span>";
            }
        }
        else
        {
            err_tr.Visible = true;
            err.Text = "<span class=\"text-danger\">Please complete the form.</span>";
        }
	}
}