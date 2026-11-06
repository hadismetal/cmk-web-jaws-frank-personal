using System;
using System.Linq;
using System.Web.UI.WebControls;

public partial class Page_Parcel1_06Create : System.Web.UI.Page {
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

		if (!IsPostBack)
        {
            rbranded.SelectedValue = "1";
            //chbranded_CheckedChanged(sender, e);
            nama.Focus();
		}
	}

	//Save
	protected void save_Click(object sender, EventArgs e) {
        try
        {
            Parcel106 n = new Parcel106();

            n.Nama = Str.CapTitle(nama.Text);
            n.NamaKode = Str.CapUpper(namakode.Text);
            n.Keterangan = Str.CapFirst(keterangan.Text);
            n.SN = db.Parcel106.Count() + 1;
            n.IsBranded = (rbranded.SelectedValue == "1") ? true : false;
            //Commit
            db.Parcel106.InsertOnSubmit(n);
            db.SubmitChanges();
        }
        catch
        {
            DLC.Reseed("Parcel106");

            Parcel106 n = new Parcel106();

            n.Nama = Str.CapTitle(nama.Text);
            n.NamaKode = Str.CapUpper(namakode.Text);
            n.Keterangan = Str.CapFirst(keterangan.Text);
            n.SN = db.Parcel106.Count() + 1;
            n.IsBranded = (rbranded.SelectedValue == "1") ? true : false;
            //Commit
            db.Parcel106.InsertOnSubmit(n);
            db.SubmitChanges();

        }

		//Redirect
		App.Reload(this);
	}

    //protected void chbranded_CheckedChanged(object sender, EventArgs e)
    //{
    //    //fill ddl
    //    var cek = from p in db.Parcel106 where p.Disable == false select p;

    //    foreach (var r in cek)
    //    {
    //        string t = r.Nama + " (" + r.NamaKode + ")";
    //        string v = r.ID.ToString();
    //        ddlbrand.Items.Add(new ListItem(t, v));
    //    }

    //    if (chbranded.Checked == true)
    //    {
    //        ddlbrand.Enabled = true;
    //    }
    //    else
    //    {
    //        ddlbrand.Enabled = false;
    //    }


    //}
}