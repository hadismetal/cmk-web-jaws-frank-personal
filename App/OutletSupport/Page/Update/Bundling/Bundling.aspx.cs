using System;
using System.Linq;

public partial class Page_Create_Product : System.Web.UI.Page
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
            nama.Focus();
            fillForm();
        }

        
    }

    protected void fillForm()
    {
        var d = db.BundleItem.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            nama.Text = d.Nama;
            keterangan.Text = d.Keterangan;
            datepicker.Text =Str.Tgl(d.TglStart);
            datepicker2.Text = Str.Tgl(d.TglExpire);
        }
    }

    protected void next_Click(object sender, EventArgs e)
    {
        var d = db.BundleItem.SingleOrDefault(p => p.ID == myID);
        if (d == null) App.DataNull(this);
        else
        {
            d.Nama = Str.CapTitle(nama.Text);
            d.Keterangan = Str.CapFirst(keterangan.Text);
            d.TglStart = Convert.ToDateTime(datepicker.Text);
            d.TglExpire = Convert.ToDateTime(datepicker2.Text);

            d.Draft = true;
            d.DraftDate = DateTime.Now;
            d.Operator = App.UserNama;
            d.OperatorTgl = DateTime.Now;
            d.ApprovalNama = null;
            d.ApprovalTgl = null;
            
            //Commit
            db.SubmitChanges();
        }
        Response.Redirect("Product_DJ.aspx?id=" + myID);
    }
}