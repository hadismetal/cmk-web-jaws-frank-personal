using System;
using System.Linq;

public partial class Page_UpdateProcess_FF : System.Web.UI.Page {
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
			me_min.Focus();

			//Number-Format
			App.NumberFormat(me_min);
            App.NumberFormat(me_max);
            App.NumberFormat(me);
            App.NumberFormat(ti1_min);
            App.NumberFormat(ti1_max);
            App.NumberFormat(ti1);
            App.NumberFormat(ti2_min);
            App.NumberFormat(ti2_max);
            App.NumberFormat(ti2);
            App.NumberFormat(r_min);
            App.NumberFormat(r_max);
            App.NumberFormat(r);


			//Form
			fillForm();
		}
	}
	protected void fillForm() {
		var d = db.ASPLD.SingleOrDefault(p => p.ID == myID);
		if (d == null) App.DataNull(this);
		else {
			//Header
			header_keterangan.InnerHtml = d.Keterangan;

			me_min.Text = d.MEPeriodeMin.ToString("N0");
            me_max.Text = d.MEPeriodeMax.ToString("N0");
            me.Text = d.MERumus.ToString("N2");
            ti1_min.Text = d.TI1_PeriodeMin.ToString("N0");
            ti1_max.Text = d.TI1_PeriodeMax.ToString("N0");
            ti1.Text = d.TI1_Rumus.ToString("N2");
            ti2_min.Text = d.TI2_PeriodeMin.ToString("N0");
            ti2_max.Text = d.TI2_PeriodeMax.ToString("N0");
            ti2.Text = d.TI2_Rumus.ToString("N2");
            r_min.Text = d.R1_PeriodeMin.ToString("N0");
            r_max.Text = d.R1_PeriodeMax.ToString("N0");
            r.Text = d.R1_Rumus.ToString("N2");
		}
	}

	
	protected void save_Click(object sender, EventArgs e) {
			var d = db.ASPLD.SingleOrDefault(p => p.ID == myID);
			if (d == null) App.DataNull(this);
			else {
                ASPLDLog n = new ASPLDLog();
                n.IDMaster = d.ID;
                n.ProductCategory = d.ProductCategory;
                n.MEPeriodeMinLama = d.MEPeriodeMin;
                n.MEPeriodeMin = Convert.ToInt32(me_min.Text);
                n.MEPeriodeMaxLama = d.MEPeriodeMax;
                n.MEPeriodeMax = Convert.ToInt32(me_max.Text);
                n.MERumusLama = d.MERumus;
                n.MERumus = Convert.ToDecimal(me.Text);
                n.TI1_PeriodeMinLama = d.TI1_PeriodeMin;
                n.TI1_PeriodeMin = Convert.ToInt32(ti1_min.Text);
                n.TI1_PeriodeMaxLama = d.TI1_PeriodeMax;
                n.TI1_PeriodeMax = Convert.ToInt32(ti1_max.Text);
                n.TI1_RumusLama = d.TI1_Rumus;
                n.TI1_Rumus = Convert.ToDecimal(ti1.Text);
                n.TI2_PeriodeMinLama = d.TI2_PeriodeMin;
                n.TI2_PeriodeMin = Convert.ToInt32(ti2_min.Text);
                n.TI2_PeriodeMaxLama = d.TI2_PeriodeMax;
                n.TI2_PeriodeMax = Convert.ToInt32(ti2_max.Text);
                n.TI2_RumusLama = d.TI2_Rumus;
                n.TI2_Rumus = Convert.ToDecimal(ti2.Text);
                n.R1_PeriodeMinLama = d.R1_PeriodeMin;
                n.R1_PeriodeMin = Convert.ToInt32(r_min.Text);
                n.R1_PeriodeMaxLama = d.R1_PeriodeMax;
                n.R1_PeriodeMax = Convert.ToInt32(r_max.Text);
                n.R1_RumusLama = d.R1_Rumus;
                n.R1_Rumus = Convert.ToDecimal(r.Text);
                n.Tgl = DateTime.Now;

                db.ASPLDLog.InsertOnSubmit(n);
                db.SubmitChanges();


                d.MEPeriodeMin = Convert.ToInt32(me_min.Text);
                d.MEPeriodeMax = Convert.ToInt32(me_max.Text);
                d.MERumus = Convert.ToDecimal(me.Text);
                d.TI1_PeriodeMin = Convert.ToInt32(ti1_min.Text);
                d.TI1_PeriodeMax = Convert.ToInt32(ti1_max.Text);
                d.TI1_Rumus = Convert.ToDecimal(ti1.Text);
                d.TI2_PeriodeMin = Convert.ToInt32(ti2_min.Text);
                d.TI2_PeriodeMax = Convert.ToInt32(ti2_max.Text);
                d.TI2_Rumus = Convert.ToDecimal(ti2.Text);
                d.R1_PeriodeMin = Convert.ToInt32(r_min.Text);
                d.R1_PeriodeMax = Convert.ToInt32(r_max.Text);
                d.R1_Rumus = Convert.ToDecimal(r.Text);

				//Commit
				db.SubmitChanges();

				//Redirect
				App.ReloadClose(this);
			}
	}
}