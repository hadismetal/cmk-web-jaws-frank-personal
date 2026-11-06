using System;
using System.Linq;

public partial class Page_Account_Stone1BUpdate : System.Web.UI.Page
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

    protected int myID { get { return App.QueryInt(this); } }

    public override void Dispose()
    {
        if (db != null)
            db.Dispose();
        base.Dispose();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        App.Protect();

        if (!IsPostBack)
        {
            gia.Focus();

            //Form
            fillForm();

            //Action
            save.Visible = Role.Operator;
        }
    }
    protected void fillForm()
    {

        var d = db.StockProductDJ_Stone1B.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {

            //Header
            plu.InnerText = d.StockProductDJ.Nomor;
            parcel.InnerText = d.Stone1B.Nama;

            gia.Text = d.GIA;
            tgl.Text = d.GIAExpired.HasValue ? Str.Tgl(d.GIAExpired.Value) : "";
        }
    }

    //Save
    protected void save_Click(object sender, EventArgs e)
    {

        var d = db.StockProductDJ_Stone1B.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            d.GIA = gia.Text;
            try
            {
                d.GIAExpired = Convert.ToDateTime(tgl.Text);
            }
            catch (Exception ex) { }

            //Commit
            db.SubmitChanges();

            //Redirect
            App.ReloadClose(this);
        }
    }
}