using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;

public partial class DJ : System.Web.UI.Page
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
           
        }
    }

    protected void save_Click(object sender, EventArgs e)
    {
            StockProductDJLD n = new StockProductDJLD();

            n.Keterangan = keterangan.Text.Trim();

            //User-Info
            n.OperatorNama = App.UserNama;
            n.OperatorTgl = DateTime.Now;
            n.Draft = true;
            n.DraftDate = DateTime.Now;

            //Commit
            db.StockProductDJLD.InsertOnSubmit(n);
            db.SubmitChanges();

            //Redirect
            Response.Redirect("DJLD_ProductDJ.aspx?id=" + n.ID);
    }
}