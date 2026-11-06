using System;
using System.Linq;

public partial class Page_Parcel3_05Create : System.Web.UI.Page {
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
			nama.Focus();
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
        try
        {
            Parcel305 n = new Parcel305();

            n.Nama = Str.CapTitle(nama.Text.Trim());
            n.NamaKode = Str.CapUpper(namakode.Text.Trim());
            n.Keterangan = Str.CapFirst(keterangan.Text.Trim());
            n.SN = db.Parcel305.Count() + 1;
            n.KodeGrade = Str.CapUpper(kodegrading.Text.Trim());
            n.Score = Convert.ToInt32(kodegrading.Text.Trim());

            //Commit
            db.Parcel305.InsertOnSubmit(n);
            db.SubmitChanges();
        }
        catch
        {
            DLC.Reseed("Parcel305");

            Parcel305 n = new Parcel305();

            n.Nama = Str.CapTitle(nama.Text.Trim());
            n.NamaKode = Str.CapUpper(namakode.Text.Trim());
            n.Keterangan = Str.CapFirst(keterangan.Text.Trim());
            n.SN = db.Parcel305.Count() + 1;
            n.KodeGrade = Str.CapUpper(kodegrading.Text.Trim());
            n.Score = Convert.ToInt32(kodegrading.Text.Trim());

            //Commit
            db.Parcel305.InsertOnSubmit(n);
            db.SubmitChanges();
        }

		//Redirect
		App.Reload(this);
	}
}