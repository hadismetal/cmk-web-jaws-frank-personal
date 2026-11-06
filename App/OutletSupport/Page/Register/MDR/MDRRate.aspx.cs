using System;
using System.Linq;

public partial class Page_Create_Product : System.Web.UI.Page {
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

            App.NumberFormat(rate);
            DLC.EDCList(edc);
            DLC.JenisKartu(jeniskartu);
            DLC.ProgramList(program);
		}
	}
	
	protected void next_Click(object sender, EventArgs e) {
        var rs = from p in db.MDRRate
                 where p.IDBank == Convert.ToInt32(edc.SelectedValue)
                 && p.IDProgram == Convert.ToInt32(program.SelectedValue)
                 && p.IDJenisKartuKredit == Convert.ToInt32(jeniskartu.SelectedValue) 
                 select p;

        if (rs.Count() == 0)
        {
            MDRRate n = new MDRRate();

            n.IDJenisKartuKredit = Convert.ToInt32(jeniskartu.SelectedValue);
            n.IDBank = Convert.ToInt32(edc.SelectedValue);
            n.IDProgram = Convert.ToInt32(program.SelectedValue);
            n.MDRRate1 = Convert.ToDecimal(rate.Text);
            n.Operator = App.UserNama;
            n.OperatorTgl = DateTime.Now;
            n.Approval = App.UserNama;
            n.ApprovalTgl = DateTime.Now;
            db.MDRRate.InsertOnSubmit(n);

            db.SubmitChanges();
        }
        else
        {
            var d = db.MDRRate.SingleOrDefault(p => p.IDJenisKartuKredit == Convert.ToInt32(jeniskartu.SelectedValue) && p.IDBank == Convert.ToInt32(edc.SelectedValue) && p.IDProgram == Convert.ToInt32(program.SelectedValue));
            d.MDRRate1 = Convert.ToDecimal(rate.Text);
            db.SubmitChanges();
        }
        Response.Redirect("MDRRate.aspx?d=1");
	}
}