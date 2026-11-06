using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Script.Serialization;

public partial class Page_Summary : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            string query = Request.QueryString["term"];
            var rs = from p in db.DataCustomer
                     where !p.Draft
                     && (p.Nama.Contains(query) || p.Nama.Contains(query))
                     select new { id = p.ID, label = p.Nama + " (" + p.NoCustomer + ")"};

            var json = new JavaScriptSerializer().Serialize(rs);

            Response.Write(json);
        }
    }
}