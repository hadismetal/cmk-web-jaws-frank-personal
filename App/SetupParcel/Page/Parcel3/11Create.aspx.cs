using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Parcel3_11Create : System.Web.UI.Page
{
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
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            nama.Focus();
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {
        try
        {
            Parcel311 n = new Parcel311();

            n.Nama = Str.CapTitle(nama.Text);
            n.NamaKode = Str.CapUpper(namakode.Text);
            n.Keterangan = Str.CapFirst(keterangan.Text);
            n.SN = db.Parcel311.Count() + 1;

            //Commit
            db.Parcel311.InsertOnSubmit(n);
            db.SubmitChanges();
        }
        catch
        {
            DLC.Reseed("Parcel311");

            Parcel311 n = new Parcel311();

            n.Nama = Str.CapTitle(nama.Text);
            n.NamaKode = Str.CapUpper(namakode.Text);
            n.Keterangan = Str.CapFirst(keterangan.Text);
            n.SN = db.Parcel311.Count() + 1;

            //Commit
            db.Parcel311.InsertOnSubmit(n);
            db.SubmitChanges();
        }

        //Redirect
        App.Reload(this);
    }
}