using System;
using System.Linq;

public partial class Page_Create_Product : System.Web.UI.Page {
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

		if (!IsPostBack) {
			nama.Focus();            
		}
	}
	
	protected void next_Click(object sender, EventArgs e) {
        BundleItem n = new BundleItem();

        n.Nama = Str.CapTitle(nama.Text);
        n.Keterangan = Str.CapFirst(keterangan.Text);
        n.TglStart = Convert.ToDateTime(datepicker.Text);
        n.TglExpire = Convert.ToDateTime(datepicker2.Text);
        
        n.Draft = true;
        n.DraftDate = DateTime.Now;
        n.OperatorTgl= DateTime.Now;
        n.Operator = App.UserNama;
        n.StatusActive = 1;

        db.BundleItem.InsertOnSubmit(n);
        db.SubmitChanges();

        Response.Redirect("Souvenir_DJ.aspx?id=" + n.ID);
	}
}