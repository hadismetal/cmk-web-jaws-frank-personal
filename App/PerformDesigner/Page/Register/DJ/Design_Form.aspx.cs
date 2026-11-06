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
	protected int myIDDesigner { get { return App.QueryInt(this, "IDDesigner"); } }

	protected void Page_Load(object sender, EventArgs e) {
		App.Protect();
		App.ProtectRole(Role.Operator);

		if (!IsPostBack) {
			keterangan.Focus();

			//Form
			fillForm();

			

			//Form-Default
			tgl.Text = Str.Tgl(DateTime.Today);
		}
	}

	//Exception
	protected void DataError() { Response.Redirect("Design.aspx"); }

	//Form
	protected void fillForm() {
		var d = db.DataHuman.SingleOrDefault(p => p.ID == myIDDesigner && !p.Disable);
		if (d == null) DataError();
		else
			designer.InnerHtml = d.Nama;
	}

	//Save
	protected bool valid {
		get { return Fv.isDateTime(tgl, tgl_ERR); }
	}
	protected void save_Click(object sender, EventArgs e) {
		if (valid) {
            var d = db.DataHuman.SingleOrDefault(p => p.ID == myIDDesigner && !p.Disable);
            if (d == null) DataError();

			ProductDesign n = new ProductDesign();

			//Location
            n.IDDesigner = myIDDesigner;

			//Trans. Form
			n.Keterangan = Str.CapFirst(keterangan.Text);
			n.Tgl = Convert.ToDateTime(tgl.Text);
			//n.Nomor = Generate_Nomor(n);
            //n.DesignKode = Generate_Nomor(n);
            n.HumanDesigner = myIDDesigner;
            n.HumanDesignerNama = d.Nama;

			//Status
			n.Draft = true;
			n.DraftDate = DateTime.Now;
            n.StatusSPK = false;

			//User-Info
			n.Operator = App.UserNama;
			n.OperatorTgl = DateTime.Now;

            //n.ImgPicture = "D:\\Images\\ProductDesign\\" + Generate_Nomor(n) + ".jpg";

			//Commit
			db.ProductDesign.InsertOnSubmit(n);
			db.SubmitChanges();

			//Redirect
			Response.Redirect("Design_Data01.aspx?id=" + n.ID);
		}
	}
	protected string Generate_Nomor(ProductDesign d) {
		string x = string.Empty;

		int Tahun = d.Tgl.Year;
		int Bulan = d.Tgl.Month;

		/*int Nomor = db.ProductDesign.Count(p => p.Tgl.Year == Tahun && p.Tgl.Month == Bulan);
		Nomor = Nomor + 1;*/

        int Nomor = 1;

        try
        {
            var s = (from p in db.ProductDesign
                     where p.Tgl.Year == Tahun && p.Tgl.Month == Bulan
                     orderby p.ID descending
                     select p).First();

            if (s.Nomor != null)
                Nomor = Convert.ToInt32(s.Nomor.Substring(10, 4)) + 1;
        }
        catch (Exception ex) { }

		return "DSG/" + Str.KodeFormat(Tahun, Bulan, Nomor, 4);
	}
}