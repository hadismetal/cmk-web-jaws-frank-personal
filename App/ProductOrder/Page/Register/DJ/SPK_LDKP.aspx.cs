using System;
using System.Linq;

public partial class Page_Register_DJ_Form : System.Web.UI.Page
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
    protected int myID { get { return App.QueryInt(this); } }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID);
            if (d == null) DataError();
            noref.InnerText = d.NoSPK;
            ldkp.Text = d.NoLDKP;
            tglldkp.Text = d.TglLDKP.HasValue ? Str.Tgl(d.TglLDKP.Value) : "";
        }
    }

    //Exception
    protected void DataError() { Response.Redirect("SPK.aspx?d=2"); }

    protected void save_Click(object sender, EventArgs e)
    {
        if (validLDKP)
        {
            var d = db.ProductOrder.SingleOrDefault(p => p.ID == myID);
            if (d != null)
            {
                d.NoLDKP = ldkp.Text;
                d.TglLDKP = Convert.ToDateTime(tglldkp.Text);
                db.SubmitChanges();
                Response.Redirect("SPK_List.aspx?d=1");
            }
        }
    }
    protected bool validLDKP
    {
        get { return Fv.isDateTime(tglldkp, tglldkp_ERR); }
    }
}