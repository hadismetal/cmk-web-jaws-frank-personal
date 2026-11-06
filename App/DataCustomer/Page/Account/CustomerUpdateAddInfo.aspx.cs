using System;
using System.Linq;

public partial class Page_Account_CustomerUpdateAddr : System.Web.UI.Page {
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

		if (!IsPostBack) {
			namapasangan.Focus();
            DLC.MemberTiering(ddltipecustomer);

			//Form
			fillForm();

			//Action
			save.Visible = Role.Operator;
		}
	}
	protected void fillForm() {
		var d = db.DataCustomer.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			h1_st.Text = d.Nama;
			h1_id.Text = Str.ID_Account(d.ID);

			namapasangan.Text = d.NamaPasangan;
            tgllahirpasangan.Text = Str.Tgl(d.TglLahirPasangan);
            weddinganniversarry.Text = Str.Tgl(d.WeddingAnniversarry);
            ddltipecustomer.SelectedValue = d.TipeCustomer.ToString();
            nomember.Text = d.NoMember;
            nonpwp.Text = d.NoNPWP;
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
		var d = db.DataCustomer.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			d.NamaPasangan = Str.CapTitle(namapasangan.Text);
            if (tgllahirpasangan.Text != "")
                d.TglLahirPasangan = Convert.ToDateTime(tgllahirpasangan.Text);
            if (weddinganniversarry.Text != "")
            d.WeddingAnniversarry = Convert.ToDateTime(weddinganniversarry.Text);

            d.NoMember = nomember.Text;
            d.NoNPWP = nonpwp.Text;
            d.TipeCustomer = Convert.ToInt32(ddltipecustomer.SelectedValue);

			//Commit
			db.SubmitChanges();

			//Redirect
			App.Reload(this, myID);
		}
	}
}