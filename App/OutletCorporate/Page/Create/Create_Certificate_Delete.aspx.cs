using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Page_Create_Create_Certificate_Delete : System.Web.UI.Page
{
    private Map_DB _db;
    private Map_CS _cs;

    private Map_DB db
    {
        get
        {
            if (_db == null)
                _db = new Map_DB();
            return _db;
        }
    }

    private Map_CS cs
    {
        get
        {
            if (_cs == null)
                _cs = new Map_CS();
            return _cs;
        }
    }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        if (cs != null)
            cs.Dispose();
        base.Dispose();
    }
    protected int myID { get { return App.QueryInt(this); } }
    protected int tipe { get { return App.QueryInt(this, "tipe"); } }
    protected int idpro { get { return App.QueryInt(this, "idpro"); } }

    protected void DataError() { Response.Redirect("Create_Certificate.aspx"); }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        var d = db.PrintCertificate.SingleOrDefault(p => p.ID == myID );
        if (d == null) DataError();
        else
        {
            var d1 = (from p in db.PrintCertificate_Detail where p.IDForm == myID && p.IDProduct == idpro select p).SingleOrDefault();
            db.PrintCertificate_Detail.DeleteOnSubmit(d1);
            db.SubmitChanges();
           
            Response.Redirect("Create_Certificate.aspx?id=" + myID);
        }
    }
}