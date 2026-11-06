using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Parcel3_10Create : System.Web.UI.Page
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
            Parcel310 n = new Parcel310();

            n.Nama = Str.CapTitle(nama.Text);
            n.NamaKode = Str.CapUpper(namakode.Text);
            n.Keterangan = Str.CapFirst(keterangan.Text);
            n.SN = db.Parcel310.Count() + 1;

            //Commit
            db.Parcel310.InsertOnSubmit(n);
            db.SubmitChanges();
        }
        catch
        {
            DLC.Reseed("Parcel310");

            Parcel310 n = new Parcel310();

            n.Nama = Str.CapTitle(nama.Text);
            n.NamaKode = Str.CapUpper(namakode.Text);
            n.Keterangan = Str.CapFirst(keterangan.Text);
            n.SN = db.Parcel310.Count() + 1;

            //Commit
            db.Parcel310.InsertOnSubmit(n);
            db.SubmitChanges();
        }

        //Redirect
        App.Reload(this);
    }
}