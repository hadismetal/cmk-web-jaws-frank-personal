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
            App.NumberFormat(discount);
            App.NumberFormat(birthday);

            //Form
            fillForm();
        }
    }
    protected void fillForm()
    {
        var d = db.MemberTiering.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            tipecustomer.Text = "<span class=\"text-lg text-danger\">" + DL.MemberTiering(d.TipeCustomer) + "</span>";

            rupiah.Text = Str.Number0(d.Rupiah);
            discount.Text = Str.Number2(d.Discount);
            birthday.Text = Str.Number0(d.BirthdayRupiah);
        }
    }

	//Save
	protected bool valid {
		get { return Fv.isDecimal(rupiah, rupiah_ERR); }
	}
    protected bool validPersen
    {
        get { return Fv.isDecimal(discount, discount_ERR); }
    }
    protected bool validBirthday
    {
        get { return Fv.isDecimal(birthday, birthday_ERR); }
    }
	protected void save_Click(object sender, EventArgs e) {
        if (valid)
        {
            var s = db.MemberTiering.SingleOrDefault(p => p.ID == myID);
            if (s != null)
            {

                MemberTieringLog n = new MemberTieringLog();

                n.IDMaster = s.ID;
                n.RupiahLama = s.Rupiah;
                n.RupiahBaru = Convert.ToDecimal(rupiah.Text);
                n.DiscountLama = s.Discount;
                n.DiscountBaru = Convert.ToDecimal(discount.Text);
                n.BirthdayLama = s.BirthdayRupiah;
                n.DiscountBaru = Convert.ToDecimal(birthday.Text);
                n.Approval = App.UserNama;
                n.Tgl = DateTime.Now;

                db.MemberTieringLog.InsertOnSubmit(n);
                db.SubmitChanges();

                //Commit
                db.SubmitChanges();

                s.Rupiah = Convert.ToDecimal(rupiah.Text);
                s.Discount = Convert.ToDecimal(discount.Text);
                s.BirthdayRupiah = Convert.ToDecimal(birthday.Text);
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