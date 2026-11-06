using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

public partial class Page_Export : System.Web.UI.Page
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
    EncryptDecrypt enc = new EncryptDecrypt();
    protected int myID { get { return App.QueryInt(this, "id"); } }
    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();
        App.ProtectRole(Role.Operator);

        if (!IsPostBack)
        {
            var d = db.PromoDJ.SingleOrDefault(p => p.ID == myID);
            d.StatusActive = false;
            db.SubmitChanges();

            string href = "DJ.aspx?d=1";

            href += "&kw=" + Request.QueryString["kw"];
            href += "&dari=" + Request.QueryString["dari"];
            href += "&sampai=" + Request.QueryString["sampai"];
            href += "&status=" + Request.QueryString["status"];

            Response.Redirect(href);
        }
    }
}